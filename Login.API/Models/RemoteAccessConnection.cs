using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace Login.API.Controllers
{
    public class RemoteAccessConnection
    {
        public RemoteAccessConnection(PSObject pso)
        {
            this.AuthMethod = pso.GetValue(nameof(AuthMethod));
            this.Bandwidth = pso.ParseInt(nameof(Bandwidth));
            this.ConnectionDuration = pso.ParseTimeSpanFromSeconds(nameof(ConnectionDuration));
            this.ConnectionStartTime = pso.ParseDate(nameof(ConnectionStartTime));
            this.ConnectionType = pso.ParseEnum<ConnectionType>(nameof(ConnectionType));
            this.TotalBytesIn = pso.ParseInt(nameof(TotalBytesIn));
            this.TotalBytesOut = pso.ParseInt(nameof(TotalBytesOut));
            this.TransitionTechnology = pso.GetValue(nameof(TransitionTechnology));
            this.TunnelType = pso.ParseEnum<TunnelType>(nameof(TunnelType));
            this.UserActivityState = pso.ParseEnum<UserActivityState>(nameof(UserActivityState));
            this.UserName = pso.ParseStringArray(nameof(UserName));
            this.ClientExternalAddress = pso.GetValue(nameof(ClientExternalAddress));
            this.ClientIPv4Address = pso.GetValue(nameof(ClientIPv4Address));
        }

        public string AuthMethod { get; set; }
        public int Bandwidth { get; set; }
        public TimeSpan? ConnectionDuration { get; set; }
        public DateTimeOffset? ConnectionStartTime { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public long TotalBytesIn { get; set; }
        public long TotalBytesOut { get; set; }
        public string TransitionTechnology { get; set; }
        public TunnelType TunnelType { get; set; }
        public UserActivityState UserActivityState { get; set; }
        public string UserName { get; set; }
        public string ClientExternalAddress { get; set; }
        public string ClientIPv4Address { get; set; }
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
