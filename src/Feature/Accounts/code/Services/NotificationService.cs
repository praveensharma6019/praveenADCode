namespace Sitecore.Feature.Accounts.Services
{
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.DependencyInjection;
    using System.IO;
    using System.Net.Mail;
    using System.Text;

    [Service(typeof(INotificationService))]
    public class NotificationService : INotificationService
    {
        private readonly IAccountsSettingsService siteSettings;

        public NotificationService(IAccountsSettingsService siteSettings)
        {
            this.siteSettings = siteSettings;
        }

        public void SendPassword(string email, string newPassword)
        {
            var mail = this.siteSettings.GetForgotPasswordMailTemplate();
            mail.To.Add(email);
            mail.Body = mail.Body.Replace("$password$", newPassword);

            MainUtil.SendMail(mail);
        }

        public void SendPasswordResetLink(string email,string userName, string redirectUrl)
        {

            try
            {
                var mail = this.siteSettings.GetForgotPasswordMailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("{0}", userName);
                mail.Body = mail.Body.Replace("{1}", redirectUrl);
                //var mailbody = new StringBuilder();
                //mailbody.AppendLine("<html><head><title>");
                //mailbody.AppendLine("Sitecore password recovery");
                //mailbody.AppendLine("</title></head><body>");
                //mailbody.AppendLine("<h1>Please confirm</h1>");
                ////sb.AppendLine("<p>Hi " + user.Profile.FullName + ",<br/></p>");
                //mailbody.AppendLine("<p>Please follow the link below to recover your password</p>");
                //mailbody.AppendLine("<a href=\"" + redirectUrl + "\">Click Here</a>");
                //mailbody.AppendLine("</body>");
                //mailbody.AppendLine("</html>");
                ////return sb.ToString();
                //MailMessage mailinfo = new MailMessage()
                //{
                //    IsBodyHtml=true,
                //    Subject = "Password Reset Link",
                //    Body = mailbody.ToString()
                //};
                //mailinfo.From = new MailAddress("rajdeep.parmar@incitesolvents.com");
                //mailinfo.To.Add("rajdeep.parmar@incitesolvents.com");

                //SmtpClient smtp = new SmtpClient(Sitecore.Configuration.Settings.MailServer, Sitecore.Configuration.Settings.MailServerPort);
                //smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential(Sitecore.Configuration.Settings.MailServerUserName, Sitecore.Configuration.Settings.MailServerPassword);
                //smtp.Send(mail);
                //string dataFolder = Sitecore.Configuration.Settings.mai;
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + email + " - Error - " + ex.Message + "", ex, this);
                throw;
            }
        }

        public bool SendComposeMail(string to, string subject, string body, byte[] attachedfile, string fileName)
        {
            bool status = false;
            try
            {
                var mail = new MailMessage();
                System.Net.Mail.Attachment attachment;
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(Sitecore.Configuration.Settings.MailServerUserName);
                if (attachedfile != null && !string.IsNullOrEmpty(fileName))
                {
                    var fileStream = new MemoryStream(attachedfile);
                    attachment = new Attachment(fileStream, fileName);
                    mail.Attachments.Add(attachment);
                }
                MainUtil.SendMail(mail);
                status = true;
                return status;
            }
            catch (System.Exception ex)
            {
                Log.Error("SendComposeMail - ", ex.Message);
                return status;
            }
        }

        public bool SendBill(string to, string subject, string body, MemoryStream ms, string fileName, string from)
        {
            bool status = false;
            try
            {
                var mail = new MailMessage();
                mail.From= new MailAddress("Helpdesk.Mumbaielectricity@adani.com");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                var ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                mail.From = new MailAddress(Sitecore.Configuration.Settings.MailServerUserName);
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, ct);
                attachment.ContentDisposition.FileName = fileName;
                mail.Attachments.Add(attachment);
                MainUtil.SendMail(mail);
                status = true;
                return status;                
            }
            catch (System.Exception ex)
            {
                Log.Error("SendBill - ", ex.Message);
                return status;
            }
        }
    }
}
