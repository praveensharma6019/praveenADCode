using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Wilmar.Website.Models;
using Sitecore.Wilmar.Website.Providers;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace Sitecore.Wilmar.Website.Controllers
{
    public class WilmarController : Controller
    {
        Recaptchav2Provider validateCaptchav2 = new Recaptchav2Provider();
        // GET: Wilmar
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
        public ActionResult InsertContact(WilmarContactModel m)
        {
            bool validationStatus = true;
            var result = new { status = "1" };
            Log.Error("Validating Wilmar ContactForm to stop auto script ", "Start");

            try
            {
                validationStatus = validateCaptchav2.IsReCaptchValid(m.reResponse);
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
                Log.Error("InsertWilmarContact", "Start");
                var getEmailTo = "";

                try
                {
                    WilmarContactUsDataContext rdb1 = new WilmarContactUsDataContext();
                    WilmarContactFormRecord r1 = new WilmarContactFormRecord();

                    r1.Name = m.Name;
                    r1.Email = m.Email;
                    r1.Mobile = m.Mobile;
                    r1.MessageType = m.MessageType;
                    r1.Message = m.Message;
                    r1.FormType = m.FormType;
                    r1.PageInfo = m.PageInfo;
                    r1.FormSubmitOn = m.FormSubmitOn;



                    #region Insert to DB
                    rdb1.WilmarContactFormRecords.InsertOnSubmit(r1);
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
                    var msgTpye = Sitecore.Context.Database.GetItem("{A5DE1AF3-2CB8-4D96-B7AC-557C04004CCB}");
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
        public ActionResult InsertContactFormdetail(WilmarExportForms m)
        {
            bool validationStatus = true;
            var result = new { status = "1" };
            Log.Error("Validating Wilmar ContactForm to stop auto script ", "Start");

            try
            {
                validationStatus = validateCaptchav2.IsReCaptchValid(m.reResponse);
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
                Log.Error("InsertWilmarExportFormRecord", "Start");
                var getEmailTo = "";

                try
                {
                    WilmarExportFormRecordDataContext rdb = new WilmarExportFormRecordDataContext();
                    WilmarExportform r = new WilmarExportform();

                    r.Salutation = m.Salutation;
                    r.FirstName = m.FirstName;
                    r.LastName = m.Lastname;
                    r.AddressLine1 = m.Address1;
                    r.AddressLine2 = m.Address2;
                    r.State = m.State;
                    r.City = m.City;
                    r.Pincode = m.Pincode;
                    r.Country = m.Country;
                    r.Mobile = m.Mobile;
                    r.LandlineNumber = m.LandlineNumber;
                    r.BusinessCategory = m.BusinessCategory;
                    r.Remarks = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageType = m.PageType;
                    r.FormSubmitOn = m.FormSubmit;


                    #region Insert to DB
                    rdb.WilmarExportforms.InsertOnSubmit(r);
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
                    message = message + "<br>Name: " + m.FirstName + "<br>Email-Id: " + m.Email + "<br>Subject of Message: " + m.MessageType + "<br>Contact Number: " + m.Mobile + "<br>Message: " + m.Message + "<br><br>Thanks";
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
     public ActionResult InsertSubscribeUsFormdetail(WilmarContactModel m)
        {


            Log.Error("InsertSubscribeUsFormdetail", "Start");
            bool validationStatus = true;
            var result = new { status = "1" };
            try
            {
                validationStatus = validateCaptchav2.IsReCaptchValid(m.reResponse);
            }
            catch (Exception ex)
            {
                result = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }
            if (validationStatus == true)
            {
                Log.Error("InsertWilmarSubscribeUsForm", "Start");
                var getEmailTo = "";

                try
                {
                    
                    WilmarContactUsDataContext rdb = new WilmarContactUsDataContext();
                    WilmarContactFormRecord r = new WilmarContactFormRecord();
                    r.Email = m.Email;
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = m.FormSubmitOn;


                    #region Insert to DB
                    rdb.WilmarContactFormRecords.InsertOnSubmit(r);
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