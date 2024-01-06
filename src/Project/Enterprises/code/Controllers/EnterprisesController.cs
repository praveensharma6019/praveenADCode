using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Enterprises.Website.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Sitecore.Foundation.SitecoreExtensions.Attributes;

namespace Sitecore.Enterprises.Website.Controllers
{
    public class EnterprisesController : Controller
    {
        // GET: Enterprises
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateRenderingId]
        public JsonResult GetContactAddress(string itemId)
        {
            var Location = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(itemId));
            string body = Location.Fields["Body"].Value;
            body = body + "Summary:" + Location.Fields["Summary"].Value;
            return Json(body);
        }

        public bool IsReCaptchValid(string reResponse)
        {
            var result = false;
            // var captchaResponse = Request.Form["g-recaptcha-response"];
            var captchaResponse = reResponse;
            string secretKey = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "");
            //var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            // var secretKey = "6LdkC64UAAAAAJiii15Up9xETgsLuPQnQ1BKZft8";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult InsertContactFormdetail(EnterprisesContactModel m)
        {
            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating Enterprises ContactForm to stop auto script ", "Start");

            try
            {
                validationStatus = IsReCaptchValid(m.reResponse);
                //if (Request.Cookies["SIDCC"]!=null)
                //{
                //    if (Session["validate"] == null)
                //    {
                //        validationStatus = true;
                //    }
                //    else
                //    {
                //        if (Session["validate"].ToString() != Request.Cookies["SIDCC"].Value)
                //        {
                //            validationStatus = true;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }

            if (validationStatus == true)
            {
                Log.Error("InsertEnterprisesContactFormRecord ", "Start");
                var getEmailTo = "";

                try
                {
                    EnterprisesContactFormRecordDataContext rdb = new EnterprisesContactFormRecordDataContext();
                    EnterprisesContactFormRecord r = new EnterprisesContactFormRecord();

                    r.Name = m.Name;
                    r.Email = m.Email;
                    r.Mobile = m.Mobile;
                    r.MessageType = m.MessageType;
                    r.Message = m.Message;
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = m.FormSubmitOn;



                    #region Insert to DB
                    rdb.EnterprisesContactFormRecords.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                    //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }

                try
                {
                    var msgTpye = Sitecore.Context.Database.GetItem("{DE1168F0-8EBF-4C53-9858-D39768FBFF94}");
                    var getfilteredItem = msgTpye.Children.Where(x => x.Fields["SubjectName"].Value.ToLower() == m.MessageType.ToLower());

                    foreach (var itemData in getfilteredItem.ToList())
                    {
                        getEmailTo = itemData.Fields["EmailTo"].Value;
                    }
                }
                catch (Exception ex)
                {
                    result = new { status = "1" };
                    Log.Error("Failed to get subject specific Email", ex.ToString());
                }

                try
                {
                    string emailText = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailText", "");
                    string message = "";
                    message = "Hello<br><br>" + emailText + "<br><br>";
                    message = message + "<br>Name: " + m.Name + "<br>Email-Id: " + m.Email + "<br>Subject of Message: " + m.MessageType + "<br>Contact Number: " + m.Mobile + "<br>Message: " + m.Message + "<br><br>Thanks";
                    //string to = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailTo", "");
                    string from = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailFrom", "");
                    string emailSubject = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailSubject", "");
                    bool results = sendEmail(getEmailTo, emailSubject, message, from);

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
            }
            else
            {
                result = new { status = "2" };

            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult InsertSubscribeUsFormdetail(EnterprisesContactModel m)
        {
            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating Enterprises ContactForm to stop auto script ", "Start");

            try
            {
                validationStatus = IsReCaptchValid(m.reResponse);
                //if (Request.Cookies["SIDCC"]!=null)
                //{
                //    if (Session["validate"] == null)
                //    {
                //        validationStatus = true;
                //    }
                //    else
                //    {
                //        if (Session["validate"].ToString() != Request.Cookies["SIDCC"].Value)
                //        {
                //            validationStatus = true;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }

            if (validationStatus == true)
            {



                Log.Error("InsertSubscribeUsFormdetail", "Start");
            
            try
            {
                EnterprisesContactFormRecordDataContext rdb = new EnterprisesContactFormRecordDataContext();
                EnterprisesContactFormRecord r = new EnterprisesContactFormRecord();

                r.Name = m.Name;
                r.Email = m.Email;
                r.Mobile = m.Mobile;
                r.Message = m.Message;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.EnterprisesContactFormRecords.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.WriteLine(ex);
            }



            try
            {
                string emailText = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailText", "");
                string message = "";
                message = "Hello<br><br>" + emailText + "<br><br>";
                message = message + "<br>Email-Id: " + m.Email + "<br><br>Thanks";
                string to = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailSubject", "");
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
            }
            else
            {
                result = new { status = "2" };

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