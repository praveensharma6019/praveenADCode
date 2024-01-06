using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Defence.Website;
using Sitecore.Defence.Website.Models;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using Sitecore.Defence.Website.Helper;
using System.Web;

namespace Sitecore.Defence.Website.Controllers
{
    public class DefenceController : Controller
    {
        public DefenceController()
        {
        }

        public ActionResult Index()
        {
            return base.View();
        }
        [HttpGet]
        public ActionResult InsertContactFormdetail()
        {
            return base.View("/Views/Defence/Sublayouts/DefenceContactForm.cshtml", new DefenceContactModel());
        }

        [HttpPost]
        public ActionResult InsertContactFormdetail(DefenceContactModel m, string SubmitApplication)
        {
            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert DefenceForm", "Start");
            bool flag = false;
            try
            {
             // flag = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                result = new { status = "2" };
                Log.Error(string.Concat("Failed to validate auto script : ", ex.ToString()), "Failed");
            }
          
           if(flag==true)
            {
                Log.Error("InsertDefenceContact", "Start");
                string getEmailTo = "";
                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        if (ModelState.IsValid)
                        {

                            DefenceContactUsFormDataContext rdb1 = new DefenceContactUsFormDataContext();
                            DefenceContactFormRecord r1 = new DefenceContactFormRecord();

                            r1.Name = m.Name;
                            r1.Email = m.EmailID;
                            r1.Mobile = m.MobileNo;
                            r1.MessageType = m.MessageType;
                            r1.Message = m.Message;
                            r1.FormType = m.FormName;
                            r1.PageInfo = m.PageInfo;
                            r1.FormSubmitOn = new DateTime?(DateTime.Now);

                            if (m.ResumeAttachment != null)
                            {

                                HttpPostedFileBase resumeAttachment = m.ResumeAttachment;
                                string fileName = resumeAttachment.FileName;
                                resumeAttachment.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/AdaniDefenceCareersResume/Resume/", m.Name, "-", m.MobileNo, "-", fileName })));
                                r1.ResumeAttachment = string.Concat(new string[] { "/AdaniDefenceCareersResume/Resume/", m.Name, "-", m.MobileNo, "-", fileName });


                            }
                            rdb1.DefenceContactFormRecords.InsertOnSubmit(r1);
                            rdb1.SubmitChanges();
                            Log.Info("Form SportsLine Contact Form data saved into db successfully: ", this);
                        }
                        else
                        {
                            actionResult = base.View("/Views/Defence/Sublayouts/DefenceContactForm.cshtml", m);
                            return actionResult;
                        }
                    
                    }
                }
                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Contact Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/Defence/Sublayouts/DefenceContactForm.cshtml", m);
                }
                try
                {
                    var msgTpye = Sitecore.Context.Database.GetItem("{DC57D9CE-13D1-4D9D-A5C8-F980B14ABEE9}");
                    var getfilteredItem = msgTpye.Children.Where(x => x.Fields["SubjectName"].Value.ToLower() == m.MessageType.ToLower());

                    foreach (var itemData in getfilteredItem.ToList())
                    {
                        getEmailTo = itemData.Fields["EmailTo"].Value;
                    }
                }

