using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.AdaniFoundation.Website;
using Sitecore.AdaniFoundation.Website.Models;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Data.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniFoundation.Website.Controllers
{
    public class AdaniFoundationController : Controller
    {
        public AdaniFoundationController()
        {
        }

        public ActionResult AdaniFoundationCareerForm()
        {
            return base.View("/Views/Foundation/Sublayouts/AdaniFoundationCareerFormpage.cshtml");
        }

        [HttpPost]
        public ActionResult AdaniFoundationCareerForm(AdaniFoundationCareer m)
        {
            bool flag = true;
            
            string errorMessage = "Invalid file type";
            var variable = new { status = "1" };
            Log.Info("Validating Foundation Career Form to stop auto script ", "Start");
            try
            {
                flag = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                variable = new { status = "2" };
                Log.Error(string.Concat("Failed to validate auto script : ", exception.ToString()), "Failed");
            }
            if (!flag)
            {
                variable = new { status = "2" };
            }
            else
            {
                Log.Info("InsertAdaniFoundationCarrerDetails", "Start");
                string value = "";
                try
                {
                    AdaniFoundationCareersFormDataContext adaniFoundationCareersFormDataContext = new AdaniFoundationCareersFormDataContext();
                    AdaniFoundationCareerForm adaniFoundationCareerForm = new AdaniFoundationCareerForm()
                    {
                        Name = m.Name,
                        Email = m.Email,
                        CurrentCTC = m.CurrentCTC,
                        CurrentOrganization = m.Current_Organization,
                        ContactNo = m.ContactNo,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        CurrentDesignation = m.CurrentDesignation,
                        Education = m.Education,
                        Experience = m.Experience,
                        SubmitOn = new DateTime?(m.SubmitOn)
                    };
                    if (m.ResumeAttachment != null)
                    {
                        
                        HttpPostedFileBase resumeAttachment = m.ResumeAttachment;
                        string fileName = resumeAttachment.FileName;
                            resumeAttachment.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/AdaniFoundationCareersResume/Resume/", m.Name, "-", m.ContactNo, "-", fileName })));
                            adaniFoundationCareerForm.ResumeAttachment = string.Concat(new string[] { "/AdaniFoundationCareersResume/Resume/", m.Name, "-", m.ContactNo, "-", fileName });
                        
                      
                    }
                    adaniFoundationCareersFormDataContext.AdaniFoundationCareerForms.InsertOnSubmit(adaniFoundationCareerForm);
                    adaniFoundationCareersFormDataContext.SubmitChanges();
                }
                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    variable = new { status = "0" };
                    Console.WriteLine(exception2);
                }
                try
                {
                    string str = "";
                    string value1 = "";
                    Item item = Sitecore.Context.Database.GetItem("{7ACE92B2-83DD-4364-BEE8-4B0C893331A9}");
                    if (item != null)
                    {
                        HttpPostedFileBase resumeAttachment = m.ResumeAttachment;
                        string fileName = resumeAttachment.FileName;
                        string str5 = string.Concat(new string[] { "<a href=\"https://adaniproduction-cd.azurewebsites.net/AdaniFoundationCareersResume/Resume/", m.Name, "-", m.ContactNo, "-", fileName,"\"", " Download>Click to Download </a>"});
                        value = item.Fields["EmailTo"].Value;
                        value1 = item.Fields["EmailSubject"].Value;
                        string str1 = item.Fields["Body"].Value;
                        str = string.Concat("Hello<br><br>", str1, "<br><br>");
                        str = string.Concat(new string[] { str, "<br>Name: ", m.Name, "<br>Email-Id: ", m.Email, "<br>Current CTC: ", m.CurrentCTC, "<br>Contact Number: ", m.ContactNo, "<br>Current Organization: ", m.Current_Organization, "<br>Current Designation: ", m.CurrentDesignation, "<br>Education: ", m.Education, "<br>  <br> Experience: ", m.Experience, "<br>  <br> Resume: ",  str5, "<br><br> Thanks" });
                        string str2 = DictionaryPhraseRepository.Current.Get("/CareerForm/EmailFrom", "");
                        this.sendEmail(value, value1, str, str2);
                    }

                }
                catch (Exception exception5)
                {
                    Exception exception4 = exception5;
                    variable = new { status = "1" };
                    Log.Error("Failed to get subject specific Email", exception4.ToString());
                }
                try
                {
                    string value2 = "";
                    string value3 = "";
                    Item item1 = Context.Database.GetItem("{E93DE827-E47E-4774-AD8D-996F7C985D14}");
                    if (item1 != null)
                    {
                        value = m.Email;
                        value3 = item1.Fields["EmailSubject"].Value;
                        value2 = item1.Fields["Body"].Value;
                        string str3 = DictionaryPhraseRepository.Current.Get("/CareerForm/EmailFrom", "");
                        this.sendEmail(value, value3, value2, str3);
                    }
                }
                catch (Exception exception7)
                {
                    Exception exception6 = exception7;
                    variable = new { status = "1" };
                    Log.Error("Failed to get subject specific Email", exception6.ToString());
                }
            }
            return this.Redirect("/Thankyou");
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
    }
}