using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.Repositories;
using Sitecore.Feature.Accounts.Services;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Sitecore.Foundation.Alerts.Models;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Attributes;
using System.Text.RegularExpressions;
using SapPiService.Domain;

using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Fields;
using System.IO;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using SI_Contactlog_WebService_website;
using System.Globalization;
using SI_DMUPLDMETRREADService;
using Sitecore.Feature.Accounts.SessionHelper;
using Sitecore.Data.Items;
using CaptchaMvc.HtmlHelpers;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using Sitecore.Feature.Accounts.Models.ElectricityRevamp;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Drawing;
using paytm;
using System.Text;
using Sitecore.Feature.Accounts.Helper;
using System.Security.Cryptography;
using ClientsideEncryption;
using RestSharp;

namespace Sitecore.Feature.Accounts.Controllers
{
    [CookieTemperingRedirectNotFound]
    public class AccountsRevampController : Controller
    {
        public AccountsRevampController(IAccountRepository accountRepository, INotificationService notificationService, IAccountsSettingsService accountsSettingsService, IGetRedirectUrlService getRedirectUrlService, IUserProfileService userProfileService, IFedAuthLoginButtonRepository fedAuthLoginRepository, IUserProfileProvider userProfileProvider, IPaymentService paymentService, IDbAccountService dbAccountService)
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

        #region ||** My Account **||
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult MyAccountRevamp()
        {
            return this.View();
        }

        [RedirectUnauthenticatedCookieTempered]
        public ActionResult LeftPanelRevamp()
        {
            var userType = UserTypes.Standard;
            if (SessionHelper.UserSession.UserSessionContext != null)
                userType = SessionHelper.UserSession.UserSessionContext.userType;
            return this.View("LeftPanelRevamp", new EditProfile() { UserType = userType.ToLower() });
        }

        #endregion

        #region ||** My Profile **||
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult MyProfileRevamp()
        {
            return this.View();
        }

        #endregion

        #region ||** Edit Main Content **||

        [RedirectUnauthenticatedCookieTempered]
        public ActionResult EditMainContentRevamp()
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
        [ValidateAntiForgeryToken]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult EditMainContentRevamp(EditProfile profile, string submit = null, string ValidateOTP = null)
        {

            var loggedinuserprofile = this.UserProfileService.GetProfile(Context.User);

            if (loggedinuserprofile.AccountNumber != profile.AccountNumber)
            {
                Sitecore.Diagnostics.Log.Info("Error at EditMainContentRevamp loggedinuserprofile.AccountNumber != profile.AccountNumber:" + loggedinuserprofile.AccountNumber + ":" + profile.AccountNumber, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile update error", "Profile was not successfully updated, please try again!"), InfoMessage.MessageType.Error);

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
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/ValidDateofBirth", "Please enter valid date in dd/mm/yyyy format."), InfoMessage.MessageType.Error);
                    return this.Redirect(this.Request.RawUrl);
                }

            }

            if (!string.IsNullOrEmpty(profile.MobileNumber))
            {
                if (submit != null)
                {

                    ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                    if (!reCaptchaResponse.success)
                    {
                        ModelState.AddModelError(nameof(profile.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue."));
                        return this.View(profile);
                    }

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
                        //if (!this.Request.Url.Host.Equals("electricity.dev.local"))
                        //{
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/Accounts/Settings/SMS API for Profile update", "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=You have initiated a request to update mobile number for account no. {1}, OTP for this request is: {2}&intflag=false"), profile.MobileNumber, profile.AccountNumber, generatedotp);
                        //var apiurl = string.Format("http://sms2.murlidhar.biz/sendSMS?username=murlidharbizOTP&message=Welcome to Adani Electricity, OTP is {0}&sendername=SOCITY&smstype=TRANS&numbers=+{1}&apikey=9714ac15-f5d3-47be-8e04-47c869d078bd", generatedotp, profile.MobileNumber);
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Error("OTP Api call success for EditMainContentRevamp", this);
                            this.ModelState.AddModelError(nameof(profile.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTP", "OTP Sent."));
                            profile.isOTPSent = true;
                            return this.View(profile);
                        }
                        else
                        {
                            Log.Error("OTP Api call failed for EditMainContentRevamp", this);
                            this.ModelState.AddModelError(nameof(profile.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                            profile.isOTPSent = false;
                            return this.View(profile);
                        }
                        //}
                        //else
                        //{
                        //    Log.Error("OTP Api call success for registration", this);
                        //    this.ModelState.AddModelError(nameof(profile.MobileNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTP", "OTP Sent."));
                        //    profile.isOTPSent = true;
                        //    return this.View(profile);
                        //}

                    }
                    catch (Exception ex)
                    {
                        Log.Error($"{0}", ex, this);
                        Log.Error("OTP Api call failed for EditMainContentRevamp", this);
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
                    string generatedOTP = string.Empty;
                    //if (!this.Request.Url.Host.Equals("electricity.dev.local"))
                    //{
                    generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(profile.MobileNumber, profile.AccountNumber, "UpdateProfile");
                    //}
                    //else
                    //{
                    //    generatedOTP = "12345";
                    //}

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
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile Success", "Profile was successfully updated"), InfoMessage.MessageType.Success);
                        else
                        {
                            Sitecore.Diagnostics.Log.Error("Profile update Error at " + MethodBase.GetCurrentMethod().Name + ":" + result1, this);
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile update error", "Profile was not successfully updated, please try again!"), InfoMessage.MessageType.Error);
                        }
                        return this.Redirect(this.Request.RawUrl);
                    }
                }
            }

            //this.UserProfileService.SaveProfile(Context.User.Profile, profile);
            //string result = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(profile.AccountNumber, profile.MobileNumber, profile.PhoneNumber, profile.Email, null);
            //if (result == "success")
            //    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile Success", "Profile was successfully updated"));
            //else
            //{
            //    Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + result, this);
            //    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Profile update error", "Profile was not successfully updated, please try again!"));
            //}
            return this.Redirect(this.Request.RawUrl);

        }

        #endregion

        [RedirectUnauthenticatedCookieTempered]
        public ActionResult SwitchAccountBodyRevamp()
        {

            #region New Code

            #region variable declaration
            var model = new SwitchAccount();
            var availableitemsinDB = _dbAccountService.GetAccountListbyusername(Context.User.Profile.UserName);
            List<KeyValuePair<string, string>> AccountNumberList = new List<KeyValuePair<string, string>>();
            string primaryAccountNumber = string.Empty;
            List<SwitchAccountComponent> switchAccountsComponetlst = new List<SwitchAccountComponent>();
            int count = 0;
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
                    count = 1;
                    foreach (var item in selecteditems)
                    {
                        //if (!string.Equals(primaryAccountNumber, item.AccountNumber))
                        //{
                        if (!string.IsNullOrEmpty(item.AccountNumber))
                        {
                            SwitchAccountComponent switchAccountComponentItem = new SwitchAccountComponent();
                            switchAccountComponentItem.AccountItemId = item.Id.ToString();
                            switchAccountComponentItem.AccountNumber = item.AccountNumber;
                            if (count <= 10)
                            {
                                switchAccountComponentItem.AccountHolderName = SapPiService.Services.RequestHandler.FetchDetail(item.AccountNumber) != null ? SapPiService.Services.RequestHandler.FetchDetail(item.AccountNumber).Name : "";
                            }
                            AccountNumberList.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.AccountNumber));
                            switchAccountsComponetlst.Add(switchAccountComponentItem);
                            count += 1;
                        }
                        // }
                    }
                }
                model.SwitchAccountList = switchAccountsComponetlst;
                model.AccountList = AccountNumberList;
                model.LoginName = this.UserProfileService.GetLoginName();
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
                    count = 1;
                    foreach (var secondaryAccountItemId in MultipleAccountItemIdList)
                    {
                        //if (!string.Equals(primaryAccountItemId, secondaryAccountItemId))
                        //{
                        secondaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(secondaryAccountItemId);
                        if (!string.IsNullOrEmpty(secondaryAccountNumber))
                        {
                            SwitchAccountComponent switchAccountComponentItem = new SwitchAccountComponent();
                            switchAccountComponentItem.AccountItemId = secondaryAccountItemId;
                            switchAccountComponentItem.AccountNumber = secondaryAccountNumber;
                            if (count <= 10)
                            {
                                switchAccountComponentItem.AccountHolderName = SapPiService.Services.RequestHandler.FetchDetail(secondaryAccountNumber) != null ? SapPiService.Services.RequestHandler.FetchDetail(secondaryAccountNumber).Name : "";
                            }


                            AccountNumberList.Add(new KeyValuePair<string, string>(secondaryAccountItemId, secondaryAccountNumber));
                            switchAccountsComponetlst.Add(switchAccountComponentItem);
                            count += 1;
                        }
                        //}
                    }
                }
                model.SwitchAccountList = switchAccountsComponetlst;
                model.AccountList = AccountNumberList;
                model.LoginName = this.UserProfileService.GetLoginName();
                model.MasterAccountNumber = GetPrimaryAccountNumber();
            }
            #endregion

            return this.View("SwitchAccountBodyRevamp", model);
        }

