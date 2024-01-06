using Rotativa;
using Sitecore.Diagnostics;
using Sitecore.ElectricityNew.Website.Model;
using Sitecore.ElectricityNew.Website.Services;
using Sitecore.ElectricityNew.Website.Utility;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Sitecore.Data;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using Sitecore.Tasks;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using Sitecore.Foundation.SitecoreExtensions.Attributes;
using Sitecore.Foundation.Dictionary.Extensions;
using Sitecore.Exceptions;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static Sitecore.ElectricityNew.Website.Controllers.FeedbackController;
using Sitecore.Feature.Accounts.Attributes;

namespace Sitecore.ElectricityNew.Website.Controllers
{
    [CookieTemperingRedirectNotFound]
    public class SwitchToGreenController : Controller
    {
        [HttpGet]
        public ActionResult ApplyForm()
        {
            ApplyFormModel model = new ApplyFormModel();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyForm(ApplyFormModel model, string applyForGreenPledge_individual = null, string applyForGreenPledge_group = null, string applyForGreenCharging_individual = null, string applyForGreenCharging_social = null, string applyForGreenPledge_partnership = null)
        {
            SwitchToGreenService obj = new SwitchToGreenService();

            var item1 = Sitecore.Context.Database.GetItem(Templates.SwitchToGreen.SwitchToGreen_ThankYouPage);
            var item2 = Sitecore.Context.Database.GetItem(Templates.SwitchToGreen.SwitchToGreen_EV_ThankYouPage);


            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

            if (!string.IsNullOrEmpty(applyForGreenPledge_individual))
            {
                model.FormType = "I";
                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha_GreenIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }
                bool isError = false;
                if (string.IsNullOrEmpty(model.FullName))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.FullName), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Name is required."));
                }
                if (string.IsNullOrEmpty(model.MobileNumber))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Mobile Number is required."));
                }
                if (string.IsNullOrEmpty(model.EmailId_GreenIn))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.EmailId_GreenIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Email Id is required."));
                }
                if (string.IsNullOrEmpty(model.ZipCode))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.ZipCode), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Zip Code is required."));
                }
                if (string.IsNullOrEmpty(model.VehicleType_In))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.VehicleType_In), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Please select Vehicle Type."));
                }
                if (isError)
                {
                    return this.View(model);
                }
                if (obj.SwitchToGreen_Pledge_CheckLimit(model.EmailId_GreenIn, model.FormType))
                {
                    var r = obj.CreateSwitchToGreen_Pledge(model);
                    if (r == true)
                    {
                        MailMessage mail = this.GetSwitchToGreenIn_EmailTemplate_();
                        SendEmail(model.EmailId_GreenIn, model.FullName, mail, true);
                        return RedirectPermanent(item1.Url());
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Captcha_GreenIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Error Occured", "Request is not saved, please try again!"));
                        return this.View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Captcha_GreenIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Limit Exceeded", "Max application limit exceeded for the Email Id."));
                    return this.View(model);
                }
            }
            if (!string.IsNullOrEmpty(applyForGreenPledge_group))
            {
                model.FormType = "O";
                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha_GreenOrg), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }
                bool isError = false;
                if (string.IsNullOrEmpty(model.OrganizationName))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.OrganizationName), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Organization Name is required."));
                }
                if (string.IsNullOrEmpty(model.EmailId_GreenOrg))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.EmailId_GreenOrg), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Email Id is required."));
                }
                if (string.IsNullOrEmpty(model.VehicleType_Org))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.VehicleType_Org), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Please select Vehicle Type."));
                }
                if (isError)
                {
                    return this.View(model);
                }
                if (obj.SwitchToGreen_Pledge_CheckLimit(model.EmailId_GreenOrg, model.FormType))
                {
                    var r = obj.CreateSwitchToGreen_Pledge(model);
                    if (r == true)
                    {
                        MailMessage mail = this.GetSwitchToGreenOrg_EmailTemplate_();
                        SendEmail(model.EmailId_GreenOrg, model.OrganizationName, mail, true);
                        return RedirectPermanent(item1.Url());
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Captcha_GreenIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Error Occured", "Request is not saved, please try again!"));
                        return this.View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Captcha_GreenOrg), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Limit Exceeded", "Max application limit exceeded for the Email Id."));
                    return this.View(model);
                }
            }
            if (!string.IsNullOrEmpty(applyForGreenCharging_individual))
            {
                model.FormType = "E";
                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha_EVIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }
                bool isError = false;
                if (string.IsNullOrEmpty(model.FullName_EV))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.FullName_EV), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Name is required."));
                }
                if (string.IsNullOrEmpty(model.MobileNumber_EVIn))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.MobileNumber_EVIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Mobile Number is required."));
                }
                if (string.IsNullOrEmpty(model.EmailId_EVIn))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.EmailId_EVIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Email Id is required."));
                }
                if (isError)
                {
                    return this.View(model);
                }
                if (obj.SwitchToGreen_EVCharging_CheckLimit(model.EmailId_EVIn, model.FormType))
                {
                    var r = obj.CreateSwitchToGreen_ChargingRequest(model);
                    if (r == true)
                    {
                        MailMessage mail = this.GetSwitchToEVIn_EmailTemplate_();
                        SendEmail(model.EmailId_EVIn, model.FullName_EV, mail);
                        return RedirectPermanent(item2.Url());
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Captcha_GreenIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Error Occured", "Request is not saved, please try again!"));
                        return this.View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Captcha_EVIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Limit Exceeded", "Max application limit exceeded for the Email Id."));
                    return this.View(model);
                }

            }
            if (!string.IsNullOrEmpty(applyForGreenCharging_social))
            {
                model.FormType = "S";
                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha_EVSo), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }
                bool isError = false;
                if (string.IsNullOrEmpty(model.FullName_EVS))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.FullName_EVS), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Name is required."));
                }
                if (string.IsNullOrEmpty(model.MobileNumber_EVS))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.MobileNumber_EVS), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Mobile Number is required."));
                }
                if (string.IsNullOrEmpty(model.EmailId_EVS))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.EmailId_EVS), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Email Id is required."));
                }
                if (isError)
                {
                    return this.View(model);
                }
                if (obj.SwitchToGreen_EVCharging_CheckLimit(model.EmailId_EVS, model.FormType))
                {
                    var r = obj.CreateSwitchToGreen_ChargingRequest(model);
                    if (r == true)
                    {
                        MailMessage mail = this.GetSwitchToEVIn_EmailTemplate_();
                        SendEmail(model.EmailId_EVS, model.FullName_EVS, mail);
                        return RedirectPermanent(item2.Url());

                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Captcha_GreenIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Error Occured", "Request is not saved, please try again!"));
                        return this.View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Captcha_EVSo), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Limit Exceeded", "Max application limit exceeded for the Email Id."));
                    return this.View(model);
                }

            }
            if (!string.IsNullOrEmpty(applyForGreenPledge_partnership))
            {
                model.FormType = "P";
                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha_EVPartner), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }
                bool isError = false;
                if (string.IsNullOrEmpty(model.FullName_EVP))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.FullName_EVP), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Name is required."));
                }
                if (string.IsNullOrEmpty(model.MobileNumberEVP))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.MobileNumberEVP), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Mobile Number is required."));
                }
                if (string.IsNullOrEmpty(model.EmailId_EVP))
                {
                    isError = true;
                    ModelState.AddModelError(nameof(model.EmailId_EVP), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Email Id is required."));
                }
                if (isError)
                {
                    return this.View(model);
                }
                if (obj.SwitchToGreen_EVCharging_CheckLimit(model.EmailId_EVP, model.FormType))
                {
                    var r = obj.CreateSwitchToGreen_ChargingRequest(model);
                    if (r == true)
                    {
                        MailMessage mail = this.GetSwitchToEVP_EmailTemplate_();
                        SendEmail(model.EmailId_EVP, model.FullName_EVP, mail);
                        return RedirectPermanent(item2.Url());
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Captcha_GreenIn), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Error Occured", "Request is not saved, please try again!"));
                        return this.View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Captcha_EVPartner), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Limit Exceeded", "Max application limit exceeded for the Email Id."));
                    return this.View(model);
                }
            }

            return RedirectPermanent(item1.Url());
        }

        public byte[] CreatePDF(string emailId, string name)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                string htmltext = DictionaryPhraseRepository.Current.Get("/SwitchToGreen/PDFText", "<div style='margin:0; padding:0; width:765px; font-size:22px; color: #242122; '> <img src='" + Server.MapPath("/images/SwitchToGreen/bg1.jpg") + "' /> <div style='text-align:center; '> <p style='width:100%; position: relative; display:block; margin:10px 0; clear: both;font-size: 20px;'>This is to certify that <b>#Name#</b> has successfully pledged to go green with your his/her vehicle and contribute significantly reduce Mumbai’s carbon emissions for a cleaner and greener future.</p> <p style='width:100%; margin-top:10px; display: block; font-family: Adani-semibold; font-size:26px; margin-top:25px;'>Your Pledge is our Pride</p> <p style='width:100%; display: block; margin-top:20px;'> Thank you for creating a better future for Mumbai, while we work towards fulfilling your EV’s needs.</p> <p style='width:90%; text-align: right; display: block; margin-top:35px;margin-right:50px;'><span>Date : " + DateTime.Now.ToString("dd-MMM-yyyy") + "</span> </p> </div> <img src='" + Server.MapPath("/images/SwitchToGreen/bg2.jpg") + "'  /> </div>");
                htmltext = htmltext.Replace("#Name#", name);
                htmltext = htmltext.Replace("#Date#", DateTime.Now.ToString("dd-MMM-yyyy"));
                //StringReader sr = new StringReader(htmltext);
                //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                ////HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                ////htmlparser.Parse(sr);
                ////pdfDoc.Close();

                StringReader sr = new StringReader(htmltext);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();


                return stream.ToArray();
            }
        }

        public void SendEmail(string emailId, string name, MailMessage mail, bool ispdf = false)
        {
            //send email to owners
            try
            {
                mail.To.Add(emailId);
                mail.Body = mail.Body.Replace("#Name#", name);

                if (ispdf)
                {
                    byte[] array_ = CreatePDF(emailId, name);
                    Stream stream1 = new MemoryStream(array_);
                    Attachment a = new Attachment(stream1, "certificate.pdf");
                    mail.Attachments.Add(a);
                }
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + " - Error - " + ex.Message + "", ex, this);
                //throw;
            }
        }

        public MailMessage GetSwitchToGreenIn_EmailTemplate_()
        {
            try
            {
                var settingsItem = Context.Database.GetItem(Template.SwitchToGreen.GetSwitchToGreenIn_EmailTemplate_);
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

                return new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch
            {
                return null;
            }
        }

        public MailMessage GetSwitchToGreenOrg_EmailTemplate_()
        {
            try
            {
                var settingsItem = Context.Database.GetItem(Template.SwitchToGreen.GetSwitchToGreenOrg_EmailTemplate_);
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

                return new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch
            {
                return null;
            }
        }

        public MailMessage GetSwitchToEVIn_EmailTemplate_()
        {
            try
            {
                var settingsItem = Context.Database.GetItem(Template.SwitchToGreen.GetSwitchToEVIn_EmailTemplate_);
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

                return new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch
            {
                return null;
            }
        }

        public MailMessage GetSwitchToEVS_EmailTemplate_()
        {
            try
            {
                var settingsItem = Context.Database.GetItem(Template.SwitchToGreen.GetSwitchToEVS_EmailTemplate_);
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

                return new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch
            {
                return null;
            }
        }

        public MailMessage GetSwitchToEVP_EmailTemplate_()
        {
            try
            {
                var settingsItem = Context.Database.GetItem(Template.SwitchToGreen.GetSwitchToEVP_EmailTemplate_);
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

                return new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch
            {
                return null;
            }
        }
    }
}

