using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.RouterReporter
{
    public class WindowsEventLogger : ILog
    {
        public WindowsEventLogger(string serviceName)
        {
            this._serviceName = serviceName;
        }
        private readonly string _serviceName;
        public void Log(EventLogEntryType type, string message)
        {
            if (!Environment.UserInteractive)
            {
                EventLog eventLog = new EventLog();

                if (!EventLog.SourceExists(_serviceName))
                {
                    EventLog.CreateEventSource(_serviceName, "Application");
                }
                eventLog.Source = _serviceName;
                int eventID = 8;
                eventLog.WriteEntry(message, type, eventID);
                eventLog.Close();
            }
            else
            {
                Console.WriteLine($"{Enum.GetName(typeof(EventLogEntryType), type)}: {message}");
            }

        }
    }
}
