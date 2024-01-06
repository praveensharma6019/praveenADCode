using System;
using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Infra.Website;
using Sitecore.Infra.website.Models;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Sitecore.Infra.Website.Controllers
{
    public class InfraController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
        public ActionResult InsertContact(InfraContactModal m)
        {
            bool validationStatus = true;
            var result = new { status = "1" };
            Log.Error("Insertcontactdetail of Infra ", "Start");
            //var getEmailTo = "";
            //string getMessage = "";
            //string getEmailSubject = "";

            try
            {
                validationStatus = IsReCaptchValid(m.reResponse);
            }
            catch (Exception ex)
            {
                result = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }

            if (validationStatus == true)
            {
                Log.Error("InsertInfraContactFormRecord ", "Start");
                var getEmailTo = "";
                try
                {
                   AdaniInfraContactFormDataContext rdb = new AdaniInfraContactFormDataContext();
                    AdaniInfraContactForm r = new AdaniInfraContactForm();
                    bool isValid = true;
                    string errorMessage = "Invalid field values:";

                    if (string.IsNullOrEmpty(m.Name) || !Regex.IsMatch(m.Name, "^[a-zA-Z][a-zA-Z ]*$"))
                    {
                        errorMessage = errorMessage + " Name";
                        isValid = false;
                    }
                    if (string.IsNullOrEmpty(m.Email) || !Regex.IsMatch(m.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    {
                        errorMessage = errorMessage + " Email";
                        isValid = false;
                    }
                    if (string.IsNullOrEmpty(m.Mobile) || !Regex.IsMatch(m.Mobile, @"^[0-9]{10,10}$"))
                    {
                        errorMessage = errorMessage + "Mobile";
                        isValid = false;
                    }

                    //validate MessageType
                    if (string.IsNullOrEmpty(m.MessageType))
                    {
                        errorMessage = errorMessage + " MessageType";
                        isValid = false;
                    }
                    //validate message
                    if (string.IsNullOrEmpty(m.Message))
                    {
                        errorMessage = errorMessage + " Message";
                        isValid = false;
                    }

                    if (!isValid)
                    {
                        result = new { status = "4" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    r.Name = m.Name;
                    r.Email = m.Email;
                    r.MessageType = m.MessageType;
                    r.Message = m.Message;
                    r.Mobile = m.Mobile;
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = m.FormSubmitOn;



                    #region Insert to DB
                    rdb.AdaniInfraContactForms.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }

                try
                {
                    var msgTpye = Sitecore.Context.Database.GetItem("{F6503BC8-C146-4502-BBCC-AEE546BDA7DA}");
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