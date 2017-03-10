using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Windows
{
    public static class PowershellParser
    {
        public static DateTime? ParseDate(this PSObject pso, string propName)
        {
            var v = pso.Members[propName]?.Value?.ToString();
            if (String.IsNullOrEmpty(v))
            {
                return null;
            }

            return DateTime.Parse(v);
        }

        public static TimeSpan? ParseTimeSpan(this PSObject pso, string propName)
        {
            var v = pso.Members[propName]?.Value?.ToString();
            if (String.IsNullOrEmpty(v))
            {
                return null;
            }

            return TimeSpan.FromTicks(long.Parse(v));
        }

        public static TimeSpan? ParseTimeSpanFromSeconds(this PSObject pso, string propName)
        {
            var v = pso.Members[propName]?.Value?.ToString();
            if (String.IsNullOrEmpty(v))
            {
                return null;
            }

            return TimeSpan.FromSeconds(long.Parse(v));
        }

        public static bool? ParseBool(this PSObject pso, string propName)
        {
            var v = pso.Members[propName]?.Value?.ToString();
            if (String.IsNullOrEmpty(v))
            {
                return null;
            }

            return bool.Parse(v);
        }

        public static int ParseInt(this PSObject pso, string propName)
        {
            var v = pso.Members[propName]?.Value?.ToString();
            if (String.IsNullOrEmpty(v))
            {
                return 0;
            }

            return int.Parse(v);
        }

        public static T ParseEnum<T>(this PSObject pso, string propName)
        {
            var v = pso.Members[propName].Value.ToString();
            return (T)Enum.Parse(typeof(T), v);
        }

        public static long ParseLong(this PSObject pso, string propName)
        {
            var v = pso.Members[propName]?.Value?.ToString();
            if (String.IsNullOrEmpty(v))
            {
                return 0;
            }

            return long.Parse(v);
        }

        public static IPAddress ParseIPAddress(this PSObject pso, string propName)
        {
            var v = pso.Members[propName]?.Value?.ToString();
            if (String.IsNullOrEmpty(v))
            {
                return null;
            }

            return IPAddress.Parse(v);
        }

        public static string ParseStringArray(this PSObject pso, string propName)
        {
            var v = pso.Members[propName]?.Value as string[];
            if (v == null)
            {
                return null;
            }

            return v.FirstOrDefault()?.Split('\\')?.LastOrDefault()?.ToLower();
        }
        public static string GetValue(this PSObject pso, string propName) => pso.Members[propName]?.Value?.ToString();

        public static RemoteAccessConnection GetRemoteAccessConnection(this PSObject pso, string machineName, DateTime timestamp)
        {
            RemoteAccessConnection rac = new RemoteAccessConnection();
            rac.MachineName = machineName;
            rac.AuthMethod = pso.GetValue(nameof(RemoteAccessConnection.AuthMethod));
            rac.Bandwidth = pso.ParseInt(nameof(RemoteAccessConnection.Bandwidth));
            rac.ConnectionDuration = pso.ParseTimeSpanFromSeconds(nameof(RemoteAccessConnection.ConnectionDuration));
            rac.ConnectionStartTime = pso.ParseDate(nameof(RemoteAccessConnection.ConnectionStartTime));
            rac.ConnectionType = pso.ParseEnum<ConnectionType>(nameof(ConnectionType));
            rac.TotalBytesIn = pso.ParseInt(nameof(RemoteAccessConnection.TotalBytesIn));
            rac.TotalBytesOut = pso.ParseInt(nameof(RemoteAccessConnection.TotalBytesOut));
            rac.TransitionTechnology = pso.GetValue(nameof(RemoteAccessConnection.TransitionTechnology));
            rac.TunnelType = pso.ParseEnum<TunnelType>(nameof(TunnelType));
            rac.UserActivityState = pso.ParseEnum<UserActivityState>(nameof(UserActivityState));
            rac.UserName = pso.ParseStringArray(nameof(RemoteAccessConnection.UserName));
            rac.ClientExternalAddress = pso.GetValue(nameof(RemoteAccessConnection.ClientExternalAddress));
            rac.ClientIPv4Address = pso.GetValue(nameof(RemoteAccessConnection.ClientIPv4Address));
            rac.TimeStamp = timestamp;

            return rac;
        }
    }
}
