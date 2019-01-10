﻿using System.ComponentModel.DataAnnotations;

namespace RestSupplyMVC.Models
{
    public class LoginViewModel
    {
        // TODO make sure fields match UserSet table
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; internal set; }
        public string UserName { get; internal set; }
    }
}