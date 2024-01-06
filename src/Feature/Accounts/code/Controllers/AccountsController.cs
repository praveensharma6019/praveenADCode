namespace Sitecore.Feature.Accounts.Controllers
{
    //extern alias itextsharp;
    //using itextsharp::iTextSharp.text;
    //using itextsharp::iTextSharp.text.pdf;
    using CaptchaMvc.HtmlHelpers;
    using DotNetIntegrationKit;
    using Newtonsoft.Json;
    using paytm;
    using RestSharp;
    using SapPiService.Domain;
    using SI_Contactlog_WebService_website;
    using SI_DMUPLDMETRREADService;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Feature.Accounts.Helper;
    using Sitecore.Feature.Accounts.Models;
    using Sitecore.Feature.Accounts.Repositories;
    using Sitecore.Feature.Accounts.Services;
    using Sitecore.Foundation.Alerts.Models;
    using Sitecore.Foundation.Dictionary.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Attributes;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mail;
    using System.Reflection;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.Security;
    using System.Xml;
    using System.Xml.Linq;
    //using itextsharp::iTextSharp.text.pdf.draw;
    //using ListItem = Models.ListItem;

    public class AccountsController : Controller
    {
        public AccountsController(IAccountRepository accountRepository, INotificationService notificationService, IAccountsSettingsService accountsSettingsService, IGetRedirectUrlService getRedirectUrlService, IUserProfileService userProfileService, IFedAuthLoginButtonRepository fedAuthLoginRepository, IUserProfileProvider userProfileProvider, IPaymentService paymentService, IDbAccountService dbAccountService)
        {
            this.FedAuthLoginRepository = fedAuthLoginRepository;
            this.AccountRepository = accountRepository;
            this.NotificationService = notificationService;
            this.AccountsSettingsService = accountsSettingsService;
            this.GetRedirectUrlService = getRedirectUrlService;
            this.UserProfileService = userProfileService;
            this.UserProfileProvider = userProfileProvider;
            this.PaymentService = paymentService;
            this._dbAccountService = dbAccountService;
        }

        private IFedAuthLoginButtonRepository FedAuthLoginRepository { get; }
        private IAccountRepository AccountRepository { get; }
        private INotificationService NotificationService { get; }
        private IAccountsSettingsService AccountsSettingsService { get; }
        private IGetRedirectUrlService GetRedirectUrlService { get; }
        private IUserProfileService UserProfileService { get; }
        private IUserProfileProvider UserProfileProvider;
        private IPaymentService PaymentService { get; }
        private IDbAccountService _dbAccountService;
        public static string UserAlreadyExistsError => DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/User Already Exists", "A user with specified login name already exists");

        public static string UserNameValidation => DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/UserName Validation", "Please enter proper Login Name. It should containcs minimum 6 characters and only alphanumeric characters.");

        private static string ForgotPasswordEmailNotConfigured => DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Email Not Configured", "The Forgot Password E-mail has not been configured");

        private static string UserDoesNotExistError => DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/User Does Not Exist", "User with specified e-mail address does not exist");

        public static Sitecore.Data.Database webDb = Sitecore.Configuration.Factory.GetDatabase("web");

        public ActionResult AccountsMenu()
        {
            var isLoggedIn = Context.IsLoggedIn && Context.PageMode.IsNormal;
            var accountsMenuInfo = new AccountsMenuInfo
            {
                IsLoggedIn = isLoggedIn,
                LoginInfo = !isLoggedIn ? this.CreateLoginInfo() : null,
                UserFullName = isLoggedIn ? Context.User.Profile.FullName : null,
                UserEmail = isLoggedIn ? Context.User.Profile.Email : null,
                AccountsDetailsPageUrl = this.AccountsSettingsService.GetPageLinkOrDefault(Context.Item, Templates.AccountsSettings.Fields.AccountsDetailsPage)
            };
            return this.View(accountsMenuInfo);
        }

        private LoginInfo CreateLoginInfo(string returnUrl = null)
        {
            return new LoginInfo
            {
                ReturnUrl = returnUrl,
                LoginButtons = this.FedAuthLoginRepository.GetAll()
            };
        }

        [HttpGet]
        public ActionResult PANUpdate()
        {
            PANUpdateModel model = new PANUpdateModel();
            return View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult PANUpdate(PANUpdateModel model, FormCollection form, string ValidateCA = null, string UpdatePAN = null, string UpdateGST = null, string ValidateOTP = null, string Submit = null)
        {
            Session["UpdateMessage"] = null;
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            if (!string.IsNullOrEmpty(ValidateCA))
            {
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number required", "Please enter valid Account Number."));
                    return this.View(model);
                }
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }
                var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);
                var consumerPANGSTDetails = SapPiService.Services.RequestHandler.FetchConsumerPANGSTDetails(model.AccountNumber);

                if (string.IsNullOrEmpty(consumerDetails.CANumber) || string.IsNullOrEmpty(consumerPANGSTDetails.CANumber))
                {
                    ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Account Number incorrect", "Account Number is invalid."));
                    return this.View(model);
                }
                else
                {
                    if (string.IsNullOrEmpty(consumerDetails.Mobile))
                    {
                        ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Mobile Number incorrect", "Mobile Number is not registered."));
                        return this.View(model);
                    }
                    model.ConsumerName = consumerDetails.Name;
                    model.MobileNumber = string.IsNullOrEmpty(consumerDetails.Mobile) ? consumerDetails.Mobile : consumerDetails.Mobile.Substring(0, 1) + "xxxxxxx" + consumerDetails.Mobile.Substring(consumerDetails.Mobile.Length - 2);
                    model.EmailId = string.IsNullOrEmpty(consumerDetails.Email) ? consumerDetails.Email : consumerDetails.Email.Substring(0, 2) + "xxxxxxxxxx" + consumerDetails.Email.Substring(consumerDetails.Email.Length - 3);
                    model.PANNumberDisplay = string.IsNullOrEmpty(consumerPANGSTDetails.PANNumber) ? consumerPANGSTDetails.PANNumber : consumerPANGSTDetails.PANNumber.Substring(0, 1) + "xxxxxxx" + consumerPANGSTDetails.PANNumber.Substring(consumerPANGSTDetails.PANNumber.Length - 2);
                    model.GSTNumberDisplay = string.IsNullOrEmpty(consumerPANGSTDetails.GSTNumber) ? consumerPANGSTDetails.GSTNumber : consumerPANGSTDetails.GSTNumber.Substring(0, 1) + "xxxxxxx" + consumerPANGSTDetails.GSTNumber.Substring(consumerPANGSTDetails.GSTNumber.Length - 2);
                    model.IsAccountNumberValidated = true;
                    model.PANNumber = consumerPANGSTDetails.PANNumber;
                    model.GSTNumber = consumerPANGSTDetails.GSTNumber;
                    Session["PANUpdateModel"] = model;
                }
            }
            if (!string.IsNullOrEmpty(UpdatePAN))
            {
                RegistrationRepository registrationRepo = new RegistrationRepository();
                PANUpdateModel accountNumberValidated = (PANUpdateModel)Session["PANUpdateModel"];

                if (model.AccountNumber == accountNumberValidated.AccountNumber)
                {
                    //send SMS for OTP
                    if (!registrationRepo.CheckForCAMaxLimit(model.AccountNumber, "PANUpdate"))
                    {
                        Log.Info("PAN Update: Number of attempt to get OTP reached for AccountNumber." + model.AccountNumber, this);
                        this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/Max20OTPPerLECMobile", "Number of attempt to get OTP reached for Entered value."));
                        return this.View(model);
                    }

                    var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);
                    string generatedotp = registrationRepo.GenerateOTPRegistration(model.AccountNumber, null, "PANUpdate", consumerDetails.Mobile);
                    //send otp via SMS
                    #region Api call to send SMS of OTP
                    try
                    {
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/PANGSTUpdate/PAN Update/OTP API URL",
                            "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Welcome to Adani Electricity. You have initiated a request for PAN/GST update for Account no. {1}. OTP for validation is {2}.&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707161053621273935"), consumerDetails.Mobile, model.AccountNumber, generatedotp);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("ChangeOfNameLECUserRegisteration: OTP Api call success for LEC registration", this);
                            model.IsPANToBeUpdated = true;
                            model.IsAccountNumberValidated = true;
                            model.IsOTPSent = true;
                            Session["PANUpdateModel"] = model;
                            return this.View(model);
                        }
                        else
                        {
                            Log.Error("ChangeOfNameLECUserRegisteration OTP Api call failed for registration", this);
                            this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/OTP sending error", "Unable to send OTP."));
                            model.IsAccountNumberValidated = false;
                            return this.View(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("ChangeOfNameLECUserRegisteration: OTP Api call failed for registration: " + ex.Message, this);
                        this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/OTP sending error", "Unable to send OTP."));
                        model.IsAccountNumberValidated = false;
                        return this.View(model);
                    }
                    #endregion
                }
                else
                {
                    ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Account Number incorrect", "Account Number is invalid."));
                    model.IsAccountNumberValidated = false;
                    return this.View(model);
                }
            }
            if (!string.IsNullOrEmpty(UpdateGST))
            {
                RegistrationRepository registrationRepo = new RegistrationRepository();
                PANUpdateModel accountNumberValidated = (PANUpdateModel)Session["PANUpdateModel"];
                if (model.AccountNumber == accountNumberValidated.AccountNumber)
                {
                    //send SMS for OTP
                    if (!registrationRepo.CheckForCAMaxLimit(model.AccountNumber, "PANUpdate"))
                    {
                        Log.Info("PAN Update: Number of attempt to get OTP reached for AccountNumber." + model.AccountNumber, this);
                        this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/Max20OTPPerLECMobile", "Number of attempt to get OTP reached for Entered value."));
                        return this.View(model);
                    }
                    var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);
                    string generatedotp = registrationRepo.GenerateOTPRegistration(model.AccountNumber, null, "PANUpdate", consumerDetails.Mobile);
                    //send otp via SMS
                    #region Api call to send SMS of OTP
                    try
                    {
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/PANGSTUpdate/PAN Update/OTP API URL",
                            "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Welcome to Adani Electricity. You have initiated a request for PAN/GST update for Account no. {1}. OTP for validation is {2}.&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707161053621273935"), consumerDetails.Mobile, model.AccountNumber, generatedotp);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("ChangeOfNameLECUserRegisteration: OTP Api call success for LEC registration", this);
                            model.IsGSTToBeUpdated = true;
                            model.IsAccountNumberValidated = true;
                            model.IsOTPSent = true;
                            Session["PANUpdateModel"] = model;
                            return this.View(model);
                        }
                        else
                        {
                            Log.Error("ChangeOfNameLECUserRegisteration OTP Api call failed for registration", this);
                            this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/OTP sending error", "Unable to send OTP."));
                            model.IsAccountNumberValidated = false;
                            return this.View(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("ChangeOfNameLECUserRegisteration: OTP Api call failed for registration: " + ex.Message, this);
                        this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/OTP sending error", "Unable to send OTP."));
                        model.IsAccountNumberValidated = false;
                        return this.View(model);
                    }
                    #endregion
                }
                else
                {
                    ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Account Number incorrect", "Account Number is invalid."));
                    model.IsAccountNumberValidated = false;
                    return this.View(model);
                }
            }
            if (!string.IsNullOrEmpty(ValidateOTP))
            {
                if (Session["PANUpdateModel"] == null)
                {
                    this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }

                PANUpdateModel accountNumberValidated = (PANUpdateModel)Session["PANUpdateModel"];
                if (model.AccountNumber == accountNumberValidated.AccountNumber)
                {
                    if (string.IsNullOrEmpty(model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter OTP", "Enter OTP."));
                        model.IsPANToBeUpdated = accountNumberValidated.IsPANToBeUpdated;
                        model.IsGSTToBeUpdated = accountNumberValidated.IsGSTToBeUpdated;
                        model.IsAccountNumberValidated = true;
                        model.IsOTPSent = true;
                        model.IsValidatedOTP = false;
                        return this.View(model);
                    }

                    var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);

                    RegistrationRepository registrationRepo = new RegistrationRepository();
                    string generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(consumerDetails.Mobile, model.AccountNumber, "PANUpdate");

                    if (!string.Equals(generatedOTP, model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                        model.IsPANToBeUpdated = accountNumberValidated.IsPANToBeUpdated;
                        model.IsGSTToBeUpdated = accountNumberValidated.IsGSTToBeUpdated;
                        model.IsAccountNumberValidated = true;
                        model.IsOTPSent = true;
                        model.IsValidatedOTP = false;
                        return this.View(model);
                    }

                    model.IsPANToBeUpdated = accountNumberValidated.IsPANToBeUpdated;
                    model.IsGSTToBeUpdated = accountNumberValidated.IsGSTToBeUpdated;
                    model.IsAccountNumberValidated = true;
                    model.IsOTPSent = true;
                    model.IsValidatedOTP = true;
                    Session["PANUpdateModel"] = model;
                }
            }
            if (!string.IsNullOrEmpty(Submit))
            {
                PANUpdateModel accountNumberValidated = (PANUpdateModel)Session["PANUpdateModel"];
                if (model.AccountNumber == accountNumberValidated.AccountNumber && accountNumberValidated.IsValidatedOTP && accountNumberValidated.IsAccountNumberValidated)
                {
                    model.IsPANToBeUpdated = accountNumberValidated.IsPANToBeUpdated;
                    model.IsGSTToBeUpdated = accountNumberValidated.IsGSTToBeUpdated;
                    model.IsValidatedOTP = true;
                    model.IsAccountNumberValidated = true;
                    if (model.IsPANToBeUpdated && Request.Files["filePANNumber"] == null)
                    {
                        this.ModelState.AddModelError(nameof(model.PANNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/file upload", "Please upload file."));
                        return this.View(model);
                    }
                    if (model.IsGSTToBeUpdated && Request.Files["fileGSTNumber"] == null)
                    {
                        this.ModelState.AddModelError(nameof(model.GSTNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/file upload", "Please upload file."));
                        return this.View(model);
                    }
                    var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);
                    var consumerPANGSTDetails = SapPiService.Services.RequestHandler.FetchConsumerPANGSTDetails(model.AccountNumber);

                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        PANUpdateLog objPANUpdate = new PANUpdateLog();

                        HttpPostedFileBase file = model.IsPANToBeUpdated ? Request.Files["filePANNumber"] : Request.Files["fileGSTNumber"];
                        if (file != null)
                        {
                            if (CheckExtension(file))
                            {
                                Stream fs = file.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                byte[] compressimage = CompressByImageAlg(10, bytes);


                                string image = Convert.ToBase64String(compressimage);
                                string docType = model.IsPANToBeUpdated ? "920" : "998";
                                ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[] returnImgObj = SapPiService.Services.RequestHandler.SelfMeterReadingImageUpload(image, file.FileName, "", "", docType, model.AccountNumber);
                                if (returnImgObj != null)
                                {
                                    if (returnImgObj.Length > 0)
                                    {
                                        if (!returnImgObj[0].TYPE.ToString().ToLower().Equals("s"))
                                        {
                                            if (model.IsPANToBeUpdated)
                                                this.ModelState.AddModelError(nameof(model.PANNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/file upload error", "Unable to upload file."));
                                            else
                                                this.ModelState.AddModelError(nameof(model.GSTNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/file upload error", "Unable to upload file."));
                                            return this.View(model);
                                        }
                                        else
                                        {
                                            //save data in SAP
                                            var result = SapPiService.Services.RequestHandler.UpdatePANGST(model.AccountNumber, consumerDetails.BP_Number, model.IsPANToBeUpdated ? model.PANNumber : null, model.IsGSTToBeUpdated ? model.GSTNumber : null);

                                            if (result.FLAG == "1")
                                            {
                                                //save in db
                                                objPANUpdate.IsPANUpdated = model.IsPANToBeUpdated;
                                                objPANUpdate.IsGSTUpdated = model.IsGSTToBeUpdated;
                                                objPANUpdate.GSTNumber = consumerPANGSTDetails.GSTNumber;
                                                objPANUpdate.PANNumber = consumerPANGSTDetails.PANNumber;
                                                objPANUpdate.UpdatedPANNumber = model.PANNumber;
                                                objPANUpdate.UpdatedGSTNumber = model.GSTNumber;
                                                objPANUpdate.Created_Date = DateTime.Now;
                                                objPANUpdate.AccountNumber = model.AccountNumber;
                                                objPANUpdate.MobileNumber = consumerDetails.Mobile;
                                                objPANUpdate.ContentType = file.ContentType;
                                                objPANUpdate.DocData = compressimage;
                                                objPANUpdate.FileName = file.FileName;
                                                objPANUpdate.ConsumerName = consumerDetails.Name;
                                                objPANUpdate.EmailId = consumerDetails.Email;
                                                dataContext.PANUpdateLogs.InsertOnSubmit(objPANUpdate);
                                                dataContext.SubmitChanges();

                                                //update contact log 
                                                var request = new DT_Contactlog_req
                                                {
                                                    ACTIVITY = model.IsPANToBeUpdated ? "0049" : "0050",
                                                    CLASS = "0002",
                                                    CONTACT_DATE = DateTime.Now.ToString("yyyyMMdd"),
                                                    CONTACT_TIME = DateTime.Now.ToString("HHmmss"),
                                                    DIRECTION = "1",
                                                    PARTNER = "",
                                                    TYPE1 = "009",
                                                    VKONT = model.AccountNumber
                                                };

                                                var response = SapPiService.Services.RequestHandler.CreateContactLogWeb(request);

                                                Sitecore.Diagnostics.Log.Info("PANUpdate Contactlog webservice response for Account: " + model.AccountNumber + ",PANUpdate:" + model.IsPANToBeUpdated + ",Service response: " + response.CONTACT + "," + response.MESSAGE, this);

                                                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/CON/LEC Profile/PAN Updated Successfully", "Updates are done successfully."));
                                                return this.Redirect(this.Request.RawUrl);
                                            }
                                            else
                                            {
                                                if (model.IsPANToBeUpdated)
                                                    this.ModelState.AddModelError(nameof(model.PANNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Data save error", "Unable to update details. " + result.MESSAGE));
                                                else
                                                    this.ModelState.AddModelError(nameof(model.GSTNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Data save error", "Unable to update details. " + result.MESSAGE));
                                                return this.View(model);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (model.IsPANToBeUpdated)
                                    ModelState.AddModelError(nameof(model.PANNumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/File is invalid", "File is invalid."));
                                if (model.IsGSTToBeUpdated)
                                    ModelState.AddModelError(nameof(model.GSTNumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/File is invalid", "File is invalid."));
                                return this.View(model);
                            }
                        }
                    }
                }
                else
                {
                    this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }
            }
            return View(model);
        }

        #region ||** Registration **||

        [RedirectAuthenticated]
        public ActionResult Register()
        {
            var registerInfo = (RegisteredValidateAccount)Session["ValidRegistration"];
            if (registerInfo == null || string.IsNullOrEmpty(registerInfo.AccountNo) || string.IsNullOrEmpty(registerInfo.MeterNo))
            {
                return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.RegistrationPage));
            }

            RegistrationInfo model = new RegistrationInfo();
            model.AccountNo = registerInfo.AccountNo;
            model.MeterNo = registerInfo.MeterNo;
            model.MobileNumber = registerInfo.MobileNo;

            var SecretQuestionList = Context.Database.GetItem(Templates.RegistrationConfig.Datasource.SecretQuestionList);
            var objQuestionList = SecretQuestionList.GetChildren().ToList().Select(x => new SelectListItem()
            {
                Text = x.Fields["Text"].Value,
                Value = x.Fields["Value"].Value,
            }).ToList();
            model.QuestionList = objQuestionList;
            return this.View(model);
        }

        [HttpPost]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationInfo registrationInfo)
        {
            var registerInfo = (RegisteredValidateAccount)Session["ValidRegistration"];
            if (registerInfo == null || registrationInfo.AccountNo != registerInfo.AccountNo)
            {
                return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.RegistrationPage));
            }
            var SecretQuestionList = Context.Database.GetItem(Templates.RegistrationConfig.Datasource.SecretQuestionList);
            var objQuestionList = SecretQuestionList.GetChildren().ToList().Select(x => new SelectListItem()
            {
                Text = x.Fields["Text"].Value,
                Value = x.Fields["Value"].Value,
            }).ToList();
            registrationInfo.QuestionList = objQuestionList;

            if (!ModelState.IsValid)
            {
                return this.View(registrationInfo);
            }

            DateTime dt;
            if (!string.IsNullOrEmpty(registrationInfo.DateofBirth))
            {
                if (!DateTime.TryParseExact(registrationInfo.DateofBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out dt))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.DateofBirth), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/ValidDateofBirth", "Please enter valid date in dd/mm/yyyy format."));
                    return this.View(registrationInfo);
                }
            }

            if (!this.IsCaptchaValid("Captcha Validation Required."))
            {
                ModelState.AddModelError(nameof(registrationInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                return this.View(registrationInfo);
            }

            if (this.AccountRepository.Exists(registrationInfo.LoginName))
            {
                this.ModelState.AddModelError(nameof(registrationInfo.LoginName), UserAlreadyExistsError);

                return this.View(registrationInfo);
            }


            #region Rajdeep(remove method)
            //var accountItemId = this.AccountRepository.GetAccountItemId(registrationInfo.AccountNo);
            //if (!ID.IsNullOrEmpty(accountItemId))
            //{
            //    this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account already exist", "Account Number already registered. Please enter another Account Number."));

            //    return this.View(registrationInfo);
            //}
            #endregion

            #region validate Exist Account number from database
            var isExist = _dbAccountService.IsAccountNumberExist(registrationInfo.AccountNo);
            if (isExist)
            {
                this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account already exist", "Account Number already registered. Please enter another Account Number."));
                return this.View(registrationInfo);
            }
            #endregion

            var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(registrationInfo.AccountNo);
            var data = billinghDetails.MeterNumbers;
            if (billinghDetails != null && billinghDetails.MeterNumbers.Length == 0)
            {
                this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number invalid", "Account Number is not valid. Please enter another Account Number."));
                return this.View(registrationInfo);
            }
            if (!billinghDetails.MeterNumbers.Contains(registrationInfo.MeterNo))
            {
                this.ModelState.AddModelError(nameof(registrationInfo.MeterNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Meter number invalid", "Meter Number is not associated with Account Number. Please enter another Meter Number."));
                return this.View(registrationInfo);
            }

            try
            {
                string errorMessage = this.AccountRepository.RegisterUser(registrationInfo, this.UserProfileService.GetUserDefaultProfileId());

                //var link = this.GetRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Authenticated);
                //return this.Redirect(link);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ModelState.AddModelError(nameof(registrationInfo.Captcha), errorMessage);
                    return this.View(registrationInfo);
                }

                var parentItem = Context.Site.GetStartItem().Parent;
                InternalLinkField link = parentItem.Fields[Templates.AccountsSettings.Fields.AfterLoginPage];
                if (link.TargetItem == null)
                {
                    return this.Redirect(Context.Site.GetRootItem().Url());
                }

                LoginInfo loginInfo = new LoginInfo() { LoginName = registrationInfo.LoginName, Password = registrationInfo.Password, ReturnUrl = link.TargetItem.Url() };
                return this.RegisterLogin(loginInfo, redirectUrl => new RedirectResult(redirectUrl));
            }
            catch (MembershipCreateUserException ex)
            {
                Log.Error($"Can't create user with {registrationInfo.Email}", ex, this);
                this.ModelState.AddModelError(nameof(registrationInfo.LoginName), ex.Message);

                return this.View(registrationInfo);
            }
        }

        [RedirectAuthenticated]
        public ActionResult RegistrationValidate()
        {
            Session["ValidRegistration"] = null;
            RegisteredValidateAccount model = new RegisteredValidateAccount();
            model.isvalidatAccount = false;
            model.isOTPSent = false;
            return this.View(model);
        }

        [HttpPost]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrationValidate(RegisteredValidateAccount registrationInfo, string ValidateAccount = null, string sendOTP = null, string ValidateOTP = null)
        {
            if (!ModelState.IsValid)
            {
                return this.View(registrationInfo);
            }
            if (!string.IsNullOrEmpty(ValidateAccount))
            {
                //if (!this.IsCaptchaValid("Captcha Validation Required."))
                //{
                //    ModelState.AddModelError(nameof(registrationInfo.Captcha), DictionaryPhraseRepository.Current.Get("/BillAmendmentDetails/Captcha required", "Captcha Validation Required."));
                //    return this.View(registrationInfo);
                //}
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(registrationInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    return this.View(registrationInfo);
                }

                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(registrationInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(registrationInfo);
                }

                var isExist = _dbAccountService.IsAccountNumberExist(registrationInfo.AccountNo);
                if (isExist)
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account already exist", "Account Number already registered. Please enter another Account Number."));
                    return this.View(registrationInfo);
                }

                var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(registrationInfo.AccountNo);
                var data = billinghDetails.MeterNumbers;
                if (billinghDetails != null && billinghDetails.MeterNumbers.Length == 0)
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number invalid", "Account Number is not valid. Please enter another Account Number."));
                    return this.View(registrationInfo);
                }
                else if (!billinghDetails.MeterNumbers.Contains(registrationInfo.MeterNo))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.MeterNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Meter number invalid", "Meter Number is not associated with Account Number. Please enter another Meter Number."));
                    return this.View(registrationInfo);
                }
                else
                {
                    registrationInfo.isvalidatAccount = true;
                    string mob = SapPiService.Services.RequestHandler.GetMobileNumber(registrationInfo.AccountNo);
                    registrationInfo.ExistingMobileNo = string.IsNullOrEmpty(mob) ? mob : mob.Substring(0, 2) + "xxxxx" + mob.Substring(mob.Length - 3);
                    registrationInfo.MobileNo = "";
                    //registrationInfo.MobileNo = "9016643434";
                }
            }
            else if (!string.IsNullOrEmpty(sendOTP))
            {
                if (string.IsNullOrEmpty(registrationInfo.MobileNo))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.MeterNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile number invalid", "Please enter valid Mobile Number"));
                    registrationInfo.isvalidatAccount = true;
                    return this.View(registrationInfo);
                }
                else if (!IsCorrectMobileNumber(registrationInfo.MobileNo))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.MeterNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile number invalid", "Please enter valid Mobile Number"));
                    registrationInfo.isvalidatAccount = true;
                    return this.View(registrationInfo);
                }
                else
                {
                    RegistrationRepository registrationRepo = new RegistrationRepository();

                    if (registrationRepo.CheckForCAMaxLimit(registrationInfo.AccountNo, "Registration") == false)
                    {
                        Log.Info("Number of attempt to get OTP reached for Account Number." + registrationInfo.AccountNo, this);
                        this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), DictionaryPhraseRepository.Current.Get("/Registration/Max20OTPPerCA", "Number of attempt to get OTP reached for Account Number."));
                        registrationInfo.isOTPSent = false;
                        return this.View(registrationInfo);
                    }
                    if (registrationRepo.CheckForMobCAMaxLimit(registrationInfo.AccountNo, "Registration", registrationInfo.MobileNo) == false)
                    {
                        Log.Info("Number of attempt to get OTP reached for Account Number and Mobile number." + registrationInfo.AccountNo + ", " + registrationInfo.MobileNo, this);
                        this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), DictionaryPhraseRepository.Current.Get("/Registration/Max20OTPPerCAMob", "Number of attempt to get OTP reached for Account Number and Mobile number."));
                        registrationInfo.isOTPSent = false;
                        return this.View(registrationInfo);
                    }

                    #region Generate New Otp for given mobile number and save to database
                    string generatedotp = registrationRepo.GenerateOTPRegistration(registrationInfo.AccountNo, registrationInfo.MeterNo, "Registration", registrationInfo.MobileNo);
                    #endregion

                    #region Api call to send SMS of OTP
                    try
                    {
                        //var apiurl = string.Format("https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=adtrotpi&subEnterpriseid=adtrotpi&pusheid=adtrotpi&pushepwd=adtrotpi22&msisdn={0}&sender=ADANIR&msgtext=Dear%20customer,%20please%20enter%20the%20verification%20code%20{1}%20to%20submit%20your%20web%20enquiry.", model.mobile, generatedotp);
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/Accounts/Settings/SMS API for registration", "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=Welcome to Adani Electricity, You have initiated a request for new registration for account no. {1}, OTP for this request is: {2}&intflag=false"), registrationInfo.MobileNo, registrationInfo.AccountNo, generatedotp);
                        //var apiurl = string.Format("http://sms2.murlidhar.biz/sendSMS?username=murlidharbizOTP&message=Welcome to Adani Electricity, OTP is {0}&sendername=SOCITY&smstype=TRANS&numbers=+{1}&apikey=9714ac15-f5d3-47be-8e04-47c869d078bd", generatedotp, registrationInfo.MobileNo);
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Error("OTP Api call success for registration", this);
                            this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTP", "OTP Sent."));
                            registrationInfo.isOTPSent = true;
                            registrationInfo.isvalidatAccount = true;
                            return this.View(registrationInfo);
                        }
                        else
                        {
                            Log.Error("OTP Api call failed for registration", this);
                            this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                            registrationInfo.isOTPSent = false;
                            return this.View(registrationInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("OTP Api call failed for registration" + ex.Message, this);
                        this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                        registrationInfo.isOTPSent = false;
                        return this.View(registrationInfo);
                    }
                    #endregion
                }
            }
            else if (!string.IsNullOrEmpty(ValidateOTP))
            {
                RegistrationRepository registrationRepo = new RegistrationRepository();
                string generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(registrationInfo.MobileNo, registrationInfo.AccountNo, "Registration");
                if (!string.Equals(generatedOTP, registrationInfo.OTPNumber))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.OTPNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                    registrationInfo.isOTPSent = true;
                    registrationInfo.isvalidatAccount = true;
                    return this.View(registrationInfo);
                }
                else
                {
                    Session["ValidRegistration"] = registrationInfo;
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.RegistrationPageAfterValidate));
                    //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.RegistrationPage));
                    //Success 
                }
            }
            return View(registrationInfo);
        }

        public bool IsCorrectMobileNumber(String strNumber)
        {
            Regex mobilePattern = new Regex(@"^[0-9]{10}$");
            return mobilePattern.IsMatch(strNumber);
        }

        #endregion

        #region ||** Login / LogOut **||

        [RedirectAuthenticated]
        public ActionResult Login(string returnUrl = null)
        {
            return this.View(this.CreateLoginInfo(returnUrl));
        }

        public ActionResult LoginTeaser()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginInfo loginInfo)
        {
            if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
            {
                ModelState.AddModelError(nameof(loginInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                return this.View(loginInfo);
            }
            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

            if (!reCaptchaResponse.success)
            {
                ModelState.AddModelError(nameof(loginInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                return this.View(loginInfo);
            }

            return this.Login(loginInfo, redirectUrl => new RedirectResult(redirectUrl));
        }

        protected virtual ActionResult Login(LoginInfo loginInfo, Func<string, ActionResult> redirectAction)
        {
            try
            {
                var user = this.AccountRepository.Login(loginInfo.LoginName, loginInfo.Password);
                if (user == null)
                {
                    this.ModelState.AddModelError(nameof(loginInfo.Password), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/User Not Valid", "Username or password is not valid."));
                    return this.View(loginInfo);
                }
                //creating session id for the user login instance
                string sessionId = Guid.NewGuid().ToString();
                SessionHelper.UserSession.UserSessionContext = new DashboardModel
                {
                    userType = UserTypes.Standard,
                    SessionId = sessionId.ToString(),
                    UserName = loginInfo.LoginName
                };

                RegistrationRepository rp = new RegistrationRepository();
                rp.StoreCurrentSession();
                //store cycle number in session 
                //var primaryAccountItemId = user.Profile.GetCustomProperty("Primary Account No");
                //string primaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(primaryAccountItemId);
                //Log.Info("Login Method - SAP service called " + user.Profile.Email, this);
                string primaryAccountNumber = string.Empty;
                primaryAccountNumber = _dbAccountService.GetAccountNumberbyUserName(user.Name);
                if (string.IsNullOrEmpty(primaryAccountNumber))
                {
                    var primaryAccountItemId = user.Profile.GetCustomProperty("Primary Account No");
                    primaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(primaryAccountItemId);
                    if (!string.IsNullOrEmpty(primaryAccountNumber))
                    {
                        Item accountInfo = this.UserProfileService.GetAccountItem(primaryAccountItemId);
                        if (accountInfo != null)
                        {
                            string accountNumber = accountInfo.Fields["Account Number"].Value;
                            string meterNumber = accountInfo.Fields["Meter Number"].Value;
                            _dbAccountService.RegisteredAccountInDb(accountNumber, meterNumber);
                        }
                    }
                }
                var CAType = SapPiService.Services.RequestHandler.GetCycleNumber(primaryAccountNumber);

                if (CAType != null)
                {
                    Log.Info("Login Method - SAP service called for User -  " + user.Profile.Email + " Cycle Number - " + CAType["cycleNumber"], this);
                    SessionHelper.UserSession.UserSessionContext.cycleNumber = CAType["cycleNumber"] ?? string.Empty;
                    SessionHelper.UserSession.UserSessionContext.userType = CAType["customerType"] ?? string.Empty;
                    SessionHelper.UserSession.UserSessionContext.primaryAccountNumber = primaryAccountNumber ?? string.Empty;
                }

                var redirectUrl = loginInfo.ReturnUrl;
                if (string.IsNullOrEmpty(redirectUrl))
                {
                    if (SessionHelper.UserSession.UserSessionContext.userType.ToLower() == UserTypes.Standard.ToLower())
                        redirectUrl = this.GetRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Authenticated);
                    else
                        redirectUrl = this.GetRedirectUrlService.GetRedirectUrl(AuthenticationStatus.PVCAuthenticated);
                }

                return redirectAction(redirectUrl);
            }
            catch (Exception ex)
            {
                Log.Error("Login Method Error - for User - " + loginInfo.LoginName, ex.Message);
                this.ModelState.AddModelError(nameof(loginInfo.Password), ex.Message);
                return this.View(loginInfo);
            }
        }

        protected virtual ActionResult RegisterLogin(LoginInfo loginInfo, Func<string, ActionResult> redirectAction)
        {
            try
            {
                var user = this.AccountRepository.Login(loginInfo.LoginName, loginInfo.Password);
                if (user == null)
                {
                    this.ModelState.AddModelError(nameof(loginInfo.Password), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/User Not Valid", "Username or password is not valid."));
                    return this.View(loginInfo);
                }
                SessionHelper.UserSession.UserSessionContext = new DashboardModel
                {
                    userType = UserTypes.Standard
                };
                //store cycle number in session 

                string primaryAccountNumber = string.Empty;
                primaryAccountNumber = _dbAccountService.GetAccountNumberbyUserName(user.Name);

                if (string.IsNullOrEmpty(primaryAccountNumber))
                {
                    var primaryAccountItemId = user.Profile.GetCustomProperty("Primary Account No");
                    primaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(primaryAccountItemId);
                }
                var CAType = SapPiService.Services.RequestHandler.GetCycleNumber(primaryAccountNumber);

                if (CAType != null)
                {
                    Log.Info("Login Method - SAP service called for User -  " + user.Profile.Email + " Cycle Number - " + CAType["cycleNumber"], this);
                    SessionHelper.UserSession.UserSessionContext.cycleNumber = CAType["cycleNumber"] ?? string.Empty;
                    SessionHelper.UserSession.UserSessionContext.userType = CAType["customerType"] ?? string.Empty;
                    SessionHelper.UserSession.UserSessionContext.primaryAccountNumber = primaryAccountNumber ?? string.Empty;
                    SessionHelper.UserSession.UserSessionContext.UserName = user.Profile.UserName;
                }

                var redirectUrl = loginInfo.ReturnUrl;
                if (string.IsNullOrEmpty(redirectUrl))
                {
                    if (SessionHelper.UserSession.UserSessionContext.userType.ToLower() == UserTypes.Standard.ToLower())
                        redirectUrl = this.GetRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Authenticated);
                    else
                        redirectUrl = this.GetRedirectUrlService.GetRedirectUrl(AuthenticationStatus.PVCAuthenticated);
                }

                return redirectAction(redirectUrl);
            }
            catch (Exception ex)
            {
                Log.Error("Login Method Error - for User - " + loginInfo.LoginName, ex.Message);
                this.ModelState.AddModelError(nameof(loginInfo.Password), ex.Message);
                return this.View(loginInfo);
            }
        }

        [HttpPost]
        [ValidateModel]
        public ActionResult _Login(LoginInfo loginInfo)
        {
            this.Session["UpdateMessage"] = null;
            return this.Login(loginInfo, redirectUrl => this.Json(new LoginResult
            {
                RedirectUrl = redirectUrl
            }));
        }

        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        public ActionResult LoginTeaser(LoginInfo loginInfo)
        {
            return this.Login(loginInfo, redirectUrl => new RedirectResult(redirectUrl));
        }

        [HttpPost]
        public ActionResult Logout()
        {
            //Session.Abandon();
            this.AccountRepository.Logout();
            this.Session["UpdateMessage"] = null;
            this.Session["ComplaintRegistrationModel"] = null;
            SessionHelper.UserSession.AEMLComplaintUserSessionContext = null;
            SessionHelper.UserSession.UserSessionContext = null;

            var parentItem = Context.Site.GetStartItem().Parent;
            InternalLinkField link = parentItem.Fields[Templates.AccountsSettings.Fields.LoginPage];
            if (link.TargetItem == null)
            {
                return this.Redirect(Context.Site.GetRootItem().Url());
            }

            return this.Redirect(link.TargetItem.Url());

        }

        [HttpGet]
        public ActionResult SelectEnquiryType(string EnquiryType)
        {
            if (EnquiryType == "CNG")
                return RedirectToAction("NewInquiryCNG");
            else
                return RedirectToAction("NewEnquiry");
        }

        [HttpPost]
        public ActionResult SelectEnquiryType1()
        {

            return RedirectToAction("NewEnquiry");
        }

        [HttpPost]
        public bool LogoutSessionOnTabclose()
        {
            this.AccountRepository.Logout();
            this.Session["UpdateMessage"] = null;
            SessionHelper.UserSession.UserSessionContext = null;

            var parentItem = Context.Site.GetStartItem().Parent;
            InternalLinkField link = parentItem.Fields[Templates.AccountsSettings.Fields.LoginPage];
            return true;
        }

        #endregion

        #region ||** Forgot Password **||


        [RedirectAuthenticated]
        public ActionResult ForgotPassword()
        {
            return this.View();
        }

        [HttpPost]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordInfo model)
        {
            try
            {
                //if (!this.IsCaptchaValid("Captcha Validation Required."))
                //{
                //    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                //    return this.View(model);
                //}

                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    return this.View(model);
                }

                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }

                if (string.IsNullOrEmpty(model.LoginName) && string.IsNullOrEmpty(model.AccountNo) && string.IsNullOrEmpty(model.Email))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/forgot required parameter", "Please enter username or email and account number."));
                    return this.View(model);
                }


                string ItemId = string.Empty;
                if (!string.IsNullOrEmpty(model.LoginName))
                {
                    if (!this.AccountRepository.Exists(model.LoginName))
                    {
                        ModelState.AddModelError(nameof(model.LoginName), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/LoginName not exists", "Login Name is not Valid. Please enter valid Login Name."));
                        return this.View(model);
                    }
                    var resetpasswordusers = this.AccountRepository.GetUserbyLoginName(model.LoginName);
                    if (resetpasswordusers != null)
                    {
                        ItemId = resetpasswordusers.GetId().ToString();

                        string token = Token();
                        resetpasswordusers.Profile.SetCustomProperty("PasswordResetToken", token);
                        resetpasswordusers.Profile.SetCustomProperty("TokenGenerationDate", Sitecore.DateUtil.ToIsoDate(Convert.ToDateTime(DateTime.UtcNow)));
                        resetpasswordusers.Profile.Save();

                        var BaseUrl = this.UserProfileService.GetPageURL(Templates.Pages.ResetPassword);
                        if (!string.IsNullOrEmpty(BaseUrl))
                        {
                            string RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + BaseUrl + "?token=" + token + "&&userId=" + ItemId + "";
                            this.NotificationService.SendPasswordResetLink(resetpasswordusers.Profile.Email, model.LoginName, RedirectUrl);
                        }
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Forgot Password Success", "password reset link is sent to your mail address."));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.AccountNo))
                    {
                        ModelState.AddModelError(nameof(model.AccountNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/forgot-emailaccountvalidation", "Please Enter Account number and Email Address."));
                        return this.View(model);
                    }

                    var resetpasswordusers = this.AccountRepository.GetUserbyEmail_Account(model.Email, model.AccountNo);
                    if (resetpasswordusers != null)
                    {
                        ItemId = resetpasswordusers.GetId().ToString();
                        string loginName = resetpasswordusers.Profile.UserName;
                        if (loginName.LastIndexOf('/') >= 0)
                        {
                            loginName = loginName.Substring(loginName.LastIndexOf('/') + 1);
                        }
                        string token = Token();
                        resetpasswordusers.Profile.SetCustomProperty("PasswordResetToken", token);
                        resetpasswordusers.Profile.SetCustomProperty("TokenGenerationDate", Sitecore.DateUtil.ToIsoDate(Convert.ToDateTime(DateTime.UtcNow)));
                        resetpasswordusers.Profile.Save();

                        var BaseUrl = this.UserProfileService.GetPageURL(Templates.Pages.ResetPassword);
                        if (!string.IsNullOrEmpty(BaseUrl))
                        {
                            string RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + BaseUrl + "?token=" + token + "&&userId=" + ItemId + "";
                            this.NotificationService.SendPasswordResetLink(resetpasswordusers.Profile.Email, loginName, RedirectUrl);
                        }
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Forgot Password Success", "password reset link is sent to your mail address."));
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.AccountNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Email Account Mismatch", "Email Address and Account number not matched. Please enter associated account number with email address."));
                        return this.View(model);

                        //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Email Account Mismatch", "Email Address and Account number not matched. Please enter asscoiated acocunt number with email address."));
                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error($"Can't reset password for user {model.AccountNo}", ex, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Exception", ex.Message));
            }
            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.Pages.ForgotPassword));

        }

        #endregion

        #region ||** Guest Meter Reading **||

        [RedirectAuthenticated]
        public ActionResult QAMeterReading()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateModel]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult QAMeterReading(QAMeterReading model)
        {
            try
            {
                //if (!this.IsCaptchaValid("Captcha Validation Required."))
                //{
                //    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                //    return this.View(model);
                //}

                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    return this.View(model);
                }

                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }

                //var ItemId = this.AccountRepository.GetAccountItemId(model.AccountNo);

                //if (!ID.IsNullOrEmpty(ItemId))
                //{

                var CAType = SapPiService.Services.RequestHandler.GetCycleNumber(model.AccountNo);

                if (CAType != null && !string.IsNullOrEmpty(CAType["cycleNumber"]))
                {
                    System.Web.HttpContext.Current.Session["QuickAccessCycleNumber"] = CAType["cycleNumber"] ?? string.Empty;
                    System.Web.HttpContext.Current.Session["QuickAccessprimaryAccountNumber"] = model.AccountNo;
                }
                else
                {
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account not exist", "Account number not available in the system."));
                    return View(model);
                }
                return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.Pages.GuestUserMeterReadingDate));

                // }
                //else
                //{
                //    //Note : return Message of Invlid Account Number.
                //    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account not exist", "Account number not available."));
                //    return View(model);
                //}

            }
            catch (Exception ex)
            {
                this.Session["UpdateMessage"] = ex.Message + " " + ex.InnerException;
                //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/technical issue", "There is technical issue. Please try after sometime."));
                Log.Error($"Can't get Detail for user {model.AccountNo}", ex, this);
            }
            return View(model);

        }

        public ActionResult GuestMeterReadingBody()
        {
            //System.Web.HttpContext.Current.Session["QuickAccessCycleNumber"] = "5";
            var model = new QAMeterReading()
            {

                CycleNumber = System.Web.HttpContext.Current.Session["QuickAccessCycleNumber"] != null ? System.Web.HttpContext.Current.Session["QuickAccessCycleNumber"].ToString() : string.Empty,
                AccountNo = System.Web.HttpContext.Current.Session["QuickAccessprimaryAccountNumber"] != null ? System.Web.HttpContext.Current.Session["QuickAccessprimaryAccountNumber"].ToString() : string.Empty,
                MonthList = getMonthsWithCurrentYear(),
                Monthval = getMonthsWithCurrentYear().FirstOrDefault().Value
            };
            //ViewBag.Sessionvalue = System.Web.HttpContext.Current.Session["QuickAccessCycleNumber"].ToString();
            return this.View("GuestMeterReadingBody", model);

        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        [RedirectAuthenticated]
        public ActionResult GuestMeterReadingBody(QAMeterReading model)
        {
            //Note : Get Available List of Items from 	EBILL MRDATE
            Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = master.GetItem(new Data.ID(Templates.ItemList.MeterReadingItemList.ToString()));

            var AvailableItem = itemInfo.GetChildren().ToList().FirstOrDefault(w => w.Fields[Templates.MeterReadingProperties.CYCLE.ToString()].Value == model.CycleNumber && w.Fields[Templates.MeterReadingProperties.BILLMONTH.ToString()].Value == model.Monthval);
            if (AvailableItem != null)
                model.ScheduleMeterReadingdate = AvailableItem.Fields[Templates.MeterReadingProperties.FULLMETERREADINGDATE.ToString()].Value;
            else
                model.ScheduleMeterReadingdate = string.Empty;

            model.MonthList = getMonthsWithCurrentYear();
            model.Monthval = getMonthsWithCurrentYear().FirstOrDefault().Value;
            return this.View("GuestMeterReadingBody", model);
        }
        #endregion

        #region ||** Reset Password **||
        [RedirectAuthenticated]
        public ActionResult ResetPassword(string token = null, string userId = null)
        {
            ResetPasswordInfo model = new ResetPasswordInfo();
            if (!string.IsNullOrEmpty(userId))
            {
                //Note : Write Logic of Finding User Based on UserId
                //PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext();
                //UserProfile userFromUserProfile = dataContext.UserProfiles.Where(u => u.UserName == model.UserID).FirstOrDefault();
                //if (userFromUserProfile != null && userFromUserProfile.LastPasswordChangedDate!=null)
                //{
                //    if (userFromUserProfile.LastPasswordChangedDate.Value.Date == DateTime.Now.Date)
                //    {
                //        model.Token = string.Empty;
                //        model.UserID = string.Empty;
                //        model.IsValidRequest = false;
                //        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Token Expired", "Reset Link Expired"));
                //    }
                //}
                var getusers = this.AccountRepository.GetUserByUserID(userId);
                if (getusers != null)
                {
                    var user = getusers;
                    //Note : Write Logic of getting Token Value for user and validate token.
                    var PasswordResetToken = user.Profile.GetCustomProperty("PasswordResetToken");
                    var TokenGenerationDate = user.Profile.GetCustomProperty("TokenGenerationDate");
                    model.IsValidRequest = true;
                    if (!string.IsNullOrEmpty(PasswordResetToken) && string.Equals(token, PasswordResetToken))
                    {
                        model.UserID = userId;
                        model.Token = token;
                        if (IsResetLinkExpired(TokenGenerationDate))
                        {
                            model.Token = string.Empty;
                            model.UserID = string.Empty;
                            model.IsValidRequest = false;
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Token Expired", "Reset Link Expired"));
                        }
                    }
                    else
                    {
                        model.Token = string.Empty;
                        model.UserID = string.Empty;
                        model.IsValidRequest = false;
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Invalid Token", "Invalid Token"));
                    }
                }
            }
            return this.View("ResetPassword", model);
        }

        [HttpPost]
        [ValidateModel]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordInfo model)
        {
            try
            {
                //if (!this.IsCaptchaValid("Captcha Validation Required."))
                //{
                //    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                //    return this.View(model);
                //}

                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    return this.View(model);
                }

                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }

                if (!string.IsNullOrEmpty(model.UserID) && !string.IsNullOrEmpty(model.Token))
                {
                    var userdetail = this.AccountRepository.GetUserByUserID(model.UserID);

                    //Note : Write Logic of Resetting Password of UserId
                    var user = System.Web.Security.Membership.GetUser(userdetail.Name);

                    var PasswordResetToken = userdetail.Profile.GetCustomProperty("PasswordResetToken");
                    var TokenGenerationDate = userdetail.Profile.GetCustomProperty("TokenGenerationDate");
                    model.IsValidRequest = true;
                    if (!string.IsNullOrEmpty(PasswordResetToken) && string.Equals(model.Token, PasswordResetToken))
                    {
                        string oldPassword = user.ResetPassword();
                        user.ChangePassword(oldPassword, model.Password);

                        //Note : Write Logic of Destroying old Token
                        userdetail.Profile.SetCustomProperty("PasswordResetToken", string.Empty);
                        userdetail.Profile.SetCustomProperty("TokenGenerationDate", string.Empty);
                        userdetail.Profile.Save();
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Reset Success", "Password Reset Successfully"));

                        PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext();
                        UserProfile userFromUserProfile = dataContext.UserProfiles.Where(u => u.UserName == userdetail.Profile.UserName).FirstOrDefault();
                        if (userFromUserProfile != null)
                        {
                            userFromUserProfile.LastPasswordChangedDate = DateTime.Now;
                            dataContext.SubmitChanges();
                        }
                    }
                    else
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Reset Failure msg", "Invalid Password Change Request."));
                    }
                }
                else
                {
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Reset Failure msg", "Invalid Password Change Request."));
                }
            }
            catch (Exception ex)
            {
                this.Session["UpdateMessage"] = ex.Message;
                Log.Error($"Can't get Detail for user {model.UserID}", ex, this);
            }
            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.Pages.ResetPassword));
        }

        #endregion

        [RedirectUnauthenticated]
        public ActionResult WelcomeMessage()
        {
            return this.View("WelcomeMessage", new EditProfile() { LoginName = this.UserProfileService.GetLoginName() });
        }

        [RedirectUnauthenticated]
        public ActionResult LeftPanel()
        {
            var userType = UserTypes.Standard;
            if (SessionHelper.UserSession.UserSessionContext != null)
                userType = SessionHelper.UserSession.UserSessionContext.userType;
            return this.View("LeftPanel", new EditProfile() { UserType = userType.ToLower() });
        }

        #region ||** Update Profile **||


        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult PVCHome()
        {
            SapPiService.Domain.PvcDetail model = new SapPiService.Domain.PvcDetail();
            try
            {
                if (SessionHelper.UserSession.UserSessionContext != null && SessionHelper.UserSession.UserSessionContext.userType != UserTypes.Premium)
                {
                    var parentItem = Context.Site.GetStartItem().Parent;
                    InternalLinkField link = parentItem.Fields[Templates.AccountsSettings.Fields.AfterLoginPage];
                    if (link.TargetItem == null)
                    {
                        return this.Redirect(Context.Site.GetRootItem().Url());
                    }
                }

                var profile = this.UserProfileService.GetProfile(Context.User);
                model = SapPiService.Services.RequestHandler.FetchPvcDetail(profile.AccountNumber);
            }
            catch (Exception ex)
            {
                Log.Error("PVCHome Method Error - for User", ex.Message);
            }
            return this.View(model);
        }


        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult EditMainContent()
        {
            if (!Context.PageMode.IsNormal)
            {
                return this.View(this.UserProfileService.GetEmptyProfile());
            }

            var profile = this.UserProfileService.GetProfile(Context.User);

            return this.View(profile);
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [RedirectUnauthenticated]
        [ValidateAntiForgeryToken]
        public ActionResult EditMainContent(EditProfile profile, string submit = null, string ValidateOTP = null)
        {
            var loggedinuserprofile = this.UserProfileService.GetProfile(Context.User);

            if (loggedinuserprofile.AccountNumber != profile.AccountNumber)
            {
                Sitecore.Diagnostics.Log.Info("Error at EditMainContent loggedinuserprofile.AccountNumber != profile.AccountNumber:" + loggedinuserprofile.AccountNumber + ":" + profile.AccountNumber, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile update error", "Profile was not successfully updated, please try again!"));

                return this.Redirect(this.Request.RawUrl);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(profile);
            }
            DateTime dt;
            if (!string.IsNullOrEmpty(profile.DateofBirth))
            {
                if (!DateTime.TryParseExact(profile.DateofBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out dt))
                {
                    //   this.ModelState.AddModelError(profile.DateofBirth, DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/ValidDateofBirth", "Please enter valid date in dd/mm/yyyy format."));
                    //                    return this.View(profile);
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/ValidDateofBirth", "Please enter valid date in dd/mm/yyyy format."));
                    return this.Redirect(this.Request.RawUrl);
                }

            }

            if (!string.IsNullOrEmpty(profile.MobileNumber))
            {
                if (submit != null)
                {
                    RegistrationRepository registrationRepo = new RegistrationRepository();

                    if (registrationRepo.CheckForCAMaxLimit(profile.AccountNumber, "UpdateProfile") == false)
                    {
                        Log.Info("Number of attempt to get OTP reached for Account Number." + profile.AccountNumber, this);
                        this.ModelState.AddModelError(nameof(profile.MobileNumber), DictionaryPhraseRepository.Current.Get("/Registration/Max20OTPPerCA", "Number of attempt to get OTP reached for Account Number."));
                        profile.isOTPSent = false;
                        return this.View(profile);
                    }
                    if (registrationRepo.CheckForMobCAMaxLimit(profile.AccountNumber, "UpdateProfile", profile.MobileNumber) == false)
                    {
                        Log.Info("Number of attempt to get OTP reached for Account Number and Mobile number." + profile.AccountNumber + ", " + profile.MobileNumber, this);
                        this.ModelState.AddModelError(nameof(profile.MobileNumber), DictionaryPhraseRepository.Current.Get("/Registration/Max20OTPPerCAMob", "Number of attempt to get OTP reached for Account Number and Mobile number."));
                        profile.isOTPSent = false;
                        return this.View(profile);
                    }

                    #region Generate New Otp for given mobile number and save to database
                    string generatedotp = registrationRepo.GenerateOTPRegistration(profile.AccountNumber, null, "UpdateProfile", profile.MobileNumber);
                    #endregion

                    #region Api call to send SMS of OTP
                    try
                    {
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/Accounts/Settings/SMS API for Profile update", "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=You have initiated a request to update mobile number for account no. {1}, OTP for this request is: {2}&intflag=false"), profile.MobileNumber, profile.AccountNumber, generatedotp);
                        //var apiurl = string.Format("http://sms2.murlidhar.biz/sendSMS?username=murlidharbizOTP&message=Welcome to Adani Electricity, OTP is {0}&sendername=SOCITY&smstype=TRANS&numbers=+{1}&apikey=9714ac15-f5d3-47be-8e04-47c869d078bd", generatedotp, profile.MobileNumber);
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Error("OTP Api call success for registration", this);
                            this.ModelState.AddModelError(nameof(profile.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTP", "OTP Sent."));
                            profile.isOTPSent = true;
                            return this.View(profile);
                        }
                        else
                        {
                            Log.Error("OTP Api call failed for registration", this);
                            this.ModelState.AddModelError(nameof(profile.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                            profile.isOTPSent = false;
                            return this.View(profile);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"{0}", ex, this);
                        Log.Error("OTP Api call failed for registration", this);
                        this.ModelState.AddModelError(nameof(profile.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                        profile.isOTPSent = false;
                        return this.View(profile);
                    }
                    #endregion
                }
                else
                {
                    //validate OTP and submit
                    RegistrationRepository registrationRepo = new RegistrationRepository();
                    string generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(profile.MobileNumber, profile.AccountNumber, "UpdateProfile");
                    if (!string.Equals(generatedOTP, profile.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(profile.OTPNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                        profile.isOTPSent = true;
                        profile.isOTPValidated = false;
                        return this.View(profile);
                    }
                    else
                    {
                        this.UserProfileService.SaveProfile(Context.User.Profile, profile);
                        string result1 = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(profile.AccountNumber, profile.MobileNumber, profile.PhoneNumber, profile.Email, null);

                        Log.Info("Contact details update:" + profile.AccountNumber + "," + profile.MobileNumber + "," + profile.Email, this);
                        if (result1 == "success")
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile Success", "Profile was successfully updated"));
                        else
                        {
                            Sitecore.Diagnostics.Log.Error("Profile update Error at " + MethodBase.GetCurrentMethod().Name + ":" + result1, this);
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile update error", "Profile was not successfully updated, please try again!"));
                        }
                        return this.Redirect(this.Request.RawUrl);
                    }
                }
            }

            this.UserProfileService.SaveProfile(Context.User.Profile, profile);
            string result = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(profile.AccountNumber, profile.MobileNumber, profile.PhoneNumber, profile.Email, null);
            if (result == "success")
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile Success", "Profile was successfully updated"));
            else
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + result, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile update error", "Profile was not successfully updated, please try again!"));
            }
            return this.Redirect(this.Request.RawUrl);

        }


        [RedirectUnauthenticated]
        public ActionResult MyAccount()
        {
            return this.View();
        }

        #endregion

        #region ||** Change Passsword **||

        [RedirectUnauthenticated]
        public ActionResult ChangePasswordBody()
        {
            return this.View("ChangePasswordBody", new ChangePassword() { LoginName = this.UserProfileService.GetLoginName(), AccountNumber = GetPrimaryAccountNumber() });
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [RedirectUnauthenticated]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordBody(ChangePassword changePassword)
        {
            var logininfo = this.AccountRepository.Login(changePassword.LoginName, changePassword.OldPassword);
            if (logininfo == null)
            {
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Change Password Failure", "Old Password is InCorrect."));
                return this.Redirect(this.Request.RawUrl);
            }
            var user = System.Web.Security.Membership.GetUser(Context.User.Profile.UserName);
            string oldPassword = user.ResetPassword();
            user.ChangePassword(oldPassword, changePassword.Password);
            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Change Password Success", "Password Changed successfully"));
            return this.Redirect(this.Request.RawUrl);
        }
        #endregion

        #region ||** Registered Account **||
        [RedirectUnauthenticated]
        public ActionResult RegisteredAccountBody()
        {
            return this.View("RegisteredAccountBody", new RegisteredAccount() { MasterAccountNumber = GetPrimaryAccountNumber() });
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [RedirectUnauthenticated]
        [ValidateAntiForgeryToken]
        public ActionResult RegisteredAccountBody(RegisteredAccount registeredAccount)
        {
            // NOTE : Authenticate User
            var userDetail = this.AccountRepository.GetUser(registeredAccount.ExistingUserId, registeredAccount.Password);
            if (userDetail == null)
            {
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Registered Account Wrong Password", "Invalid Account User Id or Password"));
                return this.Redirect(this.Request.RawUrl);
            }
            else
            {
                //RegisteredAccount using Database 
                //first check if acnt is present in Db then Registered account otherwise using sitecore default
                bool checkIfPresent = _dbAccountService.CheckAccountInDbPresent(registeredAccount.ExistingAcNo, userDetail.Profile.UserName);
                if (checkIfPresent)
                {
                    _dbAccountService.DeleteAndInertBasedOnPrimary(registeredAccount.ExistingAcNo, userDetail);
                }
                else
                {

                    var MultipleAccountItemId = userDetail.Profile.GetCustomProperty("Multiple Account");
                    List<string> MultipleAccountList = new List<string>();
                    if (MultipleAccountItemId.Contains('|'))
                    {
                        MultipleAccountList = MultipleAccountItemId.Split('|').ToList();
                    }


                    // NOTE : Retrieving Primary Account Number of another Existing User
                    var primaryAccountItemId = userDetail.Profile.GetCustomProperty("Primary Account No");
                    string primaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(primaryAccountItemId);

                    if (MultipleAccountList.Count > 1 && primaryAccountNumber.Equals(registeredAccount.ExistingAcNo)) // Primary Account Merge But Secondary Account become Primary
                    {
                        Context.User.Profile.SetCustomProperty("Multiple Account", Context.User.Profile.GetCustomProperty("Multiple Account") + "|" + primaryAccountItemId.ToString());
                        Context.User.Profile.Save();

                        //NOTE : Remove Primary Account Number from List and Set Next List Item as Primary Account Number.
                        MultipleAccountList.Remove(primaryAccountItemId);
                        userDetail.Profile.SetCustomProperty("Primary Account No", MultipleAccountList.FirstOrDefault());

                        //NOTE : Set "Multliple Account" Property After Removal of one Property Item.
                        userDetail.Profile.SetCustomProperty("Multiple Account", string.Join("|", MultipleAccountList.ToArray()));
                        userDetail.Profile.Save();
                    }
                    else if (!MultipleAccountList.Any() && primaryAccountNumber.Equals(registeredAccount.ExistingAcNo)) // Only One account primary Account
                    {
                        Context.User.Profile.SetCustomProperty("Multiple Account", Context.User.Profile.GetCustomProperty("Multiple Account") + "|" + primaryAccountItemId.ToString());
                        Context.User.Profile.Save();

                        // NOTE : Delete User once Account is Merged with LoggedIn user
                        userDetail.Delete();
                    }
                    else if (MultipleAccountList.Any()) // Secondary Account merge with logged In user
                    {
                        Boolean count = false;
                        foreach (var item in MultipleAccountList)
                        {
                            string secondaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(item);
                            if (secondaryAccountNumber.Equals(registeredAccount.ExistingAcNo))
                            {
                                Context.User.Profile.SetCustomProperty("Multiple Account", Context.User.Profile.GetCustomProperty("Multiple Account") + "|" + item);
                                Context.User.Profile.Save();
                                count = true;
                                //NOTE : Remove Primary Account Number from List and Set Next List Item as Primary Account Number.
                                MultipleAccountList.Remove(item);
                                break;
                            }
                        }
                        if (count == false)
                        {
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account not exist", "Account number not available."));
                            return this.Redirect(this.Request.RawUrl);
                        }
                        else
                        {
                            //NOTE : Set "Multliple Account" Property After Removal of one Property Item.
                            userDetail.Profile.SetCustomProperty("Multiple Account", string.Join("|", MultipleAccountList.ToArray()));
                            userDetail.Profile.Save();
                        }
                    }
                    else
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account not exist", "Account number not available."));
                        return this.Redirect(this.Request.RawUrl);
                    }
                }//End else of if present in DB
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Registered Account Successfull", "Account Merged successfully."));
                return this.Redirect(this.Request.RawUrl);
            }
        }
        #endregion

        #region ||** Switch Account **||

        [RedirectUnauthenticated]
        public ActionResult SwitchAccountBody()
        {

            #region New Code

            #region variable declaration
            var model = new SwitchAccount();
            var availableitemsinDB = _dbAccountService.GetAccountListbyusername(Context.User.Profile.UserName);
            List<KeyValuePair<string, string>> AccountNumberList = new List<KeyValuePair<string, string>>();
            string primaryAccountNumber = string.Empty;

            #endregion

            //// NOTE : check availability first in custom DB if data not available then check in to Sitecore
            if (availableitemsinDB != null && availableitemsinDB.Any())
            {
                #region Get Accounts List based on Logged in user
                var selecteditems = availableitemsinDB.Select(s => new { s.AccountNumber, s.Id }).ToList();
                #endregion

                #region Get primary Account number of Logged in user
                primaryAccountNumber = GetPrimaryAccountNumber();//availableitemsinDB.Where(w => w.IsPrimary == true).Select(s => s.AccountNumber).FirstOrDefault();
                #endregion

                if (selecteditems.Any())
                {
                    foreach (var item in selecteditems)
                    {
                        if (!string.Equals(primaryAccountNumber, item.AccountNumber))
                        {
                            if (!string.IsNullOrEmpty(item.AccountNumber))
                            {
                                AccountNumberList.Add(new KeyValuePair<string, string>(Convert.ToString(item.Id), item.AccountNumber));
                            }

                        }
                    }
                }
                model.AccountList = AccountNumberList;
                model.MasterAccountNumber = GetPrimaryAccountNumber();//_dbAccountService.GetPrimaryAccountNumberbyUserName(Context.User.Profile.UserName);
            }
            else
            {
                var MultipleAccountItemId = Context.User.Profile.GetCustomProperty("Multiple Account");
                List<string> MultipleAccountItemIdList = new List<string>();

                // NOTE : Retrieving Primary Account Number of  User
                var primaryAccountItemId = Context.User.Profile.GetCustomProperty("Primary Account No");
                primaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(primaryAccountItemId);

                if (MultipleAccountItemId.Contains('|'))
                {
                    MultipleAccountItemIdList = MultipleAccountItemId.Split('|').ToList();
                }
                if (MultipleAccountItemIdList.Any())
                {
                    string secondaryAccountNumber = string.Empty;
                    foreach (var secondaryAccountItemId in MultipleAccountItemIdList)
                    {
                        if (!string.Equals(primaryAccountItemId, secondaryAccountItemId))
                        {
                            secondaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(secondaryAccountItemId);
                            if (!string.IsNullOrEmpty(secondaryAccountNumber))
                            {
                                AccountNumberList.Add(new KeyValuePair<string, string>(secondaryAccountItemId, secondaryAccountNumber));
                            }

                        }

                    }
                }
                model.AccountList = AccountNumberList;
                model.MasterAccountNumber = GetPrimaryAccountNumber();
            }
            #endregion

            return this.View("SwitchAccountBody", model);
        }

        [RedirectUnauthenticated]
        public ActionResult SwitchAccountInternally(string ItemId)
        {
            var item = Context.Database.GetItem(Templates.Pages.SwitchAccount);
            var url = item.Url();
            string primaryAccountNumber = string.Empty;
            try
            {
                //// NOTE : check availability first in custom DB if data not available then check in to Sitecore
                var availableitemsinDB = _dbAccountService.GetAccountDetailbyId(ItemId);
                if (availableitemsinDB != null)
                {
                    #region Set Selected ItemId as Primary Account
                    _dbAccountService.SwitchAccountAsPrimary(ItemId);
                    #endregion

                    primaryAccountNumber = availableitemsinDB.AccountNumber;
                }
                else
                {
                    //NOTE : Update  "Primary Account Number" Property Switch.
                    Context.User.Profile.SetCustomProperty("Primary Account No", ItemId);
                    Context.User.Profile.Save();
                    primaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(ItemId);
                }

                var CAType = SapPiService.Services.RequestHandler.GetCycleNumber(primaryAccountNumber);
                if (CAType != null)
                {
                    SessionHelper.UserSession.UserSessionContext.cycleNumber = CAType["cycleNumber"] ?? string.Empty;
                    SessionHelper.UserSession.UserSessionContext.userType = CAType["customerType"] ?? string.Empty;
                    SessionHelper.UserSession.UserSessionContext.primaryAccountNumber = primaryAccountNumber ?? string.Empty;
                }

                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Switch Account Successfull", "Account Switched Successfully"));
            }
            catch (Exception ex)
            {
                Log.Error("Method SwitchAccountInternally -", ex.Message);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Switch Account Failure", "Something Went Wrong."));
            }
            return RedirectPermanent(url);
        }
        #endregion


        #region ||** Non Registered Account **||
        [RedirectUnauthenticated]
        public ActionResult NonRegisteredAccountBody()
        {
            return this.View("NonRegisteredAccountBody", new NonRegisteredAccount() { MasterAccountNumber = GetPrimaryAccountNumber() });
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [RedirectUnauthenticated]
        [ValidateAntiForgeryToken]
        public ActionResult NonRegisteredAccountBody(NonRegisteredAccount nonregisteredAccount)
        {
            try
            {
                if (string.IsNullOrEmpty(nonregisteredAccount.Accountnumber) || string.IsNullOrEmpty(nonregisteredAccount.MeterNumber))
                {
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Required", "Please enter Account and Meter Number."));
                    return this.Redirect(this.Request.RawUrl);
                }
                else
                {
                    var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(nonregisteredAccount.Accountnumber);
                    var data = billinghDetails.MeterNumbers;
                    if (billinghDetails != null && billinghDetails.MeterNumbers.Length == 0)
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number invalid", "Account Number is not valid. Please enter another Account Number."));
                        return this.Redirect(this.Request.RawUrl);
                    }
                    if (!billinghDetails.MeterNumbers.Contains(nonregisteredAccount.MeterNumber))
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Meter number invalid", "Meter Number is not associated with Account Number. Please enter another Meter Number."));
                        return this.Redirect(this.Request.RawUrl);
                    }

                    var accountItemId = this.AccountRepository.GetAccountItemId(nonregisteredAccount.Accountnumber);
                    //check if acnt number is in db
                    if (_dbAccountService.IsAccountNumberExist(nonregisteredAccount.Accountnumber))
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Already Exist", "This account is already registered."));
                        return this.Redirect(this.Request.RawUrl);
                    }
                    else
                    {
                        //remove sitecore multiple acnt code #OLD Code
                        //var newAccount = this.AccountRepository.CreateAccountItem(nonregisteredAccount.Accountnumber, nonregisteredAccount.MeterNumber);
                        //if (Context.User.Profile.GetCustomProperty("Multiple Account").Contains(newAccount.ID.ToString()))
                        //{
                        //    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Already Added", "This account is already added in your list."));
                        //}
                        //else
                        //{
                        //    Context.User.Profile.SetCustomProperty("Multiple Account", Context.User.Profile.GetCustomProperty("Multiple Account") + "|" + newAccount.ID.ToString());
                        //    Context.User.Profile.Save();
                        //    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Successfull", "Your account is successfully added."));
                        //}

                        //insert Acnt and Meter No. in customDb.
                        bool isAccountInList = _dbAccountService.isRegisteredAccountPresent(nonregisteredAccount.Accountnumber, nonregisteredAccount.MeterNumber);
                        if (isAccountInList)
                        {
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Already Added", "This account is already added in your list."));
                        }
                        else
                        {
                            _dbAccountService.NonRegisteredAccountInDb(nonregisteredAccount.Accountnumber, nonregisteredAccount.MeterNumber);
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Successfull", "Your account is successfully added."));
                        }

                    }
                    return this.Redirect(this.Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Method NonRegisteredAccountBody -", ex.Message);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Failure", "There is some technical issue. Please try again."));
                return this.Redirect(this.Request.RawUrl);
            }

        }
        #endregion

        #region ||** De Registered Account **||
        [RedirectUnauthenticated]
        public ActionResult DeRegisterAccountBody()
        {

            #region New Code

            #region variable declaration
            var model = new DeRegisterAccount();
            var availableitemsinDB = _dbAccountService.GetAccountListbyusername(Context.User.Profile.UserName);
            List<KeyValuePair<string, string>> AccountNumberList = new List<KeyValuePair<string, string>>();
            string primaryAccountNumber = string.Empty;

            #endregion

            //// NOTE : check availability first in custom DB if data not available then check in to Sitecore
            if (availableitemsinDB != null && availableitemsinDB.Any())
            {
                #region Get Accounts List based on Logged in user
                var selecteditems = availableitemsinDB.Select(s => new { s.AccountNumber, s.Id }).ToList();
                #endregion

                #region Get primary Account number of Logged in user
                primaryAccountNumber = availableitemsinDB.Where(w => w.IsPrimary == true).Select(s => s.AccountNumber).FirstOrDefault();
                #endregion

                if (selecteditems.Any())
                {
                    foreach (var item in selecteditems)
                    {
                        if (!string.Equals(primaryAccountNumber, item.AccountNumber))
                        {
                            if (!string.IsNullOrEmpty(item.AccountNumber))
                            {
                                AccountNumberList.Add(new KeyValuePair<string, string>(Convert.ToString(item.Id), item.AccountNumber));
                            }

                        }
                    }
                }
                model.AccountList = AccountNumberList;
                model.MasterAccountNumber = GetPrimaryAccountNumber();//_dbAccountService.GetPrimaryAccountNumberbyUserName(Context.User.Profile.UserName);
            }
            else
            {
                var MultipleAccountItemId = Context.User.Profile.GetCustomProperty("Multiple Account");
                List<string> MultipleAccountItemIdList = new List<string>();

                // NOTE : Retrieving Primary Account Number of  User
                var primaryAccountItemId = Context.User.Profile.GetCustomProperty("Primary Account No");
                primaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(primaryAccountItemId);

                if (MultipleAccountItemId.Contains('|'))
                {
                    MultipleAccountItemIdList = MultipleAccountItemId.Split('|').ToList();
                }
                if (MultipleAccountItemIdList.Any())
                {
                    string secondaryAccountNumber = string.Empty;
                    foreach (var secondaryAccountItemId in MultipleAccountItemIdList)
                    {
                        if (!string.Equals(primaryAccountItemId, secondaryAccountItemId))
                        {
                            secondaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(secondaryAccountItemId);
                            if (!string.IsNullOrEmpty(secondaryAccountNumber))
                            {
                                AccountNumberList.Add(new KeyValuePair<string, string>(secondaryAccountItemId, secondaryAccountNumber));
                            }

                        }

                    }
                }
                model.AccountList = AccountNumberList;
                model.MasterAccountNumber = GetPrimaryAccountNumber();
            }
            #endregion

            return this.View("DeRegisterAccountBody", model);
        }

        [RedirectUnauthenticated]
        public ActionResult DeRegisterAccountInternally(string ItemId)
        {
            var item = Context.Database.GetItem(Templates.Pages.DeRegisterAccount);
            var url = item.Url();

            try
            {
                //// NOTE : check availability first in custom DB if data not available then check in to Sitecore
                var IsRecordAvailableInDB = _dbAccountService.DeregisterAccount(ItemId);

                var MultipleAccountItemId = Context.User.Profile.GetCustomProperty("Multiple Account");
                List<string> MultipleAccountList = new List<string>();
                if (MultipleAccountItemId.Contains('|'))
                {
                    MultipleAccountList = MultipleAccountItemId.Split('|').ToList();
                }
                if (MultipleAccountList.Any())
                {
                    MultipleAccountList.Remove(ItemId);
                    //NOTE : Set "Multliple Account" Property After Removal of one Property Item.
                    Context.User.Profile.SetCustomProperty("Multiple Account", string.Join("|", MultipleAccountList.ToArray()));
                    Context.User.Profile.Save();

                    this.AccountRepository.RemoveAccountNumberPermenantly(ItemId);
                }

                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/DeRegistred Success", "Account De-Registered Successfully"));

            }
            catch (Exception ex)
            {
                Log.Error("Method DeRegisterAccountInternally -", ex.Message);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/DeRegistred Failure", "Something Went Wrong."));
            }
            return RedirectPermanent(url);
        }
        #endregion


        #region ||** Meter Reading Date **||
        [RedirectUnauthenticated]
        public ActionResult MeterReadingDateBody()
        {
            return this.View("MeterReadingDateBody", new MeterReadingDateinfo() { MonthList = getMonthsWithCurrentYear() });
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [RedirectUnauthenticated]
        [ValidateAntiForgeryToken]
        public ActionResult MeterReadingDateBody(MeterReadingDateinfo model)
        {
            //Note : Get Available List of Items from 	EBILL MRDATE
            Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = master.GetItem(new Data.ID(Templates.ItemList.MeterReadingItemList.ToString()));
            string cycleValue = SessionHelper.UserSession.UserSessionContext.cycleNumber;
            string billMonth = model.Monthval;
            var AvailableItem = itemInfo.GetChildren().ToList().FirstOrDefault(w => w.Fields[Templates.MeterReadingProperties.CYCLE.ToString()].Value == cycleValue && w.Fields[Templates.MeterReadingProperties.BILLMONTH.ToString()].Value == billMonth);
            if (AvailableItem != null)
                model.ScheduleMeterReadingdate = AvailableItem.Fields[Templates.MeterReadingProperties.FULLMETERREADINGDATE.ToString()].Value;
            else
                model.ScheduleMeterReadingdate = string.Empty;
            model.MonthList = getMonthsWithCurrentYear();
            return this.View("MeterReadingDateBody", model);
        }
        #endregion

        #region ||** MailBox **||
        [RedirectUnauthenticated]
        public ActionResult Inbox()
        {
            return this.View();
        }

        public ActionResult LoadData_Inbox()
        {
            //Note : Get Available List of Items from Inbox
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = dbWeb.GetItem(new Data.ID(Templates.MailBox.InboxItemID.ToString()));
            var InboxItem = itemInfo.GetChildren().ToList();
            return Json(new { data = MailBoxItemList(InboxItem) }, JsonRequestBehavior.AllowGet);
        }


        [RedirectUnauthenticated]
        public ActionResult Outbox()
        {
            return this.View();
        }

        public ActionResult LoadData_Outbox()
        {
            //Note : Get Available List of Items from Outbox
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = dbWeb.GetItem(new Data.ID(Templates.MailBox.OutBoxItemID.ToString()));
            string userId = this.UserProfileService.GetLoginName();
            var OutBoxItem = itemInfo.GetChildren().Where
                (W => !string.IsNullOrEmpty(W.Fields[Templates.MailBox.ComposeItemFields.UserId].Value)
                        && W.Fields[Templates.MailBox.ComposeItemFields.UserId].Value == userId
                        && (string.IsNullOrEmpty(W.Fields[Templates.MailBox.ComposeItemFields.IsTrash].Value) || W.Fields[Templates.MailBox.ComposeItemFields.IsTrash].Value == "0")
                    ).ToList();
            return Json(new { data = MailBoxItemList(OutBoxItem) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteOutbox(string[] values)
        {
            //Note : Delete Items from Outbox 
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                if (values != null && values.Any())
                {
                    foreach (var item in values)
                    {
                        var itemInfo = dbWeb.GetItem(item);
                        itemInfo.Editing.BeginEdit();

                        itemInfo.Fields[Templates.MailBox.ComposeItemFields.IsTrash].Value = "1";
                        itemInfo.Editing.EndEdit();

                    }
                }
            }

            return Json(new { Result = String.Format(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Delete Outbox Message", "{0} Items deleted from outbox"), values != null && values.Any() ? values.Count() : 0) }); // Set Message from Dictionary.
        }

        [RedirectUnauthenticated]
        [HttpGet]
        public ActionResult Compose()
        {
            return this.View();
        }
        [HttpPost]
        [ValidateRenderingId]
        [RedirectUnauthenticated]
        [ValidateAntiForgeryToken]
        public ActionResult ComposeMail()
        {
            try
            {
                ComposeMail model = new ComposeMail();
                model.ToEmail = Request.Form["ToEmail"];
                model.Subject = Request.Form["Subject"];
                model.BodyMessage = Request.Form["BodyMessage"];
                model.Subject = Request.Form["ToEmail"];
                //if (!this.IsCaptchaValid("Captcha Validation Required."))
                //{
                //    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                //    return new JsonResult() { Data = new { status = false, message = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required.") }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //}

                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    return new JsonResult() { Data = new { status = false, message = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required.") }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    //return this.View(model);
                }

                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return new JsonResult() { Data = new { status = false, message = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed.") }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    //  return this.View(model);
                }

                HttpFileCollectionBase files = Request.Files;
                byte[] data = null;
                string fileName = string.Empty;
                if (files.Count > 0 && files[0] != null && files[0].ContentLength > 0)
                {
                    fileName = files[0].FileName;
                    using (Stream inputStream = files[0].InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }
                }

                model.ToEmail = "helpdesk.mumbaielectricity@adani.com";
                if (this.NotificationService.SendComposeMail(model.ToEmail, model.Subject, model.BodyMessage, data, fileName))
                {
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        Sitecore.Data.Database webdb = Sitecore.Configuration.Factory.GetDatabase("web");
                        // Get the template for which you need to create item
                        TemplateItem template = webdb.GetItem("/sitecore/templates/Project/Electricity/Content Types/Email Template");

                        // Get the place in the site tree where the new item must be inserted
                        Sitecore.Data.Items.Item parentItem = webdb.GetItem("/sitecore/content/Electricity/Global/Mailbox/Outbox");
                        string ItemVal = this.UserProfileService.GetLoginName() + "_" + DateTime.UtcNow.ToString("yyyyMMddHHmm");
                        Sitecore.Data.Items.Item newItem = parentItem.Add(ItemVal, template);

                        // Set the new item in editing mode
                        newItem.Editing.BeginEdit();

                        newItem.Fields[Templates.MailBox.ComposeItemFields.FromEmail.ToString()].Value = Context.User.Profile.Email;
                        newItem.Fields[Templates.MailBox.ComposeItemFields.Subject.ToString()].Value = model.Subject;
                        newItem.Fields[Templates.MailBox.ComposeItemFields.Body.ToString()].Value = model.BodyMessage;

                        //Note : Add logic To Insert Attachment into Media Library.

                        try
                        {
                            if (files.Count > 0)
                            {
                                var mediaItem = this.UserProfileService.InsertAttachmentURL(data, fileName);
                                Sitecore.Data.Fields.FileField link = newItem.Fields[Templates.MailBox.ComposeItemFields.Attachment.ToString()];
                                link.MediaID = mediaItem.ID;
                                link.Src = Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"{0}", ex, this);
                        }

                        newItem.Fields[Templates.MailBox.ComposeItemFields.Ondate.ToString()].Value = Sitecore.DateUtil.ToIsoDate(Convert.ToDateTime(DateTime.UtcNow));
                        newItem.Fields[Templates.MailBox.ComposeItemFields.UserId.ToString()].Value = this.UserProfileService.GetLoginName();
                        newItem.Editing.EndEdit();

                    }
                    return new JsonResult() { Data = new { status = true, message = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Email Sent Success", "Email Sent Successfully.") }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult() { Data = new { status = false, message = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Email Sent Failure", "Unable to send email please try again.") }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                this.Session["UpdateMessage"] = new InfoMessage(ex.Message);
                return new JsonResult() { Data = new { status = false, message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        [RedirectUnauthenticated]
        public ActionResult Trash()
        {
            return this.View();
        }

        [RedirectUnauthenticated]
        public ActionResult DisplayEmail()
        {
            DisplayEmail objModel = new DisplayEmail();
            try
            {
                string itemEmailId = Request.QueryString["emailitemid"].ToString();

                Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = master.GetItem(new Data.ID(itemEmailId));
                objModel.Subject = itemInfo.Fields[Templates.MailBox.ComposeItemFields.Subject].Value;
                objModel.BodyMessage = itemInfo.Fields[Templates.MailBox.ComposeItemFields.Body].Value;
                objModel.EmailDate = Sitecore.DateUtil.IsoDateToDateTime(itemInfo.Fields[Templates.MailBox.ComposeItemFields.Ondate].Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty;
                objModel.FromEmail = itemInfo.Fields[Templates.MailBox.ComposeItemFields.FromEmail].Value;
                Sitecore.Data.Fields.FileField link = itemInfo.Fields[Templates.MailBox.ComposeItemFields.Attachment.ToString()];
                objModel.AttachmentLink = link.Src;
                return this.View(objModel);
            }
            catch (Exception ex)
            {
                Log.Error("DisplayError", ex.Message);
                return this.View(objModel);
            }
        }


        public ActionResult LoadData_Trash()
        {
            //Note : Get Available List of Items from Trash
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = dbWeb.GetItem(new Data.ID(Templates.MailBox.OutBoxItemID.ToString()));
            string userId = this.UserProfileService.GetLoginName();
            var TrashItem = itemInfo.GetChildren().Where
                (W => !string.IsNullOrEmpty(W.Fields[Templates.MailBox.ComposeItemFields.UserId].Value)
                        && W.Fields[Templates.MailBox.ComposeItemFields.UserId].Value == userId
                        && (!string.IsNullOrEmpty(W.Fields[Templates.MailBox.ComposeItemFields.IsTrash].Value) && W.Fields[Templates.MailBox.ComposeItemFields.IsTrash].Value == "1")
                    ).ToList();
            return Json(new { data = MailBoxItemList(TrashItem) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTrash(string[] values)
        {
            //Note : Delete Items from Outbox 
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                if (values != null && values.Any())
                {
                    foreach (var item in values)
                    {
                        var itemInfo = dbWeb.GetItem(item);
                        itemInfo.Editing.BeginEdit();
                        itemInfo.Delete();
                        itemInfo.Editing.EndEdit();

                    }
                }
            }

            return Json(new { Result = String.Format(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Delete Trash Message", "{0} Items deleted from trash"), values != null && values.Any() ? values.Count() : 0) }); // Set Message from Dictionary.
        }
        #endregion

        [ChildActionOnly]
        public static ReCaptchaResponse VerifyCaptcha(string secret, string request)
        {
            if (request != null)
            {
                using (System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient())
                {
                    var values = new Dictionary<string, string> {
                        {
                            "secret",
                            secret
                        },
                        {
                            "response",
                            request
                        }
                    };
                    var content = new System.Net.Http.FormUrlEncodedContent(values);
                    var Response = hc.PostAsync("https://www.google.com/recaptcha/api/siteverify", content).Result;
                    var responseString = Response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrWhiteSpace(responseString))
                    {
                        ReCaptchaResponse response = JsonConvert.DeserializeObject<ReCaptchaResponse>(responseString);
                        return response;
                    }
                    else
                    {
                        throw new Exception();
                        //Throw error as required  
                    }
                }
            }
            else
            {
                throw new Exception();
                //Throw error as required  
            }
        }
        public class ReCaptchaResponse
        {
            public bool success
            {
                get;
                set;
            }
            public string challenge_ts
            {
                get;
                set;
            }
            public string hostname
            {
                get;
                set;
            }
            [JsonProperty(PropertyName = "error-codes")]
            public List<string> error_codes
            {
                get;
                set;
            }
        }


        #region ||** Helper Function **||

        private string GetPrimaryAccountNumber()
        {
            string primaryAccountNumber = string.Empty;
            primaryAccountNumber = _dbAccountService.GetAccountNumberbyUserName(Context.User.Profile.UserName);

            if (string.IsNullOrEmpty(primaryAccountNumber))
            {
                var primaryAccountItemId = Context.User.Profile.GetCustomProperty("Primary Account No");
                primaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(primaryAccountItemId);
            }
            return primaryAccountNumber;
        }

        private string Token()
        {
            Guid id = Guid.NewGuid();
            return id.ToString();
        }

        private bool IsResetLinkExpired(string TokenGeneratedDateTime = null)
        {
            bool status = true;
            if (!string.IsNullOrEmpty(TokenGeneratedDateTime))
            {
                var tokendatetime = Sitecore.DateUtil.IsoDateToDateTime(TokenGeneratedDateTime);
                TimeSpan diff = DateTime.UtcNow - tokendatetime;
                double DiffInMinutes = diff.TotalHours;
                status = DiffInMinutes > 48 ? true : false;
            }
            return status;
        }

        private List<SelectListItem> getMonthsWithCurrentYear()
        {
            Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = master.GetItem(new Data.ID(Templates.ItemList.MonthList.ToString()));
            var childitems = itemInfo.GetChildren().ToList();
            string currentYear = DateTime.UtcNow.Year.ToString().Substring(2, 2);
            var monthlist = new List<SelectListItem>();
            monthlist.Add(new SelectListItem { Value = string.Empty, Text = "Select a month" });
            foreach (var item in childitems)
            {
                monthlist.Add(new SelectListItem { Value = string.Concat((item.Fields["Value"].Value), "-", currentYear), Text = string.Concat((item.Fields["Text"].Value), "-", currentYear) });
            }
            return monthlist;
        }


        private List<MailBoxInfo> MailBoxItemList(List<Item> AvailableItem)
        {
            var pageinfo = Context.Database.GetItem(Templates.Pages.MailDetail);
            string pageurl = string.Empty;
            if (pageinfo != null)
            {
                pageurl = pageinfo.Url();
            }
            var mailboxinfo = new List<MailBoxInfo>();
            foreach (var item in AvailableItem)
            {
                mailboxinfo.Add(new MailBoxInfo
                {
                    ToEmail = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Reciepent Email Address", "helpdesk.mumbaielectricity@adani.com"),//item.Fields[Templates.MailBox.ComposeItemFields.FromEmail].Value,
                    FromEmail = item.Fields[Templates.MailBox.ComposeItemFields.FromEmail].Value,
                    Subject = item.Fields[Templates.MailBox.ComposeItemFields.Subject].Value,
                    OnDate = Sitecore.DateUtil.IsoDateToDateTime(item.Fields[Templates.MailBox.ComposeItemFields.Ondate].Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty,
                    MailId = string.Empty,
                    ItemId = item.ID.ToString(),
                    PageURL = pageurl
                });
            }
            return mailboxinfo;
        }
        #endregion

        #region ||** Email Alerts **||

        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult EmailAlertsBody()
        {
            if (!Context.PageMode.IsNormal)
            {
                return this.View(this.UserProfileService.GetEmptyProfile());
            }

            var profile = this.UserProfileService.GetProfile(Context.User);

            return this.View(profile);
        }

        [HttpPost]
        [RedirectUnauthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult EmailAlertsBody(EditProfile profile)
        {
            try
            {
                var loggedinuserprofile = this.UserProfileService.GetProfile(Context.User);

                if (loggedinuserprofile.AccountNumber != profile.AccountNumber)
                {
                    Sitecore.Diagnostics.Log.Info("Error at EmailAlertsBody loggedinuserprofile.AccountNumber != profile.AccountNumber:" + loggedinuserprofile.AccountNumber + ":" + profile.AccountNumber, this);
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile update error", "Profile was not successfully updated, please try again!"));

                    return this.Redirect(this.Request.RawUrl);
                }

                this.UserProfileService.SaveEmailAlerts(Context.User.Profile, profile);
                string emailId = profile.EBill ? profile.Email : "";
                string result = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(profile.AccountNumber, profile.MobileNumber, profile.PhoneNumber, emailId, null);
                if (result == "success")
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success Email alerts", "Email Alerts flag updated successfully!"));
                else
                {
                    Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + result, this);
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Email alerts update error", "Email Alerts flag was not successfully updated, please try again!"));
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + e.Message, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Error Email alerts", "Email Alerts flag not updated, please try again!"));
            }
            return this.Redirect(this.Request.RawUrl);
        }

        #endregion

        #region ||** Change Bill Language **||

        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult ChangeBillLanguage()
        {
            ChangeBillLanguage model = new ChangeBillLanguage();
            model.BillLanguageList = new List<string>
            {
                SapPiService.Domain.BillingLanguage.English.ToString(),
                SapPiService.Domain.BillingLanguage.Hindi.ToString(),
                //SapPiService.Domain.BillingLanguage.Gujarati.ToString(),
                SapPiService.Domain.BillingLanguage.Marathi.ToString()
            };

            if (!Context.PageMode.IsNormal)
            {
                return this.View(this.UserProfileService.GetEmptyProfile());
            }
            try
            {
                var accountNumber = this.UserProfileService.GetAccountNumber(Context.User);
                var existingBillLanguage = SapPiService.Services.RequestHandler.FetchBillingLanguage(accountNumber);
                model.BillLanguageSelected = existingBillLanguage.ToString();
            }
            catch
            { }
            return this.View(model);
        }

        [HttpPost]
        [RedirectUnauthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBillLanguage(ChangeBillLanguage model)
        {
            try
            {
                var accountNumber = this.UserProfileService.GetAccountNumber(Context.User);
                SapPiService.Domain.BillingLanguage selectedLanguage = (SapPiService.Domain.BillingLanguage)Enum.Parse(typeof(SapPiService.Domain.BillingLanguage), model.BillLanguageSelected);
                SapPiService.Services.RequestHandler.UpdateBillingLanguage(accountNumber, selectedLanguage);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Change Bill Language", "Bill Language updated successfully!"));
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Change Bill Language fail", "Bill Language update Failed, please try again!" + ex.Message.ToString()));
            }
            return this.Redirect(this.Request.RawUrl);
        }

        #endregion

        #region ||** Change Bill Language **||

        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult GetOutageInformation()
        {
            SapPiService.Domain.OutageDetail model = new SapPiService.Domain.OutageDetail();
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);

                var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(profile.AccountNumber);

                model = SapPiService.Services.RequestHandler.GetOutageInformation(profile.AccountNumber);

                //OutageDetail result = new OutageDetail();
                //result = model;
                if (model.CurrentOutageDetails != null && model.CurrentOutageDetails.Count > 1)
                {
                    List<OutageRecord> currList = new List<OutageRecord>();
                    var maxEndTime = model.CurrentOutageDetails.Max(c => c.EndDatetime);
                    foreach (var r in model.CurrentOutageDetails)
                    {
                        if (r.EndDatetime == maxEndTime)
                            currList.Add(r);
                    }

                    if (currList.Count > 1)
                    {
                        var minStartTime = currList.Min(c => c.StartDateTime);
                        List<OutageRecord> currList1 = new List<OutageRecord>();
                        foreach (var r in currList)
                        {
                            if (r.StartDateTime == minStartTime)
                                currList1.Add(r);
                        }

                        if (currList1.Count > 1)
                        {
                            model.CurrentOutageDetails = new List<OutageRecord>();
                            model.CurrentOutageDetails.Add(currList1.Where(c => c.OutageType == "HT").FirstOrDefault());
                        }
                        else
                            model.CurrentOutageDetails = currList1;
                    }
                    else
                        model.CurrentOutageDetails = currList;
                }

                foreach (var r in model.CurrentOutageDetails)
                {
                    switch (r.ActivityType)
                    {
                        case "1": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                        case "2": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Disconnected for Safety", "Disconnected for Safety"); break;
                        case "3": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Breakdown", "Breakdown"); break;
                        case "4": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Transmission outage", "Transmission outage"); break;
                        default: r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                    }
                }
                foreach (var r in model.FutureOutageDetails)
                {
                    switch (r.ActivityType)
                    {
                        case "1": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                        case "2": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Disconnected for Safety", "Disconnected for Safety"); break;
                        case "3": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Breakdown", "Breakdown"); break;
                        case "4": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Transmission outage", "Transmission outage"); break;
                        default: r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                    }
                }

                model.Amount = billinghDetails.CurrentMonthCharge;
            }
            catch { }
            return this.View(model);
        }

        [HttpPost]
        [RedirectUnauthenticated]
        public ActionResult GetOutageInformation(EditProfile profiles)
        {
            SapPiService.Domain.OutageDetail model = new SapPiService.Domain.OutageDetail();
            try
            {
                model = SapPiService.Services.RequestHandler.GetOutageInformation(profiles.AccountNumber);
                
                //OutageDetail result = new OutageDetail();
                //result = model;
                if (model.CurrentOutageDetails != null && model.CurrentOutageDetails.Count > 1)
                {
                    List<OutageRecord> currList = new List<OutageRecord>();
                    var maxEndTime = model.CurrentOutageDetails.Max(c => c.EndDatetime);
                    foreach (var r in model.CurrentOutageDetails)
                    {
                        if (r.EndDatetime == maxEndTime)
                            currList.Add(r);
                    }

                    if (currList.Count > 1)
                    {
                        var minStartTime = currList.Min(c => c.StartDateTime);
                        List<OutageRecord> currList1 = new List<OutageRecord>();
                        foreach (var r in currList)
                        {
                            if (r.StartDateTime == minStartTime)
                                currList1.Add(r);
                        }

                        if (currList1.Count > 1)
                        {
                            model.CurrentOutageDetails = new List<OutageRecord>();
                            model.CurrentOutageDetails.Add(currList1.Where(c => c.OutageType == "HT").FirstOrDefault());
                        }
                        else
                            model.CurrentOutageDetails = currList1;
                    }
                    else
                        model.CurrentOutageDetails = currList;
                }

                foreach (var r in model.CurrentOutageDetails)
                {
                    switch (r.ActivityType)
                    {
                        case "1": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                        case "2": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Disconnected for Safety", "Disconnected for Safety"); break;
                        case "3": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Breakdown", "Breakdown"); break;
                        case "4": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Transmission outage", "Transmission outage"); break;
                        default: r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                    }
                }
                foreach (var r in model.FutureOutageDetails)
                {
                    switch (r.ActivityType)
                    {
                        case "1": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                        case "2": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Disconnected for Safety", "Disconnected for Safety"); break;
                        case "3": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Breakdown", "Breakdown"); break;
                        case "4": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Transmission outage", "Transmission outage"); break;
                        default: r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return this.Redirect(this.Request.RawUrl);
        }

        #endregion


        #region ||** Check Complaint Status **||

        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult CheckComplaintStatus()
        {
            if (!Context.PageMode.IsNormal)
            {
                return this.View(this.UserProfileService.GetEmptyProfile());
            }

            var profile = this.UserProfileService.GetProfile(Context.User);

            var complainStatus = new ComplaintStatus
            {
                AccountNumber = profile.AccountNumber
            };

            return this.View(complainStatus);
        }

        [HttpPost]
        [RedirectUnauthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult CheckComplaintStatus(EditProfile profile)
        {
            //var result = SapPiService.Services.RequestHandler.CheckComplaintStatus(profile.AccountNumber, profile.ComplaintNumber);
            //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Check Complaint Status", "Bill Language updated successfully!"));
            // return this.Redirect(this.Request.RawUrl);
            var complainStatus = new ComplaintStatus();
            try
            {
                SapPiService.Domain.ComplainStatus model = SapPiService.Services.RequestHandler.CheckComplaintStatus(profile.AccountNumber, profile.ComplaintNumber);

                complainStatus.AccountNumber = model.AccountNumber;
                complainStatus.ComplainCode = model.ComplainCode;

                //var complainStatus = new ComplaintStatus
                //{
                //    AccountNumber = model.AccountNumber,
                //    ComplainCode = model.ComplainCode
                //};
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return this.View(complainStatus);

        }

        #endregion



        #region ||** Quick Access Check Complaint Status **||

        [HttpGet]
        [RedirectAuthenticated]
        public ActionResult QACheckComplaintStatus()
        {
            QuickAccessServices service = new QuickAccessServices();

            return this.View(service);


        }

        [HttpPost]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult QACheckComplaintStatus(QuickAccessServices profile)
        {
            //var result = SapPiService.Services.RequestHandler.CheckComplaintStatus(profile.AccountNumber, profile.ComplaintNumber);
            //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Check Complaint Status", "Bill Language updated successfully!"));
            //return this.Redirect(this.Request.RawUrl);
            var quickAccessServices = new QuickAccessServices();
            try
            {
                //if (!this.IsCaptchaValid("Captcha Validation Required."))
                //{
                //    ModelState.AddModelError(nameof(profile.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                //    return this.View(profile);
                //}

                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(profile.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(profile);
                }

                SapPiService.Domain.ComplainStatus model = SapPiService.Services.RequestHandler.CheckComplaintStatus(profile.AccountNumber, profile.ComplaintNumber);

                //var quickAccessServices = new QuickAccessServices
                //{
                quickAccessServices.AccountNumber = model.AccountNumber;
                quickAccessServices.ComplainCode = model.ComplainCode;
                //};
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return this.View(quickAccessServices);
        }

        #endregion


        #region ||** Quick Access OutageInformation **||

        [HttpGet]
        public ActionResult QAOutageInformation()
        {
            SapPiService.Domain.OutageDetail model = new SapPiService.Domain.OutageDetail();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateRenderingId]
        public ActionResult QAOutageInformation(SapPiService.Domain.OutageDetail profile)
        {
            SapPiService.Domain.OutageDetail model = new SapPiService.Domain.OutageDetail();
            try
            {
                //if (!this.IsCaptchaValid("Captcha Validation Required."))
                //{
                //    ModelState.AddModelError(nameof(profile.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                //    return this.View(profile);
                //}
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(profile.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(profile);
                }
                model = SapPiService.Services.RequestHandler.GetOutageInformation(profile.AccountNumber);

                //OutageDetail result = new OutageDetail();
                //result = model;
                if (model.CurrentOutageDetails != null && model.CurrentOutageDetails.Count > 1)
                {
                    List<OutageRecord> currList = new List<OutageRecord>();
                    var maxEndTime = model.CurrentOutageDetails.Max(c => c.EndDatetime);
                    foreach (var r in model.CurrentOutageDetails)
                    {
                        if (r.EndDatetime == maxEndTime)
                            currList.Add(r);
                    }

                    if (currList.Count > 1)
                    {
                        var minStartTime = currList.Min(c => c.StartDateTime);
                        List<OutageRecord> currList1 = new List<OutageRecord>();
                        foreach (var r in currList)
                        {
                            if (r.StartDateTime == minStartTime)
                                currList1.Add(r);
                        }

                        if (currList1.Count > 1)
                        {
                            model.CurrentOutageDetails = new List<OutageRecord>();
                            model.CurrentOutageDetails.Add(currList1.Where(c => c.OutageType == "HT").FirstOrDefault());
                        }
                        else
                            model.CurrentOutageDetails = currList1;
                    }
                    else
                        model.CurrentOutageDetails = currList;
                }


                foreach (var r in model.CurrentOutageDetails)
                {
                    switch (r.ActivityType)
                    {
                        case "1": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                        case "2": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Disconnected for Safety", "Disconnected for Safety"); break;
                        case "3": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Breakdown", "Breakdown"); break;
                        case "4": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Transmission outage", "Transmission outage"); break;
                        default: r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                    }
                }
                foreach (var r in model.FutureOutageDetails)
                {
                    switch (r.ActivityType)
                    {
                        case "1": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                        case "2": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Disconnected for Safety", "Disconnected for Safety"); break;
                        case "3": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Breakdown", "Breakdown"); break;
                        case "4": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Transmission outage", "Transmission outage"); break;
                        default: r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/Accounts/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return this.View(model);
        }

        #endregion

        #region ||** Quick Access Check "No-Supply" Complaint Status **||

        [HttpGet]
        [RedirectAuthenticated]
        public ActionResult QACheckNoSupplyComplaintStatus()
        {
            NoSupplyComplaintStatus service = new NoSupplyComplaintStatus();

            return this.View(service);
        }

        [HttpPost]
        [RedirectAuthenticated]
        [ValidateAntiForgeryToken]
        [ValidateRenderingId]
        public ActionResult QACheckNoSupplyComplaintStatus(QuickAccessServices profile)
        {
            //var result = SapPiService.Services.RequestHandler.CheckNoSupplyComplaintStatus(profile.AccountNumber, profile.ComplaintNumber);
            //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Check Complaint Status", "Bill Language updated successfully!"));
            //return this.Redirect(this.Request.RawUrl);
            var noSupplyComplaintStatus = new NoSupplyComplaintStatus();
            try
            {
                //if (!this.IsCaptchaValid("Captcha Validation Required."))
                //{
                //    ModelState.AddModelError(nameof(noSupplyComplaintStatus.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                //    return this.View(noSupplyComplaintStatus);
                //}
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(noSupplyComplaintStatus.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(noSupplyComplaintStatus);
                }

                SapPiService.Domain.NoSupplyComplaintStatus model = SapPiService.Services.RequestHandler.CheckNoSupplyComplaintStatus(profile.AccountNumber, profile.ComplaintNumber);

                if (model.Status != null)
                {
                    noSupplyComplaintStatus.ComplaintNumber = model.ComplaintNumber;
                    noSupplyComplaintStatus.AccountNumber = model.AccountNumber;
                    noSupplyComplaintStatus.ServiceCode = model.ServiceCode;
                    noSupplyComplaintStatus.Status = model.Status;

                    Sitecore.Data.Database context = Sitecore.Context.Database;
                    Sitecore.Data.Items.Item item = context.GetItem(new Data.ID("{A55C26F1-6DBD-4924-85BD-D70855C30F9B}"));
                    var AvailableItem = item.GetChildren().ToList();

                    foreach (var status in AvailableItem)
                    {
                        if (status.Fields["ServiceCode"].Value == noSupplyComplaintStatus.ServiceCode && status.Fields["ServiceStatus"].Value == noSupplyComplaintStatus.Status)
                        {
                            noSupplyComplaintStatus.Message = status.Fields["Message"].Value;
                        }
                    }
                }
                else
                {
                    noSupplyComplaintStatus.ComplaintNumber = model.ComplaintNumber;
                    noSupplyComplaintStatus.AccountNumber = model.AccountNumber;
                    noSupplyComplaintStatus.ServiceCode = model.ServiceCode;
                    noSupplyComplaintStatus.Status = model.Status;
                    noSupplyComplaintStatus.Message = model.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return this.View(noSupplyComplaintStatus);

        }

        #endregion

        #region ||** Check No Supply Complaint Status **||

        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult CheckNoSupplyComplaintStatus()
        {
            if (!Context.PageMode.IsNormal)
            {
                NoSupplyComplaintStatus model = new NoSupplyComplaintStatus();
                return this.View(model);
            }

            var profile = this.UserProfileService.GetProfile(Context.User);


            NoSupplyComplaintStatus noSupplyCompStatus = new NoSupplyComplaintStatus();
            noSupplyCompStatus.AccountNumber = profile.AccountNumber;

            return this.View(noSupplyCompStatus);
        }

        [HttpPost]
        [RedirectUnauthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult CheckNoSupplyComplaintStatus(EditProfile profile)
        {
            var noSupplyComplaintStatus = new NoSupplyComplaintStatus();
            try
            {
                if (profile.AccountNumber == null)
                    profile.AccountNumber = string.Empty;
                if (profile.ComplaintNumber == null)
                    profile.ComplaintNumber = string.Empty;
                SapPiService.Domain.NoSupplyComplaintStatus model = SapPiService.Services.RequestHandler.CheckNoSupplyComplaintStatus(profile.AccountNumber, profile.ComplaintNumber);

                if (model.Status != null)
                {
                    noSupplyComplaintStatus.ComplaintNumber = model.ComplaintNumber;
                    noSupplyComplaintStatus.AccountNumber = model.AccountNumber;
                    noSupplyComplaintStatus.ServiceCode = model.ServiceCode;
                    noSupplyComplaintStatus.Status = model.Status;

                    Sitecore.Data.Database context = Sitecore.Context.Database;
                    Sitecore.Data.Items.Item item = context.GetItem(new Data.ID("{A55C26F1-6DBD-4924-85BD-D70855C30F9B}"));
                    var AvailableItem = item.GetChildren().ToList();

                    foreach (var status in AvailableItem)
                    {
                        if (status.Fields["ServiceCode"].Value == noSupplyComplaintStatus.ServiceCode && status.Fields["ServiceStatus"].Value == noSupplyComplaintStatus.Status)
                        {
                            noSupplyComplaintStatus.Message = status.Fields["Message"].Value;
                        }
                    }
                    if (string.IsNullOrEmpty(noSupplyComplaintStatus.Message))
                        noSupplyComplaintStatus.Message = "No record found.";
                }
                else
                {
                    noSupplyComplaintStatus.ComplaintNumber = model.ComplaintNumber;
                    noSupplyComplaintStatus.AccountNumber = model.AccountNumber;
                    noSupplyComplaintStatus.ServiceCode = model.ServiceCode;
                    noSupplyComplaintStatus.Status = model.Status;
                    noSupplyComplaintStatus.Message = model.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return this.View(noSupplyComplaintStatus);
        }

        #endregion

        #region ||** SMS Alerts **||

        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult SMSAlertsBody()
        {
            if (!Context.PageMode.IsNormal)
            {
                return this.View(this.UserProfileService.GetEmptyProfile());
            }

            var profile = this.UserProfileService.GetProfile(Context.User);

            return this.View(profile);
        }

        [HttpPost]
        [RedirectUnauthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult SMSAlertsBody(EditProfile profile)
        {
            try
            {
                var loggedinuserprofile = this.UserProfileService.GetProfile(Context.User);

                if (loggedinuserprofile.AccountNumber != profile.AccountNumber)
                {
                    Sitecore.Diagnostics.Log.Info("Error at SMSAlertsBody loggedinuserprofile.AccountNumber != profile.AccountNumber:" + loggedinuserprofile.AccountNumber + ":" + profile.AccountNumber, this);
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile update error", "Profile was not successfully updated, please try again!"));

                    return this.Redirect(this.Request.RawUrl);
                }
                this.UserProfileService.SaveSMSAlerts(Context.User.Profile, profile);
                string mobile = profile.SMSUpdate ? profile.MobileNumber : "";
                string telephone = profile.SMSUpdate ? profile.PhoneNumber : "";
                string result = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(profile.AccountNumber, mobile, telephone, profile.Email, null);
                if (result == "success")
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success SMS alerts", "SMS Alerts flag updated successfully!"));
                else
                {
                    Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + result, this);
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/SMS alerts update error", "SMS Alerts flag was not successfully updated, please try again!"));
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + e.Message, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Error SMS alerts", "SMS Alerts flag not updated, please try again!"));
            }
            return this.Redirect(this.Request.RawUrl);
        }

        #endregion

        #region ||** Paperless Billing **||

        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult PaperlessBilling()
        {
            ViewBag.Status = null;
            if (!Context.PageMode.IsNormal)
            {
                return this.View(this.UserProfileService.GetEmptyProfile());
            }

            var profile = this.UserProfileService.GetProfile(Context.User);
            PaperlessBilling model = new PaperlessBilling
            {
                AccountNumber = profile.AccountNumber,
                Paperless_Billing = profile.PaperlessBilling,
                MobileNumber = profile.MobileNumber,
                Email = profile.Email
            };
            return this.View(model);
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [RedirectUnauthenticated]
        [ValidateAntiForgeryToken]
        public ActionResult PaperlessBilling(PaperlessBilling model)
        {
            ViewBag.Status = null;
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);

                if (model.AccountNumber != profile.AccountNumber)
                {
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Fail SMS alerts", "Failed to update!"));
                    return this.View(model);
                }

                model.Paperless_Billing = !model.Paperless_Billing;
                profile.PaperlessBilling = !profile.PaperlessBilling;
                this.UserProfileService.SavePaperlessBillingFlag(Context.User.Profile, profile);

                string result = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(model.AccountNumber, model.MobileNumber, null, model.Email, model.Paperless_Billing);
                if (result == "success")
                {
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success SMS alerts", "SMS Alerts flag updated successfully!"));
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + result, this);
                    //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/SMS alerts update error", "SMS Alerts flag was not successfully updated, please try again!"));
                }
                ViewBag.Status = model.Paperless_Billing ? "registered" : "deregistered";
                //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Success Paperless Alerts", "Paperless billing registration done successfully!"));
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
                //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Error Paperless alerts", "Paperless billing flag not updated, please try again!"));
            }
            //return this.Redirect(this.Request.RawUrl);
            return this.View(model);
        }

        #endregion



        //public ActionResult LoginInner(LoginInfo loginInfo)
        //{
        //    return this.Login(loginInfo, redirectUrl => new RedirectResult(redirectUrl));
        //}
        [RedirectAuthenticated]
        public ActionResult LoginInner(string returnUrl = null)
        {
            return this.View(this.CreateLoginInfo(returnUrl));
        }


        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        public ActionResult LoginInner(LoginInfo loginInfo)
        {
            return this.LoginInner(loginInfo, redirectUrl => new RedirectResult(redirectUrl));
        }

        protected virtual ActionResult LoginInner(LoginInfo loginInfo, Func<string, ActionResult> redirectAction)
        {
            var user = this.AccountRepository.Login(loginInfo.LoginName, loginInfo.Password);
            if (user == null)
            {
                this.ModelState.AddModelError("invalidCredentials", DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/User Not Valid", "Username or password is not valid."));
                return this.View(loginInfo);
            }

            var redirectUrl = loginInfo.ReturnUrl;
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = this.GetRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Authenticated);
            }

            return redirectAction(redirectUrl);
        }


        #region Billing and Payments/ Pay Security Deposit / Payment History / Quick Pay

        #region View & Pay Bill
        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult ViewPayBill()
        {
            var model = new ViewPayBill();
            try
            {


                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                //var properties = this.UserProfileProvider.GetCustomProperties(Context.User.Profile);
                //var masterAccount = properties.ContainsKey(Constants.UserProfile.Fields.PrimaryAccountNo) ? properties[Constants.UserProfile.Fields.PrimaryAccountNo] : "";

                // Note : retrieve Information from service to bind model Values.
                //SecurityPayment
                var sdpaymentlist = db.GetItem(Templates.PaymentConfiguration.Datasource.SecurityDepositPaymentMode);
                var securitypaymentlist = sdpaymentlist.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();

                //VDS Payment
                var paymentlist = db.GetItem(Templates.PaymentConfiguration.Datasource.VDSPaymentMode);
                var VDSpaymentlist = paymentlist.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();


                var profile = this.UserProfileService.GetProfile(Context.User);

                Log.Info("Paybill fetching details for CA:" + model.AccountNumber, this);
                var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(profile.AccountNumber);
                Log.Info("Paybill details received for CA:" + accountDetails.AccountNumber, this);

                var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(profile.AccountNumber);
                Log.Info("Paybill billing amount for CA:" + billinghDetails.AmountPayable, this);

                var securityAmount = SapPiService.Services.RequestHandler.FetchSecurityDepositAmount(profile.AccountNumber);
                var emiDetails = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(profile.AccountNumber);
                Log.Info("Paybill fetching EMI details for CA:" + model.AccountNumber + ", EMI: Status-" + emiDetails.Status + ", EMI-" + emiDetails.EMIInstallmentAmount + ", Total Outstannding" + emiDetails.TotalOutstanding, this);

                //var vdsAmount = SapPiService.Services.RequestHandler.FetchVdsAmount(profile.AccountNumber);
                decimal amountpayable = billinghDetails.AmountPayable;

                model = new ViewPayBill()
                {
                    AccountNumber = accountDetails.AccountNumber,// this.UserProfileService.GetAccountNumberfromItem(masterAccount.ToString()),
                    Name = accountDetails.Name,// this.UserProfileService.GetLoginName(),
                    BookNumber = accountDetails.BookNumber,// "505",
                    CycleNumber = accountDetails.CycleNumber,// "05",
                    Zone = accountDetails.ZoneNumber,// "South Central",
                    Address = accountDetails.Address,// "300 288 Shere Punjab SOC Mahakali Caves Rd Andheri E Near Tolani College Mumbai 400067",
                    BillMonth = billinghDetails.BillMonth,// "May-2018",
                    PaymentDueDate = billinghDetails.DateDue,// "2018-06-16",
                    TariffSlab = billinghDetails.TariffSlab,// "LT-1b",
                    MeterNumber = string.Join(",", billinghDetails.MeterNumbers),// "7587321",
                    UnitsConsumed = billinghDetails.UnitsConsumed,//  "1",
                    TotalCharges = billinghDetails.TotalCharges,// "134.27",
                    CurrentMonthsBills = billinghDetails.CurrentMonthCharge,// "134.69",
                    BroughtForward = billinghDetails.BroughtForward,// "128.26",
                    TotalBillAmount = billinghDetails.TotalBillAmount,//  "246.19",
                    SecurityDeposit = securityAmount.ToString(),// "560.00",
                    SecurityDepositAmountType = "Actual",                                            //VDSAmount= vdsAmount.ToString(),
                    AmountPayable = amountpayable.ToString(),// billinghDetails.AmountPayable.ToString(),// "250"
                    LoginName = this.UserProfileService.GetLoginName(),
                    SecurityPaymentList = securitypaymentlist,
                    VDSPaymentList = VDSpaymentlist,
                    EMIEligible = emiDetails.Status == "S" ? true : false,
                    EMIInstallmentAmount = emiDetails.EMIInstallmentAmount,
                    EMIOutstandingAmount = emiDetails.TotalOutstanding,
                    ProceedWithEMI = false
                };

                Session["Amountpayable"] = amountpayable.ToString();//billinghDetails.AmountPayable.ToString();// "101";//billinghDetails.AmountPayable;
                Session["SecurityDeposit"] = securityAmount.ToString();
            }
            catch (Exception ex)
            {
                ViewBag.FetchBillPayment = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/API Issue", "There is some issue in fetching your data. Please try after some time.");
                Sitecore.Diagnostics.Log.Error("Error at ViewPayBill Get:" + ex.Message, this);
            }
            return this.View(model);
        }

        [HttpPost]
        [RedirectUnauthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ViewPayBill(ViewPayBill model, string Pay_PaymentGateway = null, string Pay_VDSpayment = null, string Pay_Other = null)
        {
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);
                if (model.AccountNumber != profile.AccountNumber)
                {
                    this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidRequest", "Invalid request."));
                    return this.View(model);
                }


                //Intial Model binding
                model.LoginName = this.UserProfileService.GetLoginName();
                model.Email = Context.User.Profile.Email;
                model.Mobile = Context.User.Profile.GetCustomProperty(Constants.UserProfile.Fields.MobileNo);
                model.UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/RegisteredUser", "Registered");

                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                var sdpaymentlist = db.GetItem(Templates.PaymentConfiguration.Datasource.SecurityDepositPaymentMode);
                var securitypaymentlist = sdpaymentlist.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();

                //VDS Payment Dropdown payment option
                var paymentlist = db.GetItem(Templates.PaymentConfiguration.Datasource.VDSPaymentMode);
                var VDSpaymentlist = paymentlist.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();

                model.SecurityPaymentList = securitypaymentlist;
                model.VDSPaymentList = VDSpaymentlist;

                if (!string.IsNullOrEmpty(Pay_PaymentGateway))
                {
                    if (model.ProceedWithEMI)
                    {
                        Log.Info("Paybill for CA:" + model.AccountNumber + ", Proceed with EMI.", this);
                        var emiDetails = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(model.AccountNumber);
                        //put checks here
                        model.AmountPayable = emiDetails.EMIInstallmentAmount.ToString();
                        model.EMIOutstandingAmount = emiDetails.EMIInstallmentAmount;
                        model.EMIOutstandingAmount = emiDetails.TotalOutstanding;
                        Log.Info("Paybill for CA:" + model.AccountNumber + ", EMI:" + model.AmountPayable, this);
                        var emiFlagUpdateDetails = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(model.AccountNumber, "U");
                        Log.Info("Paybill EMI Update details for CA:" + model.AccountNumber + ", EMI: Status-" + emiDetails.Status + ", EMI-" + emiDetails.EMIInstallmentAmount + ", Total Outstannding" + emiDetails.TotalOutstanding, this);
                    }
                    else
                    {
                        if (model.AmountPayable != null && model.AmountPayable.Any(char.IsLetter))
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidAmount", "Invalid amount payable value."));
                            return this.View(model);
                        }
                        else if (Convert.ToDecimal(model.AmountPayable) < 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountNegativeValidation", "You have no amount payable value."));
                            return this.View(model);
                        }
                        else if (Convert.ToDecimal(Session["Amountpayable"]) >= 100 && Convert.ToDecimal(model.AmountPayable) < 100)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Amount not valid", "Minimum Amount payable Value is 100. Please enter valid amount."));
                            return this.View(model);
                        }
                        else if (Convert.ToDecimal(model.AmountPayable) == 0 && Convert.ToDecimal(model.AdvanceAmmount) <= 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AdvanceAmountValidation", "You have no amount payable value. Please enter proper advance amount."));
                            return this.View(model);
                        }
                        else
                        {
                            model.AmountPayable = System.Convert.ToDecimal(model.AmountPayable.ToString().Trim()) == 0 ? System.Convert.ToDecimal(model.AdvanceAmmount.ToString().Trim()).ToString("f2") : System.Convert.ToDecimal(model.AmountPayable.ToString().Trim()).ToString("f2");
                        }
                    }
                    var checkForPAN = SapPiService.Services.RequestHandler.FetchBilling(model.AccountNumber);
                    if (!string.IsNullOrEmpty(checkForPAN.Flag) && checkForPAN.Flag == "8")
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/PANNumberMandatory", "Dear Consumer, We cannot accept the said payment since as per Income Tax Rules, your PAN number is mandatory. Kindly contact our Customer Care Services Department to update your PAN number."));
                        return this.View(model);
                    }

                    model.PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid");

                    switch (model.PaymentGateway)
                    {
                        case (int)EnumPayment.GatewayType.HDFC:
                            this.PaymentService.StorePaymentRequest(model);
                            string hdfcPostData = this.PaymentService.HDFCTransactionRequestAPIRequestPost(model);
                            return Content(hdfcPostData);

                        case (int)EnumPayment.GatewayType.PayUMoney:
                            this.PaymentService.StorePaymentRequest(model);
                            string payUHTML = this.PaymentService.PayUMoneyTransactionRequestAPIRequestPost(model);
                            return Content(payUHTML);

                        case (int)EnumPayment.GatewayType.BillDesk:
                            this.PaymentService.StorePaymentRequest(model);
                            string RequestHTML = this.PaymentService.BillDeskTransactionRequestAPIRequestPost(model);
                            return Content(RequestHTML);

                        case (int)EnumPayment.GatewayType.Paytm:
                            this.PaymentService.StorePaymentRequest(model);
                            string outputHTML = this.PaymentService.PaytmTransactionRequestAPIRequestPost(model);
                            return Content(outputHTML);

                        case (int)EnumPayment.GatewayType.Ebixcash:
                            this.PaymentService.StorePaymentRequest(model);
                            string submittHTML = this.PaymentService.EbixcashTransactionRequestAPIRequestPost(model);
                            return Content(submittHTML);

                        case (int)EnumPayment.GatewayType.ICICIBank:
                            this.PaymentService.StorePaymentRequest(model);
                            string submitHTML = this.PaymentService.ICICITransactionRequestAPIRequestPost(model);
                            return Content(submitHTML);
                        case (int)EnumPayment.GatewayType.Benow:
                            this.PaymentService.StorePaymentRequest(model);
                            if (SessionHelper.UserSession.UserSessionContext == null)
                            {
                                SessionHelper.UserSession.UserSessionContext = new DashboardModel()
                                {
                                    OrderId = model.OrderId
                                };
                            }
                            else
                            {
                                SessionHelper.UserSession.UserSessionContext.OrderId = model.OrderId;
                            }
                            // SessionHelper.UserSession.UserSessionContext.OrderId = model.OrderId;
                            // TempData["orderid"] = model.OrderId;
                            //TempData.Keep();
                            string BenowsubmitHTML = this.PaymentService.BENOWTransactionRequestAPIRequestGET(model);
                            return Content(BenowsubmitHTML);
                        case (int)EnumPayment.GatewayType.DBSUPI:
                            this.PaymentService.StorePaymentRequest(model);
                            SessionHelper.UserSession.UserSessionContext.OrderId = model.OrderId;
                            Sitecore.Diagnostics.Log.Info("Call to DBSUPITransactionRequestAPIRequestGET", this);
                            string dbsUPIsubmitdata = this.PaymentService.DBSUPITransactionRequestAPIRequestGET(model);
                            return Content(dbsUPIsubmitdata);
                        case (int)EnumPayment.GatewayType.CITYUPI:
                            this.PaymentService.StorePaymentRequest(model);
                            if (SessionHelper.UserSession.UserSessionContext == null)
                            {
                                SessionHelper.UserSession.UserSessionContext = new DashboardModel()
                                {
                                    OrderId = model.OrderId
                                };
                            }
                            else
                            {
                                SessionHelper.UserSession.UserSessionContext.OrderId = model.OrderId;
                            }
                            Sitecore.Diagnostics.Log.Info("Call to CityUPITransactionRequestAPIRequestGET", this);
                            string cityUPIsubmitdata = this.PaymentService.CityUPITransactionRequestAPIRequestGET(model);
                            Sitecore.Diagnostics.Log.Info("Call to CityUPITransactionRequestAPIRequestGET dbsUPIsubmitdata:" + cityUPIsubmitdata, this);
                            return Content(cityUPIsubmitdata);
                        default:
                            break;
                    }
                }
                else if (!string.IsNullOrEmpty(Pay_VDSpayment)) // VDS Payment Selection
                {
                    if (string.IsNullOrEmpty(model.VDSPaymentSelection.ToString()))
                    {
                        this.ModelState.AddModelError(model.VDSPaymentSelection.ToString(), DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/VDS payment selection required", "Please select VDS payment Mode."));
                        return this.View(model);
                    }

                    TempData["VDSPaymentMode"] = model.VDSPaymentSelection;
                    var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentVDS);
                    return this.Redirect(SuccessUrl);

                }
                else if (!string.IsNullOrEmpty(Pay_Other)) // Security Payment Tab
                {
                    model.PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit");
                    if (model.SecurityDepositAmountType == "Actual")
                    {
                        if (Convert.ToDecimal(model.SecurityDeposit) > 0 && Convert.ToDecimal(Session["SecurityDeposit"]) != Convert.ToDecimal(model.SecurityDeposit))
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchSD", "You have modified Security Deposit amount Value. Please refresh the page and try again."));
                            return this.View(model);
                        }
                        if (Convert.ToDecimal(model.SecurityDeposit) <= 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/SecurityDepositeValidation", "You have no security deposit payable amount."));
                            return this.View(model);
                        }
                    }
                    else if (model.SecurityDepositAmountType == "Partial")
                    {
                        if (Convert.ToDecimal(model.SecurityDepositPartial) <= 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/SecurityDepositeValidation", "Please enter true value for security deposit payable amount."));
                            return this.View(model);
                        }
                    }
                    if (string.IsNullOrEmpty(model.SecurityPayment))
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Security payment selection required  ", "Please select security Payment option."));
                        return this.View(model);
                    }
                    if (model.SecurityPayment == DictionaryPhraseRepository.Current.Get("/Payment Settings/ICICI", "ICICI"))
                    {
                        model.PaymentGateway = 5;
                        this.PaymentService.StorePaymentRequest(model);
                        string submitHTML = this.PaymentService.ICICITransactionRequestAPIRequestPost(model);
                        return Content(submitHTML);
                    }
                    else
                    {
                        model.PaymentGateway = 2;
                        this.PaymentService.StorePaymentRequest(model);
                        string RequestHTML = this.PaymentService.BillDeskSDTransactionRequestAPIRequestPost(model);
                        return Content(RequestHTML);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ViewPayBill Post:" + ex.Message, this);
            }
            return this.Redirect(this.Request.RawUrl);
        }

        #region HDFC - OnHold
        public ActionResult HDFC_UPI_CallbackS2S()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            try
            {
                //HDFC Response
                string responsemsg = Request.Form["msg"];

                string ResErrorText = Request.Form["ErrorText"];    //Error Text/message
                string ResPaymentId = Request.Form["paymentid"];    //Payment Id
                string ResTrackidReferenceCode = Request.Form["trackid"];        //Merchant Track ID
                string ResErrorNo = Request.Form["Error"];          //Error Number
                                                                    //To collect transaction result
                string TransactionStatus = Request.Form["result"] == "CAPTURED" ? "Success" : "Failed";          //Transaction Result
                string ResPosdate = Request.Form["postdate"];       //Postdate
                                                                    //To collect Payment Gateway Transaction ID, this value will be used in dual verification request
                string TransactionCode = Request.Form["tranid"];          //Transaction ID
                string ResAuth = Request.Form["auth"];              //Auth Code		
                string ResAVR = Request.Form["avr"];                //TRANSACTION avr					
                string ResRef = Request.Form["ref"];                //Reference Number also called Seq Number
                                                                    //To collect amount from response
                string ResAmount = Request.Form["amt"];             //Transaction Amount
                string CustomerReference = Request.Form["udf1"];              //UDF1
                string CustomerEmail = Request.Form["udf2"];              //UDF2
                string CustomerPhone = Request.Form["udf3"];              //UDF3
                string CustomerName = Request.Form["udf4"];              //UDF4
                string HashingValue = Request.Form["udf5"];             //UDF5
                string amount = Request.Form["amt"];             //UDF5


                Sitecore.Diagnostics.Log.Info("Payment Gateway- HDFCCallBackS2S Callback Message - " + responsemsg, this);

                //error response
                var modelviewpay = new ViewPayBill()
                {
                    TransactionId = TransactionCode,
                    ResponseStatus = TransactionStatus,
                    Responsecode = responsemsg, // ErrorStatus
                    Remark = TransactionStatus, //DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                    PaymentRef = CustomerReference,
                    OrderId = ResTrackidReferenceCode,
                    AmountPayable = amount,
                    AccountNumber = CustomerReference,
                    LoginName = CustomerReference,
                    msg = responsemsg,
                    //PaymentMode = Resudf5,
                    TransactionDate = DateTime.Now.ToString()
                };
                this.PaymentService.StorePaymentResponse(modelviewpay);

                if (TransactionStatus == "Success")
                {
                    TempData["PaymentResponse"] = modelviewpay;
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- HDFCCallBackS2S Response Success - ", ResTrackidReferenceCode);
                    return this.Redirect(SuccessUrl);
                }
                else
                {
                    TempData["PaymentResponse"] = modelviewpay;
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response Failure - ", ResTrackidReferenceCode);
                    return this.Redirect(FailureUrl);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BillDeskCallBackS2S - :" + ex.Message, this);
            }
            return this.Redirect(FailureUrl);
        }

        public ActionResult HDFC_UPI_CallbackError()
        {
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            string responsemsg = Request.Form["msg"];

            string ResErrorText = Request.Form["ErrorText"];    //Error Text/message
            string ResPaymentId = Request.Form["paymentid"];    //Payment Id
            string ResTrackidReferenceCode = Request.Form["trackid"];        //Merchant Track ID
            string ResErrorNo = Request.Form["Error"];

            if (!string.IsNullOrEmpty(ResErrorNo) || !string.IsNullOrEmpty(ResErrorText) || !string.IsNullOrEmpty(responsemsg))
            {
                return this.Redirect(FailureUrl);
            }
            else
            {
                return this.Redirect(SuccessUrl);
            }
            //var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            //try
            //{
            //    //HDFC Response
            //    string responsemsg = Request.Form["msg"];

            //    string ResErrorText = Request.Form["ErrorText"];    //Error Text/message
            //    string ResPaymentId = Request.Form["paymentid"];    //Payment Id
            //    string ResTrackidReferenceCode = Request.Form["trackid"];        //Merchant Track ID
            //    string ResErrorNo = Request.Form["Error"];          //Error Number
            //                                                        //To collect transaction result
            //    string TransactionStatus = Request.Form["result"] == "CAPTURED" ? "Success" : "Failed";          //Transaction Result
            //    string ResPosdate = Request.Form["postdate"];       //Postdate
            //                                                        //To collect Payment Gateway Transaction ID, this value will be used in dual verification request
            //    string TransactionCode = Request.Form["tranid"];          //Transaction ID
            //    string ResAuth = Request.Form["auth"];              //Auth Code		
            //    string ResAVR = Request.Form["avr"];                //TRANSACTION avr					
            //    string ResRef = Request.Form["ref"];                //Reference Number also called Seq Number
            //                                                        //To collect amount from response
            //    string ResAmount = Request.Form["amt"];             //Transaction Amount
            //    string CustomerReference = Request.Form["udf1"];              //UDF1
            //    string CustomerEmail = Request.Form["udf2"];              //UDF2
            //    string CustomerPhone = Request.Form["udf3"];              //UDF3
            //    string CustomerName = Request.Form["udf4"];              //UDF4
            //    string HashingValue = Request.Form["udf5"];				//UDF5
            //    string amount = Request.Form["amt"];             //UDF5


            //    Sitecore.Diagnostics.Log.Info("Payment Gateway- HDFCCallBackS2S Callback Message - " + responsemsg, this);

            //    //error response
            //    var modelviewpay = new ViewPayBill()
            //    {
            //        TransactionId = TransactionCode,
            //        ResponseStatus = TransactionStatus,
            //        Responsecode = responsemsg, // ErrorStatus
            //        Remark = TransactionStatus, //DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
            //        PaymentRef = CustomerReference,
            //        OrderId = ResTrackidReferenceCode,
            //        AmountPayable = amount,
            //        AccountNumber = CustomerReference,
            //        LoginName = CustomerReference,
            //        msg = responsemsg,
            //        //PaymentMode = Resudf5,
            //        TransactionDate = DateTime.Now.ToString()
            //    };
            //    this.PaymentService.StorePaymentResponse(modelviewpay);

            //    if (TransactionStatus == "Success")
            //    {
            //        TempData["PaymentResponse"] = modelviewpay;
            //        Sitecore.Diagnostics.Log.Info("Payment Gateway- HDFCCallBackS2S Response Success - ", ResTrackidReferenceCode);
            //        return this.Redirect(SuccessUrl);
            //    }
            //    else
            //    {
            //        TempData["PaymentResponse"] = modelviewpay;
            //        Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response Failure - ", ResTrackidReferenceCode);
            //        return this.Redirect(FailureUrl);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Sitecore.Diagnostics.Log.Error("Error at BillDeskCallBackS2S - :" + ex.Message, this);
            //}
            //return this.Redirect(FailureUrl);
        }

        #endregion

        public ActionResult BillDeskCallBack()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            try
            {
                #region Variable Initialization
                string checksum = string.Empty;
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Merchant_ID].Value;

                string VDSChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
                string VDSMerchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_Merchant_ID].Value;

                #endregion

                //BillDesk Response
                string responsemsg = Request.Form["msg"];
                Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Callback Message - " + responsemsg, this);

                //if (responsemsg.StartsWith(VDSMerchantId))
                //{
                //    ChecksumKey = VDSChecksumKey; //VDS checksum key
                //}

                if (responsemsg != null)
                {
                    var responselist = responsemsg.Split('|');
                    var billDeskchecksum = responselist.Last().Trim();
                    var responsemsgdata = string.Join("|", responselist.Take(responselist.Count() - 1).ToArray());//responselist.Take(responselist.Count() - 1).ToArray().Join(;

                    var checksumresponse = this.PaymentService.GetHMACSHA256(responsemsgdata, ChecksumKey);

                    if (checksumresponse.Equals(billDeskchecksum)) // Compare Checksum
                    {
                        if (Constants.BillDeskResponse.SuccessCode.Equals(responselist[14].ToString()))
                        {
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = responselist[2].ToString(),
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = responselist[14].ToString(),
                                Remark = Constants.PaymentResponse.Success,
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[19].ToString(),
                                AmountPayable = responselist[4].ToString(),
                                AccountNumber = responselist[1].ToString(),
                                LoginName = responselist[21].ToString(),
                                msg = responsemsg,
                                PaymentMode = responselist[9].ToString(),
                                PaymentModeNumber= responselist[7].ToString(),
                                TransactionDate = responselist[13].ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Realty/Payment Type/Bill Paid", "Bill Paid"),
                                PaymentGateway = 2
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);

                            //PI Service Integration


                            TempData["PaymentResponse"] = modelviewpay;

                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response - ", responsemsg);
                            return this.Redirect(SuccessUrl);
                        }
                        else
                        {
                            //error response
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = responselist[2].ToString(),
                                ResponseStatus = Constants.PaymentResponse.Failure,
                                Responsecode = responselist[14].ToString(), // ErrorStatus
                                Remark = responselist[24].ToString(), //DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[19].ToString(),
                                AmountPayable = responselist[4].ToString(),
                                AccountNumber = responselist[1].ToString(),
                                LoginName = responselist[21].ToString(),
                                msg = responsemsg,
                                PaymentMode = responselist[9].ToString(),
                                TransactionDate = responselist[13].ToString()
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);

                            TempData["PaymentResponse"] = modelviewpay;
                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response Failure - ", responsemsg);
                            return this.Redirect(FailureUrl);
                        }
                    }
                    else
                    {
                        //Checksum Mismatch
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = responselist[2].ToString(),
                            ResponseStatus = Constants.PaymentResponse.Failure,
                            Responsecode = responselist[14].ToString(), // ErrorStatus
                            Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                            PaymentRef = responselist[3].ToString(),
                            OrderId = responselist[19].ToString(),
                            AmountPayable = responselist[4].ToString(),
                            AccountNumber = responselist[1].ToString(),
                            LoginName = responselist[21].ToString(),
                            msg = responsemsg,
                            PaymentMode = responselist[9].ToString(),
                            TransactionDate = responselist[13].ToString()
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        TempData["PaymentResponse"] = modelviewpay;
                        Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response checksum mismatch - " + responsemsg, this);
                        return this.Redirect(FailureUrl);
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response NULL error - " + responsemsg, this);
                    return this.Redirect(FailureUrl);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BillDeskCallBack - :" + ex.Message, this);
                return this.Redirect(FailureUrl);
            }
        }

        public ActionResult BillDeskVDSCallBack()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            try
            {
                #region Variable Initialization
                string checksum = string.Empty;
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Merchant_ID].Value;

                string VDSChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
                string VDSMerchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_Merchant_ID].Value;

                #endregion

                //BillDesk Response
                string responsemsg = Request.Form["msg"];
                Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskVDSCallBack Callback Message - " + responsemsg, this);

                if (responsemsg.StartsWith(VDSMerchantId))
                {
                    ChecksumKey = VDSChecksumKey; //VDS checksum key
                }

                if (responsemsg != null)
                {
                    var responselist = responsemsg.Split('|');
                    var billDeskchecksum = responselist.Last().Trim();
                    var responsemsgdata = string.Join("|", responselist.Take(responselist.Count() - 1).ToArray());//responselist.Take(responselist.Count() - 1).ToArray().Join(;

                    var checksumresponse = this.PaymentService.GetHMACSHA256(responsemsgdata, ChecksumKey);

                    if (checksumresponse.Equals(billDeskchecksum)) // Compare Checksum
                    {
                        if (Constants.BillDeskResponse.SuccessCode.Equals(responselist[14].ToString()))
                        {
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = responselist[2].ToString(),
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = responselist[14].ToString(),
                                Remark = Constants.PaymentResponse.Success,
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[1].ToString(),
                                AmountPayable = responselist[4].ToString(),
                                AccountNumber = responselist[16].ToString(),
                                LoginName = responselist[17].ToString(),
                                msg = responsemsg,
                                PaymentMode = responselist[9].ToString(),

                                TransactionDate = responselist[13].ToString(),
                                PaymentGateway = 2
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);

                            TempData["PaymentResponse"] = modelviewpay;

                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response - ", responsemsg);
                            return this.Redirect(SuccessUrl);
                        }
                        else
                        {
                            //error response
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = responselist[2].ToString(),
                                ResponseStatus = Constants.PaymentResponse.Failure,
                                Responsecode = responselist[14].ToString(), // ErrorStatus
                                Remark = responselist[24].ToString(), //DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[1].ToString(),
                                AmountPayable = responselist[4].ToString(),
                                AccountNumber = responselist[16].ToString(),
                                LoginName = responselist[17].ToString(),
                                msg = responsemsg,
                                PaymentMode = responselist[9].ToString(),
                                TransactionDate = responselist[13].ToString(),
                                PaymentGateway = 2
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);

                            TempData["PaymentResponse"] = modelviewpay;
                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response Failure - ", responsemsg);
                            return this.Redirect(FailureUrl);
                        }
                    }
                    else
                    {
                        //Checksum Mismatch
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = responselist[2].ToString(),
                            ResponseStatus = Constants.PaymentResponse.Failure,
                            Responsecode = responselist[14].ToString(), // ErrorStatus
                            Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                            PaymentRef = responselist[3].ToString(),
                            OrderId = responselist[1].ToString(),
                            AmountPayable = responselist[4].ToString(),
                            AccountNumber = responselist[16].ToString(),
                            LoginName = responselist[17].ToString(),
                            msg = responsemsg,
                            PaymentMode = responselist[9].ToString(),
                            TransactionDate = responselist[13].ToString(),
                            PaymentGateway = 2
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        TempData["PaymentResponse"] = modelviewpay;
                        Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response checksum mismatch - " + responsemsg, this);
                        return this.Redirect(FailureUrl);
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response NULL error - " + responsemsg, this);
                    return this.Redirect(FailureUrl);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BillDeskCallBack - :" + ex.Message, this);
                return this.Redirect(FailureUrl);
            }
        }

        public void BillDeskCallBackS2S()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            try
            {
                #region Variable Initialization
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Merchant_ID].Value;

                string VDSChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
                string VDSMerchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_Merchant_ID].Value;

                #endregion

                //BillDesk Response
                string responsemsg = Request.Form["msg"];

                Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Callback Message - " + responsemsg, this);

                if (responsemsg.StartsWith(VDSMerchantId))
                {
                    ChecksumKey = VDSChecksumKey; //VDS checksum key
                }

                if (responsemsg != null)
                {
                    var responselist = responsemsg.Split('|');
                    var billDeskchecksum = responselist.Last().Trim();
                    var responsemsgdata = string.Join("|", responselist.Take(responselist.Count() - 1).ToArray());//responselist.Take(responselist.Count() - 1).ToArray().Join(;

                    var checksumresponse = this.PaymentService.GetHMACSHA256(responsemsgdata, ChecksumKey);
                    var amount = Convert.ToDecimal(responselist[4].ToString()).ToString();

                    if (checksumresponse.Equals(billDeskchecksum)) // Compare Checksum
                    {
                        if (Constants.BillDeskResponse.SuccessCode.Equals(responselist[14].ToString()))
                        {
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = responselist[2].ToString(),
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = responselist[14].ToString(),
                                Remark = Constants.PaymentResponse.Success,
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[19].ToString(),
                                AmountPayable = amount,// responselist[4].ToString(),
                                AccountNumber = responselist[1].ToString(),
                                LoginName = responselist[21].ToString(),
                                msg = responsemsg,
                                TransactionDate = responselist[13].ToString(),
                                PaymentMode = responselist[9].ToString(),
                                Email = responselist[20].ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                                PaymentGateway = 2
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);
                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Success Response - " + responsemsg, this);
                        }
                        else
                        {
                            //error response
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = responselist[2].ToString(),
                                ResponseStatus = Constants.PaymentResponse.Failure,
                                Responsecode = responselist[14].ToString(), // ErrorStatus
                                Remark = responselist[24].ToString(), //DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[19].ToString(),
                                AmountPayable = amount,// responselist[4].ToString(),
                                AccountNumber = responselist[1].ToString(),
                                LoginName = responselist[21].ToString(),
                                TransactionDate = responselist[13].ToString(),
                                msg = responsemsg,
                                Email = responselist[20].ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                                PaymentGateway = 2,
                                PaymentMode = responselist[9].ToString()
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);
                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Response Failure - " + responsemsg, this);
                        }
                    }
                    else
                    {
                        //Checksum Mismatch
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = responselist[2].ToString(),
                            ResponseStatus = Constants.PaymentResponse.Failure,
                            Responsecode = responselist[14].ToString(), // ErrorStatus
                            Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                            PaymentRef = responselist[3].ToString(),
                            OrderId = responselist[19].ToString(),
                            AmountPayable = amount,// responselist[4].ToString(),
                            AccountNumber = responselist[1].ToString(),
                            LoginName = responselist[21].ToString(),
                            TransactionDate = responselist[13].ToString(),
                            msg = responsemsg,
                            Email = responselist[20].ToString(),
                            PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                            PaymentMode = responselist[9].ToString(),
                            PaymentGateway = 2
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Response checksum mismatch - ", "Checksum Mismatch for Billdesk response");
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Response NULL error - " + responsemsg, this);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BillDeskCallBackS2S - :" + ex.Message, this);
            }
        }

        public void BillDeskVDSCallBackS2S()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            try
            {
                #region Variable Initialization
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Merchant_ID].Value;

                string VDSChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
                string VDSMerchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_Merchant_ID].Value;

                #endregion

                //BillDesk Response
                string responsemsg = Request.Form["msg"];
                Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskVDSCallBackS2S Callback Message - " + responsemsg, this);

                if (responsemsg.StartsWith(VDSMerchantId))
                {
                    ChecksumKey = VDSChecksumKey; //VDS checksum key
                }
                #endregion


                if (responsemsg != null)
                {
                    var responselist = responsemsg.Split('|');
                    var billDeskchecksum = responselist.Last().Trim();
                    var responsemsgdata = string.Join("|", responselist.Take(responselist.Count() - 1).ToArray());//responselist.Take(responselist.Count() - 1).ToArray().Join(;

                    var checksumresponse = this.PaymentService.GetHMACSHA256(responsemsgdata, ChecksumKey);

                    if (checksumresponse.Equals(billDeskchecksum)) // Compare Checksum
                    {
                        if (Constants.BillDeskResponse.SuccessCode.Equals(responselist[14].ToString()))
                        {
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = responselist[2].ToString(),
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = responselist[14].ToString(),
                                Remark = Constants.PaymentResponse.Success,
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[1].ToString(),
                                AmountPayable = responselist[4].ToString(),
                                AccountNumber = responselist[17].ToString(),
                                LoginName = responselist[16].ToString(),
                                msg = responsemsg,
                                TransactionDate = responselist[13].ToString(),
                                PaymentMode = responselist[9].ToString(),
                                Email = responselist[20].ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/VDS", "VDS Paid"),
                                PaymentGateway = 2
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);
                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Success Response - " + responsemsg, this);
                        }
                        else
                        {
                            //error response
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = responselist[2].ToString(),
                                ResponseStatus = Constants.PaymentResponse.Failure,
                                Responsecode = responselist[14].ToString(), // ErrorStatus
                                Remark = responselist[24].ToString(), //DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[1].ToString(),
                                AmountPayable = responselist[4].ToString(),
                                AccountNumber = responselist[17].ToString(),
                                LoginName = responselist[16].ToString(),
                                msg = responsemsg,
                                TransactionDate = responselist[13].ToString(),
                                PaymentMode = responselist[9].ToString(),
                                Email = responselist[20].ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/VDS", "VDS Paid"),
                                PaymentGateway = 2
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);
                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Response Failure - " + responsemsg, this);
                        }
                    }
                    else
                    {
                        //Checksum Mismatch
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = responselist[2].ToString(),
                            ResponseStatus = Constants.PaymentResponse.Failure,
                            Responsecode = responselist[14].ToString(), // ErrorStatus
                            Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                            PaymentRef = responselist[3].ToString(),
                            OrderId = responselist[1].ToString(),
                            AmountPayable = responselist[4].ToString(),
                            AccountNumber = responselist[17].ToString(),
                            LoginName = responselist[16].ToString(),
                            msg = responsemsg,
                            PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/VDS", "VDS Paid"),
                            PaymentMode = responselist[9].ToString(),
                            TransactionDate = responselist[13].ToString(),
                            Email = responselist[20].ToString(),
                            PaymentGateway = 2
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Response checksum mismatch - ", "Checksum Mismatch for Billdesk response");
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Response NULL error - " + responsemsg, this);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BillDeskCallBackS2S - :" + ex.Message, this);
            }
        }

        #region Discontinued - PauUMoney EBIX Cash
        public ActionResult PayUCallBack()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            string[] merc_hash_vars_seq;
            string merc_hash_string, merc_hash = string.Empty;

            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Merchant_Key].Value;
            string saltkKey = itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Merchant_Salt].Value;
            string hash_seq = itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Product_Info].Value;

            try
            {

                Dictionary<string, string> TransactionRequestAPIResponse = new Dictionary<string, string>();

                foreach (string key in Request.Form.Keys)
                {
                    TransactionRequestAPIResponse.Add(key.Trim(), Request.Form[key].Trim());
                }

                string PayUResponseMsg = string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray());

                Sitecore.Diagnostics.Log.Info("PayUMoney Response at PayUCallBack :" + PayUResponseMsg, this);
                //Note : write code to set parameters to Model Value.
                var modelviewpay = new ViewPayBill()
                {
                    TransactionId = TransactionRequestAPIResponse["mihpayid"], // PayUMoney Generated TrnansactionId
                    ResponseStatus = TransactionRequestAPIResponse["status"],
                    Responsecode = TransactionRequestAPIResponse["status"],
                    PaymentRef = !string.IsNullOrEmpty(TransactionRequestAPIResponse["issuing_bank"]) ? TransactionRequestAPIResponse["issuing_bank"] : string.Empty,
                    Remark = !string.IsNullOrEmpty(TransactionRequestAPIResponse["error_Message"]) ? TransactionRequestAPIResponse["error_Message"] : string.Empty,
                    OrderId = TransactionRequestAPIResponse["txnid"],
                    AmountPayable = TransactionRequestAPIResponse["amount"],
                    AccountNumber = TransactionRequestAPIResponse["productinfo"], // Account Number
                    LoginName = TransactionRequestAPIResponse["firstname"], // User Name for Login User
                    msg = PayUResponseMsg,
                    PaymentMode = TransactionRequestAPIResponse["mode"]
                };

                if (Request.Form["status"] == Constants.PayUResponseStatus.Success)
                {
                    merc_hash_vars_seq = hash_seq.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    merc_hash_string = saltkKey + "|" + Request.Form["status"];

                    foreach (string merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (Request.Form[merc_hash_var] != null ? Request.Form[merc_hash_var] : "");
                    }

                    merc_hash = this.PaymentService.Generatehash512(merc_hash_string).ToLower();

                    if (merc_hash != Request.Form["hash"])
                    {
                        Sitecore.Diagnostics.Log.Error("PayUMoney hash value miss matach at PayUCallBack : Hash value did not matched.", this);

                        //Value didn't match that means some paramter value change between transaction 
                        modelviewpay.Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details.");//"Technically difficulty in the system. Please contact customer care for more details.";
                        modelviewpay.Responsecode = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/PayU Hash MissMatch", "Hash value did not matched");//"Hash value did not matched";
                        TempData["PaymentResponse"] = modelviewpay;
                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        return this.Redirect(FailureUrl);
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Error("PayUMoney Success Response at PayUCallBack : " + TransactionRequestAPIResponse["status"], this);

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        TempData["PaymentResponse"] = modelviewpay;
                        return this.Redirect(SuccessUrl);
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("PayUMoney Failure Response at PayUCallBack : " + TransactionRequestAPIResponse["error_Message"], this);

                    modelviewpay.Responsecode = TransactionRequestAPIResponse["error"];
                    modelviewpay.Remark = TransactionRequestAPIResponse["error_Message"];

                    this.PaymentService.StorePaymentResponse(modelviewpay);
                    TempData["PaymentResponse"] = modelviewpay;
                    return this.Redirect(FailureUrl);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at PayUCallBack:" + ex.Message, this);
                return this.Redirect(FailureUrl);
            }
        }

        public ActionResult PayUCallBackS2S()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            string[] merc_hash_vars_seq;
            string merc_hash_string, merc_hash = string.Empty;

            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Merchant_Key].Value;
            string saltkKey = itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Merchant_Salt].Value;
            string hash_seq = itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Product_Info].Value;

            try
            {

                Dictionary<string, string> TransactionRequestAPIResponse = new Dictionary<string, string>();

                foreach (string key in Request.Form.Keys)
                {
                    TransactionRequestAPIResponse.Add(key.Trim(), Request.Form[key].Trim());
                }

                string PayUResponseMsg = string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray());

                Sitecore.Diagnostics.Log.Info("PayUMoney Response at PayUCallBackS2S :" + PayUResponseMsg, this);
                //Note : write code to set parameters to Model Value.
                var modelviewpay = new ViewPayBill()
                {
                    TransactionId = TransactionRequestAPIResponse["mihpayid"], // PayUMoney Generated TrnansactionId
                    ResponseStatus = TransactionRequestAPIResponse["status"],
                    Responsecode = TransactionRequestAPIResponse["status"],
                    PaymentRef = !string.IsNullOrEmpty(TransactionRequestAPIResponse["issuing_bank"]) ? TransactionRequestAPIResponse["issuing_bank"] : string.Empty,
                    Remark = !string.IsNullOrEmpty(TransactionRequestAPIResponse["error_Message"]) ? TransactionRequestAPIResponse["error_Message"] : string.Empty,
                    OrderId = TransactionRequestAPIResponse["txnid"],
                    AmountPayable = TransactionRequestAPIResponse["amount"],
                    AccountNumber = TransactionRequestAPIResponse["productinfo"], // Account Number
                    LoginName = TransactionRequestAPIResponse["firstname"], // User Name for Login User
                    msg = PayUResponseMsg,
                    PaymentMode = TransactionRequestAPIResponse["mode"]
                };

                if (Request.Form["status"] == Constants.PayUResponseStatus.Success)
                {
                    merc_hash_vars_seq = hash_seq.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    merc_hash_string = saltkKey + "|" + Request.Form["status"];

                    foreach (string merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (Request.Form[merc_hash_var] != null ? Request.Form[merc_hash_var] : "");
                    }

                    merc_hash = this.PaymentService.Generatehash512(merc_hash_string).ToLower();

                    if (merc_hash != Request.Form["hash"])
                    {
                        Sitecore.Diagnostics.Log.Error("PayUMoney hash value miss matach at PayUCallBackS2S : Hash value did not matched.", this);

                        //Value didn't match that means some paramter value change between transaction 
                        modelviewpay.Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details.");//"Technically difficulty in the system. Please contact customer care for more details.";
                        modelviewpay.Responsecode = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/PayU Hash MissMatch", "Hash value did not matched");//"Hash value did not matched";
                        TempData["PaymentResponse"] = modelviewpay;
                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        return null;
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Error("PayUMoney Success Response at PayUCallBackS2S : " + TransactionRequestAPIResponse["status"], this);

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        TempData["PaymentResponse"] = modelviewpay;
                        return null;
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("PayUMoney Failure Response at PayUCallBackS2S : " + TransactionRequestAPIResponse["error_Message"], this);

                    modelviewpay.Responsecode = TransactionRequestAPIResponse["error"];
                    modelviewpay.Remark = TransactionRequestAPIResponse["error_Message"];

                    this.PaymentService.StorePaymentResponse(modelviewpay);
                    TempData["PaymentResponse"] = modelviewpay;
                    return null;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at PayUCallBackS2S :" + ex.Message, this);
                return null;
            }
        }

        public ActionResult EBixCashCallBack()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);

            try
            {
                #region Variable Initialization
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));
                string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.EbixCashFields.EBIX_MERCHANT_KEY].Value;
                #endregion

                #region Response Parameters setup after Transaction Request API Call
                Dictionary<string, string> TransactionRequestAPIResponse = new Dictionary<string, string>();
                foreach (string key in Request.Form.Keys)
                {
                    TransactionRequestAPIResponse.Add(key.Trim(), Request.Form[key].Trim());
                }
                #endregion

                Sitecore.Diagnostics.Log.Info("Payment Gateway - EBixCashCallBack- Response - " + string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray()), this);

                if (TransactionRequestAPIResponse["responsecode"] == "0")
                {
                    #region Transaction Status API Call after checksum matched.

                    string responseData = this.PaymentService.EbixTransactionStatusAPIRequestPost(TransactionRequestAPIResponse);
                    try
                    {
                        #region Set model property from recieved response and Store Response to sitecore Item

                        Sitecore.Diagnostics.Log.Info("Payment Gateway -EBixCashCallBack- Transaction Status API Response", responseData);

                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = TransactionRequestAPIResponse["transactionid"],
                            ResponseStatus = responseData.Split(',')[0] == "0" ? Constants.PaymentResponse.Success : Constants.PaymentResponse.Failure,
                            Responsecode = responseData.Split(',')[0],
                            Remark = TransactionRequestAPIResponse["description"],
                            PaymentRef = TransactionRequestAPIResponse["actiontype"],
                            OrderId = TransactionRequestAPIResponse["orderid"],
                            AmountPayable = (System.Convert.ToDecimal(TransactionRequestAPIResponse["productcost"]) / 100).ToString(),
                            AccountNumber = TransactionRequestAPIResponse["accountno"],
                            LoginName = TransactionRequestAPIResponse["loginname"],
                            msg = string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray()),
                            PaymentMode = string.Empty,
                            TransactionDate = TransactionRequestAPIResponse["transactiondate"]
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        TempData["PaymentResponse"] = modelviewpay;

                        if (responseData != null && responseData.Split(',')[0] == "0")
                        {
                            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);
                            return this.Redirect(SuccessUrl);
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Info("Payment Gateway Failed- EBixCashCallBack Response - " + string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray()), this);
                            return this.Redirect(FailureUrl);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Info("Payment Gateway- EBixCashCallBack Response -" + ex.Message, this);
                    }
                    #endregion
                }
                else
                {
                    var modelviewpay = new ViewPayBill()
                    {
                        TransactionId = TransactionRequestAPIResponse.ContainsKey("transactionid") ? TransactionRequestAPIResponse["transactionid"] : "-",
                        ResponseStatus = Constants.PaymentResponse.Failure,
                        Responsecode = TransactionRequestAPIResponse["responsecode"],
                        Remark = TransactionRequestAPIResponse["description"],
                        PaymentRef = TransactionRequestAPIResponse["actiontype"],
                        OrderId = TransactionRequestAPIResponse["orderid"],
                        AmountPayable = (System.Convert.ToDecimal(TransactionRequestAPIResponse["productcost"]) / 100).ToString(),
                        AccountNumber = TransactionRequestAPIResponse["accountno"],
                        LoginName = TransactionRequestAPIResponse["loginname"],
                        msg = string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray()),
                        PaymentMode = string.Empty,
                        TransactionDate = TransactionRequestAPIResponse["transactiondate"]
                    };

                    this.PaymentService.StorePaymentResponse(modelviewpay);
                    TempData["PaymentResponse"] = modelviewpay;
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- EBixCashCallBack Response  - ", "Error in Ebixcash Response Code -" + TransactionRequestAPIResponse["responsecode"]);
                    return this.Redirect(FailureUrl);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at EBixCashCallBack :" + ex.Message, this);
                return this.Redirect(FailureUrl);
            }
            return this.Redirect(FailureUrl);
        }

        public void EBixCashCallBackS2S()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            try
            {
                #region Variable Initialization
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));
                string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.EbixCashFields.EBIX_MERCHANT_KEY].Value;
                #endregion

                #region Response Parameters setup after Transaction Request API Call
                Dictionary<string, string> TransactionRequestAPIResponse = new Dictionary<string, string>();
                foreach (string key in Request.Form.Keys)
                {
                    TransactionRequestAPIResponse.Add(key.Trim(), Request.Form[key].Trim());
                }
                #endregion

                Sitecore.Diagnostics.Log.Info("Payment Gateway - EBixCashCallBackS2S- Response - " + string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray()), this);

                if (TransactionRequestAPIResponse["responsecode"] == "0")
                {
                    #region Transaction Status API Call after checksum matched.

                    string responseData = this.PaymentService.EbixTransactionStatusAPIRequestPost(TransactionRequestAPIResponse);
                    try
                    {
                        #region Set model property from recieved response and Store Response to sitecore Item

                        Sitecore.Diagnostics.Log.Info("Payment Gateway -EBixCashCallBack- Transaction Status API Response - " + responseData, this);

                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = TransactionRequestAPIResponse["transactionid"],
                            ResponseStatus = responseData.Split(',')[0] == "0" ? Constants.PaymentResponse.Success : Constants.PaymentResponse.Failure,
                            Responsecode = responseData.Split(',')[0],
                            Remark = TransactionRequestAPIResponse["description"],
                            PaymentRef = TransactionRequestAPIResponse["actiontype"],
                            OrderId = TransactionRequestAPIResponse["orderid"],
                            AmountPayable = (System.Convert.ToDecimal(TransactionRequestAPIResponse["productcost"]) / 100).ToString(),
                            AccountNumber = TransactionRequestAPIResponse["accountno"],
                            LoginName = TransactionRequestAPIResponse["loginname"],
                            msg = string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray()),
                            PaymentMode = string.Empty
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);

                        Sitecore.Diagnostics.Log.Info("Payment Gateway- EBixCashCallBackS2S Response - " + string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray()), this);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Info("Payment Gateway- EBixCashCallBackS2S Response - " + ex.Message, this);
                    }
                    #endregion
                }
                else
                {
                    var modelviewpay = new ViewPayBill()
                    {
                        TransactionId = TransactionRequestAPIResponse.ContainsKey("transactionid") ? TransactionRequestAPIResponse["transactionid"] : "-",
                        ResponseStatus = Constants.PaymentResponse.Failure,
                        Responsecode = TransactionRequestAPIResponse["responsecode"],
                        Remark = TransactionRequestAPIResponse["description"],
                        PaymentRef = TransactionRequestAPIResponse["actiontype"],
                        OrderId = TransactionRequestAPIResponse["orderid"],
                        AmountPayable = (System.Convert.ToDecimal(TransactionRequestAPIResponse["productcost"]) / 100).ToString(),
                        AccountNumber = TransactionRequestAPIResponse["accountno"],
                        LoginName = TransactionRequestAPIResponse["loginname"],
                        msg = string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray()),
                        PaymentMode = string.Empty
                    };

                    this.PaymentService.StorePaymentResponse(modelviewpay);

                    Sitecore.Diagnostics.Log.Info("Payment Gateway- EBixCashCallBackS2S Response  - Error in Ebixcash Response Code -" + TransactionRequestAPIResponse["responsecode"], this);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at EBixCashCallBackS2S :" + ex.Message, this);
            }
        }

        #endregion

        [HttpPost]
        public ActionResult PaytmCallBack()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);

            try
            {
                #region Variable Initialization

                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_Merchant_Key].Value;
                string merchantID = itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_Merchant_ID].Value;

                #endregion

                #region Response Parameters setup after Transaction Request API Call

                Dictionary<string, string> TransactionRequestAPIResponse = new Dictionary<string, string>();
                string paytmChecksum = "";

                foreach (string key in Request.Form.Keys)
                {
                    TransactionRequestAPIResponse.Add(key.Trim(), Request.Form[key].Trim());
                }

                if (TransactionRequestAPIResponse.ContainsKey("CHECKSUMHASH"))
                {
                    paytmChecksum = TransactionRequestAPIResponse["CHECKSUMHASH"];
                    TransactionRequestAPIResponse.Remove("CHECKSUMHASH");
                }
                #endregion

                if (CheckSum.verifyCheckSum(merchantKey, TransactionRequestAPIResponse, paytmChecksum))
                {

                    #region Transaction Status API Call after checksum matched.

                    string responseData = this.PaymentService.PaytmTransactionStatusAPIRequestPost(TransactionRequestAPIResponse);
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- PaytmCallBack Callback Message - " + responseData, this);
                    try
                    {
                        var TransactionStatusApiresponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseData);

                        #region Set model property from recieved response and Store Response to sitecore Item

                        //Note : set message value with Pipe Seprated.
                        string otherResponseMsg = string.Join("|", TransactionStatusApiresponse.Select(x => x.Key + ":" + x.Value).ToArray());

                        string txnDate = string.Empty;
                        if (!string.IsNullOrEmpty(TransactionStatusApiresponse["TXNDATE"]))
                        {
                            string txnDatetemp = TransactionStatusApiresponse["TXNDATE"];
                            DateTime DT = DateTime.ParseExact(txnDatetemp, "yyyy-MM-dd HH:mm:ss.0", System.Globalization.CultureInfo.CurrentCulture);
                            txnDate = DT.ToString("dd-MM-yyyy HH:mm:ss");
                        }

                        //Note : write code to set parameters to Model Value.
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = TransactionStatusApiresponse["TXNID"],
                            ResponseStatus = TransactionStatusApiresponse["STATUS"],
                            Responsecode = TransactionStatusApiresponse["RESPCODE"],
                            Remark = TransactionStatusApiresponse["RESPMSG"],
                            PaymentRef = TransactionStatusApiresponse["BANKNAME"],
                            OrderId = TransactionStatusApiresponse["ORDERID"],
                            AmountPayable = TransactionStatusApiresponse["TXNAMOUNT"],
                            AccountNumber = TransactionRequestAPIResponse["MERC_UNQ_REF"].Split('_')[0],
                            LoginName = TransactionRequestAPIResponse["MERC_UNQ_REF"].Split('_')[1],
                            msg = otherResponseMsg,
                            PaymentMode = TransactionStatusApiresponse["PAYMENTMODE"],
                            TransactionDate = txnDate,
                            PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                            PaymentGateway = 3
                        };
                        this.PaymentService.StorePaymentResponse(modelviewpay);

                        #endregion
                        TempData["PaymentResponse"] = modelviewpay;

                        Sitecore.Diagnostics.Log.Info("Payment Gateway- Transaction Status API Response" + responseData, this);

                        if ((string.Equals(TransactionStatusApiresponse["TXNAMOUNT"], TransactionRequestAPIResponse["TXNAMOUNT"]))
                                && (string.Equals(TransactionStatusApiresponse["STATUS"], Constants.PaytmResponseStatus.Success)))
                        {
                            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

                            return this.Redirect(SuccessUrl);
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Info("Payment Gateway- PaytmCallBack Response Failed due to ammount or status mismatch", this);
                            return this.Redirect(FailureUrl);
                        }

                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Info("Payment Gateway- PaytmCallBack Response " + ex.Message, this);
                    }

                    #endregion
                }
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- PaytmCallBack Callback Message checksum mismatch for orderid and Transaction id - " + TransactionRequestAPIResponse["ORDERID"] + " , " + TransactionRequestAPIResponse["TXNID"], this);
                    string txnDate = string.Empty;
                    if (!string.IsNullOrEmpty(TransactionRequestAPIResponse["TXNDATE"]))
                    {
                        string txnDatetemp = TransactionRequestAPIResponse["TXNDATE"];
                        DateTime DT = DateTime.ParseExact(txnDatetemp, "yyyy-MM-dd HH:mm:ss.0", System.Globalization.CultureInfo.CurrentCulture);
                        txnDate = DT.ToString("dd-MM-yyyy HH:mm:ss");
                    }

                    var modelviewpay = new ViewPayBill()
                    {
                        TransactionId = TransactionRequestAPIResponse["TXNID"],
                        ResponseStatus = TransactionRequestAPIResponse["STATUS"],
                        Responsecode = TransactionRequestAPIResponse["RESPCODE"],
                        Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                        PaymentRef = TransactionRequestAPIResponse["BANKNAME"],
                        OrderId = TransactionRequestAPIResponse["ORDERID"],
                        AmountPayable = TransactionRequestAPIResponse["TXNAMOUNT"],
                        AccountNumber = TransactionRequestAPIResponse["MERC_UNQ_REF"].Split('_')[0],
                        LoginName = TransactionRequestAPIResponse["MERC_UNQ_REF"].Split('_')[1],
                        msg = string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray()),
                        PaymentMode = TransactionRequestAPIResponse["PAYMENTMODE"],
                        TransactionDate = txnDate,
                        PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                        PaymentGateway = 3
                    };

                    this.PaymentService.StorePaymentResponse(modelviewpay);
                    TempData["PaymentResponse"] = modelviewpay;
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- PaytmCallBack Response  - CheckSum Mismatch of TransactionRequestAPIResponse", this);
                    return this.Redirect(FailureUrl);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at PaytmCallBack :" + ex.Message, this);
                return this.Redirect(FailureUrl);
            }
            return this.Redirect(FailureUrl);
        }

        [HttpPost]
        public void PaytmCallBackS2S()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);

            try
            {
                #region Variable Initialization

                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_Merchant_Key].Value;
                string merchantID = itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_Merchant_ID].Value;

                #endregion

                #region Response Parameters setup after Transaction Request API Call

                Dictionary<string, string> TransactionRequestAPIResponse = new Dictionary<string, string>();
                string paytmChecksum = "";

                foreach (string key in Request.Form.Keys)
                {
                    TransactionRequestAPIResponse.Add(key.Trim(), Request.Form[key].Trim());
                }

                if (TransactionRequestAPIResponse.ContainsKey("CHECKSUMHASH"))
                {
                    paytmChecksum = TransactionRequestAPIResponse["CHECKSUMHASH"];
                    TransactionRequestAPIResponse.Remove("CHECKSUMHASH");
                }
                #endregion


                string otherResponseMsg = string.Join("|", TransactionRequestAPIResponse.Select(x => x.Key + ":" + x.Value).ToArray());

                Sitecore.Diagnostics.Log.Info("Payment Gateway S2S- PaytmCallBackS2S Response -" + otherResponseMsg, this);


                if (CheckSum.verifyCheckSum(merchantKey, TransactionRequestAPIResponse, paytmChecksum))
                {
                    #region Transaction Status API Call after checksum matched.
                    try
                    {
                        #region Set model property from recieved response and Store Response to sitecore Item

                        //Note : write code to set parameters to Model Value.
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = TransactionRequestAPIResponse["TXNID"],
                            ResponseStatus = TransactionRequestAPIResponse["STATUS"],
                            Responsecode = TransactionRequestAPIResponse["RESPCODE"],
                            Remark = TransactionRequestAPIResponse["RESPMSG"],
                            PaymentRef = TransactionRequestAPIResponse["BANKNAME"],
                            OrderId = TransactionRequestAPIResponse["ORDERID"],
                            AmountPayable = TransactionRequestAPIResponse["TXNAMOUNT"],
                            AccountNumber = TransactionRequestAPIResponse["MERC_UNQ_REF"],
                            LoginName = TransactionRequestAPIResponse["CUST_ID"],
                            msg = otherResponseMsg,
                            PaymentMode = TransactionRequestAPIResponse["PAYMENTMODE"],
                            PaymentGateway = 3,
                            PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                        };
                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Payment Gateway S2S- PaytmCallBackS2S Response - " + ex.Message, this);
                    }
                    #endregion
                }
                else
                {
                    var modelviewpay = new ViewPayBill()
                    {
                        TransactionId = TransactionRequestAPIResponse["TXNID"],
                        ResponseStatus = TransactionRequestAPIResponse["STATUS"],
                        Responsecode = TransactionRequestAPIResponse["RESPCODE"],
                        Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Checksum MissMatch", "Checksum Missmatch"),
                        PaymentRef = TransactionRequestAPIResponse["BANKNAME"],
                        OrderId = TransactionRequestAPIResponse["ORDERID"],
                        AmountPayable = TransactionRequestAPIResponse["TXNAMOUNT"],
                        AccountNumber = TransactionRequestAPIResponse["MERC_UNQ_REF"],
                        LoginName = TransactionRequestAPIResponse["CUST_ID"],
                        msg = otherResponseMsg,
                        PaymentMode = TransactionRequestAPIResponse["PAYMENTMODE"],
                        PaymentGateway = 3,
                        PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid")
                    };

                    this.PaymentService.StorePaymentResponse(modelviewpay);

                    Sitecore.Diagnostics.Log.Info("Payment Gateway- PaytmCallBackS2S Response  CheckSum Mismatch of TransactionRequestAPIResponse", this);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at PaytmCallBackS2S - : " + ex.Message, this);
            }
        }

        #region ICICI CallBack
        public ActionResult ICICICallBack()
        {
            string strPG_TxnStatus = string.Empty, strPG_ClintTxnRefNo = string.Empty, strPG_TPSLTxnBankCode = string.Empty, strPG_TPSLTxnID = string.Empty, strLoginName = string.Empty, strAccountNumber = string.Empty,
              strPG_TxnAmount = string.Empty, strPG_TxnDateTime = string.Empty, strPG_TxnDate = string.Empty, strPG_TxnTime = string.Empty, strClientBody = string.Empty, strErrorMessage = string.Empty;
            string strPGResponse;
            string[] strSplitDecryptedResponse;
            string[] strArrPG_TxnDateTime;

            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string MerchantCode = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.MerchantCode].Value;
            string SchemeCode = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.SchemeCode].Value;
            string EncryptionKey = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.EncryptionKey].Value;
            string EncryptionIV = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.EncryptionIV].Value;
            try
            {
                strPGResponse = Convert.ToString(Request.Form["msg"]);

                RequestURL objRequestURL = new RequestURL();

                //Decrypting the PG response

                //Verify Response using Key and Iv
                string strDecryptedVal = objRequestURL.VerifyPGResponse(strPGResponse, EncryptionKey, EncryptionIV);

                Sitecore.Diagnostics.Log.Info("Payment Gateway- ICICICallBack Response - " + strDecryptedVal, this);

                if (strDecryptedVal.StartsWith("ERROR"))
                {
                    string[] strGetMerchantParamForCompare;
                    string[] strclientbodydata;
                    strSplitDecryptedResponse = strPGResponse.Split('|');
                    for (int i = 0; i < strSplitDecryptedResponse.Length; i++)
                    {
                        strGetMerchantParamForCompare = strSplitDecryptedResponse[i].ToString().Split('=');
                        if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
                        {
                            strPG_TxnStatus = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
                        {
                            strPG_ClintTxnRefNo = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
                        {
                            strPG_TPSLTxnBankCode = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
                        {
                            strPG_TPSLTxnID = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
                        {
                            strPG_TxnAmount = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
                        {
                            strClientBody = Convert.ToString(strGetMerchantParamForCompare[1]);
                            strClientBody = strClientBody.Replace("{", "");
                            var strclientdata = strClientBody.Split('}');
                            for (int j = 0; j < strclientdata.Length; j++)
                            {
                                strclientbodydata = strclientdata[j].ToString().Split(':');
                                if (Convert.ToString(strclientbodydata[0]).ToUpper().Trim() == "CUSTID")
                                {
                                    strLoginName = Convert.ToString(strclientbodydata[1]);
                                }
                                else if (Convert.ToString(strclientbodydata[0]).ToUpper().Trim() == "CUSTNAME")
                                {
                                    strAccountNumber = Convert.ToString(strclientbodydata[1]);
                                }
                            }
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
                        {
                            strPG_TxnDateTime = Convert.ToString(strGetMerchantParamForCompare[1]);
                            strArrPG_TxnDateTime = strPG_TxnDateTime.Split(' ');
                            strPG_TxnDate = Convert.ToString(strArrPG_TxnDateTime[0]);
                            strPG_TxnTime = Convert.ToString(strArrPG_TxnDateTime[1]);
                        }
                    }

                    var modelviewpay = new ViewPayBill()
                    {
                        TransactionId = strPG_TPSLTxnID,
                        ResponseStatus = Constants.PaymentResponse.Failure,
                        Responsecode = strPG_TxnStatus,
                        Remark = Constants.PaymentResponse.Failure,
                        PaymentRef = string.Empty,
                        OrderId = strPG_ClintTxnRefNo,
                        AmountPayable = strPG_TxnAmount,
                        AccountNumber = strAccountNumber,
                        LoginName = strLoginName,
                        msg = strDecryptedVal,
                        PaymentMode = string.Empty,
                        TransactionDate = strPG_TxnDateTime
                    };
                    this.PaymentService.StorePaymentResponse(modelviewpay);
                    TempData["PaymentResponse"] = modelviewpay;

                    //Fail
                    return this.Redirect(FailureUrl);
                }
                else
                {
                    string[] strGetMerchantParamForCompare;
                    string[] strclientbodydata;
                    strSplitDecryptedResponse = strDecryptedVal.Split('|');
                    for (int i = 0; i < strSplitDecryptedResponse.Length; i++)
                    {
                        strGetMerchantParamForCompare = strSplitDecryptedResponse[i].ToString().Split('=');
                        if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
                        {
                            strPG_TxnStatus = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
                        {
                            strPG_ClintTxnRefNo = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
                        {
                            strPG_TPSLTxnBankCode = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
                        {
                            strPG_TPSLTxnID = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
                        {
                            strPG_TxnAmount = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_ERR_MSG")
                        {
                            strErrorMessage = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
                        {
                            strClientBody = Convert.ToString(strGetMerchantParamForCompare[1]);
                            strClientBody = strClientBody.Replace("{", "");
                            var strclientdata = strClientBody.Split('}');
                            for (int j = 0; j < strclientdata.Length; j++)
                            {
                                strclientbodydata = strclientdata[j].ToString().Split(':');
                                if (Convert.ToString(strclientbodydata[0]).ToUpper().Trim() == "CUSTID")
                                {
                                    strLoginName = Convert.ToString(strclientbodydata[1]);
                                }
                                else if (Convert.ToString(strclientbodydata[0]).ToUpper().Trim() == "CUSTNAME")
                                {
                                    strAccountNumber = Convert.ToString(strclientbodydata[1]);
                                }
                            }
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
                        {
                            strPG_TxnDateTime = Convert.ToString(strGetMerchantParamForCompare[1]);
                            strArrPG_TxnDateTime = strPG_TxnDateTime.Split(' ');
                            strPG_TxnDate = Convert.ToString(strArrPG_TxnDateTime[0]);
                            strPG_TxnTime = Convert.ToString(strArrPG_TxnDateTime[1]);
                        }
                    }

                    if (strPG_TxnStatus == "0300")
                    {
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = strPG_TPSLTxnID,
                            ResponseStatus = Constants.PaymentResponse.Success,
                            Responsecode = strPG_TxnStatus,
                            Remark = Constants.PaymentResponse.Success,
                            PaymentRef = string.Empty,
                            OrderId = strPG_ClintTxnRefNo,
                            AmountPayable = strPG_TxnAmount,
                            AccountNumber = strAccountNumber,
                            LoginName = strLoginName,
                            msg = strDecryptedVal,
                            PaymentMode = string.Empty,
                            TransactionDate = strPG_TxnDateTime
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        TempData["PaymentResponse"] = modelviewpay;
                        //Success
                        return this.Redirect(SuccessUrl);
                    }
                    else
                    {
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = strPG_TPSLTxnID,
                            ResponseStatus = Constants.PaymentResponse.Failure,
                            Responsecode = strPG_TxnStatus,
                            Remark = strErrorMessage,
                            PaymentRef = string.Empty,
                            OrderId = strPG_ClintTxnRefNo,
                            AmountPayable = strPG_TxnAmount,
                            AccountNumber = strAccountNumber,
                            LoginName = strLoginName,
                            msg = strDecryptedVal,
                            PaymentMode = string.Empty,
                            TransactionDate = strPG_TxnDateTime
                        };
                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        TempData["PaymentResponse"] = modelviewpay;

                        //Fail
                        return this.Redirect(FailureUrl);
                    }
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ICICICallBack:" + ex.Message, this);
                return this.Redirect(FailureUrl);
            }
        }

        public void ICICICallBackS2S()
        {
            string strPG_TxnStatus = string.Empty, strPG_ClintTxnRefNo = string.Empty, strPG_TPSLTxnBankCode = string.Empty, strPG_TPSLTxnID = string.Empty, strLoginName = string.Empty, strAccountNumber = string.Empty,
              strPG_TxnAmount = string.Empty, strPG_TxnDateTime = string.Empty, strPG_TxnDate = string.Empty, strPG_TxnTime = string.Empty, strClientBody = string.Empty, strErrorMessage = string.Empty;
            string strPGResponse;
            string[] strSplitDecryptedResponse;
            string[] strArrPG_TxnDateTime;

            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string MerchantCode = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.MerchantCode].Value;
            string SchemeCode = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.SchemeCode].Value;
            string EncryptionKey = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.EncryptionKey].Value;
            string EncryptionIV = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.EncryptionIV].Value;
            try
            {
                strPGResponse = Convert.ToString(Request.Form["msg"]);

                RequestURL objRequestURL = new RequestURL();

                //Decrypting the PG response

                //Verify Response using Key and Iv
                string strDecryptedVal = objRequestURL.VerifyPGResponse(strPGResponse, EncryptionKey, EncryptionIV);

                Sitecore.Diagnostics.Log.Info("Payment Gateway S2S- ICICICallBackS2S Response - " + strDecryptedVal, this);

                if (strDecryptedVal.StartsWith("ERROR"))
                {
                    string[] strGetMerchantParamForCompare;
                    string[] strclientbodydata;
                    strSplitDecryptedResponse = strPGResponse.Split('|');
                    for (int i = 0; i < strSplitDecryptedResponse.Length; i++)
                    {
                        strGetMerchantParamForCompare = strSplitDecryptedResponse[i].ToString().Split('=');
                        if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
                        {
                            strPG_TxnStatus = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
                        {
                            strPG_ClintTxnRefNo = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
                        {
                            strPG_TPSLTxnBankCode = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
                        {
                            strPG_TPSLTxnID = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
                        {
                            strPG_TxnAmount = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
                        {
                            strClientBody = Convert.ToString(strGetMerchantParamForCompare[1]);
                            strClientBody = strClientBody.Replace("{", "");
                            var strclientdata = strClientBody.Split('}');
                            for (int j = 0; j < strclientdata.Length; j++)
                            {
                                strclientbodydata = strclientdata[j].ToString().Split(':');
                                if (Convert.ToString(strclientbodydata[0]).ToUpper().Trim() == "CUSTID")
                                {
                                    strLoginName = Convert.ToString(strclientbodydata[1]);
                                }
                                else if (Convert.ToString(strclientbodydata[0]).ToUpper().Trim() == "CUSTNAME")
                                {
                                    strAccountNumber = Convert.ToString(strclientbodydata[1]);
                                }
                            }
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
                        {
                            strPG_TxnDateTime = Convert.ToString(strGetMerchantParamForCompare[1]);
                            strArrPG_TxnDateTime = strPG_TxnDateTime.Split(' ');
                            strPG_TxnDate = Convert.ToString(strArrPG_TxnDateTime[0]);
                            strPG_TxnTime = Convert.ToString(strArrPG_TxnDateTime[1]);
                        }
                    }

                    var modelviewpay = new ViewPayBill()
                    {
                        TransactionId = strPG_TPSLTxnID,
                        ResponseStatus = Constants.PaymentResponse.Failure,
                        Responsecode = strPG_TxnStatus,
                        Remark = Constants.PaymentResponse.Failure,
                        PaymentRef = string.Empty,
                        OrderId = strPG_ClintTxnRefNo,
                        AmountPayable = strPG_TxnAmount,
                        AccountNumber = strAccountNumber,
                        LoginName = strLoginName,
                        msg = strDecryptedVal,
                        PaymentMode = string.Empty
                    };
                    this.PaymentService.StorePaymentResponse(modelviewpay);

                }
                else
                {
                    string[] strGetMerchantParamForCompare;
                    string[] strclientbodydata;
                    strSplitDecryptedResponse = strDecryptedVal.Split('|');
                    for (int i = 0; i < strSplitDecryptedResponse.Length; i++)
                    {
                        strGetMerchantParamForCompare = strSplitDecryptedResponse[i].ToString().Split('=');
                        if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
                        {
                            strPG_TxnStatus = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
                        {
                            strPG_ClintTxnRefNo = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
                        {
                            strPG_TPSLTxnBankCode = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
                        {
                            strPG_TPSLTxnID = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
                        {
                            strPG_TxnAmount = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_ERR_MSG")
                        {
                            strErrorMessage = Convert.ToString(strGetMerchantParamForCompare[1]);
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
                        {
                            strClientBody = Convert.ToString(strGetMerchantParamForCompare[1]);
                            strClientBody = strClientBody.Replace("{", "");
                            var strclientdata = strClientBody.Split('}');
                            for (int j = 0; j < strclientdata.Length; j++)
                            {
                                strclientbodydata = strclientdata[j].ToString().Split(':');
                                if (Convert.ToString(strclientbodydata[0]).ToUpper().Trim() == "CUSTID")
                                {
                                    strLoginName = Convert.ToString(strclientbodydata[1]);
                                }
                                else if (Convert.ToString(strclientbodydata[0]).ToUpper().Trim() == "CUSTNAME")
                                {
                                    strAccountNumber = Convert.ToString(strclientbodydata[1]);
                                }
                            }
                        }
                        else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
                        {
                            strPG_TxnDateTime = Convert.ToString(strGetMerchantParamForCompare[1]);
                            strArrPG_TxnDateTime = strPG_TxnDateTime.Split(' ');
                            strPG_TxnDate = Convert.ToString(strArrPG_TxnDateTime[0]);
                            strPG_TxnTime = Convert.ToString(strArrPG_TxnDateTime[1]);
                        }
                    }

                    if (strPG_TxnStatus == "0300")
                    {
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = strPG_TPSLTxnID,
                            ResponseStatus = Constants.PaymentResponse.Success,
                            Responsecode = strPG_TxnStatus,
                            Remark = Constants.PaymentResponse.Success,
                            PaymentRef = string.Empty,
                            OrderId = strPG_ClintTxnRefNo,
                            AmountPayable = strPG_TxnAmount,
                            AccountNumber = strAccountNumber,
                            LoginName = strLoginName,
                            msg = strDecryptedVal,
                            PaymentMode = string.Empty
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);

                    }
                    else
                    {
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = strPG_TPSLTxnID,
                            ResponseStatus = Constants.PaymentResponse.Failure,
                            Responsecode = strPG_TxnStatus,
                            Remark = strErrorMessage,
                            PaymentRef = string.Empty,
                            OrderId = strPG_ClintTxnRefNo,
                            AmountPayable = strPG_TxnAmount,
                            AccountNumber = strAccountNumber,
                            LoginName = strLoginName,
                            msg = strDecryptedVal,
                            PaymentMode = string.Empty
                        };
                        this.PaymentService.StorePaymentResponse(modelviewpay);
                    }
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ICICICallBackS2S:" + ex.Message, this);
            }
        }

        #endregion


        #region BBPS and User Stats Web Service
        [HttpPost]
        public ActionResult BBPS_CallbackS2S(BBPSModel request)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("BBPS_CallbackS2S Method Called", this);

                string authHeader = HttpContext.Request.Headers["Authorization"];
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                if (string.IsNullOrEmpty(authHeader))
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');
                string username = usernamePassword.Substring(0, seperatorIndex);
                string password = usernamePassword.Substring(seperatorIndex + 1);

                Sitecore.Diagnostics.Log.Info("Payment Gateway- BBPS_CallbackS2S Callback Message CA and Transaction id - " + request.AccountNumber + " , " + request.TransactionId, this);
                string apiUserName = ConfigurationManager.AppSettings["apiUserName"];
                string apiPassword = ConfigurationManager.AppSettings["apiPassword"];

                if (username == apiUserName && password == apiPassword)
                {
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        var ctx = dbcontext.PaymentHistories.Where(x => x.TransactionId == request.TransactionId && x.AccountNumber == request.AccountNumber).FirstOrDefault();
                        if (ctx == null)
                        {
                            if (this.PaymentService.StorePaymentRequestBBPS(request))
                            {
                                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Successfully Added." }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                            }
                            else
                            {
                                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = false, Message = "Server Error" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                            }
                        }
                        else
                        {
                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Created, IsSuccess = false, Message = "Entry already exists." }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                    }

                }
                else
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BBPS_CallbackS2S:" + ex.Message, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpGet]
        public ActionResult stats(string date)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Stats Method Called", this);
                string authHeader = HttpContext.Request.Headers["Authorization"];
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                if (string.IsNullOrEmpty(authHeader))
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');
                string username = usernamePassword.Substring(0, seperatorIndex);
                string password = usernamePassword.Substring(seperatorIndex + 1);


                string apiUserName = ConfigurationManager.AppSettings["apiUserName"];
                string apiPassword = ConfigurationManager.AppSettings["apiPassword"];

                if (username == apiUserName && password == apiPassword)
                {
                    DateTime dt;
                    if (!string.IsNullOrEmpty(date))
                    {
                        if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out dt))
                        {
                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = false, Message = "Please enter proper date in yyyy-MM-dd format", Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                    }
                    var data = this.AccountRepository.GetUserStatistics(date);

                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = data.message, Result = data.userdata }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid", Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at stats:" + ex.Message, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = ex.Message, Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpGet]
        public ActionResult GetUserCAList()
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("GetUserCAList Method Called", this);
                string authHeader = HttpContext.Request.Headers["Authorization"];
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                if (string.IsNullOrEmpty(authHeader))
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');
                string username = usernamePassword.Substring(0, seperatorIndex);
                string password = usernamePassword.Substring(seperatorIndex + 1);

                var user = this.AccountRepository.LoginAPI(username, password);
                if (user != null)
                {
                    var properties = UserProfileProvider.GetCustomProperties(user.Profile);
                    var customername = properties.ContainsKey(Constants.UserProfile.Fields.FirstName) ? properties[Constants.UserProfile.Fields.FirstName] : "";

                    var data = this.AccountRepository.GetUserCAList(username);
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = data.message, CustomerName = customername, Result = data.userdata }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid", CustomerName = "", Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at stats:" + ex.Message, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = ex.Message, Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        #endregion

        public ActionResult PaymentSuccess()
        {
            var model = new ViewPayBill();
            if (TempData["PaymentResponse"] != null)
            {
                model = (ViewPayBill)TempData["PaymentResponse"];
            }
            else
            {
                if (SessionHelper.UserSession.UserSessionContext != null)
                {
                    string orderid = SessionHelper.UserSession.UserSessionContext.OrderId;
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        var ctx = dbcontext.PaymentHistories.Where(x => x.OrderId == orderid && x.TransactionId != null).FirstOrDefault();
                        if (ctx == null)
                        {
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = ctx.TransactionId,
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = ctx.Responsecode,
                                Remark = Constants.PaymentResponse.Success,
                                PaymentRef = ctx.PaymentRef,
                                OrderId = orderid,
                                AmountPayable = ctx.Amount,
                                AccountNumber = ctx.AccountNumber,
                                LoginName = ctx.AccountNumber,
                                msg = ctx.ResponseMsg,
                                PaymentMode = ctx.PaymentMode,
                                TransactionDate = ctx.Created_Date.ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                                PaymentGateway = 6
                            };

                            model = modelviewpay;
                        }
                    }
                }
            }

            Sitecore.Diagnostics.Log.Info("Payment Success Response", this);
            return this.View(model);
        }
        public ActionResult PaymentFailure()
        {
            var model = new ViewPayBill();
            if (TempData["PaymentResponse"] != null)
            {
                model = (ViewPayBill)TempData["PaymentResponse"];
            }
            Sitecore.Diagnostics.Log.Info("Payment Failure Response", this);
            return this.View(model);
        }
        public ActionResult PaymentPending()
        {
            var model = new ViewPayBill();
            if (TempData["PaymentResponse"] != null)
            {
                model = (ViewPayBill)TempData["PaymentResponse"];
            }
            Sitecore.Diagnostics.Log.Info("Payment Pending Response", this);
            return this.View(model);
        }


        #region Pay Security Deposit
        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult PaymentSecurityDeposit()
        {
            var model = new ViewPayBill();
            try
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");

                // Note : retrieve Information from service to bind model Values.
                //SecurityPayment
                var sdpaymentlist = db.GetItem(Templates.PaymentConfiguration.Datasource.SecurityDepositPaymentMode);
                var securitypaymentlist = sdpaymentlist.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();

                //var properties = this.UserProfileProvider.GetCustomProperties(Context.User.Profile);
                //var masterAccount = properties.ContainsKey(Constants.UserProfile.Fields.PrimaryAccountNo) ? properties[Constants.UserProfile.Fields.PrimaryAccountNo] : "";

                var profile = this.UserProfileService.GetProfile(Context.User);

                var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(profile.AccountNumber);
                var securityAmount = SapPiService.Services.RequestHandler.FetchSecurityDepositAmount(profile.AccountNumber);

                model = new ViewPayBill()
                {
                    AccountNumber = accountDetails.AccountNumber,// this.UserProfileService.GetAccountNumberfromItem(masterAccount.ToString()),
                    LoginName = this.UserProfileService.GetLoginName(),
                    BookNumber = accountDetails.BookNumber,// "505",
                    CycleNumber = accountDetails.CycleNumber,// "05",
                    Zone = accountDetails.ZoneNumber,// "South Central",
                    Address = accountDetails.Address,// "300 288 Shere Punjab SOC Mahakali Caves Rd Andheri E Near Tolani College Mumbai 400067",
                    SecurityDeposit = securityAmount.ToString(),// "560.00",
                    SecurityPaymentList = securitypaymentlist,
                    Mobile = profile.MobileNumber,
                    Email = profile.Email,
                    SecurityDepositAmountType = "Actual"
                };

                Session["securityDespositAmt"] = securityAmount.ToString();
            }
            catch (Exception ex)
            {
                ViewBag.SecurityDesposit = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/API Issue", "There is some issue in fetching your data. Please try after some time.");
                Sitecore.Diagnostics.Log.Error("Error at PaymentSecurityDeposit:" + ex.Message, this);
            }
            return this.View(model);
        }

        [HttpPost]
        [RedirectUnauthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentSecurityDeposit(ViewPayBill model)
        {
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);
                if (model.AccountNumber != profile.AccountNumber)
                {
                    this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidRequest", "Invalid request."));
                    return this.View(model);
                }

                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                var sdpaymentlist = db.GetItem(Templates.PaymentConfiguration.Datasource.SecurityDepositPaymentMode);
                var securitypaymentlist = sdpaymentlist.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();
                model.SecurityPaymentList = securitypaymentlist;

                if (model.SecurityDepositAmountType == "Actual")
                {
                    if (Session["securityDespositAmt"] != null && !model.SecurityDeposit.Equals(Session["securityDespositAmt"].ToString()))
                    {
                        this.ModelState.AddModelError(model.SecurityDeposit, DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Security deposit amount mismatch", "Security Deposit Amount Mismatch"));
                        return this.View(model);
                    }
                    if (Convert.ToDecimal(model.SecurityDeposit) <= 0)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/SecurityDepositeValidation", "You have no security deposit payable amount."));
                        return this.View(model);
                    }
                }
                else if (model.SecurityDepositAmountType == "Partial")
                {
                    if (Convert.ToDecimal(model.SecurityDepositPartial) <= 0)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/SecurityDepositeValidation", "Please enter true value for security deposit payable amount."));
                        return this.View(model);
                    }
                }
                if (string.IsNullOrEmpty(model.SecurityPayment))
                {
                    this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Security payment selection required", "Please select security Payment option."));
                    return this.View(model);
                }


                model.UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/RegisteredUser", "Registered");
                model.PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit");
                model.AmountPayable = model.SecurityDeposit;

                if (model.SecurityPayment == DictionaryPhraseRepository.Current.Get("/Payment Settings/ICICI", "ICICI"))
                {
                    model.PaymentGateway = 5;
                    this.PaymentService.StorePaymentRequest(model);
                    string submitHTML = this.PaymentService.ICICITransactionRequestAPIRequestPost(model);
                    return Content(submitHTML);
                }
                else
                {
                    model.PaymentGateway = 2;
                    this.PaymentService.StorePaymentRequest(model);
                    string submitHTML = this.PaymentService.BillDeskSDTransactionRequestAPIRequestPost(model);
                    return Content(submitHTML);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at PaymentSecurityDeposit Post:" + ex.Message, this);
            }
            return this.Redirect(this.Request.RawUrl);
        }
        #endregion

        #region Pay VDS Amount
        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult PaymentVDS()
        {
            ViewBag.VDSMessage = null;
            var model = new PayVDS();
            //var properties = this.UserProfileProvider.GetCustomProperties(Context.User.Profile);
            //var masterAccount = properties.ContainsKey(Constants.UserProfile.Fields.PrimaryAccountNo) ? properties[Constants.UserProfile.Fields.PrimaryAccountNo] : "";
            var profile = this.UserProfileService.GetProfile(Context.User);
            try
            {
                string paymentMode = string.Empty;
                if (string.IsNullOrEmpty(model.PaymentMode))
                {
                    paymentMode = TempData["VDSPaymentMode"] != null ? TempData["VDSPaymentMode"].ToString() : "CC";
                }
                else
                {
                    paymentMode = model.PaymentMode;
                }

                var VDSAmountDetails = SapPiService.Services.RequestHandler.FetchVdsAmount(profile.AccountNumber);
                //model = new PayVDS()
                //{
                //    AccountNumber = profile.AccountNumber,
                //    MobileNumber = profile.MobileNumber,
                //    EmailAddress = profile.Email,
                //    AverageVDSAmount = 3000,
                //    PANNo = "",
                //    PaymentAmount = 3000,
                //    PaymentMode = paymentMode,
                //    LoginName = this.UserProfileService.GetLoginName()
                //};
                if (string.IsNullOrEmpty(profile.AccountNumber))
                {
                    ViewBag.VDSMessage = "Dear Consumer, you need to clear your current outstanding for VDS payments.";
                    return this.View(model);
                }
                if (VDSAmountDetails.CurrentOutstanding > 0)
                {
                    ViewBag.VDSMessage = "Dear Consumer, you need to clear your current outstanding for VDS payments.";
                }
                else
                {
                    decimal minVDSAmount = 0;
                    decimal maxVDSAmount = 0;
                    var gap = 100000 - VDSAmountDetails.ExistingVdsBalance;
                    if (VDSAmountDetails.AverageBillingAmount < 3000)
                    {
                        minVDSAmount = 3000;
                    }
                    else if (VDSAmountDetails.AverageBillingAmount > 3000)
                    {
                        minVDSAmount = Math.Round(Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                    }
                    if (gap >= 0 && gap < Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
                    {
                        minVDSAmount = gap;
                    }
                    maxVDSAmount = gap;

                    //decimal VDSAmount = 0;
                    //if (VDSAmountDetails.AverageBillingAmount < 3000)
                    //    VDSAmount = 3000;
                    //else if (VDSAmountDetails.AverageBillingAmount > 10000)
                    //    VDSAmount = 10000;
                    //else if (VDSAmountDetails.AverageBillingAmount > 3000 && VDSAmountDetails.AverageBillingAmount < 10000)
                    //{
                    //    VDSAmount = Math.Round(Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                    //}

                    model = new PayVDS()
                    {
                        AccountNumber = profile.AccountNumber,
                        MobileNumber = profile.MobileNumber,
                        EmailAddress = profile.Email,
                        AverageVDSAmount = minVDSAmount,
                        MaxVDSAmount = maxVDSAmount,
                        PANNo = "",
                        PaymentAmount = minVDSAmount,
                        PaymentMode = paymentMode,
                        LoginName = this.UserProfileService.GetLoginName()
                    };
                    Session["VDSAmount"] = minVDSAmount.ToString();
                    Session["maxVDSAmount"] = maxVDSAmount.ToString();
                }
            }
            catch (Exception ex)
            {
                //model = new PayVDS()
                //{
                //    AccountNumber = profile.AccountNumber,
                //    MobileNumber = profile.MobileNumber,
                //    EmailAddress = profile.Email,
                //    AverageVDSAmount = 3000,
                //    PANNo = "",
                //    PaymentAmount = 3000,
                //    PaymentMode = "CC",
                //    LoginName = this.UserProfileService.GetLoginName()
                //};
                ViewBag.VDSMessage = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/API Issue", "There is some issue in fetching your data. Please try after some time.");
                Sitecore.Diagnostics.Log.Error("Error at PaymentVDS :" + ex.Message, this);
            }
            return this.View(model);
        }

        [HttpPost]
        [ValidateModel]
        [RedirectUnauthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentVDS(PayVDS model)
        {
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);
                if (model.AccountNumber != profile.AccountNumber)
                {
                    this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidRequest", "Invalid request."));
                    return this.View(model);
                }

                model.AverageVDSAmount = Convert.ToDecimal(Session["VDSAmount"]);
                model.MaxVDSAmount = Convert.ToDecimal(Session["maxVDSAmount"]);

                if (!this.ModelState.IsValid)
                {
                    return this.View(model);
                }
                else
                {
                    if (string.IsNullOrEmpty(model.PaymentAmount.ToString()) && model.PaymentAmount.ToString().Any(char.IsLetter))
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidAmount", "Invalid amount payable value."));
                        return this.View(model);
                    }
                    if (Convert.ToDecimal(model.PaymentAmount) < 0)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value!"));
                        return this.View(model);
                    }
                    if (Convert.ToDecimal(model.PaymentAmount) % 500 != 0)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value, it should be multiple of 500!"));
                        return this.View(model);
                    }
                    if (Convert.ToDecimal(model.PaymentAmount) < Convert.ToDecimal(Session["VDSAmount"]) || Convert.ToDecimal(model.PaymentAmount) > Convert.ToDecimal(Session["maxVDSAmount"]))
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value, it should be in between min and max values."));
                        return this.View(model);
                    }

                    if (string.IsNullOrEmpty(model.PANNo))
                    {
                        this.ModelState.AddModelError("PANNo", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/PAN No", "Please enter a valid PAN No."));
                        return this.View(model);
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$");
                        //If you want both upper case and lower case alphabets.  
                        Match match = regex.Match(model.PANNo);
                        if (!match.Success)
                        {
                            this.ModelState.AddModelError("PANNo", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/PAN No", "Please enter a valid PAN No."));
                            return this.View(model);
                        }
                    }


                    //if (Convert.ToDecimal(model.PaymentAmount) < 0 || Convert.ToDecimal(Session["VDSAmount"]) != Convert.ToDecimal(model.PaymentAmount))
                    //{
                    //    this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "You have modified VDS Amount Value. Please refresh the page and try again."));
                    //    return this.View(model);
                    //}
                    if (model != null && !string.IsNullOrEmpty(model.PaymentMode))
                    {
                        var paymodel = new ViewPayBill()
                        {
                            AccountNumber = model.AccountNumber,
                            AmountPayable = model.AverageVDSAmount.ToString("f2"),
                            LoginName = model.LoginName,
                            Email = model.EmailAddress,
                            Mobile = model.MobileNumber,
                            PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/VDS", "VDS payment"),
                            Remark = model.PANNo,
                            PaymentGateway = 2,
                            UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/RegisteredUser", "Registered"),
                            CurrencyType = "INR"
                        };

                        string paymentmode = model.PaymentMode; //CC and Net Banking parameter come from pay and bill page

                        this.PaymentService.StorePaymentRequest(paymodel);
                        string RequestHTML = this.PaymentService.BillDeskVDSTransactionRequestAPIRequestPost(paymodel);
                        return Content(RequestHTML);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at PaymentVDS Post:" + ex.Message, this);
                return this.View(model);
            }
            return this.Redirect(this.Request.RawUrl);
        }
        #endregion

        #region Download / Pay Bill
        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult DownloadPayBill()
        {
            DownloadViewBill model = new DownloadViewBill();
            try
            {
                EditProfile profile = UserProfileService.GetProfile(Context.User);
                model = new DownloadViewBill
                {
                    InvoiceLines = new List<InvoiceRecord>(),
                    AccountNumber = profile.AccountNumber,
                    Email = profile.Email,
                    MobileNumber = profile.MobileNumber
                };
                InvoiceHistory result = SapPiService.Services.RequestHandler.FetchInvoiceHistory(profile.AccountNumber);
                foreach (InvoiceLine i in result.InvoiceLines)
                {
                    model.InvoiceLines.Add(new InvoiceRecord
                    {
                        //AccountNumber = i.AccountNumber,
                        BillMonth = i.BillMonth,
                        //InvoiceNumber = i.InvoiceNumber,
                        //InvoiceUrl = "https://iss.adanielectricity.com/VAS/ProcessDownloadPDF.jsp?TXTCANO=" + i.AccountNumber + "&INVOICENO=" + i.InvoiceNumber
                    });
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateModel]
        [RedirectUnauthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadPayBill(DownloadViewBill model, string viewBill = null)
        {
            try
            {
                Session["UpdateMessage"] = null;
                if (!string.IsNullOrEmpty(viewBill))
                {
                    try
                    {
                        Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                        var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                        string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.DownloadBillSettings.EncryptionKey].Value; // "B3XbAcGCezTeqfVxWIl4tvNdI";

                        if (string.IsNullOrEmpty(model.selectedMonth))
                        {
                            Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/DownloadPayBill/DownloadBillError", "Please select a month to download bill!"));
                            return Redirect(Request.RawUrl);
                        }
                        InvoiceHistory billRecords = SapPiService.Services.RequestHandler.FetchInvoiceHistory(model.AccountNumber);
                        InvoiceLine billToDownload = billRecords.InvoiceLines.Where(i => i.BillMonth == model.selectedMonth).FirstOrDefault();

                        clsTripleDES objclsTripleDES = new clsTripleDES();
                        String encryptedCANumber = HttpUtility.UrlEncode(objclsTripleDES.Encrypt(billToDownload.AccountNumber, EncryptionKey));
                        String encryptedInvoiceNumber = HttpUtility.UrlEncode(objclsTripleDES.Encrypt(billToDownload.InvoiceNumber, EncryptionKey));

                        billToDownload.InvoiceUrl = "https://iss.adanielectricity.com/VAS/ProcessDownloadPDF.jsp?ENCR=1&ENCRCANO=" + encryptedCANumber + "&ENCRINVNO=" + encryptedInvoiceNumber;


                        System.Net.WebClient client = new System.Net.WebClient();
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        byte[] byteArray = client.DownloadData(billToDownload.InvoiceUrl);
                        Session["filestring"] = byteArray;
                        return RedirectToAction("ActualPDFRendering");
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
                        Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/DownloadPayBill/DownloadBillError", "Unable to download your bill, please try again!"));
                    }
                }

                bool result = false;
                InvoiceHistory fetchInvoice = SapPiService.Services.RequestHandler.FetchInvoiceHistory(model.AccountNumber);

                if (fetchInvoice.InvoiceLines.Where(i => i.BillMonth == model.selectedMonth).Any())
                {
                    Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                    var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                    string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.DownloadBillSettings.EncryptionKey].Value; // "B3XbAcGCezTeqfVxWIl4tvNdI";

                    InvoiceLine selectedRecord = fetchInvoice.InvoiceLines.Where(i => i.BillMonth == model.selectedMonth).FirstOrDefault();

                    clsTripleDES objclsTripleDES = new clsTripleDES();
                    String encryptedCANumber = HttpUtility.UrlEncode(objclsTripleDES.Encrypt(selectedRecord.AccountNumber, EncryptionKey));
                    String encryptedInvoiceNumber = HttpUtility.UrlEncode(objclsTripleDES.Encrypt(selectedRecord.InvoiceNumber, EncryptionKey));

                    selectedRecord.InvoiceUrl = "https://iss.adanielectricity.com/VAS/ProcessDownloadPDF.jsp?ENCR=1&ENCRCANO=" + encryptedCANumber + "&ENCRINVNO=" + encryptedInvoiceNumber;

                    //var url = Sitecore.Configuration.Settings.GetSetting("DownloadBillServiceURL");
                    //url = url.Replace("{0}", model.AccountNumber);
                    //url = url.Replace("{1}", selectedRecord.InvoiceNumber);
                    HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(selectedRecord.InvoiceUrl);
                    httpRequest.Method = WebRequestMethods.Http.Get;

                    // Get back the HTTP response for web server
                    HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    Stream httpResponseStream = httpResponse.GetResponseStream();

                    if (httpResponseStream == null)
                    {
                        //return from here
                    }
                    const int readSize = 256;
                    byte[] buffer = new byte[readSize];
                    MemoryStream ms = new MemoryStream();

                    int count = httpResponseStream.Read(buffer, 0, readSize);
                    while (count > 0)
                    {
                        ms.Write(buffer, 0, count);
                        count = httpResponseStream.Read(buffer, 0, readSize);
                    }

                    ms.Position = 0;
                    httpResponseStream.Close();
                    result = NotificationService.SendBill(
                        model.Email,
                        "Your bill details : " + DateTime.Now.ToShortDateString() + " for CA :" + model.AccountNumber,
                        "Dear Customer, \n Your Electricity Bill for " + selectedRecord.BillMonth + " is attached with this mail.",
                       ms,
                        model.AccountNumber + "_" + selectedRecord.BillMonth + ".pdf",
                        DictionaryPhraseRepository.Current.Get("/Accounts/DownloadPayBill/FromEmail", "Helpdesk.Mumbaielectricity@adani.com"));
                }
                if (result == true)
                {
                    Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/DownloadPayBill/SendBillSuccess", "Your request is saved successfully, Your will receive bill in email shortly!"));
                }
                else
                {
                    Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/DownloadPayBill/SendBillError", "Your request is not accepted, please try again!"));
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
                Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/DownloadPayBill/SendBillError", "Your request is not accepted, please try again!"));
            }
            return Redirect(Request.RawUrl);
        }


        [HttpGet]
        public ActionResult ActualPDFRendering()
        {
            byte[] fileString = (byte[])Session["filestring"];
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Response.AppendHeader("Content-Disposition", "inline; filename=" + filename);
            return File(fileString, "application/pdf", filename + ".pdf");
        }

        #endregion

        [HttpGet]
        public ActionResult BillAmendmentDetails()
        {
            BillAmendmentDetailsModel model = new BillAmendmentDetailsModel();
            model.Language = "E";
            return View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult BillAmendmentDetails(BillAmendmentDetailsModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }

                if (string.IsNullOrEmpty(model.AccountNo))
                {
                    this.ModelState.AddModelError(nameof(model.AccountNo), DictionaryPhraseRepository.Current.Get("/BillAmendmentDetails/Account number required", "Please enter valid Account Number."));
                    return this.View(model);
                }

                if (!this.IsCaptchaValid("Captcha Validation Required."))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/BillAmendmentDetails/Captcha required", "Captcha Validation Required."));
                    return this.View(model);
                }

                Log.Info("BillAmendmentDetails for Account number:" + model.AccountNo + ", language:" + model.Language, this);
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.DownloadBillSettings.EncryptionKey].Value; // "B3XbAcGCezTeqfVxWIl4tvNdI";

                Log.Info("BillAmendmentDetails for Account number:" + model.AccountNo + ", language:" + model.Language + ", successful in getting EncryptionKey", this);
                clsTripleDES objclsTripleDES = new clsTripleDES();
                String encryptedCANumber = HttpUtility.UrlEncode(objclsTripleDES.Encrypt(model.AccountNo, EncryptionKey));

                string downloadUrl = string.Format(DictionaryPhraseRepository.Current.Get("/BillAmendmentDetails/DownloadUrl", "https://iss.adanielectricity.com/VAS/ProcessDownloadPDF.jsp?ENCR={0}&ENCRCANO={1}&Amend={2}&Lang={3}"), 1, encryptedCANumber, "Y", model.Language);

                Log.Info("BillAmendmentDetails for Account number:" + model.AccountNo + ", language:" + model.Language + ", downloadUrl:" + downloadUrl, this);
                System.Net.WebClient client = new System.Net.WebClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                byte[] byteArray = client.DownloadData(downloadUrl);
                Session["filestring"] = byteArray;
                Log.Info("BillAmendmentDetails for Account number:" + model.AccountNo + ", language:" + model.Language + ", downloadUrl:" + downloadUrl + ", byteArray:" + byteArray, this);
                return RedirectToAction("ActualPDFRendering");
            }
            catch (Exception ex)
            {
                Log.Error("BillAmendmentDetails error in download: " + ex.Message, this);
                this.ModelState.AddModelError(nameof(model.AccountNo), DictionaryPhraseRepository.Current.Get("/BillAmendmentDetails/Error", "Some issue in download, please try again later!."));
                return this.View(model);
            }
        }

        #region Payment History

        [RedirectUnauthenticated]
        public ActionResult PaymentHistory()
        {
            return this.View();
        }

        public ActionResult LoadData_PaymentHistory()
        {
            SapPiService.Domain.PaymentHistory model = new SapPiService.Domain.PaymentHistory();
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);
                model = SapPiService.Services.RequestHandler.GetPaymentHistory(profile.AccountNumber);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return Json(new { data = model.PaymentHistoryList }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Consumption History

        [RedirectUnauthenticated]
        public ActionResult ConsumptionHistory()
        {
            SapPiService.Domain.ConsumptionHistory model = new SapPiService.Domain.ConsumptionHistory();
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);

                model = SapPiService.Services.RequestHandler.GetConsumptionHistory(profile.AccountNumber);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return this.View(model);
        }



        #endregion

        #region Quick Pay

        [HttpGet]
        [RedirectAuthenticated]
        public ActionResult QuickPay()
        {
            //Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            //Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            //string publicKey = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_ServerPublicKeyPath].Value; //@"C:\Users\Nidhi.Paneri\Desktop\DBS\Key Pair\from DBS\DBSSG_EN_PUBLIC.asc";
            //string privateKey = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_ClientPrivateKeyPath].Value; //@"C:\Users\Nidhi.Paneri\Desktop\DBS\Key Pair\DBS_PrivateSecret.asc";
            //string privayeKeyPwd = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_ClientSecretKey].Value; //"5&rd&Xdj#nJR";

            //Decrypt decrypt = new Decrypt();
            //decrypt.IsVerify = true;
            //decrypt.PublicKeyFilePath = Server.MapPath(publicKey) ;
            //decrypt.PrivateKeyFilePath = Server.MapPath("~" + privateKey); 
            //decrypt.PrivateKeyPassword = privayeKeyPwd;

            //string encryptedString = "-----BEGIN PGP MESSAGE-----\r\nVersion: BCPG C# v1.8.5.0\r\n\r\nhQEMA0totPEQqIvoAQf9H0P3W8IYLJtN+4d92B8A2Rs3OuvEStGxy+1H8Lh/QlTk\r\npL8qHZIvxrdR6zMtqEeW65KpQXoTcY5QndN6aqofWnOdk53CLKrGkdnps+PkiGdp\r\nfMN2nZJaq2mZ4zeYAfZdmy4mibOK3drXlH6gzSnEByzZ48MA4DZEFo0vsZlZQLQ6\r\n/HCsTvlthAOw0iYSGvg5XIIBQ9fL0bt3YNDL5paHQoNMkZQ6JddKhFpk3bXQS3p9\r\nEI0EwmjSHOsavfonv52DCBE2SDymr/aGyB+amyNBOgo7cbD03GaewJnjhxcNFezp\r\nMMTDg6iRNfVyBEep8edviJ/55A3k+tWt/nH8lK/ATtLCHwHF9QOYJUN0iA9EBkLZ\r\nnQN839wcg09DWiEsOF7wTaU8ibJFB8X/JOgd482+3Ka38BU40dbhcfJML7kL2F/Q\r\nZR8p2JyclU2x5lNxSHle3S5vG1tVkXhDJYtQEQ/YWyykQX8DdymR4O5f85RaUuJw\r\nrZhpPYRme2qmFLlUf8EdWngUHvpX+IpDXrythvx1jeIuKl7M5KHxraWPd9GJv/0H\r\ny8S3vwFkUHeAlRKTGo8Ua7GghdaRl3AitzgP7TyxFwUGYXfx6UvP/3SJlWxYLguL\r\nngQQbLHFp9Z0Txj6w8a3ntetjtSBFLakmfJJL3zNR7fkJ+sJpiCzRlacMTSFrBiJ\r\n+mUIBuyZt8hj5Jc9xDTxRw5nyS2BL2C5TAzvLS5DKbE2yrAA/EgttVXqT6MDjRoK\r\nLLxkryGoviC9csKlw1WZMWQdZK6xCRqBK1b2HsKuywlfRemE10hO3pU6gz44LHpt\r\nzbPT5vVy/wf1G9EGx2tXQArrnrli0ETFeAvq+ePLVFuGYVlHbd8aPPughhF2JiXF\r\nXzN52KVaYaxSypo/bKOfPwxOqheR3A6JNsJCtJ+2hf+OYFaUfnXosbkS3EIX6N3O\r\nOLG64y6otFok/TJlfqzhsy0bsoJOYqI11A5gVSjValgLVFNhlj8BTEbX0UXhrvG3\r\nT0dd5u71efOvLmCj4kFjDW4IH8aUMQRAl02Fgqo8uhA7eGlRarFrjdx73N1cOo76\r\nbg9UZsnn4GQ7wjpjrgusK2fRHx4yEguTNivew9+5L89J3xK8kChw5ezrzZz6KYdd\r\nhxmDMgQRDVHyhr4DrzLeJOPsTbrZ2j5N0GGEb7HDxDJQgNKGytYjunNsBwFiQj5a\r\nRFkMEvX2SBq8DBgw6pOKRy+hWqiZvrATLQfOcMHx7Cqlx3N37GcTb2+gs9QoNkT2\r\n7LdqKX+1TINQw7+HggLux3ChYEQs8oCn6B4zGZG1fshh3mZkDREkVgvhpkoyKeWt\r\n1Q==\r\n=jIlj\r\n-----END PGP MESSAGE-----\r\n";

            //BCPGPDecryptor objPgpDecrypt = new BCPGPDecryptor(decrypt);
            //string decryptedMsg = objPgpDecrypt.DecryptMessage(encryptedString);
            //Sitecore.Diagnostics.Log.Info("DBS_Callback Method Called decryptedstring:" + decryptedMsg, this);

            //DBSResponse responseMessage = new JavaScriptSerializer().Deserialize<DBSResponse>(decryptedMsg);

            //Sitecore.Diagnostics.Log.Info("DBS_Callback Method Called - request found with order id: " + responseMessage.txnResponse.customerReference, this);

            return this.View();
        }

        [HttpPost]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult QuickPay(ViewPayBill model)
        {
            if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
            {
                ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                return this.View(model);
            }

            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

            if (!reCaptchaResponse.success)
            {
                ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                return this.View(model);
            }
            //if (!this.IsCaptchaValid("Captcha Validation Required."))
            //{
            //    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
            //    return this.View(model);
            //}

            var parentItem = Context.Site.GetStartItem().Parent;
            InternalLinkField link = parentItem.Fields[Templates.AccountsSettings.Fields.QuickPayBillPage];

            SessionHelper.UserSession.UserSessionContext = new DashboardModel
            {
                AccountNumber = model.AccountNumber
            };

            return this.Redirect(link.TargetItem.Url());
        }

        [HttpGet]
        [RedirectAuthenticated]
        public ActionResult PayBill()
        {
            ViewBag.VDSMessage = null;
            ViewPayBill model = new ViewPayBill();
            if (Request.QueryString["ca_number"] != null)
            {
                model.AccountNumber = Request.QueryString["ca_number"];
                ViewBag.NoInfo = "Please provide Captcha Validation and Get your bill info.";
            }
            else if (SessionHelper.UserSession.UserSessionContext != null)
            {
                try
                {
                    Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");

                    model.AccountNumber = SessionHelper.UserSession.UserSessionContext.AccountNumber;

                    var sdpaymentlist = db.GetItem(Templates.PaymentConfiguration.Datasource.SecurityDepositPaymentMode);
                    var securitypaymentlist = sdpaymentlist.GetChildren().ToList().Select(x => new SelectListItem()
                    {
                        Text = x.Fields["Text"].Value,
                        Value = x.Fields["Value"].Value
                    }).ToList();

                    //VDS Payment
                    var paymentlist = db.GetItem(Templates.PaymentConfiguration.Datasource.VDSPaymentMode);
                    var VDSpaymentlist = paymentlist.GetChildren().ToList().Select(x => new SelectListItem()
                    {
                        Text = x.Fields["Text"].Value,
                        Value = x.Fields["Value"].Value
                    }).ToList();

                    ViewBag.NoInfo = null;

                    Log.Info("Paybill fetching details for CA:" + model.AccountNumber, this);
                    var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(model.AccountNumber);
                    Log.Info("Paybill details received for CA:" + accountDetails.AccountNumber, this);

                    var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(model.AccountNumber);
                    Log.Info("Paybill billing amount for CA:" + billinghDetails.AmountPayable, this);

                    var securityAmount = SapPiService.Services.RequestHandler.FetchSecurityDepositAmount(model.AccountNumber);
                    var emiDetails = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(model.AccountNumber);
                    Log.Info("Paybill fetching EMI details for CA:" + model.AccountNumber + ", EMI: Status-" + emiDetails.Status + ", EMI-" + emiDetails.EMIInstallmentAmount + ", Total Outstannding" + emiDetails.TotalOutstanding, this);


                    decimal minVDSAmount = 0;
                    decimal maxVDSAmount = 0;
                    //decimal VDSAmount = 0;
                    try
                    {
                        var VDSAmountDetails = SapPiService.Services.RequestHandler.FetchVdsAmount(model.AccountNumber);
                        if (VDSAmountDetails.CurrentOutstanding > 0)
                        {
                            ViewBag.VDSMessage = "Dear Consumer, you need to clear your current outstanding for VDS payments.";
                        }
                        else
                        {

                            var gap = 100000 - VDSAmountDetails.ExistingVdsBalance;
                            if (VDSAmountDetails.AverageBillingAmount < 3000)
                            {
                                minVDSAmount = 3000;
                            }
                            else if (VDSAmountDetails.AverageBillingAmount > 3000)
                            {
                                minVDSAmount = Math.Round(Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                            }
                            if (gap >= 0 && gap < Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
                            {
                                minVDSAmount = gap;
                            }
                            maxVDSAmount = gap;
                            //if (VDSAmountDetails.AverageBillingAmount <= 3000)
                            //    VDSAmount = 3000;
                            //else if (VDSAmountDetails.AverageBillingAmount > 10000)
                            //    VDSAmount = 10000;
                            //else if (VDSAmountDetails.AverageBillingAmount > 3000 && VDSAmountDetails.AverageBillingAmount <= 10000)
                            //{
                            //    VDSAmount = Math.Round(Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                            //}
                        }
                    }
                    catch
                    {
                        ViewBag.VDSMessage = "There are some issues in fetching VDS amount.";
                    }

                    if (string.IsNullOrEmpty(accountDetails.CycleNumber) || billinghDetails.BillingStatus == BillingStatus.NoInvoice)
                    {
                        ViewBag.NoInfo = "No Invoice Available.";
                    }
                    else
                    {

                        //var ItemId = this.AccountRepository.GetAccountItemId(model.AccountNumber);
                        string email = string.Empty, mobile = string.Empty, LoginName = string.Empty;

                        decimal amountpayable = billinghDetails.AmountPayable;


                        model = new ViewPayBill()
                        {
                            AccountNumber = accountDetails.AccountNumber,// this.UserProfileService.GetAccountNumberfromItem(masterAccount.ToString()),
                            Name = accountDetails.Name,// this.UserProfileService.GetLoginName(),
                            BookNumber = accountDetails.BookNumber,// "505",
                            CycleNumber = accountDetails.CycleNumber,// "05",
                            Zone = accountDetails.ZoneNumber,// "South Central",
                            Address = accountDetails.Address,// "300 288 Shere Punjab SOC Mahakali Caves Rd Andheri E Near Tolani College Mumbai 400067",
                            BillMonth = billinghDetails.BillMonth,// "May-2018",
                            PaymentDueDate = billinghDetails.DateDue,// "2018-06-16",
                            TariffSlab = billinghDetails.TariffSlab,// "LT-1b",
                            MeterNumber = string.Join(",", billinghDetails.MeterNumbers),// "7587321",
                            UnitsConsumed = billinghDetails.UnitsConsumed,//  "1",
                            TotalCharges = billinghDetails.TotalCharges,// "134.27",
                            CurrentMonthsBills = billinghDetails.CurrentMonthCharge,// "134.69",
                            BroughtForward = billinghDetails.BroughtForward,// "128.26",
                            TotalBillAmount = billinghDetails.TotalBillAmount,//  "246.19",
                            SecurityDeposit = securityAmount.ToString(),// "560.00",
                            SecurityDepositAmountType = "Actual",                                            //VDSAmount = vdsAmount.ToString(),
                            AmountPayable = amountpayable.ToString(),//billinghDetails.AmountPayable.ToString(),// "250"
                            Email = email,
                            Mobile = mobile,
                            LoginName = accountDetails.AccountNumber,
                            AverageVDSAmount = minVDSAmount,
                            PaymentVDSAmount = minVDSAmount,
                            SecurityPaymentList = securitypaymentlist,
                            VDSPaymentList = VDSpaymentlist,
                            MaxVDSAmount = maxVDSAmount,
                            Flag = billinghDetails.Flag,
                            EMIEligible = emiDetails.Status == "S" ? true : false,
                            EMIInstallmentAmount = emiDetails.EMIInstallmentAmount,
                            EMIOutstandingAmount = emiDetails.TotalOutstanding,
                            ProceedWithEMI = false
                        };
                        Session["Amountpayable"] = amountpayable.ToString();//billinghDetails.AmountPayable.ToString();
                        Session["SecurityDeposit"] = securityAmount.ToString();
                        Session["PaymentVDSAmount"] = minVDSAmount.ToString();
                        Session["maxVDSAmount"] = maxVDSAmount.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.NoInfo = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/API Issue", "There is some issue in fetching your data. Please try after some time.");
                    Sitecore.Diagnostics.Log.Error("Error at PayBill :" + ex.Message, this);
                }
            }
            else
            {
                ViewBag.NoInfo = "No Invoice Available.";
            }

            return this.View(model);
        }

        [HttpPost]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult  PayBill(ViewPayBill model, string paybillforAccount, string Pay_PaymentGateway = null, string Pay_VDSpayment = null, string Pay_Other = null)
        {
            try
            {

                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                var sdpaymentlist = db.GetItem(Templates.PaymentConfiguration.Datasource.SecurityDepositPaymentMode);
                var securitypaymentlist = sdpaymentlist.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();

                //VDS Payment
                var paymentlist = db.GetItem(Templates.PaymentConfiguration.Datasource.VDSPaymentMode);
                var VDSpaymentlist = paymentlist.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();

                model.SecurityPaymentList = securitypaymentlist;
                model.VDSPaymentList = VDSpaymentlist;
                model.UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/GuestUser", "Guest");

                #region billingAmount/vds/otherSecurity

                if (!string.IsNullOrEmpty(Pay_PaymentGateway))
                {
                    if (!this.ModelState.IsValid)
                    {
                        return this.View(model);
                    }
                    string accountnumber = SessionHelper.UserSession.UserSessionContext.AccountNumber;
                    if (model.AccountNumber != accountnumber)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidRequest", "Invalid request."));
                        return this.View(model);
                    }

                    if (model.ProceedWithEMI)
                    {
                        Log.Info("Paybill for CA:" + model.AccountNumber + ", Proceed with EMI.", this);
                        var emiDetails = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(model.AccountNumber);
                        //put checks here
                        model.AmountPayable = emiDetails.EMIInstallmentAmount.ToString();
                        model.EMIOutstandingAmount = emiDetails.EMIInstallmentAmount;
                        model.EMIOutstandingAmount = emiDetails.TotalOutstanding;
                        Log.Info("Paybill for CA:" + model.AccountNumber + ", EMI:" + model.AmountPayable, this);
                        var emiFlagUpdateDetails = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(model.AccountNumber, "U");
                        Log.Info("Paybill EMI Update details for CA:" + model.AccountNumber + ", EMI: Status-" + emiDetails.Status + ", EMI-" + emiDetails.EMIInstallmentAmount + ", Total Outstannding" + emiDetails.TotalOutstanding, this);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.AmountPayable.ToString()) || model.AmountPayable.ToString().Any(char.IsLetter))
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get(" / Accounts/Pay Bill/InvalidAmount", "Invalid amount payable value."));
                            return this.View(model);
                        }
                        else if (Convert.ToDecimal(model.AmountPayable) < 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountNegativeValidation", "You have no amount payable value."));
                            return this.View(model);
                        }
                        else if (Convert.ToDecimal(Session["Amountpayable"]) >= 100 && Convert.ToDecimal(model.AmountPayable) < 100)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Amount not valid", "Minimum Amount payable Value is 100. Please enter valid amount."));
                            return this.View(model);
                        }
                        else if (Convert.ToDecimal(model.AmountPayable) == 0 && Convert.ToDecimal(model.AdvanceAmmount) <= 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AdvanceAmountValidation", "You have no amount payable value. Please enter proper advance amount."));
                            return this.View(model);
                        }
                        else
                        {
                            model.AmountPayable = System.Convert.ToDecimal(model.AmountPayable.ToString().Trim()) == 0 ? System.Convert.ToDecimal(model.AdvanceAmmount.ToString().Trim()).ToString("f2") : System.Convert.ToDecimal(model.AmountPayable.ToString().Trim()).ToString("f2");
                        }
                    }

                    var checkForPAN = SapPiService.Services.RequestHandler.FetchBilling(model.AccountNumber);
                    if (!string.IsNullOrEmpty(checkForPAN.Flag) && checkForPAN.Flag == "8")
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/PANNumberMandatory", "Dear Consumer, We cannot accept the said payment since as per Income Tax Rules, your PAN number is mandatory. Kindly contact our Customer Care Services Department to update your PAN number."));
                        return this.View(model);
                    }

                    model.LoginName = !string.IsNullOrEmpty(model.LoginName) ? model.LoginName : model.AccountNumber;
                    model.PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid");
                    switch (model.PaymentGateway)
                    {
                        case (int)EnumPayment.GatewayType.HDFC:
                            this.PaymentService.StorePaymentRequest(model);
                            string hdfcPostData = this.PaymentService.HDFCTransactionRequestAPIRequestPost(model);
                            return Content(hdfcPostData);

                        case (int)EnumPayment.GatewayType.PayUMoney:
                            this.PaymentService.StorePaymentRequest(model);
                            string payUHTML = this.PaymentService.PayUMoneyTransactionRequestAPIRequestPost(model);
                            return Content(payUHTML);

                        case (int)EnumPayment.GatewayType.BillDesk:
                            this.PaymentService.StorePaymentRequest(model);
                            string RequestHTML = this.PaymentService.BillDeskTransactionRequestAPIRequestPost(model);
                            return Content(RequestHTML);


                        case (int)EnumPayment.GatewayType.Paytm:
                            this.PaymentService.StorePaymentRequest(model);
                            string outputHTML = this.PaymentService.PaytmTransactionRequestAPIRequestPost(model);
                            return Content(outputHTML);

                        case (int)EnumPayment.GatewayType.Ebixcash:
                            this.PaymentService.StorePaymentRequest(model);
                            string submitHTML = this.PaymentService.EbixcashTransactionRequestAPIRequestPost(model);
                            return Content(submitHTML);

                        case (int)EnumPayment.GatewayType.ICICIBank:
                            this.PaymentService.StorePaymentRequest(model);
                            string submitdata = this.PaymentService.ICICITransactionRequestAPIRequestPost(model);
                            return Content(submitdata);
                        case (int)EnumPayment.GatewayType.Benow:
                            this.PaymentService.StorePaymentRequest(model);
                            if (SessionHelper.UserSession.UserSessionContext == null)
                            {
                                SessionHelper.UserSession.UserSessionContext = new DashboardModel()
                                {
                                    OrderId = model.OrderId
                                };
                            }
                            else
                            {
                                SessionHelper.UserSession.UserSessionContext.OrderId = model.OrderId;
                            }
                            //TempData["orderid"] = model.OrderId;
                            //TempData.Keep();
                            Sitecore.Diagnostics.Log.Info("Call to BENOWTransactionRequestAPIRequestGET", this);
                            string BNsubmitdata = this.PaymentService.BENOWTransactionRequestAPIRequestGET(model);
                            return Content(BNsubmitdata);

                        case (int)EnumPayment.GatewayType.DBSUPI:
                            this.PaymentService.StorePaymentRequest(model);
                            if (SessionHelper.UserSession.UserSessionContext == null)
                            {
                                SessionHelper.UserSession.UserSessionContext = new DashboardModel()
                                {
                                    OrderId = model.OrderId
                                };
                            }
                            else
                            {
                                SessionHelper.UserSession.UserSessionContext.OrderId = model.OrderId;
                            }
                            Sitecore.Diagnostics.Log.Info("Call to DBSUPITransactionRequestAPIRequestGET", this);
                            string dbsUPIsubmitdata = this.PaymentService.DBSUPITransactionRequestAPIRequestGET(model);
                            Sitecore.Diagnostics.Log.Info("Call to DBSUPITransactionRequestAPIRequestGET dbsUPIsubmitdata:" + dbsUPIsubmitdata, this);
                            return Content(dbsUPIsubmitdata);

                        case (int)EnumPayment.GatewayType.CITYUPI:
                            this.PaymentService.StorePaymentRequest(model);
                            if (SessionHelper.UserSession.UserSessionContext == null)
                            {
                                SessionHelper.UserSession.UserSessionContext = new DashboardModel()
                                {
                                    OrderId = model.OrderId
                                };
                            }
                            else
                            {
                                SessionHelper.UserSession.UserSessionContext.OrderId = model.OrderId;
                            }
                            Sitecore.Diagnostics.Log.Info("Call to CityUPITransactionRequestAPIRequestGET", this);
                            string cityUPIsubmitdata = this.PaymentService.CityUPITransactionRequestAPIRequestGET(model);
                            Sitecore.Diagnostics.Log.Info("Call to CityUPITransactionRequestAPIRequestGET dbsUPIsubmitdata:" + cityUPIsubmitdata, this);
                            return Content(cityUPIsubmitdata);

                        default:
                            break;
                    }
                }
                else if (!string.IsNullOrEmpty(Pay_VDSpayment)) //VDS Payment
                {
                    string accountnumber = SessionHelper.UserSession.UserSessionContext.AccountNumber;
                    if (model.AccountNumber != accountnumber)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidRequest", "Invalid request."));
                        return this.View(model);
                    }
                    model.AverageVDSAmount = Convert.ToDecimal(Session["PaymentVDSAmount"]);
                    model.MaxVDSAmount = Convert.ToDecimal(Session["maxVDSAmount"]);
                    model.Remark = model.PANNo;
                    model.PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/VDS", "VDS payment");
                    model.LoginName = !string.IsNullOrEmpty(model.LoginName) ? model.LoginName : model.AccountNumber;
                    model.PaymentGateway = 2;
                    //if (Convert.ToDecimal(model.PaymentVDSAmount) > 0 && Convert.ToDecimal(Session["PaymentVDSAmount"]) != Convert.ToDecimal(model.PaymentVDSAmount))
                    //{
                    //    this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "You have modified VDS Amount Value. Please refresh the page and try again."));
                    //    return this.View(model);
                    //}
                    //if (string.IsNullOrEmpty(model.PaymentVDSAmount.ToString()) && model.PaymentVDSAmount.ToString().Any(char.IsLetter))
                    //{
                    //    this.ModelState.AddModelError("PaymentVDSAmount", DictionaryPhraseRepository.Current.Get(" / Accounts/Pay Bill/InvalidAmount", "Invalid amount payable value."));
                    //    return this.View(model);
                    //}
                    //if (model.PaymentVDSAmount <= 0)
                    //{
                    //    this.ModelState.AddModelError("PaymentVDSAmount", DictionaryPhraseRepository.Current.Get(" / Accounts/Pay Bill/InvalidAmount", "Invalid amount payable value."));
                    //    return this.View(model);
                    //}
                    if (string.IsNullOrEmpty(model.PaymentVDSAmount.ToString()) && model.PaymentVDSAmount.ToString().Any(char.IsLetter))
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get(" / Accounts/Pay Bill/InvalidAmount", "Invalid amount payable value."));
                        return this.View(model);
                    }
                    if (Convert.ToDecimal(model.PaymentVDSAmount) < 0)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value!"));
                        return this.View(model);
                    }
                    if (Convert.ToDecimal(model.PaymentVDSAmount) % 500 != 0)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value, it should be multiple of 500!"));
                        return this.View(model);
                    }
                    if (Convert.ToDecimal(model.PaymentVDSAmount) < Convert.ToDecimal(Session["PaymentVDSAmount"]) || Convert.ToDecimal(model.PaymentVDSAmount) > Convert.ToDecimal(Session["maxVDSAmount"]))
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value, it should be in between min and max values."));
                        return this.View(model);
                    }

                    if (string.IsNullOrEmpty(model.PANNo))
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/PAN No", "Please enter a valid PAN No."));
                        return this.View(model);
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$");
                        //If you want both upper case and lower case alphabets.  
                        Match match = regex.Match(model.PANNo);
                        if (!match.Success)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/PAN No", "Please enter a valid PAN No."));
                            return this.View(model);
                        }
                    }
                    if (string.IsNullOrEmpty(model.VDSPaymentSelection))
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/VDS payment selection required", "Please select VDS payment Mode."));
                        return this.View(model);
                    }

                    model.AmountPayable = model.PaymentVDSAmount.ToString("f2");
                    this.PaymentService.StorePaymentRequest(model);
                    string RequestHTML = this.PaymentService.BillDeskVDSTransactionRequestAPIRequestPost(model);
                    return Content(RequestHTML);
                }
                else if (!string.IsNullOrEmpty(Pay_Other)) //Security Desposit
                {
                    string accountnumber = SessionHelper.UserSession.UserSessionContext.AccountNumber;
                    if (model.AccountNumber != accountnumber)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidRequest", "Invalid request."));
                        return this.View(model);
                    }
                    model.LoginName = !string.IsNullOrEmpty(model.LoginName) ? model.LoginName : model.AccountNumber;
                    model.PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit");

                    if (model.SecurityDepositAmountType == "Actual")
                    {
                        if (Convert.ToDecimal(model.SecurityDeposit) > 0 && Convert.ToDecimal(Session["SecurityDeposit"]) != Convert.ToDecimal(model.SecurityDeposit))
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchSD", "You have modified Security Deposit amount Value. Please refresh the page and try again."));
                            return this.View(model);
                        }
                        if (Convert.ToDecimal(model.SecurityDeposit) <= 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/SecurityDepositeValidation", "You have no security deposit payable amount."));
                            return this.View(model);
                        }
                    }
                    else if (model.SecurityDepositAmountType == "Partial")
                    {
                        if (Convert.ToDecimal(model.SecurityDepositPartial) <= 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/SecurityDepositeValidation", "Please enter true value for security deposit payable amount."));
                            return this.View(model);
                        }
                    }
                    if (string.IsNullOrEmpty(model.SecurityPayment))
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Security payment selection required  ", "Please select security Payment option."));
                        return this.View(model);
                    }
                    if (model.SecurityPayment == DictionaryPhraseRepository.Current.Get("/Payment Settings/ICICI", "ICICI"))
                    {
                        model.PaymentGateway = 5;
                        this.PaymentService.StorePaymentRequest(model);
                        string submitHTML = this.PaymentService.ICICITransactionRequestAPIRequestPost(model);
                        return Content(submitHTML);
                    }
                    else
                    {
                        model.PaymentGateway = 2;
                        this.PaymentService.StorePaymentRequest(model);
                        string RequestHTML = this.PaymentService.BillDeskSDTransactionRequestAPIRequestPost(model);
                        return Content(RequestHTML);
                    }
                }
                #endregion

                #region leftpanel AccountDetail Search

                if (!string.IsNullOrEmpty(paybillforAccount))
                {
                    if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                    {
                        ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                        ViewBag.NoInfo = "No Invoice Available.";
                        return this.View(model);
                    }

                    ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                    if (!reCaptchaResponse.success)
                    {
                        ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                        ViewBag.NoInfo = "No Invoice Available.";
                        return this.View(model);
                    }
                    //if (!this.IsCaptchaValid("Captcha Validation Required."))
                    //{
                    //    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    //    ViewBag.NoInfo = "No Invoice Available.";
                    //    return this.View(model);
                    //}
                    ViewBag.NoInfo = null;
                    try
                    {
                        Log.Info("Paybill fetching details for CA:" + model.AccountNumber, this);
                        var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(model.AccountNumber);
                        Log.Info("Paybill details received for CA:" + accountDetails.AccountNumber, this);

                        var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(model.AccountNumber);
                        Log.Info("Paybill billing amount for CA:" + billinghDetails.AmountPayable, this);

                        var securityAmount = SapPiService.Services.RequestHandler.FetchSecurityDepositAmount(model.AccountNumber);

                        var emiDetails = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(model.AccountNumber);
                        Log.Info("Paybill fetching EMI details for CA:" + model.AccountNumber + ", EMI: Status-" + emiDetails.Status + ", EMI-" + emiDetails.EMIInstallmentAmount + ", Total Outstannding" + emiDetails.TotalOutstanding, this);

                        //var vdsAmount = SapPiService.Services.RequestHandler.FetchVdsAmount(profile.AccountNumber);
                        Session["SecurityDeposit"] = securityAmount.ToString();
                        if (SessionHelper.UserSession.UserSessionContext == null)
                        {
                            SessionHelper.UserSession.UserSessionContext = new DashboardModel
                            {
                                AccountNumber = model.AccountNumber
                            };
                        }
                        else
                        {
                            SessionHelper.UserSession.UserSessionContext.AccountNumber = model.AccountNumber;
                        }

                        //var ItemId = this.AccountRepository.GetAccountItemId(model.AccountNumber);
                        string email = string.Empty, mobile = string.Empty, LoginName = string.Empty;
                        //if (!ID.IsNullOrEmpty(ItemId))
                        //{
                        //    var getusers = this.AccountRepository.GetUserbyItemId(ItemId.ToString());
                        //    if (getusers != null && getusers.Count > 0)
                        //    {
                        //        //Note : Get User Detail Based on Account Number and Set Property like "Token" and "TokenGenerationDate" in User Profile.
                        //        var existingUser = getusers.FirstOrDefault();
                        //        email = existingUser.Profile.Email;
                        //        mobile = existingUser.Profile.GetCustomProperty(Constants.UserProfile.Fields.MobileNo);
                        //        LoginName = !string.IsNullOrEmpty(existingUser.Profile.UserName.Split('\\').LastOrDefault()) ? existingUser.Profile.UserName.Split('\\').LastOrDefault() : accountDetails.AccountNumber;
                        //    }
                        //}

                        decimal minVDSAmount = 0;
                        decimal maxVDSAmount = 0;
                        try
                        {
                            var VDSAmountDetails = SapPiService.Services.RequestHandler.FetchVdsAmount(model.AccountNumber);
                            if (VDSAmountDetails.CurrentOutstanding > 0)
                            {
                                ViewBag.VDSMessage = "Dear Consumer, you need to clear your current outstanding for VDS payments.";
                            }
                            else
                            {
                                var gap = 100000 - VDSAmountDetails.ExistingVdsBalance;
                                if (VDSAmountDetails.AverageBillingAmount < 3000)
                                {
                                    minVDSAmount = 3000;
                                }
                                else if (VDSAmountDetails.AverageBillingAmount > 3000)
                                {
                                    minVDSAmount = Math.Round(Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                                }
                                if (gap >= 0 && gap < Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
                                {
                                    minVDSAmount = gap;
                                }
                                maxVDSAmount = gap;
                                //if (VDSAmountDetails.AverageBillingAmount < 3000)
                                //    VDSAmount = 3000;
                                //else if (VDSAmountDetails.AverageBillingAmount > 10000)
                                //    VDSAmount = 10000;
                                //else if (VDSAmountDetails.AverageBillingAmount > 3000 && VDSAmountDetails.AverageBillingAmount < 10000)
                                //{
                                //    VDSAmount = Math.Round(Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;

                                //}
                            }
                        }
                        catch
                        {
                            ViewBag.VDSMessage = "There are some issues in fetching VDS amount.";
                        }

                        if (string.IsNullOrEmpty(accountDetails.CycleNumber) || billinghDetails.BillingStatus == BillingStatus.NoInvoice)
                        {
                            ViewBag.NoInfo = "No Invoice Available.";
                        }
                        else
                        {
                            decimal amountpayable = billinghDetails.AmountPayable;


                            model = new ViewPayBill()
                            {
                                AccountNumber = accountDetails.AccountNumber,// this.UserProfileService.GetAccountNumberfromItem(masterAccount.ToString()),
                                Name = accountDetails.Name,// this.UserProfileService.GetLoginName(),
                                BookNumber = accountDetails.BookNumber,// "505",
                                CycleNumber = accountDetails.CycleNumber,// "05",
                                Zone = accountDetails.ZoneNumber,// "South Central",
                                Address = accountDetails.Address,// "300 288 Shere Punjab SOC Mahakali Caves Rd Andheri E Near Tolani College Mumbai 400067",
                                BillMonth = billinghDetails.BillMonth,// "May-2018",
                                PaymentDueDate = billinghDetails.DateDue,// "2018-06-16",
                                TariffSlab = billinghDetails.TariffSlab,// "LT-1b",
                                MeterNumber = string.Join(",", billinghDetails.MeterNumbers),// "7587321",
                                UnitsConsumed = billinghDetails.UnitsConsumed,//  "1",
                                TotalCharges = billinghDetails.TotalCharges,// "134.27",
                                CurrentMonthsBills = billinghDetails.CurrentMonthCharge,// "134.69",
                                BroughtForward = billinghDetails.BroughtForward,// "128.26",
                                TotalBillAmount = billinghDetails.TotalBillAmount,//  "246.19",
                                SecurityDeposit = securityAmount.ToString(),// "560.00",
                                SecurityDepositAmountType = "Actual",                                      //VDSAmount = vdsAmount.ToString(),
                                AmountPayable = amountpayable.ToString(),//billinghDetails.AmountPayable.ToString(),// "250"
                                Email = email,
                                Mobile = mobile,
                                LoginName = accountDetails.AccountNumber,
                                AverageVDSAmount = minVDSAmount,
                                PaymentVDSAmount = minVDSAmount,
                                SecurityPaymentList = securitypaymentlist,
                                VDSPaymentList = VDSpaymentlist,
                                MaxVDSAmount = maxVDSAmount,
                                EMIEligible = emiDetails.Status == "S" ? true : false,
                                EMIInstallmentAmount = emiDetails.EMIInstallmentAmount,
                                EMIOutstandingAmount = emiDetails.TotalOutstanding,
                                ProceedWithEMI = false
                            };
                            Session["Amountpayable"] = amountpayable.ToString(); // billinghDetails.AmountPayable.ToString();
                            Session["PaymentVDSAmount"] = minVDSAmount.ToString();
                            Session["maxVDSAmount"] = maxVDSAmount.ToString();
                            this.ModelState["Email"].Errors.Clear();
                            this.ModelState["Mobile"].Errors.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at leftpanel AccountDetail Search- PayBill - Method - PayBill :" + ex.Message, this);
                    }
                }
                #endregion
                return this.View(model);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at PayBill:" + ex.Message, this);
                return this.View(model);

            }
        }
        #endregion
        #region BeNow Web Service
        [HttpPost]
        public ActionResult Benow_CallbackS2S()
        {
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            string decryptedstring = "";
            string response = "";
            try
            {
                StreamReader bodyStream = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);

                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                string encryptedString = bodyStream.ReadToEnd();

                Sitecore.Diagnostics.Log.Info("BeNow_Callback Method Called encryptedString:" + encryptedString, this);

                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string BNW_HashKey = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_HashKey].Value;
                byte[][] hashKeys = EncryptionDecryption.GetHashKeys(BNW_HashKey);
                decryptedstring = EncryptionDecryption.Decrypt(encryptedString, hashKeys);

                Sitecore.Diagnostics.Log.Info("BeNow_Callback Method Called decryptedstring:" + decryptedstring, decryptedstring);
                //response = new JavaScriptSerializer().Deserialize<string>(encryptedString);


                BeNowResponse request = JsonConvert.DeserializeObject<BeNowResponse>(decryptedstring);
                request.ResponseMessage = decryptedstring;

                Sitecore.Diagnostics.Log.Info("BeNow_Callback Method Called - request found with order id: " + request.refNumber, this);

                //string authHeader = HttpContext.Request.Headers["Authorization"];
                //Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                //if (string.IsNullOrEmpty(authHeader))
                //{
                //    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //}

                //string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                //string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                //int seperatorIndex = usernamePassword.IndexOf(':');
                //string username = usernamePassword.Substring(0, seperatorIndex);
                //string password = usernamePassword.Substring(seperatorIndex + 1);

                //string apiUserName = ConfigurationManager.AppSettings["apiUserName"];
                //string apiPassword = ConfigurationManager.AppSettings["apiPassword"];

                //if (username == apiUserName && password == apiPassword)
                //{
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    Sitecore.Diagnostics.Log.Info("BeNow_Callback Method Called - finding PaymentHistories request.refNumber:" + request.refNumber, this);
                    Accounts.PaymentHistory ctx = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == request.refNumber).FirstOrDefault();
                    if (ctx != null)
                    {
                        Sitecore.Diagnostics.Log.Info("BeNow_Callback Method Called - PaymentHistories record found: " + request.refNumber, this);
                        request.AccountNumber = ctx.AccountNumber;
                        request.PaymentType = ctx.PaymentType;
                        request.PaymentMode = ctx.PaymentMode;
                        Sitecore.Diagnostics.Log.Info("BeNow_Callback Method Called - StorePaymentRequestBeNow called: " + request.refNumber, this);
                        if (PaymentService.StorePaymentRequestBeNow(request))
                        {
                            Sitecore.Diagnostics.Log.Info("BeNow_Callback Method Called - StorePaymentRequestBeNow called Successfully " + request.refNumber, this);

                            ViewPayBill modelviewpay = new ViewPayBill()
                            {
                                TransactionId = request.txnId.ToString(),
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = request.txnStatus.ToString() ?? string.Empty,
                                Remark = Constants.PaymentResponse.Success,
                                PaymentRef = request.refNumber.ToString(),
                                OrderId = request.refNumber.ToString(),
                                AmountPayable = request.amount.ToString(),
                                AccountNumber = request.AccountNumber.ToString(),
                                msg = Constants.PaymentResponse.Success,
                                PaymentMode = request.PaymentMode.ToString(),
                                TransactionDate = request.transactionDate.ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                                PaymentGateway = 6
                            };
                            TempData["PaymentResponse"] = modelviewpay;
                            TempData.Keep();
                            //if (PaymentHistoryRecord.Status == "Successful")
                            //{
                            //    return SuccessUrl;
                            //}
                            //else
                            //{
                            //    return FailureUrl;
                            //}

                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Successfully Added." }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Info("BeNow_Callback Method Called - StorePaymentRequestBeNow called Server Error " + request.refNumber, this);
                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = false, Message = "Server Error" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Info("BeNow_Callback Method Called - PaymentHistories called no record found " + request.refNumber, this);
                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Created, IsSuccess = false, Message = "Entry does not exists." }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }

                //}
                //else
                //{
                //    Sitecore.Diagnostics.Log.Error("BeNow Credentials not valid a BeNow_Callback"+" Response:"+ response+ " Decryptedstring:" + decryptedstring, this);
                //    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //}
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BeNow_Callback :" + ex.Message + " Response: " + response + " Decryptedstring: " + decryptedstring, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpGet]
        public string IsSuccess()
        {
            string id = string.Empty;
            //id = TempData["orderid"].ToString();
            id = TempData.Peek("orderid").ToString();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        if (dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == id && x.TransactionId != null).Any())
                        {
                            TempData.Remove("orderid");
                            return DictionaryPhraseRepository.Current.Get("/Payment Messages/UPI/Success Url", "/success");
                        }
                        else
                        {
                            //call again
                        }
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at Benow payment IsSuccess:" + ex.Message, this);
                    return DictionaryPhraseRepository.Current.Get("/Payment Messages/UPI/Failure", "Pleae try agian..");
                }
            }
            return string.Empty;
        }



        [HttpPost]
        public string Benow_Callback()
        {
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            string id = string.Empty;
            id = SessionHelper.UserSession.UserSessionContext.OrderId.ToString();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        Accounts.PaymentHistory PaymentHistoryRecord = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == id && x.TransactionId != null).FirstOrDefault();
                        if (PaymentHistoryRecord != null)
                        {
                            ViewPayBill modelviewpay = new ViewPayBill()
                            {
                                TransactionId = PaymentHistoryRecord.TransactionId.ToString(),
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = PaymentHistoryRecord.Responsecode.ToString(),
                                Remark = Constants.PaymentResponse.Success,
                                PaymentRef = PaymentHistoryRecord.PaymentRef.ToString(),
                                OrderId = PaymentHistoryRecord.OrderId.ToString(),
                                AmountPayable = PaymentHistoryRecord.Amount.ToString(),
                                AccountNumber = PaymentHistoryRecord.AccountNumber.ToString(),
                                msg = Constants.PaymentResponse.Success,
                                PaymentMode = PaymentHistoryRecord.PaymentMode.ToString(),
                                TransactionDate = PaymentHistoryRecord.Created_Date.ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                                PaymentGateway = 6
                            };
                            TempData["PaymentResponse"] = modelviewpay;
                            TempData.Keep();
                            if (PaymentHistoryRecord.Status == "Successful")
                            {
                                return SuccessUrl;
                            }
                            else
                            {
                                return FailureUrl;
                            }
                        }
                        else
                        {
                            //call again
                        }
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at Benow payment IsSuccess:" + ex.Message, this);
                    return string.Empty;
                }
            }
            return "";
        }

        [HttpPost]
        public ActionResult DBS_Callback()
        {
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            string decryptedstring = "";
            string response = "";
            try
            {
                Sitecore.Diagnostics.Log.Info("DBS_Callback Method Called ", this);

                StreamReader bodyStream = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);

                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                string encryptedString = bodyStream.ReadToEnd();

                Sitecore.Diagnostics.Log.Info("DBS_Callback Method Called encryptedString:" + encryptedString, this);

                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string publicKey = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_ServerPublicKeyPath].Value; //@"C:\Users\Nidhi.Paneri\Desktop\DBS\Key Pair\from DBS\DBSSG_EN_PUBLIC.asc";
                string privateKey = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_ClientPrivateKeyPath].Value; //@"C:\Users\Nidhi.Paneri\Desktop\DBS\Key Pair\DBS_PrivateSecret.asc";
                string privayeKeyPwd = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_ClientSecretKey].Value; //"5&rd&Xdj#nJR";

                Decrypt decrypt = new Decrypt();
                decrypt.IsVerify = true;
                decrypt.PublicKeyFilePath = Server.MapPath(publicKey);
                decrypt.PrivateKeyFilePath = Server.MapPath(privateKey);
                decrypt.PrivateKeyPassword = privayeKeyPwd;

                BCPGPDecryptor objPgpDecrypt = new BCPGPDecryptor(decrypt);
                string decryptedMsg = objPgpDecrypt.DecryptMessage(encryptedString);
                Sitecore.Diagnostics.Log.Info("DBS_Callback Method Called decryptedstring:" + decryptedMsg, this);

                DBSResponse request = new JavaScriptSerializer().Deserialize<DBSResponse>(decryptedMsg);

                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    CityPayment obj = new CityPayment
                    {
                        //NPCITxnId = request.txnInfo.txnRefId,
                        //OrderNo = request.txnInfo.refId,
                        ////RespCode = request.RespCode,
                        //SettlementAmount = request.txnInfo.txnAmount.ToString(),
                        ////SettlementCurrency = request.SettlementCurrency,
                        ////StatusCode = request.StatusCode,
                        //StatusDesc = request.ResponseMessage,
                        //TimeStamp = request.header.timeStamp.ToString(),
                        //TranAuthDate = request.header.timeStamp.ToString(),
                        //TxnRefNo = request.txnInfo.refId,
                        Created_Date = DateTime.Now,
                        Response_Msg = decryptedMsg
                    };
                    dbcontext.CityPayments.InsertOnSubmit(obj);
                    dbcontext.SubmitChanges();

                    Sitecore.Diagnostics.Log.Info("DBS_Callback Method Called - finding PaymentHistories request orderid refNumber:" + request.txnInfo.refId + ", Account number:" + request.txnInfo.customerReference, this);
                    Accounts.PaymentHistory ctx = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == request.txnInfo.customerReference).OrderByDescending(p => p.Created_Date).FirstOrDefault();
                    if (ctx != null)
                    {
                        Sitecore.Diagnostics.Log.Info("DBS_Callback Method Called - PaymentHistories record found: " + ctx.Id, this);
                        request.AccountNumber = ctx.AccountNumber;
                        request.PaymentType = ctx.PaymentType;
                        request.PaymentMode = ctx.PaymentMode;
                        request.ResponseMessage = decryptedMsg;
                        request.paymentMethod = ctx.PaymentMode;
                        if (PaymentService.StorePaymentRequestDBS(request))
                        {
                            Sitecore.Diagnostics.Log.Info("DBS_Callback Method Called - StorePaymentRequestBeNow called Successfully " + request.refNumber, this);

                            ViewPayBill modelviewpay = new ViewPayBill()
                            {
                                TransactionId = request.txnInfo.txnRefId.ToString(),
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = Constants.PaymentResponse.Success.ToString() ?? string.Empty,
                                Remark = request.txnInfo.customerReference,
                                PaymentRef = request.txnInfo.refId.ToString(),
                                OrderId = request.txnInfo.customerReference.ToString(),
                                AmountPayable = request.txnInfo.txnAmount.ToString(),
                                AccountNumber = ctx.AccountNumber,
                                msg = Constants.PaymentResponse.Success,
                                PaymentMode = ctx.PaymentMode.ToString(),
                                TransactionDate = request.header.timeStamp.ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                                PaymentGateway = 9
                            };
                            TempData["PaymentResponse"] = modelviewpay;
                            TempData.Keep();

                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Successfully Added." }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Info("DBS_Callback Method Called - StorePaymentRequestBeNow called Server Error " + request.refNumber, this);
                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = false, Message = "Server Error" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Info("DBS_Callback Method Called - PaymentHistories called no record found " + request.refNumber, this);
                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Created, IsSuccess = false, Message = "Entry does not exists." }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at DBS_Callback :" + ex.Message + " Response: " + response + " Decryptedstring: " + decryptedstring, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public string IsSuccessDBS_Callback()
        {
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);

            string id = string.Empty;
            id = SessionHelper.UserSession.UserSessionContext.OrderId.ToString();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        Accounts.PaymentHistory PaymentHistoryRecord = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == id && x.TransactionId != null).FirstOrDefault();
                        if (PaymentHistoryRecord != null)
                        {
                            ViewPayBill modelviewpay = new ViewPayBill()
                            {
                                TransactionId = PaymentHistoryRecord.TransactionId.ToString(),
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = PaymentHistoryRecord.Responsecode.ToString(),
                                Remark = Constants.PaymentResponse.Success,
                                PaymentRef = PaymentHistoryRecord.PaymentRef.ToString(),
                                OrderId = PaymentHistoryRecord.OrderId.ToString(),
                                AmountPayable = PaymentHistoryRecord.Amount.ToString(),
                                AccountNumber = PaymentHistoryRecord.AccountNumber.ToString(),
                                msg = Constants.PaymentResponse.Success,
                                PaymentMode = PaymentHistoryRecord.PaymentMode.ToString(),
                                TransactionDate = PaymentHistoryRecord.Created_Date.ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                                PaymentGateway = 9
                            };
                            TempData["PaymentResponse"] = modelviewpay;
                            TempData.Keep();
                            if (PaymentHistoryRecord.Status == DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/DBS/Payment Success", "Success"))
                            {
                                return SuccessUrl;
                            }
                            else
                            {
                                return FailureUrl;
                            }
                        }
                        else
                        {
                            //call again
                        }
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at Benow payment IsSuccess:" + ex.Message, this);
                    return string.Empty;
                }
            }
            return "";
        }

        #endregion
        #endregion




        //API for Chatbot - Payment Gateway

        [HttpGet]
        public ActionResult GetPaymentLink(string caNumber, string mobileNumber, string emailId, int paymentGateway)
        {
            Sitecore.Diagnostics.Log.Info("GetPaymentLink Method Called", this);
            string authHeader = HttpContext.Request.Headers["Authorization"];
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            if (string.IsNullOrEmpty(authHeader))
            {
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            int seperatorIndex = usernamePassword.IndexOf(':');
            string username = usernamePassword.Substring(0, seperatorIndex);
            string password = usernamePassword.Substring(seperatorIndex + 1);

            string apiUserName = ConfigurationManager.AppSettings["apiUserName"];
            string apiPassword = ConfigurationManager.AppSettings["apiPassword"];

            Sitecore.Diagnostics.Log.Info("GetPaymentLink Method called at " + DateTime.Now.ToString(), this);
            Sitecore.Diagnostics.Log.Info("GetPaymentLink caNumber:" + caNumber + " mobileNumber:" + mobileNumber + " emailId:" + emailId + " paymentGateway:" + paymentGateway, this);
            Sitecore.Diagnostics.Log.Info("GetPaymentLink apiUserName:" + apiUserName + " apiPassword:" + apiPassword, this);
            Sitecore.Diagnostics.Log.Info("GetPaymentLink username:" + username + " password:" + password, this);
            if (username == apiUserName && password == apiPassword)
            {
                string RequestHTML = "";

                try
                {
                    var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(caNumber);
                    var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(caNumber);

                    ViewPayBill model = new ViewPayBill();
                    model.Email = emailId;
                    model.AccountNumber = caNumber;
                    model.Mobile = mobileNumber;
                    model.PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid");

                    model.BillMonth = billinghDetails.BillMonth;
                    model.LoginName = caNumber;
                    model.AmountPayable = billinghDetails.AmountPayable.ToString();
                    model.CycleNumber = accountDetails.CycleNumber;
                    model.PaymentDueDate = billinghDetails.DateDue;
                    model.PaymentGateway = paymentGateway;
                    model.msg = "Chat Bot";
                    model.UserType = "Chat Bot";

                    switch (paymentGateway)
                    {
                        case (int)EnumPayment.GatewayType.BillDesk:
                            this.PaymentService.StorePaymentRequest(model);
                            RequestHTML = this.PaymentService.BillDeskTransactionRequestAPIRequestPost(model);
                            break;
                        //return Content(RequestHTML);

                        case (int)EnumPayment.GatewayType.Paytm:
                            this.PaymentService.StorePaymentRequest(model);
                            RequestHTML = this.PaymentService.PaytmTransactionRequestAPIRequestPost(model);
                            break;
                        //return Content(outputHTML);
                        case (int)EnumPayment.GatewayType.ICICIBank:
                            this.PaymentService.StorePaymentRequest(model);
                            RequestHTML = this.PaymentService.ICICITransactionRequestAPIRequestPost(model);
                            break;
                        //return Content(submitHTML);
                        case (int)EnumPayment.GatewayType.Benow:
                            this.PaymentService.StorePaymentRequest(model);
                            SessionHelper.UserSession.UserSessionContext.OrderId = model.OrderId;
                            RequestHTML = this.PaymentService.BENOWTransactionRequestAPIRequestGET(model);
                            break;
                        //return Content(BenowsubmitHTML);
                        default:
                            break;
                    }
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Success", Result = RequestHTML }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at getting UserBillingDetails :" + ex.Message, this);
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = ex.Message, Result = RequestHTML }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        [HttpGet]
        public ActionResult UserBillingDetails(string caNumber)
        {
            //Sitecore.Diagnostics.Log.Info("GetUserCAList Method Called", this);
            //string authHeader = HttpContext.Request.Headers["Authorization"];
            //Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            //if (string.IsNullOrEmpty(authHeader))
            //{
            //    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //}

            //string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            //string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            //int seperatorIndex = usernamePassword.IndexOf(':');
            //string username = usernamePassword.Substring(0, seperatorIndex);
            //string password = usernamePassword.Substring(seperatorIndex + 1);
            ViewPayBill model = new ViewPayBill();

            try
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");

                model.AccountNumber = caNumber;

                var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(model.AccountNumber);
                var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(model.AccountNumber);

                if (string.IsNullOrEmpty(accountDetails.CycleNumber) || billinghDetails.BillingStatus == BillingStatus.NoInvoice)
                {
                    ViewBag.NoInfo = "No Invoice Available.";
                }
                else
                {
                    string email = string.Empty, mobile = string.Empty, LoginName = string.Empty;
                    decimal amountpayable = billinghDetails.AmountPayable;
                    model = new ViewPayBill()
                    {
                        AccountNumber = accountDetails.AccountNumber,// this.UserProfileService.GetAccountNumberfromItem(masterAccount.ToString()),
                        Name = accountDetails.Name,// this.UserProfileService.GetLoginName(),
                        BookNumber = accountDetails.BookNumber,// "505",
                        CycleNumber = accountDetails.CycleNumber,// "05",
                        Zone = accountDetails.ZoneNumber,// "South Central",
                        Address = accountDetails.Address,// "300 288 Shere Punjab SOC Mahakali Caves Rd Andheri E Near Tolani College Mumbai 400067",
                        BillMonth = billinghDetails.BillMonth,// "May-2018",
                        PaymentDueDate = billinghDetails.DateDue,// "2018-06-16",
                        TariffSlab = billinghDetails.TariffSlab,// "LT-1b",
                        MeterNumber = string.Join(",", billinghDetails.MeterNumbers),// "7587321",
                        UnitsConsumed = billinghDetails.UnitsConsumed,//  "1",
                        TotalCharges = billinghDetails.TotalCharges,// "134.27",
                        CurrentMonthsBills = billinghDetails.CurrentMonthCharge,// "134.69",
                        BroughtForward = billinghDetails.BroughtForward,// "128.26",
                        TotalBillAmount = billinghDetails.TotalBillAmount,//  "246.19",
                        AmountPayable = amountpayable.ToString(),//billinghDetails.AmountPayable.ToString(),// "250"
                        Email = email,
                        Mobile = mobile,
                        LoginName = accountDetails.AccountNumber,
                        Flag = billinghDetails.Flag
                    };
                }
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Success", Result = model }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at getting UserBillingDetails :" + ex.Message, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = false, Message = ex.Message, Result = model }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        [HttpGet]
        public ActionResult eNachRegistration()
        {
            ENachRegistrationModel model = new ENachRegistrationModel();
            model.IsvalidatAccount = false;
            model.IsOTPSent = false;
            return this.View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult eNachRegistration(ENachRegistrationModel registrationInfo, string ValidateAccount = null, string submit = null, string Reset = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View(registrationInfo);
                }

                if (string.IsNullOrEmpty(registrationInfo.AccountNo))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number required", "Please enter valid Account Number."));
                    return this.View(registrationInfo);
                }
                if (registrationInfo.AccountNo.StartsWith("5"))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number start with 5", "Entered  Account no. is not valid account no."));
                    return this.View(registrationInfo);
                }

                if (!string.IsNullOrEmpty(ValidateAccount))
                {
                    //validate account number call sap service
                    var accountDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(registrationInfo.AccountNo);
                    string checkCA = ChangeOfNameService.ValidateCAforENACH(accountDetails);
                    if (!string.IsNullOrEmpty(checkCA))
                    {
                        this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), checkCA);
                        registrationInfo.AccountNo = null;
                        Session["AccountNumberforEnach"] = null;
                        return this.View(registrationInfo);
                    }
                    else
                    {
                        registrationInfo.IsvalidatAccount = true;
                        registrationInfo.Name = accountDetails.NAME_CustomerName;
                        Session["AccountNumberforEnach"] = registrationInfo.AccountNo;
                    }
                }
                else if (!string.IsNullOrEmpty(submit))
                {
                    if (Session["AccountNumberforEnach"] == null || (Session["AccountNumberforEnach"] != null && Session["AccountNumberforEnach"].ToString().Trim() != registrationInfo.AccountNo.Trim()))
                    {
                        this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), "Account number is changed, please try again!");
                        Session["AccountNumberforEnach"] = null;
                        return this.View(registrationInfo);


                    }
                    this.PaymentService.StorePaymentRequestENachRegistration(registrationInfo);

                    List<ENachRedirectionModel> lst = new List<ENachRedirectionModel>();
                    ENachRedirectionModel obj = new ENachRedirectionModel
                    {
                        AppID = DictionaryPhraseRepository.Current.Get("/ENachRegistration/AppID", "800002"),
                        EntityMerchantKey = DictionaryPhraseRepository.Current.Get("/ENachRegistration/EntityMerchantKey", "UAT200210001"),
                        ToDebit = "",
                        Amount = "",
                        Frequency = "",
                        DebitType = "",
                        Reference1 = registrationInfo.AccountNo,
                        Reference2 = registrationInfo.isPPISet.ToString(),
                        PhoneNumber = "",
                        EmailID = "",
                        FromDate = "",
                        ToDate = "",
                        NameInBankRecords1 = "",
                        NameInBankRecords2 = "",
                        NameInBankRecords3 = ""
                    };

                    lst.Add(obj);
                    var inputJson = new JavaScriptSerializer().Serialize(lst);

                    NameValueCollection collections = new NameValueCollection();
                    collections.Add("Request", inputJson);
                    string remoteUrl = DictionaryPhraseRepository.Current.Get("/ENachRegistration/RemoteUrl", "https://insurance.zipnach.com/Master/GetPostedData.aspx");// URL for Posting Data
                    string html = "<html><head><title>Redirecting to NACH</title></head><body><center><h2>Redirecting, Please do not refresh your page...</h2></center>";
                    html += string.Format("<form name='f1' method='POST' action='{0}'>", remoteUrl);
                    foreach (string key in collections.Keys)
                    {
                        html += string.Format("<input type='hidden' name='{0}' type='text' value='{1}'>", key, collections[key]);
                    }

                    html += "<script type='text/javascript'>document.f1.submit();</script></form></body></html>";
                    //html += "</form></body></html>";
                    return Content(html);
                }
                else if (!string.IsNullOrEmpty(Reset))
                {
                    registrationInfo.IsvalidatAccount = false;
                    registrationInfo.AccountNo = null;
                    registrationInfo.isPPISet = false;
                    registrationInfo.Name = null;
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ENACHRegistrationPage));
                }
                return View(registrationInfo);
            }
            catch (Exception ex)
            {
                Log.Error("eNachRegistration:", ex, this);
                this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), ex.Message);
                return this.View(registrationInfo);
            }
        }


        [HttpGet]
        //[RedirectAuthenticated]
        public ActionResult MyAccountSubmitMeterReading()
        {
            var profile = this.UserProfileService.GetProfile(Context.User);

            SubmitMeterReading meterReading = new SubmitMeterReading();

            if (profile.AccountNumber != null)
            {

                try
                {
                    meterReading.Source = "3";
                    meterReading.CANumber = profile.AccountNumber;

                    //  ZBAPI_DM_GETMETERNUMBERService.ZBAPI_DM_T_GETMETERNUMBER_OUT[] it_Output = new ZBAPI_DM_GETMETERNUMBERService.ZBAPI_DM_T_GETMETERNUMBER_OUT[100];


                    var Reqoutput = SapPiService.Services.RequestHandler.ReadMeterNumberForSelfMeterReading(meterReading.CANumber, "", meterReading.Source);


                    List<MeterReadingDetail> MeterList = new List<MeterReadingDetail>();
                    List<MeterAttachment> MeterAttachmentList = new List<MeterAttachment>();

                    if (Reqoutput.IT_OUTPUT.Length > 0)
                    {
                        foreach (ZBAPI_DM_GETMETERNUMBERService.ZBAPI_DM_T_GETMETERNUMBER_OUT output in Reqoutput.IT_OUTPUT)
                        {
                            MeterReadingDetail detail = new MeterReadingDetail();
                            detail.MeterNumber = output.METER_NUMBER;
                            detail.SMRD = output.SMRD;
                            // detail.MeterReadingDate = Convert.ToDateTime(System.DateTime.Now.ToString("dd-MM-yyyy"));

                            string DT = System.DateTime.Now.ToString("dd-MM-yyyy");
                            detail.MeterReadingDate = DateTime.ParseExact(DT, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                            MeterList.Add(detail);
                            // MeterList.Add(detail);

                            MeterAttachment attachment = new MeterAttachment();
                            attachment.Id = Guid.NewGuid();
                            MeterAttachmentList.Add(attachment);

                        }

                    }
                    else
                    {
                        if (Reqoutput.IT_RETURN != null)
                        {
                            if (Reqoutput.IT_RETURN.Length > 0)
                            {
                                meterReading.Result = Reqoutput.IT_RETURN[0].MESSAGE.ToString().Replace(" ", "_"); ;
                            }
                        }
                        //else
                        //{
                        //    meterReading.Result = "This facility is not available for CA number mentioned by you. In case you still want to submit your Meter Reading please write to us at helpdesk.mumbaielectricity@adani.com";
                        //}
                        // return Redirect("/submitmeter-error?er=data_na&msg=" + meterReading.Result);
                        return Redirect("/submitmeter-error?er=data_na&msg=" + meterReading.Result);
                        //meterReading.Result = "Unable to process the request, the CA number is not valid.";
                        //return this.View(meterReading);
                        //return Redirect("/submitmeter-error?er=data_na");
                    }


                    meterReading.MeterList = MeterList;
                    meterReading.MeterAttachments = MeterAttachmentList;
                }
                catch (Exception ex)
                {
                    meterReading.Result = ex.Message;
                    Log.Error(" my account submit meter reading " + ex.Message, this);
                    // return this.View(meterReading);
                    return Redirect("/submitmeter-error?er=ca_na&msg=");
                }
            }


            return this.View(meterReading);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyAccountSubmitMeterReading(SubmitMeterReading m)
        {

            try
            {
                ElectricitySelfMeterReadingDataContext rdb = new ElectricitySelfMeterReadingDataContext();

                List<SelfMeterReading> SelfMeterReadingList = new List<SelfMeterReading>();

                int index = 0;
                m.Result = "";
                m.IsSubmitted = false;

                foreach (MeterReadingDetail meter in m.MeterList)
                {
                    if (m.MobileNumber.Length != 10)
                    {
                        m.Result = "Please enter 10 digit mobile number.";
                        return this.View(m);
                    }
                    // DateTime meterReadingDate = new DateTime();
                    /* DateTime pastDate = DateTime.Now.AddDays(-4);
                     pastDate = Convert.ToDateTime(pastDate.ToString("dd-MM-yyyy"));

                     DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy"));


                     if (!(meter.MeterReadingDate <= dateNow && meter.MeterReadingDate >= pastDate))
                     {
                         m.Result = "Date selected is beyond Meter Reading period.";
                         return this.View(m);
                     }*/
                    DateTime pastDate = DateTime.Now.AddDays(-4);
                    string pDT = pastDate.ToString("dd-MM-yyyy");
                    // pastDate = Convert.ToDateTime(pastDate.ToString("dd-MM-yyyy"));
                    pastDate = DateTime.ParseExact(pDT, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    DateTime TodayDate = DateTime.Now;
                    string tDT = TodayDate.ToString("dd-MM-yyyy");

                    //  DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy"));
                    DateTime dateNow = DateTime.ParseExact(tDT, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    string mtrDT = meter.MeterReadingDate.ToString("dd-MM-yyyy");
                    DateTime meterDT = DateTime.ParseExact(mtrDT, "dd-MM-yyyy", CultureInfo.InvariantCulture);


                    if (!(meterDT <= dateNow && meterDT >= pastDate))
                    {
                        m.Result = "Date selected is beyond Meter Reading period.";
                        return this.View(m);
                    }
                }

                foreach (MeterReadingDetail meter in m.MeterList)
                {
                    SelfMeterReading r1 = new SelfMeterReading();

                    foreach (HttpPostedFileBase file in meter.File)
                    {
                        if (file != null)
                        {
                            if (CheckExtension(file))
                            {

                                Stream fs = file.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                byte[] compressimage = CompressByImageAlg(10, bytes);

                                //saveMeterImgFile(compressimage, "", file.FileName.ToString());

                                // Convert.ToBase64String(compressimage);



                                /*
                                                                var updateStrm = new MemoryStream();
                                                                Stream strm = file.InputStream;
                                                                decimal size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);
                                                                byte[] bytes2 = null;
                                                                byte[] data = new byte[0];
                                                                byte[] bytes = new byte[0];
                                                                var scaleFactor = 0.5;
                                                                if (size > 100 )
                                                                {
                                                                    while (size > 100)
                                                                    {

                                                                        //  System.Drawing.Image sourceimage = System.Drawing.Image.FromStream(file.InputStream);
                                                                        // System.Drawing.Image sourceimage1 = ReduceImg(sourceimage, scaleFactor);
                                                                        updateStrm = new MemoryStream();
                                                                        var image = System.Drawing.Image.FromStream(file.InputStream);

                                                                            var newWidth = (int)(image.Width * scaleFactor);
                                                                            var newHeight = (int)(image.Height * scaleFactor);
                                                                            var thumbnailImg = new Bitmap(newWidth, newHeight);
                                                                            var thumbGraph = Graphics.FromImage(thumbnailImg);
                                                                            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                                                                            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                                                                            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                                            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                                                                            thumbGraph.DrawImage(image, imageRectangle);




                                                                          using (MemoryStream ms = new MemoryStream())
                                                                      {
                                                                            thumbnailImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                                                          ms.Seek(0, 0);
                                                                          data = ms.ToArray();
                                                                          strm = ms;

                                                                          size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                                                                          using (BinaryReader br = new BinaryReader(strm))
                                                                          {
                                                                              bytes2 = br.ReadBytes(file.ContentLength);
                                                                          }
                                                                          scaleFactor = scaleFactor - .1;
                                                                      }


                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    using (BinaryReader br = new BinaryReader(strm))
                                                                    {
                                                                        bytes2 = br.ReadBytes(file.ContentLength);
                                                                    }
                                                                }*/


                                r1.ContentType = file.ContentType;
                                r1.DocData = compressimage;
                                r1.FileName = file.FileName;

                                m.MeterAttachments[index].FileByte = compressimage;
                                m.MeterAttachments[index].FileCT = file.ContentType;
                                m.MeterAttachments[index].FileName = file.FileName;
                            }
                            else
                            {
                                m.Result = "Invalid image file.";
                                return this.View(m);
                            }
                        }
                    }




                    r1.AccountNumber = m.CANumber;
                    r1.Source = m.Source;
                    r1.MeterNumber = meter.MeterNumber;
                    r1.MeterReading = meter.MeterReading;
                    r1.MeterReadingDate = meter.MeterReadingDate;
                    r1.MobileNumber = m.MobileNumber;
                    r1.Created_Date = System.DateTime.Now;
                    rdb.SelfMeterReadings.InsertOnSubmit(r1);
                    index = index + 1;
                    SelfMeterReadingList.Add(r1);
                }
                rdb.SubmitChanges();
                //Session["MeterReadingObj"] = m;

                foreach (SelfMeterReading meter in SelfMeterReadingList)
                {

                    MeterReadingDetail mtr = m.MeterList.Where(x => x.MeterNumber == meter.MeterNumber).FirstOrDefault();


                    //var yy = new DateFormat
                    //DateTime dt = Convert.ToDateTime(meter.MeterReadingDate);

                    DateTime meterDT = Convert.ToDateTime(meter.MeterReadingDate);
                    //DateTime dt = Convert.ToDateTime(meterDT.ToString("dd-MM-yyyy"));
                    DateTime dt = DateTime.ParseExact(meterDT.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string meterReadingDate = dt.Year + "" + dt.Month.ToString("d2") + "" + dt.Day.ToString("d2");
                    DateTime dtSMRD = Convert.ToDateTime(mtr.SMRD);
                    string SMRD = dtSMRD.Year + "" + dtSMRD.Month.ToString("d2") + "" + dtSMRD.Day.ToString("d2");


                    ZBAPI_DM_T_GETMETERNUMBER_OUT[] returnDataObj = SapPiService.Services.RequestHandler.SelfMeterReadingDataUpload(meter.AccountNumber, meter.MeterNumber, meter.Source, meterReadingDate, meter.MeterReading, SMRD, meter.MobileNumber);

                    if (returnDataObj != null)
                    {
                        if (returnDataObj.Length > 0)
                        {
                            if (!(returnDataObj[0].AVAILABILITY.ToString().ToLower().Equals("y")))
                            {
                                //m.Result = "Meter Number : " + meter.MeterNumber + "  error in uploading data.";
                                m.Result = "Our team is trying hard to resolve technical issues.";
                                return this.View(m);
                            }
                            else
                            {

                                byte[] docData = meter.DocData.ToArray();

                                //  string image = Convert.ToBase64String(docData, 0, docData.Length);
                                string image = Convert.ToBase64String(docData);

                                ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[] returnImgObj = SapPiService.Services.RequestHandler.SelfMeterReadingImageUpload(image, meter.FileName, meter.MeterNumber, "", "319", "");

                                if (returnImgObj != null)
                                {
                                    if (returnImgObj.Length > 0)
                                    {
                                        if (!returnImgObj[0].TYPE.ToString().ToLower().Equals("s"))
                                        {
                                            // m.Result = "Meter Number : " + meter.MeterNumber + "  Error : " + returnImgObj[0].MESSAGE;
                                            Log.Error(" my account submit meter reading " + returnImgObj[0].MESSAGE, this);
                                            m.Result = "Our team is trying hard to resolve technical issues.";
                                            return this.View(m);
                                        }
                                    }
                                }
                                else
                                {
                                    // m.Result = "Meter Number : " + meter.MeterNumber + "  error in uploading image.";
                                    m.Result = "Our team is trying hard to resolve technical issues.";
                                    return this.View(m);
                                }
                            }
                        }
                    }
                    else
                    {
                        //m.Result = "Meter Number : " + meter.MeterNumber + "  error in uploading data.";
                        m.Result = "Our team is trying hard to resolve technical issues.";
                        return this.View(m);
                    }

                }


                //  m.IsSubmitted = true;
                // Session["MeterReadingObj"] = m;
            }
            catch (Exception ex)
            {
                // result = new { status = "0" };
                Log.Error(" my account submit meter reading " + ex.Message, this);
                //m.Result = ex.Message.ToString();
                m.Result = "Our team is trying hard to resolve technical issues.";
                Console.WriteLine(ex);
                return this.View(m);
            }
            return Redirect("/submitmeter-thankyou");
            // return this.View(m);
        }

        [HttpGet]
        public ActionResult SubmitMeterReading()
        {

            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
            string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.SelfMeterReadingSettings.EncryptionKey].Value;
            //staging  "B3XbAcGCezTeqfVxWIl4tvNdI";
            //Production "PdSgVkYp3s6v9yBEHMbQeThWm"



            SubmitMeterReading meterReading = new SubmitMeterReading();
            if (Request.QueryString["ca_number"] != null)
            {
                if (Request.QueryString["source"] != null)
                {
                    meterReading.Source = Request.QueryString["source"].ToString();
                }
                var CANumber = Request.QueryString["ca_number"].ToString();

                clsTripleDES objclsTripleDES = new clsTripleDES();

                //string test = objclsTripleDES.Encrypt("102543012", EncryptionKey);
                //string test1 = HttpUtility.UrlEncode(objclsTripleDES.Encrypt("102543012", EncryptionKey));
                //string test2 = objclsTripleDES.Decrypt(test, EncryptionKey);
                //string test3 = objclsTripleDES.Decrypt(test1, EncryptionKey);
                //string test4 = HttpUtility.UrlEncode(objclsTripleDES.Encrypt(test, EncryptionKey));
                //string test5 = HttpUtility.UrlEncode(objclsTripleDES.Encrypt(test1, EncryptionKey));

                /*
                                meterReading.CANumber = HttpUtility.UrlEncode(objclsTripleDES.Encrypt(CANumber, (DictionaryPhraseRepository.Current.Get("/Accounts/Settings/EncryptionKeyForBill/EncryptionKeyForBill", "B3XbAcGCezTeqfVxWIl4tvNdI"))));
                                */
                try
                {
                    meterReading.CANumber = HttpUtility.UrlEncode(objclsTripleDES.Decrypt(CANumber, EncryptionKey));


                    //  ZBAPI_DM_GETMETERNUMBERService.ZBAPI_DM_T_GETMETERNUMBER_OUT[] it_Output = new ZBAPI_DM_GETMETERNUMBERService.ZBAPI_DM_T_GETMETERNUMBER_OUT[100];


                    var Reqoutput = SapPiService.Services.RequestHandler.ReadMeterNumberForSelfMeterReading(meterReading.CANumber, "", meterReading.Source);



                    List<MeterReadingDetail> MeterList = new List<MeterReadingDetail>();
                    List<MeterAttachment> MeterAttachmentList = new List<MeterAttachment>();

                    if (Reqoutput.IT_OUTPUT.Length > 0)
                    {
                        foreach (ZBAPI_DM_GETMETERNUMBERService.ZBAPI_DM_T_GETMETERNUMBER_OUT output in Reqoutput.IT_OUTPUT)
                        {
                            MeterReadingDetail detail = new MeterReadingDetail();
                            detail.MeterNumber = output.METER_NUMBER;
                            detail.SMRD = output.SMRD;
                            string DT = System.DateTime.Now.ToString("dd-MM-yyyy");
                            detail.MeterReadingDate = DateTime.ParseExact(DT, "dd-MM-yyyy", CultureInfo.InvariantCulture);// Convert.ToDateTime(System.DateTime.Now).ToString("dd-MM-yyyy");

                            MeterList.Add(detail);
                            // MeterList.Add(detail);

                            MeterAttachment attachment = new MeterAttachment();
                            attachment.Id = Guid.NewGuid();
                            MeterAttachmentList.Add(attachment);

                        }

                    }
                    else
                    {


                        if (Reqoutput.IT_RETURN != null)
                        {
                            if (Reqoutput.IT_RETURN.Length > 0)
                            {
                                meterReading.Result = Reqoutput.IT_RETURN[0].MESSAGE.ToString().Replace(" ", "_");
                            }
                        }
                        /* else
                         {
                             meterReading.Result = "This facility is not available for CA number mentioned by you. In case you still want to submit your Meter Reading please write to us at helpdesk.mumbaielectricity@adani.com";
                         }*/
                        return Redirect("/submitmeter-error?er=data_na&msg=" + meterReading.Result);


                    }


                    meterReading.MeterList = MeterList;
                    meterReading.MeterAttachments = MeterAttachmentList;
                }
                catch (Exception ex)
                {
                    Log.Error("SubmitMeterReading : " + ex.Message, this);
                    meterReading.Result = "Facility is not available for mentioned CA Number.";
                    // return this.View(meterReading);
                    return Redirect("/submitmeter-error?er=ca_na&msg=");
                }
            }

            return this.View(meterReading);
        }

        public static Stream ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream;
            }
        }

        private Stream GenerateThumbnails(double scaleFactor, Stream sourcePath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);

                return ImageToByte2(thumbnailImg);

            }
        }

        public static Image ReduceImg(Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                return null; // throw new ArgumentOutOfRangeException("Quality is between 0 and 100");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            using (var stream = new MemoryStream())
            {
                img.Save(stream, jpegCodec, encoderParams);
                var b = new Bitmap(stream);
                return new Bitmap(b);
            }
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }

        static byte[] CompressByImageAlg(int jpegQuality, byte[] data)
        {
            using (MemoryStream inputStream = new MemoryStream(data))
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(inputStream))
                {

                    System.Drawing.Imaging.ImageCodecInfo jpegEncoder = ImageCodecInfo.GetImageDecoders()
                        .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, jpegQuality);
                    //  byte[] outputBytes = null;
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        image.Save(outputStream, jpegEncoder, encoderParameters);
                        return outputStream.ToArray();
                    }
                }
            }
        }

        private void saveMeterImgFile(byte[] byteArray, string fileextension, string filename)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/AEMLSelfMeterReading"));
            foreach (FileInfo file in di.GetFiles())
            {

                file.Delete();
                Console.Write("file delete " + file.FullName.ToString() + "\n");
            }

            using (FileStream fs = new FileStream(@"D:\\Compressfile\\test.jpg", FileMode.CreateNew, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
                fs.Flush();
                fs.Close();
            }
        }

        [HttpPost]
        //[ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitMeterReading(SubmitMeterReading m)
        {
            try
            {
                ElectricitySelfMeterReadingDataContext rdb = new ElectricitySelfMeterReadingDataContext();

                List<SelfMeterReading> SelfMeterReadingList = new List<SelfMeterReading>();

                int index = 0;
                m.Result = "";
                m.IsSubmitted = false;




                foreach (MeterReadingDetail meter in m.MeterList)
                {
                    if (m.MobileNumber.Length != 10)
                    {
                        m.Result = "Please enter 10 digit mobile number.";
                        return this.View(m);
                    }
                    // DateTime meterReadingDate = new DateTime();

                    DateTime pastDate = DateTime.Now.AddDays(-4);
                    string pDT = pastDate.ToString("dd-MM-yyyy");
                    // pastDate = Convert.ToDateTime(pastDate.ToString("dd-MM-yyyy"));
                    pastDate = DateTime.ParseExact(pDT, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    DateTime TodayDate = DateTime.Now;
                    string tDT = TodayDate.ToString("dd-MM-yyyy");

                    //  DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy"));
                    DateTime dateNow = DateTime.ParseExact(tDT, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    string mtrDT = meter.MeterReadingDate.ToString("dd-MM-yyyy");
                    DateTime meterDT = DateTime.ParseExact(mtrDT, "dd-MM-yyyy", CultureInfo.InvariantCulture);


                    if (!(meterDT <= dateNow && meterDT >= pastDate))
                    {
                        m.Result = "Date selected is beyond Meter Reading period.";
                        return this.View(m);
                    }
                }




                foreach (MeterReadingDetail meter in m.MeterList)
                {


                    SelfMeterReading r1 = new SelfMeterReading();

                    foreach (HttpPostedFileBase file in meter.File)
                    {
                        if (file != null)
                        {
                            if (CheckExtension(file))
                            {

                                Stream fs = file.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                byte[] compressimage = CompressByImageAlg(10, bytes);

                                //saveMeterImgFile(compressimage, "", file.FileName.ToString());

                                // Convert.ToBase64String(compressimage);



                                /*
                                                                var updateStrm = new MemoryStream();
                                                                Stream strm = file.InputStream;
                                                                decimal size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);
                                                                byte[] bytes2 = null;
                                                                byte[] data = new byte[0];
                                                                byte[] bytes = new byte[0];
                                                                var scaleFactor = 0.5;
                                                                if (size > 100 )
                                                                {
                                                                    while (size > 100)
                                                                    {

                                                                        //  System.Drawing.Image sourceimage = System.Drawing.Image.FromStream(file.InputStream);
                                                                        // System.Drawing.Image sourceimage1 = ReduceImg(sourceimage, scaleFactor);
                                                                        updateStrm = new MemoryStream();
                                                                        var image = System.Drawing.Image.FromStream(file.InputStream);

                                                                            var newWidth = (int)(image.Width * scaleFactor);
                                                                            var newHeight = (int)(image.Height * scaleFactor);
                                                                            var thumbnailImg = new Bitmap(newWidth, newHeight);
                                                                            var thumbGraph = Graphics.FromImage(thumbnailImg);
                                                                            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                                                                            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                                                                            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                                            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                                                                            thumbGraph.DrawImage(image, imageRectangle);




                                                                          using (MemoryStream ms = new MemoryStream())
                                                                      {
                                                                            thumbnailImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                                                          ms.Seek(0, 0);
                                                                          data = ms.ToArray();
                                                                          strm = ms;

                                                                          size = Math.Round(((decimal)strm.Length / (decimal)1024), 2);

                                                                          using (BinaryReader br = new BinaryReader(strm))
                                                                          {
                                                                              bytes2 = br.ReadBytes(file.ContentLength);
                                                                          }
                                                                          scaleFactor = scaleFactor - .1;
                                                                      }


                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    using (BinaryReader br = new BinaryReader(strm))
                                                                    {
                                                                        bytes2 = br.ReadBytes(file.ContentLength);
                                                                    }
                                                                }*/


                                r1.ContentType = file.ContentType;
                                r1.DocData = compressimage;
                                r1.FileName = file.FileName;

                                m.MeterAttachments[index].FileByte = compressimage;
                                m.MeterAttachments[index].FileCT = file.ContentType;
                                m.MeterAttachments[index].FileName = file.FileName;
                            }
                            else
                            {
                                m.Result = "Invalid image file.";
                                return this.View(m);
                            }
                        }
                    }




                    r1.AccountNumber = m.CANumber;
                    r1.Source = m.Source;
                    r1.MeterNumber = meter.MeterNumber;
                    r1.MeterReading = meter.MeterReading;
                    r1.MeterReadingDate = meter.MeterReadingDate;
                    r1.MobileNumber = m.MobileNumber;
                    r1.Created_Date = System.DateTime.Now;
                    rdb.SelfMeterReadings.InsertOnSubmit(r1);
                    index = index + 1;
                    SelfMeterReadingList.Add(r1);
                }
                rdb.SubmitChanges();
                //Session["MeterReadingObj"] = m;

                foreach (SelfMeterReading meter in SelfMeterReadingList)
                {

                    MeterReadingDetail mtr = m.MeterList.Where(x => x.MeterNumber == meter.MeterNumber).FirstOrDefault();


                    //var yy = new DateFormat
                    DateTime meterDT = Convert.ToDateTime(meter.MeterReadingDate);
                    DateTime dt = DateTime.ParseExact(meterDT.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    // Convert.ToDateTime(meterDT.ToString("dd-MM-yyyy"));
                    string meterReadingDate = dt.Year + "" + dt.Month.ToString("d2") + "" + dt.Day.ToString("d2");
                    DateTime dtSMRD = Convert.ToDateTime(mtr.SMRD);
                    string SMRD = dtSMRD.Year + "" + dtSMRD.Month.ToString("d2") + "" + dtSMRD.Day.ToString("d2");


                    ZBAPI_DM_T_GETMETERNUMBER_OUT[] returnDataObj = SapPiService.Services.RequestHandler.SelfMeterReadingDataUpload(meter.AccountNumber, meter.MeterNumber, meter.Source, meterReadingDate, meter.MeterReading, SMRD, meter.MobileNumber);

                    if (returnDataObj != null)
                    {
                        if (returnDataObj.Length > 0)
                        {
                            if (!(returnDataObj[0].AVAILABILITY.ToString().ToLower().Equals("y")))
                            {
                                m.Result = "Our team is trying hard to resolve technical issues.";
                                return this.View(m);
                            }
                            else
                            {

                                byte[] docData = meter.DocData.ToArray();

                                //  string image = Convert.ToBase64String(docData, 0, docData.Length);
                                string image = Convert.ToBase64String(docData);

                                ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[] returnImgObj = SapPiService.Services.RequestHandler.SelfMeterReadingImageUpload(image, meter.FileName, meter.MeterNumber, "", "319", "");

                                if (returnImgObj != null)
                                {
                                    if (returnImgObj.Length > 0)
                                    {
                                        if (!returnImgObj[0].TYPE.ToString().ToLower().Equals("s"))
                                        {
                                            // m.Result = "Meter Number : " + meter.MeterNumber + "  Error : " + returnImgObj[0].MESSAGE;
                                            m.Result = "Our team is trying hard to resolve technical issues.";
                                            Log.Error("Meter Number : " + meter.MeterNumber + "  Error : " + returnImgObj[0].MESSAGE, this);
                                            return this.View(m);
                                        }
                                    }
                                }
                                else
                                {
                                    m.Result = "Our team is trying hard to resolve technical issues.";
                                    return this.View(m);
                                }
                            }
                        }
                    }
                    else
                    {
                        m.Result = "Our team is trying hard to resolve technical issues.";
                        return this.View(m);
                    }

                }


                //  m.IsSubmitted = true;
                // Session["MeterReadingObj"] = m;
            }
            catch (Exception ex)
            {
                // result = new { status = "0" };
                //m.Result = ex.Message.ToString(); 
                m.Result = "Our team is trying hard to resolve technical issues.";
                Console.WriteLine(ex);
                Log.Error("submit meter reading " + ex.Message, this);
                return this.View(m);
            }
            return Redirect("/submitmeter-thankyou");
            // return this.View(m);
        }



        public FileResult DownloadFile(Guid Id)
        {
            SubmitMeterReading meters = (SubmitMeterReading)Session["MeterReadingObj"];
            MeterAttachment selectedMeter = new MeterAttachment();
            List<MeterAttachment> attch = meters.MeterAttachments;
            foreach (MeterAttachment meter in attch)
            {
                if (meter.Id == Id)
                {
                    selectedMeter.FileName = meter.FileName;
                    selectedMeter.FileCT = meter.FileCT;
                    selectedMeter.FileByte = meter.FileByte;
                }
            }
            return File(((byte[])selectedMeter.FileByte).ToArray(), selectedMeter.FileCT.ToString(), selectedMeter.FileName.ToString());
        }

        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {

            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        private bool CheckExtension(HttpPostedFileBase postedFile)
        {
            if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var postedFileExtension = Path.GetExtension(postedFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            int ImageMinimumBytes = 10;
            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //   Check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.ContentLength < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.InputStream.Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFile.InputStream.Position = 0;
            }
            return true;

            ////'jpg', 'jpeg', 'dwg', 'pdf', 'doc', 'docx', 'xls', 'xlsx'
            //string fileExtension = System.IO.Path.GetExtension(fileName);
            //if (fileExtension == ".jpg" || fileExtension == ".JPG" ||
            //    fileExtension == ".png" || fileExtension == ".PNG" ||
            //    fileExtension == ".jpeg" || fileExtension == ".JPEG"
            //    )
            //    return true;
            //else
            //    return false;
        }


        [HttpGet]
        public ActionResult EMIProcess()
        {

            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
            string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.EMISettings.EncryptionKey].Value;
            //staging  "B3XbAcGCezTeqfVxWIl4tvNdI";
            //Production "PdSgVkYp3s6v9yBEHMbQeThWm"

            EMIProcess emiProcessModel = new EMIProcess();
            if (Request.QueryString["ca_number"] != null)
            {
                if (Request.QueryString["source"] != null)
                {
                    emiProcessModel.Source = Request.QueryString["source"].ToString();
                }
                var CANumber = Request.QueryString["ca_number"].ToString();

                clsTripleDES objclsTripleDES = new clsTripleDES();

                try
                {
                    emiProcessModel.CANumber = HttpUtility.UrlEncode(objclsTripleDES.Decrypt(CANumber, EncryptionKey));
                    Session["EMICA"] = emiProcessModel.CANumber;
                    var emiDetails = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(emiProcessModel.CANumber, "E", emiProcessModel.Source);
                    if (emiDetails.Status == "S")
                    {
                        emiProcessModel.OutstandingAmount = emiDetails.TotalOutstanding;
                        emiProcessModel.ConsumerName = emiDetails.ConsumerName;
                        emiProcessModel.EMIInstallmentAmount = emiDetails.EMIInstallmentAmount;
                        emiProcessModel.IsEMIEligible = true;
                    }
                    else
                    {
                        emiProcessModel.Result = emiDetails.Remarks;
                        emiProcessModel.IsEMIEligible = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("EMI Process exception for: " + emiProcessModel.CANumber + ", Error:" + ex.Message, this);
                    emiProcessModel.IsEMIEligible = false;
                    emiProcessModel.Result = "Facility is not available for mentioned CA Number.";
                }
            }

            return this.View(emiProcessModel);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult EMIProcess(EMIProcess model)
        {
            if (model.ProceedWithEMI)
            {
                if (Session["EMICA"] != null && Session["EMICA"].ToString() == model.CANumber)
                {
                    var emiDetailsUpdate = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(model.CANumber, "U", model.Source);
                    if (emiDetailsUpdate.Status == "S")
                    {
                        model.IsEMIEligible = false; //to display message
                        model.Result = emiDetailsUpdate.Remarks;
                    }
                    else
                    {
                        model.IsEMIEligible = false;
                        model.Result = emiDetailsUpdate.Remarks;
                    }
                }
                else
                {
                    model.IsEMIEligible = false;
                    model.Result = "Invalid request.";
                }
            }
            else
            {
                model.IsEMIEligible = false;
                model.Result = "Invalid request.";
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult SDInstallmentOptIn()
        {
            SDEMIProcess SDEMIProcessModel = new SDEMIProcess();
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.EMISettings.EncryptionKey].Value;
                //staging  "B3XbAcGCezTeqfVxWIl4tvNdI";
                //Production "PdSgVkYp3s6v9yBEHMbQeThWm"

                if (Request.QueryString["ca_number"] != null)
                {
                    if (Request.QueryString["source"] != null)
                    {
                        SDEMIProcessModel.Source = Request.QueryString["source"].ToString();
                    }
                    var CANumber = Request.QueryString["ca_number"].ToString();

                    clsTripleDES objclsTripleDES = new clsTripleDES();
                    string caNumber = objclsTripleDES.Encrypt("100517857", EncryptionKey);

                    SDEMIProcessModel.CANumber = objclsTripleDES.Decrypt(CANumber, EncryptionKey);
                    Session["SDInstOptModel"] = SDEMIProcessModel;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.Source);
            }
            return this.View(SDEMIProcessModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateRenderingId]
        public ActionResult SDInstallmentOptIn(SDEMIProcess model, string ValidateCA = null, string SendOTP = null, string ValidateOTP = null, string Submit = null)
        {
            if (!string.IsNullOrEmpty(ValidateCA))
            {
                if (string.IsNullOrEmpty(model.CANumber))
                {
                    this.ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number required", "Please enter valid Account Number."));
                    return this.View(model);
                }
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }
                var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                if (string.IsNullOrEmpty(consumerDetails.AccountNumber))
                {
                    ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Account Number incorrect", "Account Number is invalid."));
                    return this.View(model);
                }
                else
                {
                    string checkCA = ChangeOfNameService.ValidateCAForSDOptIn(consumerDetails);
                    if (!string.IsNullOrEmpty(checkCA))
                    {
                        this.ModelState.AddModelError(nameof(model.CANumber), checkCA);
                        return this.View(model);
                    }

                    //Validate SD
                    var sdDetails = SapPiService.Services.RequestHandler.FetchSecurityDepositAmountDetails(model.CANumber);

                    ////for testing
                    //sdDetails.Message = "";
                    //sdDetails.IsSuccess = true;

                    if (!sdDetails.IsSuccess || (sdDetails.IsSuccess && !string.IsNullOrEmpty(sdDetails.Message)))
                    {
                        ModelState.AddModelError(nameof(model.CANumber), sdDetails.Message);
                        return this.View(model);
                    }

                    //Get Mobile number
                    var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);
                    if (string.IsNullOrEmpty(registeredMobileNumber))
                    {
                        ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Mobile Number incorrect", "Mobile Number is not registered."));
                        return this.View(model);
                    }
                    model.MobileNumber = string.IsNullOrEmpty(consumerDetails.MOBILE_NO) ? consumerDetails.MOBILE_NO : consumerDetails.MOBILE_NO.Substring(0, 2) + "xxxxxxx" + consumerDetails.MOBILE_NO.Substring(consumerDetails.MOBILE_NO.Length - 3);
                    model.IsvalidatAccount = true;

                    Session["SDInstOptModel"] = model;
                }
            }
            if (!string.IsNullOrEmpty(SendOTP))
            {
                //send OTP to registered mobile number
                RegistrationRepository registrationRepo = new RegistrationRepository();
                SDEMIProcess accountNumberValidated = (SDEMIProcess)Session["SDInstOptModel"];

                if (model.CANumber == accountNumberValidated.CANumber)
                {
                    //send SMS for OTP
                    if (!registrationRepo.CheckForCAMaxLimit(model.CANumber, "SDEMI"))
                    {
                        Log.Info("SDInstallmentOptIn: Number of attempt to get OTP reached for AccountNumber." + model.CANumber, this);
                        this.ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/Max20OTPPerLECMobile", "Number of attempt to get OTP reached for Entered value."));
                        return this.View(model);
                    }

                    var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);
                    string generatedotp = registrationRepo.GenerateOTPRegistration(model.CANumber, null, "SDEMI", registeredMobileNumber);
                    //send otp via SMS
                    #region Api call to send SMS of OTP
                    try
                    {
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/SDInstallmentOptIn//OTP API URL",
                           "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Welcome to Adani Electricity. You have initiated an SD installment Opt-In request for Account no {1}. OTP for validation is {2}&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707162046256235660"), registeredMobileNumber, model.CANumber, generatedotp);

                        //var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/PANGSTUpdate/PAN Update/OTP API URL",
                        //    "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Welcome to Adani Electricity. You have initiated a request for PAN/GST update for Account no. {1}. OTP for validation is {2}.&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707161053621273935"), registeredMobileNumber, model.CANumber, generatedotp);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("SDInstallmentOptIn: OTP Api call success for LEC registration", this);
                            model.IsvalidatAccount = true;
                            model.IsOTPSend = true;
                            Session["SDInstOptModel"] = model;
                            return this.View(model);
                        }
                        else
                        {
                            Log.Error("SDInstallmentOptIn OTP Api call failed for registration", this);
                            this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/OTP sending error", "Unable to send OTP."));
                            model.IsvalidatAccount = true;
                            model.IsOTPSend = false;
                            return this.View(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("SDInstallmentOptIn: OTP Api call failed for registration: " + ex.Message, this);
                        this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/OTP sending error", "Unable to send OTP."));
                        model.IsvalidatAccount = true;
                        model.IsOTPSend = false;
                        return this.View(model);
                    }
                    #endregion
                }
                else
                {
                    ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Account Number incorrect", "Account Number is invalid."));
                    model.IsvalidatAccount = false;
                    return this.View(model);
                }
            }
            if (!string.IsNullOrEmpty(ValidateOTP))
            {
                if (Session["SDInstOptModel"] == null)
                {
                    this.ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/SDInstOptModel/Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }

                SDEMIProcess accountNumberValidated = (SDEMIProcess)Session["SDInstOptModel"];
                if (model.CANumber == accountNumberValidated.CANumber)
                {
                    if (string.IsNullOrEmpty(model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/SDInstOptModel/Registration/Enter OTP", "Enter OTP."));
                        model.IsvalidatAccount = true;
                        model.IsOTPSend = true;
                        return this.View(model);
                    }

                    var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);

                    RegistrationRepository registrationRepo = new RegistrationRepository();
                    string generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(registeredMobileNumber, model.CANumber, "SDEMI");

                    if (!string.Equals(generatedOTP, model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                        model.IsvalidatAccount = true;
                        model.IsOTPSend = true;
                        model.IsvalidatOTP = false;
                        return this.View(model);
                    }

                    var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                    model.ConsumerName = consumerDetails.NAME_CustomerName;

                    var sdDetails = SapPiService.Services.RequestHandler.FetchSecurityDepositAmountDetails(model.CANumber);
                    model.SecurityDepositAmount = Convert.ToDecimal(sdDetails.SDAmount);

                    model.IsvalidatAccount = true;
                    model.IsOTPSend = true;
                    model.IsvalidatOTP = true;
                    Session["SDInstOptModel"] = model;
                }
            }
            if (!string.IsNullOrEmpty(Submit))
            {
                SDEMIProcess accountNumberValidated = (SDEMIProcess)Session["SDInstOptModel"];
                if (model.CANumber == accountNumberValidated.CANumber)
                {
                    Log.Info("SDInstallmentOptIn submit installment SAP call started for CA:" + model.CANumber, this);
                    Log.Info("SDInstallmentOptIn submit installment SAP call started for CA and details" + model.CANumber + "," + model.SelectedNumberOfInstalments + "," + model.SecurityDepositAmount + "," + model.Source, this);

                    var resultMessage = SapPiService.Services.RequestHandler.SDInstallmentOptInPost(model.CANumber, model.SelectedNumberOfInstalments, model.Source);
                    Log.Info("SDInstallmentOptIn submit installment SAP call done for CA:" + model.CANumber + "," + resultMessage.Message + "," + resultMessage.Number + "," + resultMessage.IsSuccess, this);

                    try
                    {
                        var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);

                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            SecurityDepositInstallmentOptIn objSecurityDepositInstallmentOptIn = new SecurityDepositInstallmentOptIn
                            {
                                AccountNumber = model.CANumber,
                                ConsumerName = model.ConsumerName,
                                CreatedDate = DateTime.Now,
                                MobileNumber = consumerDetails.MOBILE_NO,
                                NumberOfInstallments = Convert.ToInt32(model.SelectedNumberOfInstalments),
                                Source = model.Source,
                                SDAmount = model.SecurityDepositAmount.ToString(),
                                IsPostToSAP = resultMessage.IsSuccess,
                                SAPPostMessage = resultMessage.Number + "-" + resultMessage.Message,
                            };
                            dbcontext.SecurityDepositInstallmentOptIns.InsertOnSubmit(objSecurityDepositInstallmentOptIn);
                            dbcontext.SubmitChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("SDInstallmentOptIn error in db save: " + e.Message, this);
                    }

                    if (!resultMessage.IsSuccess)
                    {
                        this.ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/SDInstOptModel/Registration/Service error", "An error occured, please try again!"));
                        return this.View(model);
                    }
                    else
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(resultMessage.Message);
                        ViewBag.Message = resultMessage.Message;
                        Session["Message"] = resultMessage.Message;
                        return this.Redirect(this.Request.RawUrl);
                    }
                }
            }
            return this.View(model);
        }

        #region city bank

        [HttpPost]
        public ActionResult City_Callback()
        {
            CityNotificationModel model = new CityNotificationModel();
            PushNotificationToSSGModel request = new PushNotificationToSSGModel();
            string datastring = "";
            try
            {
                StreamReader bodyStream = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);

                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                datastring = bodyStream.ReadToEnd();

                model = JsonConvert.DeserializeObject<CityNotificationModel>(datastring);
                request = model.PushNotificationToSSG;
                request.ResponseMessage = datastring;

                Sitecore.Diagnostics.Log.Info("Method - City_Callback request message :" + datastring, this);
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    CityPayment obj = new CityPayment
                    {
                        NPCITxnId = request.NPCITxnId,
                        OrderNo = request.OrderNo,
                        RespCode = request.RespCode,
                        SettlementAmount = request.SettlementAmount,
                        SettlementCurrency = request.SettlementCurrency,
                        StatusCode = request.StatusCode,
                        StatusDesc = request.StatusDesc,
                        TimeStamp = request.TimeStamp,
                        TranAuthDate = request.TranAuthDate,
                        TxnRefNo = request.TxnRefNo,
                        Created_Date = DateTime.Now,
                        Response_Msg = datastring
                    };
                    dbcontext.CityPayments.InsertOnSubmit(obj);
                    dbcontext.SubmitChanges();

                    Accounts.PaymentHistory ctx = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == request.OrderNo).FirstOrDefault();

                    if (ctx != null)
                    {
                        Sitecore.Diagnostics.Log.Info("Method - City_Callback record found with order id :" + request.OrderNo, this);
                        request.AccountNumber = ctx.AccountNumber;
                        request.PaymentType = ctx.PaymentType;
                        request.PaymentMode = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/City/Payment Mode", "UPI");
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Info("City_Callback Method Called - StorePaymentRequestCity called record not exists " + request.TxnRefNo, this);
                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = false, Message = "Record not exists" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    if (PaymentService.StorePaymentRequestCity(request))
                    {
                        Sitecore.Diagnostics.Log.Info("City_Callback Method Called - StorePaymentRequestCity called Successfully " + request.TxnRefNo, this);

                        ViewPayBill modelviewpay = new ViewPayBill()
                        {
                            TransactionId = request.TxnRefNo.ToString(),
                            ResponseStatus = Constants.PaymentResponse.Success,
                            Responsecode = request.RespCode.ToString() ?? string.Empty,
                            Remark = Constants.PaymentResponse.Success,
                            PaymentRef = request.NPCITxnId.ToString(),
                            OrderId = request.OrderNo.ToString(),
                            AmountPayable = request.SettlementAmount.ToString(),
                            AccountNumber = request.AccountNumber.ToString(),
                            msg = Constants.PaymentResponse.Success,
                            PaymentMode = request.PaymentMode,
                            TransactionDate = request.TranAuthDate.ToString(),
                            PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                            PaymentGateway = (int)EnumPayment.GatewayType.CITYUPI
                        };
                        TempData["PaymentResponse"] = modelviewpay;
                        TempData.Keep();
                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Successfully Added." }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Info("City_Callback Method Called - StorePaymentRequestCity called Server Error " + request.TxnRefNo, this);
                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = false, Message = "Server Error" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at City_Callback :" + ex.Message + " Response: " + datastring, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public string City_Callback_Check()
        {
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailure);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccess);
            string PendingUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentPending);

            string id = string.Empty;
            id = SessionHelper.UserSession.UserSessionContext.OrderId.ToString();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        Accounts.PaymentHistory PaymentHistoryRecord = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == id && x.TransactionId != null).FirstOrDefault();
                        if (PaymentHistoryRecord != null)
                        {
                            ViewPayBill modelviewpay = new ViewPayBill()
                            {
                                TransactionId = PaymentHistoryRecord.TransactionId.ToString(),
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = PaymentHistoryRecord.Responsecode.ToString(),
                                Remark = PaymentHistoryRecord.Remark,
                                PaymentRef = PaymentHistoryRecord.PaymentRef.ToString(),
                                OrderId = PaymentHistoryRecord.OrderId.ToString(),
                                AmountPayable = PaymentHistoryRecord.Amount.ToString(),
                                AccountNumber = PaymentHistoryRecord.AccountNumber.ToString(),
                                msg = Constants.PaymentResponse.Success,
                                PaymentMode = PaymentHistoryRecord.PaymentMode.ToString(),
                                TransactionDate = PaymentHistoryRecord.Created_Date.ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                                PaymentGateway = (int)EnumPayment.GatewayType.CITYUPI
                            };
                            TempData["PaymentResponse"] = modelviewpay;
                            TempData.Keep();
                            if (PaymentHistoryRecord.Responsecode == "101")
                            {
                                return SuccessUrl;
                            }
                            else if (PaymentHistoryRecord.Responsecode == "00")
                            {
                                return PendingUrl;
                            }
                            else
                            {
                                return FailureUrl;
                            }
                        }
                        else
                        {
                            //call again
                        }
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at City payment IsSuccess:" + ex.Message, this);
                    return string.Empty;
                }
            }
            return "";
        }

        #endregion

        #region GreenPower Opt in 

        [HttpGet]
        public ActionResult GreenPowerOptIn()
        {
            GreenPowerOptInModel greenPowerOptInModel = new GreenPowerOptInModel();
            return this.View(greenPowerOptInModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateRenderingId]
        public ActionResult GreenPowerOptIn(GreenPowerOptInModel model, string ValidateCA = null, string SendOTP = null, string ValidateOTP = null, string Accept = null, string Submit = null, string CancelSubmit = null, string IPledge = null, string SubmitCapture = null, string UpdateMobileNumber = null, string UpdateMobileSendOtp = null, string UpdateMobileValidateOTP = null)
        {
            if (!string.IsNullOrEmpty(ValidateCA))
            {
                if (string.IsNullOrEmpty(model.CANumber))
                {
                    this.ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number required", "Please enter valid Account Number."));
                    return this.View(model);
                }
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }
                var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                if (string.IsNullOrEmpty(consumerDetails.AccountNumber))
                {
                    ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Account Number incorrect", "Account Number is invalid."));
                    return this.View(model);
                }
                else
                {
                    string checkCA = ChangeOfNameService.ValidateCAForGreenPower(consumerDetails);
                    if (!string.IsNullOrEmpty(checkCA))
                    {
                        this.ModelState.AddModelError(nameof(model.CANumber), checkCA);
                        return this.View(model);
                    }

                    GreenPowerOptInService objGreenPowerOptInService = new GreenPowerOptInService();
                    bool isOpted = objGreenPowerOptInService.IsAlreadyOptedForGreenPower(model.CANumber);
                    if (isOpted)
                    {
                        ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Already Opted", "You have already Opted for Green Power for the mentioned Account Number."));
                        return this.View(model);
                    }

                    //Get Mobile number
                    var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);
                    if (string.IsNullOrEmpty(registeredMobileNumber))
                    {
                        ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Mobile Number incorrect", "Mobile Number is not registered."));
                        return this.View(model);
                    }
                    model.MobileNumber = string.IsNullOrEmpty(consumerDetails.MOBILE_NO) ? consumerDetails.MOBILE_NO : consumerDetails.MOBILE_NO.Substring(0, 2) + "xxxxxxx" + consumerDetails.MOBILE_NO.Substring(consumerDetails.MOBILE_NO.Length - 3);
                    model.IsvalidatAccount = true;

                    Session["GreenPowerOptModel"] = model;
                    return this.View(model);
                }
            }
            if (!string.IsNullOrEmpty(UpdateMobileNumber))
            {
                model.MobileNumber = null;
                model.IsUpdateMobile = true;
                Session["GreenPowerOptModel"] = model;
                return this.View(model);
            }
            if (!string.IsNullOrEmpty(UpdateMobileSendOtp))
            {
                model.IsUpdateMobile = true;
                GreenPowerOptInModel accountNumberValidated = (GreenPowerOptInModel)Session["GreenPowerOptModel"];

                if (model.CANumber != accountNumberValidated.CANumber)
                {
                    this.ModelState.AddModelError(nameof(model.MeterNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Request is not valid", "Request is not valid."));
                    return this.View(model);
                }
                if (string.IsNullOrEmpty(model.MeterNumber))
                {
                    this.ModelState.AddModelError(nameof(model.MeterNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Meter number required", "Please enter valid Meter Number."));
                    return this.View(model);
                }
                if (string.IsNullOrEmpty(model.MobileNumber))
                {
                    this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/MobileNumber number required", "Please enter valid MobileNumber Number."));
                    return this.View(model);
                }
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }

                var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(model.CANumber);
                var data = billinghDetails.MeterNumbers;
                if (billinghDetails != null && billinghDetails.MeterNumbers.Length == 0)
                {
                    this.ModelState.AddModelError(nameof(model.MeterNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number invalid", "Account Number is not valid."));
                    return this.View(model);
                }
                else if (!billinghDetails.MeterNumbers.Contains(model.MeterNumber))
                {
                    this.ModelState.AddModelError(nameof(model.MeterNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Meter number invalid", "Meter Number is not associated with Account Number. Please enter another Meter Number."));
                    return this.View(model);
                }

                //Send OTP for update mobile
                RegistrationRepository registrationRepo = new RegistrationRepository();

                if (registrationRepo.CheckForCAMaxLimit(model.CANumber, "UpdateProfile") == false)
                {
                    Log.Info("Number of attempt to get OTP reached for Account Number." + model.CANumber, this);
                    this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/Registration/Max20OTPPerCA", "Number of attempt to get OTP reached for Account Number."));
                    model.IsUpdateMobile = true;
                    model.IsOTPSend = false;
                    return this.View(model);
                }
                if (registrationRepo.CheckForMobCAMaxLimit(model.CANumber, "UpdateProfile", model.MobileNumber) == false)
                {
                    Log.Info("Number of attempt to get OTP reached for Account Number and Mobile number." + model.CANumber + ", " + model.MobileNumber, this);
                    this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/Registration/Max20OTPPerCAMob", "Number of attempt to get OTP reached for Account Number and Mobile number."));
                    model.IsUpdateMobile = true;
                    model.IsOTPSend = false;

                    return this.View(model);
                }

                #region Generate New Otp for given mobile number and save to database
                string generatedotp = registrationRepo.GenerateOTPRegistration(model.CANumber, null, "UpdateProfile", model.MobileNumber);
                #endregion

                #region Api call to send SMS of OTP
                try
                {
                    var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/Accounts/Settings/SMS API for Profile update", "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=You have initiated a request to update mobile number for account no. {1}, OTP for this request is: {2}&intflag=false"), model.MobileNumber, model.CANumber, generatedotp);

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call success for registration", this);
                        this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTP", "OTP Sent."));
                        model.IsUpdateMobile = true;
                        model.IsOTPSend = true;
                        return this.View(model);
                    }
                    else
                    {
                        Log.Error("OTP Api call failed for registration", this);
                        this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                        model.IsUpdateMobile = true;
                        model.IsOTPSend = false;
                        return this.View(model);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{0}", ex, this);
                    Log.Error("OTP Api call failed for registration", this);
                    this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                    model.IsUpdateMobile = true;
                    model.IsOTPSend = false;
                    return this.View(model);
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(UpdateMobileValidateOTP))
            {
                model.IsUpdateMobile = true;

                GreenPowerOptInModel accountNumberValidated = (GreenPowerOptInModel)Session["GreenPowerOptModel"];

                if (model.CANumber != accountNumberValidated.CANumber)
                {
                    this.ModelState.AddModelError(nameof(model.MeterNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Request is not valid", "Request is not valid."));
                    return this.View(model);
                }
                //validate OTP and submit
                RegistrationRepository registrationRepo = new RegistrationRepository();
                string generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(model.MobileNumber, model.CANumber, "UpdateProfile");
                if (!string.Equals(generatedOTP, model.OTPNumber))
                {
                    this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                    model.IsOTPSend = true;
                    model.IsvalidatOTP = false;
                    return this.View(model);
                }
                else
                {
                    var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                    string result1 = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(model.CANumber, model.MobileNumber, "", consumerDetails.SMTP_ADDR_Email, null);

                    Log.Info("Green Power Contact details update:" + model.CANumber + "," + model.MobileNumber + "," + consumerDetails.SMTP_ADDR_Email, this);
                    if (result1 == "success")
                    {
                        model.ConsumerName = consumerDetails.NAME_CustomerName;
                        model.IsUpdateMobile = false;
                        model.IsvalidatAccount = true;
                        model.IsOTPSend = true;
                        model.IsvalidatOTP = true;
                        model.IsAccepted = false;

                        var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);
                        if (string.IsNullOrEmpty(registeredMobileNumber))
                        {
                            ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Mobile Number incorrect", "Mobile Number is not registered."));
                            return this.View(model);
                        }
                        model.MobileNumber = string.IsNullOrEmpty(consumerDetails.MOBILE_NO) ? consumerDetails.MOBILE_NO : consumerDetails.MOBILE_NO.Substring(0, 2) + "xxxxxxx" + consumerDetails.MOBILE_NO.Substring(consumerDetails.MOBILE_NO.Length - 3);

                        Session["GreenPowerOptModel"] = model;
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile number not updated", "Mobile number not updated, please try again."));
                        model.IsOTPSend = true;
                        model.IsvalidatOTP = false;
                        return this.View(model);
                    }
                }
                return this.View(model);
            }
            if (!string.IsNullOrEmpty(SendOTP))
            {
                //send OTP to registered mobile number
                RegistrationRepository registrationRepo = new RegistrationRepository();
                GreenPowerOptInModel accountNumberValidated = (GreenPowerOptInModel)Session["GreenPowerOptModel"];

                if (model.CANumber == accountNumberValidated.CANumber)
                {
                    //send SMS for OTP
                    if (!registrationRepo.CheckForCAMaxLimit(model.CANumber, "GreenPowerOptIn"))
                    {
                        Log.Info("SDInstallmentOptIn: Number of attempt to get OTP reached for AccountNumber." + model.CANumber, this);
                        this.ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/Max20OTPPerLECMobile", "Number of attempt to get OTP reached for Entered value."));
                        return this.View(model);
                    }

                    var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);
                    string generatedotp = registrationRepo.GenerateOTPRegistration(model.CANumber, null, "GreenPowerOptIn", registeredMobileNumber);
                    //send otp via SMS
                    #region Api call to send SMS of OTP
                    try
                    {
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/SDInstallmentOptIn//OTP API URL",
                            "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Dear Customer, OTP is {1} for your transaction for {2} . Adani Electricity.&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707162256666913751"), registeredMobileNumber, generatedotp, "Green Power Opt-In");

                        //var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/PANGSTUpdate/PAN Update/OTP API URL",
                        //    "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Welcome to Adani Electricity. You have initiated a request for PAN/GST update for Account no. {1}. OTP for validation is {2}.&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707161053621273935"), registeredMobileNumber, model.CANumber, generatedotp);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("SDInstallmentOptIn: OTP Api call success for LEC registration", this);
                            model.IsvalidatAccount = true;
                            model.IsOTPSend = true;
                            Session["GreenPowerOptModel"] = model;
                            return this.View(model);
                        }
                        else
                        {
                            Log.Error("GreenPowerOptIn OTP Api call failed for registration", this);
                            this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/OTP sending error", "Unable to send OTP."));
                            model.IsvalidatAccount = true;
                            model.IsOTPSend = false;
                            return this.View(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("GreenPowerOptIn: OTP Api call failed for registration: " + ex.Message, this);
                        this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/OTP sending error", "Unable to send OTP."));
                        model.IsvalidatAccount = true;
                        model.IsOTPSend = false;
                        return this.View(model);
                    }
                    #endregion
                }
                else
                {
                    ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Account Number incorrect", "Account Number is invalid."));
                    model.IsvalidatAccount = false;
                    return this.View(model);
                }
            }
            if (!string.IsNullOrEmpty(ValidateOTP))
            {
                if (Session["GreenPowerOptModel"] == null)
                {
                    this.ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/GreenPowerOptModel/Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }

                GreenPowerOptInModel accountNumberValidated = (GreenPowerOptInModel)Session["GreenPowerOptModel"];
                if (model.CANumber == accountNumberValidated.CANumber)
                {
                    if (string.IsNullOrEmpty(model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/GreenPowerOptModel/Registration/Enter OTP", "Enter OTP."));
                        model.IsvalidatAccount = true;
                        model.IsOTPSend = true;
                        return this.View(model);
                    }

                    var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);

                    RegistrationRepository registrationRepo = new RegistrationRepository();
                    string generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(registeredMobileNumber, model.CANumber, "GreenPowerOptIn");

                    if (!string.Equals(generatedOTP, model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                        model.IsvalidatAccount = true;
                        model.IsOTPSend = true;
                        model.IsvalidatOTP = false;
                        return this.View(model);
                    }

                    var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                    model.ConsumerName = consumerDetails.NAME_CustomerName;

                    model.IsvalidatAccount = true;
                    model.IsOTPSend = true;
                    model.IsvalidatOTP = true;
                    model.IsAccepted = false;
                    Session["GreenPowerOptModel"] = model;
                }
                return this.View(model);
            }
            if (!string.IsNullOrEmpty(CancelSubmit))
            {
                GreenPowerOptInModel accountNumberValidated = (GreenPowerOptInModel)Session["GreenPowerOptModel"];

                model.CANumber = accountNumberValidated.CANumber;
                model.IsvalidatAccount = true;
                model.IsOTPSend = true;
                model.IsvalidatOTP = true;
                model.IsAccepted = false;
                Session["GreenPowerOptModel"] = model;
                return this.View(model);
            }
            if (!string.IsNullOrEmpty(Accept))
            {
                model.IsvalidatAccount = true;
                model.IsOTPSend = true;
                model.IsvalidatOTP = true;
                model.IsAccepted = true;
                Session["GreenPowerOptModel"] = model;
                return this.View(model);
            }
            if (!string.IsNullOrEmpty(Submit))
            {
                GreenPowerOptInModel accountNumberValidated = (GreenPowerOptInModel)Session["GreenPowerOptModel"];
                if (model.CANumber == accountNumberValidated.CANumber)
                {
                    try
                    {
                        GreenPowerOptInService obj = new GreenPowerOptInService();
                        var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                        var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);

                        model.MobileNumber = registeredMobileNumber;
                        model.ActivateBillingPeriod = accountNumberValidated.ActivateBillingPeriod;
                        model.EmailId = consumerDetails.SMTP_ADDR_Email;
                        var r = obj.InsertGreenPowerOptRecord(model);
                        if (r == true)
                        {
                            //Send email and SMS
                            if (!string.IsNullOrEmpty(consumerDetails.SMTP_ADDR_Email))
                            {
                                try
                                {
                                    Data.Items.Item settingsItem;
                                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.GreenPowerOptInEmail);
                                    var mailTemplateItem = settingsItem;
                                    var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
                                    var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
                                    var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

                                    MailMessage mail = new MailMessage
                                    {
                                        From = new MailAddress(fromMail.Value),
                                        Body = body.Value,
                                        Subject = subject.Value,
                                        IsBodyHtml = true
                                    };

                                    mail.To.Add(consumerDetails.SMTP_ADDR_Email);
                                    mail.Body = mail.Body.Replace("#canumber#", model.CANumber);
                                    MainUtil.SendMail(mail);
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Green Power Opt in send email error: " + e.Message, this);
                                }
                            }

                            //send SMS
                            if (!string.IsNullOrEmpty(registeredMobileNumber))
                            {
                                try
                                {
                                    var apiurl1 = string.Format(DictionaryPhraseRepository.Current.Get("/SDInstallmentOptIn//OTP API URL",
                                "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Dear Customer, Welcome to the Green Club ! We have received your request for Green Power Tariff for account no. {1} which is now effective from {2}. Adani Electricity.&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707162256700084479"), registeredMobileNumber, model.CANumber, DateTime.Now.Date.ToShortDateString());

                                    HttpClient client = new HttpClient();
                                    client.BaseAddress = new Uri(apiurl1);
                                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    HttpResponseMessage response = client.GetAsync(apiurl1).Result;
                                    if (response.IsSuccessStatusCode)
                                    {
                                        Log.Info("Green Power submit SMS to user Api call success: " + apiurl1, this);
                                    }
                                    else
                                    {
                                        Log.Info("Green Power app submit SMS to user Api call failed: " + apiurl1, this);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Green Power Opt in send SMS error: " + e.Message, this);
                                }
                            }

                            model.IsvalidatAccount = true;
                            model.IsvalidatOTP = true;
                            model.IsSubmit = true;
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/GreenPowerOptModel/Registration/Service error", "An error occured, please try again!"));
                            return this.View(model);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("SDInstallmentOptIn error in db save: " + e.Message, this);
                    }
                }
                return this.View(model);
            }
            if (!string.IsNullOrEmpty(IPledge))
            {
                GreenPowerOptInModel accountNumberValidated = (GreenPowerOptInModel)Session["GreenPowerOptModel"];
                if (model.CANumber == accountNumberValidated.CANumber)
                {
                    var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                    model.ConsumerName = consumerDetails.NAME_CustomerName;
                    model.IsAccepted = true;
                    model.IsIPledge = true;
                }
                else
                {
                    model.IsvalidatAccount = false;
                }
                return this.View(model);
            }
            if (!string.IsNullOrEmpty(SubmitCapture))
            {
                GreenPowerOptInModel accountNumberValidated = (GreenPowerOptInModel)Session["GreenPowerOptModel"];
                if (model.CANumber == accountNumberValidated.CANumber)
                {
                    try
                    {
                        GreenPowerOptInService obj = new GreenPowerOptInService();
                        var r = obj.UpdateRecordByAccountNumber(model);
                        model.IsProcessingDone = true;
                    }
                    catch (Exception e)
                    {
                        Log.Error("SDInstallmentOptIn error in db save: " + e.Message, this);
                    }
                }
                return this.View(model);
            }

            if (Session["GreenPowerOptModel"] != null)
            {
                GreenPowerOptInModel accountNumberValidated = (GreenPowerOptInModel)Session["GreenPowerOptModel"];
                GreenPowerOptInService objGreenPowerOptInService = new GreenPowerOptInService();

                GreenPowerOptIn obj = objGreenPowerOptInService.GetRecordByAccountNumber(accountNumberValidated.CANumber);
                accountNumberValidated.ImageSrc = obj.ImageContentType + obj.ImageDate.ToString();

                var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                accountNumberValidated.ConsumerName = consumerDetails.NAME_CustomerName;
                accountNumberValidated.IsAccepted = true;
                accountNumberValidated.IsAccepted = true;
                accountNumberValidated.IsIPledge = true;
                accountNumberValidated.IsSubmit = true;

                return this.View(accountNumberValidated);
            }
            return this.View(model);
        }

        [HttpPost]
        public JsonResult CaptureImage(string accountNumber, string data)
        {
            try
            {
                string filename = accountNumber + "_" + DateTime.Now.ToString("dd-MM-yy-hh-mm-ss");
                string filePath = Path.Combine(Server.MapPath("~/GreenPower"), filename + ".jpg");

                //Convert Base64 Encoded string to Byte Array.
                byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);

                Image imageBackground = (Bitmap)((new ImageConverter()).ConvertFrom(imageBytes));
                string IconImage = Server.MapPath("~/GreenPower/frame.png");

                System.Drawing.Image imageOverlay = System.Drawing.Image.FromFile(IconImage); //small image

                System.Drawing.Image img = new Bitmap(imageBackground.Width, imageBackground.Height);
                using (Graphics gr = Graphics.FromImage(img))
                {
                    gr.DrawImage(imageBackground, new Point(0, 0));
                    gr.DrawImage(imageOverlay, new Point(0, 0));
                }

                //img.Save(OutputImage, ImageFormat.Png);
                ImageConverter _imageConverter = new ImageConverter();
                imageBytes = (byte[])_imageConverter.ConvertTo(img, typeof(byte[]));

                //Save the Byte Array as Image File.
                System.IO.File.WriteAllBytes(filePath, imageBytes);

                try
                {
                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        if (dataContext.GreenPowerOptIns.Any(l => l.AccountNumber == accountNumber))
                        {
                            GreenPowerOptIn recordToUpdate = dataContext.GreenPowerOptIns.FirstOrDefault(l => l.AccountNumber == accountNumber);
                            recordToUpdate.ImageDate = imageBytes;
                            recordToUpdate.ImageContentType = data.Split(',')[0];
                            recordToUpdate.ImageLink = "/GreenPower/" + filename + ".jpg";
                            recordToUpdate.ImageName = filename + ".jpg";
                            dataContext.SubmitChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("CaptureImage error in DB save" + ex.Message, this);
                }

                return Json(new { result = "success", filePathPic = "/GreenPower/" + filename + ".jpg" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = "error:" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult DownloadImage(string accountNumber)
        {
            try
            {
                if (Session["GreenPowerOptModel"] != null)
                {
                    GreenPowerOptInModel accountNumberValidated = (GreenPowerOptInModel)Session["GreenPowerOptModel"];
                    if (accountNumber == accountNumberValidated.CANumber)
                    {
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            var fileToDownload = dbcontext.GreenPowerOptIns.Where(i => i.AccountNumber == accountNumber).FirstOrDefault();
                            if (fileToDownload != null)
                            {
                                return File(fileToDownload.ImageDate.ToArray(), "image/jpeg", fileToDownload.ImageName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at DownloadFile:" + ex.Message, this);
            }
            return null;
        }

        private static Bitmap AddBorderToImage(Image image, int borderSize)
        {
            using (Bitmap bmp = new Bitmap(image.Width + 2 * borderSize,
                image.Height + 2 * borderSize))
            {
                using (Graphics destGraph = Graphics.FromImage(bmp))
                {
                    destGraph.FillRectangle(Brushes.Green, new Rectangle(new Point(0, 0), bmp.Size));
                    destGraph.DrawImage(image, new Point(borderSize, borderSize));
                }

                return bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), image.PixelFormat);
            }
        }

        //[HttpPost]
        //public string CaptureImage(string accountNumber, string data)
        //{
        //    string filename = accountNumber + "_" + DateTime.Now.ToString("dd-MM-yy-hh-mm-ss");
        //    string filePath = Path.Combine(Server.MapPath("~/GreenPower"), filename + ".jpg");

        //    //Convert Base64 Encoded string to Byte Array.
        //    byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);

        //    //Save the Byte Array as Image File.
        //    System.IO.File.WriteAllBytes(filePath, imageBytes);

        //    try
        //    {
        //        using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
        //        {
        //            if (dataContext.GreenPowerOptIns.Any(l => l.AccountNumber == accountNumber))
        //            {
        //                GreenPowerOptIn recordToUpdate = dataContext.GreenPowerOptIns.FirstOrDefault(l => l.AccountNumber == accountNumber);
        //                recordToUpdate.ImageDate = imageBytes;
        //                recordToUpdate.ImageContentType = data.Split(',')[0];
        //                recordToUpdate.ImageLink = filePath;
        //                recordToUpdate.ImageName = filename;
        //                dataContext.SubmitChanges();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("CaptureImage error in DB save" + ex.Message, this);
        //    }

        //    return "success";
        //}

        #endregion


        #region

        [HttpGet]
        public ActionResult ITDeclarations()
        {
            ITDeclarationsModel objITDeclarationsModel = new ITDeclarationsModel();
            try
            {
                if (Request.QueryString["source"] != null)
                {
                    objITDeclarationsModel.Source = Request.QueryString["source"].ToString();
                }
                else
                {
                    objITDeclarationsModel.Source = "1";
                }

                //objITDeclarationsModel.CANumber = "152222350";
                //objITDeclarationsModel.DeclarationType = objITDeclarationsModel.Source;
                //objITDeclarationsModel.IsvalidatAccount = true;
                //objITDeclarationsModel.IsOTPSend = true;
                //objITDeclarationsModel.IsvalidatOTP = true;
                //objITDeclarationsModel.PANNumber = "BBBBB9999K";
                Session["ITDeclarationsModel"] = objITDeclarationsModel;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex.Source);
            }
            return this.View(objITDeclarationsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateRenderingId]
        public ActionResult ITDeclarations(ITDeclarationsModel model, string LinkPAN = null, string FileITR = null, string TDSApplicability = null, string SubmitTDS = null, string ValidateCA = null, string SendOTP = null, string ValidateOTP = null, string Submit = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(LinkPAN))
                {
                    this.ModelState["CANumber"].Errors.Clear();
                    model.DeclarationType = "1";
                    Session["ITDeclarationsModel"] = model;
                }
                if (!string.IsNullOrEmpty(FileITR))
                {
                    this.ModelState["CANumber"].Errors.Clear();
                    model.DeclarationType = "3";
                    Session["ITDeclarationsModel"] = model;
                }
                if (!string.IsNullOrEmpty(TDSApplicability))
                {
                    this.ModelState["CANumber"].Errors.Clear();
                    model.DeclarationType = "2";
                    Session["ITDeclarationsModel"] = model;
                }
                if (!string.IsNullOrEmpty(SubmitTDS))
                {
                    this.ModelState["CANumber"].Errors.Clear();
                    model.DeclarationType = "4";
                    Session["ITDeclarationsModel"] = model;
                }
                if (!string.IsNullOrEmpty(ValidateCA))
                {
                    if (Session["ITDeclarationsModel"] == null)
                    {
                        return this.View(model);
                    }
                    ITDeclarationsModel ITDeclarationsModel = (ITDeclarationsModel)Session["ITDeclarationsModel"];
                    model.DeclarationType = ITDeclarationsModel.DeclarationType;

                    if (string.IsNullOrEmpty(model.CANumber))
                    {
                        ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/Enter User Name", "Please enter valid Account Number."));
                        return this.View(model);
                    }

                    if (model.CANumber.StartsWith("37"))
                    {
                        ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/ITDeclaration/Account Number incorrect", "Entered Account no is invalid. Please enter a valid 9 digit Account No."));
                        return this.View(model);
                    }
                    //Validate CA
                    var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                    if (string.IsNullOrEmpty(consumerDetails.AccountNumber) || consumerDetails.INVALIDCAFLAG == "X")
                    {
                        ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/ITDeclaration/Account Number incorrect", "Entered Account no is invalid. Please enter a valid 9 digit Account No."));
                        return this.View(model);
                    }
                    //Once CA is validated, proceed for captcha validation
                    if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                    {
                        ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                        return this.View(model);
                    }
                    ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                    if (!reCaptchaResponse.success)
                    {
                        ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                        return this.View(model);
                    }
                    //Check If PAN number is updated
                    var consumerPANGSTDetails = SapPiService.Services.RequestHandler.FetchConsumerPANGSTDetails(model.CANumber);
                    if (string.IsNullOrEmpty(consumerPANGSTDetails.CANumber) || string.IsNullOrEmpty(consumerPANGSTDetails.PANNumber))
                    {
                        model.PANNotLinked = true;
                        ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/ITDeclaration/PAN not mapped", "Please update PAN number against entered CA number to proceed further."));
                        return this.View(model);
                    }
                    //-	Check if declaration / intimation is already submitted for current FY:
                    if (model.DeclarationType != "4")
                    {
                        var result = SapPiService.Services.RequestHandler.ITDeclarationCheck(model.CANumber, model.DeclarationType, consumerPANGSTDetails.PANNumber);
                        if (result == null || result.IsSuccess == false)
                        {
                            ModelState.AddModelError(nameof(model.CANumber), result.Message);
                            return this.View(model);
                        }
                    }
                    //-	Call service to get Mobile number from CA.
                    var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);
                    if (string.IsNullOrEmpty(registeredMobileNumber))
                    {
                        ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/ITDeclaration/Mobile Number incorrect", "Please register mobile number with us to submit declaration."));
                        return this.View(model);
                    }
                    model.MobileNumber = string.IsNullOrEmpty(registeredMobileNumber) ? registeredMobileNumber : registeredMobileNumber.Substring(0, 2) + "xxxxxxx" + registeredMobileNumber.Substring(registeredMobileNumber.Length - 3);
                    model.IsvalidatAccount = true;
                    model.PANNumber = consumerPANGSTDetails.PANNumber;
                    Session["ITDeclarationsModel"] = model;
                }
                if (!string.IsNullOrEmpty(SendOTP))
                {
                    if (Session["ITDeclarationsModel"] == null)
                    {
                        return this.View(model);
                    }
                    ITDeclarationsModel ITDeclarationsModel = (ITDeclarationsModel)Session["ITDeclarationsModel"];
                    model.PANNumber = ITDeclarationsModel.PANNumber;
                    model.DeclarationType = ITDeclarationsModel.DeclarationType;
                    if (model.CANumber == ITDeclarationsModel.CANumber)
                    {
                        //if valid then send otp to registered mobile number 
                        //send SMS for OTP
                        RegistrationRepository registrationRepo = new RegistrationRepository();
                        if (!registrationRepo.CheckForCAMaxLimit(model.CANumber, "ITDeclaration"))
                        {
                            Log.Info("PAN Update: Number of attempt to get OTP reached for AccountNumber." + model.CANumber, this);
                            this.ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/Max20OTPPerLECMobile", "Number of attempt to get OTP reached for Entered AccountNumber."));
                            return this.View(model);
                        }

                        var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);
                        string generatedotp = registrationRepo.GenerateOTPRegistration(model.CANumber, null, "ITDeclaration", registeredMobileNumber);
                        //send otp via SMS
                        #region Api call to send SMS of OTP
                        try
                        {
                            string type = "";
                            if (model.DeclarationType == "1")
                                type = DictionaryPhraseRepository.Current.Get("/Accounts/ITDeclarations/Type1", "linking PAN");
                            else if (model.DeclarationType == "2")
                                type = DictionaryPhraseRepository.Current.Get("/Accounts/ITDeclarations/Type1", "filing ITR");
                            else if (model.DeclarationType == "3")
                                type = DictionaryPhraseRepository.Current.Get("/Accounts/ITDeclarations/Type1", "TDS");
                            else type = DictionaryPhraseRepository.Current.Get("/Accounts/ITDeclarations/Type1", "Submit TDS");
                            var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ITDeclaration//OTP API URL",
                            "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Dear Customer, OTP is {1} for your transaction for {2} . Adani Electricity.&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707162256666913751"), registeredMobileNumber, generatedotp, type);

                            HttpClient client = new HttpClient();
                            client.BaseAddress = new Uri(apiurl);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            HttpResponseMessage response = client.GetAsync(apiurl).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                Log.Info("ChangeOfNameLECUserRegisteration: OTP Api call success for LEC registration", this);
                                model.IsvalidatAccount = true;
                                model.IsOTPSend = true;
                                Session["ComplaintRegistrationModel"] = model;
                                return this.View(model);
                            }
                            else
                            {
                                Log.Error("ChangeOfNameLECUserRegisteration OTP Api call failed for registration", this);
                                this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/OTP sending error", "Unable to send OTP."));
                                model.IsvalidatAccount = true;
                                model.IsOTPSend = false;
                                return this.View(model);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error("ChangeOfNameLECUserRegisteration: OTP Api call failed for registration: " + ex.Message, this);
                            this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/OTP sending error", "Unable to send OTP."));
                            model.IsvalidatAccount = true;
                            model.IsOTPSend = false;
                            return this.View(model);
                        }
                        #endregion
                    }
                }
                if (!string.IsNullOrEmpty(ValidateOTP))
                {
                    if (Session["ITDeclarationsModel"] == null)
                    {
                        return this.View(model);
                    }
                    ITDeclarationsModel ITDeclarationsModel = (ITDeclarationsModel)Session["ITDeclarationsModel"];
                    model.PANNumber = ITDeclarationsModel.PANNumber;
                    model.DeclarationType = ITDeclarationsModel.DeclarationType;
                    if (model.CANumber == ITDeclarationsModel.CANumber)
                    {
                        if (string.IsNullOrEmpty(model.OTPNumber))
                        {
                            this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter OTP", "Enter OTP."));
                            model.IsvalidatAccount = true;
                            model.IsOTPSend = true;
                            return this.View(model);
                        }

                        RegistrationRepository registrationRepo = new RegistrationRepository();
                        var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);
                        string generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(registeredMobileNumber, model.CANumber, "ITDeclaration");

                        if (!string.Equals(generatedOTP, model.OTPNumber))
                        {
                            this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                            model.IsvalidatAccount = true;
                            model.IsOTPSend = true;
                            return this.View(model);
                        }

                        var consumerPANGSTDetails = SapPiService.Services.RequestHandler.FetchConsumerPANGSTDetails(model.CANumber);

                        model.PANNumber = consumerPANGSTDetails.PANNumber;
                        model.IsvalidatAccount = true;
                        model.IsOTPSend = true;
                        model.IsvalidatOTP = true;

                        if (model.DeclarationType == "4")
                        {
                            var billingAmount = SapPiService.Services.RequestHandler.FetchAmountForTDS(model.CANumber);
                            if (billingAmount != null)
                            {
                                model.Bill_Amount = billingAmount;
                            }
                            else
                            {
                                this.ModelState.AddModelError(nameof(model.Bill_Amount), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/No BILL_PAYABLE_AMT", "Unable to fetch Bill Amount."));
                                return this.View(model);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(Submit))
                {
                    if (Session["ITDeclarationsModel"] == null)
                    {
                        return this.View(model);
                    }
                    ITDeclarationsModel ITDeclarationsModel = (ITDeclarationsModel)Session["ITDeclarationsModel"];
                    model.DeclarationType = ITDeclarationsModel.DeclarationType;
                    if (model.CANumber == ITDeclarationsModel.CANumber)
                    {
                        ITDeclarationService objService = new ITDeclarationService();
                        var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                        var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.CANumber);
                        ITDeclarationResult result;
                        ITDeclarationInfo reqParam;
                        byte[] pdfToAttach;
                        switch (ITDeclarationsModel.DeclarationType)
                        {
                            case "1":
                                //Validate AADHAR number entered by Consumer
                                if (model.AgreeOption == "1" && string.IsNullOrEmpty(model.AadharNumber))
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    this.ModelState.AddModelError(nameof(model.AadharNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter valid Aadhar", "Please enter a valid 12 digit Aadhar Number."));
                                    return this.View(model);
                                }

                                //Update SAP
                                reqParam = new ITDeclarationInfo
                                {
                                    AadharNumber = model.AadharNumber,
                                    AccountNumber = model.CANumber,
                                    CANumber = model.CANumber,
                                    AgreeOption = model.AgreeOption,
                                    DeclarationType = model.DeclarationType,
                                    PANNumber = ITDeclarationsModel.PANNumber,
                                    MobileNumber = registeredMobileNumber,
                                    Source = model.Source,
                                    //FY_1 = model.FY_1.Substring(3, model.FY_3.Length),
                                    //FY_2 = model.FY_2.Substring(3, model.FY_3.Length),
                                    //FY_2AcknowledgementNumber = model.FY_2AcknowledgementNumber,
                                    //FY_2DateOfFilingReturn = model.FY_2DateOfFilingReturn,
                                    //FY_3 = model.FY_3.Substring(3, model.FY_3.Length),
                                    //FY_3AcknowledgementNumber = model.FY_3AcknowledgementNumber,
                                    //FY_3DateOfFilingReturn = model.FY_3DateOfFilingReturn
                                };
                                result = SapPiService.Services.RequestHandler.ITDeclarationPost(model.CANumber, reqParam);
                                if (result == null || result.IsSuccess == false)
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    ModelState.AddModelError(nameof(model.SuccessMessage), result.Message);
                                    return this.View(model);
                                }
                                else
                                {
                                    model.IsSuccess = true;
                                    model.SuccessMessage = result.Message;

                                    if (model.AgreeOption == "1")
                                    {
                                        //Generate PDF / Image file of declaration
                                        pdfToAttach = objService.GeneratePDF_LinkPANWithAdhar(model.CANumber, ITDeclarationsModel.PANNumber, model.AadharNumber, Server);

                                        //Upload PDF against CA number 
                                        try
                                        {
                                            string image = Convert.ToBase64String(pdfToAttach);

                                            ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[] returnImgObj = SapPiService.Services.RequestHandler.SelfMeterReadingImageUpload(image, model.CANumber + ".pdf", "", "", "139", model.CANumber);

                                            if (returnImgObj != null)
                                            {
                                                if (returnImgObj.Length > 0)
                                                {
                                                    if (!returnImgObj[0].TYPE.ToString().ToLower().Equals("s"))
                                                    {
                                                        //not uploaded
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //not uploaded
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Log.Error(e.Message, e.Source);
                                        }

                                        //Trigger Email
                                        if (!string.IsNullOrEmpty(consumerDetails.SMTP_ADDR_Email))
                                        {
                                            try
                                            {
                                                Data.Items.Item settingsItem;
                                                settingsItem = Context.Database.GetItem(Templates.MailTemplate.ITDeclarationType1Email);
                                                var mailTemplateItem = settingsItem;
                                                var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
                                                var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
                                                var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

                                                MailMessage mail = new MailMessage
                                                {
                                                    From = new MailAddress(fromMail.Value),
                                                    Body = body.Value,
                                                    Subject = subject.Value,
                                                    IsBodyHtml = true
                                                };

                                                mail.To.Add(consumerDetails.SMTP_ADDR_Email);
                                                mail.Body = mail.Body.Replace("#canumber#", model.CANumber);
                                                //attach PDF TBD
                                                if (model.AgreeOption == "1")
                                                {
                                                    mail.Attachments.Add(new Attachment(new MemoryStream(pdfToAttach), model.CANumber + ".pdf"));
                                                }
                                                MainUtil.SendMail(mail);
                                            }
                                            catch (Exception e)
                                            {
                                                Log.Error("ITDeclarationType1Email in send email error: " + e.Message, this);
                                            }
                                        }

                                        //Trigger SMS
                                        if (!string.IsNullOrEmpty(registeredMobileNumber))
                                        {
                                            try
                                            {
                                                var apiurl1 = string.Format(DictionaryPhraseRepository.Current.Get("/SDInstallmentOptIn//OTP API URL",
                                            "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Dear Consumer, we acknowledge receipt of your Declaration for linking PAN with your AADHAR.  Thank you. Adani Electricity&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707162366091035628"), registeredMobileNumber);

                                                HttpClient client = new HttpClient();
                                                client.BaseAddress = new Uri(apiurl1);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                HttpResponseMessage response = client.GetAsync(apiurl1).Result;
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    Log.Info("ITDeclarationType1 submit SMS to user Api call success: " + apiurl1, this);
                                                }
                                                else
                                                {
                                                    Log.Info("ITDeclarationType1 submit SMS to user Api call failed: " + apiurl1, this);
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                Log.Error("ITDeclarationType1 in send SMS error: " + e.Message, this);
                                            }
                                        }
                                    }
                                }
                                break;
                            case "2":

                                //Update SAP
                                reqParam = new ITDeclarationInfo
                                {
                                    AadharNumber = model.AadharNumber,
                                    AccountNumber = model.CANumber,
                                    CANumber = model.CANumber,
                                    AgreeOption = model.AgreeOption,
                                    DeclarationType = model.DeclarationType,
                                    PANNumber = ITDeclarationsModel.PANNumber,
                                    MobileNumber = registeredMobileNumber,
                                    Source = model.Source,
                                    //FY_1 = model.FY_1.Substring(3, model.FY_3.Length),
                                    //FY_2 = model.FY_2.Substring(3, model.FY_3.Length),
                                    //FY_2AcknowledgementNumber = model.FY_2AcknowledgementNumber,
                                    //FY_2DateOfFilingReturn = model.FY_2DateOfFilingReturn,
                                    //FY_3 = model.FY_3.Substring(3, model.FY_3.Length),
                                    //FY_3AcknowledgementNumber = model.FY_3AcknowledgementNumber,
                                    //FY_3DateOfFilingReturn = model.FY_3DateOfFilingReturn
                                };
                                result = SapPiService.Services.RequestHandler.ITDeclarationPost(model.CANumber, reqParam);
                                if (result == null || result.IsSuccess == false)
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    ModelState.AddModelError(nameof(model.SuccessMessage), result.Message);
                                    return this.View(model);
                                }
                                else
                                {
                                    model.IsSuccess = true;
                                    model.SuccessMessage = result.Message;

                                    if (model.AgreeOption == "1")
                                    {
                                        //Generate PDF / Image file of declaration
                                        pdfToAttach = objService.GeneratePDF_194QApplicabilityofTDS(model.CANumber, ITDeclarationsModel.PANNumber, Server);
                                        //Image upload
                                        try
                                        {
                                            string image = Convert.ToBase64String(pdfToAttach);

                                            ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[] returnImgObj = SapPiService.Services.RequestHandler.SelfMeterReadingImageUpload(image, model.CANumber + ".pdf", "", "", "194", model.CANumber);

                                            if (returnImgObj != null)
                                            {
                                                if (returnImgObj.Length > 0)
                                                {
                                                    if (!returnImgObj[0].TYPE.ToString().ToLower().Equals("s"))
                                                    {
                                                        //not uploaded
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //not uploaded
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Log.Error(e.Message, e.Source);
                                        }

                                        //Trigger Email
                                        if (!string.IsNullOrEmpty(consumerDetails.SMTP_ADDR_Email))
                                        {
                                            try
                                            {
                                                Data.Items.Item settingsItem;
                                                settingsItem = Context.Database.GetItem(Templates.MailTemplate.ITDeclarationType2Email);
                                                var mailTemplateItem = settingsItem;
                                                var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
                                                var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
                                                var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

                                                MailMessage mail = new MailMessage
                                                {
                                                    From = new MailAddress(fromMail.Value),
                                                    Body = body.Value,
                                                    Subject = subject.Value,
                                                    IsBodyHtml = true
                                                };

                                                mail.To.Add(consumerDetails.SMTP_ADDR_Email);
                                                mail.Body = mail.Body.Replace("#canumber#", model.CANumber);
                                                //attach PDF TBD
                                                if (model.AgreeOption == "1")
                                                {
                                                    mail.Attachments.Add(new Attachment(new MemoryStream(pdfToAttach), model.CANumber + ".pdf"));
                                                }
                                                MainUtil.SendMail(mail);
                                            }
                                            catch (Exception e)
                                            {
                                                Log.Error("ITDeclarationType2Email in send email error: " + e.Message, this);
                                            }
                                        }

                                        //Trigger SMS
                                        if (!string.IsNullOrEmpty(registeredMobileNumber))
                                        {
                                            try
                                            {
                                                var apiurl1 = string.Format(DictionaryPhraseRepository.Current.Get("/SDInstallmentOptIn//OTP API URL",
                                            "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Dear Consumer, we acknowledge receipt of your Declaration for applicability of TDS U/s. 194Q. Thank you. Adani Electricity&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707162366099120579"), registeredMobileNumber);

                                                HttpClient client = new HttpClient();
                                                client.BaseAddress = new Uri(apiurl1);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                HttpResponseMessage response = client.GetAsync(apiurl1).Result;
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    Log.Info("ITDeclarationType2 submit SMS to user Api call success: " + apiurl1, this);
                                                }
                                                else
                                                {
                                                    Log.Info("ITDeclarationType2 submit SMS to user Api call failed: " + apiurl1, this);
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                Log.Error("ITDeclarationType2 in send SMS error: " + e.Message, this);
                                            }
                                        }
                                    }
                                }
                                break;
                            case "3":

                                if (model.AgreeOption == "1" && string.IsNullOrEmpty(model.FY_3AcknowledgementNumber))
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    this.ModelState.AddModelError(nameof(model.FY_3AcknowledgementNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter valid FY_AcknowledgementNumber", "Please enter a valid Acknowledgement Number."));
                                    return this.View(model);
                                }
                                if (model.AgreeOption == "1" && string.IsNullOrEmpty(model.FY_3DateOfFilingReturn))
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    this.ModelState.AddModelError(nameof(model.FY_3DateOfFilingReturn), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter valid FilingDate", "Please enter a valid Filing Return Date."));
                                    return this.View(model);
                                }
                                if (model.AgreeOption == "1" && string.IsNullOrEmpty(model.FY_2AcknowledgementNumber))
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    this.ModelState.AddModelError(nameof(model.FY_2AcknowledgementNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter valid FY_AcknowledgementNumber", "Please enter a valid Acknowledgement Number."));
                                    return this.View(model);
                                }
                                if (model.AgreeOption == "1" && string.IsNullOrEmpty(model.FY_2DateOfFilingReturn))
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    this.ModelState.AddModelError(nameof(model.FY_2DateOfFilingReturn), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter valid FilingDate", "Please enter a valid Filing Return Date."));
                                    return this.View(model);
                                }


                                //Update SAP
                                reqParam = new ITDeclarationInfo
                                {
                                    AadharNumber = model.AadharNumber,
                                    AccountNumber = model.CANumber,
                                    CANumber = model.CANumber,
                                    AgreeOption = model.AgreeOption,
                                    DeclarationType = model.DeclarationType,
                                    PANNumber = ITDeclarationsModel.PANNumber,
                                    MobileNumber = registeredMobileNumber,
                                    Source = model.Source,
                                    FY_1 = model.FY_1,
                                    FY_2 = model.FY_2,
                                    FY_2AcknowledgementNumber = model.FY_2AcknowledgementNumber,
                                    FY_2DateOfFilingReturn = model.FY_2DateOfFilingReturn,
                                    FY_3 = model.FY_3,
                                    FY_3AcknowledgementNumber = model.FY_3AcknowledgementNumber,
                                    FY_3DateOfFilingReturn = model.FY_3DateOfFilingReturn
                                };
                                result = SapPiService.Services.RequestHandler.ITDeclarationPost(model.CANumber, reqParam);
                                if (result == null || result.IsSuccess == false)
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    ModelState.AddModelError(nameof(model.SuccessMessage), result.Message);
                                    return this.View(model);
                                }
                                else
                                {
                                    model.IsSuccess = true;
                                    model.SuccessMessage = result.Message;

                                    if (model.AgreeOption == "1")
                                    {
                                        //Generate PDF / Image file of declaration
                                        pdfToAttach = objService.GeneratePDF_206CFilingITR(model.CANumber, ITDeclarationsModel.PANNumber, model.FY_3AcknowledgementNumber, model.FY_3DateOfFilingReturn, model.FY_2AcknowledgementNumber, model.FY_2DateOfFilingReturn, Server);

                                        //Image upload
                                        try
                                        {
                                            string image = Convert.ToBase64String(pdfToAttach);

                                            ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[] returnImgObj = SapPiService.Services.RequestHandler.SelfMeterReadingImageUpload(image, model.CANumber + ".pdf", "", "", "207", model.CANumber);

                                            if (returnImgObj != null)
                                            {
                                                if (returnImgObj.Length > 0)
                                                {
                                                    if (!returnImgObj[0].TYPE.ToString().ToLower().Equals("s"))
                                                    {
                                                        //not uploaded
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //not uploaded
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Log.Error(e.Message, e.Source);
                                        }

                                        //Trigger Email
                                        if (!string.IsNullOrEmpty(consumerDetails.SMTP_ADDR_Email))
                                        {
                                            try
                                            {
                                                Data.Items.Item settingsItem;
                                                settingsItem = Context.Database.GetItem(Templates.MailTemplate.ITDeclarationType3Email);
                                                var mailTemplateItem = settingsItem;
                                                var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
                                                var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
                                                var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

                                                MailMessage mail = new MailMessage
                                                {
                                                    From = new MailAddress(fromMail.Value),
                                                    Body = body.Value,
                                                    Subject = subject.Value,
                                                    IsBodyHtml = true
                                                };

                                                int currentFYYear = DateTime.Now.Year;
                                                if (DateTime.Now.Month <= 3)
                                                {
                                                    currentFYYear = DateTime.Now.Year - 1;
                                                };

                                                string FY_3 = "FY " + (currentFYYear - 3).ToString() + "-" + ((currentFYYear - 2) - 2000).ToString();
                                                string FY_2 = "FY " + (currentFYYear - 2).ToString() + "-" + ((currentFYYear - 1) - 2000).ToString();
                                                string FY_1 = "FY " + (currentFYYear - 1).ToString() + "-" + ((currentFYYear) - 2000).ToString();
                                                string FY = "FY " + (currentFYYear).ToString() + "-" + ((currentFYYear + 1) - 2000).ToString();


                                                mail.To.Add(consumerDetails.SMTP_ADDR_Email);
                                                mail.Body = mail.Body.Replace("#canumber#", model.CANumber);
                                                mail.Body = mail.Body.Replace("#CurrentFY#", FY);
                                                mail.Body = mail.Body.Replace("#CurrentFY3#", FY_3);
                                                mail.Body = mail.Body.Replace("#CurrentFY2#", FY_2);
                                                //attach PDF
                                                mail.Attachments.Add(new Attachment(new MemoryStream(pdfToAttach), model.CANumber + ".pdf"));
                                                MainUtil.SendMail(mail);
                                            }
                                            catch (Exception e)
                                            {
                                                Log.Error("ITDeclarationType3Email in send email error: " + e.Message, this);
                                            }
                                        }

                                        //Trigger SMS
                                        if (!string.IsNullOrEmpty(registeredMobileNumber))
                                        {
                                            try
                                            {
                                                var apiurl1 = string.Format(DictionaryPhraseRepository.Current.Get("/SDInstallmentOptIn//OTP API URL",
                                            "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Dear Consumer, we acknowledge receipt of your Declaration for filing of Income Tax Returns for F. Y. 18-19 and F. Y. 19-20.  Thank you. Adani Electricity&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707162366095428313"), registeredMobileNumber, model.FY_3, model.FY_2);

                                                HttpClient client = new HttpClient();
                                                client.BaseAddress = new Uri(apiurl1);
                                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                HttpResponseMessage response = client.GetAsync(apiurl1).Result;
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    Log.Info("ITDeclarationType3 submit SMS to user Api call success: " + apiurl1, this);
                                                }
                                                else
                                                {
                                                    Log.Info("ITDeclarationType3 submit SMS to user Api call failed: " + apiurl1, this);
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                Log.Error("ITDeclarationType3 in send SMS error: " + e.Message, this);
                                            }
                                        }
                                    }
                                }
                                break;
                            case "4":
                                //Update SAP
                                var billingAmount = SapPiService.Services.RequestHandler.FetchAmountForTDS(model.CANumber);

                                //Validate entered amount
                                if (string.IsNullOrEmpty(model.Amount_considered_for_TDS.ToString()) || model.Amount_considered_for_TDS.ToString().Any(char.IsLetter))
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    this.ModelState.AddModelError(nameof(model.Amount_considered_for_TDS), DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidAmount", "Invalid amount value."));
                                    return this.View(model);
                                }
                                if (Convert.ToDecimal(model.Amount_considered_for_TDS) > Convert.ToDecimal(billingAmount))
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    this.ModelState.AddModelError(nameof(model.Amount_considered_for_TDS), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid TDS amount", "This amount should be less than or equal to 'Latest Bill Amount'."));
                                    return this.View(model);
                                }

                                reqParam = new ITDeclarationInfo
                                {
                                    //AadharNumber = model.AadharNumber,
                                    //AccountNumber = model.CANumber,
                                    CANumber = model.CANumber,
                                    //AgreeOption = model.AgreeOption,
                                    DeclarationType = model.DeclarationType,
                                    PANNumber = ITDeclarationsModel.PANNumber,
                                    TDS_AMOUNT = model.Amount_considered_for_TDS,
                                    BILL_PAYABLE_AMT = billingAmount,
                                    PAYMENT_AMT = billingAmount,
                                    TDS_RATE = "0.1",
                                    POSTING_DATE = model.POSTING_DATE,
                                    //MobileNumber = registeredMobileNumber,
                                    Source = model.Source,
                                    //FY_1 = model.FY_1,
                                    //FY_2 = model.FY_2,
                                    //FY_2AcknowledgementNumber = model.FY_2AcknowledgementNumber,
                                    //FY_2DateOfFilingReturn = model.FY_2DateOfFilingReturn,
                                    //FY_3 = model.FY_3,
                                    //FY_3AcknowledgementNumber = model.FY_3AcknowledgementNumber,
                                    //FY_3DateOfFilingReturn = model.FY_3DateOfFilingReturn
                                };
                                result = SapPiService.Services.RequestHandler.ITDeclarationPost(model.CANumber, reqParam);
                                if (result == null || result.IsSuccess == false)
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    model.Bill_Amount = billingAmount;
                                    ModelState.AddModelError(nameof(model.SuccessMessage), result.Message);
                                    return this.View(model);
                                }
                                else
                                {
                                    model.IsSuccess = true;
                                    model.SuccessMessage = result.Message;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                return this.View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex.Source);
                return this.View(model);
            }
        }



        //private
        // static string FontRegular = "";
        //private static string FontBold = "";
        //private static string FontMedium = "";

        //Font ColorFont = FontFactory.GetFont(FontRegular, 8, Font.NORMAL, new BaseColor(171, 49, 117));
        //Font boldFont = FontFactory.GetFont(FontBold, 8, Font.NORMAL);
        //Font RegularFont = FontFactory.GetFont(FontRegular, 10, Font.NORMAL);//23, 56, 133
        //Font RegularFontWhite = FontFactory.GetFont(FontRegular, 8, Font.NORMAL, new BaseColor(255, 255, 255));
        //Font RegularFont2 = FontFactory.GetFont(FontRegular, 8, Font.NORMAL, new BaseColor(23, 56, 133));
        //Font smallFontWhite = FontFactory.GetFont(FontRegular, 6, new BaseColor(255, 255, 255));
        //Font smallFont = FontFactory.GetFont(FontRegular, 6);
        //Font smallredFont = FontFactory.GetFont(FontRegular, 6, Font.NORMAL, new BaseColor(209, 85, 82));
        //Font FontTextmed8 = FontFactory.GetFont(FontMedium, 8, Font.NORMAL, new BaseColor(209, 85, 82));

        //public byte[] GeneratePDF_LinkPANWithAdhar(string accountNumber, string panNumber, string aadharNumber)
        //{
        //    using (MemoryStream output = new MemoryStream())
        //    {
        //        Document document = new Document(PageSize.A4, 40, 40, 40, 40);
        //        PdfWriter writer = PdfWriter.GetInstance(document, output);
        //        document.Open();
        //        PdfContentByte cb = writer.DirectContent;

        //        //Add logo Image                

        //        String imagePath2 = Server.MapPath("/Images/powerofservicelogo.jpg");
        //        Image logoImage2 = Image.GetInstance(imagePath2.Replace("/PDFGenerate", ""));

        //        logoImage2.ScalePercent(50, 50);
        //        logoImage2.SetAbsolutePosition(20, 800);// 75,75
        //        document.Add(logoImage2);


        //        String imagePath3 = Server.MapPath("/Images/Adani_logo.jpg");
        //        Image logoImage3 = Image.GetInstance(imagePath3.Replace("/PDFGenerate", ""));

        //        logoImage3.ScalePercent(50, 50);
        //        logoImage3.SetAbsolutePosition(500, 800);// 75,75                
        //        document.Add(logoImage3);

        //        string text = @"Declaration for linking PAN with AADHAR, u/s 139 AA of income tax Act 1961";
        //        Paragraph paragraph = new Paragraph();
        //        paragraph.SpacingBefore = 10;
        //        paragraph.SpacingAfter = 10;
        //        paragraph.Alignment = Element.ALIGN_LEFT;
        //        paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.ORANGE);
        //        paragraph.Add(text);
        //        document.Add(paragraph);

        //        string Date = DateTime.Now.ToString("dd/MM/yyyy");
        //        Paragraph Dates = new Paragraph();
        //        Dates.SpacingBefore = 10;
        //        Dates.SpacingAfter = 10;
        //        Dates.Alignment = Element.ALIGN_LEFT;
        //        Dates.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        Dates.Add("Date:" + Date);
        //        document.Add(Dates);
        //        string text1 = @"To,";
        //        Paragraph paragraph1 = new Paragraph();
        //        paragraph1.SpacingBefore = 10;
        //        paragraph1.SpacingAfter = 10;
        //        paragraph1.Alignment = Element.ALIGN_LEFT;
        //        paragraph1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        paragraph1.Add(text1);
        //        document.Add(paragraph1);
        //        string text2 = @"Adani Electricity Mumbai Limited";
        //        Paragraph paragraph2 = new Paragraph();
        //        paragraph2.SpacingBefore = 10;
        //        paragraph2.SpacingAfter = 10;
        //        paragraph2.Alignment = Element.ALIGN_LEFT;
        //        paragraph2.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        paragraph2.Add(text2);
        //        document.Add(paragraph2);
        //        string text3 = @"Subject : Confirmation for linking of PAN with Aadhaar";
        //        Paragraph paragraph3 = new Paragraph();
        //        paragraph3.SpacingBefore = 10;
        //        paragraph3.SpacingAfter = 10;
        //        paragraph3.Alignment = Element.ALIGN_LEFT;
        //        paragraph3.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        paragraph3.Add(text3);
        //        document.Add(paragraph3);
        //        string text4 = @"Contract Account Number :" + accountNumber;
        //        Paragraph paragraph4 = new Paragraph();
        //        paragraph4.SpacingBefore = 10;
        //        paragraph4.SpacingAfter = 10;
        //        paragraph4.Alignment = Element.ALIGN_LEFT;
        //        paragraph4.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        paragraph4.Add(text4);
        //        document.Add(paragraph4);
        //        string text5 = @"Reference : " + "Section 139AA of the Income Tax Act, 1961 ('the Act')";
        //        Paragraph paragraph5 = new Paragraph();
        //        paragraph5.SpacingBefore = 10;
        //        paragraph5.SpacingAfter = 10;
        //        paragraph5.Alignment = Element.ALIGN_LEFT;
        //        paragraph5.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        paragraph5.Add(text5);
        //        document.Add(paragraph5);
        //        LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
        //        document.Add(line);

        //        string text7 = @"In terms of the provisions of section 139AA of the Act, I, hereby confirm that my Permanent Account Number (PAN) " + panNumber + " has been linked with my Aadhar Number " + aadharNumber;
        //        Paragraph paragraph7 = new Paragraph();
        //        paragraph7.SpacingBefore = 10;
        //        paragraph7.SpacingAfter = 10;
        //        paragraph7.Alignment = Element.ALIGN_LEFT;
        //        paragraph7.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        paragraph7.Add(text7);
        //        document.Add(paragraph7);
        //        List list = new List(List.ORDERED);
        //        list.Add(new itextsharp.iTextSharp.text.ListItem("The above PAN is already registered with AEML against the CA number/s mentioned above.", RegularFont));
        //        list.Add(new itextsharp.iTextSharp.text.ListItem("Any change in the above declaration shall be communicated to AEML.", RegularFont));
        //        list.Add(new itextsharp.iTextSharp.text.ListItem("If the above declaration is found to be incorrect, AEML will be liable to deduct/collect TDS/TCS at higher rate, as applicable, along with interest/penalty, if applicable.", RegularFont));
        //        list.Add(new itextsharp.iTextSharp.text.ListItem("The above information is true and correct to the best of my knowledge.", RegularFont));
        //        string texting = "Terms and Conditions:";
        //        Paragraph paragraph9 = new Paragraph();
        //        paragraph9.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        paragraph9.Add(texting);

        //        document.Add(paragraph9);
        //        document.Add(list);
        //        string text10 = @"I shall indemnify you in case the above disclosure results in false declaration/undertaking.";
        //        Paragraph paragraph10 = new Paragraph();
        //        paragraph10.SpacingBefore = 10;
        //        paragraph10.SpacingAfter = 10;
        //        paragraph10.Alignment = Element.ALIGN_LEFT;
        //        paragraph10.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        paragraph10.Add(text10);
        //        document.Add(paragraph10);
        //        string text11 = @"I hereby, agree to above terms and conditions.";
        //        Paragraph paragraph11 = new Paragraph();
        //        paragraph11.SpacingBefore = 10;
        //        paragraph11.SpacingAfter = 10;
        //        paragraph11.Alignment = Element.ALIGN_LEFT;
        //        paragraph11.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        paragraph11.Add(text11);
        //        document.Add(paragraph11);
        //        string CurrentDate = DateTime.Now.ToShortDateString();
        //        string CurrentTime = DateTime.Now.ToShortTimeString();
        //        Paragraph CurrentTimes = new Paragraph();
        //        Paragraph CurrentDates = new Paragraph();
        //        CurrentDates.SpacingBefore = 10;
        //        CurrentDates.SpacingAfter = 10;
        //        CurrentDates.Alignment = Element.ALIGN_LEFT;
        //        CurrentTimes.SpacingBefore = 10;
        //        CurrentTimes.SpacingAfter = 10;
        //        CurrentTimes.Alignment = Element.ALIGN_LEFT;
        //        CurrentDates.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        CurrentDates.Add("Date:" + CurrentDate);
        //        CurrentTimes.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
        //        CurrentTimes.Add("Time :" + CurrentTime);

        //        document.Add(CurrentDates);
        //        document.Add(CurrentTimes);

        //        document.Close();
        //        return output.ToArray();
        //    }
        //}


        #endregion
    }
}
