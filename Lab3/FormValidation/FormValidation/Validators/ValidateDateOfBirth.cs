using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace FormValidation
{
    [AttributeUsage(AttributeTargets.Property |
  AttributeTargets.Field, AllowMultiple = false)]
    sealed public class ValidateDateOfBirth : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = false;
            if ((value != null))
            {
                if (DateTime.TryParse(value.ToString(), out DateTime birthDate))
                {
                    int age = DateTime.Today.Year - birthDate.Year;

                    if (birthDate > DateTime.Today.AddYears(-age))
                    {
                        age--;
                    }

                    result = age > 18;
                }
            }
            return result;
        }
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }
    }
}