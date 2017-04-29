using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.RouterReporter
{
    public interface ILog
    {
        void Log(EventLogEntryType type, string message);
    }
}
