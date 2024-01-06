using Newtonsoft.Json.Linq;
using Sitecore.APTRI.website;
using Sitecore.APTRI.website.Models;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.APTRI.Website.Controllers
{
    public class APTRIController : Controller
    {
        // GET: APTRI
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
        public ActionResult InsertContact(APTRIContactModal m)
        {
            bool validationStatus = true;
            var result = new { status = "1" };
            Log.Error("Validating APTRI ContactForm to stop auto script ", "Start");

            try
            {
                validationStatus = IsReCaptchValid(m.reResponse);
                //    //if (Request.Cookies["SIDCC"]!=null)
                //    //{
                //    //    if (Session["validate"] == null)
                //    //    {
                //    //        validationStatus = true;
                //    //    }
                //    //    else
                //    //    {
                //    //        if (Session["validate"].ToString() != Request.Cookies["SIDCC"].Value)
                //    //        {
                //    //            validationStatus = true;
                //    //        }
                //    //    }
                //    //}
            }
            catch (Exception ex)
            {
                result = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }

            if (validationStatus == true)
            {
                Log.Error("InsertAPTRI", "Start");
                var getEmailTo = "";

                try
                {
                    APTRIContactFormDataContext rdb1 = new APTRIContactFormDataContext();
                    APTRIContactFormRecord r1 = new APTRIContactFormRecord();
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
                    r1.Name = m.Name;
                    r1.Email = m.Email;
                    r1.MessageType = m.MessageType;
                    r1.Message = m.Message;
                    r1.Mobile = m.Mobile;
                    r1.FormType = m.FormType;
                    r1.PageInfo = m.PageInfo;
                    r1.FormSubmitOn = m.FormSubmitOn;



                    #region Insert to DB
                    rdb1.APTRIContactFormRecords.InsertOnSubmit(r1);
                    rdb1.SubmitChanges();
                    //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }

                try
                {
                    var msgTpye = Sitecore.Context.Database.GetItem("{C99E4F2C-7EFA-428D-86B7-E0B4FED94B16}");
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
                    string emailText = DictionaryPhraseRepository.Current.Get("/ContactForm/EmailText", "");
                    string message = "";
                    message = "Hello<br><br>" + emailText + "<br><br>";
                    message = message + "<br>Name: " + m.Name + "<br>Email-Id: " + m.Email + "<br>Subject of Message: " + m.MessageType + "<br>Message: " + m.Message + "<br><br>Thanks";
                    //string to = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailTo", "");
                    string from = DictionaryPhraseRepository.Current.Get("/ContactForm/EmailFrom", "");
                    string emailSubject = DictionaryPhraseRepository.Current.Get("/ContactForm/EmailSubject", "");
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