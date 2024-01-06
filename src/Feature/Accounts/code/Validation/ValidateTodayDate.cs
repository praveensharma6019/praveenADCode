using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Validation
{
    public class ValidateTodayDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date;
            if (value != null && !string.IsNullOrEmpty(value.ToString()) && value.ToString() != "01-01-0001 00:00:00")
            {
                bool parsed = DateTime.TryParse(value.ToString(), out date);
                if (!parsed)
                    return new ValidationResult("Invalid Date");
                else if (date < DateTime.Now)
                {
                    try
                    {
                        var msg = string.Format("Date should be greater than today date");
                        return new ValidationResult(msg);
                    }
                    catch (Exception e)
                    {
                        return new ValidationResult(e.Message);
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}