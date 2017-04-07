using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedProject;

namespace Accounting.API.Models
{
    public class Acct : IEqualityComparer<Acct>, IEquatable<Acct>
    {
        [JsonProperty("MS_RAS_Correlation_ID")]
        public string MSRASCorrelationID { get; set; }
        [JsonProperty("Event_Timestamp")]
        public DateTime EventTimestamp { get; set; }
        public DateTimeOffset EventTimestampUtc => new DateTimeOffset(EventTimestamp);
        [JsonProperty("User_Name")]
        public string UserName { get; set; }
        public string Docs { get; set; }
        [JsonProperty("Acct_Status_Type")]
        public AcctStatusType AcctStatusType { get; set; }
        [JsonProperty("Acct_Session_Time")]
        public double AcctSessionTime { get; internal set; }
        [JsonProperty("Acct_Input_Octets")]
        public double AcctInputOctets { get; set; }
        [JsonProperty("Acct_Output_Packets")]
        public double AcctOutputOctets { get; set; }
        [JsonProperty("Acct_Input_Packets")]
        public double AcctInputPackets { get; set; }
        [JsonProperty("Acct_Output_Octets")]
        public double AcctOutputPackets { get; set; }
        [JsonProperty("Tunnel_Client_Endpt")]
        public string TunnelClientEndpt { get; set; }
        [JsonProperty("Framed_MTU")]
        public double FramedMTU { get; internal set; }
        [JsonProperty("Framed_IP_Address")]
        public string FramedIPAddress { get; internal set; }
        [JsonProperty("Client_Friendly_Name")]
        public string ClientFriendlyName { get; internal set; }
        [JsonProperty("Client_IP_Address")]
        public string ClientIPAddress { get; internal set; }
        [JsonProperty("Packet_Type")]
        public PacketType PacketType { get; internal set; }
        [JsonProperty("Tunnel_Type")]
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
