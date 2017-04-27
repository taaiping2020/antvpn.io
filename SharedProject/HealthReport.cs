using System;

namespace SharedProject
{
    public class HealthReport
    {
        public string MachineName { get; set; }
        public double NetworkBytesInPerSec { get; set; }
        public double NetworkBytesOutPerSec { get; set; }
        public double NetworkBytesTotalPerSec { get; set; }
        public double ProcessorTime { get; set; }
        public double AvailableMemoryMegaBytes { get; set; }

        public DateTime BeginTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public int SampleIntervalInSec { get; set; }

        public string CertIssuer { get; set; }
        public string CertSubject { get; set; }
        public DateTime CertNotBefore { get; set; }
        public DateTime CertNotAfter { get; set; }

        public string RoutingStatus { get; set; }
        public string SstpProxyStatus { get; set; }
        public string VpnS2SStatus { get; set; }
        public string VpnStatus { get; set; }
        public bool UseHttp { get; set; }
        public bool IsShadowsocksServerRunning { get; set; }
    }
}
