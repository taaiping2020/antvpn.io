﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WebMVC.ViewModels
{
    public class LoginBindingModel
    {
        [Required]
        //[RegularExpression("[a-zA-Z]+[a-zA-Z0-9]+")]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]{2,}", ErrorMessage = "The field Username must start with Letter, at least 3 characters, may include number.")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 100, MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 100, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
    }
}
