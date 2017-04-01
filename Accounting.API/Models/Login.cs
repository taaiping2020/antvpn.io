using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API.Models
{
    public class Login
    {
        [StringLength(450)]
        [Required]
        public string UserId { get; set; }
        [Required]
        [StringLength(256)]
        public string LoginName { get; set; }
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public bool AllowDialIn { get; set; }
        public bool Enabled { get; set; }
        [StringLength(256)]
        public string GroupName { get; set; }
        public long? MonthlyTraffic { get; set; }
    }
}
