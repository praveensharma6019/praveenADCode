using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.SportsLine.Website;
using Sitecore.SportsLine.Website.Helper;
using Sitecore.SportsLine.Website.Models;
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

namespace Sitecore.SportsLine.Website.Controllers
{
    public class SportsLineController : Controller
    {
        public SportsLineController()
        {
        }


        [HttpGet]
        public ActionResult SportsLineContactForm()
        {
            return base.View("/Views/SportsLine/Sublayouts/SportsLineContactForm.cshtml", new SportsLineContactModel());
        }

        [HttpPost]
        public ActionResult SportsLineContactForm(SportsLineContactModel m, string SubmitApplication)
        {
            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert SportsLineRegistratiobForm", "Start");
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

                Log.Info("Insert SportsLineRegistratiobForm Captcha Validated", "Start");

                SportsLineContactFormHelper helper = new SportsLineContactFormHelper();
                var getEmailTo = "";
                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        if (ModelState.IsValid)
                        {
                           SportsLineContactUsFormDataContext sportsLineregistrationDataContext = new SportsLineContactUsFormDataContext();
                            m.RegistrationNo = helper.GetUniqueRegNo();
                            while (true)
                            {
                                if ((
                                    from a in sportsLineregistrationDataContext.SportsLineContactUs
                                    where a.RegistrationNo == m.RegistrationNo
                                    select a).FirstOrDefault<SportsLineContactUs>() == null)
                                {
                                    break;
                                }
                                m.RegistrationNo = helper.GetUniqueRegNo();
                            }
                            m.Id = Guid.NewGuid();

                            SportsLineContactUs sportslineRegistrationForm = new SportsLineContactUs()

                            {

                                Id = m.Id,
                                RegistrationNo = m.RegistrationNo,
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

                            sportsLineregistrationDataContext.SportsLineContactUs.InsertOnSubmit(sportslineRegistrationForm);
                            sportsLineregistrationDataContext.SubmitChanges();
                            Log.Info("Form SportsLine Contact Form data saved into db successfully: ", this);
                        }
                        else
                        {
                            actionResult = base.View("/Views/SportsLine/Sublayouts/SportsLineContactForm.cshtml", m);
                            return actionResult;
                        }
                    }

                }

                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Contact Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/SportsLine/Sublayouts/SportsLineContactForm.cshtml", m);
                }
                try
                {
                    var msgTpye = Sitecore.Context.Database.GetItem("{9BAA12C9-2A94-40CA-B4BE-780B0DCAB426}");
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
            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/SportsLine/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/SportsLine/Sublayouts/SportsLineContactForm.cshtml", m);
            }



