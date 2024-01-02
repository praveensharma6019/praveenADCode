using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Adani.Feature.Common.Models
{
    public class OtpRequestModel : IValidatableObject
    {
        [Required]
        public string ID { get; set; }
        public SMSData SMSData { get; set; }
        public MailData MailData { get; set; }
        public int? Length { get; set; }
        public int? ExpiresInMinutes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var contextObject = (OtpRequestModel)validationContext.ObjectInstance;
            if (contextObject != null)
            {
                if (contextObject.MailData == null && contextObject.SMSData == null)
                    yield return new ValidationResult("Both MailData and SMSData is null. It must have value for any one of them.", new[] { "" });

                if (contextObject.MailData != null && !IsValidEmailAddress(contextObject.MailData.To))
                    yield return new ValidationResult("Email format is not in valid format", new[] { "MailData.To" });

            }
        }

        static bool IsValidEmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class MailData
    {
        [Required] public string To { get; set; }

        [Required] public string From { get; set; }

        [Required] public string Subject { get; set; }

        [Required] public string Body { get; set; }
    }

    public class SMSData
    {

        [Required] public string Recipient { get; set; }
        [Required] public string Body { get; set; }
    }
}