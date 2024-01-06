
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.AdaniVidyamandir.Website;

using Sitecore.AdaniVidyamandir.Website.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniVidyamandir.Website.Controllers
{
    public class AdaniVidyamandir : Controller
    {
        public AdaniVidyamandir()
        {
        }

        [HttpGet]
        public ActionResult AdaniVidyamandirContactForm()
        {
            return base.View("/Views/AdaniVidyamandir/Sublayouts/AdaniVidyamandirContactForm.cshtml", new VidyaMandirContactModel());
        }
        [HttpPost]
        public ActionResult AdaniVidyamandirContactForm(VidyaMandirContactModel m, string SubmitApplication)
        {
            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert Form", "Start");
            bool flag = true;

            try
            {
                flag = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Info(string.Concat("Failed to validate auto script : ", exception.ToString()), "Failed");
            }
            if (flag == true)

            {

                Log.Info("Insert ContactForm Captcha Validated", "Start");


                var getEmailTo = "";
                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        if (ModelState.IsValid)
                        {
                            AdaniVidyaMandirDataDataContext vidyamandircontactDataContext = new AdaniVidyaMandirDataDataContext();

                            m.Id = Guid.NewGuid();

                            VidyaMandirContactUs VidyaMandirContactForm = new VidyaMandirContactUs()

                            {

                                Id = m.Id,
                                Name = m.Name,
                                Email = m.EmailID,
                                Message = m.Message,
                                MobileNo = m.MobileNo,
                                MessageType = m.MessageType,
                                PageInfo = m.PageInfo,
                                FormType = m.FormName,
                                SubmittedBy = m.SubmittedBy,
                                FormSubmitOn = new DateTime?(DateTime.Now),
                                CreatedOn = new DateTime?(DateTime.Now)

                            };

                            vidyamandircontactDataContext.VidyaMandirContactUs.InsertOnSubmit(VidyaMandirContactForm);
                            vidyamandircontactDataContext.SubmitChanges();
                            Log.Info("Form Contact Form data saved into db successfully: ", this);
                            try
                            {
                                var msgTpye = Sitecore.Context.Database.GetItem("{5EED1B9F-CE29-4007-8C6C-00372E784295}");
                                var getfilteredItem = msgTpye.Children.Where(x => x.Fields["SubjectName"].Value.ToLower() == m.MessageType.ToLower());

                                foreach (var itemData in getfilteredItem.ToList())
                                {
                                    getEmailTo = itemData.Fields["EmailTo"].Value;
                                }
                            }

                            catch (Exception exception5)
                            {
                                Exception exception4 = exception5;
                                result = new { status = "0" };
                                Log.Error("Failed to get email from subject", exception4.ToString());
                            }

                            try
                            {
                                string emailText = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailText", "");
                                string message = "";
                                message = "Hello<br><br>" + emailText + "<br><br>";
                                message = message + "<br>Name: " + m.Name + "<br>Email-Id: " + m.EmailID + "<br>Subject of Message: " + m.MessageType + "<br>Contact Number: " + m.MobileNo + "<br>Message: " + m.Message + "<br><br>Thanks";
                                //string to = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailTo", "");
                                string from = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailFrom", "");
                                string emailSubject = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailSubject", "");
                                bool results = sendEmail(getEmailTo, emailSubject, message, from);

                                if (results)
                                {
                                    Log.Error("Email Sent- ", "");
                                }
                            }
                            catch (Exception exception7)
                            {
                                Exception exception6 = exception7;
                                result = new { status = "1" };
                                Log.Error("Failed to sent Email", exception6.ToString());
                            }
                            return this.Redirect("/thankyou");
                        }
                        else
                        {
                            actionResult = base.View("/Views/AdaniVidyamandir/Sublayouts/AdaniVidyamandirContactForm.cshtml", m);
                            return actionResult;
                        }
                    }

                }

                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Contact Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/AdaniVidyamandir/Sublayouts/AdaniVidyamandirContactForm.cshtml", m);
                }

            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/GyaanGalaxy/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/AdaniVidyamandir/Sublayouts/AdaniVidyamandirContactForm.cshtml", m);
            }



          ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
            actionResult = base.View("/Views/AdaniVidyamandir/Sublayouts/AdaniVidyamandirContactForm.cshtml", m);
            return actionResult;
        }


        public bool IsReCaptchValid(string reResponse)
        {
            bool flag = false;
            string str = reResponse;
            string str1 = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "");
            string str2 = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", str1, str);
            using (WebResponse response = ((HttpWebRequest)WebRequest.Create(str2)).GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    flag = (JObject.Parse(streamReader.ReadToEnd()).Value<bool>("success") ? true : false);
                }
            }
            return flag;
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