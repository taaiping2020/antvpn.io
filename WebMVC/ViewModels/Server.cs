using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMVC.ViewModels
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IPv4 { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string CountryName { get; set; }
        public string CountryFlag { get; set; }
        public string RedirectorServerCountryName { get; set; }
        public string RedirectorServerCountryFlag { get; set; }
        public string TrafficServerCountryName { get; set; }
        public string TrafficServerCountryFlag { get; set; }
        public bool IsPublic { get; set; }
        public bool IsHybrid { get; set; }
        public bool Off { get; set; }
        public int[] Protocals { get; set; }
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
    }
}
