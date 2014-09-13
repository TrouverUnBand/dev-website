﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrouveUnBand.Models
{
    public class User
    {
        [Required(ErrorMessage="This field is required")]
        [RegularExpression(@"^[a-zA-Z]{2,}$", ErrorMessage = "Must be at least 2 characters long, letters only")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[a-zA-Z]{2,}$", ErrorMessage = "Must be at least 2 characters long, letters only")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        //[RegularExpression(@"^(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Must be a valid date")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,}$", ErrorMessage = "Must be at least 3 characters long")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@".*@.*", ErrorMessage = "Must be a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[a-zA-Z]{2,}$", ErrorMessage = "Must be at least 2 letters long")]
        public string City { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[\S]{4,138}$", ErrorMessage = "Must be at least 4 letters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[\S]{4,138}$", ErrorMessage = "Must be at least 4 letters long")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Nickname or Email")]
        public string Nickname { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember password?")]
        public bool RememberMe { get; set; }
    }
}