            return this.Redirect("/thankyou");
        }
        [HttpGet]
        public ActionResult SportsLineRegistrationForm()
        {
            return base.View("/Views/SportsLine/Sublayouts/SportsLineRegistrationForm.cshtml", new SportsLineRegistrationFormModel());
        }
        [HttpPost]
        public ActionResult SportsLineRegistrationForm(SportsLineRegistrationFormModel m, string SubmitApplications)
        {
            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert SportsLineRegistrationForm", "Start");
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

                Log.Info("Insert SportsLineRegistrationForm Captcha Validated", "Start");

                SportsLineRegistrationFormHelper helper = new SportsLineRegistrationFormHelper();

                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplications))
                    {
                        if (ModelState.IsValid)
                        {
                            if (m.SportsTypeList != null && m.SportsTypeList.Count > 0)
                            {
                                m.SportsType = helper.GetSelectedCheckboxValues(m.SportsTypeList);
                            }
                            SportsLineContactUsFormDataContext sportsLineregistrationDataContext = new SportsLineContactUsFormDataContext();
                            m.RegistrationNo = helper.GetUniqueRegNo();
                            while (true)
                            {
                                if ((
                                    from a in sportsLineregistrationDataContext.SportsLineRegistrationForms
                                    where a.RegistrationNo == m.RegistrationNo
                                    select a).FirstOrDefault<SportsLineRegistrationForm>() == null)
                                {
                                    break;
                                }
                                m.RegistrationNo = helper.GetUniqueRegNo();
                            }
                            m.Id = Guid.NewGuid();

                            SportsLineRegistrationForm sportslineRegistrationForm = new SportsLineRegistrationForm()

                            {
                               
                                Id = m.Id,
                                RegistrationNo = m.RegistrationNo,
                                Name = m.Name,
                                LastName = m.LastName,
                                DateOfBirth= m.DateofBirth,
                                Email = m.EmailID,
                                Address =  "\"" + m.Address+ "\"",
                                ContactNumber = m.MobileNo,
                                Gender = m.Gender,
                                PageInfo = m.PageInfo,
                                FormType = m.FormName,
                                SportType = m.SportsType,
                                SubmittedBy = m.SubmittedBy,
                                FormSubmitOn = new DateTime?(DateTime.Now),
                                CreatedOn = new DateTime?(DateTime.Now)

                            };

                            sportsLineregistrationDataContext.SportsLineRegistrationForms.InsertOnSubmit(sportslineRegistrationForm);
                            sportsLineregistrationDataContext.SubmitChanges();
                            Log.Info("Form SportsLine Registration Form data saved into db successfully: ", this);
                        }
                        else
                        {
                            actionResult = base.View("/Views/SportsLine/Sublayouts/SportsLineRegistrationForm.cshtml", m);
                            return actionResult;
                        }



                        SendMailforVendorEnroll(m);
                        return Redirect("/thankyou");
                    }
                }
                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Registration Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/SportsLine/Sublayouts/SportsLineRegistrationForm.cshtml", m);
                }
               
               
            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/SportsLine/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/SportsLine/Sublayouts/SportsLineRegistrationForm.cshtml", m);
            }



            return this.Redirect("/thankyou");
        }
        
        public void SendMailforVendorEnroll(SportsLineRegistrationFormModel model)
        {
            try
            {
                Item mailconfig = Context.Database.GetItem(Templates.MailConfiguration.MailConfigurationItemID);
                Log.Info("Payment Success mail sending to client", this);
                string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                string CustomerTo = model.EmailID;
                string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                string CustomerMailBody = CustomerMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SuccessMessage].Value;

                string OfficialsFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_MailFrom].Value;
                string OfficialsTo = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_RecipientMail].Value;
                string OfficialsMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_Message].Value;
                string OfficialsSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_SubjectName].Value;

                using (SportsLineContactUsFormDataContext dbcontext = new SportsLineContactUsFormDataContext())
                {
                    SportsLineRegistrationForm ctx = dbcontext.SportsLineRegistrationForms.Where(x => x.Id == model.Id && x.RegistrationNo == model.RegistrationNo).FirstOrDefault();
                   CustomerMailBody = CustomerMailBody.Replace("$name", ctx.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$lastname", ctx.LastName);
                    CustomerMailBody = CustomerMailBody.Replace("$address", ctx.Address);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", ctx.ContactNumber);
                    //CustomerMailBody = CustomerMailBody.Replace("$teleno", ctx.TelephoneNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", ctx.Email);
                    CustomerMailBody = CustomerMailBody.Replace("$gender", ctx.Gender);
                    CustomerMailBody = CustomerMailBody.Replace("$sporttype", ctx.SportType);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", ctx.RegistrationNo);
                    //officials mail body
                    OfficialsMailBody = OfficialsMailBody.Replace("$mobile", ctx.ContactNumber);
                    OfficialsMailBody = OfficialsMailBody.Replace("$name", ctx.Name);
                    OfficialsMailBody = OfficialsMailBody.Replace("$address", ctx.Address);
                    OfficialsMailBody = OfficialsMailBody.Replace("$lastname", ctx.LastName);
                    OfficialsMailBody = OfficialsMailBody.Replace("$mail", ctx.Email);
                    OfficialsMailBody = OfficialsMailBody.Replace("$gender", ctx.Gender);
                    OfficialsMailBody = OfficialsMailBody.Replace("$sporttype", ctx.SportType);

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


