using DotNetIntegrationKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using paytm;
using SapPiService.Domain;
using Sitecore.Data.Items;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.Models.HDFC_Payment_Gateway;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using CCA.Util;
using System.Collections.Specialized;
using Sitecore.Diagnostics;

namespace Sitecore.Feature.Accounts.Services
{
    [Service(typeof(IPaymentService))]
    public class PaymentService : IPaymentService
    {
        private IUserProfileService UserProfileService { get; }
        private IUserProfileProvider UserProfileProvider { get; }

        private readonly IDictionary<string, string> properties;
        private static Sitecore.Data.Database webdbObj = Sitecore.Configuration.Factory.GetDatabase("web");
        public PaymentService()
        {
            properties = UserProfileProvider.GetCustomProperties(Context.User.Profile);
        }
        public PaymentService(IUserProfileService userProfileService, IUserProfileProvider userProfileProvider)
        {
            UserProfileService = userProfileService;
            UserProfileProvider = userProfileProvider;
            properties = UserProfileProvider.GetCustomProperties(Context.User.Profile);
        }



        #region Request / Response Store in DB

        public void StorePaymentRequestENachRegistration(ENachRegistrationModel model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequestENachRegistration Store request in Database Order Id : - " + model.AccountNo, this);
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        ENachRegistration objdata = new ENachRegistration();
                        Guid newid = Guid.NewGuid();
                        objdata.Id = newid;
                        objdata.AccountNumber = model.AccountNo;
                        objdata.Name = model.Name;
                        objdata.Email = model.EmailId ?? string.Empty;
                        objdata.Mobile = model.MobileNo ?? string.Empty;
                        objdata.Created_Date = System.DateTime.Now;
                        objdata.CreatedBy = model.AccountNo;
                        objdata.PPIFlag = model.isPPISet;
                        dbcontext.ENachRegistrations.InsertOnSubmit(objdata);
                        dbcontext.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentRequest :" + model.AccountNo + " " + ex.Message, this);
            }
        }

