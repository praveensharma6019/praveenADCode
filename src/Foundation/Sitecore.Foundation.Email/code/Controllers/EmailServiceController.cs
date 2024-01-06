using Sitecore.Diagnostics;
using Sitecore.Foundation.Email.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using MailMessage = System.Net.Mail.MailMessage;

namespace Sitecore.Foundation.Email.Controllers
{
    public class EmailServiceController : Controller
    {
        // GET: Email
        [HttpPost]
        public ActionResult SendEmail(EmailModel emailModel)
        {
            try
            {
                Log.Error("Email Start TRY", this);
                var From = emailModel.From;
                var Subject = emailModel.Subject;
                var To = emailModel.To;
                var Cc = emailModel.Cc;
                var Bcc = emailModel.Bcc;
                string Body = HttpUtility.HtmlDecode(emailModel.Body); ;

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(From),
                    Body = Body,
                    Subject = Subject,
                    IsBodyHtml = true,

                };
                Log.Error("Email After Body", this);


                foreach (var address in To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                   // mail.To.Add(new MailAddress(address));
                    mail.To.Add(address);
                }


                MainUtil.SendMail(mail);
                emailModel.isSucess = true;
                //Result = new { status = "126", message = "Mail Sent" };
            }
            catch (Exception ex)
            {
                emailModel.isSucess = false;
                emailModel.isError = true;
                emailModel.ErrorMessage = ex.Message + "Error Detail" + ex.StackTrace;
                //result = new { status = "127", message = ex.Message };
                Sitecore.Diagnostics.Log.Error(ex.Message + " : ", ex, this);
                Log.Error("Email Not Working", this);
                return Json(emailModel,JsonRequestBehavior.AllowGet);
            }
            return Json(emailModel, JsonRequestBehavior.AllowGet);
        }
    }
}