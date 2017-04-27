using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.ViewModels
{
    public class LoginConfigureBindingModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]{2,}", ErrorMessage = "The field Username must start with Letter, at least 3 characters, may include number.")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        public long? MonthlyTraffic { get; set; }
    }
}
