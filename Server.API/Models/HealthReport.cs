using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.API.Models
{
    public class HealthReport
    {
        public int Id { get; set; }
        public string HealthReportJson { get; set; }
    }
}
