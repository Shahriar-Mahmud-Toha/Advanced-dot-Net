using FoodRescueTrackerSystem.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FoodRescueTrackerSystem.Validators
{
    [AttributeUsage(AttributeTargets.Property |
AttributeTargets.Field, AllowMultiple = false)]
    sealed public class UniqueAdminAuthorValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = false;
            var db = new FoodRescueTrackerSystemEntities();
            if (db.NGOAuthorities.Find(value) == null)
            {
                result = true;
            }
            return result;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }
    }

    [AttributeUsage(AttributeTargets.Property |
AttributeTargets.Field, AllowMultiple = false)]
    sealed public class UniqueEmpAuthorValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = false;
            var db = new FoodRescueTrackerSystemEntities();
            if (db.NGOEmployees.Find(value) == null)
            {
                result = true;
            }
            return result;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }
    }

    [AttributeUsage(AttributeTargets.Property |
AttributeTargets.Field, AllowMultiple = false)]
    sealed public class UniqueResAuthorValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = false;
            var db = new FoodRescueTrackerSystemEntities();
            if (db.RestaurantAuthorities.Find(value) == null)
            {
                result = true;
            }
            return result;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }
    }
}