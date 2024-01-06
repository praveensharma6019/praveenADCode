using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;

namespace Adani.Feature.Common.Models
{
    public class EmailSendRequestModel : IValidatableObject
    {
        [Required]
        public string To { get; set; }
        
        public string From { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public List<Param> Params { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var contextObject = (EmailSendRequestModel)validationContext.ObjectInstance;
            if (contextObject != null)
            {
                string[] invalidEmails = (from p in contextObject.To.Split(';')
                                          select p.Trim() into p
                                          where !IsValidEmailAddress(p)
                                          select p).ToArray();

                if (invalidEmails.Length > 0)
                    yield return new ValidationResult("Email address is not in valid format", new[] { "To" });

                //validate CC
                if (!string.IsNullOrEmpty(contextObject.Cc))
                {
                    string[] invalidCCEmails = (from p in contextObject.Cc.Split(';')
                                                select p.Trim() into p
                                                where !IsValidEmailAddress(p)
                                                select p).ToArray();

                    if (invalidCCEmails.Length > 0)
                        yield return new ValidationResult("Email address is not in valid format", new[] { "Cc" });
                }

                // validate BCC
                if (!string.IsNullOrEmpty(contextObject.Bcc))
                {
                    string[] invalidBCCEmails = (from p in contextObject.Bcc.Split(';')
                                                 select p.Trim() into p
                                                 where !IsValidEmailAddress(p)
                                                 select p).ToArray();

                    if (invalidBCCEmails.Length > 0)
                        yield return new ValidationResult("Email address is not in valid format", new[] { "Bcc" });
                }
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

    public class Param
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
