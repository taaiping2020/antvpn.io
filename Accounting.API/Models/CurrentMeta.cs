using System;
using System.ComponentModel.DataAnnotations;

namespace Accounting.API.Models
{
    public class CurrentMeta
    {
        [StringLength(100)]
        public string MachineName { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}