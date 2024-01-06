using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.AdaniCapital.Website;
using Sitecore.AdaniCapital.Website.Helper;
using Sitecore.AdaniCapital.Website.Models;
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
using Sitecore.AdaniCapital.Website.Services;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;
using Sitecore.Mvc.Configuration;
using Sitecore.Foundation.SitecoreExtensions.Attributes;
using Sitecore.AdaniCapital.Website.Attributes;
using System.Reflection;

namespace Sitecore.AdaniCapital.Website.Controllers
{
    public class AdaniCapitalController : Controller
    {
        public AdaniCapitalController(IGetRedirectUrlService getRedirectUrlService, IAccountsSettingsService accountsSettingsService)
        {
            this.GetRedirectUrlService = getRedirectUrlService;
            this.AccountsSettingsService = accountsSettingsService;
        }
        private IGetRedirectUrlService GetRedirectUrlService { get; }
        private IAccountsSettingsService AccountsSettingsService { get; }
        AdaniCapitalWebAPIServices WebServices = new AdaniCapitalWebAPIServices();
        DatabaseServices dbServices = new DatabaseServices();
        AdaniCapitalFormsDataContext dbContext = new AdaniCapitalFormsDataContext();
        AdaniCapitalGetBranchDetails BranchDataObj = new AdaniCapitalGetBranchDetails();
        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
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
        public void SendMailforVendorEnroll(AdaniCapitalCareerFormModel model)
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

