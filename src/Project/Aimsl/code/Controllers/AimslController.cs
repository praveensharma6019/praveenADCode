using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.Aimsl.Website;
using Sitecore.Aimsl.Website.Models;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Aimsl.Website.Controllers
{
    public class AimslController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AimslSustainabilityInnerContent()
        {
            string dataSource = RenderingContext.Current.Rendering.DataSource;
            Item item = Context.Database.GetItem(dataSource);
            string fileName = Path.GetFileName(base.Request.Url.AbsolutePath);
            IEnumerable<Item> children =
                from i in item.Children
                where !item.DisplayName.Contains(fileName)
                select i;
            return base.View("AimslSustainabilityInnerContent", children);
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
        public ActionResult InsertContact(AimslContactModel m)
        {
            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating Aimsl ContactForm to stop auto script ", "Start");
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
                Log.Error("InsertAimslContact", "Start");
                var getEmailTo = "";
                try
                {
                    AimslContactFormDataContext rdb = new AimslContactFormDataContext();
                    AimslContactFormRecord r = new AimslContactFormRecord();
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
                   r.Mobile = m.Mobile;
                    r.MessageType = m.MessageType;
                   r.Message = m.Message;
                   r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                   r.FormSubmitOn = m.FormSubmitOn;
                    
                    rdb.AimslContactFormRecords.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }
                try
                {
                    var msgTpye = Sitecore.Context.Database.GetItem("{EF6DC1F9-979E-4031-8FCC-830C24685723}");
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
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertSubscribeUsFormdetail(AimslContactModel m)
        {
            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating Aimsl ContactForm to stop auto script ", "Start");
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
                Log.Error("InsertAimslContact", "Start");
                var getEmailTo = "";
                try
                
                   
                {
                    AimslContactFormDataContext rdb = new AimslContactFormDataContext();
                    AimslContactFormRecord r = new AimslContactFormRecord();
                    
                        r.Email = m.Email;
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = m.FormSubmitOn;
                    
                    rdb.AimslContactFormRecords.InsertOnSubmit(r);
                   rdb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
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
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool flag;
            bool flag1 = false;
            try
            {
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(from)
                };
                mailMessage.To.Add(to);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                ContentType contentType = new ContentType("application/pdf");
                mailMessage.From = new MailAddress(from);
                MainUtil.SendMail(mailMessage);
                flag1 = true;
                flag = flag1;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Console.WriteLine(exception.Message, "sendEmail - ");
                Log.Error(exception.Message, "sendEmail - ");
                Log.Error(exception.InnerException.ToString(), "sendEmail - ");
                flag = flag1;
            }
            return flag;
        }
    }
}



