using FoodRescueTrackerSystem.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodRescueTrackerSystem.DTOs
{
    public class NGOAuthorityDTO
    {
        [Required]
        [RegularExpression("^(?=.{1,64}$)[a-z][a-z0-9.-]*[a-z0-9]@[a-z]{2,}(?:\\.[a-z0-9]+)*\\.[a-z]{2,}$", ErrorMessage = "Invalid Email Format")]
        [UniqueEmpAuthorValidator(ErrorMessage = "This email has been used already. Choose another email.")]
        public string Email { get; set; }
        private string name;

        [Required]
        [RegularExpression("^[a-zA-Z ]{1,16}$", ErrorMessage = "Name can contain only alphabets with maximum length 16")]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value?.Trim();
            }
        }
        [Required]
        [RegularExpression("^(?=.*[A-Za-z].*[A-Za-z])(?=.*\\d)(?=.*[^A-Za-z\\d\\s].*[^A-Za-z\\d\\s]).{8,}$", ErrorMessage = "* At least 8 characters. * No space allowed. * At least 2 alphabets, 1 number, 2 special characters.")]
        public string Password { get; set; }
    }
}