using Rotativa;
using Sitecore.Diagnostics;
using Sitecore.Electricity.Website.Model;
using Sitecore.Electricity.Website.Services;
using Sitecore.Electricity.Website.Utility;
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
using static Sitecore.Electricity.Website.Controllers.FeedbackController;

namespace Sitecore.Electricity.Website.Controllers
{
    public class ITSRFormController : Controller
    {
        [HttpGet]
        public ActionResult ITSRFormCreate()
        {
            ITSRBidderFormService serviceObj = new ITSRBidderFormService();
            if (Session["ITSRUserLogin"] == null || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }

            ITSRFormCreateModel model = new ITSRFormCreateModel();
            model.TenderNo = serviceObj.GetProposalNumber();
            return View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ITSRFormCreate(ITSRFormCreateModel model, FormCollection form, string SubmitAndClose = null, string SubmitAll = null)
        {
            if (Session["ITSRUserLogin"] == null || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            ITSRBidderFormService objService = new ITSRBidderFormService();
            var id = objService.CreateNewForm(model);

            model.IsFormCreated = true;
            model.FormId = id.ToString();

            //send email to owners with link
            //send emails

            string bidderformlink = DictionaryPhraseRepository.Current.Get("/ITSR/Registration/Enter OTP", "https://preprod.adanielectricity.com/bidderform?id=") + id.ToString();

            objService.SendOnCreateEmail(model.ProposalOwnerEmailId, model.Title, model.TenderNo, bidderformlink);
            objService.SendOnCreateEmail(model.BuyerOwnerEmailId, model.Title, model.TenderNo, bidderformlink);

            return this.View(model);
        }

        [HttpGet]
        public ActionResult ITSRFormEdit()
        {
            ITSRFormCreateModel model = new ITSRFormCreateModel();
            try
            {
                if (Session["ITSRUserLogin"] == null || !ValidateCurrentSession())
                {
                    this.Session["ITSRUserLogin"] = null;
                    ITSRUserSession.ITSRUserSessionContext = null;
                    var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                    return this.Redirect(item.Url());
                }

                string formid = Request.QueryString["id"];
                if (string.IsNullOrEmpty(formid))
                {
                    var item = Context.Database.GetItem(Templates.ITSR.ITSRBidderFormListing);
                    return this.Redirect(item.Url());
                }

                ITSRBidderFormService objService = new ITSRBidderFormService();

                TempData["id"] = formid;

                Guid id = new Guid(formid);
                var formdetails = objService.GetFormDetails(id);
                model = new ITSRFormCreateModel
                {
                    Title = formdetails.Title,
                    TenderNo = formdetails.TenderNo,
                    StartDate = formdetails.StartDate.ToString("dd/MM/yyyy"),
                    EndDate = formdetails.EndDate.ToString("dd/MM/yyyy"),
                    ProposalOwnerEmailId = formdetails.ProposalOwnerEmail,
                    ProposalOwnerName = formdetails.ProposalOwnerName,
                    BuyerOwnerEmailId = formdetails.BuyerEmail,
                    BuyerOwnerName = formdetails.BuyerName,
                    FormId = formdetails.FormId.ToString()
                };
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error("Error at ITSRFormEdit " + e.Message, this);
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ITSRFormEdit(ITSRFormCreateModel model, FormCollection form, string SubmitAndClose = null, string SubmitAll = null)
        {
            if (Session["ITSRUserLogin"] == null || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            ITSRBidderFormService objService = new ITSRBidderFormService();
            var r = objService.UpdateForm(model);

            model.IsFormCreated = true;

            return this.View(model);
        }

        [HttpGet]
        public ActionResult BidderProcessForm()
        {
            ITSRBidderFormService objService = new ITSRBidderFormService();
            ITSRBidderProcessFormModel model = new ITSRBidderProcessFormModel();
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    Guid id = new Guid(Request.QueryString["id"]);
                    if (objService.IsFormExists(id))
                    {
                        model.FormId = id.ToString();
                        model.IsExists = true;

                        ITSRBidderProcessFormSessionModel sessionModel = new ITSRBidderProcessFormSessionModel
                        {
                            FormId = id.ToString(),
                            IsExists = true
                        };

                        Session["ITSRBidderProcessFormModel"] = sessionModel;
                    }
                    else
                        model.IsExists = false;
                }
                else
                {
                    model.IsExists = false;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error at BidderProcessForm " + e.Message, this);
                model.IsExists = false;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult BidderProcessForm(ITSRBidderProcessFormModel model, string GetExistingForm = null, string GetOTP = null, string ValidateOTP = null, string Submit1 = null, string SaveAsDraft = null, string GetOTPForExistingForm = null, string ValidateOTPForExistingForm = null)
        {
            ITSRBidderFormService objService = new ITSRBidderFormService();
            model.IsExists = true;

            if (!string.IsNullOrEmpty(GetExistingForm))
            {
                if (Session["ITSRBidderProcessFormModel"] == null)
                {
                    this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }
                model.IsExists = true;
                model.IsExistingUser = true;
                return this.View(model);
            }
            if (!string.IsNullOrEmpty(GetOTPForExistingForm))
            {
                if (ModelState["MobileNumber"].Errors.Any())
                {
                    return this.View(model);
                }

                if (Session["ITSRBidderProcessFormModel"] == null)
                {
                    this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }
                ITSRBidderProcessFormSessionModel accountNumberValidated = (ITSRBidderProcessFormSessionModel)Session["ITSRBidderProcessFormModel"];

                var existingForm = objService.GetBidderFormDetails(model.MobileNumber, accountNumberValidated.FormId);
                //Unique URL / Link should work only once with the registered Mobile no.
                if (existingForm == null)
                {
                    Log.Info("BidderProcessForm: form already submitted for MobileNumber." + model.MobileNumber, this);
                    this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Form not saved", "Form does not exists this mobile number."));
                    model.IsExists = true;
                    model.IsExistingUser = true;
                    return this.View(model);
                }
                if (existingForm != null && existingForm.IsSubmit == true)
                {
                    Log.Info("BidderProcessForm: form already submitted for MobileNumber." + model.MobileNumber, this);
                    this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Form not saved", "Form is already submitted for this mobile number."));
                    model.IsExists = true;
                    model.IsExistingUser = true;
                    return this.View(model);
                }
                //send SMS for OTP
                if (!objService.CheckForCAMaxOTPLimit(model.MobileNumber, "ITSRBidderProcessForm"))
                {
                    Log.Info("BidderProcessForm: Number of attempt to get OTP reached for MobileNumber." + model.MobileNumber, this);
                    this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ITSR/LEC Login/Max20OTPPerLECMobile", "Number of attempt to get OTP reached for Entered Mobile Number."));
                    model.IsExists = true;
                    model.IsExistingUser = true;
                    return this.View(model);
                }

                string generatedotp = objService.GenerateOTP(model.FormId, "ITSRBidderProcessForm", model.MobileNumber);
                //model.OTPNumber = generatedotp;
                //model.IsOTPSent = true;
                //model.IsExists = true;
                //model.IsExistingUser = true;
                //accountNumberValidated.MobileNumber = model.MobileNumber;
                //accountNumberValidated.IsOTPSent = true;
                //accountNumberValidated.IsExists = true;
                //Session["ITSRBidderProcessFormModel"] = accountNumberValidated;
                //return this.View(model);
                //send otp via SMS
                #region Api call to send SMS of OTP
                try
                {
                    var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/SDInstallmentOptIn//OTP API URL",
                            "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Dear Customer, OTP is {1} for your transaction for {2} . Adani Electricity.&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707162256666913751"), model.MobileNumber, generatedotp, "Bidder form process");

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Info("BidderProcessForm: OTP Api call success for LEC registration", this);
                        model.IsOTPSent = true;
                        model.IsExists = true;
                        model.IsExistingUser = true;

                        accountNumberValidated.MobileNumber = model.MobileNumber;
                        accountNumberValidated.IsOTPSent = true;
                        accountNumberValidated.IsExists = true;

                        Session["ITSRBidderProcessFormModel"] = accountNumberValidated;
                        return this.View(model);
                    }
                    else
                    {
                        Log.Error("BidderProcessForm OTP Api call failed for registration", this);
                        this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ITSR/LEC Login/OTP sending error", "Unable to send OTP."));
                        model.IsExists = true;
                        model.IsExistingUser = true;
                        return this.View(model);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("BidderProcessForm: OTP Api call failed for registration: " + ex.Message, this);
                    this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ITSR/LEC Login/OTP sending error", "Unable to send OTP."));
                    model.IsExists = true;
                    model.IsExistingUser = true;
                    return this.View(model);
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(ValidateOTPForExistingForm))
            {
                if (string.IsNullOrEmpty(model.OTPNumber))
                {
                    this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Enter OTP", "Please enter valid OTP."));
                    model.IsOTPSent = true;
                    model.IsOTPValid = false;
                    model.IsExists = true;
                    model.IsExistingUser = true;
                    return this.View(model);
                }
                if (Session["ITSRBidderProcessFormModel"] == null)
                {
                    this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }

                ITSRBidderProcessFormSessionModel accountNumberValidated = (ITSRBidderProcessFormSessionModel)Session["ITSRBidderProcessFormModel"];
                if (model.MobileNumber == accountNumberValidated.MobileNumber)
                {
                    if (string.IsNullOrEmpty(model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Registration/Enter OTP", "Enter OTP."));
                        model.IsOTPSent = true;
                        model.IsExists = true;
                        model.IsExistingUser = true;
                        return this.View(model);
                    }

                    string generatedOTP = objService.GetOTP(model.MobileNumber, model.FormId, "ITSRBidderProcessForm");

                    if (!string.Equals(generatedOTP, model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                        model.IsOTPSent = true;
                        model.IsOTPValid = false;
                        model.IsExists = true;
                        model.IsExistingUser = true;
                        return this.View(model);
                    }

                    model.IsOTPSent = true;
                    model.IsOTPValid = true;
                    model.IsExistingUser = true;

                    Guid id = new Guid(accountNumberValidated.FormId);
                    var formDetails = objService.GetFormDetails(id);
                    var formSubmissionDetails = objService.GetBidderFormDetails(accountNumberValidated.MobileNumber, accountNumberValidated.FormId);

                    accountNumberValidated.IsOTPSent = true;
                    accountNumberValidated.IsOTPValid = true;
                    accountNumberValidated.Title = formDetails.Title;
                    accountNumberValidated.TenderNo = formDetails.TenderNo;

                    model.Title = formDetails.Title;
                    model.TenderNo = formDetails.TenderNo;

                    model.CompanyName = formSubmissionDetails.CompanyName;
                    model.SPOCPersonName = formSubmissionDetails.SPOCPersonName;
                    model.EmailId = formSubmissionDetails.EmailId;
                    model.Location = formSubmissionDetails.Location;
                    model.Deliveryschedule = formSubmissionDetails.Delivery_Schedule_terms_of_weeks;
                    model.Deviation = formSubmissionDetails.Deviation_if_any;
                    model.AcceptanceOfBillOfQuantity = formSubmissionDetails.Acceptance_of_Bill_of_Quantity;
                    model.SiteVisitDone = formSubmissionDetails.Site_Visit_done;
                    model.DemonstrationOfProduct = formSubmissionDetails.Demonstration_of_product;

                    model.UploadedDocuments = objService.GetBidderSubmittedDocuments((int)formSubmissionDetails.Id);

                    Session["ITSRBidderProcessFormModel"] = accountNumberValidated;
                }
            }
            if (!string.IsNullOrEmpty(GetOTP))
            {
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }

                if (Session["ITSRBidderProcessFormModel"] == null)
                {
                    this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }
                ITSRBidderProcessFormSessionModel accountNumberValidated = (ITSRBidderProcessFormSessionModel)Session["ITSRBidderProcessFormModel"];

                //Unique URL / Link should work only once with the registered Mobile no.
                var existingForm = objService.GetBidderFormDetails(model.MobileNumber, accountNumberValidated.FormId);
                if (existingForm != null)
                {
                    Log.Info("BidderProcessForm: form already submitted for MobileNumber." + model.MobileNumber, this);
                    if (existingForm.IsSubmit == true)
                    {
                        this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Form is already submitted", "Form is already submitted using this mobile number."));
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Form already saved", "Form is saved using this mobile number, please use existing form option to open saved form for submission."));
                    }
                    model.IsExists = true;
                    return this.View(model);
                }

                //send SMS for OTP
                if (!objService.CheckForCAMaxOTPLimit(model.MobileNumber, "ITSRBidderProcessForm"))
                {
                    Log.Info("BidderProcessForm: Number of attempt to get OTP reached for MobileNumber." + model.MobileNumber, this);
                    this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ITSR/LEC Login/Max20OTPPerLECMobile", "Number of attempt to get OTP reached for Entered Mobile Number."));
                    model.IsExists = true;
                    return this.View(model);
                }

                string generatedotp = objService.GenerateOTP(model.FormId, "ITSRBidderProcessForm", model.MobileNumber);

                //model.OTPNumber = generatedotp;
                //model.IsOTPSent = true;
                //model.IsExists = true;
                //accountNumberValidated.CompanyName = model.CompanyName;
                //accountNumberValidated.MobileNumber = model.MobileNumber;
                //accountNumberValidated.Location = model.Location;
                //accountNumberValidated.SPOCPersonName = model.SPOCPersonName;
                //accountNumberValidated.EmailId = model.EmailId;
                //accountNumberValidated.IsOTPSent = true;
                //accountNumberValidated.IsExists = true;

                //Session["ITSRBidderProcessFormModel"] = accountNumberValidated;
                //return this.View(model);

                //send otp via SMS
                #region Api call to send SMS of OTP
                try
                {
                    var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/SDInstallmentOptIn//OTP API URL",
                            "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Dear Customer, OTP is {1} for your transaction for {2} . Adani Electricity.&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707162256666913751"), model.MobileNumber, generatedotp, "Bidder form process");

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Info("BidderProcessForm: OTP Api call success for LEC registration", this);
                        model.IsOTPSent = true;
                        model.IsExists = true;


                        accountNumberValidated.CompanyName = model.CompanyName;
                        accountNumberValidated.MobileNumber = model.MobileNumber;
                        accountNumberValidated.Location = model.Location;
                        accountNumberValidated.SPOCPersonName = model.SPOCPersonName;
                        accountNumberValidated.EmailId = model.EmailId;
                        accountNumberValidated.IsOTPSent = true;
                        accountNumberValidated.IsExists = true;

                        Session["ITSRBidderProcessFormModel"] = accountNumberValidated;
                        return this.View(model);
                    }
                    else
                    {
                        Log.Error("BidderProcessForm OTP Api call failed for registration", this);
                        this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ITSR/LEC Login/OTP sending error", "Unable to send OTP."));
                        model.IsExists = true;
                        return this.View(model);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("BidderProcessForm: OTP Api call failed for registration: " + ex.Message, this);
                    this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ITSR/LEC Login/OTP sending error", "Unable to send OTP."));
                    model.IsExists = true;
                    return this.View(model);
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(ValidateOTP))
            {
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }
                if (string.IsNullOrEmpty(model.OTPNumber))
                {
                    this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Enter OTP", "Please enter valid OTP."));
                    model.IsOTPSent = true;
                    model.IsOTPValid = false;
                    return this.View(model);
                }

                if (Session["ITSRBidderProcessFormModel"] == null)
                {
                    this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }

                ITSRBidderProcessFormSessionModel accountNumberValidated = (ITSRBidderProcessFormSessionModel)Session["ITSRBidderProcessFormModel"];
                if (model.MobileNumber == accountNumberValidated.MobileNumber)
                {
                    if (string.IsNullOrEmpty(model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Registration/Enter OTP", "Enter OTP."));
                        model.IsOTPSent = true;
                        return this.View(model);
                    }

                    string generatedOTP = objService.GetOTP(model.MobileNumber, model.FormId, "ITSRBidderProcessForm");

                    if (!string.Equals(generatedOTP, model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                        model.IsOTPSent = true;
                        model.IsOTPValid = false;
                        return this.View(model);
                    }

                    model.IsOTPSent = true;
                    model.IsOTPValid = true;

                    Guid id = new Guid(accountNumberValidated.FormId);
                    var formDetails = objService.GetFormDetails(id);

                    accountNumberValidated.IsOTPSent = true;
                    accountNumberValidated.IsOTPValid = true;
                    accountNumberValidated.Title = formDetails.Title;
                    accountNumberValidated.TenderNo = formDetails.TenderNo;

                    model.Title = formDetails.Title;
                    model.TenderNo = formDetails.TenderNo;

                    Session["ITSRBidderProcessFormModel"] = accountNumberValidated;
                }
            }
            if (!string.IsNullOrEmpty(Submit1))
            {
                Log.Info("BidderProcessForm submit button clicked", this);
                model.IsOTPSent = true;
                model.IsOTPValid = true;
                if (!this.ModelState.IsValid)
                {
                    Log.Info("Bidder form model state invalid", this);
                    return View(model);
                }

                var formSubmissionDetails = objService.GetBidderFormDetails(model.MobileNumber, model.FormId);

                List<ITSR_BidderFormSubmissions_Document> previouslyUploadedDocuments = new List<ITSR_BidderFormSubmissions_Document>();
                if (formSubmissionDetails != null)
                    previouslyUploadedDocuments = objService.GetBidderSubmittedDocuments((int)formSubmissionDetails.Id);

                model.UploadedDocuments = previouslyUploadedDocuments;

                //check all required fields
                Log.Info("BidderProcessForm submit button clicked, check all required fields", this);
                bool isError = false;
                if (model.PastExperienceList == null || model.PastExperienceList.FirstOrDefault() == null)
                {
                    if (previouslyUploadedDocuments.Where(d => d.DocumenttypeCode == "PastExperience").Count() > 0)
                    {
                        //file already exists so continue
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.PastExperienceList), DictionaryPhraseRepository.Current.Get("/ITSR/File Mandatory", "Please upload file."));
                        isError = true;
                        //return this.View(model);
                    }
                }
                if (model.GuaranteeTechnicalParticulars == null || model.GuaranteeTechnicalParticulars.FirstOrDefault() == null)
                {
                    if (previouslyUploadedDocuments.Where(d => d.DocumenttypeCode == "GuaranteeTechnicalParticulars").Count() > 0)
                    {
                        //file already exists so continue
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.GuaranteeTechnicalParticulars), DictionaryPhraseRepository.Current.Get("/ITSR/File Mandatory", "Please upload file."));
                        isError = true;
                        //return this.View(model);
                    }
                }
                if (model.GeneralArrangement == null || model.GeneralArrangement.FirstOrDefault() == null)
                {
                    if (previouslyUploadedDocuments.Where(d => d.DocumenttypeCode == "GeneralArrangement").Count() > 0)
                    {
                        //file already exists so continue
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.GeneralArrangement), DictionaryPhraseRepository.Current.Get("/ITSR/File Mandatory", "Please upload file."));
                        isError = true;
                        //return this.View(model);
                    }
                }
                if (string.IsNullOrEmpty(model.Deliveryschedule) || ModelState["Deliveryschedule"].Errors.Any())
                {
                    this.ModelState.AddModelError(nameof(model.Deliveryschedule), DictionaryPhraseRepository.Current.Get("/ITSR/File Mandatory", "Please provide valid details."));
                    isError = true;
                    //return this.View(model);
                }
                if (model.Deviation == "Yes" || model.AcceptanceOfBillOfQuantity == "Deviation")
                {
                    if (model.ConsolidatedDeviationSheet == null || model.ConsolidatedDeviationSheet.FirstOrDefault() == null)
                    {
                        if (previouslyUploadedDocuments.Where(d => d.DocumenttypeCode == "ConsolidatedDeviationSheet").Count() > 0)
                        {
                            //file already exists so continue
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(model.ConsolidatedDeviationSheet), DictionaryPhraseRepository.Current.Get("/ITSR/File Mandatory", "Please upload file."));
                            isError = true;
                            //return this.View(model);
                        }
                    }
                }

                Log.Info("BidderProcessForm submit button clicked, isError:" + isError, this);

                if (isError)
                {
                    model.IsOTPSent = true;
                    model.IsOTPValid = true;
                    return this.View(model);
                }

                try
                {
                    if (Session["ITSRBidderProcessFormModel"] == null)
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Registration/Enter OTP", "Please validate."));
                        return this.View(model);
                    }

                    ITSRBidderProcessFormSessionModel accountNumberValidated = (ITSRBidderProcessFormSessionModel)Session["ITSRBidderProcessFormModel"];

                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        Guid formid = new Guid(accountNumberValidated.FormId);
                        var formdetails = dbcontext.ITSR_BidderFormDetails.Where(f => f.FormId == formid).FirstOrDefault();
                        if (formdetails != null)
                        {
                            var formIdNum = formdetails.Id;
                            ITSR_BidderFormSubmission objSubmission = dbcontext.ITSR_BidderFormSubmissions.Where(f => f.MobileNumber == accountNumberValidated.MobileNumber && f.FormId == accountNumberValidated.FormId).FirstOrDefault();

                            if (objSubmission == null)
                            {
                                //Save application
                                objSubmission = new ITSR_BidderFormSubmission
                                {
                                    CompanyName = model.CompanyName,
                                    MobileNumber = model.MobileNumber,
                                    Location = model.Location,
                                    EmailId = model.EmailId,
                                    FormId = accountNumberValidated.FormId,
                                    Deviation_if_any = model.Deviation,
                                    Acceptance_of_Bill_of_Quantity = model.AcceptanceOfBillOfQuantity,
                                    Delivery_Schedule_terms_of_weeks = model.Deliveryschedule,
                                    Site_Visit_done = model.SiteVisitDone,
                                    Demonstration_of_product = model.DemonstrationOfProduct,
                                    SPOCPersonName = model.SPOCPersonName,
                                    TenderNo = model.TenderNo,
                                    Title = model.Title,
                                    CreatedDate = DateTime.Now,
                                    IsSubmit = true
                                };
                                dbcontext.ITSR_BidderFormSubmissions.InsertOnSubmit(objSubmission);
                                dbcontext.SubmitChanges();

                            }
                            else
                            {
                                objSubmission.Deviation_if_any = model.Deviation;
                                objSubmission.Acceptance_of_Bill_of_Quantity = model.AcceptanceOfBillOfQuantity;
                                objSubmission.Delivery_Schedule_terms_of_weeks = model.Deliveryschedule;
                                objSubmission.Site_Visit_done = model.SiteVisitDone;
                                objSubmission.Demonstration_of_product = model.DemonstrationOfProduct;
                                //modifield date
                                objSubmission.IsSubmit = true;

                                dbcontext.SubmitChanges();
                            }


                            //Save Documents

                            //PastExperienceList
                            foreach (HttpPostedFileBase file in model.PastExperienceList)
                            {
                                if (file != null)
                                {
                                    string message = objService.ValidateFile(file);
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        var obj = objService.CreateDocumentObject(file, "PastExperience", "PastExperience", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                        if (obj != null)
                                        {
                                            dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                        }
                                    }
                                    else
                                    {
                                        this.ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Form Submit/Invalid file", "File upload error: ") + message);
                                        return this.View(model);
                                    }
                                }
                            }

                            //PastOrders
                            foreach (HttpPostedFileBase file in model.PastOrders)
                            {
                                if (file != null)
                                {
                                    string message = objService.ValidateFile(file);
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        var obj = objService.CreateDocumentObject(file, "PastOrder", "PastOrder", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                        if (obj != null)
                                        {
                                            dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                        }
                                    }
                                    else
                                    {
                                        this.ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Form Submit/Invalid file", "File upload error: ") + message);
                                        return this.View(model);
                                    }
                                }
                            }

                            //PerformanceCertificate
                            foreach (HttpPostedFileBase file in model.PerformanceCertificate)
                            {
                                if (file != null)
                                {
                                    string message = objService.ValidateFile(file);
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        var obj = objService.CreateDocumentObject(file, "PerformanceCertificate", "PerformanceCertificate", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                        if (obj != null)
                                        {
                                            dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                        }
                                    }
                                    else
                                    {
                                        this.ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Form Submit/Invalid file", "File upload error: ") + message);
                                        return this.View(model);
                                    }
                                }
                            }

                            //TestCertificate
                            foreach (HttpPostedFileBase file in model.TestCertificate)
                            {
                                if (file != null)
                                {
                                    string message = objService.ValidateFile(file);
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        var obj = objService.CreateDocumentObject(file, "TestCertificate", "TestCertificate", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                        if (obj != null)
                                        {
                                            dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                        }
                                    }
                                    else
                                    {
                                        this.ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Form Submit/Invalid file", "File upload error: ") + message);
                                        return this.View(model);
                                    }
                                }
                            }

                            //GeneralArrangement
                            foreach (HttpPostedFileBase file in model.GeneralArrangement)
                            {
                                if (file != null)
                                {
                                    string message = objService.ValidateFile(file);
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        var obj = objService.CreateDocumentObject(file, "GeneralArrangement", "GeneralArrangement", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                        if (obj != null)
                                        {
                                            dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                        }
                                    }
                                    else
                                    {
                                        this.ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Form Submit/Invalid file", "File upload error: ") + message);
                                        return this.View(model);
                                    }
                                }
                            }

                            //GuaranteeTechnicalParticulars
                            foreach (HttpPostedFileBase file in model.GuaranteeTechnicalParticulars)
                            {
                                if (file != null)
                                {
                                    string message = objService.ValidateFile(file);
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        var obj = objService.CreateDocumentObject(file, "GuaranteeTechnicalParticulars", "GuaranteeTechnicalParticulars", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                        if (obj != null)
                                        {
                                            dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                        }
                                    }
                                    else
                                    {
                                        this.ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Form Submit/Invalid file", "File upload error: ") + message);
                                        return this.View(model);
                                    }
                                }
                            }

                            //ConsolidatedDeviationSheet
                            foreach (HttpPostedFileBase file in model.ConsolidatedDeviationSheet)
                            {
                                if (file != null)
                                {
                                    string message = objService.ValidateFile(file);
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        var obj = objService.CreateDocumentObject(file, "ConsolidatedDeviationSheet", "ConsolidatedDeviationSheet", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                        if (obj != null)
                                        {
                                            dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                        }
                                    }
                                    else
                                    {
                                        this.ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Form Submit/Invalid file", "File upload error: ") + message);
                                        return this.View(model);
                                    }
                                }
                            }

                            //OtherdocumentsSpecified
                            foreach (HttpPostedFileBase file in model.OtherdocumentsSpecified)
                            {
                                if (file != null)
                                {
                                    string message = objService.ValidateFile(file, 10);
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        var obj = objService.CreateDocumentObject(file, "OtherdocumentSpecified", "OtherdocumentSpecified", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                        if (obj != null)
                                        {
                                            dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                        }
                                    }
                                    else
                                    {
                                        this.ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Form Submit/Invalid file", "File upload error: ") + message);
                                        return this.View(model);
                                    }
                                }
                            }

                            dbcontext.SubmitChanges();

                            model.Title = formdetails.Title;
                            model.TenderNo = formdetails.TenderNo;

                            //send emails
                            objService.SendOnSubmitEmail(formdetails.ProposalOwnerEmail, model);
                            objService.SendOnSubmitEmail(formdetails.BuyerEmail, model);
                        }
                    }
                }
                catch (AggregateException err)
                {
                    string exceptionMessage = "";
                    foreach (var errInner in err.InnerExceptions)
                    {
                        exceptionMessage = exceptionMessage + "_" + errInner.ToString(); //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                    }
                    Log.Error("Error at BidderProcessForm " + exceptionMessage, this);
                    ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Error", "An error occured, please try again!."));
                    return this.View(model);
                }
                catch (Exception e)
                {
                    Log.Error("Error at BidderProcessForm " + e.Message, this);
                    ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Error", "An error occured, please try again!."));
                    return this.View(model);
                }
                model.IsSubmitted = true;
            }
            if (!string.IsNullOrEmpty(SaveAsDraft))
            {
                try
                {
                    if (Session["ITSRBidderProcessFormModel"] == null)
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/ITSR/Registration/Enter OTP", "Please validate."));
                        return this.View(model);
                    }

                    ITSRBidderProcessFormSessionModel accountNumberValidated = (ITSRBidderProcessFormSessionModel)Session["ITSRBidderProcessFormModel"];

                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        Guid formid = new Guid(accountNumberValidated.FormId);
                        var formdetails = dbcontext.ITSR_BidderFormDetails.Where(f => f.FormId == formid).FirstOrDefault();
                        if (formdetails != null)
                        {
                            var formIdNum = formdetails.Id;

                            ITSR_BidderFormSubmission objSubmission = dbcontext.ITSR_BidderFormSubmissions.Where(f => f.MobileNumber == accountNumberValidated.MobileNumber && f.FormId == accountNumberValidated.FormId).FirstOrDefault();

                            if (objSubmission == null)
                            {
                                //Save application
                                objSubmission = new ITSR_BidderFormSubmission
                                {
                                    CompanyName = model.CompanyName,
                                    MobileNumber = model.MobileNumber,
                                    Location = model.Location,
                                    EmailId = model.EmailId,
                                    FormId = accountNumberValidated.FormId,
                                    Deviation_if_any = model.Deviation,
                                    Acceptance_of_Bill_of_Quantity = model.AcceptanceOfBillOfQuantity,
                                    Delivery_Schedule_terms_of_weeks = model.Deliveryschedule,
                                    Site_Visit_done = model.SiteVisitDone,
                                    Demonstration_of_product = model.DemonstrationOfProduct,
                                    SPOCPersonName = model.SPOCPersonName,
                                    TenderNo = model.TenderNo,
                                    Title = model.Title,
                                    CreatedDate = DateTime.Now,
                                    IsSubmit = false
                                };
                                dbcontext.ITSR_BidderFormSubmissions.InsertOnSubmit(objSubmission);
                                dbcontext.SubmitChanges();
                            }
                            else
                            {

                                objSubmission.Deviation_if_any = model.Deviation;
                                objSubmission.Acceptance_of_Bill_of_Quantity = model.AcceptanceOfBillOfQuantity;
                                objSubmission.Delivery_Schedule_terms_of_weeks = model.Deliveryschedule;
                                objSubmission.Site_Visit_done = model.SiteVisitDone;
                                objSubmission.Demonstration_of_product = model.DemonstrationOfProduct;
                                //modifield date
                                objSubmission.IsSubmit = false;

                                dbcontext.SubmitChanges();
                            }
                            //Save Documents

                            //PastExperienceList
                            foreach (HttpPostedFileBase file in model.PastExperienceList)
                            {
                                if (file != null)
                                {
                                    var obj = objService.CreateDocumentObject(file, "PastExperience", "PastExperience", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                    if (obj != null)
                                    {
                                        dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                    }
                                }
                            }

                            //PastOrders
                            foreach (HttpPostedFileBase file in model.PastOrders)
                            {
                                if (file != null)
                                {
                                    var obj = objService.CreateDocumentObject(file, "PastOrder", "PastOrder", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                    if (obj != null)
                                    {
                                        dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                    }
                                }
                            }

                            //PerformanceCertificate
                            foreach (HttpPostedFileBase file in model.PerformanceCertificate)
                            {
                                if (file != null)
                                {
                                    var obj = objService.CreateDocumentObject(file, "PerformanceCertificate", "PerformanceCertificate", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                    if (obj != null)
                                    {
                                        dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                    }
                                }
                            }

                            //TestCertificate
                            foreach (HttpPostedFileBase file in model.TestCertificate)
                            {
                                if (file != null)
                                {
                                    var obj = objService.CreateDocumentObject(file, "TestCertificate", "TestCertificate", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                    if (obj != null)
                                    {
                                        dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                    }
                                }
                            }

                            //GeneralArrangement
                            foreach (HttpPostedFileBase file in model.GeneralArrangement)
                            {
                                if (file != null)
                                {
                                    var obj = objService.CreateDocumentObject(file, "GeneralArrangement", "GeneralArrangement", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                    if (obj != null)
                                    {
                                        dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                    }
                                }
                            }

                            //GeneralArrangement
                            foreach (HttpPostedFileBase file in model.GuaranteeTechnicalParticulars)
                            {
                                if (file != null)
                                {
                                    var obj = objService.CreateDocumentObject(file, "GuaranteeTechnicalParticulars", "GuaranteeTechnicalParticulars", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                    if (obj != null)
                                    {
                                        dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                    }
                                }
                            }

                            //ITSRsolidatedDeviationSheet
                            foreach (HttpPostedFileBase file in model.ConsolidatedDeviationSheet)
                            {
                                if (file != null)
                                {
                                    var obj = objService.CreateDocumentObject(file, "ConsolidatedDeviationSheet", "ConsolidatedDeviationSheet", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                    if (obj != null)
                                    {
                                        dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                    }
                                }
                            }

                            //OtherdocumentsSpecified
                            foreach (HttpPostedFileBase file in model.OtherdocumentsSpecified)
                            {
                                if (file != null)
                                {
                                    var obj = objService.CreateDocumentObject(file, "OtherdocumentSpecified", "OtherdocumentSpecified", accountNumberValidated.FormId, formIdNum, objSubmission.Id);
                                    if (obj != null)
                                    {
                                        dbcontext.ITSR_BidderFormSubmissions_Documents.InsertOnSubmit(obj);
                                    }
                                }
                            }

                            dbcontext.SubmitChanges();

                            model.Title = formdetails.Title;
                            model.TenderNo = formdetails.TenderNo;

                            ////send emails
                            //objService.SendOnSubmitEmail(formdetails.ProposalOwnerEmail, model);
                            //objService.SendOnSubmitEmail(formdetails.BuyerEmail, model);
                        }
                    }
                    model.IsSaved = true;
                }
                catch (Exception e)
                {
                    Log.Error("Error at BidderProcessForm " + e.Message, this);
                    ModelState.AddModelError(nameof(model.Message), DictionaryPhraseRepository.Current.Get("/ITSR/Error", "An error occured, please try again!."));
                    return this.View(model);
                }

            }

            return View(model);
        }


        public bool DeleteBidderFile(string id)
        {
            try
            {
                if (Session["ITSRUserLogin"] != null || Session["ITSRBidderProcessFormModel"] != null)
                {
                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var fileToDelete = dbcontext.ITSR_BidderFormSubmissions_Documents.Where(i => i.Id == System.Convert.ToDouble(id)).FirstOrDefault();
                        if (fileToDelete != null)
                        {
                            dbcontext.ITSR_BidderFormSubmissions_Documents.DeleteOnSubmit(fileToDelete);
                            dbcontext.SubmitChanges();
                            return true;
                            //var item = Context.Database.GetItem(Templates.ITSR.ITSRBidderFormSubmission);
                            //return Redirect(item.Url());
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error("Error at DownloadFile:" + ex.Message, this);
            }
            return false;
        }

        public FileResult DownloadBidderFile(long id)
        {
            try
            {
                if (Session["ITSRUserLogin"] != null || Session["ITSRBidderProcessFormModel"] != null)
                {
                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var fileToDownload = dbcontext.ITSR_BidderFormSubmissions_Documents.Where(i => i.Id == id).FirstOrDefault();
                        if (fileToDownload != null)
                            return File(fileToDownload.DocumentData.ToArray(), fileToDownload.DocumentContentType, fileToDownload.DocumentName);
                        else
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at DownloadFile:" + ex.Message, this);
            }
            return null;
        }

        public ActionResult ITSRLogin()
        {
            return View(new Login());
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ITSRLogin(Login model)
        {
            try
            {
                var itsradminSection = Context.Database.GetItem(Templates.ITSR.ITSRBidderFormListing);
                var loginpage = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);
                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/SwitchToGreen/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }

                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    var userinfo = dbcontext.ITSR_Logins.Where(x => x.UserId == model.LoginName && x.Password == model.Password && x.IsActive == true).FirstOrDefault();
                    if (userinfo != null && userinfo.IsActive == true && (userinfo.UserRole == "SuperAdmin" || userinfo.UserRole == "ProposalOwner" || userinfo.UserRole == "CPGTeam"))
                    {
                        ITSRUserSession.ITSRUserSessionContext = new ITSRLoginModel
                        {
                            userId = model.LoginName,
                            UserRole = userinfo.UserRole.ToString(),
                            SessionId = Request.Cookies["ASP.NET_SessionId"].Value
                        };
                        StoreCurrentSession();
                        var url = itsradminSection.Url();
                        return this.Redirect(url);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Login Technical Error", "Username or password not valid."));
                        return this.View(model);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at TenderLogin Post:" + ex.Message, this);
                ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Login Technical Error", "There is technical problem. Please try after sometime."));
                return this.View(model);
            }
        }

        [HttpGet]
        public ActionResult ITSRFormListing()
        {
            string currentSessionId = Request.Cookies["ASP.NET_SessionId"].Value;
            if (Session["ITSRUserLogin"] == null || ITSRUserSession.ITSRUserSessionContext.SessionId != currentSessionId || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }

            ITSRBidderFormService objService = new ITSRBidderFormService();

            string UserId = ITSRUserSession.ITSRUserSessionContext.userId;
            string UserRole = ITSRUserSession.ITSRUserSessionContext.UserRole;
            ViewBag.UserRole = UserRole;

            List<ITSRFormListing> model = new List<ITSRFormListing>();
            model = objService.GetAllFormsByUser(UserId, UserRole);
            return View(model);
        }

        [HttpGet]
        public ActionResult ITSRFormBidderListing()
        {
            List<ITSR_BidderFormSubmission> model = new List<ITSR_BidderFormSubmission>();
            string currentSessionId = Request.Cookies["ASP.NET_SessionId"].Value;
            if (Session["ITSRUserLogin"] == null || ITSRUserSession.ITSRUserSessionContext.SessionId != currentSessionId || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }

            ITSRBidderFormService objService = new ITSRBidderFormService();

            if (Request.QueryString["id"] != null)
            {
                string formid = Request.QueryString["id"].ToString();
                model = objService.GetAllBidderSubmissionsByForm(formid);
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ITSRBidderProcessFormAdminView()
        {
            ITSRBidderFormService objService = new ITSRBidderFormService();
            ITSRBidderProcessFormModel model = new ITSRBidderProcessFormModel();

            string currentSessionId = Request.Cookies["ASP.NET_SessionId"].Value;
            if (Session["ITSRUserLogin"] == null || ITSRUserSession.ITSRUserSessionContext.SessionId != currentSessionId || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    int id = int.Parse(Request.QueryString["id"].ToString());
                    var details = objService.GetBidderSubmittedFormDetails(id);
                    Guid formid = new Guid(details.FormId);
                    var formDetails = objService.GetFormDetails(formid);

                    if (details != null && formDetails != null)
                    {
                        model = new ITSRBidderProcessFormModel
                        {
                            Title = formDetails.Title,
                            TenderNo = formDetails.TenderNo,
                            AcceptanceOfBillOfQuantity = details.Acceptance_of_Bill_of_Quantity,
                            BuyerOwnerEmailId = formDetails.BuyerEmail,
                            BuyerOwnerName = formDetails.BuyerName,
                            CompanyName = details.CompanyName,
                            Deliveryschedule = details.Delivery_Schedule_terms_of_weeks,
                            DemonstrationOfProduct = details.Demonstration_of_product,
                            Deviation = details.Deviation_if_any,
                            EmailId = details.EmailId,
                            Location = details.Location,
                            MobileNumber = details.MobileNumber,
                            SiteVisitDone = details.Site_Visit_done,
                            ProposalOwnerEmailId = formDetails.ProposalOwnerEmail,
                            ProposalOwnerName = formDetails.ProposalOwnerName,
                            SPOCPersonName = details.SPOCPersonName
                        };
                        model.IsExists = true;
                        model.UploadedDocuments = objService.GetBidderSubmittedDocuments(id);
                    }
                    else
                    {
                        model.IsExists = false;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error at ITSRBidderProcessFormAdminView " + e.Message, this);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);

            this.Session["ITSRUserLogin"] = null;
            ITSRUserSession.ITSRUserSessionContext = null;
            return this.Redirect(item.Url());
        }

        public ActionResult ITSRUserListing()
        {
            string currentSessionId = Request.Cookies["ASP.NET_SessionId"].Value;

            if (Session["ITSRUserLogin"] == null || ITSRUserSession.ITSRUserSessionContext.SessionId != currentSessionId || ITSRUserSession.ITSRUserSessionContext.UserRole != "SuperAdmin" || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }
            string UserId = ITSRUserSession.ITSRUserSessionContext.userId;
            string UserRole = ITSRUserSession.ITSRUserSessionContext.UserRole;
            ViewBag.UserRole = UserRole;
            ITSRBidderFormService objService = new ITSRBidderFormService();
            return View(objService.GetITSRUsers());

        }

        public ActionResult DisableUser(string id)
        {
            string currentSessionId = Request.Cookies["ASP.NET_SessionId"].Value;

            if (Session["ITSRUserLogin"] == null || ITSRUserSession.ITSRUserSessionContext.SessionId != currentSessionId || ITSRUserSession.ITSRUserSessionContext.UserRole != "SuperAdmin" || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }

            ITSRBidderFormService objService = new ITSRBidderFormService();
            Guid userid = new Guid(id);
            objService.DisableITSRUser(userid);

            var item1 = Context.Database.GetItem(Templates.ITSR.ManageUsersFormItem);
            return this.Redirect(item1.Url());
        }

        public ActionResult ResendPassword(string id)
        {
            string currentSessionId = Request.Cookies["ASP.NET_SessionId"].Value;

            if (Session["ITSRUserLogin"] == null || ITSRUserSession.ITSRUserSessionContext.SessionId != currentSessionId || ITSRUserSession.ITSRUserSessionContext.UserRole != "SuperAdmin" || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }

            ITSRBidderFormService objService = new ITSRBidderFormService();
            string RedirectUrl = string.Empty;
            var ITSRLoginitem = Sitecore.Context.Database.GetItem(Templates.ITSR.ITSRLogin);
            if (ITSRLoginitem != null)
            {
                string baseurl = ITSRLoginitem.Url();
                RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
            }
            Guid userid = new Guid(id);
            var userDetails = objService.GetITSRUser(userid);

            objService.SendOnResendPasswordEmail(userDetails.Email, userDetails.UserId, userDetails.Password, RedirectUrl);

            var item1 = Context.Database.GetItem(Templates.ITSR.ManageUsersFormItem);
            return this.Redirect(item1.Url());
        }

        public ActionResult DeleteUser(string id)
        {
            string currentSessionId = Request.Cookies["ASP.NET_SessionId"].Value;


            if (Session["ITSRUserLogin"] == null || ITSRUserSession.ITSRUserSessionContext.SessionId != currentSessionId || ITSRUserSession.ITSRUserSessionContext.UserRole != "SuperAdmin" || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }

            ITSRBidderFormService objService = new ITSRBidderFormService();
            Guid userid = new Guid(id);
            objService.DeleteITSRUser(userid);

            var item1 = Context.Database.GetItem(Templates.ITSR.ManageUsersFormItem);
            return this.Redirect(item1.Url());
        }

        public ActionResult EnableUser(string id)
        {
            string currentSessionId = Request.Cookies["ASP.NET_SessionId"].Value;

            if (Session["ITSRUserLogin"] == null || ITSRUserSession.ITSRUserSessionContext.SessionId != currentSessionId || ITSRUserSession.ITSRUserSessionContext.UserRole != "SuperAdmin" || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }

            ITSRBidderFormService objService = new ITSRBidderFormService();
            Guid userid = new Guid(id);
            objService.EnableITSRUser(userid);

            var item1 = Context.Database.GetItem(Templates.ITSR.ManageUsersFormItem);
            return this.Redirect(item1.Url());
        }

        public ActionResult ITSRCreateUser()
        {
            string currentSessionId = Request.Cookies["ASP.NET_SessionId"].Value;

            if (Session["ITSRUserLogin"] == null || ITSRUserSession.ITSRUserSessionContext.SessionId != currentSessionId || ITSRUserSession.ITSRUserSessionContext.UserRole != "SuperAdmin" || !ValidateCurrentSession())
            {
                this.Session["ITSRUserLogin"] = null;
                ITSRUserSession.ITSRUserSessionContext = null;
                var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                return this.Redirect(item.Url());
            }

            ITSRUserModel model = new ITSRUserModel();
            return View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ITSRCreateUser(ITSRUserModel model)
        {
            try
            {
                string currentSessionId = Request.Cookies["ASP.NET_SessionId"].Value;

                if (Session["ITSRUserLogin"] == null || ITSRUserSession.ITSRUserSessionContext.SessionId != currentSessionId || ITSRUserSession.ITSRUserSessionContext.UserRole != "SuperAdmin" || !ValidateCurrentSession())
                {
                    this.Session["ITSRUserLogin"] = null;
                    ITSRUserSession.ITSRUserSessionContext = null;
                    var item = Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                    return this.Redirect(item.Url());
                }

                ITSRBidderFormService objService = new ITSRBidderFormService();
                if (objService.IsUserExists(model))
                {
                    ModelState.AddModelError(nameof(model.UserName), DictionaryPhraseRepository.Current.Get("/ITSR/Username Exists", "Username already exists"));
                    return this.View(model);
                }

                //check if username already exists 

                string password = objService.CreateNewUser(model);

                string RedirectUrl = string.Empty;
                var ITSRLoginitem = Sitecore.Context.Database.GetItem(Templates.ITSR.ITSRLogin);
                if (ITSRLoginitem != null)
                {
                    string baseurl = ITSRLoginitem.Url();
                    RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                }

                objService.SendOnCreateUserEmail(model, password, RedirectUrl);

                var item1 = Context.Database.GetItem(Templates.ITSR.ManageUsersFormItem);
                return this.Redirect(item1.Url());
            }
            catch (Exception ex)
            {
                Log.Error("Error at CreateUser Post:" + ex.Message, this);
                ViewBag.ErrorMsg = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/EnvelopUser Exception Msg", "Error in Insert Please try again");

            }
            return View(model);
        }

        //public bool IsUserLoggedIn()
        //{
        //    if (Context.User != null && Context.User.IsAuthenticated)
        //    {
        //        if (SessionHelper.UserSession.UserSessionContext == null)
        //        {
        //            return false;
        //        }
        //        //else if (!SessionHelper.UserSession.UserSessionContext.SessionId.Equals(HttpContext.Current.Request.Cookies["SessionId"].Value))
        //        //{
        //        //    return false;
        //        //}
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public bool StoreCurrentSession()
        //{
        //    try
        //    {

        //        if (SessionHelper.UserSession.UserSessionContext == null)
        //        {
        //            return false;
        //        }
        //        else if (SessionHelper.UserSession.UserSessionContext.UserName != null)
        //        {
        //            UserLoginSession currentsessionExists = (
        //                from x in dc.UserLoginSessions
        //                where x.UserName == SessionHelper.UserSession.UserSessionContext.UserName && x.IsActive == true
        //                select x).FirstOrDefault<UserLoginSession>();
        //            if (currentsessionExists != null)
        //            {
        //                currentsessionExists.SessionId = SessionHelper.UserSession.UserSessionContext.SessionId;
        //                dc.SubmitChanges();
        //                return true;
        //            }
        //            else
        //            {
        //                UserLoginSession loginObj = new UserLoginSession()
        //                {
        //                    CreatedDate = DateTime.Now,
        //                    UserName = SessionHelper.UserSession.UserSessionContext.UserName,
        //                    IsActive = true,
        //                    SessionId = SessionHelper.UserSession.UserSessionContext.SessionId
        //                };
        //                dc.UserLoginSessions.InsertOnSubmit(loginObj);
        //                dc.SubmitChanges();
        //                return true;
        //            };
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error("Session store failed for adani housing login: " + e.Message, this);
        //        return false;
        //    }
        //}

        //public bool ValidateCurrentSession()
        //{
        //    try
        //    {
        //        if (SessionHelper.UserSession.UserSessionContext == null)
        //        {
        //            return false;
        //        }
        //        else if (SessionHelper.UserSession.UserSessionContext.UserName != null)
        //        {
        //            UserLoginSession currentsessionExists = (
        //                from x in dc.UserLoginSessions
        //                where x.UserName == SessionHelper.UserSession.UserSessionContext.UserName && x.SessionId == SessionHelper.UserSession.UserSessionContext.SessionId && x.IsActive == true
        //                select x).FirstOrDefault<UserLoginSession>();
        //            if (currentsessionExists != null)
        //            {
        //                return true;
        //            }
        //            return false;
        //        }
        //        else
        //        {
        //            return false;
        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error("Session store failed for my accoutn login: " + e.Message, this);
        //        return false;
        //    }
        //}

        public bool StoreCurrentSession()
        {
            try
            {
                if (ITSRUserSession.ITSRUserSessionContext == null)
                {
                    return false;
                }
                else if (ITSRUserSession.ITSRUserSessionContext.SessionId != null)
                {
                    using (TenderDataContext dc = new TenderDataContext())
                    {

                        UserLoginSession currentsessionExists = (
                        from x in dc.UserLoginSessions
                        where x.UserName == ITSRUserSession.ITSRUserSessionContext.userId && x.IsActive == true
                        select x).FirstOrDefault<UserLoginSession>();
                        if (currentsessionExists != null)
                        {
                            currentsessionExists.SessionId = ITSRUserSession.ITSRUserSessionContext.SessionId;
                            dc.SubmitChanges();
                            return true;
                        }
                        else
                        {
                            UserLoginSession loginObj = new UserLoginSession()
                            {
                                CreatedDate = DateTime.Now,
                                UserName = ITSRUserSession.ITSRUserSessionContext.userId,
                                IsActive = true,
                                SessionId = ITSRUserSession.ITSRUserSessionContext.SessionId
                            };
                            dc.UserLoginSessions.InsertOnSubmit(loginObj);
                            dc.SubmitChanges();
                            return true;
                        };
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                Log.Error("Session store failed for adani housing login: " + e.Message, this);
                return false;
            }
        }

        public bool ValidateCurrentSession()
        {
            try
            {
                if (ITSRUserSession.ITSRUserSessionContext == null)
                {
                    return false;
                }
                else if (ITSRUserSession.ITSRUserSessionContext.SessionId != null)
                {
                    using (TenderDataContext dc = new TenderDataContext())
                    {
                        UserLoginSession currentsessionExists = (
                        from x in dc.UserLoginSessions
                        where x.UserName == ITSRUserSession.ITSRUserSessionContext.userId && x.SessionId == ITSRUserSession.ITSRUserSessionContext.SessionId && x.IsActive == true
                        select x).FirstOrDefault<UserLoginSession>();
                        if (currentsessionExists != null)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.Error("Session store failed for my accoutn login: " + e.Message, this);
                return false;
            }
        }
    }
}

