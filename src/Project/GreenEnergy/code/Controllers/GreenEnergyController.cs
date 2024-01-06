using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.GreenEnergy.Website.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sitecore.GreenEnergy.Website.Controllers
{
    public class GreenEnergyController : Controller
    {
        // GET: GreenEnergy
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
        public ActionResult InsertContactFormdetail(GreenEnergyContactModel m)
        {
            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating GreenEnergy ContactForm to stop auto script ", "Start");

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
                Log.Error("InsertGreenEnergyContactFormRecord ", "Start");
                var getEmailTo = "";

                try
                {
                    GreenEnergyContactFormRecordDataContext rdb = new GreenEnergyContactFormRecordDataContext();
                    GreenEnergyContactFormRecord r = new GreenEnergyContactFormRecord();

                    r.Name = m.Name;
                    r.Email = m.Email;
                    r.Mobile = m.Mobile;
                    r.MessageType = m.MessageType;
                    r.Message = m.Message;
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = m.FormSubmitOn;



                    #region Insert to DB
                    rdb.GreenEnergyContactFormRecords.InsertOnSubmit(r);
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
        public ActionResult InsertSubscribeUsFormdetail(GreenEnergyContactModel m)
        {

            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating GreenEnergy ContactForm to stop auto script ", "Start");

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
                GreenEnergyContactFormRecordDataContext rdb = new GreenEnergyContactFormRecordDataContext();
                GreenEnergyContactFormRecord r = new GreenEnergyContactFormRecord();

                r.Name = m.Name;
                r.Email = m.Email;
                r.Mobile = m.Mobile;
                r.Message = m.Message;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.GreenEnergyContactFormRecords.InsertOnSubmit(r);
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

        public ActionResult PowerGenerationGraph()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GreenEnergyAPIDataConnection"].ConnectionString;
            string text = "garvhaistagecontainer";
            string text2 = DictionaryPhraseRepository.Current.Get("/LivePowerGenerationGraph/ActivePowerFileName", "ActivePowerData.json");
            string text3 = DictionaryPhraseRepository.Current.Get("/LivePowerGenerationGraph/SolarIrradianceFileName", "SolarIrradianceData.json");
            string text4 = "";
            string text5 = "";
            CloudStorageAccount val = CloudStorageAccount.Parse(connectionString);
            CloudBlobContainer containerReference = val.CreateCloudBlobClient().GetContainerReference(text);
            CloudBlockBlob blockBlobReference = containerReference.GetBlockBlobReference(text2);
            CloudBlockBlob blockBlobReference2 = containerReference.GetBlockBlobReference(text3);
            List<GraphDataPoints> list = new List<GraphDataPoints>();
            List<GraphDataPoints> list2 = new List<GraphDataPoints>();
            double num = (double)DateTimeOffset.Parse(DateTime.Today.Date.ToString()).ToUnixTimeMilliseconds() + 19800000.0;
            double num2 = (double)DateTimeOffset.Parse(DateTime.Today.AddDays(1.0).AddTicks(-2L).ToString()).ToUnixTimeMilliseconds() + 19800000.0;
            try
            {
                if (blockBlobReference.Exists((BlobRequestOptions)null, (OperationContext)null))
                {
                    using (StreamReader streamReader = new StreamReader(blockBlobReference.OpenRead((AccessCondition)null, (BlobRequestOptions)null, (OperationContext)null)))
                        text4 = streamReader.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(text4))
                    {
                        dynamic val2 = JsonConvert.DeserializeObject<object>(text4);
                        if (val2 != null)
                        {
                            double y = 0.0;
                            for (int i = 0; !((i > val2.Count - 1) ? true : false); i++)
                            {
                                if ((val2[i]["dbValue"] != null) && (double)val2[i]["dbValue"] > 0.0)
                                {
                                    y = Math.Round((double)val2[i]["dbValue"], 2, MidpointRounding.AwayFromZero);
                                }
                                double x = (double)val2[i]["start"];
                                list.Add(new GraphDataPoints(x, y));
                            }
                        }
                    }
                }
                else
                {
                    text4 = "";
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                Log.Error("Solar Live Power Generation Graph:Active Power Error" + ex2.Message, (object)"Error");
                throw;
            }
            try
            {
                if (blockBlobReference2.Exists((BlobRequestOptions)null, (OperationContext)null))
                {
                    using (StreamReader streamReader2 = new StreamReader(blockBlobReference2.OpenRead((AccessCondition)null, (BlobRequestOptions)null, (OperationContext)null)))
                        text5 = streamReader2.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(text5))
                    {
                        double y2 = 0.0;
                        dynamic val3 = JsonConvert.DeserializeObject<object>(text5);
                        if (val3 != null)
                        {
                            for (int j = 0; !((j > val3.Count - 1) ? true : false); j++)
                            {
                                if ((val3[j]["dbValue"] != null) && (double)val3[j]["dbValue"] > 0.0)
                                {
                                    y2 = Math.Round((double)val3[j]["dbValue"], 2, MidpointRounding.AwayFromZero);
                                }
                                double x2 = (double)val3[j]["start"];
                                list2.Add(new GraphDataPoints(x2, y2));
                            }
                        }
                    }
                }
                else
                {
                    text5 = "";
                }
            }
            catch (Exception ex3)
            {
                Log.Error("Solar Live Power Generation Graph:Active Power Error" + ex3.Message, (object)"Error");
                throw;
            }
            ViewBag.DataPoints1 = JsonConvert.SerializeObject((object)list);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject((object)list2);
            ViewBag.minTime = num;
            ViewBag.minTime = num2;
            return View("/Views/GreenEnergy/Sublayouts/GreenEnergyPowerGenerationGraph.cshtml");
        }
    }
}