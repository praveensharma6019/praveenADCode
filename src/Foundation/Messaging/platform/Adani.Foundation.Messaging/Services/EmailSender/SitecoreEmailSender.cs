using Sitecore;
using System.Net.Mail;
using System.Web;

namespace Adani.Foundation.Messaging.Services.EmailSender
{
    public class SitecoreEmailSender : IEmailSender
    {
        public bool SendMail(MailMessage message)
        {
            try
            {
                message.Body = HttpUtility.HtmlDecode(message.Body);
                MainUtil.SendMail(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
