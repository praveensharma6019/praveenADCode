using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using Sitecore.Farmpik.Website.Attribute;
using Sitecore.Farmpik.Website.Models;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Mvc.Common;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Sitecore.Farmpik.Website.Controllers
{
    public class FarmpikController : Controller
    {

        // GET: Farmpik
        public ActionResult Index()
        {
            return View();
        }

        public bool IsReCaptchValid(string reResponse)
        {
            bool flag = false;
            try
            {
                Log.Info("Recaptcha Validation Process Start. Input Response: ", "reResponse");
                string str = reResponse;
                string str1 = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "6Lcql9QZAAAAAOSIsZ-9gNRiZaVPrDmSrbKSe3y2");
                string str2 = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", str1, str);
                Log.Info("Recaptcha secret key: " + str1, "str1");
                using (WebResponse response = ((HttpWebRequest)WebRequest.Create(str2)).GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        flag = (JObject.Parse(streamReader.ReadToEnd()).Value<bool>("success") ? true : false);
                        Log.Info("Is Recpatcha successfull: " + flag, "flag");
                    }
                }
                Log.Info("Return value: " + flag, "flag");
            }
            catch(Exception ex)
            {
                Log.Info("Exception occured while recaptcha v2 validation.Exception message is:"+ ex.Message, ex.Message);
            }
            return flag;
        }

        public bool IsReCaptchValidV3(string reResponse)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6LdmYcUgAAAAAKSWZwgyTdY4sFL8gCydKAK2uccX&response={reResponse}").Result;

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }
                string JSONres = res.Content.ReadAsStringAsync().Result;
                dynamic JSONdata = JObject.Parse(JSONres);

                if (JSONdata.success != "true" || JSONdata.score <= 0.5)
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                Log.Info("Exception occured while recaptcha v3 validation.Exception message is:" + ex.Message, ex.Message);
            }
            return true;
        }
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult InsertContact(FarmpikContactModel m)
        {
            bool validationStatus = false;
            var result = new { status = "1" };
            String[] message = { "Business Inquiry", "General Inquiry", "Media Inquiry", "Website Feedback" };
            if (!message.Contains(m.MessageType))
            {
                result = new { status = "2" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            Log.Error("Validating Farmpik ContactForm to stop auto script ", "Start");
            if (ModelState.IsValid)
            {
                try
                {
                    validationStatus = this.IsReCaptchValidV3(m.reResponse);
                }
                catch (Exception ex)
                {
                    Exception exception = ex;
                    Log.Info(string.Concat("ContactUsForm Failed to validate auto script : ", ex.ToString()), this);
                    result = new { status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    if (validationStatus)
                    {
                        Log.Error("InsertFarmpikContact", "Start");
                          
                        FarmpikContactUsFormDataContext rdb1 = new FarmpikContactUsFormDataContext();
                            FarmpikContactFormRecord r1 = new FarmpikContactFormRecord();

                            r1.Name = m.Name;
                            r1.Email = m.Email;
                            r1.Mobile = m.Mobile;
                            r1.MessageType = m.MessageType;
                            r1.Message = m.Message;
                            r1.FormType = m.FormType;
                            r1.PageInfo = m.PageInfo;
                            r1.FormSubmitOn = m.FormSubmitOn;
                            #region Insert to DB
                            rdb1.FarmpikContactFormRecords.InsertOnSubmit(r1);
                            rdb1.SubmitChanges();                    

                    }
                    else
                    {
                        result = new { status = "3" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                        Log.Info("IsReCaptchValid Failed." + result, "");

                    }

                }
                catch (Exception ex)
                {
                    result = new { status = "fail" };
                    Console.Write(ex);
                }
            }
            else
            {
                result = new { status = "4" };
                return Json(result, JsonRequestBehavior.AllowGet);
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
                mail.From = new MailAddress(from);
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