                catch (Exception exception2)
                {
                    Exception ex = exception2;
                    result = new { status = "1" };
                    Log.Error("Failed to get subject specific Email", ex.ToString());
                }
                try
                {
                    if (m.ResumeAttachment == null)
                    {

                        string emailText = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailText", "");
                        string message = "";
                        message = string.Concat("Hello<br><br>", emailText, "<br><br>");
                        message = string.Concat(new string[] { message, "<br>Name: ", m.Name, "<br>Email-Id: ", m.EmailID, "<br>Subject of Message: ", m.MessageType, "<br>Contact Number: ", m.MobileNo, "<br>Message: ", m.Message, "<br><br>Thanks" });
                        string from = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailFrom", "");
                        if (this.sendEmail(getEmailTo, DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailSubject", ""), message, from))
                        {
                            Log.Error("Email Sent- ", "");
                        }
                    }
                    else
                    {
                        HttpPostedFileBase resumeAttachment = m.ResumeAttachment;
                        string fileName = resumeAttachment.FileName;
                        string str5 = string.Concat(new string[] { "<a href=\"https://adaniproduction-cd.azurewebsites.net/AdaniDefenceCareersResume/Resume/", m.Name, "-", m.MobileNo, "-", fileName, "\"", " Download>Click to Download </a>" });
                        string emailText = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailText", "");
                        string message = "";
                        message = string.Concat("Hello<br><br>", emailText, "<br><br>");
                        message = string.Concat(new string[] { message, "<br>Name: ", m.Name, "<br>Email-Id: ", m.EmailID, "<br>Subject of Message: ", m.MessageType, "<br>Contact Number: ", m.MobileNo, "<br>Message: ", m.Message, "<br>  <br> Resume: ", str5, "<br><br> Thanks" });
                        string from = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailFrom", "");
                        if (this.sendEmail(getEmailTo, DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailSubject", ""), message, from))
                        {
                            Log.Error("Email Sent- ", "");
                        }
                    }

                }
                catch (Exception exception3)
                {
                    Exception ex = exception3;
                    result = new { status = "1" };
                    Log.Error("Failed to sent Email", ex.ToString());
                }
            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/Defence/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/Defence/Sublayouts/DefenceContactForm.cshtml", m);
            }



            return this.Redirect("/thankyou");
        }
    

        [HttpPost]
        public ActionResult InsertSubscribeUsFormdetail(DefenceContactModel m)
        {
            Log.Error("InsertSubscribeUsFormdetail", "Start");
            bool validationStatus = true;
            var result = new { status = "1" };
            try
            {
                validationStatus = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                result = new { status = "2" };
                Log.Error(string.Concat("Failed to validate auto script : ", ex.ToString()), "Failed");
            }
            if (!validationStatus)
            {
                result = new { status = "2" };
            }
            else
            {
                Log.Error("InsertDefenceSubscribeUsForm", "Start");
                try
                {
                    DefenceContactUsFormDataContext rdb = new DefenceContactUsFormDataContext();
                    DefenceContactFormRecord r = new DefenceContactFormRecord()
                    {
                        Email = m.EmailID,
                        FormType = m.FormName,
                        PageInfo = m.PageInfo,
                        FormSubmitOn = new DateTime?(m.SubmitOnDate)
                    };
                    rdb.DefenceContactFormRecords.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                }
                catch (Exception exception1)
                {
                    Exception ex = exception1;
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }
                try
                {
                    string emailText = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailText", "");
                    string message = "";
                    message = string.Concat("Hello<br><br>", emailText, "<br><br>");
                    message = string.Concat(message, "<br>Email-Id: ", m.EmailID, "<br><br>Thanks");
                    string to = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailTo", "");
                    string from = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailFrom", "");
                    if (this.sendEmail(to, DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailSubject", ""), message, from))
                    {
                        Log.Error("Email Sent- ", "");
                    }
                }
                catch (Exception exception2)
                {
                    Exception ex = exception2;
                    result = new { status = "1" };
                    Log.Error("Failed to sent Email", ex.ToString());
                }
            }
            return base.Json(result, 0);
        }
        [HttpGet]
        public ActionResult DefenceVendorEnrollmentForm()
        {
            DefenceVendorEnrollmentModel m = new DefenceVendorEnrollmentModel();
            return base.View("/Views/Defence/Sublayouts/DefenceVendorForm.cshtml", m);
        }

        [HttpPost]
        public ActionResult DefenceVendorEnrollmentForm(DefenceVendorEnrollmentModel m, string SubmitApplication)
        {
            Log.Info("Insert DefenceVendorEnrollmentForm", "Start");
            bool validationStatus = true;
            try
            {
                validationStatus = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Info(string.Concat("Failed to validate auto script : ", ex.ToString()), "Failed");
            }
            if (!validationStatus)
            {
                ModelState.AddModelError(nameof(m.reResponse), DictionaryPhraseRepository.Current.Get("/Defence/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                return View("/Views/Defence/Sublayouts/DefenceVendorForm.cshtml", m);
            }
            else
            {
                Log.Info("Insert DefenceVendorEnrollmentForm Captcha Validated", "Start");
                Helper.Helper helper = new Helper.Helper();
                byte[] bytes;
                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        if (!ModelState.IsValid)
                        {
                            return View("/Views/Defence/Sublayouts/DefenceVendorForm.cshtml", m);
                        }
                        if (m.SectorServedList != null && m.SectorServedList.Count > 0)
                        {
                            m.SectorServed = helper.GetSelectedCheckboxValues(m.SectorServedList);
                        }
                        if (m.SupplierTypeList != null && (m.SupplierTypeList.Count > 0))
                        {
                            m.TypeofSupplier = helper.GetSelectedCheckboxValues(m.SupplierTypeList);
                        }
                        if (m.SegmentServedTypeList != null && m.SegmentServedTypeList.Count > 0)
                        {
                            m.SegmentsServed = helper.GetSelectedCheckboxValues(m.SegmentServedTypeList);
                        }
                        if (m.DA_PlatformsServedList != null && m.DA_PlatformsServedList.Count > 0)
                        {
                            m.PlatformsServed = helper.GetSelectedCheckboxValues(m.DA_PlatformsServedList);
                        }
                        if (m.SupplierToList != null && m.SupplierToList.Count > 0)
                        {
                            m.SupplierTo = helper.GetSelectedCheckboxValues(m.SupplierToList);
                        }
                        if (m.selectedManufacturType != null && m.selectedManufacturType.Count() > 0)
                        {
                            m.ManufacturTypeList = string.Join("|", m.selectedManufacturType);
                        }
                        if (m.selectedTrader_D_DType != null && m.selectedTrader_D_DType.Count() > 0)
                        {
                            m.Trade_Distriubtion_Deal_TypeList = string.Join("|", m.selectedTrader_D_DType);
                        }
                        if (m.selectedSpclPro_TLType != null && m.selectedSpclPro_TLType.Count() > 0)
                        {
                            m.SpclProcess_TestLabs_TypeList = string.Join("|", m.selectedSpclPro_TLType);
                        }
                        if (m.selectedEnggServicesType != null && m.selectedEnggServicesType.Count() > 0)
                        {
                            m.ManufacturTypeList = string.Join("|", m.selectedEnggServicesType);
                        }
                        if (m.UploadedCompanyProfileDoc != null && m.UploadedCompanyProfileDoc.ContentLength > 0)
                        {
                            HttpPostedFileBase file = m.UploadedCompanyProfileDoc;
                            FileUpload1 fu = new FileUpload1();
                            fu.filesize = 8192;
                            string validate = fu.UploadUserFile(file);
                            if (!string.IsNullOrEmpty(validate))
                            {
                                ModelState.AddModelError(nameof(m.UploadedCompanyProfileDoc), DictionaryPhraseRepository.Current.Get("/Defence/Controller Messages/FileType Error", validate));
                                return View("/Views/Defence/Sublayouts/DefenceVendorForm.cshtml", m);
                            }
                        }
                        
                        VendorEnrollmentFormDataContext dbcontext = new VendorEnrollmentFormDataContext();
                        m.RegistrationNo = helper.GetUniqueRegNo();
                        for (; ; )
                        {
                            if (dbcontext.DefenceVendorEnrollmentForms.Where(a => a.RegistrationNumber == m.RegistrationNo).FirstOrDefault() != null)
                                m.RegistrationNo = helper.GetUniqueRegNo();
                            else
                                break;
                        }

                        m.Id = Guid.NewGuid();
                        m.CompanyProfileDocDLink = m.PageInfo.Replace(m.CompanyProfileDocDLink,"/")+ "api/Defence/DownloadFile?id="+m.Id+"&RegNo="+m.RegistrationNo;
                        DefenceVendorEnrollmentForm rdb = new DefenceVendorEnrollmentForm()
                        {
                            Id = m.Id,
                            RegistrationNumber = m.RegistrationNo,
                            CompanyFullName = m.CompanyFullName,
                            Address = m.Address,
                            City = m.City,
                            Pincode = m.Pincode,
                            ContactPerson = m.ContactPerson,
                            EmailId = m.EmailId,
                            MobileNo = m.MobileNo,
                            TelephoneNo = m.TelephoneNo,
                            Website = m.Website,
                            GSTIN = m.GSTIN,
                            PanNo = m.PanNo,
                            Tin = m.Tin,
                            ImportExportCode = m.ImportExportCode,
                            MSME=m.MSME,
                            TypeofOwnership = m.TypeofOwnership,
                            NumberofEmployees = m.NumberofEmployees,
                            SectorServed = m.SectorServed,
                            TypeofSupplier = m.TypeofSupplier,
                            SegmentsServed = m.SegmentsServed,
                            PlatformsServed = m.PlatformsServed,
                            AreYouaSupplierTo = m.SupplierTo,
                            Clientele1 = m.Clientele1,
                            Clientele2 = m.Clientele2,
                            Clientele3 = m.Clientele3,
                            Clientele4 = m.Clientele4,
                            Clientele5 = m.Clientele5,
                            CountryandCustomer1 = m.CountryandCustomer1,
                            CountryandCustomer2 = m.CountryandCustomer2,
                            CountryandCustomer3 = m.CountryandCustomer3,
                            SalesValueDandA_FY1 = m.SalesValueDandA_FY1,
                            SalesValueDandA_FY2 = m.SalesValueDandA_FY2,
                            SalesValueDandA_FY3 = m.SalesValueDandA_FY3,
                            SalesValueNonDandA_FY1 = m.SalesValueNonDandA_FY1,
                            SalesValueNonDandA_FY2 = m.SalesValueNonDandA_FY2,
                            SalesValueNonDandA_FY3 = m.SalesValueNonDandA_FY3,
                            AnnualSales_FY1 = m.AnnualSales_FY1,
                            AnnualSales_FY2 = m.AnnualSales_FY2,
                            AnnualSales_FY3 = m.AnnualSales_FY3,
                            QualityCertification1 = m.QualityCertification1,
                            QualityCertification2 = m.QualityCertification2,
                            QualityCertification3 = m.QualityCertification3,
                            QualityCertification4 = m.QualityCertification4,
                            QualityCertification5 = m.QualityCertification5,
                            AnyOtherCertification1 = m.AnyOtherCertification1,
                            AnyOtherCertification2 = m.AnyOtherCertification2,
                            AnyOtherCertification3 = m.AnyOtherCertification3,
                            AnyOtherCertification4 = m.AnyOtherCertification4,
                            AnyOtherCertification5 = m.AnyOtherCertification5,
                            IsManufacturer = m.IsManufacturer,
                            ManufacturTypeList = m.ManufacturTypeList,
                            ManufacturerSpecification = m.ManufacturerSpecification,
                            IsTrader_Distributer_Dealer_Reseller = m.IsTrader_Distributer_Dealer_Reseller,
                            Trade_Distriubtion_Deal_TypeList = m.Trade_Distriubtion_Deal_TypeList,
                            Trade_Distriubtion_Deal_Specifcation = m.Trade_Distriubtion_Deal_Specifcation,
                            IsSpclProcess_TestingLabs = m.IsSpclProcess_TestingLabs,
                            SpclProcess_TestLabs_TypeList = m.SpclProcess_TestLabs_TypeList,
                            SpclProcess_TestLabsSpecification = m.SpclProcess_TestLabsSpecification,
                            IsEngineeringServices = m.IsEngineeringServices,
                            EngineeringServicesTypeList = m.EngineeringServicesTypeList,
                            EngineeringServicesSpecification = m.EngineeringServicesSpecification,
                            PageInfo = m.PageInfo,
                            FormName = m.FormName,
                            FormDate = m.FormDate,
                            Place = m.Place,
                            UploadedCompanyProfileFileLink = m.CompanyProfileDocDLink,
                            SubmittedBy = m.SubmittedBy,
                            SubmitOnDate = DateTime.Now,
                            CreatedOn = DateTime.Now
                        };
                        dbcontext.DefenceVendorEnrollmentForms.InsertOnSubmit(rdb);
                        dbcontext.SubmitChanges();
                        Log.Info("Form Defence Vendor data savec into db successfully: ", this);
                        if (!string.IsNullOrEmpty(m.RegistrationNo))
                        {
                            if (m.UploadedCompanyProfileDoc != null && m.UploadedCompanyProfileDoc.ContentLength > 0)
                            {
                                using (VendorEnrollmentFormDataContext EnrollFile = new VendorEnrollmentFormDataContext())
                                {
                                    HttpPostedFileBase file = m.UploadedCompanyProfileDoc;
                                    using (BinaryReader br = new BinaryReader(file.InputStream))
                                    {
                                        bytes = br.ReadBytes(file.ContentLength);
                                    }
                                    DefenceVendorEnrollmentFile EFile = new DefenceVendorEnrollmentFile()
                                    {
                                        Id = m.Id,
                                        RegistrationNumber = m.RegistrationNo,
                                        CompanyFullName = m.CompanyFullName,
                                        UploadedFileName = file.FileName,
                                        UploadedFile = bytes,
                                        UploadedFileType = file.ContentType
                                    };
                                    dbcontext.DefenceVendorEnrollmentFiles.InsertOnSubmit(EFile);
                                    dbcontext.SubmitChanges();
                                    Log.Info("DefenceVendorForm Successfully UploadedCompanyProfile:" + file.FileName + ", " + file.ContentType, this);
                                };
                            }
                        }
                    }
                    SendMailforVendorEnroll(m);
                    return Redirect("/vendorregistration-thankyou");
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at Saving Form Defence Vendor: " + ex.Message, this); 
                    ViewBag.Message = "Something has been wrong, Please try again later";
                    return View("/Views/Defence/Sublayouts/DefenceVendorForm.cshtml", m);
                }
            }
        }
        public FileResult DownloadFile(Guid id,string RegNo)
        {
            try
            {                
                using (VendorEnrollmentFormDataContext dbcontext = new VendorEnrollmentFormDataContext())
                {                   

                    if (dbcontext.DefenceVendorEnrollmentFiles.Any(a => a.Id == id && a.RegistrationNumber == RegNo))
                    {
                        var fileToDownload = dbcontext.DefenceVendorEnrollmentFiles.Where(i => i.Id == id && i.RegistrationNumber == RegNo).FirstOrDefault();
                        return File(fileToDownload.UploadedFile.ToArray(), fileToDownload.UploadedFileType, fileToDownload.UploadedFileName);
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

        public void SendMailforVendorEnroll(DefenceVendorEnrollmentModel model)
        {
            try
            {
                Item mailconfig = Context.Database.GetItem(Templates.MailConfiguration.MailConfigurationItemID);
                Log.Info("Payment Success mail sending to client", this);
                string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                string CustomerTo = model.EmailId;
                string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                string CustomerMailBody = CustomerMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SuccessMessage].Value;

                string OfficialsFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_MailFrom].Value;
                string OfficialsTo = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_RecipientMail].Value;
                string OfficialsMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_Message].Value;
                string OfficialsSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_SubjectName].Value;

                using (VendorEnrollmentFormDataContext dbcontext = new VendorEnrollmentFormDataContext())
                {
                    DefenceVendorEnrollmentForm ctx = dbcontext.DefenceVendorEnrollmentForms.Where(x => x.Id == model.Id && x.RegistrationNumber == model.RegistrationNo).FirstOrDefault();
                    //customer mail body
                    CustomerMailBody = CustomerMailBody.Replace("$contactname", ctx.ContactPerson);
                    CustomerMailBody = CustomerMailBody.Replace("$name", ctx.CompanyFullName);
                    CustomerMailBody = CustomerMailBody.Replace("$address", ctx.Address);
                    CustomerMailBody = CustomerMailBody.Replace("$city", ctx.City);
                    CustomerMailBody = CustomerMailBody.Replace("$pincode", ctx.Pincode);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", ctx.MobileNo);
                    //CustomerMailBody = CustomerMailBody.Replace("$teleno", ctx.TelephoneNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", ctx.EmailId);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", ctx.RegistrationNumber);
                    //officials mail body
                    OfficialsMailBody = OfficialsMailBody.Replace("$contactname", ctx.ContactPerson);
                    OfficialsMailBody = OfficialsMailBody.Replace("$name", ctx.CompanyFullName);
                    OfficialsMailBody = OfficialsMailBody.Replace("$address", ctx.Address);
                    OfficialsMailBody = OfficialsMailBody.Replace("$city", ctx.City);
                    OfficialsMailBody = OfficialsMailBody.Replace("$pincode", ctx.Pincode);
                    OfficialsMailBody = OfficialsMailBody.Replace("$mobile", ctx.MobileNo);
                    //OfficialsMailBody = OfficialsMailBody.Replace("$teleno", ctx.TelephoneNo);
                    OfficialsMailBody = OfficialsMailBody.Replace("$registrationno", ctx.RegistrationNumber);
                    OfficialsMailBody = OfficialsMailBody.Replace("$filelink", ctx.UploadedCompanyProfileFileLink??string.Empty);
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
            catch(Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at SendMailforVendorEnroll Form Defence Vendor: " + ex.Message, this);
            }
            
        }

        public bool IsReCaptchValid(string reResponse)
        {
            bool result = false;
            string captchaResponse = reResponse;
            string secretKey = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "");
            string requestUri = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, captchaResponse);
            using (WebResponse response = ((HttpWebRequest)WebRequest.Create(requestUri)).GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    result = (JObject.Parse(stream.ReadToEnd()).Value<bool>("success") ? true : false);
                }
            }
            return result;
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

        public bool sendEmailforVendorEnroll(string to, string subject, string body, string from)
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
                mail.From = new MailAddress(from);
                MainUtil.SendMail(mail);
                status = true;
                return status;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message, "Defence sendEmail - ");
                Log.Error(ex.Message, "Defence sendEmail - ");
                Log.Error(ex.InnerException.ToString(), "Defence sendEmail - ");
                return status;
            }
        }
    }
}