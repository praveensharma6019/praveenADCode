namespace Sitecore.Feature.Accounts.Controllers
{

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
    using Sitecore.Feature.Accounts.SessionHelper;
    using Sitecore.Foundation.Alerts.Models;
    using Sitecore.Foundation.Dictionary.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Attributes;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
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
    using static Sitecore.Feature.Accounts.Controllers.AccountsController;

    [CookieTemperingRedirectNotFound]
    public class AEMLRevampComplaintController : Controller
    {
        public AEMLRevampComplaintController(IAccountRepository accountRepository, INotificationService notificationService, IAccountsSettingsService accountsSettingsService, IGetRedirectUrlService getRedirectUrlService, IUserProfileService userProfileService, IFedAuthLoginButtonRepository fedAuthLoginRepository, IUserProfileProvider userProfileProvider, IPaymentService paymentService, IDbAccountService dbAccountService)
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


        #region ||** Login / LogOut **||

        [RedirectAuthenticatedComplaintPortal]
        public ActionResult LoginComplaintRevamp(string returnUrl = null)
        {
            //if logged in with my account then also allow this
            if (Context.User != null && Context.User.IsAuthenticated)
            {
                ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                var profile = this.UserProfileService.GetProfile(Context.User);
                UserSession.AEMLComplaintUserSessionContext = new LoginInfoComplaint
                {
                    LoginName = profile.LoginName,
                    AccountNumber = profile.AccountNumber,
                    MobileNumber = profile.MobileNumber,
                    IsRegistered = true,
                    IsAuthenticated = true
                };
                var isLogCreated = objComplaintPortalService.CreateLoginLog(UserSession.AEMLComplaintUserSessionContext, false);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp);
                return this.Redirect(item.Url());
            }

            LoginInfoComplaint model = new LoginInfoComplaint
            {
                ReturnUrl = returnUrl,
                LoginButtons = this.FedAuthLoginRepository.GetAll()
            };
            return this.View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult LoginComplaintRevamp(LoginInfoComplaint model, string loginWithCredentials = null, string loginWithOTP = null, string GetOTP = null, string validateOTP = null)
        {
            if (!string.IsNullOrEmpty(loginWithCredentials))
            {
                this.ModelState["AccountNumber"].Errors.Clear();
                if (string.IsNullOrEmpty(model.LoginName) && string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError(nameof(model.LoginName), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/Enter User Name password", "Please enter valid User Name and Password."));
                    return this.View(model);
                }
                if (string.IsNullOrEmpty(model.LoginName))
                {
                    ModelState.AddModelError(nameof(model.LoginName), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/Enter User Name", "Please enter valid User Name."));
                    return this.View(model);
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/Enter password", "Please enter valid Password."));
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

                return this.LoginComplaintRevamp(model, redirectUrl => new RedirectResult(redirectUrl));
            }
            else if (!string.IsNullOrEmpty(loginWithOTP))
            {
                this.ModelState["LoginName"].Errors.Clear();
                this.ModelState["Password"].Errors.Clear();
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/Enter User Name", "Please enter valid Account Number."));
                    return this.View(model);
                }
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha2), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha2), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    return this.View(model);
                }

                var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.AccountNumber);
                if (string.IsNullOrEmpty(consumerDetails.AccountNumber) || consumerDetails.INVALIDCAFLAG == "X")
                {
                    ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Account Number incorrect", "Entered Account no is invalid. Please enter a valid 9 digit Account No."));
                    return this.View(model);
                }

                //Get Mobile number
                var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.AccountNumber);
                if (string.IsNullOrEmpty(registeredMobileNumber))
                {
                    ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Mobile Number incorrect", "Mobile Number is not registered."));
                    return this.View(model);
                }
                model.MobileNumber = string.IsNullOrEmpty(consumerDetails.MOBILE_NO) ? consumerDetails.MOBILE_NO : consumerDetails.MOBILE_NO.Substring(0, 2) + "xxxxxxx" + consumerDetails.MOBILE_NO.Substring(consumerDetails.MOBILE_NO.Length - 3);
                model.IsLoginViaOTP = true;
                model.IsAccountValid = true;
                Session["ComplaintRegistrationModel"] = model;
            }
            else if (!string.IsNullOrEmpty(GetOTP))
            {
                this.ModelState["LoginName"].Errors.Clear();
                this.ModelState["Password"].Errors.Clear();
                ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                if (Session["ComplaintRegistrationModel"] == null)
                {
                    this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }
                LoginInfoComplaint accountNumberValidated = (LoginInfoComplaint)Session["ComplaintRegistrationModel"];
                if (model.AccountNumber == accountNumberValidated.AccountNumber)
                {

                    //if valid then send otp to registered mobile number 
                    //send SMS for OTP
                    RegistrationRepository registrationRepo = new RegistrationRepository();
                    if (!registrationRepo.CheckForCAMaxLimit(model.AccountNumber, "ComplaintRegistration"))
                    {
                        Log.Info("PAN Update: Number of attempt to get OTP reached for AccountNumber." + model.AccountNumber, this);
                        this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/Max20OTPPerLECMobile", "Number of attempt to get OTP reached for Entered AccountNumber."));
                        return this.View(model);
                    }

                    var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.AccountNumber);
                    string generatedotp = registrationRepo.GenerateOTPRegistration(model.AccountNumber, null, "ComplaintRegistration", registeredMobileNumber);
                    //send otp via SMS
                    #region Api call to send SMS of OTP
                    try
                    {
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/OTP API URL",
                            "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=You have initiated a complaint portal login request for Account no. {1}. OTP for validation is {2}. Adani Electricity.&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707161667115108405"), registeredMobileNumber, model.AccountNumber, generatedotp);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("ChangeOfNameLECUserRegisteration: OTP Api call success for LEC registration", this);
                            model.IsLoginViaOTP = true;
                            model.IsAccountValid = true;
                            model.IsOTPSend = true;
                            Session["ComplaintRegistrationModel"] = model;
                            return this.View(model);
                        }
                        else
                        {
                            Log.Error("ChangeOfNameLECUserRegisteration OTP Api call failed for registration", this);
                            this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/OTP sending error", "Unable to send OTP."));
                            model.IsLoginViaOTP = true;
                            model.IsAccountValid = true;
                            model.IsOTPSend = false;
                            return this.View(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("ChangeOfNameLECUserRegisteration: OTP Api call failed for registration: " + ex.Message, this);
                        this.ModelState.AddModelError(nameof(model.MobileNumber), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/OTP sending error", "Unable to send OTP."));
                        model.IsLoginViaOTP = true;
                        model.IsAccountValid = true;
                        model.IsOTPSend = false;
                        return this.View(model);
                    }
                    #endregion

                }
            }
            else if (!string.IsNullOrEmpty(validateOTP))
            {
                this.ModelState["LoginName"].Errors.Clear();
                this.ModelState["Password"].Errors.Clear();
                ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                if (Session["ComplaintRegistrationModel"] == null)
                {
                    this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter OTP", "Please validate."));
                    return this.View(model);
                }
                LoginInfoComplaint accountNumberValidated = (LoginInfoComplaint)Session["ComplaintRegistrationModel"];
                if (model.AccountNumber == accountNumberValidated.AccountNumber)
                {
                    if (string.IsNullOrEmpty(model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter OTP", "Enter OTP."));
                        model.IsLoginViaOTP = true;
                        return this.View(model);
                    }

                    RegistrationRepository registrationRepo = new RegistrationRepository();
                    var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(model.AccountNumber);
                    string generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(registeredMobileNumber, model.AccountNumber, "ComplaintRegistration");

                    if (!string.Equals(generatedOTP, model.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(model.OTPNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                        model.IsLoginViaOTP = true;
                        return this.View(model);
                    }

                    var userDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);
                    UserSession.AEMLComplaintUserSessionContext = new LoginInfoComplaint
                    {
                        LoginName = userDetails.Name,
                        AccountNumber = model.AccountNumber,
                        MobileNumber = model.MobileNumber,
                        IsRegistered = false,
                        IsAuthenticated = true
                    };

                    var isLogCreated = objComplaintPortalService.CreateLoginLog(model, false);
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp));
                }
            }
            return View(model);
        }

        protected virtual ActionResult LoginComplaintRevamp(LoginInfoComplaint loginInfo, Func<string, ActionResult> redirectAction)
        {
            try
            {
                ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();

                //first check if login is for Secretary or zonal admin
                string adminRole = objComplaintPortalService.IsComplaintPortalAdminLogin(loginInfo.LoginName, loginInfo.Password);
                if (!string.IsNullOrEmpty(adminRole))
                {
                    UserSession.AEMLComplaintUserSessionContext = new LoginInfoComplaint
                    {
                        LoginName = loginInfo.LoginName,
                        AccountNumber = null,
                        MobileNumber = null,
                        IsRegistered = false,
                        IsAdmin = true,
                        AdminRole = adminRole,
                        IsAuthenticated = true
                    };
                    var isLogCreated = objComplaintPortalService.CreateLoginLog(loginInfo, true);
                    if (adminRole.ToLower() == "icrsadmin")
                    {
                        return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalICRSAdminHomePageRevamp));
                    }
                    else
                    {
                        return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp));
                    }
                }
                else
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
                        //SessionId = sessionId.ToString(),
                        SessionId = Request.Cookies["ASP.NET_SessionId"].Value,
                        UserName = loginInfo.LoginName
                    };

                    RegistrationRepository rp = new RegistrationRepository();
                    rp.StoreCurrentSession();

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

                    var profile = this.UserProfileService.GetProfile(Context.User);
                    UserSession.AEMLComplaintUserSessionContext = new LoginInfoComplaint
                    {
                        LoginName = profile.LoginName,
                        AccountNumber = profile.AccountNumber,
                        MobileNumber = profile.MobileNumber,
                        IsRegistered = true,
                        //SessionId = sessionId.ToString(),
                        SessionId = Request.Cookies["ASP.NET_SessionId"].Value,
                        IsAuthenticated = true
                    };

                    loginInfo.AccountNumber = profile.AccountNumber;
                    loginInfo.MobileNumber = profile.MobileNumber;
                    var isLogCreated = objComplaintPortalService.CreateLoginLog(loginInfo, true);
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp));
                }
            }
            catch (Exception ex)
            {
                Log.Error("Login Method Error - for User - " + loginInfo.LoginName, ex.Message);
                this.ModelState.AddModelError(nameof(loginInfo.Password), ex.Message);
                return this.View(loginInfo);
            }
        }

        #endregion

        public ComplaintFileRegistrationModel GetSubcategory(ComplaintFileRegistrationModel model)
        {
            if (!string.IsNullOrEmpty(model.SelectedComplaintCategory))
            {
                ComplaintPortalService complaintPortalService = new ComplaintPortalService();
                var selectedsubCategory = model.SelectedComplaintSubCategory;
                var listComplaintsubCategory = complaintPortalService.GetComplaintSubCategoryList(Convert.ToInt32(model.SelectedComplaintCategory));
                model.ComplaintSubCategorySelectList = new List<ListItem>();
                if (listComplaintsubCategory != null && listComplaintsubCategory.Any())
                {
                    foreach (var item in listComplaintsubCategory)
                    {
                        model.ComplaintSubCategorySelectList.Add(new ListItem
                        {
                            Text = item.SubCategory,
                            Value = item.SubCategoryId.ToString()
                        });
                    }
                }
                //if (model.ComplaintSubCategorySelectList != null && model.ComplaintSubCategorySelectList.Count() == 1)
                //    model.SelectedComplaintSubCategory = model.ComplaintSubCategorySelectList.FirstOrDefault().Value;
                //else
                //    model.SelectedComplaintSubCategory = selectedsubCategory;
            }
            return model;
        }


        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult Home()
        {
            try
            {
                if (UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                {
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalAdminHomePage));
                }
                LoginInfoComplaint model = new LoginInfoComplaint();
                model.LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName;
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error("Complaint portal home " + e.Message, this);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult CGRFHome()
        {
            try
            {
                if (UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                {
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp));
                }
                LoginInfoComplaint model = new LoginInfoComplaint();
                model.LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName;
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error("Complaint portal home " + e.Message, this);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult HelpdeskHome()
        {
            try
            {
                if (UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                {
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp));
                }
                LoginInfoComplaint model = new LoginInfoComplaint();
                model.LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName;
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error("Complaint portal home " + e.Message, this);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult ICRSAdminHome()
        {
            try
            {
                if (!UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                {
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp));
                }
                ComplaintPortalService objservice = new ComplaintPortalService();

                TrackComplaintModel model = new TrackComplaintModel();
                model.LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName;

                model.StartDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
                model.EndDate = DateTime.Now.ToString("dd/MM/yyyy");

                model.ComplaintList = objservice.GetICRSComplaints(DateTime.Now.AddDays(-30), DateTime.Now, null);

                return View(model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, this);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ICRSAdminHome(TrackComplaintModel model, string Status)
        {
            try
            {
                ComplaintPortalService objservice = new ComplaintPortalService();

                DateTime startDate = (DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                DateTime endDate = (DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                model.ComplaintList = objservice.GetICRSComplaints(startDate, endDate, Status);

                return this.View(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error at TrackComplaint Post " + ex.Message, this);
            }
            return this.View(model);
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult CGRFAdminHome()
        {
            try
            {
                if (!UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                {
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp));
                }
                ComplaintPortalService objservice = new ComplaintPortalService();

                TrackComplaintModel model = new TrackComplaintModel();
                model.LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName;

                model.StartDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
                model.EndDate = DateTime.Now.ToString("dd/MM/yyyy");

                model.ComplaintList = objservice.GetCGRFComplaints(DateTime.Now.AddDays(-30), DateTime.Now, null);

                return View(model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, "CGRFAdminHome");
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult CGRFAdminHome(TrackComplaintModel model, string Status)
        {
            try
            {
                ComplaintPortalService objservice = new ComplaintPortalService();

                DateTime startDate = (DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                DateTime endDate = (DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                model.ComplaintList = objservice.GetCGRFComplaints(startDate, endDate, Status);

                return this.View(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error at TrackComplaint Post " + ex.Message, this);
            }
            return this.View(model);
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult CGRFAdminReports()
        {
            try
            {
                if (!UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                {
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp));
                }
                ComplaintPortalService objservice = new ComplaintPortalService();

                ComplaintReportModel model = new ComplaintReportModel();
                model.LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName;

                model.StartDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
                model.EndDate = DateTime.Now.ToString("dd/MM/yyyy");

                //model.ComplaintList = objservice.GetCGRFComplaints(DateTime.Now.AddDays(-30), DateTime.Now, null);

                return View(model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, this);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        public ActionResult ExportComplaints(string startDate, string endDate, string consumerZone, string consumerDivision, string complaintstatus, string complaintCategory)
        {
            try
            {
                ComplaintPortalService objservice = new ComplaintPortalService();

                DateTime sd = (DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                DateTime ed = (DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                var datalist = objservice.GetCGRFComplaintsForReport(sd, ed, consumerZone, consumerDivision, complaintstatus, complaintCategory);
                DataTable table = ToDataTable(datalist);
                DatatableToCSV(table);
                //return this.View(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error at TrackComplaint Post " + ex.Message, this);
            }
            var reportPageurl = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminReportsPageRevamp).Url();
            return Redirect(reportPageurl);
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult FileComplaintRevamp(string complaintNumber = null)
        {
            try
            {
                //to do - make it dynamic
                UserSession.AEMLComplaintUserSessionContext.ComplaintLevel = "Level1";
                ComplaintPortalService objservice = new ComplaintPortalService();
                ComplaintFileRegistrationModel model = new ComplaintFileRegistrationModel();

                if (!string.IsNullOrEmpty(complaintNumber))
                {
                    //if complaint exists and is related to logged in account number or not
                    if (objservice.CheckIfComplaintExists(complaintNumber))
                    {
                        model = objservice.GetComplaintDetails(complaintNumber);
                        if (!string.IsNullOrEmpty(model.AccountNumber))
                        {
                            model = GetSubcategory(model);
                            if (model.ComplaintStatus != "1")
                                model.isReadOnly = true;

                            return View(model);
                        }
                        else
                        {
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/COmplaintPortal/No complaint", "Complaint Does not exists or not related to this account."));
                            var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalTrackComplaintPage);
                            return this.Redirect(item.Url());
                        }
                    }
                    else
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/COmplaintPortal/No complaint", "Complaint Does not exists or not related to this account."));
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalTrackComplaintPage);
                        return this.Redirect(item.Url());
                    }

                    //if saved - open in editable mode
                    //if submitted - open in read only mode
                    //if closed - open in read only mode with status and comments 
                }

                var userDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                //var userDetails2 = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                model = new ComplaintFileRegistrationModel
                {
                    LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName,
                    AccountNumber = userDetails.CANumber,
                    Address = userDetails.Street + ", " + userDetails.Street2 + ", " + userDetails.Street3 + ", " + userDetails.City + ", " + userDetails.PinCode,
                    City = userDetails.City,
                    //ConsumerCategory = userDetails2.TARIFTYP_Ratecategory,
                    ConsumerName = userDetails.Name,
                    EmailId = userDetails.Email,
                    MobileNumber = userDetails.Mobile,
                    //Pincode = userDetails.PinCode,
                    //ZoneName = userDetails2.ZoneName,
                    //DivisionName = userDetails2.DivisionName,
                    //SelectedConsumerCategory = userDetails2.TARIFTYP_Ratecategory
                };
                Session["ComplaintFileRegistrationModel"] = model;
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, this);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult FileComplaintRevamp(ComplaintFileRegistrationModel model, string latCoordinate, string longCoordinate, string SaveAsDraft = null, string SubmitApplication = null, string SubmitFeedbackYes = null, string SubmitFeedbackNo = null, string SubmitFeedback = null, string SubmitFeedbackCancel = null)
        {
            try
            {
                string areaAndPoleDetails = string.Empty;
                ComplaintPortalService complaintPortalService = new ComplaintPortalService();
                
                if (!string.IsNullOrEmpty(SaveAsDraft))
                {
                    ComplaintFileRegistrationModel accountNumberValidated = (ComplaintFileRegistrationModel)Session["ComplaintFileRegistrationModel"];

                    if (model.AccountNumber == accountNumberValidated.AccountNumber)
                    {
                        //Save details in database 
                        //Show message 
                        //If Street Light Category
                        ComplaintRegistrationResponse result = complaintPortalService.SaveComplaintApplication(model, UserSession.AEMLComplaintUserSessionContext.ComplaintLevel, false, areaAndPoleDetails);

                        if (string.IsNullOrEmpty(result.Error))
                        {
                            ViewBag.IsSuccessful = true;
                            ViewBag.IsSubmit = true;

                            string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSave", "Your application is saved successfully. The application can be viewed and submitted via the Track Application Section.");

                            //ViewBag.Message = messagetobedisplayed;
                            this.Session["UpdateRegisterComplaintMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSaveHeading", "Success"), messagetobedisplayed, InfoMessageRevamp.MessageTypeRevamp.Success, true);
                            //Session["Message"] = messagetobedisplayed;
                            //var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalHelpdeskHomePage);
                            //return RedirectPermanent(item.Url());

                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), messagetobedisplayed);
                            return this.View(model);
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again! " + result.Error));
                            return this.View(model);
                        }
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(SubmitApplication))
                {
                    //Save details in database 
                    //SAP integration
                    //show message with complaint registration number generated from SAP
                    //SMS will be sent from SAP service
                    bool isValid = true;
                    if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                    {
                        ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                        //return this.View(model);
                        isValid = false;
                    }
                    ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                    if (!reCaptchaResponse.success)
                    {
                        ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                        //return this.View(model);
                        isValid = false;
                    }

                    ComplaintFileRegistrationModel accountNumberValidated = (ComplaintFileRegistrationModel)Session["ComplaintFileRegistrationModel"];

                    if (model.AccountNumber == accountNumberValidated.AccountNumber)
                    {
                        //check for description
                        if (string.IsNullOrEmpty(model.ComplaintDescription))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ComplaintDescription", "Kindly provide Complaint details."));
                            isValid = false;
                        }

                        //check for uploads Save documents, document upload
                        if (model.SelectedComplaintCategory == "2" && (model.SelectedComplaintSubCategory == "4" || model.SelectedComplaintSubCategory == "5"))
                        {

                            if (Request.Files["fileTransactionReceipt"] == null)
                            {
                                this.ModelState.AddModelError(nameof(model.TransactionReceipt), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TransactionReceipt", "Kindly attach transaction receipt."));
                                isValid = false;
                            }
                            else
                            {
                                HttpPostedFileBase file = Request.Files["fileTransactionReceipt"];
                                if (!CheckExtension(file))
                                {
                                    this.ModelState.AddModelError(nameof(model.TransactionReceipt), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TransactionReceiptInvalid", "Kindly attach transaction receipt."));
                                    isValid = false;
                                }
                            }
                            if (Request.Files["fileBankAccountStatement"] == null)
                            {
                                this.ModelState.AddModelError(nameof(model.BankAccountStatement), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/BankAccountStatement", "Kindly attach latest Bank Account statement showing the said debit. "));
                                isValid = false;
                            }
                            else
                            {
                                HttpPostedFileBase file = Request.Files["fileBankAccountStatement"];
                                if (!CheckExtension(file))
                                {
                                    this.ModelState.AddModelError(nameof(model.BankAccountStatement), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/BankAccountStatementInvalid", "Kindly attach latest Bank Account statement showing the said debit. "));
                                    isValid = false;
                                }
                            }

                        }

                        if (!isValid)
                        {
                            model = GetSubcategory(model);
                            return this.View(model);
                        }
                        //check if similar type of complaint is already exists and is open, exept Other category

                        //Other: M24
                        //New Connection: M40
                        //Wrong connection: M50
                        //Reconnection: M51
                        //ICRS : M20
                        //4.Billing Related Wrong tariff plan-M06
                        //5.Billing Related Wrong Reading - I15
                        //6.Billing Related Wrong Payment - M03
                        //7.Billing Related Payment not credited-M04
                        //8.Billing Related Outcalling for high consumption complaint - M52
                        string PMACT = string.Empty;
                        //if (model.SelectedComplaintCategory == "6") //Other
                        //    PMACT = "M24";
                        //else 
                        if (model.SelectedComplaintCategory == "3")
                            PMACT = "M40";
                        else if (model.SelectedComplaintCategory == "4")
                            PMACT = "M50";
                        else if (model.SelectedComplaintCategory == "5")
                            PMACT = "M51";
                        else if (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "2")
                            PMACT = "M06";
                        else if (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "3")
                            PMACT = "I15";
                        else if (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "4")
                            PMACT = "M03";
                        else if (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "5")
                            PMACT = "M04";
                        else if (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "6")
                            PMACT = "M52";

                        ComplaintPortalService objservice = new ComplaintPortalService();
                        var ComplaintList = objservice.GetAllcomplaintsFromSAPByCA(UserSession.AEMLComplaintUserSessionContext.AccountNumber);

                        if (ComplaintList.Any(c => c.ComplaintSubCategory == PMACT && c.ComplaintStatusCode == "2"))
                        {
                            var comp = ComplaintList.FirstOrDefault(c => c.ComplaintSubCategory == PMACT && c.ComplaintStatusCode == "2");
                            //dispay error, complaint exists with similar category
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/already exists", "Similar type of complaint already exists with complaint number " + comp.ComplaintNumber));
                            model = GetSubcategory(model);
                            return this.View(model);
                        }
                        //var userDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(GetPrimaryAccountNumber());
                        //if (string.IsNullOrEmpty(userDetails.Mobile))
                        if (string.IsNullOrEmpty(model.MobileNumber))
                        {
                            //dispay error, if mobile number is not registered
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/mobile number is not registered", "Please register your Mobile number in my account before submitting any complaint."));
                            model = GetSubcategory(model);
                            return this.View(model);
                        }

                        ComplaintRegistrationResponse result = complaintPortalService.SaveComplaintApplication(model, UserSession.AEMLComplaintUserSessionContext.ComplaintLevel, true, areaAndPoleDetails);

                        //removing starting 2 Zeros from ComplaintNumber
                        result.ComplaintNumber = result.ComplaintNumber.TrimStart('0');

                        model.ComplaintId = result.ComplaintNumber;

                        bool isRegistered = false;
                        if (string.IsNullOrEmpty(result.Error) && !string.IsNullOrEmpty(result.Message))
                        {
                            ViewBag.IsSuccessful = true;
                            ViewBag.IsSubmit = true;

                            if (model.SelectedComplaintCategory == "2" && (model.SelectedComplaintSubCategory == "4" || model.SelectedComplaintSubCategory == "5"))
                            {
                                try
                                {
                                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                                    {
                                        //long ComplaintId = System.Convert.ToInt64(model.ComplaintId);
                                        if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.ComplaintRegistrationNumber == model.ComplaintId))
                                        {
                                            ComplaintPortalRegisteredComplaint existingComplaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == model.ComplaintId).FirstOrDefault();

                                            if (Request.Files["fileTransactionReceipt"] != null)
                                            {
                                                HttpPostedFileBase file = Request.Files["fileTransactionReceipt"];
                                                //HttpPostedFileBase file = Request.Files["fileBankAccountStatement"];
                                                //var file = model.TransactionReceipt;
                                                Stream fs = file.InputStream;
                                                BinaryReader br = new BinaryReader(fs);
                                                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                                string image = Convert.ToBase64String(bytes);

                                                ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[] returnImgObj = SapPiService.Services.RequestHandler.SelfMeterReadingImageUpload(image, file.FileName, "", model.ComplaintId, "327", model.AccountNumber);
                                                if (returnImgObj != null)
                                                {
                                                    if (returnImgObj.Length > 0)
                                                    {
                                                        if (!returnImgObj[0].TYPE.ToString().ToLower().Equals("s"))
                                                        {
                                                            Log.Error("Complaint doc upload : " + model.AccountNumber + "  Error : " + returnImgObj[0].MESSAGE, this);
                                                            existingComplaint.TransactionReceiptSentToSAP = false;
                                                            existingComplaint.TransactionReceiptSentToSAPResponse = returnImgObj[0].MESSAGE;
                                                            //this.ModelState.AddModelError(nameof(model.TransactionReceipt), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TransactionReceipt not registered", "Unable to upload Transaction Receipt, please try again."));
                                                            //model = GetSubcategory(model);
                                                            //return this.View(model);
                                                        }
                                                        else
                                                        {
                                                            existingComplaint.TransactionReceiptSentToSAP = true;
                                                            existingComplaint.TransactionReceiptSentToSAPResponse = returnImgObj[0].MESSAGE;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        existingComplaint.TransactionReceiptSentToSAP = false;
                                                        existingComplaint.TransactionReceiptSentToSAPResponse = "Unable to upload Transaction Receipt";
                                                    }
                                                }
                                                else
                                                {
                                                    existingComplaint.TransactionReceiptSentToSAP = false;
                                                    existingComplaint.TransactionReceiptSentToSAPResponse = "Unable to upload Transaction Receipt";
                                                    //Log.Error("Complaint doc upload : " + model.AccountNumber + "  Error : " + returnImgObj[0].MESSAGE, this);
                                                    //this.ModelState.AddModelError(nameof(model.TransactionReceipt), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TransactionReceipt not registered", "Unable to upload Transaction Receipt, please try again."));
                                                    //model = GetSubcategory(model);
                                                    //return this.View(model);
                                                }
                                            }
                                            if (Request.Files["fileBankAccountStatement"] != null)
                                            {
                                                HttpPostedFileBase file = Request.Files["fileBankAccountStatement"];
                                                //var file = model.BankAccountStatement;
                                                Stream fs = file.InputStream;
                                                BinaryReader br = new BinaryReader(fs);
                                                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                                string image = Convert.ToBase64String(bytes);

                                                ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[] returnImgObj = SapPiService.Services.RequestHandler.SelfMeterReadingImageUpload(image, file.FileName, "", model.ComplaintId, "328", model.AccountNumber);
                                                if (returnImgObj != null)
                                                {
                                                    if (returnImgObj.Length > 0)
                                                    {
                                                        if (!returnImgObj[0].TYPE.ToString().ToLower().Equals("s"))
                                                        {
                                                            Log.Error("Complaint doc upload : " + model.AccountNumber + "  Error : " + returnImgObj[0].MESSAGE, this);
                                                            existingComplaint.BankAccountStatementSentToSAP = false;
                                                            existingComplaint.BankAccountStatementSentToSAPResponse = returnImgObj[0].MESSAGE;
                                                            //this.ModelState.AddModelError(nameof(model.TransactionReceipt), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TransactionReceipt not registered", "Unable to upload Transaction Receipt, please try again."));
                                                            //model = GetSubcategory(model);
                                                            //return this.View(model);
                                                        }
                                                        else
                                                        {
                                                            existingComplaint.BankAccountStatementSentToSAP = true;
                                                            existingComplaint.BankAccountStatementSentToSAPResponse = returnImgObj[0].MESSAGE;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Log.Error("Complaint doc upload : " + model.AccountNumber + "  Error : " + returnImgObj[0].MESSAGE, this);
                                                    existingComplaint.BankAccountStatementSentToSAP = false;
                                                    existingComplaint.BankAccountStatementSentToSAPResponse = "Unable to upload Transaction Receipt";
                                                    //this.ModelState.AddModelError(nameof(model.TransactionReceipt), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TransactionReceipt not registered", "Unable to upload Transaction Receipt, please try again."));
                                                    //model = GetSubcategory(model);
                                                    //return this.View(model);
                                                }
                                            }

                                            dataContext.SubmitChanges();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Error("Error at complaint doc upload " + ex.Message, this);
                                }
                            }

                            string messagetobedisplayed = string.Empty;
                            string CategoryWithSubcategoryName = complaintPortalService.GetComplaintCategorySubCategoryName(model.SelectedComplaintCategory, model.SelectedComplaintSubCategory);

                            if (result.Message.ToLower() == "order created successfully")
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_success", "Dear Consumer, Your Complaint has been registered and your complaint number is {0}. Please use the complaint number for all future correspondence with AEML."), result.ComplaintNumber);
                                isRegistered = true;
                            }
                            else if (result.Message == "REL_SUCCESS" && result.ComplaintStatus == "99")
                            {
                                if (result.ComplaintStatus == "99")
                                {
                                    messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_success", "Dear Consumer, Your Complaint for Complaint Type: {0} has been registered and your complaint number is {1}. Please use the complaint number for all future correspondence with AEML."), CategoryWithSubcategoryName, result.ComplaintNumber);
                                }
                                isRegistered = true;
                            }
                            else if (result.Message == "REL_SUCCESS")
                            {
                                if (result.ComplaintStatus == "91" && result.TATInfo != "0" && result.LTHTInfo == "0")
                                {
                                    messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_openWithTAT", "Dear Consumer, Complaint for Complaint Type: {0} already registered against your CA number {1} and will be resolved shortly."), CategoryWithSubcategoryName, model.AccountNumber, result.TATInfo);
                                }
                                else if (result.ComplaintStatus == "91" && result.TATInfo == "0" && result.LTHTInfo == "0")
                                {
                                    messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_openDelayed", "Dear Consumer, Complaint for Complaint Type: {0} already registered against your CA number {1} is still OPEN. Request you to please call 19122 for further details."), CategoryWithSubcategoryName, model.AccountNumber);
                                }
                                else if (result.ComplaintStatus == "51")
                                {
                                    messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_maintenance", "Dear Consumer, There is a planned preventive maintenance in your area and power is expected to be restored by {0}. We thank you for your patience in the interim."), result.TATInfo);
                                }
                                else if (result.ComplaintStatus == "52")
                                {
                                    messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_breakdown", "Dear Consumer, Power Supply in your area has been affected due to a major breakdown and power is expected to be restored by {0}. We thank you for your patience in the interim."), result.TATInfo);
                                }
                                else if (result.ComplaintStatus == "53")
                                {
                                    messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_cadisconnected", "Dear Consumers, Supply to your premise has been disconnected due to safety issues. Request you to please call 19122 for further details."), model.AccountNumber);
                                }
                                else if (result.ComplaintStatus == "90")
                                {
                                    messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_closed", "Dear Consumer, Complaint for Complaint Type: {0} already registered against your CA number {1} is resolved. Please call 19122 for any further details required."), CategoryWithSubcategoryName, model.AccountNumber);
                                }
                                else if (result.ComplaintStatus == "93")
                                {
                                    messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_cadisconnected", "Dear Consumer, On further investigation on your Complaint no against CA number {0}, we have found the installation at your premise as Faulty. Request you to please call 19122 for further details."), model.AccountNumber);
                                }
                                else
                                {
                                    messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_error", "Your complaint is not registered due to the following reason {0}, {1}, {2}, {3}. Please call 19122 for any further details required."), result.Message, result.ComplaintStatus, result.LTHTInfo, result.TATInfo);
                                }
                            }
                            else if (result.Message == "REL_EXCEPTION")
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_error", "An exception occured and your complaint is not registered. Please call 19122 for any further details required."), result.Message, result.ComplaintStatus, result.LTHTInfo, result.TATInfo);
                            }
                            else if (result.Message == "REL_INVALID_SECURITYKEY")
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/REL_INVALID_SECURITYKEY", "Your complaint is not registered for CA number {0} due to the reason {1}. Please call 19122 fro any further details required."), model.AccountNumber, "REL_INVALID_SECURITYKEY");
                            }
                            else if (result.Message == "REL_ORDER_PRESENT")
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/REL_ORDER_PRESENT", "Dear Consumer, Complaint for {0} Complaint Type already registered against your CA number {1} and will be resolved shortly."), CategoryWithSubcategoryName, model.AccountNumber, result.TATInfo);
                            }
                            else if (result.Message == "REL_PAPERLESS_BILL")
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/REL_PAPERLESS_BILL", "Dear Consumers, As per our records, you have opted for E-bill. You would have received your bill on your registered email address."), model.AccountNumber, "REL_PAPERLESS_BILL");
                            }
                            else if (result.Message == "REL_ORDER_COMPLETED_3DAYS")
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/REL_ORDER_COMPLETED_3DAYS", "Dear Consumer, Your similar Complaint for {0} has been resolved."), model.AccountNumber, result.TATInfo);
                            }
                            else if (result.Message.Contains("REL_Bill_GENERATED_LAST_3_DAYS"))
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/REL_Bill_GENERATED_LAST_3_DAYS", "Dear Consumer, You bill for has been generated on and will be delivered to your registered billing address within 2 working days."), model.AccountNumber, "REL_Bill_GENERATED_LAST_3_DAYS");
                            }
                            else if (result.Message == "REL_ORDER_COMPLETED_2MONTHS")
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/REL_ORDER_COMPLETED_2MONTHS", "Your complaint is not registered for CA number {0} due to the reason {1}. Please call 19122 fro any further details required."), model.AccountNumber, "REL_ORDER_COMPLETED_2MONTHS");
                            }
                            else if (result.Message == "REL_CONSUMPTION_VAR_30")
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/REL_CONSUMPTION_VAR_30", "Your complaint is not registered for CA number {0} due to the reason {1}. Please call 19122 fro any further details required."), model.AccountNumber, "REL_CONSUMPTION_VAR_30");
                            }
                            else if (result.Message == "REL_METER_REPLACED")
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/REL_METER_REPLACED", "Your complaint is not registered for CA number {0} as the mter is replaced. Please call 19122 fro any further details required."), model.AccountNumber, "REL_METER_REPLACED");
                            }
                            else
                            {
                                messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/not registered", "Your complaint is not registered, please try again!." + result.Message));
                            }

                            if (isRegistered)
                            {
                                model.IsRegistered = true;
                                accountNumberValidated.ComplaintId = model.ComplaintId;
                                Session["ComplaintFileRegistrationModel"] = accountNumberValidated;
                                //ViewBag.Message = messagetobedisplayed;
                                //Session["Message"] = messagetobedisplayed;
                                this.Session["UpdateRegisterComplaintMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Success);
                                Session["SuccessfulRegisterComplaint"] = "1";
                            }
                            else
                            {
                                //ViewBag.Message = messagetobedisplayed;
                                //Session["Message"] = messagetobedisplayed;
                                this.Session["UpdateRegisterComplaintMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Info);
                            }
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                            return this.View(model);
                        }
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(SubmitFeedbackYes))
                {
                    if (Session["ComplaintFileRegistrationModel"] != null)
                    {
                        ComplaintFileRegistrationModel accountNumberValidated = (ComplaintFileRegistrationModel)Session["ComplaintFileRegistrationModel"];
                        model.IsRegistered = true;
                        model.ComplaintId = accountNumberValidated.ComplaintId;
                        model.IsReadyToShareFeedback = true;
                        return View(model);
                    }
                    else
                    {
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                        return this.Redirect(item.Url());
                    }
                }
                if (!string.IsNullOrEmpty(SubmitFeedbackNo))
                {
                    if (Session["ComplaintFileRegistrationModel"] != null)
                    {
                        ComplaintFileRegistrationModel accountNumberValidated = (ComplaintFileRegistrationModel)Session["ComplaintFileRegistrationModel"];
                        model.IsRegistered = true;
                        model.ComplaintId = accountNumberValidated.ComplaintId;
                        model.IsReadyToShareFeedback = false;
                        ComplaintPortalService objService = new ComplaintPortalService();
                        objService.InsertcomplaintFeedback(model.ComplaintId, model.AccountNumber, false, model.MobileNumber);
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                        return this.Redirect(item.Url());
                    }
                    else
                    {
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                        return this.Redirect(item.Url());
                    }

                }
                if (!string.IsNullOrEmpty(SubmitFeedback))
                {
                    if (Session["ComplaintFileRegistrationModel"] != null)
                    {
                        ComplaintFileRegistrationModel accountNumberValidated = (ComplaintFileRegistrationModel)Session["ComplaintFileRegistrationModel"];
                        model.IsRegistered = true;
                        model.ComplaintId = accountNumberValidated.ComplaintId;
                        model.IsReadyToShareFeedback = true;
                        ComplaintPortalService objService = new ComplaintPortalService();

                        //Validate feedback values
                        //SAP Integration
                        //Database intergation
                        //Redirect
                        objService.InsertcomplaintFeedback(model.ComplaintId, model.AccountNumber, true, model.MobileNumber, model.OverallExperience, model.ConcernAddressed, model.FeedbackRemarks);
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                        return this.Redirect(item.Url());
                    }
                    else
                    {
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                        return this.Redirect(item.Url());
                    }
                }
                if (!string.IsNullOrEmpty(SubmitFeedbackCancel))
                {
                    if (Session["ComplaintFileRegistrationModel"] != null)
                    {
                        ComplaintFileRegistrationModel accountNumberValidated = (ComplaintFileRegistrationModel)Session["ComplaintFileRegistrationModel"];
                        model.IsRegistered = true;
                        model.ComplaintId = accountNumberValidated.ComplaintId;
                        model.IsReadyToShareFeedback = false;
                        ComplaintPortalService objService = new ComplaintPortalService();
                        objService.InsertcomplaintFeedback(model.ComplaintId, model.AccountNumber, false, model.MobileNumber);
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                        return this.Redirect(item.Url());
                    }
                    else
                    {
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                        return this.Redirect(item.Url());
                    }
                }
                model = GetSubcategory(model);
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.Source);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult CGRFTrackComplaintRevamp()
        {
            TrackComplaintModel model = new TrackComplaintModel();
            ComplaintPortalService objservice = new ComplaintPortalService();

            model.StartDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
            model.EndDate = DateTime.Now.ToString("dd/MM/yyyy");

            model.ComplaintList = objservice.GetCGRFComplaintsByAccountNumber(DateTime.Now.AddDays(-30), DateTime.Now, null);

            return this.View(model);
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult TrackComplaint()
        {
            ComplaintPortalService objservice = new ComplaintPortalService();
            TrackComplaintModel model = new TrackComplaintModel();

            model.ComplaintList = objservice.GetAllcomplaintsFromSAPByCA(GetPrimaryAccountNumber());

            return this.View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult TrackComplaint(TrackComplaintModel model, string Status, string createComplaint = null)
        {
            try
            {
                ComplaintPortalService complaintPortalService = new ComplaintPortalService();
                ComplaintRegistrationResponse retResult = new ComplaintRegistrationResponse();

                if (!string.IsNullOrEmpty(createComplaint))
                {
                    try
                    {
                        retResult = complaintPortalService.SaveEscalateToICRS_M20ComplaintApplication(UserSession.AEMLComplaintUserSessionContext.AccountNumber, model.ExcalationComplaintId, model.ExcalationRemarks);
                        if (retResult.Message.ToLower() == "order created successfully" && retResult.IsRegistered)
                        {
                            var messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit_success", "Dear Consumer, Your Complaint has been registered and your complaint number is {0}. Please use the complaint number for all future correspondence with AEML."), retResult.ComplaintNumber);

                            //ViewBag.Message = messagetobedisplayed;
                            //Session["Message"] = messagetobedisplayed;

                            this.Session["UpdateRegisterComplaintMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Success);
                        }
                        else
                        {
                            var messagetobedisplayed = "Complaint not created.";

                            //ViewBag.Message = messagetobedisplayed;
                            //Session["Message"] = messagetobedisplayed;

                            this.Session["UpdateRegisterComplaintMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Info);
                        }
                    }
                    catch (Exception ex)
                    {
                        retResult.Error = ex.Message;
                        //ViewBag.Message = retResult.Error;
                        //Session["Message"] = retResult.Error;
                        this.Session["UpdateRegisterComplaintMessage"] = new InfoMessage(retResult.Error, InfoMessage.MessageType.Error);
                    }
                }

                //ComplaintPortalService objservice = new ComplaintPortalService();
                //model = new TrackComplaintModel();

                //model.ComplaintList = objservice.GetAllcomplaintsFromSAPByCA(UserSession.AEMLComplaintUserSessionContext.AccountNumber);

                //return this.View(model);

                return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp));
            }
            catch (Exception ex)
            {
                Log.Error("Error at TrackComplaint Post " + ex.Message, this);
            }
            return this.View(model);
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult SubmitSavedComplaints()
        {
            ComplaintPortalService objservice = new ComplaintPortalService();
            TrackComplaintModel model = new TrackComplaintModel();

            //model.StartDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
            //model.EndDate = DateTime.Now.ToString("dd/MM/yyyy");
            //model.SelectedMonth = DateTime.Now.Month.ToString();
            model.SavedComplaintList = objservice.GetSavedComplaint();

            //ComplaintGetCaseDetailsSAP complaintDetails = objservice.FetchcomplaintDetails(UserSession.AEMLComplaintUserSessionContext.AccountNumber, model.SelectedMonth);

            //model.ComplaintList = new List<ComplaintDetails>();
            //if (complaintDetails != null && complaintDetails.Complaints != null && complaintDetails.Complaints.Count() > 0)
            //{
            //    foreach (var com in complaintDetails.Complaints)
            //    {
            //        model.ComplaintList.Add(new ComplaintDetails
            //        {
            //            ComplaintNumber = com.Complaintnumber,
            //            ComplaintStatusName = com.ComplaintStatus,
            //            ComplaintCategory = com.ComplaintType,
            //            ComplaintSubCategory = com.ComplaintSUbtype,
            //            CreatedOnSAP = com.CreatedOn,
            //            ModifiedOnSAP = com.ModifiedOn
            //        });
            //    }
            //}
            return this.View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult SubmitSavedComplaints(TrackComplaintModel model, string Status)
        {
            try
            {
                ComplaintPortalService objservice = new ComplaintPortalService();
                objservice.FetchcomplaintDetails(UserSession.AEMLComplaintUserSessionContext.AccountNumber, "3");
                DateTime startDate = (DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                DateTime endDate = (DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                model.ComplaintList = objservice.GetComplaintsByAccountNumber(startDate, endDate, Status);

                return this.View(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error at TrackComplaint Post " + ex.Message, this);
            }
            return this.View(model);
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public FileResult DownloadComplaintFile(string id)
        {
            try
            {
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    var fileToDownload = dbcontext.ComplaintPortalRegisteredComplaints.Where(i => i.Id == System.Convert.ToInt32(id)).FirstOrDefault();

                    if (fileToDownload.DocumentData != null)
                        return File(fileToDownload.DocumentData.ToArray(), fileToDownload.DocumentContentType, fileToDownload.DocumentFileName);
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

        [RedirectUnAuthenticatedComplaintPortal]
        public FileResult DownloadComplaintSupportingDocumentFile(string id, string complaintId)
        {
            try
            {
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    //if user is an admin or the complaint is related to logged in account number 
                    if (UserSession.AEMLComplaintUserSessionContext.IsAdmin || dbcontext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == System.Convert.ToInt32(complaintId) && c.AccountNumber == ComplaintPortalService.FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber)))
                    {
                        var fileToDownload = dbcontext.ComplaintPortalCGRFComplaintDocuments.Where(i => i.Id == System.Convert.ToInt32(id) && i.ComplaintNumber == System.Convert.ToInt32(complaintId)).FirstOrDefault();

                        if (fileToDownload.DocumentData != null)
                            return File(fileToDownload.DocumentData.ToArray(), fileToDownload.DocumentContentType, fileToDownload.DocumentName);
                        else
                            return null;
                    }
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                Log.Error("Error at DownloadComplaintSupportingDocumentFile:" + ex.Message, this);
            }
            return null;
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public FileResult DownloadComplaintMOM(string id, string complaintId)
        {
            try
            {
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    //if user is an admin or the complaint is related to logged in account number 
                    if (UserSession.AEMLComplaintUserSessionContext.IsAdmin || dbcontext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == System.Convert.ToInt32(complaintId) && c.AccountNumber == ComplaintPortalService.FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber)))
                    {
                        var fileToDownload = dbcontext.ComplaintPortalCGRFComplaintHearingSchedules.Where(i => i.Id == System.Convert.ToInt32(id) && i.ComplaintId == System.Convert.ToInt32(complaintId)).FirstOrDefault();

                        if (fileToDownload.MOMDocumentData != null)
                            return File(fileToDownload.MOMDocumentData.ToArray(), fileToDownload.MOMDocumentContentType, fileToDownload.MOMFileName);
                        else
                            return null;
                    }
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                Log.Error("Error at DownloadComplaintMOM:" + ex.Message, this);
            }
            return null;
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult CGRFFileComplaintRevamp(string complaintNumber = null, string esc = null)
        {
            try
            {
                UserSession.AEMLComplaintUserSessionContext.ComplaintLevel = "CGRF";
                ComplaintPortalService objservice = new ComplaintPortalService();
                CGRFComplaintFileRegistrationModel model = new CGRFComplaintFileRegistrationModel();

                if (!string.IsNullOrEmpty(complaintNumber))
                {
                    if (!string.IsNullOrEmpty(esc) && esc == "1")
                    {
                        //check if the complaint number is valid for this account
                        //if yes fill complaint number and date of complaint
                        //if no ask if this is first complaint /error message

                        //Get all IGR M20 Complaints 
                        List<ComplaintDetails> complaints = new List<ComplaintDetails>();
                        bool isComplaintValid = false;

                        List<ComplaintDetailsIGR> result = SapPiService.Services.RequestHandler.FetchComplaints(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                        if (result != null && result.Count() > 0)
                        {
                            foreach (var com in result)
                            {
                                complaints.Add(new ComplaintDetails
                                {
                                    IsIGR = true,
                                    CreatedOnSAPDateType = DateTime.Parse(com.ERDate.ToString()),
                                    ComplaintNumber = com.AUFNR,
                                    ComplaintStatusCode = com.IPHAS,
                                    ComplaintStatusName = com.Complaint_Status,
                                    ComplaintCategory = com.Complaint_Type,
                                    ComplaintSubCategory = com.ILART,
                                    CreatedOnSAP = com.ERDate, //(DateTime.Parse(com.ERDate.ToString())).ToString("dd/MM/yyyy"),
                                    TATDate = com.GLTRP
                                });
                            }
                        }

                        //if IGR M20 complaint is closed or TAT date is passed then only complaint is validated for CGRF
                        if (complaints.Any(c => c.ComplaintNumber == complaintNumber || c.ComplaintNumber.TrimStart('0') == complaintNumber))
                        {
                            var comp = complaints.FirstOrDefault(c => c.ComplaintNumber == complaintNumber || c.ComplaintNumber.TrimStart('0') == complaintNumber);
                            if (comp.IsIGR == true && comp.ComplaintSubCategory == "M20")
                            {
                                if (comp.ComplaintStatusCode == "3" || Convert.ToDateTime(comp.TATDate) < DateTime.Now)
                                {
                                    isComplaintValid = true;
                                }
                            }
                            if (isComplaintValid)
                            {
                                //proceed
                                //fill complaint number and date of complaint
                                var userDetails1 = SapPiService.Services.RequestHandler.FetchConsumerDetails(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                                var userDetails21 = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                                model = new CGRFComplaintFileRegistrationModel
                                {
                                    LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                    AccountNumber = userDetails1.CANumber,
                                    Address = userDetails1.Street + userDetails1.Street2 + userDetails1.Street3,
                                    City = userDetails1.City,
                                    ConsumerCategory = userDetails1.ConnectionType,
                                    ConsumerName = userDetails1.Name,
                                    EmailId = userDetails1.Email,
                                    MobileNumber = userDetails1.Mobile,
                                    Pincode = userDetails1.PinCode,
                                    ComplaintFromPreviousLevel = complaintNumber,
                                    ComplaintFromPreviousLevelAppliedDate = comp.CreatedOnSAPDateType.ToString("dd/MM/yyyy"),
                                    SelectedConsumerZone = userDetails21.ZoneName,
                                    DivisionName = userDetails21.DivisionName,
                                    SelectedConsumerCategory = userDetails21.TARIFTYP_Ratecategory,
                                    SelectedComplaintCategory = comp.ComplaintCategory,
                                    IsEscalated = true
                                };

                                return View(model);
                            }
                            else
                            {
                                this.Session["UpdateCGRFMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/COmplaintPortal/No CGRf Redirection", "Complaint cannot be escalated to CGRF, please contact administrator!"));
                                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalTrackComplaintPage);
                                return this.Redirect(item.Url());
                            }
                        }
                        else
                        {
                            this.Session["UpdateCGRFMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/COmplaintPortal/No CGRf Redirection", "Complaint cannot be escalated to CGRF, please contact administrator!"));
                            var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalTrackComplaintPage);
                            return this.Redirect(item.Url());
                        }
                    }

                    //if complaint exists and is related to logged in account number or not
                    if (objservice.CheckIfComplaintExists(complaintNumber))
                    {
                        model = objservice.GetCGRFComplaintDetails(complaintNumber);
                        if (!string.IsNullOrEmpty(model.AccountNumber))
                        {
                            if (model.ComplaintStatus != "1")
                                model.IsReadOnly = true;

                            model.IsComplaintFromPreviousLevelValid = true;
                            return View(model);
                        }
                        else
                        {
                            this.Session["UpdateCGRFMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/COmplaintPortal/No complaint", "Complaint Does not exists or not related to this account."), InfoMessage.MessageType.Info);
                            var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalTrackComplaintPage);
                            return this.Redirect(item.Url());
                        }
                    }
                    else
                    {
                        this.Session["UpdateCGRFMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/COmplaintPortal/No complaint", "Complaint Does not exists or not related to this account."), InfoMessage.MessageType.Info);
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalTrackComplaintPage);
                        return this.Redirect(item.Url());
                    }

                    //if saved - open in editable mode
                    //if submitted - open in read only mode
                    //if closed - open in read only mode with status and comments
                }

                var userDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                var userDetails2 = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(UserSession.AEMLComplaintUserSessionContext.AccountNumber);

                model = new CGRFComplaintFileRegistrationModel
                {
                    LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName,
                    AccountNumber = userDetails.CANumber,
                    Address = userDetails.Street + userDetails.Street2 + userDetails.Street3,
                    City = userDetails.City,
                    ConsumerCategory = userDetails.ConnectionType,
                    ConsumerName = userDetails.Name,
                    EmailId = userDetails.Email,
                    MobileNumber = userDetails.Mobile,
                    Pincode = userDetails.PinCode,
                    SelectedConsumerZone = userDetails2.ZoneName,
                    SelectedConsumerCategory = userDetails2.TARIFTYP_Ratecategory,
                    DivisionName = userDetails2.DivisionName
                };

                return View(model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.Source);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult CGRFFileComplaintRevamp(CGRFComplaintFileRegistrationModel model, string ValidateComplaintNumber = null, string SaveAsDraft = null, string SubmitApplication = null, string Resubmit = null, string Rejoinder = null, string OrderReviewRequest = null)
        {
            try
            {
                
                ComplaintPortalService complaintPortalService = new ComplaintPortalService();
                var userDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(UserSession.AEMLComplaintUserSessionContext.AccountNumber);

                if (!string.IsNullOrEmpty(ValidateComplaintNumber))
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
                    //to do -validate complaint number
                    //Get all IGR M20 Complaints 
                    ComplaintPortalService objservice = new ComplaintPortalService();
                    List<ComplaintDetails> complaints = new List<ComplaintDetails>();
                    string complaintNumberInput = model.ComplaintFromPreviousLevel;
                    bool isComplaintValid = false;

                    complaints = objservice.GetAllcomplaintsFromSAPByCA(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                    var comp = complaints.FirstOrDefault(c => c.ComplaintNumber == complaintNumberInput || c.ComplaintNumber.TrimStart('0') == complaintNumberInput);

                    if (comp != null)
                    {
                        //For M20 Complaints
                        //1. Closed - valid
                        //2. TAT Passed - Valid
                        //3. not cloased and TAT not passed - Message with TAT
                        if (comp.IsIGR == true && comp.ComplaintSubCategory == "M20")
                        {
                            if (comp.ComplaintStatusCode == "3" || Convert.ToDateTime(comp.TATDate) < DateTime.Now)
                            {
                                isComplaintValid = true;
                            }
                            else if (comp.ComplaintStatusCode != "3" && Convert.ToDateTime(comp.TATDate) >= DateTime.Now)
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintFromPreviousLevel), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ComplaintFromPreviousLevel not valid CGRF TAT", "Dear Consumer, Your complaint number " + comp.ComplaintNumber + " raised with us is in process and will be attended by " + DateTime.Parse(comp.TATDate).ToString("dd.MM.yyyy") + ". We appreciate your patience in the interim."));
                                model.IsComplaintFromPreviousLevelValid = false;
                                return this.View(model);
                            }
                        }
                        //For M24 and M14 - Others or Billing 
                        else if (comp.IsIGR == true && comp.ComplaintSubCategory != "M20")
                        {
                            if (comp.ComplaintStatusCode == "3")
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintFromPreviousLevel), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ComplaintFromPreviousLevel not valid CGRF TAT", "Dear Consumer, Your complaint number " + comp.ComplaintNumber + " raised with us is already closed, for more details please go to ICRS section. In case of any queries, please contact us on 19122 or write to us on helpdesk.mumbaielectricity@adani.com."));
                                model.IsComplaintFromPreviousLevelValid = false;
                                return this.View(model);
                            }
                            else
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintFromPreviousLevel), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ComplaintFromPreviousLevel not valid CGRF TAT", "Dear Consumer, Your complaint number " + comp.ComplaintNumber + " raised with us is in process and will be attended by " + DateTime.Parse(comp.TATDate).ToString("dd.MM.yyyy") + ". We appreciate your patience in the interim."));
                                model.IsComplaintFromPreviousLevelValid = false;
                                return this.View(model);
                            }
                        }
                        else //non IGR complaint
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintFromPreviousLevel), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ComplaintFromPreviousLevel not valid CGRF TAT", "Dear Consumer, Your complaint number " + comp.ComplaintNumber + " raised with us is in process and will be attended shortly. We appreciate your patience in the interim."));
                            model.IsComplaintFromPreviousLevelValid = false;
                            return this.View(model);
                        }

                        if (isComplaintValid)
                        {
                            try
                            {
                                if (!model.IsEscalated && (string.IsNullOrEmpty(model.ComplaintFromPreviousLevelAppliedDate) || (DateTime.ParseExact(model.ComplaintFromPreviousLevelAppliedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy-MM-dd") != comp.CreatedOnSAP))
                                {
                                    this.ModelState.AddModelError(nameof(model.ComplaintFromPreviousLevelAppliedDate), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ComplaintFromPreviousLevel wrong date", "Please select correct date."));

                                    return this.View(model);
                                }
                            }
                            catch
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintFromPreviousLevelAppliedDate), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ComplaintFromPreviousLevel wrong date", "Please select correct date."));

                                return this.View(model);
                            }

                            var isEscalatedDetails = objservice.IsEscalatedToCGRFAlready(UserSession.AEMLComplaintUserSessionContext.AccountNumber, model.ComplaintFromPreviousLevel);

                            if (!string.IsNullOrEmpty(isEscalatedDetails))
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintFromPreviousLevel), string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ComplaintFromPreviousLevel already escalated to CGRF", "Dear Consumer, your complaint number ref {0} is already escalated to CGRF and you can track the status in CGRF."), isEscalatedDetails));

                                return this.View(model);
                            }

                            var userDetails2 = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                            CGRFComplaintFileRegistrationModel model1 = new CGRFComplaintFileRegistrationModel
                            {
                                LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                AccountNumber = userDetails.CANumber,
                                Address = userDetails.Street + userDetails.Street2 + userDetails.Street3,
                                City = userDetails.City,
                                ConsumerName = userDetails.Name,
                                EmailId = userDetails.Email,
                                MobileNumber = userDetails.Mobile,
                                Pincode = userDetails.PinCode,
                                DivisionName = userDetails2.DivisionName,
                                ReasonToApply = model.ReasonToApply,
                                //ZoneName = complaintDetails.ZoneName,
                                ComplaintFromPreviousLevel = model.ComplaintFromPreviousLevel,
                                //RemarksFromPreviousLevel = complaintDetails.RemarksFromPreviousLevel,
                                //SelectedComplaintCategory = complaintDetails.SelectedComplaintCategory,
                                //ComplaintDescriptionFromPreviousLevel = complaintDetails.ComplaintDescription,
                                ComplaintFromPreviousLevelAppliedDate = model.ComplaintFromPreviousLevelAppliedDate,
                                SelectedConsumerCategory = userDetails2.TARIFTYP_Ratecategory,
                                SelectedConsumerZone = userDetails2.ZoneName
                            };
                            model1.IsComplaintFromPreviousLevelValid = true;
                            return View(model1);
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintFromPreviousLevel), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ComplaintFromPreviousLevel not valid CGRF", "Dear Consumer, Kindly provide your existing complaint number raised with us earlier to escalate your concern with CGRF. In case of any queries, please contact us on 19122 or write to us on helpdesk.mumbaielectricity@adani.com."));
                            model.IsComplaintFromPreviousLevelValid = false;
                            return this.View(model);
                        }
                    }
                    else
                    {

                        this.ModelState.AddModelError(nameof(model.ComplaintFromPreviousLevel), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ComplaintFromPreviousLevel not found valid CGRF", "Dear Consumer, This Complaint number does not exists, Kindly provide your existing complaint number raised with us earlier to escalate your concern with CGRF. In case of any queries, please contact us on 19122 or write to us on helpdesk.mumbaielectricity@adani.com."));
                        model.IsComplaintFromPreviousLevelValid = false;
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(SaveAsDraft))
                {
                    //validate schedule a form 
                    HttpPostedFileBase scheduleAFormFile = Request.Files["fileComplaintDocScheduleA"];
                    if (scheduleAFormFile != null && scheduleAFormFile.ContentLength > 0)
                    {
                        //check only word and pdf and 10 mb
                        if (!complaintPortalService.ValidateScheduleAFile(scheduleAFormFile))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid file."));
                            return this.View(model);
                        }
                    }

                    //validate supporting documents 
                    IList<HttpPostedFileBase> supportingDocuments = Request.Files.GetMultiple("fileComplaintSupportingDocs");
                    foreach (var sdFile in supportingDocuments)
                    {
                        if (sdFile != null && sdFile.ContentLength > 0)
                        {
                            //check only image and pdf and 10 mb
                            if (!complaintPortalService.ValidateComplaintDocument(sdFile))
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid files. " + sdFile.FileName + " is not valid."));
                                return this.View(model);
                            }
                        }
                    }

                    string registrationNumber = complaintPortalService.SaveCGRFComplaintApplication(model, scheduleAFormFile, supportingDocuments, false);

                    if (!string.IsNullOrEmpty(registrationNumber))
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;
                        
                        string messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSave", "Your complaint is saved successfully. The complaint can be checked via the Track Complaints Section on the Home screen with Complaint Reference No. {0}."), registrationNumber);
                        
                        ViewBag.Message = messagetobedisplayed;
                        this.Session["UpdateCGRFMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Success);
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp);
                        return RedirectPermanent(item.Url());
                        
                        
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                    //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.AfterLoginPageComplaintPortal));
                }
                if (!string.IsNullOrEmpty(SubmitApplication))
                {
                    //validate schedule a form 
                    HttpPostedFileBase scheduleAFormFile = Request.Files["fileComplaintDocScheduleA"];
                    if (scheduleAFormFile != null && scheduleAFormFile.ContentLength > 0)
                    {
                        //check only word and pdf and 10 mb
                        if (!complaintPortalService.ValidateScheduleAFile(scheduleAFormFile))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid file."));
                            return this.View(model);
                        }
                    }

                    //validate supporting documents 
                    IList<HttpPostedFileBase> supportingDocuments = Request.Files.GetMultiple("fileComplaintSupportingDocs");
                    foreach (var sdFile in supportingDocuments)
                    {
                        if (sdFile != null && sdFile.ContentLength > 0)
                        {
                            //check only image and pdf and 10 mb
                            if (!complaintPortalService.ValidateComplaintDocument(sdFile))
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid files. " + sdFile.FileName + " is not valid."));
                                return this.View(model);
                            }
                        }
                    }

                    string registrationNumber = complaintPortalService.SaveCGRFComplaintApplication(model, scheduleAFormFile, supportingDocuments, true);

                    //sap integration
                    //show message with complaint registration number
                    //SMS to the user
                    if (!string.IsNullOrEmpty(registrationNumber))
                    {
                        string TATDuration = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TAT/CGRFApplictionSubmit", "30 business days");

                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        //to do  - sap complaint number to be passed here
                        string messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit", "Your complaint is registered successfully. The complaint can be checked via the Track Complaints Section on the Home screen with Complaint Reference No. {0}."), registrationNumber);

                        ViewBag.Message = messagetobedisplayed;

                        this.Session["UpdateCGRFMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Success);

                        //SMS and Email to the user
                        bool isEmailSent = complaintPortalService.SendSMSandEmailtoConsumer(model.MobileNumber, model.EmailId, model.AccountNumber, registrationNumber, TATDuration, (int)ComplaintPortalService.ActionType.SubmitCGRF);

                        //Intimation of complaint will be sent to Secretary through e-mail 
                        bool isSentEmail = complaintPortalService.SendSMSandEmailToSecretary(model.AccountNumber, registrationNumber, TATDuration, (int)ComplaintPortalService.ActionType.Submit);

                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp);
                        return RedirectPermanent(item.Url());
                        //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.AfterLoginPageComplaintPortal));
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(Resubmit))
                {
                    //validate schedule a form 
                    HttpPostedFileBase scheduleAFormFile = Request.Files["fileComplaintDocScheduleAResubmit"];
                    if (scheduleAFormFile != null && scheduleAFormFile.ContentLength > 0)
                    {
                        //check only word and pdf and 10 mb
                        if (!complaintPortalService.ValidateScheduleAFile(scheduleAFormFile))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid file."));
                            return this.View(model);
                        }
                    }

                    string registrationNumber = complaintPortalService.ResubmitCGRFComplaintApplication(model.ComplaintId, scheduleAFormFile);

                    //sap integration
                    //show message with complaint registration number
                    //SMS to the user
                    if (!string.IsNullOrEmpty(registrationNumber))
                    {
                        string TATDuration = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TAT/CGRFApplictionSubmit", "30 business days");

                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        var complaintDetails = complaintPortalService.GetCGRFComplaintDetails(model.ComplaintId);

                        //to do  - sap complaint number to be passed here
                        string messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnReSubmit", "Your complaint is resubmitted successfully. The complaint can be checked via the Track Complaints Section on the Home screen with Complaint Reference No. {0}."), registrationNumber);

                        ViewBag.Message = messagetobedisplayed;

                        this.Session["UpdateCGRFMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Success);

                        //SMS and email to the user

                        bool isEmailSent = complaintPortalService.SendSMSandEmailtoConsumer(complaintDetails.MobileNumber, complaintDetails.EmailId, complaintDetails.AccountNumber, registrationNumber, TATDuration, (int)ComplaintPortalService.ActionType.Resubmission);

                        //Intimation of complaint will be sent to Secretary through e-mail 
                        bool isSentEmail = complaintPortalService.SendSMSandEmailToSecretary(complaintDetails.AccountNumber, registrationNumber, TATDuration, (int)ComplaintPortalService.ActionType.Resubmission);

                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp);
                        return RedirectPermanent(item.Url());
                        //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.AfterLoginPageComplaintPortal));
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(Rejoinder))
                {
                    //validate word and pdf documents 
                    IList<HttpPostedFileBase> rejoinderDocuments = Request.Files.GetMultiple("fileComplaintDocRejoinder");
                    foreach (var sdFile in rejoinderDocuments)
                    {
                        if (sdFile != null && sdFile.ContentLength > 0)
                        {
                            if (!complaintPortalService.ValidateComplaintDocument(sdFile))
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid files. " + sdFile.FileName + " is not valid."));
                                return this.View(model);
                            }
                        }
                    }

                    string registrationNumber = complaintPortalService.RejoinderByComplainantApplication(model.ComplaintId, rejoinderDocuments);

                    //sap integration
                    //show message with complaint registration number
                    //SMS to the user
                    if (!string.IsNullOrEmpty(registrationNumber))
                    {
                        string TATDuration = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TAT/CGRFApplictionSubmit", "30 business days");

                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        //to do  - sap complaint number to be passed here
                        string messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnReSubmit", "Rejoinder is submitted successfully."), registrationNumber);

                        ViewBag.Message = messagetobedisplayed;

                        this.Session["UpdateCGRFMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Success);

                        //SMS and email to the secretary
                        bool isEmailSent = complaintPortalService.SendSMSandEmailToSecretary(model.AccountNumber, registrationNumber, TATDuration, (int)ComplaintPortalService.ActionType.Rejoinder);

                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp);
                        return RedirectPermanent(item.Url());
                        //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.AfterLoginPageComplaintPortal));
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(OrderReviewRequest))
                {
                    //validate word and pdf documents 
                    IList<HttpPostedFileBase> orderReviewRequestDocuments = Request.Files.GetMultiple("fileComplaintOrderReviewRequest");
                    foreach (var odFile in orderReviewRequestDocuments)
                    {
                        if (odFile != null && odFile.ContentLength > 0)
                        {
                            if (!complaintPortalService.ValidateComplaintDocument(odFile))
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid files. " + odFile.FileName + " is not valid."));
                                return this.View(model);
                            }
                        }
                    }

                    string registrationNumber = complaintPortalService.OrderReviewRequestForComplaint(model, orderReviewRequestDocuments);

                    //sap integration
                    //show message with complaint registration number
                    //SMS to the user
                    if (!string.IsNullOrEmpty(registrationNumber))
                    {
                        string TATDuration = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TAT/CGRFApplictionSubmit", "30 business days");

                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnOrderReviewRequest", "Order Review Request is submitted successfully."), registrationNumber);

                        ViewBag.Message = messagetobedisplayed;

                        this.Session["UpdateCGRFMessage"] = new InfoMessage(messagetobedisplayed, InfoMessage.MessageType.Success);

                        //SMS and email on order review request
                        bool isEmailSent = complaintPortalService.SendSMSandEmailOnOrderReviewRequest(model.AccountNumber, registrationNumber, TATDuration, (int)ComplaintPortalService.ActionType.Review);

                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp);
                        return RedirectPermanent(item.Url());
                        //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.AfterLoginPageComplaintPortal));
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                //when category changes, documents needs to be fetched and display again
                if (model.ComplaintId != null)
                {
                    var complaintDocDetails = complaintPortalService.GetCGRFComplaintDocumentsOnFormPost(model.ComplaintId);
                    model.IsDocumentUploaded = complaintDocDetails.IsDocumentUploaded;
                    model.DocumentName = complaintDocDetails.DocumentName;
                    model.ComplaintSupportingDocuments = complaintDocDetails.ComplaintSupportingDocuments;
                }
                if (model.ConsumerName != null)
                {
                    model.IsComplaintFromPreviousLevelValid = true;
                }
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.Source);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalHomePageRevamp);
                return this.Redirect(item.Url());
            }
        }

        [RedirectUnAuthenticatedComplaintPortal]
        public ActionResult CGRFAdminFileComplaint(string complaintNumber = null)
        {
            try
            {
                //to do - make it dynamic
                UserSession.AEMLComplaintUserSessionContext.ComplaintLevel = "CGRF";
                ComplaintPortalService objservice = new ComplaintPortalService();
                CGRFComplaintFileRegistrationModel model = new CGRFComplaintFileRegistrationModel();

                if (!string.IsNullOrEmpty(complaintNumber))
                {
                    model = objservice.GetCGRFComplaintDetails(complaintNumber);
                    model.AdminRemarks = null;
                    if (string.IsNullOrEmpty(model.AccountNumber))
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/COmplaintPortal/admin No complaint", "Complaint Does not exists."));
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return this.Redirect(item.Url());
                    }
                    else
                    {
                        model.IsComplaintFromPreviousLevelValid = true;
                        return View(model);
                    }
                }

                else if (UserSession.AEMLComplaintUserSessionContext.IsAdmin && UserSession.AEMLComplaintUserSessionContext.AdminRole.ToLower() == "secretary")
                {
                    //admin apply on behalf of consumer, ask account number and continue
                    var userDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                    model = new CGRFComplaintFileRegistrationModel
                    {
                        LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName
                    };

                    return View(model);
                }
                else
                {
                    var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                    return this.Redirect(item.Url());
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.Source);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult CGRFAdminFileComplaint(CGRFComplaintFileRegistrationModel model, string GetUserDetails = null, string SaveAsDraft = null, string SubmitApplication = null, string ApproveApplication = null, string ReSubmitApplication = null, string NodelReply = null, string UpdateDetails = null, string ScheduleHearing = null, string HearingMOMUpdload = null, string HearingMOMReUpdload = null, string CloseComplaint = null, string ClosingDocUpdload = null, string ForwardDetails = null, string OrderReviewRequest = null)
        {
            try
            {
                if (!UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                {
                    var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                    return this.Redirect(item.Url());
                }
                ComplaintPortalService complaintPortalService = new ComplaintPortalService();
                var userDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);
                var userDetails2 = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.AccountNumber);
                if (!string.IsNullOrEmpty(GetUserDetails))
                {
                    if (!string.IsNullOrEmpty(model.ComplaintFromPreviousLevel))
                    {
                        //string complaintNumberInput = model.ComplaintFromPreviousLevel;
                        //var complaintDetails = complaintPortalService.ValidateComplaintFromPreviousLevel(model.ComplaintFromPreviousLevel, model.AccountNumber);
                        //if (complaintDetails != null)
                        //{
                        model = new CGRFComplaintFileRegistrationModel
                        {
                            LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName,
                            AccountNumber = userDetails.CANumber,
                            Address = userDetails.Street + userDetails.Street2 + userDetails.Street3,
                            City = userDetails.City,
                            ConsumerName = userDetails.Name,
                            EmailId = userDetails.Email,
                            MobileNumber = userDetails.Mobile,
                            Pincode = userDetails.PinCode,
                            DivisionName = userDetails2.DivisionName,
                            ComplaintFromPreviousLevel = model.ComplaintFromPreviousLevel,
                            //RemarksFromPreviousLevel = complaintDetails.RemarksFromPreviousLevel,
                            //ComplaintDescriptionFromPreviousLevel = complaintDetails.ComplaintDescription,
                            SelectedConsumerCategory = userDetails2.TARIFTYP_Ratecategory,
                            SelectedConsumerZone = userDetails2.ZoneName
                        };
                        model.IsComplaintFromPreviousLevelValid = true;
                        return View(model);
                        //}
                    }
                    if (!string.IsNullOrEmpty(model.AccountNumber))
                    {
                        model = new CGRFComplaintFileRegistrationModel
                        {
                            //ComplaintFromPreviousLevel = complaintDetails.ComplaintRegistrationNumber,
                            //RemarksFromPreviousLevel = complaintDetails.RemarksFromPreviousLevel,
                            //ComplaintDescriptionFromPreviousLevel = complaintDetails.ComplaintDescription,
                            //SelectedConsumerCategory = complaintDetails.SelectedConsumerCategory
                            LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName,
                            AccountNumber = userDetails.CANumber,
                            Address = userDetails.Street + userDetails.Street2 + userDetails.Street3,
                            City = userDetails.City,
                            ConsumerCategory = userDetails.ConnectionType,
                            ConsumerName = userDetails.Name,
                            EmailId = userDetails.Email,
                            MobileNumber = userDetails.Mobile,
                            Pincode = userDetails.PinCode,
                            SelectedConsumerCategory = userDetails2.TARIFTYP_Ratecategory,
                            SelectedConsumerZone = userDetails2.ZoneName
                        };
                        model.IsComplaintFromPreviousLevelValid = true;
                        return View(model);
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintFromPreviousLevel), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/Account number not valid", "Account number is not valid."));
                        model.IsComplaintFromPreviousLevelValid = false;
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(SaveAsDraft))
                {
                    //validate schedule a form 
                    HttpPostedFileBase scheduleAFormFile = Request.Files["fileComplaintDocScheduleA"];
                    if (scheduleAFormFile != null && scheduleAFormFile.ContentLength > 0)
                    {
                        //check only word and pdf and 10 mb
                        if (!complaintPortalService.ValidateScheduleAFile(scheduleAFormFile))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid file."));
                            return this.View(model);
                        }
                    }

                    //validate supporting documents 
                    IList<HttpPostedFileBase> supportingDocuments = Request.Files.GetMultiple("fileComplaintSupportingDocs");
                    foreach (var sdFile in supportingDocuments)
                    {
                        if (sdFile != null && sdFile.ContentLength > 0)
                        {
                            //check only image and pdf and 10 mb
                            if (!complaintPortalService.ValidateComplaintDocument(sdFile))
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid files. " + sdFile.FileName + " is not valid."));
                                return this.View(model);
                            }
                        }
                    }

                    string registrationNumber = complaintPortalService.SaveCGRFComplaintApplication(model, scheduleAFormFile, supportingDocuments, false);

                    if (!string.IsNullOrEmpty(registrationNumber))
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit", "Your complaint is saved successfully. The complaint can be checked via the Track Complaints Section on the Home screen with Complaint Reference No. {0}."), registrationNumber);

                        ViewBag.Message = messagetobedisplayed;

                        Session["Message"] = messagetobedisplayed;
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                    //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.AfterLoginPageComplaintPortal));
                }
                if (!string.IsNullOrEmpty(SubmitApplication))
                {
                    //validate schedule a form 
                    HttpPostedFileBase scheduleAFormFile = Request.Files["fileComplaintDocScheduleA"];
                    if (scheduleAFormFile != null && scheduleAFormFile.ContentLength > 0)
                    {
                        //check only word and pdf and 10 mb
                        if (!complaintPortalService.ValidateScheduleAFile(scheduleAFormFile))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid file."));
                            return this.View(model);
                        }
                    }

                    //validate supporting documents 
                    IList<HttpPostedFileBase> supportingDocuments = Request.Files.GetMultiple("fileComplaintSupportingDocs");
                    foreach (var sdFile in supportingDocuments)
                    {
                        if (sdFile != null && sdFile.ContentLength > 0)
                        {
                            //check only word and pdf and 10 mb
                            if (!complaintPortalService.ValidateComplaintDocument(sdFile))
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid files. " + sdFile.FileName + " is not valid."));
                                return this.View(model);
                            }
                        }
                    }

                    string registrationNumber = complaintPortalService.SaveCGRFComplaintApplication(model, scheduleAFormFile, supportingDocuments, true);

                    //sap integration
                    //show message with complaint registration number
                    //SMS to the user
                    if (!string.IsNullOrEmpty(registrationNumber))
                    {
                        string TATDuration = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TAT/CGRFApplictionSubmit", "30 business days");

                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit", "Your complaint is registered successfully. The complaint can be checked via the Track Complaints Section on the Home screen with Complaint Reference No. {0}."), registrationNumber);

                        ViewBag.Message = messagetobedisplayed;

                        Session["Message"] = messagetobedisplayed;

                        //SMS and email to the user
                        //Intimation of complaint will be sent to Secretary through e-mail 
                        bool isEmailSent = complaintPortalService.SendSMSandEmailtoConsumer(model.MobileNumber, model.EmailId, model.AccountNumber, registrationNumber, TATDuration, (int)ComplaintPortalService.ActionType.SubmitCGRF);

                        bool isSentEmail = complaintPortalService.SendSMSandEmailToSecretary(model.AccountNumber, registrationNumber, TATDuration, (int)ComplaintPortalService.ActionType.Submit);

                        //var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalCGRFHomePage);
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(ApproveApplication))
                {
                    bool result = complaintPortalService.UpdateComplaintStatus(model.ComplaintId, model.AdminRemarks, (int)ComplaintPortalService.ComplaintStatus.Approved, ComplaintPortalService.ComplaintStatus.Approved.ToString());
                    //sap integration
                    //show message with complaint registration number
                    //SMS to the user
                    if (result)
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        //to do  - sap complaint number to be passed here
                        string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit", "Complaint is approved successfully.");

                        ViewBag.Message = messagetobedisplayed;

                        Session["Message"] = messagetobedisplayed;

                        //Notification will go to Consumer/Nodal Officer/CGRF Members
                        bool isEmailSent = complaintPortalService.SendSMSandEmailForApproval(model.AccountNumber, model.ComplaintRegistrationNumber, "", (int)ComplaintPortalService.ActionType.Approved);

                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in approval", "Complaint is not approved, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(ReSubmitApplication))
                {
                    bool result = complaintPortalService.UpdateComplaintStatus(model.ComplaintId, model.AdminRemarks, (int)ComplaintPortalService.ComplaintStatus.Resubmit, ComplaintPortalService.ComplaintStatus.Resubmit.ToString());

                    //show message with complaint registration number
                    //SMS to the user
                    if (result)
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        //to do  - sap complaint number to be passed here
                        string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSubmit", "Complaint is rejected for resubmission of Grievance.");

                        ViewBag.Message = messagetobedisplayed;

                        Session["Message"] = messagetobedisplayed;

                        //to do send intimation to consumer
                        bool isEmailSent = complaintPortalService.SendSMSandEmailtoConsumer(model.MobileNumber, model.EmailId, model.AccountNumber, model.ComplaintRegistrationNumber, "", (int)ComplaintPortalService.ActionType.Resubmit);

                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/Error Rejection for Resubmission of Grievance", "Error in rejection for Resubmission of Grievance!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(UpdateDetails))
                {
                    //validate schedule a form 
                    HttpPostedFileBase scheduleAFormFile = Request.Files["fileComplaintDocScheduleA"];
                    if (scheduleAFormFile != null && scheduleAFormFile.ContentLength > 0)
                    {
                        //check only word and pdf and 10 mb
                        if (!complaintPortalService.ValidateScheduleAFile(scheduleAFormFile))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid file."));
                            return this.View(model);
                        }
                    }

                    //validate supporting documents 
                    IList<HttpPostedFileBase> supportingDocuments = Request.Files.GetMultiple("fileComplaintSupportingDocs");
                    foreach (var sdFile in supportingDocuments)
                    {
                        if (sdFile != null && sdFile.ContentLength > 0)
                        {
                            //check only word and pdf and 10 mb
                            if (!complaintPortalService.ValidateComplaintDocument(sdFile))
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid files. " + sdFile.FileName + " is not valid."));
                                return this.View(model);
                            }
                        }
                    }

                    bool result = complaintPortalService.UpdateCGRFComplaintApplication(model, scheduleAFormFile, supportingDocuments);

                    if (result)
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSave", "Application is updated successfully. The application can be viewed and submitted via the Track Application Section.");

                        ViewBag.Message = messagetobedisplayed;

                        Session["Message"] = messagetobedisplayed;
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePage);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in update", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(NodelReply))
                {
                    //validate supporting documents 
                    IList<HttpPostedFileBase> nodelReplyDocuments = Request.Files.GetMultiple("fileComplaintDocNodelAdmin");
                    foreach (var sdFile in nodelReplyDocuments)
                    {
                        if (sdFile != null && sdFile.ContentLength > 0)
                        {
                            //check only image and pdf and 10 mb
                            if (!complaintPortalService.ValidateComplaintDocument(sdFile))
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid files. " + sdFile.FileName + " is not valid."));
                                return this.View(model);
                            }
                        }
                    }

                    string result = complaintPortalService.ZonelAdminReplyComplaintApplication(model.ComplaintId, nodelReplyDocuments, model.AdminRemarks);

                    if (!string.IsNullOrEmpty(result))
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnNodalReplySubmit", "Nodal Reply is submitted successfully.");

                        ViewBag.Message = messagetobedisplayed;


                        //Notification will go to Consumer/Nodal Officer/CGRF Members
                        bool isEmailSent = complaintPortalService.SendSMSandEmailForNodalReply(model.ComplaintId, (int)ComplaintPortalService.ActionType.NodalReply);

                        Session["Message"] = messagetobedisplayed;
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in update", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(ScheduleHearing))
                {
                    bool isReviewRequest = model.ComplaintStatus == ((int)ComplaintPortalService.ComplaintStatus.Review).ToString() ? true : false;
                    bool result = complaintPortalService.ScheduleHearingCGRFComplaintApplication(model.ComplaintId, model.ComplaintHearingSelectedDate);

                    if (result)
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSave", "Hearing is scheduled successfully.");

                        ViewBag.Message = messagetobedisplayed;

                        Session["Message"] = messagetobedisplayed;

                        var complaintDetails = complaintPortalService.GetCGRFComplaintDetails(model.ComplaintId);

                        //Notification will go to Consumer
                        bool isEmailSent = complaintPortalService.SendSMSandEmailtoConsumer(complaintDetails.MobileNumber, complaintDetails.EmailId, complaintDetails.AccountNumber, complaintDetails.ComplaintRegistrationNumber, "", (int)ComplaintPortalService.ActionType.HearingScheduled, complaintDetails.CGRFCaseNumber);

                        //to secretary
                        isEmailSent = complaintPortalService.SendSMSandEmailToSecretary(complaintDetails.AccountNumber, complaintDetails.ComplaintRegistrationNumber, "", (int)ComplaintPortalService.ActionType.HearingScheduled);

                        //Notification will go to Consumer
                        isEmailSent = complaintPortalService.SendEmailToMembers(complaintDetails.AccountNumber, complaintDetails.ComplaintRegistrationNumber, "", (int)ComplaintPortalService.ActionType.HearingScheduled);

                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in update", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(HearingMOMUpdload))
                {
                    //validate schedule a form 
                    HttpPostedFileBase complaintHearingMOMFile = Request.Files["fileComplaintHearingMOM"];
                    if (complaintHearingMOMFile != null && complaintHearingMOMFile.ContentLength > 0)
                    {
                        //check only image and pdf and 10 mb
                        if (!complaintPortalService.ValidateComplaintDocument(complaintHearingMOMFile))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid file."));
                            return this.View(model);
                        }
                    }

                    string result = complaintPortalService.HearingMOMUploadCGRFComplaintApplication(model.ComplaintId, complaintHearingMOMFile);

                    if (!string.IsNullOrEmpty(result))
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnNodalReplySubmit", "MOM uploaded successfully.");

                        ViewBag.Message = messagetobedisplayed;

                        //to do : Notifications

                        Session["Message"] = messagetobedisplayed;
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in update", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(HearingMOMReUpdload))
                {
                    //validate schedule a form 
                    HttpPostedFileBase complaintHearingMOMFile = Request.Files["fileComplaintHearingMOMReupload"];
                    if (complaintHearingMOMFile != null && complaintHearingMOMFile.ContentLength > 0)
                    {
                        //check only image and pdf and 10 mb
                        if (!complaintPortalService.ValidateComplaintDocument(complaintHearingMOMFile))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid file."));
                            return this.View(model);
                        }
                    }

                    string result = complaintPortalService.HearingMOMReUploadCGRFComplaintApplication(model.ComplaintId, complaintHearingMOMFile);

                    if (!string.IsNullOrEmpty(result))
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnNodalReplySubmit", "MOM reuploaded successfully.");

                        ViewBag.Message = messagetobedisplayed;

                        //to do : Notifications

                        Session["Message"] = messagetobedisplayed;
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in update", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(CloseComplaint))
                {
                    HttpPostedFileBase complaintFinalDoc = Request.Files["fileComplainClosingDoc"];
                    if (complaintFinalDoc != null && complaintFinalDoc.ContentLength > 0)
                    {
                        //check only image and pdf and 10 mb
                        if (!complaintPortalService.ValidateComplaintDocument(complaintFinalDoc))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid file."));
                            return this.View(model);
                        }
                    }

                    bool result = complaintPortalService.CloseCGRFComplaintApplication(model.ComplaintId, model.AdminRemarks, complaintFinalDoc);

                    if (result)
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnClose", "Complaint is Closed successfully.");

                        ViewBag.Message = messagetobedisplayed;

                        Session["Message"] = messagetobedisplayed;

                        var complaintDetails = complaintPortalService.GetCGRFComplaintDetails(model.ComplaintId);

                        //Notification will go to Consumer
                        bool isEmailSent = complaintPortalService.SendSMSandEmailtoConsumer(complaintDetails.MobileNumber, complaintDetails.EmailId, complaintDetails.AccountNumber, complaintDetails.ComplaintRegistrationNumber, "", (int)ComplaintPortalService.ActionType.Closed, complaintDetails.CGRFCaseNumber);

                        //to secretary
                        isEmailSent = complaintPortalService.SendSMSandEmailToSecretary(complaintDetails.AccountNumber, complaintDetails.ComplaintRegistrationNumber, "", (int)ComplaintPortalService.ActionType.Closed);

                        //Notification will go to Consumer
                        isEmailSent = complaintPortalService.SendEmailToMembers(complaintDetails.AccountNumber, complaintDetails.ComplaintRegistrationNumber, "", (int)ComplaintPortalService.ActionType.Closed);

                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in close", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(ClosingDocUpdload))
                {
                    HttpPostedFileBase complaintFinalDoc = Request.Files["fileComplainClosingDoc"];
                    if (complaintFinalDoc != null && complaintFinalDoc.ContentLength > 0)
                    {
                        //check only image and pdf and 10 mb
                        if (!complaintPortalService.ValidateComplaintDocument(complaintFinalDoc))
                        {
                            this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid file."));
                            return this.View(model);
                        }
                    }


                    bool result = complaintPortalService.ClosingFinalDocumentUpload(model.ComplaintId, complaintFinalDoc);

                    if (result)
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnSave", "Final document is reuploaded successfully.");

                        ViewBag.Message = messagetobedisplayed;

                        Session["Message"] = messagetobedisplayed;
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in update", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                if (!string.IsNullOrEmpty(ForwardDetails))
                {
                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        if (model.ComplaintId != null)
                        {
                            long existingComplaintId = System.Convert.ToInt64(model.ComplaintId);
                            //create new complaint forward item
                            model.Id = Guid.NewGuid();
                            CGRFForwardMail obj = new CGRFForwardMail
                            {
                                Id = model.Id,
                                EmailId = model.ForwardDetailsEmailTo,
                                CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                ComplainId = existingComplaintId,
                                CreatedDate = DateTime.Now
                            };
                            dataContext.CGRFForwardMails.InsertOnSubmit(obj);
                            dataContext.SubmitChanges();
                        }
                    }

                    string isEmailSent = complaintPortalService.SendForwardEmailToConsumers(model.ComplaintId, model.Id.ToString());

                    if (isEmailSent == "1")
                    {
                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/ForwardDetails", "Complaint details are forwarded successfully.");

                        ViewBag.Message = messagetobedisplayed;

                        Session["Message"] = messagetobedisplayed;
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.ComplaintId))
                        {
                            model = complaintPortalService.GetCGRFComplaintDetails(model.ComplaintId);
                            model.AdminRemarks = null;
                            if (string.IsNullOrEmpty(model.AccountNumber))
                            {
                                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/errorforwardingcomplaintdetails", "error in forwarding complaint details, please try again!") + isEmailSent);
                                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                                return this.Redirect(item.Url());
                            }
                            else
                            {
                                model.IsComplaintFromPreviousLevelValid = true;
                                this.ModelState.AddModelError(nameof(model.AdminRemarks), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/errorforwardingcomplaintdetails", "error in forwarding complaint details, please try again!") + isEmailSent);
                                return this.View(model);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(OrderReviewRequest))
                {
                    //validate word and pdf documents 
                    IList<HttpPostedFileBase> orderReviewRequestDocuments = Request.Files.GetMultiple("fileComplaintOrderReviewRequest");
                    foreach (var odFile in orderReviewRequestDocuments)
                    {
                        if (odFile != null && odFile.ContentLength > 0)
                        {
                            if (!complaintPortalService.ValidateComplaintDocument(odFile))
                            {
                                this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/file upload", "Please upload valid files. " + odFile.FileName + " is not valid."));
                                return this.View(model);
                            }
                        }
                    }

                    string registrationNumber = complaintPortalService.OrderReviewRequestForComplaint(model, orderReviewRequestDocuments);

                    //sap integration
                    //show message with complaint registration number
                    //SMS to the user
                    if (!string.IsNullOrEmpty(registrationNumber))
                    {
                        string TATDuration = DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/TAT/CGRFApplictionSubmit", "30 business days");

                        ViewBag.IsSuccessful = true;
                        ViewBag.IsSubmit = true;

                        string messagetobedisplayed = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/PopupMessageOnOrderReviewRequest", "Order Review Request is submitted successfully."), registrationNumber);

                        ViewBag.Message = messagetobedisplayed;

                        Session["Message"] = messagetobedisplayed;

                        var complaintDetails = complaintPortalService.GetCGRFComplaintDetails(model.ComplaintId);

                        //SMS and email on order review request
                        bool isEmailSent = complaintPortalService.SendSMSandEmailOnOrderReviewRequest(complaintDetails.AccountNumber, registrationNumber, TATDuration, (int)ComplaintPortalService.ActionType.Review);

                        //var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalCGRFHomePage);
                        var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalAdminHomePageRevamp);
                        return RedirectPermanent(item.Url());
                        //return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.AfterLoginPageComplaintPortal));
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(model.ComplaintDescription), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/error in save", "Complaint is not registered, please try again!"));
                        return this.View(model);
                    }
                }
                //when category changes, documents needs to be fetched and display again
                if (model.ComplaintId != null)
                {
                    var complaintDocDetails = complaintPortalService.GetCGRFComplaintDocumentsOnFormPost(model.ComplaintId);
                    model.IsDocumentUploaded = complaintDocDetails.IsDocumentUploaded;
                    model.DocumentName = complaintDocDetails.DocumentName;
                    model.ComplaintSupportingDocuments = complaintDocDetails.ComplaintSupportingDocuments;
                }
                if (model.ConsumerName != null)
                {
                    model.IsComplaintFromPreviousLevelValid = true;
                }
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.Source);
                var item = Context.Database.GetItem(Templates.ComplaintPortal.ComplaintPortalLoginPageRevamp);
                return this.Redirect(item.Url());
            }
        }

        //Method: To data table
        //Used for Convert List to Datatable
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        //Method: DatatableToCSV
        //Used for data table to csv and download CSV
        public void DatatableToCSV(DataTable dtDataTable)
        {
            string fileName = "Complaints_" + DateTime.Now.ToShortDateString();
            Stopwatch stw = new Stopwatch();
            stw.Start();
            StringBuilder sb = new StringBuilder();
            //Column headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sb.Append(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append(Environment.NewLine);
            //Column Values
            foreach (DataRow dr in dtDataTable.Rows)
            {
                sb.AppendLine(string.Join(",", dr.ItemArray));
            }

            stw.Stop();
            //Download File
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/csv";
            Response.Output.Write(sb);
            Response.Flush();
            Response.End();
        }

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
    }
}
