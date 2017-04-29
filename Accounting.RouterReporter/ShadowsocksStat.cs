using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Accounting.RouterReporter
{
    public class ShadowsocksStat
    {
        public int Port { get; set; }
        public long Traffic { get; set; }
        public static IEnumerable<ShadowsocksStat> Parse(string stat)
        {
            if (String.IsNullOrEmpty(stat))
            {
                yield break;
            }
            if (stat.Contains("stat"))
            {
                stat = stat.Replace("stat:", string.Empty);
            }
            else
            {
                yield break;
            }
            var obj = (JObject)JsonConvert.DeserializeObject(stat);
            foreach (var item in obj)
            {
                yield return new ShadowsocksStat() { Port = int.Parse(item.Key), Traffic = item.Value.ToObject<long>() };
            }
        }
    }
}