        [RedirectUnauthenticatedCookieTempered]
        public ActionResult SwitchAccountHomePageRevamp()
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
                    foreach (var item in selecteditems.Take(10))
                    {
                        if (!string.Equals(primaryAccountNumber, item.AccountNumber))
                        {
                            if (!string.IsNullOrEmpty(item.AccountNumber))
                            {
                                AccountNumberList.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.AccountNumber));
                            }

                        }
                    }
                }
                model.AccountList = AccountNumberList;
                model.LoginName = this.UserProfileService.GetLoginName();
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
                    foreach (var secondaryAccountItemId in MultipleAccountItemIdList.Take(10))
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
                model.LoginName = this.UserProfileService.GetLoginName();
                model.MasterAccountNumber = GetPrimaryAccountNumber();
            }
            #endregion

            return this.View("SwitchAccountHomePageRevamp", model);
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

        #region Payment History

        [RedirectUnauthenticatedCookieTempered]
        public ActionResult PaymentHistoryRevamp()
        {
            return this.View();
        }

        public ActionResult LoadData_PaymentHistoryRevamp()
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

        [RedirectUnauthenticatedCookieTempered]
        public ActionResult ConsumptionHistoryRevamp()
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

        public ActionResult LoadData_ConsumptionHistoryRevamp()
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
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
            //return this.View(model);
        }

        #endregion


        #region Download / Pay Bill

        public ActionResult LoadData_DownloadPayBillRevamp()
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

            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
            //return View(model);
        }


        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult DownloadPayBillRevamp()
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
        public ActionResult DownloadPayBillRevamp(string AccountNumber, string selectedMonth, string viewBill = null)
        {
            try
            {
                Session["UpdateMessage"] = null;
                if (!string.IsNullOrEmpty(viewBill))
                {
                    try
                    {
                        AccountNumber = AESEncrytDecry.DecryptStringAES(AccountNumber);
                        Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                        var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                        string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.DownloadBillSettings.EncryptionKey].Value; // "B3XbAcGCezTeqfVxWIl4tvNdI";

                        if (string.IsNullOrEmpty(selectedMonth))
                        {
                            Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/DownloadPayBill/DownloadBillError", "Please select a month to download bill!"));
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                        InvoiceHistory billRecords = SapPiService.Services.RequestHandler.FetchInvoiceHistory(AccountNumber);
                        InvoiceLine billToDownload = billRecords.InvoiceLines.Where(i => i.BillMonth == selectedMonth).FirstOrDefault();

                        clsTripleDES objclsTripleDES = new clsTripleDES();
                        String encryptedCANumber = HttpUtility.UrlEncode(objclsTripleDES.Encrypt(billToDownload.AccountNumber, EncryptionKey));
                        String encryptedInvoiceNumber = HttpUtility.UrlEncode(objclsTripleDES.Encrypt(billToDownload.InvoiceNumber, EncryptionKey));

                        billToDownload.InvoiceUrl = "https://iss.adanielectricity.com/VAS/ProcessDownloadPDF.jsp?ENCR=1&ENCRCANO=" + encryptedCANumber + "&ENCRINVNO=" + encryptedInvoiceNumber;


                        System.Net.WebClient client = new System.Net.WebClient();
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        byte[] byteArray = client.DownloadData(billToDownload.InvoiceUrl);
                        Session["filestring"] = byteArray;
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
                        Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/DownloadPayBill/DownloadBillError", "Unable to download your bill, please try again!"));
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
                Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/DownloadPayBill/SendBillError", "Your request is not accepted, please try again!"));
            }
            return Json("0", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ActualPDFRenderingRevamp()
        {
            byte[] fileString = (byte[])Session["filestring"];
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //Response.AppendHeader("Content-Disposition", "inline; filename=" + filename);
            return File(fileString, "application/pdf", filename + ".pdf");
        }

        #endregion



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




        [RedirectUnauthenticatedCookieTempered]
        public ActionResult SwitchAccountInternallyRevamp(string ItemId)
        {

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

                #region ||AEML Revamp Complaint : Added to create AEMLComplaintUserSessionContext in case user switch Account||
                var profile = this.UserProfileService.GetProfile(Context.User);
                UserSession.AEMLComplaintUserSessionContext = new LoginInfoComplaint
                {
                    LoginName = profile.LoginName,
                    AccountNumber = profile.AccountNumber,
                    MobileNumber = profile.MobileNumber,
                    IsRegistered = true,
                    SessionId = SessionHelper.UserSession.UserSessionContext.SessionId.ToString(),
                    IsAuthenticated = true
                };
                #endregion

                this.Session["UpdateRevampMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Switch Account Successfull Heading", "Success"), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Switch Account Successfull", "Done! Account switched successfully!"), InfoMessageRevamp.MessageTypeRevamp.Success, false);
            }
            catch (Exception ex)
            {
                Log.Error("Method SwitchAccountInternally -", ex.Message);
                this.Session["UpdateRevampMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Switch Account Failed Heading", "Failed"), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Switch Account Failure", "Something Went Wrong."), InfoMessageRevamp.MessageTypeRevamp.Success, false);
                return Json(new { data = "0" }, JsonRequestBehavior.AllowGet);
            }
            //return RedirectPermanent(contextUrl);
            return Json(new { data = "1" }, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult ViewPayBillRevamp()
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
                string date1 = string.Empty;
                if (billinghDetails.DateDue == "0000-00-00")
                {
                    date1 = "0000-00-00";
                }
                else
                {
                    CultureInfo culture = new CultureInfo("es-ES");
                    String myDate = billinghDetails.DateDue;
                    DateTime date = DateTime.Parse(myDate, culture);
                    date1 = date.ToString("dd MMM yyyy");
                }
                model = new ViewPayBill()
                {
                    AccountNumber = accountDetails.AccountNumber,// this.UserProfileService.GetAccountNumberfromItem(masterAccount.ToString()),
                    Name = accountDetails.Name,// this.UserProfileService.GetLoginName(),
                    BookNumber = accountDetails.BookNumber,// "505",
                    CycleNumber = accountDetails.CycleNumber,// "05",
                    Zone = accountDetails.ZoneNumber,// "South Central",
                    Address = accountDetails.Address,// "300 288 Shere Punjab SOC Mahakali Caves Rd Andheri E Near Tolani College Mumbai 400067",
                    BillMonth = billinghDetails.BillMonth,// "May-2018",
                    //PaymentDueDate = billinghDetails.DateDue,// "2018-06-16",

                    //PaymentDueDate = (billinghDetails.DateDue == "0000-00-00") ? "0000-00-00" : DateTime.ParseExact(billinghDetails.DateDue, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd MMM yyyy"),
                    PaymentDueDate = date1,
                    DueDateGraterthenFourDays = (billinghDetails.DateDue == "0000-00-00") ? false : (DateTime.ParseExact(billinghDetails.DateDue, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture) > DateTime.Now.AddDays(-4) ? false : true),


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
        [RedirectUnauthenticatedCookieTempered]
        [ValidateRenderingId]
        public ActionResult ViewPayBillRevamp(ViewPayBill model, string Pay_PaymentGateway = null, string Pay_VDSpayment = null, string Pay_Other = null)
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
                        // Captcha
                        if (model.AmountPayable != null && model.AmountPayable.Any(char.IsLetter))
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidAmount", "Invalid amount payable value."));
                            return this.View(model);
                        }
                        else if (System.Convert.ToDecimal(model.AmountPayable) < 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountNegativeValidation", "You have no amount payable value."));
                            return this.View(model);
                        }
                        else if (System.Convert.ToDecimal(Session["Amountpayable"]) >= 100 && System.Convert.ToDecimal(model.AmountPayable) < 100)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Amount not valid", "Minimum Amount payable Value is 100. Please enter valid amount."));
                            return this.View(model);
                        }
                        else if (System.Convert.ToDecimal(model.AmountPayable) == 0 && System.Convert.ToDecimal(model.AdvanceAmmount) <= 0)
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
                    model.PaymentGateway = 2;

                    switch (model.PaymentGateway)
                    {

                        case (int)EnumPayment.GatewayType.BillDesk:
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string RequestHTML = this.PaymentService.BillDeskTransactionRequestAPIRequestPostRevamp(model);
                            return Content(RequestHTML);

                        case (int)EnumPayment.GatewayType.Paytm:
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string outputHTML = this.PaymentService.PaytmTransactionRequestAPIRequestPostRevamp(model);
                            return Content(outputHTML);

                        case (int)EnumPayment.GatewayType.Ebixcash:
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string submittHTML = this.PaymentService.EbixcashTransactionRequestAPIRequestPost(model);
                            return Content(submittHTML);

                        case (int)EnumPayment.GatewayType.ICICIBank:
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string submitHTML = this.PaymentService.ICICITransactionRequestAPIRequestPostRevamp(model);
                            return Content(submitHTML);
                        case (int)EnumPayment.GatewayType.Benow:
                            this.PaymentService.StorePaymentRequestRevamp(model);
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
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            SessionHelper.UserSession.UserSessionContext.OrderId = model.OrderId;
                            Sitecore.Diagnostics.Log.Info("Call to DBSUPITransactionRequestAPIRequestGET", this);
                            string dbsUPIsubmitdata = this.PaymentService.DBSUPITransactionRequestAPIRequestGETRevamp(model);
                            return Content(dbsUPIsubmitdata);
                        case (int)EnumPayment.GatewayType.CITYUPI:

                            this.PaymentService.StorePaymentRequestRevamp(model);
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
                            string cityUPIsubmitdata = this.PaymentService.CityUPITransactionRequestAPIRequestGETRevamp(model);
                            Sitecore.Diagnostics.Log.Info("Call to CityUPITransactionRequestAPIRequestGET dbsUPIsubmitdata:" + cityUPIsubmitdata, this);
                            return Content(cityUPIsubmitdata);
                        case (int)EnumPayment.GatewayType.SafeXPay:
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string RequestHTMLSafeXPay = this.PaymentService.SafeXPayTransactionRequestAPIRequestPost(model);
                            return Content(RequestHTMLSafeXPay);
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
                        if (System.Convert.ToDecimal(model.SecurityDeposit) > 0 && System.Convert.ToDecimal(Session["SecurityDeposit"]) != System.Convert.ToDecimal(model.SecurityDeposit))
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchSD", "You have modified Security Deposit amount Value. Please refresh the page and try again."));
                            return this.View(model);
                        }
                        if (System.Convert.ToDecimal(model.SecurityDeposit) <= 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/SecurityDepositeValidation", "You have no security deposit payable amount."));
                            return this.View(model);
                        }
                    }
                    else if (model.SecurityDepositAmountType == "Partial")
                    {
                        if (System.Convert.ToDecimal(model.SecurityDepositPartial) <= 0)
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
                        string submitHTML = this.PaymentService.ICICITransactionRequestAPIRequestPostRevamp(model);
                        return Content(submitHTML);
                    }
                    else
                    {
                        model.PaymentGateway = 2;
                        this.PaymentService.StorePaymentRequest(model);
                        string RequestHTML = this.PaymentService.BillDeskSDTransactionRequestAPIRequestPostRevamp(model);
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


        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult PaymentSecurityDepositRevamp()
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


                var profile = this.UserProfileService.GetProfile(Context.User);

                var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(profile.AccountNumber);
                var securityAmount = SapPiService.Services.RequestHandler.FetchSecurityDepositAmountDetails(profile.AccountNumber);
                var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(profile.AccountNumber);
                model = new ViewPayBill()
                {
                    MeterNumber = string.Join(",", billinghDetails.MeterNumbers),
                    TariffSlab = billinghDetails.TariffSlab,
                    AccountNumber = accountDetails.AccountNumber,// this.UserProfileService.GetAccountNumberfromItem(masterAccount.ToString()),
                    LoginName = this.UserProfileService.GetLoginName(),
                    BookNumber = accountDetails.BookNumber,// "505",
                    CycleNumber = accountDetails.CycleNumber,// "05",
                    Zone = accountDetails.ZoneNumber,// "South Central",
                    Address = accountDetails.Address,// "300 288 Shere Punjab SOC Mahakali Caves Rd Andheri E Near Tolani College Mumbai 400067",
                    SecurityDeposit = securityAmount.SDAmount.ToString(),// "560.00",
                    SecurityDepositMsg = securityAmount.Message.ToString(),// "560.00",
                    SecurityPaymentList = securitypaymentlist,
                    Mobile = profile.MobileNumber,
                    Email = profile.Email,
                    SecurityDepositAmountType = "Actual"
                };

                Session["securityDespositAmt"] = securityAmount.SDAmount.ToString();
            }
            catch (Exception ex)
            {
                ViewBag.SecurityDesposit = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/API Issue", "There is some issue in fetching your data. Please try after some time.");
                Sitecore.Diagnostics.Log.Error("Error at PaymentSecurityDeposit:" + ex.Message, this);
            }
            return this.View(model);
        }

        [HttpPost]
        [RedirectUnauthenticatedCookieTempered]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentSecurityDepositRevamp(ViewPayBill model)
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
                    if (System.Convert.ToDecimal(model.SecurityDeposit) <= 0)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/SecurityDepositeValidation", "You have no security deposit payable amount."));
                        return this.View(model);
                    }
                }
                else if (model.SecurityDepositAmountType == "Partial")
                {
                    if (System.Convert.ToDecimal(model.SecurityDepositPartial) <= 0)
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
                    string submitHTML = this.PaymentService.ICICITransactionRequestAPIRequestPostRevamp(model);
                    return Content(submitHTML);
                }
                else
                {
                    model.PaymentGateway = 2;
                    this.PaymentService.StorePaymentRequest(model);
                    string submitHTML = this.PaymentService.BillDeskSDTransactionRequestAPIRequestPostRevamp(model);
                    return Content(submitHTML);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at PaymentSecurityDeposit Post:" + ex.Message, this);
            }
            return this.Redirect(this.Request.RawUrl);
        }


        #region Pay VDS Amount
        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult PaymentVDSRevamp()
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
                    decimal minVDSAmount = 0;
                    decimal maxVDSAmount = 0;
                    var gap = 100000 - VDSAmountDetails.ExistingVdsBalance;
                    if (VDSAmountDetails.AverageBillingAmount < 3000)
                    {
                        minVDSAmount = 3000;
                    }
                    else if (VDSAmountDetails.AverageBillingAmount > 3000)
                    {
                        minVDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                    }
                    if (gap >= 0 && gap < System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
                    {
                        minVDSAmount = gap;
                    }
                    maxVDSAmount = gap;
                    var consumerPANGSTDetails = SapPiService.Services.RequestHandler.FetchConsumerPANGSTDetails(profile.AccountNumber);
                    ViewBag.VDSMessage = "Dear Consumer, you need to clear your current outstanding for VDS payments.";
                    var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(profile.AccountNumber);
                    model = new PayVDS()
                    {
                        AccountNumber = profile.AccountNumber,
                        MobileNumber = profile.MobileNumber,
                        EmailAddress = profile.Email,
                        AverageVDSAmount = minVDSAmount,
                        MaxVDSAmount = maxVDSAmount,
                        PANNo = consumerPANGSTDetails.PANNumber,
                        PaymentAmount = minVDSAmount,
                        PaymentMode = paymentMode,
                        LoginName = this.UserProfileService.GetLoginName(),
                        PaymentDueDate = billinghDetails.DateDue,
                        CurrentOutstanding = VDSAmountDetails.CurrentOutstanding,
                        AmountPayable = billinghDetails.AmountPayable.ToString()

                    };
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
                        minVDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                    }
                    if (gap >= 0 && gap < System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
                    {
                        minVDSAmount = gap;
                    }
                    maxVDSAmount = gap;
                    var consumerPANGSTDetails = SapPiService.Services.RequestHandler.FetchConsumerPANGSTDetails(profile.AccountNumber);
                    //decimal VDSAmount = 0;
                    //if (VDSAmountDetails.AverageBillingAmount < 3000)
                    //    VDSAmount = 3000;
                    //else if (VDSAmountDetails.AverageBillingAmount > 10000)
                    //    VDSAmount = 10000;
                    //else if (VDSAmountDetails.AverageBillingAmount > 3000 && VDSAmountDetails.AverageBillingAmount < 10000)
                    //{
                    //    VDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                    //}
                    var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(profile.AccountNumber);
                    model = new PayVDS()
                    {
                        AccountNumber = profile.AccountNumber,
                        MobileNumber = profile.MobileNumber,
                        EmailAddress = profile.Email,
                        AverageVDSAmount = minVDSAmount,
                        MaxVDSAmount = maxVDSAmount,
                        PANNo = consumerPANGSTDetails.PANNumber,
                        PaymentAmount = minVDSAmount,
                        PaymentMode = paymentMode,
                        LoginName = this.UserProfileService.GetLoginName(),
                        PaymentDueDate = billinghDetails.DateDue,
                        CurrentOutstanding = VDSAmountDetails.CurrentOutstanding

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
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult PaymentVDSRevamp(PayVDS model)
        {
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);
                if (model.AccountNumber != profile.AccountNumber)
                {
                    ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidRequest", "Invalid request."));
                    return this.View(model);
                }

                model.AverageVDSAmount = System.Convert.ToDecimal(Session["VDSAmount"]);
                model.MaxVDSAmount = System.Convert.ToDecimal(Session["maxVDSAmount"]);

                if (!this.ModelState.IsValid)
                {
                    return this.View(model);
                }
                else
                {
                    if (string.IsNullOrEmpty(model.PaymentAmount.ToString()) && model.PaymentAmount.ToString().Any(char.IsLetter))
                    {
                        ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidAmount", "Invalid amount payable value."));
                        return this.View(model);
                    }
                    if (System.Convert.ToDecimal(model.PaymentAmount) < 0)
                    {
                        ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value!"));
                        return this.View(model);
                    }
                    if (System.Convert.ToDecimal(model.PaymentAmount) % 500 != 0)
                    {
                        ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value, it should be multiple of ₹500"));
                        return this.View(model);
                    }
                    if (System.Convert.ToDecimal(model.PaymentAmount) < System.Convert.ToDecimal(Session["VDSAmount"]) || System.Convert.ToDecimal(model.PaymentAmount) > System.Convert.ToDecimal(Session["maxVDSAmount"]))
                    {
                        ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value, it should be in between min and max values."));
                        return this.View(model);
                    }

                    if (string.IsNullOrEmpty(model.PANNo))
                    {
                        ModelState.AddModelError("PANNo", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/PAN No", "Please enter a valid PAN No."));
                        return this.View(model);
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$");
                        //If you want both upper case and lower case alphabets.  
                        Match match = regex.Match(model.PANNo);
                        if (!match.Success)
                        {
                            ModelState.AddModelError("PANNo", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/PAN No", "Please enter a valid PAN No."));
                            return this.View(model);
                        }
                    }


                    //if (System.Convert.ToDecimal(model.PaymentAmount) < 0 || System.Convert.ToDecimal(Session["VDSAmount"]) != System.Convert.ToDecimal(model.PaymentAmount))
                    //{
                    //    this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "You have modified VDS Amount Value. Please refresh the page and try again."));
                    //    return this.View(model);
                    //}
                    var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(profile.AccountNumber);
                    if (model != null && !string.IsNullOrEmpty(model.PaymentMode))
                    {
                        var paymodel = new ViewPayBill()
                        {
                            AccountNumber = model.AccountNumber,
                            // AmountPayable = model.AverageVDSAmount.ToString("f2"),
                            AmountPayable = model.PaymentAmount.ToString("f2"),
                            LoginName = model.LoginName,
                            Email = model.EmailAddress,
                            Mobile = model.MobileNumber,
                            PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/VDS", "VDS payment"),
                            Remark = model.PANNo,
                            PaymentGateway = 2,
                            UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/RegisteredUser", "Registered"),
                            CurrencyType = "INR",
                            PaymentDueDate = billinghDetails.DateDue
                        };
                        //if (model.AverageVDSAmount.ToString("f2") == "0.00")
                        //{
                        //    paymodel.AmountPayable = model.CurrentOutstanding.ToString();
                        //}
                        string paymentmode = model.PaymentMode; //CC and Net Banking parameter come from pay and bill page

                        this.PaymentService.StorePaymentRequestRevamp(paymodel);
                        string RequestHTML = this.PaymentService.BillDeskVDSTransactionRequestAPIRequestPostRevamp(paymodel);
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


        [HttpPost]
        // public ActionResult PaymentVDSRevampQuickBill(string AccountNumber,string AmountPayable,string PANNo,string MobileNumber,string EmailAddress)
        public ActionResult PaymentVDSRevampQuickBill(ViewPayBill model)
        {
            try
            {
                decimal AverageVDSAmount = System.Convert.ToDecimal(Session["VDSAmount"]);
                decimal MaxVDSAmount = System.Convert.ToDecimal(Session["maxVDSAmount"]);
                model.LoginName = !string.IsNullOrEmpty(model.LoginName) ? model.LoginName : model.AccountNumber;
                var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(model.AccountNumber);
                if (!string.IsNullOrEmpty(model.AmountPayable))
                {
                    var paymodel = new ViewPayBill()
                    {
                        AccountNumber = model.AccountNumber,
                        // AmountPayable = model.AverageVDSAmount.ToString("f2"),
                        AmountPayable = System.Convert.ToDecimal(model.AmountPayable).ToString("f2"),
                        LoginName = model.LoginName,
                        Email = model.Email,
                        Mobile = model.Mobile,
                        PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/VDS", "VDS payment"),
                        Remark = model.PANNo,
                        PaymentGateway = 2,
                        UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/RegisteredUser", "Registered"),
                        CurrencyType = "INR",
                        PaymentDueDate = billinghDetails.DateDue
                    };
                    //if (model.AverageVDSAmount.ToString("f2") == "0.00")
                    //{
                    //    paymodel.AmountPayable = model.CurrentOutstanding.ToString();
                    //}
                    // string paymentmode = "CC"; //CC and Net Banking parameter come from pay and bill page

                    this.PaymentService.StorePaymentRequest(paymodel);
                    string RequestHTML = this.PaymentService.BillDeskVDSTransactionRequestAPIRequestPostRevamp(paymodel);

                    return Content(RequestHTML);
                }
                // }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at PaymentVDS Post:" + ex.Message, this);
                //return ex.Message.ToString();
            }
            return Json(new { data = this.Request.RawUrl }, JsonRequestBehavior.AllowGet);
            //return this.Redirect(this.Request.RawUrl);
        }





        [HttpGet]
        public void PayBillRevampNew(string AccountNumber, Nullable<int> SecurityDeposit = null)
        {

            ViewBag.VDSMessage = null;
            Nullable<int> AdvanceSecurity = 0;
            ViewPayBill model = new ViewPayBill();
            try
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");

                //  model.AccountNumber = SessionHelper.UserSession.UserSessionContext.AccountNumber;
                if (Request.QueryString["ca_number"] != null)
                {
                    model.AccountNumber = Request.QueryString["ca_number"];
                }
                else
                {
                    if (AccountNumber != null)
                    {
                        model.AccountNumber = AccountNumber;
                    }
                    else
                    {
                        model.AccountNumber = this.UserProfileService.GetAccountNumber(Context.User);
                    }
                }

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
                            minVDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                        }
                        if (gap >= 0 && gap < System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
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
                        //    VDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
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
                    var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);
                    if (consumerDetails.Email != null)
                    {
                        email = consumerDetails.Email;
                    }
                    if (consumerDetails.Mobile != null)
                    {
                        mobile = consumerDetails.Mobile;
                    }
                    decimal amountpayable = billinghDetails.AmountPayable;

                    if (SecurityDeposit > 0)
                    {
                        AdvanceSecurity = SecurityDeposit;
                    }
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
                        ProceedWithEMI = false,
                        AdvanceAmmount = System.Convert.ToDouble(AdvanceSecurity)
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

            this.Redirect("/Payment/pay-your-bill?ca_number=" + model.AccountNumber);
            //return View("/Views/AccountsRevamp/PayBillRevamp.cshtml", model);

        }

        [HttpGet]
        public ActionResult PayBillRevamp(string AccountNumber, Nullable<int> SecurityDeposit = null)
        {
            ModelState.Clear();
            ViewBag.VDSMessage = null;
            Nullable<int> AdvanceSecurity = 0;
            ViewPayBill model = new ViewPayBill();
            if (!string.IsNullOrEmpty(AccountNumber))
            {
                AccountNumber = AESEncrytDecry.DecryptStringAES(AccountNumber);
            }
            try
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                if (Request.QueryString["SecurityDeposit"] != null)
                {
                    try
                    {
                        string check = AESEncrytDecry.DecryptStringAES(Request.QueryString["SecurityDeposit"]);

                    }
                    catch
                    {
                        var item = Context.Database.GetItem("{1E9F3E3D-D6B0-4AE1-A7E6-D1BB2C619E71}");
                        return this.Redirect(item.Url());
                    }
                }

                //  model.AccountNumber = SessionHelper.UserSession.UserSessionContext.AccountNumber;
                if (Request.QueryString["ca_number"] != null)
                {
                    try
                    {
                        model.AccountNumber = AESEncrytDecry.DecryptStringAES(Request.QueryString["ca_number"]);
                    }
                    catch
                    {
                        var item = Context.Database.GetItem("{1E9F3E3D-D6B0-4AE1-A7E6-D1BB2C619E71}");
                        return this.Redirect(item.Url());
                    }
                }
                else
                {
                    if (AccountNumber != null)
                    {
                        model.AccountNumber = AccountNumber;
                    }
                    else
                    {
                        model.AccountNumber = this.UserProfileService.GetAccountNumber(Context.User);
                    }
                }

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
                            minVDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                        }
                        if (gap >= 0 && gap < System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
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
                        //    VDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
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
                    var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);

                    if (!(string.IsNullOrEmpty(consumerDetails.Email)))
                    {
                        email = consumerDetails.Email;
                    }
                    else
                    {
                        if (UserSession.UserSessionContext != null)
                        {
                            var profile = this.UserProfileService.GetProfile(Context.User);
                            email = profile.Email;
                        }
                    }
                    if (consumerDetails.Mobile != null)
                    {
                        mobile = consumerDetails.Mobile;
                    }
                    else
                    {
                        if (UserSession.UserSessionContext != null)
                        {
                            var profile = this.UserProfileService.GetProfile(Context.User);
                            mobile = profile.MobileNumber;
                        }
                    }
                    decimal amountpayable = billinghDetails.AmountPayable;

                    if (SecurityDeposit > 0)
                    {
                        AdvanceSecurity = SecurityDeposit;
                    }
                    string date1 = string.Empty;
                    if (billinghDetails.DateDue == "0000-00-00")
                    {
                        date1 = "0000-00-00";
                    }
                    else
                    {
                        CultureInfo culture = new CultureInfo("es-ES");
                        String myDate = billinghDetails.DateDue;
                        DateTime date = DateTime.Parse(myDate, culture);
                        date1 = date.ToString("dd MMM yyyy");
                    }
                    model = new ViewPayBill()
                    {
                        AccountNumber = accountDetails.AccountNumber,// this.UserProfileService.GetAccountNumberfromItem(masterAccount.ToString()),
                        Name = accountDetails.Name,// this.UserProfileService.GetLoginName(),
                        BookNumber = accountDetails.BookNumber,// "505",
                        CycleNumber = accountDetails.CycleNumber,// "05",
                        Zone = accountDetails.ZoneNumber,// "South Central",
                        Address = accountDetails.Address,// "300 288 Shere Punjab SOC Mahakali Caves Rd Andheri E Near Tolani College Mumbai 400067",
                        BillMonth = billinghDetails.BillMonth,// "May-2018",
                                                              //PaymentDueDate = billinghDetails.DateDue,// "2018-06-16",
                                                              // PaymentDueDate = (billinghDetails.DateDue == "0000-00-00") ? "0000-00-00" : DateTime.ParseExact(billinghDetails.DateDue, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd MMM yyyy"),
                        PaymentDueDate = date1,
                        DueDateGraterthenFourDays = (billinghDetails.DateDue == "0000-00-00") ? false : (DateTime.ParseExact(billinghDetails.DateDue, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture) >= DateTime.Now.AddDays(-4) ? false : true),

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
                        ProceedWithEMI = false,
                        AdvanceAmmount = System.Convert.ToDouble(AdvanceSecurity),
                        AmountPayableshow = billinghDetails.AmountPayable.ToString()

                    };
                    var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
                    var iv = Encoding.UTF8.GetBytes("8080808080808080");
                    Session["Amountpayable"] = amountpayable.ToString();//billinghDetails.AmountPayable.ToString();
                    Session["SecurityDeposit"] = securityAmount.ToString();
                    Session["PaymentVDSAmount"] = minVDSAmount.ToString();
                    Session["maxVDSAmount"] = maxVDSAmount.ToString();
                    Session["PAcheck"] = AESEncrytDecry.EncryptStringToBytes(model.AccountNumber, keybytes, iv);
                }
            }
            catch (Exception ex)
            {
                ViewBag.NoInfo = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/API Issue", "There is some issue in fetching your data. Please try after some time.");
                Sitecore.Diagnostics.Log.Error("Error at PayBill :" + ex.Message, this);
            }
            return this.View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[RedirectUnauthenticatedCookieTempered]
        public ActionResult PayBillRevamp(ViewPayBill model, string paybillforAccount, string Pay_PaymentGateway = null, string Pay_VDSpayment = null, string Pay_Other = null)
        {
            try
            {
                var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
                var iv = Encoding.UTF8.GetBytes("8080808080808080");
                if (Session["PAcheck"] != null)
                {
                    string can = AESEncrytDecry.DecryptStringFromBytes((byte[])Session["PAcheck"], keybytes, iv);
                    if (!can.Equals(model.AccountNumber))
                    {
                        Session["PAcheck"] = null;
                        return this.Redirect("/page-not-found");
                    }
                }

                if (System.Convert.ToDecimal(model.SecurityDeposit) > 0)
                {
                    Pay_Other = "Security Deposit";
                    Pay_PaymentGateway = null;
                }


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
                var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);
                var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(model.AccountNumber);
                var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(model.AccountNumber);

                model.PaymentDueDate = billinghDetails.DateDue;
                model.CycleNumber = accountDetails.CycleNumber;
                model.AmountPayableshow = billinghDetails.AmountPayable.ToString();
                if (string.IsNullOrEmpty(model.Email.Trim()))
                {
                    this.ModelState.AddModelError("Email", DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address"));
                    return this.View(model);

                    //if (!(string.IsNullOrEmpty(consumerDetails.Email)))
                    //{
                    //    model.Email = consumerDetails.Email;
                    //}
                    //else
                    //{
                    //    if (UserSession.UserSessionContext != null)
                    //    {
                    //        var profile = this.UserProfileService.GetProfile(Context.User);
                    //        model.Email = profile.Email;
                    //    }

                    //}
                }
                if (string.IsNullOrEmpty(model.Mobile.Trim()))
                {
                    this.ModelState.AddModelError("Mobile", DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number"));
                    return this.View(model);
                    //if (!(string.IsNullOrEmpty(consumerDetails.Mobile)))
                    //{
                    //    model.Mobile = consumerDetails.Mobile;
                    //}
                    //else
                    //{

                    //    if (UserSession.UserSessionContext != null)
                    //    {
                    //        var profile = this.UserProfileService.GetProfile(Context.User);
                    //        model.Mobile = profile.MobileNumber;
                    //    }
                    //}
                }

                #region billingAmount/vds/otherSecurity

                if (!string.IsNullOrEmpty(Pay_PaymentGateway))
                {
                    ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                    if (!reCaptchaResponse.success)
                    {
                        ModelState.AddModelError("Captcha", DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue."));
                        return this.View(model);
                    }

                    if (!this.ModelState.IsValid)
                    {

                        return this.View(model);
                    }
                    //string accountnumber = SessionHelper.UserSession.UserSessionContext.AccountNumber;

                    string accountnumber = string.Empty;

                    if (!string.IsNullOrEmpty(model.AccountNumber))
                    {
                        accountnumber = model.AccountNumber;
                    }
                    else if (string.IsNullOrEmpty(accountnumber))
                    {
                        accountnumber = SessionHelper.UserSession.UserSessionContext.AccountNumber;
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
                        else if (System.Convert.ToDecimal(model.AmountPayable) < 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountNegativeValidation", "You have no amount payable value."));
                            return this.View(model);
                        }
                        else if (System.Convert.ToDecimal(Session["Amountpayable"]) >= 100 && System.Convert.ToDecimal(model.AmountPayable) < 100)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Amount not valid", "Minimum Amount payable Value is 100. Please enter valid amount."));
                            model.AmountPayable = Session["Amountpayable"].ToString();
                            return this.View(model);
                        }
                        else if (System.Convert.ToDecimal(model.AmountPayable) == 0 && System.Convert.ToDecimal(model.AdvanceAmmount) <= 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AdvanceAmountValidation", "Please enter the advance payment amount that you would like to pay."));
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


                        case (int)EnumPayment.GatewayType.BillDesk:
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string RequestHTML = this.PaymentService.BillDeskTransactionRequestAPIRequestPostRevamp(model);
                            return Content(RequestHTML);


                        case (int)EnumPayment.GatewayType.Paytm:
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string outputHTML = this.PaymentService.PaytmTransactionRequestAPIRequestPostRevamp(model);
                            return Content(outputHTML);
                        case (int)EnumPayment.GatewayType.Benow:
                            this.PaymentService.StorePaymentRequestRevamp(model);
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
                            string BNsubmitdata = this.PaymentService.BENOWTransactionRequestAPIRequestGETRevamp(model);
                            return Content(BNsubmitdata);

                        case (int)EnumPayment.GatewayType.DBSUPI:
                            this.PaymentService.StorePaymentRequestRevamp(model);
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
                            Sitecore.Diagnostics.Log.Info("Call to DBSUPITransactionRequestAPIRequestGETRevamp", this);
                            string dbsUPIsubmitdata = this.PaymentService.DBSUPITransactionRequestAPIRequestGETRevamp(model);
                            Sitecore.Diagnostics.Log.Info("Call to DBSUPITransactionRequestAPIRequestGETRevamp dbsUPIsubmitdata:" + dbsUPIsubmitdata, this);
                            return Content(dbsUPIsubmitdata);

                        case (int)EnumPayment.GatewayType.CITYUPI:
                            this.PaymentService.StorePaymentRequestRevamp(model);
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
                            string cityUPIsubmitdata = this.PaymentService.CityUPITransactionRequestAPIRequestGETRevamp(model);
                            Sitecore.Diagnostics.Log.Info("Call to CityUPITransactionRequestAPIRequestGET dbsUPIsubmitdata:" + cityUPIsubmitdata, this);
                            return Content(cityUPIsubmitdata);

                        case (int)EnumPayment.GatewayType.SafeXPay:
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string RequestHTMLSafeXPay = this.PaymentService.SafeXPayTransactionRequestAPIRequestPost(model);
                            return Content(RequestHTMLSafeXPay);

                        case (int)EnumPayment.GatewayType.CashFree:
                            model.PaymentType = "Bill Paid";
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string ResponseCashFree = CashFreeGenerateOrderId(model);
                            //string RequestHTMLCashFree = "";//this.PaymentService.SafeXPayTransactionRequestAPIRequestPost(model);
                            return Content(ResponseCashFree);

                        default:
                            break;
                    }
                }
                else if (!string.IsNullOrEmpty(Pay_VDSpayment)) //VDS Payment
                {
                    string accountnumber = model.AccountNumber;
                    if (UserSession.UserSessionContext != null)
                    {
                        accountnumber = SessionHelper.UserSession.UserSessionContext.AccountNumber;
                    }
                    if (model.AccountNumber != accountnumber)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidRequest", "Invalid request."));
                        return this.View(model);
                    }
                    model.AverageVDSAmount = System.Convert.ToDecimal(Session["PaymentVDSAmount"]);
                    model.MaxVDSAmount = System.Convert.ToDecimal(Session["maxVDSAmount"]);
                    model.Remark = model.PANNo;
                    model.PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/VDS", "VDS payment");
                    model.LoginName = !string.IsNullOrEmpty(model.LoginName) ? model.LoginName : model.AccountNumber;
                    model.PaymentGateway = 2;

                    if (string.IsNullOrEmpty(model.PaymentVDSAmount.ToString()) && model.PaymentVDSAmount.ToString().Any(char.IsLetter))
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get(" / Accounts/Pay Bill/InvalidAmount", "Invalid amount payable value."));
                        return this.View(model);
                    }
                    if (System.Convert.ToDecimal(model.PaymentVDSAmount) < 0)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value!"));
                        return this.View(model);
                    }
                    if (System.Convert.ToDecimal(model.PaymentVDSAmount) % 500 != 0)
                    {
                        this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchVDS", "Please enter a valid VDS Amount Value, it should be multiple of ₹500"));
                        return this.View(model);
                    }
                    if (System.Convert.ToDecimal(model.PaymentVDSAmount) < System.Convert.ToDecimal(Session["PaymentVDSAmount"]) || System.Convert.ToDecimal(model.PaymentVDSAmount) > System.Convert.ToDecimal(Session["maxVDSAmount"]))
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
                    string RequestHTML = this.PaymentService.BillDeskVDSTransactionRequestAPIRequestPostRevamp(model);
                    return Content(RequestHTML);
                }
                else if (!string.IsNullOrEmpty(Pay_Other)) //Security Desposit
                {
                    string accountnumber = "";

                    if (!string.IsNullOrEmpty(model.AccountNumber))
                    {
                        accountnumber = model.AccountNumber;
                    }
                    else if (string.IsNullOrEmpty(accountnumber))
                    {
                        accountnumber = SessionHelper.UserSession.UserSessionContext.AccountNumber;
                    }

                    model.LoginName = !string.IsNullOrEmpty(model.LoginName) ? model.LoginName : model.AccountNumber;
                    model.PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit");


                    if (model.SecurityDepositAmountType == "Actual")
                    {
                        if (System.Convert.ToDecimal(model.SecurityDeposit) > 0 && System.Convert.ToDecimal(Session["SecurityDeposit"]) != System.Convert.ToDecimal(model.SecurityDeposit))
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/AmountMismatchSD", "You have modified Security Deposit amount Value. Please refresh the page and try again."));
                            return this.View(model);
                        }
                        if (System.Convert.ToDecimal(model.SecurityDeposit) <= 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/SecurityDepositeValidation", "You have no security deposit payable amount."));
                            return this.View(model);
                        }
                    }
                    else if (model.SecurityDepositAmountType == "Partial")
                    {
                        if (System.Convert.ToDecimal(model.SecurityDepositPartial) <= 0)
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/SecurityDepositeValidation", "Please enter true value for security deposit payable amount."));
                            return this.View(model);
                        }
                    }
                    //if (string.IsNullOrEmpty(model.SecurityPayment))
                    //{
                    //    this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Security payment selection required  ", "Please select security Payment option."));
                    //    return this.View(model);
                    //}
                    switch (model.PaymentGateway)
                    {
                        case (int)EnumPayment.GatewayType.BillDesk:
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string RequestHTML = this.PaymentService.BillDeskSDTransactionRequestAPIRequestPostRevamp(model);
                            return Content(RequestHTML);

                        case (int)EnumPayment.GatewayType.SafeXPay:
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string RequestHTMLSafeXPay = this.PaymentService.SafeXPayTransactionRequestAPIRequestPost(model);
                            return Content(RequestHTMLSafeXPay);

                        case (int)EnumPayment.GatewayType.CashFree:
                            model.PaymentType = "Security Deposit";
                            this.PaymentService.StorePaymentRequestRevamp(model);
                            string ResponseCashFree = CashFreeGenerateOrderId(model);
                            //string RequestHTMLCashFree = "";//this.PaymentService.SafeXPayTransactionRequestAPIRequestPost(model);
                            return Content(ResponseCashFree);
                        default:
                            break;
                    }

                    //if (model.SecurityPayment == DictionaryPhraseRepository.Current.Get("/Payment Settings/ICICI", "ICICI"))
                    //{
                    //    model.PaymentGateway = 5;
                    //    this.PaymentService.StorePaymentRequest(model);
                    //    string submitHTML = this.PaymentService.ICICITransactionRequestAPIRequestPostRevamp(model);
                    //    return Content(submitHTML);
                    //}
                    //else
                    //{
                    //    model.PaymentGateway = 2;
                    //    this.PaymentService.StorePaymentRequestRevamp(model);
                    //    string RequestHTML = this.PaymentService.BillDeskSDTransactionRequestAPIRequestPostRevamp(model);
                    //    return Content(RequestHTML);
                    //}


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

        [HttpGet]
        //[RedirectUnauthenticatedCookieTempered]
        public ActionResult QuickPayRevamp()
        {

            var model = new ViewPayBill();
            return this.View(model);
        }





        [HttpGet]
        public ActionResult QuickPayRevampHumb(string AccountNumber, string recaptcha)
        {


            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), recaptcha);
            var model = new ViewPayBill();

            if (!(recaptcha == "Home") && !reCaptchaResponse.success)
            {
                model = new ViewPayBill()
                {
                    Captcha = null,
                    Message = "Please validate captcha to continue"
                };
            }
            else
            {
                var parentItem = Context.Site.GetStartItem().Parent;
                InternalLinkField link = parentItem.Fields[Templates.AccountsSettings.Fields.QuickPayBillPage];

                if (!string.IsNullOrEmpty(AccountNumber))
                {
                    AccountNumber = AESEncrytDecry.DecryptStringAES(AccountNumber);
                }

                var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(AccountNumber);
                var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(AccountNumber);
                var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(AccountNumber);
                var sdDetails = SapPiService.Services.RequestHandler.FetchSecurityDepositAmountDetails(AccountNumber);
                var emiDetails = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(AccountNumber);
                var VDSAmountDetails = SapPiService.Services.RequestHandler.FetchVdsAmount(AccountNumber);
                var consumerPANGSTDetails = SapPiService.Services.RequestHandler.FetchConsumerPANGSTDetails(AccountNumber);


                decimal amountpayable = billinghDetails.AmountPayable;
                if (string.IsNullOrEmpty(accountDetails.CycleNumber) || billinghDetails.BillingStatus == BillingStatus.NoInvoice)
                {
                    model = new ViewPayBill()
                    {
                        AccountNumber = "",
                        Captcha = "Valid"
                    };

                }
                else
                {
                    decimal minVDSAmount = 0;
                    decimal maxVDSAmount = 0;
                    if (VDSAmountDetails.CurrentOutstanding > 0)
                    {

                        var gap = 100000 - VDSAmountDetails.ExistingVdsBalance;
                        if (VDSAmountDetails.AverageBillingAmount < 3000)
                        {
                            minVDSAmount = 3000;
                        }
                        else if (VDSAmountDetails.AverageBillingAmount > 3000)
                        {
                            minVDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                        }
                        if (gap >= 0 && gap < System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
                        {
                            minVDSAmount = gap;
                        }
                        maxVDSAmount = gap;
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
                            minVDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                        }
                        if (gap >= 0 && gap < System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
                        {
                            minVDSAmount = gap;
                        }
                        maxVDSAmount = gap;

                    }
                    Session["VDSAmount"] = minVDSAmount.ToString();
                    Session["maxVDSAmount"] = maxVDSAmount.ToString();

                    string date1 = string.Empty;
                    if (billinghDetails.DateDue == "0000-00-00")
                    {
                        date1 = "0000-00-00";
                    }
                    else
                    {
                        CultureInfo culture = new CultureInfo("es-ES");
                        String myDate = billinghDetails.DateDue;
                        DateTime date = DateTime.Parse(myDate, culture);
                        date1 = date.ToString("dd MMM yyyy");
                    }

                    model = new ViewPayBill()
                    {

                        AccountNumber = accountDetails.AccountNumber,// this.UserProfileService.GetAccountNumberfromItem(masterAccount.ToString()),
                        Name = accountDetails.Name,// this.UserProfileService.GetLoginName(),
                        BookNumber = accountDetails.BookNumber,// "505",
                        CycleNumber = accountDetails.CycleNumber,// "05",
                        Zone = accountDetails.ZoneNumber,// "South Central",
                        Address = accountDetails.Address,// "300 288 Shere Punjab SOC Mahakali Caves Rd Andheri E Near Tolani College Mumbai 400067",
                        BillMonth = billinghDetails.BillMonth,// "May-2018",
                                                              //PaymentDueDate = (billinghDetails.DateDue == "0000-00-00") ? "0000-00-00" : DateTime.ParseExact(billinghDetails.DateDue, "yyyy-mm-dd", CultureInfo.InvariantCulture).ToString("dd MMM yyyy"),
                        PaymentDueDate = date1,
                        DueDateGraterthenFourDays = (billinghDetails.DateDue == "0000-00-00") ? false : (DateTime.ParseExact(billinghDetails.DateDue, "yyyy-mm-dd", CultureInfo.InvariantCulture) > DateTime.Now.AddDays(-4) ? false : true),

                        TariffSlab = billinghDetails.TariffSlab,// "LT-1b",
                        MeterNumber = string.Join(",", billinghDetails.MeterNumbers),// "7587321",
                        UnitsConsumed = billinghDetails.UnitsConsumed,//  "1",
                        TotalCharges = billinghDetails.TotalCharges,// "134.27",
                        CurrentMonthsBills = billinghDetails.CurrentMonthCharge,// "134.69",
                        BroughtForward = billinghDetails.BroughtForward,// "128.26",
                        TotalBillAmount = billinghDetails.TotalBillAmount,//  "246.19",
                        SecurityDeposit = sdDetails.SDAmount.ToString(),// "560.00",
                        SecurityDepositMsg = sdDetails.Message.ToString(),// "560.00",
                        SecurityDepositAmountType = "Actual",                                            //VDSAmount= vdsAmount.ToString(),
                        AmountPayable = amountpayable.ToString(),// billinghDetails.AmountPayable.ToString(),// "250"
                        LoginName = this.UserProfileService.GetLoginName(),

                        EMIEligible = emiDetails.Status == "S" ? true : false,
                        EMIInstallmentAmount = emiDetails.EMIInstallmentAmount,
                        EMIOutstandingAmount = emiDetails.TotalOutstanding,
                        ProceedWithEMI = false,
                        AverageVDSAmount = minVDSAmount,
                        MaxVDSAmount = maxVDSAmount,
                        PANNo = consumerPANGSTDetails.PANNumber,
                        PaymentVDSAmount = VDSAmountDetails.CurrentOutstanding,
                        Mobile = consumerDetails.Mobile,
                        Email = consumerDetails.Email,
                        Captcha = "Valid"
                    };
                    Session["securityDespositAmt"] = sdDetails.SDAmount.ToString();
                }
            }

            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult QuickPayRevampNew(string AccountNumber, string recaptcha)
        {

            var parentItem = Context.Site.GetStartItem().Parent;
            InternalLinkField link = parentItem.Fields[Templates.AccountsSettings.Fields.QuickPayBillPage];

            if (string.IsNullOrWhiteSpace(recaptcha))
            {
                //ModelState.AddModelError(nameof(loginInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                //return this.View(loginInfo);
            }
            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), recaptcha);
            var model = new ViewPayBill();
            if (!reCaptchaResponse.success)
            {
                model = new ViewPayBill()
                {
                    Captcha = "Invalid"

                };
                return Json(new { data = model }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(AccountNumber))
            {

                AccountNumber = AESEncrytDecry.DecryptStringAES(AccountNumber);
            }

            var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(AccountNumber);
            var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(AccountNumber);
            var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(AccountNumber);
            var sdDetails = SapPiService.Services.RequestHandler.FetchSecurityDepositAmountDetails(AccountNumber);
            var emiDetails = SapPiService.Services.RequestHandler.ValidateCAForEMIOption(AccountNumber);
            var VDSAmountDetails = SapPiService.Services.RequestHandler.FetchVdsAmount(AccountNumber);
            var consumerPANGSTDetails = SapPiService.Services.RequestHandler.FetchConsumerPANGSTDetails(AccountNumber);


            decimal amountpayable = billinghDetails.AmountPayable;
            if (string.IsNullOrEmpty(accountDetails.CycleNumber) || billinghDetails.BillingStatus == BillingStatus.NoInvoice)
            {
                model = new ViewPayBill()
                {
                    AccountNumber = ""
                };

            }
            else
            {
                decimal minVDSAmount = 0;
                decimal maxVDSAmount = 0;
                if (VDSAmountDetails.CurrentOutstanding > 0)
                {

                    var gap = 100000 - VDSAmountDetails.ExistingVdsBalance;
                    if (VDSAmountDetails.AverageBillingAmount < 3000)
                    {
                        minVDSAmount = 3000;
                    }
                    else if (VDSAmountDetails.AverageBillingAmount > 3000)
                    {
                        minVDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                    }
                    if (gap >= 0 && gap < System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
                    {
                        minVDSAmount = gap;
                    }
                    maxVDSAmount = gap;
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
                        minVDSAmount = Math.Round(System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount) / 500, 0) * 500;
                    }
                    if (gap >= 0 && gap < System.Convert.ToDecimal(VDSAmountDetails.AverageBillingAmount))
                    {
                        minVDSAmount = gap;
                    }
                    maxVDSAmount = gap;

                }
                Session["VDSAmount"] = minVDSAmount.ToString();
                Session["maxVDSAmount"] = maxVDSAmount.ToString();

                string date1 = string.Empty;
                if (billinghDetails.DateDue == "0000-00-00")
                {
                    date1 = "0000-00-00";
                }
                else
                {
                    CultureInfo culture = new CultureInfo("es-ES");
                    String myDate = billinghDetails.DateDue;
                    DateTime date = DateTime.Parse(myDate, culture);
                    date1 = date.ToString("dd MMM yyyy");
                }

                model = new ViewPayBill()
                {

                    AccountNumber = accountDetails.AccountNumber,// this.UserProfileService.GetAccountNumberfromItem(masterAccount.ToString()),
                    Name = accountDetails.Name,// this.UserProfileService.GetLoginName(),
                    BookNumber = accountDetails.BookNumber,// "505",
                    CycleNumber = accountDetails.CycleNumber,// "05",
                    Zone = accountDetails.ZoneNumber,// "South Central",
                                                     // Address = accountDetails.Address,// "300 288 Shere Punjab SOC Mahakali Caves Rd Andheri E Near Tolani College Mumbai 400067",
                    BillMonth = billinghDetails.BillMonth,// "May-2018",
                    //PaymentDueDate = (billinghDetails.DateDue == "0000-00-00") ? "0000-00-00" : DateTime.ParseExact(billinghDetails.DateDue, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd MMM yyyy"),
                    PaymentDueDate = date1,
                    DueDateGraterthenFourDays = (billinghDetails.DateDue == "0000-00-00") ? false : (DateTime.ParseExact(billinghDetails.DateDue, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture) > DateTime.Now.AddDays(-4) ? false : true),

                    TariffSlab = billinghDetails.TariffSlab,// "LT-1b",
                    MeterNumber = string.Join(",", billinghDetails.MeterNumbers),// "7587321",
                    UnitsConsumed = billinghDetails.UnitsConsumed,//  "1",
                    TotalCharges = billinghDetails.TotalCharges,// "134.27",
                    CurrentMonthsBills = billinghDetails.CurrentMonthCharge,// "134.69",
                    BroughtForward = billinghDetails.BroughtForward,// "128.26",
                    TotalBillAmount = billinghDetails.TotalBillAmount,//  "246.19",
                    SecurityDeposit = sdDetails.SDAmount.ToString(),// "560.00",
                    SecurityDepositMsg = sdDetails.Message.ToString(),
                    SecurityDepositAmountType = "Actual",                                            //VDSAmount= vdsAmount.ToString(),
                    AmountPayable = amountpayable.ToString(),// billinghDetails.AmountPayable.ToString(),// "250"
                    LoginName = this.UserProfileService.GetLoginName(),

                    EMIEligible = emiDetails.Status == "S" ? true : false,
                    EMIInstallmentAmount = emiDetails.EMIInstallmentAmount,
                    EMIOutstandingAmount = emiDetails.TotalOutstanding,
                    ProceedWithEMI = false,
                    AverageVDSAmount = minVDSAmount,
                    MaxVDSAmount = maxVDSAmount,
                    PANNo = consumerPANGSTDetails.PANNumber,
                    PaymentVDSAmount = VDSAmountDetails.CurrentOutstanding,
                    Mobile = consumerDetails.Mobile,
                    Email = consumerDetails.Email
                };
                Session["securityDespositAmt"] = sdDetails.SDAmount.ToString();
            }
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);


        }


        [HttpGet]
        public ActionResult ITDeclarationsRevamp()
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
            if (UserSession.UserSessionContext != null)
            {
                var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(GetPrimaryAccountNumber());
                objITDeclarationsModel.MobileNumber = registeredMobileNumber;
            }
            return this.View(objITDeclarationsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateRenderingId]
        public ActionResult ITDeclarationsRevamp(ITDeclarationsModel model, string LinkPAN = null, string FileITR = null, string TDSApplicability = null, string SubmitTDS = null, string ValidateCA = null, string SendOTP = null, string ValidateOTP = null, string Submit = null, string ReSendOTP = null)
        {
            var registeredMobileNumberLoggedIn = string.Empty;
            if (UserSession.UserSessionContext != null)
            {
                registeredMobileNumberLoggedIn = SapPiService.Services.RequestHandler.GetMobileNumber(GetPrimaryAccountNumber());
            }
            try
            {
                if (!string.IsNullOrEmpty(LinkPAN))
                {

                    this.ModelState["CANumber"].Errors.Clear();
                    model.DeclarationType = "1";
                    model.MobileNumber = registeredMobileNumberLoggedIn;
                    if (UserSession.UserSessionContext != null)
                    {
                        model.CANumber = GetPrimaryAccountNumber();
                    }
                    Session["ITDeclarationsModel"] = model;
                    Session["Message"] = null;
                }
                if (!string.IsNullOrEmpty(FileITR))
                {
                    this.ModelState["CANumber"].Errors.Clear();
                    model.DeclarationType = "3";
                    model.MobileNumber = registeredMobileNumberLoggedIn;
                    if (UserSession.UserSessionContext != null)
                    {
                        model.CANumber = GetPrimaryAccountNumber();
                    }
                    Session["ITDeclarationsModel"] = model;
                    Session["Message"] = null;
                }
                if (!string.IsNullOrEmpty(TDSApplicability))
                {
                    this.ModelState["CANumber"].Errors.Clear();
                    model.DeclarationType = "2";
                    model.MobileNumber = registeredMobileNumberLoggedIn;
                    if (UserSession.UserSessionContext != null)
                    {
                        model.CANumber = GetPrimaryAccountNumber();
                    }
                    Session["ITDeclarationsModel"] = model;
                    Session["Message"] = null;
                }
                if (!string.IsNullOrEmpty(SubmitTDS))
                {
                    this.ModelState["CANumber"].Errors.Clear();
                    model.DeclarationType = "4";
                    model.MobileNumber = registeredMobileNumberLoggedIn;
                    if (UserSession.UserSessionContext != null)
                    {
                        model.CANumber = GetPrimaryAccountNumber();

                    }
                    Session["ITDeclarationsModel"] = model;
                    Session["Message"] = null;
                }
                if (UserSession.UserSessionContext != null)
                {
                    var consumerPANGSTDetails = SapPiService.Services.RequestHandler.FetchConsumerPANGSTDetails(model.CANumber);
                    if (string.IsNullOrEmpty(consumerPANGSTDetails.CANumber) || string.IsNullOrEmpty(consumerPANGSTDetails.PANNumber))
                    {
                        Session["Message"] = null;
                        model.PANNotLinked = true;
                        ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/ITDeclaration/PAN not mapped", "Please update PAN number against entered CA number to proceed further."));
                        return this.View(model);
                    }
                    if (model.DeclarationType != "4")
                    {
                        var result = SapPiService.Services.RequestHandler.ITDeclarationCheck(model.CANumber, model.DeclarationType, consumerPANGSTDetails.PANNumber);
                        if (result == null || result.IsSuccess == false)
                        {
                            model.AlreadySubmittedforCurrentFY = true;
                            ModelState.AddModelError(nameof(model.CANumber), result.Message);
                            //Session["Message"] = result.Message;

                            return this.View(model);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ValidateCA))
                {
                    if (Session["ITDeclarationsModel"] == null)
                    {
                        return this.View(model);
                    }
                    ITDeclarationsModel ITDeclarationsModel = (ITDeclarationsModel)Session["ITDeclarationsModel"];
                    model.DeclarationType = ITDeclarationsModel.DeclarationType;

                    if (UserSession.UserSessionContext != null)
                    {
                        model.CANumber = GetPrimaryAccountNumber();
                    }

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
                        ModelState.AddModelError(nameof(model.Captcha), "Please validate captcha to continue.");
                        return this.View(model);
                    }
                    ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                    if (!reCaptchaResponse.success)
                    {
                        ModelState.AddModelError(nameof(model.Captcha), "Please validate captcha to continue.");
                        return this.View(model);
                    }
                    //Check If PAN number is updated
                    var consumerPANGSTDetails = SapPiService.Services.RequestHandler.FetchConsumerPANGSTDetails(model.CANumber);
                    if (string.IsNullOrEmpty(consumerPANGSTDetails.CANumber) || string.IsNullOrEmpty(consumerPANGSTDetails.PANNumber))
                    {
                        Session["Message"] = null;
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
                            this.Session["ItDeclerationMessage"] = new InfoMessage(result.Message, InfoMessage.MessageType.Error);
                            // Session["Message"] = result.Message;
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
                if (!string.IsNullOrEmpty(ReSendOTP))
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
                            Log.Info("PAN Update: Number of attempt to Re Send OTP reached for AccountNumber." + model.CANumber, this);
                            this.ModelState.AddModelError(nameof(model.CANumber), DictionaryPhraseRepository.Current.Get("/ComplaintRegistration/Max20OTPPerLECMobile", "Number of attempt to Resend OTP reached for Entered AccountNumber."));
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
                                model.ReSendMessage = DictionaryPhraseRepository.Current.Get("/Accounts/ITDeclarations/ReSendOTPMessage", "OTP send");
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

                        if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                        {
                            ModelState.AddModelError(nameof(model.Captcha), "Please validate captcha to continue.");
                            return this.View(model);
                        }
                        ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                        if (!reCaptchaResponse.success)
                        {
                            ModelState.AddModelError(nameof(model.Captcha), "Please validate captcha to continue.");
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
                                else
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;

                                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[0-9]{12}$");
                                    //If you want both upper case and lower case alphabets.  
                                    Match match = regex.Match(model.AadharNumber);
                                    if (!match.Success)
                                    {
                                        this.ModelState.AddModelError(nameof(model.AadharNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Enter valid Aadhar", "Please enter a valid 12 digit Aadhar Number."));
                                        return this.View(model);
                                    }
                                    if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                                    {
                                        ModelState.AddModelError(nameof(model.Captcha), "Please validate captcha to continue.");
                                        return this.View(model);
                                    }
                                    ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                                    if (!reCaptchaResponse.success)
                                    {
                                        ModelState.AddModelError(nameof(model.Captcha), "Please validate captcha to continue.");
                                        return this.View(model);
                                    }


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
                                    this.Session["ItDeclerationMessage"] = new InfoMessage(result.Message, InfoMessage.MessageType.Error);
                                    //model.SuccessMessage = result.Message;

                                    ModelState.AddModelError(nameof(model.SuccessMessage), result.Message);
                                    return this.View(model);
                                }
                                else
                                {
                                    model.IsSuccess = true;
                                    // model.SuccessMessage = result.Message;
                                    this.Session["ItDeclerationMessage"] = new InfoMessage(result.Message, InfoMessage.MessageType.Success);
                                    if (model.AgreeOption == "1")
                                    {
                                        //Generate PDF / Image file of declaration
                                        pdfToAttach = objService.GeneratePDF_LinkPANWithAdhar(model.CANumber, ITDeclarationsModel.PANNumber, model.AadharNumber, Server);

                                        //Upload PDF against CA number 
                                        try
                                        {
                                            string image = System.Convert.ToBase64String(pdfToAttach);

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
                                    // model.SuccessMessage = result.Message;
                                    this.Session["ItDeclerationMessage"] = new InfoMessage(result.Message, InfoMessage.MessageType.Error);
                                    ModelState.AddModelError(nameof(model.SuccessMessage), result.Message);
                                    return this.View(model);
                                }
                                else
                                {
                                    model.IsSuccess = true;
                                    // model.SuccessMessage = result.Message;
                                    this.Session["ItDeclerationMessage"] = new InfoMessage(result.Message, InfoMessage.MessageType.Success);
                                    if (model.AgreeOption == "1")
                                    {
                                        //Generate PDF / Image file of declaration
                                        pdfToAttach = objService.GeneratePDF_194QApplicabilityofTDS(model.CANumber, ITDeclarationsModel.PANNumber, Server);
                                        //Image upload
                                        try
                                        {
                                            string image = System.Convert.ToBase64String(pdfToAttach);

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
                                    // model.SuccessMessage = result.Message;
                                    this.Session["ItDeclerationMessage"] = new InfoMessage(result.Message, InfoMessage.MessageType.Error);
                                    ModelState.AddModelError(nameof(model.SuccessMessage), result.Message);
                                    return this.View(model);
                                }
                                else
                                {
                                    model.IsSuccess = true;
                                    //model.SuccessMessage = result.Message;
                                    this.Session["ItDeclerationMessage"] = new InfoMessage(result.Message, InfoMessage.MessageType.Success);
                                    if (model.AgreeOption == "1")
                                    {
                                        //Generate PDF / Image file of declaration
                                        pdfToAttach = objService.GeneratePDF_206CFilingITR(model.CANumber, ITDeclarationsModel.PANNumber, model.FY_3AcknowledgementNumber, model.FY_3DateOfFilingReturn, model.FY_2AcknowledgementNumber, model.FY_2DateOfFilingReturn, Server);

                                        //Image upload
                                        try
                                        {
                                            string image = System.Convert.ToBase64String(pdfToAttach);

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

                                //Validate entered amount || model.Amount_considered_for_TDS.ToString().Any(char.IsLetter)
                                if (string.IsNullOrEmpty(model.Amount_considered_for_TDS.ToString()))
                                {
                                    model.IsvalidatOTP = true;
                                    model.IsvalidatAccount = true;
                                    model.IsOTPSend = true;
                                    this.ModelState.AddModelError(nameof(model.Amount_considered_for_TDS), DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/InvalidAmount", "Invalid amount value."));
                                    return this.View(model);
                                }
                                if (System.Convert.ToDecimal(model.Amount_considered_for_TDS) > System.Convert.ToDecimal(billingAmount))
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
                                    this.Session["ItDeclerationMessage"] = new InfoMessage(result.Message, InfoMessage.MessageType.Error);
                                    // model.SuccessMessage = result.Message;
                                    ModelState.AddModelError(nameof(model.SuccessMessage), result.Message);
                                    return this.View(model);
                                }
                                else
                                {
                                    model.IsSuccess = true;
                                    // model.SuccessMessage = result.Message;
                                    this.Session["ItDeclerationMessage"] = new InfoMessage(result.Message, InfoMessage.MessageType.Success);
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



        [HttpGet]
        public ActionResult PANUpdateRevamp()
        {
            PANUpdateModel model = new PANUpdateModel();
            return View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult PANUpdateRevamp(PANUpdateModel model, FormCollection form, string ValidateCA = null, string UpdatePAN = null, string UpdateGST = null, string ValidateOTP = null, string Submit = null)
        {

            Session["UpdateMessage"] = null;
            //if (!ModelState.IsValid)
            //{
            //    return this.View(model);
            //}
            if (!string.IsNullOrEmpty(ValidateCA))
            {
                if (string.IsNullOrEmpty(model.AccountNumber))
                {
                    this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number required", "Please enter valid Account Number."));
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
                        if (this.Request.Url.Host.Equals("electricity.dev.local"))
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


                        //if (true)
                        //{
                        //    Log.Info("ChangeOfNameLECUserRegisteration: OTP Api call success for LEC registration", this);
                        //    model.IsPANToBeUpdated = true;
                        //    model.IsAccountNumberValidated = true;
                        //    model.IsOTPSent = true;
                        //    Session["PANUpdateModel"] = model;
                        //    model.OTPNumber = generatedotp;
                        //    return this.View(model);
                        //}
                        //else
                        //{
                        //    Log.Error("ChangeOfNameLECUserRegisteration OTP Api call failed for registration", this);
                        //    this.ModelState.AddModelError(nameof(model.AccountNumber), DictionaryPhraseRepository.Current.Get("/CON/LEC Login/OTP sending error", "Unable to send OTP."));
                        //    model.IsAccountNumberValidated = false;
                        //    return this.View(model);
                        //}
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

                    string generatedOTP = string.Empty;

                    if (this.Request.Url.Host.Equals("electricity.dev.local"))
                    {
                        generatedOTP = "12345";
                    }
                    else
                    {
                        generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(consumerDetails.Mobile, model.AccountNumber, "PANUpdate");
                    }

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


                                string image = System.Convert.ToBase64String(compressimage);
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

                                                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/CON/LEC Profile/PAN Updated Successfully", "Updates are done successfully."), InfoMessage.MessageType.Success);
                                                // return this.Redirect(this.Request.RawUrl);
                                                return Redirect("/it-declarations");
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

        #endregion


        [HttpPost]
        //[ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitMeterReadingRevamp(SubmitMeterReading m)
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
                    DateTime meterDT = System.Convert.ToDateTime(meter.MeterReadingDate);
                    DateTime dt = DateTime.ParseExact(meterDT.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    // Convert.ToDateTime(meterDT.ToString("dd-MM-yyyy"));
                    string meterReadingDate = dt.Year + "" + dt.Month.ToString("d2") + "" + dt.Day.ToString("d2");
                    DateTime dtSMRD = System.Convert.ToDateTime(mtr.SMRD);
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
                                string image = System.Convert.ToBase64String(docData);

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


        [HttpGet]
        public ActionResult SubmitMeterReadingRevamp(string AccountNumber, string source)
        {
            AccountNumber = GetPrimaryAccountNumber();
            source = "4";
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
            string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.SelfMeterReadingSettings.EncryptionKey].Value;
            //staging  "B3XbAcGCezTeqfVxWIl4tvNdI";
            //Production "PdSgVkYp3s6v9yBEHMbQeThWm"



            SubmitMeterReading meterReading = new SubmitMeterReading();
            if (AccountNumber != null)
            {
                if (source != null)
                {
                    meterReading.Source = source;
                }
                var CANumber = AccountNumber;

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
                    // meterReading.CANumber = HttpUtility.UrlEncode(objclsTripleDES.Decrypt(CANumber, EncryptionKey));

                    meterReading.CANumber = AccountNumber;
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
                                string[] result = Reqoutput.IT_RETURN[0].MESSAGE.Split('.');
                                ViewBag.Result = null;
                                if (result.Length > 1)
                                {
                                    string[] nextdate = result[1].Split(' ');

                                    if (nextdate.Length > 9)
                                    {
                                        meterReading.Result = nextdate[10].ToString();
                                    }
                                    else
                                    {
                                        ViewBag.Result = result[0].ToString();
                                        meterReading.Result = result[0].ToString(); ;
                                    }
                                }
                                else
                                {
                                    ViewBag.Result = result[0].ToString();
                                    meterReading.Result = result[0].ToString(); ;
                                }
                                // string [] nextdate= result[1].Split(' ');


                                // meterReading.Result = Reqoutput.IT_RETURN[0].MESSAGE.ToString().Replace(" ", "_");
                            }
                        }
                        /* else
                         {
                             meterReading.Result = "This facility is not available for CA number mentioned by you. In case you still want to submit your Meter Reading please write to us at helpdesk.mumbaielectricity@adani.com";
                         }*/
                        // return Redirect("/submitmeter-error?er=data_na&msg=" + meterReading.Result);


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
            meterReading.CANumber = GetPrimaryAccountNumber();
            return this.View(meterReading);
        }


        #region ||MyAccount Submit Meter Reading||

        [HttpGet]
        //[RedirectAuthenticated]
        public ActionResult MyAccountSubmitMeterReadingRevamp()
        {
            var profile = this.UserProfileService.GetProfile(Context.User);

            SubmitMeterReading meterReading = new SubmitMeterReading();
            meterReading.IsSubmitted = false;
            if (profile.AccountNumber != null)
            {

                try
                {
                    meterReading.Source = "3";
                    meterReading.CANumber = profile.AccountNumber;

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
                                meterReading.IsSubmitted = true;
                                meterReading.Result = Reqoutput.IT_RETURN[0].MESSAGE.ToString().Replace(" ", "_"); ;
                            }
                        }


                        //return Redirect("/submitmeter-error?er=data_na&msg=" + meterReading.Result);
                        return this.View(meterReading);
                    }

                    meterReading.MeterList = MeterList;
                    meterReading.MeterAttachments = MeterAttachmentList;
                }
                catch (Exception ex)
                {
                    meterReading.Result = ex.Message;
                    Log.Error(" my account submit meter reading " + ex.Message, this);
                    //return Redirect("/submitmeter-error?er=ca_na&msg=");
                    return this.View(meterReading);
                }
            }

            return this.View(meterReading);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyAccountSubmitMeterReadingRevamp(SubmitMeterReading m)
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

                foreach (SelfMeterReading meter in SelfMeterReadingList)
                {

                    MeterReadingDetail mtr = m.MeterList.Where(x => x.MeterNumber == meter.MeterNumber).FirstOrDefault();

                    DateTime meterDT = System.Convert.ToDateTime(meter.MeterReadingDate);
                    //DateTime dt = Convert.ToDateTime(meterDT.ToString("dd-MM-yyyy"));
                    DateTime dt = DateTime.ParseExact(meterDT.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string meterReadingDate = dt.Year + "" + dt.Month.ToString("d2") + "" + dt.Day.ToString("d2");
                    DateTime dtSMRD = System.Convert.ToDateTime(mtr.SMRD);
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
                                string image = System.Convert.ToBase64String(docData);

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
            m.Result = "Thank you for submitting Meter Reading.";
            m.IsSubmitted = true;
            //return Redirect("/submitmeter-thankyou");
            return this.View(m);
        }
        #endregion


        #region ||** Change Bill Language **||



        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult ChangeBillLanguageRevamp()
        {
            ChangeBillLanguage model = new ChangeBillLanguage();
            model.BillLanguageList = new List<string>
                {
                SapPiService.Domain.BillingLanguage.English.ToString(),
                SapPiService.Domain.BillingLanguage.Hindi.ToString(),
                SapPiService.Domain.BillingLanguage.Marathi.ToString()
                //SapPiService.Domain.BillingLanguage.Gujarati.ToString(),
                
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
        [RedirectUnauthenticatedCookieTempered]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBillLanguageRevamp(ChangeBillLanguage model)
        {
            try
            {
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    model.BillLanguageList = new List<string>
                {
                SapPiService.Domain.BillingLanguage.English.ToString(),
                SapPiService.Domain.BillingLanguage.Hindi.ToString(),
                SapPiService.Domain.BillingLanguage.Marathi.ToString()
                //SapPiService.Domain.BillingLanguage.Gujarati.ToString(),
                
                };
                    var existingBillLanguage = SapPiService.Services.RequestHandler.FetchBillingLanguage(this.UserProfileService.GetAccountNumber(Context.User));
                    model.BillLanguageSelected = existingBillLanguage.ToString();

                    ModelState.AddModelError("Captcha", DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue."));
                    return this.View(model);
                }

                var accountNumber = this.UserProfileService.GetAccountNumber(Context.User);
                SapPiService.Domain.BillingLanguage selectedLanguage = (SapPiService.Domain.BillingLanguage)Enum.Parse(typeof(SapPiService.Domain.BillingLanguage), model.BillLanguageSelected);
                SapPiService.Services.RequestHandler.UpdateBillingLanguage(accountNumber, selectedLanguage);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Change Bill Language", "Bill Language updated successfully!"), InfoMessage.MessageType.Success);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Change Bill Language fail", "Bill Language update Failed, please try again!" + ex.Message.ToString()), InfoMessage.MessageType.Error);
            }
            return this.Redirect(this.Request.RawUrl);
        }



        #endregion



        #region ||** Power Outage Information **||



        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult GetOutageInformationRevamp()
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
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult GetOutageInformationRevamp(EditProfile profiles)
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




        #region ||** Login / LogOut **||

        [RedirectAuthenticated]
        public ActionResult LoginRevamp(string returnUrl = null)
        {
            return this.View(this.CreateLoginInfo(returnUrl));
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult LoginRevamp(LoginInfo loginInfo)
        {
            if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
            {
                ModelState.AddModelError(nameof(loginInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Please validate captcha to continue"));
                return this.View(loginInfo);
            }
            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

            if (!reCaptchaResponse.success)
            {
                ModelState.AddModelError(nameof(loginInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                return this.View(loginInfo);
            }

            return this.LoginRevamp(loginInfo, redirectUrl => new RedirectResult(redirectUrl));
        }

        protected virtual ActionResult LoginRevamp(LoginInfo loginInfo, Func<string, ActionResult> redirectAction)
        {
            try
            {
                var user = this.AccountRepository.Login(loginInfo.LoginName, loginInfo.Password);
                if (user == null)
                {

                    this.ModelState.AddModelError(nameof(loginInfo.Password), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/User Not Valid", "Username or password is not valid."));
                    Session["LoginSession"] = "2";
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
                Session["LoginSession"] = "1";
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

                //#region ||AEML Revamp Complaint : Added to create AEMLComplaintUserSessionContext||
                //var profile = this.UserProfileService.GetProfile(Context.User);
                //UserSession.AEMLComplaintUserSessionContext = new LoginInfoComplaint
                //{
                //    LoginName = profile.LoginName,
                //    AccountNumber = profile.AccountNumber,
                //    MobileNumber = profile.MobileNumber,
                //    IsRegistered = true,
                //    SessionId = sessionId.ToString(),
                //    IsAuthenticated = true
                //};
                //#endregion

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
                Session["LoginSession"] = "2";
                Log.Error("Login Method Error - for User - " + loginInfo.LoginName, ex.Message);
                this.ModelState.AddModelError(nameof(loginInfo.Password), ex.Message);
                return this.View(loginInfo);
            }
        }


        [HttpPost]
        public ActionResult LogoutRevamp()
        {
            //Session.Abandon();
            RegistrationRepository rp = new RegistrationRepository();
            rp.DeleteSessionAfterLogout();
            this.AccountRepository.Logout();
            this.Session["UpdateMessage"] = null;
            this.Session["ComplaintRegistrationModel"] = null;
            SessionHelper.UserSession.AEMLComplaintUserSessionContext = null;
            SessionHelper.UserSession.UserSessionContext = null;
            Session.ClearSession();
            Response.DestroyCookie();
            var parentItem = Context.Site.GetStartItem().Parent;
            InternalLinkField link = parentItem.Fields[Templates.AccountsSettings.Fields.AfterLogoutPage];
            if (link.TargetItem == null)
            {
                return this.Redirect(Context.Site.GetRootItem().Url());
            }

            return this.Redirect(link.TargetItem.Url());

        }

        #endregion

        private LoginInfo CreateLoginInfo(string returnUrl = null)
        {
            return new LoginInfo
            {
                ReturnUrl = returnUrl,
                LoginButtons = this.FedAuthLoginRepository.GetAll()
            };
        }

        #region ||** Forgot Password **||


        [RedirectAuthenticated]
        public ActionResult ForgotPasswordRevamp()
        {
            return this.View();
        }

        [HttpPost]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPasswordRevamp(ForgotPasswordInfo model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Please validate captcha to continue"));
                    return this.View(model);
                }

                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/ElectricityNew/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                    return this.View(model);
                }

                if (string.IsNullOrEmpty(model.LoginName) && string.IsNullOrEmpty(model.AccountNo) && string.IsNullOrEmpty(model.Email))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/forgot required parameter", "Please enter username or email and account number."));
                    return this.View(model);
                }

                //Added condition to check if input is Email Address
                if (IsEmailAddress(model.LoginName))
                {
                    model.Email = model.LoginName;
                    model.LoginName = string.Empty;
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
                        resetpasswordusers.Profile.SetCustomProperty("TokenGenerationDate", Sitecore.DateUtil.ToIsoDate(System.Convert.ToDateTime(DateTime.UtcNow)));
                        resetpasswordusers.Profile.Save();

                        var BaseUrl = this.UserProfileService.GetPageURL(Templates.Pages.ResetPasswordRevamp);
                        if (!string.IsNullOrEmpty(BaseUrl))
                        {
                            string RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + BaseUrl + "?token=" + token + "&&userId=" + ItemId + "";
                            Log.SingleError($"Reset Password Link for {model.LoginName} = {RedirectUrl}", this);
                            //Commented For Demo as SMTP is not configured on DEV
                            //Added Condition For Demo as SMTP is not configured on DEV
                            //if (!this.Request.Url.Host.Equals("electricity.dev.local"))
                            //{
                            this.NotificationService.SendPasswordResetLink(resetpasswordusers.Profile.Email, model.LoginName, RedirectUrl);
                            //}
                        }
                        Session["ResetPasswordSuccessLink"] = "2";
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Forgot Password Success", "password reset link is sent to your mail address."), InfoMessage.MessageType.Success);
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
                        resetpasswordusers.Profile.SetCustomProperty("TokenGenerationDate", Sitecore.DateUtil.ToIsoDate(System.Convert.ToDateTime(DateTime.UtcNow)));
                        resetpasswordusers.Profile.Save();

                        var BaseUrl = this.UserProfileService.GetPageURL(Templates.Pages.ResetPasswordRevamp);
                        if (!string.IsNullOrEmpty(BaseUrl))
                        {
                            string RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + BaseUrl + "?token=" + token + "&&userId=" + ItemId + "";
                            Log.SingleError($"Reset Password Link for {loginName} = {RedirectUrl}", this);
                            //Added Condition For Demo as SMTP is not configured on DEV
                            if (!this.Request.Url.Host.Equals("electricity.dev.local"))
                            {
                                this.NotificationService.SendPasswordResetLink(resetpasswordusers.Profile.Email, loginName, RedirectUrl);
                            }

                        }
                        Session["ResetPasswordSuccessLink"] = "2";
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Forgot Password Success", "password reset link is sent to your mail address."));
                    }
                    else
                    {

                        Session["ForgotPasswordRevamp"] = "2";
                        ModelState.AddModelError(nameof(model.AccountNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Email Account Mismatch", "Email Address and Account number not matched. Please enter associated account number with email address."));
                        return this.View(model);


                        //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Email Account Mismatch", "Email Address and Account number not matched. Please enter asscoiated acocunt number with email address."));
                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error($"Can't reset password for user {model.AccountNo}", ex, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Exception", ex.Message), InfoMessage.MessageType.Error);
            }
            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.Pages.ForgotPasswordRevamp));

        }

        #endregion

        #region ||** Change Passsword **||

        [RedirectUnauthenticatedCookieTempered]
        public ActionResult ChangePasswordBodyRevamp()
        {
            return this.View("ChangePasswordBodyRevamp", new ChangePassword() { LoginName = this.UserProfileService.GetLoginName(), AccountNumber = GetPrimaryAccountNumber() });
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [RedirectUnauthenticatedCookieTempered]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordBodyRevamp(ChangePassword changePassword)
        {

            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);
            //if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
            //{
            //    ModelState.AddModelError(nameof(changePassword.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
            //    return this.View(changePassword);
            //}
            if (!reCaptchaResponse.success)
            {
                ModelState.AddModelError(nameof(changePassword.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue."));
                return this.View(changePassword);
            }
            var logininfo = this.AccountRepository.Login(changePassword.LoginName, changePassword.OldPassword);
            if (logininfo == null)
            {
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Change Password Failure", "Old Password is InCorrect."), InfoMessage.MessageType.Error);
                return this.Redirect(this.Request.RawUrl);
            }
            if (!this.ModelState.IsValid)
            {
                return this.View(changePassword);
            }
            var user = System.Web.Security.Membership.GetUser(Context.User.Profile.UserName);
            string oldPassword = user.ResetPassword();
            //if (changePassword.OldPassword.Equals(changePassword.Password))
            //{
            //    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/ChooseDifferentPassword", "Please choose different Password"));
            //}
            //else
            //{
            Session["Passwordchangesuccess"] = "2";
            user.ChangePassword(oldPassword, changePassword.Password);
            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Change Password Success", "Password Changed successfully"), InfoMessage.MessageType.Success);
            //}
            return this.Redirect(this.Request.RawUrl);
        }
        #endregion

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


        #region ||** Reset Password **||
        [RedirectAuthenticated]
        public ActionResult ResetPasswordRevamp(string token = null, string userId = null)
        {
            ResetPasswordInfo model = new ResetPasswordInfo();
            if (!string.IsNullOrEmpty(userId))
            {
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
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Token Expired", "Reset Link Expired"), InfoMessage.MessageType.Error);
                        }
                    }
                    else
                    {
                        model.Token = string.Empty;
                        model.UserID = string.Empty;
                        model.IsValidRequest = false;
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Invalid Token", "Invalid Token"), InfoMessage.MessageType.Error);
                    }
                }
            }
            return this.View("ResetPasswordRevamp", model);
        }

        [HttpPost]
        [ValidateModel]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPasswordRevamp(ResetPasswordInfo model)
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                //{
                //    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                //    return this.View(model);
                //}

                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/ElectricityNew/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
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
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Reset Success", "Password Reset Successfully"), InfoMessage.MessageType.Success);

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
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Reset Failure msg", "Invalid Password Change Request."), InfoMessage.MessageType.Error);
                    }
                }
                else
                {
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Reset Failure msg", "Invalid Password Change Request."), InfoMessage.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                this.Session["UpdateMessage"] = new InfoMessage(ex.Message, InfoMessage.MessageType.Error);
                Log.Error($"Can't get Detail for user {model.UserID}", ex, this);
            }
            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.Pages.ResetPasswordRevamp));
        }

        #endregion


        #region ||** Registration **||

        [RedirectAuthenticated]
        public ActionResult RegisterRevamp()
        {
            var registerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<RegisteredValidateAccount>(Session["ValidRegistration"]?.ToString());
            if (registerInfo == null || string.IsNullOrEmpty(registerInfo.AccountNo) || string.IsNullOrEmpty(registerInfo.MeterNo))
            {
                return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.RegistrationPageRevamp));
            }

            RegistrationInfo model = new RegistrationInfo();
            model.AccountNo = registerInfo.AccountNo;
            model.MeterNo = registerInfo.MeterNo;
            model.MobileNumber = registerInfo.MobileNo;

            var SecretQuestionList = Context.Database.GetItem(Templates.RegistrationConfig.Datasource.SecretQuestionListRevamp);
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
        public ActionResult RegisterRevamp(RegistrationInfo registrationInfo)
        {
            //var registerInfo = (RegisteredValidateAccount)Session["ValidRegistration"];
            var registerInfo = Session != null && Session["ValidRegistration"] != null ? Newtonsoft.Json.JsonConvert.DeserializeObject<RegisteredValidateAccount>(Session["ValidRegistration"]?.ToString()) : null;
            if (registerInfo == null || registrationInfo.AccountNo != registerInfo.AccountNo)
            {
                return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.RegistrationPageRevamp));
            }
            var SecretQuestionList = Context.Database.GetItem(Templates.RegistrationConfig.Datasource.SecretQuestionListRevamp);
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
                ModelState.AddModelError(nameof(registrationInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Please validate captcha to continue"));
                return this.View(registrationInfo);
            }

            if (this.AccountRepository.Exists(registrationInfo.LoginName))
            {
                this.ModelState.AddModelError(nameof(registrationInfo.LoginName), UserAlreadyExistsError);

                return this.View(registrationInfo);
            }

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
                return this.RegisterLoginRevamp(loginInfo, redirectUrl => new RedirectResult(redirectUrl));
            }
            catch (MembershipCreateUserException ex)
            {
                Log.Error($"Can't create user with {registrationInfo.Email}", ex, this);
                this.ModelState.AddModelError(nameof(registrationInfo.LoginName), ex.Message);

                return this.View(registrationInfo);
            }
        }

        [RedirectAuthenticated]
        public ActionResult RegistrationValidateRevamp()
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
        public ActionResult RegistrationValidateRevamp(RegisteredValidateAccount registrationInfo, string ValidateAccount = null, string sendOTP = null, string ValidateOTP = null)
        {
            if (!ModelState.IsValid)
            {
                return this.View(registrationInfo);
            }
            if (!string.IsNullOrEmpty(ValidateAccount))
            {
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(registrationInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Please validate captcha to continue"));
                    return this.View(registrationInfo);
                }

                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(registrationInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                    return this.View(registrationInfo);
                }

                var isExist = _dbAccountService.IsAccountNumberExist(registrationInfo.AccountNo);
                if (isExist)
                {
                    Session["AccountAlreadyRegistered"] = "2";
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
                }
            }
            else if (!string.IsNullOrEmpty(sendOTP))
            {
                string mob = SapPiService.Services.RequestHandler.GetMobileNumber(registrationInfo.AccountNo);

                if (string.IsNullOrEmpty(registrationInfo.MobileNo))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile number invalid", "Please enter valid Mobile Number"));
                    registrationInfo.isvalidatAccount = true;
                    return this.View(registrationInfo);
                }
                else if (!IsCorrectMobileNumber(registrationInfo.MobileNo))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile number invalid", "Please enter valid Mobile Number"));
                    registrationInfo.isvalidatAccount = true;
                    return this.View(registrationInfo);
                }
                else if (!mob.Equals(registrationInfo.MobileNo))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/MobileNumberNotMatched", "Mobile number does not Match"));
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
                        //Added Condition to bypass OTP services as OTP services not working on DEV env
                        //if (this.Request.Url.Host.Equals("electricity.dev.local"))
                        //{
                        //    Log.Error("OTP Api call success for registration", this);
                        //    this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTP", "OTP Sent."));
                        //    registrationInfo.isOTPSent = true;
                        //    registrationInfo.isvalidatAccount = true;
                        //    return this.View(registrationInfo);
                        //}
                        //else
                        //{
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/Accounts/Settings/SMS API for registration", "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=Welcome to Adani Electricity, You have initiated a request for new registration for account no. {1}, OTP for this request is: {2}&intflag=false"), registrationInfo.MobileNo, registrationInfo.AccountNo, generatedotp);
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
                        //}
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
                string generatedOTP = string.Empty;
                //Added Condition to bypass OTP services as OTP services not working on DEV env
                //if (this.Request.Url.Host.Equals("electricity.dev.local"))
                //{
                //    generatedOTP = "12345";
                //}
                //else
                //{
                generatedOTP = registrationRepo.GetOTPRegistrationByMobAndCA(registrationInfo.MobileNo, registrationInfo.AccountNo, "Registration");
                //}

                if (!string.Equals(generatedOTP, registrationInfo.OTPNumber))
                {
                    this.ModelState.AddModelError(nameof(registrationInfo.OTPNumber), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                    registrationInfo.isOTPSent = true;
                    registrationInfo.isvalidatAccount = true;
                    return this.View(registrationInfo);
                }
                else
                {
                    Session["ValidRegistration"] = Newtonsoft.Json.JsonConvert.SerializeObject(registrationInfo);
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.RegistrationRevampPageAfterValidate));
                }
            }
            return View(registrationInfo);
        }

        public bool IsCorrectMobileNumber(String strNumber)
        {
            Regex mobilePattern = new Regex(@"^[0-9]{10}$");
            return mobilePattern.IsMatch(strNumber);
        }


        public bool IsEmailAddress(String str)
        {
            Regex mobilePattern = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return mobilePattern.IsMatch(str);
        }
        #endregion

        protected virtual ActionResult RegisterLoginRevamp(LoginInfo loginInfo, Func<string, ActionResult> redirectAction)
        {
            try
            {
                var user = this.AccountRepository.Login(loginInfo.LoginName, loginInfo.Password);
                if (user == null)
                {
                    this.ModelState.AddModelError(nameof(loginInfo.Password), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/User Not Valid", "Username or password is not valid."));
                    return this.View(loginInfo);
                }

                //SessionHelper.UserSession.UserSessionContext = new DashboardModel
                //{
                //    userType = UserTypes.Standard
                //};

                SessionHelper.UserSession.UserSessionContext = new DashboardModel
                {
                    userType = UserTypes.Standard,
                    //SessionId = sessionId.ToString(),
                    SessionId = Request.Cookies["ASP.NET_SessionId"].Value,
                    UserName = loginInfo.LoginName
                };

                Session["LoginSession"] = "1";
                RegistrationRepository rp = new RegistrationRepository();
                rp.StoreCurrentSession();
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
                    //SessionHelper.UserSession.UserSessionContext.UserName = user.Profile.UserName;
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


        public ActionResult PaymentSuccessRevamp()
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
                        if (ctx != null)
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
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment Success Response UserSessionContext is null", this);
                }
            }

            Sitecore.Diagnostics.Log.Info("Payment Success Response", this);
            PaymentHistoryDataContext dc = new PaymentHistoryDataContext();
            UserLoginSession currentsessionExists = (
                        from x in dc.UserLoginSessions
                        where x.UserName == Sitecore.Context.User.LocalName && x.IsActive == true
                        select x).FirstOrDefault<UserLoginSession>();

            //Persisting session id for the user login instance
            string sessionId = Guid.NewGuid().ToString();
            if (currentsessionExists != null && UserSession.UserSessionContext == null)
            {
                SessionHelper.UserSession.UserSessionContext = new DashboardModel
                {
                    userType = UserTypes.Standard,
                    SessionId = currentsessionExists.SessionId,
                    UserName = Sitecore.Context.User.LocalName
                };

                RegistrationRepository rp = new RegistrationRepository();
                rp.StoreCurrentSession();
            }
            return this.View(model);
        }
        public ActionResult PaymentFailureRevamp()
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
                        if (ctx != null)
                        {
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = ctx.TransactionId,
                                ResponseStatus = Constants.PaymentResponse.Failure,
                                Responsecode = ctx.Responsecode,
                                Remark = Constants.PaymentResponse.Failure,
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
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment failure Response UserSessionContext is null", this);
                }
            }
            PaymentHistoryDataContext dc = new PaymentHistoryDataContext();
            UserLoginSession currentsessionExists = (
                        from x in dc.UserLoginSessions
                        where x.UserName == Sitecore.Context.User.LocalName && x.IsActive == true
                        select x).FirstOrDefault<UserLoginSession>();
            //Persisting session id for the user login instance
            string sessionId = Guid.NewGuid().ToString();
            if (currentsessionExists != null && UserSession.UserSessionContext == null)
            {
                SessionHelper.UserSession.UserSessionContext = new DashboardModel
                {
                    userType = UserTypes.Standard,
                    SessionId = currentsessionExists.SessionId,
                    UserName = Sitecore.Context.User.LocalName
                };

                RegistrationRepository rp = new RegistrationRepository();
                rp.StoreCurrentSession();
            }
            Sitecore.Diagnostics.Log.Info("Payment Failure Response", this);
            return this.View(model);
        }
        public ActionResult PaymentPendingRevamp()
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
                        if (ctx != null)
                        {
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = ctx.TransactionId,
                                ResponseStatus = Constants.PaymentResponse.Pending,
                                Responsecode = ctx.Responsecode,
                                Remark = Constants.PaymentResponse.Pending,
                                PaymentRef = ctx.PaymentRef,
                                OrderId = orderid,
                                AmountPayable = ctx.Amount,
                                AccountNumber = ctx.AccountNumber,
                                LoginName = ctx.AccountNumber,
                                msg = ctx.ResponseMsg,
                                PaymentMode = ctx.PaymentMode,
                                TransactionDate = ctx.Created_Date.ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Bill Paid", "Bill Paid"),
                                // PaymentGateway = 6
                            };

                            model = modelviewpay;
                        }
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment Pending Response UserSessionContext is null", this);
                }
            }
            Sitecore.Diagnostics.Log.Info("Payment Pending Response", this);
            return this.View(model);
        }

        public ActionResult PaymentPendingCashFree(string order_id, string order_token)
        {
            var model = new RootJson();

            model.order_id = order_id;
            model.order_token = order_token;
            if (!string.IsNullOrEmpty(order_id) && !string.IsNullOrEmpty(order_token))
            {
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    var ctx = dbcontext.PaymentHistories.Where(x => x.OrderId == order_id).FirstOrDefault();
                    if (ctx != null)
                    {
                        model.order_amount = ctx.Amount;
                        model.customer_details.customer_id = ctx.AccountNumber;
                        model.Transaction_date = System.DateTime.Now;
                    }
                }
                if (SessionHelper.UserSession.UserSessionContext == null)
                {
                    SessionHelper.UserSession.UserSessionContext = new DashboardModel()
                    {
                        OrderId = model.order_id
                    };
                }
                else
                {
                    SessionHelper.UserSession.UserSessionContext.OrderId = model.order_id;
                }
                PaymentHistoryDataContext dc = new PaymentHistoryDataContext();
                UserLoginSession currentsessionExists = (
                            from x in dc.UserLoginSessions
                            where x.UserName == Sitecore.Context.User.LocalName && x.IsActive == true
                            select x).FirstOrDefault<UserLoginSession>();
                //Persisting session id for the user login instance
                string sessionId = Guid.NewGuid().ToString();
                if (currentsessionExists != null && UserSession.UserSessionContext == null)
                {
                    SessionHelper.UserSession.UserSessionContext = new DashboardModel
                    {
                        userType = UserTypes.Standard,
                        SessionId = currentsessionExists.SessionId,
                        UserName = Sitecore.Context.User.LocalName
                    };

                    RegistrationRepository rp = new RegistrationRepository();
                    rp.StoreCurrentSession();
                }
            }
            Sitecore.Diagnostics.Log.Info("Payment Pending Response for Cash Free", this);
            return this.View(model);
        }
        public void SessionRestoringRevamp()
        {
            PaymentHistoryDataContext dc = new PaymentHistoryDataContext();
            UserLoginSession currentsessionExists = (
                        from x in dc.UserLoginSessions
                        where x.UserName == Sitecore.Context.User.LocalName && x.IsActive == true
                        select x).FirstOrDefault<UserLoginSession>();

            //Persisting session id for the user login instance
            string sessionId = Guid.NewGuid().ToString();
            if (currentsessionExists != null && UserSession.UserSessionContext == null)
            {
                SessionHelper.UserSession.UserSessionContext = new DashboardModel
                {
                    userType = UserTypes.Standard,
                    SessionId = currentsessionExists.SessionId,
                    UserName = Sitecore.Context.User.LocalName,
                    primaryAccountNumber = GetPrimaryAccountNumber() ?? string.Empty
                };

                RegistrationRepository rp = new RegistrationRepository();
                rp.StoreCurrentSession();
            }
        }

        public ActionResult BillDeskCallBack()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);

            try
            {
                #region Variable Initialization
                string checksum = string.Empty;
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_Merchant_ID].Value;

                string VDSChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
                string VDSMerchantId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_Merchant_ID].Value;

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
                                PaymentModeNumber = responselist[7].ToString(),
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
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);

            try
            {
                #region Variable Initialization
                string checksum = string.Empty;
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_Merchant_ID].Value;

                string VDSChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
                string VDSMerchantId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_Merchant_ID].Value;

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
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);

            try
            {
                #region Variable Initialization
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_Merchant_ID].Value;

                string VDSChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
                string VDSMerchantId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_Merchant_ID].Value;

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
                    var amount = System.Convert.ToDecimal(responselist[4].ToString()).ToString();

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
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);

            try
            {
                #region Variable Initialization
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_Merchant_ID].Value;

                string VDSChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
                string VDSMerchantId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_Merchant_ID].Value;

                #endregion

                //BillDesk Response
                string responsemsg = Request.Form["msg"];
                Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskVDSCallBackS2S Callback Message - " + responsemsg, this);

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

        [HttpPost]
        public ActionResult PaytmCallBack()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);

            try
            {
                #region Variable Initialization

                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string merchantKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_Merchant_Key].Value;
                string merchantID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_Merchant_ID].Value;

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

                    string responseData = this.PaymentService.PaytmTransactionStatusAPIRequestPostRevamp(TransactionRequestAPIResponse);
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

                        string paymentRef = string.Empty;

                        TransactionStatusApiresponse.TryGetValue("GATEWAYNAME", out paymentRef);
                        if (string.IsNullOrEmpty(paymentRef))
                            TransactionStatusApiresponse.TryGetValue("BANKNAME", out paymentRef);

                        //Note : write code to set parameters to Model Value.
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = TransactionStatusApiresponse["TXNID"],
                            ResponseStatus = TransactionStatusApiresponse["STATUS"],
                            Responsecode = TransactionStatusApiresponse["RESPCODE"],
                            Remark = TransactionStatusApiresponse["RESPMSG"],
                            PaymentRef = paymentRef,
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
                            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);

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

                    string paymentRef = string.Empty;

                    TransactionRequestAPIResponse.TryGetValue("GATEWAYNAME", out paymentRef);
                    if (string.IsNullOrEmpty(paymentRef))
                        TransactionRequestAPIResponse.TryGetValue("BANKNAME", out paymentRef);

                    var modelviewpay = new ViewPayBill()
                    {
                        TransactionId = TransactionRequestAPIResponse["TXNID"],
                        ResponseStatus = TransactionRequestAPIResponse["STATUS"],
                        Responsecode = TransactionRequestAPIResponse["RESPCODE"],
                        Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                        PaymentRef = paymentRef,
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
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);

            try
            {
                #region Variable Initialization

                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string merchantKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_Merchant_Key].Value;
                string merchantID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_Merchant_ID].Value;

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

                        string paymentRef = string.Empty;

                        TransactionRequestAPIResponse.TryGetValue("GATEWAYNAME", out paymentRef);
                        if (string.IsNullOrEmpty(paymentRef))
                            TransactionRequestAPIResponse.TryGetValue("BANKNAME", out paymentRef);

                        //Note : write code to set parameters to Model Value.
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = TransactionRequestAPIResponse["TXNID"],
                            ResponseStatus = TransactionRequestAPIResponse["STATUS"],
                            Responsecode = TransactionRequestAPIResponse["RESPCODE"],
                            Remark = TransactionRequestAPIResponse["RESPMSG"],
                            PaymentRef = paymentRef,
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
                    string paymentRef = string.Empty;

                    TransactionRequestAPIResponse.TryGetValue("GATEWAYNAME", out paymentRef);
                    if (string.IsNullOrEmpty(paymentRef))
                        TransactionRequestAPIResponse.TryGetValue("BANKNAME", out paymentRef);

                    var modelviewpay = new ViewPayBill()
                    {
                        TransactionId = TransactionRequestAPIResponse["TXNID"],
                        ResponseStatus = TransactionRequestAPIResponse["STATUS"],
                        Responsecode = TransactionRequestAPIResponse["RESPCODE"],
                        Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Checksum MissMatch", "Checksum Missmatch"),
                        PaymentRef = paymentRef,
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

        #region BeNow Web Service
        [HttpPost]
        public ActionResult Benow_CallbackS2S()
        {
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);

            string decryptedstring = "";
            string response = "";
            try
            {
                StreamReader bodyStream = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);

                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                string encryptedString = bodyStream.ReadToEnd();

                Sitecore.Diagnostics.Log.Info("BeNow_Callback Method Called encryptedString:" + encryptedString, this);

                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string BNW_HashKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_HashKey].Value;
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
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);

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
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);

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
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string publicKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.DBSFields.DBS_ServerPublicKeyPath].Value; //@"C:\Users\Nidhi.Paneri\Desktop\DBS\Key Pair\from DBS\DBSSG_EN_PUBLIC.asc";
                string privateKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.DBSFields.DBS_ClientPrivateKeyPath].Value; //@"C:\Users\Nidhi.Paneri\Desktop\DBS\Key Pair\DBS_PrivateSecret.asc";
                string privayeKeyPwd = itemInfo.Fields[Templates.PaymentConfigurationRevamp.DBSFields.DBS_ClientSecretKey].Value; //"5&rd&Xdj#nJR";

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
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);

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

        #region ||** Change Bill Language Setting**||



        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult ChangeBillLanguageSettingRevamp()
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

                var profile = this.UserProfileService.GetProfile(Context.User);
                //ViewBag.Paperless_Billing = profile.PaperlessBilling;
                ViewBag.Paperless_Billing = SapPiService.Services.RequestHandler.CheckPaperlessBillingSettings(profile.AccountNumber);
                ViewBag.EmailAlert = profile.EBill;
                ViewBag.SMSAlert = profile.SMSUpdate;

            }
            catch
            { }
            return this.View(model);
        }

        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult UserDashboardSetting()
        {



            if (!Context.PageMode.IsNormal)
            {
                return this.View(this.UserProfileService.GetEmptyProfile());
            }
            try
            {



                var profile = this.UserProfileService.GetProfile(Context.User);
                ViewBag.Paperless_Billing = SapPiService.Services.RequestHandler.CheckPaperlessBillingSettings(profile.AccountNumber);
                //ViewBag.Paperless_Billing = profile.PaperlessBilling;
                ViewBag.EmailAlert = profile.EBill;
                ViewBag.SMSAlert = profile.SMSUpdate;

            }
            catch
            { }
            return this.View();
        }

        [HttpPost]
        [RedirectUnauthenticatedCookieTempered]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBillLanguageSettingRevamp(ChangeBillLanguage model)
        {
            try
            {
                var accountNumber = this.UserProfileService.GetAccountNumber(Context.User);
                SapPiService.Domain.BillingLanguage selectedLanguage = (SapPiService.Domain.BillingLanguage)Enum.Parse(typeof(SapPiService.Domain.BillingLanguage), model.BillLanguageSelected);
                SapPiService.Services.RequestHandler.UpdateBillingLanguage(accountNumber, selectedLanguage);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Change Bill Language", "Bill Language updated successfully!"), InfoMessage.MessageType.Success);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Change Bill Language fail", "Bill Language update Failed, please try again!" + ex.Message.ToString()), InfoMessage.MessageType.Success);
            }
            return this.Redirect(this.Request.RawUrl);
        }

        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]

        public ActionResult PaperlessBilling(bool PaperlessBilling)
        {
            AjaxResponse resp;
            string msg = string.Empty;
            string header = string.Empty;
            //ViewBag.Status = null;
            string result = string.Empty;
            var isOperationCompleted = false;
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);
                profile.PaperlessBilling = PaperlessBilling;
                this.UserProfileService.SavePaperlessBillingFlag(Context.User.Profile, profile);

                result = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(profile.AccountNumber, profile.MobileNumber, null, profile.Email, PaperlessBilling);
                if (result == "success")
                {
                    isOperationCompleted = true;
                    //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success Paperless Billing", "Paperless Billing flag updated successfully!"), InfoMessage.MessageType.Success);
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + result, this);
                    //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/SMS alerts update error", "SMS Alerts flag was not successfully updated, please try again!"), InfoMessage.MessageType.Error);
                }
                //ViewBag.Status = PaperlessBilling ? "registered" : "deregistered";
                //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Success Paperless Alerts", "Paperless billing registration done successfully!"), InfoMessage.MessageType.Success);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
                //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Error Paperless alerts", "Paperless billing flag not updated, please try again!"));
            }

            if (isOperationCompleted)
            {
                header = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success Paperless Billing", "Account Updated Successfully!");

                if (PaperlessBilling)
                    msg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success Paperless Billing Register", "Wow! You’ve just moved closer to a greener Earth.");
                else
                    msg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success Paperless Billing Deregister", "Your paperless billing has been deactivated. You will continue to receive your physical bill.");
            }
            else
            {
                header = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Fail Paperless Billing", "Unable to update details");
                msg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Fail Paperless Billing Message", "Sorry for the inconvenience. Due to some technical issue, we are not able to update your preference. Please try after some time.");
            }
            resp = new AjaxResponse() { IsSuccess = isOperationCompleted, Header = header, Message = msg };

            //return this.Redirect(this.Request.RawUrl);
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult SMSAlertsBody(bool SMSAlert)
        {
            AjaxResponse resp;
            var msg = string.Empty;
            var header = string.Empty;
            var result = string.Empty;
            var isOperationCompleted = false;
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);

                profile.SMSUpdate = SMSAlert;

                this.UserProfileService.SaveSMSAlerts(Context.User.Profile, profile);
                string mobile = profile.SMSUpdate ? profile.MobileNumber : "";
                string telephone = profile.SMSUpdate ? profile.PhoneNumber : "";
                result = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(profile.AccountNumber, mobile, telephone, profile.Email, null);

                //this.UserProfileService.SaveSMSAlerts(Context.User.Profile, loggedinuserprofile);
                //result = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(loggedinuserprofile.AccountNumber, loggedinuserprofile.PhoneNumber, telephone, profile.Email, null);
                if (result == "success")
                {
                    isOperationCompleted = true;
                    //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success SMS alerts", "SMS Alerts flag updated successfully!"), InfoMessage.MessageType.Success);
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + result, this);
                    //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/SMS alerts update error", "SMS Alerts flag was not successfully updated, please try again!"), InfoMessage.MessageType.Error);
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + e.Message, this);
                //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Error SMS alerts", "SMS Alerts flag not updated, please try again!"), InfoMessage.MessageType.Error);
            }


            if (isOperationCompleted)
            {
                header = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success SMS alerts", "Account Updated Successfully!");

                if (SMSAlert)
                    msg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success SMS alerts enable", "See you on your phone! We’ll send you SMS alerts about your electricity account.");
                else
                    msg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success SMS alerts disable", "Alerts updated successfully! Your SMS alerts have been deactivated.");
            }
            else
            {
                header = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Fail SMS alerts", "Unable to update details");
                msg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Fail SMS alerts message", "Sorry for the inconvenience. Due to some technical issue, we are not able to update your preference. Please try after some time.");
            }
            resp = new AjaxResponse() { IsSuccess = isOperationCompleted, Header = header, Message = msg };

            //return Json(result, JsonRequestBehavior.AllowGet);
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult EmailAlertsBody(bool EmailAlert)
        {
            AjaxResponse resp;
            var msg = string.Empty;
            var header = string.Empty;
            var result = string.Empty;
            var isOperationCompleted = false;
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);

                profile.EBill = EmailAlert;

                this.UserProfileService.SaveEmailAlerts(Context.User.Profile, profile);

                this.UserProfileService.SaveEmailAlerts(Context.User.Profile, profile);
                string emailId = profile.EBill ? profile.Email : "";
                result = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(profile.AccountNumber, profile.MobileNumber, profile.PhoneNumber, emailId, null);

                // result = SapPiService.Services.RequestHandler.UpdateEmailSmsPaperlessSettings(loggedinuserprofile.AccountNumber, loggedinuserprofile.MobileNumber, loggedinuserprofile.PhoneNumber, loggedinuserprofile.Email, null);
                if (result == "success")
                {
                    isOperationCompleted = true;
                    // this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success Email alerts", "Email Alerts flag updated successfully!"), InfoMessage.MessageType.Success);
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + result, this);
                    //this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Email alerts update error", "Email Alerts flag was not successfully updated, please try again!"), InfoMessage.MessageType.Error);
                }
            }
            catch (Exception e)
            {
                result = "Fail";
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + e.Message, this);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Error Email alerts", "Email Alerts flag not updated, please try again!"), InfoMessage.MessageType.Error);
            }


            if (isOperationCompleted)
            {
                header = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success Email alerts", "Account Updated Successfully!");

                if (EmailAlert)
                    msg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success Email alerts enable", "Cool!! We’ll alert you about the updates on your electricity account via email.");
                else
                    msg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Success Email alerts disable", "Alerts updated successfully! Your email alerts have been deactivated.");
            }
            else
            {
                header = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Fail Email alerts", "Unable to update details");
                msg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Fail Email alerts message", "Sorry for the inconvenience. Due to some technical issue, we are not able to update your preference. Please try after some time.");
            }
            resp = new AjaxResponse() { IsSuccess = isOperationCompleted, Header = header, Message = msg };

            //return Json(result, JsonRequestBehavior.AllowGet);
            return Json(resp, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region || eNach Registration ||
        [HttpGet]
        public ActionResult eNachRegistrationRevamp()
        {
            ENachRegistrationModel model = new ENachRegistrationModel();
            model.IsvalidatAccount = false;
            model.IsOTPSent = false;
            return this.View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult eNachRegistrationRevamp(ENachRegistrationModel registrationInfo, string ValidateAccount = null, string submit = null, string Reset = null)
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

                    ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                    if (!reCaptchaResponse.success)
                    {
                        ModelState.AddModelError(nameof(registrationInfo.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                        return this.View(registrationInfo);
                    }
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
                        Session["EnachRegistrationSuccess"] = "1";
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
                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.AccountsSettings.Fields.ENACHRegistrationPageRevamp));
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

        #endregion

        #region ||** Manage Connection **||
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult ManageConnectionRevamp()
        {
            ManageConnections model = new ManageConnections();

            if (Context.User != null)
            {
                model.AccountNumber = this.UserProfileService.GetAccountNumber(Context.User);

                var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(model.AccountNumber);

                var securityAmount = SapPiService.Services.RequestHandler.FetchSecurityDepositAmount(model.AccountNumber);

                var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(model.AccountNumber);

                model = new ManageConnections()
                {
                    MasterAccountNumber = GetPrimaryAccountNumber(),
                    LoginName = this.UserProfileService.GetLoginName(),
                    AccountNumber = accountDetails.AccountNumber,
                    MeterNumber = string.Join(",", billinghDetails.MeterNumbers),
                    Name = accountDetails.Name,
                    Division = accountDetails.ZoneNumber,
                    Address = accountDetails.Address,
                    Email = Context.User.Profile.Email,
                    Mobile = Context.User.Profile.GetCustomProperty(Constants.UserProfile.Fields.MobileNo),
                    SecurityDeposit = securityAmount.ToString(),
                    TariffCategory = billinghDetails.TariffSlab
                };
            }

            return this.View(model);
        }

        #endregion

        #region ||** Non Registered Account **||
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult NonRegisteredAccountBodyRevamp()
        {
            return this.View("NonRegisteredAccountBodyRevamp", new NonRegisteredAccount() { MasterAccountNumber = GetPrimaryAccountNumber() });
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [RedirectUnauthenticatedCookieTempered]
        [ValidateAntiForgeryToken]
        public ActionResult NonRegisteredAccountBodyRevamp(NonRegisteredAccount nonregisteredAccount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(nonregisteredAccount.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Please validate captcha to continue"));
                    return this.View(nonregisteredAccount);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(nonregisteredAccount.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                    return this.View(nonregisteredAccount);
                }

                if (string.IsNullOrEmpty(nonregisteredAccount.Accountnumber) || string.IsNullOrEmpty(nonregisteredAccount.MeterNumber))
                {
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Required", "Please enter Account and Meter Number."), InfoMessage.MessageType.Error);
                    return this.Redirect(this.Request.RawUrl);
                }
                else
                {
                    var billinghDetails = SapPiService.Services.RequestHandler.FetchBilling(nonregisteredAccount.Accountnumber);
                    var data = billinghDetails.MeterNumbers;
                    if (billinghDetails != null && billinghDetails.MeterNumbers.Length == 0)
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account number invalid", "Account Number is not valid. Please enter another Account Number."), InfoMessage.MessageType.Error);
                        return this.Redirect(this.Request.RawUrl);
                    }
                    if (!billinghDetails.MeterNumbers.Contains(nonregisteredAccount.MeterNumber))
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Meter number invalid", "Meter Number is not associated with Account Number. Please enter another Meter Number."), InfoMessage.MessageType.Error);
                        return this.Redirect(this.Request.RawUrl);
                    }

                    //var accountItemId = this.AccountRepository.GetAccountItemId(nonregisteredAccount.Accountnumber);
                    //check if acnt number is in db
                    if (_dbAccountService.IsAccountNumberExist(nonregisteredAccount.Accountnumber))
                    {
                        this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Already Exist", "Oops! This account is already linked or registered."), InfoMessage.MessageType.Error);
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
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Already Added", "This account is already added in your list."), InfoMessage.MessageType.Error);
                        }
                        else
                        {
                            _dbAccountService.NonRegisteredAccountInDb(nonregisteredAccount.Accountnumber, nonregisteredAccount.MeterNumber);
                            this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Successfull", "Your account is successfully added."), InfoMessage.MessageType.Success);
                            Session["LinkNonRegisterAccountSubmit"] = "1";
                        }

                    }
                    return this.Redirect(this.Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Method NonRegisteredAccountBodyRevamp -", ex.Message);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Non Registered Failure", "There is some technical issue. Please try again."), InfoMessage.MessageType.Error);
                return this.Redirect(this.Request.RawUrl);
            }

        }
        #endregion

        #region ||** Registered Account **||
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult RegisteredAccountBodyRevamp()
        {
            return this.View("RegisteredAccountBodyRevamp", new RegisteredAccount() { MasterAccountNumber = GetPrimaryAccountNumber() });
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [RedirectUnauthenticatedCookieTempered]
        [ValidateAntiForgeryToken]
        public ActionResult RegisteredAccountBodyRevamp(RegisteredAccount registeredAccount)
        {
            if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
            {
                ModelState.AddModelError(nameof(registeredAccount.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Please validate captcha to continue"));
                return this.View(registeredAccount);
            }
            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

            if (!reCaptchaResponse.success)
            {
                ModelState.AddModelError(nameof(registeredAccount.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                return this.View(registeredAccount);
            }
            // NOTE : Authenticate User
            var userDetail = this.AccountRepository.GetUser(registeredAccount.ExistingUserId, registeredAccount.Password);
            if (userDetail == null)
            {
                this.Session["UpdateRevampMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Registered Account Wrong Password Heading", "Error"), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Registered Account Wrong Password", "Invalid Account User Id or Password"), InfoMessageRevamp.MessageTypeRevamp.Error, false);
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
                            this.Session["UpdateRevampMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account not exist Heading", "Error"), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account not exist", "Please enter the registered Consumer Account no."), InfoMessageRevamp.MessageTypeRevamp.Error, false);
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
                        this.Session["UpdateMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account not exist Heading", "Error"), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Account not exist", "Please enter the registered Consumer Account no."), InfoMessageRevamp.MessageTypeRevamp.Error, false);
                        return this.Redirect(this.Request.RawUrl);
                    }
                }//End else of if present in DB

                this.Session["UpdateMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Registered Account Successfull Heading", "Success"), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Registered Account Successfull", "Great! You have linked your registered accounts successfully."), InfoMessageRevamp.MessageTypeRevamp.Error, false);
                Session["LinkRegisterAccountSubmit"] = "1";
                return this.Redirect(this.Request.RawUrl);

            }

        }
        #endregion


        #region ||** De Registered Account **||
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult DeRegisterAccountBodyRevamp()
        {
            #region variable declaration
            var model = new DeRegisterAccount();
            var availableitemsinDB = _dbAccountService.GetAccountListbyusername(Context.User.Profile.UserName);
            List<KeyValuePair<string, string>> AccountNumberList = new List<KeyValuePair<string, string>>();
            string primaryAccountNumber = GetPrimaryAccountNumber(); ;
            #endregion

            #region New Code
            //// NOTE : check availability first in custom DB if data not available then check in to Sitecore
            if (availableitemsinDB != null && availableitemsinDB.Any())
            {
                #region Get Accounts List based on Logged in user
                var selecteditems = availableitemsinDB.Select(s => new { s.AccountNumber, s.Id }).ToList();
                #endregion

                #region Get Current Account Number and Item ID
                if (selecteditems.Any() && selecteditems.Count > 1)
                {
                    model.CurrentAccountNumber = this.UserProfileService.GetAccountNumber(Context.User);
                    model.CurrentAccountItemID = selecteditems.Where(w => w.AccountNumber == model.CurrentAccountNumber).Select(s => s.Id.ToString()).FirstOrDefault();
                    model.CurrentAccountName = SapPiService.Services.RequestHandler.FetchDetail(model.CurrentAccountNumber) != null ? SapPiService.Services.RequestHandler.FetchDetail(model.CurrentAccountNumber).Name : "";
                }
                else
                {
                    model.CurrentAccountNumber = string.Empty;
                    model.CurrentAccountItemID = string.Empty;
                    model.CurrentAccountName = "";
                }
                #endregion
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

                    #region Get Current Account Number and Item ID
                    if (AccountNumberList.Any() && AccountNumberList.Count > 1)
                    {
                        model.CurrentAccountNumber = this.UserProfileService.GetAccountNumber(Context.User);
                        model.CurrentAccountItemID = AccountNumberList.Where(w => w.Value == model.CurrentAccountNumber).Select(s => s.Value.ToString()).FirstOrDefault();
                        model.CurrentAccountName = SapPiService.Services.RequestHandler.FetchDetail(model.CurrentAccountNumber) != null ? SapPiService.Services.RequestHandler.FetchDetail(model.CurrentAccountNumber).Name : "";
                    }
                    else
                    {
                        model.CurrentAccountNumber = string.Empty;
                        model.CurrentAccountItemID = string.Empty;
                        model.CurrentAccountName = "";
                    }
                    #endregion
                }
            }
            #endregion

            return this.View("DeRegisterAccountBodyRevamp", model);
        }

        [RedirectUnauthenticatedCookieTempered]
        public ActionResult DeRegisterAccountInternallyRevamp(string ItemId)
        {
            var item = Context.Database.GetItem(Templates.Pages.ManageConnectionsRevamp);
            var url = item.Url();

            try
            {
                //// NOTE : check availability first in custom DB if data not available then check in to Sitecore
                var IsRecordAvailableInDB = _dbAccountService.DeregisterAccountRevamp(ItemId);

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

                Session["DeRegisterAccountInternallyRevampSuccess"] = "2";
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/DeRegistred Success", "Done! Account de-registered successfully"), InfoMessage.MessageType.Success);

            }
            catch (Exception ex)
            {
                Log.Error("Method DeRegisterAccountInternally -", ex.Message);
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/DeRegistred Failure", "Something Went Wrong."), InfoMessage.MessageType.Error);
            }
            return RedirectPermanent(url);
        }
        #endregion

        #region ||Online Payment Quick Pay||

        [HttpGet]
        [RedirectAuthenticated]
        public ActionResult QuickPayOnlinePaymentRevamp()
        {
            return this.View();
        }

        [HttpPost]
        [RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult QuickPayOnlinePaymentRevamp(ViewPayBill model)
        {
            if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
            {
                ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Please validate captcha to continue"));
                return this.View(model);
            }

            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

            if (!reCaptchaResponse.success)
            {
                ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                return this.View(model);
            }

            var parentItem = Context.Site.GetStartItem().Parent;
            InternalLinkField link = parentItem.Fields[Templates.AccountsSettings.Fields.QuickPayBillPage];

            SessionHelper.UserSession.UserSessionContext = new DashboardModel
            {
                AccountNumber = model.AccountNumber
            };

            return this.Redirect(link.TargetItem.Url());
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
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Please validate captcha to continue"));
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
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
                    Session["ValidateGreenOptInCA"] = "1";
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
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Please validate captcha to continue"));
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
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

                    Session["ValidOtp"] = "1";
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
                        model.ConsumerName = consumerDetails.NAME_CustomerName;
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
                byte[] imageBytes = System.Convert.FromBase64String(data.Split(',')[1]);

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

        #region ||** Quick Access Check Complaint Status **||

        [HttpGet]
        //[RedirectAuthenticated]
        public ActionResult QACheckComplaintStatusRevamp()
        {
            QuickAccessServices service = new QuickAccessServices();

            return this.View(service);


        }

        [HttpPost]
        //[RedirectAuthenticated]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult QACheckComplaintStatusRevamp(QuickAccessServices profile)
        {
            var quickAccessServices = new QuickAccessServices();
            try
            {
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(profile.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                    return this.View(profile);
                }

                SapPiService.Domain.ComplainStatus model = SapPiService.Services.RequestHandler.CheckComplaintStatus(profile.AccountNumber, profile.ComplaintNumber);

                quickAccessServices.AccountNumber = model.AccountNumber;
                quickAccessServices.ComplainCode = model.ComplainCode;
                quickAccessServices.Message = model.Message;

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
        public ActionResult QAOutageInformationRevamp()
        {
            SapPiService.Domain.OutageDetail model = new SapPiService.Domain.OutageDetail();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateRenderingId]
        public ActionResult QAOutageInformationRevamp(SapPiService.Domain.OutageDetail profile)
        {
            SapPiService.Domain.OutageDetail model = new SapPiService.Domain.OutageDetail();
            try
            {
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(profile.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                    return this.View(profile);
                }
                model = SapPiService.Services.RequestHandler.GetOutageInformation(profile.AccountNumber);

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

        public ActionResult RatusRevamp()
        {
            //{9CECD379-1CB6-4590-A107-3385D8C3C9D0}
            var parent = Sitecore.Context.Database.GetItem("{9CECD379-1CB6-4590-A107-3385D8C3C9D0}").Children;

            Models.RatusRevamp rateusRevampModel = new Models.RatusRevamp();

            foreach (var item in parent.ToList())
            {
                TypeofCategory objTypeofCategory = new TypeofCategory();
                objTypeofCategory.Name = item["Name"];
                rateusRevampModel.TypeofCategoryList.Add(objTypeofCategory);
            }
            return this.View(rateusRevampModel);
        }

        [HttpPost]
        public ActionResult RatingRevamp(string Category, string Rating, string Textareavalue, string Captcha)
        {
            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Captcha);

            if (!reCaptchaResponse.success)
            {
                return Json("captchinvalid", JsonRequestBehavior.AllowGet);
            }

            var accountNumber = GetPrimaryAccountNumber();//this.UserProfileService.GetAccountNumber(Context.User);
            RatusRevamp rateusRevampModel = new RatusRevamp();
            RateusServices objratus = new RateusServices();
            rateusRevampModel.CANumber = accountNumber;
            rateusRevampModel.CategoryName = Category;
            rateusRevampModel.Rating = Rating;
            rateusRevampModel.AppreciationNote = Textareavalue;
            var r = objratus.RateusInsert(rateusRevampModel);
            var result = "";
            if (r)
            {
                result = "success";
                this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Ratus Success", "Ratus successfully submitted"), InfoMessage.MessageType.Success);
            }
            else
                result = "";
            return Json(result, JsonRequestBehavior.AllowGet);

        }

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

        #region ||** Meter Reading Date **||
        // [RedirectUnauthenticatedCookieTempered]
        public ActionResult MeterReadingDateBodyRevamp()
        {
            // return this.View();
            if (UserSession.UserSessionContext != null)
            {
                return this.View("MeterReadingDateBodyRevamp", new MeterReadingDateinfoRevamp() { MonthList = getMonthsWithCurrentYear(), IsCheckData = true, CANumber = string.Empty });
            }
            else
            {
                return this.View("MeterReadingDateBodyRevamp", new MeterReadingDateinfoRevamp() { MonthList = getMonthsWithCurrentYear(), IsCheckData = false });
            }
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        // [RedirectUnauthenticatedCookieTempered]
        [ValidateAntiForgeryToken]
        public ActionResult MeterReadingDateBodyRevamp(MeterReadingDateinfoRevamp model, string SendCA = null, string Reset = null)
        {
            

            string hdnCANumber = Request.Form["hdnCANumber"];
            if (!string.IsNullOrEmpty(SendCA))
            {
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                    return this.View("MeterReadingDateBodyRevamp", new MeterReadingDateinfoRevamp()
                    {
                        MonthList = getMonthsWithCurrentYear(),
                        IsCheckData = false

                    });
                }
                // Session["CANumberCheck"] = model.CANumber;
                return this.View("MeterReadingDateBodyRevamp", new MeterReadingDateinfoRevamp()
                {
                    MonthList = getMonthsWithCurrentYear(),
                    IsCheckData = true
                });
            }
            else if (!string.IsNullOrEmpty(Reset))
            {
                // Session["CANumberCheck"] = model.CANumber;
                return this.View("MeterReadingDateBodyRevamp", new MeterReadingDateinfoRevamp()
                {
                    MonthList = getMonthsWithCurrentYear(),
                    IsCheckData = false

                });
            }
            else
            {
                //Note : Get Available List of Items from 	EBILL MRDATE
                Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = master.GetItem(new Data.ID(Templates.ItemList.MeterReadingItemList.ToString()));
                string AccountNo = string.Empty;
                string cycleValue = string.Empty;
                if (UserSession.UserSessionContext != null)
                {
                    AccountNo = GetPrimaryAccountNumber();
                    cycleValue = SessionHelper.UserSession.UserSessionContext.cycleNumber;
                }
                else
                {
                    AccountNo = model.CANumber;
                    var CAType = SapPiService.Services.RequestHandler.GetCycleNumber(AccountNo);
                    if (CAType != null && !string.IsNullOrEmpty(CAType["cycleNumber"]))
                    {
                        cycleValue = CAType["cycleNumber"];
                    }
                }
                ///var accountDetails = SapPiService.Services.RequestHandler.FetchDetail(AccountNo);
                //string cycleValue = accountDetails.CycleNumber;
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
                    return this.View("MeterReadingDateBodyRevamp", new MeterReadingDateinfoRevamp()
                    {
                        MonthList = getMonthsWithCurrentYear()
                        //IsCheckData = false
                    });
                }

                string billMonth = model.Monthval;
                model.IsCheckData = true;
                var AvailableItem = itemInfo.GetChildren().ToList().FirstOrDefault(w => w.Fields[Templates.MeterReadingProperties.CYCLE.ToString()].Value == cycleValue && w.Fields[Templates.MeterReadingProperties.BILLMONTH.ToString()].Value == billMonth);
                if (AvailableItem != null)
                    model.ScheduleMeterReadingdate = AvailableItem.Fields[Templates.MeterReadingProperties.FULLMETERREADINGDATE.ToString()].Value;
                else
                    model.ScheduleMeterReadingdate = string.Empty;
                model.MonthList = getMonthsWithCurrentYear();
                return this.View("MeterReadingDateBodyRevamp", model);
            }
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

        #endregion

        #region ||** Security Deposit Installmet OptIn **||
        [HttpGet]
        public ActionResult SDInstallmentOptInrevamp()
        {
            SDEMIProcess SDEMIProcessModel = new SDEMIProcess();
            try
            {
                //Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                //var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                //string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.EMISettings.EncryptionKey].Value;
                ////staging  "B3XbAcGCezTeqfVxWIl4tvNdI";
                ////Production "PdSgVkYp3s6v9yBEHMbQeThWm"

                //if (Request.QueryString["ca_number"] != null)
                //{
                //    if (Request.QueryString["source"] != null)
                //    {
                //        SDEMIProcessModel.Source = Request.QueryString["source"].ToString();
                //    }
                //    var CANumber = Request.QueryString["ca_number"].ToString();

                //    clsTripleDES objclsTripleDES = new clsTripleDES();
                //    string caNumber = objclsTripleDES.Encrypt("100517857", EncryptionKey);

                //    SDEMIProcessModel.CANumber = objclsTripleDES.Decrypt(CANumber, EncryptionKey);
                //    Session["SDInstOptModel"] = SDEMIProcessModel;
                //}

                if (UserSession.UserSessionContext != null)
                {
                    var CAnumber = GetPrimaryAccountNumber();
                    var consumerDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(CAnumber);
                    if (string.IsNullOrEmpty(consumerDetails.AccountNumber))
                    {
                        ModelState.AddModelError(nameof(SDEMIProcessModel.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Account Number incorrect", "Account Number is invalid."));
                        return this.View(SDEMIProcessModel);
                    }
                    else
                    {
                        string checkCA = ChangeOfNameService.ValidateCAForSDOptIn(consumerDetails);
                        if (!string.IsNullOrEmpty(checkCA))
                        {
                            this.ModelState.AddModelError(nameof(SDEMIProcessModel.CANumber), checkCA);
                            SDEMIProcessModel.Result = checkCA;
                            return this.View(SDEMIProcessModel);
                        }

                        var sdDetails = SapPiService.Services.RequestHandler.FetchSecurityDepositAmountDetails(CAnumber);

                        ////for testing
                        //sdDetails.Message = "";
                        //sdDetails.IsSuccess = true;

                        if (!sdDetails.IsSuccess || (sdDetails.IsSuccess && !string.IsNullOrEmpty(sdDetails.Message)))
                        {
                            ModelState.AddModelError(nameof(SDEMIProcessModel.CANumber), sdDetails.Message);
                            return this.View(SDEMIProcessModel);
                        }

                        //Get Mobile number
                        var registeredMobileNumber = SapPiService.Services.RequestHandler.GetMobileNumber(CAnumber);
                        if (string.IsNullOrEmpty(registeredMobileNumber))
                        {
                            ModelState.AddModelError(nameof(SDEMIProcessModel.CANumber), DictionaryPhraseRepository.Current.Get("/Accounts/PANUpdate/Mobile Number incorrect", "Mobile Number is not registered."));
                            return this.View(SDEMIProcessModel);
                        }

                        SDEMIProcessModel.MobileNumber = string.IsNullOrEmpty(registeredMobileNumber) ? registeredMobileNumber : registeredMobileNumber.Substring(0, 2) + "xxxxxxx" + registeredMobileNumber.Substring(registeredMobileNumber.Length - 3);
                        SDEMIProcessModel.CANumber = CAnumber;
                        SDEMIProcessModel.IsvalidatAccount = true;
                        Session["SDInstOptModel"] = SDEMIProcessModel;
                        //SDEMIProcessModel.IsOTPSend = false;
                    }
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
        public ActionResult SDInstallmentOptInrevamp(SDEMIProcess model, string ValidateCA = null, string SendOTP = null, string ValidateOTP = null, string Submit = null)
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
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Please validate captcha to continue"));
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue"));
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
                    model.MobileNumber = string.IsNullOrEmpty(registeredMobileNumber) ? registeredMobileNumber : registeredMobileNumber.Substring(0, 2) + "xxxxxxx" + registeredMobileNumber.Substring(registeredMobileNumber.Length - 3);
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
                         "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Welcome to Adani Electricity. You have initiated a Security Deposit installment Opt-In request for Account no {1} OTP for validation is {2}&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707165158092915132"), registeredMobileNumber, model.CANumber, generatedotp);
                        //"https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Welcome to Adani Electricity. You have initiated a SD installment Opt-In request for Account no {1}. OTP for validation is {2}&dlt_peid=1001756674011432608&dlt_tmid=120210000043&dlt_templateid=1707162046256235660"), registeredMobileNumber, model.CANumber, generatedotp);

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
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), "Please validate captcha to continue.");
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), "Please validate captcha to continue.");
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
                    model.SecurityDepositAmount = System.Convert.ToDecimal(sdDetails.SDAmount);

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
                                NumberOfInstallments = System.Convert.ToInt32(model.SelectedNumberOfInstalments),
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
                        this.Session["SDInstalmentUpdateMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/SDInstOptModel/Registration/SuccessHeading", "Success"), resultMessage.Message, InfoMessageRevamp.MessageTypeRevamp.Success, true);
                        ViewBag.Message = resultMessage.Message;
                        Session["Message"] = resultMessage.Message;
                        return this.Redirect(this.Request.RawUrl);
                    }
                }
            }
            return this.View(model);
        }
        #endregion

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
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);
            string PendingUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentPendingRevamp);

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

        #region SafeXPay Integration
        public ActionResult SafeXPayCallBack()
        {
            var FailureUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            var SuccessUrl = this.UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);

            try
            {
                #region Variable Initialization
                string checksum = string.Empty;
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_Merchant_ID].Value;
                string MerchantEncryptionKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_Merchant_Encryption_Key].Value;
                #endregion

                //SafeXPay Response
                AesSafeXpay objAes = new AesSafeXpay();
                string enc_txn_response = (!String.IsNullOrEmpty(Request.Params["txn_response"])) ? Request.Params["txn_response"] : string.Empty;
                string enc_pg_details = (!String.IsNullOrEmpty(Request.Params["pg_details"])) ? Request.Params["pg_details"] : string.Empty;
                string enc_fraud_details = (!String.IsNullOrEmpty(Request.Params["fraud_details"])) ? Request.Params["fraud_details"] : string.Empty;
                string enc_other_details = (!String.IsNullOrEmpty(Request.Params["other_details"])) ? Request.Params["other_details"] : string.Empty;

                string txn_response = objAes.decrypt(enc_txn_response, MerchantEncryptionKey);
                Sitecore.Diagnostics.Log.Info("Payment Gateway - SafeXPayCallBack Callback Message - " + txn_response, this);


                string[] txn_hash = txn_response.Split('~');

                if (Request.Params.Count > 0 && txn_hash.Length > 0)
                {

                    string txn_res_hash = txn_hash[1];

                    string txn_res_actual = txn_hash[0] + "" + txn_hash[2];


                    string[] responselist = txn_res_actual.Split('|');

                    string Hash = responselist[10] + "~" + responselist[1] + "~" + responselist[2] + "~" + responselist[3] + "~" + responselist[4] + "~" + responselist[5] + "~" + responselist[8];
                    string hashing = objAes.ComputeSha256Hash(Hash);
                    string encHash = objAes.encrypt(hashing, MerchantEncryptionKey);


                    string genuine = "genuine";
                    string fake = "fake";

                    string protocol = "";

                    if (txn_res_hash == encHash)
                    {
                        protocol = genuine;
                    }
                    else
                    {
                        protocol = fake;
                    }

                    string other_details = objAes.decrypt(enc_other_details, MerchantEncryptionKey);
                    string[] other_details_arr = other_details.Split('|');
                    string udf_1 = (!String.IsNullOrEmpty(other_details_arr[0])) ? other_details_arr[0] : string.Empty;
                    string udf_2 = (!String.IsNullOrEmpty(other_details_arr[1])) ? other_details_arr[1] : string.Empty;

                    string pg_details = objAes.decrypt(enc_pg_details, MerchantEncryptionKey);

                    string paymode = string.Empty;
                    string PaymentModedata = string.Empty;
                    if (!String.IsNullOrEmpty(pg_details))
                    {
                        string[] pg_details_arr = pg_details.Split('|');
                        string pg_id = (!String.IsNullOrEmpty(pg_details_arr[0])) ? pg_details_arr[0] : string.Empty;
                        string pg_name = (!String.IsNullOrEmpty(pg_details_arr[1])) ? pg_details_arr[1] : string.Empty;
                        paymode = (!String.IsNullOrEmpty(pg_details_arr[2])) ? pg_details_arr[2] : string.Empty;
                    }

                    switch (paymode)
                    {
                        case "NB":
                            PaymentModedata = "NETBANKING"; break;
                        case "CC":
                            PaymentModedata = "CREDITCARD"; break;
                        case "DC":
                            PaymentModedata = "DEBITCARD"; break;
                        case "PP":
                            PaymentModedata = "PREPAIDCARD"; break;
                        case "WA":
                            PaymentModedata = "WALLETS"; break;
                        case "PT":
                            PaymentModedata = "WALLETS"; break;
                        case "UP":
                            PaymentModedata = "UPI"; break;
                        case "CE":
                            PaymentModedata = "CREDITCARDEMI"; break;
                        default:
                            PaymentModedata = "";
                            Sitecore.Diagnostics.Log.Error("Method - SafeXPay StorePaymentResponse default Store response:" + "", this);
                            break;

                    }

                    if (txn_res_hash.Equals(encHash)) // Compare Checksum
                    {
                        if (Constants.SafeXPayResponse.SuccessCode.Equals(responselist[11].ToString()))
                        {
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = responselist[8].ToString(),
                                ResponseStatus = Constants.PaymentResponse.Success,
                                Responsecode = responselist[11].ToString(),
                                Remark = Constants.PaymentResponse.Success,
                                PaymentRef = responselist[9].ToString(),
                                OrderId = responselist[2].ToString(),
                                AmountPayable = responselist[3].ToString(),
                                AccountNumber = udf_1,
                                LoginName = udf_1,
                                msg = txn_response + " " + txn_res_actual + " " + pg_details + " " + other_details,
                                PaymentMode = PaymentModedata,
                                PaymentModeNumber = paymode,
                                TransactionDate = responselist[6].ToString() + " " + responselist[7].ToString(),
                                PaymentType = udf_2,
                                PaymentGateway = 11
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);

                            //PI Service Integration


                            TempData["PaymentResponse"] = modelviewpay;

                            Sitecore.Diagnostics.Log.Info("Payment Gateway- SafeXPayCallBack Response - ", txn_res_actual + " " + other_details);
                            return this.Redirect(SuccessUrl);
                        }
                        else
                        {
                            //error response
                            var modelviewpay = new ViewPayBill()
                            {
                                TransactionId = responselist[8].ToString(),
                                ResponseStatus = Constants.PaymentResponse.Failure,
                                Responsecode = responselist[11].ToString(), // ErrorStatus
                                Remark = responselist[12].ToString(), //DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                                PaymentRef = responselist[9].ToString(),
                                OrderId = responselist[2].ToString(),
                                AmountPayable = responselist[3].ToString(),
                                AccountNumber = udf_1,
                                LoginName = udf_1,
                                msg = txn_response + " " + txn_res_actual + " " + pg_details + " " + other_details,
                                PaymentMode = PaymentModedata,
                                PaymentType = udf_2,
                                PaymentModeNumber = paymode,
                                TransactionDate = responselist[6].ToString() + " " + responselist[7].ToString()
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);

                            TempData["PaymentResponse"] = modelviewpay;
                            Sitecore.Diagnostics.Log.Info("Payment Gateway- SafeXPayCallBack Response Failure - " + txn_res_actual + " " + other_details, this);
                            return this.Redirect(FailureUrl);
                        }
                    }
                    else
                    {
                        //Checksum Mismatch
                        var modelviewpay = new ViewPayBill()
                        {
                            TransactionId = responselist[8].ToString(),
                            ResponseStatus = Constants.PaymentResponse.Failure,
                            Responsecode = responselist[11].ToString(), // ErrorStatus
                            Remark = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                            PaymentRef = responselist[9].ToString(),
                            OrderId = responselist[2].ToString(),
                            AmountPayable = responselist[3].ToString(),
                            AccountNumber = udf_1,
                            LoginName = udf_1,
                            msg = txn_response + " " + txn_res_actual + " " + pg_details + " " + other_details,
                            PaymentMode = PaymentModedata,
                            PaymentType = udf_2,
                            TransactionDate = responselist[6].ToString() + " " + responselist[7].ToString()
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        TempData["PaymentResponse"] = modelviewpay;
                        Sitecore.Diagnostics.Log.Info("Payment Gateway- SafeXPayCallBack Response checksum mismatch - " + txn_res_actual + " " + other_details, this);
                        return this.Redirect(FailureUrl);
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- SafeXPayDeskCallBack Response NULL error - " + txn_response, this);
                    return this.Redirect(FailureUrl);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at SafeXPayCallBack - :" + ex.Message, this);
                return this.Redirect(FailureUrl);
            }
        }

        #endregion

        #region CashFreePayment Integration

        [HttpPost]
        public ActionResult CashFree_Payment_Callback()
        {
            Root model = new Root();
            string payload = "";
            try
            {
                StreamReader bodyStream = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
                var timestamp = System.Web.HttpContext.Current.Request.Headers["x-webhook-timestamp"];
                var headersignature = System.Web.HttpContext.Current.Request.Headers["x-webhook-signature"];
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                payload = bodyStream.ReadToEnd();


                model = JsonConvert.DeserializeObject<Root>(payload);
                Sitecore.Diagnostics.Log.Info("Method - CashFree_Payment_Callback request message :" + payload, this);

                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    CashFreeOrderDetail obj = new CashFreeOrderDetail
                    {
                        OrderNo = timestamp,
                        PaymentLink = headersignature,
                        Response_Msg = payload,
                        Created_Date = DateTime.Now,
                        StatusDesc = model.data.order.order_tags.payment_types
                    };
                    dbcontext.CashFreeOrderDetails.InsertOnSubmit(obj);
                    dbcontext.SubmitChanges();
                }

                #region Variable Initialization
                string checksum = string.Empty;
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CashFreeFields.CheckSumKey].Value;
                string KayId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CashFreeFields.CashFree_Key_Id].Value;
                string SecretKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CashFreeFields.CashFree_secret_Key].Value;
                #endregion


                var signedPayload = $"{timestamp}{payload}";
                HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
                var expectedSignature = System.Convert.ToBase64String(hashObject.ComputeHash(Encoding.UTF8.GetBytes(signedPayload)));

                Sitecore.Diagnostics.Log.Info("Method - CashFree_Payment_Callback checksum match :" + expectedSignature + "-" + headersignature, this);

                if (expectedSignature.Equals(headersignature)) // Compare Checksum
                {
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        Accounts.PaymentHistory ctx = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == model.data.order.order_id).FirstOrDefault();

                        if (ctx != null)
                        {
                            // model.data.order_Tags.payment_types = ctx.PaymentType.ToString();
                            Sitecore.Diagnostics.Log.Info("Method - CashFree_Callback record found with order id :" + model.data.order.order_id, this);

                            if (PaymentService.StorePaymentRequestCashFree(model, payload))
                            {
                                Sitecore.Diagnostics.Log.Info("CashFree_Callback Method Called - StorePaymentRequestCity called Successfully " + model.data.order.order_id, this);

                                ViewPayBill modelviewpay = new ViewPayBill()
                                {
                                    ResponseStatus = model.data.payment.payment_status,
                                    Responsecode = model.data.payment.payment_status ?? string.Empty,
                                    Remark = Constants.PaymentResponse.Success,
                                    PaymentRef = model.data.payment.cf_payment_id.ToString(),
                                    AmountPayable = model.data.order.order_amount.ToString(),
                                    AccountNumber = model.data.customer_details.customer_id.ToString(),
                                    msg = payload,
                                    PaymentMode = model.data.payment.payment_group.ToString(),
                                    PaymentType = model.data.order.order_tags.payment_types,
                                    PaymentGateway = (int)EnumPayment.GatewayType.CashFree
                                };
                                TempData["PaymentResponse"] = modelviewpay;
                                TempData.Keep();
                                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Successfully Added." }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                            }
                            else
                            {
                                Sitecore.Diagnostics.Log.Info("CashFree_Callback Method Called - StorePaymentRequestCashFree called Server Error " + model.data.order.order_id, this);
                                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = "Unable to update SAP" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                            }
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Info("CashFree_Callback Method Called - StorePaymentRequestCashFree called record not exists " + model.data.order.order_id, this);
                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.NotFound, IsSuccess = false, Message = "OrderID not exists" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }

                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("Error at CashFree_Payment_Callback CheckSum Mismatch" + model.data.order.order_id.ToString() + " Response: " + payload, this);
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "expectedSignature not match" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at CashFree_Payment_Callback :" + ex.Message + " Response: " + payload, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


        }

        [HttpPost]
        public string CashFree_Payment_Check()
        {
            string FailureUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentFailureRevamp);
            string SuccessUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentSuccessRevamp);
            string PendingUrl = UserProfileService.GetPageURL(Templates.Pages.PaymentPendingRevamp);

            string id = string.Empty;
            id = SessionHelper.UserSession.UserSessionContext.OrderId.ToString();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        Accounts.PaymentHistory PaymentHistoryRecord = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == id && x.Responsecode != null).FirstOrDefault();
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
                                PaymentType = PaymentHistoryRecord.PaymentType.ToString(),
                                PaymentGateway = (int)EnumPayment.GatewayType.CashFree
                            };
                            TempData["PaymentResponse"] = modelviewpay;
                            TempData.Keep();
                            if (PaymentHistoryRecord.Responsecode == "SUCCESS")
                            {
                                return SuccessUrl;
                            }
                            else if (PaymentHistoryRecord.Responsecode == "FAILED")
                            {
                                return FailureUrl;
                            }
                            else
                            {
                                return FailureUrl;
                            }
                        }
                        else
                        {
                            //call again
                            return string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at CashFree payment IsSuccess:" + ex.Message, this);
                    return string.Empty;
                }
            }
            return string.Empty;
        }


        [ChildActionOnly]
        public string CashFreeGenerateOrderId(ViewPayBill model)
        {
            try
            {
                string res = string.Empty;

                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string url = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CashFreeFields.CashFree_Order_Request_URL].Value;

                string apiContentType = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CashFreeFields.CashFree_Content_Type].Value;
                string apiClientId = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CashFreeFields.CashFree_Key_Id].Value;
                string apiSecret = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CashFreeFields.CashFree_secret_Key].Value;
                string apiVersion = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CashFreeFields.CashFree_api_version].Value;

                string amount = string.Empty;
                if (model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
                {
                    if (model.SecurityDepositAmountType == "Actual")
                        amount = model.SecurityDeposit;
                    else
                        amount = model.SecurityDeposit.ToString();
                }
                else
                {
                    amount = model.AmountPayable;
                }

                RootJsonRequest objrootJson = new RootJsonRequest();
                var customerdetails = new CustomerDetails();
                var objordermeta = new OrderMeta();
                var objordertags = new OrderTags();

                customerdetails.customer_id = model.AccountNumber.ToString();
                customerdetails.customer_email = model.Email.ToString();
                customerdetails.customer_phone = model.Mobile.ToString();

                objrootJson.customer_details = customerdetails;
                objordermeta.return_url = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CashFreeFields.CashFree_Return_Url].Value;
                objordermeta.notify_url = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CashFreeFields.CashFree_Notify_Url].Value;

                objrootJson.order_meta = objordermeta;
                objrootJson.order_id = model.OrderId;
                objrootJson.order_amount = amount;
                objrootJson.order_currency = "INR";

                objordertags.payment_types = model.PaymentType;

                objrootJson.order_tags = objordertags;

                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", apiContentType);
                request.AddHeader("x-api-version", apiVersion);
                request.AddHeader("x-client-id", apiClientId);
                request.AddHeader("x-client-secret", apiSecret);
                request.AddJsonBody(JsonConvert.SerializeObject(objrootJson));
                IRestResponse response = client.Execute(request);
                var RootJsondata = JsonConvert.DeserializeObject<RootJson>(response.Content);
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    CashFreeOrderDetail obj = new CashFreeOrderDetail
                    {
                        OrderNo = RootJsondata.order_id,
                        OrderToken = RootJsondata.order_token,
                        AccountNo = RootJsondata.customer_details.customer_id,
                        Response_Msg = response.Content,
                        Created_Date = System.Convert.ToDateTime(DateTime.UtcNow)
                    };
                    dbcontext.CashFreeOrderDetails.InsertOnSubmit(obj);
                    dbcontext.SubmitChanges();
                }
                // TempData["PaymentResponse"] = RootJsondata;
                if (response.IsSuccessful)
                {
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        PaymentHistory ctx = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == RootJsondata.order_id).FirstOrDefault();
                        if (ctx != null)
                        {
                            // Update response in Database
                            ctx.TransactionId = RootJsondata.order_id ?? string.Empty;
                            ctx.Status = string.Empty;  //UPIReference                            
                            ctx.ResponseTime = System.Convert.ToDateTime(DateTime.UtcNow);
                            ctx.PaymentType = model.PaymentType;
                            ctx.GatewayType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/CashFree/Gateway Type", "CashFree");
                            dbcontext.SubmitChanges();
                            Response.Redirect(RootJsondata.payment_link);
                            Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse CashFree  Store response in Database :" + RootJsondata.order_id, this);
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse CashFree  Record not found in database :" + RootJsondata.order_id, this);

                        }
                    }


                }
                else
                {
                    return "";
                }
                return "";
            }

            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at CashFree_Payment_Callback Catch Block:" + ex.Message, this);
                return "";
            }

        }




        #endregion

        #region Submit Your Query
        [HttpGet]
        public ActionResult SubmitYourQuery()
        {
            //{9CECD379-1CB6-4590-A107-3385D8C3C9D0}

            var parent = Sitecore.Context.Database.GetItem("{BC42AAE2-D10F-48B2-8C0F-67386C5D7137}").Children;

            var parentArea = Sitecore.Context.Database.GetItem("{2FF42F30-A483-4DFA-85CD-05F217F0E032}").Children;

            SubmitYourQuery SubmitYourQueryModel = new SubmitYourQuery();

            foreach (var item in parent.ToList())
            {
                TypeofArea objTypeofQueryCategory = new TypeofArea();
                objTypeofQueryCategory.Name = item["Title"];
                objTypeofQueryCategory.Value = item["Title"];
                SubmitYourQueryModel.TypeofQueryCategoryList.Add(objTypeofQueryCategory);
            }

            foreach (var item in parentArea.ToList())
            {
                TypeofArea objTypeofArea = new TypeofArea();
                objTypeofArea.Name = item["Title"];
                objTypeofArea.Value = item["Title"];
                SubmitYourQueryModel.TypeofAreaList.Add(objTypeofArea);
            }
            return this.View(SubmitYourQueryModel);
        }
        [HttpPost]
        [ValidateRenderingId]
        // [RedirectUnauthenticatedCookieTempered]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitYourQuery(SubmitYourQuery model)
        {

            SubmitYourQueryServices objSubmitYourQuery = new SubmitYourQueryServices();
            SubmitYourQuery objsubmitquery = new SubmitYourQuery();
           
            try
            {

                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                    //return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                    //return this.View(model);
                }

                if (!this.ModelState.IsValid)
                {
                    var parent = Sitecore.Context.Database.GetItem("{BC42AAE2-D10F-48B2-8C0F-67386C5D7137}").Children;

                    var parentArea = Sitecore.Context.Database.GetItem("{2FF42F30-A483-4DFA-85CD-05F217F0E032}").Children;

                    //SubmitYourQuery SubmitYourQueryModel = new SubmitYourQuery();

                    foreach (var item in parent.ToList())
                    {
                        TypeofArea objTypeofQueryCategory = new TypeofArea();
                        objTypeofQueryCategory.Name = item["Title"];
                        objTypeofQueryCategory.Value = item["Title"];
                        model.TypeofQueryCategoryList.Add(objTypeofQueryCategory);
                    }

                    foreach (var item in parentArea.ToList())
                    {
                        TypeofArea objTypeofArea = new TypeofArea();
                        objTypeofArea.Name = item["Title"];
                        objTypeofArea.Value = item["Title"];
                        model.TypeofAreaList.Add(objTypeofArea);
                    }
                    return this.View(model);
                }

                if (UserSession.UserSessionContext != null)
                {

                    model.CreatedBy = SessionHelper.UserSession.UserSessionContext.AccountNumber;
                }
                else
                {
                    model.CreatedBy = "";
                }
                var r = objSubmitYourQuery.SubmitYourQueryInsert(model);
                if (r)
                {

                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Submit Your Query Success", "Your Query successfully submitted"), InfoMessage.MessageType.Success);
                }
                else
                {
                    // return this.View(model);
                    this.Session["UpdateMessage"] = new InfoMessage(DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Submit Your Query Error", "Some error in Submit Your Query"), InfoMessage.MessageType.Error);
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Error at SubmitYourQueryInsert - : " + e.Message, this);
            }
            return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.Pages.Submityourquery));
        }

        #endregion

        #region ||** Report Electricity Theft **||
        public ActionResult ReportElectricityTheftFormRevamp()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult ReportElectricityTheftFormRevamp(ReportElectricityTheftRevamp model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/Validation/Captcha", "Please confirm you are not a robot."));
                    return this.View(model);
                }
                ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/Common/CaptchaSecretKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);



                if (!reCaptchaResponse.success)
                {
                    ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/Validation/Captchafailure", "Captcha Validation Failed."));
                    return this.View(model);
                }
                ComplaintRegistrationResponse retResult = new ComplaintRegistrationResponse();
                retResult = SapPiService.Services.RequestHandler.ReportTheftCSOrderCreate(model.Area, model.Name, model.MobileNumber, model.Email, model.ReportTheft);



                if (retResult != null && retResult.IsRegistered)
                {
                    model.isRegistered = retResult.IsRegistered;
                    this.Session["ReportElectricityTheftFormRevampMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/FormAction/SuccessfullHeading", "success"), DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/FormAction/Successfull", "Your form is submitted successfully. Your Complaint Number : #ComplaintNumber#").Replace("#ComplaintNumber#", retResult.ComplaintNumber), InfoMessageRevamp.MessageTypeRevamp.Success, true);
                }
                else
                {
                    model.isRegistered = false;
                    this.Session["ReportElectricityTheftFormRevampMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/FormAction/FailureHeading", "Failed"), DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/FormAction/Failed", "Some Thing Went Wrong"), InfoMessageRevamp.MessageTypeRevamp.Error, true);
                }



                return this.Redirect(this.Request.RawUrl);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ReportElectricityTheftFormRevamp :" + ex.Message + "Stack Trace ReportElectricityTheftFormRevamp : " + ex.StackTrace, this);
                this.Session["ReportElectricityTheftFormRevampMessage"] = new InfoMessageRevamp(DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/FormAction/FailureHeading", "Failed"), DictionaryPhraseRepository.Current.Get("/ReportElectricityTheft/FormAction/Failed", "Some Thing Went Wrong"), InfoMessageRevamp.MessageTypeRevamp.Error, true);
                return this.Redirect(this.Request.RawUrl);
            }
        }
        #endregion

        #region ||** MultipleAccountsRevamp **||
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult MultipleAccountsRevamp()
        {

            #region New Code

            #region variable declaration
            var model = new SwitchAccount();
            var availableitemsinDB = _dbAccountService.GetAccountListbyusername(Context.User.Profile.UserName);
            List<KeyValuePair<string, string>> AccountNumberList = new List<KeyValuePair<string, string>>();
            string primaryAccountNumber = string.Empty;
            List<SwitchAccountComponent> switchAccountsComponetlst = new List<SwitchAccountComponent>();
            int count = 0;
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
                    count = 1;
                    foreach (var item in selecteditems)
                    {
                        //if (!string.Equals(primaryAccountNumber, item.AccountNumber))
                        //{
                        if (!string.IsNullOrEmpty(item.AccountNumber))
                        {
                            SwitchAccountComponent switchAccountComponentItem = new SwitchAccountComponent();
                            switchAccountComponentItem.AccountItemId = item.Id.ToString();
                            switchAccountComponentItem.AccountNumber = item.AccountNumber;
                            if (count <= 10)
                            {
                                switchAccountComponentItem.AccountHolderName = SapPiService.Services.RequestHandler.FetchDetail(item.AccountNumber) != null ? SapPiService.Services.RequestHandler.FetchDetail(item.AccountNumber).Name : "";
                            }
                            AccountNumberList.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.AccountNumber));
                            switchAccountsComponetlst.Add(switchAccountComponentItem);
                            count += 1;
                        }
                        // }
                    }
                }
                model.SwitchAccountList = switchAccountsComponetlst;
                model.AccountList = AccountNumberList;
                model.LoginName = this.UserProfileService.GetLoginName();
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
                    count = 1;
                    foreach (var secondaryAccountItemId in MultipleAccountItemIdList)
                    {
                        //if (!string.Equals(primaryAccountItemId, secondaryAccountItemId))
                        //{
                        secondaryAccountNumber = this.UserProfileService.GetAccountNumberfromItem(secondaryAccountItemId);
                        if (!string.IsNullOrEmpty(secondaryAccountNumber))
                        {
                            SwitchAccountComponent switchAccountComponentItem = new SwitchAccountComponent();
                            switchAccountComponentItem.AccountItemId = secondaryAccountItemId;
                            switchAccountComponentItem.AccountNumber = secondaryAccountNumber;
                            if (count <= 10)
                            {
                                switchAccountComponentItem.AccountHolderName = SapPiService.Services.RequestHandler.FetchDetail(secondaryAccountNumber) != null ? SapPiService.Services.RequestHandler.FetchDetail(secondaryAccountNumber).Name : "";
                            }


                            AccountNumberList.Add(new KeyValuePair<string, string>(secondaryAccountItemId, secondaryAccountNumber));
                            switchAccountsComponetlst.Add(switchAccountComponentItem);
                            count += 1;
                        }
                        //}
                    }
                }
                model.SwitchAccountList = switchAccountsComponetlst;
                model.AccountList = AccountNumberList;
                model.LoginName = this.UserProfileService.GetLoginName();
                model.MasterAccountNumber = GetPrimaryAccountNumber();
            }
            #endregion

            return this.View("MultipleAccountsRevamp", model);
        }
        #endregion
    }
}