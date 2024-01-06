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
    using static Sitecore.Feature.Accounts.Controllers.AccountsController;
    using System.Drawing.Imaging;
    using Sitecore.Feature.Accounts.UploadDocuments.AEMLDocuments;
    using Sitecore.Feature.Accounts.Validation;
    using System.ComponentModel.DataAnnotations;

    public class AEMLNewConnectionController : Controller
    {
        public AEMLNewConnectionController(IAccountRepository accountRepository, INotificationService notificationService, IAccountsSettingsService accountsSettingsService, IGetRedirectUrlService getRedirectUrlService, IUserProfileService userProfileService, IFedAuthLoginButtonRepository fedAuthLoginRepository, IUserProfileProvider userProfileProvider, IPaymentService paymentService, IDbAccountService dbAccountService)
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

        [HttpGet]
        public ActionResult NewConnectionRegistration()
        {
            NewConnectionRegistrationModel model = new NewConnectionRegistrationModel();
            model.IsvalidatAccount = false;
            model.IsOTPSent = false;
            return this.View(model);
        }




        [HttpPost]
        [ValidateRenderingId]
        public ActionResult NewConnectionRegistration(NewConnectionRegistrationModel registrationInfo, string ValidateAccount = null, string sendOTP = null, string ValidateOTP = null, string submit = null, string CheckApplication = null)
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
                        registrationInfo.IsvalidatAccount = true;
                        #region Delete Available otp from database for given mobile number
                        RegistrationRepository registrationRepo = new RegistrationRepository();
                        registrationRepo.DeleteOldOtp(registrationInfo.MobileNo);
                        #endregion

                        #region Generate New Otp for given mobile number and save to database
                        var moelRegisteredValidateAccount = new RegisteredValidateAccount
                        {
                            MobileNo = registrationInfo.MobileNo

                        };
                        string generatedotp = registrationRepo.StoreGeneratedOtp(moelRegisteredValidateAccount);
                        #endregion

                        #region Api call to send SMS of OTP
                        try
                        {
                            var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ChangeOfName/SMS API for registration",
                                  "https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=relmobotp&subEnterpriseid=relmobotp&pusheid=relmobotp&pushepwd=relmobotp1&msisdn={0}&sender=ADANIE&msgtext=Welcome to Adani Electricity. Your OTP, for Mobile no. {1}. is {2}&intflag=false"), registrationInfo.MobileNo, registrationInfo.MobileNo, generatedotp);

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
                                Session["NewConnectionApplication"] = registrationInfo;
                                return this.View(registrationInfo);
                            }
                            else
                            {
                                Log.Error("CON OTP Api call failed for registration", this);
                                this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                                registrationInfo.IsOTPSent = false;
                                registrationInfo.IsvalidatAccount = false;
                                return this.View(registrationInfo);
                            }

                        }

                        catch (Exception ex)
                        {
                            Log.Error("CON OTP Api call failed for registration, catch" + ex.Message, this);
                            this.ModelState.AddModelError(nameof(registrationInfo.MobileNo), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Mobile numberOTPFailed", "Unable to send OTP."));
                            registrationInfo.IsOTPSent = false;
                            registrationInfo.IsvalidatAccount = false;
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

                        Log.Info("ChangeOfNameRegisteration redirecting to Application form for CA:" + registrationInfo.MobileNo, this);

                        return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.NewConnection.NewConnectionHomePage));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at ChangeOfNameRegisteration Post " + ex.Message, this);
            }
            return View(registrationInfo);
        }

        public bool IsCorrectMobileNumber(String strNumber)
        {
            Regex mobilePattern = new Regex(@"^[0-9]{10}$");
            return mobilePattern.IsMatch(strNumber);
        }



        [HttpGet]
        public ActionResult NewConnectionIndividualConnectionForm()
        {


            NewConnectionService newconnectionservice = new NewConnectionService();
            NewConnectionApplication model = new NewConnectionApplication();

            try
            {
                if (Session["LECUserLogin"] == null)
                {
                    model.MobileNo = UserSession.AEMLNewConnectionSessionContext.MobileNo;
                }
            }
            catch (Exception e)
            {
                Log.Error("Complaint portal home " + e.Message, this);



                var item = Context.Database.GetItem(Templates.NewConnection.LoginPage);



                return this.Redirect(item.Url());
            }




            string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                var connectionData = newconnectionservice.GetApplicationConnectionDetail(id);

                model.Id = connectionData.Id;
                model.ApplicationNo = connectionData.ApplicationNumber;
                model.selectedGovernmentType = connectionData.GovernmentType;
                model.ApplicationTitle = connectionData.ApplicationTitle;
                model.FirstName = connectionData.FirstName;
                model.MiddleName = connectionData.MiddleName;
                model.LastName = connectionData.LastName;
                model.OrganizationName = connectionData.OrganizationName;
                model.Name1Joint = connectionData.Name1Joint;
                model.Name2Joint = connectionData.Name2Joint;
                model.Name1JointDateofBirth = connectionData.DateOfBirthJoint1;

                if (!string.IsNullOrEmpty(connectionData.AddressOfSupply))
                {
                    var address = connectionData.AddressOfSupply.Split('#');
                    model.FlatNumber = address[0];
                    model.BuildingName = address[1];
                    model.Street = address[2];
                    model.Landmark = address[3];
                }
                model.SelectedSuburb = connectionData.Surburb;
                model.SelectedPincode = connectionData.Pincode;

                //Billing Address
                model.billingdifferentthanAddresswheresupply = connectionData.BillingAddressDifferentFromSupply;
                if (!string.IsNullOrEmpty(connectionData.BillingAddress))
                {
                    var address = connectionData.BillingAddress.Split('#');
                    model.ApplicantCorrespondenceFlatNumber = address[0];
                    model.ApplicantCorrespondenceBuildingName = address[1];
                    model.ApplicantCorrespondenceStreet = address[2];
                    model.ApplicantCorrespondenceLandmark = address[3];
                }
                model.ApplicantCorrespondenceSuburb = connectionData.BillingSuburb;
                model.ApplicantCorrespondencePincode = connectionData.BillingPincode;

                //Rental Address
                model.AddressInCaseOfRental = connectionData.AddressIncaseOfRental;
                model.RentalNameoftheOwner = connectionData.NameOfRental;
                model.RentalContactNumber = connectionData.RentalContactNo;
                if (!string.IsNullOrEmpty(connectionData.AddressOfRental))
                {
                    var address = connectionData.AddressOfRental.Split('#');
                    model.RentalFlatNumber = address[0];
                    model.RentalBuildingName = address[1];
                    model.RentalStreet = address[2];
                }
                model.RentalSuburb = connectionData.RentalSuburb;
                model.RentalPincode = connectionData.RentalPincode;
                model.RentalOwnerEmail = connectionData.RentalOwnerEmail;


                model.MobileNo = connectionData.MobileNumber;
                model.Email = connectionData.Email;
                model.LandlineNo = connectionData.LandlineNumber;
                model.BillLanguage = connectionData.BillLanguage;
                model.BillFormat = connectionData.BillingFormat;


                model.Bank = connectionData.Bank;
                model.MICR = connectionData.MICR;
                model.BankAccountNumber = connectionData.BankAccountNumber;
                model.Branch = connectionData.Branch;
                model.BankAccountType = connectionData.TypeOfAccount;

                //New Connection Detail
                model.ApplicantType = connectionData.IsJoint == true ? "0" : "1";
                model.ApplicationType = connectionData.ApplicationType;
                model.ApplicationSubType = connectionData.ApplicationSubType;
                model.IsExistingCustomer = connectionData.IsExistingCustomer;
                model.CANumber = connectionData.CANumber;
                model.ApplicanttType = connectionData.IsJoint == true ? "1" : "2";
                model.IsApplicantType = connectionData.ApplicantType == true ? "1" : "0";
                model.hdnStatus = connectionData.ApplicationModel;
                model.Status = connectionData.Status.ToString();
                model.ApplicationMode = connectionData.ApplicationModel;
                AEMLNewConnectionLibrary.GetApplicationSubtypeList(model);
                if (model.ApplicationsubtypeList == null)
                    model.ApplicationsubtypeList = new List<SelectListItem>();

                //Application Detail
                model.DateofBirth = connectionData.DateOfBirth;
                model.IsGreenTariffApplied = (connectionData.IsGreenTariffApplied.Value ? "1" : "0");
                model.GreenTariff = connectionData.GreenTariff;
                model.IsSez = (connectionData.IsSEZ != null && connectionData.IsSEZ.Value ? "1" : "0");

                //Load Detail

                var connectionLoadData = newconnectionservice.GetApplicationLoadDetailConnectionDetail(model.ApplicationNo);
                if (connectionLoadData != null && (!string.IsNullOrEmpty(connectionLoadData.ConnectedLoadKW) || !(string.IsNullOrEmpty(connectionLoadData.ConnectedLoadHP))))
                {
                    model.ConnectedLoadKW = connectionLoadData.ConnectedLoadKW;
                    model.ConnectedLoadHP = connectionLoadData.ConnectedLoadHP;
                    ViewData["TotalLoad"] = connectionLoadData.TotalLoad;
                    model.TotalLoad = System.Convert.ToDouble(connectionLoadData.TotalLoad);
                    model.SelectedConnectionType = connectionLoadData.ConnectionType;
                    model.VoltageLevel = connectionLoadData.VoltageLevel;
                    model.PurposeOfSupply = connectionLoadData.PurposeOfSupply;
                    model.MeterLoad = connectionLoadData.MeterLoad;
                    model.MeterType = connectionLoadData.MeterType;
                    model.PremiseType = connectionLoadData.PremiseType;
                    model.ContractDemand = connectionLoadData.ContractDemand;
                    model.ConnectionTypeCode = connectionLoadData.ConnectionCode;
                }

                //Other Detail
                var connectionOtherData = newconnectionservice.GetApplicationOtherDetailConnectionDetail(model.ApplicationNo);
                if (connectionOtherData != null && !string.IsNullOrEmpty(connectionOtherData.IsMeterConnectionCabinExist))
                {
                    model.Meterconnectioninexistingmetercabin = connectionOtherData.IsMeterConnectionCabinExist;
                    model.NearestConsumerAccountNo = connectionOtherData.NearestConsumerAccountNo;
                    model.NearestConsumerMeterNo = connectionOtherData.NearestConsumerMeterNo;
                    model.ExistingConnection = connectionOtherData.ExistingConnection;
                    model.ConsumerNo = connectionOtherData.ConsumerNo;
                    model.Utility = connectionOtherData.Utility;
                    model.WiringCompleted = connectionOtherData.WiringCompleted;
                    model.LECNumber = connectionOtherData.LECNumber;
                    model.NameOnLEC = connectionOtherData.NameOnLEC;
                    model.LECEmail = connectionOtherData.LECEmail;
                    model.LECLandlineNo = connectionOtherData.LECLandline;
                    model.LECMobileNo = connectionOtherData.LECMobileNumber;
                    model.TRNumber = connectionOtherData.TR_NO;
                    model.metersupplier = (connectionOtherData.IS_METER_SUPPLIER != null && connectionOtherData.IS_METER_SUPPLIER.Value) ? "AEML" : "SELF";
                }

                AEMLNewConnectionLibrary.GetApplicationDetail(newconnectionservice, model);


                AEMLUploadDocumentLibrary.GetApplicationDocumentList(model);
            }
            else
            {
                model.AccountNo = "123456789";
                model.Id = Guid.NewGuid();
                model.hdnID = model.Id;
                PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext();
                NEW_CON_APPLICATION_FORM obj = new NEW_CON_APPLICATION_FORM();
                var lstAreaGen = dataContext.NEW_CON_APPLICATION_FORMs.OrderBy(o => o.Id).Distinct().ToList();
                var c = lstAreaGen.Count;

                var tempnumber = "N" + c.ToString().PadLeft(9, '0');
                model.ApplicationNo = tempnumber;
                model.Status = "1";
                model.hdnStatus = model.Status;
                model.ApplicationsubtypeList = new List<SelectListItem>();
            }
            ////Shekhar Gigras Upload Document - Start
            //#region doc section
            ////GetApplicationDocumentList(model);
            //model.GetExistingDocuments = AEMLUploadDocumentLibrary.GetExistingDocument("");
            //#endregion
            ////Shekhar Gigras Upload Document - End
            return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);


        }
        [HttpPost]
        public ActionResult NewConnectionIndividualConnectionForm(NewConnectionApplication model, string SubmitApplication)
        {
            bool isValid = true;
            NewConnectionService newconnectionservice = new NewConnectionService();
            //model.IsApplicantType = (model.IsApplicantType == null ? "1" : model.IsApplicantType);
            //model.ApplicantType = (model.ApplicantType == null ? "1" : model.ApplicanttType);
            //model.ApplicationTitle = (model.ApplicationTitle == null ? "" : model.ApplicationTitle);
            if (Session["LECUserLogin"] == null)
            {
                model.MobileNo = UserSession.AEMLNewConnectionSessionContext.MobileNo;
            }
            AEMLNewConnectionLibrary.GetApplicationSubtypeList(model);
            if (model.ApplicationsubtypeList == null)
                model.ApplicationsubtypeList = new List<SelectListItem>();
            ViewData["TotalLoad"] = "0";

            model.GetExistingDocuments = new List<NEW_CON_APPLICATION_DOCUMENT_DETAIL>();
            AEMLNewConnectionLibrary.GetSuburbList(model.SelectedPincode, model);
            string CAMessage = AEMLNewConnectionLibrary.ValidateCANumber(model);
            if (!string.IsNullOrEmpty(CAMessage) && model.IsExistingCustomer == "Yes")
            {

                this.ModelState.AddModelError(nameof(model.CANumber), CAMessage);

                return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
            }

            if (model.IsApplicantType == "0" && string.IsNullOrEmpty(model.selectedGovernmentType))
            {
                ModelState.AddModelError(nameof(model.selectedGovernmentType), DictionaryPhraseRepository.Current.Get("/Accounts/NewCon/GovernmentType", "Please select government type"));
                return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
            }

            string bankMessage = AEMLNewConnectionLibrary.GetBankDetail(model);
            if (!string.IsNullOrEmpty(bankMessage))
            {
                this.ModelState.AddModelError(nameof(model.MICR), bankMessage);
                return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
            }

            if ((!string.IsNullOrEmpty(model.FirstName)) || (!string.IsNullOrEmpty(model.LastName)) || (!string.IsNullOrEmpty(model.MiddleName)))
            {
                string name = model.FirstName + " " + model.MiddleName + " " + model.LastName;
                model.BankHoldersName = name.ToUpper();
            }



            model.IsSez = (model.IsSez == null ? "0" : model.IsSez);



            if (!string.IsNullOrEmpty(model.ConnectedLoadHP) || ((!string.IsNullOrEmpty(model.ConnectedLoadKW))))
            {
                if (!string.IsNullOrEmpty(model.ConnectedLoadKW) && (!Regex.IsMatch(model.ConnectedLoadKW, @"^((\d+)((\.\d{1,3})?))$")))
                {
                    ViewData["TotalLoad"] = 0;
                    ((dynamic)base.ViewBag).Message = "Valid Decimal number with maximum 3 decimal places is valid";
                    return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
                }
                else if (!string.IsNullOrEmpty(model.ConnectedLoadHP) && (!Regex.IsMatch(model.ConnectedLoadHP, @"^((\d+)((\.\d{1,3})?))$")))
                {
                    ViewData["TotalLoad"] = 0;
                    ((dynamic)base.ViewBag).Message = "Decimal number with maximum 3 decimal places is valid";
                    return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
                }

                double TotalLoad = AEMLNewConnectionLibrary.CalculateTotalLoad(model);
                ViewData["TotalLoad"] = TotalLoad;
                if ((model.ApplicationSubType == "1") || (model.ApplicationSubType == "2"))
                {
                    if (TotalLoad <= 0 || TotalLoad > 99999)
                    {
                        ViewData["TotalLoad"] = model.TotalLoad;

                        ((dynamic)base.ViewBag).Message = "Please enter a valid load";
                        return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
                    }
                    else
                    {
                        AEMLNewConnectionLibrary.GetConnectionTypeList(newconnectionservice, model, TotalLoad);
                        AEMLNewConnectionLibrary.GetVoltageLevel(newconnectionservice, model, model.TotalLoad);
                        AEMLNewConnectionLibrary.GetPurposeOfSupply(newconnectionservice, model);
                        AEMLNewConnectionLibrary.GetMeterType(newconnectionservice, model);

                    }
                }
                else if (model.ApplicationSubType == "3")
                {
                    if (TotalLoad <= 100 || TotalLoad >= 160)
                    {
                        ViewData["TotalLoad"] = model.TotalLoad;

                        ((dynamic)base.ViewBag).Message = "Please enter a valid load";
                        return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
                    }
                    else
                    {
                        AEMLNewConnectionLibrary.GetConnectionTypeList(newconnectionservice, model, TotalLoad);
                        AEMLNewConnectionLibrary.GetVoltageLevel(newconnectionservice, model, model.TotalLoad);
                        AEMLNewConnectionLibrary.GetPurposeOfSupply(newconnectionservice, model);
                        AEMLNewConnectionLibrary.GetMeterType(newconnectionservice, model);

                    }
                }

            }

            if ((!string.IsNullOrEmpty(model.LDPOrderNumber)))
            {

                var LPDOrderNumber = AEMLNewConnectionLibrary.LPDNumber(model);
                if (!string.IsNullOrEmpty(LPDOrderNumber))
                {
                    this.ModelState.AddModelError(nameof(model.LDPOrderNumber), "Please enter a valid LDP Order Number");
                    return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
                }

            }

            if (!string.IsNullOrEmpty(model.NearestConsumerAccountNo))
            {
                string ExistingNumberMessage = AEMLNewConnectionLibrary.ValidateExistingNumber(model);
                if (!string.IsNullOrEmpty(ExistingNumberMessage))
                {
                    this.ModelState.AddModelError(nameof(model.NearestConsumerAccountNo), "Please enter a valid existing CA number");
                    return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
                }
            }
            if (model.Utility == "AEML")
            {
                if (!string.IsNullOrEmpty(model.ConsumerNo))
                {
                    var ConsumerNumberMessage = AEMLNewConnectionLibrary.ValidateConsumerNumber(model);
                    if (!string.IsNullOrEmpty(ConsumerNumberMessage))

                    {
                        this.ModelState.AddModelError(nameof(model.ConsumerNo), "Please enter a valid Consumer number");
                        return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
                    }
                }
            }

            if (!string.IsNullOrEmpty(model.LECNumber))
            {
                var LECMessage = AEMLNewConnectionLibrary.GetLECDetails(model);
                if (!string.IsNullOrEmpty(LECMessage))
                {
                    this.ModelState.AddModelError(nameof(model.LECNumber), "Please enter a valid LEC number");
                    return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
                }
            }

            if ((!string.IsNullOrEmpty(model.TRNumber)) && (!string.IsNullOrEmpty(model.LECNumber)))
            {
                var trmessage = AEMLNewConnectionLibrary.ValidateTRNumber(model);
                if (!string.IsNullOrEmpty(trmessage))
                {

                    this.ModelState.AddModelError(nameof(model.TRNumber), "Please enter a valid TR Number");
                    return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
                }
            }
            model.Id = model.hdnID;
            Session["ID"] = model.Id;
            //   model.Id = SessionHelper.UserSession.AEMLNewConnectionSessionUserID.Id;



            model.DedicatedDistributionfacilityRs = AEMLNewConnectionLibrary.CalculateNormativeCharge(model);



            if ((!string.IsNullOrEmpty(model.ApplicantType)) && (!string.IsNullOrEmpty(model.ApplicationTitle)) && (!string.IsNullOrEmpty(model.IsApplicantType)))
            {


                model.Status = "1";
                AEMLUploadDocumentLibrary.GetApplicationDocumentList(model);
            }

            model.ApplicationMode = (string.IsNullOrEmpty(model.ApplicationMode) ? (Request.Form["SaveApplicationDetail"] != null ? "SAD" : model.ApplicationMode) : model.ApplicationMode);
            model.ApplicationMode = (string.IsNullOrEmpty(model.ApplicationMode) ? (Request.Form["SaveLoadDetail"] != null ? "SLD" : model.ApplicationMode) : model.ApplicationMode);
            model.ApplicationMode = (string.IsNullOrEmpty(model.ApplicationMode) ? (Request.Form["SaveOtherDetail"] != null ? "SOD" : model.ApplicationMode) : model.ApplicationMode);
            model.ApplicationMode = (string.IsNullOrEmpty(model.ApplicationMode) ? (Request.Form["SaveDocumentDetail"] != null ? "SDD" : model.ApplicationMode) : model.ApplicationMode);



            if (Request.Form["SaveApplicationDetail"] != null)
            {


                if ((model.DateofBirth != null) && (ModelState["DateofBirth"].Errors.Any()))
                {
                    this.ModelState.AddModelError(nameof(model.DateofBirth), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Date Of Birth"));
                    //return this.View(model);
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(model.LandlineNo) && (!Regex.IsMatch(model.LandlineNo, (@"^\d{6,9}$"))))
                {
                    this.ModelState.AddModelError(nameof(model.LandlineNo), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Landline Number"));
                    //return this.View(model);
                    isValid = false;
                }

                if (!string.IsNullOrEmpty(model.FirstName) && (!Regex.IsMatch(model.FirstName, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                {
                    this.ModelState.AddModelError(nameof(model.FirstName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a First Name"));
                    //return this.View(model);
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(model.MiddleName) && (!Regex.IsMatch(model.MiddleName, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                {
                    this.ModelState.AddModelError(nameof(model.MiddleName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Middle Name"));
                    //return this.View(model);
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(model.LastName) && (!Regex.IsMatch(model.LastName, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                {
                    this.ModelState.AddModelError(nameof(model.LastName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Last Name"));
                    //return this.View(model);
                    isValid = false;
                }
                if (model.ApplicanttType == "2")
                {
                    if (!string.IsNullOrEmpty(model.Name1Joint) && (!Regex.IsMatch(model.Name1Joint, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                    {
                        this.ModelState.AddModelError(nameof(model.Name1Joint), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Name"));
                        //return this.View(model);
                        isValid = false;
                    }
                    if (!string.IsNullOrEmpty(model.Name2Joint) && (!Regex.IsMatch(model.Name2Joint, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                    {
                        this.ModelState.AddModelError(nameof(model.Name2Joint), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Name"));
                        //return this.View(model);
                        isValid = false;
                    }
                    if (model.Name1JointDateofBirth != null && (ModelState["Name1JointDateofBirth"].Errors.Any()))
                    {
                        this.ModelState.AddModelError(nameof(model.Name1JointDateofBirth), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Date of Birth"));
                        //return this.View(model);
                        isValid = false;
                    }
                }
                if (model.BankAccountNumberConfirm != null && (ModelState["BankAccountNumberConfirm"].Errors.Any()))
                {
                    this.ModelState.AddModelError(nameof(model.BankAccountNumberConfirm), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Bank Account No and Confirm Bank Account number should be same"));
                    //return this.View(model);
                    isValid = false;
                }
                if (model.ApplicationTitle == "0006")
                {

                    if (!string.IsNullOrEmpty(model.OrganizationName) && (!Regex.IsMatch(model.OrganizationName, (@"^[a-zA-Z]([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                    {
                        this.ModelState.AddModelError(nameof(model.OrganizationName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Organization Name"));
                        //return this.View(model);
                        isValid = false;
                    }
                }
                if (!string.IsNullOrEmpty(model.FlatNumber) && (!Regex.IsMatch(model.FlatNumber, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                {
                    this.ModelState.AddModelError(nameof(model.FlatNumber), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Flat Number"));
                    //return this.View(model);
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(model.BuildingName) && (!Regex.IsMatch(model.BuildingName, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                {
                    this.ModelState.AddModelError(nameof(model.BuildingName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Building Name"));
                    //return this.View(model);
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(model.Street) && (!Regex.IsMatch(model.Street, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                {
                    this.ModelState.AddModelError(nameof(model.Street), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Street Name"));
                    //return this.View(model);
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(model.Landmark) && (!Regex.IsMatch(model.Landmark, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                {
                    this.ModelState.AddModelError(nameof(model.Landmark), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Landmark"));
                    //return this.View(model);
                    isValid = false;
                }
                if (model.billingdifferentthanAddresswheresupply == "Yes")
                {
                    if (!string.IsNullOrEmpty(model.ApplicantCorrespondenceFlatNumber) && (!Regex.IsMatch(model.ApplicantCorrespondenceFlatNumber, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                    {
                        this.ModelState.AddModelError(nameof(model.ApplicantCorrespondenceFlatNumber), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Falt Number"));
                        //return this.View(model);
                        isValid = false;
                    }
                    if (!string.IsNullOrEmpty(model.ApplicantCorrespondenceBuildingName) && (!Regex.IsMatch(model.ApplicantCorrespondenceBuildingName, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                    {
                        this.ModelState.AddModelError(nameof(model.ApplicantCorrespondenceBuildingName), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Building Name"));
                        //return this.View(model);
                        isValid = false;
                    }
                    if (!string.IsNullOrEmpty(model.ApplicantCorrespondenceStreet) && (!Regex.IsMatch(model.ApplicantCorrespondenceStreet, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                    {
                        this.ModelState.AddModelError(nameof(model.ApplicantCorrespondenceStreet), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Street"));
                        //return this.View(model);
                        isValid = false;
                    }
                    if (!string.IsNullOrEmpty(model.ApplicantCorrespondenceLandmark) && (!Regex.IsMatch(model.ApplicantCorrespondenceLandmark, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                    {
                        this.ModelState.AddModelError(nameof(model.ApplicantCorrespondenceLandmark), DictionaryPhraseRepository.Current.Get("/CON/Controller Messages/HouseNumber none", "Please enter a valid Landmark"));
                        //return this.View(model);
                        isValid = false;
                    }



                }
                if (isValid == false)
                {

                    return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
                }
                else
                {
                    PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext();
                    if (model.ApplicationMode == "SAD" && Request.Form["SaveApplicationDetail"] != null)
                    {
                        model.ApplicationMode = "SAD";
                    }
                    AEMLNewConnectionLibrary.SaveNewConnectionApplicationInfo(dataContext, model);
                }

            }

            if (Request.Form["SaveLoadDetail"] != null)
            {
                PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext();
                if (model.ApplicationMode == "SAD" && Request.Form["SaveLoadDetail"] != null)
                {
                    model.ApplicationMode = "SLD";
                }
                AEMLNewConnectionLibrary.SaveNewConnectionLoadDetailInfo(dataContext, model);

            }
            if (Request.Form["SaveOtherDetail"] != null)
            {
                PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext();
                if (model.ApplicationMode == "SLD" && Request.Form["SaveOtherDetail"] != null)
                {
                    model.ApplicationMode = "SOD";
                }
                if (Session["LECUserLogin"] != null)
                {
                    ChangeOfNameService conService = new ChangeOfNameService();
                    ChangeOfNameLECUserProfileModel lecmodel = conService.GetLECDetails(UserSession.AEMLCONLECUserSessionContext.RegistrationNumber);
                    model.LECEmail = lecmodel.LECEmailId;
                    model.LECNumber = lecmodel.LECRegistrationNumber;
                    model.NameOnLEC = lecmodel.LECName;
                    model.LECMobileNo = lecmodel.LECMobileNumber;
                    model.LECLandlineNo = "";
                }
                AEMLNewConnectionLibrary.SaveNewConnectionOtherInfo(dataContext, model);

            }
            model.ApplicationMode = (string.IsNullOrEmpty(model.ApplicationMode) ? "" : model.ApplicationMode);
            //model.ApplicationMode = (Request.Form["SaveApplicationDetail"] != null ? "SAD" : model.ApplicationMode);
            //model.ApplicationMode = (Request.Form["SaveLoadDetail"] != null ? "SLD" : model.ApplicationMode);
            //model.ApplicationMode = (Request.Form["SaveOtherDetail"] != null ? "SOD" : model.ApplicationMode);
            //model.ApplicationMode = (Request.Form["SaveDocumentDetail"] != null ? "SDD" : model.ApplicationMode);
            try
            {
                if (Request.Form["SaveDocumentDetail"] != null)
                {
                    if (AEMLUploadDocumentLibrary.ValidateUploadFile(model))
                    {
                        if (model.ApplicationMode == "SOD" && Request.Form["SaveDocumentDetail"] != null)
                        {
                            model.ApplicationMode = "SDD";
                        }
                        bool status = AEMLUploadDocumentLibrary.UploadDocumentFile(model);
                    }
                    else
                    {
                        ViewBag.Message = AEMLUploadDocumentLibrary.validateResult;
                    }
                }

                if (!string.IsNullOrEmpty(SubmitApplication))
                {
                    model.Status = "2";
                    if (model.ApplicationMode == "SOD" || model.ApplicationMode == "SDD")
                    {
                        if (AEMLUploadDocumentLibrary.ValidateUploadFile(model))
                        {
                            if (model.ApplicationMode == "SOD" && Request.Form["SaveDocumentDetail"] != null)
                            {
                                model.ApplicationMode = "SDD";
                            }
                            bool status = AEMLUploadDocumentLibrary.UploadDocumentFile(model);
                            if (status)
                            {
                                AEMLNewConnectionLibrary.SaveNewConnectionInfo(model);
                                if (model.ApplicationSubType == "3")
                                {
                                    try
                                    {

                                        PostDataWebUpdate requestObj = new PostDataWebUpdate

                                        {

                                            TempRegistrationNumber = model.ApplicationNo,

                                            PurposeofSupply = model.PurposeOfSupply,
                                            AppliedTariff = model.AppliedTariff,
                                            PremiseType = model.PremiseType,
                                            MeterLoad = model.MeterLoad,
                                            VoltageLevel = model.VoltageLevel,
                                            ApplicationTitle = model.ApplicationTitle,

                                            BuildingName = model.BuildingName,
                                            LaneStreet = model.Street,
                                            Pincode = model.SelectedPincode,
                                            Suburb = model.SelectedSuburb,
                                            MobileNumber = model.MobileNo,
                                            Email = model.Email,




                                        };



                                        if (model.IsApplicantType == "0")
                                        {
                                            requestObj.SectorType = "1";
                                        }
                                        else if (model.IsApplicantType == "1")
                                        {
                                            requestObj.SectorType = "2";

                                        }
                                        if ((!string.IsNullOrEmpty(model.FlatNumber)))
                                        {

                                            requestObj.HouseNumber = model.FlatNumber;
                                        }
                                        else
                                        {
                                            requestObj.HouseNumber = "";
                                        }

                                        if (model.Meterconnectioninexistingmetercabin == "Yes")
                                        {
                                            requestObj.MeterCabin = "2";
                                        }
                                        else if (model.Meterconnectioninexistingmetercabin == "No")
                                        {
                                            requestObj.MeterCabin = "1";
                                        }
                                        if (model.IsGreenTariffApplied == "1")
                                        {
                                            requestObj.IsGreenTariffApplied = "X";
                                        }
                                        else
                                        {
                                            requestObj.IsGreenTariffApplied = "";
                                        }
                                        if (model.BillFormat == "EBill")
                                        {
                                            requestObj.BillingFormatEBill = "X";
                                        }
                                        else
                                        {
                                            requestObj.BillingFormatEBill = "";
                                        }
                                        if (model.BillFormat == "EPBill")
                                        {
                                            requestObj.BillingFormatEPBill = "X";
                                        }
                                        else
                                        {
                                            requestObj.BillingFormatEPBill = "";
                                        }
                                        if (model.metersupplier == "SELF")
                                        {
                                            requestObj.SelfMeter = "X";
                                        }
                                        else
                                        {
                                            requestObj.SelfMeter = "";
                                        }
                                        if (model.IsApplicantType == "0")
                                        {
                                            requestObj.GovernmentSelected = "X";
                                        }
                                        else
                                        {
                                            requestObj.GovernmentSelected = "";
                                        }

                                        if (model.TotalLoad <= 25)
                                        {
                                            requestObj.TotalLoad = "2";
                                        }
                                        else if (model.TotalLoad > 25)
                                        {
                                            requestObj.TotalLoad = "1";
                                        }
                                        if (model.SelectedConnectionType == "0")
                                        {
                                            requestObj.ConnectionType = "LT";
                                        }
                                        else if (model.SelectedConnectionType == "1")
                                        {
                                            requestObj.ConnectionType = "HT";
                                        }

                                        if (model.MeterType == "1PH")
                                        {
                                            requestObj.MeterTypeCount1PH = "1";
                                            if (model.ConnectedLoadKW != null)
                                            {
                                                requestObj.ConnectedLoadKW1PH = model.ConnectedLoadKW;
                                            }
                                            else
                                            {
                                                requestObj.ConnectedLoadKW1PH = "";
                                            }
                                            if (model.ConnectedLoadHP != null)
                                            {
                                                requestObj.ConnectedLoadHP1PH = model.ConnectedLoadHP;
                                            }
                                            else
                                            {
                                                requestObj.ConnectedLoadHP1PH = "";
                                            }
                                        }

                                        if (model.MeterType == "3PH")
                                        {
                                            requestObj.MeterTypeCount3PH = "1";
                                            if (model.ConnectedLoadKW != null)
                                            {
                                                requestObj.ConnectedLoadKW3PH = model.ConnectedLoadKW;
                                            }
                                            else
                                            {
                                                requestObj.ConnectedLoadKW3PH = "";
                                            }
                                            if (model.ConnectedLoadHP != null)
                                            {
                                                requestObj.ConnectedLoadHP3PH = model.ConnectedLoadHP;
                                            }
                                            else
                                            {
                                                requestObj.ConnectedLoadHP3PH = "";
                                            }
                                        }

                                        if (model.MeterType == "HT")
                                        {
                                            requestObj.MeterTypeCountHT = "1";
                                            if (model.ConnectedLoadKW != null)
                                            {
                                                requestObj.ConnectedLoadKWHT = model.ConnectedLoadKW;
                                            }
                                            else
                                            {
                                                requestObj.ConnectedLoadKWHT = "";
                                            }
                                            if (model.ConnectedLoadHP != null)
                                            {
                                                requestObj.ConnectedLoadHT = model.ConnectedLoadHP;
                                            }
                                            else
                                            {
                                                requestObj.ConnectedLoadHT = "";
                                            }
                                        }
                                        if (model.TempStartDate.HasValue)
                                        {
                                            requestObj.TempStartDate = model.TempStartDate.ToString();
                                        }
                                        else
                                        {
                                            requestObj.TempStartDate = "";
                                        }
                                        if (model.TempEndDate.HasValue)
                                        {
                                            requestObj.TempEndDate = model.TempEndDate.ToString();
                                        }
                                        else
                                        {
                                            requestObj.TempEndDate = "";
                                        }
                                        var listAllArea = newconnectionservice.ListAreaPinWorkcenterMapping();
                                        var AreaList = listAllArea.Where(a => a.Area == model.SelectedSuburb).FirstOrDefault();
                                        model.AreaTobeSent = AreaList.AreaSentToSAP;
                                        if ((string.IsNullOrEmpty(model.LDPOrderNumber)))
                                        {
                                            var LDPorderNumber = listAllArea.Where(a => a.Pincode == model.SelectedPincode).FirstOrDefault();
                                            model.LDPOrderNumber = LDPorderNumber.DummyLDP;
                                            requestObj.LDPNumber = model.LDPOrderNumber;
                                        }
                                        else
                                        {
                                            requestObj.LDPNumber = model.LDPOrderNumber;
                                        }
                                        var WorkCentre = listAllArea.Where(a => a.Pincode == model.SelectedPincode).FirstOrDefault();
                                        requestObj.Workcenter = WorkCentre.WorkStation;
                                        var city = listAllArea.Where(a => a.Pincode == model.SelectedPincode).FirstOrDefault();
                                        requestObj.City = city.City;
                                        if (model.billingdifferentthanAddresswheresupply == "Yes")
                                        {
                                            requestObj.billingdifferentthanAddresswheresupply = "X";
                                            requestObj.BillingHouseNumber = model.ApplicantCorrespondenceFlatNumber;
                                            requestObj.BillingBuildingName = model.ApplicantCorrespondenceFlatNumber;
                                            requestObj.BillingLandmark = model.ApplicantCorrespondenceLandmark;
                                            requestObj.BillingLane = model.ApplicantCorrespondenceStreet;
                                            requestObj.BillingPincode = model.ApplicantCorrespondencePincode;
                                            requestObj.BillingSuburb = model.ApplicantCorrespondenceSuburb;

                                        }
                                        else
                                        {
                                            requestObj.billingdifferentthanAddresswheresupply = "";
                                            requestObj.BillingHouseNumber = "";
                                            requestObj.BillingBuildingName = "";
                                            requestObj.BillingLandmark = "";
                                            requestObj.BillingLane = "";
                                            requestObj.BillingPincode = "";
                                            requestObj.BillingSuburb = "";
                                        }
                                        if (model.AddressInCaseOfRental == "Yes")
                                        {
                                            requestObj.RentalAddress = "X";
                                            requestObj.RentalBuildingName = model.RentalBuildingName;
                                            requestObj.RentalHouseNumber = model.RentalFlatNumber;
                                            requestObj.NameofRentalOwner = model.RentalNameoftheOwner;
                                            requestObj.RentalLane = model.RentalStreet;
                                            requestObj.RentalPincode = model.RentalPincode;
                                            requestObj.RentalSuburb = model.RentalSuburb;
                                            requestObj.RentalMobileNumber = model.RentalContactNumber;
                                            requestObj.RentalEmail = model.RentalOwnerEmail;

                                        }
                                        else
                                        {
                                            requestObj.RentalAddress = "";
                                            requestObj.RentalBuildingName = "";
                                            requestObj.RentalHouseNumber = "";
                                            requestObj.NameofRentalOwner = "";
                                            requestObj.RentalLane = "";
                                            requestObj.RentalPincode = "";
                                            requestObj.RentalSuburb = "";
                                            requestObj.RentalMobileNumber = "";
                                            requestObj.RentalEmail = "";
                                        }

                                        if (!string.IsNullOrEmpty(model.NearestConsumerAccountNo))
                                        {
                                            requestObj.NearestCAnumber = model.NearestConsumerAccountNo;
                                        }
                                        else
                                        {
                                            requestObj.NearestCAnumber = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.LandlineNo))
                                        {
                                            requestObj.LandlineNumber = model.LandlineNo;
                                        }
                                        else
                                        {
                                            requestObj.LandlineNumber = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.NearestConsumerMeterNo))
                                        {
                                            requestObj.NearestMeternumber = model.NearestConsumerMeterNo;
                                        }
                                        else
                                        {
                                            requestObj.NearestMeternumber = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.LECNumber))
                                        {
                                            requestObj.LECNumber = model.LECNumber;
                                        }
                                        else
                                        {
                                            requestObj.LECNumber = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.BankAccountNumber))
                                        {
                                            requestObj.BankAccountNumber = model.BankAccountNumber;
                                        }
                                        else
                                        {
                                            requestObj.BankAccountNumber = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.MICR))
                                        {
                                            requestObj.MICR = model.MICR;
                                        }
                                        else
                                        {
                                            requestObj.MICR = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.Bank))
                                        {
                                            requestObj.Bank = model.Bank;
                                        }
                                        else
                                        {
                                            requestObj.Bank = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.Branch))
                                        {
                                            requestObj.Branch = model.Branch;
                                        }
                                        else
                                        {
                                            requestObj.Branch = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.BillLanguage))
                                        {
                                            requestObj.BillLangianguage = model.BillLanguage;
                                        }
                                        else
                                        {
                                            requestObj.BillLangianguage = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.LandlineNo))
                                        {
                                            requestObj.LandlineNumber = model.LandlineNo;
                                        }
                                        else
                                        {
                                            requestObj.LandlineNumber = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.ConsumerNo))
                                        {
                                            requestObj.ConsumerNumber = model.ConsumerNo;
                                        }
                                        else
                                        {
                                            requestObj.ConsumerNumber = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.Utility))
                                        {
                                            requestObj.Utility = model.Utility;
                                        }
                                        else
                                        {
                                            requestObj.Utility = "";
                                        }

                                        if (!string.IsNullOrEmpty(model.OrganizationName))
                                        {
                                            requestObj.OrganizationName = model.OrganizationName;
                                        }
                                        else
                                        {
                                            requestObj.OrganizationName = "";
                                        }

                                        if (!string.IsNullOrEmpty(model.FirstName))
                                        {
                                            requestObj.FirstName = model.FirstName;
                                        }
                                        else
                                        {
                                            requestObj.FirstName = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.MiddleName))
                                        {
                                            requestObj.MiddleName = model.MiddleName;
                                        }
                                        else
                                        {
                                            requestObj.MiddleName = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.LastName))
                                        {
                                            requestObj.LastName = model.LastName;
                                        }
                                        else
                                        {
                                            requestObj.LastName = "";
                                        }
                                        if (model.WiringCompleted == "Yes")
                                        {
                                            requestObj.WiringCompleted = "1";
                                        }
                                        else
                                        {
                                            requestObj.WiringCompleted = "2";
                                        }

                                        if (!string.IsNullOrEmpty(model.ContractDemand))
                                        {
                                            requestObj.ContractDemand = model.ContractDemand;
                                        }
                                        else
                                        {
                                            requestObj.ContractDemand = "";
                                        }
                                        if (!string.IsNullOrEmpty(model.Landmark))
                                        {
                                            requestObj.Landmark = model.Landmark;
                                        }
                                        else
                                        {
                                            requestObj.Landmark = "";
                                        }

                                        model.GetExistingDocuments = AEMLUploadDocumentLibrary.GetExistingDocument(model.ApplicationNo);
                                        List<PostDocumentsWebUpdate> requestDocList = new List<PostDocumentsWebUpdate>();
                                        foreach (var document in model.GetExistingDocuments)
                                        {
                                            if (document.DocumentTypeCode.ToLower() != "tr")
                                            {
                                                requestDocList.Add(new PostDocumentsWebUpdate() { DocumentDescription = document.DocumentDescription, DocumentSerialNumber = document.DocumentType, DocumentData = document.DocumentData.ToArray() });
                                            }
                                        }
                                        var PostData = SapPiService.Services.RequestHandler.PostDataForNewCon(requestObj, requestDocList);
                                        string ID_Order = "";
                                        if (PostData.IsSuccess == true)
                                        {
                                            var GetOrderId = SapPiService.Services.RequestHandler.GetOrderIdForNewCon(requestObj.TempRegistrationNumber);
                                            if (GetOrderId.IsSuccess == true)
                                            {

                                                ID_Order = GetOrderId.OrderIdSAP;
                                                string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/NewCon/PopupMessageOnSave", "Your application has been sent to SAP service" + "Your Reference NUmber is" + model.ApplicationNo + "and Order Id is" + ID_Order);
                                                Session["Message"] = messagetobedisplayed;
                                                return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.NewConnection.ApplicationList));
                                            }


                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex.Message + " For Mobil number:" + model.MobileNo, "PostDataToSAP");
                                    }
                                }

                                else if (model.ApplicationSubType == "1" || model.ApplicationSubType == "2")
                                {
                                    string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/NewCon/PopupMessageOnSave", "Your application has been received and" + "your Reference NUmber is" + model.ApplicationNo);
                                    Session["Message"] = messagetobedisplayed;
                                    return RedirectPermanent(this.UserProfileService.GetPageURL(Templates.NewConnection.ApplicationList));
                                }



                            }
                        }
                        else
                        {
                            ViewBag.Message = AEMLUploadDocumentLibrary.validateResult;
                        }
                    }
                }
                model.GetExistingDocuments = AEMLUploadDocumentLibrary.GetExistingDocument(model.ApplicationNo);
            }
            catch (Exception exception3)
            {
                Exception exception2 = exception3;
                Log.Error(string.Concat("Error at Saving Form Registration Form: ", exception2.Message), this);
                ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
            }





            return this.View("~/Views/AEMLNewConnection/NewConnectionHome.cshtml", model);
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
                //if (Session["NewConnectionRegistrationModel"] != null)
                //{
                //    var accountDetailsForNEWCON = (NEW_CON_APPLICATION_FORM)Session["NewConnectionRegistrationModel"];
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    var fileToDownload = dbcontext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.Where(i => i.Id == id).FirstOrDefault();

                    if (dbcontext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.Any(a => a.RegistrationSerialNumber == fileToDownload.RegistrationSerialNumber))
                        return File(fileToDownload.DocumentData.ToArray(), fileToDownload.DocumentDataContenttype, fileToDownload.DocumentName);
                    else
                        return null;
                }
                //}
            }
            catch (Exception ex)
            {
                Log.Error("Error at DownloadFile:" + ex.Message, this);
            }
            return null;
        }

        public ActionResult NewConnectionList(string searchtext = "")
        {
            try
            {
                if (Session["LECUserLogin"] != null)
                {
                    ChangeOfNameService conService = new ChangeOfNameService();
                    ChangeOfNameLECUserProfileModel lecmodel = conService.GetLECDetails(UserSession.AEMLCONLECUserSessionContext.RegistrationNumber);
                    string lecno = lecmodel.LECRegistrationNumber;
                    NewConnectionService newconnectionservice = new NewConnectionService();
                    var model = newconnectionservice.GetLECApplicationConnectionList(lecno, searchtext);
                    return View(model);
                }
                else
                {
                    string mobileno = UserSession.AEMLNewConnectionSessionContext.MobileNo;
                    NewConnectionService newconnectionservice = new NewConnectionService();
                    var model = newconnectionservice.GetApplicationConnectionList(mobileno, searchtext);
                    return View(model);
                }
            }
            catch (Exception e)
            {
                Log.Error("New Connect portal home " + e.Message, this);

                var item = Context.Database.GetItem(Templates.NewConnection.LoginPage);

                return this.Redirect(item.Url());
            }
        }

        public ActionResult Logout()
        {
            this.AccountRepository.Logout();
            SessionHelper.UserSession.AEMLNewConnectionSessionContext = null;
            var item = Context.Database.GetItem(Templates.NewConnection.LoginPage);

            return this.Redirect(item.Url());
        }
    }
}
