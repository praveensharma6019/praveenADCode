using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.GyaanGalaxy.Website;
using Sitecore.GyaanGalaxy.Website.Helper;
using Sitecore.GyaanGalaxy.Website.Models;
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

namespace Sitecore.GyaanGalaxy.Website.Controllers
{
    public class GyaanGalaxyController : Controller
    {
        public GyaanGalaxyController()
        {
        }



        [HttpGet]
        public ActionResult GyaanGalaxySubmissionForm()
        {
            return base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxySubmissionForm.cshtml", new GyaanGalaxySubmisisonModal());
        }
        [HttpPost]
        public ActionResult GyaanGalaxySubmissionForm(GyaanGalaxySubmisisonModal m, string SubmitApplication)
        {
            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert Gyaan Galaxy Submission Form", "Start");
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
                
                Log.Info("Insert Gyaan Galaxy Submission Form Captcha Validated", "Start");
                GyaanGalaxySubmissionFormHelper helper = new GyaanGalaxySubmissionFormHelper();
                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        if (ModelState.IsValid)
                        {
                            
                            m.Id = Guid.NewGuid();
                           
                         
                                GyaanGalaxySubmissionForm CapitalCareerForm = new GyaanGalaxySubmissionForm()
                                {
                                    Id = m.Id,
                                    RegistrationNo = m.RegistrationNo,
                                    Name = m.Name,
                                    SchoolName = m.SchoolName.Replace(',', '-'),
                                    Age=m.Age,
                                    ClassOrGrade=m.ClassOrGrade,
                                    SchoolAddress= "\"" + m.SchoolAddress + "\"",
                                    HouseAddress= "\"" + m.HouseAddress + "\"",
                                    MobileNo = m.MobileNo,
                                    EmailID = m.EmailID,
                                    CategoryOfProject=m.CategoryOfProject,
                                    TitleOfProject= "\"" + m.TitleOfProject + "\"",
                                    Purpose = "\"" + m.Purpose + "\"",
                                    Procedure = "\"" + m.Procedure + "\"",
                                    NatureDataCollection = "\"" + m.NatureOfDataCollection + "\"",
                                    Conclusions = "\"" + m.Conclusions + "\"",
                                    PossibleResearchApplication = "\"" + m.PossibleResreachApplication+ "\"",
                                    PageInfo = m.PageInfo,
                                    FormType = m.FormName,
                                    FormSubmitOn = new DateTime?(DateTime.Now)

                                };

                             
                                //rdb.GyaanGalaxySubmissionForms.InsertOnSubmit(CapitalCareerForm);
                                //rdb.SubmitChanges();
                                //Log.Info("Form Gyaan Galaxy Contact Form data saved into db successfully: ", this);
                                //SendMailforSubmissionEnroll(m);
                                //return Redirect("/thankyou");
                            
                        }
                    }
                }

                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Registration Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxySubmissionForm.cshtml", m);
                    return actionResult;
                }
                
            }
            else
            {

                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/GyaanGalaxy/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxySubmissionForm.cshtml", m);
                return actionResult;
            }
             ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
            actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxySubmissionForm.cshtml", m);
            return actionResult;
        }

        public void SendMailforSubmissionEnroll(GyaanGalaxySubmisisonModal model)
        {
            try
            {
                Item mailconfigs = Context.Database.GetItem(Templates.MailConfigurationSubmission.MailConfigurationSubmissionItemID);
                Log.Info("Submission Success mail sending to client", this);
                string CustomerFrom = mailconfigs.Fields[Templates.MailConfigurationSubmission.MailConfigurationSubmissionFields.Customer_MailFrom].Value;
                string CustomerTo = model.EmailID;
                string CustomerSubject = mailconfigs.Fields[Templates.MailConfigurationSubmission.MailConfigurationSubmissionFields.Customer_SubjectName].Value;
                string CustomerMailBody = CustomerMailBody = mailconfigs.Fields[Templates.MailConfigurationSubmission.MailConfigurationSubmissionFields.Customer_SuccessMessage].Value;

                string OfficialsFrom = mailconfigs.Fields[Templates.MailConfigurationSubmission.MailConfigurationSubmissionFields.Officials_MailFrom].Value;
                string OfficialsTo = mailconfigs.Fields[Templates.MailConfigurationSubmission.MailConfigurationSubmissionFields.Officials_RecipientMail].Value;
                string OfficialsMailBody = mailconfigs.Fields[Templates.MailConfigurationSubmission.MailConfigurationSubmissionFields.Officials_Message].Value;
                string OfficialsSubject = mailconfigs.Fields[Templates.MailConfigurationSubmission.MailConfigurationSubmissionFields.Officials_SubjectName].Value;

                using (GyaanGalaxyDataContext dbcontexts = new GyaanGalaxyDataContext())
                {
                  GyaanGalaxySubmissionForm ctxs = dbcontexts.GyaanGalaxySubmissionForms.Where(x => x.Id == model.Id && x.RegistrationNo == model.RegistrationNo).FirstOrDefault();
                    HttpPostedFileBase UploadedResumeLinks = model.ResearchPaperAttachment;
                    string UploadedResumeLink = UploadedResumeLinks.FileName;
                   string str5 = string.Concat(new string[] { "<a href=\"https://adaniproduction-cd.azurewebsites.net/GyaanGalaxy/Uploads/", model.Name, "-", model.MobileNo, "-", UploadedResumeLink, "\"", " Download>Click to Download </a>" });
                    CustomerMailBody = CustomerMailBody.Replace("$name", ctxs.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$SchoolName", ctxs.SchoolName);
                    CustomerMailBody = CustomerMailBody.Replace("$MobileNo", ctxs.MobileNo);
                    CustomerMailBody = CustomerMailBody.Replace("$Age", ctxs.Age);
                    CustomerMailBody = CustomerMailBody.Replace("$ClassOrGrade", ctxs.ClassOrGrade);
                    CustomerMailBody = CustomerMailBody.Replace("$HouseAddress", ctxs.HouseAddress);
                    CustomerMailBody = CustomerMailBody.Replace("$SchoolAddress", ctxs.SchoolAddress);
                    CustomerMailBody = CustomerMailBody.Replace("$Email", ctxs.EmailID);
                    CustomerMailBody = CustomerMailBody.Replace("$CategoryOfProject", ctxs.CategoryOfProject);
                    CustomerMailBody = CustomerMailBody.Replace("$TitleOfProject", ctxs.TitleOfProject);
                    CustomerMailBody = CustomerMailBody.Replace("$Email", ctxs.EmailID);
                    //CustomerMailBody = CustomerMailBody.Replace("$teleno", ctxs.TelephoneNo);
                    CustomerMailBody = CustomerMailBody.Replace("$Purpose", ctxs.Purpose);
                    CustomerMailBody = CustomerMailBody.Replace("$Procedure", ctxs.Procedure);
                    CustomerMailBody = CustomerMailBody.Replace("$NatureofDataCollection", ctxs.NatureDataCollection);
                    CustomerMailBody = CustomerMailBody.Replace("$Conclusions", ctxs.Conclusions);
                    CustomerMailBody = CustomerMailBody.Replace("$PossibleResearchApplication", ctxs.PossibleResearchApplication);
                    CustomerMailBody = CustomerMailBody.Replace("$ResearchLink", str5);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", ctxs.RegistrationNo);
                    //officials mail body
                    OfficialsMailBody = OfficialsMailBody.Replace("$name", ctxs.Name);
                    OfficialsMailBody = OfficialsMailBody.Replace("$SchoolName", ctxs.SchoolName);
                    OfficialsMailBody = OfficialsMailBody.Replace("$MobileNo", ctxs.MobileNo);
                    OfficialsMailBody = OfficialsMailBody.Replace("$Age", ctxs.Age);
                    OfficialsMailBody = OfficialsMailBody.Replace("$ClassOrGrade", ctxs.ClassOrGrade);
                    OfficialsMailBody = OfficialsMailBody.Replace("$HouseAddress", ctxs.HouseAddress);
                    OfficialsMailBody = OfficialsMailBody.Replace("$SchoolAddress", ctxs.SchoolAddress);
                    OfficialsMailBody = OfficialsMailBody.Replace("$Email", ctxs.EmailID);
                    OfficialsMailBody = OfficialsMailBody.Replace("$CategoryOfProject", ctxs.CategoryOfProject);
                    OfficialsMailBody = OfficialsMailBody.Replace("$TitleOfProject", ctxs.TitleOfProject);
                    OfficialsMailBody = OfficialsMailBody.Replace("$Purpose", ctxs.Purpose);
                    OfficialsMailBody = OfficialsMailBody.Replace("$Procedure", ctxs.Procedure);
                    OfficialsMailBody = OfficialsMailBody.Replace("$NatureofDataCollection", ctxs.NatureDataCollection);
                    OfficialsMailBody = OfficialsMailBody.Replace("$Conclusions", ctxs.Conclusions);
                    OfficialsMailBody = OfficialsMailBody.Replace("$PossibleResearchApplication", ctxs.PossibleResearchApplication);
                    OfficialsMailBody = OfficialsMailBody.Replace("$ResearchLink", str5);
                    //OfficialsMailBody = OfficialsMailBody.Replace("$teleno", ctx.TelephoneNo);
                    OfficialsMailBody = OfficialsMailBody.Replace("$registrationno", ctxs.RegistrationNo);

                }
                var mailSendingCust = sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom);
                if (mailSendingCust == true)
                {
                    Log.Info("Sending mail to customer is Successfull", this);
                }
                else
                {
                    Log.Info("Sending mail to customer is Failed", this);
                }
                var mailSendingOfc = sendEmail(OfficialsTo, OfficialsSubject, OfficialsMailBody, OfficialsFrom);
                if (mailSendingOfc == true)
                {
                    Log.Info("Sending mail to Officials is Successfull", this);
                }
                else
                {
                    Log.Info("Sending mail to Officials is Failed", this);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at SendMailforVendorEnroll Form Defence Vendor: " + ex.Message, this);
            }

        }
        [HttpGet]
        public ActionResult GyaanGalaxyContactForm()
        {
            return base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxyContactForm.cshtml", new GyaanGalaxyContactModel());
        }
        [HttpPost]
        public ActionResult GyaanGalaxyContactForm(GyaanGalaxyContactModel m, string SubmitApplication)
        {
            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert GyaanGalaxyContactForm", "Start");
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

                Log.Info("Insert GyaanGalaxyContactForm Captcha Validated", "Start");


                var getEmailTo = "";
                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        if (ModelState.IsValid)
                        {
                            GyaanGalaxyDataContext gyaangalaxycontactDataContext = new GyaanGalaxyDataContext();

                            m.Id = Guid.NewGuid();

                            GyaanGalaxyContactUs GyaanGalaxyContactForm = new GyaanGalaxyContactUs()

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

                            gyaangalaxycontactDataContext.GyaanGalaxyContactUs.InsertOnSubmit(GyaanGalaxyContactForm);
                            gyaangalaxycontactDataContext.SubmitChanges();
                            Log.Info("Form Gyaan Galaxy Contact Form data saved into db successfully: ", this);
                            try
                            {
                                var msgTpye = Sitecore.Context.Database.GetItem("{6BA3B6FF-9431-463D-B4F7-3264CC7004CC}");
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
                            actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxyContactForm.cshtml", m);
                            return actionResult;
                        }
                    }

                }

                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Contact Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxyContactForm.cshtml", m);
                }
              
            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/GyaanGalaxy/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxyContactForm.cshtml", m);
            }



          ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
            actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxyContactForm.cshtml", m);
            return actionResult;
        }
        [HttpGet]
        public ActionResult GyaanGalaxyRegistrationForm()
        {
            return base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxyRegistrationForm.cshtml", new GyaanGalaxyRegistrationModal());
        }
        [HttpPost]
        public ActionResult GyaanGalaxyRegistrationForm(GyaanGalaxyRegistrationModal m, string SubmitApplication)
        {
            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert GyaanGalaxyRegistratiobForm", "Start");
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

                Log.Info("Insert GyaanGalaxyRegistratiobForm Captcha Validated", "Start");

                

                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        if (ModelState.IsValid)
                        {
                            GyaanGalaxyDataContext gyaangalaxyregistrationDataContext = new GyaanGalaxyDataContext();
                           
                            m.Id = Guid.NewGuid();

                            GyannGalaxyRegistrationForm gyaangalaxyRegistrationForm = new GyannGalaxyRegistrationForm()

                            {

                                Id = m.Id,
                                
                                StudentName = m.StudentName,
                                Email = m.EmailID,
                                Age = m.Age,
                                MobileNo = m.MobileNo,
                                ClassGrade = "\"" + m.ClassOrGrade + "\"",
                                HouseAddress = "\"" + m.HouseAddress + "\"",
                                SchoolAddress = "\"" + m.SchoolAddress + "\"",
                                SchoolName = "\"" + m.SchoolName + "\"",
                                PageInfo = m.PageInfo,
                                FormType = m.FormName,
                                SubmittedBy = m.SubmittedBy,
                                FormSubmitOn = new DateTime?(DateTime.Now),
                                CreatedOn = new DateTime?(DateTime.Now)

                            };

                            gyaangalaxyregistrationDataContext.GyannGalaxyRegistrationForms.InsertOnSubmit(gyaangalaxyRegistrationForm);
                            gyaangalaxyregistrationDataContext.SubmitChanges();
                            Log.Info("Form GyaanGalaxy Registartion Form data saved into db successfully: ", this);
                            SendMailforVendorEnroll(m);
                            return Redirect("/thankyou");
                        }
                        else
                        {
                            actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxyRegistrationForm.cshtml", m);
                            return actionResult;
                        }

                       
                    }
                }
                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Registration Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxyRegistrationForm.cshtml", m);
                }


            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/GyaanGalaxy/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxyRegistrationForm.cshtml", m);
            }



             ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
            actionResult = base.View("/Views/GyaanGalaxy/Sublayouts/GyaanGalaxyRegistrationForm.cshtml", m);
            return actionResult;
        }


        public void SendMailforVendorEnroll(GyaanGalaxyRegistrationModal model)
        {
            try
            {
                Item mailconfig = Context.Database.GetItem(Templates.MailConfiguration.MailConfigurationItemID);
                Log.Info("Registration Success mail sending to client", this);
                string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                string CustomerTo = model.EmailID;
                string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                string CustomerMailBody = CustomerMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SuccessMessage].Value;

                string OfficialsFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_MailFrom].Value;
                string OfficialsTo = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_RecipientMail].Value;
                string OfficialsMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_Message].Value;
                string OfficialsSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_SubjectName].Value;

                using (GyaanGalaxyDataContext dbcontext = new GyaanGalaxyDataContext())
                {
                    GyannGalaxyRegistrationForm ctx = dbcontext.GyannGalaxyRegistrationForms.Where(x => x.Id == model.Id).FirstOrDefault();
                    CustomerMailBody = CustomerMailBody.Replace("$name", ctx.StudentName);
                    CustomerMailBody = CustomerMailBody.Replace("$Age", ctx.Age);
                    CustomerMailBody = CustomerMailBody.Replace("$MobileNo", ctx.MobileNo);
                    CustomerMailBody = CustomerMailBody.Replace("$Email", ctx.Email);
                    //CustomerMailBody = CustomerMailBody.Replace("$teleno", ctx.TelephoneNo);
                    CustomerMailBody = CustomerMailBody.Replace("$ClassOrGrade", ctx.ClassGrade);
                    CustomerMailBody = CustomerMailBody.Replace("$HouseAddress", ctx.HouseAddress);
                    CustomerMailBody = CustomerMailBody.Replace("$SchoolName", ctx.SchoolName);
                    CustomerMailBody = CustomerMailBody.Replace("$SchoolAddress", ctx.SchoolAddress);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", ctx.RegistrationNo);
                    //officials mail body
                    OfficialsMailBody = OfficialsMailBody.Replace("$name", ctx.StudentName);
                    OfficialsMailBody = OfficialsMailBody.Replace("$Age", ctx.Age);
                    OfficialsMailBody = OfficialsMailBody.Replace("$MobileNo", ctx.MobileNo);
                    OfficialsMailBody = OfficialsMailBody.Replace("$Email", ctx.Email);
                    OfficialsMailBody = OfficialsMailBody.Replace("$ClassOrGrade", ctx.ClassGrade);
                    OfficialsMailBody = OfficialsMailBody.Replace("$HouseAddress", ctx.HouseAddress);
                    OfficialsMailBody = OfficialsMailBody.Replace("$SchoolName", ctx.SchoolName);
                    OfficialsMailBody = OfficialsMailBody.Replace("$SchoolAddress", ctx.SchoolAddress);
                    //OfficialsMailBody = OfficialsMailBody.Replace("$teleno", ctx.TelephoneNo);
                    OfficialsMailBody = OfficialsMailBody.Replace("$registrationno", ctx.RegistrationNo);

                }
                var mailSendingCust = sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom);
                if (mailSendingCust == true)
                {
                    Log.Info("Sending mail to customer is Successfull", this);
                }
                else
                {
                    Log.Info("Sending mail to customer is Failed", this);
                }
                var mailSendingOfc = sendEmail(OfficialsTo, OfficialsSubject, OfficialsMailBody, OfficialsFrom);
                if (mailSendingOfc == true)
                {
                    Log.Info("Sending mail to Officials is Successfull", this);
                }
                else
                {
                    Log.Info("Sending mail to Officials is Failed", this);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at SendMailforVendorEnroll Form Defence Vendor: " + ex.Message, this);
            }

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


