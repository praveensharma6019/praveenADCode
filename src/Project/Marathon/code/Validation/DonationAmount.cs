using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Marathon.Website.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DonationAmount : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var CharityRaceDetails = Sitecore.Context.Database.GetItem("{7B220E91-F649-4E9C-8DDA-A72451876EDE}");
            var model = (Models.RegisteredUserRegistration)validationContext.ObjectInstance;
            if (model.RunType.ToString().Equals("Charity"))
            {
                foreach (var charityRace in CharityRaceDetails.GetChildren().ToList())
                {
                    if (charityRace.Fields["Distance"].Value.ToString().Equals(model.RaceDistance))
                    {
                        if (decimal.Parse(model.DonationAmount.ToString()) >= decimal.Parse(charityRace.Fields["Amount"].Value.ToString()))
                        {
                            return ValidationResult.Success;
                        }
                    }
                }
                return new ValidationResult("Please enter a valid donation amount");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}