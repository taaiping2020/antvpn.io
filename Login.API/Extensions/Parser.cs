using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Threading.Tasks;

namespace Login.API.Controllers
{
    public static class Parser
    {
        public static DateTimeOffset? ParseDate(this PSObject pso, string propName)
        {
            var v = pso.Members[propName]?.Value?.ToString();
            if (String.IsNullOrEmpty(v))
            {
                return null;
            }

            return DateTimeOffset.Parse(v);
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

            return v.FirstOrDefault()?.Split('\\')?.LastOrDefault();
        }
        public static string GetValue(this PSObject pso, string propName) => pso.Members[propName]?.Value?.ToString();
    }
}
