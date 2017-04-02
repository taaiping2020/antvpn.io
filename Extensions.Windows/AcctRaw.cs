using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Windows
{
    public class AcctRaw
    {
        public string Timestamp { get; set; }
        public string Computer_Name { get; set; }
        public string Event_Source { get; set; }
        public string NAS_Identifier { get; set; }
        public string NAS_IP_Address { get; set; }
        public long Service_Type { get; set; }
        public long Framed_Protocol { get; set; }
        public long NAS_Port { get; set; }
        public NASPortType NAS_Port_Type { get; set; }
        public TunnelType Tunnel_Type { get; set; }
        public long Tunnel_Medium_Type { get; set; }
        public string Called_Station_Id { get; set; }
        public string Tunnel_Server_Endpt { get; set; }
        public string Calling_Station_Id { get; set; }
        public string Tunnel_Client_Endpt { get; set; }
        public string Class { get; set; }
        public string Acct_Session_Id { get; set; }
        public string User_Name { get; set; }
        public string Framed_IP_Address { get; set; }
        public long Framed_MTU { get; set; }
        public string Acct_Multi_Session_Id { get; set; }
        public long Acct_Link_Count { get; set; }
        public string Event_Timestamp { get; set; }
        public AcctAuthentic Acct_Authentic { get; set; }
        public long Acct_Session_Time { get; set; }
        public long Acct_Output_Octets { get; set; }
        public long Acct_Input_Octets { get; set; }
        public long Acct_Output_Packets { get; set; }
        public long Acct_Input_Packets { get; set; }
        public AcctTerminateCause Acct_Terminate_Cause { get; set; }
        public AcctStatusType Acct_Status_Type { get; set; }
        public string Client_IP_Address { get; set; }
        public long Client_Vendor { get; set; }
        public string Client_Friendly_Name { get; set; }
        public long MS_RAS_Vendor { get; set; }
        public string MS_RAS_Version { get; set; }
        public string MS_RAS_Correlation_ID { get; set; }
        public string MS_RAS_Client_Version { get; set; }
        public string MS_RAS_Client_Name { get; set; }
        public long MS_Network_Access_Server_Type { get; set; }
        public string MS_CHAP_Domain { get; set; }
        public long MS_MPPE_Encryption_Types { get; set; }
        public string Proxy_Policy_Name { get; set; }
        public PacketType Packet_Type { get; set; }
        public long Reason_Code { get; set; }
        public string MS_RAS_RoutingDomain_ID { get; set; }
        public long Provider_Type { get; set; }
        public string SAM_Account_Name { get; set; }
        public string Fully_Qualifed_User_Name { get; set; }
        public long Authentication_Type { get; set; }
        public long MS_Extended_Quarantine_State { get; set; }
        public long MS_Quarantine_State { get; set; }
        public string EAP_Friendly_Name { get; set; }
        public long MS_MPPE_Encryption_Policy { get; set; }
        public long Quarantine_Update_Non_Compliant { get; set; }
        public string NP_Policy_Name { get; set; }
    }
}
