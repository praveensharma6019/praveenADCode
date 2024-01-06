using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateTimeNotLessThan : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "Start date can't be less than End Date.";

        public string OtherProperty { get; private set; }

        public DateTimeNotLessThan(string otherProperty)
            : base(DefaultErrorMessage)
        {
            if (!string.IsNullOrEmpty(otherProperty))
            {
                OtherProperty = otherProperty;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty);
                var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

                DateTime dtThis = System.Convert.ToDateTime(value);
                DateTime dtOther = System.Convert.ToDateTime(otherPropertyValue);

                if (dtThis < dtOther)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                else if((dtThis-dtOther).TotalDays > 365)
                {
                    return new ValidationResult("Temporary Connection can't be more than 365 days");
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
                                                      ModelMetadata metadata,
                                                      ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "notlessthan"//
            };

            clientValidationRule.ValidationParameters.Add("otherproperty", OtherProperty);

            return new[] { clientValidationRule };
        }
    }
}