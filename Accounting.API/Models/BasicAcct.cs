using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API.Models
{
    public class BasicAcct
    {
        public string UserName { get; set; }
        public string TotalIn { get; set; }
        public string TotalOut { get; set; }
    }
}
