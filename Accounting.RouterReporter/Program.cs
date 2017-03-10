using System;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Bson;
using System.ServiceProcess;
using System.Diagnostics;

namespace Accounting.RouterReporter
{
    class Program
    {
        #region Nested classes to support running as service
        public const string ServiceName = "RouterReporter";

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

        }

        private static void Start(string[] args)
        {

            var autoEvent = new AutoResetEvent(false);
            ps.AddCommand("Get-RemoteAccessConnectionStatistics");
            int interval = 30;
            if (args.Length > 0)
                interval = int.Parse(args[0]);

            if (args.Length == 3)
                uri = $"mongodb://{args[1]}:{args[2]}";
            else
                uri = "mongodb://104.160.35.172:27017";

            Log(EventLogEntryType.Information, $"uri: {uri}, uploads interval: {interval}");

            client = new MongoClient(uri);

            t = new Timer(ReportRouterInfo, autoEvent, TimeSpan.FromSeconds(interval), TimeSpan.FromSeconds(interval));
            t2 = new Timer(DisconnectVpnUser, autoEvent, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(interval));

            //collectiontime.InsertOne(new BsonDocument() { new BsonElement("name", "currenttimestamp"), new BsonElement("timestamp", DateTime.UtcNow) });
            //collectiontime.InsertOne(new BsonDocument() { new BsonElement("name", "disconnectusers"), });
        }
        #endregion

        static PowerShell ps = PowerShell.Create();
        static MongoClient client;
        static string uri;
        static string machineName = Environment.MachineName;
        static Timer t;
        static Timer t2;
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
                var database = client.GetDatabase("accountingdata");
                var collection = database.GetCollection<BsonDocument>("current");
                var meta = database.GetCollection<Meta>("meta");

                var psos = ps.Invoke();
                if (psos.IsNullOrCountEqualsZero())
                {
                    Console.WriteLine("not client on this server...");
                }
                else
                {
                    var now = DateTime.UtcNow;
                    var racs = psos.Select(c => new RemoteAccessConnection(c, machineName, now)).ToList();
                    collection.InsertMany(racs.Select(c => c.ToBsonDocument()));

                    var filterBuilder = Builders<Meta>.Filter;
                    var updateBuilder = Builders<Meta>.Update;
                    var filter = filterBuilder.Eq(c => c.name, "currenttimestamp");
                    var update = updateBuilder.Set(c => c.timestamp, now);
                    meta.FindOneAndUpdate(filter, update);
                    //Log(EventLogEntryType.Information, $"Success {now} : {DateTime.Now}");
                    Console.WriteLine($"Success {now} : {DateTime.Now}");
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
        public static void DisconnectVpnUser(object state)
        {
            try
            {
                var database = client.GetDatabase("accountingdata");
                var collection = database.GetCollection<BsonDocument>("current");
                var meta = database.GetCollection<Meta>("meta");

                var disconnectusers = meta.Find(c => c.name == "disconnectusers").FirstOrDefault();
                if (disconnectusers != null && !disconnectusers.users.IsNullOrCountEqualsZero())
                {
                    foreach (var username in disconnectusers.users)
                    {
                        Console.WriteLine($"try disconnecting {username}");
                        using (var psd = PowerShell.Create())
                        {
                            psd.AddScript($@"Get-RemoteAccessConnectionStatistics | where {{ $_.UserName -like ""*\{username}"" -or $_UserName -like ""{username}"" }} | Select-Object UserName | Disconnect-VpnUser");
                            psd.Invoke();
                        }
                        using (var psd = PowerShell.Create())
                        {
                            psd.AddCommand($@"Disconnect-VpnUser");
                            psd.AddParameter("UserName", username);
                            psd.Invoke();
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
