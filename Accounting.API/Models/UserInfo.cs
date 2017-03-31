using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API.Models
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public long MonthlyTraffic { get; set; }
    }
}
