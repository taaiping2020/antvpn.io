using System;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.ServiceProcess;
using System.Diagnostics;
using Extensions.Windows;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

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
        }

        private static void Start(string[] args)
        {
            var autoEvent = new AutoResetEvent(false);
            t = new Timer(ReportRouterInfo, autoEvent, TimeSpan.FromSeconds(_interval), TimeSpan.FromSeconds(_interval));
        }
        #endregion

        static PowerShell ps = PowerShell.Create();
        static string machineName = Environment.MachineName;
        public const string ServiceName = "RouterReporter";
        static Timer t;

        static int _interval = int.Parse(ConfigurationManager.AppSettings["interval"]);
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

        public static void ReportRouterInfo(object state)
        {
            try
            {
                ps.Commands.Clear();
                ps.AddCommand("Get-RemoteAccessConnectionStatistics");
                var psos = ps.Invoke();
                if (psos.IsNullOrCountEqualsZero())
                {
                    Console.WriteLine("not client on this server...");
                }
                else
                {
                    var now = DateTime.UtcNow;
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

