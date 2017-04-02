using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extensions;

namespace WebMVC.ViewModels
{
    public class AcctRawViewModel
    {
        public string EventTime { get; set; }
        public string TotalTraffic { get; set; }
        public string Protocal { get; set; }
        public string UserName { get; set; }
        public TimeSpan Duration { get; set; }

        public AcctRawViewModel(AcctRaw acctRaw)
        {
            this.EventTime = DateTime.Parse(acctRaw.Event_Timestamp).ToLocalTime().ToString("MM-dd HH:mm");
            this.TotalTraffic = ToMegaByte(acctRaw.Acct_Input_Octets + acctRaw.Acct_Output_Octets);
            this.Protocal = Enum.GetName(typeof(TunnelType), acctRaw.Tunnel_Type);
            this.UserName = acctRaw.User_Name;
            this.Duration = TimeSpan.FromSeconds(acctRaw.Acct_Session_Time);
        }

        private static string ToMegaByte(long? bytes)
        {
            if (bytes == null)
            {
                return string.Empty;
            }
            return (bytes / 1024d / 1024d)?.ToString("0.00") + " MB";
        }

        public string DurationDisplay()
        {
            if (Duration.TotalMinutes < 60)
            {
                return $"{(int)Duration.TotalMinutes}m";
            }
            if (Duration.TotalHours < 24)
            {
                return $"{(int)Duration.TotalHours}h";
            }

            return "over a day";
        }
    }
}