        public void StorePaymentRequestSolarApplication(SolarApplicationDetail model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database Order Id : - " + model.TempRegistrationNumber, this);
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        PaymentHistory objdata = new PaymentHistory();
                        Guid newid = Guid.NewGuid();
                        objdata.Id = newid;
                        objdata.Remark = model.ApplicationNumber.ToString();
                        objdata.AccountNumber = model.AccountNumber ?? string.Empty;
                        objdata.UserId = model.UserId;
                        objdata.OrderId = model.OrderId;
                        objdata.Amount = model.AmountTobePaid ?? string.Empty;
                        objdata.RequestTime = System.Convert.ToDateTime(DateTime.UtcNow);
                        objdata.Checksumkey = string.Empty;
                        objdata.CurrencyType = "INR";
                        objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), "BillDesk").ToString() ?? string.Empty;
                        objdata.AdvanceAmmount = string.Empty;
                        objdata.msg = model.TempRegistrationNumber;
                        objdata.PaymentType = "Solat Application Payment";
                        objdata.UserType = "Registered";
                        objdata.Email = model.EmailAddress;
                        objdata.Mobile = model.MobileNumber;
                        objdata.Created_Date = System.DateTime.Now;
                        objdata.CreatedBy = model.AccountNumber;
                        dbcontext.PaymentHistories.InsertOnSubmit(objdata);
                        dbcontext.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentRequest :" + ex.Message, this);
            }


        }

        public void StorePaymentRequest(ViewPayBill model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    if (model.PaymentGateway == (int)EnumPayment.GatewayType.Benow || model.PaymentGateway == (int)EnumPayment.GatewayType.DBSUPI || model.PaymentGateway == (int)EnumPayment.GatewayType.CITYUPI)
                    {
                        InvoiceHistory result = SapPiService.Services.RequestHandler.FetchInvoiceHistory(model.AccountNumber);
                        if (result != null && result.InvoiceLines != null && result.InvoiceLines.Any(x => x.BillMonth == model.BillMonth))
                        {

                            model.OrderId = model.AccountNumber + result.InvoiceLines.Where(x => x.BillMonth == model.BillMonth).ToList().FirstOrDefault().InvoiceNumber + EncryptionDecryption.GenerateShortRandomOrderId(string.Empty); ;
                            Sitecore.Diagnostics.Log.Info("StorePaymentRequest Benow Billmonth and Account Match:" + model.OrderId, this);
                        }
                        else
                        {
                            model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                            Sitecore.Diagnostics.Log.Info("StorePaymentRequest Benow Billmonth and Account Match not found:" + model.OrderId, this);
                        }
                    }
                    else
                    {
                        model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                    }
                    string amount = string.Empty;
                    if (model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
                    {
                        if (model.SecurityDepositAmountType == "Actual")
                            amount = model.SecurityDeposit;
                        else
                            amount = model.SecurityDepositPartial.ToString();
                    }
                    else
                    {
                        amount = model.AmountPayable;
                    }

                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database Order Id : - " + model.OrderId, this);
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            PaymentHistory objdata = new PaymentHistory();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            objdata.Remark = model.Remark;
                            objdata.AccountNumber = model.AccountNumber ?? string.Empty;
                            objdata.UserId = model.LoginName;
                            objdata.OrderId = model.OrderId;
                            objdata.Amount = amount ?? string.Empty;
                            objdata.RequestTime = System.Convert.ToDateTime(DateTime.UtcNow);
                            objdata.Checksumkey = string.Empty;
                            objdata.CurrencyType = model.CurrencyType ?? string.Empty;
                            objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString() ?? string.Empty;
                            objdata.AdvanceAmmount = model.AdvanceAmmount.ToString() ?? string.Empty;
                            objdata.msg = model.msg ?? string.Empty;
                            objdata.PaymentType = model.PaymentType ?? string.Empty;
                            objdata.UserType = model.UserType ?? string.Empty;
                            objdata.Email = model.Email ?? string.Empty;
                            objdata.Mobile = model.Mobile ?? string.Empty;
                            objdata.Created_Date = System.DateTime.Now;
                            objdata.CreatedBy = model.LoginName;
                            objdata.EMIOption = model.ProceedWithEMI;
                            objdata.EMIAmount = model.EMIInstallmentAmount.ToString();
                            objdata.EMIOutstanding = model.EMIOutstandingAmount.ToString();
                            dbcontext.PaymentHistories.InsertOnSubmit(objdata);
                            dbcontext.SubmitChanges();

                        }
                    }
                    catch (System.Exception ex)
                    {
                        // Note : Log the message on any failure to sitecore log
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item " + model.LoginName + ": " + ex.Message, this);
                    }


                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentRequest :" + ex.Message, this);
            }


        }

        public void StorePaymentResponse(ViewPayBill model)
        {
            try
            {
                if (model != null)
                {

                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in Database and PI Service :" + model.TransactionId, this);
                        //Update response in DB
                        string ISU_UPDATE_FLAG = string.Empty, ISU_UPDATE_MESSAGE = string.Empty;
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            PaymentHistory ctx = dbcontext.PaymentHistories.Where(x => x.UserId == model.LoginName && x.OrderId == model.OrderId).FirstOrDefault();
                            if (ctx != null)
                            {
                                try
                                {
                                    Sitecore.Diagnostics.Log.Info("Check payment type for VDS Registration : - " + ctx.PaymentType, this);
                                    if (ctx.PaymentType.Trim() == "VDS payment" && model.ResponseStatus == Constants.PaymentResponse.Success)
                                    {
                                        Sitecore.Diagnostics.Log.Info("VDS Registration : OrderId- " + model.OrderId, this);
                                        Sitecore.Diagnostics.Log.Info("VDS Registration : - TransactionId" + ctx.TransactionId, this);
                                        Sitecore.Diagnostics.Log.Info("VDS Registration : Mobile- " + ctx.Mobile, this);
                                        Sitecore.Diagnostics.Log.Info("VDS Registration : - Email" + ctx.Email, this);
                                        Sitecore.Diagnostics.Log.Info("VDS Registration : Remark- " + ctx.Remark, this);
                                        Sitecore.Diagnostics.Log.Info("VDS Registration : - Amount" + ctx.Amount, this);

                                        Sitecore.Diagnostics.Log.Info("VDS Registration : - Created_Date" + ctx.Created_Date.Value.ToString("yyyyMMdd"), this);
                                        VDSRegistration response = SapPiService.Services.RequestHandler.VdsRegistration(ctx.AccountNumber, model.TransactionId, ctx.Mobile, ctx.Email, ctx.Remark, ctx.Amount, ctx.Created_Date.Value.ToString("yyyyMMdd"));
                                        Sitecore.Diagnostics.Log.Info("VDS Registration response: - " + response.ResultFlag + "," + response.AccountNumber + "," + response.MobileNumber + "," + response.EmailId + "," + response.PANNumber + "," + response.TransactionId, this);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Sitecore.Diagnostics.Log.Info("VDS Registration exception in order : - " + model.OrderId + ", exception:" + e.Message, this);
                                }

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database Update payment Order Id : - " + model.OrderId, this);
                                // Update response in Database
                                ctx.TransactionId = model.TransactionId ?? string.Empty;
                                ctx.Status = model.ResponseStatus ?? string.Empty;
                                ctx.Responsecode = model.Responsecode.ToString() ?? string.Empty;
                                //ctx.Remark = model.Remark.ToString() ?? string.Empty;
                                ctx.ResponseTime = System.Convert.ToDateTime(DateTime.UtcNow);
                                ctx.PaymentRef = model.PaymentRef.ToString() ?? string.Empty;
                                ctx.ResponseMsg = model.msg ?? string.Empty;
                                ctx.PaymentMode = model.PaymentMode ?? string.Empty;
                                ctx.Modified_Date = System.DateTime.Now;
                                ctx.ModifiedBy = model.LoginName;
                                ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                dbcontext.SubmitChanges();

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in Database update successfull :" + model.TransactionId, this);
                            }
                            else
                            {
                                //kiosk payment
                                try
                                {
                                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database kiosk payment Order Id : - " + model.OrderId, this);
                                    PaymentHistory objdata = new PaymentHistory();
                                    Guid newid = Guid.NewGuid();
                                    objdata.Id = newid;
                                    objdata.AccountNumber = model.AccountNumber ?? string.Empty;
                                    objdata.UserId = model.LoginName;
                                    objdata.OrderId = model.OrderId;
                                    objdata.Amount = model.AmountPayable ?? string.Empty;
                                    objdata.RequestTime = model.TransactionDate == null ? DateTime.Now : DateTime.ParseExact(model.TransactionDate, "dd-MM-yyyy HH:mm:ss", null); // System.Convert.ToDateTime(model.TransactionDate);
                                    objdata.Checksumkey = string.Empty;
                                    objdata.CurrencyType = "INR";
                                    objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString() ?? string.Empty;
                                    objdata.AdvanceAmmount = model.AdvanceAmmount.ToString() ?? string.Empty;
                                    objdata.msg = model.msg ?? string.Empty;
                                    objdata.Remark = "Kiosk Payment " + model.Remark ?? string.Empty;
                                    objdata.PaymentType = model.PaymentType ?? string.Empty;
                                    objdata.UserType = model.UserType ?? string.Empty;
                                    objdata.Email = model.Email ?? string.Empty;
                                    objdata.Mobile = model.Mobile ?? string.Empty;
                                    objdata.Created_Date = System.DateTime.Now;
                                    objdata.CreatedBy = model.LoginName;
                                    objdata.TransactionId = model.TransactionId;
                                    objdata.Responsecode = model.Responsecode;
                                    objdata.Status = model.ResponseStatus;
                                    objdata.PaymentMode = model.PaymentMode;
                                    objdata.ResponseMsg = JsonConvert.SerializeObject(model);
                                    objdata.Created_Date = System.DateTime.Now;
                                    objdata.CreatedBy = model.AccountNumber;
                                    objdata.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                    objdata.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                    objdata.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                    dbcontext.PaymentHistories.InsertOnSubmit(objdata);
                                    dbcontext.SubmitChanges();

                                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in Database Add kiosk payment successfull :" + model.TransactionId, this);
                                }
                                catch (System.Exception ex)
                                {
                                    // Note : Log the message on any failure to sitecore log
                                    Sitecore.Diagnostics.Log.Error("Error at Adding kiosk payment Item " + model.AccountNumber + ": " + ex.Message, this);
                                }
                            }

                            if (ctx != null)//disable for kiosk for now
                            {
                                try
                                {
                                    string gateway = model.PaymentGateway > 0 ? Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString() : string.Empty;
                                    // BillDesk - Update response in PI Service
                                    if (gateway == "BillDesk" && model.Responsecode == "0300" && string.IsNullOrEmpty(ctx.ISU_UPDATE_FLAG))
                                    {
                                        string paymentModeTobepassed = model.PaymentModeNumber;
                                        switch (model.PaymentModeNumber)
                                        {
                                            case "01":
                                                paymentModeTobepassed = "NETBANKING"; break;
                                            case "02":
                                                paymentModeTobepassed = "CREDITCARD"; break;
                                            case "03":
                                                paymentModeTobepassed = "DEBITCARD"; break;
                                            case "04":
                                                paymentModeTobepassed = "WALLETS"; break;
                                            case "10":
                                                paymentModeTobepassed = "UPI"; break;
                                            default:
                                                paymentModeTobepassed = "WALLETS"; break;

                                        }
                                        SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                                           {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString().ToUpper(),
                                                        TransactionTime = DateTime.Now,
                                                        PaymentMode = paymentModeTobepassed, //model.PaymentMode,
                                                        PaymentType = ctx.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.AmountPayable),
                                                        TransactionId = model.TransactionId
                                                    }
                                                };

                                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in PIService :" + model.TransactionId, this);

                                        SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                                        if (response != null && response.Count() > 0)
                                        {
                                            ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                                            ISU_UPDATE_MESSAGE = response[0].Message;
                                        }
                                        else
                                        {
                                            ISU_UPDATE_FLAG = "N";
                                            ISU_UPDATE_MESSAGE = "No Response";
                                        }
                                        ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                        ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                        ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                        dbcontext.SubmitChanges();
                                    }
                                    //PayTM - Update response in PI Service
                                    else if (gateway == "Paytm" && model.Responsecode == "01" && string.IsNullOrEmpty(ctx.ISU_UPDATE_FLAG))
                                    {
                                        string paymentModeTobepassed = model.PaymentMode;
                                        switch (model.PaymentMode)
                                        {
                                            case "DC":
                                                paymentModeTobepassed = "DEBITCARD"; break;
                                            case "CC":
                                                paymentModeTobepassed = "CREDITCARD"; break;
                                            case "PPI":
                                                paymentModeTobepassed = "WALLETS"; break;
                                            case "NB":
                                                paymentModeTobepassed = "NETBANKING"; break;
                                            case "UPI":
                                                paymentModeTobepassed = "UPI"; break;
                                            default:
                                                paymentModeTobepassed = model.PaymentMode; break;

                                        }
                                        string transactionId = (model.TransactionId.Length > 16) ? model.TransactionId.Substring(model.TransactionId.Length - 16, 16) : model.TransactionId;
                                        SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                                           {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString().ToUpper(),
                                                        TransactionTime = DateTime.Now,
                                                        PaymentMode = paymentModeTobepassed,//model.PaymentMode,
                                                        PaymentType = ctx.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.AmountPayable),
                                                        TransactionId = transactionId
                                                    }
                                                };

                                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in PIService :" + model.TransactionId, this);

                                        SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                                        if (response != null && response.Count() > 0)
                                        {
                                            ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                                            ISU_UPDATE_MESSAGE = response[0].Message;
                                        }
                                        else
                                        {
                                            ISU_UPDATE_FLAG = "N";
                                            ISU_UPDATE_MESSAGE = "No Response";
                                        }

                                        ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                        ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                        ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                        dbcontext.SubmitChanges();
                                    }
                                    //ICICI - Update response in PI Service
                                    else if (gateway == "ICICIBank" && model.Responsecode.TrimStart('0') == "300" && string.IsNullOrEmpty(ctx.ISU_UPDATE_FLAG))
                                    {
                                        try
                                        {
                                            string transactionId = (model.TransactionId.Length > 16) ? model.TransactionId.Substring(model.TransactionId.Length - 16, 16) : model.TransactionId;
                                            SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                                               {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString().ToUpper(),
                                                        TransactionTime = DateTime.Now,
                                                        PaymentMode = model.PaymentMode,
                                                        PaymentType = ctx.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.AmountPayable),
                                                        TransactionId = model.TransactionId
                                                    }
                                                };

                                            Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in PIService :" + model.TransactionId, this);

                                            SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                                            if (response != null && response.Count() > 0)
                                            {
                                                ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                                                ISU_UPDATE_MESSAGE = response[0].Message;
                                            }
                                            else
                                            {
                                                ISU_UPDATE_FLAG = "N";
                                                ISU_UPDATE_MESSAGE = "No Response";
                                            }
                                            ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                            ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                            ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                            dbcontext.SubmitChanges();
                                        }
                                        catch (Exception e) { Sitecore.Diagnostics.Log.Error("Method - StorePaymentResponse Store response in PI Service for ICICI:" + model.TransactionId + " : " + e.Message, this); }
                                    }
                                    //BeNow - Update response in PI Service
                                    else if (gateway == "Benow" && model.Responsecode.ToLower() == "successful" && string.IsNullOrEmpty(ctx.ISU_UPDATE_FLAG))
                                    {
                                        try
                                        {
                                            string transactionId = (model.TransactionId.Length > 16) ? model.TransactionId.Substring(model.TransactionId.Length - 16, 16) : model.TransactionId;
                                            SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                                               {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString().ToUpper(),
                                                        TransactionTime = DateTime.Now,
                                                        PaymentMode = model.PaymentMode,
                                                        PaymentType = ctx.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.AmountPayable),
                                                        TransactionId = model.TransactionId
                                                    }
                                                };

                                            Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in PIService :" + model.TransactionId, this);

                                            SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                                            if (response != null && response.Count() > 0)
                                            {
                                                ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                                                ISU_UPDATE_MESSAGE = response[0].Message;
                                            }
                                            else
                                            {
                                                ISU_UPDATE_FLAG = "N";
                                                ISU_UPDATE_MESSAGE = "No Response";
                                            }
                                            ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                            ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                            ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                            dbcontext.SubmitChanges();
                                        }
                                        catch (Exception e) { Sitecore.Diagnostics.Log.Error("Method - StorePaymentResponse Store response in PI Service for Benow:" + model.TransactionId + " : " + e.Message, this); }
                                    }
                                    // SafeXPay - Update response in PI Service
                                    else if (gateway == "SafeXPay" && model.Responsecode == "0" && string.IsNullOrEmpty(ctx.ISU_UPDATE_FLAG))
                                    {
                                        string paymentModeTobepassed = model.PaymentModeNumber;
                                        switch (model.PaymentModeNumber)
                                        {
                                            case "NB":
                                                paymentModeTobepassed = "NETBANKING"; break;
                                            case "CC":
                                                paymentModeTobepassed = "CREDITCARD"; break;
                                            case "DC":
                                                paymentModeTobepassed = "DEBITCARD"; break;
                                            case "PP":
                                                paymentModeTobepassed = "PREPAIDCARD"; break;
                                            case "WA":
                                                paymentModeTobepassed = "WALLETS"; break;
                                            case "PT":
                                                paymentModeTobepassed = "WALLETS"; break;
                                            case "UP":
                                                paymentModeTobepassed = "UPI"; break;
                                            case "CE":
                                                paymentModeTobepassed = "CREDITCARDEMI"; break;
                                            default:
                                                paymentModeTobepassed = model.PaymentModeNumber;
                                                Sitecore.Diagnostics.Log.Error("Method - SafeXPay StorePaymentResponse default Store response:" + model.PaymentModeNumber, this);
                                                break;

                                        }
                                        SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                                           {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString().ToUpper(),
                                                        TransactionTime = DateTime.Now,
                                                        PaymentMode = paymentModeTobepassed, //model.PaymentMode,
                                                        PaymentType = ctx.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.AmountPayable),
                                                        TransactionId = model.TransactionId
                                                    }
                                                };

                                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in PIService :" + model.TransactionId, this);

                                        SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                                        if (response != null && response.Count() > 0)
                                        {
                                            ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                                            ISU_UPDATE_MESSAGE = response[0].Message;
                                        }
                                        else
                                        {
                                            ISU_UPDATE_FLAG = "N";
                                            ISU_UPDATE_MESSAGE = "No Response";
                                        }
                                        ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                        ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                        ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                        dbcontext.SubmitChanges();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Sitecore.Diagnostics.Log.Error("Method - StorePaymentResponse Store response in PI Service :" + model.TransactionId + " : " + ex.Message, this);
                                }
                            }

                            #region contact log
                            ////Contactlog webservice integration in case of each payment response irrespective of success/Failure
                            //try
                            //{
                            //    Sitecore.Diagnostics.Log.Info("Contactlog webservice calling for Account: " + model.AccountNumber + ",TransactionId:" + model.TransactionId + ",Responsecode:" + model.Responsecode + ",ResponseMessage:" + model.msg, this);
                            //    var request = new DT_Contactlog_req
                            //    {
                            //        ACTIVITY = "",
                            //        CLASS = "0006",
                            //        CONTACT_DATE = model.TransactionDate,
                            //        CONTACT_TIME = DateTime.Now.ToString(),
                            //        DIRECTION = "1",
                            //        PARTNER = "",
                            //        TYPE1 = "",
                            //        VKONT = model.AccountNumber
                            //    };

                            //    if (model.PaymentGateway == (int)EnumPayment.GatewayType.BillDesk)
                            //    {
                            //        if (model.ResponseStatus.ToLower() == "success")
                            //        {
                            //            request.ACTIVITY = "0042";
                            //            request.TYPE1 = "024";
                            //            request.PARTNER = "Rs. " + model.AmountPayable + " , paid ; Transaction id " + model.TransactionId;
                            //        }
                            //        else
                            //        {
                            //            request.ACTIVITY = "0043";
                            //            request.TYPE1 = "024";
                            //            request.PARTNER = model.Remark + " ; Transaction id : " + model.TransactionId;
                            //        }
                            //    }
                            //    else if (model.PaymentGateway == (int)EnumPayment.GatewayType.Paytm)
                            //    {
                            //        if (model.ResponseStatus.ToLower() == "success")
                            //        {
                            //            request.ACTIVITY = "0042";
                            //            request.TYPE1 = "029";
                            //        }
                            //        else
                            //        {
                            //            request.ACTIVITY = "0043";
                            //            request.TYPE1 = "029";
                            //        }
                            //    }
                            //    var response = SapPiService.Services.RequestHandler.CreateContactLogWeb(request);
                            //    Sitecore.Diagnostics.Log.Info("Contactlog webservice response for Account: " + model.AccountNumber + ",TransactionId:" + model.TransactionId + ",Service response: " + response.CONTACT + "," + response.MESSAGE, this);

                            //}
                            //catch (Exception ex)
                            //{
                            //    Sitecore.Diagnostics.Log.Error("Error Contactlog webservice calling for Account: " + model.AccountNumber + ", Exception: " + ex.Message, this);
                            //}
                            #endregion contact log
                        }
                    }
                    catch (System.Exception ex)
                    {
                        // Note : Log the message on any failure to sitecore log
                        Sitecore.Diagnostics.Log.Error("Error at Add/Updating Item " + model.TransactionId + ": " + ex.Message, this);
                    }
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentResponse :" + ex.Message, this);
            }
        }

        #endregion

        #region Paytm

        public string PaytmTransactionRequestAPIRequestPost(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_Merchant_Key].Value;

            string callbackURL = itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_Response_URL_B2B].Value;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "MID", itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_Merchant_ID].Value },
                { "CHANNEL_ID", itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_CHANNEL_ID].Value },
                { "INDUSTRY_TYPE_ID", itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_INDUSTRY_TYPE_ID].Value },
                { "WEBSITE", itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_WEBSITE].Value },
                { "EMAIL", Model.Email },
                { "MOBILE_NO", Model.Mobile },
                { "CUST_ID", Model.LoginName },
                { "ORDER_ID", Model.OrderId },
                { "TXN_AMOUNT", System.Convert.ToDecimal(Model.AmountPayable.Trim()).ToString("f2") },
                //{ "CALLBACK_URL", callbackURL }, //This parameter is not mandatory. Use this to pass the callback url dynamically.
                { "MERC_UNQ_REF", Model.AccountNumber + "_" + Model.LoginName }
            };

            string checksum = CheckSum.generateCheckSum(merchantKey, parameters);

            string paytmURL = string.Format(itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_Request_URL].Value, Model.OrderId); //"https://securegw-stage.paytm.in/theia/processTransaction?orderid=" + OrderId;

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }



        //PayTM Transaction Status API for verify Status and Amount after Response
        public string PaytmTransactionStatusAPIRequestPost(IDictionary<string, string> TransactionRequestAPIResponse)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_Merchant_Key].Value;
            string merchantID = itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_Merchant_ID].Value;

            string postURL = itemInfo.Fields[Templates.PaymentConfiguration.PayTMFields.PTM_TransactionStatus_URL].Value; //"https://securegw-stage.paytm.in/merchant-status/getTxnStatus?JsonData="; 
            string order_id = TransactionRequestAPIResponse["ORDERID"];

            Dictionary<string, string> innerrequest = new Dictionary<string, string>
            {
                { "MID", merchantID },
                { "ORDERID", TransactionRequestAPIResponse["ORDERID"] }
            };
            string first_jason = new JavaScriptSerializer().Serialize(innerrequest);

            string Check = CheckSum.generateCheckSum(merchantKey, innerrequest);
            string correct_check = Check.Replace("+", "%2b");
            innerrequest.Add("CHECKSUMHASH", correct_check);
            string final = new JavaScriptSerializer().Serialize(innerrequest);
            final = final.Replace("\\", "").Replace(":\"{", ":{").Replace("}\",", "},");

            string url = postURL + final;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("ContentType", "application/json");
            request.Method = "POST";

            string responseData = string.Empty;
            using (StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                responseData = responseReader.ReadToEnd();
            }
            return responseData;
        }

        #endregion

        #region PayUMoney
        public string PayUMoneyTransactionRequestAPIRequestPost(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Merchant_Key].Value;
            string saltkKey = itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Merchant_Salt].Value;
            string hashSEQ = itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Product_Info].Value;
            string LoggedInUserId = UserProfileService.GetLoginName();

            string hashvalue = GenerateHashforPayU(Model, hashSEQ, merchantKey, saltkKey);

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "key", merchantKey },
                { "txnid", Model.OrderId },
                { "amount", System.Convert.ToDecimal(Model.AmountPayable.Trim()).ToString("g29") },
                { "productinfo", Model.AccountNumber },
                { "firstname", Model.LoginName },
                { "email", Model.Email },
                { "phone", Model.Mobile },
                { "surl", itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Resp_URL_B2B].Value },
                { "furl", itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Resp_URL_B2B].Value },
                { "curl", itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Resp_URL_B2B].Value }, //This parameter is not mandatory. Use this to pass the callback url dynamically.
                { "hash", hashvalue }
            };


            string payUURL = itemInfo.Fields[Templates.PaymentConfiguration.PayuMoneyFields.PUM_Request_URL].Value;

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1> " + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            //outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHTML += "<form method='post' action='" + payUURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }


        public string GenerateHashforPayU(ViewPayBill model, string hashseq, string marchentKey, string salt)
        {
            string[] hashVarsSeq = hashseq.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = marchentKey;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txnid")
                {
                    hash_string = hash_string + model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "amount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(model.AmountPayable).ToString("g29");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "productinfo")
                {
                    hash_string = hash_string + model.AccountNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "firstname")
                {
                    hash_string = hash_string + model.LoginName;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "email")
                {
                    hash_string = hash_string + model.Email;
                    hash_string = hash_string + '|';
                }
                else
                {
                    hash_string = hash_string + "";// isset if else
                    hash_string = hash_string + '|';
                }
            }

            hash_string += salt;// appending SALT

            hash1 = Generatehash512(hash_string).ToLower();         //ge
            return hash1;
        }

        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += string.Format("{0:x2}", x);
            }
            return hex;

        }
        #endregion

        #region EbixCash

        //Ebixcash Transaction Status API for verify Status and Amount after Response
        public string EbixTransactionStatusAPIRequestPost(IDictionary<string, string> TransactionRequestAPIResponse)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.EbixCashFields.EBIX_MERCHANT_KEY].Value;
            string responseData = string.Empty;

            string postURL = itemInfo.Fields[Templates.PaymentConfiguration.EbixCashFields.EBIX_Transaction_VerifyURL].Value; //"https://staging.itzcash.com/payment/servlet/OrderVerificationServlet"
            string order_id = TransactionRequestAPIResponse["transactionid"];

            List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("orderid", TransactionRequestAPIResponse["orderid"]),
                new KeyValuePair<string, string>("merchanttypekey", merchantKey),
                new KeyValuePair<string, string>("productcost", TransactionRequestAPIResponse["productcost"]),
                new KeyValuePair<string, string>("transactionid", TransactionRequestAPIResponse["transactionid"])
            };

            using (HttpClient client = new HttpClient())
            {
                System.Threading.Tasks.Task<HttpResponseMessage> res = client.PostAsync(postURL, new FormUrlEncodedContent(keyValues));
                res.Result.EnsureSuccessStatusCode();
                responseData = res.Result.Content.ReadAsStringAsync().Result;
            }

            return responseData;
        }
        //Payment Request Form Post Call
        public string EbixcashTransactionRequestAPIRequestPost(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfiguration.EbixCashFields.EBIX_MERCHANT_KEY].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfiguration.EbixCashFields.EBIX_Response_URL].Value;
            string S2ScallbackURL = itemInfo.Fields[Templates.PaymentConfiguration.EbixCashFields.EBIX_S2S_URL].Value;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "merchanttypekey", merchantKey },
                { "orderid", Model.OrderId },
                { "productcost", (System.Convert.ToDecimal(Model.AmountPayable) * 100).ToString("f2") },
                { "accountno", Model.AccountNumber },
                { "email", Model.Email },
                { "mobile_no", Model.Mobile },
                { "loginname", Model.LoginName },
                { "redirectionurl", callbackURL }, //This parameter is not mandatory. Use this to pass the callback url dynamically.
                                                   // parameters.Add("processingurl", S2ScallbackURL);
                { "caccountno", Model.AccountNumber }
            };
            //info1
            //caccountno
            string paytmURL = itemInfo.Fields[Templates.PaymentConfiguration.EbixCashFields.EBIX_Request_URL].Value;

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }
        #endregion

        #region ICICI
        public string ICICITransactionRequestAPIRequestPost(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string MerchantCode = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.MerchantCode].Value;
            string SchemeCode = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.SchemeCode].Value;
            string EncryptionKey = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.EncryptionKey].Value;
            string EncryptionIV = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.EncryptionIV].Value;
            string BankCode = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.BankCode].Value;
            string Currency = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.Currency].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.ICICI_Response_URL_B2B].Value;
            string ITC = itemInfo.Fields[Templates.PaymentConfiguration.ICICIFields.ITC].Value;

            string shiipingdetails = SchemeCode + "_" + Model.AmountPayable + "_0.0"; //SchemeCode + "_3.50_0.0";
            string txndate = System.DateTime.Now.ToString("dd-MM-yyyy");
            RequestURL objRequestURL = new RequestURL();

            string amount = string.Empty;
            if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
            {
                if (Model.SecurityDepositAmountType == "Actual")
                    amount = Model.SecurityDeposit;
                else
                    amount = Model.SecurityDepositPartial.ToString();
            }
            else
            {
                amount = Model.AmountPayable;
            }

            string response = objRequestURL.SendRequest("T", MerchantCode, Model.OrderId, ITC, amount, Currency, Model.LoginName, callbackURL, "", "", shiipingdetails, txndate, Model.Email, "", BankCode, Model.AccountNumber, "", EncryptionKey, EncryptionIV);
            //string response = objRequestURL.SendRequest("T", "T484", "26092rtret01845654142711", "Saleel_K", "130.00", "INR", "mitesh123", "http://electricity.dev.local/PaymentGateway/ICICI_Callback", "", "", "test_130.00_0.0", "26-09-2018", "", "", "10", "", "", "4221685887CMQFNW", "9130484889IWSWEN");
            //public string SendRequest(string RequestType, string MerchantCode, string MerchantTxnRefNumber, string ITC, string Amount, string CurrencyCode, string UniqueCustomerId, string ReturnURL, string S2SReturnURL, string TPSLTxnID, string ShoppingCartDetails, string txnDate, string Email, string MobileNumber, string BankCode, string CustomerName, string CardId, string IsKey, string IsIv);
            string strResponse = response.ToUpper();

            Sitecore.Diagnostics.Log.Info("Payment Gateway- ICICITransactionRequestAPIRequestPost Request - " + response, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            if (strResponse.StartsWith("ERROR"))
            {
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Error Msg", "There is some error. Please try again.") + "</h1></center>";
                outputHTML += "<form method='post' action='' name='f1'>";
            }
            else
            {
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
                outputHTML += "<form method='post' action='" + response + "' name='f1'>";
            }
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }

        public string ICICITransactionRequestAPIRequestPostRevamp(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

            string MerchantCode = itemInfo.Fields[Templates.PaymentConfigurationRevamp.ICICIFields.MerchantCode].Value;
            string SchemeCode = itemInfo.Fields[Templates.PaymentConfigurationRevamp.ICICIFields.SchemeCode].Value;
            string EncryptionKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.ICICIFields.EncryptionKey].Value;
            string EncryptionIV = itemInfo.Fields[Templates.PaymentConfigurationRevamp.ICICIFields.EncryptionIV].Value;
            string BankCode = itemInfo.Fields[Templates.PaymentConfigurationRevamp.ICICIFields.BankCode].Value;
            string Currency = itemInfo.Fields[Templates.PaymentConfigurationRevamp.ICICIFields.Currency].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.ICICIFields.ICICI_Response_URL_B2B].Value;
            string ITC = itemInfo.Fields[Templates.PaymentConfigurationRevamp.ICICIFields.ITC].Value;

            string shiipingdetails = SchemeCode + "_" + Model.AmountPayable + "_0.0"; //SchemeCode + "_3.50_0.0";
            string txndate = System.DateTime.Now.ToString("dd-MM-yyyy");
            RequestURL objRequestURL = new RequestURL();

            string amount = string.Empty;
            if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
            {
                if (Model.SecurityDepositAmountType == "Actual")
                    amount = Model.SecurityDeposit;
                else
                    amount = Model.SecurityDepositPartial.ToString();
            }
            else
            {
                amount = Model.AmountPayable;
            }

            string response = objRequestURL.SendRequest("T", MerchantCode, Model.OrderId, ITC, amount, Currency, Model.LoginName, callbackURL, "", "", shiipingdetails, txndate, Model.Email, "", BankCode, Model.AccountNumber, "", EncryptionKey, EncryptionIV);
            //string response = objRequestURL.SendRequest("T", "T484", "26092rtret01845654142711", "Saleel_K", "130.00", "INR", "mitesh123", "http://electricity.dev.local/PaymentGateway/ICICI_Callback", "", "", "test_130.00_0.0", "26-09-2018", "", "", "10", "", "", "4221685887CMQFNW", "9130484889IWSWEN");
            //public string SendRequest(string RequestType, string MerchantCode, string MerchantTxnRefNumber, string ITC, string Amount, string CurrencyCode, string UniqueCustomerId, string ReturnURL, string S2SReturnURL, string TPSLTxnID, string ShoppingCartDetails, string txnDate, string Email, string MobileNumber, string BankCode, string CustomerName, string CardId, string IsKey, string IsIv);
            string strResponse = response.ToUpper();

            Sitecore.Diagnostics.Log.Info("Payment Gateway- ICICITransactionRequestAPIRequestPost Request - " + response, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            if (strResponse.StartsWith("ERROR"))
            {
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Error Msg", "There is some error. Please try again.") + "</h1></center>";
                outputHTML += "<form method='post' action='' name='f1'>";
            }
            else
            {
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
                outputHTML += "<form method='post' action='" + response + "' name='f1'>";
            }
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }

        #endregion

        #region BillDesk

        //Generate CheckSum Value
        public string GetHMACSHA256(string text, string key)
        {
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] hashValue;
            byte[] keybyt = encoder.GetBytes(key);
            byte[] message = encoder.GetBytes(text);

            HMACSHA256 hashString = new HMACSHA256(keybyt);
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += string.Format("{0:x2}", x);
            }
            return hex.ToUpper();
        }

        public string HDFCTransactionRequestAPIRequestPost(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string appId = itemInfo.Fields[Templates.PaymentConfiguration.HDFCFields.HDFC_App_Id].Value;
            string secretKey = itemInfo.Fields[Templates.PaymentConfiguration.HDFCFields.HDFC_Secret_Key].Value;
            string callbackURL_Success = itemInfo.Fields[Templates.PaymentConfiguration.HDFCFields.HDFC_Resp_URL_Success].Value;
            string callbackURL_Error = itemInfo.Fields[Templates.PaymentConfiguration.HDFCFields.HDFC_Resp_URL_Error].Value;


            //string appId = "9017104"; //Terminal ID
            //string secretKey = "password1";

            var paymentdata = new PaymentRequestData
            {
                OrderId = Model.OrderId,
                CustomerReference = Model.LoginName,
                CustomerName = Model.Name,
                CustomerPhone = Model.Mobile,
                CustomerEmail = Model.Email,
                OrderAmount = System.Convert.ToDecimal(Model.AmountPayable),
                ReturnUrl = callbackURL_Success,// "https://preprod.adanielectricity.com/api/accounts/HDFC_UPI_CallbackS2S",
                ErrorUrl = callbackURL_Error //"https://preprod.adanielectricity.com/api/accounts/HDFC_UPI_Callbackerror"
            };

            var service = new OrderService();

            var orderLink = service.GetOrderLink(paymentdata, appId, secretKey);

            //return orderLink.PaymentLink;
            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + orderLink.PaymentLink + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='msg' value='" + Model.AccountNumber + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }
        public string BillDeskTransactionRequestAPIRequestPost_SolarApplication(SolarApplicationDetail Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string paytmURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Request_URL].Value;
            string MerchantID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Merchant_ID].Value;
            string SecurityID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_SECURITY_ID].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Resp_URL_B2B].Value;
            string ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_ChecksumKey].Value;
            string requestMsg = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Req_Msg].Value;
            string curreny = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_CURRENCY_TYPE].Value;

            //Old - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|NA|NA|NA|NA|RU
            //New - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|AdditionalInfo4|AdditionalInfo5|NA|NA|RU

            string[] hashVarsSeq = requestMsg.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;

            string amount = string.Empty;
            string paymenttype = string.Empty;

            amount = Model.AmountTobePaid;
            paymenttype = "SOLAR APPLICATION PAYMENT";

            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "MerchantID")
                {
                    hash_string = MerchantID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CustomerID")
                {
                    hash_string = hash_string + Model.AccountNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TxnAmount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(amount).ToString("f2");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CurrencyType")
                {
                    hash_string = hash_string + curreny;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField1")
                {
                    hash_string = hash_string + "R";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "SecurityID")
                {
                    hash_string = hash_string + SecurityID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField2")
                {
                    hash_string = hash_string + "F";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo1")
                {
                    hash_string = hash_string + Model.ApplicationNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo2")
                {
                    hash_string = hash_string + Model.TempRegistrationNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo3")
                {
                    hash_string = hash_string + paymenttype;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo4")
                {
                    hash_string = hash_string + Model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo5")
                {
                    hash_string = hash_string + Model.EmailAddress;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo6")
                {
                    hash_string = hash_string + Model.UserId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "RU")
                {
                    hash_string = hash_string + callbackURL;
                    // hash_string = hash_string + '|';
                }
                else
                {
                    hash_string = hash_string + "NA";// isset if else
                    hash_string = hash_string + '|';
                }
            }

            string checksumvalue = GetHMACSHA256(hash_string, ChecksumKey);
            if (!string.IsNullOrEmpty(checksumvalue))
            {
                hash_string = hash_string + "|" + checksumvalue;
            }

            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskTransactionRequestAPIRequestPost Request - " + hash_string, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='msg' value='" + hash_string + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }

        public string BillDeskTransactionRequestAPIRequestPost(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string paytmURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Request_URL].Value;
            string MerchantID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Merchant_ID].Value;
            string SecurityID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_SECURITY_ID].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Resp_URL_B2B].Value;
            string ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_ChecksumKey].Value;
            string requestMsg = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Req_Msg].Value;
            string curreny = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_CURRENCY_TYPE].Value;

            //Old - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|NA|NA|NA|NA|RU
            //New - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|AdditionalInfo4|AdditionalInfo5|NA|NA|RU

            string[] hashVarsSeq = requestMsg.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;

            string amount = string.Empty;
            string paymenttype = string.Empty;
            if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
            {
                if (Model.SecurityDepositAmountType == "Actual")
                    amount = Model.SecurityDeposit;
                else
                    amount = Model.SecurityDepositPartial.ToString();
                paymenttype = "SECURITY DEPOSIT";
            }
            else
            {
                amount = Model.AmountPayable;
                paymenttype = "BILL PAYMENT";
            }

            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "MerchantID")
                {
                    hash_string = MerchantID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CustomerID")
                {
                    hash_string = hash_string + Model.AccountNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TxnAmount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(amount).ToString("f2");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CurrencyType")
                {
                    hash_string = hash_string + curreny;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField1")
                {
                    hash_string = hash_string + "R";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "SecurityID")
                {
                    hash_string = hash_string + SecurityID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField2")
                {
                    hash_string = hash_string + "F";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo1")
                {
                    hash_string = hash_string + Model.CycleNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo2")
                {
                    hash_string = hash_string + Model.PaymentDueDate.Replace("-", "");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo3")
                {
                    hash_string = hash_string + paymenttype;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo4")
                {
                    hash_string = hash_string + Model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo5")
                {
                    hash_string = hash_string + Model.Email;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo6")
                {
                    hash_string = hash_string + Model.LoginName;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "RU")
                {
                    hash_string = hash_string + callbackURL;
                    // hash_string = hash_string + '|';
                }
                else
                {
                    hash_string = hash_string + "NA";// isset if else
                    hash_string = hash_string + '|';
                }
            }

            string checksumvalue = GetHMACSHA256(hash_string, ChecksumKey);
            if (!string.IsNullOrEmpty(checksumvalue))
            {
                hash_string = hash_string + "|" + checksumvalue;
            }

            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskTransactionRequestAPIRequestPost Request - " + hash_string, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='msg' value='" + hash_string + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }

        public string BillDeskSDTransactionRequestAPIRequestPost(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string paytmURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskSecurityDepositeFields.BDSD_Request_URL].Value;
            string MerchantID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskSecurityDepositeFields.BDSD_Merchant_ID].Value;
            string SecurityID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskSecurityDepositeFields.BDSD_SECURITY_ID].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskSecurityDepositeFields.BDSD_Resp_URL_B2B].Value;
            string ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskSecurityDepositeFields.BDSD_ChecksumKey].Value;
            string requestMsg = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskSecurityDepositeFields.BDSD_Req_Msg].Value;
            string curreny = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskSecurityDepositeFields.BDSD_CURRENCY_TYPE].Value;

            //Old - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|NA|NA|NA|NA|RU
            //New - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|AdditionalInfo4|AdditionalInfo5|NA|NA|RU

            string[] hashVarsSeq = requestMsg.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;

            string amount = string.Empty;
            string paymenttype = string.Empty;
            if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
            {
                if (Model.SecurityDepositAmountType == "Actual")
                    amount = Model.SecurityDeposit;
                else
                    amount = Model.SecurityDepositPartial.ToString();
                paymenttype = "SECURITY DEPOSIT";
            }
            else
            {
                amount = Model.AmountPayable;
                paymenttype = "BILL PAYMENT";
            }

            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "MerchantID")
                {
                    hash_string = MerchantID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CustomerID")
                {
                    hash_string = hash_string + Model.AccountNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TxnAmount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(amount).ToString("f2");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CurrencyType")
                {
                    hash_string = hash_string + curreny;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField1")
                {
                    hash_string = hash_string + "R";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "SecurityID")
                {
                    hash_string = hash_string + SecurityID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField2")
                {
                    hash_string = hash_string + "F";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo1")
                {
                    hash_string = hash_string + Model.CycleNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo2")
                {
                    hash_string = hash_string + System.DateTime.Now.ToString("yyyyMMdd");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo3")
                {
                    hash_string = hash_string + paymenttype;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo4")
                {
                    hash_string = hash_string + Model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo5")
                {
                    hash_string = hash_string + Model.Email;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo6")
                {
                    hash_string = hash_string + Model.LoginName;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "RU")
                {
                    hash_string = hash_string + callbackURL;
                    // hash_string = hash_string + '|';
                }
                else
                {
                    hash_string = hash_string + "NA";// isset if else
                    hash_string = hash_string + '|';
                }
            }

            string checksumvalue = GetHMACSHA256(hash_string, ChecksumKey);
            if (!string.IsNullOrEmpty(checksumvalue))
            {
                hash_string = hash_string + "|" + checksumvalue;
            }

            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskTransactionRequestAPIRequestPost Request - " + hash_string, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='msg' value='" + hash_string + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }

        public string BillDeskVDSTransactionRequestAPIRequestPost(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

            string paytmURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_Request_URL].Value;
            string MerchantID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_Merchant_ID].Value;
            string SecurityID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_SECURITY_ID].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_Resp_URL_B2B].Value;
            string ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
            string requestMsg = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_Req_Msg].Value;
            string curreny = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_CURRENCY_TYPE].Value;

            //MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|NA|NA|NA|NA|RU

            string[] hashVarsSeq = requestMsg.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;

            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "MerchantID")
                {
                    hash_string = MerchantID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CustomerID")
                {
                    hash_string = hash_string + Model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TxnAmount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(Model.AmountPayable).ToString("f2");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CurrencyType")
                {
                    hash_string = hash_string + curreny;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField1")
                {
                    hash_string = hash_string + "R";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "SecurityID")
                {
                    hash_string = hash_string + SecurityID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField2")
                {
                    hash_string = hash_string + "F";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo1")
                {
                    hash_string = hash_string + Model.AccountNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo2")
                {
                    hash_string = hash_string + Model.LoginName;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo3")
                {
                    hash_string = hash_string + Model.Email;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "RU")
                {
                    hash_string = hash_string + callbackURL;
                    // hash_string = hash_string + '|';
                }
                else
                {
                    hash_string = hash_string + "NA";// isset if else
                    hash_string = hash_string + '|';
                }
            }

            string checksumvalue = GetHMACSHA256(hash_string, ChecksumKey);
            if (!string.IsNullOrEmpty(checksumvalue))
            {
                hash_string = hash_string + "|" + checksumvalue;
            }

            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskVDSTransactionRequestAPIRequestPost Request - " + hash_string, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='msg' value='" + hash_string + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }
        #endregion


        #region Billdesk ElectricityNew

        public void StorePaymentRequestRevamp(ViewPayBill model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    if (model.PaymentGateway == (int)EnumPayment.GatewayType.Benow || model.PaymentGateway == (int)EnumPayment.GatewayType.DBSUPI || model.PaymentGateway == (int)EnumPayment.GatewayType.CITYUPI)
                    {
                        InvoiceHistory result = SapPiService.Services.RequestHandler.FetchInvoiceHistory(model.AccountNumber);
                        if (result != null && result.InvoiceLines != null && result.InvoiceLines.Any(x => x.BillMonth == model.BillMonth))
                        {

                            model.OrderId = model.AccountNumber + result.InvoiceLines.Where(x => x.BillMonth == model.BillMonth).ToList().FirstOrDefault().InvoiceNumber + EncryptionDecryption.GenerateShortRandomOrderId(string.Empty); ;
                            Sitecore.Diagnostics.Log.Info("StorePaymentRequest Benow Billmonth and Account Match:" + model.OrderId, this);
                        }
                        else
                        {
                            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                            {
                                if (model.PaymentGateway == (int)EnumPayment.GatewayType.CITYUPI)
                                {
                                    model.OrderId = model.AccountNumber + EncryptionDecryption.GenerateRandomOrderId(string.Empty);

                                    while (dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == model.OrderId).FirstOrDefault() != null)
                                    {
                                        model.OrderId = model.OrderId = model.AccountNumber + EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                                    }


                                    Sitecore.Diagnostics.Log.Info("StorePaymentRequest Benow Billmonth and Account Match:" + model.OrderId, this);
                                }
                                else
                                {
                                    model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                                    while (dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == model.OrderId).FirstOrDefault() != null)
                                    {
                                        model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                                    }
                                    Sitecore.Diagnostics.Log.Info("StorePaymentRequest Benow Billmonth and Account Match not found:" + model.OrderId, this);
                                }

                            }
                        }
                    }
                    else
                    {
                        model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            while (dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == model.OrderId).FirstOrDefault() != null)
                            {
                                model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                            }

                        }

                    }

                }
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

                try
                {
                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database Order Id : - " + model.OrderId, this);
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        PaymentHistory objdata = new PaymentHistory();
                        Guid newid = Guid.NewGuid();
                        objdata.Id = newid;
                        objdata.Remark = model.Remark;
                        objdata.AccountNumber = model.AccountNumber ?? string.Empty;
                        objdata.UserId = model.LoginName;
                        objdata.OrderId = model.OrderId;
                        objdata.Amount = amount ?? string.Empty;
                        objdata.RequestTime = System.Convert.ToDateTime(DateTime.UtcNow);
                        objdata.Checksumkey = string.Empty;
                        objdata.CurrencyType = model.CurrencyType ?? string.Empty;
                        objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString() ?? string.Empty;
                        objdata.AdvanceAmmount = model.AdvanceAmmount.ToString() ?? string.Empty;
                        objdata.msg = model.msg ?? string.Empty;
                        objdata.PaymentType = model.PaymentType ?? string.Empty;
                        objdata.UserType = model.UserType ?? string.Empty;
                        objdata.Email = model.Email ?? string.Empty;
                        objdata.Mobile = model.Mobile ?? string.Empty;
                        objdata.Created_Date = System.DateTime.Now;
                        objdata.CreatedBy = model.LoginName;
                        objdata.EMIOption = model.ProceedWithEMI;
                        objdata.EMIAmount = model.EMIInstallmentAmount.ToString();
                        objdata.EMIOutstanding = model.EMIOutstandingAmount.ToString();
                        dbcontext.PaymentHistories.InsertOnSubmit(objdata);
                        dbcontext.SubmitChanges();

                    }
                }
                catch (System.Exception ex)
                {
                    // Note : Log the message on any failure to sitecore log
                    Sitecore.Diagnostics.Log.Error("Error at Updating Item " + model.LoginName + ": " + ex.Message, this);
                }

            }

            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentRequest :" + ex.Message, this);
            }


        }

        public string BillDeskTransactionRequestAPIRequestPostRevamp(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

            string paytmURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_Request_URL].Value;
            string MerchantID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_Merchant_ID].Value;
            string SecurityID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_SECURITY_ID].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_Resp_URL_B2B].Value;
            string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_ChecksumKey].Value;
            string requestMsg = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_Req_Msg].Value;
            string curreny = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskFields.BDSK_CURRENCY_TYPE].Value;

            //Old - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|NA|NA|NA|NA|RU
            //New - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|AdditionalInfo4|AdditionalInfo5|NA|NA|RU

            string[] hashVarsSeq = requestMsg.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;

            string amount = string.Empty;
            string paymenttype = string.Empty;
            if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
            {
                if (Model.SecurityDepositAmountType == "Actual")
                    amount = Model.SecurityDeposit;
                else
                    amount = Model.SecurityDepositPartial.ToString();
                paymenttype = "SECURITY DEPOSIT";
            }
            else
            {
                amount = Model.AmountPayable;
                paymenttype = "BILL PAYMENT";
            }

            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "MerchantID")
                {
                    hash_string = MerchantID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CustomerID")
                {
                    hash_string = hash_string + Model.AccountNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TxnAmount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(amount).ToString("f2");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CurrencyType")
                {
                    hash_string = hash_string + curreny;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField1")
                {
                    hash_string = hash_string + "R";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "SecurityID")
                {
                    hash_string = hash_string + SecurityID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField2")
                {
                    hash_string = hash_string + "F";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo1")
                {

                    hash_string = hash_string + Model.CycleNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo2")
                {
                    hash_string = hash_string + Model.PaymentDueDate.Replace("-", "");
                    // hash_string = hash_string + Model.PaymentDueDate;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo3")
                {
                    hash_string = hash_string + paymenttype;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo4")
                {
                    hash_string = hash_string + Model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo5")
                {
                    hash_string = hash_string + Model.Email;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo6")
                {
                    hash_string = hash_string + Model.LoginName;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "RU")
                {
                    hash_string = hash_string + callbackURL;
                    // hash_string = hash_string + '|';
                }
                else
                {
                    hash_string = hash_string + "NA";// isset if else
                    hash_string = hash_string + '|';
                }
            }
            //hash_string="BSESTST | 100517857 | NA | 900.00 | NA | NA | NA | INR | NA | R | bsestst | NA | NA | F | 21 | 20170705 | BILL PAYMENT | T8GE02I3BWEC3PK | mahajan23@gmail.com | 100517857 | NA| http://electricity.dev.local/PaymentGateway/BillDesk_Callback";
            string checksumvalue = GetHMACSHA256(hash_string, ChecksumKey);
            if (!string.IsNullOrEmpty(checksumvalue))
            {

                hash_string = hash_string + "|" + checksumvalue;
            }

            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskTransactionRequestAPIRequestPost Request - " + hash_string, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h2>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h2></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='msg' value='" + hash_string + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }

        public string BillDeskSDTransactionRequestAPIRequestPostRevamp(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

            string paytmURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskSecurityDepositeFields.BDSD_Request_URL].Value;
            string MerchantID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskSecurityDepositeFields.BDSD_Merchant_ID].Value;
            string SecurityID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskSecurityDepositeFields.BDSD_SECURITY_ID].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskSecurityDepositeFields.BDSD_Resp_URL_B2B].Value;
            string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskSecurityDepositeFields.BDSD_ChecksumKey].Value;
            string requestMsg = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskSecurityDepositeFields.BDSD_Req_Msg].Value;
            string curreny = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskSecurityDepositeFields.BDSD_CURRENCY_TYPE].Value;

            //Old - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|NA|NA|NA|NA|RU
            //New - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|AdditionalInfo4|AdditionalInfo5|NA|NA|RU

            string[] hashVarsSeq = requestMsg.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;

            string amount = string.Empty;
            string paymenttype = string.Empty;
            if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
            {
                if (Model.SecurityDepositAmountType == "Actual")
                    amount = Model.SecurityDeposit;
                else
                    amount = Model.SecurityDeposit.ToString();
                paymenttype = "SECURITY DEPOSIT";
            }
            else
            {
                amount = Model.AmountPayable;
                paymenttype = "BILL PAYMENT";
            }

            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "MerchantID")
                {
                    hash_string = MerchantID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CustomerID")
                {
                    hash_string = hash_string + Model.AccountNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TxnAmount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(amount).ToString("f2");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CurrencyType")
                {
                    hash_string = hash_string + curreny;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField1")
                {
                    hash_string = hash_string + "R";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "SecurityID")
                {
                    hash_string = hash_string + SecurityID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField2")
                {
                    hash_string = hash_string + "F";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo1")
                {
                    hash_string = hash_string + Model.CycleNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo2")
                {
                    hash_string = hash_string + System.DateTime.Now.ToString("yyyyMMdd");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo3")
                {
                    hash_string = hash_string + paymenttype;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo4")
                {
                    hash_string = hash_string + Model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo5")
                {
                    hash_string = hash_string + Model.Email;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo6")
                {
                    hash_string = hash_string + Model.LoginName;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "RU")
                {
                    hash_string = hash_string + callbackURL;
                    // hash_string = hash_string + '|';
                }
                else
                {
                    hash_string = hash_string + "NA";// isset if else
                    hash_string = hash_string + '|';
                }
            }

            string checksumvalue = GetHMACSHA256(hash_string, ChecksumKey);
            if (!string.IsNullOrEmpty(checksumvalue))
            {
                hash_string = hash_string + "|" + checksumvalue;
            }

            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskTransactionRequestAPIRequestPost Request - " + hash_string, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h2>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h2></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='msg' value='" + hash_string + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }

        public string BillDeskVDSTransactionRequestAPIRequestPostRevamp(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

            string paytmURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_Request_URL].Value;
            string MerchantID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_Merchant_ID].Value;
            string SecurityID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_SECURITY_ID].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_Resp_URL_B2B].Value;
            string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
            string requestMsg = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_Req_Msg].Value;
            string curreny = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BillDeskVDSFields.BDVDS_CURRENCY_TYPE].Value;

            //MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|NA|NA|NA|NA|RU

            string[] hashVarsSeq = requestMsg.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;

            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "MerchantID")
                {
                    hash_string = MerchantID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CustomerID")
                {
                    hash_string = hash_string + Model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TxnAmount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(Model.AmountPayable).ToString("f2");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CurrencyType")
                {
                    hash_string = hash_string + curreny;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField1")
                {
                    hash_string = hash_string + "R";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "SecurityID")
                {
                    hash_string = hash_string + SecurityID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField2")
                {
                    hash_string = hash_string + "F";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo1")
                {
                    hash_string = hash_string + Model.AccountNumber;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo2")
                {
                    hash_string = hash_string + Model.LoginName;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo3")
                {
                    hash_string = hash_string + Model.Email;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "RU")
                {
                    hash_string = hash_string + callbackURL;
                    // hash_string = hash_string + '|';
                }
                else
                {
                    hash_string = hash_string + "NA";// isset if else
                    hash_string = hash_string + '|';
                }
            }

            string checksumvalue = GetHMACSHA256(hash_string, ChecksumKey);
            if (!string.IsNullOrEmpty(checksumvalue))
            {
                hash_string = hash_string + "|" + checksumvalue;
            }

            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskVDSTransactionRequestAPIRequestPostRevamp Request - " + hash_string, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h2>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h2></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='msg' value='" + hash_string + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }
        #endregion

        #region Paytm Revamp

        public string PaytmTransactionRequestAPIRequestPostRevamp(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_Merchant_Key].Value;

            string callbackURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_Response_URL_B2B].Value;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "MID", itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_Merchant_ID].Value },
                { "CHANNEL_ID", itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_CHANNEL_ID].Value },
                { "INDUSTRY_TYPE_ID", itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_INDUSTRY_TYPE_ID].Value },
                { "WEBSITE", itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_WEBSITE].Value },
                { "EMAIL", Model.Email },
                { "MOBILE_NO", Model.Mobile },
                { "CUST_ID", Model.LoginName },
                { "ORDER_ID", Model.OrderId },
                { "TXN_AMOUNT", System.Convert.ToDecimal(Model.AmountPayable.Trim()).ToString("f2") },
               // { "CALLBACK_URL", callbackURL }, //This parameter is not mandatory. Use this to pass the callback url dynamically.
                { "MERC_UNQ_REF", Model.AccountNumber + "_" + Model.LoginName }
            };

            string checksum = CheckSum.generateCheckSum(merchantKey, parameters);

            string paytmURL = string.Format(itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_Request_URL].Value, Model.OrderId); //"https://securegw-stage.paytm.in/theia/processTransaction?orderid=" + OrderId;

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }

        //PayTM Transaction Status API for verify Status and Amount after Response
        public string PaytmTransactionStatusAPIRequestPostRevamp(IDictionary<string, string> TransactionRequestAPIResponse)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_Merchant_Key].Value;
            string merchantID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_Merchant_ID].Value;

            string postURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.PayTMFields.PTM_TransactionStatus_URL].Value; //"https://securegw-stage.paytm.in/merchant-status/getTxnStatus?JsonData="; 
            string order_id = TransactionRequestAPIResponse["ORDERID"];

            Dictionary<string, string> innerrequest = new Dictionary<string, string>
            {
                { "MID", merchantID },
                { "ORDERID", TransactionRequestAPIResponse["ORDERID"] }
            };
            string first_jason = new JavaScriptSerializer().Serialize(innerrequest);

            string Check = CheckSum.generateCheckSum(merchantKey, innerrequest);
            string correct_check = Check.Replace("+", "%2b");
            innerrequest.Add("CHECKSUMHASH", correct_check);
            string final = new JavaScriptSerializer().Serialize(innerrequest);
            final = final.Replace("\\", "").Replace(":\"{", ":{").Replace("}\",", "},");

            string url = postURL + final;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("ContentType", "application/json");
            request.Method = "POST";

            string responseData = string.Empty;
            using (StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                responseData = responseReader.ReadToEnd();
            }
            return responseData;
        }

        #endregion

        #region BBPS

        public bool StorePaymentRequestBBPS(BBPSModel model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequestBBPS Store REquest called :" + model.TransactionId, this);
                string ISU_UPDATE_FLAG = string.Empty, ISU_UPDATE_MESSAGE = string.Empty;
                DateTime dt = DateTime.Now;
                if (!string.IsNullOrEmpty(model.TransactionDate))
                {
                    dt = System.Convert.ToDateTime(model.TransactionDate);
                }
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    //model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                    SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                       {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = Enum.GetName(typeof(EnumPayment.GatewayType), 2).ToString().ToUpper(),
                                                        TransactionTime = dt,
                                                        PaymentMode ="BBPS",// model.PaymentMode,
                                                        PaymentType = model.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.AmountPayable),
                                                        TransactionId = model.TransactionId
                                                    }
                                                };

                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequestBBPS Store REquest in PIService :" + model.TransactionId, this);

                    SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                    if (response != null && response.Count() > 0)
                    {
                        ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                        ISU_UPDATE_MESSAGE = response[0].Message;
                    }
                    else
                    {
                        ISU_UPDATE_FLAG = "N";
                        ISU_UPDATE_MESSAGE = "No Response";
                    }


                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database Order Id : - " + model.OrderId, this);
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            PaymentHistory objdata = new PaymentHistory();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            objdata.AccountNumber = model.AccountNumber ?? string.Empty;
                            objdata.UserId = model.AccountNumber;
                            objdata.OrderId = model.OrderId;
                            objdata.Amount = model.AmountPayable ?? string.Empty;
                            objdata.RequestTime = System.Convert.ToDateTime(dt);
                            objdata.Checksumkey = string.Empty;
                            objdata.CurrencyType = "INR";
                            objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), 7).ToString() ?? string.Empty;
                            objdata.AdvanceAmmount = string.Empty;
                            objdata.Remark = model.Remark ?? string.Empty;
                            objdata.PaymentType = model.PaymentType ?? string.Empty;
                            objdata.UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/GuestUser", "Guest");
                            objdata.Email = model.Email ?? string.Empty;
                            objdata.Mobile = model.Mobile ?? string.Empty;
                            objdata.TransactionId = model.TransactionId;
                            objdata.Responsecode = model.Responsecode;
                            objdata.Status = model.ResponseStatus;
                            objdata.PaymentMode = model.PaymentMode;
                            objdata.ResponseMsg = JsonConvert.SerializeObject(model);
                            objdata.Created_Date = System.DateTime.Now;
                            objdata.CreatedBy = model.AccountNumber;
                            objdata.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                            objdata.ISU_UPDATE_DATETIME = System.DateTime.Now;
                            objdata.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                            dbcontext.PaymentHistories.InsertOnSubmit(objdata);
                            dbcontext.SubmitChanges();
                        }

                        return true;
                    }
                    catch (System.Exception ex)
                    {
                        // Note : Log the message on any failure to sitecore log
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item " + model.AccountNumber + ": " + ex.Message, this);
                        return false;
                    }


                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentRequest :" + ex.Message, this);
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    PaymentHistory objdata = new PaymentHistory();
                    Guid newid = Guid.NewGuid();
                    objdata.Id = newid;
                    objdata.AccountNumber = model.AccountNumber ?? string.Empty;
                    objdata.UserId = model.AccountNumber;
                    objdata.OrderId = model.OrderId;
                    objdata.Amount = model.AmountPayable ?? string.Empty;
                    objdata.RequestTime = System.Convert.ToDateTime(DateTime.UtcNow);
                    objdata.Checksumkey = string.Empty;
                    objdata.CurrencyType = "INR";
                    objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), 7).ToString() ?? string.Empty;
                    objdata.AdvanceAmmount = string.Empty;
                    objdata.Remark = model.Remark ?? string.Empty;
                    objdata.PaymentType = model.PaymentType ?? string.Empty;
                    objdata.UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/GuestUser", "Guest");
                    objdata.Email = model.Email ?? string.Empty;
                    objdata.Mobile = model.Mobile ?? string.Empty;
                    objdata.TransactionId = model.TransactionId;
                    objdata.Responsecode = model.Responsecode;
                    objdata.Status = model.ResponseStatus;
                    objdata.PaymentMode = model.PaymentMode;
                    objdata.Created_Date = System.DateTime.Now;
                    objdata.CreatedBy = model.AccountNumber;
                    objdata.msg = "Exception";
                    objdata.ISU_UPDATE_FLAG = "N";
                    objdata.ISU_UPDATE_DATETIME = System.DateTime.Now;
                    objdata.ISU_UPDATE_MESSAGE = ex.Message;
                    dbcontext.PaymentHistories.InsertOnSubmit(objdata);
                    dbcontext.SubmitChanges();
                }
                return false;
            }

        }
        #endregion

        #region DBS

        public string DBSUPITransactionRequestAPIRequestGET(ViewPayBill Model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside BENOWTransactionRequestAPIRequestGET", this);
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string DBS_PayeeVPA = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_PayeeVPA].Value;
                string DBS_PayeeName = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_PayeeName].Value;
                string DBS_ORGID = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_ORGID].Value;
                string DBS_DefaultTN = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_DefaultTN].Value;
                string DBS_Currency = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_Currency].Value;
                string DBS_ResponseCallbackURL = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_ResponseCallbackURL].Value;
                string DBS_ModeOfTransaction = itemInfo.Fields[Templates.PaymentConfiguration.DBSFields.DBS_ModeOfTransaction].Value;

                string txndate = System.DateTime.Now.ToString("dd-MM-yyyy");
                string amount = string.Empty;
                if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
                {
                    amount = Model.SecurityDeposit;
                }
                else
                {
                    amount = Model.AmountPayable;
                }

                if (string.IsNullOrEmpty(Model.Remark))
                    Model.Remark = DBS_DefaultTN;

                string str = "upi://pay?pa=" + DBS_PayeeVPA + "&pn=" + DBS_PayeeName + "&tr=" + Model.OrderId + "&tn=" + Model.OrderId + "&am=" + Model.AmountPayable + "&cu=" + DBS_Currency + "&mode=" + DBS_ModeOfTransaction + "&orgid=" + DBS_ORGID;
                Sitecore.Diagnostics.Log.Info("DBSUPITransactionRequestAPIRequestGET generated UPI QR code string:" + str, this);
                string code = EncryptionDecryption.GenerateQr(str);
                string img = "data:image;base64," + code.ToString();
                string outputHTML = "<html>";
                outputHTML += "<head>";
                outputHTML += "<title>QR Code</title>";
                outputHTML += "</head>";
                outputHTML += "<body>";
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/QR Code Msg", "Please scan code to proceed further...") + "</h1></center><div class='row mobile-none'><div class='col-sm-12'>";
                outputHTML += "<img src='" + img + "' style='width:26%;height:auto;margin-left:36%;' /></div></div>";
                outputHTML += "<div class='row desktop-none'><div class='col-sm-12 txt-center'><a target='_blank' class='btn btn-primary mt-2' href='" + str + "'>Click here</a></div></div>";
                outputHTML += "</body>";
                outputHTML += "<script>";
                outputHTML += "var myVar = setInterval('IsSuccessDBS();', 2000);";
                outputHTML += "</script>";
                outputHTML += "</html>";
                return outputHTML;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BENOWTransactionRequestAPIRequestGET :" + ex.Message, this);
                return null;
            }
            //return string.Empty;
        }

        #endregion

        #region DBS Revamp

        public string BENOWTransactionRequestAPIRequestGETRevamp(ViewPayBill Model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside BENOWTransactionRequestAPIRequestGET Revamp", this);
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string BNW_MerchantCode = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_MerchantCode].Value;
                string BNW_Request_URL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_Request_URL].Value;
                string BNW_EncryptedString = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_EncryptedString].Value;
                string BNW_S2S_URL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_S2S_URL].Value;
                string BNW_Response_URL_B2B = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_Response_URL_B2B].Value;
                string BNW_AuthorizationKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_AuthorizationKey].Value;
                string BNW_X_EMAIL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_XEMAIL].Value;
                string BNW_ORGID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_ORGID].Value;
                string BNW_PaymentMethod = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_PaymentMethod].Value;
                string BNW_Remark = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_Remark].Value;
                string BNW_HashKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_HashKey].Value;
                string BNW_PayerVPA = itemInfo.Fields[Templates.PaymentConfigurationRevamp.BenowFields.BNW_PayerVPA].Value;


                string txndate = System.DateTime.Now.ToString("dd-MM-yyyy");
                string amount = string.Empty;
                if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
                {
                    amount = Model.SecurityDeposit;
                }
                else
                {
                    amount = Model.AmountPayable;
                }
                HttpClient client = new HttpClient();
                Sitecore.Diagnostics.Log.Info("request BENOWTransactionRequestAPIRequestGET Revamp", this);
                PaymentRequest request = new PaymentRequest
                {
                    MerchantCode = BNW_MerchantCode,
                    Amount = System.Convert.ToDecimal(amount),
                    RefNumber = Model.OrderId,
                    PaymentMethod = BNW_PaymentMethod,
                    PayerVPA = BNW_PayerVPA,
                    Remarks = BNW_Remark
                };

                string jsonString = JsonConvert.SerializeObject(request, new JsonSerializerSettings { Formatting = Formatting.None, ContractResolver = new CamelCasePropertyNamesContractResolver() });
                Sitecore.Diagnostics.Log.Info("Benow request1: " + jsonString, this);

                byte[][] hashKeys = EncryptionDecryption.GetHashKeys(BNW_HashKey);
                string encryptedString = EncryptionDecryption.Encrypt(jsonString, hashKeys);

                //string decryptedString = Decrypt(encryptedString, hashKeys);
                //Debug.Assert(jsonString == decryptedString);

                BenowRequest requestPayload = new BenowRequest { EncryptedString = encryptedString, JsonString = jsonString };

                string paymentRequest = JsonConvert.SerializeObject(requestPayload, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                StringContent content = new StringContent(paymentRequest, Encoding.UTF8, "application/json");

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("AuthorizationKey", BNW_AuthorizationKey);
                content.Headers.Add("X-EMAIL", BNW_X_EMAIL);
                //content.Headers.Add("PayerVPA", "AF8Y1@yesbank");
                content.Headers.Add("ORGID", BNW_ORGID.ToString());

                Sitecore.Diagnostics.Log.Info("Benow request2: " + content.ToString(), this);
                HttpResponseMessage response = client.PostAsync(BNW_Request_URL.ToString(), content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    string code = EncryptionDecryption.GenerateQr(responseString);
                    string img = "data:image;base64," + code.ToString();
                    string outputHTML = "<html>";
                    outputHTML += "<head>";
                    outputHTML += "<title>QR Code</title>";
                    outputHTML += "</head>";
                    outputHTML += "<body>";
                    outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
                    outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/QR Code Msg", "Please scan code to proceed further...") + "</h1></center><div class='row mobile-none'><div class='col-sm-12'>";
                    outputHTML += "<img src='" + img + "' style='width:26%;height:auto;margin-left:36%;' /></div></div>";
                    outputHTML += "<div class='row desktop-none'><div class='col-sm-12 txt-center'><a target='_blank' class='btn btn-primary mt-2' href='" + responseString + "'>Click here</a></div></div>";
                    outputHTML += "</body>";
                    outputHTML += "<script>";
                    outputHTML += "var myVar = setInterval('IsSuccess();', 2000);";
                    outputHTML += "</script>";
                    outputHTML += "</html>";
                    return outputHTML;
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("Error at BENOWTransactionRequestAPIRequestGET Revamp : Request:" + BNW_Request_URL + " Content:" + content.ToString() + " Response:" + response.IsSuccessStatusCode + response.RequestMessage + response.StatusCode, this);
                    return "There is an issue with UPI, please try again after some time!";
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BENOWTransactionRequestAPIRequestGET Revamp:" + ex.Message, this);
                return null;
            }
            //return string.Empty;
        }
        public string DBSUPITransactionRequestAPIRequestGETRevamp(ViewPayBill Model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside BENOWTransactionRequestAPIRequestGET Revamp", this);
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string DBS_PayeeVPA = itemInfo.Fields[Templates.PaymentConfigurationRevamp.DBSFields.DBS_PayeeVPA].Value;
                string DBS_PayeeName = itemInfo.Fields[Templates.PaymentConfigurationRevamp.DBSFields.DBS_PayeeName].Value;
                string DBS_ORGID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.DBSFields.DBS_ORGID].Value;
                string DBS_DefaultTN = itemInfo.Fields[Templates.PaymentConfigurationRevamp.DBSFields.DBS_DefaultTN].Value;
                string DBS_Currency = itemInfo.Fields[Templates.PaymentConfigurationRevamp.DBSFields.DBS_Currency].Value;
                string DBS_ResponseCallbackURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.DBSFields.DBS_ResponseCallbackURL].Value;
                string DBS_ModeOfTransaction = itemInfo.Fields[Templates.PaymentConfigurationRevamp.DBSFields.DBS_ModeOfTransaction].Value;

                string txndate = System.DateTime.Now.ToString("dd-MM-yyyy");
                string amount = string.Empty;
                if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
                {
                    amount = Model.SecurityDeposit;
                }
                else
                {
                    amount = Model.AmountPayable;
                }

                if (string.IsNullOrEmpty(Model.Remark))
                    Model.Remark = DBS_DefaultTN;

                string str = "upi://pay?pa=" + DBS_PayeeVPA + "&pn=" + DBS_PayeeName + "&tr=" + Model.OrderId + "&tn=" + Model.OrderId + "&am=" + Model.AmountPayable + "&cu=" + DBS_Currency + "&mode=" + DBS_ModeOfTransaction + "&orgid=" + DBS_ORGID;
                Sitecore.Diagnostics.Log.Info("DBSUPITransactionRequestAPIRequestGET Revamp generated UPI QR code string:" + str, this);
                string code = EncryptionDecryption.GenerateQr(str);
                string img = "data:image;base64," + code.ToString();
                string outputHTML = "<html>";
                outputHTML += "<head>";
                outputHTML += "<title>QR Code</title>";
                outputHTML += "</head>";
                outputHTML += "<body>";
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/QR Code Msg", "Please scan code to proceed further...") + "</h1></center><div class='row mobile-none'><div class='col-sm-12'>";
                outputHTML += "<img src='" + img + "' style='width:26%;height:auto;margin-left:36%;' /></div></div>";
                outputHTML += "<div class='row desktop-none'><div class='col-sm-12 txt-center'><a target='_blank' class='btn btn-primary mt-2' href='" + str + "'>Click here</a></div></div>";
                outputHTML += "</body>";
                outputHTML += "<script>";
                outputHTML += "var myVar = setInterval('IsSuccessDBS();', 2000);";
                outputHTML += "</script>";
                outputHTML += "</html>";
                return outputHTML;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BENOWTransactionRequestAPIRequestGET Revamp :" + ex.Message, this);
                return null;
            }
            //return string.Empty;
        }

        #endregion

        #region City

        public bool StorePaymentRequestNewCity(PushNotificationToSSGModel model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequestCity Store new Request called :" + model.TxnRefNo, this);
                string ISU_UPDATE_FLAG = string.Empty, ISU_UPDATE_MESSAGE = string.Empty;
                DateTime dt = DateTime.Now;
                if (!string.IsNullOrEmpty(model.TranAuthDate))
                {
                    dt = System.Convert.ToDateTime(model.TranAuthDate);
                }
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    //model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                    SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                       {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = Enum.GetName(typeof(EnumPayment.GatewayType), 7).ToString().ToUpper(),
                                                        TransactionTime = dt,
                                                        PaymentMode = model.PaymentMode,
                                                        PaymentType = model.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.SettlementAmount),
                                                        TransactionId = model.TxnRefNo
                                                    }
                                                };

                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequestBBPS Store REquest in PIService :" + model.TxnRefNo, this);

                    SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                    if (response != null && response.Count() > 0)
                    {
                        ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                        ISU_UPDATE_MESSAGE = response[0].Message;
                    }
                    else
                    {
                        ISU_UPDATE_FLAG = "N";
                        ISU_UPDATE_MESSAGE = "No Response";
                    }


                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database Order Id : - " + model.OrderNo, this);
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            PaymentHistory objdata = new PaymentHistory();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            objdata.AccountNumber = model.AccountNumber ?? string.Empty;
                            objdata.UserId = model.AccountNumber;
                            objdata.OrderId = model.OrderNo;
                            objdata.Amount = model.SettlementAmount ?? string.Empty;
                            objdata.RequestTime = System.Convert.ToDateTime(dt);
                            objdata.Checksumkey = string.Empty;
                            objdata.CurrencyType = "INR";
                            objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), 10).ToString() ?? string.Empty;
                            objdata.AdvanceAmmount = string.Empty;
                            objdata.Remark = model.ResponseMessage ?? string.Empty;
                            objdata.PaymentType = model.PaymentType ?? string.Empty;
                            objdata.UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/GuestUser", "Guest");
                            //objdata.Email = model.Email ?? string.Empty;
                            //objdata.Mobile = model.Mobile ?? string.Empty;
                            objdata.TransactionId = model.TxnRefNo;
                            objdata.Responsecode = model.RespCode;
                            objdata.Status = model.StatusDesc;
                            objdata.PaymentMode = model.PaymentMode;
                            objdata.ResponseMsg = JsonConvert.SerializeObject(model);
                            objdata.Created_Date = System.DateTime.Now;
                            objdata.CreatedBy = model.AccountNumber;
                            objdata.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                            objdata.ISU_UPDATE_DATETIME = System.DateTime.Now;
                            objdata.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                            dbcontext.PaymentHistories.InsertOnSubmit(objdata);
                            dbcontext.SubmitChanges();
                        }

                        return true;
                    }
                    catch (System.Exception ex)
                    {
                        // Note : Log the message on any failure to sitecore log
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item " + model.AccountNumber + ": " + ex.Message, this);
                        return false;
                    }


                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentRequest :" + ex.Message, this);
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    PaymentHistory objdata = new PaymentHistory();
                    Guid newid = Guid.NewGuid();
                    objdata.Id = newid;
                    objdata.AccountNumber = model.AccountNumber ?? string.Empty;
                    objdata.UserId = model.AccountNumber;
                    objdata.OrderId = model.OrderNo;
                    objdata.Amount = model.SettlementAmount ?? string.Empty;
                    objdata.RequestTime = System.Convert.ToDateTime(DateTime.UtcNow);
                    objdata.Checksumkey = string.Empty;
                    objdata.CurrencyType = "INR";
                    objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), 7).ToString() ?? string.Empty;
                    objdata.AdvanceAmmount = string.Empty;
                    objdata.Remark = model.ResponseMessage ?? string.Empty;
                    objdata.PaymentType = model.PaymentType ?? string.Empty;
                    objdata.UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/GuestUser", "Guest");
                    //objdata.Email = model.Email ?? string.Empty;
                    //objdata.Mobile = model.Mobile ?? string.Empty;
                    objdata.TransactionId = model.TxnRefNo;
                    objdata.Responsecode = model.RespCode;
                    objdata.Status = model.StatusDesc;
                    objdata.PaymentMode = model.PaymentMode;
                    objdata.Created_Date = System.DateTime.Now;
                    objdata.CreatedBy = model.AccountNumber;
                    objdata.msg = "Exception";
                    objdata.ISU_UPDATE_FLAG = "N";
                    objdata.ISU_UPDATE_DATETIME = System.DateTime.Now;
                    objdata.ISU_UPDATE_MESSAGE = ex.Message;
                    dbcontext.PaymentHistories.InsertOnSubmit(objdata);
                    dbcontext.SubmitChanges();
                }
                return false;
            }

        }

        public string CityUPITransactionRequestAPIRequestGET(ViewPayBill Model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside CityUPITransactionRequestAPIRequestGET", this);
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string City_PayeeVPA = itemInfo.Fields[Templates.PaymentConfiguration.CityFields.City_PayeeVPA].Value;
                string City_PayeeName = itemInfo.Fields[Templates.PaymentConfiguration.CityFields.City_PayeeName].Value;
                string City_ORGID = itemInfo.Fields[Templates.PaymentConfiguration.CityFields.City_ORGID].Value;
                string City_DefaultTN = itemInfo.Fields[Templates.PaymentConfiguration.CityFields.City_DefaultTN].Value;
                string City_Currency = itemInfo.Fields[Templates.PaymentConfiguration.CityFields.City_Currency].Value;
                string City_ModeOfTransaction = itemInfo.Fields[Templates.PaymentConfiguration.CityFields.City_ModeOfTransaction].Value;

                string txndate = System.DateTime.Now.ToString("dd-MM-yyyy");
                string amount = string.Empty;
                if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
                {
                    amount = Model.SecurityDeposit;
                }
                else
                {
                    amount = Model.AmountPayable;
                }

                if (string.IsNullOrEmpty(Model.Remark))
                    Model.Remark = City_DefaultTN;

                string str = "upi://pay?pa=" + City_PayeeVPA + "&pn=" + City_PayeeName + "&tr=" + Model.OrderId + "&tn=" + City_DefaultTN + "&am=" + Model.AmountPayable + "&cu=" + City_Currency + "&mode=" + City_ModeOfTransaction + "&orgid=" + City_ORGID;
                Sitecore.Diagnostics.Log.Info("CityUPITransactionRequestAPIRequestGET generated UPI QR code string:" + str, this);
                string code = EncryptionDecryption.GenerateQr(str);
                string img = "data:image;base64," + code.ToString();
                string outputHTML = "<html>";
                outputHTML += "<head>";
                outputHTML += "<title>QR Code</title>";
                outputHTML += "</head>";
                outputHTML += "<body>";
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/QR Code Msg", "Please scan code to proceed further...") + "</h1></center><div class='row mobile-none'><div class='col-sm-12'>";
                outputHTML += "<img src='" + img + "' style='width:26%;height:auto;margin-left:36%;' /></div></div>";
                outputHTML += "<div class='row desktop-none'><div class='col-sm-12 txt-center'><a target='_blank' class='btn btn-primary mt-2' href='" + str + "'>Click here</a></div></div>";
                outputHTML += "</body>";
                outputHTML += "<script>";
                outputHTML += "var myVar = setInterval('IsSuccessCity();', 2000);";
                outputHTML += "</script>";
                outputHTML += "</html>";
                return outputHTML;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at CityUPITransactionRequestAPIRequestGET :" + ex.Message, this);
                return null;
            }
            //return string.Empty;
        }

        #endregion

        #region City Revamp

        public bool StorePaymentRequestNewCityRevamp(PushNotificationToSSGModel model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequestCity Store new Request called :" + model.TxnRefNo, this);
                string ISU_UPDATE_FLAG = string.Empty, ISU_UPDATE_MESSAGE = string.Empty;
                DateTime dt = DateTime.Now;
                if (!string.IsNullOrEmpty(model.TranAuthDate))
                {
                    dt = System.Convert.ToDateTime(model.TranAuthDate);
                }
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    //model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                    SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                       {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = Enum.GetName(typeof(EnumPayment.GatewayType), 7).ToString().ToUpper(),
                                                        TransactionTime = dt,
                                                        PaymentMode = model.PaymentMode,
                                                        PaymentType = model.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.SettlementAmount),
                                                        TransactionId = model.TxnRefNo
                                                    }
                                                };

                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequestBBPS Store REquest in PIService :" + model.TxnRefNo, this);

                    SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                    if (response != null && response.Count() > 0)
                    {
                        ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                        ISU_UPDATE_MESSAGE = response[0].Message;
                    }
                    else
                    {
                        ISU_UPDATE_FLAG = "N";
                        ISU_UPDATE_MESSAGE = "No Response";
                    }


                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database Order Id : - " + model.OrderNo, this);
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            PaymentHistory objdata = new PaymentHistory();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            objdata.AccountNumber = model.AccountNumber ?? string.Empty;
                            objdata.UserId = model.AccountNumber;
                            objdata.OrderId = model.OrderNo;
                            objdata.Amount = model.SettlementAmount ?? string.Empty;
                            objdata.RequestTime = System.Convert.ToDateTime(dt);
                            objdata.Checksumkey = string.Empty;
                            objdata.CurrencyType = "INR";
                            objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), 10).ToString() ?? string.Empty;
                            objdata.AdvanceAmmount = string.Empty;
                            objdata.Remark = model.ResponseMessage ?? string.Empty;
                            objdata.PaymentType = model.PaymentType ?? string.Empty;
                            objdata.UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/GuestUser", "Guest");
                            //objdata.Email = model.Email ?? string.Empty;
                            //objdata.Mobile = model.Mobile ?? string.Empty;
                            objdata.TransactionId = model.TxnRefNo;
                            objdata.Responsecode = model.RespCode;
                            objdata.Status = model.StatusDesc;
                            objdata.PaymentMode = model.PaymentMode;
                            objdata.ResponseMsg = JsonConvert.SerializeObject(model);
                            objdata.Created_Date = System.DateTime.Now;
                            objdata.CreatedBy = model.AccountNumber;
                            objdata.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                            objdata.ISU_UPDATE_DATETIME = System.DateTime.Now;
                            objdata.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                            dbcontext.PaymentHistories.InsertOnSubmit(objdata);
                            dbcontext.SubmitChanges();
                        }

                        return true;
                    }
                    catch (System.Exception ex)
                    {
                        // Note : Log the message on any failure to sitecore log
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item " + model.AccountNumber + ": " + ex.Message, this);
                        return false;
                    }


                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentRequest :" + ex.Message, this);
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    PaymentHistory objdata = new PaymentHistory();
                    Guid newid = Guid.NewGuid();
                    objdata.Id = newid;
                    objdata.AccountNumber = model.AccountNumber ?? string.Empty;
                    objdata.UserId = model.AccountNumber;
                    objdata.OrderId = model.OrderNo;
                    objdata.Amount = model.SettlementAmount ?? string.Empty;
                    objdata.RequestTime = System.Convert.ToDateTime(DateTime.UtcNow);
                    objdata.Checksumkey = string.Empty;
                    objdata.CurrencyType = "INR";
                    objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), 7).ToString() ?? string.Empty;
                    objdata.AdvanceAmmount = string.Empty;
                    objdata.Remark = model.ResponseMessage ?? string.Empty;
                    objdata.PaymentType = model.PaymentType ?? string.Empty;
                    objdata.UserType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/GuestUser", "Guest");
                    //objdata.Email = model.Email ?? string.Empty;
                    //objdata.Mobile = model.Mobile ?? string.Empty;
                    objdata.TransactionId = model.TxnRefNo;
                    objdata.Responsecode = model.RespCode;
                    objdata.Status = model.StatusDesc;
                    objdata.PaymentMode = model.PaymentMode;
                    objdata.Created_Date = System.DateTime.Now;
                    objdata.CreatedBy = model.AccountNumber;
                    objdata.msg = "Exception";
                    objdata.ISU_UPDATE_FLAG = "N";
                    objdata.ISU_UPDATE_DATETIME = System.DateTime.Now;
                    objdata.ISU_UPDATE_MESSAGE = ex.Message;
                    dbcontext.PaymentHistories.InsertOnSubmit(objdata);
                    dbcontext.SubmitChanges();
                }
                return false;
            }

        }

        public string CityUPITransactionRequestAPIRequestGETRevamp(ViewPayBill Model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside CityUPITransactionRequestAPIRequestGETRevamp", this);
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

                string City_PayeeVPA = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CityFields.City_PayeeVPA].Value;
                string City_PayeeName = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CityFields.City_PayeeName].Value;
                string City_ORGID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CityFields.City_ORGID].Value;
                string City_DefaultTN = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CityFields.City_DefaultTN].Value;
                string City_Currency = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CityFields.City_Currency].Value;
                string City_ModeOfTransaction = itemInfo.Fields[Templates.PaymentConfigurationRevamp.CityFields.City_ModeOfTransaction].Value;

                string txndate = System.DateTime.Now.ToString("dd-MM-yyyy");
                string amount = string.Empty;
                if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
                {
                    amount = Model.SecurityDeposit;
                }
                else
                {
                    amount = Model.AmountPayable;
                }

                if (string.IsNullOrEmpty(Model.Remark))
                    Model.Remark = City_DefaultTN;

                string str = "upi://pay?pa=" + City_PayeeVPA + "&pn=" + City_PayeeName + "&tr=" + Model.OrderId + "&tn=" + City_DefaultTN + "&am=" + Model.AmountPayable + "&cu=" + City_Currency + "&mode=" + City_ModeOfTransaction + "&orgid=" + City_ORGID;
                Sitecore.Diagnostics.Log.Info("CityUPITransactionRequestAPIRequestGET PaymentConfigurationRevamp generated UPI QR code string:" + str, this);
                string code = EncryptionDecryption.GenerateQr(str);
                string img = "data:image;base64," + code.ToString();
                string outputHTML = "<html>";
                outputHTML += "<head>";
                outputHTML += "<title>QR Code</title>";
                outputHTML += "</head>";
                outputHTML += "<body>";
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
                outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/QR Code Msg", "Please scan code to proceed further...") + "</h1></center><div class='row mobile-none'><div class='col-sm-12'>";
                outputHTML += "<img src='" + img + "' style='width:26%;height:auto;margin-left:36%;' /></div></div>";
                outputHTML += "<div class='row desktop-none'><div class='col-sm-12 txt-center'><a target='_blank' class='btn btn-primary mt-2' href='" + str + "'>Click here</a></div></div>";
                outputHTML += "</body>";
                outputHTML += "<script>";
                outputHTML += "var myVar = setInterval('IsSuccessCity();', 2000);";
                outputHTML += "</script>";
                outputHTML += "</html>";
                return outputHTML;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at CityUPITransactionRequestAPIRequestGET :" + ex.Message, this);
                return null;
            }
            //return string.Empty;
        }

        #endregion

        #region BeNow

        public string BENOWTransactionRequestAPIRequestGET(ViewPayBill Model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside BENOWTransactionRequestAPIRequestGET", this);
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string BNW_MerchantCode = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_MerchantCode].Value;
                string BNW_Request_URL = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_Request_URL].Value;
                string BNW_EncryptedString = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_EncryptedString].Value;
                string BNW_S2S_URL = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_S2S_URL].Value;
                string BNW_Response_URL_B2B = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_Response_URL_B2B].Value;
                string BNW_AuthorizationKey = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_AuthorizationKey].Value;
                string BNW_X_EMAIL = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_XEMAIL].Value;
                string BNW_ORGID = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_ORGID].Value;
                string BNW_PaymentMethod = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_PaymentMethod].Value;
                string BNW_Remark = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_Remark].Value;
                string BNW_HashKey = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_HashKey].Value;
                string BNW_PayerVPA = itemInfo.Fields[Templates.PaymentConfiguration.BenowFields.BNW_PayerVPA].Value;


                string txndate = System.DateTime.Now.ToString("dd-MM-yyyy");
                string amount = string.Empty;
                if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
                {
                    amount = Model.SecurityDeposit;
                }
                else
                {
                    amount = Model.AmountPayable;
                }
                HttpClient client = new HttpClient();
                Sitecore.Diagnostics.Log.Info("request BENOWTransactionRequestAPIRequestGET", this);
                PaymentRequest request = new PaymentRequest
                {
                    MerchantCode = BNW_MerchantCode,
                    Amount = System.Convert.ToDecimal(amount),
                    RefNumber = Model.OrderId,
                    PaymentMethod = BNW_PaymentMethod,
                    PayerVPA = BNW_PayerVPA,
                    Remarks = BNW_Remark
                };

                string jsonString = JsonConvert.SerializeObject(request, new JsonSerializerSettings { Formatting = Formatting.None, ContractResolver = new CamelCasePropertyNamesContractResolver() });
                Sitecore.Diagnostics.Log.Info("Benow request1: " + jsonString, this);

                byte[][] hashKeys = EncryptionDecryption.GetHashKeys(BNW_HashKey);
                string encryptedString = EncryptionDecryption.Encrypt(jsonString, hashKeys);

                //string decryptedString = Decrypt(encryptedString, hashKeys);
                //Debug.Assert(jsonString == decryptedString);

                BenowRequest requestPayload = new BenowRequest { EncryptedString = encryptedString, JsonString = jsonString };

                string paymentRequest = JsonConvert.SerializeObject(requestPayload, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                StringContent content = new StringContent(paymentRequest, Encoding.UTF8, "application/json");

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("AuthorizationKey", BNW_AuthorizationKey);
                content.Headers.Add("X-EMAIL", BNW_X_EMAIL);
                //content.Headers.Add("PayerVPA", "AF8Y1@yesbank");
                content.Headers.Add("ORGID", BNW_ORGID.ToString());

                Sitecore.Diagnostics.Log.Info("Benow request2: " + content.ToString(), this);
                HttpResponseMessage response = client.PostAsync(BNW_Request_URL.ToString(), content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    string code = EncryptionDecryption.GenerateQr(responseString);
                    string img = "data:image;base64," + code.ToString();
                    string outputHTML = "<html>";
                    outputHTML += "<head>";
                    outputHTML += "<title>QR Code</title>";
                    outputHTML += "</head>";
                    outputHTML += "<body>";
                    outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
                    outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/QR Code Msg", "Please scan code to proceed further...") + "</h1></center><div class='row mobile-none'><div class='col-sm-12'>";
                    outputHTML += "<img src='" + img + "' style='width:26%;height:auto;margin-left:36%;' /></div></div>";
                    outputHTML += "<div class='row desktop-none'><div class='col-sm-12 txt-center'><a target='_blank' class='btn btn-primary mt-2' href='" + responseString + "'>Click here</a></div></div>";
                    outputHTML += "</body>";
                    outputHTML += "<script>";
                    outputHTML += "var myVar = setInterval('IsSuccess();', 2000);";
                    outputHTML += "</script>";
                    outputHTML += "</html>";
                    return outputHTML;
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("Error at BENOWTransactionRequestAPIRequestGET : Request:" + BNW_Request_URL + " Content:" + content.ToString() + " Response:" + response.IsSuccessStatusCode + response.RequestMessage + response.StatusCode, this);
                    return "There is an issue with UPI, please try again after some time!";
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BENOWTransactionRequestAPIRequestGET :" + ex.Message, this);
                return null;
            }
            //return string.Empty;
        }

        public bool StorePaymentRequestDBS(DBSResponse model)
        {
            try
            {
                if (model != null)
                {
                    string ISU_UPDATE_FLAG = string.Empty, ISU_UPDATE_MESSAGE = string.Empty;
                    try
                    {
                        DateTime dt = DateTime.Now;
                        using (new Sitecore.SecurityModel.SecurityDisabler())
                        {

                            SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                               {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/DBS/Gateway Type", "DBS"),
                                                        TransactionTime = dt,
                                                        PaymentMode = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/UPI/Payment Mode", "UPI"),
                                                        PaymentType = model.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.txnInfo.txnAmount),
                                                        TransactionId = model.txnInfo.txnRefId
                                                    }
                                                };

                            Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest DBS Store REquest in PIService :" + model.refNumber, this);

                            SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                            if (response != null && response.Count() > 0)
                            {
                                ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                                ISU_UPDATE_MESSAGE = response[0].Message;
                            }
                            else
                            {
                                ISU_UPDATE_FLAG = "N";
                                ISU_UPDATE_MESSAGE = "No Response";
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item  DBS  StorePaymentRequestBeNow PI service update" + model.txnInfo.refId + ": " + ex.Message, this);
                    }

                    try
                    {
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            PaymentHistory ctx = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == model.txnInfo.customerReference).OrderByDescending(p => p.Created_Date).FirstOrDefault();
                            if (ctx != null)
                            {
                                // Update response in Database
                                ctx.TransactionId = model.txnInfo.txnRefId ?? string.Empty;
                                ctx.Status = Constants.PaymentResponse.Success;
                                ctx.Responsecode = Constants.PaymentResponse.Success.ToString() ?? string.Empty;
                                ctx.Remark = model.txnInfo.refId.ToString() ?? string.Empty + " Amount:" + model.txnInfo.txnAmount + " Payer:" + model.txnInfo.senderParty.name;
                                ctx.ResponseTime = System.Convert.ToDateTime(DateTime.UtcNow);
                                ctx.PaymentRef = model.txnInfo.refId.ToString() ?? string.Empty;
                                ctx.ResponseMsg = model.ResponseMessage ?? string.Empty;
                                ctx.PaymentMode = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/UPI/Payment Mode", "UPI");
                                ctx.Modified_Date = System.DateTime.Now;
                                ctx.msg = Constants.PaymentResponse.Success;
                                ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                ctx.GatewayType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/DBS/Gateway Type", "DBS");
                                dbcontext.SubmitChanges();

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse DBS  Store response in Database :" + model.refNumber, this);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        // Note : Log the message on any failure to sitecore log
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item  DBS  StorePaymentRequestBeNow PaymentHistory update" + model.refNumber + ": " + ex.Message, this);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentResponse StorePaymentRequest DBS:" + ex.Message, this);
                return false;
            }
        }

        public bool StorePaymentRequestBeNow(BeNowResponse model)
        {
            try
            {
                if (model != null)
                {
                    string ISU_UPDATE_FLAG = string.Empty, ISU_UPDATE_MESSAGE = string.Empty;
                    try
                    {
                        if (model.txnStatus == "Successful")
                        {
                            DateTime dt = DateTime.Now;
                            using (new Sitecore.SecurityModel.SecurityDisabler())
                            {

                                SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                                   {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Benow/Gateway Type", "BENOW"),
                                                        TransactionTime = dt,
                                                        PaymentMode =  DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/UPI/Payment Mode", "UPI"),
                                                        PaymentType = model.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.amount),
                                                        TransactionId = model.txnId
                                                    }
                                                };

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest BeNow Store REquest in PIService :" + model.refNumber, this);

                                SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                                if (response != null && response.Count() > 0)
                                {
                                    ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                                    ISU_UPDATE_MESSAGE = response[0].Message;
                                }
                                else
                                {
                                    ISU_UPDATE_FLAG = "N";
                                    ISU_UPDATE_MESSAGE = "No Response";
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item  BeNow  StorePaymentRequestBeNow PI service update" + model.refNumber + ": " + ex.Message, this);
                    }

                    try
                    {
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            PaymentHistory ctx = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == model.refNumber).FirstOrDefault();
                            if (ctx != null)
                            {
                                // Update response in Database
                                ctx.TransactionId = model.txnId ?? string.Empty;
                                ctx.Status = model.txnStatus ?? string.Empty;  //UPIReference
                                ctx.Responsecode = model.txnStatus.ToString() ?? string.Empty;
                                ctx.Remark = model.txnStatus.ToString() ?? string.Empty + " Amount:" + model.amount + " Payer:" + model.payerName;
                                ctx.ResponseTime = System.Convert.ToDateTime(DateTime.UtcNow);
                                ctx.PaymentRef = model.refNumber.ToString() ?? string.Empty;
                                ctx.ResponseMsg = model.ResponseMessage ?? string.Empty;
                                ctx.PaymentMode = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/UPI/Payment Mode", "UPI");
                                //ctx.OrderId = model.refNumber;
                                ctx.Modified_Date = System.DateTime.Now;
                                ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                ctx.GatewayType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Benow/Gateway Type", "BENOW");
                                dbcontext.SubmitChanges();

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse BeNow  Store response in Database :" + model.refNumber, this);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        // Note : Log the message on any failure to sitecore log
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item  BeNow  StorePaymentRequestBeNow PaymentHistory update" + model.refNumber + ": " + ex.Message, this);
                    }

                }
                return true;

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentResponse StorePaymentRequestBeNow:" + ex.Message, this);
                return false;
            }
        }

        public bool StorePaymentRequestCity(PushNotificationToSSGModel model)
        {
            try
            {
                if (model != null)
                {
                    string ISU_UPDATE_FLAG = string.Empty, ISU_UPDATE_MESSAGE = string.Empty;
                    try
                    {
                        if (model.RespCode == "101")
                        {
                            DateTime dt = DateTime.Now;
                            using (new Sitecore.SecurityModel.SecurityDisabler())
                            {
                                SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                                   {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.AccountNumber,
                                                        PaymentGateway = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/City/Gateway Type", "CITIBANK"),
                                                        TransactionTime = dt,
                                                        PaymentMode = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/UPI/Payment Mode", "UPI"),
                                                        PaymentType = model.PaymentType.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.SettlementAmount),
                                                        TransactionId = model.TxnRefNo
                                                    }
                                                };

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest City Store REquest in PIService :" + model.TxnRefNo, this);

                                SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                                if (response != null && response.Count() > 0)
                                {
                                    ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                                    ISU_UPDATE_MESSAGE = response[0].Message;
                                }
                                else
                                {
                                    ISU_UPDATE_FLAG = "N";
                                    ISU_UPDATE_MESSAGE = "No Response";
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item  City  StorePaymentRequestBeNow PI service update" + model.TxnRefNo + ": " + ex.Message, this);
                    }

                    try
                    {
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            PaymentHistory ctx = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == model.OrderNo).FirstOrDefault();
                            if (ctx != null)
                            {
                                // Update response in Database
                                ctx.TransactionId = model.TxnRefNo ?? string.Empty;
                                ctx.Status = model.StatusCode ?? string.Empty;  //UPIReference
                                ctx.Responsecode = model.RespCode.ToString() ?? string.Empty;
                                ctx.Remark = model.StatusDesc;
                                ctx.ResponseTime = System.Convert.ToDateTime(DateTime.UtcNow);
                                ctx.PaymentRef = model.NPCITxnId.ToString() ?? string.Empty;
                                ctx.ResponseMsg = model.ResponseMessage ?? string.Empty;
                                ctx.PaymentMode = model.PaymentMode ?? string.Empty;
                                //ctx.OrderId = model.refNumber;
                                ctx.Modified_Date = System.DateTime.Now;
                                ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                ctx.PaymentMode = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/UPI/Payment Mode", "UPI");
                                ctx.GatewayType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/City/Gateway Type", "CITYBANK");
                                dbcontext.SubmitChanges();

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse city  Store response in Database :" + model.TxnRefNo, this);
                            }
                            else
                            {
                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse city  Record not found in database :" + model.TxnRefNo, this);
                                return false;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        // Note : Log the message on any failure to sitecore log
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item  City  StorePaymentRequestBeNow PaymentHistory update" + model.TxnRefNo + ": " + ex.Message, this);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentResponse StorePaymentRequestBeNow:" + ex.Message, this);
                return false;
            }
        }

        #endregion

        #region AdaniGas

        #region Request / Response Store in DB
        public void StorePaymentRequestAdaniGas(PayOnline model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);

                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequestAdaniGas Store request in Database Order Id : - " + model.OrderId, this);
                        using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                        {
                            PaymentHistoryData objdata = new PaymentHistoryData();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            model.CustomerID = model.CustomerID ?? string.Empty;
                            objdata.AccountNumber = model.CustomerID ?? string.Empty;
                            objdata.UserId = model.CustomerID ?? string.Empty;
                            objdata.OrderId = model.OrderId;
                            objdata.Amount = model.Amount.ToString();
                            objdata.AdvanceAmmount = model.AdvanceAmount.ToString();
                            objdata.RequestTime = System.DateTime.Now;
                            objdata.Checksumkey = string.Empty;
                            objdata.CurrencyType = "INR" ?? string.Empty;
                            objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString() ?? string.Empty;
                            objdata.CustomerType = model.CustomerType ?? string.Empty;
                            objdata.PaymentType = model.PaymentType ?? string.Empty;
                            objdata.UserType = model.UserType ?? string.Empty;
                            objdata.Email = model.Email ?? string.Empty;
                            objdata.Mobile = model.Mobile ?? string.Empty;
                            objdata.Created_Date = System.DateTime.Now;
                            objdata.CreatedBy = model.CustomerName;

                            dbcontext.PaymentHistoryDatas.InsertOnSubmit(objdata);
                            if (objdata.PaymentType == "Name Transfer")
                            {
                                NameTransferRequestDataContext objNameTransferRequestDataContext = new NameTransferRequestDataContext();
                                NameTransferRequestDetail objNameTransferRequest = objNameTransferRequestDataContext.NameTransferRequestDetails.Where(i => i.CustomerId.ToString() == model.CustomerID).OrderByDescending(c => c.CreatedDate).FirstOrDefault();
                                if (objNameTransferRequest != null)
                                {
                                    objNameTransferRequest.OrderId = model.OrderId;
                                }
                                objNameTransferRequestDataContext.SubmitChanges();
                            }
                            dbcontext.SubmitChanges();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item " + model.LoginName + ": " + ex.Message, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentRequestAdaniGas :" + ex.Message, this);
            }
        }

        public void StorePaymentRequestAfterSalesAdaniGas(afterSalesServices model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);

                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequestAdaniGas Store request in Database Order Id : - " + model.OrderId, this);
                        using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                        {
                            PaymentHistoryData objdata = new PaymentHistoryData();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            model.CustomerID = model.CustomerID ?? string.Empty;
                            objdata.AccountNumber = model.CustomerID ?? string.Empty;
                            objdata.UserId = model.CustomerID ?? string.Empty;
                            objdata.OrderId = model.OrderId;
                            objdata.Amount = model.Amount.ToString();
                            objdata.AdvanceAmmount = model.AdvanceAmount.ToString();
                            objdata.RequestTime = System.DateTime.Now;
                            objdata.Checksumkey = string.Empty;
                            objdata.CurrencyType = "INR" ?? string.Empty;
                            objdata.GatewayType = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString() ?? string.Empty;
                            objdata.CustomerType = model.CustomerType ?? string.Empty;
                            objdata.PaymentType = model.PaymentType ?? string.Empty;
                            objdata.UserType = model.UserType ?? string.Empty;
                            objdata.Email = model.Email ?? string.Empty;
                            objdata.Mobile = model.Mobile ?? string.Empty;
                            objdata.Created_Date = System.DateTime.Now;
                            objdata.CreatedBy = model.CustomerName;

                            objdata.IsAfterSalesService = true;
                            objdata.Comp_Cat = model.AfterSalesServiceRecord.Comp_Cat;
                            objdata.Comp_Type = model.AfterSalesServiceRecord.Comp_Type;
                            objdata.MessageText = model.AfterSalesServiceRecord.Message;
                            objdata.SrNo = model.AfterSalesServiceRecord.SrNo;
                            objdata.Quantity_Min = System.Convert.ToInt32(model.AfterSalesServiceRecord.Quantity_Min);

                            objdata.CustId = model.AfterSalesServiceRecord.CustomerId;
                            objdata.CustPass = model.AfterSalesServiceRecord.CustomerPassword;
                            objdata.CustPartnerId = model.AfterSalesServiceRecord.CustomerPartnerNo;

                            dbcontext.PaymentHistoryDatas.InsertOnSubmit(objdata);
                            dbcontext.SubmitChanges();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item " + model.LoginName + ": " + ex.Message, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentRequestAdaniGas :" + ex.Message, this);
            }
        }

        public void StorePaymentResponseAdaniGas(PayOnline model)
        {
            if (model != null)
            {
                try
                {
                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponseAdaniGas Store response in Database and PI Service :" + model.TransactionId, this);


                    string ISU_UPDATE_FLAG = string.Empty, ISU_UPDATE_MESSAGE = string.Empty, ISU_Aftersales_message = string.Empty, ISU_Aftersales_flag = string.Empty;

                    WebAPIAdaniGas webAPIAdaniGas = new WebAPIAdaniGas();
                    using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                    {
                        PaymentHistoryData ctx = dbcontext.PaymentHistoryDatas.Where(x => x.UserId == model.CustomerID && x.OrderId == model.OrderId).FirstOrDefault();

                        if (ctx != null)
                        {
                            try
                            {
                                if (ctx.PaymentType == "Name Transfer")
                                {
                                    //Name transfer request creation
                                    model.PaymentType = ctx.PaymentType;
                                    NameTransferRequestDataContext objNameTransferRequestDataContext = new NameTransferRequestDataContext();
                                    var objNameTransferRequest = objNameTransferRequestDataContext.NameTransferRequestDetails.Where(i => i.CustomerId.ToString() == ctx.UserId && i.OrderId == ctx.OrderId && (i.RequestNumber == null || i.RequestNumber == "")).FirstOrDefault();

                                    if (objNameTransferRequest != null)
                                    {
                                        //var checkrequestnumber = objNameTransferRequestDataContext.NameTransferRequestDetails.Where(i => i.RequestNumber==null || i.RequestNumber=="").ToList();


                                        if (model.IsSuccess == 1)
                                        {
                                            WebAPIAdaniGas objWebAPIAdaniGas = new WebAPIAdaniGas();
                                            var CustomerInformation = objWebAPIAdaniGas.ValidateCustomerById(model.CustomerID);
                                            var outstandingAmount = objWebAPIAdaniGas.GetOutstandingAmount(model.CustomerID);
                                            var Meterinfo = GetMeterReadingDetails(model.CustomerID);

                                            string quantity = "";
                                            string Taskcode = "";
                                            if (objNameTransferRequest.ApplicationType == "Builder_Case")
                                            {
                                                Taskcode = "0016";
                                                quantity = "01";
                                            }
                                            else if (objNameTransferRequest.ApplicationType == "Property_ReSale")
                                            {
                                                Taskcode = "0078";
                                                quantity = "01";
                                            }
                                            else if (objNameTransferRequest.ApplicationType == "Demise")
                                            {
                                                Taskcode = "0080";
                                                quantity = "00";
                                            }
                                            //var r = objWebAPIAdaniGas.NameTransferRequestDataPost(Meterinfo.BusinessPartnerNumber, "R", "04", Taskcode, "Name transfer – " + model.ApplicationType, quantity);

                                            var r = objWebAPIAdaniGas.NameTransferRequestDataPost(Meterinfo.BusinessPartnerNumber, "R", "04", Taskcode, "Name_Transfer – " + objNameTransferRequest.ApplicationType, quantity);

                                            if (r.MessageFlag.ToLower() == "s")
                                            {
                                                //save request number and show in receipt
                                                objNameTransferRequest.RequestNumber = r.RequestNumber;
                                                objNameTransferRequest.SAPResponseComment = r.MessageFlag + ";" + r.Message;
                                                //objNameTransferRequest.IsRequestClosed = true;

                                                objNameTransferRequestDataContext.SubmitChanges();
                                            }
                                            else
                                            {
                                                objNameTransferRequest.SAPResponseComment = r.MessageFlag + ";" + r.Message;
                                                objNameTransferRequest.IsRequestClosed = false;
                                                objNameTransferRequestDataContext.SubmitChanges();
                                                //string messagetobedisplayed = DictionaryPhraseRepository.Current.Get("/NameChange/PopupMessageOnFailedSave", "Your name transfer request has been failed. Please Contact ATGL. ");
                                                Log.Info("ATGL Name transfer failed " + r.CustomerID, "ATGL Name Change ");
                                                Log.Info("ATGL Name transfer failed " + r.Message, "ATGL Name Change ");
                                            }
                                        }
                                        else
                                        {
                                            objNameTransferRequest.RequestNumber = string.Empty;
                                            objNameTransferRequest.Comment = "Error in payment";
                                            objNameTransferRequestDataContext.SubmitChanges();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error at Name Transfer Request Registartion after Payment :" + ex, "Name Transfer Payment");
                            }
                            //SAP API call to post data of payment
                            Sitecore.Diagnostics.Log.Info("Payment Post to SAP API call Start", this);

                            if (ctx.IsAfterSalesService == true && model.ResponseStatus.ToLower().Contains("success"))
                            {
                                if (!string.IsNullOrEmpty(ctx.Comp_Cat))
                                {
                                    Sitecore.Diagnostics.Log.Info("Payment Post to SAP API call for AfterSalesService PI Service", this);

                                    var sendData = webAPIAdaniGas.SalesServiceDataPost(ctx.Comp_Cat, ctx.SrNo, ctx.Comp_Type, ctx.Quantity_Min.GetValueOrDefault(), ctx.MessageText, ctx.CustId, ctx.CustPass, ctx.CustPartnerId);

                                    ISU_Aftersales_message = sendData.Message;
                                    ISU_Aftersales_flag = sendData.MessageFlag;
                                }
                            }

                            if (!model.CustomerID.StartsWith("DODO") && model.ResponseStatus.ToLower().Contains("success")) // For dodo payment
                            {
                                var response = webAPIAdaniGas.PaymentPostToSAP(model);
                                ISU_UPDATE_FLAG = response.MessageFlag;
                                ISU_UPDATE_MESSAGE = response.Message;
                            }

                            Sitecore.Diagnostics.Log.Info("Payment Post to SAP Completed", this);

                            //Update response in DB
                            if (model.ResponseStatus.Contains("TXN_FAILURE"))
                            {
                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponseAdaniGas Store request in Database Update payment Order Id : - " + model.OrderId, this);
                                // Update response in Database
                                ctx.TransactionId = model.TransactionId ?? string.Empty;
                                ctx.Status = model.ResponseStatus ?? string.Empty;
                                ctx.Responsecode = model.Responsecode.ToString() ?? string.Empty;
                                ctx.Remark = model.Remark.ToString() ?? string.Empty;
                                ctx.ResponseTime = System.DateTime.Now;
                                ctx.PaymentRef = model.PaymentRef.ToString() ?? string.Empty;
                                ctx.ResponseMsg = model.Message ?? string.Empty;
                                ctx.PaymentMode = model.PaymentMode ?? string.Empty;
                                ctx.Modified_Date = System.DateTime.Now;
                                ctx.ModifiedBy = model.LoginName;
                                ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                ctx.Checksumkey = model.IsS2S ?? string.Empty;
                                dbcontext.SubmitChanges();

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponseAdaniGas Store response in Database update successfull :" + model.TransactionId, this);
                            }
                            else
                            {

                                //in case of extra geyser connection
                                if (ctx.PaymentType == "Geyser Connection")
                                {
                                    var serviceNumber = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/serviceNumber", "R_0022_03");
                                    string[] words = serviceNumber.Split('_');
                                    var Comp_Cat = words[0];
                                    var SrNo = words[1];
                                    var Comp_Type = words[2];
                                    var Quantity = 1;

                                    var sendData = webAPIAdaniGas.SalesServiceDataPostGC(Comp_Cat, SrNo, Comp_Type, Quantity, "", model.CustomerID);

                                    ISU_Aftersales_message = sendData.Message;
                                    ISU_Aftersales_flag = sendData.MessageFlag;
                                }

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponseAdaniGas Store request in Database Update payment Order Id : - " + model.OrderId, this);
                                // Update response in Database
                                ctx.TransactionId = model.TransactionId ?? string.Empty;
                                ctx.Status = model.ResponseStatus ?? string.Empty;
                                ctx.Responsecode = model.Responsecode.ToString() ?? string.Empty;
                                ctx.Remark = model.Remark.ToString() ?? string.Empty;
                                ctx.ResponseTime = System.Convert.ToDateTime(DateTime.UtcNow);
                                ctx.PaymentRef = model.PaymentRef.ToString() ?? string.Empty;
                                ctx.ResponseMsg = model.Message ?? string.Empty;
                                ctx.PaymentMode = model.PaymentMode ?? string.Empty;
                                ctx.Modified_Date = System.DateTime.Now;
                                ctx.ModifiedBy = model.LoginName;
                                ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                ctx.ISUVDS_Update_Flag = ISU_Aftersales_flag;
                                ctx.ISUVDS_Update_Message = ISU_Aftersales_message;
                                ctx.Checksumkey = model.IsS2S ?? string.Empty;
                                dbcontext.SubmitChanges();

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponseAdaniGas Store response in Database update successfull :" + model.TransactionId, this);
                            }
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Error("Error at StorePaymentResponseAdaniGas, record not found for :" + model.OrderId + ", " + model.CustomerID + "," + model.CustomerName, this);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at StorePaymentResponseAdaniGas :" + ex.Message, this);
                }
            }
        }

        public void UpdateDODOPaymentStatus(PayOnline model)
        {
            if (model != null)
            {
                try
                {
                    Sitecore.Diagnostics.Log.Info("Method - UpdateDODOPaymentStatus store response in Database and PI Service :" + model.TransactionId, this);

                    using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                    {
                        if (model.Responsecode == "0300")
                        {
                            DODOForm ctx1 = dbcontext.DODOForms.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                            if (ctx1 != null)
                            {
                                Sitecore.Diagnostics.Log.Info("Method - UpdateDODOPaymentStatus Store request in Database Update payment Order Id : - " + model.OrderId, this);
                                // Update response in Database
                                ctx1.TransactionId = model.TransactionId ?? string.Empty;
                                ctx1.IsPaymentDone = true;
                                dbcontext.SubmitChanges();
                                Sitecore.Diagnostics.Log.Info("Method - UpdateDODOPaymentStatus Store response in Database update successfull :" + model.TransactionId, this);
                            }
                        }

                    }
                }
                catch (System.Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at StorePaymentResponseAdaniGas :" + ex.Message, this);
                }
            }
        }

        public SelfBillingAdaniGas GetMeterReadingDetails(string CustomerID)
        {

            SelfBillingAdaniGas model = new SelfBillingAdaniGas();
            try
            {
                WebAPIAdaniGas _webApiAdaniGas = new WebAPIAdaniGas();
                var response = _webApiAdaniGas.GetPreviousReadingSelfBilling(CustomerID);

                if (response != null)
                {
                    StringReader stringReader = new StringReader(response.ToString());
                    using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader))
                    {
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                switch (reader.Name.ToString())
                                {
                                    case "d:CustomerId":
                                        model.CustomerID = reader.ReadString();
                                        break;
                                    case "d:NAME":
                                        model.CustomerName = reader.ReadString();
                                        break;
                                    case "d:ADDRESS":
                                        model.CustomerAddress = reader.ReadString();
                                        break;
                                    case "d:pmeter_date":
                                        model.PreviousMeterReadingDateandTime = reader.ReadString();
                                        break;
                                    case "d:pmeter_read":
                                        model.PreviousMeterReading = reader.ReadString();
                                        break;
                                    case "d:FLAG":
                                        model.MsgFlag = reader.ReadString();
                                        break;
                                    case "d:VERTRAG":
                                        model.CONTRACTNO = reader.ReadString();
                                        break;
                                    case "d:mr_doc":
                                        model.MRIDNUMBER = reader.ReadString();
                                        break;
                                    case "d:MATNR":
                                        model.DEV_CAT = reader.ReadString();
                                        break;
                                    case "d:SERGE":
                                        if (!string.IsNullOrEmpty(reader.ReadString()))
                                            model.MeterNumber = reader.ReadString();
                                        break;
                                    case "d:SERNR":
                                        if (string.IsNullOrEmpty(model.MeterNumber))
                                            model.MeterNumber = reader.ReadString();
                                        model.SERNR = model.MeterNumber;
                                        break;
                                    case "d:ISTABLART":
                                        model.ISTABLART = reader.ReadString();
                                        break;
                                    case "d:nx_sch_date":
                                        model.NextMeterReadingDateandTime = reader.ReadString();
                                        break;
                                    case "d:BpNo":
                                        model.BusinessPartnerNumber = reader.ReadString();
                                        break;
                                    case "d:PlantCode":
                                        model.CityCode = reader.ReadString();
                                        break;
                                    case "d:PlantName":
                                        model.CityName = reader.ReadString();
                                        break;
                                }
                            }
                        }
                    }

                    var perviodReading = model.PreviousMeterReading;
                    var decimalPoint = System.Convert.ToDecimal(perviodReading);
                    var readingInDecimal = Math.Round((Double)decimalPoint, 2);
                    model.PreviousMeterReading = readingInDecimal.ToString();
                }
            }
            catch (Exception e)
            {
                Log.Error("Error at GetMeterReadingDetails " + e.Message, this);
            }
            return model;
        }

        public string GetNameTransferRequestNumber(string customerId, string orderId)
        {
            NameTransferRequestDataContext objNameTransferRequestDataContext = new NameTransferRequestDataContext();
            WebAPIAdaniGas api = new WebAPIAdaniGas();
            //var reqnumber=api.NameTransferRequestDataPost

            var objNameTransferRequest = objNameTransferRequestDataContext.NameTransferRequestDetails.Where(i => i.CustomerId.ToString() == customerId && i.OrderId == orderId).FirstOrDefault();
            if (objNameTransferRequest != null)
            {
                return objNameTransferRequest.RequestNumber;
            }
            else
                return null;
        }

        #endregion

        #region Adani Gas Payment Integration

        #region PayUMoney

        public string PayUMoneyTransactionRequestAPIAdaniGasRequestPost(PayOnline Model)
        {
            //Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = webdbObj.GetItem(new Data.ID(Templates.PaymentConfigurationAdaniGas.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayuMoneyFields.PUM_Merchant_Key].Value;
            string saltkKey = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayuMoneyFields.PUM_Merchant_Salt].Value;
            string service_provider = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayuMoneyFields.PUM_Service_Provider].Value;
            string hashSEQ = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayuMoneyFields.PUM_Product_Info].Value;
            //string LoggedInUserId = UserProfileService.GetLoginName();

            string hashvalue = GenerateHashforPayU(Model, hashSEQ, merchantKey, saltkKey);

            Sitecore.Diagnostics.Log.Info("PayUMoneyTransactionRequestAPIAdaniGasRequestPost for Order ID - :" + Model.OrderId + " HasValue - : " + hashvalue, this);

            string amout = string.Empty;
            if (Model.AdvanceAmount > 0)
            {
                amout = Model.AdvanceAmount.ToString();

            }
            else
            {
                amout = Model.Amount.ToString();
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "key", merchantKey },
                { "txnid", Model.OrderId },
                { "amount", System.Convert.ToDecimal(amout.Trim()).ToString("g29") },
                { "productinfo",  Model.CustomerID },
                { "firstname", Model.CustomerID },
                { "service_provider", service_provider},
                { "email", Model.Email },
                { "phone", Model.Mobile },
                { "udf1", Model.CustomerID },
                { "surl", itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayuMoneyFields.PUM_Resp_URL_B2B].Value },
                { "furl", itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayuMoneyFields.PUM_Resp_URL_B2B].Value },
                { "curl", itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayuMoneyFields.PUM_Resp_URL_B2B].Value }, //This parameter is not mandatory. Use this to pass the callback url dynamically.
                { "hash", hashvalue }
            };




            string payUURL = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayuMoneyFields.PUM_Request_URL].Value;

            if (payUURL.Contains("test.payu.in"))
            {
                parameters.Remove("service_provider");
            }


            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1> " + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            //outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHTML += "<form method='post' action='" + payUURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }

        public string GenerateHashforPayU(PayOnline model, string hashseq, string marchentKey, string salt)
        {
            string amount = string.Empty;
            if (model.AdvanceAmount > 0)
            {
                amount = model.AdvanceAmount.ToString();
            }
            else
            {
                amount = model.Amount.ToString();
            }
            string[] hashVarsSeq = hashseq.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = marchentKey;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txnid")
                {
                    hash_string = hash_string + model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "amount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(amount).ToString("g29");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "productinfo")
                {
                    hash_string = hash_string + model.CustomerID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "firstname")
                {
                    hash_string = hash_string + model.CustomerID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "email")
                {
                    hash_string = hash_string + model.Email;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "udf1")
                {
                    hash_string = hash_string + model.CustomerID;
                    hash_string = hash_string + '|';
                }
                else
                {
                    hash_string = hash_string + "";// isset if else
                    hash_string = hash_string + '|';
                }
            }

            hash_string += salt;// appending SALT

            hash1 = Generatehash512(hash_string).ToLower();         //ge
            return hash1;
        }

        #endregion

        #region BillDesk
        public string BillDeskTransactionRequestAPIRequestAdaniGasPost(PayOnline Model)
        {
            var amount = string.Empty;

            if (Model.AdvanceAmount > 0)
            {
                amount = Model.AdvanceAmount.ToString();
            }
            else
            {
                amount = Model.Amount.ToString();
            }
            //Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = webdbObj.GetItem(new Data.ID(Templates.PaymentConfigurationAdaniGas.ID.ToString()));

            string paytmURL = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.BillDeskFields.BDSK_Request_URL].Value;
            string MerchantID = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.BillDeskFields.BDSK_Merchant_ID].Value;
            string SecurityID = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.BillDeskFields.BDSK_SECURITY_ID].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.BillDeskFields.BDSK_Resp_URL_B2B].Value;
            string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.BillDeskFields.BDSK_ChecksumKey].Value;
            string requestMsg = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.BillDeskFields.BDSK_Req_Msg].Value;
            string curreny = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.BillDeskFields.BDSK_CURRENCY_TYPE].Value;

            //Old - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|NA|NA|NA|NA|RU
            //New - MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|AdditionalInfo4|AdditionalInfo5|NA|NA|RU

            string[] hashVarsSeq = requestMsg.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;

            string paymenttype = "Bill Payment";


            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "MerchantID")
                {
                    hash_string = MerchantID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CustomerID")
                {
                    hash_string = hash_string + Model.CustomerID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TxnAmount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(amount).ToString("f2");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CurrencyType")
                {
                    hash_string = hash_string + curreny;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField1")
                {
                    hash_string = hash_string + "R";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "SecurityID")
                {
                    hash_string = hash_string + SecurityID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField2")
                {
                    hash_string = hash_string + "F";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo1")
                {
                    hash_string = hash_string + Model.CustomerID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo2")
                {
                    if (!String.IsNullOrEmpty(Model.Due_Date))
                    {
                        hash_string = hash_string + Model.Due_Date.Replace("-", "");
                        hash_string = hash_string + '|';
                    }
                    else
                    {
                        hash_string = hash_string + "NA";
                        hash_string = hash_string + '|';
                    }

                }
                else if (hash_var == "AdditionalInfo3")
                {
                    hash_string = hash_string + paymenttype;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo4")
                {
                    hash_string = hash_string + Model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo5")
                {
                    if (string.IsNullOrEmpty(Model.Email))
                    {
                        hash_string = hash_string + "-";
                        hash_string = hash_string + '|';
                    }
                    else
                    {
                        hash_string = hash_string + Model.Email;
                        hash_string = hash_string + '|';
                    }
                }
                else if (hash_var == "AdditionalInfo6")
                {
                    hash_string = hash_string + Model.CustomerID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "RU")
                {
                    hash_string = hash_string + callbackURL;
                }
                else
                {
                    hash_string = hash_string + "NA";// isset if else
                    hash_string = hash_string + '|';
                }
            }

            string checksumvalue = GetHMACSHA256(hash_string, ChecksumKey);
            if (!string.IsNullOrEmpty(checksumvalue))
            {
                hash_string = hash_string + "|" + checksumvalue;
            }

            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskTransactionRequestAPIRequestPost Request - " + hash_string, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='msg' value='" + hash_string + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }
        #endregion

        #region Paytm
        public string PaytmTransactionRequestAPIRequestAdaniGasPost(PayOnline Model)
        {
            var amount = string.Empty;
            if (Model.AdvanceAmount > 0)
            {
                amount = Model.AdvanceAmount.ToString();
            }
            else
            {
                amount = Model.Amount.ToString();
            }
            //Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = webdbObj.GetItem(new Data.ID(Templates.PaymentConfigurationAdaniGas.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayTMFields.PTM_Merchant_Key].Value;

            string callbackURL = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayTMFields.PTM_Response_URL_B2B].Value;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "MID", itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayTMFields.PTM_Merchant_ID].Value },
                { "CHANNEL_ID", itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayTMFields.PTM_CHANNEL_ID].Value },
                { "INDUSTRY_TYPE_ID", itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayTMFields.PTM_INDUSTRY_TYPE_ID].Value },
                { "WEBSITE", itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayTMFields.PTM_WEBSITE].Value },
                { "EMAIL", Model.Email??"" },
                { "MOBILE_NO", Model.Mobile },
                { "CUST_ID", Model.CustomerID },
                { "ORDER_ID", Model.OrderId },
                { "TXN_AMOUNT", System.Convert.ToDecimal(amount.ToString().Trim()).ToString("f2") },
                { "CALLBACK_URL", callbackURL }, //This parameter is not mandatory. Use this to pass the callback url dynamically.
                { "MERC_UNQ_REF", Model.CustomerID + "_" + Model.OrderId }
            };

            string checksum = CheckSum.generateCheckSum(merchantKey, parameters);

            Sitecore.Diagnostics.Log.Info("PaytmTransactionRequestAPIRequestAdaniGasPost for Order ID - :" + Model.OrderId + " checksum - : " + checksum, this);

            string paytmURL = string.Format(itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayTMFields.PTM_Request_URL].Value, Model.OrderId); //"https://securegw-stage.paytm.in/theia/processTransaction?orderid=" + OrderId;

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }
        public string PaytmTransactionStatusAPIRequestPostAdaniGas(IDictionary<string, string> TransactionRequestAPIResponse)
        {
            //Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = webdbObj.GetItem(new Data.ID(Templates.PaymentConfigurationAdaniGas.ID.ToString()));

            string merchantKey = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayTMFields.PTM_Merchant_Key].Value;
            string merchantID = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayTMFields.PTM_Merchant_ID].Value;

            string postURL = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.PayTMFields.PTM_TransactionStatus_URL].Value; //"https://securegw-stage.paytm.in/merchant-status/getTxnStatus?JsonData="; 
            string order_id = TransactionRequestAPIResponse["ORDERID"];

            Dictionary<string, string> innerrequest = new Dictionary<string, string>
            {
                { "MID", merchantID },
                { "ORDERID", TransactionRequestAPIResponse["ORDERID"] }
            };
            string first_jason = new JavaScriptSerializer().Serialize(innerrequest);

            string Check = CheckSum.generateCheckSum(merchantKey, innerrequest);
            string correct_check = Check.Replace("+", "%2b");
            innerrequest.Add("CHECKSUMHASH", correct_check);
            string final = new JavaScriptSerializer().Serialize(innerrequest);
            final = final.Replace("\\", "").Replace(":\"{", ":{").Replace("}\",", "},");

            string url = postURL + final;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("ContentType", "application/json");
            request.Method = "POST";

            string responseData = string.Empty;
            using (StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                responseData = responseReader.ReadToEnd();
            }
            return responseData;
        }

        #endregion

        //HDFC PG Added by Ketan
        #region HDFC
        public Int64 GetTID()
        {
            var time = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1));
            return (Int64)(time.TotalMilliseconds + 0.5);
        }
        public string HDFCTransactionRequestAPIRequestAdaniGasPost(PayOnline Model)
        {
            var amount = string.Empty;

            if (Model.AdvanceAmount > 0)
            {
                amount = Model.AdvanceAmount.ToString();
            }
            else
            {
                amount = Model.Amount.ToString();
            }
            CCACrypto ccaCrypto = new CCACrypto();
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationAdaniGas.ID.ToString()));
            string MerchantId = "", AccessCode = "", WorkingKey = "", RequestURL = "", callbackURL_Success = "", callbackURL_Error = "", HDFC_Currency_Type = "", HDFC_Req_Param = "";
            if (System.Convert.ToDecimal(amount.ToString().Trim()) > 10000)
            {
                MerchantId = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFieldsForATGL.HDFC_Merchant_ID].Value;
                AccessCode = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFieldsForATGL.HDFC_Access_Code].Value;
                WorkingKey = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFieldsForATGL.HDFC_Working_Key].Value;
                RequestURL = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFieldsForATGL.HDFC_Request_URL].Value;
                callbackURL_Success = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFieldsForATGL.HDFC_Resp_URL_Success].Value;
                callbackURL_Error = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFieldsForATGL.HDFC_Resp_URL_Error].Value;
                HDFC_Currency_Type = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFieldsForATGL.HDFC_Currency_Type].Value;
                HDFC_Req_Param = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFieldsForATGL.HDFC_Req_Param].Value;
            }
            else
            {
                MerchantId = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Merchant_ID].Value;
                AccessCode = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Access_Code].Value;
                WorkingKey = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Working_Key].Value;
                RequestURL = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Request_URL].Value;
                callbackURL_Success = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Resp_URL_Success].Value;
                callbackURL_Error = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Resp_URL_Error].Value;
                HDFC_Currency_Type = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Currency_Type].Value;
                HDFC_Req_Param = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Req_Param].Value;
            }

            string[] ccaRequest_Param = HDFC_Req_Param.Split('&');
            string cca_string = string.Empty;
            string strEncRequest = string.Empty;
            string TID = GetTID().ToString();
            foreach (string hash_var in ccaRequest_Param)
            {
                if (hash_var == "tid")
                {
                    cca_string = cca_string + hash_var + "=" + TID;
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "merchant_id")
                {
                    cca_string = cca_string + hash_var + "=" + MerchantId;
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "order_id")
                {
                    cca_string = cca_string + hash_var + "=" + Model.OrderId;
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "amount")
                {
                    cca_string = cca_string + hash_var + "=" + System.Convert.ToDecimal(amount.ToString().Trim()).ToString("f2");
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "currency")
                {
                    cca_string = cca_string + hash_var + "=" + HDFC_Currency_Type;
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "redirect_url")
                {
                    cca_string = cca_string + hash_var + "=" + callbackURL_Success;
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "cancel_url")
                {
                    cca_string = cca_string + hash_var + "=" + callbackURL_Error;
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "merchant_param1")
                {
                    cca_string = cca_string + hash_var + "=" + Model.CustomerID;
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "merchant_param2")
                {
                    if (!String.IsNullOrEmpty(Model.Due_Date))
                    {
                        cca_string = cca_string + hash_var + "=" + Model.Due_Date.Replace("-", "");
                        cca_string = cca_string + '&';
                    }
                    else
                    {
                        cca_string = cca_string + hash_var + "= NA";
                        cca_string = cca_string + '&';
                    }

                }
                else if (hash_var == "merchant_param3")
                {
                    cca_string = cca_string + hash_var + "=" + Model.CustomerName;
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "merchant_param4")
                {
                    cca_string = cca_string + hash_var + "=" + Model.PaymentType;
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "merchant_param5")
                {
                    cca_string = cca_string + hash_var + "=" + Model.Email;
                    cca_string = cca_string + '&';
                }
                else if (hash_var == "customer_identifier")
                {
                    cca_string = cca_string + hash_var + "=" + Model.CustomerID;
                    cca_string = cca_string + '&';
                }
                else
                {
                    cca_string = cca_string + hash_var + "= NA";
                    cca_string = cca_string + '&';
                }
            }
            strEncRequest = ccaCrypto.Encrypt(cca_string, WorkingKey);

            //return orderLink.PaymentLink;
            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + RequestURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='encRequest' value='" + strEncRequest + "'>"; // Pass Msg request Parameter
            outputHTML += "<input type='hidden' name='access_code' value='" + AccessCode + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }
        public void HDFCTransactionStatusAPIAdaniGasPost(PayOnline Model)
        {

            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationAdaniGas.ID.ToString()));

            string MerchantId = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Merchant_ID].Value;
            string AccessCode = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Access_Code].Value;
            string WorkingKey = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Working_Key].Value;
            string StatusAPI = "HDFC-Status-API";
            //string RequestURL = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Request_URL].Value;
            //string callbackURL_Success = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Resp_URL_Success].Value;
            //string callbackURL_Error = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Resp_URL_Error].Value;
            //string HDFC_Currency_Type = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Currency_Type].Value;
            //string HDFC_Req_Param = itemInfo.Fields[Templates.PaymentConfigurationAdaniGas.HDFCFields.HDFC_Req_Param].Value;

            //string[] ccaRequest_Param = HDFC_Req_Param.Split('&');
            //string cca_string = string.Empty;
            //string strEncRequest = string.Empty;
            //string TID = GetTID().ToString();

            //HDFC order Status API call;
            try
            {
                //string accessCode = "";//from avenues
                //string workingKey = "";// from avenues

                string orderStatusQueryJson = "{ \"reference_no\":\"" + Model.PaymentRef + "\", \"order_no\":\"" + Model.OrderId + "\" }"; //Ex. { "reference_no":"CCAvenue_Reference_No" , "order_no":"123456"} 
                string encJson = "";

                string queryUrl = "https://logintest.ccavenue.com/apis/servlet/DoWebTrans";

                CCACrypto ccaCrypto = new CCACrypto();
                encJson = ccaCrypto.Encrypt(orderStatusQueryJson, WorkingKey);

                // make query for the status of the order to ccAvenues change the command param as per your need
                string authQueryUrlParam = "enc_request=" + encJson + "&access_code=" + AccessCode + "&command=orderStatusTracker&request_type=JSON&response_type=JSON";

                // Url Connection
                String message = postPaymentRequestToGateway(queryUrl, authQueryUrlParam);
                //Response.Write(message);
                NameValueCollection param = getResponseMap(message);
                String status = "";
                String encResJson = "";
                if (param != null && param.Count == 2)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        if ("status".Equals(param.Keys[i]))
                        {
                            status = param[i];
                            Log.Error("HDFC DEC:" + status, StatusAPI);
                        }
                        if ("enc_response".Equals(param.Keys[i]))
                        {
                            encResJson = param[i];
                            Log.Error("HDFC DEC:" + encResJson, StatusAPI);
                            //Response.Write(encResXML);
                        }
                    }
                    if (!"".Equals(status) && status.Equals("0"))
                    {
                        String ResJson = ccaCrypto.Decrypt(encResJson, WorkingKey);
                        Log.Error("HDFC DEC:" + ResJson, StatusAPI);
                        //Response.Write(ResJson);
                    }
                    else if (!"".Equals(status) && status.Equals("1"))
                    {
                        Sitecore.Diagnostics.Log.Error("failure response from ccAvenues:" + Model.OrderId, this);
                    }

                }

            }
            catch (Exception exp)
            {
                Sitecore.Diagnostics.Log.Error("Error at HDFC Status API call :" + Model.OrderId + " " + exp.Message, this);
                Log.Error("HDFC DEC:" + Model.OrderId + " " + exp.Message, StatusAPI);
                //Response.Write("Exception " + exp);

            }
        }
        public string postPaymentRequestToGateway(String queryUrl, String urlParam)
        {

            String message = "";
            try
            {
                StreamWriter myWriter = null;// it will open a http connection with provided url
                WebRequest objRequest = WebRequest.Create(queryUrl);//send data using objxmlhttp object
                objRequest.Method = "POST";
                //objRequest.ContentLength = TranRequest.Length;
                objRequest.ContentType = "application/x-www-form-urlencoded";//to set content type
                myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(urlParam);//send data
                myWriter.Close();//closed the myWriter object

                // Getting Response
                System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();//receive the responce from objxmlhttp object 
                using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
                {
                    message = sr.ReadToEnd();
                    //Response.Write(message);
                    Log.Error("HDFC ENC:" + message, message);
                }
            }
            catch (Exception exception)
            {
                Console.Write("Exception occured while connection." + exception);
            }
            return message;

        }

        private NameValueCollection getResponseMap(String message)
        {
            NameValueCollection Params = new NameValueCollection();
            if (message != null || !"".Equals(message))
            {
                string[] segments = message.Split('&');
                foreach (string seg in segments)
                {
                    string[] parts = seg.Split('=');
                    if (parts.Length > 0)
                    {
                        string Key = parts[0].Trim();
                        string Value = parts[1].Trim();
                        Params.Add(Key, Value);
                    }
                }
            }
            return Params;
        }
        #endregion

        #endregion Gas Payment

        #region AdaniGas ENACH

        public void StoreENachRequestAdaniGas(AdaniGasENachRegistrationModel model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    model.OrderId = DateTime.Now.ToString("yyyyMMddHHmmssff");
                    //model.OrderId = Guid.NewGuid().ToString();
                    model.PaymentGateway = "Billdesk";

                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StoreENachRequestAdaniGas Store request in Database Order Id : - " + model.OrderId, this);
                        using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                        {
                            ENachRegistrationHistory objdata = new ENachRegistrationHistory();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            objdata.AccountNumber = model.CustomerID ?? string.Empty;
                            objdata.UserId = model.CustomerID ?? string.Empty;
                            objdata.OrderId = model.OrderId;
                            objdata.Amount = model.Amount.ToString();
                            objdata.RequestTime = System.DateTime.Now;
                            objdata.Checksumkey = string.Empty;
                            objdata.CurrencyType = "INR" ?? string.Empty;
                            objdata.GatewayType = model.PaymentGateway ?? string.Empty;
                            objdata.CustomerType = model.CustomerType ?? string.Empty;
                            objdata.UserType = model.UserType ?? string.Empty;
                            //objdata.Email = model.Email ?? string.Empty;
                            //objdata.Mobile = model.Mobile ?? string.Empty;
                            objdata.Created_Date = System.DateTime.Now;
                            objdata.CreatedBy = model.CustomerName;
                            objdata.Status = "Initiated";

                            dbcontext.ENachRegistrationHistories.InsertOnSubmit(objdata);
                            dbcontext.SubmitChanges();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at Updating Item " + model.Name + ": " + ex.Message, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StoreENachRequestAdaniGas :" + ex.Message, this);
            }
        }

        public void StoreENachResponseAdaniGas(AdaniGasENachRegistrationModel model)
        {
            if (model != null)
            {
                try
                {
                    Sitecore.Diagnostics.Log.Info("Method - StoreENachResponseAdaniGas Store response in Database and PI Service :" + model.TransactionId, this);


                    string ISU_UPDATE_FLAG = string.Empty, ISU_UPDATE_MESSAGE = string.Empty;
                    //string ISU_Aftersales_message = string.Empty, ISU_Aftersales_flag = string.Empty;

                    WebAPIAdaniGas webAPIAdaniGas = new WebAPIAdaniGas();
                    using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                    {
                        ENachRegistrationHistory ctx = dbcontext.ENachRegistrationHistories.Where(x => x.UserId == model.CustomerID && x.OrderId == model.OrderId).FirstOrDefault();

                        if (ctx != null)
                        {
                            //SAP API call to post data of payment
                            Sitecore.Diagnostics.Log.Info("ENACH Post to SAP API call Start", this);

                            if (model.ResponseStatus.ToLower().Contains("success"))
                            {
                                Sitecore.Diagnostics.Log.Info("ENACH Post to SAP API call for StoreENachResponseAdaniGas PI Service", this);

                                var sendData = webAPIAdaniGas.EcsPostingService(model);
                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponseAdaniGas Store request in Database Update payment Order Id : - " + model.OrderId, this);
                                Sitecore.Diagnostics.Log.Info("ENACH Post to SAP Completed", this);

                                // Update response in Database
                                ctx.TransactionId = model.TransactionId ?? string.Empty;
                                ctx.Status = model.ResponseStatus ?? string.Empty;
                                ctx.Responsecode = model.Responsecode.ToString() ?? string.Empty;
                                ctx.Remark = model.Remark.ToString() ?? string.Empty;
                                ctx.ResponseTime = System.Convert.ToDateTime(DateTime.UtcNow);
                                ctx.BankRefNo = model.BankRefNo.ToString() ?? string.Empty;
                                ctx.SIAmount = model.SIAmount.ToString() ?? string.Empty;
                                ctx.ResponseMsg = model.Message ?? string.Empty;
                                ctx.AccountHolderName = model.AccountHolderName ?? string.Empty;
                                ctx.AccountType = model.AccountType ?? string.Empty;
                                ctx.AccountNumber = model.AccountNumber ?? string.Empty;
                                ctx.BankName = model.BankName ?? string.Empty;
                                ctx.UMRN = model.UMRN ?? string.Empty;
                                ctx.TxnDate = model.TransactionDate ?? string.Empty;
                                ctx.Modified_Date = System.DateTime.Now;
                                ctx.ModifiedBy = model.CustomerID;
                                ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                ctx.SapECSMsgFlag = model.SapECSMsgFlag ?? string.Empty;
                                ctx.SapECSMsg = model.SapECSMsg ?? string.Empty;
                                ctx.IsSapECSPostError = model.IsSapECSPostError;
                                dbcontext.SubmitChanges();
                            }
                            else
                            {
                                Sitecore.Diagnostics.Log.Info("ENACH Post to SAP API call for StoreENachResponseAdaniGas PI Service", this);

                                var sendData = webAPIAdaniGas.EcsPostingService(model);
                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponseAdaniGas Store request in Database Update payment Order Id : - " + model.OrderId, this);
                                // Update response in Database
                                ctx.TransactionId = model.TransactionId ?? string.Empty;
                                ctx.Status = model.ResponseStatus ?? string.Empty;
                                ctx.Responsecode = model.Responsecode.ToString() ?? string.Empty;
                                ctx.Remark = model.Remark.ToString() ?? string.Empty;
                                ctx.ResponseTime = System.Convert.ToDateTime(DateTime.UtcNow);
                                ctx.BankRefNo = model.BankRefNo.ToString() ?? string.Empty;
                                ctx.SIAmount = model.SIAmount.ToString() ?? string.Empty;
                                ctx.ResponseMsg = model.Message ?? string.Empty;
                                ctx.AccountHolderName = model.AccountHolderName ?? string.Empty;
                                ctx.AccountType = model.AccountType ?? string.Empty;
                                ctx.AccountNumber = model.AccountNumber ?? string.Empty;
                                ctx.BankName = model.BankName ?? string.Empty;
                                ctx.UMRN = model.UMRN ?? string.Empty;
                                ctx.TxnDate = model.TransactionDate ?? string.Empty;
                                ctx.Modified_Date = System.DateTime.Now;
                                ctx.ModifiedBy = model.CustomerID;
                                ctx.ISU_UPDATE_FLAG = ISU_UPDATE_FLAG;
                                ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                ctx.ISU_UPDATE_MESSAGE = ISU_UPDATE_MESSAGE;
                                ctx.SapECSMsgFlag = model.SapECSMsgFlag ?? string.Empty;
                                ctx.SapECSMsg = model.SapECSMsg ?? string.Empty;
                                ctx.IsSapECSPostError = model.IsSapECSPostError;
                                ctx.ErrorStatus = model.ErrorStatus ?? string.Empty;
                                ctx.ErrorDescription = model.ErrorDescription ?? string.Empty;
                                dbcontext.SubmitChanges();

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponseAdaniGas Store response in Database update successfull :" + model.TransactionId, this);
                            }
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Error("Error at StorePaymentResponseAdaniGas, record not found for :" + model.OrderId + ", " + model.CustomerID + "," + model.CustomerName, this);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at StorePaymentResponseAdaniGas :" + ex.Message, this);
                }
            }
        }

        public string BillDeskENACHRequestAPIRequestPost_AdaniGas(AdaniGasENachRegistrationModel Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.AdaniGasENACHRegisterConfiguration.ID.ToString()));

            string RequestURL = itemInfo.Fields[Templates.AdaniGasENACHRegisterConfiguration.BillDeskENach.BDSK_Request_URL].Value;
            string MerchantID = itemInfo.Fields[Templates.AdaniGasENACHRegisterConfiguration.BillDeskENach.BDSK_Merchant_ID].Value;
            string UserID = itemInfo.Fields[Templates.AdaniGasENACHRegisterConfiguration.BillDeskENach.BDSK_USER_ID].Value;
            string callbackURL = itemInfo.Fields[Templates.AdaniGasENACHRegisterConfiguration.BillDeskENach.BDSK_Resp_URL_B2B].Value;
            string ChecksumKey = itemInfo.Fields[Templates.AdaniGasENACHRegisterConfiguration.BillDeskENach.BDSK_ChecksumKey].Value;
            string requestMsg = itemInfo.Fields[Templates.AdaniGasENACHRegisterConfiguration.BillDeskENach.BDSK_Req_Msg].Value;
            string curreny = itemInfo.Fields[Templates.AdaniGasENACHRegisterConfiguration.BillDeskENach.BDSK_CURRENCY_TYPE].Value;
            string BankId = itemInfo.Fields[Templates.AdaniGasENACHRegisterConfiguration.BillDeskENach.BDSK_BankId].Value;
            string ItemCode = itemInfo.Fields[Templates.AdaniGasENACHRegisterConfiguration.BillDeskENach.BDSK_ItemCode].Value;

            //SI Details Start
            Item SIInfo = dbWeb.GetItem(new Data.ID(Templates.AdaniGasENACHBilldeskSIDetails.ID.ToString()));
            string SIRequest_String = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.Request_String].Value;
            string SIaccountnumber = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.accountnumber].Value.Trim();
            string SIaccounttype = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.accounttype].Value.Trim();
            string SIamount = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.amount].Value.Trim();
            string SIamounttype = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.amounttype].Value.Trim();
            string SIstartdate = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.startdate].Value.Trim();
            string SIenddate = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.enddate].Value.Trim();
            string SIfrequency = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.frequency].Value.Trim();
            string SIRef1 = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.Ref1].Value.Trim();
            //string SIRef2_CustomerNo = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.Ref2_CustomerNo].Value;
            string SIcustomername = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.customername].Value.Trim();
            string SImandaterefno = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.mandaterefno].Value.Trim();
            string SIIfscCode = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.IfscCode].Value.Trim();
            string SIMICR = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.MICR].Value.Trim();
            string SIMobileNumber = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.MobileNumber].Value.Trim();
            string SIemailID = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.emailID].Value.Trim();
            string SIModeOfRegistration = SIInfo.Fields[Templates.AdaniGasENACHBilldeskSIDetails.BillDeskENachSIDetail.ModeOfRegistration].Value.Trim();
            string[] SIhashVarsSeq = SIRequest_String.Split(':');
            string SIhash_string = string.Empty;
            foreach (string SIhash_var in SIhashVarsSeq)
            {
                switch (SIhash_var)
                {
                    case "accountnumber":
                        SIhash_string = !string.IsNullOrEmpty(SIaccountnumber) ? SIaccountnumber : "NA";
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "accounttype":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIaccounttype) ? SIaccounttype : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "amount":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIamount) ? SIamount : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "amounttype":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIamounttype) ? SIamounttype : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "startdate":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIstartdate) ? SIstartdate : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "enddate":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIenddate) ? SIenddate : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "frequency":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIfrequency) ? SIfrequency : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "Ref1":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIRef1) ? SIRef1 : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "Ref2":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(Model.CustomerID) ? Model.CustomerID : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "customername":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(Model.CustomerName) ? Model.CustomerName : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "mandaterefno":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SImandaterefno) ? SImandaterefno : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "IfscCode":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIIfscCode) ? SIIfscCode : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "MICR":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIMICR) ? SIMICR : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "MobileNumber":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIMobileNumber) ? SIMobileNumber : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "emailID":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIemailID) ? SIemailID : "NA");
                        SIhash_string = SIhash_string + ':';
                        break;
                    case "ModeOfRegistration":
                        SIhash_string = SIhash_string + (!string.IsNullOrEmpty(SIModeOfRegistration) ? SIModeOfRegistration : "NA");
                        break;
                }

            }
            //SI details End
            if (string.IsNullOrWhiteSpace(BankId) && string.IsNullOrEmpty(BankId))
            {
                BankId = "NA";
            }

            string[] hashVarsSeq = requestMsg.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;
            //string UniqueOrderID = "";
            string amount = string.Empty;
            string paymenttype = string.Empty;

            amount = Model.Amount.ToString();
            paymenttype = "ENACH Registration";

            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "MerchantID")
                {
                    hash_string = MerchantID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "UniqueOrderID")
                {
                    hash_string = hash_string + Model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "SIDetails")
                {
                    hash_string = hash_string + SIhash_string;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TxnAmount")
                {
                    hash_string = hash_string + System.Convert.ToDecimal(amount).ToString("f2");
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "BankId")
                {
                    hash_string = hash_string + BankId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "Filler1")
                {
                    hash_string = hash_string + "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "Filler2")
                {
                    hash_string = hash_string + "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "CurrencyType")
                {
                    hash_string = hash_string + curreny;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "ItemCode")
                {
                    hash_string = hash_string + ItemCode;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField1")
                {
                    hash_string = hash_string + "R";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "UserID")
                {
                    hash_string = hash_string + UserID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "Filler4")
                {
                    hash_string = hash_string + "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "Filler5")
                {
                    hash_string = hash_string + "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "TypeField2")
                {
                    hash_string = hash_string + "F";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo1")
                {
                    hash_string = hash_string + Model.CustomerID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo2")
                {
                    hash_string = hash_string + "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo3")
                {
                    hash_string = hash_string + "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo4")
                {
                    hash_string = hash_string + "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo5")
                {
                    hash_string = hash_string + "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo6")
                {
                    hash_string = hash_string + "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "AdditionalInfo7")
                {
                    hash_string = hash_string + "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "RU")
                {
                    hash_string = hash_string + callbackURL;
                    // hash_string = hash_string + '|';
                }
                else
                {
                    hash_string = hash_string + "NA";// isset if else
                    hash_string = hash_string + '|';
                }
            }

            string checksumvalue = GetHMACSHA256(hash_string, ChecksumKey);
            if (!string.IsNullOrEmpty(checksumvalue))
            {
                hash_string = hash_string + "|" + checksumvalue;
            }

            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskTransactionRequestAPIRequestPost Request - " + hash_string, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + RequestURL + hash_string + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            //outputHTML += "<input type='hidden' name='msg' value='" + hash_string + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }

        #endregion AdaniGas ENACH
        #endregion AdaniGas

        #region SafeXPay
        public string SafeXPayTransactionRequestAPIRequestPost(ViewPayBill Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfigurationRevamp.ID.ToString()));

            string paytmURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_Request_URL].Value;
            string MerchantID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_Merchant_ID].Value;
            string AggregatorID = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_Aggregator_ID].Value;
            string MerchantEncryptionKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_Merchant_Encryption_Key].Value;
            string callbackURL = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_Resp_URL_B2B].Value;
            // string ChecksumKey = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_ChecksumKey].Value;
            string requestMsg = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_Req_Msg].Value;
            // string curreny = itemInfo.Fields[Templates.PaymentConfigurationRevamp.SafeXPayFields.SafeXPay_CURRENCY_TYPE].Value;


            //New - ag_id|me_id|me_Key|order_no|Amount|Country|Currency|txn_type|success_url|failure_url|Channel|pg_id|Paymode|Scheme|emi_months|card_no|exp_month|exp_year|cvv|card_name|cust_name|email_id|mobile_no|unique_id|is_logged_in|
            //bill_address|bill_city|bill_state|bill country|

            string[] hashVarsSeq = requestMsg.Split('|');
            // string hash_string = string.Empty;
            string txn_details = string.Empty;

            string amount = string.Empty;
            string paymenttype = string.Empty;
            if (Model.PaymentType == DictionaryPhraseRepository.Current.Get("/Accounts/Payment Type/Security_Deposit", "Security Deposit"))
            {
                if (Model.SecurityDepositAmountType == "Actual")
                    amount = Model.SecurityDeposit;
                else
                    amount = Model.SecurityDeposit;
                paymenttype = "SECURITY DEPOSIT";
            }
            else
            {
                amount = Model.AmountPayable;
                paymenttype = "BILL PAYMENT";
            }

            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "ag_id")
                {
                    txn_details = AggregatorID;
                    txn_details = txn_details + '|';
                }
                else if (hash_var == "me_id")
                {
                    txn_details = txn_details + MerchantID;
                    txn_details = txn_details + '|';
                }
                //else if (hash_var == "me_Key")
                //{
                //    txn_details = txn_details + MerchantEncryptionKey;
                //    txn_details = txn_details + '|';
                //}
                else if (hash_var == "order_no")
                {
                    txn_details = txn_details + Model.OrderId;
                    txn_details = txn_details + '|';
                }
                else if (hash_var == "Amount")
                {
                    txn_details = txn_details + System.Convert.ToDecimal(amount).ToString("f2");
                    txn_details = txn_details + '|';
                }
                else if (hash_var == "Country")
                {
                    txn_details = txn_details + "IND";
                    txn_details = txn_details + '|';
                }
                else if (hash_var == "Currency")
                {
                    txn_details = txn_details + "INR";
                    txn_details = txn_details + '|';
                }
                else if (hash_var == "txn_type")
                {
                    txn_details = txn_details + "SALE";
                    txn_details = txn_details + '|';
                }
                else if (hash_var == "success_url")
                {
                    txn_details = txn_details + callbackURL;
                    txn_details = txn_details + '|';
                }
                else if (hash_var == "failure_url")
                {

                    txn_details = txn_details + callbackURL;
                    txn_details = txn_details + '|';
                }
                else if (hash_var == "Channel")
                {
                    txn_details = txn_details + "WEB";

                }

            }
            string pg_details = "|" + "|" + "|";
            string card_details = "|" + "|" + "|" + "|";
            string custisLoggedIn = (!String.IsNullOrEmpty(Model.LoginName)) ? "Y" : string.Empty;
            // string cust_details =  "|" +  "|" +  "|" + "|"+"Y" ;
            string cust_details = "|" + Model.Email + "|" + Model.Mobile + "|" + "|" + custisLoggedIn;
            string bill_details = "|" + "|" + "|" + "|";
            string ship_details = "|" + "|" + "|" + "|" + "|" + "|";
            string item_details = "|" + "|";
            string upi_details = string.Empty;
            string other_details = ((!String.IsNullOrEmpty(Model.AccountNumber)) ? Model.AccountNumber : string.Empty) + "|" + paymenttype + "|" + "|" + "|";
            // string other_details =  "|" +  "|" + "|" + "|";

            string request = txn_details + "~" + pg_details + "~" + card_details + "~" + cust_details + "~" + bill_details + "~" + ship_details + "~" + item_details + "~" + upi_details + "~" + other_details;

            string Hash = MerchantID + "~" + Model.OrderId + "~" + System.Convert.ToDecimal(amount).ToString("f2") + "~" + "IND" + "~" + "INR";

            //hash_string="BSESTST | 100517857 | NA | 900.00 | NA | NA | NA | INR | NA | R | bsestst | NA | NA | F | 21 | 20170705 | BILL PAYMENT | T8GE02I3BWEC3PK | mahajan23@gmail.com | 100517857 | NA| http://electricity.dev.local/PaymentGateway/BillDesk_Callback";

            AesSafeXpay objAes = new AesSafeXpay();
            string enc_request = objAes.encrypt(request, MerchantEncryptionKey);
            string hashing = objAes.ComputeSha256Hash(Hash);
            string enc_hash = objAes.encrypt(hashing, MerchantEncryptionKey);



            Sitecore.Diagnostics.Log.Info("Payment Gateway- SafeXPayTransactionRequestAPIRequestPost Request - " + request, this);

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h2>" + DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Redirect Msg", "Please do not refresh this page...") + "</h2></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' id='f1' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='me_id' value='" + MerchantID + "'>"; // Pass Msg request Parameter            
            outputHTML += "<input type='hidden' name='merchant_request' value='" + enc_request + "'>"; // Pass Msg request Parameter
            outputHTML += "<input type='hidden' name='hash' value='" + enc_hash + "'>"; // Pass Msg request Parameter
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            return outputHTML;
        }


        #endregion

        #region CashFree
        public bool StorePaymentRequestCashFree(Root model, string msg)
        {
            bool isSAPUpdated = true;
            try
            {
                if (model != null)
                {
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                        {
                            Accounts.PaymentHistory ctx = dbcontext.PaymentHistories.Where(x => x.OrderId.ToString() == model.data.order.order_id).FirstOrDefault();

                            if (ctx != null)
                            {
                                if (model.data.payment.payment_status == "SUCCESS")
                                {
                                    if (!string.IsNullOrEmpty(ctx.ISU_UPDATE_FLAG) && ctx.ISU_UPDATE_FLAG == "Y")
                                    {
                                        ctx.ISU_UPDATE_MESSAGE = "Record already updated in SAP";
                                    }
                                    else
                                    {
                                        // UPDATE SAP SERVICE
                                        ctx.ISU_UPDATE_DATETIME = System.DateTime.Now;
                                        try
                                        {
                                            string paymentModeTobepassed = model.data.payment.payment_group;

                                            switch (paymentModeTobepassed)
                                            {
                                                case "net_banking":
                                                    paymentModeTobepassed = "NETBANKING"; break;
                                                case "credit_card":
                                                    paymentModeTobepassed = "CREDITCARD"; break;
                                                case "debit_card":
                                                    paymentModeTobepassed = "DEBITCARD"; break;
                                                case "wallet":
                                                    paymentModeTobepassed = "WALLETS"; break;
                                                case "upi":
                                                    paymentModeTobepassed = "UPI"; break;
                                                default:
                                                    paymentModeTobepassed = "WALLETS"; break;

                                            }

                                            SapPiService.Helper.BillPaymentItem[] paymentItems = new[]
                                            {
                                                    new SapPiService.Helper.BillPaymentItem
                                                    {
                                                        AccountNumber = model.data.customer_details.customer_id,
                                                        PaymentGateway = (DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/CashFree/Gateway Type", "CashFree")).ToUpper(),
                                                        TransactionTime = model.data.payment.payment_time,
                                                        PaymentMode = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/UPI/Payment Mode", paymentModeTobepassed),
                                                        PaymentType = model.data.order.order_tags.payment_types.ToUpper(),
                                                        PaymentAmount = System.Convert.ToDecimal(model.data.payment.payment_amount),
                                                        TransactionId = model.data.payment.cf_payment_id.ToString()
                                                    }
                                                };

                                            Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest CashFree Store REquest in PIService :" + model.data.order.order_id + " at " + model.data.payment.payment_time.ToString("dd MM yyyy HH mm ss"), this);

                                            SapPiService.Helper.BillPaymentItem[] response = SapPiService.Services.RequestHandler.UpdatePaymentInformation(paymentItems);
                                            if (response != null && response.Count() > 0)
                                            {
                                                ctx.ISU_UPDATE_FLAG = response[0].IsSuccess == true ? "Y" : "N";
                                                ctx.ISU_UPDATE_MESSAGE = response[0].Message;
                                            }
                                            else
                                            {
                                                ctx.ISU_UPDATE_FLAG = "N";
                                                ctx.ISU_UPDATE_MESSAGE = "No Response";
                                            }

                                            isSAPUpdated = ctx.ISU_UPDATE_FLAG == "Y";
                                        }
                                        catch (Exception ex)
                                        {
                                            Sitecore.Diagnostics.Log.Error("Error at Updating Item  CashFree  StorePaymentRequestCashFree PIService update " + model.data.order.order_id, ex, this);
                                        }
                                    }
                                }

                                //UPDATE DATABASE
                                try
                                {
                                    ctx.TransactionId = model.data.payment.cf_payment_id.ToString();
                                    ctx.Responsecode = model.data.payment.payment_status ?? string.Empty;
                                    ctx.Status = model.data.payment.payment_status ?? string.Empty;
                                    ctx.Remark = model.data.payment.payment_message ?? string.Empty;
                                    ctx.ResponseTime = System.Convert.ToDateTime(DateTime.UtcNow);
                                    ctx.PaymentRef = model.data.payment.cf_payment_id.ToString();
                                    ctx.ResponseMsg = msg;
                                    ctx.PaymentMode = model.data.payment.payment_group.ToString();
                                    ctx.Modified_Date = System.DateTime.Now;
                                    ctx.GatewayType = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/CashFree/Gateway Type", "CashFree");

                                    dbcontext.SubmitChanges();

                                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse CashFree  Store response in Database :" + model.data.order.order_id, this);
                                }
                                catch (Exception ex)
                                {
                                    Sitecore.Diagnostics.Log.Error("Error at Updating DB Item  StorePaymentRequestCashFree PaymentHistory update " + model.data.order.order_id, ex, this);
                                    return false;
                                }
                            }
                            else
                            {
                                //record not found
                                Sitecore.Diagnostics.Log.Error("Method - StorePaymentResponse CashFree  Record not found in database : " + model.data.order.order_id, this);
                                return false;
                            }
                        }
                    }
                }
                return isSAPUpdated;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentResponse StorePaymentRequestCashFree:" + ex.Message, this);
                return false;
            }
        }
        #endregion
    }
}


