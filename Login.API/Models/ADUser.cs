using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login.API.Controllers
{
    public class ADUser
    {
        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool? Enabled { get; set; }
        public bool? AllowDialIn { get; set; }
        public string GroupName { get; set; }
    }
}
