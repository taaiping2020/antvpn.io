using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.ViewModels
{
    public class Login
    {
        public int Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [Required]
        [StringLength(256)]
        public string LoginName { get; set; }
        [Required]
        [StringLength(256)]
        public string NormalizedLoginName { get; set; }
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool AllowDialIn { get; set; }
        public bool Enabled { get; set; }
        [StringLength(256)]
        public string GroupName { get; set; }
    }
}
