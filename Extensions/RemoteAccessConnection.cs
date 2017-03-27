using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public class RemoteAccessConnection
    {
        public string AuthMethod { get; set; }
        public int Bandwidth { get; set; }
        public long? ConnectionDuration { get; set; }
        public DateTime? ConnectionStartTime { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public long TotalBytesIn { get; set; }
        public long TotalBytesOut { get; set; }
        public string TransitionTechnology { get; set; }
        public TunnelType TunnelType { get; set; }
        public UserActivityState UserActivityState { get; set; }
        public string UserName { get; set; }
        public string ClientExternalAddress { get; set; }
        public string ClientIPv4Address { get; set; }
        public string MachineName { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public enum ConnectionType
    {
        Vpn,
        Da
    }

    public enum UserActivityState
    {
        Idle,
        Active
    }
}
