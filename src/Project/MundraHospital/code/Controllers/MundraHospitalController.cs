using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using Sitecore.Foundation.SitecoreExtensions.Attributes;
using System.Data;
using Sitecore.MundraHospital.Website.Models;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Sitecore.MundraHospital.Website.Controllers
{
    public class MundraHospitalController : Controller
    {
        private bool IsReCaptchValid(string reResponse)
        {
            var result = false;
            // var captchaResponse = Request.Form["g-recaptcha-response"];
            var captchaResponse = reResponse;
            string secretKey = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "");
            //var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            //var secretKey = "6LdCibIUAAAAAL1v8AeYxIT4b0ze7BT8nrb45AZy";
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
        public ActionResult InsertContactdetail(MundraHContactModel mh)
        {
            bool validationStatus = false;
            Log.Error("Insertcontactdetail of MundraHospital ", "Start");
            var getEmailTo = "";
            string getMessage = "";
            string getEmailSubject = "";
            var result = new { status = "1" };
            try
            {
                validationStatus = IsReCaptchValid(mh.reResponse);
            }
            catch (Exception ex)

            {

                result = new { status = "2" };

                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");

            }

            if (validationStatus == true)
            {
                Log.Error("InsertMundrahospitalContactFormRecord ", "Start");
                try
                {
                    MundraHContactUsDataDataContext rdb = new MundraHContactUsDataDataContext();
                    MundraHospitalContactUs r = new MundraHospitalContactUs();

                    r.Name = mh.Name;
                    r.Email = mh.Email;
                    r.Mobile = mh.Mobile;
                    r.Message = mh.Message;
                    r.SubjectType = mh.SubjectType;
                    r.FormType = mh.FormType;
                    r.PageInfo = mh.PageInfo;
                    r.FormSubmitOn = mh.FormSubmitOn;



                    #region Insert to DB
                    rdb.MundraHospitalContactUs.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }
                try
                {
                    var msgType = Sitecore.Context.Database.GetItem("{51CAA0BB-29D4-4FC3-BED7-5DA6D994E339}");
                    var getfilteredItem = msgType.Children.Where(x => x.Fields["SubjectName"].Value.ToLower() == mh.SubjectType.ToLower());

                    foreach (var itemData in getfilteredItem.ToList())
                    {
                        getEmailTo = itemData.Fields["Email"].Value;
                        getMessage = itemData.Fields["Body"].Value;
                        getEmailSubject = itemData.Fields["EmailSubject"].Value;



                    }
                    getMessage = "Hello " + mh.Name + ",<br><br>" + getMessage;
                    string from = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailFrom", "");
                    bool results = sendEmail(mh.Email, getEmailSubject, getMessage, from);

                    if (results)
                    {
                        Log.Error("Email Sent to specific Message Subject type- ", "");
                    }

                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Log.Error("Failed to get email from subject", ex.ToString());
                }


                try
                {
                    // string to = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailTo", "");
                    string from = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailFrom", "");
                    //  string emailSubject = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailSubject", "");
                    string emailText = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailText", "");
                    string message = "";
                    message = "Hello<br><br>" + emailText + "<br><br>Name: " + mh.Name;
                    message = message + "<br>Email: " + mh.Email + "<br>Subject of Message: " + mh.SubjectType + "<br>Message: " + mh.Message + "<br><br>Thanks";

                    bool results = sendEmail(getEmailTo, getEmailSubject, message, from);

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

        // GET: MundraHospital
    }
}