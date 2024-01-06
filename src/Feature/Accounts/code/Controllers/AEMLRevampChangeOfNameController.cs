namespace Sitecore.Feature.Accounts.Controllers
{
    extern alias itextsharp;
    using itextsharp::iTextSharp.text;
    using itextsharp::iTextSharp.text.pdf;
    using CaptchaMvc.HtmlHelpers;
    using DotNetIntegrationKit;
    using Newtonsoft.Json;
    using paytm;
    using RestSharp;
    using SapPiService.Domain;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Accounts.Attributes;
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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
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
    using Sitecore.Feature.Accounts.SessionHelper;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing.Imaging;

    [CookieTemperingRedirectNotFound]
    public class AEMLRevampChangeOfNameController : Controller
    {
        public AEMLRevampChangeOfNameController(IAccountRepository accountRepository, INotificationService notificationService, IAccountsSettingsService accountsSettingsService, IGetRedirectUrlService getRedirectUrlService, IUserProfileService userProfileService, IFedAuthLoginButtonRepository fedAuthLoginRepository, IUserProfileProvider userProfileProvider, IPaymentService paymentService, IDbAccountService dbAccountService)
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

        public static Sitecore.Data.Database webDb = Sitecore.Configuration.Factory.GetDatabase("web");


        #region Change of Name

        #region Registration

        //[HttpGet]
        public ActionResult ChangeOfNameRegisterationRevamp()
        {
            Session["Message"] = null;
            ChangeOfNameRegistrationModel model = new ChangeOfNameRegistrationModel();
            model.IsvalidatAccount = false;
            model.IsOTPSent = false;
            return this.View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        public ActionResult ChangeOfNameRegisterationRevamp(ChangeOfNameRegistrationModel registrationInfo, string ValidateAccount = null, string sendOTP = null, string ValidateOTP = null, string submit = null, string CheckApplication = null)
        {
            try
            {
                Session["Message"] = null;
                if (!ModelState.IsValid)
                {
                    return this.View(registrationInfo);
                }
                if (!string.IsNullOrEmpty(ValidateAccount))
                {
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

                    Log.Info("ChangeOfNameRegisteration validate CA from DB", this);
                    ChangeOfNameService changeOfNameService = new ChangeOfNameService();
                    if (changeOfNameService.IsApplicationExistsForCARegistration(registrationInfo.AccountNo))
                    {
                        this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/Account Number Exists", "An application for Change of Name is already registered for this Account Number"));
                        registrationInfo.AccountNo = null;
                        return this.View(registrationInfo);
                    }

                    //validate account number call sap service
                    Log.Info("ChangeOfNameRegisteration validate CA from SAP", this);
                    var accountDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(registrationInfo.AccountNo);

                    string checkCA = ChangeOfNameService.ValidateCA(accountDetails);
                    if (!string.IsNullOrEmpty(checkCA))
                    {
                        this.ModelState.AddModelError(nameof(registrationInfo.AccountNo), checkCA);
                        registrationInfo.AccountNo = null;
                        return this.View(registrationInfo);
                    }
                    else
                    {
                        registrationInfo.IsvalidatAccount = true;
                        //registrationInfo.MobileNo = accountDetails.MOBILE_NO;
                        registrationInfo.Name = accountDetails.NAME_CustomerName;
                        Session["OTPVersions"] = 1;
                    }
                }
                else if (!string.IsNullOrEmpty(sendOTP))
                {
                    if (string.IsNullOrEmpty(registrationInfo.MobileNo))
                    {
                        this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile number invalid", "Please enter valid Mobile Number"));
                        registrationInfo.IsvalidatAccount = true;
                        return this.View(registrationInfo);
                    }
                    else if (!IsCorrectMobileNumber(registrationInfo.MobileNo))
                    {
                        this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile number invalid", "Please enter valid Mobile Number"));
                        registrationInfo.IsvalidatAccount = true;
                        return this.View(registrationInfo);
                    }
                    else
                    {
                        if (Session["OTPVersions"] != null)
                        {
                            Session["OTPVersions"] = Convert.ToInt32(Session["OTPVersions"].ToString()) + 1;
                            if (Convert.ToInt32(Session["OTPVersions"].ToString()) > 5)
                            {
                                this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Number of OTP validation exceed", "You cannot request OTP more than 5 times, please try later."));
                                registrationInfo.IsvalidatAccount = true;
                                registrationInfo.IsOTPCallExceeded = true;
                                return this.View(registrationInfo);
                            }
                        }
                        else
                            Session["OTPVersions"] = 1;

                        //registrationInfo.AccountNoForCheckApplication = null;
                        //registrationInfo.RegistrationNoForCheckApplication = null;
                        //registrationInfo.RegistrationNo = null;
                        //Session["CONAccountDetails"] = registrationInfo;
                        //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameApplicationFormPage));

                        #region Delete Available otp from database for given mobile number
                        RegistrationRepository registrationRepo = new RegistrationRepository();
                        registrationRepo.DeleteOldOtp(registrationInfo.MobileNo);
                        #endregion

                        #region Generate New Otp for given mobile number and save to database
                        var moelRegisteredValidateAccount = new RegisteredValidateAccount
                        {
                            MobileNo = registrationInfo.MobileNo,
                            AccountNo = registrationInfo.AccountNo
                        };
                        string generatedotp = registrationRepo.StoreGeneratedOtp(moelRegisteredValidateAccount);
                        #endregion

                        #region Api call to send SMS of OTP
                        try
                        {
                            var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API for registration",
                                "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=Welcome to Adani Electricity. You have initiated a request to change the name on your bill, for Account no. {1}. OTP for validation is {2}&intflag=false"), registrationInfo.MobileNo, registrationInfo.AccountNo, generatedotp);
                            Log.Info("CON Register OTP APi URL" + apiurl, this);
                            HttpClient client = new HttpClient();
                            client.BaseAddress = new Uri(apiurl);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage response = client.GetAsync(apiurl).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                Log.Error("CON OTP Api call success for registration", this);
                                //this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTP", "OTP Sent."));
                                registrationInfo.IsOTPSent = true;
                                registrationInfo.IsvalidatAccount = true;
                                return this.View(registrationInfo);
                            }
                            else
                            {
                                Log.Error("CON OTP Api call failed for registration", this);
                                this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                                registrationInfo.IsOTPSent = false;
                                registrationInfo.IsvalidatAccount = true;
                                return this.View(registrationInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error("CON OTP Api call failed for registration, catch" + ex.Message, this);
                            this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                            registrationInfo.IsOTPSent = false;
                            registrationInfo.IsvalidatAccount = true;
                            return this.View(registrationInfo);
                            ////Temp
                            //registrationInfo.AccountNoForCheckApplication = null;
                            //registrationInfo.RegistrationNoForCheckApplication = null;
                            //registrationInfo.RegistrationNo = null;
                            //Session["CONAccountDetails"] = registrationInfo;
                            //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameApplicationFormPage));
                        }
                        #endregion
                    }
                }
                else if (!string.IsNullOrEmpty(ValidateOTP))
                {
                    if (string.IsNullOrEmpty(registrationInfo.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(registrationInfo.OTPNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile number OTP no value", "Enter OTP."));
                        registrationInfo.IsOTPSent = true;
                        registrationInfo.IsvalidatAccount = true;
                        return this.View(registrationInfo);
                    }
                    RegistrationRepository registrationRepo = new RegistrationRepository();
                    string generatedOTP = registrationRepo.GetOTP(registrationInfo.MobileNo);
                    if (!string.Equals(generatedOTP, registrationInfo.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(registrationInfo.OTPNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                        registrationInfo.IsOTPSent = true;
                        registrationInfo.IsvalidatAccount = true;
                        return this.View(registrationInfo);
                    }
                    else
                    {
                        registrationInfo.AccountNoForCheckApplication = null;
                        registrationInfo.RegistrationNoForCheckApplication = null;
                        registrationInfo.RegistrationNo = null;
                        Log.Info("ChangeOfNameRegisteration redirecting to Application form for CA:" + registrationInfo.AccountNo, this);
                        Session["CONAccountDetails"] = registrationInfo;
                        //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                        return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameApplicationFormPageRevamp));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at ChangeOfNameRegisteration Post " + ex.Message, this);
            }
            return View(registrationInfo);
        }


        //[HttpGet]
        public ActionResult ChangeOfNameCheckApplicationRevamp()
        {
            return View();
        }


        [HttpPost]
        [ValidateRenderingId]
        public ActionResult ChangeOfNameCheckApplicationRevamp(string accountNumber, string registrationNumber, string OTPNumber, string recaptcha)
        {
            bool isLEC = false;
            CheckApplicationModel result = new CheckApplicationModel
            {
                Issuccess = false,
                IsOTPSent = false,
                IsVerified = false,
                Message = ""
            };
            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), recaptcha);
            
            if (!reCaptchaResponse.success)
            {
                result = new CheckApplicationModel()
                {
                    Issuccess = false,
                    IsOTPSent = false,
                    IsVerified = false,
                    Message = "Please validate captcha to continue"
                };
                return Json(result , JsonRequestBehavior.AllowGet);
            }
            string message = "";
            try
            {

                accountNumber = accountNumber.TrimStart('0');
                ChangeOfNameService changeOfNameService = new ChangeOfNameService();
                if (string.IsNullOrEmpty(accountNumber) && string.IsNullOrEmpty(registrationNumber))
                {
                    message = DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/Account Number not Exists", "Please provide CA number or Registration number");
                    result.Issuccess = false;
                    result.Message = message;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(accountNumber))
                {
                    if (!string.IsNullOrEmpty(OTPNumber))
                    {
                        var existingAPP = changeOfNameService.GetExistingApplication(accountNumber);

                        RegistrationRepository registrationRepo = new RegistrationRepository();
                        string generatedOTP = registrationRepo.GetOTP(existingAPP.MobileNumber);
                        if (!string.Equals(generatedOTP, OTPNumber))
                        {
                            result.Issuccess = true;
                            result.IsVerified = false;
                            result.Message = "OTP does not match";
                        }
                        else
                        {
                            ChangeOfNameRegistrationModel registrationInfo = new ChangeOfNameRegistrationModel
                            {
                                AccountNoForCheckApplication = existingAPP.AccountNumber,
                                MobileNo = existingAPP.MobileNumber,
                                EmailId = "",
                                IsLEC = false
                            };
                            Session["CONAccountDetails"] = registrationInfo;
                            string redirectUrl = this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameApplicationFormPageRevamp);
                            result.Issuccess = true;
                            result.IsVerified = true;
                            result.Message = redirectUrl;
                        }
                    }
                    else if (!changeOfNameService.IsApplicationExistsForCA(accountNumber))
                    {
                        message = DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/CheckApplicationExistsForAccountNumber", "No application for Change of Name is registered for this Account Number");
                        result.Issuccess = false;
                        result.Message = message;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var existingAPP = changeOfNameService.GetExistingApplication(accountNumber);

                        ChangeOfNameRegistrationModel registrationInfo = new ChangeOfNameRegistrationModel
                        {
                            AccountNoForCheckApplication = accountNumber,
                            MobileNo = isLEC ? existingAPP.LECMobileNumber : existingAPP.MobileNumber,
                            EmailId = "",
                            IsLEC = false
                        };
                        //Session["CONAccountDetails"] = null;
                        result.Issuccess = true;
                        //result.Message = redirectUrl;

                        #region Delete Available otp from database for given mobile number
                        RegistrationRepository registrationRepo = new RegistrationRepository();
                        registrationRepo.DeleteOldOtp(registrationInfo.MobileNo);
                        #endregion

                        #region Generate New Otp for given mobile number and save to database
                        var moelRegisteredValidateAccount = new RegisteredValidateAccount
                        {
                            MobileNo = registrationInfo.MobileNo,
                            AccountNo = registrationInfo.AccountNoForCheckApplication
                        };
                        string generatedotp = registrationRepo.StoreGeneratedOtp(moelRegisteredValidateAccount);
                        #endregion

                        #region Api call to send SMS of OTP
                        try
                        {
                            var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API for check submitted application",
                                "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=Welcome to Adani Electricity. OTP to check your submitted/saved application for Change of Name on the bill, for A/C no. {1} is {2}&intflag=false"), registrationInfo.MobileNo, accountNumber, generatedotp);
                            Log.Info("CON Check application OTP APi URL" + apiurl, this);
                            HttpClient client = new HttpClient();
                            client.BaseAddress = new Uri(apiurl);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage response = client.GetAsync(apiurl).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                Log.Error("CON OTP Api call success for check application", this);
                                result.IsOTPSent = true;
                                result.Message = "OTP sent to your registered mobile number.";// registrationInfo.MobileNo;
                            }
                            else
                            {
                                Log.Error("CON OTP Api call failed for check application", this);
                                result.IsOTPSent = false;
                                result.Message = "Unable to send OTP to registered mobile number.";
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error("CON OTP Api call failed for check application, catch" + ex.Message, this);
                            result.IsOTPSent = false;
                            result.Message = "Unable to send OTP to registered mobile number.";// registrationInfo.MobileNo;
                        }
                        #endregion
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!string.IsNullOrEmpty(registrationNumber))
                {
                    if (!string.IsNullOrEmpty(OTPNumber))
                    {
                        var existingApp = changeOfNameService.GetExistingApplicationByRegistrationNumber(registrationNumber);
                        RegistrationRepository registrationRepo = new RegistrationRepository();
                        string generatedOTP = registrationRepo.GetOTP(existingApp.MobileNumber);
                        if (!string.Equals(generatedOTP, OTPNumber))
                        {
                            result.Issuccess = true;
                            result.IsVerified = false;
                            result.Message = "OTP does not match";
                        }
                        else
                        {
                            ChangeOfNameRegistrationModel registrationInfo = new ChangeOfNameRegistrationModel
                            {
                                AccountNo = existingApp.AccountNumber,
                                MobileNo = isLEC ? existingApp.LECMobileNumber : existingApp.MobileNumber,
                                EmailId = "",
                                RegistrationNo = registrationNumber,
                                IsLEC = false
                            };
                            Session["CONAccountDetails"] = registrationInfo;
                            string redirectUrl = this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameApplicationFormPageRevamp);
                            result.Issuccess = true;
                            result.IsVerified = true;
                            result.Message = redirectUrl;
                        }
                    }
                    else if (!changeOfNameService.IsApplicationExistsForTempRegister(registrationNumber))
                    {
                        message = DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/CheckApplicationExistsForAccountNumber", "No application for Change of Name is registered for this Registration Number");
                        result.Issuccess = false;
                        result.Message = message;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var existingApp = changeOfNameService.GetExistingApplicationByRegistrationNumber(registrationNumber);
                        if (existingApp.ApplicationStatusCode == "1")
                        {
                            message = DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/CheckApplicationExistsForAccountNumber", "No application submitted for Change of Name is registered for this Registration Number");
                            result.Issuccess = false;
                            result.Message = message;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        //continue
                        ChangeOfNameRegistrationModel registrationInfo = new ChangeOfNameRegistrationModel
                        {
                            AccountNo = existingApp.AccountNumber,
                            MobileNo = existingApp.MobileNumber,
                            EmailId = "",
                            RegistrationNo = registrationNumber,
                            IsLEC = false
                        };
                        Session["CONAccountDetails"] = null;
                        result.Issuccess = true;
                        //result.Message = redirectUrl;

                        #region Delete Available otp from database for given mobile number
                        RegistrationRepository registrationRepo = new RegistrationRepository();
                        registrationRepo.DeleteOldOtp(registrationInfo.MobileNo);
                        #endregion

                        #region Generate New Otp for given mobile number and save to database
                        var moelRegisteredValidateAccount = new RegisteredValidateAccount
                        {
                            MobileNo = registrationInfo.MobileNo,
                            AccountNo = registrationInfo.AccountNo
                        };
                        string generatedotp = registrationRepo.StoreGeneratedOtp(moelRegisteredValidateAccount);
                        #endregion

                        #region Api call to send SMS of OTP
                        try
                        {
                            var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API for check submitted application",
                              "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=Welcome to Adani Electricity. OTP to check your submitted/saved application for Change of Name on the bill, for A/C no. {1} is {2}&intflag=false"), registrationInfo.MobileNo, registrationInfo.AccountNo.TrimStart('0'), generatedotp);

                            HttpClient client = new HttpClient();
                            client.BaseAddress = new Uri(apiurl);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage response = client.GetAsync(apiurl).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                Log.Error("OTP Api call success for registration", this);
                                result.IsOTPSent = true;
                                result.Message = "OTP sent to your registred mobile number.";// registrationInfo.MobileNo;
                            }
                            else
                            {
                                Log.Error("OTP Api call failed for registration", this);
                                result.IsOTPSent = false;
                                result.Message = "Unable to send OTP to registered mobile number.";
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error("OTP Api call failed for registration" + ex.Message, this);
                            result.IsOTPSent = true;
                            result.Message = "Unable to send OTP to registered mobile number.";// registrationInfo.MobileNo;
                        }
                        #endregion
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at ChangeOfNameCheckApplication Post " + ex.Message, this);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool IsCorrectMobileNumber(String strNumber)
        {
            Regex mobilePattern = new Regex(@"^[0-9]{10}$");
            return mobilePattern.IsMatch(strNumber);
        }

        #endregion Registration

        #region Application Form

        //[HttpGet]
        public ActionResult ChangeOfNameApplicationFormRevamp()
        {
            Session["Message"] = null;
            ChangeOfNameApplicationFromModel model = new ChangeOfNameApplicationFromModel();
            ChangeOfNameService changeOfNameService = new ChangeOfNameService();

            try
            {
                var listAllArea = changeOfNameService.ListAreaPinWorkcenterMapping();
                ChangeOfNameRegistrationModel accountDetailsForCON = new ChangeOfNameRegistrationModel();
                if (Session["CONAccountDetails"] == null)
                {
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                }
                else
                {
                    accountDetailsForCON = (ChangeOfNameRegistrationModel)Session["CONAccountDetails"];
                    if (!string.IsNullOrEmpty(accountDetailsForCON.AccountNoForCheckApplication))
                    {
                        var CONApp = changeOfNameService.GetExistingApplication(accountDetailsForCON.AccountNoForCheckApplication);
                        var CONAppDocuments = changeOfNameService.GetExistingDocument(CONApp.RegistrationSerialNumber);

                        if (CONApp == null)
                        {
                            ViewBag.Message = "No application exists with this Account number!";
                            Session["Message"] = "No application exists with this Account number!";
                            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                        }
                        else if (CONApp.ApplicationStatusCode == "1")
                        {
                            //Saved application already exists - display in editable mode
                            var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(CONApp.AccountNumber);
                            var accountDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(CONApp.AccountNumber);
                            var consumerSD = SapPiService.Services.RequestHandler.GetSecurityDeposityAmountCON(CONApp.AccountNumber);
                            Session["AccountNumber"] = CONApp.AccountNumber.TrimStart('0');
                            model = new ChangeOfNameApplicationFromModel
                            {
                                AccountNo = CONApp.AccountNumber.TrimStart('0'),
                                Aklasse = accountDetails.AKLASSE,
                                IsLEC = accountDetailsForCON.IsLEC,
                                LECMobileNumber = CONApp.LECMobileNumber,
                                LECRegistrationNumber = CONApp.LECRegistrationNumber,
                                Area = CONApp.Area_New,
                                City = CONApp.City,
                                SelectedCity = CONApp.City_New,
                                ConnectionType = consumerDetails.ConnectionType,
                                ConsumerName = consumerDetails.Name,
                                SelectedTitle = CONApp.TitleValue,
                                FirstName = CONApp.FirstName,
                                LastName = CONApp.LastName,
                                MiddleName = CONApp.MiddleName,
                                Name1Joint = CONApp.FirstName,
                                Name2Joint = CONApp.LastName,
                                Landline = CONApp.LandlineNumber,
                                ApplicantType = CONApp.ApplicantTypeCode,
                                OrganizationName = CONApp.OrgName,
                                SelectedPremiseType = CONApp.PremiseTypeCode,
                                MobileNo = CONApp.MobileNumber,
                                EmailId = CONApp.EmailAddress,
                                HouseNumber = CONApp.Housenumber_New,
                                Landmark = CONApp.Landmark_New,

                                MeterNumber = consumerDetails.MeterNumber.TrimStart('0').Substring(0, 2) + "xxxxx" + consumerDetails.MeterNumber.TrimStart('0').Substring(consumerDetails.MeterNumber.TrimStart('0').Length - 1),
                                ExistingEmailId = string.IsNullOrEmpty(consumerDetails.Email) ? consumerDetails.Email : consumerDetails.Email.Substring(0, 2) + "xxxxxxxxxx" + consumerDetails.Email.Substring(consumerDetails.Email.Length - 3),
                                ExistingMobileNumber = string.IsNullOrEmpty(consumerDetails.Mobile) ? consumerDetails.Mobile : consumerDetails.Mobile.Substring(0, 1) + "xxxxxxx" + consumerDetails.Mobile.Substring(consumerDetails.Mobile.Length - 2),

                                ExistingSecurityDepositeAmount = consumerSD,
                                SecurityDepositeAmount = System.Convert.ToDecimal(CONApp.SecurityDepositAmount),
                                Vertrag_Contract = consumerDetails.Vertrag_Contract,
                                Pincode = CONApp.Pincode_New,
                                Street = CONApp.Streetname_New,
                                SelectedSuburb = CONApp.Suburb_New,
                                SelectedBillLanguage = CONApp.BillingLanguage,
                                ConsumerRemark = CONApp.ConsumerRemarks,
                                LandlordName = CONApp.Rented_Ownername,
                                LandlordEmail = CONApp.Rented_Owner_Email,
                                LandlordLandline = CONApp.Rented_Owner_Landline,
                                LandlordMobile = CONApp.Rented_Owner_Mobile,
                                GetExistingDocuments = CONAppDocuments,
                                SelectedPincode = CONApp.Pincode_New,
                                Address = consumerDetails.HouseNumber + ", " + consumerDetails.Street + ", " + consumerDetails.Street2 + ", " + consumerDetails.Street3 + ", " + consumerDetails.City
                                + "," + consumerDetails.PinCode
                            };
                            if (!string.IsNullOrEmpty(model.SelectedPincode) && listAllArea.Any(a => a.PinCode == model.SelectedPincode))
                            {
                                var existingArea = listAllArea.Where(a => a.PinCode == model.SelectedPincode).FirstOrDefault();
                                model.SelectedSuburb = existingArea.Area;

                                var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                                model.CitySelectList = new List<SelectListItem>();
                                if (cityList != null && cityList.Any())
                                {
                                    foreach (var item in cityList)
                                    {
                                        model.CitySelectList.Add(new SelectListItem
                                        {
                                            Text = item,
                                            Value = item
                                        });
                                    }
                                }
                                model.SelectedCity = existingArea.City;
                                if (model.SelectedCity != null)
                                {
                                    var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                                    model.PincodeSelectList = new List<SelectListItem>();
                                    if (pinList != null && pinList.Any())
                                    {
                                        foreach (var item in pinList)
                                        {
                                            model.PincodeSelectList.Add(new SelectListItem
                                            {
                                                Text = item,
                                                Value = item
                                            });
                                        }
                                    }
                                }
                                model.SelectedPincode = existingArea.PinCode;
                            }
                            else
                            {
                                if (model.SelectedSuburb != null)
                                {
                                    var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                                    model.CitySelectList = new List<SelectListItem>();
                                    if (cityList != null && cityList.Any())
                                    {
                                        foreach (var item in cityList)
                                        {
                                            model.CitySelectList.Add(new SelectListItem
                                            {
                                                Text = item,
                                                Value = item
                                            });
                                        }
                                    }
                                }
                                if (model.SelectedCity != null)
                                {
                                    var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                                    model.PincodeSelectList = new List<SelectListItem>();
                                    if (pinList != null && pinList.Any())
                                    {
                                        foreach (var item in pinList)
                                        {
                                            model.PincodeSelectList.Add(new SelectListItem
                                            {
                                                Text = item,
                                                Value = item
                                            });
                                        }
                                    }
                                }
                            }


                            if (CONApp.ApplicationTypeId != null)
                            {
                                model.IsStillLiving = CONApp.ApplicationTypeId == "21" ? "Yes" : "No";
                            }

                            if (CONApp.IsContinueExitingSDvalueOpt != null)
                            {
                                model.IsContinueWithExistingSD = CONApp.IsContinueExitingSDvalueOpt.ToString() == "True" ? "Yes" : "No";
                            }
                            if (CONApp.PaperlessBillingFlag.ToString() != null)
                            {
                                model.IsPaperlessBilling = CONApp.PaperlessBillingFlag.ToString() == "True" ? "Yes" : "No";
                            }
                            if (CONApp.IsAddressChangeRequired.ToString() != null)
                            {
                                model.IsAddressCorrectionRequired = CONApp.IsAddressChangeRequired.ToString() == "True" ? "Yes" : "No";
                            }
                            //var listAllArea = changeOfNameService.ListAreaPinWorkcenterMapping();
                            var applicantionType = model.IsStillLiving == "Yes" ? "21" : "22";
                            var titleValue = "";

                            if (model.ApplicantType == "1" || model.ApplicantType == "3")
                            {
                                if (model.SelectedTitle == "0006")
                                    titleValue = "0006";
                            }
                            else
                            {
                                titleValue = "0005";
                            }

                            #region doc section
                            var listOfDocs = changeOfNameService.GetDocuments(applicantionType, titleValue, model.SelectedPremiseType, model.ApplicantType, model.IsContinueWithExistingSD);

                            //ID Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (doc.AKLASSE == model.Aklasse))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.IDDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.IDDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //ID2 Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID2").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.ID2DocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.ID2DocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //OD Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "OD").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.ODDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.ODDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //PH Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "PH").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.PHDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.PHDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //SD Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "SD").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.SDDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.SDDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //OT Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "OT").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.OTDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.OTDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (model.ApplicantType == "3")
                                    {
                                        model.OTDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = true
                                        });
                                    }
                                }
                            }
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "").ToList())
                            {
                                if (!model.OTDocumentsList.Any(d => d.DocTypeCode == doc.DocumentTypeCode && d.DocName == doc.DocumentDescription))
                                {
                                    model.OTDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = true
                                    });
                                }
                            }
                            #endregion doc section
                        }
                        else
                        {
                            //Get the values - submitted application display read only
                            Session["CONAccountDetails"] = accountDetailsForCON;
                            string redirectUrl = this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameSubmittedApplicationPageRevamp);
                            return Redirect(redirectUrl);
                        }
                    }
                    else if (!string.IsNullOrEmpty(accountDetailsForCON.RegistrationNo))
                    {
                        var CONApp = changeOfNameService.GetExistingApplicationByRegistrationNumber(accountDetailsForCON.RegistrationNo);
                        //Error - If application with this registration number
                        //- Does not exists
                        //- In save as draft status 
                        if (CONApp == null || CONApp.ApplicationStatusCode == "1")
                        {
                            ViewBag.Message = "No application exists with this Registration number!";
                            Session["Message"] = "No application exists with this Registration number!";
                            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                        }
                        else if (CONApp != null)
                        {
                            Session["CONAccountDetails"] = accountDetailsForCON;
                            string redirectUrl = this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameSubmittedApplicationPageRevamp);
                            return Redirect(redirectUrl);
                        }
                        //check if submitted application exists
                        //check status
                        //read only display
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(accountDetailsForCON.AccountNo) && !string.IsNullOrEmpty(accountDetailsForCON.AccountNoForCheckApplication))
                            accountDetailsForCON.AccountNo = accountDetailsForCON.AccountNoForCheckApplication;
                        var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(accountDetailsForCON.AccountNo);
                        var consumerSD = SapPiService.Services.RequestHandler.GetSecurityDeposityAmountCON(accountDetailsForCON.AccountNo);
                        var accountDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(accountDetailsForCON.AccountNo);
                        var isLEC = false;
                        Session["IsLEC"] = isLEC;
                        Session["AccountNumber"] = consumerDetails.CANumber.TrimStart('0');
                        model = new ChangeOfNameApplicationFromModel
                        {
                            AccountNo = consumerDetails.CANumber.TrimStart('0'),
                            Aklasse = accountDetails.AKLASSE,
                            Area = consumerDetails.Street2,
                            City = consumerDetails.City,
                            ConnectionType = consumerDetails.ConnectionType,
                            ConsumerName = consumerDetails.Name,
                            EmailId = consumerDetails.Email,
                            HouseNumber = consumerDetails.HouseNumber,
                            Landmark = consumerDetails.Street3,
                            MobileNo = accountDetailsForCON.MobileNo,
                            LECMobileNumber = accountDetailsForCON.LECMobileNo,
                            LECRegistrationNumber = accountDetailsForCON.LECRegistrationNumber,
                            IsLEC = isLEC,
                            MeterNumber = consumerDetails.MeterNumber.TrimStart('0').Substring(0, 2) + "xxxxx" + consumerDetails.MeterNumber.TrimStart('0').Substring(consumerDetails.MeterNumber.TrimStart('0').Length - 1),
                            ExistingEmailId = string.IsNullOrEmpty(consumerDetails.Email) ? consumerDetails.Email : consumerDetails.Email.Substring(0, 2) + "xxxxxxxxxx" + consumerDetails.Email.Substring(consumerDetails.Email.Length - 3),
                            ExistingMobileNumber = string.IsNullOrEmpty(consumerDetails.Mobile) ? consumerDetails.Mobile : consumerDetails.Mobile.Substring(0, 1) + "xxxxxxx" + consumerDetails.Mobile.Substring(consumerDetails.Mobile.Length - 2),
                            ExistingSecurityDepositeAmount = consumerSD,
                            SecurityDepositeAmount = consumerSD,
                            Vertrag_Contract = consumerDetails.Vertrag_Contract,
                            Pincode = consumerDetails.PinCode,
                            Street = consumerDetails.Street,
                            Address = consumerDetails.HouseNumber + ", " + consumerDetails.Street + ", " + consumerDetails.Street2 + ", " + consumerDetails.Street3 + ", " + consumerDetails.City
                            + "," + consumerDetails.PinCode
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at ChangeOfNameApplicationForm get: " + ex.Message, this);
            }
            return this.View(model);
        }

        [HttpPost]
        public ActionResult ChangeOfNameApplicationFormRevamp(ChangeOfNameApplicationFromModel model, FormCollection form, string SaveAsDraft = null, string SubmitApplication = null, string formActiveId = null)
        {
            Session["Message"] = null;

            model.formActiveId = formActiveId != null ? formActiveId : "";

            if (Session["AccountNumber"] == null || Session["AccountNumber"].ToString() != model.AccountNo)
            {
                return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
            }

            ChangeOfNameService objChangeOfNameService = new ChangeOfNameService();
            try
            {
                var listAllArea = objChangeOfNameService.ListAreaPinWorkcenterMapping();
                var consumerSD = SapPiService.Services.RequestHandler.GetSecurityDeposityAmountCON(model.AccountNo);

                var conapp = objChangeOfNameService.GetExistingApplicationForCARegistration(model.AccountNo);
                List<CONApplicationDocumentDetail> CONAppDocuments = null;

                if (conapp != null)
                    CONAppDocuments = objChangeOfNameService.GetExistingDocument(conapp.RegistrationSerialNumber);


                if (string.IsNullOrEmpty(model.SelectedPremiseType))
                {
                    if (CONAppDocuments != null)
                    {
                        model.GetExistingDocuments = CONAppDocuments;
                    }
                    this.ModelState.AddModelError(nameof(model.SelectedPremiseType), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/Premise type none", "Please select Premise type to move further."));
                    return this.View(model);
                }

                if (!string.IsNullOrEmpty(SaveAsDraft) || !string.IsNullOrEmpty(SubmitApplication))
                {

                    var applicantionType = model.IsStillLiving == "Yes" ? "21" : "22";
                    var titleValue = "";
                    if (model.ApplicantType == "1" || model.ApplicantType == "3")
                    {
                        if (model.SelectedTitle == "0006")
                            titleValue = "0006";
                    }
                    else
                    {
                        titleValue = "0005";
                    }

                    var listOfDocs = objChangeOfNameService.GetDocuments(applicantionType, titleValue, model.SelectedPremiseType, model.ApplicantType, model.IsContinueWithExistingSD);
                    if (!ModelState.IsValid)
                    {
                        model.ExistingSecurityDepositeAmount = consumerSD;
                        if (!string.IsNullOrEmpty(model.SelectedPincode) && listAllArea.Any(a => a.PinCode == model.SelectedPincode))
                        {
                            var existingArea = listAllArea.Where(a => a.PinCode == model.SelectedPincode).FirstOrDefault();
                            model.SelectedSuburb = existingArea.Area;

                            var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                            model.CitySelectList = new List<SelectListItem>();
                            if (cityList != null && cityList.Any())
                            {
                                foreach (var item in cityList)
                                {
                                    model.CitySelectList.Add(new SelectListItem
                                    {
                                        Text = item,
                                        Value = item
                                    });
                                }
                            }
                            model.SelectedCity = existingArea.City;
                            if (model.SelectedCity != null)
                            {
                                var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                                model.PincodeSelectList = new List<SelectListItem>();
                                if (pinList != null && pinList.Any())
                                {
                                    foreach (var item in pinList)
                                    {
                                        model.PincodeSelectList.Add(new SelectListItem
                                        {
                                            Text = item,
                                            Value = item
                                        });
                                    }
                                }
                            }
                            model.SelectedPincode = existingArea.PinCode;
                        }
                        else
                        {
                            if (model.SelectedSuburb != null)
                            {
                                var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                                model.CitySelectList = new List<SelectListItem>();
                                if (cityList != null && cityList.Any())
                                {
                                    foreach (var item in cityList)
                                    {
                                        model.CitySelectList.Add(new SelectListItem
                                        {
                                            Text = item,
                                            Value = item
                                        });
                                    }
                                }
                            }
                            if (model.SelectedCity != null)
                            {
                                var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                                model.PincodeSelectList = new List<SelectListItem>();
                                if (pinList != null && pinList.Any())
                                {
                                    foreach (var item in pinList)
                                    {
                                        model.PincodeSelectList.Add(new SelectListItem
                                        {
                                            Text = item,
                                            Value = item
                                        });
                                    }
                                }
                            }
                        }
                        #region docs
                        //ID Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (doc.AKLASSE == model.Aklasse))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.IDDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.IDDocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                            }
                        }
                        //ID2 Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID2").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.ID2DocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.ID2DocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                            }
                        }
                        //OD Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "OD").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.ODDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.ODDocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                            }
                        }
                        //PH Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "PH").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.PHDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.PHDocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                            }
                        }
                        //SD Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "SD").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.SDDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.SDDocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                            }
                        }
                        //OT Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "OT").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.OTDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.OTDocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (model.ApplicantType == "3")
                                {
                                    model.OTDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = true
                                    });
                                }
                            }
                        }
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "").ToList())
                        {
                            if (!model.OTDocumentsList.Any(d => d.DocTypeCode == doc.DocumentTypeCode && d.DocName == doc.DocumentDescription))
                            {
                                model.OTDocumentsList.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = true
                                });
                            }
                        }
                        if (CONAppDocuments != null)
                        {
                            model.GetExistingDocuments = CONAppDocuments;
                        }
                        else
                            model.GetExistingDocuments = new List<CONApplicationDocumentDetail>();

                        return this.View(model);
                        #endregion
                    }

                    bool validateFile = true;
                    string validateResult = string.Empty;
                    if (Request.Files["file_ID"] != null && Request.Files["file_ID"].ContentLength > 0)
                    {
                        HttpPostedFileBase file = Request.Files["file_ID"];
                        FileUpload1 fu = new FileUpload1();
                        fu.filesize = 2048;
                        string validate = fu.UploadUserFile(file);
                        if (!string.IsNullOrEmpty(validate))
                        {
                            validateResult = validateResult + validate + "\n";
                            validateFile = false;
                        }
                    }
                    if (Request.Files["file_ID2"] != null && Request.Files["file_ID2"].ContentLength > 0)
                    {
                        HttpPostedFileBase file = Request.Files["file_ID2"];
                        FileUpload1 fu = new FileUpload1();
                        fu.filesize = 2048;
                        string validate = fu.UploadUserFile(file);
                        if (!string.IsNullOrEmpty(validate))
                        {
                            validateResult = validateResult + validate + "\n";
                            validateFile = false;
                        }
                    }
                    if (Request.Files["file_PH"] != null && Request.Files["file_PH"].ContentLength > 0)
                    {
                        HttpPostedFileBase file = Request.Files["file_PH"];
                        FileUpload2 fu = new FileUpload2();
                        fu.filesize = 2048;
                        string validate = fu.UploadUserFile(file);
                        if (!string.IsNullOrEmpty(validate))
                        {
                            validateResult = validateResult + validate + "\n";
                            validateFile = false;
                        }
                    }
                    if (Request.Files["file_SD"] != null && Request.Files["file_SD"].ContentLength > 0)
                    {
                        HttpPostedFileBase file = Request.Files["file_SD"];
                        FileUpload1 fu = new FileUpload1();
                        fu.filesize = 2048;
                        string validate = fu.UploadUserFile(file);
                        if (!string.IsNullOrEmpty(validate))
                        {
                            validateResult = validateResult + validate + "\n";
                            validateFile = false;
                        }
                    }
                    if (Request.Files["file_OD"] != null && Request.Files["file_OD"].ContentLength > 0)
                    {
                        HttpPostedFileBase file = Request.Files["file_OD"];
                        FileUpload1 fu = new FileUpload1();
                        fu.filesize = 2048;
                        string validate = fu.UploadUserFile(file);
                        if (!string.IsNullOrEmpty(validate))
                        {
                            validateResult = validateResult + validate + "\n";
                            validateFile = false;
                        }
                    }
                    if (Request.Files["file_OT"] != null && Request.Files["file_OT"].ContentLength > 0)
                    {
                        HttpPostedFileBase file = Request.Files["file_OT"];
                        FileUpload1 fu = new FileUpload1();
                        fu.filesize = 2048;
                        string validate = fu.UploadUserFile(file);
                        if (!string.IsNullOrEmpty(validate))
                        {
                            validateResult = validateResult + validate + "\n";
                            validateFile = false;
                        }
                    }


                    foreach (var doc in listOfDocs.Where(d => d.FLAG_MANDATORY == "X" || d.FLAG_DOCNO != "X").ToList())
                    {

                        HttpPostedFileBase file = Request.Files["file_" + doc.DocumentType];
                        if (file != null && file.ContentLength > 0)
                        {
                            if (doc.DocumentTypeCode == "PH")
                            {
                                FileUpload2 fu = new FileUpload2();
                                fu.filesize = 2048;
                                string validate = fu.UploadUserFile(file);
                                if (!string.IsNullOrEmpty(validate))
                                {
                                    validateResult = validateResult + validate + "\n";
                                    validateFile = false;
                                }
                            }
                            else
                            {
                                FileUpload1 fu = new FileUpload1();
                                fu.filesize = 2048;
                                string validate = fu.UploadUserFile(file);
                                if (!string.IsNullOrEmpty(validate))
                                {
                                    validateResult = validateResult + validate + "\n";
                                    validateFile = false;
                                }
                            }

                        }

                    }
                    if (!validateFile)
                    {
                        model.ExistingSecurityDepositeAmount = consumerSD;
                        if (!string.IsNullOrEmpty(model.SelectedPincode) && listAllArea.Any(a => a.PinCode == model.SelectedPincode))
                        {
                            var existingArea = listAllArea.Where(a => a.PinCode == model.SelectedPincode).FirstOrDefault();
                            model.SelectedSuburb = existingArea.Area;

                            var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                            model.CitySelectList = new List<SelectListItem>();
                            if (cityList != null && cityList.Any())
                            {
                                foreach (var item in cityList)
                                {
                                    model.CitySelectList.Add(new SelectListItem
                                    {
                                        Text = item,
                                        Value = item
                                    });
                                }
                            }
                            model.SelectedCity = existingArea.City;
                            if (model.SelectedCity != null)
                            {
                                var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                                model.PincodeSelectList = new List<SelectListItem>();
                                if (pinList != null && pinList.Any())
                                {
                                    foreach (var item in pinList)
                                    {
                                        model.PincodeSelectList.Add(new SelectListItem
                                        {
                                            Text = item,
                                            Value = item
                                        });
                                    }
                                }
                            }
                            model.SelectedPincode = existingArea.PinCode;
                        }
                        else
                        {
                            if (model.SelectedSuburb != null)
                            {
                                var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                                model.CitySelectList = new List<SelectListItem>();
                                if (cityList != null && cityList.Any())
                                {
                                    foreach (var item in cityList)
                                    {
                                        model.CitySelectList.Add(new SelectListItem
                                        {
                                            Text = item,
                                            Value = item
                                        });
                                    }
                                }
                            }
                            if (model.SelectedCity != null)
                            {
                                var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                                model.PincodeSelectList = new List<SelectListItem>();
                                if (pinList != null && pinList.Any())
                                {
                                    foreach (var item in pinList)
                                    {
                                        model.PincodeSelectList.Add(new SelectListItem
                                        {
                                            Text = item,
                                            Value = item
                                        });
                                    }
                                }
                            }
                        }
                        #region model valid check
                        //ID Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (doc.AKLASSE == model.Aklasse))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.IDDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.IDDocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                            }
                        }
                        //ID2 Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID2").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.ID2DocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.ID2DocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                            }
                        }
                        //OD Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "OD").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.ODDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.ODDocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                            }
                        }
                        //PH Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "PH").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.PHDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.PHDocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                            }
                        }
                        //SD Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "SD").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.SDDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.SDDocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                            }
                        }
                        //OT Documents
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "OT").ToList())
                        {
                            if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                            {
                                if (doc.FLAG_MANDATORY == "X")
                                {
                                    model.OTDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (doc.FLAG_DOCNO == "X")
                                {
                                    model.OTDocumentsListOnly1.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                    });
                                }
                                else if (model.ApplicantType == "3")
                                {
                                    model.OTDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = true
                                    });
                                }
                            }
                        }
                        foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "").ToList())
                        {
                            if (!model.OTDocumentsList.Any(d => d.DocTypeCode == doc.DocumentTypeCode && d.DocName == doc.DocumentDescription))
                            {
                                model.OTDocumentsList.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = true
                                });
                            }
                        }
                        if (CONAppDocuments != null)
                        {
                            model.GetExistingDocuments = CONAppDocuments;
                        }
                        else
                            model.GetExistingDocuments = new List<CONApplicationDocumentDetail>();
                        ViewBag.Message = validateResult;
                        return this.View(model);
                        #endregion
                    }

                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        bool isValid = true;
                        if (!ModelState.IsValid)
                        {
                            isValid = false;
                        }
                        else
                        {
                            #region region Validation based on Individeual/Joint
                            if (model.ApplicantType == "1" || model.ApplicantType == "3")
                            {
                                if (string.IsNullOrEmpty(model.SelectedTitle))
                                {
                                    this.ModelState.AddModelError(nameof(model.SelectedTitle), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/Title none", "Please select Title."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (model.SelectedTitle == "0006")
                                {
                                    if (string.IsNullOrEmpty(model.OrganizationName))
                                    {
                                        this.ModelState.AddModelError(nameof(model.OrganizationName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/OrgName none", "Please enter Organization Name."));
                                        //return this.View(model);
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(model.FirstName))
                                    {
                                        this.ModelState.AddModelError(nameof(model.FirstName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/FirstName none", "Please enter First Name."));
                                        //return this.View(model);
                                        isValid = false;
                                    }
                                    if (string.IsNullOrEmpty(model.LastName))
                                    {
                                        this.ModelState.AddModelError(nameof(model.LastName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/LastName none", "Please enter Last Name."));
                                        //return this.View(model);
                                        isValid = false;
                                    }
                                }
                            }
                            else if (model.ApplicantType == "2")
                            {
                                if (string.IsNullOrEmpty(model.Name1Joint))
                                {
                                    this.ModelState.AddModelError(nameof(model.Name1Joint), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/FirstName1 none", "Please enter Name of Applicant 1."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.Name2Joint))
                                {
                                    this.ModelState.AddModelError(nameof(model.Name2Joint), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/FirstName2 none", "Please enter Name of Applicant 2"));
                                    //return this.View(model);
                                    isValid = false;
                                }
                            }
                            #endregion
                            if (model.SelectedPremiseType == "026" && model.IsAddressCorrectionRequired == "Yes")
                            {
                                if (string.IsNullOrEmpty(model.HouseNumber))
                                {
                                    this.ModelState.AddModelError(nameof(model.HouseNumber), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter House Number."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.Street))
                                {
                                    this.ModelState.AddModelError(nameof(model.Street), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/Street none", "Please enter Street."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.Landmark))
                                {
                                    this.ModelState.AddModelError(nameof(model.Landmark), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/Landmark none", "Please enter Landmark."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.Area))
                                {
                                    this.ModelState.AddModelError(nameof(model.Area), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/Area none", "Please enter Area."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.SelectedSuburb))
                                {
                                    this.ModelState.AddModelError(nameof(model.SelectedSuburb), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/Suburb none", "Please select Suburb."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.SelectedCity))
                                {
                                    this.ModelState.AddModelError(nameof(model.SelectedCity), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/City none", "Please select City."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.SelectedPincode))
                                {
                                    this.ModelState.AddModelError(nameof(model.SelectedPincode), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/Pincode none", "Please select Pincode."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                            }

                            if (model.IsBillingAddressDifferent == "Yes")
                            {
                                if (string.IsNullOrEmpty(model.BillingHouseNumber))
                                {
                                    this.ModelState.AddModelError(nameof(model.BillingHouseNumber), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/BillingHouseNumber none", "Please enter Billing House Number."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.BillingBuildingName))
                                {
                                    this.ModelState.AddModelError(nameof(model.BillingBuildingName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/BillingBuildingName none", "Please enter Billing BuildingName."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.BillingStreet))
                                {
                                    this.ModelState.AddModelError(nameof(model.BillingStreet), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/BillingStreet none", "Please enter Billing Street."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.BillingLandmark))
                                {
                                    this.ModelState.AddModelError(nameof(model.BillingLandmark), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/BillingLandmark none", "Please enter Billing Landmark."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.BillingSelectedSuburb))
                                {
                                    this.ModelState.AddModelError(nameof(model.BillingSelectedSuburb), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/BillingSelectedSuburb none", "Please select Billing Suburb."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.BillingSelectedCity))
                                {
                                    this.ModelState.AddModelError(nameof(model.BillingSelectedCity), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/City none", "Please select Billing City."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.BillingSelectedPincode))
                                {
                                    this.ModelState.AddModelError(nameof(model.BillingSelectedPincode), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/Pincode none", "Please select Billing Pincode."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                            }
                            if (string.IsNullOrEmpty(model.EmailId))
                            {
                                this.ModelState.AddModelError(nameof(model.EmailId), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/EmailId none", "Please enter Email."));
                                //return this.View(model);
                                isValid = false;
                            }
                            if (string.IsNullOrEmpty(model.MobileNo))
                            {
                                this.ModelState.AddModelError(nameof(model.MobileNo), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/ConsumerName none", "Please enter MobileNo."));
                                //return this.View(model);
                                isValid = false;
                            }
                            if (string.IsNullOrEmpty(model.SelectedBillLanguage))
                            {
                                this.ModelState.AddModelError(nameof(model.SelectedBillLanguage), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/SelectedBillLanguage none", "Please select Bill Language."));
                                //return this.View(model);
                                isValid = false;
                            }

                            if (model.SelectedPremiseType == "034" || model.IsRentalProperty == "Yes")
                            {
                                if (string.IsNullOrEmpty(model.LandlordName))
                                {
                                    this.ModelState.AddModelError(nameof(model.LandlordName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/LandlordName none", "Please enter Landlord Name."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                                if (string.IsNullOrEmpty(model.LandlordMobile))
                                {
                                    this.ModelState.AddModelError(nameof(model.LandlordMobile), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/LandlordMobile none", "Please enter Landlord Mobile."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                            }

                            //if (string.IsNullOrEmpty(model.ConsumerRemark))
                            //{
                            //    this.ModelState.AddModelError(nameof(model.ConsumerRemark), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/ConsumerName none", "Please enter Remarks."));
                            //    //return this.View(model);
                            //    isValid = false;
                            //}

                            if (model.IsContinueWithExistingSD == "Yes" && model.SecurityDepositeAmount != consumerSD)
                            {
                                if (model.SecurityDepositeAmount <= 0 || model.SecurityDepositeAmount > consumerSD)
                                {
                                    this.ModelState.AddModelError(nameof(model.SecurityDepositeAmount), DictionaryPhraseRepository.Current.Get("/CON/SD amount/EmailId none", "Security Deposit Amount should be greater than 0 and less than exisiting Security Deposit Amount."));
                                    //return this.View(model);
                                    isValid = false;
                                }
                            }

                            if (CONAppDocuments != null)
                            {
                                var IDDocument = CONAppDocuments != null ? CONAppDocuments.Where(x => x.DocumentTypeCode.Trim() == "ID").FirstOrDefault() : null;
                                var ID2Document = CONAppDocuments != null ? CONAppDocuments.Where(x => x.DocumentTypeCode.Trim() == "ID2").FirstOrDefault() : null;
                                if (model.ApplicantType == "1" || model.ApplicantType == "3")
                                {
                                    if (IDDocument != null)
                                    {
                                        if (string.IsNullOrEmpty(IDDocument.DocumentNumber))
                                        {
                                            if (string.IsNullOrEmpty(form["docnumber_ID"]))
                                            {
                                                this.ModelState.AddModelError(nameof(model.docnumber_ID), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/DocumentNumber1 none", "Please enter Document Number."));
                                                isValid = false;
                                            }
                                            String AllowedChars = @"^[a-zA-Z0-9]([a-zA-Z0-9]|[- /\\.,#()])*$";
                                            if (!Regex.IsMatch(form["docnumber_ID"], AllowedChars))
                                            {
                                                this.ModelState.AddModelError(nameof(model.docnumber_ID2), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/DocumentNumber1 invalid", "Please enter valid Document Number for Applicant 1."));
                                                isValid = false;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (IDDocument != null)
                                    {
                                        if (string.IsNullOrEmpty(IDDocument.DocumentNumber))
                                        {
                                            if (string.IsNullOrEmpty(form["docnumber_ID"]))
                                            {
                                                this.ModelState.AddModelError(nameof(model.docnumber_ID), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/DocumentNumber1 none", "Please enter Document Number for Applicant 1."));
                                                isValid = false;
                                            }
                                            else
                                            {
                                                String AllowedChars = @"^[a-zA-Z]([a-zA-Z0-9]|[-/\\.,#&!()])*$";
                                                if (!Regex.IsMatch(form["docnumber_ID"], AllowedChars))
                                                {
                                                    this.ModelState.AddModelError(nameof(model.docnumber_ID2), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/DocumentNumber1 invalid", "Please enter valid Document Number for Applicant 1."));
                                                    isValid = false;
                                                }
                                            }
                                        }
                                    }
                                    if (ID2Document != null)
                                    {
                                        if (string.IsNullOrEmpty(ID2Document.DocumentNumber))
                                        {
                                            if (string.IsNullOrEmpty(form["docnumber_ID2"]))
                                            {
                                                this.ModelState.AddModelError(nameof(model.docnumber_ID2), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/DocumentNumber2 none", "Please enter Document Number for Applicant 2."));
                                                isValid = false;
                                            }
                                            else
                                            {
                                                String AllowedChars = @"^[a-zA-Z]([a-zA-Z0-9]|[-/\\.,#&!()])*$";
                                                if (!Regex.IsMatch(form["docnumber_ID2"], AllowedChars))
                                                {
                                                    this.ModelState.AddModelError(nameof(model.docnumber_ID2), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/DocumentNumber2 invalid", "Please enter valid Document Number for Applicant 2."));
                                                    isValid = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (Request.Files["file_ID"] != null && Request.Files["file_ID"].ContentLength > 0)
                            {
                                if (model.ApplicantType == "1" || model.ApplicantType == "3")
                                {
                                    if (string.IsNullOrEmpty(form["docnumber_ID"]))
                                    {
                                        this.ModelState.AddModelError(nameof(model.docnumber_ID), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/DocumentNumber1 none", "Please enter Document Number."));
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(form["docnumber_ID"]))
                                    {
                                        this.ModelState.AddModelError(nameof(model.docnumber_ID), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/DocumentNumber1 none", "Please enter Document Number for Applicant 1."));
                                        isValid = false;
                                    }
                                }
                            }
                            if (Request.Files["file_ID2"] != null && Request.Files["file_ID2"].ContentLength > 0)
                            {
                                if (model.ApplicantType == "2")
                                {
                                    if (string.IsNullOrEmpty(form["docnumber_ID2"]))
                                    {
                                        this.ModelState.AddModelError(nameof(model.docnumber_ID2), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/DocumentNumber2 none", "Please enter Document Number for Applicant 2."));
                                        isValid = false;
                                    }
                                }
                            }

                        }
                        if (!isValid)
                        {
                            //ID Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (doc.AKLASSE == model.Aklasse))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.IDDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.IDDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //ID2 Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID2").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.ID2DocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.ID2DocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //OD Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "OD").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.ODDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.ODDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //PH Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "PH").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.PHDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.PHDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //SD Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "SD").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.SDDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.SDDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //OT Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "OT").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.OTDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.OTDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (model.ApplicantType == "3")
                                    {
                                        model.OTDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = true
                                        });
                                    }
                                }
                            }
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "").ToList())
                            {
                                if (!model.OTDocumentsList.Any(d => d.DocTypeCode == doc.DocumentTypeCode && d.DocName == doc.DocumentDescription))
                                {
                                    model.OTDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = true
                                    });
                                }
                            }
                            if (CONAppDocuments != null)
                            {
                                model.GetExistingDocuments = CONAppDocuments;
                            }
                            else
                                model.GetExistingDocuments = new List<CONApplicationDocumentDetail>();
                            return this.View(model);
                        }
                    }

                    string ApplicationTypeId = model.IsStillLiving == "Yes" ? "21" : "22";
                    bool isApplicationReInitiated = false; //if true delete all docs
                    if (conapp != null && (model.SelectedPremiseType != conapp.PremiseTypeCode
                         || model.SelectedTitle != conapp.TitleValue
                         || model.ApplicantType != conapp.ApplicantTypeCode
                         || ApplicationTypeId != conapp.ApplicationTypeId))
                    {
                        isApplicationReInitiated = true;
                    }
                    SaveApplicationResult result = new SaveApplicationResult();

                    var isLEC = false;
                    //validations check
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        result = objChangeOfNameService.SaveApplication(model, true, isLEC);
                        if (result.IsSavedInDatabase == false)
                        {
                            //ViewBag.Message = "An error occured in saving the application, please try again after some time.";
                            Log.Error("Change of Name: CA:" + model.AccountNo + ", mob:" + model.MobileNo + result.Message + ", User message: An error occured in saving the application, please try again after some time.", this);
                            this.Session["UpdateCNBMessage"] = new InfoMessage("An error occured in saving the application, please try again after some time.", InfoMessage.MessageType.Error);
                            //Session["Message"] = "An error occured in saving the application, please try again after some time.";
                            //Session["CONAccountDetails"] = null;
                            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                            //return View(model);
                        }
                    }
                    else if (!string.IsNullOrEmpty(SaveAsDraft))
                    {
                        result = objChangeOfNameService.SaveApplication(model, false, isLEC);
                        if (result.IsSavedInDatabase == false)
                        {
                            //ViewBag.Message = "An error occured in saving the application, please try again after some time.";
                            Log.Error("Change of Name: CA:" + model.AccountNo + ", mob:" + model.MobileNo + result.Message + ", User message: An error occured in saving the application, please try again after some time.", this);
                            //Session["Message"] = "An error occured in saving the application, please try again after some time.";
                            this.Session["UpdateCNBMessage"] = new InfoMessage("An error occured in saving the application, please try again after some time.", InfoMessage.MessageType.Error);
                            //Session["CONAccountDetails"] = null;
                            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                            //return View(model);
                        }
                    }

                    try
                    {
                        Log.Info("CON application saved now save documents for " + model.AccountNo, this);
                        using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                        {
                            CONApplicationDetail savedApplication = dataContext.CONApplicationDetails.Where(m => m.AccountNumber == ChangeOfNameService.FormatAccountNumber(model.AccountNo) && m.ApplicationStatusCode != "4").OrderByDescending(d => d.CreatedDate).FirstOrDefault();
                            if (savedApplication != null)
                            {
                                Log.Info("CON application exist for " + model.AccountNo + ", " + savedApplication.RegistrationSerialNumber, this);
                                try
                                {
                                    if (isApplicationReInitiated)
                                    {
                                        Log.Info("CON application reinitiated for " + model.AccountNo, this);
                                        dataContext.CONApplicationDocumentDetails.DeleteAllOnSubmit(dataContext.CONApplicationDocumentDetails.Where(d => d.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber));
                                        dataContext.SubmitChanges();
                                        Log.Info("CON application reinitiated and deleting docs for " + model.AccountNo, this);
                                    }

                                    CONApplicationDocumentDetail obj;

                                    if (Request.Files["file_ID"] != null && Request.Files["file_ID"].ContentLength > 0)
                                    {
                                        HttpPostedFileBase file = Request.Files["file_ID"];
                                        if (dataContext.CONApplicationDocumentDetails.Any(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "ID"))
                                        {
                                            dataContext.CONApplicationDocumentDetails.DeleteOnSubmit(dataContext.CONApplicationDocumentDetails.First(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "ID"));
                                            dataContext.SubmitChanges();
                                        }

                                        var doc = listOfDocs.Where(d => d.DocumentSerialNumber == Convert.ToInt32(model.SelectedIDDocumentOnly1) && d.DocumentTypeCode.Trim() == "ID").FirstOrDefault();
                                        if (doc != null)
                                        {
                                            byte[] bytes;
                                            using (BinaryReader br = new BinaryReader(file.InputStream))
                                            {
                                                bytes = br.ReadBytes(file.ContentLength);
                                            }

                                            //byte[] compressimage = CompressAnImage(10, bytes);

                                            bool isDocSaved = objChangeOfNameService.SaveDocumentInOracle(savedApplication, file.FileName, bytes, doc, "");
                                            Log.Info("CON Document save details:" + model.AccountNo + ", " + file.FileName + ", " + file.ContentType, this);
                                            obj = CreateDocumentObject(bytes, file.FileName, file.ContentType, doc, model.AccountNo, savedApplication.RegistrationSerialNumber, isDocSaved, form["docnumber_ID"]);
                                            dataContext.CONApplicationDocumentDetails.InsertOnSubmit(obj);
                                        }
                                    }
                                    if (Request.Files["file_ID2"] != null && Request.Files["file_ID2"].ContentLength > 0)
                                    {
                                        if (dataContext.CONApplicationDocumentDetails.Any(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "ID2"))
                                        {
                                            dataContext.CONApplicationDocumentDetails.DeleteOnSubmit(dataContext.CONApplicationDocumentDetails.First(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "ID2"));
                                            dataContext.SubmitChanges();
                                        }
                                        HttpPostedFileBase file = Request.Files["file_ID2"];
                                        var doc = listOfDocs.Where(d => d.DocumentSerialNumber == Convert.ToInt32(model.SelectedID2DocumentOnly1) && d.DocumentTypeCode.Trim() == "ID2").FirstOrDefault();
                                        if (doc != null)
                                        {
                                            byte[] bytes;
                                            using (BinaryReader br = new BinaryReader(file.InputStream))
                                            {
                                                bytes = br.ReadBytes(file.ContentLength);
                                            }

                                            bool isDocSaved = objChangeOfNameService.SaveDocumentInOracle(savedApplication, file.FileName, bytes, doc, "");
                                            Log.Info("CON Document save details:" + model.AccountNo + ", " + file.FileName + ", " + file.ContentType, this);
                                            obj = CreateDocumentObject(bytes, file.FileName, file.ContentType, doc, model.AccountNo, savedApplication.RegistrationSerialNumber, isDocSaved, form["docnumber_ID2"]);
                                            dataContext.CONApplicationDocumentDetails.InsertOnSubmit(obj);
                                        }
                                    }
                                    if (Request.Files["file_PH"] != null && Request.Files["file_PH"].ContentLength > 0)
                                    {
                                        if (dataContext.CONApplicationDocumentDetails.Any(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "PH"))
                                        {
                                            dataContext.CONApplicationDocumentDetails.DeleteOnSubmit(dataContext.CONApplicationDocumentDetails.First(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "PH"));
                                            dataContext.SubmitChanges();
                                        }
                                        HttpPostedFileBase file = Request.Files["file_PH"];
                                        var doc = listOfDocs.Where(d => d.DocumentSerialNumber == Convert.ToInt32(model.SelectedPHDocumentOnly1)).FirstOrDefault();
                                        if (doc != null)
                                        {
                                            byte[] bytes;
                                            using (BinaryReader br = new BinaryReader(file.InputStream))
                                            {
                                                bytes = br.ReadBytes(file.ContentLength);
                                            }

                                            bool isDocSaved = objChangeOfNameService.SaveDocumentInOracle(savedApplication, file.FileName, bytes, doc, "");
                                            Log.Info("CON Document save details:" + model.AccountNo + ", " + file.FileName + ", " + file.ContentType, this);
                                            obj = CreateDocumentObject(bytes, file.FileName, file.ContentType, doc, model.AccountNo, savedApplication.RegistrationSerialNumber, isDocSaved);
                                            dataContext.CONApplicationDocumentDetails.InsertOnSubmit(obj);

                                        }
                                    }
                                    if (Request.Files["file_SD"] != null && Request.Files["file_SD"].ContentLength > 0)
                                    {
                                        if (dataContext.CONApplicationDocumentDetails.Any(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "SD"))
                                        {
                                            dataContext.CONApplicationDocumentDetails.DeleteOnSubmit(dataContext.CONApplicationDocumentDetails.First(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "SD"));
                                            dataContext.SubmitChanges();
                                        }
                                        HttpPostedFileBase file = Request.Files["file_Sd"];
                                        var doc = listOfDocs.Where(d => d.DocumentSerialNumber == Convert.ToInt32(model.SelectedSDDocumentOnly1)).FirstOrDefault();
                                        if (doc != null)
                                        {
                                            byte[] bytes;
                                            using (BinaryReader br = new BinaryReader(file.InputStream))
                                            {
                                                bytes = br.ReadBytes(file.ContentLength);
                                            }

                                            bool isDocSaved = objChangeOfNameService.SaveDocumentInOracle(savedApplication, file.FileName, bytes, doc, "");
                                            Log.Info("CON Document save details:" + model.AccountNo + ", " + file.FileName + ", " + file.ContentType, this);
                                            obj = CreateDocumentObject(bytes, file.FileName, file.ContentType, doc, model.AccountNo, savedApplication.RegistrationSerialNumber, isDocSaved);
                                            dataContext.CONApplicationDocumentDetails.InsertOnSubmit(obj);

                                        }
                                    }
                                    if (Request.Files["file_OD"] != null && Request.Files["file_OD"].ContentLength > 0)
                                    {
                                        if (dataContext.CONApplicationDocumentDetails.Any(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "OD"))
                                        {
                                            dataContext.CONApplicationDocumentDetails.DeleteOnSubmit(dataContext.CONApplicationDocumentDetails.First(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "OD"));
                                            dataContext.SubmitChanges();
                                        }
                                        HttpPostedFileBase file = Request.Files["file_OD"];
                                        var doc = listOfDocs.Where(d => d.DocumentSerialNumber == Convert.ToInt32(model.SelectedODDocumentOnly1)).FirstOrDefault();
                                        if (doc != null)
                                        {
                                            byte[] bytes;
                                            using (BinaryReader br = new BinaryReader(file.InputStream))
                                            {
                                                bytes = br.ReadBytes(file.ContentLength);
                                            }

                                            bool isDocSaved = objChangeOfNameService.SaveDocumentInOracle(savedApplication, file.FileName, bytes, doc, "");
                                            Log.Info("CON Document save details:" + model.AccountNo + ", " + file.FileName + ", " + file.ContentType, this);
                                            obj = CreateDocumentObject(bytes, file.FileName, file.ContentType, doc, model.AccountNo, savedApplication.RegistrationSerialNumber, isDocSaved);
                                            dataContext.CONApplicationDocumentDetails.InsertOnSubmit(obj);

                                        }
                                    }
                                    if (Request.Files["file_OT"] != null && Request.Files["file_OT"].ContentLength > 0)
                                    {
                                        if (dataContext.CONApplicationDocumentDetails.Any(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "OT"))
                                        {
                                            dataContext.CONApplicationDocumentDetails.DeleteOnSubmit(dataContext.CONApplicationDocumentDetails.First(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == "OT"));
                                            dataContext.SubmitChanges();
                                        }
                                        HttpPostedFileBase file = Request.Files["file_OT"];
                                        var doc = listOfDocs.Where(d => d.DocumentSerialNumber == Convert.ToInt32(model.SelectedOTDocumentOnly1)).FirstOrDefault();
                                        if (doc != null)
                                        {
                                            byte[] bytes;
                                            using (BinaryReader br = new BinaryReader(file.InputStream))
                                            {
                                                bytes = br.ReadBytes(file.ContentLength);
                                            }

                                            bool isDocSaved = objChangeOfNameService.SaveDocumentInOracle(savedApplication, file.FileName, bytes, doc, "");
                                            Log.Info("CON Document save details:" + model.AccountNo + ", " + file.FileName + ", " + file.ContentType, this);

                                            obj = CreateDocumentObject(bytes, file.FileName, file.ContentType, doc, model.AccountNo, savedApplication.RegistrationSerialNumber, isDocSaved);
                                            dataContext.CONApplicationDocumentDetails.InsertOnSubmit(obj);

                                        }
                                    }


                                    foreach (var doc in listOfDocs.Where(d => d.FLAG_MANDATORY == "X" || d.FLAG_DOCNO != "X").ToList())
                                    {
                                        HttpPostedFileBase file = Request.Files["file_" + doc.DocumentType];
                                        if (file != null && file.ContentLength > 0)
                                        {
                                            if (dataContext.CONApplicationDocumentDetails.Any(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == doc.DocumentTypeCode.Trim() && x.DocumentType == doc.DocumentType))
                                            {
                                                dataContext.CONApplicationDocumentDetails.DeleteOnSubmit(dataContext.CONApplicationDocumentDetails.First(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentTypeCode.Trim() == doc.DocumentTypeCode.Trim() && x.DocumentType == doc.DocumentType));
                                                dataContext.SubmitChanges();
                                            }
                                            byte[] bytes;
                                            using (BinaryReader br = new BinaryReader(file.InputStream))
                                            {
                                                bytes = br.ReadBytes(file.ContentLength);
                                            }

                                            bool isDocSaved = objChangeOfNameService.SaveDocumentInOracle(savedApplication, file.FileName, bytes, doc, "");

                                            Log.Info("CON Document save details:" + model.AccountNo + ", " + file.FileName + ", " + file.ContentType, this);

                                            var documentNumber = form["docnumber_" + doc.DocumentSerialNumber.ToString()];

                                            if (doc.DocumentType == "178" && doc.DocumentTypeCode == "ID")
                                            {
                                                documentNumber = form["docnumber_ID"];
                                                if (form["docnumber_ID"] != null)
                                                {
                                                    string s = form["docnumber_ID"];
                                                    string[] values = s.Split(',');
                                                    if (values.Length > 0)
                                                        documentNumber = values[0];
                                                }
                                            }
                                            else if (doc.DocumentType == "437" && doc.DocumentTypeCode == "ID")
                                            {
                                                documentNumber = form["docnumber_ID"];
                                                if (form["docnumber_ID"] != null)
                                                {
                                                    string s = form["docnumber_ID"];
                                                    string[] values = s.Split(',');
                                                    if (values.Length > 1)
                                                        documentNumber = values[1];
                                                }
                                            }

                                            obj = CreateDocumentObject(bytes, file.FileName, file.ContentType, doc, model.AccountNo, savedApplication.RegistrationSerialNumber, isDocSaved, documentNumber);
                                            dataContext.CONApplicationDocumentDetails.InsertOnSubmit(obj);

                                        }

                                    }
                                    dataContext.SubmitChanges();
                                }
                                catch (Exception ex)
                                {
                                    Log.Error("CON Document save:" + ex.Message, this);
                                    dataContext.SubmitChanges();
                                    bool r = objChangeOfNameService.ChangeStatusToSave(savedApplication);

                                    string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ChangeOfName/PopupMessageOnSaveOnError", "Your application is saved successfully. The saved application can be accessed via the 'Check status of Application / Access Draft Application' Section on the login screen. Some documents are not uploaded, please upload them back and submit.");

                                    //ViewBag.Message = messagetobedisplayed;
                                    //Session["Message"] = messagetobedisplayed;
                                    this.Session["UpdateCNBMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Success);

                                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                                }


                                if (!string.IsNullOrEmpty(SubmitApplication))
                                {
                                    //form 16.1
                                    try
                                    {
                                        //check if other documents are uploaded
                                        //If there are no documents for the applciation 
                                        //do not submit
                                        //move it to save state
                                        int doccount = objChangeOfNameService.GetDocCount(savedApplication.RegistrationSerialNumber);
                                        int docCountFromMaster = objChangeOfNameService.GetDocCountForApplication(savedApplication);
                                        if (doccount < docCountFromMaster)
                                        {
                                            Log.Info("CON Submit error, doccount is " + doccount + " save only.", this);

                                            bool r = objChangeOfNameService.ChangeStatusToSave(savedApplication);

                                            string messagetobedisplayed1 = DictionaryPhraseRepository.Current.Get("/ChangeOfName/PopupMessageOnSaveOnError", "Your application is saved successfully. The saved application can be accessed via the 'Check status of Application / Access Draft Application' Section on the login screen. Some documents are not uploaded, please upload them back and submit.");
                                            this.Session["UpdateCNBMessage"] = new InfoMessage(messagetobedisplayed1, InfoMessage.MessageType.Info);
                                            //ViewBag.Message = messagetobedisplayed1;
                                            //Session["Message"] = messagetobedisplayed1;
                                            //Session["CONAccountDetails"] = null;
                                            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                                        }
                                        byte[] getPDFstream = CreatePDF(model.AccountNo);
                                        var doc = objChangeOfNameService.GetForm16DocMaster();
                                        if (doc != null)
                                        {
                                            if (dataContext.CONApplicationDocumentDetails.Any(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentDescription.Contains("Change Of Name 16.1 form")))
                                            {
                                                dataContext.CONApplicationDocumentDetails.DeleteOnSubmit(dataContext.CONApplicationDocumentDetails.First(x => x.RegistrationSerialNumber == savedApplication.RegistrationSerialNumber && x.DocumentDescription.Contains("Change Of Name 16.1 form")));
                                                dataContext.SubmitChanges();
                                            }

                                            bool isDocSaved = objChangeOfNameService.SaveDocumentInOracle(savedApplication, "CHN_" + savedApplication.RegistrationSerialNumber + ".pdf", getPDFstream, doc, "");

                                            DateTime? sappost = null;
                                            if (isDocSaved)
                                                sappost = DateTime.Now;
                                            CONApplicationDocumentDetail objPDF = new CONApplicationDocumentDetail
                                            {
                                                Id = Guid.NewGuid(),
                                                DocumentType = doc.DocumentType,
                                                DocumentTypeCode = doc.DocumentTypeCode,
                                                CreatedBy = ChangeOfNameService.FormatAccountNumber(model.AccountNo),
                                                CreatedDate = DateTime.Now,
                                                DocumentChecklistSerialNumber = doc.DocumentSerialNumber.ToString(),
                                                DocumentData = getPDFstream,
                                                DocumentDescription = doc.DocumentDescription,
                                                DocumentName = "CHN_" + savedApplication.RegistrationSerialNumber + ".pdf",
                                                DocumentDataContenttype = "blob",
                                                DocumentNumber = model.AccountNo,
                                                IsSentToSAP = isDocSaved,
                                                RegistrationSerialNumber = savedApplication.RegistrationSerialNumber,
                                                SAPSentDate = sappost
                                            };

                                            dataContext.CONApplicationDocumentDetails.InsertOnSubmit(objPDF);
                                            dataContext.SubmitChanges();
                                        }
                                        else
                                        {
                                            Log.Error("Unable to get Doc master to generate Form 16.1", this);

                                            bool r = objChangeOfNameService.ChangeStatusToSave(savedApplication);

                                            string messagetobedisplayed1 = DictionaryPhraseRepository.Current.Get("/ChangeOfName/PopupMessageOnSaveOnError", "Your application is saved successfully. The saved application can be accessed via the 'Check status of Application / Access Draft Application' Section on the login screen. Some documents are not uploaded, please upload them back and submit.");
                                            this.Session["UpdateCNBMessage"] = new InfoMessage(messagetobedisplayed1, InfoMessage.MessageType.Info);
                                            //ViewBag.Message = messagetobedisplayed1;
                                            //Session["Message"] = messagetobedisplayed1;
                                            //Session["CONAccountDetails"] = null;
                                            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error("CON PDF not Generated after Form Submit CA: " + model.AccountNo + " error:" + ex.Message, this);

                                        bool r = objChangeOfNameService.ChangeStatusToSave(savedApplication);

                                        string messagetobedisplayed1 = DictionaryPhraseRepository.Current.Get("/ChangeOfName/PopupMessageOnSaveOnError", "Your application is saved successfully. The saved application can be accessed via the 'Check status of Application / Access Draft Application' Section on the login screen. Some documents are not uploaded, please upload them back and submit.");
                                        this.Session["UpdateCNBMessage"] = new InfoMessage(messagetobedisplayed1, InfoMessage.MessageType.Info);
                                        //ViewBag.Message = messagetobedisplayed1;
                                        //Session["Message"] = messagetobedisplayed1;
                                        //Session["CONAccountDetails"] = null;
                                        return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                                    }

                                    ViewBag.IsSuccessful = true;
                                    ViewBag.IsSubmit = true;

                                    string messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/PopupMessageOnSubmit", "Your application is submitted successfully. The application can be tracked via the 'Check status of Application / Access Draft Application' Section on the login screen with Reference No. {0}."), savedApplication.TempRegistrationSerialNumber);
                                    this.Session["UpdateCNBMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Success);
                                    //ViewBag.Message = messagetobedisplayed;
                                    //Session["Message"] = messagetobedisplayed;
                                    Session["AlertMessage"] = DictionaryPhraseRepository.Current.Get("/ChangeOfName/AlertMessageOnSubmit", "On account of the current COVID-19 situation, there might be some accounts for which the billing has been estimated. For such cases, the Change of Name request shall be kept on hold and will be processed after generation of the bill based on actual consumption. We regret the inconvenience caused.");

                                    
                                    //Session["CONAccountDetails"] = null;
                                    string accountNumberTrimmed = savedApplication.AccountNumber.TrimStart('0');
                                    #region Api call to send SMS - New mobile number for CON application submitted successfully
                                    try
                                    {
                                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API for submission",
                                            "http://push3.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=relialt&pass=relialt&appid=relialt&subappid=relialt&contenttype=1&to={0}&from=ADANIE&text=Thank you for submitting your request for name change on the bill, for account no. {1}. Reference reg. no. is {2}. Adani Electricity&selfid=true&alert=1&dlrreq=true"), model.MobileNo, accountNumberTrimmed, savedApplication.TempRegistrationSerialNumber);

                                        Log.Info("CON SubmitApplication SMS Api call URL: " + apiurl, this);
                                        HttpClient client = new HttpClient();
                                        client.BaseAddress = new Uri(apiurl);
                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                                        if (response.IsSuccessStatusCode)
                                        {
                                            Log.Info("CON SubmitApplication SMS Api call success for submit to new number: " + apiurl, this);
                                        }
                                        else
                                        {
                                            Log.Info("CON SubmitApplication SMS Api call failed for submit to new number: " + apiurl, this);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error("CON SubmitApplication SMS Api call success for submit to new number: " + model.AccountNo + " error:" + ex.Message, this);
                                    }
                                    #endregion

                                    #region Api call to send SMS - Old mobile number for Intimation that CON application submitted againts your CA number
                                    try
                                    {
                                        var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNo);

                                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API for submission to old number",
                                       "http://push3.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=relialt&pass=relialt&appid=relialt&subappid=relialt&contenttype=1&to={0}&from=ADANIE&text=A request for Change of Name on the bill has been received for account no. {1}. In case, you wish to raise any objection to this request, pls e-mail us on Helpdesk.Mumbaielectricity@adani.com. Adani Electricity&selfid=true&alert=1&dlrreq=true"), consumerDetails.Mobile, accountNumberTrimmed);
                                        Log.Info("CON SubmitApplication SMS to old user Api call URL: " + apiurl, this);
                                        HttpClient client = new HttpClient();
                                        client.BaseAddress = new Uri(apiurl);
                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                                        if (response.IsSuccessStatusCode)
                                        {
                                            Log.Info("CON SubmitApplication SMS Api call success for submit to old number: " + apiurl, this);
                                        }
                                        else
                                        {
                                            Log.Info("CON SubmitApplication SMS Api call failed for submit to old number: " + apiurl, this);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error("CON SubmitApplication SMS Api call error for submit to old number: " + model.AccountNo + " error:" + ex.Message, this);
                                    }
                                    #endregion

                                    #region Api call to send SMS - To LEC if applied by LEC
                                    if (model.IsLEC)
                                    {
                                        try
                                        {
                                            var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNo);

                                            var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API form submission to LEC number",
                                           "http://push3.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=relialt&pass=relialt&appid=relialt&subappid=relialt&contenttype=1&to={0}&from=ADANIE&text=We confirm the receipt of the Change of Name application. The reference no. for the application is {1}. The request shall be reviewed and processed at the earliest. To check status of this application click here: https://www.adanielectricity.com/changeofnameregistration Adani Electricity&selfid=true&alert=1&dlrreq=true"), model.LECMobileNumber, accountNumberTrimmed);
                                            Log.Info("CON SubmitApplication SMS to old user Api call URL: " + apiurl, this);
                                            HttpClient client = new HttpClient();
                                            client.BaseAddress = new Uri(apiurl);
                                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                            HttpResponseMessage response = client.GetAsync(apiurl).Result;
                                            if (response.IsSuccessStatusCode)
                                            {
                                                Log.Info("CON SubmitApplication SMS Api call success for submit to LEC: " + apiurl, this);
                                            }
                                            else
                                            {
                                                Log.Info("CON SubmitApplication SMS Api call failed for submit to LEC: " + apiurl, this);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Log.Error("CON SubmitApplication SMS Api call error for submit to LEC: " + model.AccountNo + " error:" + ex.Message, this);
                                        }
                                    }
                                    #endregion

                                    if (model.IsLEC)
                                        return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameLECRegistrationPage));
                                    else
                                        return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                                }
                                else if (!string.IsNullOrEmpty(SaveAsDraft))
                                {
                                    ViewBag.IsSuccessful = true;
                                    ViewBag.IsSubmit = false;

                                    string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ChangeOfName/PopupMessageOnSave", "Your application is saved successfully. The saved application can be accessed via the 'Check status of Application / Access Draft Application' Section on the login screen.");
                                    this.Session["UpdateCNBMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Success);
                                    //ViewBag.Message = messagetobedisplayed;
                                    //Session["Message"] = messagetobedisplayed;
                                    Session["AlertMessage"] = null;
                                    Session["CONAccountDetails"] = null;
                                    if (model.IsLEC)
                                        return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameLECRegistrationPage));
                                    else
                                        return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = "An error occured in saving the documents, please try again after some time.";
                        Log.Error("Change of Name: CA:" + model.AccountNo + ", mob:" + model.MobileNo + e.Message, this);
                        return View(model);
                    }
                }
                else
                {
                    //Flat in registrered society
                    if (model.SelectedPremiseType == "026" && model.IsAddressCorrectionRequired == "Yes" && string.IsNullOrEmpty(model.HouseNumber) && string.IsNullOrEmpty(model.Street))
                    {
                        var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNo);
                        model.HouseNumber = consumerDetails.HouseNumber.ToUpper();
                        model.Street = consumerDetails.Street.ToUpper();
                        model.Area = consumerDetails.Street2.ToUpper();
                        model.Landmark = consumerDetails.Street3.ToUpper();

                        if (!string.IsNullOrEmpty(consumerDetails.PinCode) && listAllArea.Any(a => a.PinCode.ToUpper() == consumerDetails.PinCode.ToUpper()))
                        {
                            var existingArea = listAllArea.Where(a => a.PinCode.ToUpper() == consumerDetails.PinCode.ToUpper()).FirstOrDefault();
                            model.SelectedSuburb = existingArea.Area;

                            var cityList = listAllArea.Where(a => a.Area.ToUpper() == model.SelectedSuburb.ToUpper()).Select(c => c.City).Distinct().ToList();
                            model.CitySelectList = new List<SelectListItem>();
                            if (cityList != null && cityList.Any())
                            {
                                foreach (var item in cityList)
                                {
                                    model.CitySelectList.Add(new SelectListItem
                                    {
                                        Text = item,
                                        Value = item
                                    });
                                }
                            }
                            model.SelectedCity = existingArea.City;
                            if (model.SelectedCity != null)
                            {
                                var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City.ToUpper() == model.SelectedCity.ToUpper()).Select(c => c.PinCode).Distinct().ToList();
                                model.PincodeSelectList = new List<SelectListItem>();
                                if (pinList != null && pinList.Any())
                                {
                                    foreach (var item in pinList)
                                    {
                                        model.PincodeSelectList.Add(new SelectListItem
                                        {
                                            Text = item,
                                            Value = item
                                        });
                                    }
                                }
                            }
                            model.SelectedPincode = existingArea.PinCode;
                        }
                        //model.SelectedSuburb = consumerDetails.Street3;
                        //model.SelectedCity = consumerDetails.City;
                        //model.SelectedPincode = consumerDetails.PinCode;
                    }
                    if (model.SelectedPremiseType == "034") //Rental/Pagdi
                    {
                        model.IsRentalProperty = "Yes";
                    }
                    //common services
                    if (model.SelectedPremiseType == "009")
                    {
                        model.TitleSelectList = new List<SelectListItem> {
                            new SelectListItem{ Value="0006", Text="M/S"}
                            //new SelectListItem{ Value="0002", Text="Mr."},
                            //new SelectListItem{ Value="0001", Text="Ms."}
                        };
                        model.SelectedTitle = "0006";
                        model.ApplicantType = "1";
                    }
                    else
                    {
                        model.TitleSelectList = new List<SelectListItem> {
                            new SelectListItem{ Value="0006", Text="M/S"},
                            new SelectListItem{ Value="0002", Text="Mr."},
                            new SelectListItem{ Value="0001", Text="Ms."}
                        };
                        //model.SelectedTitle = null;
                        //model.ApplicantType = "1";
                    }
                    if (model.IsContinueWithExistingSD == "Yes")
                    {
                        model.ExistingSecurityDepositeAmount = consumerSD;
                        if (model.SecurityDepositeAmount == 0)
                            model.SecurityDepositeAmount = consumerSD;
                    }

                    if (model.SelectedSuburb != null)
                    {
                        var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                        model.CitySelectList = new List<SelectListItem>();
                        if (cityList != null && cityList.Any())
                        {
                            foreach (var item in cityList)
                            {
                                model.CitySelectList.Add(new SelectListItem
                                {
                                    Text = item,
                                    Value = item
                                });
                            }
                        }
                    }
                    if (model.SelectedCity != null)
                    {
                        var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                        model.PincodeSelectList = new List<SelectListItem>();
                        if (pinList != null && pinList.Any())
                        {
                            foreach (var item in pinList)
                            {
                                model.PincodeSelectList.Add(new SelectListItem
                                {
                                    Text = item,
                                    Value = item
                                });
                            }
                        }
                    }
                    if (model.BillingSelectedSuburb != null)
                    {
                        var cityList = listAllArea.Where(a => a.Area == model.BillingSelectedSuburb).Select(c => c.City).Distinct().ToList();
                        model.BillingCitySelectList = new List<SelectListItem>();
                        if (cityList != null && cityList.Any())
                        {
                            foreach (var item in cityList)
                            {
                                model.BillingCitySelectList.Add(new SelectListItem
                                {
                                    Text = item,
                                    Value = item
                                });
                            }
                        }
                    }
                    if (model.BillingSelectedCity != null)
                    {
                        var pinList = listAllArea.Where(a => a.Area == model.BillingSelectedSuburb && a.City == model.BillingSelectedCity).Select(c => c.PinCode).Distinct().ToList();
                        model.BillingPincodeSelectList = new List<SelectListItem>();
                        if (pinList != null && pinList.Any())
                        {
                            foreach (var item in pinList)
                            {
                                model.BillingPincodeSelectList.Add(new SelectListItem
                                {
                                    Text = item,
                                    Value = item
                                });
                            }
                        }
                    }

                    var applicantionType = model.IsStillLiving == "Yes" ? "21" : "22";
                    var titleValue = "";
                    if (model.ApplicantType == "1" || model.ApplicantType == "3")
                    {
                        if (model.SelectedTitle == "0006")
                            titleValue = "0006";
                    }
                    else
                    {
                        titleValue = "0005";
                    }

                    var listOfDocs = objChangeOfNameService.GetDocuments(applicantionType, titleValue, model.SelectedPremiseType, model.ApplicantType, model.IsContinueWithExistingSD);
                    //ID Documents
                    foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID").ToList())
                    {
                        if (string.IsNullOrEmpty(doc.AKLASSE) || (doc.AKLASSE == model.Aklasse))
                        {
                            if (doc.FLAG_MANDATORY == "X")
                            {
                                model.IDDocumentsList.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                            else if (doc.FLAG_DOCNO == "X")
                            {
                                model.IDDocumentsListOnly1.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                        }
                    }
                    //ID2 Documents
                    foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID2").ToList())
                    {
                        if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                        {
                            if (doc.FLAG_MANDATORY == "X")
                            {
                                model.ID2DocumentsList.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                            else if (doc.FLAG_DOCNO == "X")
                            {
                                model.ID2DocumentsListOnly1.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                        }
                    }
                    //OD Documents
                    foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "OD").ToList())
                    {
                        if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                        {
                            if (doc.FLAG_MANDATORY == "X")
                            {
                                model.ODDocumentsList.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                            else if (doc.FLAG_DOCNO == "X")
                            {
                                model.ODDocumentsListOnly1.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                        }
                    }
                    //PH Documents
                    foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "PH").ToList())
                    {
                        if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                        {
                            if (doc.FLAG_MANDATORY == "X")
                            {
                                model.PHDocumentsList.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                            else if (doc.FLAG_DOCNO == "X")
                            {
                                model.PHDocumentsListOnly1.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                        }
                    }
                    //SD Documents
                    foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "SD").ToList())
                    {
                        if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                        {
                            if (doc.FLAG_MANDATORY == "X")
                            {
                                model.SDDocumentsList.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                            else if (doc.FLAG_DOCNO == "X")
                            {
                                model.SDDocumentsListOnly1.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                        }
                    }
                    //OT Documents
                    foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "OT").ToList())
                    {
                        if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                        {
                            if (doc.FLAG_MANDATORY == "X")
                            {
                                model.OTDocumentsList.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                            else if (doc.FLAG_DOCNO == "X")
                            {
                                model.OTDocumentsListOnly1.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                });
                            }
                            else if (model.ApplicantType == "3")
                            {
                                model.OTDocumentsList.Add(new DocumentCheckList
                                {
                                    DocName = doc.DocumentDescription,
                                    DocId = doc.Id.ToString(),
                                    DocType = doc.DocumentType,
                                    DocTypeCode = doc.DocumentTypeCode,
                                    DocSerialNumber = doc.DocumentSerialNumber,
                                    IsMandatory = true
                                });
                            }
                        }
                    }
                    foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "").ToList())
                    {
                        if (!model.OTDocumentsList.Any(d => d.DocTypeCode == doc.DocumentTypeCode && d.DocName == doc.DocumentDescription))
                        {
                            model.OTDocumentsList.Add(new DocumentCheckList
                            {
                                DocName = doc.DocumentDescription,
                                DocId = doc.Id.ToString(),
                                DocType = doc.DocumentType,
                                DocTypeCode = doc.DocumentTypeCode,
                                DocSerialNumber = doc.DocumentSerialNumber,
                                IsMandatory = true
                            });
                        }
                    }
                    //if Premise type, title, applicant type or Application type changed then all documents will get revised
                    if (conapp != null && CONAppDocuments != null)
                    {
                        string ApplicationTypeId = model.IsStillLiving == "Yes" ? "21" : "22";
                        if (model.SelectedPremiseType == conapp.PremiseTypeCode
                             && model.SelectedTitle == conapp.TitleValue
                             && model.ApplicantType == conapp.ApplicantTypeCode
                             && ApplicationTypeId == conapp.ApplicationTypeId)
                        {
                            model.GetExistingDocuments = CONAppDocuments;
                        }
                        else
                            model.GetExistingDocuments = new List<CONApplicationDocumentDetail>();
                    }
                    else if (model.GetExistingDocuments == null)
                        model.GetExistingDocuments = new List<CONApplicationDocumentDetail>();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at ChangeOfNameApplicationForm:" + ex.Message, this);
            }
            //Session["CONAccountDetails"] = null;
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangeOfNameSubmittedApplicationRevamp()
        {
            Session["Message"] = null;
            ChangeOfNameApplicationFromModel model = new ChangeOfNameApplicationFromModel();
            ChangeOfNameService changeOfNameService = new ChangeOfNameService();
            ChangeOfNameRegistrationModel accountDetailsForCON = new ChangeOfNameRegistrationModel();
            try
            {
                if (Session["CONAccountDetails"] == null)
                {
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                }
                else
                {
                    accountDetailsForCON = (ChangeOfNameRegistrationModel)Session["CONAccountDetails"];
                    if (!string.IsNullOrEmpty(accountDetailsForCON.AccountNoForCheckApplication))
                    {
                        var CONApp = changeOfNameService.GetExistingApplication(accountDetailsForCON.AccountNoForCheckApplication);
                        var CONAppDocuments = changeOfNameService.GetExistingDocument(CONApp.RegistrationSerialNumber);

                        if (CONApp == null)
                        {
                            ViewBag.Message = "No application exists with this Account number!";
                            Session["Message"] = "No application exists with this Account number!";
                            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                        }
                        else if (CONApp.ApplicationStatusCode == "2" || CONApp.ApplicationStatusCode == "3" || CONApp.ApplicationStatusCode == "4")
                        {
                            //Saved application already exists - display in editable mode
                            var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(CONApp.AccountNumber);
                            var accountDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(CONApp.AccountNumber);
                            var consumerSD = SapPiService.Services.RequestHandler.GetSecurityDeposityAmountCON(CONApp.AccountNumber);
                            model = new ChangeOfNameApplicationFromModel
                            {
                                LECMobileNumber = CONApp.LECMobileNumber,
                                LECRegistrationNumber = CONApp.LECRegistrationNumber,
                                IsLEC = accountDetailsForCON.IsLEC,
                                ApplicationStatusName = CONApp.ApplicationStatusName,
                                ApplicationStatus = CONApp.ApplicationStatusCode,
                                DateOfSubmission = CONApp.CreatedDate.ToString(),
                                TempRegistrationNumber = CONApp.TempRegistrationSerialNumber,
                                EmployeeRemarks = CONApp.ApplicationStatusUpdateEmpRemarks,
                                AccountNo = CONApp.AccountNumber.TrimStart('0'),
                                Aklasse = accountDetails.AKLASSE,
                                Area = CONApp.Area_New,
                                City = CONApp.City,
                                NewAccountNumber = string.IsNullOrEmpty(CONApp.NewAccountNumber) ? "" : CONApp.NewAccountNumber.TrimStart('0'),
                                SelectedCity = CONApp.City_New,
                                ConnectionType = consumerDetails.ConnectionType,
                                ConsumerName = consumerDetails.Name,
                                SelectedTitle = CONApp.TitleValue,
                                FirstName = CONApp.FirstName,
                                LastName = CONApp.LastName,
                                MiddleName = CONApp.MiddleName,
                                Name1Joint = CONApp.FirstName,
                                Name2Joint = CONApp.LastName,
                                Landline = CONApp.LandlineNumber,
                                ApplicantType = CONApp.ApplicantTypeCode,
                                OrganizationName = CONApp.OrgName,
                                SelectedPremiseType = CONApp.PremiseTypeCode,
                                ExistingEmailId = string.IsNullOrEmpty(consumerDetails.Email) ? consumerDetails.Email : consumerDetails.Email.Substring(0, 2) + "xxxxxxxxxx" + consumerDetails.Email.Substring(consumerDetails.Email.Length - 3),
                                EmailId = CONApp.EmailAddress,
                                HouseNumber = CONApp.Housenumber_New,
                                Landmark = CONApp.Landmark_New,
                                MeterNumber = string.IsNullOrEmpty(CONApp.MeterNumber) ? CONApp.MeterNumber : CONApp.MeterNumber.TrimStart('0').Substring(0, 2) + "xxxxx" + CONApp.MeterNumber.TrimStart('0').Substring(CONApp.MeterNumber.TrimStart('0').Length - 1),
                                MobileNo = CONApp.MobileNumber,
                                ExistingMobileNumber = string.IsNullOrEmpty(consumerDetails.Mobile) ? consumerDetails.Mobile : consumerDetails.Mobile.Substring(0, 1) + "xxxxxxx" + consumerDetails.Mobile.Substring(consumerDetails.Mobile.Length - 2),
                                ExistingSecurityDepositeAmount = Convert.ToDecimal(CONApp.ExistingSecurityDepositAmount),
                                SecurityDepositeAmount = Convert.ToDecimal(CONApp.SecurityDepositAmount),
                                Vertrag_Contract = consumerDetails.Vertrag_Contract,
                                Pincode = CONApp.Pincode_New,
                                Street = CONApp.Streetname_New,
                                SelectedSuburb = CONApp.Suburb_New,
                                SelectedPincode = CONApp.Pincode_New,
                                SelectedBillLanguage = CONApp.BillingLanguage,
                                ConsumerRemark = CONApp.ConsumerRemarks,
                                LandlordName = CONApp.Rented_Ownername,
                                GetExistingDocuments = CONAppDocuments,

                                Address = consumerDetails.HouseNumber + ", " + consumerDetails.Street + ", " + consumerDetails.Street2 + ", " + consumerDetails.Street3 + ", " + consumerDetails.City
                                + "," + consumerDetails.PinCode
                            };
                            if (CONApp.ApplicationTypeId != null)
                            {
                                model.IsStillLiving = CONApp.ApplicationTypeId == "21" ? "Yes" : "No";
                            }

                            if (CONApp.IsContinueExitingSDvalueOpt != null)
                            {
                                model.IsContinueWithExistingSD = CONApp.IsContinueExitingSDvalueOpt.ToString() == "True" ? "Yes" : "No";
                            }
                            if (CONApp.PaperlessBillingFlag.ToString() != null)
                            {
                                model.IsPaperlessBilling = CONApp.PaperlessBillingFlag.ToString() == "True" ? "Yes" : "No";
                            }
                            if (CONApp.IsAddressChangeRequired.ToString() != null)
                            {
                                model.IsAddressCorrectionRequired = CONApp.IsAddressChangeRequired.ToString() == "True" ? "Yes" : "No";
                            }
                            var listAllArea = changeOfNameService.ListAreaPinWorkcenterMapping();

                            if (!string.IsNullOrEmpty(model.SelectedPincode) && listAllArea.Any(a => a.PinCode == model.SelectedPincode))
                            {
                                var existingArea = listAllArea.Where(a => a.PinCode == model.SelectedPincode).FirstOrDefault();
                                model.SelectedSuburb = existingArea.Area;

                                var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                                model.CitySelectList = new List<SelectListItem>();
                                if (cityList != null && cityList.Any())
                                {
                                    foreach (var item in cityList)
                                    {
                                        model.CitySelectList.Add(new SelectListItem
                                        {
                                            Text = item,
                                            Value = item
                                        });
                                    }
                                }
                                model.SelectedCity = existingArea.City;
                                if (model.SelectedCity != null)
                                {
                                    var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                                    model.PincodeSelectList = new List<SelectListItem>();
                                    if (pinList != null && pinList.Any())
                                    {
                                        foreach (var item in pinList)
                                        {
                                            model.PincodeSelectList.Add(new SelectListItem
                                            {
                                                Text = item,
                                                Value = item
                                            });
                                        }
                                    }
                                }
                                model.SelectedPincode = existingArea.PinCode;
                            }
                            else
                            {
                                if (model.SelectedSuburb != null)
                                {
                                    var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                                    model.CitySelectList = new List<SelectListItem>();
                                    if (cityList != null && cityList.Any())
                                    {
                                        foreach (var item in cityList)
                                        {
                                            model.CitySelectList.Add(new SelectListItem
                                            {
                                                Text = item,
                                                Value = item
                                            });
                                        }
                                    }
                                }
                                if (model.SelectedCity != null)
                                {
                                    var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                                    model.PincodeSelectList = new List<SelectListItem>();
                                    if (pinList != null && pinList.Any())
                                    {
                                        foreach (var item in pinList)
                                        {
                                            model.PincodeSelectList.Add(new SelectListItem
                                            {
                                                Text = item,
                                                Value = item
                                            });
                                        }
                                    }
                                }
                            }

                            var applicantionType = model.IsStillLiving == "Yes" ? "21" : "22";
                            var titleValue = "";

                            if (model.ApplicantType == "1" || model.ApplicantType == "3")
                            {
                                if (model.SelectedTitle == "0006")
                                    titleValue = "0006";
                            }
                            else
                            {
                                titleValue = "0005";
                            }

                            var listOfDocs = changeOfNameService.GetDocuments(applicantionType, titleValue, model.SelectedPremiseType, model.ApplicantType, model.IsContinueWithExistingSD);

                            //ID Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (doc.AKLASSE == model.Aklasse))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.IDDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.IDDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //ID2 Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID2").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.ID2DocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.ID2DocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //OD Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "OD").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.ODDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.ODDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //PH Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "PH").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.PHDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.PHDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //SD Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "SD").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.SDDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.SDDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                }
                            }
                            //OT Documents
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "OT").ToList())
                            {
                                if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                {
                                    if (doc.FLAG_MANDATORY == "X")
                                    {
                                        model.OTDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (doc.FLAG_DOCNO == "X")
                                    {
                                        model.OTDocumentsListOnly1.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                        });
                                    }
                                    else if (model.ApplicantType == "3")
                                    {
                                        model.OTDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = true
                                        });
                                    }
                                }
                            }
                            foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "").ToList())
                            {
                                if (!model.OTDocumentsList.Any(d => d.DocTypeCode == doc.DocumentTypeCode && d.DocName == doc.DocumentDescription))
                                {
                                    model.OTDocumentsList.Add(new DocumentCheckList
                                    {
                                        DocName = doc.DocumentDescription,
                                        DocId = doc.Id.ToString(),
                                        DocType = doc.DocumentType,
                                        DocTypeCode = doc.DocumentTypeCode,
                                        DocSerialNumber = doc.DocumentSerialNumber,
                                        IsMandatory = true
                                    });
                                }
                            }
                        }
                        else
                        {
                            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                        }
                    }
                    else if (!string.IsNullOrEmpty(accountDetailsForCON.RegistrationNo))
                    {
                        var CONApp = changeOfNameService.GetExistingApplicationByRegistrationNumber(accountDetailsForCON.RegistrationNo);
                        //Error - If application with this registration number
                        //- Does not exists
                        //- In save as draft status 
                        if (CONApp == null)
                        {
                            ViewBag.Message = "No application exists with this Registration number!";
                            Session["Message"] = "No application exists with this Registration number!";
                            return Redirect(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                        }
                        else
                        {
                            accountDetailsForCON.AccountNoForCheckApplication = CONApp.AccountNumber;
                            var CONAppDocuments = changeOfNameService.GetExistingDocument(CONApp.RegistrationSerialNumber);

                            if (CONApp == null)
                            {
                                ViewBag.Message = "No application exists with this Account number!";
                                Session["Message"] = "No application exists with this Account number!";
                                return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                            }
                            else if (CONApp.ApplicationStatusCode == "2" || CONApp.ApplicationStatusCode == "3" || CONApp.ApplicationStatusCode == "4")
                            {
                                //Saved application already exists - display in editable mode
                                var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(CONApp.AccountNumber);
                                var accountDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(CONApp.AccountNumber);
                                var consumerSD = SapPiService.Services.RequestHandler.GetSecurityDeposityAmountCON(CONApp.AccountNumber);
                                model = new ChangeOfNameApplicationFromModel
                                {
                                    LECMobileNumber = CONApp.LECMobileNumber,
                                    LECRegistrationNumber = CONApp.LECRegistrationNumber,
                                    IsLEC = accountDetailsForCON.IsLEC,
                                    ApplicationStatusName = CONApp.ApplicationStatusName,
                                    ApplicationStatus = CONApp.ApplicationStatusCode,
                                    DateOfSubmission = CONApp.CreatedDate.ToString(),
                                    TempRegistrationNumber = CONApp.TempRegistrationSerialNumber,
                                    EmployeeRemarks = CONApp.ApplicationStatusUpdateEmpRemarks,
                                    NewAccountNumber = string.IsNullOrEmpty(CONApp.NewAccountNumber) ? "" : CONApp.NewAccountNumber.TrimStart('0'),
                                    AccountNo = CONApp.AccountNumber.TrimStart('0'),
                                    Aklasse = accountDetails.AKLASSE,
                                    Area = CONApp.Area_New,
                                    City = CONApp.City,
                                    SelectedCity = CONApp.City_New,
                                    ConnectionType = consumerDetails.ConnectionType,
                                    ConsumerName = consumerDetails.Name,
                                    SelectedTitle = CONApp.TitleValue,
                                    FirstName = CONApp.FirstName,
                                    LastName = CONApp.LastName,
                                    MiddleName = CONApp.MiddleName,
                                    Name1Joint = CONApp.FirstName,
                                    Name2Joint = CONApp.LastName,
                                    Landline = CONApp.LandlineNumber,
                                    ApplicantType = CONApp.ApplicantType != null ? CONApp.ApplicantType.ToString() : "1",
                                    OrganizationName = CONApp.OrgName,
                                    SelectedPremiseType = CONApp.PremiseTypeCode,
                                    ExistingEmailId = string.IsNullOrEmpty(consumerDetails.Email) ? consumerDetails.Email : consumerDetails.Email.Substring(0, 2) + "xxxxxxxxxx" + consumerDetails.Email.Substring(consumerDetails.Email.Length - 3),
                                    EmailId = CONApp.EmailAddress,
                                    HouseNumber = CONApp.Housenumber_New,
                                    Landmark = CONApp.Landmark_New,
                                    MeterNumber = consumerDetails.MeterNumber.TrimStart('0').Substring(0, 2) + "xxxxx" + consumerDetails.MeterNumber.TrimStart('0').Substring(consumerDetails.MeterNumber.TrimStart('0').Length - 1),
                                    MobileNo = CONApp.MobileNumber,
                                    ExistingMobileNumber = string.IsNullOrEmpty(consumerDetails.Mobile) ? consumerDetails.Mobile : consumerDetails.Mobile.Substring(0, 1) + "xxxxxxx" + consumerDetails.Mobile.Substring(consumerDetails.Mobile.Length - 2),
                                    ExistingSecurityDepositeAmount = Convert.ToDecimal(CONApp.ExistingSecurityDepositAmount),
                                    SecurityDepositeAmount = Convert.ToDecimal(CONApp.SecurityDepositAmount),
                                    Vertrag_Contract = consumerDetails.Vertrag_Contract,
                                    Pincode = CONApp.Pincode_New,
                                    Street = CONApp.Streetname_New,
                                    SelectedSuburb = CONApp.Suburb_New,
                                    SelectedPincode = CONApp.Pincode_New,
                                    SelectedBillLanguage = CONApp.BillingLanguage,
                                    ConsumerRemark = CONApp.ConsumerRemarks,
                                    LandlordName = CONApp.Rented_Ownername,
                                    GetExistingDocuments = CONAppDocuments,


                                    Address = consumerDetails.HouseNumber + ", " + consumerDetails.Street + ", " + consumerDetails.Street2 + ", " + consumerDetails.Street3 + ", " + consumerDetails.City
                                    + "," + consumerDetails.PinCode
                                };
                                if (CONApp.ApplicationTypeId != null)
                                {
                                    model.IsStillLiving = CONApp.ApplicationTypeId == "21" ? "Yes" : "No";
                                }

                                if (CONApp.IsContinueExitingSDvalueOpt != null)
                                {
                                    model.IsContinueWithExistingSD = CONApp.IsContinueExitingSDvalueOpt.ToString() == "True" ? "Yes" : "No";
                                }
                                if (CONApp.PaperlessBillingFlag.ToString() != null)
                                {
                                    model.IsPaperlessBilling = CONApp.PaperlessBillingFlag.ToString() == "True" ? "Yes" : "No";
                                }
                                if (CONApp.IsAddressChangeRequired.ToString() != null)
                                {
                                    model.IsAddressCorrectionRequired = CONApp.IsAddressChangeRequired.ToString() == "True" ? "Yes" : "No";
                                }
                                var listAllArea = changeOfNameService.ListAreaPinWorkcenterMapping();

                                if (!string.IsNullOrEmpty(model.SelectedPincode) && listAllArea.Any(a => a.PinCode == model.SelectedPincode))
                                {
                                    var existingArea = listAllArea.Where(a => a.PinCode == model.SelectedPincode).FirstOrDefault();
                                    model.SelectedSuburb = existingArea.Area;

                                    var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                                    model.CitySelectList = new List<SelectListItem>();
                                    if (cityList != null && cityList.Any())
                                    {
                                        foreach (var item in cityList)
                                        {
                                            model.CitySelectList.Add(new SelectListItem
                                            {
                                                Text = item,
                                                Value = item
                                            });
                                        }
                                    }
                                    model.SelectedCity = existingArea.City;
                                    if (model.SelectedCity != null)
                                    {
                                        var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                                        model.PincodeSelectList = new List<SelectListItem>();
                                        if (pinList != null && pinList.Any())
                                        {
                                            foreach (var item in pinList)
                                            {
                                                model.PincodeSelectList.Add(new SelectListItem
                                                {
                                                    Text = item,
                                                    Value = item
                                                });
                                            }
                                        }
                                    }
                                    model.SelectedPincode = existingArea.PinCode;
                                }
                                else
                                {
                                    if (model.SelectedSuburb != null)
                                    {
                                        var cityList = listAllArea.Where(a => a.Area == model.SelectedSuburb).Select(c => c.City).Distinct().ToList();
                                        model.CitySelectList = new List<SelectListItem>();
                                        if (cityList != null && cityList.Any())
                                        {
                                            foreach (var item in cityList)
                                            {
                                                model.CitySelectList.Add(new SelectListItem
                                                {
                                                    Text = item,
                                                    Value = item
                                                });
                                            }
                                        }
                                    }
                                    if (model.SelectedCity != null)
                                    {
                                        var pinList = listAllArea.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity).Select(c => c.PinCode).Distinct().ToList();
                                        model.PincodeSelectList = new List<SelectListItem>();
                                        if (pinList != null && pinList.Any())
                                        {
                                            foreach (var item in pinList)
                                            {
                                                model.PincodeSelectList.Add(new SelectListItem
                                                {
                                                    Text = item,
                                                    Value = item
                                                });
                                            }
                                        }
                                    }
                                }

                                var applicantionType = model.IsStillLiving == "Yes" ? "21" : "22";
                                var titleValue = "";

                                if (model.ApplicantType == "1" || model.ApplicantType == "3")
                                {
                                    if (model.SelectedTitle == "0006")
                                        titleValue = "0006";
                                }
                                else
                                {
                                    titleValue = "0005";
                                }

                                var listOfDocs = changeOfNameService.GetDocuments(applicantionType, titleValue, model.SelectedPremiseType, model.ApplicantType, model.IsContinueWithExistingSD);
                                //ID Documents
                                foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID").ToList())
                                {
                                    if (string.IsNullOrEmpty(doc.AKLASSE) || (doc.AKLASSE == model.Aklasse))
                                    {
                                        if (doc.FLAG_MANDATORY == "X")
                                        {
                                            model.IDDocumentsList.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                        else if (doc.FLAG_DOCNO == "X")
                                        {
                                            model.IDDocumentsListOnly1.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                    }
                                }
                                //ID2 Documents
                                foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "ID2").ToList())
                                {
                                    if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                    {
                                        if (doc.FLAG_MANDATORY == "X")
                                        {
                                            model.ID2DocumentsList.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                        else if (doc.FLAG_DOCNO == "X")
                                        {
                                            model.ID2DocumentsListOnly1.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                    }
                                }
                                //OD Documents
                                foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "OD").ToList())
                                {
                                    if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                    {
                                        if (doc.FLAG_MANDATORY == "X")
                                        {
                                            model.ODDocumentsList.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                        else if (doc.FLAG_DOCNO == "X")
                                        {
                                            model.ODDocumentsListOnly1.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                    }
                                }
                                //PH Documents
                                foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "PH").ToList())
                                {
                                    if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                    {
                                        if (doc.FLAG_MANDATORY == "X")
                                        {
                                            model.PHDocumentsList.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                        else if (doc.FLAG_DOCNO == "X")
                                        {
                                            model.PHDocumentsListOnly1.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                    }
                                }
                                //SD Documents
                                foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "SD").ToList())
                                {
                                    if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                    {
                                        if (doc.FLAG_MANDATORY == "X")
                                        {
                                            model.SDDocumentsList.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                        else if (doc.FLAG_DOCNO == "X")
                                        {
                                            model.SDDocumentsListOnly1.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                    }
                                }
                                //OT Documents
                                foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode.Trim() == "OT").ToList())
                                {
                                    if (string.IsNullOrEmpty(doc.AKLASSE) || (model.Aklasse == doc.AKLASSE))
                                    {
                                        if (doc.FLAG_MANDATORY == "X")
                                        {
                                            model.OTDocumentsList.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                        else if (doc.FLAG_DOCNO == "X")
                                        {
                                            model.OTDocumentsListOnly1.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = doc.FLAG_MANDATORY == "X" ? true : false
                                            });
                                        }
                                        else if (model.ApplicantType == "3")
                                        {
                                            model.OTDocumentsList.Add(new DocumentCheckList
                                            {
                                                DocName = doc.DocumentDescription,
                                                DocId = doc.Id.ToString(),
                                                DocType = doc.DocumentType,
                                                DocTypeCode = doc.DocumentTypeCode,
                                                DocSerialNumber = doc.DocumentSerialNumber,
                                                IsMandatory = true
                                            });
                                        }
                                    }
                                }
                                foreach (var doc in listOfDocs.Where(d => d.DocumentTypeCode == "").ToList())
                                {
                                    if (!model.OTDocumentsList.Any(d => d.DocSerialNumber == doc.DocumentSerialNumber && d.DocName == doc.DocumentDescription))
                                    {
                                        model.OTDocumentsList.Add(new DocumentCheckList
                                        {
                                            DocName = doc.DocumentDescription,
                                            DocId = doc.Id.ToString(),
                                            DocType = doc.DocumentType,
                                            DocTypeCode = doc.DocumentTypeCode,
                                            DocSerialNumber = doc.DocumentSerialNumber,
                                            IsMandatory = true
                                        });
                                    }
                                }
                            }
                            else
                            {
                                return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPageRevamp));
                            }
                        }
                        //else
                        //{
                        //    //ViewBag.Message = "Your application with this Registration number is Rejected!";
                        //    //Session["Message"] = "Your application with this Registration number is Rejected!";
                        //    return Redirect(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ChangeOfNameRegistrationPage));
                        //}
                        //check if submitted application exists
                        //check status
                        //read only display
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at ChangeOfNameSubmittedApplication:" + ex.Message, this);
            }
            return this.View(model);
        }

        private CONApplicationDocumentDetail CreateDocumentObject(byte[] bytes, string fileName, string contenttype, CONDocumentMaster doc, string accountNumber, string RegistrationSerialNumber, bool isDocSaved, string docNumber = null)
        {
            CONApplicationDocumentDetail obj = new CONApplicationDocumentDetail();
            Regex reg = new Regex("[*'\",_&#^@$%]");
            fileName = reg.Replace(fileName, string.Empty);
            fileName = fileName.Replace(" ", string.Empty);
            try
            {
                DateTime? sapSentDate = null;
                if (isDocSaved) sapSentDate = DateTime.Now;
                obj = new CONApplicationDocumentDetail
                {
                    Id = Guid.NewGuid(),
                    DocumentType = doc.DocumentType,
                    DocumentTypeCode = doc.DocumentTypeCode,
                    CreatedBy = ChangeOfNameService.FormatAccountNumber(accountNumber),
                    CreatedDate = DateTime.Now,
                    DocumentChecklistSerialNumber = doc.DocumentSerialNumber.ToString(),
                    DocumentData = bytes,
                    DocumentDescription = doc.DocumentDescription,
                    DocumentName = fileName,
                    DocumentDataContenttype = contenttype,
                    DocumentNumber = docNumber,
                    IsSentToSAP = isDocSaved,
                    RegistrationSerialNumber = RegistrationSerialNumber,
                    SAPSentDate = sapSentDate
                };
            }
            catch (Exception ex)
            {
                Log.Error("Error at ChangeOfNameSubmittedApplication:" + ex.Message, this);
            }
            return obj;
        }

        /// <summary>
        /// Download file, if file belongs to the same user only
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileResult DownloadFile(Guid id)
        {
            try
            {
                if (Session["CONAccountDetails"] != null)
                {
                    var accountDetailsForCON = (ChangeOfNameRegistrationModel)Session["CONAccountDetails"];
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        var fileToDownload = dbcontext.CONApplicationDocumentDetails.Where(i => i.Id == id).FirstOrDefault();

                        if (dbcontext.CONApplicationDetails.Any(a => a.RegistrationSerialNumber == fileToDownload.RegistrationSerialNumber))
                            return File(fileToDownload.DocumentData.ToArray(), fileToDownload.DocumentDataContenttype, fileToDownload.DocumentName);
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

        /// <summary>
        /// Service to update Status , called from SAP/ISS application Employee portal
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult CONApplicationStatusUpdate(CONApplicationStatusUpdateModel request)
        {
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string apiUserName = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.StatusUpdateAPIUserId].Value; //"electricity";
                string apiPassword = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.StatusUpdateAPIUserPassword].Value; // "admin@123";
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";

                Sitecore.Diagnostics.Log.Info("CONApplicationStatusUpdate Method Called", this);
                string authHeader = HttpContext.Request.Headers["Authorization"];
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                if (string.IsNullOrEmpty(authHeader))
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');
                string username = objcrypt.DecryptText(usernamePassword.Substring(0, seperatorIndex));
                string password = objcrypt.DecryptText(usernamePassword.Substring(seperatorIndex + 1));

                string tempRegNo = objcrypt.DecryptText(request.TEMP_REG_NO);
                string accountNumer = objcrypt.DecryptText(request.Old_CA);
                string status = objcrypt.DecryptText(request.Status);
                string newaccountNumer = objcrypt.DecryptText(request.New_CA);
                string remarks = objcrypt.DecryptText(request.Remark);
                string actiondate = objcrypt.DecryptText(request.Action_datetime);

                Sitecore.Diagnostics.Log.Info("CONApplicationStatusUpdate Method called at " + DateTime.Now.ToString(), this);
                Sitecore.Diagnostics.Log.Info("CONApplicationStatusUpdate caNumber:" + accountNumer + " applicationStatus:" + status +
                    " employeeComments:" + remarks + " newcaNumber:" + newaccountNumer, this);
                Sitecore.Diagnostics.Log.Info("CONApplicationStatusUpdate apiUserName:" + apiUserName + " apiPassword:" + apiPassword, this);
                Sitecore.Diagnostics.Log.Info("CONApplicationStatusUpdate username:" + username + " password:" + password, this);
                if (username == apiUserName && password == apiPassword)
                {
                    //return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = request.Old_CA + "Credential is valid "+ objcrypt.DecryptText(request.Old_CA) }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    string RequestHTML = "";

                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        if (dataContext.CONApplicationDetails.Any(a => a.RegistrationSerialNumber == tempRegNo && a.AccountNumber == ChangeOfNameService.FormatAccountNumber(accountNumer)))
                        {
                            CONApplicationDetail appDetails = dataContext.CONApplicationDetails.Where(a => a.RegistrationSerialNumber == tempRegNo && a.AccountNumber == ChangeOfNameService.FormatAccountNumber(accountNumer)).FirstOrDefault();
                            appDetails.ApplicationStatusCode = status;
                            if (status == "3")
                                appDetails.ApplicationStatusName = "Approved";
                            else if (status == "4")
                                appDetails.ApplicationStatusName = "Rejected";
                            appDetails.ApplicationStatusUpdateEmpRemarks = remarks;
                            appDetails.NewAccountNumber = newaccountNumer;
                            appDetails.ApplicationStatusUpdateDate = DateTime.Now;
                            appDetails.ApplicationstatusUpdatedBy = "Service update on:" + actiondate;
                            dataContext.SubmitChanges();
                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Success", Result = RequestHTML }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                        else
                        {
                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = false, Message = "Application does not exists!", Result = RequestHTML }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                    }
                }
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = request.Old_CA + "Credential is not valid" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at getting CONApplicationStatusUpdate :" + ex.Message, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = ex.Message, Result = "" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        #endregion Application Form


        public static byte[] CompressAnImage(int jpegQuality, byte[] data)
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

        #region Generate Pdf

        private static string FontRegular = "";
        private static string FontBold = "";
        private static string FontMedium = "";

        Font ColorFont = FontFactory.GetFont(FontRegular, 8, Font.NORMAL, new BaseColor(171, 49, 117));
        Font boldFont = FontFactory.GetFont(FontBold, 8, Font.NORMAL);
        Font RegularFont = FontFactory.GetFont(FontRegular, 8, Font.NORMAL);//23, 56, 133
        Font RegularFontWhite = FontFactory.GetFont(FontRegular, 8, Font.NORMAL, new BaseColor(255, 255, 255));
        Font RegularFont2 = FontFactory.GetFont(FontRegular, 8, Font.NORMAL, new BaseColor(23, 56, 133));
        Font smallFontWhite = FontFactory.GetFont(FontRegular, 6, new BaseColor(255, 255, 255));
        Font smallFont = FontFactory.GetFont(FontRegular, 6);
        Font smallredFont = FontFactory.GetFont(FontRegular, 6, Font.NORMAL, new BaseColor(209, 85, 82));
        Font FontTextmed8 = FontFactory.GetFont(FontMedium, 8, Font.NORMAL, new BaseColor(209, 85, 82));

        public FileResult DownloadPDF(Guid ID)
        {
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                var fileToDownload = dbcontext.CONApplicationDocumentDetails.Where(i => i.Id == ID).FirstOrDefault();
                return File(fileToDownload.DocumentData.ToArray(), "application/pdf", fileToDownload.DocumentName);
            }
        }

        public void DownloadPDFFile(string ca)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + "AEML-CON-ApplicationForm.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(CreatePDF(ca));
            Response.End();
        }

        public byte[] CreatePDF(string ca)
        {
            PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext();
            CONApplicationDetail con_data = dataContext.CONApplicationDetails.Where(p => p.AccountNumber == ChangeOfNameService.FormatAccountNumber(ca)).OrderByDescending(d => d.CreatedDate).FirstOrDefault();

            string oAadhar = "";
            string oPAN = "";
            string oGSTN = "";
            int oZCnt_1ph = 0;
            //int oZCnt_3ph = 0;
            //int oZCnt_HT = 0;
            int oZCnt_3ph_Tot = 0;

            Document document = new Document(PageSize.A4, 40, 40, 40, 40);
            //DataSet dslec = objConsumeWS.leccheck(dsinput.Tables[0].Rows[0]["ZZ_LICNO_LECNO"].ToString().Trim());

            DateTime dt = new DateTime();
            dt = System.DateTime.Now;

            string oDt = String.Format("{0:d-MMM-yyyy}", dt);


            //AADHAR CARD,PAN CARD,GSTN
            var documentDetail = dataContext.CONApplicationDocumentDetails.Where(p => p.RegistrationSerialNumber == con_data.RegistrationSerialNumber && p.DocumentTypeCode == "ID").ToList();//990-AADHAR Card,920-PAN,998-GSTN

            if (documentDetail.Count > 0)
            {
                foreach (var doc in documentDetail)
                {
                    if (doc.DocumentChecklistSerialNumber == "990")
                    {
                        oAadhar = doc.DocumentNumber != null ? doc.DocumentNumber.ToString() : "";
                    }
                    else if (doc.DocumentChecklistSerialNumber == "920")
                    {
                        oPAN = doc.DocumentNumber != null ? doc.DocumentNumber.ToString() : "";
                    }
                    else if (doc.DocumentChecklistSerialNumber == "998")
                    {
                        oGSTN = doc.DocumentNumber != null ? doc.DocumentNumber.ToString() : "";
                    }
                }
            }


            //Document Type

            //Document document = new Document(PageSize.A4, 40, 40, 40, 40);
            using (MemoryStream output = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, output);
                document.Open();
                PdfContentByte cb = writer.DirectContent;

                //Add logo Image                

                String imagePath2 = Server.MapPath("/Images/powerofservicelogo.jpg");
                Image logoImage2 = Image.GetInstance(imagePath2.Replace("/PDFGenerate", ""));

                logoImage2.ScalePercent(50, 50);
                logoImage2.SetAbsolutePosition(20, 800);// 75,75
                document.Add(logoImage2);


                String imagePath3 = Server.MapPath("/Images/Adani_logo.jpg");
                Image logoImage3 = Image.GetInstance(imagePath3.Replace("/PDFGenerate", ""));

                logoImage3.ScalePercent(50, 50);
                logoImage3.SetAbsolutePosition(500, 800);// 75,75                
                document.Add(logoImage3);



                float[] widths = { 20, 10, 250, 10, 200, 10, 200, 150, 200 };

                PdfPTable mainTable = new PdfPTable(widths);
                //mainTable.SetWidthPercentage(float widths,Rectangle 100.0F);
                mainTable.WidthPercentage = 100.0F;
                mainTable.SetWidths(widths);
                PdfPCell mainCell;
                Paragraph mainParagraph;


                mainParagraph = new Paragraph(" \n FORM 16.1\n\nTHIS FORM IS NOT CHARGEABLE\n\n (ALL INFORMATION TO BE FILLED IN BLOCK LETTERES.PLEASE TICK WHEREVER APPLICABLE)", RegularFont);
                mainCell = new PdfPCell(mainParagraph);
                mainCell.PaddingLeft = 15.0F;
                mainCell.PaddingRight = 10.0F;
                mainCell.PaddingBottom = 10.0F;
                mainCell.HorizontalAlignment = Element.ALIGN_LEFT;
                mainCell.Border = PdfPCell.NO_BORDER;
                mainCell.CellEvent = new RoundedBorder();
                mainCell.Colspan = 8;
                mainTable.AddCell(mainCell);

                //string path = "";
                var applicantType = con_data.ApplicantType;
                CONApplicationDocumentDetail imageDocument = null;
                if (applicantType == "2")
                {
                    imageDocument = dataContext.CONApplicationDocumentDetails.Where(p => p.RegistrationSerialNumber == con_data.RegistrationSerialNumber && p.DocumentTypeCode == "PH" && p.DocumentDescription == "PASSPORT SIZE PHOTOGRAPH OF THE APPLICANT 1").FirstOrDefault();
                }
                else
                {
                    imageDocument = dataContext.CONApplicationDocumentDetails.Where(p => p.RegistrationSerialNumber == con_data.RegistrationSerialNumber && p.DocumentTypeCode == "PH").FirstOrDefault();
                }

                if (imageDocument != null)
                {
                    if (imageDocument.DocumentData != null)
                    {
                        try
                        {
                            itextsharp.iTextSharp.text.Image img1 = itextsharp.iTextSharp.text.Image.GetInstance(imageDocument.DocumentData.ToArray());
                            img1.Alignment = itextsharp.iTextSharp.text.Image.ALIGN_CENTER;
                            img1.ScaleToFit(95f, 115f);
                            mainCell = new PdfPCell(img1);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message, e.Source);
                            mainParagraph = new Paragraph("Affix Passport size photograph\nand sign across", smallFont);
                            mainCell = new PdfPCell(mainParagraph);
                        }
                    }
                    else
                    {
                        mainParagraph = new Paragraph("Affix Passport size photograph\nand sign across", smallFont);
                        mainCell = new PdfPCell(mainParagraph);
                    }
                }
                //path = objcomm.Getimage_CHN(con_data.);
                //string path = objcomm.Getimage_CHN("421");
                //if (!string.IsNullOrEmpty(path))
                //{
                //    iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(Server.MapPath(path));
                //    img1.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                //    img1.ScaleToFit(95f, 115f);
                //    mainCell = new PdfPCell(img1);

                //    //mainParagraph = new Paragraph("Affix Passport size photograph\nand sign across", smallFont);
                //    //mainCell = new PdfPCell(mainParagraph);
                //}
                else
                {
                    mainParagraph = new Paragraph("Affix Passport size photograph\nand sign across", smallFont);
                    mainCell = new PdfPCell(mainParagraph);
                }

                mainCell.HorizontalAlignment = Element.ALIGN_CENTER;
                mainCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                mainCell.Rowspan = 5;
                mainCell.Colspan = 1;
                mainCell.PaddingTop = 0.0F;
                mainTable.AddCell(mainCell);


                mainParagraph = new Paragraph("");
                mainCell = new PdfPCell(mainParagraph);
                mainCell.Border = Rectangle.NO_BORDER;
                mainCell.Colspan = 1;
                mainTable.AddCell(mainCell);

                mainParagraph = new Paragraph("", RegularFontWhite);
                mainCell = new PdfPCell(mainParagraph);
                mainCell.HorizontalAlignment = Element.ALIGN_LEFT;
                mainCell.PaddingRight = 30.0F;
                mainCell.Border = Rectangle.NO_BORDER;
                mainCell.Colspan = 7;
                mainCell.PaddingRight = 30.0F;
                mainTable.AddCell(mainCell);


                mainParagraph = new Paragraph("");
                mainCell = new PdfPCell(mainParagraph);
                mainCell.Border = Rectangle.NO_BORDER;
                mainCell.Colspan = 1;
                mainTable.AddCell(mainCell);

                //mainParagraph = new Paragraph("(please provide complete details)", smallFont);
                mainParagraph = new Paragraph(" ", RegularFontWhite);
                mainCell = new PdfPCell(mainParagraph);
                mainCell.HorizontalAlignment = Element.ALIGN_LEFT;
                mainCell.Border = Rectangle.NO_BORDER;
                mainCell.Colspan = 7;
                //mainCell.FixedHeight = 5.0F;
                mainCell.PaddingBottom = 5.0F;
                mainTable.AddCell(mainCell);


                mainParagraph = new Paragraph("", RegularFontWhite);
                mainCell = new PdfPCell(mainParagraph);
                mainCell.HorizontalAlignment = Element.ALIGN_LEFT;
                mainCell.Border = Rectangle.NO_BORDER;
                mainCell.Colspan = 4;
                mainCell.Padding = 5.0F;
                //mainCell.BackgroundColor = new BaseColor(171, 49, 117);
                mainTable.AddCell(mainCell);

                mainParagraph = new Paragraph("", RegularFontWhite);
                mainCell = new PdfPCell(mainParagraph);
                mainCell.HorizontalAlignment = Element.ALIGN_LEFT;
                mainCell.Border = Rectangle.NO_BORDER;
                mainCell.Colspan = 4;
                mainCell.Padding = 5.0F;
                mainTable.AddCell(mainCell);


                mainParagraph = new Paragraph("");
                mainCell = new PdfPCell(mainParagraph);
                mainCell.Border = Rectangle.NO_BORDER;
                mainCell.Colspan = 1;
                mainTable.AddCell(mainCell);


                //Creating Nested table starts
                float[] afwidths = { 20, 280, 20, 280, 20, 280 };
                PdfPTable afTable = new PdfPTable(afwidths);

                //Add checkbox
                String[] applicationFor = { "NEW CONNECTION", "CONTRACT DEMAND CHANGE", "LOAD CHANGE", "SHIFTING", "CHANGE OF NAME", "CHANGEOVER SUPPLY", "SWITCH OVER OF SUPPLY" };


                PdfFormField fieldAF;
                Rectangle rectAF = new Rectangle(10, 10, 15, 15);
                PdfFormField radiogroup = PdfFormField.CreateRadioButton(writer, true);
                radiogroup.FieldName = "Type";
                RadioCheckField checkboxAF;

                for (int i = 0; i < applicationFor.Length; i++)
                {

                    checkboxAF = new RadioCheckField(writer, rectAF, applicationFor[i], "Yes");
                    //checkboxAF.BorderColor = GrayColor.GRAYBLACK;
                    checkboxAF.BackgroundColor = BaseColor.WHITE;
                    checkboxAF.CheckType = RadioCheckField.TYPE_CIRCLE;

                    if (applicationFor[i].Equals("CHANGE OF NAME"))
                        checkboxAF.Checked = true;

                    //checkboxAF.BackgroundColor = new BaseColor(253, 230, 220);
                    checkboxAF.BorderColor = new BaseColor(247, 170, 165);
                    checkboxAF.Options = BaseField.READ_ONLY;

                    fieldAF = checkboxAF.RadioField;
                    fieldAF.SetFieldFlags(PdfFormField.FF_READ_ONLY);

                    fieldAF = checkboxAF.RadioField;
                    fieldAF.SetWidget(rectAF, PdfAnnotation.HIGHLIGHT_INVERT);

                    radiogroup.AddKid(fieldAF);


                    writer.AddAnnotation(radiogroup);

                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, fieldAF);
                    cell.Border = Rectangle.NO_BORDER;
                    afTable.AddCell(cell);

                    Paragraph paragraph = new Paragraph(applicationFor[i], RegularFont);
                    cell = new PdfPCell(paragraph);
                    cell.Border = Rectangle.NO_BORDER;
                    afTable.AddCell(cell);

                    if ((i + 1) % 3 == 0)
                    {
                        mainParagraph = new Paragraph(" ");
                        mainCell = new PdfPCell(mainParagraph);
                        mainCell.Border = Rectangle.NO_BORDER;
                        mainCell.Colspan = 9;
                        mainCell.PaddingTop = -10.0F;
                        afTable.AddCell(mainCell);
                    }

                }

                mainParagraph = new Paragraph("");
                mainCell = new PdfPCell(mainParagraph);
                mainCell.Border = Rectangle.NO_BORDER;
                mainCell.Padding = 10;
                mainCell.Colspan = 4;
                afTable.AddCell(mainCell);

                //Creating Nested table ends.

                mainCell = new PdfPCell(afTable);
                mainCell.Border = Rectangle.NO_BORDER;
                mainCell.Colspan = 7;
                mainTable.AddCell(mainCell);

                //mainParagraph = new Paragraph("To the premise owned/occupied by " +
                //"me/us and situated within  the area of supply specified in\n" +
                //"the Adani Electricity License, 2011.", RegularFont);
                mainParagraph = new Paragraph(" ", smallFont);
                mainCell = new PdfPCell(mainParagraph);
                mainCell.HorizontalAlignment = Element.ALIGN_LEFT;
                mainCell.Border = Rectangle.NO_BORDER;
                mainCell.Colspan = 8;

                mainTable.AddCell(mainCell);

                mainParagraph = new Paragraph("Note:Please do not staple the photograph", smallFont);
                mainParagraph.Alignment = Element.ALIGN_BOTTOM;
                mainCell = new PdfPCell(mainParagraph);
                mainCell.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(mainCell);
                mainTable.SpacingAfter = 10.0F;

                document.Add(mainTable);


                //Applicant's details starts.
                float[] adwidths = { 30, 210, 140, 100, 120, 210, 100, 160 };
                PdfPTable adTable = new PdfPTable(adwidths);
                adTable.WidthPercentage = 100.0F;
                PdfPCell adCell;
                Paragraph adParagraph;


                adParagraph = new Paragraph("1.", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Customer Details ", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(" ", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 6;
                adTable.AddCell(adCell);
                //----------------------------------


                adParagraph = new Paragraph(" ", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                //adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                if (con_data.TitleValue == "0006")
                {
                    adParagraph = new Paragraph("Applicant's Name M/S.", RegularFont);
                }
                else if (con_data.TitleValue == "0002")
                {
                    adParagraph = new Paragraph("Applicant's Name Mr.", RegularFont);
                }
                else if (con_data.TitleValue == "0001")
                {
                    adParagraph = new Paragraph("Applicant's Name Ms.", RegularFont);
                }
                else
                {
                    adParagraph = new Paragraph("Name Mr./Mrs./Ms. ", RegularFont);
                }
                //adParagraph = new Paragraph("Name Mr./Mrs./Ms.", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                //adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                string NAME = "";
                if (!string.IsNullOrEmpty(con_data.FirstName))
                {
                    NAME = !string.IsNullOrEmpty(con_data.FirstName) ? con_data.FirstName.ToString().Trim() : "";
                    if (!string.IsNullOrEmpty(con_data.MiddleName))
                    {
                        NAME += "                  ";
                        NAME += con_data.MiddleName.ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(con_data.LastName))
                    {
                        NAME += "                  ";
                        NAME += con_data.LastName.ToString().Trim();
                    }
                    adParagraph = new Paragraph(NAME.ToUpper(), RegularFont);
                }
                else if (!string.IsNullOrEmpty(con_data.OrgName))
                {

                    NAME = con_data.OrgName.ToString().Trim();

                    adParagraph = new Paragraph(NAME.ToUpper(), RegularFont);
                }
                else
                {
                    adParagraph = new Paragraph(" ", RegularFont);
                }
                adParagraph = new Paragraph(NAME.ToUpper(), RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //adCell.Border = Rectangle.NO_BORDER;
                adCell.FixedHeight = 10.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);

                adCell.Colspan = 6;
                adTable.AddCell(adCell);
                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                //adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                adParagraph = new Paragraph("(New Name in the event of Change Of Name)", smallFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                //adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(" ", smallFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_CENTER;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 2;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(" ", smallFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_CENTER;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 3;
                adTable.AddCell(adCell);
                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 3;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(" ", smallFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_CENTER;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 5;
                adTable.AddCell(adCell);
                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Existing Account No", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 17.0F;
                adTable.AddCell(adCell);

                //Existing Account Number
                if (!string.IsNullOrEmpty(con_data.AccountNumber))
                {
                    adParagraph = new Paragraph(con_data.AccountNumber.ToString().Trim().Substring(3), RegularFont);
                }
                else
                {
                    adParagraph = new Paragraph(" ", RegularFont);//
                }

                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 10.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 2;
                adTable.AddCell(adCell);



                adParagraph = new Paragraph("Mob No.", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);
                //Mobile Number
                if (!string.IsNullOrEmpty(con_data.MobileNumber))
                {
                    adParagraph = new Paragraph(con_data.MobileNumber.ToString().Trim(), RegularFont);
                }
                else
                {
                    adParagraph = new Paragraph("", RegularFont);
                }

                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 5.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(" ", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 2;
                adTable.AddCell(adCell);
                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 3;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(" ", smallFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_CENTER;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 5;
                adTable.AddCell(adCell);
                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Landline No", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 17.0F;
                adTable.AddCell(adCell);

                //Landline
                if (!string.IsNullOrEmpty(con_data.LandlineNumber))
                {
                    adParagraph = new Paragraph(con_data.LandlineNumber.ToString().Trim(), RegularFont);
                }
                else
                {
                    adParagraph = new Paragraph(" ", RegularFont);
                }

                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 10.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 2;
                adTable.AddCell(adCell);



                adParagraph = new Paragraph("Email", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                //Email
                if (!string.IsNullOrEmpty(con_data.EmailAddress))
                {
                    adParagraph = new Paragraph(con_data.EmailAddress.ToString().Trim(), RegularFont);
                }
                else
                {
                    adParagraph = new Paragraph("", RegularFont);
                }

                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 10.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 2;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                //------------------------


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("AADHAR No", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 17.0F;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(oAadhar, RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 10.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 2;
                adTable.AddCell(adCell);


                adParagraph = new Paragraph("PAN", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                adParagraph = new Paragraph(oPAN, RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 15.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 2;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);
                //------------------------



                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("GSTN", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 17.0F;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(oGSTN, RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 10.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 2;
                adTable.AddCell(adCell);


                adParagraph = new Paragraph(" ", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                adParagraph = new Paragraph("(As per Rule 114B of Income Tax Rules 1962) Refer point 19 from declaration cum undertaking", smallFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;

                adCell.Colspan = 3;
                adTable.AddCell(adCell);
                //------------------------



                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 17.0F;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("(AS per Rule 49 of GST Rules,2017)Refer point 19 from declaration cum underatking ", smallFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 3;
                adTable.AddCell(adCell);


                //adParagraph = new Paragraph(" ", RegularFont);
                //adCell = new PdfPCell(adParagraph);
                //adCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //adCell.Border = Rectangle.NO_BORDER;
                //adCell.Colspan = 3;
                //adTable.AddCell(adCell);

                //adTable.AddCell(adCell);
                //------------------------a

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.FixedHeight = 15.0F;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Bank Name", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.FixedHeight = 15.0F;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                string BANKNAME = " ";
                //BankName
                //if (!string.IsNullOrEmpty(con_data.B))
                //{
                //    BANKNAME = dsinput.Tables[0].Rows[0]["ZBANKA_BANKNAME"].ToString().Trim();
                //}
                //else
                //{
                //    BANKNAME = "";
                //}
                adParagraph = new Paragraph(BANKNAME, RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 15.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 6;
                adTable.AddCell(adCell);

                //Blank line starts
                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.FixedHeight = 5.0F;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //Blank line ends
                //------------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.FixedHeight = 15.0F;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);
                adParagraph = new Paragraph("Bank Branch.", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.FixedHeight = 15.0F;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                string BANKBRANCH = " ";
                //if (dsinput.Tables[0].Rows[0]["ZBRNCH_BANKNRANCH"].ToString().Trim() != "")
                //{
                //    BANKBRANCH = dsinput.Tables[0].Rows[0]["ZBRNCH_BANKNRANCH"].ToString().Trim();
                //}
                //else
                //{
                //    BANKBRANCH = "";
                //}
                adParagraph = new Paragraph(BANKBRANCH, RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 15.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 6;
                adTable.AddCell(adCell);
                //-----------------------------
                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);
                adParagraph = new Paragraph("Bank Account Number", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                //if (dsinput.Tables[0].Rows[0]["ZBANKN_BANKACCOUNTNO"].ToString().Trim() != "")
                //{
                //    adParagraph = new Paragraph(dsinput.Tables[0].Rows[0]["ZBANKN_BANKACCOUNTNO"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    adParagraph = new Paragraph("", RegularFont);
                //}
                adParagraph = new Paragraph(" ", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 10.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 6;
                adTable.AddCell(adCell);

                //Blank line starts
                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //Blank line ends
                //----------------------------------


                //2nd Start...

                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("2.", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Customer Power Supply Address ", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 2;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(" ", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 5;
                adTable.AddCell(adCell);

                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Flat No./Building Name", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                //HouseNumber
                if (!string.IsNullOrEmpty(con_data.IsAddressChangeRequired.ToString()))
                {
                    if (con_data.IsAddressChangeRequired == true)
                    {
                        if (!string.IsNullOrEmpty(con_data.Housenumber_New))
                        {
                            adParagraph = new Paragraph(con_data.Housenumber_New.ToString().Trim(), RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(con_data.HouseNumber))
                        {
                            adParagraph = new Paragraph(con_data.HouseNumber.ToString().Trim(), RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(con_data.HouseNumber))
                    {
                        adParagraph = new Paragraph(con_data.HouseNumber.ToString().Trim(), RegularFont);
                    }
                    else
                    {
                        adParagraph = new Paragraph("", RegularFont);
                    }
                }


                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 15.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                //BuildingName
                if (!string.IsNullOrEmpty(con_data.IsAddressChangeRequired.ToString()))
                {
                    if (con_data.IsAddressChangeRequired == true)
                    {
                        if (!string.IsNullOrEmpty(con_data.Streetname_New))
                        {
                            adParagraph = new Paragraph(con_data.Streetname_New.ToString().Trim(), RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(con_data.StreetName))
                        {
                            adParagraph = new Paragraph(con_data.StreetName.ToString().Trim(), RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(con_data.BuildingName))
                    {
                        adParagraph = new Paragraph(con_data.BuildingName.ToString().Trim(), RegularFont);
                    }
                    else
                    {
                        adParagraph = new Paragraph("", RegularFont);
                    }
                }

                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 15.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 4;
                adTable.AddCell(adCell);
                //------------------------


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //------------------------


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Lane/Street", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 17.0F;
                adTable.AddCell(adCell);

                //StreetName
                if (!string.IsNullOrEmpty(con_data.IsAddressChangeRequired.ToString()))
                {
                    if (con_data.IsAddressChangeRequired == true)
                    {
                        if (!string.IsNullOrEmpty(con_data.Area_New))
                        {
                            adParagraph = new Paragraph(con_data.Area_New.ToString().Trim(), RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(con_data.Area))
                        {
                            adParagraph = new Paragraph(con_data.Area.ToString().Trim(), RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(con_data.Area))
                    {
                        adParagraph = new Paragraph(con_data.Area.ToString().Trim(), RegularFont);
                    }
                    else
                    {
                        adParagraph = new Paragraph("", RegularFont);
                    }
                }

                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 15.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 6;
                adTable.AddCell(adCell);
                //------------------------


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //------------------------


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Landmark", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                if (!string.IsNullOrEmpty(con_data.IsAddressChangeRequired.ToString()))
                {
                    if (con_data.IsAddressChangeRequired == true)
                    {
                        if (!string.IsNullOrEmpty(con_data.Landmark_New))
                        {
                            adParagraph = new Paragraph(con_data.Landmark_New.ToString().Trim(), RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(con_data.Landmark))
                        {
                            adParagraph = new Paragraph(con_data.Landmark.ToString(), RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(con_data.Landmark))
                    {
                        adParagraph = new Paragraph(con_data.Landmark.ToString(), RegularFont);
                    }
                    else
                    {
                        adParagraph = new Paragraph("", RegularFont);
                    }
                }

                //adParagraph = new Paragraph("ADDRESS3", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 17.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 6;
                adTable.AddCell(adCell);
                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //------------------------


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Suburb/City", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 17.0F;
                adTable.AddCell(adCell);

                string oCity = "";
                if (!string.IsNullOrEmpty(con_data.IsAddressChangeRequired.ToString()))
                {
                    if (con_data.IsAddressChangeRequired == true)
                    {
                        if (!string.IsNullOrEmpty(con_data.Suburb_New) && !string.IsNullOrEmpty(con_data.City_New))
                        {
                            oCity = con_data.Suburb_New.ToString().Trim() + ", " + con_data.City_New.ToString().Trim();
                            adParagraph = new Paragraph(oCity, RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(con_data.City))
                        {
                            oCity = con_data.City.ToString().Trim();
                            adParagraph = new Paragraph(oCity, RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(con_data.City))
                    {
                        oCity = con_data.City.ToString().Trim();
                        adParagraph = new Paragraph(oCity, RegularFont);
                    }
                    else
                    {
                        adParagraph = new Paragraph("", RegularFont);
                    }
                }

                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 10.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 4;
                adTable.AddCell(adCell);



                adParagraph = new Paragraph("Pin code", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                if (!string.IsNullOrEmpty(con_data.IsAddressChangeRequired.ToString()))
                {
                    if (con_data.IsAddressChangeRequired == true)
                    {
                        if (!string.IsNullOrEmpty(con_data.Pincode_New))
                        {
                            adParagraph = new Paragraph(con_data.Pincode_New.ToString().Trim(), RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(con_data.Pincode))
                        {
                            adParagraph = new Paragraph(con_data.Pincode.ToString().Trim(), RegularFont);
                        }
                        else
                        {
                            adParagraph = new Paragraph("", RegularFont);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(con_data.Pincode))
                    {
                        adParagraph = new Paragraph(con_data.Pincode.ToString().Trim(), RegularFont);
                    }
                    else
                    {
                        adParagraph = new Paragraph("", RegularFont);
                    }
                }


                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 15.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);
                //------------------------


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //------------------------


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Area of premise", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 17.0F;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 10.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 1; ;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Sq.mtr.", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                adParagraph = new Paragraph(" ", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 4;
                adTable.AddCell(adCell);

                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.FixedHeight = 19.0F;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Existing/Nearest Consumer Account Number", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.FixedHeight = 19.0F;
                adCell.Colspan = 2;
                adTable.AddCell(adCell);


                //Existing/Nearest Account Number
                if (!string.IsNullOrEmpty(con_data.AccountNumber))
                {
                    adParagraph = new Paragraph(con_data.AccountNumber.ToString().Trim().Substring(3), RegularFont);
                }
                else
                {
                    adParagraph = new Paragraph("", RegularFont);
                }

                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 19.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 3;
                adTable.AddCell(adCell);



                adParagraph = new Paragraph("Meter Number", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.FixedHeight = 19.0F;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                if (!string.IsNullOrEmpty(con_data.MeterNumber))
                {
                    adParagraph = new Paragraph(con_data.MeterNumber.ToString().Trim(), RegularFont);
                }
                else
                {
                    adParagraph = new Paragraph("", RegularFont);
                }
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 19.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                //Blank line starts
                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);
                ////Blank line ends

                //Category of premises starts
                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                adParagraph = new Paragraph("Structure type", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 7;
                adTable.AddCell(adCell);


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                //float[] cpwidths = { 20, 140, 70, 20, 190, 20, 190, 20, 350 };
                float[] cpwidths = { 20, 30, 30, 20, 30, 20, 80, 20, 130, 20, 110, 300 };
                //float[] cpwidths = { 20, 70, 40, 20, 70, 20, 90, 20, 130, 20, 130, 150 };
                //float[] cpwidths = { 20, 70, 20,70, 20,70, 20,70,20,70, 350 };
                PdfPTable cpTable = new PdfPTable(cpwidths);
                Paragraph cpParagraph;
                PdfPCell cpCell;


                //Add checkbox
                String[] premisesCategory = { "Chawl", "Hut", "Bungalows", "Multistoried Bldg.", "Others(Specify) " };

                RadioCheckField checkboxpc = null;
                PdfFormField fieldpc;
                Rectangle rectpc = new Rectangle(10, 10, 15, 15);
                PdfFormField radiogrouppc = PdfFormField.CreateRadioButton(writer, true);
                radiogrouppc.FieldName = "PrmiseC";


                for (int i = 0; i < premisesCategory.Length; i++)
                {
                    checkboxpc = new RadioCheckField(writer, rectpc, premisesCategory[i], "Yes");
                    checkboxpc.BackgroundColor = BaseColor.WHITE;
                    checkboxpc.CheckType = RadioCheckField.TYPE_CIRCLE;
                    checkboxpc.BorderWidth = 0.25F;
                    if (premisesCategory[i].ToUpper().Equals(con_data.PremiseTypeName.ToString().Trim().ToUpper()))
                    {
                        checkboxpc.Checked = true;
                    }
                    else
                    {
                        if (premisesCategory[i].Equals("Others(Specify) "))
                            checkboxpc.Checked = true;
                    }
                    checkboxpc.BorderColor = new BaseColor(247, 170, 165);
                    checkboxpc.Options = BaseField.READ_ONLY;

                    fieldpc = checkboxpc.RadioField;
                    fieldpc.SetFieldFlags(PdfFormField.FF_READ_ONLY);

                    fieldpc = checkboxpc.RadioField;
                    fieldpc.SetWidget(rectAF, PdfAnnotation.HIGHLIGHT_INVERT);


                    radiogrouppc.AddKid(fieldpc);


                    writer.AddAnnotation(radiogrouppc);
                    //writer.AddAnnotation(fieldpc);

                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, fieldpc);
                    cell.Border = Rectangle.NO_BORDER;

                    cpTable.AddCell(cell);

                    Paragraph paragraph = new Paragraph(premisesCategory[i], RegularFont);
                    cell = new PdfPCell(paragraph);
                    cell.Border = Rectangle.NO_BORDER;
                    if (i == 0)
                        cell.Colspan = 2;
                    cpTable.AddCell(cell);
                }

                if (checkboxpc != null && checkboxpc.FieldName.ToString() == "Others(Specify) ")
                {
                    cpParagraph = new Paragraph(con_data.PremiseTypeName.ToString().Trim(), RegularFont);
                }
                else
                {
                    cpParagraph = new Paragraph("", RegularFont);
                }
                cpCell = new PdfPCell(cpParagraph);
                cpCell.HorizontalAlignment = Element.ALIGN_LEFT;
                cpCell.FixedHeight = 10.0F;
                cpCell.UseVariableBorders = true;
                cpCell.BorderColorTop = new BaseColor(255, 255, 255);
                cpCell.BorderColorLeft = new BaseColor(255, 255, 255);
                cpCell.BorderColorRight = new BaseColor(255, 255, 255);
                cpCell.BorderColorBottom = new BaseColor(23, 56, 133);
                cpCell.Colspan = 3;
                cpTable.AddCell(cpCell);

                cpParagraph = new Paragraph("");
                cpCell = new PdfPCell(cpParagraph);
                cpCell.Border = Rectangle.NO_BORDER;
                cpCell.Colspan = 4;
                cpTable.AddCell(cpCell);


                adCell = new PdfPCell(cpTable);
                adCell.Colspan = 7;
                adCell.Border = Rectangle.NO_BORDER;
                adTable.AddCell(adCell);

                //Category of premises ends               

                //------------------------
                //Blank line starts
                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);
                //Blank line ends




                //--------------------------

                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);





                //Blank line starts
                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);
                //Blank line ends

                //In case of premises on Rent or Lease or not owned by applicant starts

                //type of premises starts
                adParagraph = new Paragraph("3.", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Bill Delivery/Correspondence Address ", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 2;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(" ", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 5;
                adTable.AddCell(adCell);
                //-------------

                adParagraph = new Paragraph(" ", boldFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                //float[] bwidths = { 20, 210, 20, 190, 20, 190, 20, 130, 220 };
                float[] bdawidths = { 20, 800 };
                PdfPTable bdaTable = new PdfPTable(bdawidths);
                //Paragraph bdaParagraph;
                //PdfPCell bdaCell;

                //Add checkbox
                //String[] premisesType = { "Owned", "Rented", "Lease", "Others(Specify)" };
                String[] bdapremisesType = { "Same as Power Supply Address (In the event of bill Delivery Address diffrent than Power Supply Address)" };

                RadioCheckField checkboxbda;
                PdfFormField fieldbda;
                Rectangle rectbda = new Rectangle(10, 10, 15, 15);
                PdfFormField radiogroupbda = PdfFormField.CreateRadioButton(writer, true);
                radiogroupbda.FieldName = "BillDeliveryAddress";


                for (int i = 0; i < bdapremisesType.Length; i++)
                {
                    checkboxbda = new RadioCheckField(writer, rectbda, bdapremisesType[i], "Yes");
                    checkboxbda.BackgroundColor = BaseColor.WHITE;
                    checkboxbda.CheckType = RadioCheckField.TYPE_CIRCLE;
                    checkboxbda.BorderWidth = 0.25F;
                    checkboxbda.Checked = false;
                    //if (!string.IsNullOrEmpty(con_data.IsDiffrentBillingAddressValueOpt.ToString()))
                    //{
                    //    if (con_data.IsDiffrentBillingAddressValueOpt.ToString() != "True")
                    //    {
                    //        checkboxbda.Checked = true;
                    //    }
                    //}

                    checkboxbda.BorderColor = new BaseColor(247, 170, 165);
                    checkboxbda.Options = BaseField.READ_ONLY;

                    fieldbda = checkboxbda.RadioField;
                    fieldbda.SetFieldFlags(PdfFormField.FF_READ_ONLY);

                    fieldbda = checkboxbda.RadioField;
                    fieldbda.SetWidget(rectAF, PdfAnnotation.HIGHLIGHT_INVERT);

                    radiogroupbda.AddKid(fieldbda);


                    writer.AddAnnotation(radiogroupbda);

                    //writer.AddAnnotation(fieldpt);

                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, fieldbda);
                    cell.Border = Rectangle.NO_BORDER;

                    bdaTable.AddCell(cell);

                    Paragraph paragraph = new Paragraph(bdapremisesType[i], RegularFont);
                    cell = new PdfPCell(paragraph);
                    cell.Border = Rectangle.NO_BORDER;
                    bdaTable.AddCell(cell);
                }

                adCell = new PdfPCell(bdaTable);
                adCell.Colspan = 7;
                adCell.Border = Rectangle.NO_BORDER;
                adTable.AddCell(adCell);

                //Blank line starts
                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);
                //Blank line ends

                //Blank line starts
                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //Blank line ends

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Flat No/Building Name", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.FixedHeight = 18.0F;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                //if (dsinput.Tables[0].Rows[0]["ZHOUSE_NO_B"].ToString().Trim() != "")
                //{
                //    adParagraph = new Paragraph(dsinput.Tables[0].Rows[0]["ZHOUSE_NO_B"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    adParagraph = new Paragraph("", RegularFont);
                //}
                adParagraph = new Paragraph(" ", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 18.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                //if (dsinput.Tables[0].Rows[0]["ZSTREET_B"].ToString().Trim() != "")
                //{
                //    adParagraph = new Paragraph(dsinput.Tables[0].Rows[0]["ZSTREET_B"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    adParagraph = new Paragraph("", RegularFont);
                //}
                adParagraph = new Paragraph(" ", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 18.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 4;
                adTable.AddCell(adCell);


                //Blank line starts
                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //Blank line ends

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Lane/Street", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 18.0F;
                adTable.AddCell(adCell);

                //if (dsinput.Tables[0].Rows[0]["ZSTREET1_B"].ToString().Trim() != "")
                //{
                //    adParagraph = new Paragraph(dsinput.Tables[0].Rows[0]["ZSTREET1_B"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    adParagraph = new Paragraph("", RegularFont);
                //}
                adParagraph = new Paragraph(" ", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 18.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 6;
                adTable.AddCell(adCell);


                //Blank line starts
                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //Blank line ends



                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);
                adParagraph = new Paragraph("Landmark", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 18.0F;
                adTable.AddCell(adCell);

                //if (dsinput.Tables[0].Rows[0]["ZSTREET2_B"].ToString().Trim() != "")
                //{
                //    adParagraph = new Paragraph(dsinput.Tables[0].Rows[0]["ZSTREET2_B"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    adParagraph = new Paragraph("", RegularFont);
                //}
                adParagraph = new Paragraph(" ", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 18.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 6;
                adTable.AddCell(adCell);


                //Blank line starts
                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adTable.AddCell(adCell);
                //Blank line ends



                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Suburb/City", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adCell.FixedHeight = 18.0F;
                adTable.AddCell(adCell);

                //if (dsinput.Tables[0].Rows[0]["ZSURB_B"].ToString().Trim() != "")
                //{
                //    adParagraph = new Paragraph(dsinput.Tables[0].Rows[0]["ZSURB_B"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    adParagraph = new Paragraph("", RegularFont);
                //}
                adParagraph = new Paragraph(" ", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 18.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 4;
                adTable.AddCell(adCell);



                adParagraph = new Paragraph("Pin code", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                //if (dsinput.Tables[0].Rows[0]["ZPIN_B"].ToString().Trim() != "")
                //{
                //    adParagraph = new Paragraph(dsinput.Tables[0].Rows[0]["ZPIN_B"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    adParagraph = new Paragraph("", RegularFont);
                //}
                adParagraph = new Paragraph(" ", RegularFont);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.FixedHeight = 18.0F;
                adCell.UseVariableBorders = true;
                adCell.BorderColorTop = new BaseColor(255, 255, 255);
                adCell.BorderColorLeft = new BaseColor(255, 255, 255);
                adCell.BorderColorRight = new BaseColor(255, 255, 255);
                adCell.BorderColorBottom = new BaseColor(23, 56, 133);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                //Blank line starts
                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);
                //Blank line ends

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                //Preferred Bill Language is 
                float[] blwidths = { 270, 20, 90, 20, 90, 20, 90, 640 };
                PdfPTable blTable = new PdfPTable(blwidths);
                Paragraph blParagraph;
                PdfPCell blCell;


                blParagraph = new Paragraph("Preferred Bill Language is", RegularFont);
                blCell = new PdfPCell(blParagraph);
                blCell.HorizontalAlignment = Element.ALIGN_LEFT;
                blCell.Border = Rectangle.NO_BORDER;
                blCell.Colspan = 1;
                blTable.AddCell(blCell);

                //Add checkbox

                String[] bl = { "Marathi", "Hindi", "English" };

                string LANGUAGE = "";
                if (!string.IsNullOrEmpty(con_data.BillingLanguage))
                {
                    LANGUAGE = con_data.BillingLanguage.ToString().Trim();
                    if (LANGUAGE == "EN")
                    {
                        LANGUAGE = "English";
                    }
                    else if (LANGUAGE == "Z2")
                    {
                        LANGUAGE = "Hindi";
                    }
                    else if (LANGUAGE == "Z3")
                    {
                        LANGUAGE = "Marathi";
                    }
                }
                else
                {
                    LANGUAGE = "";
                }

                PdfFormField fieldbl;
                Rectangle rectbl = new Rectangle(10, 10, 15, 15);
                PdfFormField radiogroupbl = PdfFormField.CreateRadioButton(writer, true);
                radiogroupbl.FieldName = "Language";
                RadioCheckField checkboxbl;


                for (int i = 0; i < bl.Length; i++)
                {
                    checkboxbl = new RadioCheckField(writer, rectbl, bl[i], "Yes");
                    checkboxbl.BackgroundColor = BaseColor.WHITE;
                    checkboxbl.CheckType = RadioCheckField.TYPE_CIRCLE;
                    checkboxbl.BorderWidth = 0.25F;

                    if (bl[i].Equals(LANGUAGE))
                        checkboxbl.Checked = true;

                    ////checkboxAF.BackgroundColor = new BaseColor(253, 230, 220);
                    checkboxbl.BorderColor = new BaseColor(247, 170, 165);
                    checkboxbl.Options = BaseField.READ_ONLY;

                    fieldbl = checkboxbl.RadioField;
                    fieldbl.SetFieldFlags(PdfFormField.FF_READ_ONLY);

                    fieldbl = checkboxbl.RadioField;
                    fieldbl.SetWidget(rectbl, PdfAnnotation.HIGHLIGHT_INVERT);

                    radiogroupbl.AddKid(fieldbl);

                    writer.AddAnnotation(radiogroupbl);


                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, fieldbl);
                    cell.Border = Rectangle.NO_BORDER;

                    blTable.AddCell(cell);

                    Paragraph paragraph = new Paragraph(bl[i], RegularFont);
                    cell = new PdfPCell(paragraph);
                    cell.Border = Rectangle.NO_BORDER;
                    blTable.AddCell(cell);
                }
                blParagraph = new Paragraph("");
                blCell = new PdfPCell(blParagraph);
                blCell.Border = Rectangle.NO_BORDER;
                blCell.Colspan = 1;
                blTable.AddCell(blCell);

                adCell = new PdfPCell(blTable);
                adCell.Colspan = 7;
                adCell.Border = Rectangle.NO_BORDER;
                adTable.AddCell(adCell);


                //Blank line starts
                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);
                //Blank line ends

                //Blank line starts
                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);
                //Blank line ends

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                //I Want my electricity Bill 
                //float[] blwidths = { 270, 20, 90, 20, 90, 20, 90, 20, 90, 530 };
                //float[] eblwidths = { 270, 20, 150, 20, 140, 20, 210, 400 };
                float[] eblwidths = { 270, 20, 250, 20, 210, 460 };
                PdfPTable eblTable = new PdfPTable(eblwidths);
                Paragraph eblParagraph;
                PdfPCell eblCell;


                eblParagraph = new Paragraph("I want my electricity bill", RegularFont);
                eblCell = new PdfPCell(eblParagraph);
                eblCell.HorizontalAlignment = Element.ALIGN_LEFT;
                eblCell.Border = Rectangle.NO_BORDER;
                eblCell.Colspan = 1;
                eblTable.AddCell(eblCell);

                //Add checkbox
                //String[] bl = { "Marathi", "Hindi", "Gujrati", "English" };
                //String[] ebl = { "On Email", "Whatsapp", "As a Hard Copy" };
                String[] ebl = { "E-Bill + Hard Copy", "Paperless" };

                string eLANGUAGE = "";
                if (!string.IsNullOrEmpty(con_data.PaperlessBillingFlag.ToString()) && !string.IsNullOrEmpty(con_data.EbillPhysicalFlag.ToString()))
                {
                    eLANGUAGE = con_data.PaperlessBillingFlag.ToString().Trim();
                    if (eLANGUAGE == "True")
                    {
                        eLANGUAGE = "Paperless";
                    }
                    else
                    {
                        eLANGUAGE = "E-Bill + Hard Copy";
                    }
                    //else if (LANGUAGE == "Z4")
                    //{
                    //    LANGUAGE = "Gujrati";
                    //}
                }
                else
                {
                    eLANGUAGE = "";
                }

                PdfFormField fieldebl;
                //Rectangle rectebl = new Rectangle(10, 10, 15, 15);
                Rectangle rectebl = new Rectangle(10, 10, 15);
                PdfFormField radiogroupebl = PdfFormField.CreateRadioButton(writer, true);
                radiogroupebl.FieldName = "Bill";
                RadioCheckField checkboxebl;


                for (int i = 0; i < ebl.Length; i++)
                {
                    checkboxebl = new RadioCheckField(writer, rectebl, ebl[i], "Yes");
                    checkboxebl.BackgroundColor = BaseColor.WHITE;
                    checkboxebl.CheckType = RadioCheckField.TYPE_CIRCLE;
                    checkboxebl.BorderWidth = 0.25F;

                    if (ebl[i].Equals(eLANGUAGE))
                        checkboxebl.Checked = true;

                    ////checkboxAF.BackgroundColor = new BaseColor(253, 230, 220);
                    checkboxebl.BorderColor = new BaseColor(247, 170, 165);
                    checkboxebl.Options = BaseField.READ_ONLY;

                    fieldebl = checkboxebl.RadioField;
                    fieldebl.SetFieldFlags(PdfFormField.FF_READ_ONLY);

                    fieldebl = checkboxebl.RadioField;
                    fieldebl.SetWidget(rectebl, PdfAnnotation.HIGHLIGHT_INVERT);

                    radiogroupebl.AddKid(fieldebl);

                    writer.AddAnnotation(radiogroupebl);


                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, fieldebl);
                    cell.Border = Rectangle.NO_BORDER;

                    eblTable.AddCell(cell);

                    Paragraph paragraph = new Paragraph(ebl[i], RegularFont);
                    cell = new PdfPCell(paragraph);
                    cell.Border = Rectangle.NO_BORDER;
                    eblTable.AddCell(cell);
                }
                eblParagraph = new Paragraph("");
                eblCell = new PdfPCell(eblParagraph);
                eblCell.Border = Rectangle.NO_BORDER;
                eblCell.Colspan = 1;
                eblTable.AddCell(eblCell);

                adCell = new PdfPCell(eblTable);
                adCell.Colspan = 7;
                adCell.Border = Rectangle.NO_BORDER;
                adTable.AddCell(adCell);


                //Blank line starts
                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);
                //Blank line ends

                //type of premises starts
                adParagraph = new Paragraph("4.", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph("Types of premises ", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.BackgroundColor = new BaseColor(171, 49, 117);
                adCell.Colspan = 1;
                adTable.AddCell(adCell);

                adParagraph = new Paragraph(" ", RegularFontWhite);
                adCell = new PdfPCell(adParagraph);
                adCell.HorizontalAlignment = Element.ALIGN_LEFT;
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 6;
                adTable.AddCell(adCell);
                //-------------

                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);


                //------------------------

                adParagraph = new Paragraph("");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 1;
                adTable.AddCell(adCell);


                float[] pwidths = { 20, 210, 20, 190, 20, 190, 20, 130, 220 };
                PdfPTable pTable = new PdfPTable(pwidths);
                Paragraph pParagraph;
                PdfPCell pCell;

                //Add checkbox
                String[] premisesType = { "Owned", "Rented", "Lease", "Others(Specify)" };
                //String[] premisesType = { "Owned", "Rented" };
                RadioCheckField checkboxpt;
                PdfFormField fieldpt;
                Rectangle rectpt = new Rectangle(10, 10, 15, 15);
                PdfFormField radiogrouppt = PdfFormField.CreateRadioButton(writer, true);
                radiogrouppt.FieldName = "Premise";


                for (int i = 0; i < premisesType.Length; i++)
                {
                    checkboxpt = new RadioCheckField(writer, rectpt, premisesType[i], "Yes");
                    checkboxpt.BackgroundColor = BaseColor.WHITE;
                    checkboxpt.CheckType = RadioCheckField.TYPE_CIRCLE;
                    checkboxpt.BorderWidth = 0.25F;

                    if (con_data.IsRentedProperty.ToString().Trim() == "True")
                    {
                        if (premisesType[i].Equals("Rented"))
                            checkboxpt.Checked = true;
                    }
                    else
                    {
                        if (premisesType[i].Equals("Others(Specify)"))
                            checkboxpt.Checked = true;
                    }
                    checkboxpt.BorderColor = new BaseColor(247, 170, 165);
                    checkboxpt.Options = BaseField.READ_ONLY;

                    fieldpt = checkboxpt.RadioField;
                    fieldpt.SetFieldFlags(PdfFormField.FF_READ_ONLY);
                    fieldpt = checkboxpt.RadioField;
                    fieldpt.SetWidget(rectAF, PdfAnnotation.HIGHLIGHT_INVERT);

                    radiogrouppt.AddKid(fieldpt);


                    writer.AddAnnotation(radiogrouppt);

                    //writer.AddAnnotation(fieldpt);

                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, fieldpt);
                    cell.Border = Rectangle.NO_BORDER;

                    pTable.AddCell(cell);

                    Paragraph paragraph = new Paragraph(premisesType[i], RegularFont);
                    cell = new PdfPCell(paragraph);
                    cell.Border = Rectangle.NO_BORDER;
                    pTable.AddCell(cell);
                }
                if (checkboxpc != null && checkboxpc.FieldName.ToString() == "Others(Specify) ")
                {
                    pParagraph = new Paragraph(con_data.PremiseTypeName.ToString().Trim(), RegularFont);
                }
                else
                {
                    pParagraph = new Paragraph("", RegularFont);
                }
                //pParagraph = new Paragraph("", RegularFont);
                pCell = new PdfPCell(pParagraph);
                pCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pCell.FixedHeight = 10.0F;
                pCell.UseVariableBorders = true;
                pCell.BorderColorTop = new BaseColor(255, 255, 255);
                pCell.BorderColorLeft = new BaseColor(255, 255, 255);
                pCell.BorderColorRight = new BaseColor(255, 255, 255);
                pCell.BorderColorBottom = new BaseColor(23, 56, 133);
                pCell.Colspan = 1;
                pTable.AddCell(pCell);

                adCell = new PdfPCell(pTable);
                adCell.Colspan = 7;
                adCell.Border = Rectangle.NO_BORDER;
                adTable.AddCell(adCell);


                //type of premises ends

                //Blank line starts
                adParagraph = new Paragraph(" ");
                adCell = new PdfPCell(adParagraph);
                adCell.Border = Rectangle.NO_BORDER;
                adCell.Colspan = 8;
                adCell.PaddingTop = -10.0F;
                adTable.AddCell(adCell);
                //Blank line ends

                //In case of premises on Rent or Lease or not owned by applicant ends 

                document.Add(adTable);

                Paragraph paragraph2;
                PdfPCell cell2;
                //float[] widths2 = { 30, 40, 20, 110, 20, 60, 20, 180, 20, 250, 20, 250, 70 };
                float[] widths2 = { 30, 20, 130, 20, 130, 20, 150, 20, 150, 20, 230, 200 };

                PdfPTable table2 = new PdfPTable(widths2);
                table2.WidthPercentage = 100.0F;
                paragraph2 = new Paragraph("5.", RegularFontWhite);
                cell2 = new PdfPCell(paragraph2);
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = Rectangle.NO_BORDER;
                cell2.BackgroundColor = new BaseColor(171, 49, 117);
                cell2.Colspan = 1;
                table2.AddCell(cell2);

                paragraph2 = new Paragraph("Purpose of Supply:", RegularFontWhite);
                cell2 = new PdfPCell(paragraph2);
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = Rectangle.NO_BORDER;
                cell2.BackgroundColor = new BaseColor(171, 49, 117);
                cell2.Colspan = 3;
                table2.AddCell(cell2);


                paragraph2 = new Paragraph("(In the event of New Connection)", smallFontWhite);
                cell2 = new PdfPCell(paragraph2);
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = Rectangle.NO_BORDER;
                cell2.BackgroundColor = new BaseColor(171, 49, 117);
                cell2.Colspan = 5;
                table2.AddCell(cell2);

                paragraph2 = new Paragraph(" ", smallFontWhite);
                cell2 = new PdfPCell(paragraph2);
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = Rectangle.NO_BORDER;
                //cell2.BackgroundColor = new BaseColor(171, 49, 117);
                cell2.Colspan = 2;
                table2.AddCell(cell2);

                paragraph2 = new Paragraph("", boldFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 3;
                table2.AddCell(cell2);

                //5.1 starts

                paragraph2 = new Paragraph("");
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 1;
                table2.AddCell(cell2);


                //String[] requirement1 = { "Residential", "BPL", "Charitable/Religious", "Non-commercial/Educational", "Others(Specify)  " };
                String[] requirement1 = { "Residential", "Commercial", "Temp-Religious", "Temp-Others", "Contsruction Power" };

                RadioCheckField checkbox1;
                PdfFormField field1;
                Rectangle rect1 = new Rectangle(10, 10, 15, 15);
                PdfFormField radiogroup1 = PdfFormField.CreateRadioButton(writer, true);
                radiogroup1.FieldName = "Requirement1";


                for (int i = 0; i < requirement1.Length; i++)
                {
                    checkbox1 = new RadioCheckField(writer, rect1, requirement1[i], "Yes");
                    checkbox1.BackgroundColor = BaseColor.WHITE;
                    checkbox1.CheckType = RadioCheckField.TYPE_CIRCLE;
                    checkbox1.BorderWidth = 0.25F;
                    //checkbox1.BorderWidth(BaseField.BORDER_WIDTH_THIN);
                    checkbox1.BorderColor = new BaseColor(247, 170, 165);
                    checkbox1.Options = BaseField.READ_ONLY;
                    //if((objTCBean.getRESI_LOAD_KW()!=null && !objTCBean.getRESI_LOAD_KW().equals("")   && i==0) )
                    //checkbox1.setChecked(true);
                    //string oTarifType = "";
                    //if (dsCADetails.Tables["OUTPUT_TABLE"].Rows[0]["TARIFTYP"].ToString().Trim().ToUpper() == "BPL" || dsCADetails.Tables["OUTPUT_TABLE"].Rows[0]["TARIFTYP"].ToString().Trim().ToUpper() == "RESI1" || dsCADetails.Tables["OUTPUT_TABLE"].Rows[0]["TARIFTYP"].ToString().Trim().ToUpper() == "RESI3")
                    //{
                    //    oTarifType = "Residential";
                    //}
                    //if (dsCADetails.Tables["OUTPUT_TABLE"].Rows[0]["TARIFTYP"].ToString().Trim().ToUpper() == "COMM" || dsCADetails.Tables["OUTPUT_TABLE"].Rows[0]["TARIFTYP"].ToString().Trim().ToUpper() == "COMM(A)" || dsCADetails.Tables["OUTPUT_TABLE"].Rows[0]["TARIFTYP"].ToString().Trim().ToUpper() == "COMM(B)")
                    //{
                    //    oTarifType = "Commercial";
                    //}
                    //oTarifType = "Residential";
                    //if (requirement1[i].ToUpper().Equals(dsinput.Tables[0].Rows[0]["ZPSUPP_PURPOSEOFSUPPLY"].ToString().Trim().ToUpper()))

                    //if (oTarifType.Trim() != "")
                    //{
                    //    if (requirement1[i].ToUpper().Equals(oTarifType.ToUpper()))
                    //    {
                    //        checkbox1.Checked = true;
                    //    }
                    //    else
                    //    {
                    //        //if (requirement1[i].Equals("Others(Specify)  "))
                    //        //checkbox1.Checked = true;
                    //    }
                    //}
                    checkbox1.Checked = false;


                    field1 = checkbox1.RadioField;
                    field1.SetFieldFlags(PdfFormField.FF_READ_ONLY);

                    field1 = checkbox1.RadioField;
                    field1.SetWidget(rectAF, PdfAnnotation.HIGHLIGHT_INVERT);

                    radiogroup1.AddKid(field1);


                    writer.AddAnnotation(radiogroup1);

                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, field1);
                    cell.Border = Rectangle.NO_BORDER;

                    table2.AddCell(cell);

                    Paragraph paragraph = new Paragraph(requirement1[i], RegularFont);
                    cell = new PdfPCell(paragraph);
                    cell.Border = Rectangle.NO_BORDER;
                    table2.AddCell(cell);
                }

                //paragraph2 = new Paragraph(dsinput.Tables[0].Rows[0]["ZPSUPP_PURPOSEOFSUPPLY"].ToString().Trim(), RegularFont);
                paragraph2 = new Paragraph("", RegularFont);
                ////paragraph2 = new Paragraph(dsCADetails.Tables["OUTPUT_TABLE"].Rows[0]["TARIFTYP"].ToString().Trim(), RegularFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 1;
                table2.AddCell(cell2);

                //Blank line starts
                paragraph2 = new Paragraph("");
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 14;
                cell2.PaddingTop = -10.0F;
                table2.AddCell(cell2);
                //Blank line ends

                //4 Starts
                paragraph2 = new Paragraph("");
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 1;
                table2.AddCell(cell2);

                String[] requirement4 = { "Others(Specify)" };

                RadioCheckField checkbox4;
                PdfFormField field4;
                Rectangle rect4 = new Rectangle(10, 10, 15, 15);
                PdfFormField radiogroup4 = PdfFormField.CreateRadioButton(writer, true);
                radiogroup4.FieldName = "Requirement4";


                for (int i = 0; i < requirement4.Length; i++)
                {
                    checkbox4 = new RadioCheckField(writer, rect4, requirement4[i], "Yes");
                    checkbox4.BackgroundColor = BaseColor.WHITE;
                    checkbox4.CheckType = RadioCheckField.TYPE_CIRCLE;
                    checkbox4.BorderWidth = 0.25F;
                    //checkbox1.BorderWidth(BaseField.BORDER_WIDTH_THIN);
                    checkbox4.BorderColor = new BaseColor(247, 170, 165);
                    checkbox4.Options = BaseField.READ_ONLY;
                    //if((objTCBean.getRESI_LOAD_KW()!=null && !objTCBean.getRESI_LOAD_KW().equals("")   && i==0) )
                    //checkbox1.setChecked(true);
                    //if (requirement4[i].ToUpper().Equals(dsinput.Tables[0].Rows[0]["ZPSUPP_PURPOSEOFSUPPLY"].ToString().Trim().ToUpper()))
                    //{
                    //    checkbox4.Checked = true;
                    //}
                    //"Residential", "Commercial", "Temp-Religious", "Temp-Others", "Contsruction Power"
                    //if (dsinput.Tables[0].Rows[0]["ZPSUPP_PURPOSEOFSUPPLY"].ToString().Trim().ToUpper() != "RESIDENTIAL" && dsinput.Tables[0].Rows[0]["ZPSUPP_PURPOSEOFSUPPLY"].ToString().Trim().ToUpper() != "COMMERCIAL" && dsinput.Tables[0].Rows[0]["ZPSUPP_PURPOSEOFSUPPLY"].ToString().Trim().ToUpper() != "TEMP-RELIGIOUS" && dsinput.Tables[0].Rows[0]["ZPSUPP_PURPOSEOFSUPPLY"].ToString().Trim().ToUpper() != "TEMP-OTHERS" && dsinput.Tables[0].Rows[0]["ZPSUPP_PURPOSEOFSUPPLY"].ToString().Trim().ToUpper() != "CONTSRUCTION POWER")
                    //{
                    //    //checkbox4.Checked = true;
                    //}

                    field4 = checkbox4.RadioField;
                    field4.SetFieldFlags(PdfFormField.FF_READ_ONLY);

                    field4 = checkbox4.RadioField;
                    field4.SetWidget(rectAF, PdfAnnotation.HIGHLIGHT_INVERT);

                    radiogroup4.AddKid(field4);


                    writer.AddAnnotation(radiogroup4);

                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, field4);
                    cell.Border = Rectangle.NO_BORDER;

                    table2.AddCell(cell);

                    Paragraph paragraph = new Paragraph(requirement4[i], RegularFont);
                    cell = new PdfPCell(paragraph);
                    cell.Border = Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    table2.AddCell(cell);
                }

                //paragraph2 = new Paragraph(dsinput.Tables[0].Rows[0]["ZPSUPP_PURPOSEOFSUPPLY"].ToString().Trim(), RegularFont);
                paragraph2 = new Paragraph(" ", RegularFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.FixedHeight = 10.0F;
                cell2.UseVariableBorders = true;
                cell2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2.BorderColorRight = new BaseColor(255, 255, 255);
                cell2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2.Colspan = 3;
                table2.AddCell(cell2);

                paragraph2 = new Paragraph(" ", RegularFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.FixedHeight = 10.0F;
                cell2.UseVariableBorders = true;
                cell2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2.BorderColorRight = new BaseColor(255, 255, 255);
                cell2.BorderColorBottom = new BaseColor(255, 255, 255);
                cell2.Colspan = 7;
                table2.AddCell(cell2);


                paragraph2 = new Paragraph(" ", RegularFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 1;
                table2.AddCell(cell2);


                //paragraph2 = new Paragraph("\nPeroid of Temp Connection (Max. 1 Year):  From :  " + dsinput.Tables[0].Rows[0]["Z_VBEGIN_TEMPFROMDATE"].ToString(), RegularFont);
                paragraph2 = new Paragraph("\nPeroid of Temp Connection (Max. 1 Year):  From :  ", RegularFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 8;
                table2.AddCell(cell2);


                //paragraph2 = new Paragraph("\nFrom :" + dsinput.Tables[0].Rows[0]["Z_VBEGIN_TEMPFROMDATE"].ToString() , RegularFont);                
                //cell2 = new PdfPCell(paragraph2);
                //paragraph2 = new Paragraph("\nPeroid of Temp Connection (Max. 1 Year):  From :  " , RegularFont);
                //cell2.Colspan = 3;
                //table2.AddCell(cell2);


                //paragraph2 = new Paragraph("\nTo :  " + dsinput.Tables[0].Rows[0]["Z_VENDE_TEMPTODATE"].ToString(), RegularFont);
                paragraph2 = new Paragraph("\nTo :  ", RegularFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 6;
                table2.AddCell(cell2);

                //--------------

                paragraph2 = new Paragraph(" ", RegularFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 1;
                table2.AddCell(cell2);




                paragraph2 = new Paragraph("\nIndustrial certificate valid till :   ", RegularFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 5;
                table2.AddCell(cell2);


                paragraph2 = new Paragraph("\n", RegularFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 4;
                table2.AddCell(cell2);


                paragraph2 = new Paragraph("\n", RegularFont);
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 4;
                table2.AddCell(cell2);


                //paragraph2 = new Paragraph(" ", RegularFont);                
                //cell2 = new PdfPCell(paragraph2);
                //cell2.Border = Rectangle.NO_BORDER;                
                //cell2.Colspan = 1;
                //table2.AddCell(cell2);


                //Blank line starts
                paragraph2 = new Paragraph("");
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 14;
                cell2.PaddingTop = -10.0F;
                table2.AddCell(cell2);
                //Blank line ends

                document.Add(table2);
                ////5.1 ends
                //1st Page Ends...



                //2nd Page Starts...

                //document.NewPage();

                //6 starts
                float[] widths2_2 = { 30, 140, 145, 245, 245, 245 };
                PdfPTable table2_2 = new PdfPTable(widths2_2);
                table2_2.WidthPercentage = 100.0F;
                Paragraph paragraph2_2;
                PdfPCell cell2_2;

                paragraph2_2 = new Paragraph("6", RegularFontWhite);
                cell2_2 = new PdfPCell(paragraph2_2);
                cell2_2.Border = Rectangle.NO_BORDER;
                cell2_2.BackgroundColor = new BaseColor(171, 49, 117);
                cell2_2.Colspan = 1;
                table2_2.AddCell(cell2_2);


                paragraph2_2 = new Paragraph("Load/Contract Demand", RegularFontWhite);
                cell2_2 = new PdfPCell(paragraph2_2);
                cell2_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2_2.Border = Rectangle.NO_BORDER;
                cell2_2.BackgroundColor = new BaseColor(171, 49, 117);
                cell2_2.Colspan = 2;
                table2_2.AddCell(cell2_2);

                paragraph2_2 = new Paragraph("(In the event of New Connection,Extention/Reduction of Load Contract Demand)", smallFontWhite);
                cell2_2 = new PdfPCell(paragraph2_2);
                cell2_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2_2.Border = Rectangle.NO_BORDER;
                cell2_2.BackgroundColor = new BaseColor(171, 49, 117);
                cell2_2.Colspan = 2;
                table2_2.AddCell(cell2_2);

                paragraph2_2 = new Paragraph("");
                cell2_2 = new PdfPCell(paragraph2_2);
                cell2_2.Border = Rectangle.NO_BORDER;
                cell2_2.Colspan = 1;
                table2_2.AddCell(cell2_2);

                //Blank line starts
                paragraph2 = new Paragraph("");
                cell2 = new PdfPCell(paragraph2);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.Colspan = 6;
                cell2.PaddingTop = -10.0F;
                table2_2.AddCell(cell2);
                //Blank line ends

                document.Add(table2_2);

                //-------------------

                float[] widths2_2_2 = { 30, 400, 100, 100, 100, 100 };
                PdfPTable table2_2_2 = new PdfPTable(widths2_2_2);
                table2_2_2.WidthPercentage = 100.0F;
                Paragraph paragraph2_2_2;
                PdfPCell cell2_2_2;

                //Blank Row
                paragraph2_2_2 = new Paragraph("");
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.Border = Rectangle.NO_BORDER;
                cell2_2_2.Colspan = 6;
                cell2_2_2.FixedHeight = 5.0F;
                table2_2_2.AddCell(cell2_2_2);

                //1st Row

                //HEading
                paragraph2_2_2 = new Paragraph("");
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.Border = Rectangle.NO_BORDER;
                cell2_2_2.Colspan = 1;
                cell2_2_2.Rowspan = 2;
                table2_2_2.AddCell(cell2_2_2);


                paragraph2_2_2 = new Paragraph("Particular", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.Rowspan = 2;
                cell2_2_2.HorizontalAlignment = 0;
                cell2_2_2.VerticalAlignment = 1;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(23, 56, 133);
                cell2_2_2.BorderColorTop = new BaseColor(23, 56, 133);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);

                table2_2_2.AddCell(cell2_2_2);


                paragraph2_2_2 = new Paragraph("Existing", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.Colspan = 2;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(23, 56, 133);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);

                table2_2_2.AddCell(cell2_2_2);


                paragraph2_2_2 = new Paragraph("Required", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.Colspan = 2;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(23, 56, 133);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                table2_2_2.AddCell(cell2_2_2);

                //2nd Row

                paragraph2_2_2 = new Paragraph("kW", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph("HP", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph("kW", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph("HP", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);


                //3rd Row                
                paragraph2_2_2 = new Paragraph("");
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.Border = Rectangle.NO_BORDER;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph("New Load", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(23, 56, 133);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph("", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph("", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);


                //string Totalapplied_2_2_2 = ((Convert.ToDouble(ds.Tables[0].Rows[0]["ZALKW_1"]) + Convert.ToDouble(ds.Tables[0].Rows[0]["ZALKW_3"])
                //                    + Convert.ToDouble(ds.Tables[0].Rows[0]["ZALKW_H"]))
                //                    + ((0.745) * (Convert.ToDouble(ds.Tables[0].Rows[0]["ZALHP_1"])
                //                    + Convert.ToDouble(ds.Tables[0].Rows[0]["ZALHP_3"])
                //                    + Convert.ToDouble(ds.Tables[0].Rows[0]["ZALHP_H"])))).ToString() + " kW";

                //paragraph2_2_2 = new Paragraph(Totalapplied_2_2_2, RegularFont);
                paragraph2_2_2 = new Paragraph("", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);


                paragraph2_2_2 = new Paragraph(" ", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_1.BorderColorLeft = new BaseColor(23, 56, 133);
                //cell2_2_1.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133); ;
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                //4th row

                paragraph2_2_2 = new Paragraph("");
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.Border = Rectangle.NO_BORDER;
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph("Change of Load", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(23, 56, 133);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph(" ", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph(" ", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph(" ", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph(" ", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                //5th Row               
                paragraph2_2_2 = new Paragraph("");
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.Border = Rectangle.NO_BORDER;
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph("Contract demand in kVA(Non-Residentail Load > 20kW)", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(23, 56, 133);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 1;
                table2_2_2.AddCell(cell2_2_2);

                paragraph2_2_2 = new Paragraph(" ", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                //cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 2;
                table2_2_2.AddCell(cell2_2_2);



                //if (ds.Tables[0].Rows[0]["ZALCD"].ToString().Trim() == "0" || ds.Tables[0].Rows[0]["ZALCD"].ToString().Trim() == "00")
                //{
                //    //tParagraph = new Paragraph(ds.Tables[0].Rows[0]["ZALCD"].ToString().Trim(), RegularFont);
                //    paragraph2_2_2 = new Paragraph(ds.Tables[0].Rows[0]["ZALCD"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    //tParagraph = new Paragraph(ds.Tables[0].Rows[0]["ZALCD"].ToString().Trim() + "kva", RegularFont);
                //    paragraph2_2_2 = new Paragraph(ds.Tables[0].Rows[0]["ZALCD"].ToString().Trim() + "kVA", RegularFont);
                //}
                //cell2_2_2 = new PdfPCell(paragraph2_2_2);
                paragraph2_2_2 = new Paragraph("", RegularFont);
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_2_2.FixedHeight = 15.0F;
                cell2_2_2.UseVariableBorders = true;
                cell2_2_2.BorderColorTop = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorLeft = new BaseColor(255, 255, 255);
                cell2_2_2.BorderColorRight = new BaseColor(23, 56, 13);
                cell2_2_2.BorderColorBottom = new BaseColor(23, 56, 133);
                cell2_2_2.Colspan = 2;
                table2_2_2.AddCell(cell2_2_2);

                //Blank line starts
                //blank record 
                paragraph2_2_2 = new Paragraph("");
                cell2_2_2 = new PdfPCell(paragraph2_2_2);
                cell2_2_2.Border = Rectangle.NO_BORDER;
                cell2_2_2.Colspan = 6;
                cell2_2_2.FixedHeight = 10.0F;
                table2_2_2.AddCell(cell2_2_2);
                //Blank line ends


                document.Add(table2_2_2);

                //--------------------------------                

                float[] widths6_2 = { 30, 50, 300, 10, 40, 40, 40, 430 };
                PdfPTable table6_2 = new PdfPTable(widths6_2);
                table6_2.WidthPercentage = 100.0F;
                table6_2.SpacingBefore = 05.0F;
                table6_2.SpacingAfter = 10.0F;
                Paragraph paragraph6_2;
                PdfPCell cell6_2;


                paragraph6_2 = new Paragraph("");
                cell6_2 = new PdfPCell(paragraph6_2);
                cell6_2.Border = Rectangle.NO_BORDER;
                cell6_2.Colspan = 1;
                table6_2.AddCell(cell6_2);


                paragraph6_2 = new Paragraph("Number of meters (In case of building application)", RegularFont);
                cell6_2 = new PdfPCell(paragraph6_2);
                cell6_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6_2.Border = Rectangle.NO_BORDER;
                cell6_2.Colspan = 3;
                table6_2.AddCell(cell6_2);

                paragraph6_2 = new Paragraph("", RegularFont);
                cell6_2 = new PdfPCell(paragraph6_2);
                cell6_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell6_2.Border = Rectangle.NO_BORDER;
                cell6_2.Colspan = 3;
                table6_2.AddCell(cell6_2);


                //paragraph5 = new Paragraph("");
                //cell5 = new PdfPCell(paragraph5);
                //cell5.Border = Rectangle.NO_BORDER;
                //cell5.Colspan = 2;
                //table5.AddCell(cell5);


                //float[] nwidths5_1 = { 20, 80, 20, 80, 20, 80 };
                float[] nwidths6_3 = { 20, 80, 30, 20, 80, 30 };
                PdfPTable nTable6_3 = new PdfPTable(nwidths6_3);
                //Paragraph nParagraph6_3;
                PdfPCell nCell6_3;


                //Add checkbox
                //String[] providedBy = { "AEML", "TPC", "Self" };
                String[] providedBylcd = { "Single Phase", "Three Phase" };

                RadioCheckField checkboxpblcd;
                PdfFormField fieldpblcd;
                Rectangle rectpblcd = new Rectangle(10, 10, 15, 15);

                PdfFormField radiogrouppblcd = PdfFormField.CreateRadioButton(writer, true);
                radiogrouppblcd.FieldName = "LoadContractDemand";


                for (int i = 0; i < providedBylcd.Length; i++)
                {
                    checkboxpblcd = new RadioCheckField(writer, rectpblcd, providedBylcd[i], "Yes");
                    checkboxpblcd.BackgroundColor = BaseColor.WHITE;
                    checkboxpblcd.CheckType = RadioCheckField.TYPE_CHECK;
                    checkboxpblcd.BorderWidth = 0.25F;
                    //checkboxpb.setBorderWidth(BaseField.BORDER_WIDTH_THIN);
                    checkboxpblcd.BorderColor = new BaseColor(247, 170, 165);
                    checkboxpblcd.Options = BaseField.READ_ONLY;

                    if (i == 0 && oZCnt_1ph != 0)
                        checkboxpblcd.Checked = true;

                    if (i == 1 && oZCnt_3ph_Tot != 0)
                        checkboxpblcd.Checked = true;

                    fieldpblcd = checkboxpblcd.RadioField;
                    fieldpblcd.SetFieldFlags(PdfFormField.FF_READ_ONLY);

                    fieldpblcd = checkboxpblcd.RadioField;
                    fieldpblcd.SetWidget(rectAF, PdfAnnotation.HIGHLIGHT_INVERT);

                    radiogrouppblcd.AddKid(fieldpblcd);

                    writer.AddAnnotation(radiogrouppblcd);
                    //writer.AddAnnotation(fieldpb);

                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, fieldpblcd);
                    cell.Border = Rectangle.NO_BORDER;

                    nTable6_3.AddCell(cell);

                    if (i == 0)
                    {
                        Paragraph paragraph = new Paragraph(providedBylcd[i] + ":- ", RegularFont);
                        cell = new PdfPCell(paragraph);
                        cell.Border = Rectangle.NO_BORDER;
                        nTable6_3.AddCell(cell);

                        paragraph = new Paragraph(" " + oZCnt_1ph, RegularFont);
                        cell = new PdfPCell(paragraph);
                        //cell.Border = Rectangle.NO_BORDER;
                        cell.UseVariableBorders = true;
                        cell.BorderColorTop = new BaseColor(255, 255, 255);
                        cell.BorderColorLeft = new BaseColor(255, 255, 255);
                        cell.BorderColorRight = new BaseColor(255, 255, 255);
                        cell.BorderColorBottom = new BaseColor(23, 56, 133);
                        nTable6_3.AddCell(cell);
                    }
                    else
                    {
                        Paragraph paragraph = new Paragraph(providedBylcd[i] + " :- ", RegularFont);
                        cell = new PdfPCell(paragraph);
                        cell.Border = Rectangle.NO_BORDER;
                        nTable6_3.AddCell(cell);


                        paragraph = new Paragraph(" " + oZCnt_3ph_Tot, RegularFont);
                        cell = new PdfPCell(paragraph);
                        //cell.Border = Rectangle.NO_BORDER;
                        cell.UseVariableBorders = true;
                        cell.BorderColorTop = new BaseColor(255, 255, 255);
                        cell.BorderColorLeft = new BaseColor(255, 255, 255);
                        cell.BorderColorRight = new BaseColor(255, 255, 255);
                        cell.BorderColorBottom = new BaseColor(23, 56, 133);
                        nTable6_3.AddCell(cell);
                    }


                }

                nCell6_3 = new PdfPCell(nTable6_3);
                nCell6_3.Colspan = 2;
                nCell6_3.Border = Rectangle.NO_BORDER;
                table6_2.AddCell(nCell6_3);
                //---------                


                document.Add(table6_2);

                //6 Ends



                //7 Starts...
                float[] widths3 = { 30, 200, 450, 50, 400 };
                PdfPTable table3 = new PdfPTable(widths3);
                table3.WidthPercentage = 100.0F;
                Paragraph paragraph3;
                PdfPCell cell3;

                paragraph3 = new Paragraph("7.", RegularFontWhite);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.Border = Rectangle.NO_BORDER;
                cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.Colspan = 1;
                table3.AddCell(cell3);

                paragraph3 = new Paragraph("The wiring carried out/certified by the following Licensed Electrical Contractor ", RegularFontWhite);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.Border = Rectangle.NO_BORDER;
                cell3.Colspan = 3;
                table3.AddCell(cell3);

                paragraph3 = new Paragraph(" ", RegularFontWhite);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.Border = Rectangle.NO_BORDER;
                cell3.Colspan = 1;
                table3.AddCell(cell3);

                //Blank Line start
                paragraph3 = new Paragraph(" ", RegularFontWhite);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.Border = Rectangle.NO_BORDER;
                cell3.Colspan = 5;
                table3.AddCell(cell3);
                //Blank Line End

                paragraph3 = new Paragraph(" ", RegularFontWhite);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.Border = Rectangle.NO_BORDER;
                //cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.Colspan = 1;
                table3.AddCell(cell3);

                paragraph3 = new Paragraph("Name", RegularFont);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.UseVariableBorders = true;
                cell3.BorderColorTop = new BaseColor(23, 56, 133);
                cell3.BorderColorLeft = new BaseColor(23, 56, 133);
                cell3.BorderColorRight = new BaseColor(255, 255, 255);
                cell3.BorderColorBottom = new BaseColor(255, 255, 255);
                cell3.Colspan = 1;
                table3.AddCell(cell3);

                //if (dslec.Tables.Count > 0 && dslec.Tables[1].Rows.Count != 0 && dslec.Tables[1].Rows[0]["ZZ_LEC"].ToString().Trim() != "")
                //{
                //    paragraph3 = new Paragraph(dslec.Tables[1].Rows[0]["ZZ_LEC"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                paragraph3 = new Paragraph(" ", RegularFont);
                //}

                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.UseVariableBorders = true;
                cell3.BorderColorTop = new BaseColor(23, 56, 133);
                cell3.BorderColorLeft = new BaseColor(255, 255, 255);
                cell3.BorderColorRight = new BaseColor(23, 56, 133);
                cell3.BorderColorBottom = new BaseColor(23, 56, 133);
                cell3.Colspan = 3;
                table3.AddCell(cell3);


                paragraph3 = new Paragraph(" ", RegularFontWhite);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.Border = Rectangle.NO_BORDER;
                //cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.Colspan = 1;
                table3.AddCell(cell3);

                paragraph3 = new Paragraph("Address", RegularFont);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.UseVariableBorders = true;
                cell3.BorderColorTop = new BaseColor(255, 255, 255);
                cell3.BorderColorLeft = new BaseColor(23, 56, 133);
                cell3.BorderColorRight = new BaseColor(255, 255, 255);
                cell3.BorderColorBottom = new BaseColor(255, 255, 255);
                //cell3.BackgroundColor = new BaseColor(171, 49, 117);

                cell3.Colspan = 1;
                table3.AddCell(cell3);

                String lecAddress = string.Empty, lecAddress2 = string.Empty;
                //String lecAddress = printFormat(objTCBean.getLEC_ADD1(), "STRING", "RCAT")+","+printFormat(objTCBean.getLEC_ADD2(), "STRING", "CONCAT")+","+printFormat(objTCBean.getLEC_ADD3(), "STRING", "");
                //if (dslec.Tables.Count > 0 && dslec.Tables[1].Rows.Count != 0)
                //{
                //    lecAddress = dslec.Tables[1].Rows[0]["ZADD_LINE_1"].ToString().Trim() + "," + dslec.Tables[1].Rows[0]["ZADD_LINE_2"].ToString().Trim() + " " + dslec.Tables[1].Rows[0]["ZADD_LINE_3"].ToString().Trim();

                //    //lecAddress2 = dslec.Tables[1].Rows[0]["ZADD_LINE_3"].ToString().Trim();
                //}

                lecAddress = " ";
                lecAddress2 = " ";

                paragraph3 = new Paragraph(lecAddress, RegularFont);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.UseVariableBorders = true;
                //cell3.BorderColorTop = new BaseColor(255, 255, 255);
                cell3.BorderColorLeft = new BaseColor(255, 255, 255);
                cell3.BorderColorRight = new BaseColor(23, 56, 133);
                cell3.BorderColorBottom = new BaseColor(23, 56, 133);
                cell3.Colspan = 3;
                table3.AddCell(cell3);
                //-------

                paragraph3 = new Paragraph("", RegularFont);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.Border = Rectangle.NO_BORDER;
                //cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.FixedHeight = 15.0F;
                cell3.Colspan = 1;
                table3.AddCell(cell3);

                paragraph3 = new Paragraph("Telephone Number", RegularFont);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.UseVariableBorders = true;
                cell3.BorderColorTop = new BaseColor(255, 255, 255);
                cell3.BorderColorLeft = new BaseColor(23, 56, 133);
                cell3.BorderColorRight = new BaseColor(255, 255, 255);
                cell3.BorderColorBottom = new BaseColor(255, 255, 255);

                cell3.FixedHeight = 15.0F;
                cell3.Colspan = 1;
                table3.AddCell(cell3);


                //if (dslec.Tables.Count > 0 && dslec.Tables[1].Rows.Count != 0 && dslec.Tables[1].Rows[0]["ZLANDLINE_NO"].ToString().Trim() != "")
                //{
                //    paragraph3 = new Paragraph(dslec.Tables[1].Rows[0]["ZLANDLINE_NO"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    paragraph3 = new Paragraph("", RegularFont);
                //}

                paragraph3 = new Paragraph(" ", RegularFont);

                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.UseVariableBorders = true;
                //cell3.BorderColorTop = new BaseColor(255, 255, 255);
                cell3.BorderColorLeft = new BaseColor(255, 255, 255);
                cell3.BorderColorRight = new BaseColor(23, 56, 133);
                cell3.BorderColorBottom = new BaseColor(23, 56, 133);
                cell3.FixedHeight = 15.0F;
                cell3.Colspan = 3;
                table3.AddCell(cell3);


                paragraph3 = new Paragraph(" ", RegularFontWhite);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.Border = Rectangle.NO_BORDER;
                //cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.Colspan = 1;
                cell3.FixedHeight = 15.0F;
                table3.AddCell(cell3);

                paragraph3 = new Paragraph("E-mail", RegularFont);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.UseVariableBorders = true;
                cell3.BorderColorTop = new BaseColor(255, 255, 255);
                cell3.BorderColorLeft = new BaseColor(23, 56, 133);
                cell3.BorderColorRight = new BaseColor(255, 255, 255);
                cell3.BorderColorBottom = new BaseColor(255, 255, 255);

                cell3.FixedHeight = 15.0F;
                cell3.Colspan = 1;
                table3.AddCell(cell3);

                //if (dslec.Tables.Count > 0 && dslec.Tables[1].Rows.Count != 0 && dslec.Tables[1].Rows[0]["ZEMAIL"].ToString().Trim() != "")
                //{
                //    paragraph3 = new Paragraph(dslec.Tables[1].Rows[0]["ZEMAIL"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    paragraph3 = new Paragraph("", RegularFont);
                //}
                paragraph3 = new Paragraph(" ", RegularFont);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.UseVariableBorders = true;
                cell3.BorderColorTop = new BaseColor(255, 255, 255);
                cell3.BorderColorLeft = new BaseColor(255, 255, 255);
                cell3.BorderColorRight = new BaseColor(23, 56, 133);
                cell3.BorderColorBottom = new BaseColor(23, 56, 133);
                cell3.Colspan = 3;
                cell3.FixedHeight = 15.0F;
                table3.AddCell(cell3);


                paragraph3 = new Paragraph(" ", RegularFontWhite);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.Border = Rectangle.NO_BORDER;
                //cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.Colspan = 1;
                table3.AddCell(cell3);

                paragraph3 = new Paragraph("Licence No", RegularFont);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3.BackgroundColor = new BaseColor(171, 49, 117);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.UseVariableBorders = true;
                cell3.BorderColorTop = new BaseColor(255, 255, 255);
                cell3.BorderColorLeft = new BaseColor(23, 56, 133);
                cell3.BorderColorRight = new BaseColor(255, 255, 255);
                cell3.BorderColorBottom = new BaseColor(23, 56, 133);
                cell3.Colspan = 1;
                table3.AddCell(cell3);

                //if (dslec.Tables.Count > 0 && dslec.Tables[1].Rows.Count != 0 && dslec.Tables[1].Rows[0]["ZZ_LICNO"].ToString().Trim() != "")
                //{
                //    paragraph3 = new Paragraph(dslec.Tables[1].Rows[0]["ZZ_LICNO"].ToString().Trim(), RegularFont);
                //}
                //else
                //{
                //    paragraph3 = new Paragraph("", RegularFont);
                //}
                paragraph3 = new Paragraph(" ", RegularFont);
                cell3 = new PdfPCell(paragraph3);
                cell3 = new PdfPCell(paragraph3);
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.UseVariableBorders = true;
                cell3.BorderColorTop = new BaseColor(255, 255, 255);
                cell3.BorderColorLeft = new BaseColor(255, 255, 255);
                cell3.BorderColorRight = new BaseColor(23, 56, 133);
                cell3.BorderColorBottom = new BaseColor(23, 56, 133);
                cell3.Colspan = 3;
                table3.AddCell(cell3);

                //table3.AddCell(cell3);
                table3.SpacingAfter = 5.0F;
                document.Add(table3);
                //7 ends


                //8 starts
                float[] widths5 = { 30, 50, 200, 150, 80, 100, 100, 360 };
                PdfPTable table5 = new PdfPTable(widths5);
                table5.WidthPercentage = 100.0F;
                table5.SpacingBefore = 10.0F;
                table5.SpacingAfter = 10.0F;
                Paragraph paragraph5;
                PdfPCell cell5;

                paragraph5 = new Paragraph("8.", RegularFontWhite);
                cell5 = new PdfPCell(paragraph5);
                cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5.Border = Rectangle.NO_BORDER;
                cell5.BackgroundColor = new BaseColor(171, 49, 117);
                cell5.Colspan = 1;
                table5.AddCell(cell5);

                paragraph5 = new Paragraph("Changeover of Supply", RegularFontWhite);
                cell5 = new PdfPCell(paragraph5);
                cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5.Border = Rectangle.NO_BORDER;
                cell5.BackgroundColor = new BaseColor(171, 49, 117);
                cell5.Colspan = 2;
                table5.AddCell(cell5);

                //paragraph5 = new Paragraph("(In te hevent of ChangeOver Supply)", smallFontWhite);
                paragraph5 = new Paragraph(" ", smallFontWhite);
                cell5 = new PdfPCell(paragraph5);
                cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5.Border = Rectangle.NO_BORDER;
                cell5.Colspan = 5;
                table5.AddCell(cell5);


                //---------

                paragraph5 = new Paragraph("");
                cell5 = new PdfPCell(paragraph5);
                cell5.Border = Rectangle.NO_BORDER;
                cell5.Colspan = 1;
                table5.AddCell(cell5);


                paragraph5 = new Paragraph("Meter Number", RegularFont);
                cell5 = new PdfPCell(paragraph5);
                cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5.Border = Rectangle.NO_BORDER;
                cell5.Colspan = 2;
                table5.AddCell(cell5);


                paragraph5 = new Paragraph("", RegularFont);
                cell5 = new PdfPCell(paragraph5);
                cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5.FixedHeight = 10.0F;
                cell5.UseVariableBorders = true;
                cell5.BorderColorTop = new BaseColor(255, 255, 255);
                cell5.BorderColorLeft = new BaseColor(255, 255, 255);
                cell5.BorderColorRight = new BaseColor(255, 255, 255);
                cell5.BorderColorBottom = new BaseColor(23, 56, 133);
                cell5.Colspan = 1;
                table5.AddCell(cell5);


                paragraph5 = new Paragraph("The Meter will be provided by", RegularFont);
                cell5 = new PdfPCell(paragraph5);
                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5.Border = Rectangle.NO_BORDER;
                cell5.Colspan = 3;
                table5.AddCell(cell5);


                //paragraph5 = new Paragraph("");
                //cell5 = new PdfPCell(paragraph5);
                //cell5.Border = Rectangle.NO_BORDER;
                //cell5.Colspan = 2;
                //table5.AddCell(cell5);


                //float[] nwidths5_1 = { 20, 80, 20, 80, 20, 80 };
                float[] nwidths5_1 = { 20, 80, 20, 80, };
                PdfPTable nTable5_1 = new PdfPTable(nwidths5_1);
                //Paragraph nParagraph5_1;
                //PdfPCell nCell5_1;


                //Add checkbox
                //String[] providedBy = { "AEML", "TPC", "Self" };
                String[] providedBy = { "AEML", "Self" };

                RadioCheckField checkboxpb;
                PdfFormField fieldpb;
                Rectangle rectpb = new Rectangle(10, 10, 15, 15);

                PdfFormField radiogrouppb = PdfFormField.CreateRadioButton(writer, true);
                radiogrouppb.FieldName = "ChangeOverSupply";


                for (int i = 0; i < providedBy.Length; i++)
                {
                    checkboxpb = new RadioCheckField(writer, rectpb, providedBy[i], "Yes");
                    checkboxpb.BackgroundColor = BaseColor.WHITE;
                    checkboxpb.CheckType = RadioCheckField.TYPE_CIRCLE;
                    checkboxpb.BorderWidth = 0.25F;
                    //checkboxpb.setBorderWidth(BaseField.BORDER_WIDTH_THIN);
                    checkboxpb.BorderColor = new BaseColor(247, 170, 165);
                    checkboxpb.Options = BaseField.READ_ONLY;

                    fieldpb = checkboxpb.RadioField;
                    fieldpb.SetFieldFlags(PdfFormField.FF_READ_ONLY);

                    fieldpb = checkboxpb.RadioField;
                    fieldpb.SetWidget(rectAF, PdfAnnotation.HIGHLIGHT_INVERT);

                    radiogrouppb.AddKid(fieldpb);

                    writer.AddAnnotation(radiogrouppb);
                    //writer.AddAnnotation(fieldpb);

                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, fieldpb);
                    cell.Border = Rectangle.NO_BORDER;

                    nTable5_1.AddCell(cell);

                    Paragraph paragraph = new Paragraph(providedBy[i], RegularFont);
                    cell = new PdfPCell(paragraph);
                    cell.Border = Rectangle.NO_BORDER;
                    nTable5_1.AddCell(cell);
                }

                cell5 = new PdfPCell(nTable5_1);
                cell5.Colspan = 2;
                cell5.Border = Rectangle.NO_BORDER;
                table5.AddCell(cell5);
                //---------                




                document.Add(table5);
                //8 ends


                //9 strats

                float[] widths10 = { 20, 350, 250 };
                PdfPTable table10 = new PdfPTable(widths10);
                table10.WidthPercentage = 100.0F;
                Paragraph paragraph10;
                PdfPCell cell10;

                paragraph10 = new Paragraph("9.", RegularFontWhite);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                cell10.BackgroundColor = new BaseColor(171, 49, 117);
                cell10.Colspan = 1;
                table10.AddCell(cell10);

                paragraph10 = new Paragraph("Documents Attached", RegularFontWhite);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                cell10.BackgroundColor = new BaseColor(171, 49, 117);
                cell10.Colspan = 1;
                table10.AddCell(cell10);

                paragraph10 = new Paragraph(" ", RegularFont);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                //cell22.BackgroundColor = new BaseColor(171, 49, 117);
                cell10.Colspan = 1;
                table10.AddCell(cell10);
                //-----------------

                paragraph10 = new Paragraph(" ", RegularFontWhite);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                cell10.Colspan = 1;
                table10.AddCell(cell10);

                paragraph10 = new Paragraph("A. Identity Proof submitted along with this application form :- ", RegularFont);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                cell10.Colspan = 1;
                table10.AddCell(cell10);

                //paragraph10 = new Paragraph(oID_Doc, RegularFont);
                paragraph10 = new Paragraph("", RegularFont);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell10.Border = Rectangle.NO_BORDER;
                cell10.UseVariableBorders = true;
                cell10.BorderColorTop = new BaseColor(255, 255, 255);
                cell10.BorderColorLeft = new BaseColor(255, 255, 255);
                cell10.BorderColorRight = new BaseColor(255, 255, 255);
                cell10.BorderColorBottom = new BaseColor(23, 56, 133);
                cell10.Colspan = 1;
                table10.AddCell(cell10);

                //-----------------

                paragraph10 = new Paragraph(" ", RegularFontWhite);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                cell10.Colspan = 1;
                table10.AddCell(cell10);

                paragraph10 = new Paragraph("> if applicant is a person", RegularFont);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                cell10.Colspan = 2;
                table10.AddCell(cell10);
                //-----------------------------


                paragraph10 = new Paragraph(" ", RegularFontWhite);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                cell10.Colspan = 1;
                table10.AddCell(cell10);

                paragraph10 = new Paragraph(">If applicant is an organization (mention Applicable)-Certificate of incorporation/registration issued by the Registrar", RegularFont);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                cell10.Colspan = 2;
                table10.AddCell(cell10);
                //-----------------------------

                paragraph10 = new Paragraph(" ", RegularFontWhite);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                cell10.Colspan = 1;
                table10.AddCell(cell10);

                paragraph10 = new Paragraph("B. Proof of ownership or occupancy of premises for which electricity connection is required_____________________ ", RegularFont);
                cell10 = new PdfPCell(paragraph10);
                cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell10.Border = Rectangle.NO_BORDER;
                cell10.Colspan = 2;
                table10.AddCell(cell10);
                //------------------

                //paragraph10 = new Paragraph(" ", RegularFontWhite);
                //cell10 = new PdfPCell(paragraph10);
                //cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell10.Border = Rectangle.NO_BORDER;
                //cell10.Colspan = 1;
                //table10.AddCell(cell10);



                ////paragraph10 = new Paragraph(oOD_Doc, RegularFont);
                //paragraph10 = new Paragraph("", RegularFont);
                //cell10 = new PdfPCell(paragraph10);
                //cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                ////cell10.Border = Rectangle.NO_BORDER;
                //cell10.UseVariableBorders = true;
                //cell10.BorderColorTop = new BaseColor(255, 255, 255);
                //cell10.BorderColorLeft = new BaseColor(255, 255, 255);
                //cell10.BorderColorRight = new BaseColor(255, 255, 255);
                //cell10.BorderColorBottom = new BaseColor(23, 56, 133);
                //cell10.Colspan = 2;
                //table10.AddCell(cell10);
                //-----------------------------

                table10.SpacingAfter = 5.0F;
                document.Add(table10);
                //10 ends


                //7 starts               

                float[] widths6 = { 20, 500, 529 };
                PdfPTable table6 = new PdfPTable(widths6);
                table6.WidthPercentage = 100.0F;
                Paragraph paragraph6;
                PdfPCell cell6;


                paragraph6 = new Paragraph("", boldFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                cell6.BackgroundColor = new BaseColor(171, 49, 117);
                cell6.Colspan = 1;
                table6.AddCell(cell6);


                paragraph6 = new Paragraph("THE APPLICANT HEREBY AGREES AND UNDERTAKES", RegularFontWhite);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                cell6.BackgroundColor = new BaseColor(171, 49, 117);
                cell6.Colspan = 1;
                table6.AddCell(cell6);

                paragraph6 = new Paragraph("", boldFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell6.BackgroundColor = new BaseColor(171, 49, 117);
                cell6.Colspan = 1;
                table6.AddCell(cell6);
                //-----------------------

                paragraph6 = new Paragraph(" ", RegularFontWhite);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3pg.Padding = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                cell6.Border = Rectangle.NO_BORDER;
                table6.AddCell(cell6);

                paragraph6 = new Paragraph("1. That the Applicant/Consumer is not a minor.\n.......................................................................................................", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);


                paragraph6 = new Paragraph("issued by Licensing Authority, Government of Maharashtra and the test certificate is available with the Applicant and can be produced as may be required at any later stage.\n.......................................................................................................", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);
                //-----------   


                paragraph6 = new Paragraph(" ", RegularFontWhite);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3pg.Padding = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                cell6.Border = Rectangle.NO_BORDER;
                table6.AddCell(cell6);
                //2
                paragraph6 = new Paragraph("2. That in case the applicant is not the sole owner of the premises, no objection certificate has been obtained from the co-owner.\n.......................................................................................................", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);

                //10
                paragraph6 = new Paragraph("8. That the Premises/building/structure has been constructed as per prevalent building Bye- Laws and the total height of the Premises/building/structure-\n (i) does not exceed 15 (fifteen) metres on the date of seeking electricity connection, or\n.......................................................................................................", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);
                //-----------                   


                paragraph6 = new Paragraph(" ", RegularFontWhite);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3pg.Padding = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                cell6.Border = Rectangle.NO_BORDER;
                table6.AddCell(cell6);
                //2
                paragraph6 = new Paragraph("3. To abide by the provisions of the Electricity Act,2003, all applicable laws, conditions of Supply/Tariff Orders and any other Rules or Regulations as may be notified by the Commission/Authority, as applicable from time to time.\n.......................................................................................................", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);

                //10
                paragraph6 = new Paragraph("(ii) is more than 15 (fifteen) metres and has obtained valid Clearance certificate from appropriate government authority, which is available with the Applicant.\n.......................................................................................................", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);
                //----------- 

                paragraph6 = new Paragraph(" ", RegularFontWhite);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3pg.Padding = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                cell6.Border = Rectangle.NO_BORDER;
                table6.AddCell(cell6);
                //2
                paragraph6 = new Paragraph("4. That AEML shall be at liberty to adjust the electricity consumption charges due/outstanding along with any other charges against the Consumer Security Deposit paid by the Applicant upon permanent disconnection of supply.\n.......................................................................................................", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);

                //10
                paragraph6 = new Paragraph("9. That there is a provision of lift in the Premises and the Applicant has obtained the valid lift fitness certificate from the Electrical Inspector for the lift in the said Premises and the same is available with the Applicant and can be produced as may be required at any later stage.\n.......................................................................................................", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);
                //----------- 

                paragraph6 = new Paragraph(" ", RegularFontWhite);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3pg.Padding = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                cell6.Border = Rectangle.NO_BORDER;
                table6.AddCell(cell6);
                //2
                paragraph6 = new Paragraph("5. To indemnify AEML against all proceedings, claims, demands, costs, damages, expenses that AEML may incur by reason of a fresh electricity connection given to the Applicant.\n.......................................................................................................", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);

                //10
                paragraph6 = new Paragraph("10. That the Applicant would let the Distribution Licensee disconnect the electric connection under reference, in the event of any default, non-compliance of statutory provisions and in the event of a legally binding directive by Statutory Authority(ies) to effect such an order. This shall be without prejudice to any other rights of the licensee including that of getting its payment as on the date of disconnection.\n", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 3;
                table6.AddCell(cell6);
                //----------- 

                paragraph6 = new Paragraph(" ", RegularFontWhite);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3pg.Padding = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                cell6.Border = Rectangle.NO_BORDER;
                table6.AddCell(cell6);
                //2
                paragraph6 = new Paragraph("6. That to the best of applicant’s knowledge, all electrical works done within the premises are as per Central Electricity Authority (Measures relating to Safety and Electricity Supply) Regulations, 2010 as amended from time to time.\n.......................................................................................................", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);

                paragraph6 = new Paragraph(" ", RegularFontWhite);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3pg.Padding = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                cell6.Border = Rectangle.NO_BORDER;
                table6.AddCell(cell6);

                paragraph6 = new Paragraph(" ", RegularFontWhite);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3pg.Padding = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                cell6.Border = Rectangle.NO_BORDER;
                table6.AddCell(cell6);

                //10
                paragraph6 = new Paragraph("7. That the internal wiring at the premises has been tested by a Licensed Electrical Contractor having valid license\n", RegularFont);
                cell6 = new PdfPCell(paragraph6);
                cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                cell6.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell6.PaddingRight = 5;
                cell6.Colspan = 1;
                cell6.Rowspan = 2;
                table6.AddCell(cell6);
                //----------- 

                table6.SpacingBefore = 10.0F;
                table6.SpacingAfter = 05.0F;
                document.Add(table6);


                float[] lastwidths = { 30, 250, 100, 400, 120, 150 };
                PdfPTable lasttable = new PdfPTable(lastwidths);
                lasttable.WidthPercentage = 100.0F;
                lasttable.SpacingBefore = 50;
                Paragraph lastParagraph;
                PdfPCell lastCell;

                lastParagraph = new Paragraph("Name & Applicant's Signature", RegularFont);
                lastCell = new PdfPCell(lastParagraph);
                lastCell.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell.Border = Rectangle.NO_BORDER;
                lastCell.Colspan = 2;
                lasttable.AddCell(lastCell);


                lastParagraph = new Paragraph("", RegularFont);
                lastCell = new PdfPCell(lastParagraph);
                lastCell.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell.FixedHeight = 10.0F;
                lastCell.UseVariableBorders = true;
                lastCell.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell.BorderColorBottom = new BaseColor(23, 56, 133);
                lastCell.Colspan = 1;
                lasttable.AddCell(lastCell);


                lastParagraph = new Paragraph("");
                lastCell = new PdfPCell(lastParagraph);
                lastCell.Border = Rectangle.NO_BORDER;
                lastCell.Colspan = 1;
                lasttable.AddCell(lastCell);



                lastParagraph = new Paragraph("Date", RegularFont);
                lastCell = new PdfPCell(lastParagraph);
                lastCell.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell.Border = Rectangle.NO_BORDER;
                lastCell.Colspan = 1;
                lasttable.AddCell(lastCell);


                lastParagraph = new Paragraph(con_data.ApplicationStatusUpdateDate.Value.Date.ToString("dd/MMM/yyyy").Trim(), RegularFont);
                //lastParagraph = new Paragraph("22/01/2020", RegularFont);
                lastCell = new PdfPCell(lastParagraph);
                lastCell.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell.FixedHeight = 10.0F;
                lastCell.UseVariableBorders = true;
                lastCell.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell.BorderColorBottom = new BaseColor(23, 56, 133);
                lastCell.Colspan = 1;
                lasttable.AddCell(lastCell);



                document.Add(lasttable);
                //6 ends

                //Applicant's details ends.

                //Page 3 Starts...

                document.NewPage();



                string pt_11 = "\n11. That if the premises applied for is falling under CRZ,Forest area or any other area where any Court of Law/Statutory Authority has prohibited release of power supply, the Applicant shall submit a No Objection Certificate (NoC) to seek electricity connection, from such Authority.";
                string pt_12 = "\n12. That the applicant is solely and exclusively responsible for the genuineness and correctness of the document/s submitted by him in support of the application and the other documents as indicated at various points herein. In case any such documents are found to be fraudulent and/or fake and/or forged and/or incorrect and/or invalid/incomplete, then the applicant/consumer shall be solely and exclusively responsible for the criminal proceedings and/or any court proceedings initiated and/or any dispute which arises any time in future and AEML and/or its successors, assignees and/or any employee thereof, shall not be responsible for the same in any manner whatsoever. In such a case, AEML reserves the right to disconnect the connection granted and in case of Change of Name, re-transfer connection in the name of previous registered consumer under this declaration cum undertaking, without any notice.";
                string pt_13 = "\n13. That all details mentioned in the application form are correct and all applicable customer-end obligations (E.g. Documentary and Payment compliance, Site readiness,Way-Leave permits) are complete and complied in all respects, at the time of applying for new electricity connection, through online/offline mode. In case any of these requirements are not ready, or in case of an objection,AEML reserves the right to cancel the application.";
                string pt_14 = "\n14. That the Applicant will provide a copy of any of above applicable documents to the Distribution Licensee pursuant to the request from any external Government agency, judicial forum or any other authority empowered under statute seeking such information.";
                string pt_15 = "\n15. That Applicant’s industry/trade has not been declared to be releasing obnoxious hazardous/pollutant by any Government agency and that no orders of any court or judicial authority would be breached by running of Applicant’s industry/trade or granting any electricity connection to the same. The Applicant shall indemnify and keep indemnified AEML against any loss of any nature on this account.";
                string pt_16 = "\n16. The Applicant further agrees to indemnify and hold harmless, AEML and/or its successors and/or its employees, in case of any injury or incident on account of any fault in electrical works in the Premises and from point of supply to the Premises. AEML shall not be held responsible/liable for any mishap or incident occurring at the Premises or from point of supply to the Premises on account of any faulty/defective/inferior quality wiring/installation, right from the time of energizing the new electricity connection.On occurrence of such incident, AEML may disconnect the Applicant’s supply, if continuance of supply to such installation is threat to the life or property. Such disconnected supply shall be reconnected only after rectification of faulty installation and submission of test report from Licensed Electrical Contractor.";
                string pt_17 = "\n17. The applicant further agrees to bear the expenses towards repairing/ replacement of AEML’s assets or works, in case there is sufficient proof that the damage/fault has been occurred due to faulty electrical work/ wiring in his premises.";
                string pt_18 = "\n18. To pay the electricity consumption bills and all other charges, at the rates set out in the licensee’s Tariff Schedule and the MERC approved Schedule of Charges,";
                string pt_18_a = "\n  as may be in force from time to time, regularly, as and when the same becomes due for payment.\n";
                string pt_19 = "\n19. To deposit the additional security deposit (as applicable) from time to time, based on the prevailing Orders/rules, directions and Regulations of the Commission.";
                string pt_20 = "\n20. For GSTIN :In compliance of Rule 49 of CGST Rules,2017, I/We shall forthwith submit with AEML my/our Goods and Service Tax Identification Number or Unique Identity No. a Bill of supply (monthly electricity bill ) shall print Goods and Services Tax Identification Number or Unique Identity Number, if registered, of the consumer\n\n For PAN :\n i.)As per Rule 114B of Income Tax Rules 1962 every person is required to quote his PAN in all documents pertaining to the transaction of sale or purchase of any nature for a value exceeding Rs. 2 lacs per transaction for the purpose of clause (c) of sub-section (5) of Section 139A";
                string pt_20_a = "\n  ii.)As per section - 206AA of Income-tax Act, 1961, TDS (if applicable)@20% on Security Deposit Interest will be deducted instead of 10% for non-availability of valid PAN";
                string pt_21 = "\n21. Applicable only for applied Industrial/Commercial oad between 100 and 150 kW: The Applicant authorizes AEML to debit the approved connection charges and applicable security deposit in first electricity bill (as and when enabled by AEML), and undertake to pay the same within the due date. The Applicant / Consumer may opt to pay such Security Deposit by way of a Bank Guarantee.";
                string pt_22 = "\n22. That the Applicant shall not hold AEML liable for delay in providing connection or meeting any Standards of Performance subsequent to grant of connection in case AEML is prevented from doing so on account of any force majeure events/exemptions as specified in the Standards of Performance Regulations, as amended from time to time.";
                string pt_23 = "\n23. That there are no arrears towards energy or related charges outstanding in his/her name and /or on account of the said premises. In the event of aforesaid declaration/undertaking given by the applicant/consumer is found false in view of any pre-existing liability, the applicant/consumer shall willfully, without demur pay such amount on demand. The Consumer/applicant shall be duty bound to extend full co-operation to AEML while determining and/or identifying the arrears, if any, of the premises or account no, within a period of 60 days. The applicant/consumer shall make payment of the said arrears immediately on demand.";
                string pt_24 = "\n24. The Applicant/Consumer shall identify suitable space agreeable to AEML to install electrical meter and related devices and ensure un-hindered passage to personnel of AEML to access the meter cabin.";
                string pt_25 = "\n25. That the following terms and conditions are complied with respect to space for meter installation:\n\na. Metering installation is protected from excessive dust and moisture, exposure to direct sunlight, rain and water seepage and vermin. The Applicant/Consumer shall ensure that temperature within the meter room shall not be more than 10°C, above ambient. It should not be in proximity of machineries, heating devices,equipment generating high vibration or magnetic fields and areas prone to fire and toxic hazards.";
                string Dotted_Ln = "\n.......................................................................................................";

                float[] widths3_2 = { 20, 500, 529 };
                PdfPTable table3_2 = new PdfPTable(widths3_2);
                table3_2.WidthPercentage = 100.0F;
                Paragraph paragraph3_2;
                PdfPCell cell3_2;
                //----------------------------
                paragraph3_2 = new Paragraph(" ", RegularFontWhite);
                cell3_2 = new PdfPCell(paragraph3_2);
                cell3_2.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3pg.Padding = 5;
                cell3_2.Colspan = 1;
                cell3_2.Rowspan = 2;
                cell3_2.Border = Rectangle.NO_BORDER;
                table3_2.AddCell(cell3_2);

                //paragraph3_2 = new Paragraph("11. That if the premises applied for is falling under CRZ,Forest area or any other area where any Court of Law/Statutory Authority has prohibited release of power supply, the Applicant shall submit a No Objection Certificate (NoC) to seek electricity connection, from such Authority.\n.......................................................................................................", RegularFont);
                paragraph3_2 = new Paragraph(pt_11 + Dotted_Ln + pt_12 + Dotted_Ln + pt_13 + Dotted_Ln + pt_14 + Dotted_Ln + pt_15 + Dotted_Ln + pt_16 + Dotted_Ln + pt_17 + Dotted_Ln + pt_18, RegularFont);
                cell3_2 = new PdfPCell(paragraph3_2);
                cell3_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3_2.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell3_2.PaddingRight = 5;
                cell3_2.Colspan = 1;
                cell3_2.Rowspan = 2;
                table3_2.AddCell(cell3_2);


                //paragraph3_2 = new Paragraph("as may be in force from time to time, regularly, as and when the same becomes due for payment.\n\n 19. To deposit the additional security deposit (as applicable) from time to time, based on the prevailing Orders/rules, directions and Regulations of the Commission.\n.......................................................................................................", RegularFont);
                paragraph3_2 = new Paragraph(pt_18_a + pt_19 + Dotted_Ln + pt_20 + Dotted_Ln + pt_20_a + Dotted_Ln + pt_21 + Dotted_Ln + pt_22 + Dotted_Ln + pt_23 + Dotted_Ln + pt_24 + Dotted_Ln + pt_25, RegularFont);
                cell3_2 = new PdfPCell(paragraph3_2);
                cell3_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3_2.Border = Rectangle.NO_BORDER;
                //cell3pg.PaddingLeft = 5;
                cell3_2.PaddingRight = 5;
                cell3_2.Colspan = 1;
                cell3_2.Rowspan = 2;
                table3_2.AddCell(cell3_2);
                //-----------   




                document.Add(table3_2);
                //document.Add(table3pg);



                float[] lastwidths3_3 = { 30, 250, 100, 400, 120, 150 };
                PdfPTable lasttable3_3 = new PdfPTable(lastwidths3_3);
                lasttable3_3.WidthPercentage = 100.0F;
                lasttable3_3.SpacingBefore = 200;
                Paragraph lastParagraph3_3;
                PdfPCell lastCell3_3;

                lastParagraph3_3 = new Paragraph("Name & Applicant's Signature", RegularFont);
                lastCell3_3 = new PdfPCell(lastParagraph3_3);
                lastCell3_3.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell3_3.Border = Rectangle.NO_BORDER;
                lastCell3_3.Colspan = 2;
                lasttable3_3.AddCell(lastCell3_3);


                lastParagraph3_3 = new Paragraph("", RegularFont);
                lastCell3_3 = new PdfPCell(lastParagraph3_3);
                lastCell3_3.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell3_3.FixedHeight = 10.0F;
                lastCell3_3.UseVariableBorders = true;
                lastCell3_3.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell3_3.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell3_3.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell3_3.BorderColorBottom = new BaseColor(23, 56, 133);
                lastCell3_3.Colspan = 1;
                lasttable3_3.AddCell(lastCell3_3);


                lastParagraph3_3 = new Paragraph("");
                lastCell3_3 = new PdfPCell(lastParagraph3_3);
                lastCell3_3.Border = Rectangle.NO_BORDER;
                lastCell3_3.Colspan = 1;
                lasttable3_3.AddCell(lastCell3_3);



                lastParagraph3_3 = new Paragraph("Date", RegularFont);
                lastCell3_3 = new PdfPCell(lastParagraph3_3);
                lastCell3_3.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell3_3.Border = Rectangle.NO_BORDER;
                lastCell3_3.Colspan = 1;
                lasttable3_3.AddCell(lastCell3_3);


                lastParagraph3_3 = new Paragraph(oDt, RegularFont);
                lastCell3_3 = new PdfPCell(lastParagraph3_3);
                lastCell3_3.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell3_3.FixedHeight = 10.0F;
                lastCell3_3.UseVariableBorders = true;
                lastCell3_3.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell3_3.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell3_3.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell3_3.BorderColorBottom = new BaseColor(23, 56, 133);
                lastCell3_3.Colspan = 1;
                lasttable3_3.AddCell(lastCell3_3);



                lastParagraph3_3 = new Paragraph(" ", RegularFont);
                lastCell3_3 = new PdfPCell(lastParagraph3_3);
                lastCell3_3.Border = Rectangle.NO_BORDER;
                lastCell3_3.Colspan = 6;
                lasttable3_3.AddCell(lastCell3_3);

                lasttable3_3.SpacingBefore = 80F;
                lasttable3_3.SpacingAfter = 20F;
                document.Add(lasttable3_3);


                //Page 3 ends


                //Page 4 starts

                document.NewPage();
                string pt_25_b = "\nb. Meters are not located at an elevated area or a depressed area that does not have access by means of a stairway of normal rise. The meters shall not be installed below/in the stair case or in common passage or lobby.";
                string pt_25_c = "\nc. Board wiring is carried out preferably on Fire retardant plywood, complying IS 5509. In case of nonavailability of FR plywood, marine plywood complying IS 710 can be used. The plywood shall be of 19 mm thickness and the same shall be provided by an applicant. Typically plywood is available in following sizes (7’x4’ , 7’x3’, 8’x3’,8’x4’ ).IS mark is printed on the center of either face of the ply for identification.";
                string pt_26 = "\n26. That the following terms and conditions shall be complied with, for applicant/consumer’s installation beyond the point of supply of AEML:\n";
                string pt_26_a = "\na. The Applicant/Consumer must install appropriately rate MCB with enclosure near the start of the electricity supply and shall not install DP Switch.";
                string pt_26_b = "\nb. The Applicant/Consumer, if having voltage exceeding 250 Volts, shall provide his own earthing system for installation.";
                string pt_26_c = "\nc. The Applicant/Consumer, if having sanctioned load above 2 KW, shall install Earth Leakage protective device";
                string pt_27 = "\n27. The applicant/consumer shall provide way leave in favour of AEML:\n";
                string pt_27_i = "\ni.) to lay its cables through his/owner’s land/ property";
                string pt_27_ii = "\nii.) to access AEML’s equipment and work in its premises in accordance with the applicable provisions of Supply Code or any other Regulations, as prevailing from time to time";
                string pt_27_iii = "\niii.) to provide permanent Right of Way to AEML personnel along with vehicles and cables and/or apparatus, without any further consent (for now and in future)";
                string pt_27_iv = "\niv.) for laying, strengthening and maintaining of electrical network and system";
                string pt_27_v = "\nv.) for laying outgoing feeders (LT/ HT) from their premises as the same arrangement is for making interconnection with outside network which shall further help in arranging alternate supply in case of break down on mains. In the event land under way leave permission is sold, leased, rented or otherwise disposed off, while entering to such a transaction the applicant/consumer shall ensure by necessary covenants that this terms and conditions will be abided by the other parties to the said transaction.";
                string pt_28 = "\n28. That the excavated trenches shall be reinstated by the Applicant/Consumer at his/her own cost when the cable is laid to provide supply to their premises. AEML shall be responsible to reinstate in the event the works is carried out to provide supply to any other customer.";
                string pt_29 = "\n29. That AEML shall have full rights to optimally utilize its assets (Substation/ pillars) in applicant/consumer’s premises and shall not object to laying outgoing feeders (LT/ HT) from their premises as the same arrangement is for making interconnection with outside network which shall further help in arranging alternate supply in case of break down on mains.";
                string pt_30 = "\n30. Additional Term and Conditions for Change-over applications:\n";
                string pt_30_a = "\na. The consumer/applicant shall clear all it's dues with existing distribution licensee and submit the proof.";
                string pt_30_b = "\nb. Changeover is permissible to existing consumer where name and purpose or classification of category for which the electricity has been provided by previous distribution licensee remains same.";
                string pt_30_c = "\nc. The meter reading for final billing shall be taken jointly and signed off by both distribution licensees in presence of the applicant/ it's representative.";
                string pt_30_d = "\nd. In the event the consumer/applicant is not present at the time of final meter reading, the readings taken jointly by the previous distribution licensee and AEML shall be final and binding upon the applicant / consumer. The Final meter reading shall be the opening meter reading for AEML.";


                float[] widths5_1 = { 210, 210 };

                PdfPTable table5_1 = new PdfPTable(widths5_1);
                table5_1.WidthPercentage = 100.0F;
                PdfPCell cell5_1;
                Paragraph para5_1;


                //para5_1 = new Paragraph("iii.) to provide permanent Right of Way to AEML personnel along with vehicles and cables and/or apparatus, without any further consent (for now and in future)\n\n.......................................................................................................", RegularFont);
                para5_1 = new Paragraph(pt_25_b + Dotted_Ln + pt_25_c + Dotted_Ln + pt_26 + pt_26_a + Dotted_Ln + pt_26_b + Dotted_Ln + pt_26_c + Dotted_Ln + pt_27 + pt_27_i + Dotted_Ln + pt_27_ii + Dotted_Ln + pt_27_iii + Dotted_Ln + pt_27_iv, RegularFont);
                cell5_1 = new PdfPCell(para5_1);
                cell5_1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5_1.Border = Rectangle.NO_BORDER;
                cell5_1.PaddingLeft = 5;
                cell5_1.PaddingRight = 5;
                cell5_1.Colspan = 1;
                cell5_1.Rowspan = 2;
                table5_1.AddCell(cell5_1);


                //para5_1 = new Paragraph("payments of last bill as served by the existing distribution licensee and other relevant documents as required under the Supply Code, 05.\n\n.......................................................................................................", RegularFont);
                para5_1 = new Paragraph(pt_27_v + Dotted_Ln + pt_28 + Dotted_Ln + pt_29 + Dotted_Ln + pt_30 + pt_30_a + Dotted_Ln + pt_30_b + Dotted_Ln + pt_30_c + Dotted_Ln + pt_30_d, RegularFont);
                cell5_1 = new PdfPCell(para5_1);
                cell5_1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5_1.Border = Rectangle.NO_BORDER;
                cell5_1.PaddingLeft = 5;
                cell5_1.PaddingRight = 5;
                cell5_1.Colspan = 1;
                cell5_1.Rowspan = 2;
                table5_1.AddCell(cell5_1);
                //---------------------------
                para5_1 = new Paragraph("", RegularFont);
                cell5_1 = new PdfPCell(para5_1);
                cell5_1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5_1.Border = Rectangle.NO_BORDER;
                cell5_1.Colspan = 1;
                table5_1.AddCell(cell5_1);

                para5_1 = new Paragraph("", RegularFont);
                cell5_1 = new PdfPCell(para5_1);
                cell5_1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5_1.Border = Rectangle.NO_BORDER;
                cell5_1.Colspan = 1;
                table5_1.AddCell(cell5_1);

                document.Add(table5_1);


                float[] widths5_2 = { 20, 800 };
                PdfPTable Table5_2 = new PdfPTable(widths5_2);
                Table5_2.WidthPercentage = 100.0F;
                Table5_2.SpacingBefore = 20.0F;
                Table5_2.SpacingAfter = 20.0F;
                Paragraph Paragraph5_2;
                PdfPCell Cell5_2;

                //Add checkbox
                //String[] premisesType = { "Owned", "Rented", "Lease", "Others(Specify)" };
                String[] premisesType5_2 = { "I/We declare and undertake that I/we have complied with all statutory requirements under all Statutes for the time being in force." };
                RadioCheckField checkbox5_2;
                PdfFormField field5_2;
                Rectangle rect5_2 = new Rectangle(10, 10, 15, 15);
                PdfFormField radiogroup5_2 = PdfFormField.CreateRadioButton(writer, true);
                radiogroup5_2.FieldName = "Page5Undertaking";


                for (int i = 0; i < premisesType5_2.Length; i++)
                {
                    checkbox5_2 = new RadioCheckField(writer, rect5_2, premisesType5_2[i], "Yes");
                    checkbox5_2.BackgroundColor = BaseColor.WHITE;
                    checkbox5_2.CheckType = RadioCheckField.TYPE_CIRCLE;
                    checkbox5_2.BorderWidth = 0.25F;
                    checkbox5_2.Checked = true;

                    checkbox5_2.BorderColor = new BaseColor(247, 170, 165);
                    checkbox5_2.Options = BaseField.READ_ONLY;

                    field5_2 = checkbox5_2.RadioField;
                    field5_2.SetFieldFlags(PdfFormField.FF_READ_ONLY);

                    field5_2 = checkbox5_2.RadioField;
                    field5_2.SetWidget(rect5_2, PdfAnnotation.HIGHLIGHT_INVERT);

                    radiogroup5_2.AddKid(field5_2);


                    writer.AddAnnotation(radiogroup5_2);

                    //writer.AddAnnotation(fieldpt);

                    PdfPCell cell = new PdfPCell();
                    cell.CellEvent = new itextsharp.iTextSharp.text.pdf.events.FieldPositioningEvents(writer, field5_2);
                    cell.Border = Rectangle.NO_BORDER;

                    Table5_2.AddCell(cell);

                    Paragraph paragraph = new Paragraph(premisesType5_2[i], RegularFont);
                    cell = new PdfPCell(paragraph);
                    cell.Border = Rectangle.NO_BORDER;
                    cell.Colspan = 2;
                    Table5_2.AddCell(cell);



                }

                Paragraph5_2 = new Paragraph("I/we shall be solely responsible for any issue arising out of any such non-compliance and further I/we do hereby jointly and severally Indemnify AEML, its agents and servants against all claims, proceedings, demands, costs, expenses and consequences" +
                                            " whatsoever, which may arise on account of processing my/our application.", RegularFont);
                Cell5_2 = new PdfPCell(Paragraph5_2);
                Cell5_2.Colspan = 2;
                Cell5_2.Border = Rectangle.NO_BORDER;
                Table5_2.AddCell(Cell5_2);

                document.Add(Table5_2);



                float[] widths5_3 = { 30, 300, 70, 300, 100 };
                PdfPTable table5_3 = new PdfPTable(widths5_3);
                table5_3.WidthPercentage = 100.0F;
                Paragraph paragraph5_3;
                PdfPCell cell5_3;

                //blank record 
                paragraph5_3 = new Paragraph("");
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.Border = Rectangle.NO_BORDER;
                cell5_3.Colspan = 5;
                cell5_3.FixedHeight = 10.0F;
                table5_3.AddCell(cell5_3);

                //HEading
                paragraph5_3 = new Paragraph("");
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.Border = Rectangle.NO_BORDER;
                cell5_3.Colspan = 1;
                cell5_3.FixedHeight = 40.0F;
                table5_3.AddCell(cell5_3);

                paragraph5_3 = new Paragraph("", RegularFont);
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5_3.FixedHeight = 40.0F;
                cell5_3.UseVariableBorders = true;
                //cell5_3.BorderColorTop = new BaseColor(255, 255, 255);
                cell5_3.BorderColorLeft = new BaseColor(23, 56, 133);
                cell5_3.BorderColorTop = new BaseColor(23, 56, 133);
                cell5_3.BorderColorRight = new BaseColor(23, 56, 13);
                cell5_3.BorderColorBottom = new BaseColor(23, 56, 133);
                cell5_3.Colspan = 1;
                table5_3.AddCell(cell5_3);

                paragraph5_3 = new Paragraph(" ", RegularFont);
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_3.FixedHeight = 40.0F;
                cell5_3.Border = Rectangle.NO_BORDER;
                //cell5_3.UseVariableBorders = true;
                ////cell5_3.BorderColorTop = new BaseColor(255, 255, 255);
                //cell5_3.BorderColorLeft = new BaseColor(23, 56, 133);
                //cell5_3.BorderColorTop = new BaseColor(23, 56, 133);
                //cell5_3.BorderColorRight = new BaseColor(23, 56, 13);
                //cell5_3.BorderColorBottom = new BaseColor(23, 56, 133);
                cell5_3.Colspan = 1;
                table5_3.AddCell(cell5_3);

                paragraph5_3 = new Paragraph("", RegularFont);
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_3.FixedHeight = 40.0F;
                cell5_3.UseVariableBorders = true;
                //cell5_3.BorderColorTop = new BaseColor(255, 255, 255);
                cell5_3.BorderColorLeft = new BaseColor(23, 56, 133);
                cell5_3.BorderColorTop = new BaseColor(23, 56, 133);
                cell5_3.BorderColorRight = new BaseColor(23, 56, 13);
                cell5_3.BorderColorBottom = new BaseColor(23, 56, 133);
                cell5_3.Colspan = 1;
                table5_3.AddCell(cell5_3);

                paragraph5_3 = new Paragraph(" ", RegularFont);
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_3.FixedHeight = 40.0F;
                cell5_3.Border = Rectangle.NO_BORDER;
                //cell5_3.UseVariableBorders = true;
                ////cell5_3.BorderColorTop = new BaseColor(255, 255, 255);
                //cell5_3.BorderColorTop = new BaseColor(23, 56, 133);
                //cell5_3.BorderColorLeft = new BaseColor(23, 56, 133);
                //cell5_3.BorderColorRight = new BaseColor(23, 56, 13);
                //cell5_3.BorderColorBottom = new BaseColor(23, 56, 133);
                cell5_3.Colspan = 1;
                table5_3.AddCell(cell5_3);
                //--------------------
                //
                paragraph5_3 = new Paragraph("");
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.Border = Rectangle.NO_BORDER;
                cell5_3.Colspan = 1;
                cell5_3.FixedHeight = 20.0F;
                table5_3.AddCell(cell5_3);

                paragraph5_3 = new Paragraph("Applicatn's Signature", RegularFont);
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_3.FixedHeight = 20.0F;
                cell5_3.Border = Rectangle.NO_BORDER;
                cell5_3.UseVariableBorders = true;
                cell5_3.Colspan = 1;
                table5_3.AddCell(cell5_3);

                paragraph5_3 = new Paragraph(" ", RegularFont);
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_3.FixedHeight = 20.0F;
                cell5_3.Border = Rectangle.NO_BORDER;
                cell5_3.Colspan = 1;
                table5_3.AddCell(cell5_3);

                paragraph5_3 = new Paragraph("(Signed by Second Party where the account is to be in joint names)", RegularFont);
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_3.Border = Rectangle.NO_BORDER;
                cell5_3.FixedHeight = 20.0F;
                cell5_3.Colspan = 1;
                table5_3.AddCell(cell5_3);

                paragraph5_3 = new Paragraph(" ", RegularFont);
                cell5_3 = new PdfPCell(paragraph5_3);
                cell5_3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_3.FixedHeight = 20.0F;
                cell5_3.Border = Rectangle.NO_BORDER;
                cell5_3.Colspan = 1;
                table5_3.AddCell(cell5_3);


                document.Add(table5_3);

                //Date and Place

                //For oOffice Use Only
                float[] lastwidth5_4 = { 30, 100, 150, 520, 100, 150 };
                PdfPTable lasttable5_4 = new PdfPTable(lastwidth5_4);
                lasttable5_4.WidthPercentage = 100.0F;
                lasttable5_4.SpacingBefore = 60;
                Paragraph lastParagraph5_4;
                PdfPCell lastCell5_4;

                lastParagraph5_4 = new Paragraph(" ", RegularFont);
                lastCell5_4 = new PdfPCell(lastParagraph5_4);
                lastCell5_4.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell5_4.Border = Rectangle.NO_BORDER;
                lastCell5_4.Colspan = 1;
                lasttable5_4.AddCell(lastCell5_4);



                //lastParagraph25 = new Paragraph(dsinput.Tables[0].Rows[0]["ZDATE_REG_REGDATE"].ToString().Trim(), RegularFont);
                lastParagraph5_4 = new Paragraph("Date   ", RegularFont);
                lastCell5_4 = new PdfPCell(lastParagraph5_4);
                lastCell5_4.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell5_4.Border = Rectangle.NO_BORDER;
                lastCell5_4.Colspan = 1;
                lasttable5_4.AddCell(lastCell5_4);

                //lastParagraph25 = new Paragraph(dsinput.Tables[0].Rows[0]["ZDATE_REG_REGDATE"].ToString().Trim(), RegularFont);
                lastParagraph5_4 = new Paragraph(oDt, RegularFont);
                lastCell5_4 = new PdfPCell(lastParagraph5_4);
                lastCell5_4.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell5_4.FixedHeight = 10.0F;
                lastCell5_4.UseVariableBorders = true;
                lastCell5_4.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell5_4.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell5_4.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell5_4.BorderColorBottom = new BaseColor(23, 56, 133);
                lastCell5_4.Colspan = 1;
                lasttable5_4.AddCell(lastCell5_4);


                lastParagraph5_4 = new Paragraph("");
                lastCell5_4 = new PdfPCell(lastParagraph5_4);
                lastCell5_4.Border = Rectangle.NO_BORDER;
                lastCell5_4.Colspan = 1;
                lasttable5_4.AddCell(lastCell5_4);


                lastParagraph5_4 = new Paragraph("Place", RegularFont);
                lastCell5_4 = new PdfPCell(lastParagraph5_4);
                lastCell5_4.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell5_4.Border = Rectangle.NO_BORDER;
                lastCell5_4.Colspan = 1;
                lasttable5_4.AddCell(lastCell5_4);


                lastParagraph5_4 = new Paragraph(" ", RegularFont);
                lastCell5_4 = new PdfPCell(lastParagraph5_4);
                lastCell5_4.HorizontalAlignment = Element.ALIGN_LEFT;
                lastCell5_4.FixedHeight = 10.0F;
                lastCell5_4.UseVariableBorders = true;
                lastCell5_4.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell5_4.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell5_4.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell5_4.BorderColorBottom = new BaseColor(23, 56, 133);
                lastCell5_4.Colspan = 1;
                lasttable5_4.AddCell(lastCell5_4);


                document.Add(lasttable5_4);


                //For oOffice Use Only
                float[] lastwidth25 = { 30, 220, 200, 320, 100, 150 };
                PdfPTable lasttable25 = new PdfPTable(lastwidth25);
                lasttable25.WidthPercentage = 100.0F;
                lasttable25.SpacingBefore = 30;
                Paragraph lastParagraph25;
                PdfPCell lastCell25;

                lastParagraph25 = new Paragraph(" ");
                lastCell25 = new PdfPCell(lastParagraph25);
                lastCell25.Border = Rectangle.NO_BORDER;
                lastCell25.Colspan = 6;
                lasttable25.AddCell(lastCell25);

                lastParagraph25 = new Paragraph("For Office Use", ColorFont);
                lastCell25 = new PdfPCell(lastParagraph25);
                lastCell25.Rowspan = 4;
                lastCell25.Rotation = 90;
                lastCell25.VerticalAlignment = 1;
                lastCell25.HorizontalAlignment = 1;
                //lastCell.CellEvent = new RoundedBorder();
                lastCell25.UseVariableBorders = true;
                lastCell25.BorderColorTop = new BaseColor(247, 170, 165);
                lastCell25.BorderColorLeft = new BaseColor(247, 170, 165);
                lastCell25.BorderColorRight = new BaseColor(247, 170, 165);
                lastCell25.BorderColorBottom = new BaseColor(247, 170, 165);
                lasttable25.AddCell(lastCell25);

                //1st ROW
                lastParagraph25 = new Paragraph("Business Partner Number", RegularFont);
                lastCell25 = new PdfPCell(lastParagraph25);
                //lastCell.BackgroundColor = new BaseColor(184, 199, 229);
                //lastCell.Border = Rectangle.NO_BORDER;
                lastCell25.Padding = 5.0F;
                lastCell25.FixedHeight = 20.0F;
                lastCell25.UseVariableBorders = true;
                lastCell25.BorderColorTop = new BaseColor(247, 170, 165);
                lastCell25.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell25.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell25.BorderColorBottom = new BaseColor(255, 255, 255);
                lastCell25.Colspan = 1;
                lasttable25.AddCell(lastCell25);


                lastParagraph25 = new Paragraph("");
                lastCell25 = new PdfPCell(lastParagraph25);
                //lastCell.BackgroundColor = new BaseColor(184, 199, 229);
                //lastCell.Border = Rectangle.NO_BORDER;
                lastCell25.UseVariableBorders = true;
                lastCell25.BorderColorTop = new BaseColor(247, 170, 165);
                lastCell25.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell25.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell25.BorderColorBottom = new BaseColor(23, 56, 133);
                lastCell25.FixedHeight = 20.0F;
                lastCell25.Padding = 5.0F;
                lastCell25.Colspan = 1;
                lasttable25.AddCell(lastCell25);


                lastParagraph25 = new Paragraph("Account No", RegularFont);
                lastCell25 = new PdfPCell(lastParagraph25);
                //lastCell.BackgroundColor = new BaseColor(184, 199, 229);
                //lastCell.Border = Rectangle.NO_BORDER;
                lastCell25.Padding = 5.0F;
                lastCell25.FixedHeight = 20.0F;
                lastCell25.HorizontalAlignment = 2;
                lastCell25.UseVariableBorders = true;
                lastCell25.BorderColorTop = new BaseColor(247, 170, 165);
                lastCell25.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell25.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell25.BorderColorBottom = new BaseColor(255, 255, 255);
                lastCell25.Colspan = 1;
                lasttable25.AddCell(lastCell25);

                lastParagraph25 = new Paragraph("");
                lastCell25 = new PdfPCell(lastParagraph25);
                //lastCell.BackgroundColor = new BaseColor(184, 199, 229);
                //lastCell.Border = Rectangle.NO_BORDER;
                lastCell25.UseVariableBorders = true;
                lastCell25.BorderColorTop = new BaseColor(247, 170, 165);
                lastCell25.BorderColorRight = new BaseColor(247, 170, 165);
                lastCell25.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell25.BorderColorBottom = new BaseColor(23, 56, 133);
                lastCell25.FixedHeight = 20.0F;
                lastCell25.Padding = 5.0F;
                lastCell25.Colspan = 2;
                lasttable25.AddCell(lastCell25);

                //2nd Row
                lastParagraph25 = new Paragraph("", RegularFont);
                lastCell25 = new PdfPCell(lastParagraph25);
                //lastCell.BackgroundColor = new BaseColor(184, 199, 229);
                lastCell25.Border = Rectangle.NO_BORDER;
                //lastCell25.UseVariableBorders = true;
                ////lastCell25.BorderColorBottom = new BaseColor(255, 255, 255);
                //lastCell25.BorderColorTop = new BaseColor(255, 255, 255);
                ////lastCell25.BorderColorLeft = new BaseColor(255, 255, 255);
                //lastCell25.BorderColorRight = new BaseColor(247, 170, 165);
                lastCell25.FixedHeight = 20.0F;
                lastCell25.Padding = 5.0F;
                lastCell25.Colspan = 4;
                lasttable25.AddCell(lastCell25);


                lastParagraph25 = new Paragraph("", RegularFont);
                lastCell25 = new PdfPCell(lastParagraph25);
                //lastCell.BackgroundColor = new BaseColor(184, 199, 229);
                //lastCell25.Border = Rectangle.NO_BORDER;                
                lastCell25.UseVariableBorders = true;
                lastCell25.BorderColorBottom = new BaseColor(255, 255, 255);
                //lastCell25.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell25.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell25.BorderColorRight = new BaseColor(247, 170, 165);
                lastCell25.FixedHeight = 20.0F;
                lastCell25.Padding = 5.0F;
                lastCell25.Colspan = 1;
                lasttable25.AddCell(lastCell25);

                //3rd Row
                lastParagraph25 = new Paragraph("Application processed by", RegularFont);
                lastCell25 = new PdfPCell(lastParagraph25);
                //lastCell.BackgroundColor = new BaseColor(184, 199, 229);
                //lastCell.Border = Rectangle.NO_BORDER;                
                lastCell25.UseVariableBorders = true;
                lastCell25.BorderColorBottom = new BaseColor(255, 255, 255);
                lastCell25.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell25.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell25.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell25.FixedHeight = 20.0F;
                lastCell25.Padding = 5.0F;
                lastCell25.Colspan = 1;
                lasttable25.AddCell(lastCell25);

                //lastParagraph25 = new Paragraph(dsinput.Tables[0].Rows[0]["ZNRCA_NEARSTCA"].ToString().Trim(), RegularFont);
                lastParagraph25 = new Paragraph("");
                lastCell25 = new PdfPCell(lastParagraph25);
                //lastCell.BackgroundColor = new BaseColor(184, 199, 229);
                lastCell25.UseVariableBorders = true;
                lastCell25.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell25.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell25.BorderColorBottom = new BaseColor(23, 56, 133);
                lastCell25.BorderColorRight = new BaseColor(247, 170, 165);
                //lastCell.Border = Rectangle.NO_BORDER;
                lastCell25.FixedHeight = 20.0F;
                lastCell25.Padding = 5.0F;
                lastCell25.Colspan = 5;
                lasttable25.AddCell(lastCell25);

                //4th row
                lastParagraph25 = new Paragraph("", RegularFont);
                lastCell25 = new PdfPCell(lastParagraph25);
                //lastCell.BackgroundColor = new BaseColor(184, 199, 229);
                //lastCell.Border = Rectangle.NO_BORDER;
                lastCell25.UseVariableBorders = true;
                lastCell25.BorderColorBottom = new BaseColor(247, 170, 165);
                lastCell25.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell25.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell25.BorderColorRight = new BaseColor(255, 255, 255);
                lastCell25.FixedHeight = 20.0F;
                lastCell25.Padding = 5.0F;
                lastCell25.Colspan = 1;
                lasttable25.AddCell(lastCell25);

                lastParagraph25 = new Paragraph("", RegularFont);
                lastCell25 = new PdfPCell(lastParagraph25);
                //lastCell.BackgroundColor = new BaseColor(184, 199, 229);
                //lastCell.Border = Rectangle.NO_BORDER;
                lastCell25.UseVariableBorders = true;
                lastCell25.BorderColorBottom = new BaseColor(247, 170, 165);
                //lastCell25.BorderColorTop = new BaseColor(255, 255, 255);
                lastCell25.BorderColorLeft = new BaseColor(255, 255, 255);
                lastCell25.BorderColorRight = new BaseColor(247, 170, 165);
                lastCell25.FixedHeight = 20.0F;
                lastCell25.Padding = 5.0F;
                lastCell25.Colspan = 4;
                lasttable25.AddCell(lastCell25);


                document.Add(lasttable25);

                var imageDocument2 = dataContext.CONApplicationDocumentDetails.Where(p => p.RegistrationSerialNumber == con_data.RegistrationSerialNumber && p.DocumentTypeCode == "PH" && p.DocumentDescription == "PASSPORT SIZE PHOTOGRAPH OF THE APPLICANT 2").FirstOrDefault();
                if (imageDocument2 != null)
                {
                    document.NewPage();

                    float[] width60 = { 20, 500, 529 };
                    PdfPTable table60 = new PdfPTable(widths6);
                    table60.WidthPercentage = 100.0F;
                    Paragraph paragraph60;
                    PdfPCell cell60;

                    paragraph60 = new Paragraph("", boldFont);
                    cell60 = new PdfPCell(paragraph60);
                    cell60.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell60.Border = Rectangle.NO_BORDER;
                    cell60.BackgroundColor = new BaseColor(171, 49, 117);
                    cell60.Colspan = 1;
                    table60.AddCell(cell60);
                    paragraph60 = new Paragraph("Applicant 2", RegularFontWhite);
                    cell60 = new PdfPCell(paragraph60);
                    cell60.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell60.Border = Rectangle.NO_BORDER;
                    cell60.BackgroundColor = new BaseColor(171, 49, 117);
                    cell60.Colspan = 1;
                    table60.AddCell(cell60);

                    paragraph60 = new Paragraph("", boldFont);
                    cell60 = new PdfPCell(paragraph60);
                    cell60.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell60.Border = Rectangle.NO_BORDER;
                    cell60.BackgroundColor = new BaseColor(171, 49, 117);
                    cell60.Colspan = 1;
                    table60.AddCell(cell60);
                    //-----------------------

                    paragraph60 = new Paragraph(" ", RegularFontWhite);
                    cell60 = new PdfPCell(paragraph60);
                    cell60.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell3pg.Padding = 5;
                    cell60.Colspan = 1;
                    cell60.Rowspan = 2;
                    cell60.Border = Rectangle.NO_BORDER;
                    table60.AddCell(cell60);
                    document.Add(table60);


                    float[] PH2widths = { 2, 1, 2 };

                    PdfPTable PH2Table = new PdfPTable(3);
                    //mainTable.SetWidthPercentage(float widths,Rectangle 100.0F);
                    PH2Table.WidthPercentage = 100.0F;
                    PH2Table.SetWidths(PH2widths);
                    PH2Table.HorizontalAlignment = 1;
                    PH2Table.SpacingBefore = 20f;
                    PdfPCell PH2Cell;
                    Paragraph PH2Paragraph;

                    PdfPTable PH2Table2 = new PdfPTable(1);
                    PdfPCell PH2Cell2;
                    PH2Paragraph = new Paragraph(" ");
                    PH2Cell2 = new PdfPCell(PH2Paragraph);
                    PH2Cell2.Colspan = 1;
                    PH2Cell2.Border = 0;
                    PH2Cell2.HorizontalAlignment = 1;
                    PH2Table2.AddCell(PH2Cell2);
                    PH2Table2.AddCell(PH2Cell2);
                    PH2Table2.AddCell(PH2Cell2);
                    PH2Table2.AddCell(PH2Cell2);
                    PH2Table2.AddCell(PH2Cell2);
                    PH2Table2.AddCell(PH2Cell2);

                    PH2Cell = new PdfPCell(PH2Table2);
                    PH2Cell.Border = 0;
                    PH2Table.AddCell(PH2Cell);

                    //PH2Table.AddCell(PH2Paragraph);
                    //PH2Table.AddCell(PH2Cell);

                    //PdfPTable PH2Table2 = new PdfPTable(1);
                    //PH2Paragraph = new Paragraph(" ");
                    //PdfPCell PH2Cell1 = new PdfPCell(PH2Paragraph);
                    //PH2Cell1.Border = 0;
                    //PH2Table2.AddCell(PH2Cell1);
                    //PH2Table2.AddCell(PH2Cell1);
                    //PH2Table2.AddCell(PH2Cell1);



                    if (imageDocument2.DocumentData != null)
                    {
                        itextsharp.iTextSharp.text.Image img1 = itextsharp.iTextSharp.text.Image.GetInstance(imageDocument2.DocumentData.ToArray());
                        img1.Alignment = itextsharp.iTextSharp.text.Image.ALIGN_CENTER;
                        img1.ScaleToFit(95f, 115f);
                        PH2Cell = new PdfPCell(img1);
                    }
                    else
                    {
                        PH2Paragraph = new Paragraph("Affix Passport size photograph\nand sign across", smallFont);
                        PH2Cell = new PdfPCell(PH2Paragraph);
                    }

                    PH2Cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    PH2Cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    PH2Cell.Rowspan = 7;
                    PH2Cell.Colspan = 1;
                    PH2Cell.PaddingTop = 0.0F;
                    PH2Table.AddCell(PH2Cell);


                    PH2Paragraph = new Paragraph("");
                    PH2Cell = new PdfPCell(PH2Paragraph);
                    PH2Cell.Border = Rectangle.NO_BORDER;
                    PH2Cell.Colspan = 1;
                    PH2Table.AddCell(PH2Cell);

                    PH2Paragraph = new Paragraph("", RegularFontWhite);
                    PH2Cell = new PdfPCell(PH2Paragraph);
                    PH2Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    PH2Cell.PaddingRight = 30.0F;
                    PH2Cell.Border = Rectangle.NO_BORDER;
                    PH2Cell.Colspan = 7;
                    PH2Cell.PaddingRight = 30.0F;
                    PH2Table.AddCell(PH2Cell);


                    PH2Paragraph = new Paragraph("");
                    PH2Cell = new PdfPCell(PH2Paragraph);
                    PH2Cell.Border = Rectangle.NO_BORDER;
                    PH2Cell.Colspan = 1;
                    PH2Table.AddCell(PH2Cell);

                    //mainParagraph = new Paragraph("(please provide complete details)", smallFont);
                    PH2Paragraph = new Paragraph(" ", RegularFontWhite);
                    PH2Cell = new PdfPCell(PH2Paragraph);
                    PH2Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    PH2Cell.Border = Rectangle.NO_BORDER;
                    PH2Cell.Colspan = 7;
                    //mainCell.FixedHeight = 5.0F;
                    PH2Cell.PaddingBottom = 5.0F;
                    PH2Table.AddCell(PH2Cell);


                    PH2Paragraph = new Paragraph("", RegularFontWhite);
                    PH2Cell = new PdfPCell(PH2Paragraph);
                    PH2Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    PH2Cell.Border = Rectangle.NO_BORDER;
                    PH2Cell.Colspan = 4;
                    PH2Cell.Padding = 5.0F;
                    //mainCell.BackgroundColor = new BaseColor(171, 49, 117);
                    PH2Table.AddCell(PH2Cell);

                    PH2Paragraph = new Paragraph("", RegularFontWhite);
                    PH2Cell = new PdfPCell(PH2Paragraph);
                    PH2Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    PH2Cell.Border = Rectangle.NO_BORDER;
                    PH2Cell.Colspan = 4;
                    PH2Cell.Padding = 5.0F;
                    PH2Table.AddCell(PH2Cell);


                    PH2Paragraph = new Paragraph("");
                    PH2Cell = new PdfPCell(PH2Paragraph);
                    PH2Cell.Border = Rectangle.NO_BORDER;
                    PH2Cell.Colspan = 1;
                    PH2Table.AddCell(PH2Cell);




                    //Creating Nested table ends.

                    PH2Table.SpacingAfter = 10.0F;

                    document.Add(PH2Table);

                }



                document.Close();

                return output.ToArray();
            }

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

        #endregion
    }
}