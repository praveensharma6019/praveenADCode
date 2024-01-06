using Sitecore.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;

namespace Adani.Foundation.Messaging.Services.EmailSender
{
    public class ApiEndpointEMailSender : IEmailSender
    {
        public bool SendMail(MailMessage mailMessage)
        {
            using (var client = new HttpClient())
            {
                var apiUrl = Settings.GetSetting("EmailSender:ApiPath");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("To", string.Join(";", mailMessage.To)),
                    new KeyValuePair<string, string>("From", mailMessage.From.Address),
                    new KeyValuePair<string, string>("Cc", string.Join(";", mailMessage.CC)),
                    new KeyValuePair<string, string>("Bcc", string.Join(";", mailMessage.Bcc)),
                    new KeyValuePair<string, string>("Subject", mailMessage.Subject),
                    new KeyValuePair<string, string>("Body", mailMessage.Body)
                };

                var content = new FormUrlEncodedContent(postData);

                HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}
