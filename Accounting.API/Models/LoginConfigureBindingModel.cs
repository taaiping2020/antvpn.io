using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API.Models
{
    public class LoginConfigureBindingModel
    {
        public string UserName { get; set; }
        public long? MonthlyTraffic { get; set; }
    }
}
