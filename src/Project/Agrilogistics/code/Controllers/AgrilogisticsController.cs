using Sitecore.Agrilogistics.Website.Models;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Agrilogistics.Website.Controllers
{
    public class AgrilogisticsController : Controller
    {
        // GET: Agrilogistics
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertContactdetail(AgrilogisticsContactModal m)
        {


            Log.Error("Insertcontactdetail ", "Start");

            var result = new { status = "1" };
            try
            {
                AgrilogisticsContactFormRecordDataContext rdb = new AgrilogisticsContactFormRecordDataContext();
                AgrilogisticsContactFormRecord r = new AgrilogisticsContactFormRecord();

                r.Name = m.Name;
                r.Email = m.Email;
                r.Mobile = m.Mobile;
                r.SubjectType = m.SubjectType;
                r.Message = m.Message;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.AgrilogisticsContactFormRecords.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.WriteLine(ex);
            }



            try
            {
                string to = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailSubject", "");
                string emailText = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailText", "");
                string message = "";
                message = "Hello<br><br>" + emailText + "<br><br>Name: " + m.Name;
                message = message + "<br>Email: " + m.Email + "<br>Contact Number: " + m.Mobile + "<br>Message Subject Type: " + m.SubjectType + "<br>Message: " + m.Message + "<br><br>Thanks";

                bool results = sendEmail(to, emailSubject, message, from);

                if (results)
                {
                    Log.Error("Email Sent- ", "");
                }
            }
            catch (Exception ex)
            {
                result = new { status = "1" };
                Log.Error("Failed to sent Email", ex.ToString());
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool status = false;
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                var ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                //mail.From = new MailAddress(Sitecore.Configuration.Settings.MailServerUserName);
                mail.From = new MailAddress(from);
                //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, ct);
                // attachment.ContentDisposition.FileName = fileName;
                // mail.Attachments.Add(attachment);
                MainUtil.SendMail(mail);
                status = true;
                return status;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message, "sendEmail - ");
                Log.Error(ex.Message, "sendEmail - ");
                Log.Error(ex.InnerException.ToString(), "sendEmail - ");
                return status;
            }
        }
    }
}