using System.Net.Mail;
using Adani.Foundation.Messaging.Models;
using System.Collections.Generic;
using System.Linq;
using Adani.Foundation.Messaging.Services.EmailSender;

namespace Adani.Foundation.Messaging.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _mailSender;

        public EmailService(IEmailSender mailSender)
        {
            _mailSender = mailSender;
        }

        public bool Send(EmailData emailData, IEnumerable<KeyValuePair<string, string>> formData)
        {
            if (emailData == null)
                return false;

            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    SetMessageAttributes(mailMessage, emailData, formData);
                    return _mailSender.SendMail(mailMessage);
                }
            }
            catch
            {
                return false;
            }
        }

        private void SetMessageAttributes(MailMessage message, EmailData data, IEnumerable<KeyValuePair<string, string>> formData)
        {
            if (!string.IsNullOrEmpty(data.From))
            {
                message.From = new MailAddress(ReplaceFieldValues(data.From, formData));
            }
            message.Subject = ReplaceFieldValues(data.Subject, formData);
            message.Body = ReplaceFieldValues(data.Body, formData);
            message.IsBodyHtml = true;

            AddRecipients(message.To, ReplaceFieldValues(data.To, formData));
            AddRecipients(message.CC, ReplaceFieldValues(data.Cc, formData));
            AddRecipients(message.Bcc, ReplaceFieldValues(data.Bcc, formData));
        }

        private static string ReplaceFieldValues(string content, IEnumerable<KeyValuePair<string, string>> formData)
        {
            if (formData == null || string.IsNullOrEmpty(content)) return content;

            foreach (var item in formData)
            {
                content = content.Replace($"[{item.Key}]", item.Value);
            }

            return content;
        }

        private static void AddRecipients(MailAddressCollection mailAddressCollection, string mailAddresses)
        {
            if (string.IsNullOrEmpty(mailAddresses)) return;

            string[] array = (from p in mailAddresses.Split(';')
                              select p.Trim() into p
                              where !string.IsNullOrEmpty(p)
                              select p).ToArray();

            foreach (string addresses in array)
            {
                mailAddressCollection.Add(addresses);
            }
        }
    }
}
