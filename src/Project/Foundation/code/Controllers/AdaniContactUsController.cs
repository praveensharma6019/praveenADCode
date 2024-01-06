using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.Website.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Mvc;

namespace Sitecore.Foundation.Website.Controllers
{
    public class AdaniContactUsController : Controller
    {
        public bool IsReCaptchValid(string reResponse)
        {
            bool flag = false;
            string str = reResponse;
            using (WebResponse response = WebRequest.Create(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", (object)DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", ""), (object)str)).GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    flag = ((JToken)JObject.Parse(streamReader.ReadToEnd())).Value<bool>((object)"success");
            }
            return flag;
        }

        [HttpPost]
        public ActionResult InsertContactdetail(AdaniFoundationContactModel m)
        {
            bool flag = false;
            var data = new { status = "1" };
            Log.Error("Insertcontactdetail of AdaniFoundation ", (object)"Start");
            string to = "";
            string str = "";
            string subject = "";
            try
            {
                flag = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception ex)
            {
                data = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), (object)"Failed");
            }
            if (flag)
            {
                Log.Error("InsertBunkeringContactFormRecord ", (object)"Start");
                try
                {
                    AdaniFoundationContactUsDataContext contactUsDataContext = new AdaniFoundationContactUsDataContext();
                    contactUsDataContext.AdaniFoundationContactUs.InsertOnSubmit(new AdaniFoundationContactUs()
                    {
                        Name = m.Name,
                        Email = m.Email,
                        Mobile = m.Mobile,
                        SubjectType = m.SubjectType,
                        Message = m.Message,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        FormSubmitOn = new DateTime?(m.FormSubmitOn)
                    });
                    contactUsDataContext.SubmitChanges();
                }
                catch (Exception ex)
                {
                    data = new { status = "0" };
                    Console.WriteLine((object)ex);
                }
                try
                {
                    foreach (Item obj in ((IEnumerable<Item>)Context.Database.GetItem("{0C79D5BF-503F-4BA8-BC97-EA78E0205EB2}").Children).Where<Item>((Func<Item, bool>)(x => ((BaseItem)x).Fields["SubjectName"].Value.ToLower() == m.SubjectType.ToLower())).ToList<Item>())
                    {
                        to = ((BaseItem)obj).Fields["EmailTo"].Value;
                        str = ((BaseItem)obj).Fields["Body"].Value;
                        subject = ((BaseItem)obj).Fields["EmailSubject"].Value;
                    }
                    string body = "Hello " + m.Name + ",<br><br>" + str;
                    string from = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailFrom", "");
                    if (this.sendEmail(m.Email, subject, body, from))
                        Log.Error("Email Sent to specific Message Subject type- ", (object)"");
                }
                catch (Exception ex)
                {
                    data = new { status = "0" };
                    Log.Error("Failed to get email from subject", (object)ex.ToString());
                }
                try
                {
                    string from = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailFrom", "");
                    string body = "Hello<br><br>" + DictionaryPhraseRepository.Current.Get("/ContactUs/EmailText", "") + "<br><br>" + "<br>Name: " + m.Name + "<br>Email-Id: " + m.Email + "<br>Subject of Message: " + m.SubjectType + "<br>Contact Number: " + m.Mobile + "<br>Message: " + m.Message + "<br><br>Thanks";
                    if (this.sendEmail(to, subject, body, from))
                        Log.Error("Email Sent- ", (object)"");
                }
                catch (Exception ex)
                {
                    data = new { status = "1" };
                    Log.Error("Failed to sent Email", (object)ex.ToString());
                }
            }
            else
                data = new { status = "2" };
            return (ActionResult)this.Json((object)data, (JsonRequestBehavior)0);
        }

        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool flag = false;
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(from);
                mailMessage.To.Add(to);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                ContentType contentType = new ContentType("application/pdf");
                mailMessage.From = new MailAddress(from);
                MainUtil.SendMail(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, (object)"sendEmail - ");
                Log.Error(ex.Message, (object)"sendEmail - ");
                Log.Error(ex.InnerException.ToString(), (object)"sendEmail - ");
                return flag;
            }
        }

        [HttpPost]
        public ActionResult InsertSubscribeUsFormdetail(AdaniFoundationContactModel m)
        {
            Log.Error(nameof(InsertSubscribeUsFormdetail), (object)"Start");
            bool flag = true;
            var data = new { status = "1" };
            if (flag)
            {
                Log.Error("InsertDefenceSubscribeUsForm", (object)"Start");
                try
                {
                    AdaniFoundationContactUsDataContext contactUsDataContext = new AdaniFoundationContactUsDataContext();
                    contactUsDataContext.AdaniFoundationContactUs.InsertOnSubmit(new AdaniFoundationContactUs()
                    {
                        Email = m.Email,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        FormSubmitOn = new DateTime?(m.FormSubmitOn)
                    });
                    contactUsDataContext.SubmitChanges();
                }
                catch (Exception ex)
                {
                    data = new { status = "0" };
                    Console.WriteLine((object)ex);
                }
                try
                {
                    string body = "Hello<br><br>" + DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailText", "") + "<br><br>" + "<br>Email-Id: " + m.Email + "<br><br>Thanks";
                    string to = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailTo", "");
                    string from = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailFrom", "");
                    string subject = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailSubject", "");
                    if (this.sendEmail(to, subject, body, from))
                        Log.Error("Email Sent- ", (object)"");
                }
                catch (Exception ex)
                {
                    data = new { status = "1" };
                    Log.Error("Failed to sent Email", (object)ex.ToString());
                }
            }
            else
                data = new { status = "2" };
            return (ActionResult)this.Json((object)data, (JsonRequestBehavior)0);
        }
    }
}
