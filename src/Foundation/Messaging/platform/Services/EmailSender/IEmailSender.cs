using System.Net.Mail;

namespace Adani.Foundation.Messaging.Services.EmailSender
{
    public interface IEmailSender
    {
        bool SendMail(MailMessage mailMessage);
    }
}
