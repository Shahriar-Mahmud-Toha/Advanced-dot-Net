using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FormValidation.Models
{
    public class Person
    {
        [Required]
        [RegularExpression("^[a-zA-Z.\\-' ]{3,50}$", ErrorMessage = "* Length at most 50 atleast 3\n* No number, special charchter allowed.\n* .(dot), -(dash), space are allowed.")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_-]{5,7}$", ErrorMessage = "* Length 5 to 7\n* No space allowed.\n* .(dot) not allowed.\n* -(dash), _(underscore) are allowed only.\n* Numbers are allowed.")]
        public string Username { get; set; }

        [Required]
        [RegularExpression("^\\d{2}-\\d{5}-[1-3]@student\\.aiub\\.edu$", ErrorMessage = "* Must follow xx-xxxxx-x@student.aiub.edu")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^\\d{2}-\\d{5}-[1-3]$", ErrorMessage = "* Must follow xx-xxxxx-x")]
        public string ID { get; set; }

        [Required]
        [RegularExpression("^(?=.*[A-Za-z].*[A-Za-z])(?=.*\\d)(?=.*[^A-Za-z\\d\\s].*[^A-Za-z\\d\\s]).{8,}$", ErrorMessage = "* At least 8 characters. * No space allowed. * At least 2 alphabets, 1 number, 2 special characters.")]
        public string Password { get; set; }

        [Required]
        [ValidateDateOfBirth(ErrorMessage ="* Age must be >18")]
        public string Dob { get; set; }

    }
}