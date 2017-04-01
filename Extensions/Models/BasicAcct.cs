using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions
{
    public class BasicAcct
    {
        public double TotalIn { get; set; }
        public double TotalOut { get; set; }

        private static string ToMegaByte(double bytes)
        {
            return (bytes / 1024d / 1024d).ToString("0.00") + " MB";
        }

        public string TotalInDisplay => ToMegaByte(TotalIn);
        public string TotalOutDisplay => ToMegaByte(TotalOut);
        public double TotalInOut => TotalOut + TotalIn;
        public string TotalInOutDisplay => ToMegaByte(TotalInOut);
    }
}
