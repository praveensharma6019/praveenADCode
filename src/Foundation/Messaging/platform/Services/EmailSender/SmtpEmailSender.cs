using Sitecore.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace Adani.Foundation.Messaging.Services.EmailSender
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly string _port;
        private readonly string _host;
        private readonly string _from;
        private readonly string _password;

        public SmtpEmailSender()
        {
            _host = Settings.GetSetting("SmtpMailSender:Host");
            _from =     Settings.GetSetting("SmtpMailSender:From");
            _password = Settings.GetSetting("SmtpMailSender:Password");
            _port = Settings.GetSetting("SmtpMailSender:Port");
        }

        public bool SendMail(MailMessage message)
        {
            try
            {
                SmtpClient smtp = new SmtpClient()
                {
                    Port = string.IsNullOrEmpty(_port) ? 587 : Convert.ToInt32(_port),
                    Host = _host,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_from, _password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                smtp.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
