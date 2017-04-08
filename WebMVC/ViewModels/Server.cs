using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedProject;

namespace WebMVC.ViewModels
{
    public class Server : ServerViewModel
    {
        public IEnumerable<string> GetProtocalNames()
        {
            if (this.Protocals.IsNullOrCountEqualsZero())
            {
                yield break;
            }
            foreach (var p in this.Protocals)
            {
                yield return Enum.GetName(typeof(Protocal), (Protocal)p);
            }
        }
        public string ProtocalsDisplay => ToString(GetProtocalNames(), ',');
        private static string ToString(IEnumerable<string> stringArray, char spliter)
        {
            if (stringArray.IsNullOrCountEqualsZero())
            {
                return null;
            }
            StringBuilder b = new StringBuilder();
            foreach (var item in stringArray)
            {
                b.Append(spliter.ToString()).Append(item);
            }
            return b.ToString().Remove(0, 1);
        }
        public enum Protocal
        {
            SSTP = 1,
            PPTP = 2,
            IKEv2 = 3,
            L2TP = 4
        }

        public bool IsServerStatusGood
        {
            get
            {
                if (this.HealthReport == null)
                {
                    return false;
                }
                if (this.HealthReport.EndTimestamp.AddMinutes(5) < DateTime.UtcNow)
                {
                    return false;
                }
                return true;
            }
        }

        public string DurationDisplay
        {
            get
            {
                if (this.HealthReport == null)
                {
                    return "unavailable";
                }
                var duration = DateTime.UtcNow - this.HealthReport.EndTimestamp;
                if (duration.TotalMinutes < 60)
                {
                    return $"{(int)duration.TotalMinutes}m ago";
                }
                if (duration.TotalHours < 24)
                {
                    return $"{(int)duration.TotalHours}h ago";
                }

                return "over a day";
            }
        }

        public string BandwidthTotal
        {
            get
            {
                if (this.HealthReport == null)
                {
                    return "unavailable";
                }
                return (HealthReport.NetworkBytesTotalPerSec / 1024d).ToString("0.00") + " KB/sec";
            }
        }

        public string BandwidthIn
        {
            get
            {
                if (this.HealthReport == null)
                {
                    return "unavailable";
                }
                return (HealthReport.NetworkBytesInPerSec / 1024d).ToString("0.00") + " KB/sec";
            }
        }
        public string BandwidthOut
        {
            get
            {
                if (this.HealthReport == null)
                {
                    return "unavailable";
                }
                return (HealthReport.NetworkBytesOutPerSec / 1024d).ToString("0.00") + " KB/sec";
            }
        }
        public string CPUPercentage
        {
            get
            {
                if (this.HealthReport == null)
                {
                    return "unavailable";
                }
                return (HealthReport.ProcessorTime).ToString("0") + " %";
            }
        }
    }
}
