using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Accounting.RouterReporter
{
    public class CounterName
    {
        public const string NetworkIn = "\\Network Interface(*)\\Bytes Received/sec";
        public const string NetworkOut = "\\Network Interface(*)\\Bytes Sent/sec";
        public const string NetworkTotal = "\\Network Interface(*)\\Bytes Total/sec";
        public const string ProcessorInformationTotal = "\\Processor Information(_total)\\% Processor Time";
        public const string MemoryAvailableMBytes = "\\Memory\\Available MBytes";

        public static string GetNetworkInterfaceName(string counterPath)
        {
            Regex re = new Regex($@"\([\w\s-_]+\)");
            return re.Match(counterPath).Value;
        }
    }
}
