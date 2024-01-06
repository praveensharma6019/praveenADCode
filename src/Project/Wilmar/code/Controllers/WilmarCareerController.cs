using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Wilmar.Website;
using Sitecore.Wilmar.Website.Models;
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

namespace Sitecore.Wilmar.Website.Controllers
{
    public class WilmarCareerController : Controller
    {
        public WilmarCareerController()
        {
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

        public ActionResult WilmarCareerForm()
        {
            return base.View();
        }

        [HttpPost]
        public ActionResult WilmarCareerForm(WilmarContactModel m)
        {
            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating Career Form to stop auto script ", "Start");
            try
            {
                validationStatus = this.IsReCaptchValid(m.reResponse);
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

                    WilmarContactUsDataContext rdb = new WilmarContactUsDataContext();
                    WilmarContactFormRecord r = new WilmarContactFormRecord();

                    bool isValid = true;
                    string errorMessage = "Invalid field values:";

                    if (string.IsNullOrEmpty(m.Name) || !Regex.IsMatch(m.Name, "^[a-zA-Z][a-zA-Z ]*$"))
                    {
                        errorMessage = errorMessage + "Name";
                        isValid = false;
                    }
                    if (string.IsNullOrEmpty(m.Email) || !Regex.IsMatch(m.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    {
                        errorMessage = errorMessage + "Email";
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
                        errorMessage = errorMessage + "MessageType";
                        isValid = false;
                    }
                    //validate message
                    if (string.IsNullOrEmpty(m.Message))
                    {
                        errorMessage = errorMessage + "Message";
                        isValid = false;
                    }

                    if (!isValid)
                    {
                        ViewBag.Message = errorMessage;
                        return View();
                    }




                    r.Name = m.Name;
                    r.Email = m.Email;
                    r.Mobile = m.Mobile;
                    r.MessageType = m.MessageType;
                    r.Message = m.Message;
                    
                    if (m.FileAttachment != null)
                    {
                        HttpPostedFileBase fileAttachment = m.FileAttachment;
                        string fileName = fileAttachment.FileName;
                        string str = base.Server.MapPath(string.Concat("/WilmarFiles/Uploads/-", fileName));
                        fileAttachment.SaveAs(str);
                        r.FilesUpload = string.Concat("/WilmarFiles/Uploads/-", fileName);
                    }
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    DateTime now = DateTime.Now;
                    r.FormSubmitOn = new DateTime?(DateTime.Parse(now.ToString()));
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
            return this.Redirect("/thankyou");
        }
    }
}