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
            if (bool.Parse(ConfigurationManager.AppSettings["ReportRouterInfo"]))
            {
                _timerReportRouterInfo = new Timer(ReportRouterInfo, null, Timeout.Infinite, Timeout.Infinite);
                _timerReportRouterInfo.Change(0, Timeout.Infinite);
            }

            if (bool.Parse(ConfigurationManager.AppSettings["ReportServerHealth"]))
            {
                _timerReportServerHealth = new Timer(ReportServerHealth, null, Timeout.Infinite, Timeout.Infinite);
                _timerReportServerHealth.Change(0, Timeout.Infinite);
            }

            if (bool.Parse(ConfigurationManager.AppSettings["Shadowsocks"]))
            {
                _shadowsocksUser = new Shadowsocks(_shadowsocksManageAddress, _shadowsocksManagePort, machineName, logger, repo);
                _timerShadowsocks = new Timer(_shadowsocksUser.Fetch, null, Timeout.Infinite, Timeout.Infinite);
                _shadowsocksUser.SetTimer(_timerShadowsocks);
                _timerShadowsocks.Change(0, Timeout.Infinite);
            }
        }
        #endregion

        static PowerShell ps = PowerShell.Create();
        static PowerShell perfCounterPS = PowerShell.Create();
        readonly static string machineName = Environment.MachineName;
        public const string ServiceName = "RouterReporter";
        public static Timer _timerReportRouterInfo { get; set; }
        public static Timer _timerReportServerHealth { get; set; }
        public static Timer _timerShadowsocks { get; set; }
        public static Shadowsocks _shadowsocksUser { get; set; }
        static int _interval = int.Parse(ConfigurationManager.AppSettings["interval"]);
        static int _perfCounterInterval = int.Parse(ConfigurationManager.AppSettings["perfCounterInterval"]);
        static WindowsEventLogger logger = new WindowsEventLogger(ServiceName);
        static string _shadowsocksManageAddress = ConfigurationManager.AppSettings["ShadowsocksManageAddress"];
        static int _shadowsocksManagePort = int.Parse(ConfigurationManager.AppSettings["ShadowsocksManagePort"]);

        static Repo repo = new Repo(ConfigurationManager.AppSettings["connectionString"], ConfigurationManager.AppSettings["connectionStringDc"], ConfigurationManager.AppSettings["connectionStringServer"]);

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
                logger.Log(EventLogEntryType.Error, ex.Message);
                logger.Log(EventLogEntryType.Error, ex.StackTrace);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            _timerReportRouterInfo.Change(_interval * 1000, Timeout.Infinite);
        }
        public static void ReportServerHealth(object state)
        {
            try
            {
                HealthReport report = new HealthReport()
                {
                    SampleIntervalInSec = _perfCounterInterval,
                    MachineName = machineName,
                    BeginTimestamp = DateTime.UtcNow
                };
                CheckingServiceAreRunning();

                SetPerfCounter(report);
                SetRemoteAccessSslCertificate(report);
                SetRemoteAccess(report);
                SetRemoteAccessRadius(report);
                SetSSServerRunningStatus(report);

                report.EndTimestamp = DateTime.UtcNow;
                repo.InsertServerHealthReport(report);
            }
            catch (Exception ex)
            {
                logger.Log(EventLogEntryType.Error, ex.Message);
                logger.Log(EventLogEntryType.Error, ex.StackTrace);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            _timerReportServerHealth.Change(1000, Timeout.Infinite);

            void CheckingServiceAreRunning()
            {
                perfCounterPS.Commands.Clear();
                perfCounterPS.AddScript("Get-Service RaMgmtSvc, RemoteAccess, RouterReporter | Where-Object { $PSItem.Status -eq 'Running'}");
                var psos = perfCounterPS.Invoke<ServiceController>();
                if (psos.Count != 3)//TODO
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
            void SetSSServerRunningStatus(HealthReport report)
            {
                perfCounterPS.Commands.Clear();
                perfCounterPS.AddScript($"Get-Process ssserver");
                var psos = perfCounterPS.Invoke();
                if (psos.IsNullOrCountEqualsZero())
                {
                    report.IsShadowsocksServerRunning = false;
                }
                else
                {
                    report.IsShadowsocksServerRunning = true;
                }
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
                logger.Log(EventLogEntryType.Error, ex.Message);
                logger.Log(EventLogEntryType.Error, ex.StackTrace);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

       
    }
}