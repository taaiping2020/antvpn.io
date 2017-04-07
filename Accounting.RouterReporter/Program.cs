using System;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.ServiceProcess;
using System.Diagnostics;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using SharedProject;
using Microsoft.PowerShell.Commands.GetCounter;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Accounting.RouterReporter
{
    class Program
    {
        #region Nested classes to support running as service
        public class Service : ServiceBase
        {
            public Service()
            {
                ServiceName = Program.ServiceName;
            }

            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
            }
        }

        private static void Stop()
        {
            ps?.Dispose();
            perfCounterPS?.Dispose();
        }

        private static void Start(string[] args)
        {
            //ReportRouterInfo();
            ReportServerHealth();
        }
        #endregion

        static PowerShell ps = PowerShell.Create();
        static PowerShell perfCounterPS = PowerShell.Create();
        static string machineName = Environment.MachineName;
        public const string ServiceName = "RouterReporter";

        static int _interval = int.Parse(ConfigurationManager.AppSettings["interval"]);
        static int _perfCounterInterval = int.Parse(ConfigurationManager.AppSettings["perfCounterInterval"]);
        static Repo repo = new Repo(ConfigurationManager.AppSettings["connectionString"], ConfigurationManager.AppSettings["connectionStringDc"]);

        static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
                // running as service
                using (var service = new Service())
                    ServiceBase.Run(service);
            else
            {
                // running as console app
                Start(args);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);

                Stop();
            }
        }

        public async static void ReportRouterInfo()
        {
            while (true)
            {
                try
                {
                    ps.Commands.Clear();
                    ps.AddCommand("Get-RemoteAccessConnectionStatistics");
                    var psos = ps.Invoke();
                    var now = DateTime.UtcNow;
                    if (psos.IsNullOrCountEqualsZero())
                    {
                        repo.InsertOrUpdateTimetamp(machineName, now);
                        Console.WriteLine("not client on this server...");
                    }
                    else
                    {
                        var racs = psos.Select(c => c.GetRemoteAccessConnection(machineName, now)).ToList();
                        repo.InsertDatas(racs);
                        repo.InsertOrUpdateTimetamp(machineName, now);

                        TryDisconnectVpnUser(racs);
                    }
                }
                catch (Exception ex)
                {
                    Log(EventLogEntryType.Error, ex.Message);
                    Log(EventLogEntryType.Error, ex.StackTrace);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }

                await Task.Delay(TimeSpan.FromSeconds(_interval));
            }
            
        }
        public static void ReportServerHealth()
        {
            while (true)
            {
                try
                {
                    HealthReport report = new HealthReport();
                    report.SampleIntervalInSec = _perfCounterInterval;
                    report.BeginTimestamp = DateTime.UtcNow;

                    CheckingServiceAreRunning();

                    SetPerfCounter(report);
                    SetRemoteAccessSslCertificate(report);
                    SetRemoteAccess(report);
                    SetRemoteAccessRadius(report);

                    report.EndTimestamp = DateTime.UtcNow;
                    Console.WriteLine(report.EndTimestamp);
                }
                catch (Exception ex)
                {
                    Log(EventLogEntryType.Error, ex.Message);
                    Log(EventLogEntryType.Error, ex.StackTrace);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
          

            void CheckingServiceAreRunning()
            {
                perfCounterPS.Commands.Clear();
                perfCounterPS.AddScript("Get-Service RaMgmtSvc, RemoteAccess, RouterReporter | Where-Object { $PSItem.Status -eq 'Running'}");
                var psos = perfCounterPS.Invoke<ServiceController>();
                if (psos.Count == 3)//TODO
                {
                    throw new Exception("service not running...");
                }
            }
            void SetPerfCounter(HealthReport report)
            {
                perfCounterPS.Commands.Clear();
                perfCounterPS.AddScript($"Get-Counter -Counter '{CounterName.NetworkIn}','{CounterName.NetworkOut}','{CounterName.NetworkTotal}','{CounterName.ProcessorInformationTotal}','{CounterName.MemoryAvailableMBytes}' -SampleInterval {_perfCounterInterval} | Select-Object -ExpandProperty CounterSamples");
                var psos = perfCounterPS.Invoke<PerformanceCounterSample>();

                if (!psos.IsNullOrCountEqualsZero())
                {
                    var netCounterSample = psos.FirstOrDefault(c => c.Path.Contains("network") && c.CookedValue != 0);
                    if (netCounterSample != null)
                    {
                        var networkInterfaceName = CounterName.GetNetworkInterfaceName(netCounterSample.Path);
                        var receivedCounterSample = psos.FirstOrDefault(c => c.Path.Contains(networkInterfaceName) && c.Path.Contains("received"));
                        var sentCounterSample = psos.FirstOrDefault(c => c.Path.Contains(networkInterfaceName) && c.Path.Contains("sent"));
                        var totalCounterSample = psos.FirstOrDefault(c => c.Path.Contains(networkInterfaceName) && c.Path.Contains("total"));
                        report.SetNetwork(receivedCounterSample, sentCounterSample, totalCounterSample);
                    }
                    var proTotal = psos.FirstOrDefault(c => c.Path.Contains("processor"));
                    if (proTotal != null)
                    {
                        report.ProcessorTime = proTotal.CookedValue;
                    }
                    var memoTotal = psos.FirstOrDefault(c => c.Path.Contains("memory"));
                    if (memoTotal != null)
                    {
                        report.AvailableMemoryMegaBytes = memoTotal.CookedValue;
                    }
                }
            }
            void SetRemoteAccessRadius(HealthReport report)
            {
            }
            void SetRemoteAccessSslCertificate(HealthReport report)
            {
                perfCounterPS.Commands.Clear();
                perfCounterPS.AddScript($"Get-RemoteAccess | Select-Object -ExpandProperty SslCertificate");
                var psos = perfCounterPS.Invoke<X509Certificate2>();
                if (psos.IsNullOrCountEqualsZero())
                {
                    throw new ItemNotFoundException("SslCertificate");
                }
                report.SetX509Certificate2(psos.First());
            }
            void SetRemoteAccess(HealthReport report)
            {
                perfCounterPS.Commands.Clear();
                perfCounterPS.AddCommand($"Get-RemoteAccess");
                var psos = perfCounterPS.Invoke();
                if (psos.IsNullOrCountEqualsZero())
                {
                    throw new Exception("Get-RemoteAccess fail.");
                }
                report.SetRemoteAccess(psos.First());
            }
        }

        public static void TryDisconnectVpnUser(List<RemoteAccessConnection> connections)
        {
            if (connections == null)
            {
                throw new ArgumentNullException(nameof(connections));
            }
            try
            {
                var disconnectusers = repo.GetDisconnectUserNames();
                if (disconnectusers != null && disconnectusers.Any())
                {
                    foreach (var username in connections.Select(c => c.UserName))
                    {
                        if (disconnectusers.Contains(username))
                        {
                            Console.WriteLine($"try disconnecting {username}");
                            ps.AddScript($@"Get-RemoteAccessConnectionStatistics | where {{ $_.UserName -like ""*\{username}"" -or $_UserName -like ""{username}"" }} | Select-Object UserName | Disconnect-VpnUser");
                            ps.Invoke();

                            ps.Commands.Clear();

                            ps.AddCommand($@"Disconnect-VpnUser");
                            ps.AddParameter("UserName", username);
                            ps.Invoke();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(EventLogEntryType.Error, ex.Message);
                Log(EventLogEntryType.Error, ex.StackTrace);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void Log(EventLogEntryType type, string message)
        {
            // Create an instance of EventLog
            EventLog eventLog = new EventLog();

            // Check if the event source exists. If not create it.
            if (!EventLog.SourceExists(ServiceName))
            {
                EventLog.CreateEventSource(ServiceName, "Application");
            }

            // Set the source name for writing log entries.
            eventLog.Source = ServiceName;

            // Create an event ID to add to the event log
            int eventID = 8;

            // Write an entry to the event log.
            eventLog.WriteEntry(message, type, eventID);

            // Close the Event Log
            eventLog.Close();
        }
    }
}
















//repo.InsertDatas(new System.Collections.Generic.List<RemoteAccessConnection>
//{
//    new RemoteAccessConnection { AuthMethod = "sdf", Bandwidth = 123, ClientExternalAddress = "12012.12413", ClientIPv4Address = "45454", ConnectionDuration = TimeSpan.FromHours(1), ConnectionStartTime = DateTime.Now, ConnectionType = ConnectionType.Vpn, MachineName = "HK", TimeStamp = DateTime.Now, TotalBytesIn = 1234343214, TotalBytesOut = 34324, TransitionTechnology = "tech", TunnelType = TunnelType.Ikev2, UserActivityState = UserActivityState.Active, UserName = "bosxixi" },
//    new RemoteAccessConnection { AuthMethod = "1232", Bandwidth = 123, ClientExternalAddress = "12012.12413", ClientIPv4Address = "45454", ConnectionDuration = TimeSpan.FromHours(1), ConnectionStartTime = DateTime.Now, ConnectionType = ConnectionType.Vpn, MachineName = "HK", TimeStamp = DateTime.Now, TotalBytesIn = 1234343214, TotalBytesOut = 34324, TransitionTechnology = "tech", TunnelType = TunnelType.Ikev2, UserActivityState = UserActivityState.Active, UserName = "bosxixi" },
//    new RemoteAccessConnection { AuthMethod = "fsdf", Bandwidth = 123, ClientExternalAddress = "12012.12413", ClientIPv4Address = "45454", ConnectionDuration = TimeSpan.FromHours(1), ConnectionStartTime = DateTime.Now, ConnectionType = ConnectionType.Vpn, MachineName = "HK", TimeStamp = DateTime.Now, TotalBytesIn = 1234343214, TotalBytesOut = 34324, TransitionTechnology = "tech", TunnelType = TunnelType.Ikev2, UserActivityState = UserActivityState.Active, UserName = "bosxixi" },
//    new RemoteAccessConnection { AuthMethod = "sdfs", Bandwidth = 123, ClientExternalAddress = "12012.12413", ClientIPv4Address = "45454", ConnectionDuration = TimeSpan.FromHours(1), ConnectionStartTime = DateTime.Now, ConnectionType = ConnectionType.Vpn, MachineName = "HK", TimeStamp = DateTime.Now, TotalBytesIn = 1234343214, TotalBytesOut = 34324, TransitionTechnology = "tech", TunnelType = TunnelType.Ikev2, UserActivityState = UserActivityState.Active, UserName = "bosxixi" },
//});

