using Org.BouncyCastle.Asn1.Ocsp;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Validation
{
    public class DOBDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date;
            NewConnectionApplication model = new NewConnectionApplication();
           
            if (value != null && !string.IsNullOrEmpty(value.ToString()) && value.ToString() != "01-01-0001 00:00:00")
            {
                
                bool parsed = DateTime.TryParse(value.ToString(), out date);
               
                if (!parsed)
                    return new ValidationResult("Invalid Date");
                else
                {
                    //change below as per requirement
                    var min = DateTime.Now.AddYears(-18); //for min 18 age
                    var max = DateTime.Now.AddYears(-100); //for max 100 age
                    
                    var msg = string.Format("Please enter a valid date of birth i.e 18 years or above", max, min);
                    try
                    {
                        if ((!model.DateofBirth.HasValue))
                        {
                            if (date > min || date < max)
                            {
                            
                                return new ValidationResult(msg);

                            }

                            else
                            {
                                return ValidationResult.Success;
                            }
                        }
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