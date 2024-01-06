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
    using System.Data;
    using System.Diagnostics;

    public class AEMLSolarApplicationController : Controller
    {
        public AEMLSolarApplicationController(IAccountRepository accountRepository, INotificationService notificationService, IAccountsSettingsService accountsSettingsService, IGetRedirectUrlService getRedirectUrlService, IUserProfileService userProfileService, IFedAuthLoginButtonRepository fedAuthLoginRepository, IUserProfileProvider userProfileProvider, IPaymentService paymentService, IDbAccountService dbAccountService)
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

        #region Solar application


        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult SolarNewApplication()
        {
            var model = new SolarNewApplicationModel();
            SolarApplicationService solarApplicationServiceService = new SolarApplicationService();
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);
                var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetailsForSolar(profile.AccountNumber);
                var consumerOutstandingDetails = SapPiService.Services.RequestHandler.FetchAmountDetailsForSolar(profile.AccountNumber);

                if (!string.IsNullOrEmpty(consumerDetails.TATA_CONSUMER) && consumerDetails.TATA_CONSUMER == "X")
                {
                    ViewBag.CAValidationFailed = DictionaryPhraseRepository.Current.Get("/Accounts/SolarNewApplication/CAValidationFailed_TATAConsumers", "TATA Consumers are not eligible for Solar application, please contact administrator.");
                }
                else if (!string.IsNullOrEmpty(consumerDetails.MOVE_OUT_DATE) && Convert.ToDateTime(consumerDetails.MOVE_OUT_DATE) <= DateTime.Now)
                {
                    ViewBag.CAValidationFailed = DictionaryPhraseRepository.Current.Get("/Accounts/SolarNewApplication/CAValidationFailed_MovedOut", "Moved Out Consumers are not eligible for Solar application, please contact administrator.");
                }
                else if (!string.IsNullOrEmpty(consumerDetails.Rate_Category) && (consumerDetails.Rate_Category == "TIL" || consumerDetails.Rate_Category == "TRL" || consumerDetails.Rate_Category == "TRM" || consumerDetails.Rate_Category == "TSP" || consumerDetails.Rate_Category == "TSR" || consumerDetails.Rate_Category == "TTP" || consumerDetails.Rate_Category == "TIL"))
                {
                    ViewBag.CAValidationFailed = DictionaryPhraseRepository.Current.Get("/Accounts/SolarNewApplication/CAValidationFailed_RateCat", "TIL, TRL, TRM, TSP, TSR, TTP Rate Categories are not allowed, Consumers are not eligible for Solar application, please contact administrator.");
                }
                else if (!string.IsNullOrEmpty(consumerDetails.BILL_CLASS) && (consumerDetails.BILL_CLASS == "CHO" || consumerDetails.BILL_CLASS == "LEA"))
                {
                    ViewBag.CAValidationFailed = DictionaryPhraseRepository.Current.Get("/Accounts/SolarNewApplication/CAValidationFailed_BILL_CLASS", "Consumers with Billing Class as CHO or LEA are not eligible for Solar application, please contact administrator.");
                }
                else if (!string.IsNullOrEmpty(consumerOutstandingDetails.OUTSTANDING_AMOUNT) && (Convert.ToDecimal(consumerOutstandingDetails.OUTSTANDING_AMOUNT) < 0))
                {
                    ViewBag.CAValidationFailed = DictionaryPhraseRepository.Current.Get("/Accounts/SolarNewApplication/CAValidationFailed_OUTSTANDING_AMOUNT", "Outstanding amount should be greater than 0 to apply for Solar, please contact administrator.");
                }
                else if (!string.IsNullOrEmpty(consumerDetails.MeterNumber))
                {
                    model = new SolarNewApplicationModel()
                    {
                        AccountNo = profile.AccountNumber,
                        Address = consumerDetails.HouseNumber + " " + consumerDetails.Street + " " + consumerDetails.Street2 + " " + consumerDetails.Street3 + " " + consumerDetails.City + " " + consumerDetails.PinCode,
                        EmailAddress = consumerDetails.Email,
                        MobileNo = consumerDetails.Mobile,
                        Name = consumerDetails.Name,
                        RateCategory = consumerDetails.Rate_Category,
                        MeterNo = consumerDetails.MeterNumber,
                        IsSubsidizedCategory = (consumerDetails.Rate_Category.ToLower() == "resi1" || consumerDetails.Rate_Category.ToLower() == "resi3") ? true : false
                    };

                    model.VoltageCategoryList = new List<SelectListItem>();
                    var voltageList = solarApplicationServiceService.GetVoltageDetails();
                    foreach (var voltage in voltageList)
                    {
                        model.VoltageCategoryList.Add(new SelectListItem
                        {
                            Value = voltage.VoltageLevel,
                            Text = voltage.VoltageDescription
                        });
                    }

                    //Get documents
                    var documents = solarApplicationServiceService.GetDocuments();
                    model.DocumentsList = new List<SolarDocumentCheckList>();
                    foreach (var doc in documents)
                    {
                        model.DocumentsList.Add(new SolarDocumentCheckList
                        {
                            DocId = doc.Id,
                            DocName = doc.DocumentName,
                            DocSerialNumber = doc.DocumentSerialNumber,
                            IsSubsidized = doc.IsSubsidizedCategory
                        });
                    }

                    //Get Vendors
                    model.VendorsList = solarApplicationServiceService.GetVendors();
                    foreach (var vendor in model.VendorsList)
                    {
                        model.VendorsListSelectItem.Add(
                new SelectListItem { Value = vendor.NameOfVendorAgency, Text = vendor.NameOfVendorAgency });
                    }
                }
                else
                {
                    ViewBag.CAValidationFailed = DictionaryPhraseRepository.Current.Get("/Accounts/SolarNewApplication/CAValidationFailed", "You are not eligible for Solar Application, please contact administrator.");
                }
            }
            catch (Exception ex)
            {
                ViewBag.SolarApplication = DictionaryPhraseRepository.Current.Get("/SolarApplicationAPI Issue", "There is some issue in fetching your data. Please try after some time.");
                Sitecore.Diagnostics.Log.Error("Error at SolarNewApplication Get:" + ex.Message, this);
            }
            return this.View(model);
        }

        [HttpPost]
        [RedirectUnauthenticated]
        [ValidateAntiForgeryToken]
        public ActionResult SolarNewApplication(SolarNewApplicationModel model, FormCollection form, string SubmitApplication = null, string PayAmount = null)
        {
            try
            {
                SolarApplicationDetail objNewSolarApplication = new SolarApplicationDetail();
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }
                if (!string.IsNullOrEmpty(SubmitApplication))
                {
                    SolarApplicationService solarApplicationServiceService = new SolarApplicationService();
                    var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetailsForSolar(model.AccountNo);
                    var caInfo_workcenter = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.AccountNo);

                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        var selectedVendorDetails = solarApplicationServiceService.GetVendors().Where(v => v.NameOfVendorAgency == model.SelectedVendor).FirstOrDefault();
                        objNewSolarApplication = new SolarApplicationDetail
                        {
                            Id = Guid.NewGuid(),
                            AccountNumber = model.AccountNo,
                            UserId = Profile.UserName,
                            Address = model.Address,
                            ConsumerName = model.Name,
                            EmailAddress = model.EmailAddress,
                            InstallationCost = model.InstallationCost,
                            IsNetMeterOpted = model.NetMeterOrBilling == "Meter" ? true : false,
                            IsObligatedEntity = model.IsObligatedEntity == "Yes" ? true : false,
                            IsOwnershipLeased = model.SolarOwnershipType == "Leased" ? true : false,
                            IsRooftopSolarInstalled = model.IsSolarInstalled == "Yes" ? true : false,
                            LECNumber = model.LECNumber,
                            MeterNumber = model.MeterNo,
                            MobileNumber = model.MobileNo,
                            ProposedACcapacity = model.ProposedACCategory,
                            RateCategory = model.RateCategory,
                            VenderNameSeleted = model.SelectedVendor,
                            VenderSelected = selectedVendorDetails?.Id,
                            VoltageCategorySelected = model.SelectedVoltageCategory,
                            AadharNumber = model.AadharNumber,
                            ApplicationCategory = model.ApplicationCategory,
                            IsRooftopPlusGroundCapacity = model.RooftopSolarType == "Residential" ? false : true,
                            IsSubsidizedCategory = (consumerDetails.Rate_Category.ToLower() == "resi1" || consumerDetails.Rate_Category.ToLower() == "resi3") ? true : false,
                            CreatedBy = model.AccountNo,
                            CreatedDate = DateTime.Now,
                            ApplicationStatus = "Submitted",
                            Workcenter = caInfo_workcenter.VAPLZ_WORK_CENTER
                            //OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty)
                        };
                        dataContext.SolarApplicationDetails.InsertOnSubmit(objNewSolarApplication);

                        var listOfDocs = solarApplicationServiceService.GetDocuments();
                        List<PostDocumentsWebUpdate> objServiceListDocs = new List<PostDocumentsWebUpdate>();

                        foreach (var doc in listOfDocs.ToList())
                        {

                            HttpPostedFileBase file = Request.Files["file_" + doc.DocumentSerialNumber];
                            if (file != null && file.ContentLength > 0)
                            {
                                byte[] bytes;
                                using (BinaryReader br = new BinaryReader(file.InputStream))
                                {
                                    bytes = br.ReadBytes(file.ContentLength);
                                }

                                SolarApplicationDocumentDetail objSolarApplicationDocumentDetail = new SolarApplicationDocumentDetail
                                {
                                    AccountNumber = objNewSolarApplication.AccountNumber,
                                    CreatedBy = objNewSolarApplication.AccountNumber,
                                    CreatedDate = DateTime.Now,
                                    DocumentData = bytes,
                                    DocumentDataContenttype = file.ContentType,
                                    DocumentId = doc.Id,
                                    DocumentSerialNumber = doc.DocumentSerialNumber,
                                    Id = Guid.NewGuid(),
                                    DocumentName = file.FileName,
                                    IsSentToSAP = false,
                                    SolarApplicationId = objNewSolarApplication.Id,
                                    ApplicationNumber = objNewSolarApplication.ApplicationNumber
                                };
                                dataContext.SolarApplicationDocumentDetails.InsertOnSubmit(objSolarApplicationDocumentDetail);

                                //Doc for SAP service
                                PostDocumentsWebUpdate objDoc = new PostDocumentsWebUpdate
                                {
                                    DocumentData = bytes,
                                    DocumentSerialNumber = objSolarApplicationDocumentDetail.DocumentSerialNumber.ToString(),
                                    DocumentDescription = objSolarApplicationDocumentDetail.DocumentName
                                };
                                objServiceListDocs.Add(objDoc);
                            }
                        }
                        dataContext.SubmitChanges();

                        var objSolarApplication = dataContext.SolarApplicationDetails.Where(a => a.Id == objNewSolarApplication.Id).FirstOrDefault();
                        //SAP integration
                        //1. Post data
                        PostDataWebUpdate objService = new PostDataWebUpdate
                        {
                            AccountNumber = model.AccountNo,
                            ApplicationCategory = model.ApplicationCategory,
                            InstallationCost = model.InstallationCost,
                            IsNetMeter = model.NetMeterOrBilling == "Meter" ? true : false,
                            IsObligatedEntity = model.IsObligatedEntity == "Yes" ? true : false,
                            IsRooftopOwned = model.SolarOwnershipType == "Leased" ? true : false,
                            IsRooftopsolarInstalled = model.IsSolarInstalled == "Yes" ? true : false,
                            IsRooftopPlusGroundCapacity = model.RooftopSolarType == "Residential" ? false : true,
                            LECNumber = model.LECNumber,
                            TempRegistrationNumber = objNewSolarApplication.TempRegistrationNumber,
                            VendorCode = model.SelectedVendor,
                            VendorName = model.SelectedVendor,
                            VoltageLevel = model.SelectedVoltageCategory,
                            VoltageLevelDesc = model.SelectedVoltageCategory,
                            Workcenter = caInfo_workcenter.VAPLZ_WORK_CENTER
                        };

                        PostDataResult objResult = SapPiService.Services.RequestHandler.PostDataForSolar(objService, objServiceListDocs);
                        // TBD - check for objResult.IsSuccess

                        //2. Get Order Id
                        OrderFetchResult objOrderResult = SapPiService.Services.RequestHandler.GetOrderIdForSolar(objSolarApplication.TempRegistrationNumber);
                        if (objOrderResult.IsSuccess && string.IsNullOrEmpty(objOrderResult.ExceptionMessage) && !string.IsNullOrEmpty(objOrderResult.OrderIdSAP))
                        {
                            model.IsApplicationSaved = true;
                            objSolarApplication.OrderId = objOrderResult.OrderIdSAP;
                            dataContext.SubmitChanges();
                        }
                        else
                        {
                            //Call another service ZBAPI_POSTCONFEE
                            model.IsApplicationSaved = false;
                        }

                        model.IsApplicationSaved = true;// objResult.IsSuccess;

                        //3. To be fetched from service
                        model.AmountToBePaid = 100;
                        return this.View(model);
                    }
                }
                else if (!string.IsNullOrEmpty(PayAmount))
                {
                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        var objSolarApplication = dataContext.SolarApplicationDetails.Where(a => a.Id == objNewSolarApplication.Id).FirstOrDefault();
                        if (string.IsNullOrEmpty(model.AmountToBePaid.ToString()) || model.AmountToBePaid.ToString().Any(char.IsLetter))
                        {
                            this.ModelState.AddModelError("AmountPayable", DictionaryPhraseRepository.Current.Get("/Accounts/SolarApplication/InvalidAmount", "Invalid amount payable value."));
                            return this.View(model);
                        }

                        this.PaymentService.StorePaymentRequestSolarApplication(objSolarApplication);
                        string RequestHTML = this.PaymentService.BillDeskTransactionRequestAPIRequestPost_SolarApplication(objSolarApplication);
                        return Content(RequestHTML);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at SolarNewApplication Post:" + ex.Message, this);
            }
            return this.View(model);
        }

        [HttpGet]
        [RedirectUnauthenticated]
        public ActionResult SolarTrackApplications()
        {
            var model = new SolarNewApplicationModel();
            try
            {
                var profile = this.UserProfileService.GetProfile(Context.User);
                SolarApplicationService solarApplicationServiceService = new SolarApplicationService();
                var existingApplications = solarApplicationServiceService.GetExistingApplications(profile.AccountNumber);

                return this.View(existingApplications);
            }
            catch (Exception ex)
            {
                ViewBag.SolarApplication = DictionaryPhraseRepository.Current.Get("/SolarApplicationAPI Issue", "There is some issue in fetching your data. Please try after some time.");
                Sitecore.Diagnostics.Log.Error("Error at SolarNewApplication Get:" + ex.Message, this);
            }
            return this.View(model);
        }

        [HttpPost]
        [RedirectUnauthenticated]
        [ValidateAntiForgeryToken]
        public ActionResult SolarTrackApplications(ViewPayBill model, string Pay_PaymentGateway = null, string Pay_VDSpayment = null, string Pay_Other = null)
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


        public ActionResult ExportVendorDetails()
        {
            try
            {
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    var datalist = dbcontext.SolarApplicationVendorLists.ToList();

                    DataTable table = ToDataTable(datalist);
                    DatatableToCSV(table);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderUserListing Get:" + ex.Message, this);
            }
            return null;
        }


        #endregion

        #region Download Functions
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
            #region New Method
            string fileName = "Vendordetails_" + DateTime.Now.Ticks.ToString();
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


            #endregion
        }

        #endregion
    }
}