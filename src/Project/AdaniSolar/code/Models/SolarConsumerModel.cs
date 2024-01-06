using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniSolar.Website.Models
{
    public class SolarConsumerModel : IValidatableObject
    {
        [RegularExpression(@"^[#.0-9a-zA-Z\s,-]+$", ErrorMessage = "special characters are not allowed.")]
        public string UserName { get; set; }

        [RegularExpression(@"^[#.0-9a-zA-Z\s,-]+$", ErrorMessage = "special characters are not allowed.")]
        public string Address { get; set; }

        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not allowed.")]
        public string Country { get; set; }

        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not allowed.")]
        public string State { get; set; }

        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not allowed.")]
        public string City { get; set; }

        public string Email { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string Mobile { get; set; }
        public string Response { get; set; }

        public DateTime? FormSubmitOn { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Custom validation logic goes here
            if (string.IsNullOrEmpty(UserName))
            {
                yield return new ValidationResult("Username is required");
            }
            if (string.IsNullOrEmpty(Address))
            {
                yield return new ValidationResult("Address is required");
            }
            if (string.IsNullOrEmpty(Country))
            {
                yield return new ValidationResult("Country is required");
            }
            if (string.IsNullOrEmpty(State))
            {
                yield return new ValidationResult("State is required");
            }
            if (string.IsNullOrEmpty(City))
            {
                yield return new ValidationResult("City is required");
            }
            if (string.IsNullOrEmpty(Email))
            {
                yield return new ValidationResult("Email is required");
            }
            if (string.IsNullOrEmpty(Mobile))
            {
                yield return new ValidationResult("Mobile Value is required");
            }
        }
    }
    public class Consumer
    {
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "405")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter the Serial Number")]
        public string Serial_No { get; set; }
    }

    public class ConsumerInvoice
    {
        public Consumer Record { get; set; }
    }
}