using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.APMUL.Website;
using Sitecore.APMUL.Website.Models;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
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
using System.Web.Mvc;

namespace Sitecore.APMUL.Website.Controllers
{
    public class APMULController : Controller
    {
        public APMULController()
        {
        }

        public ActionResult Index()
        {
            return base.View();
        }

        [HttpPost]
        public ActionResult InsertContactFormdetail(APMULContactModel m)
        {
            bool validationStatus = true;
            var result = new { status = "1" };
            Log.Error("Validating Defence ContactForm to stop auto script ", "Start");
            try
            {
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                result = new { status = "2" };
                Log.Error(string.Concat("Failed to validate auto script : ", ex.ToString()), "Failed");
            }
            if (!validationStatus)
            {
                result = new { status = "2" };
            }
            else
            {
                Log.Error("InsertAPMULContact", "Start");
                string getEmailTo = "";
                try
                {
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
                    APMULContactusFormDataContext rdb1 = new APMULContactusFormDataContext();
                    APMULContactFormRecord r1 = new APMULContactFormRecord()
                    
                    {
                        Name = m.Name,
                        Email = m.Email,
                        Mobile = m.Mobile,
                        MessageType = m.MessageType,
                        Message = m.Message,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        FormSubmitOn = new DateTime?(m.FormSubmitOn)
                    };
                    rdb1.APMULContactFormRecords.InsertOnSubmit(r1);
                    rdb1.SubmitChanges();
                }
                catch (Exception exception1)
                {
                    Exception ex = exception1;
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }
                try
                {
                    Item msgTpye = Context.Database.GetItem("{CD077318-D702-4526-8ED0-19FC492650E0}");
                    IEnumerable<Item> getfilteredItem =
                        from x in msgTpye.Children
                        where x.Fields["SubjectName"].Value.ToLower() == m.MessageType.ToLower()
                        select x;
                    foreach (Item itemData in getfilteredItem.ToList<Item>())
                    {
                        getEmailTo = itemData.Fields["EmailTo"].Value;
                    }
                }
                catch (Exception exception2)
                {
                    Exception ex = exception2;
                    result = new { status = "1" };
                    Log.Error("Failed to get subject specific Email", ex.ToString());
                }
                try
                {
                    string emailText = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailText", "");
                    string message = "";
                    message = string.Concat("Hello<br><br>", emailText, "<br><br>");
                    message = string.Concat(new string[] { message, "<br>Name: ", m.Name, "<br>Email-Id: ", m.Email, "<br>Subject of Message: ", m.MessageType, "<br>Contact Number: ", m.Mobile, "<br>Message: ", m.Message, "<br><br>Thanks" });
                    string from = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailFrom", "");
                    if (this.sendEmail(getEmailTo, DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailSubject", ""), message, from))
                    {
                        Log.Error("Email Sent- ", "");
                    }
                }
                catch (Exception exception3)
                {
                    Exception ex = exception3;
                    result = new { status = "1" };
                    Log.Error("Failed to sent Email", ex.ToString());
                }
            }
            return base.Json(result, 0);
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
        public ActionResult InsertSubscribeUsFormdetail(APMULContactModel m)
        {
            Log.Error("InsertSubscribeUsFormdetail", "Start");
            bool validationStatus = false;
            var result = new { status = "1" };
            try
            {
                validationStatus = IsReCaptchValid(m.reResponse);

            }
            catch (Exception exception)
            {
                Exception ex = exception;
                result = new { status = "2" };
                Log.Error(string.Concat("Failed to validate auto script : ", ex.ToString()), "Failed");
            }
            if (validationStatus == true)
            {
                
                Log.Error("InsertDefenceSubscribeUsForm", "Start");
                try
                {
                    APMULContactusFormDataContext rdb = new APMULContactusFormDataContext();
                    APMULContactFormRecord r = new APMULContactFormRecord()
                    {
                        Email = m.Email,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        FormSubmitOn = new DateTime?(m.FormSubmitOn)
                    };
                    rdb.APMULContactFormRecords.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                }
                catch (Exception exception1)
                {
                    Exception ex = exception1;
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }
                try
                {
                    string emailText = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailText", "");
                    string message = "";
                    message = string.Concat("Hello<br><br>", emailText, "<br><br>");
                    message = string.Concat(message, "<br>Email-Id: ", m.Email, "<br><br>Thanks");
                    string to = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailTo", "");
                    string from = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailFrom", "");
                    if (this.sendEmail(to, DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailSubject", ""), message, from))
                    {
                        Log.Error("Email Sent- ", "");
                    }
                }
                catch (Exception exception2)
                {
                    Exception ex = exception2;
                    result = new { status = "1" };
                    Log.Error("Failed to sent Email", ex.ToString());
                }
            }
            return base.Json(result, 0);
        }

        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool flag;
            bool status = false;
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(from)
                };
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                ContentType ct = new ContentType("application/pdf");
                mail.From = new MailAddress(from);
                MainUtil.SendMail(mail);
                status = true;
                flag = status;
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Console.WriteLine(ex.Message, "sendEmail - ");
                Log.Error(ex.Message, "sendEmail - ");
                Log.Error(ex.InnerException.ToString(), "sendEmail - ");
                flag = status;
            }
            return flag;
        }
    }
}