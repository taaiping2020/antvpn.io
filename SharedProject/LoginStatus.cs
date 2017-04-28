using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public class LoginStatus
    {
        public string UserId { get; set; }
        public string LoginName { get; set; }
        public bool AllowDialIn { get; set; }
        public bool Enabled { get; set; }
        public bool IsOnline { get; set; }
        public BasicAcct BasicAcct { get; set; }
        public DateTimeOffset? LastUpdated { get; set; }
        public int Port { get; set; }
        public long? MonthlyTraffic { get; set; }
        public string MonthlyTrafficDisplay => ToMegaByte(MonthlyTraffic);
        public long? SSMonthlyTraffic { get; set; }
        public string SSMonthlyTrafficDisplay => ToMegaByte(SSMonthlyTraffic);
        public string Percent()
        {
            if (this.BasicAcct != null && MonthlyTraffic != null)
            {
                var percent = $"{((this.BasicAcct.TotalInOut / (double)MonthlyTraffic) * 100).ToString("0")}%";
                if (this.BasicAcct.TotalInOut != 0 && percent == "0%")
                {
                    percent = "1%";
                }
                return percent;
            }
            return null;
        }



        private static string ToMegaByte(long? bytes)
        {
            if (bytes == null)
            {
                return string.Empty;
            }
            return (bytes / 1024d / 1024d)?.ToString("0.00") + " MB";
        }
    }
}
