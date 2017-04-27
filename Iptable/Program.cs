using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;

namespace Iptable
{
    class Program
    {
        static string _ports = ConfigurationManager.AppSettings["ports"];
        static string _VPNServerIPAddress = ConfigurationManager.AppSettings["VPNServerIPAddress"];
        static string _RedirectorInternalIPAddress = ConfigurationManager.AppSettings["RedirectorInternalIPAddress"];
        static string _PathToSave = ConfigurationManager.AppSettings["PathToSave"];
        static void Main(string[] args)
        {
            var lines = GetLines(GetPorts(_ports));
            File.WriteAllLines(_PathToSave, lines);
        }

        static IEnumerable<string> GetLines(IEnumerable<string> ports)
        {
            string template1 = "iptables -t nat -A PREROUTING -p tcp --dport ForwadPort -j DNAT --to-destination VPNServerIPAddress:ForwadPort";
            string template2 = "iptables -t nat -A POSTROUTING -p tcp -d VPNServerIPAddress --dport ForwadPort -j SNAT --to-source RedirectorInternalIPAddress";
            string template3 = "iptables -t nat -A PREROUTING -p udp --dport ForwadPort -j DNAT --to-destination VPNServerIPAddress:ForwadPort";
            string template4 = "iptables -t nat -A POSTROUTING -p udp -d VPNServerIPAddress --dport ForwadPort -j SNAT --to-source RedirectorInternalIPAddress";
            foreach (var port in ports)
            {
                yield return template1.Replace("VPNServerIPAddress", _VPNServerIPAddress).Replace("RedirectorInternalIPAddress", _RedirectorInternalIPAddress).Replace("ForwadPort", port);
                yield return template2.Replace("VPNServerIPAddress", _VPNServerIPAddress).Replace("RedirectorInternalIPAddress", _RedirectorInternalIPAddress).Replace("ForwadPort", port);
                yield return template3.Replace("VPNServerIPAddress", _VPNServerIPAddress).Replace("RedirectorInternalIPAddress", _RedirectorInternalIPAddress).Replace("ForwadPort", port);
                yield return template4.Replace("VPNServerIPAddress", _VPNServerIPAddress).Replace("RedirectorInternalIPAddress", _RedirectorInternalIPAddress).Replace("ForwadPort", port);
            }
        }
        static IEnumerable<string> GetPorts(string ports)
        {
            var part = _ports.Split(',');
            foreach (var p in part)
            {
                if (p.Contains(":"))
                {
                    var range = p.Split(':');
                    var ranges = Enumerable.Range(int.Parse(range.First()), int.Parse(range.Last()) - int.Parse(range.First()) + 1).Select(c => c.ToString()).ToArray();
                    foreach (var r in ranges)
                    {
                        yield return r;
                    }
                }
                else
                {
                    yield return p;
                }
            }

        }
    }
}
