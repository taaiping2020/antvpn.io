using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public class ServerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IPv4 { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string CountryName { get; set; }
        public string CountryFlag { get; set; }

        public string RedirectorServerName { get; set; }
        public string RedirectorServerCountryName { get; set; }
        public string RedirectorServerCountryFlag { get; set; }

        public string TrafficServerName { get; set; }
        public string TrafficServerCountryName { get; set; }
        public string TrafficServerCountryFlag { get; set; }
        public bool Off { get; set; }
        public bool IsPublic { get; set; }
        public bool IsHybrid { get; set; }
        public int[] Protocals { get; set; }
        public HealthReport HealthReport { get; set; }
    }
}
