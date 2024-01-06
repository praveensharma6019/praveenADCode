using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Bunkering.Website.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Bunkering.Website.Controllers
{
    public class BunkeringController : Controller
    {
        // GET: Bunkering
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
        public ActionResult InsertContactdetail(BunkeringContactModel m)
        {
            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating Bunkering ContactForm to stop auto script ", "Start");

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
                Log.Error("InsertBunkeringContactFormRecord ", "Start");
                var getEmailTo = "";

                try
                {
                    BunkeringContactFormRecordDataContext rdb = new BunkeringContactFormRecordDataContext();
                    BunkeringContactFormRecord r = new BunkeringContactFormRecord();

                    r.Name = m.Name;
                    r.Email = m.Email;
                    r.Mobile = m.Mobile;
                    r.MessageType = m.MessageType;
                    r.Message = m.Message;
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = m.FormSubmitOn;



                    #region Insert to DB
                    rdb.BunkeringContactFormRecords.InsertOnSubmit(r);
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
                    var msgTpye = Sitecore.Context.Database.GetItem("{EBE6810C-8BA6-4789-BBE3-74C6DDDD3FED}");
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
                    string emailText = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailText", "");
                    string message = "";
                    message = "Hello<br><br>" + emailText + "<br><br>";
                    message = message + "<br>Name: " + m.Name + "<br>Email-Id: " + m.Email + "<br>Subject of Message: " + m.MessageType + "<br>Contact Number: " + m.Mobile + "<br>Message: " + m.Message + "<br><br>Thanks";
                    //string to = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailTo", "");
                    string from = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailFrom", "");
                    string emailSubject = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailSubject", "");
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
        public ActionResult InsertRequestQuotedetail(BunkeringRequestQuoteModel m)
        {
            bool validationStatus = false;

            Log.Error("Bunkering - InsertRequestQuotedetail ", "Start");

            var result = new { status = "1" };
            try
            {
                validationStatus = IsReCaptchValid(m.reResponse);
                
            }
            catch (Exception ex)
            {
                result = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }
            if(validationStatus == true)
            {
                try
                {
                    BunkeringContactFormRecordDataContext rdb = new BunkeringContactFormRecordDataContext();
                    BunkeringRequestQuoteRecord r = new BunkeringRequestQuoteRecord();

                    r.CompanyName = m.CompanyName;
                    r.ContactPerson = m.ContactPerson;
                    r.Mobile = m.Mobile;
                    r.Email = m.Email;
                    r.ProductType = m.ProductType;
                    r.ProductSpec = m.ProductSpec;
                    r.ProductQuantity = m.ProductQuantity;
                    r.VesselName = m.VesselName;
                    r.Port = m.Port;
                    r.Berth = m.Berth;
                    r.Estimated_DOA = m.Estimated_DOA;
                    r.Estimated_TOA = m.Estimated_TOA;
                    r.Estimated_DOD = m.Estimated_DOD;
                    r.Estimated_TOD = m.Estimated_TOD;
                    r.Agent = m.Agent;
                    r.OtherDetails = m.OtherDetails;
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = m.FormSubmitOn;



                    #region Insert to DB
                    rdb.BunkeringRequestQuoteRecords.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }
                try
                {

                    string to = DictionaryPhraseRepository.Current.Get("/RequestQuote/EmailTo", "");
                    string from = DictionaryPhraseRepository.Current.Get("/RequestQuote/EmailFrom", "");
                    string emailSubject = DictionaryPhraseRepository.Current.Get("/RequestQuote/EmailSubject", "");
                    string emailText = DictionaryPhraseRepository.Current.Get("/RequestQuote/EmailText", "");
                    string message = "";
                    message = "Hello<br><br>" + emailText + "<br><br>Company Name: " + m.CompanyName;
                    message = message + "<br>Contact Person: " + m.ContactPerson + "<br>Mobile Number: " + m.Mobile + "<br>Email Id: " + m.Email + "<br>Product Type: " + m.ProductType + "<br>Product Specific: " + m.ProductSpec + "<br>Product Quantity: " + m.ProductQuantity + "<br>Vessel Name: " + m.VesselName + "<br>Port: " + m.Port + "<br>Berth/Location: " + m.Berth + "<br>Estimated Date of Arrival: " + m.Estimated_DOA + "<br>Estimated Time of Arrival: " + m.Estimated_TOA + "<br>Estimated Date of Departure: " + m.Estimated_DOD + "<br>Estimated Time of Departure: " + m.Estimated_TOD + "<br>Agent: " + m.Agent + "<br>Other Details: " + m.OtherDetails + "<br><br>Thanks";

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
                result = new { status = "3" };
            }
            
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertSubscribeUsFormdetail(BunkeringContactModel m)
        {


            Log.Error("InsertSubscribeUsFormdetail", "Start");
            var result = new { status = "1" };
            try
            {
                BunkeringContactFormRecordDataContext rdb = new BunkeringContactFormRecordDataContext();
                BunkeringContactFormRecord r = new BunkeringContactFormRecord();

                r.Name = m.Name;
                r.Email = m.Email;
                r.Mobile = m.Mobile;
                r.Message = m.Message;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.BunkeringContactFormRecords.InsertOnSubmit(r);
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