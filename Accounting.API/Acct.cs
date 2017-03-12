using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API
{
    public class Acct : IEqualityComparer<Acct>, IEquatable<Acct>
    {
        public string MSRASCorrelationID { get; set; }
        public DateTime EventTimestamp { get; set; }
        public DateTimeOffset EventTimestampUtc => new DateTimeOffset(EventTimestamp);
        public string UserName { get; set; }
        public string Docs { get; set; }
        public AcctStatusType AcctStatusType { get; set; }
        public double AcctSessionTime { get; internal set; }
        public double AcctInputOctets { get; set; }
        public double AcctOutputOctets { get; set; }
        public double AcctInputPackets { get; set; }
        public double AcctOutputPackets { get; set; }
        public string TunnelClientEndpt { get; set; }
        public double FramedMTU { get; internal set; }
        public string FramedIPAddress { get; internal set; }
        public string ClientFriendlyName { get; internal set; }
        public string ClientIPAddress { get; internal set; }
        public PacketType PacketType { get; internal set; }
        public double TunnelType { get; internal set; }

        public static bool operator ==(Acct acct1, Acct acct2) => acct1?.MSRASCorrelationID == acct2?.MSRASCorrelationID && acct1?.UserName == acct2?.UserName;
        public static bool operator !=(Acct acct1, Acct acct2) => !(acct1?.MSRASCorrelationID == acct2?.MSRASCorrelationID && acct1?.UserName == acct2?.UserName);
        public override int GetHashCode() => $"{MSRASCorrelationID}{UserName}".GetHashCode();

        public override bool Equals(object obj)
        {
            var o = obj as Acct;
            if (o == null)
            {
                throw new InvalidCastException();
            }

            return this == o;
        }

        public bool Equals(Acct x, Acct y)
        {
            return x == y;
        }

        public int GetHashCode(Acct obj)
        {
            return obj.GetHashCode();
        }

        public bool Equals(Acct other)
        {
            return this == other;
        }

        #region
        /*
         * 
         * 

                      
                     64 for Tunnel-Type

                     Length
                        Always 6.

                     Tag
                        The Tag field is one octet in length and is intended to provide a
                        means of grouping attributes in the same packet which refer to the
                        same tunnel.  Valid values for this field are 0x01 through 0x1F,
                        inclusive.  If the Tag field is unused, it MUST be zero (0x00).

                     Value
                        The Value field is three octets and contains one of the following
                        values, indicating the type of tunnel to be started.

                     1      Point-to-Point Tunneling Protocol (PPTP) [1]
                     2      Layer Two Forwarding (L2F) [2]
                     3      Layer Two Tunneling Protocol (L2TP) [3]
                     4      Ascend Tunnel Management Protocol (ATMP) [4]
                     5      Virtual Tunneling Protocol (VTP)
                     6      IP Authentication Header in the Tunnel-mode (AH) [5]
                     7      IP-in-IP Encapsulation (IP-IP) [6]
                     8      Minimal IP-in-IP Encapsulation (MIN-IP-IP) [7]
                     9      IP Encapsulating Security Payload in the Tunnel-mode (ESP) [8]
                     10     Generic Route Encapsulation (GRE) [9]
                     11     Bay Dial Virtual Services (DVS)
                     12     IP-in-IP Tunneling [10]






         * 
         * 
         * 
         */
        #endregion
    }
}