                using (AdaniCapitalCareerFormDataContext dbcontext = new AdaniCapitalCareerFormDataContext())
                {
                    AdaniCapitalCareersForm ctx = dbcontext.AdaniCapitalCareersForms.Where(x => x.Id == model.Id && x.RegistrationNo == model.RegistrationNo).FirstOrDefault();
                    CustomerMailBody = CustomerMailBody.Replace("$name", ctx.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$lastname", ctx.LastName);
                    CustomerMailBody = CustomerMailBody.Replace("$city", ctx.City);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", ctx.ContactNumber);
                    //CustomerMailBody = CustomerMailBody.Replace("$teleno", ctx.TelephoneNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", ctx.Email);
                    CustomerMailBody = CustomerMailBody.Replace("$experience", ctx.Experience);
                    CustomerMailBody = CustomerMailBody.Replace("$position", ctx.Position);
                    CustomerMailBody = CustomerMailBody.Replace("$expectedsalary", ctx.ExpectedSalary);
                    CustomerMailBody = CustomerMailBody.Replace("$dateofjoining", ctx.DateOfJoining.ToString());
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", ctx.RegistrationNo);
                    //officials mail body
                    OfficialsMailBody = OfficialsMailBody.Replace("$mobile", ctx.ContactNumber);
                    OfficialsMailBody = OfficialsMailBody.Replace("$name", ctx.Name);
                    OfficialsMailBody = OfficialsMailBody.Replace("$lastname", ctx.LastName);
                    OfficialsMailBody = OfficialsMailBody.Replace("$city", ctx.City);
                    OfficialsMailBody = OfficialsMailBody.Replace("$mail", ctx.Email);
                    OfficialsMailBody = OfficialsMailBody.Replace("$experience", ctx.Experience);
                    OfficialsMailBody = OfficialsMailBody.Replace("$position", ctx.Position);
                    OfficialsMailBody = OfficialsMailBody.Replace("$expectedsalary", ctx.ExpectedSalary);
                    OfficialsMailBody = OfficialsMailBody.Replace("$dateofjoining", ctx.DateOfJoining.ToString());


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
                Sitecore.Diagnostics.Log.Error("Error at SendMailform Form Career: " + ex.Message, this);
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
        [HttpGet]
        public ActionResult AdaniCapitalCareerForms()
        {
            return base.View("/Views/AdaniCapital/Sublayouts/CareerPageForm.cshtml", new AdaniCapitalCareerFormModel());
        }
        [HttpPost]
        public ActionResult AdaniCapitalCareerForms(AdaniCapitalCareerFormModel m, string SubmitApplication)
        {
            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert AdaniCapitalCareerForm", "Start");
            bool flag = false;

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

                Log.Info("Insert AdaniCapitalCareerForm Captcha Validated", "Start");

                AdaniCapitalCareerForm helper = new AdaniCapitalCareerForm();
                byte[] bytes;
                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        if (ModelState.IsValid)
                        {
                            AdaniCapitalCareerFormDataContext rdb = new AdaniCapitalCareerFormDataContext();

                            AdaniCapitalCareersForm CapitalCareerForm = new AdaniCapitalCareersForm()
                            {
                                Id = m.Id,
                                RegistrationNo = m.RegistrationNo,
                                Name = m.Name,
                                LastName = m.LastName,
                                City = m.City,
                                Email = m.EmailID,
                                ExpectedSalary = m.ExpectedSalary,
                                ContactNumber = m.MobileNo,
                                DateOfJoining = m.ExpectedJoiningDate,
                                PageInfo = m.PageInfo,
                                FormType = m.FormName,
                                Position = m.Position,
                                Experience = m.Experience,
                                LinkedinProfile = m.LinkedinProfile,
                                FormSubmitOn = new DateTime?(DateTime.Now)

                            };

                            if (m.ResumeAttachment != null && m.ResumeAttachment.ContentLength > 0)
                            {
                                HttpPostedFileBase resumeAttachment = m.ResumeAttachment;
                                var supportedTypes = new[] { "doc", "docx", "pdf" };
                                AdaniCapitalCareerFormModel.FileUpload1 fu = new AdaniCapitalCareerFormModel.FileUpload1();
                                var fileExt = System.IO.Path.GetExtension(resumeAttachment.FileName).Substring(1);
                                if (!supportedTypes.Contains(fileExt.ToLower()))
                                {
                                    Log.Error("File Extension Is InValid - Only Upload DOC/PDF/DOCX File", this);
                                    base.ModelState.AddModelError("ResumeAttachment", DictionaryPhraseRepository.Current.Get("/AdaniCapital/Controller Messages/Invalid File", "File Extension Is InValid - Only Upload DOC/PDF/DOCX File"));
                                    Session["Key"] = true;
                                    return View("/Views/AdaniCapital/Sublayouts/CareerPageForm.cshtml", m);
                                }
                                else if (resumeAttachment.ContentLength > ((4 * 1024) * 1024))
                                {
                                    Log.Error("File size Should Be UpTo 4 MB", this);
                                    base.ModelState.AddModelError("ResumeAttachment", DictionaryPhraseRepository.Current.Get("/AdaniCapital/Controller Messages/Invalid File Size", "File size Should Be UpTo 4 MB"));
                                    Session["Key"] = true;
                                    return View("/Views/AdaniCapital/Sublayouts/CareerPageForm.cshtml", m);
                                }
                                else if (!fu.FileMIMEisValid(resumeAttachment))
                                {
                                    Log.Error("File type Is InValid - Only Upload DOC/PDF/DOCX File", this);
                                    base.ModelState.AddModelError("ResumeAttachment", DictionaryPhraseRepository.Current.Get("/AdaniCapital/Controller Messages/Invalid File", "File Type Is InValid - Only Upload DOC/PDF/DOCX File"));
                                    Session["Key"] = true;
                                    return View("/Views/AdaniCapital/Sublayouts/CareerPageForm.cshtml", m);
                                }
                                HttpPostedFileBase file = m.ResumeAttachment;
                                using (BinaryReader br = new BinaryReader(file.InputStream))
                                {
                                    bytes = br.ReadBytes(file.ContentLength);
                                }
                                CapitalCareerForm.UploadedResume = bytes;
                                CapitalCareerForm.UploadedResumeLink = m.PageInfo.Replace(m.UploadedResumeLink, "/") + "api/adanicapital/DownloadFile?id=" + m.Id + "&RegNo=" + m.RegistrationNo;
                                CapitalCareerForm.UploadedFileType = resumeAttachment.ContentType;
                                CapitalCareerForm.UploadedFileName = m.Name + "_" + m.LastName + "_" + m.MobileNo;
                            }
                            rdb.AdaniCapitalCareersForms.InsertOnSubmit(CapitalCareerForm);
                            rdb.SubmitChanges();
                            Log.Info("Form Career Form data saved into db successfully: ", this);
                            SendMailforVendorEnroll(m);
                            return Redirect("/thankyou");
                        }
                        else
                        {
                            actionResult = base.View("/Views/AdaniCapital/Sublayouts/CareerPageForm.cshtml", m);
                            Session["Key"] = true;
                            return actionResult;
                        }
                    }
                }
                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Registration Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/AdaniCapital/Sublayouts/CareerPageForm.cshtml", m);
                    return actionResult;
                }
            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/AdaniCapital/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/AdaniCapital/Sublayouts/CareerPageForm.cshtml", m);
                return actionResult;
            }
            ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
            actionResult = base.View("/Views/AdaniCapital/Sublayouts/CareerPageForm.cshtml", m);
            return actionResult;
        }
        public FileResult DownloadFile(Guid id, string RegNo)
        {
            try
            {
                using (AdaniCapitalCareerFormDataContext dbcontext = new AdaniCapitalCareerFormDataContext())
                {

                    if (dbcontext.AdaniCapitalCareersForms.Any(a => a.Id == id && a.RegistrationNo == RegNo))
                    {
                        var fileToDownload = dbcontext.AdaniCapitalCareersForms.Where(i => i.Id == id && i.RegistrationNo == RegNo).FirstOrDefault();
                        return File(fileToDownload.UploadedResume.ToArray(), fileToDownload.UploadedFileType, fileToDownload.UploadedFileName);
                    }

                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at DownloadFile:" + ex.Message, this);
            }
            return null;
        }
        public ActionResult AdaniCapitalContactUs()
        {
            AdaniCapitalContactUsModal model = new AdaniCapitalContactUsModal();
            Item SubjectItemList = db.GetItem(Templates.ContactUSSubject.ContactUsSubjectListItemID);
            if (SubjectItemList.HasChildren)
            {
                model.SubjectList = SubjectItemList.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value ?? "",
                    Value = x.Fields["Value"].Value ?? ""
                }).ToList();
            }
            return View("/Views/AdaniCapital/ContactUsForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult AdaniCapitalContactUs(AdaniCapitalContactUsModal model, string Submit)
        {
            Item SubjectItemList = db.GetItem(Templates.ContactUSSubject.ContactUsSubjectListItemID);
            bool Validated = false;
            model.SubjectList = SubjectItemList.GetChildren().ToList().Select(x => new SelectListItem()
            {
                Text = x.Fields["Text"].Value ?? "",
                Value = x.Fields["Value"].Value ?? ""
            }).ToList();
            Log.Info("Insert AdaniCapitalContactUsForm", "Start");
            try
            {
                if (!string.IsNullOrEmpty(Submit))
                {
                    try
                    {
                        Validated = this.IsReCaptchValid(model.reResponse);
                    }
                    catch (Exception ex)
                    {
                        Exception exception = ex;
                        Log.Info(string.Concat("AdaniCapital Contact Us Failed to validate auto script : ", ex.ToString()), this);
                        return View("/Views/AdaniCapital/ContactUsForm.cshtml", model);
                    }
                    if (Validated)
                    {
                        if (!ModelState.IsValid)
                        {
                            Log.Info("Failed to Submit AdaniCapital contactUs form as Invalid values entered", this);
                            return View("/Views/AdaniCapital/ContactUsForm.cshtml", model);
                        }
                        var m = WebServices.FreshDeskContactUsCreateTicket(model);
                        dbServices.StoreContactUsData(m);
                        Session["ContactTicketId"] = m.TicketID ?? null;
                        var item = Context.Database.GetItem(Templates.SitecoreItems.ThankyouPage);
                        return Redirect(item.Url());
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.reResponse), DictionaryPhraseRepository.Current.Get("/Controller/Messages/CaptchaError", "Captcha is Invalid"));
                    }
                }
                ViewBag.Message(model.TicketID ?? "Request Failed");
                return View("/Views/AdaniCapital/ContactUsForm.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("Failed to Submit AdaniCapital ContactUs Form : ", ex.ToString()), this);
                return View("/Views/AdaniCapital/ContactUsForm.cshtml", model);
            }
        }

        [HttpGet]
        [ImportModelState]
        public ActionResult AdaniCapitalBecomeAPartner()
        {
            BecomeAPartnerModel model = new BecomeAPartnerModel();
            return View("/Views/AdaniCapital/BecomeAPartnerForm.cshtml", model);
        }
        [HttpPost]
        [ExportModelState]
        public ActionResult AdaniCapitalBecomeAPartner(BecomeAPartnerModel model, string Submit)
        {
            bool Validated = false;
            Log.Info("Insert AdaniCapitalBecomeAPartnerForm", "Start");
            try
            {
                if (!string.IsNullOrEmpty(Submit))
                {
                    try
                    {
                        Validated = this.IsReCaptchValid(model.reResponse);
                    }
                    catch (Exception ex)
                    {
                        Exception exception = ex;
                        Log.Info(string.Concat("AdaniCapitalBecomeAPartner Failed to validate auto script : ", ex.ToString()), this);
                        Session["Key"] = true;
                        return View("/Views/AdaniCapital/BecomeAPartnerForm.cshtml", model);
                    }
                    if (Validated)
                    {
                        if (!ModelState.IsValid)
                        {
                            Log.Info("Failed to Submit AAdaniCapitalBecomeAPartner form as Invalid values entered", this);
                            Session["Key"] = true;
                            return View("/Views/AdaniCapital/BecomeAPartnerForm.cshtml", model);
                        }
                        var m = WebServices.FreshDeskBecomePartnerCreateTicket(model);
                        dbServices.StoreBecomePartnerData(m);
                        Session["BAPTicketId"] = m.TicketID ?? null;
                        var item = Context.Database.GetItem(Templates.SitecoreItems.ThankyouPage);
                        Session["Key"] = false;
                        return Redirect(item.Url());
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.reResponse), DictionaryPhraseRepository.Current.Get("/Controller/Messages/CaptchaError", "Captcha is Invalid"));
                    }
                }
                Session["Key"] = true;
                ViewBag.Message(model.TicketID ?? "Request Error");
                return View("/Views/AdaniCapital/BecomeAPartnerForm.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("Failed to Submit AdaniCapital ContactUs Form : ", ex.ToString()), this);
                Session["Key"] = true;
                ViewBag.Message("Something has been worng, Please try again later.");
                return View("/Views/AdaniCapital/BecomeAPartnerForm.cshtml", model);
            }
        }

        [HttpGet]
        public ActionResult AdaniCapitalApplyForLoan()
        {
            AdaniCapitalApplyforLoanModel model = new AdaniCapitalApplyforLoanModel();
            Item ProductsItemList = db.GetItem(Templates.ApplyForLoanDropdown.ProductListItem);
            //if (ProductsItemList != null)
            //{
            //    model.ProductsList = ProductsItemList.GetChildren().ToList().Select(x => new SelectListItem()
            //    {
            //        Text = x.Fields["Text"].Value ?? "",
            //        Value = x.Fields["Value"].Value ?? ""
            //    }).ToList();
            //}
            return View("/Views/AdaniCapital/ApplyForLoanForm.cshtml", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdaniCapitalApplyForLoan(AdaniCapitalApplyforLoanModel model, string Submit)
        {
            bool Validated = true;
            Item ProductsItemList = db.GetItem(Templates.ApplyForLoanDropdown.ProductListItem);
            //Item EnquirySourceList = db.GetItem(Templates.ApplyForLoanDropdown.EnquirySourceList);
            //Item OccupationList = db.GetItem(Templates.ApplyForLoanDropdown.OccupationList);
            if (ProductsItemList != null)
            {
                model.ProductsList = ProductsItemList.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value ?? "",
                    Value = x.Fields["Value"].Value ?? ""
                }).ToList();
            }
            Log.Info("Insert AdaniCapitalApplyForLoanForm", "Start");
            try
            {
                if (!string.IsNullOrEmpty(Submit))
                {
                    //Session["LMSLeadMsg"] = null;
                    //Session["LMSLeadNo"] = null;
                    Session["ApplyLoanTicketStatus"] = null;
                    Session["ApplyTicketNo"] = null;
                    try
                    {
                        //Validated = this.IsReCaptchValid(model.reResponse);
                    }
                    catch (Exception ex)
                    {
                        Exception exception = ex;
                        Log.Info(string.Concat("AdaniCapital Contact Us Failed to validate auto script : ", ex.ToString()), this);
                        return View("/Views/AdaniCapital/ApplyForLoanForm.cshtml", model);
                    }
                    if (Validated)
                    {
                        if (!string.IsNullOrEmpty(model.PinCode))
                        {
                            var pincodeDetails = POdetails(model.PinCode);
                            if (pincodeDetails == null)
                            {
                                ModelState.AddModelError(nameof(model.PinCode), DictionaryPhraseRepository.Current.Get("/Controller/Messages/PincodeNotFound", "Pincode not found."));
                                return View("/Views/AdaniCapital/ApplyForLoanForm.cshtml", model);
                            }
                        }
                        if (!ModelState.IsValid)
                        {
                            Log.Info("Failed to Submit AdaniCapital contactUs form as Invalid values entered", this);
                            return View("/Views/AdaniCapital/ApplyForLoanForm.cshtml", model);
                        }
                        var m = WebServices.ApplyForLoanCreateEnquiry(model);
                        //var FDTicket = WebServices.FreshDeskApplyForLoanTicket(model);
                        if (!string.IsNullOrEmpty(m.LMS_Response))
                        {
                            if (m.LMS_Response != "Lead with specified mobile already exists.")
                            {
                                dbServices.StoreApplyForLoanData(m);
                            }
                            //Session["LMSLeadMsg"] = m.LMS_Response;
                            //Session["LMSLeadNo"] = m.LMS_RequestKey;
                            Session["ApplyLoanTicketStatus"] = true;
                            Session["ApplyTicketNo"] = m.LMS_Response;
                            var item = Context.Database.GetItem(Templates.SitecoreItems.ThankyouPage);
                            Session["Key"] = false;
                            return Redirect(item.Url());
                        }
                        else
                        {
                            ViewBag.Message("Something has been wrong. Please try again.");
                            Session["ApplyLoanTicketStatus"] = false;
                            return View("/Views/AdaniCapital/ApplyForLoanForm.cshtml", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.reResponse), DictionaryPhraseRepository.Current.Get("/Controller/Messages/CaptchaError", "Captcha is Invalid"));
                        return View("/Views/AdaniCapital/ApplyForLoanForm.cshtml", model);
                    }
                }
                //ViewBag.Message(model.LMS_Response);
                ViewBag.Message("Something has been wrong. Please try again.");
                return View("/Views/AdaniCapital/ApplyForLoanForm.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("Failed to Submit AdaniCapital ApplyForLoan Form : ", ex.ToString()), this);
                ViewBag.Message("Something has been wrong. Please try again.");
                Session["ApplyLoanTicketStatus"] = false;
                return View("/Views/AdaniCapital/ApplyForLoanForm.cshtml", model);
            }
        }
        [HttpPost]
        public ActionResult AdaniCapitalApplyForLoanPopForm(AdaniCapitalApplyforLoanModel model)
        {
            bool Validated = false;
            var result = new { status = "0", msg = "" };
            string ErrorMsg = "";
            Log.Info("Insert AdaniCapitalApplyForLoanForm", "Start");
            try
            {
                Session["ApplyLoanTicketStatus"] = null;
                Session["ApplyTicketNo"] = null;
                try
                {
                    Validated = this.IsReCaptchValid(model.reResponse);
                }
                catch (Exception ex)
                {
                    Exception exception = ex;
                    Log.Info(string.Concat("AdaniCapital Contact Us Failed to validate auto script : ", ex.ToString()), this);
                    return View("/Views/AdaniCapital/OnLoadLoanFormPopUp.cshtml", model);
                }
                if (Validated)
                {
                    if (!string.IsNullOrEmpty(model.PinCode))
                    {
                        var pincodeDetails = POdetails(model.PinCode);
                        if (pincodeDetails == null)
                        {
                            result = new { status = "0", msg = DictionaryPhraseRepository.Current.Get("/Controller/Messages/PincodeNotFound", "Pincode not found.") };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (!ModelState.IsValid)
                    {
                        Log.Info("Failed to Submit AdaniCapital contactUs form as Invalid values entered", this);
                        foreach (ModelState modelState in ModelState.Values)
                        {
                            foreach (ModelError error in modelState.Errors)
                            {
                                ErrorMsg = ErrorMsg + error.ErrorMessage + "\n";
                            }
                        }
                        result = new { status = "0", msg = ErrorMsg };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    var m = WebServices.ApplyForLoanCreateEnquiry(model);
                    //var FDTicket = WebServices.FreshDeskApplyForLoanTicket(model);
                    if (!string.IsNullOrEmpty(m.LMS_Response))
                    {
                        if (m.LMS_Response != "Lead with specified mobile already exists.")
                        {
                            dbServices.StoreApplyForLoanData(m);
                        }
                        //Session["LMSLeadMsg"] = m.LMS_Response;
                        //Session["LMSLeadNo"] = m.LMS_RequestKey;
                        Session["ApplyLoanTicketStatus"] = true;
                        Session["ApplyTicketNo"] = m.LMS_Response;
                        var item = Context.Database.GetItem(Templates.SitecoreItems.ThankyouPage);
                        result = new { status = "1", msg = "/thankyou" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result = new { status = "0", msg = "Something has been wrong. Please try again." };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.reResponse), DictionaryPhraseRepository.Current.Get("/Controller/Messages/CaptchaError", "Captcha is Invalid"));
                    result = new { status = "0", msg = "Captcha is Invalid" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("Failed to Submit AdaniCapital ApplyForLoan Form : ", ex.ToString()), this);
                result = new { status = "0", msg = "Failed to Submit. Something has been wrong. Please try again." };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FeedbackForm()
        {
            FeedbackFormModel model = new FeedbackFormModel();
            return View("/Views/AdaniCapital/FeedbackForm.cshtml", model);
        }
        [HttpPost]
        public ActionResult FeedbackForm(FeedbackFormModel model)
        {
            bool Validated = false;
            Log.Info("Insert AdaniCapitalFeedbackForm", "Start");
            var result = new { status = "0", msg = "" };
            string ErrorMsg = "";
            try
            {
                try
                {
                    Validated = this.IsReCaptchValid(model.reResponse);
                }
                catch (Exception ex)
                {
                    Exception exception = ex;
                    Log.Info(string.Concat("AdaniCapitalFeedbackForm Failed to validate auto script : ", ex.ToString()), this);
                    result = new { status = "0", msg = "Captcha Validation failed" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (Validated)
                {
                    if (!ModelState.IsValid)
                    {
                        Log.Info("Failed to Submit AdaniCapitalFeedbackForm as Invalid values entered", this);
                        foreach (ModelState modelState in ModelState.Values)
                        {
                            foreach (ModelError error in modelState.Errors)
                            {
                                ErrorMsg = ErrorMsg + error.ErrorMessage + "\n";
                            }
                        }
                        result = new { status = "0", msg = ErrorMsg };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    dbServices.StoreFeedbackFormData(model);
                    if (model.SavedinDB)
                    {
                        result = new { status = "1", msg = "Thank your for your valuable feedback" };
                        SendMailforFeedbackForm(model);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result = new { status = "0", msg = "Something has been wrong, Please try again" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.reResponse), DictionaryPhraseRepository.Current.Get("/Controller/Messages/CaptchaError", "Captcha is Invalid"));
                    result = new { status = "0", msg = "Invalid Captcha" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //ViewBag.Message(model.LMS_Response);

                //return View("/Views/AdaniCapital/FeedbackForm.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("Failed to Submit AdaniCapital ApplyForLoan Form : ", ex.ToString()), this);
                //return View("/Views/AdaniCapital/FeedbackForm.cshtml", model);
                result = new { status = "0", msg = "Something has been wrong, Please try again" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public void SendMailforFeedbackForm(FeedbackFormModel model)
        {
            try
            {
                Item mailconfig = Context.Database.GetItem(Templates.FeedbackMailConfiguration.MailConfigurationItemID);
                Log.Info("Adani Capital Feedback mail sending to client", this);
                string CustomerFrom = mailconfig.Fields[Templates.FeedbackMailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                string CustomerTo = model.EmailID;
                string CustomerSubject = mailconfig.Fields[Templates.FeedbackMailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                string CustomerMailBody = CustomerMailBody = mailconfig.Fields[Templates.FeedbackMailConfiguration.MailConfigurationFields.Customer_SuccessMessage].Value;

                string OfficialsFrom = mailconfig.Fields[Templates.FeedbackMailConfiguration.MailConfigurationFields.Officials_MailFrom].Value;
                string OfficialsTo = mailconfig.Fields[Templates.FeedbackMailConfiguration.MailConfigurationFields.Officials_RecipientMail].Value;
                string OfficialsMailBody = mailconfig.Fields[Templates.FeedbackMailConfiguration.MailConfigurationFields.Officials_Message].Value;
                string OfficialsSubject = mailconfig.Fields[Templates.FeedbackMailConfiguration.MailConfigurationFields.Officials_SubjectName].Value;

                using (AdaniCapitalFormsDataContext dbcontext = new AdaniCapitalFormsDataContext())
                {
                    AdaniCapitalFeedbackForm ctx = dbcontext.AdaniCapitalFeedbackForms.Where(x => x.Id == model.Id).FirstOrDefault();
                    CustomerMailBody = CustomerMailBody.Replace("$name", ctx.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", ctx.Email);
                    //officials mail body
                    OfficialsMailBody = OfficialsMailBody.Replace("$name", ctx.Name);

                }
                var mailSendingCust = sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom);
                if (mailSendingCust == true)
                {
                    Log.Info("Sending mail to ACPL feedback customer is Successfull", this);
                }
                else
                {
                    Log.Info("Sending mail to ACPL feedback customer is Failed", this);
                }
                var mailSendingOfc = sendEmail(OfficialsTo, OfficialsSubject, OfficialsMailBody, OfficialsFrom);
                if (mailSendingOfc == true)
                {
                    Log.Info("Sending mail to ACPL feedback Officials is Successfull", this);
                }
                else
                {
                    Log.Info("Sending mail to ACPL feedback Officials is Failed", this);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at SendMailform ACPL feedback : " + ex.Message, this);
            }

        }
        public ActionResult RequestCallbackForm()
        {
            RequestCallbackModel model = new RequestCallbackModel();
            return View("/Views/AdaniCapital/RequestCallbackPopupForm.cshtml", model);
        }
        [HttpPost]
        public ActionResult RequestCallbackForm(RequestCallbackModel model)
        {
            bool Validated = false;
            Log.Info("Insert AdaniCapitalRequestCallbackForm", "Start");
            var result = new { status = "0", msg = "" };
            string ErrorMsg = "";
            try
            {
                try
                {
                    Validated = this.IsReCaptchValid(model.reResponse);
                }
                catch (Exception ex)
                {
                    Exception exception = ex;
                    Log.Info(string.Concat("AdaniCapitalRequestCallbackForm Failed to validate auto script : ", ex.ToString()), this);
                    result = new { status = "0", msg = "Captcha Validation failed" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (Validated)
                {
                    if (!ModelState.IsValid)
                    {
                        Log.Info("Failed to Submit AdaniCapitalRequestCallbackForm as Invalid values entered", this);
                        foreach (ModelState modelState in ModelState.Values)
                        {
                            foreach (ModelError error in modelState.Errors)
                            {
                                ErrorMsg = ErrorMsg + error.ErrorMessage + "\n";
                            }
                        }
                        result = new { status = "0", msg = ErrorMsg };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    var m = WebServices.FreshDeskReqCallbackTicket(model);
                    if (!string.IsNullOrEmpty(m.TicketID))
                    {
                        model.TicketID = m.TicketID;
                        model.IsSubmittedToFreshdesk = m.IsSubmittedToFreshdesk;
                    }
                    dbServices.StoreRequestCallbackFormData(m);
                    if (m.SavedinDB)
                    {
                        result = new { status = "1", msg = DictionaryPhraseRepository.Current.Get("/Controller/Messages/RequestCallbackSuccessMsg", "Thanks for reaching, we will call you back shortly. Kindly note your ticket No: " + model.TicketID) };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result = new { status = "0", msg = DictionaryPhraseRepository.Current.Get("/Controller/Messages/SomethingWrongMsg", "Something has been wrong, Please try again") };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.reResponse), DictionaryPhraseRepository.Current.Get("/Controller/Messages/CaptchaError", "Captcha is Invalid"));
                    result = new { status = "0", msg = "Invalid Captcha" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //ViewBag.Message(model.LMS_Response);

                //return View("/Views/AdaniCapital/RequestCallbackForm.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("Failed to Submit AdaniCapital RequestCallback Form : ", ex.ToString()), this);
                //return View("/Views/AdaniCapital/RequestCallbackForm.cshtml", model);
                result = new { status = "0", msg = "Something has been wrong, Please try again" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult BranchLocater()
        {
            AdaniCapitalBranchListModel m = new AdaniCapitalBranchListModel();
            m.StateList = new List<SelectListItem>();
            if (BranchDataObj.GetStates() != null)
            {
                foreach (string s in BranchDataObj.GetStates())
                {
                    m.StateList.Add(new SelectListItem() { Text = s.ToUpper(), Value = s });
                }
            }
            return View("/Views/AdaniCapital/Sublayouts/BranchLocator.cshtml", m);
        }

        [HttpGet]
        public ActionResult BranchLocation(string State, string City)
        {
            String regex = "^[a-zA-Z0-9_ ]*$";
            // Compile the ReGex
            if (!string.IsNullOrEmpty(State) & string.IsNullOrEmpty(City))
            {
                var ValidateState = Regex.Match(State, regex);
                if (ValidateState.Success)
                {
                    var CityList = BranchDataObj.GetCities(State);
                    if (CityList != null)
                    {
                        return Json(CityList, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (!string.IsNullOrEmpty(State) & !string.IsNullOrEmpty(City))
            {
                var ValidateState = Regex.Match(State, regex);
                var ValidateCity = Regex.Match(State, regex);
                if (ValidateState.Success && ValidateCity.Success)
                {
                    var AddressList = BranchDataObj.GetBranchLocation(State, City);
                    if (AddressList != null)
                    {
                        return Json(AddressList, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return null;
        }

        AdaniCapitalGetCCDetails CollCentreObj = new AdaniCapitalGetCCDetails();

        [HttpGet]
        public ActionResult CollectionCentreLocater()
        {
            AdaniCapitalCollectionCentreListModel m = new AdaniCapitalCollectionCentreListModel();
            m.StateList = new List<SelectListItem>();
            if (CollCentreObj.GetStates() != null)
            {
                foreach (string s in CollCentreObj.GetStates())
                {
                    m.StateList.Add(new SelectListItem() { Text = s, Value = s });
                }
            }
            return View("/Views/AdaniCapital/Sublayouts/CollectionCentreLocator.cshtml", m);
        }

        [HttpGet]
        public ActionResult CentreLocation(string State, string City)
        {
            String regex = "^[a-zA-Z0-9_ ]*$";
            // Compile the ReGex
            if (!string.IsNullOrEmpty(State) & string.IsNullOrEmpty(City))
            {
                var ValidateState = Regex.Match(State, regex);
                if (ValidateState.Success)
                {
                    var CityList = CollCentreObj.GetCities(State);
                    if (CityList != null)
                    {
                        return Json(CityList, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (!string.IsNullOrEmpty(State) & !string.IsNullOrEmpty(City))
            {
                var ValidateState = Regex.Match(State, regex);
                var ValidateCity = Regex.Match(State, regex);
                if (ValidateState.Success && ValidateCity.Success)
                {
                    var AddressList = CollCentreObj.GetCentreLocation(State, City);
                    if (AddressList != null)
                    {
                        return Json(AddressList, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return null;
        }

        [HttpGet]
        public ActionResult PincodeDetails(string pin)
        {
            // Regex to check qq1valid pin code of India. 
            String regex = "^[1-9]{1}[0-9]{2}\\s{0,1}[0-9]{3}$";
            // Compile the ReGex 
            var validatePincode = Regex.Match(pin, regex);
            if (validatePincode.Success)
            {
                var poDetails = POdetails(pin);
                if (poDetails != null)
                {
                    return Json(poDetails, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        public PincodeDataModel POdetails(string pincode)
        {
            AdaniCapitalHousingPincodeList pincodeInfo = dbContext.AdaniCapitalHousingPincodeLists.Where(x => x.Pincode == pincode).FirstOrDefault();
            if (pincodeInfo != null)
            {
                var model = new PincodeDataModel
                {
                    City = pincodeInfo.District,
                    State = pincodeInfo.State

                };
                //var details = new { State = pincodeInfo.State, City = pincodeInfo.District };
                return model;
            }
            else
            {
                return null;
            }
        }
        #region Login
        private LoginInfoModel CreateLoginInfo(string returnUrl = null)
        {
            return new LoginInfoModel
            {
                ReturnUrl = returnUrl,
                //LoginButtons = this.FedAuthLoginRepository.GetAll()
            };
        }

        public ActionResult LoginAdaniCapital(string returnUrl = null)
        {
            if (TempData["LoginModel"] != null)
            {
                UserSession.UserSessionContext = null;
                LoginInfoModel Rmodel = TempData["LoginModel"] as LoginInfoModel;
                if (!string.IsNullOrEmpty(Rmodel.Message))
                {
                    ModelState.AddModelError(nameof(Rmodel.MobileNoOrLoanAccountNumberValue), Rmodel.Message);
                }
                return base.View(Rmodel);
            }
            LoginInfoModel model = CreateLoginInfo(returnUrl);
            var currentVisiterIP = WebServices.GetIPAddress();
            if (WebServices.IsUserLoggedIn() && UserSession.UserSessionContext != null && UserSession.UserSessionContext.UserIP != null && currentVisiterIP != null && currentVisiterIP != UserSession.UserSessionContext.UserIP)
            {
                model.LoanAccountNumber = UserSession.UserSessionContext.LoanAccountNumber;
                model.MobileNo = UserSession.UserSessionContext.MobileNo;
            }
            else
            {
                UserSession.UserSessionContext = null;
                //model = TempData["LoginInfoModel"] as LoginInfoModel;
                //Session.Clear();
                //Session.Abandon();
                //Session.RemoveAll();
                //if (Request.Cookies["AuthToken"] != null)
                //{
                //    Response.Cookies["AuthToken"].Value = string.Empty;
                //    Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                //}
            }
            return base.View(model);
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdaniCapital(LoginInfoModel model, string sendOTP, string signIn, string ResendOTP)
        {
            Log.Info("Insert AdaniCapitalLogin", "Start");
            try
            {
                model.PageInfo = Request.RawUrl;
                if ((sendOTP != null && sendOTP == DictionaryPhraseRepository.Current.Get("/Controller/Login/sendOTP", "sendOTP")) || (ResendOTP != null && ResendOTP == DictionaryPhraseRepository.Current.Get("/Controller/Login/ResendOTP", "ResendOTP")))
                {
                    if (string.IsNullOrEmpty(model.MobileNoOrLoanAccountNumberValue))
                    {
                        this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), DictionaryPhraseRepository.Current.Get("/Controller/Messages/EmptyMobileOrLAN", "Please enter Mobile No or LAN"));
                        return this.View(model);
                    }
                    if (model.MobileNoOrLoanAccountNumber == DictionaryPhraseRepository.Current.Get("/Controller/Login/IsMobileNo", "Mobile No"))
                    {
                        model.MobileNo = model.MobileNoOrLoanAccountNumberValue.Trim();
                        String regexMobileNo = @"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[789]\d{9}|(\d[ -]?){10}\d$";
                        var ValidateMobile = Regex.Match(model.MobileNo, regexMobileNo);
                        if (!ValidateMobile.Success)
                        {
                            this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), DictionaryPhraseRepository.Current.Get("/Controller/Messages/InvalidMobileOrLAN", "Please enter Mobile No"));
                            return this.View(model);
                        }
                        var userDetails = WebServices.LMSGetLoans(model);
                        if (userDetails.IsAccountValid)
                        {
                            model.PageInfo = Request.RawUrl;
                            OTPModel oTPModel = WebServices.SendOTP(model.MobileNo, model.PageInfo);
                            if ((oTPModel == null || !(oTPModel.Status == DictionaryPhraseRepository.Current.Get("/WebAPIServices/SMS/SuccessResponse", "success")) ? true : !oTPModel.SavedinDB))
                            {
                                LoginInfoModel newModel = new LoginInfoModel();
                                newModel.IsOTPSend = false;
                                newModel.Message = oTPModel.Message;
                                newModel.MobileNoOrLoanAccountNumber = model.MobileNoOrLoanAccountNumber;
                                newModel.MobileNoOrLoanAccountNumberValue = model.MobileNoOrLoanAccountNumberValue;
                                TempData["LoginModel"] = newModel;
                                return Redirect(Request.RawUrl);
                                //return View(model);
                            }
                            else
                            {
                                model.IsOTPSend = true;
                                base.ModelState.AddModelError("OTP", DictionaryPhraseRepository.Current.Get("/Controller/Messages/OTP sent", "OTP has been sent to your mobile no."));
                                return View(model);
                            }
                            //return LoginAdaniCapital(model, redirectUrl => new RedirectResult(redirectUrl));
                        }
                        else
                        {
                            string errormsg = !string.IsNullOrEmpty(model.Message) ? model.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/SystemError", "Something has been wrong, Please try again later");
                            this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), errormsg);
                            return this.View(model);
                        }
                    }
                    else if (model.MobileNoOrLoanAccountNumber == DictionaryPhraseRepository.Current.Get("/Controller/Login/IsLAN", "Loan Account Number"))
                    {
                        model.LoanAccountNumber = model.MobileNoOrLoanAccountNumberValue.Trim();
                        String regexLAN = @"^[a-zA-Z0-9_]*$";
                        var ValidateLAN = Regex.Match(model.LoanAccountNumber, regexLAN);
                        if (!ValidateLAN.Success)
                        {
                            this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), DictionaryPhraseRepository.Current.Get("/Controller/Messages/InvalidLAN", "Please enter valid LAN"));
                            return this.View(model);
                        }
                        var userDetails = WebServices.LMSGet_Loan_Details(model);
                        if (userDetails.IsAccountValid)
                        {
                            OTPModel oTPModel1 = this.WebServices.SendOTP(model.MobileNo, model.PageInfo);
                            if ((oTPModel1 == null || !(oTPModel1.Status == DictionaryPhraseRepository.Current.Get("/WebAPIServices/SMS/SuccessResponse", "success")) ? true : !oTPModel1.SavedinDB))
                            {
                                string errormsg = !string.IsNullOrEmpty(oTPModel1.Message) ? oTPModel1.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/OTPsendFailed", "Unable to send OTP, Please try again later");
                                this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), errormsg);
                                return View(model);
                            }
                            else
                            {
                                model.IsOTPSend = true;
                                base.ModelState.AddModelError("OTP", DictionaryPhraseRepository.Current.Get("/Controller/Messages/OTP sent2", "OTP has been sent to your associated mobile no."));
                                return View(model);
                            }
                            //return LoginAdaniCapital(model, redirectUrl => new RedirectResult(redirectUrl));
                        }
                        else
                        {
                            string errormsg = !string.IsNullOrEmpty(model.Message) ? model.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/SystemError", "Something has been wrong, Please try again later");
                            this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), errormsg);
                            return this.View(model);
                        }
                    }
                    else
                    {
                        string errormsg = !string.IsNullOrEmpty(model.Message) ? model.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/SystemError", "Something has been wrong, Please try again later");
                        this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), errormsg);
                        return this.View(model);
                    }
                }
                else if (signIn != null && signIn == DictionaryPhraseRepository.Current.Get("/Controller/Login/signIn", "signIn"))
                {
                    string OTPValidation = DictionaryPhraseRepository.Current.Get("/Controller/Login/OtpRegexValidation", @"^\d{6}$");
                    Regex OTPregex = new Regex(OTPValidation);
                    if (string.IsNullOrEmpty(model.MobileNoOrLoanAccountNumberValue))
                    {
                        this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), DictionaryPhraseRepository.Current.Get("/Controller/Messages/EmptyMobileOrLAN", "Please enter Mobile No or LAN"));
                        return this.View(model);
                    }
                    if (string.IsNullOrEmpty(model.OTP))
                    {
                        ModelState.AddModelError("MobileNoOrLoanAccountNumberValue", DictionaryPhraseRepository.Current.Get("/Controller/Messages/EmptyOTP", "Please enter OTP."));
                        model.IsOTPSend = true;
                        return View(model);
                    }
                    if (model.MobileNoOrLoanAccountNumber == DictionaryPhraseRepository.Current.Get("/Controller/Login/IsMobileNo", "Mobile No"))
                    {
                        model.MobileNo = model.MobileNoOrLoanAccountNumberValue.Trim(); ;
                        var userDetails = WebServices.LMSGetLoans(model);
                        if (userDetails.IsAccountValid)
                        {
                            if (!string.IsNullOrEmpty(model.OTP) && OTPregex.IsMatch(model.OTP))
                            {
                                var validateResp = WebServices.ValidateOTP(model.MobileNo, model.OTP, model.PageInfo ?? "adaniCapital");
                                if (validateResp.IsValidAttemptExceeded)
                                {
                                    LoginInfoModel newModel = new LoginInfoModel();
                                    newModel.IsOTPSend = false;
                                    newModel.Message = !string.IsNullOrEmpty(validateResp.Message) ? validateResp.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/OTP valid attempt", "You have exceeded attempts for OTP validate.");
                                    newModel.MobileNoOrLoanAccountNumber = model.MobileNoOrLoanAccountNumber;
                                    newModel.MobileNoOrLoanAccountNumberValue = model.MobileNoOrLoanAccountNumberValue;
                                    TempData["LoginModel"] = newModel;
                                    return Redirect(Request.RawUrl);
                                }
                                if (validateResp.IsOTPvalid)
                                {
                                    return LoginAdaniCapital(model, redirectUrl => new RedirectResult(redirectUrl));
                                }
                                else
                                {
                                    model.IsOTPSend = true;
                                    ModelState.AddModelError("OTP", DictionaryPhraseRepository.Current.Get("/Controller/Messages/OTP Error", "Invalid OTP"));
                                    return View(model);
                                }
                            }
                            else
                            {
                                model.IsOTPSend = true;
                                ModelState.AddModelError("OTP", DictionaryPhraseRepository.Current.Get("/Controller/Messages/OTP Error", "Invalid OTP"));
                                return View(model);
                            }
                            //return LoginAdaniCapital(model, redirectUrl => new RedirectResult(redirectUrl));
                        }
                        else
                        {
                            string errormsg = !string.IsNullOrEmpty(model.Message) ? model.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/SystemError", "Something has been wrong, Please try again later");
                            this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), errormsg);
                            return this.View(model);
                        }
                    }
                    else if (model.MobileNoOrLoanAccountNumber == DictionaryPhraseRepository.Current.Get("/Controller/Login/IsLAN", "Loan Account Number"))
                    {
                        model.LoanAccountNumber = model.MobileNoOrLoanAccountNumberValue;
                        var userDetails = WebServices.LMSGet_Loan_Details(model);
                        if (userDetails.IsAccountValid)
                        {
                            if (!string.IsNullOrEmpty(model.OTP) && OTPregex.IsMatch(model.OTP))
                            {
                                var validateResp = WebServices.ValidateOTP(model.MobileNo, model.OTP, model.PageInfo ?? "adaniCapital");
                                if (validateResp.IsValidAttemptExceeded)
                                {
                                    LoginInfoModel newModel = new LoginInfoModel();
                                    newModel.IsOTPSend = false;
                                    newModel.Message = !string.IsNullOrEmpty(validateResp.Message) ? validateResp.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/OTP valid attempt", "You have exceeded attempts for OTP validate.");
                                    newModel.MobileNoOrLoanAccountNumber = model.MobileNoOrLoanAccountNumber;
                                    newModel.MobileNoOrLoanAccountNumberValue = model.MobileNoOrLoanAccountNumberValue;
                                    TempData["LoginModel"] = newModel;
                                    return Redirect(Request.RawUrl);
                                }
                                if (validateResp.IsOTPvalid)
                                {
                                    return LoginAdaniCapital(model, redirectUrl => new RedirectResult(redirectUrl));
                                }
                                else
                                {
                                    model.IsOTPSend = true;
                                    ModelState.AddModelError("OTP", DictionaryPhraseRepository.Current.Get("/Controller/Messages/OTP Error", "Invalid OTP"));
                                    return View(model);
                                }
                            }
                            else
                            {
                                model.IsOTPSend = true;
                                ModelState.AddModelError("OTP", DictionaryPhraseRepository.Current.Get("/Controller/Messages/OTP Error", "Invalid OTP"));
                                return View(model);
                            }
                        }
                        else
                        {
                            string errormsg = !string.IsNullOrEmpty(model.Message) ? model.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/SystemError", "Something has been wrong, Please try again later");
                            this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), errormsg);
                            return this.View(model);
                        }
                    }
                    else
                    {
                        string errormsg = !string.IsNullOrEmpty(model.Message) ? model.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/SystemError", "Something has been wrong, Please try again later");
                        this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), errormsg);
                        return this.View(model);
                    }
                }
                else
                {
                    string errormsg = !string.IsNullOrEmpty(model.Message) ? model.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/SystemError", "Something has been wrong, Please try again later");
                    this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), errormsg);
                    return this.View(model);
                }
            }
            catch (Exception e)
            {
                Log.Error("Error in LoginAdaniCapital - GetLoans Adani Capital LMS login :" + model.MobileNoOrLoanAccountNumberValue + e.Message, this);
                string errormsg = !string.IsNullOrEmpty(model.Message) ? model.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/SystemError", "Something has been wrong, Please try again later");
                this.ModelState.AddModelError(nameof(model.MobileNoOrLoanAccountNumberValue), errormsg);
                return this.View(model);
            }
        }
        protected virtual ActionResult LoginAdaniCapital(LoginInfoModel loginInfo, Func<string, ActionResult> redirectAction)
        {
            try
            {

                //LoginInfoModel loginData = WebServices.LMSGetLoans(loginInfo);
                //if (loginData.IsAccountValid == false) // unauthorized
                //{
                //    string errormsg = !string.IsNullOrEmpty(loginInfo.Message) ? loginInfo.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/User Not Valid", loginInfo.MobileNoOrLoanAccountNumber + " is not valid.");
                //    this.ModelState.AddModelError(nameof(loginInfo.MobileNoOrLoanAccountNumberValue), errormsg);
                //    return this.View(loginInfo);
                //}
                //else
                //{
                OTPModel m = new OTPModel();
                if (string.IsNullOrEmpty(loginInfo.MobileNoOrLoanAccountNumberValue))
                {
                    this.ModelState.AddModelError(nameof(loginInfo.MobileNoOrLoanAccountNumberValue), DictionaryPhraseRepository.Current.Get("/Controller/Messages/User Not Valid", "Mobile No or LAN is not valid."));
                    return this.View(loginInfo);
                }
                else
                {
                    string guid = Guid.NewGuid().ToString();
                    //Creating second session for the same user and assigning a randmon GUID

                    Helper.UserSession.UserSessionContext = new UserLoginModel
                    {
                        UserIP = WebServices.GetIPAddress(),
                        IsLoggedIn = true,
                        LoginName = loginInfo.getLoansList.Select(x => x.customerName).FirstOrDefault(),
                        LoginUser = loginInfo.MobileNoOrLoanAccountNumberValue,
                        LoanAccountNumber = loginInfo.LoanAccountNumber ?? loginInfo.getLoansList.Select(x => x.loanDetailsList.Select(y => y.loanAccountNumber).FirstOrDefault()).FirstOrDefault(),
                        MobileNo = loginInfo.MobileNo,
                        OTP = loginInfo.OTP,
                        AuthToken = guid
                    };
                    Log.Info("User Logged in Adani Capital Login with IP: " + WebServices.GetIPAddress() + ", MobileorLAN Value: " + loginInfo.MobileNoOrLoanAccountNumberValue + ", Mobile No: " + loginInfo.MobileNo + ", OTP: " + loginInfo.OTP + ", Name: " + loginInfo.getLoansList.Select(x => x.customerName).FirstOrDefault() + ", AuthToken: " + guid.ToString(), this);
                    UserLoginSessionModel sessionObj = new UserLoginSessionModel();
                    sessionObj.UserIP = WebServices.GetIPAddress();
                    sessionObj.MobileNo = loginInfo.MobileNoOrLoanAccountNumberValue;
                    sessionObj.SessionId = guid;
                    WebServices.StoreCurrentSession();
                    dbServices.StoreUserLoginSession(sessionObj);
                    Response.Cookies.Add(new System.Web.HttpCookie("AuthToken", guid));
                }
                //}

                string redirectUrl = loginInfo.ReturnUrl;
                if (string.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = GetRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Authenticated);
                }

                return redirectAction(redirectUrl);
            }
            catch (Exception ex)
            {
                Log.Error("LoginAdaniCapital Method Error - for User - " + loginInfo.MobileNoOrLoanAccountNumber + ":" + loginInfo.MobileNoOrLoanAccountNumberValue, ex.Message);
                ModelState.AddModelError(nameof(loginInfo.MobileNoOrLoanAccountNumberValue), ex.Message);
                return View(loginInfo);
            }
        }
        #endregion
        #region Logout
        [HttpPost]
        public ActionResult LogoutAdaniCapital()
        {
            string redirectUrl = this.GetRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Unauthenticated, null);
            UserSession.UserSessionContext = null;
            base.Session["UpdateMessage"] = null;
            base.Session.Clear();
            base.Session.Abandon();
            base.Session.RemoveAll();
            if (base.Request.Cookies["AuthToken"] != null)
            {
                base.Response.Cookies["AuthToken"].Value = string.Empty;
                base.Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            return this.Redirect(redirectUrl);
        }
        public void ClearedSession()
        {
            UserSession.UserSessionContext = null;
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
        #endregion
        [RedirectUnAuthenticatedAdaniCapital]
        public ActionResult LoginNavigation()
        {
            LoginInfoModel loginInfo = new LoginInfoModel();
            try
            {
                loginInfo.MobileNoOrLoanAccountNumberValue = (UserSession.UserSessionContext.MobileNo);
                loginInfo.LoanAccountNumber = UserSession.UserSessionContext.LoanAccountNumber;
                loginInfo.MobileNo = UserSession.UserSessionContext.MobileNo;
                loginInfo.MobileNoOrLoanAccountNumber = UserSession.UserSessionContext.LoginUser;
                LoginInfoModel model = WebServices.LMSGetLoans(loginInfo);
                LoginInfoModel updatedmodel = WebServices.UpdateAssocitedLoanList(loginInfo);
                LoginInfoModel loanRecord = WebServices.LMSGet_UserIdentityDetails(loginInfo);
                loginInfo.LastLoginInfo = WebServices.GetLastLoginDateTime();
            }
            catch (Exception ex)
            {
                Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return base.View(loginInfo);
        }
        
        [RedirectUnAuthenticatedAdaniCapital]
        public ActionResult UserDashboard()
        {
            LoginInfoModel loginInfo = new LoginInfoModel();
            loginInfo.LastLoginInfo = WebServices.GetLastLoginDateTime();
            try
            {
                loginInfo.MobileNoOrLoanAccountNumberValue = (UserSession.UserSessionContext.MobileNo);
                loginInfo.LoanAccountNumber = UserSession.UserSessionContext.LoanAccountNumber;
                loginInfo.MobileNo = UserSession.UserSessionContext.MobileNo;
                loginInfo.MobileNoOrLoanAccountNumber = UserSession.UserSessionContext.LoginUser;
                LoginInfoModel model = WebServices.LMSGetLoans(loginInfo);
                LoginInfoModel updatedmodel = WebServices.UpdateAssocitedLoanList(loginInfo);
                LoginInfoModel loanRecord = WebServices.LMSGet_UserIdentityDetails(loginInfo);

            }
            catch (Exception ex)
            {
                Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return base.View(loginInfo);
        }
        //[HttpGet]
        //[RedirectUnAuthenticatedAdaniCapital]
        //public JsonResult Get_SOA_Report(string LAN)
        //{
        //    ReportsModel report = new ReportsModel();
        //    try
        //    {
        //        //if (Request.IsAuthenticated)
        //        //{
        //            report = WebServices.LMSGenerate_SOA_Report(LAN);
        //        //}                    
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
        //    }
        //    return Json(report, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        [RedirectUnAuthenticatedAdaniCapital]
        public JsonResult Get_InterestCertificate(string LAN)
        {
            ReportsModel report = new ReportsModel();
            try
            {
                //if (Request.IsAuthenticated)
                //{
                report = WebServices.LMSGenerateInterestCertificate(LAN);
                //}                    
            }
            catch (Exception ex)
            {
                Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return Json(report, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [RedirectUnAuthenticatedAdaniCapital]
        public JsonResult Get_BalConfLetter(string LAN)
        {
            ReportsModel report = new ReportsModel();
            try
            {
                //if (Request.IsAuthenticated)
                //{
                report = WebServices.LMSGetBalnaceConfirmLetter(LAN);
                //}                    
            }
            catch (Exception ex)
            {
                Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return Json(report, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [RedirectUnAuthenticatedAdaniCapital]
        public JsonResult Get_WelcomeLetter(string LAN)
        {
            ReportsModel report = new ReportsModel();
            try
            {
                //if (Request.IsAuthenticated)
                //{
                report = WebServices.LMSGetWelcomeLetter(LAN);
                //}                    
            }
            catch (Exception ex)
            {
                Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return Json(report, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [RedirectUnAuthenticatedAdaniCapital]
        public JsonResult Get_RepaymentSchedule(string LAN)
        {
            ReportsModel report = new ReportsModel();
            try
            {
                //if (Request.IsAuthenticated)
                //{
                report = WebServices.LMSGenerateRepaymentSchedule(LAN);
                //}                    
            }
            catch (Exception ex)
            {
                Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        //Post login customer support ticket in freshdesk
        [RedirectUnAuthenticatedAdaniCapital]
        public ActionResult AfterLoginSupportForm()
        {
            LoginInfoModel loginInfoModel = new LoginInfoModel();
            try
            {
                loginInfoModel.MobileNoOrLoanAccountNumberValue = UserSession.UserSessionContext.MobileNo;
                loginInfoModel.LoanAccountNumber = UserSession.UserSessionContext.LoanAccountNumber;
                loginInfoModel.MobileNo = UserSession.UserSessionContext.MobileNo;
                loginInfoModel.MobileNoOrLoanAccountNumber = UserSession.UserSessionContext.LoginUser;
                this.WebServices.LMSGetLoans(loginInfoModel);
                this.WebServices.UpdateAssocitedLoanList(loginInfoModel);
                this.WebServices.LMSGet_UserIdentityDetails(loginInfoModel);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("Error at ", MethodBase.GetCurrentMethod().Name, ":", exception.Message), this);
            }
            FreshdeskCreateTicketModel freshdeskCreateTicketModel = new FreshdeskCreateTicketModel()
            {

                MobileNo = loginInfoModel.MobileNo,
                Name = (
                    from x in loginInfoModel.getLoansList
                    select x.customerName).FirstOrDefault<string>()
            };
            Item SubjectItemList = db.GetItem(Templates.AfterLoginSupportSubject.SubjectListItemID);
            if (SubjectItemList != null && SubjectItemList.HasChildren)
            {
                freshdeskCreateTicketModel.SubjectList = SubjectItemList.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value ?? "",
                    Value = x.Fields["Value"].Value ?? ""
                }).ToList();
            }
            if (loginInfoModel.AssociatedLANList != null)
            {
                freshdeskCreateTicketModel.LoanAccountNoList = loginInfoModel.AssociatedLANList;
            }
            return base.View(freshdeskCreateTicketModel);
        }

        [HttpPost]
        [RedirectUnAuthenticatedAdaniCapital]
        [ValidateRenderingId]
        public ActionResult AfterLoginSupportForm(FreshdeskCreateTicketModel model)
        {
            string str;
            bool flag = false;
            Log.Info("Insert AfterLoginSupportForm", "Start");
            Item SubjectItemList = db.GetItem(Templates.AfterLoginSupportSubject.SubjectListItemID);
            if (SubjectItemList != null && SubjectItemList.HasChildren)
            {
                model.SubjectList = SubjectItemList.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value ?? "",
                    Value = x.Fields["Value"].Value ?? ""
                }).ToList();
            }
            LoginInfoModel loginInfoModel = new LoginInfoModel();
            loginInfoModel.LastLoginInfo = WebServices.GetLastLoginDateTime();
            try
            {
                if (!string.IsNullOrEmpty(model.MobileNo))
                {
                    loginInfoModel.MobileNo = model.MobileNo;
                    WebServices.LMSGetLoans(loginInfoModel);
                    WebServices.UpdateAssocitedLoanList(loginInfoModel);
                    if (loginInfoModel.AssociatedLANList != null)
                    {
                        model.LoanAccountNoList = loginInfoModel.AssociatedLANList;
                    }
                }
                else
                {
                    ViewBag.result = DictionaryPhraseRepository.Current.Get("/Controller/Messages/FreshdeskError", "Invalid Mobile No");
                    ViewBag.Error = "show";
                    return View(model);
                }
            }
            catch (Exception e)
            {
                Log.Error("Error in Adani Capital After Login support form mobile no:" + e.Message, this);
                ViewBag.result = DictionaryPhraseRepository.Current.Get("/Controller/Messages/SomethingWrongMsg", "Something has been wrong, Please try again");
                ViewBag.Error = "show";
                return View(model);
            }
            try
            {
                try
                {
                    flag = this.IsReCaptchValid(model.reResponse);
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    Log.Info(string.Concat("AfterLoginSupportForm Failed to validate auto script : ", exception.ToString()), this);
                    str = (!string.IsNullOrEmpty(model.Message) ? model.Message : DictionaryPhraseRepository.Current.Get("/Controller/Messages/SystemError", "Something has been wrong, Please try again later"));
                    base.ModelState.AddModelError("reResponse", str);
                    ViewBag.Error = "show";
                    return View(model);
                }
                if (flag)
                {
                    base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/Controller/Messages/CaptchaError", "Captcha is Invalid"));
                    ViewBag.Error = "show";
                    return View(model);
                }
                else if (base.ModelState.IsValid)
                {
                    FreshdeskCreateTicketModel freshdeskCreateTicketModel = this.WebServices.FreshDeskCreateTicket(model);
                    if (string.IsNullOrEmpty(freshdeskCreateTicketModel.TicketID))
                    {
                        ViewBag.result = DictionaryPhraseRepository.Current.Get("/Controller/Messages/FreshdeskError", "Your request is not successful. You can mail us at customercare.ahfpl@adani.com");
                        ViewBag.Error = "show";
                        return View(model);
                    }
                    else
                    {
                        model.TicketID = freshdeskCreateTicketModel.TicketID;
                        model.IsSubmittedToFreshdesk = freshdeskCreateTicketModel.IsSubmittedToFreshdesk;
                        model.Message = "\"" + model.Message + "\"";
                        this.dbServices.StoreAfterLoginSupportFormData(freshdeskCreateTicketModel);
                        if (!freshdeskCreateTicketModel.SavedinDB)
                        {
                            ViewBag.result = DictionaryPhraseRepository.Current.Get("/Controller/Messages/SomethingWrongMsg", "Something has been wrong, Please try again");
                            ViewBag.Error = "show";
                            return View(model);
                        }
                        else
                        {
                            Session["result"] = DictionaryPhraseRepository.Current.Get("/Controller/Messages/RequestCallbackSuccessMsg", string.Concat("Thanks for reaching, we will call you back shortly. Kindly note your ticket No: ", model.TicketID));
                            ViewBag.result = Session["result"].ToString();
                            ViewBag.Error = "show";
                            return Redirect(Request.RawUrl);
                        }
                    }
                }
                else
                {
                    Log.Info("Failed to Submit AfterLoginSupportForm as Invalid values entered", this);
                    ViewBag.result = DictionaryPhraseRepository.Current.Get("/Controller/Messages/FreshdeskError", "Your request is not successful. You can mail us at customercare.ahfpl@adani.com");
                    ViewBag.Error = "show";
                    return View(model);
                }
            }
            catch (Exception exception3)
            {
                Exception exception2 = exception3;
                Log.Error(string.Concat("Failed to Submit AdaniCapital RequestCallback Form : ", exception2.ToString()), this);
                ViewBag.result = DictionaryPhraseRepository.Current.Get("/Controller/Messages/SomethingWrongMsg", "Something has been wrong, Please try again");
                ViewBag.Error = "show";
                return View(model);
            }
        }


        [RedirectUnAuthenticatedAdaniCapital]
        public ActionResult TicketHistory()
        {
            FreshdeskCreateTicketModel model = new FreshdeskCreateTicketModel();
            return View(model);
        }
        [RedirectUnAuthenticatedAdaniCapital]
        public ActionResult AfterLoginAdaniCapitalApplyForLoan()
        {
            AdaniCapitalApplyforLoanModel adaniCapitalApplyforLoanModel = new AdaniCapitalApplyforLoanModel();
            this.db.GetItem(Templates.ApplyForLoanDropdown.ProductListItem);
            return base.View("/Views/AdaniCapital/AfterLoginApplyForLoanForm.cshtml", adaniCapitalApplyforLoanModel);
        }

        [HttpPost]
        [RedirectUnAuthenticatedAdaniCapital]
        [ValidateAntiForgeryToken]
        [ValidateRenderingId]
        public ActionResult AfterLoginAdaniCapitalApplyForLoan(AdaniCapitalApplyforLoanModel model, string Submit)
        {
            bool flag = true;
            Item item = this.db.GetItem(Templates.ApplyForLoanDropdown.ProductListItem);
            if (item != null)
            {
                model.ProductsList = (
                    from x in item.GetChildren().ToList<Item>()
                    select new SelectListItem()
                    {
                        Text = x.Fields["Text"].Value ?? "",
                        Value = x.Fields["Value"].Value ?? ""
                    }).ToList<SelectListItem>();
            }
            Log.Info("Insert AdaniCapitalApplyForLoanForm", "Start");
            try
            {
                if (string.IsNullOrEmpty(Submit))
                {
                    ViewBag.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/SomethingWrongMsg", "Something has been wrong, Please try again");
                    return View("/Views/AdaniCapital/ApplyForLoanForm.cshtml", model);
                }
                else
                {
                    base.Session["ApplyLoanTicketStatus"] = null;
                    base.Session["ApplyTicketNo"] = null;
                    try
                    {
                        //flag = this.IsReCaptchValid(model.reResponse);
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        Log.Error(string.Concat("AdaniCapital Contact Us Failed to validate auto script : ", exception.ToString()), this);
                        return View("/Views/AdaniCapital/AfterLoginApplyForLoanForm.cshtml", model);
                    }
                    if (!flag)
                    {
                        base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/Controller/Messages/CaptchaError", "Captcha is Invalid"));
                        return View("/Views/AdaniCapital/AfterLoginApplyForLoanForm.cshtml", model);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.PinCode))
                        {
                            if (this.POdetails(model.PinCode) == null)
                            {
                                base.ModelState.AddModelError("PinCode", DictionaryPhraseRepository.Current.Get("/Controller/Messages/PincodeNotFound", "Pincode not found."));
                                return View("/Views/AdaniCapital/AfterLoginApplyForLoanForm.cshtml", model);
                            }
                        }
                        if (base.ModelState.IsValid)
                        {
                            AdaniCapitalApplyforLoanModel adaniCapitalApplyforLoanModel = this.WebServices.ApplyForLoanCreateEnquiry(model);
                            if (string.IsNullOrEmpty(adaniCapitalApplyforLoanModel.LMS_Response))
                            {
                                ViewBag.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/SomethingWrongMsg", "Something has been wrong, Please try again");
                                base.Session["ApplyLoanTicketStatus"] = false;
                                return View("/Views/AdaniCapital/AfterLoginApplyForLoanForm.cshtml", model);
                            }
                            else
                            {
                                if (adaniCapitalApplyforLoanModel.LMS_Response != "Lead with specified mobile already exists.")
                                {
                                    this.dbServices.StoreApplyForLoanData(adaniCapitalApplyforLoanModel);
                                }
                                base.Session["ApplyLoanTicketStatus"] = true;
                                base.Session["ApplyTicketNo"] = adaniCapitalApplyforLoanModel.LMS_Response;
                                base.Session["Key"] = false;
                                return View("/Views/AdaniCapital/AfterLoginApplyForLoanThankyou.cshtml");
                            }
                        }
                        else
                        {
                            Log.Info("Failed to Submit AdaniCapital contactUs form as Invalid values entered", this);
                            return View("/Views/AdaniCapital/AfterLoginApplyForLoanForm.cshtml", model);
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                Exception exception2 = exception3;
                Log.Error(string.Concat("Failed to Submit AdaniCapital ApplyForLoan Form : ", exception2.ToString()), this);
                ViewBag.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/SomethingWrongMsg", "Something has been wrong, Please try again");
                base.Session["ApplyLoanTicketStatus"] = false;
                return View("/Views/AdaniCapital/ApplyForLoanForm.cshtml", model);
            }
        }
        [RedirectUnAuthenticatedAdaniCapital]
        public ActionResult UserProfile()
        {
            LoginInfoModel loginInfoModel = new LoginInfoModel();
            loginInfoModel.LastLoginInfo = WebServices.GetLastLoginDateTime();
            TransactionsModel transactionsModel = new TransactionsModel();
            try
            {
                loginInfoModel.MobileNoOrLoanAccountNumberValue = UserSession.UserSessionContext.MobileNo;
                loginInfoModel.LoanAccountNumber = UserSession.UserSessionContext.LoanAccountNumber;
                loginInfoModel.MobileNo = UserSession.UserSessionContext.MobileNo;
                loginInfoModel.MobileNoOrLoanAccountNumber = UserSession.UserSessionContext.LoginUser;
                this.WebServices.LMSGetLoans(loginInfoModel);
                this.WebServices.UpdateAssocitedLoanList(loginInfoModel);
                this.WebServices.LMSGet_UserIdentityDetails(loginInfoModel);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("Error at ", MethodBase.GetCurrentMethod().Name, ":", exception.Message), this);
            }
            return base.View(loginInfoModel);
        }

        [RedirectUnAuthenticatedAdaniCapital]
        public ActionResult LoanDetails()
        {
            LoginInfoModel loginInfo = new LoginInfoModel();
            loginInfo.LastLoginInfo = WebServices.GetLastLoginDateTime();
            try
            {
                loginInfo.MobileNoOrLoanAccountNumberValue = UserSession.UserSessionContext.MobileNo;
                loginInfo.LoanAccountNumber = UserSession.UserSessionContext.LoanAccountNumber;
                loginInfo.MobileNo = UserSession.UserSessionContext.MobileNo;
                loginInfo.MobileNoOrLoanAccountNumber = UserSession.UserSessionContext.LoginUser;
                this.WebServices.LMSGetLoans(loginInfo);
                this.WebServices.UpdateAssocitedLoanList(loginInfo);
                this.WebServices.LMSGet_UserIdentityDetails(loginInfo);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Error at ", MethodBase.GetCurrentMethod().Name, ":", ex.Message), this);
            }
            return base.View(loginInfo);
        }

        [RedirectUnAuthenticatedAdaniCapital]
        public ActionResult LoanSummary(string lan)
        {
            LoginInfoModel loginInfo = new LoginInfoModel();
            loginInfo.LastLoginInfo = WebServices.GetLastLoginDateTime();
            TransactionsModel transactions = new TransactionsModel();
            try
            {
                loginInfo.MobileNoOrLoanAccountNumberValue = UserSession.UserSessionContext.MobileNo;
                if (lan != null)
                {
                    transactions.LoanAccountNumber = lan.ToString();
                }
                loginInfo.MobileNo = UserSession.UserSessionContext.MobileNo;
                loginInfo.MobileNoOrLoanAccountNumber = UserSession.UserSessionContext.LoginUser;
                this.WebServices.LMSGet_Transactions(transactions);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Error at ", MethodBase.GetCurrentMethod().Name, ":", ex.Message), this);
            }
            return base.View(transactions);
        }
    }

}