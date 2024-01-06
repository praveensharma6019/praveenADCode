using Newtonsoft.Json.Linq;
using Sitecore.AdaniCare.Website.Models;
using Sitecore.AdaniCare.Website.Services;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniCare.Website.Controllers
{
    public class AdaniCareController : Controller
    {

        private IUserProfileService UserProfileService { get; }
        AdaniCaresService objAdaniCaresService = new AdaniCaresService();

        [HttpGet]
        public ActionResult AdaniCareLogin()
        {
            
            UserSession.UserSessionContext = null;
            AdaniCareLoginModel model = new AdaniCareLoginModel();
            return this.View(model);
        }

        [HttpPost]
        [ValidateRenderingId]
        [ValidateAntiForgeryToken]
        public ActionResult AdaniCareLogin(AdaniCareLoginModel LoginInfo, string Validate = null, string ValidateOTP = null, string SendOTP = null, string ValidateOTPForCA = null, string Cancel = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(Cancel))
                {
                    LoginInfo.InputByUser = string.Empty;
                    LoginInfo.IsInputValidated = false;
                    LoginInfo.IsOTPSent = false;
                    return View(LoginInfo);
                }
                //if (!ModelState.IsValid)
                //{
                //    return this.View(LoginInfo);
                //}
                if (!string.IsNullOrEmpty(Validate))
                {
                    if (string.IsNullOrEmpty(LoginInfo.InputByUser))
                    {
                        this.ModelState.AddModelError(nameof(LoginInfo.InputByUser), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/No Input", "Please enter a valid Mobile number or Account Number"));
                        return this.View(LoginInfo);
                    }
                    bool flag = true;
                    try
                    {
                        flag = this.IsReCaptchValid(LoginInfo.reResponse);
                    }
                    catch (Exception ex)
                    {
                        Log.Info(string.Concat("AdaniCareClaimOffer Failed to insert data : ", ex.ToString()), this);
                    }
                    if (!flag)
                    {
                        this.ModelState.AddModelError(nameof(LoginInfo.reResponse), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/reResponse", "Captcha is not valid."));
                        return this.View(LoginInfo);
                    }


                    //1. check assuming entered number is Mobile Number
                    string accountNumber = SapPiService.Services.RequestHandler.ValidateMobileAndGetCA(LoginInfo.InputByUser);
                    if (!string.IsNullOrEmpty(accountNumber))
                    {
                        LoginInfo.IsInputMobileNumber = true;
                        //Send OTP 
                        if (objAdaniCaresService.IsOTPMaxLimitExceed(LoginInfo.InputByUser, accountNumber))
                        {
                            Log.Info("AdaniCareLogin: Number of attempt to get OTP reached for Account Number and Mobile Number." + LoginInfo.InputByUser + ", " + accountNumber, this);
                            this.ModelState.AddModelError(nameof(LoginInfo.InputByUser), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Max5OTPPerCAMobile", "Number of attempt to get OTP reached for Entered Mobile Number."));
                            return this.View(LoginInfo);
                        }

                        #region Generate New Otp for given mobile number and save to database
                        string generatedotp = objAdaniCaresService.GenerateOTP(LoginInfo.InputByUser, accountNumber);
                        #endregion

                        #region Api call to send SMS of OTP
                        try
                        {
                            var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API for registration",
                                "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=Welcome to Adani Cares. You have initiated a Login request for Account no. {1}, OTP for this request is: {2}&intflag=false"), LoginInfo.InputByUser, accountNumber, generatedotp);

                            //var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API for registration",
                            //    "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=Welcome to Adani Electricity. You have initiated a request to change the name on your bill, for Account no. {1}. OTP for validation is {2}&intflag=false"), LoginInfo.InputByUser, accountNumber, generatedotp);

                            HttpClient client = new HttpClient();
                            client.BaseAddress = new Uri(apiurl);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            HttpResponseMessage response = client.GetAsync(apiurl).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                Log.Info("AdaniCareLogin: OTP Api call success for registration", this);
                                //this.ModelState.AddModelError(nameof(LoginInfo.InputByUser), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile numberOTP", "OTP Sent."));
                                LoginInfo.IsInputMobileNumber = true;
                                LoginInfo.IsInputValidated = true;
                                LoginInfo.IsOTPSent = true;
                                return this.View(LoginInfo);
                            }
                            else
                            {
                                Log.Error("OTP Api call failed for registration", this);
                                this.ModelState.AddModelError(nameof(LoginInfo.InputByUser), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile numberOTPFailed", "Unable to send OTP."));
                                LoginInfo.IsInputMobileNumber = true;
                                LoginInfo.IsInputValidated = false;
                                LoginInfo.IsOTPSent = false;
                                return this.View(LoginInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error("OTP Api call failed for registration: " + ex.Message, this);
                            this.ModelState.AddModelError(nameof(LoginInfo.InputByUser), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile numberOTPFailed", "Unable to send OTP."));
                            LoginInfo.IsInputMobileNumber = true;
                            LoginInfo.IsInputValidated = false;
                            LoginInfo.IsOTPSent = false;
                            return this.View(LoginInfo);
                        }
                        #endregion
                    }
                    else
                    {
                        var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(LoginInfo.InputByUser);
                        if (!string.IsNullOrEmpty(consumerDetails.MeterNumber))
                        {
                            LoginInfo.MaskedMobileNumber = consumerDetails.Mobile.Substring(0, 1) + "xxxxxxx" + consumerDetails.Mobile.Substring(consumerDetails.Mobile.Length - 2);
                            LoginInfo.IsInputValidated = true;
                            LoginInfo.IsInputMobileNumber = false;
                            return this.View(LoginInfo);
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(LoginInfo.InputByUser), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Input invalid", "No account found linked with specified input as Mobile number or Account Number, please provide correct details and try again!"));
                            return this.View(LoginInfo);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(ValidateOTP))
                {
                    if (string.IsNullOrEmpty(LoginInfo.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(LoginInfo.OTPNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile number OTP no value", "Enter OTP."));
                        LoginInfo.IsInputValidated = true;
                        LoginInfo.IsInputMobileNumber = true;
                        LoginInfo.IsOTPValid = false;
                        return this.View(LoginInfo);
                    }
                    string accountNumber = SapPiService.Services.RequestHandler.ValidateMobileAndGetCA(LoginInfo.InputByUser);
                    string generatedOTP = objAdaniCaresService.GetOTP(LoginInfo.InputByUser, accountNumber);
                    if (!string.Equals(generatedOTP, LoginInfo.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(LoginInfo.OTPNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                        LoginInfo.IsInputValidated = true;
                        LoginInfo.IsInputMobileNumber = true;
                        LoginInfo.IsOTPValid = false;
                        return this.View(LoginInfo);
                    }
                    else
                    {
                        //get consumer details for associated Account number and preceed
                        //1. get details
                        //2. save in db
                        //3. save in session 
                        //4. continue to home page if authenticated
                        var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(accountNumber);
                        if (string.IsNullOrEmpty(consumerDetails.MeterNumber))
                        {
                            this.ModelState.AddModelError(nameof(LoginInfo.InputByUser), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile number invalid", "Failed to authenticate, please try again!"));
                            return this.View(LoginInfo);
                        }
                        var userdetails = new AdaniCareConsumerDetails
                        {
                            AccountNumber = consumerDetails.CANumber,
                            ConsumerEmail = consumerDetails.Email,
                            ConsumerName = consumerDetails.Name,
                            MobileNumber = consumerDetails.Mobile
                        };
                        bool isSaved = objAdaniCaresService.SaveAuthenticationLog(userdetails);
                        if (isSaved)
                        {
                            UserSession.UserSessionContext = userdetails;
                            return RedirectPermanent(objAdaniCaresService.GetPageURL(Templates.Pages.OffersPage));
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(LoginInfo.InputByUser), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile number invalid", "Failed to authenticate, please try again!"));
                            return this.View(LoginInfo);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(SendOTP))
                {
                    var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(LoginInfo.InputByUser);
                    if (string.IsNullOrEmpty(LoginInfo.MobileNumber))
                    {
                        this.ModelState.AddModelError(nameof(LoginInfo.MobileNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile number validate", "Please enter valid Mobile number"));
                        LoginInfo.IsInputMobileNumber = false;
                        LoginInfo.IsInputValidated = true;
                        LoginInfo.IsOTPValid = false;
                        return this.View(LoginInfo);
                    }
                    if (LoginInfo.MobileNumber.Trim() != consumerDetails.Mobile.Trim())
                    {
                        this.ModelState.AddModelError(nameof(LoginInfo.MobileNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile number compare", "Entered  Mobile number is not correct"));
                        LoginInfo.IsInputMobileNumber = false;
                        LoginInfo.IsInputValidated = true;
                        LoginInfo.IsOTPValid = false;
                        return this.View(LoginInfo);
                    }
                    if (objAdaniCaresService.IsOTPMaxLimitExceed(LoginInfo.MobileNumber, LoginInfo.InputByUser))
                    {
                        Log.Info("AdaniCareLogin: Number of attempt to get OTP reached for Account Number and Mobile Number." + LoginInfo.MobileNumber + ", " + LoginInfo.InputByUser, this);
                        this.ModelState.AddModelError(nameof(LoginInfo.MobileNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Max5OTPPerCAMobile", "Number of attempt to get OTP reached for Account Number and Mobile Number."));
                        LoginInfo.IsInputMobileNumber = false;
                        LoginInfo.IsInputValidated = true;
                        return this.View(LoginInfo);
                    }

                    #region Generate New Otp for given mobile number and save to database
                    string generatedotp = objAdaniCaresService.GenerateOTP(LoginInfo.MobileNumber, LoginInfo.InputByUser);
                    #endregion

                    #region Api call to send SMS of OTP
                    try
                    {
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API for registration",
                               "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=Welcome to Adani Cares. You have initiated a Login request for Account no. {1}, OTP for this request is: {2}&intflag=false"), LoginInfo.MobileNumber, LoginInfo.InputByUser, generatedotp);

                        //var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API for registration",
                        //       "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=Welcome to Adani Electricity. You have initiated a request to change the name on your bill, for Account no. {1}. OTP for validation is {2}&intflag=false"), LoginInfo.MobileNumber, LoginInfo.InputByUser, generatedotp);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("AdaniCareLogin: OTP Api call success for registration", this);
                            //this.ModelState.AddModelError(nameof(LoginInfo.MobileNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile numberOTP", "OTP Sent."));
                            LoginInfo.IsInputMobileNumber = false;
                            LoginInfo.IsInputValidated = true;
                            LoginInfo.IsOTPSent = true;
                            return this.View(LoginInfo);
                        }
                        else
                        {
                            Log.Error("OTP Api call failed for registration", this);
                            this.ModelState.AddModelError(nameof(LoginInfo.MobileNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile numberOTPFailed", "Unable to send OTP."));
                            LoginInfo.IsInputMobileNumber = false;
                            LoginInfo.IsInputValidated = true;
                            LoginInfo.IsOTPSent = false;
                            return this.View(LoginInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("OTP Api call failed for registration: " + ex.Message, this);
                        this.ModelState.AddModelError(nameof(LoginInfo.MobileNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile numberOTPFailed", "Unable to send OTP."));
                        LoginInfo.IsInputMobileNumber = false;
                        LoginInfo.IsInputValidated = true;
                        LoginInfo.IsOTPSent = false;
                        return this.View(LoginInfo);
                    }
                    #endregion
                }
                else if (!string.IsNullOrEmpty(ValidateOTPForCA))
                {
                    if (string.IsNullOrEmpty(LoginInfo.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(LoginInfo.OTPNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile number OTP no value", "Enter OTP."));
                        LoginInfo.IsInputMobileNumber = false;
                        LoginInfo.IsInputValidated = true;
                        LoginInfo.IsOTPSent = true;
                        LoginInfo.IsOTPValid = false;
                        return this.View(LoginInfo);
                    }
                    string generatedOTP = objAdaniCaresService.GetOTP(LoginInfo.MobileNumber, LoginInfo.InputByUser);
                    if (!string.Equals(generatedOTP, LoginInfo.OTPNumber))
                    {
                        this.ModelState.AddModelError(nameof(LoginInfo.OTPNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile numberOTPmismatch", "OTP does not match. Please enter valid OTP."));
                        LoginInfo.IsInputMobileNumber = false;
                        LoginInfo.IsInputValidated = true;
                        LoginInfo.IsOTPSent = true;
                        LoginInfo.IsOTPValid = false;
                        return this.View(LoginInfo);
                    }
                    else
                    {
                        //get consumer details for associated Account number and preceed
                        //1. get details
                        //2. save in db
                        //3. save in session 
                        //4. continue to home page if authenticated
                        var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(LoginInfo.InputByUser);
                        var userdetails = new AdaniCareConsumerDetails
                        {
                            AccountNumber = consumerDetails.CANumber,
                            ConsumerEmail = consumerDetails.Email,
                            ConsumerName = consumerDetails.Name,
                            MobileNumber = consumerDetails.Mobile
                        };
                        bool isSaved = objAdaniCaresService.SaveAuthenticationLog(userdetails);
                        if (isSaved)
                        {
                            UserSession.UserSessionContext = userdetails;
                            return RedirectPermanent(objAdaniCaresService.GetPageURL(Templates.Pages.OffersPage));
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(LoginInfo.MobileNumber), DictionaryPhraseRepository.Current.Get("/LandingPage/Login/Mobile number invalid", "Failed to authenticate, please try again!"));
                            return this.View(LoginInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at ChangeOfNameRegisteration Post " + ex.Message, this);
            }
            return View(LoginInfo);
        }

        [HttpGet]
        public ActionResult AdaniCareOffers()
        {
            try
            {
                if (UserSession.UserSessionContext == null || string.IsNullOrEmpty(UserSession.UserSessionContext.ConsumerName))
                    return RedirectPermanent(objAdaniCaresService.GetPageURL(Templates.Pages.LoginPage));

                AdaniCareConsumerDetails model = new AdaniCareConsumerDetails();
                model = UserSession.UserSessionContext;
                return this.View(model);
            }
            catch (Exception ex)
            {
                Log.Error("AdaniCareOffers error at session check and redirect" + ex.Message, this);
                return RedirectPermanent(objAdaniCaresService.GetPageURL(Templates.Pages.LoginPage));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdaniCareOffers(AdaniCareConsumerDetails model)
        {
            UserSession.UserSessionContext = null;
            return RedirectPermanent(objAdaniCaresService.GetPageURL(Templates.Pages.LoginPage));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdaniCareClaimOfferDirectRedirect(AdaniCareOfferDetails model)
        {
            AdaniCaresService objService = new AdaniCaresService();
            if (UserSession.UserSessionContext == null)
                return RedirectPermanent(objAdaniCaresService.GetPageURL(Templates.Pages.LoginPage));

            AdaniCareConsumerDetails userdetails = UserSession.UserSessionContext;
            model.ClaimEmailAddress = userdetails.ConsumerEmail;
            //model.OfferId = model.OfferCode;
            var result = objService.SaveClaimDetails(userdetails, model, false);
            if (!string.IsNullOrEmpty(model.OfferLink))
                return RedirectPermanent(model.OfferLink);
            else
                return RedirectPermanent(objAdaniCaresService.GetPageURL(Templates.Pages.ThankYouPage));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdaniCareClaimOffer(AdaniCareOfferDetails model)
        {
            ActionResult actionResult;
            var variable = new { status = "1" };
            Log.Info("Insert AdaniCareClaimOffer data", this);
            bool flag = true;
            try
            {
                flag = this.IsReCaptchValid(model.reResponse);
            }
            catch (Exception ex)
            {
                Log.Info(string.Concat("AdaniCareClaimOffer Failed to insert data : ", ex.ToString()), this);
            }
            if (!flag)
            {
                return View(model);
            }

            if (!ModelState.IsValid)
                return View(model);

            AdaniCaresService objService = new AdaniCaresService();
            if (UserSession.UserSessionContext == null)
                return RedirectPermanent(objAdaniCaresService.GetPageURL(Templates.Pages.LoginPage));

            AdaniCareConsumerDetails userdetails = UserSession.UserSessionContext;
            var result = objService.SaveClaimDetails(userdetails, model, true);
            if (!string.IsNullOrEmpty(model.OfferLink))
                return RedirectPermanent(model.OfferLink);
            else
                return RedirectPermanent(objAdaniCaresService.GetPageURL(Templates.Pages.ThankYouPage));

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



    }

}
