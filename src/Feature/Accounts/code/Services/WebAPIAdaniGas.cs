using DotNetIntegrationKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using paytm;
using RestSharp;
using SapPiService.Domain;
using Sitecore.ApplicationCenter.Applications;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using static Sitecore.Feature.Accounts.Services.NameTransferService;

namespace Sitecore.Feature.Accounts.Services
{
    public class WebAPIAdaniGas
    {
        private string _customerID;
        private HttpClient client;
        private HttpClient client2;
        public WebAPIAdaniGas()
        {
            _customerID = SessionHelper.UserSession.AdaniGasUserSessionContext == null ? string.Empty : SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
            client = new HttpClient();
        }

        public static string GetIPAddress()
        {
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }

                return context.Request.ServerVariables["REMOTE_ADDR"];
                // return System.Net.Dns.GetHostEntry
                //(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string EncryptionKey = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/EncryptionKey", "Tl;jld@456763909QPwOeiRuTy873XY7");
        public string EncryptionIV = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/EncryptionIV", "CEIVRAJWquG8iiMw");
        public string EnachSendOTPEncryptionKey = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/EnachSendOTPEncryptionKey", "lDZUZ5nkY33NEPm2tih1SDGYgxfEz3sY");
        public string EnachSendOTPEncryptionIV = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/EnachSendOTPEncryptionIV", "0000000000000000");
        public string UserName = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/UserName", "UMC_SRV_USR");
        public string Password = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/Password", "init@123");
        public string eNACHSAPUserName = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/eNACHSAPUserName", "UMC_SRV_USR1");
        public string eNACHSAPPassword = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/eNACHSAPPassword", "Adani@123");
        public string ServiceURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ServiceURL", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMCF001_SRV/");
        public string ServiceURL_utilities = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ServiceURL_utilities", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ERP_UTILITIES_UMC/");
        public string ServiceURL_2 = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ServiceURL_2", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMCF002_SRV/");
        public string ServiceURL_4 = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ServiceURL_4", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMCF004_SRV/");
        public string ServiceURL_5 = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ServiceURL_5", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/");
        public string ServiceURL_6 = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ServiceURL_6", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMCF001_SRV/");
        public string ServiceURL_7 = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ServiceURL_7", "https://www.adaniportal.com:8081/sap/opu/odata/sap");
        public string ServiceURL_8 = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ServiceURL_8", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL005_SRV/");
        public string ServiceURL_9 = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ServiceURL_9", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL002_SRV/");


        //AdaniGas ENACH by Ketan Start
        public string ENachSendOtpSet = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/EnachSendOtpSet", "SendOtpSet");
        public string ECSPosting = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ECSPosting", "EcsPostingSet(CustomerNo='{0}',ReferenceNo='{1}',ResponseStatus='{2}')?$filter=BankAcctNo eq '{3}' and BankName eq '{4}' and AcctHolderName eq '{5}' and BankRefNo eq '{6}'");
        public string ENACHValidateOtpSet = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ENACHValidateOtpSet", "ValidateOtpSet(MobileNo='{0}',ValidOtp='{1}')");
        public string ReadENACHPostingSet = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ReadEcsPostingSet", "ReadEcsPostingSet(CustomerNo='{0}')");
        //AdaniGas ENACH by Ketan End

        public string GetEmailListURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetEmailListURL", "ReadEmailSet?$filter=Cust_No%20eq%20'{0}'");
        public string RegisterEmailURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/RegisterEmailURL", "AddEmailSet(Cust_No='{0}',Email_ID='{1}')");
        public string ModifyEmailURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ModifyEmailURL", "UpdateEmailSet(Cust_No='{0}',Old_Email_ID='{1}',New_Email_ID='{2}',Update_Flag='{3}')");
        public string DeleteEmailURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/DeleteEmailURL", "DeleteEmailSet(Cust_No='{0}',Email_ID='{1}',Delete_Flag='{2}')");

        public string GetMobileNumbersListURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetMobileNumbersListURL", "ReadMobileSet?$filter=Cust_No%20eq%20'{0}'");
        public string RegisterMobileNumberURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/RegisterMobileNumberURL", "AddMobileSet(Cust_No='{0}',Mobile_No='{1}')");
        public string ModifyMobileNumberURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ModifyMobileNumberURL", "UpdateMobileSet(Cust_No='{0}',Old_Mobile_No='{1}',New_Mobile_No='{2}',Update_Flag='{3}')");
        public string DeleteMobileNumberURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/DeleteMobileNumberURL", "DeleteMobileSet(Cust_No='{0}',Mobile_No='{1}',Delete_Flag='{2}')");
        public string LoginURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/LoginURL", "LoginSet?$filter=Cust_No%20eq%20'{0}'");
        public string QuickBillSetURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/QuickBillSetURL", "Quick_BillSet(Cust_No='{0}')");
        public string BillingInvoicesURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/BillingInvoicesURL", "ContractAccounts('{0}')/Invoices");
        public string RegisterSMSAlertURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/RegisterSMSAlert", "Update_SMS_checkboxSet(Cust_No='{0}',Mobile_No='{1}',SMS_Enabled='{2}')");

        public string GetCityListURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetCityListURL", "GetRegStrAreasSet");
        public string GetPriceByCityURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetPriceByCityURL", "PriceListSet?$filter=City%20eq%20'{0}'");
        public string GetGasPriceURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetGasPriceURL", "PriceListSet");
        public string PaymentPosting = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/PaymentPosting", "PaymentPostingNewSet(CustomerNo='{0}',Currency='INR',ClearingAcct='{1}',CompanyCode='{2}',BusArea='{3}',PaymentAmount='{4}',PymtMethodOrChqNr='{5}',UsageText='{6}',AdditionalInfo='{7}',Source='{8}')");

        public string GetComplaintQueryURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetComplaintQueryURL", "CQR_TypesSet?$filter=Category eq '{0}' and Cust_No eq '{1}'");
        public string GetComplaintQueryListURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetComplaintQueryListURL", "Search_CQRSet?$filter= Cust_No eq '{0}' and Category eq '{1}'");
        public string RegisterComplaintQuery = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetComplaintQueryListURL", "Create_CompQuerySet(Bpno='{0}',Category='{1}',Comptype='{2}',Taskcode='{3}',Text='{4}')");

        public string _RegisterAccountQuery = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/RegisterAccountURL", "Create_AccountSet");


        public string GetEBillReadInvoice = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetEBillReadInvoice", "ReadEInvoiceSet(Cust_No='{0}')");
        public string GetEBillUpdateInvoice = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetEBillUpdateInvoice", "UpdateEInvoiceSet(Cust_No='{0}',EInvoice_Type='{1}')");
        public string _PostFeedbackQuery = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/PostFeedbackURL", "Post_FeedbackSet");
        public string GetRegStrGroupsSetURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetRegStrGroupsSetURL", "GetRegStrGroupsSet?$filter=Reg_Str_Area eq '{0}'");
        public string RSA_MappingSet = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/RSA_MappingSet", "RSA_MappingSet?$filter=BPKind eq '{0}'&$format=json");
        public string ConnObjSet = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ConnObjSet", "ConnObjSet?$format=json&&$filter=RegioGroup eq '{0}' and Plant eq '{1}' and Blocked eq 'X'");
        public string ApplyForInquiryURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetComplaintQueryListURL", "Create_CompQuerySet?$filter=Category eq '{0}' and Comptype eq '{1}' and Taskcode eq '{2}' and Priority eq '{3}' and Text eq '{4}' and Plant eq '{5}' and Partner_Type eq '{6}' and Name eq '{7}' and House_No eq '{8}' and Street eq '{9}' and Street2 eq '{10}' and Street3 eq '{11}' and Street4 eq '{12}' and Street5 eq '{13}' and Postal_Code eq '{14}' and City eq '{15}' and Reg_Str_Grp eq '{16}' and Country eq '{17}' and Email eq '{18}' and Mobile eq '{19}'&$select=MsgFlag,Message,Complaint");


        public string GetPaymentInformationForDashboard = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetPaymentInformationForDashboard", "Accounts('{0}')/PaymentDocuments");

        public string GetAfterSalesService = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetAfterSalesService", "CompTypeSet?$filter=Comp_Cat eq 'R'");
        public string GetAfterSalesServiceDropDownChange = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetAfterSalesDropChange", "SimulateSalesOrderSet(Comptype='{0}',CompCat='{1}',Partner='{2}',Quantity={3},Taskcode='{4}')");

        public string GetAreaCenterCentersCityURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetAreaCenterCentersCityURL", "Near_LocationSet");

        public string GetAreabyCenterAndCityURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetAreabyCenterAndCityURL", "Near_LocationSet?$filter=LocType eq '{0}' and City eq '{1}'");
        public string GetPreviousMeterReadingURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetPreviousMeterReadingURL", "Accounts('{0}')/ContractAccounts?$expand=Contracts,Contracts/ContractConsumptionValues,Contracts/Division,Contracts/Devices/MeterReadingResults,Contracts");

        public string GetPreviousMeterReadingForSelfBillingURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetPreviousMeterReadingForSelfBillingURL", "/ZMCF_SELF_BILLING_SRV/SelfBillingSet('{0}')");

        //https://www.adaniportal.com:8080/sap/opu/odata/sap/ZMCF_SELF_BILLING_SRV/SelfBillingSet('1000082138')
        public string _SubmitMeaterReadinURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/_SubmitMeaterReadin", "Devices('{0}')/MeterReadingResults");
        public string PrintBillURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/PrintBillURL", "Invoices('{0}')/InvoicePDF/$value");

        public string ChangePasswordSet = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/Change_PasswordSet", "Change_PasswordSet");

        public string ForgotPasswordSendOTPCall = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ForgotPassSendOTP", "Send_OTPSet(Cust_No='{0}',OtpPurpose='{1}')");
        public string ForgotPasswordPostOTP = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ForgotPasswordSet", "ForgotPasswordSet");

        public string PostSalesServiceData = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/PostSalesServiceData", "Create_ServReqSet(Bpno='{0}',Category='{1}',Comptype='{2}',Taskcode='{3}',Text='{4}',Quantity='{5}')");
        public string GasConsumptionDetailURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GasConsumptionDetailURL", "Contracts('{0}')/ContractConsumptionValues?$filter=EndDate ge datetime'{1}' and EndDate le datetime'{2}'");

        public string GetPNGNetwork = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetPNGNetwork", "GetConnObjsSet/?$filter=Reg_Str_Grp eq '{0}' and Partner_Type eq '{1}'");

        public string GetCustomerDetails = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetPNGNetwork", "GetLinkageSet?$filter=UserID eq '{0}'");
        //https://www.adaniportal.com:8081/sap/opu/odata/sap/zmcf001_srv/GetLinkageSet?$filter=UserID eq 'U01000082139'

        public string FetchQuoteURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/FetchQuoteURL", "SimulateSalesOrderSet(Comptype='{0}',CompCat='{1}',Partner='{2}',Quantity=1,Taskcode='{3}')");

        public string PostNameChangedComplaintCloseSet = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ComplaintCloseSet", "ComplaintCloseSet");

        public string PostCommentComplaintTextUpdateSetUpdate = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ComplaintTextUpdateSet", "ComplaintTextUpdateSet");

        public string NameChangeUpdateNameSet = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/UpdateNameSet", "UpdateNameSet?$filter=(CustNo eq '{0}' and Name_First eq '{1}' and Name_Middle eq '{2}' and Name_Last eq '{3}')");

        public string NameChangeDocumentUploadSet = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/DocumentUploadSet", "DocumentUploadSet");

        public string OutstandingAmountURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetOutstandingSet", "GetOutstandingSet('{0}')");

        public string GetDetailsByMeterNumSetURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetDetailsByMeterNumSetURL", "GetCABPDetailsSet('{0}')");

        public string housegetsuburl = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/housegetsuburl", "GetHouseNoSet?$filter=SocietyNo eq '{0}'");


        public UserLoginInfo Login(string username, string password)
        {
            UserLoginInfo model = new UserLoginInfo();
            try
            {
                Sitecore.Diagnostics.Log.Info("WebAPIAdaniGas Login API Call Start, API Name: " + LoginURL, typeof(UserLoginInfo));
                var authenticationBytes = Encoding.ASCII.GetBytes("U0" + username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(LoginURL, username);

                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                if (!response.IsSuccessStatusCode)
                {
                    model.MessageFlag = "F";
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {

                        model.Message = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/User Not Valid", "Username or Password is not valid.");
                    }
                    else
                    {

                        model.Message = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/PI Service Failed", "Unable to fetch data from PI service.");
                        Sitecore.Diagnostics.Log.Error("WebAPIAdaniGas Login API Error, ErrorMessage: " + response.StatusCode + "Content - " + response.Content, typeof(Exception));
                    }
                }
                else
                {
                    var resultContent = response.Content.ReadAsStreamAsync().Result;
                    XDocument incomingXml = XDocument.Load(resultContent);
                    StringReader stringReader = new StringReader(incomingXml.ToString());
                    Sitecore.Diagnostics.Log.Info("WebAPIAdaniGas Login API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                    using (XmlReader reader = XmlReader.Create(stringReader))
                    {
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                switch (reader.Name.ToString())
                                {
                                    case "d:Cust_No":
                                        model.CustomerID = reader.ReadString();
                                        break;
                                    case "d:Name":
                                        model.Name = reader.ReadString();
                                        break;
                                    case "d:Cust_Type":
                                        model.CustomerType = reader.ReadString();
                                        break;
                                    case "d:Contract_No":
                                        model.Contract_No = reader.ReadString();
                                        break;
                                    case "d:Partner":
                                        model.Partner = reader.ReadString();
                                        break;
                                    case "d:Inst_No":
                                        model.Inst_No = reader.ReadString();
                                        break;
                                    case "d:Meter_Fromdt":
                                        model.Meter_Fromdt = reader.ReadString();
                                        break;
                                    case "d:Meter_Uptodt":
                                        model.Meter_Uptodt = reader.ReadString();
                                        break;
                                    case "d:Meter_SerialNumber":
                                        model.Meter_SerialNumber = reader.ReadString();
                                        break;
                                    case "d:Msg_Flag":
                                        model.MessageFlag = reader.ReadString();
                                        break;
                                    case "d:Message":
                                        model.Message = reader.ReadString();
                                        break;
                                    case "d:DeviceID":
                                        model.DeviceId = reader.ReadString();
                                        break;
                                    case "d:Reg_No":
                                        model.RegNo = reader.ReadString();
                                        break;
                                    case "d:ReadingUnit":
                                        model.ReadingUnit = reader.ReadString();
                                        break;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("WebAPIAdaniGas Login API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Sitecore.Diagnostics.Log.Error("WebAPIAdaniGas Login API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageEmailsAdaniGas GetEmailList(string customerID)
        {
            ManageEmailsAdaniGas model = new ManageEmailsAdaniGas();
            EmailEntry obj = new EmailEntry();
            try
            {
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;

                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API Name: " + GetEmailListURL, typeof(ManageEmailsAdaniGas));
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetEmailListURL, customerID);
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, typeof(ManageEmailsAdaniGas));
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetEmailList API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Email_ID":
                                    obj.EmailId = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.EmailsList.Add(obj);
                                    obj = new EmailEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetEmailList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetEmailList API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageEmailsAdaniGas RegisterEmail(string customerID, string emailID)
        {
            ManageEmailsAdaniGas model = new ManageEmailsAdaniGas();
            EmailEntry obj = new EmailEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas RegisterEmail API Call Start, API Name: " + RegisterEmailURL, typeof(ManageEmailsAdaniGas));
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;

                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Ketan --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedemailID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, emailID, EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(RegisterEmailURL, EncryptedcustomerID, EncryptedemailID);
                //Addedd by Ketan --End--
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, typeof(ManageEmailsAdaniGas));
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas RegisterEmail API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Email_ID":
                                    obj.EmailId = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.EmailsList.Add(obj);
                                    obj = new EmailEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterEmail API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Sitecore.Diagnostics.Log.Error("WebAPIAdaniGas RegisterEmail API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageEmailsAdaniGas ModifyEmail(string customerID, string oldEmailID, string newEmailID)
        {
            ManageEmailsAdaniGas model = new ManageEmailsAdaniGas();
            EmailEntry obj = new EmailEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas ModifyEmail API Call Start, API Name: " + ModifyEmailURL, this);
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedoldEmailID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, oldEmailID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptednewEmailID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, newEmailID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedUpdate_Flag = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, "U", EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(ModifyEmailURL, EncryptedcustomerID, EncryptedoldEmailID, EncryptednewEmailID, EncryptedUpdate_Flag);
                //Addedd by Meenakshi --End--
                //    string partialServiceUrl = string.Format(ModifyEmailURL, customerID, oldEmailID, newEmailID);
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas ModifyEmail API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Email_ID":
                                    obj.EmailId = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.EmailsList.Add(obj);
                                    obj = new EmailEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas ModifyEmail API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas ModifyEmail API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageEmailsAdaniGas DeleteEmail(string customerID, string emailID)
        {
            ManageEmailsAdaniGas model = new ManageEmailsAdaniGas();
            EmailEntry obj = new EmailEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas DeleteEmail API Call Start, API Name: " + DeleteEmailURL, typeof(ManageEmailsAdaniGas));
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedemailID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, emailID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedDelete_Flag = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, "D", EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(DeleteEmailURL, EncryptedcustomerID, EncryptedemailID, EncryptedDelete_Flag);
                //Addedd by Meenakshi --End--
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas DeleteEmail API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Email_ID":
                                    obj.EmailId = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.EmailsList.Add(obj);
                                    obj = new EmailEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas DeleteEmail API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas DeleteEmail API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageMobileAdaniGas GetMobileNumbersList(string customerID)
        {
            ManageMobileAdaniGas model = new ManageMobileAdaniGas();
            MobileEntry obj = new MobileEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas GetMobileNumbersList API Call Start, API Name: " + GetMobileNumbersListURL, typeof(ManageMobileAdaniGas));
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetMobileNumbersListURL, customerID);
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetMobileNumbersList API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Mobile_No":
                                    obj.MobileNo = reader.ReadString();
                                    break;
                                case "d:SMS_Enabled":
                                    obj.Ischecked = string.IsNullOrEmpty(reader.ReadString()) ? false : true;
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.MobileNumbersList.Add(obj);
                                    obj = new MobileEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetMobileNumbersList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetMobileNumbersList API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageMobileAdaniGas RegisterMobileNumber(string customerID, string mobileNumber)
        {
            ManageMobileAdaniGas model = new ManageMobileAdaniGas();
            var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
            var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
            MobileEntry obj = new MobileEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas RegisterMobileNumber API Call Start, API Name: " + RegisterMobileNumberURL, typeof(ManageMobileAdaniGas));

                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedmobileNumber = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, mobileNumber, EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(RegisterMobileNumberURL, EncryptedcustomerID, EncryptedmobileNumber);
                //Addedd by Meenakshi --End--

                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());

                Diagnostics.Log.Info("WebAPIAdaniGas RegisterMobileNumber API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Mobile_No":
                                    obj.MobileNo = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.MobileNumbersList.Add(obj);
                                    obj = new MobileEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterMobileNumber API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterMobileNumber API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageMobileAdaniGas ModifyMobileNumber(string customerID, string oldMobileNumber, string newMobileNumber)
        {
            ManageMobileAdaniGas model = new ManageMobileAdaniGas();
            MobileEntry obj = new MobileEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas ModifyMobileNumber API Call Start, API Name: " + ModifyMobileNumberURL, typeof(ManageMobileAdaniGas));
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedoldMobileNumber = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, oldMobileNumber, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptednewMobileNumber = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, newMobileNumber, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedUpdate_Flag = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, "U", EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(ModifyMobileNumberURL, EncryptedcustomerID, EncryptedoldMobileNumber, EncryptednewMobileNumber, EncryptedUpdate_Flag);
                //Addedd by Meenakshi --End--
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas ModifyMobileNumber API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Mobile_No":
                                    obj.MobileNo = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.MobileNumbersList.Add(obj);
                                    obj = new MobileEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas ModifyMobileNumber API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas ModifyMobileNumber API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }


        public ManageMobileAdaniGas DeleteMobileNumber(string customerID, string mobileNumber)
        {
            ManageMobileAdaniGas model = new ManageMobileAdaniGas();
            MobileEntry obj = new MobileEntry();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas DeleteMobileNumber API Call Start, API Name: " + DeleteMobileNumberURL, typeof(ManageMobileAdaniGas));
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedmobileNumber = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, mobileNumber, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedDelete_Flag = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, "D", EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(DeleteMobileNumberURL, EncryptedcustomerID, EncryptedmobileNumber, EncryptedDelete_Flag);
                //Addedd by Meenakshi --End--
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas DeleteMobileNumber API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Mobile_No":
                                    obj.MobileNo = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.MobileNumbersList.Add(obj);
                                    obj = new MobileEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas DeleteMobileNumber API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas DeleteMobileNumber API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public string RegisterSMSAlert(string customerID, string mobileNumber, bool isEnabled)
        {
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas RegisterSMSAlert API Call Start, API Name: " + RegisterSMSAlertURL, typeof(HttpClient));
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedmobileNumber = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, mobileNumber, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedSMS_Enabled = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, "X", EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(RegisterSMSAlertURL, EncryptedcustomerID, EncryptedmobileNumber, isEnabled ? EncryptedSMS_Enabled : "");
                //Addedd by Meenakshi --End--
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas RegisterSMSAlert API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                var messageFlag = "S";

                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Msg_Flag":
                                    messageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    var message = reader.ReadString();
                                    if (messageFlag == "S")
                                        return "success";
                                    return
                                        message;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterSMSAlert API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterSMSAlert API Error: ", e, typeof(Exception));
                return e.Message;
            }
            return "success";
        }
        public static bool IsCNGDealerUserLoggedIn()
        {
            if (SessionHelper.UserSession.CNGDealerUserSessionContext == null)
            {
                return false;
            }
            else if (!SessionHelper.UserSession.CNGDealerUserSessionContext.AuthToken.Equals(HttpContext.Current.Request.Cookies["AuthToken"].Value))
            {
                return false;
            }
            return true;
        }
        public static bool IsCNGAdminUserLoggedIn()
        {
            if (SessionHelper.UserSession.CNGAdminUserSessionContext == null)
            {
                return false;
            }
            else if (!SessionHelper.UserSession.CNGAdminUserSessionContext.AuthToken.Equals(HttpContext.Current.Request.Cookies["AuthToken"].Value))
            {
                return false;
            }
            return true;
        }
        public static bool IsUserLoggedIn()
        {
            if (SessionHelper.UserSession.AdaniGasUserSessionContext == null)
            {
                return false;
            }
            else if (!SessionHelper.UserSession.AdaniGasUserSessionContext.AuthToken.Equals(HttpContext.Current.Request.Cookies["AuthToken"].Value))
            {
                return false;
            }
            return true;
        }

        public static string LoginModuleType(string name)
        {
            if (name.Contains("domestics") || name.Contains("domestic"))
                return AdaniGasUserTypes.PNG;
            if (name.Contains("commercial"))
                return AdaniGasUserTypes.PNG;
            if (name.Contains("industrial"))
                return AdaniGasUserTypes.PNG;
            return null;
        }
        public static string AnotherPayment(string name)
        {
            if (name.Contains("domestic"))
                return "domestics";
            if (name.Contains("commercial"))
                return "commercial-png";
            if (name.Contains("industrial"))
                return "industrial";
            return null;
        }

        public PayOnline QuickPay(string customerID)
        {
            PayOnline obj = new PayOnline();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas QuickPay API Call Start, API Name: " + QuickBillSetURL, typeof(PayOnline));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(QuickBillSetURL, customerID);

                HttpResponseMessage response = client.GetAsync(ServiceURL_2 + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas QuickPay API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Name":
                                    obj.CustomerName = reader.ReadString();
                                    break;
                                case "d:Mobile_No":
                                    obj.Mobile = reader.ReadString();
                                    break;
                                case "d:Email_ID":
                                    obj.Email = reader.ReadString();
                                    break;
                                case "d:Bill_No":
                                    obj.Bill_No = reader.ReadString();
                                    break;
                                case "d:Bill_Date":
                                    obj.Bill_Date = reader.ReadString();
                                    break;
                                case "d:Due_Date":
                                    obj.Due_Date = reader.ReadString();
                                    break;
                                case "d:Amount":
                                    obj.Amount = System.Convert.ToDouble(reader.ReadString());
                                    break;
                                case "d:Current_Outstanding_Amount":
                                    obj.Current_Outstanding_Amount = reader.ReadString();
                                    break;
                                case "d:Partner_Type":
                                    obj.Partner_Type = reader.ReadString();
                                    break;
                                case "d:Execution_time":
                                    obj.Execution_time = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error: ", e, typeof(Exception));
                obj.IsError = true;
                obj.Message = e.Message;
            }
            return obj;
        }

        //ENACH Registration Begin

        public AdaniGasENachRegistrationModel ENachSendOtp(AdaniGasENachRegistrationModel model)
        {
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas SendOTPset API Call Start, API Name: " + ENachSendOtpSet, typeof(AdaniGasENachRegistrationModel));
                model.IsError = false;
                var authenticationBytes = Encoding.ASCII.GetBytes(eNACHSAPUserName + ":" + eNACHSAPPassword);
                IEnumerable<string> cookies = new List<string>();
                CookieContainer cookieJar = new CookieContainer();
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(eNACHSAPUserName, eNACHSAPPassword);

                var client = new RestClient(ServiceURL_5);
                client.CookieContainer = cookieJar;
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes));
                request.AddHeader("x-csrf-token", "fetch");
                IRestResponse response = client.Execute(request);
                string csrfToken = "";
                if (response.IsSuccessful)
                {
                    csrfToken = response.Headers.Where(x => x.Name == "x-csrf-token").Select(x => x.Value).FirstOrDefault().ToString();
                    if (!string.IsNullOrEmpty(csrfToken))
                    {
                        var cookieContainer = new CookieContainer();
                        var client2 = new RestClient(ServiceURL_5 + ENachSendOtpSet);
                        client.BaseUrl = client2.BaseUrl;
                        request.Method = Method.POST;
                        request.AddOrUpdateParameter("x-csrf-token", csrfToken, ParameterType.HttpHeader);
                        request.AddHeader("content-type", "application/json");
                        request.RequestFormat = DataFormat.Json;
                        var buildUri = client.BuildUri(request);
                        //request.AddHeader("authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes));
                        string EncryptedMobileNo = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.MobileNo, EnachSendOTPEncryptionIV);
                        string EncryptedBPUA = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.BPUA, EnachSendOTPEncryptionIV);
                        request.AddParameter("application/json", "{\r\n\"MobileNo\" : \"" + EncryptedMobileNo + "\",\r\n\"BPUA\" : \"" + EncryptedBPUA + "\"\r\n}", ParameterType.RequestBody);
                        IRestResponse response2 = client.Execute(request);

                        if (response2.IsSuccessful)
                        {
                            XmlDocument doc = JsonConvert.DeserializeXmlNode(response2.Content);
                            XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                            StringReader stringReader = new StringReader(incomingXml.ToString());
                            Diagnostics.Log.Info("WebAPIAdaniGas EnachSendOTP API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));

                            using (XmlReader reader = XmlReader.Create(stringReader))
                            {
                                while (reader.Read())
                                {
                                    if (reader.IsStartElement())
                                    {
                                        switch (reader.Name.ToString())
                                        {
                                            case "Msg_Flag":
                                                model.MessageFlag = reader.ReadString(); //EnachSendOTP_EncryptDecrypt.DecryptString(EnachSendOTPEncryptionKey, reader.ReadString(), EnachSendOTPEncryptionIV);
                                                break;
                                            case "Message":
                                                model.Message = reader.ReadString();
                                                break;
                                            case "BPUA":
                                                model.BPUA = EnachSendOTP_EncryptDecrypt.DecryptString(EnachSendOTPEncryptionKey, reader.ReadString(), EnachSendOTPEncryptionIV);
                                                break;
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(model.MessageFlag))
                            {
                                model.MessageFlag = EnachSendOTP_EncryptDecrypt.DecryptString(EnachSendOTPEncryptionKey, model.MessageFlag, model.BPUA);
                            }
                        }
                        else
                        {
                            model.Message = "OTP Sending Failed";
                            model.IsError = true;
                            return model;
                        }
                    }
                    else
                    {
                        model.Message = "CSRF Token Error";
                        model.IsError = true;
                        return model;
                    }
                }
                else
                {
                    model.Message = "CSRF Token Error";
                    model.IsError = true;
                    return model;
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetAfterSalesServiceAmount API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetAfterSalesServiceAmount API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }
        public ValidatedOTPModel EnachValidateOtp(string MobileNo, string OTP)
        {
            ValidatedOTPModel obj = new ValidatedOTPModel();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas EnachValidateOtp API Call Start, API Name: EnachValidateOtp", typeof(ValidatedOTPModel));
                var authenticationBytes = Encoding.ASCII.GetBytes(eNACHSAPUserName + ":" + eNACHSAPPassword);
                IEnumerable<string> cookies = new List<string>();
                CookieContainer cookieJar = new CookieContainer();
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(eNACHSAPUserName, eNACHSAPPassword);
                string partialServiceUrl = string.Format(ENACHValidateOtpSet, MobileNo, OTP);
                var client = new RestClient(ServiceURL_5 + partialServiceUrl);
                var request = new RestRequest(Method.GET);
                request.Credentials = credentials;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes));

                client.CookieContainer = cookieJar;
                IRestResponse response = client.Execute(request);
                XmlDocument doc = JsonConvert.DeserializeXmlNode(response.Content);
                XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas EnachValidateOtp API Response: " + incomingXml.ToString(), typeof(ValidatedOTPModel));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "Msg_Flag":
                                    obj.Msg_Flag = reader.ReadString();
                                    break;
                                case "Message":
                                    obj.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error: ", e, typeof(Exception));
                obj.IsError = true;
                obj.Message = e.Message;
            }
            return obj;
        }
        public AdaniGasENachRegistrationModel EcsPostingService(AdaniGasENachRegistrationModel model)
        {
            try
            {
                string ReponseCode = model.Responsecode == "0300" ? "S" : "F";
                Diagnostics.Log.Info("WebAPIAdaniGas EcsPostingSetService API Call Start, API Name: " + ECSPosting, typeof(afterSalesServices));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(ECSPosting, model.CustomerID, model.TransactionId, ReponseCode, model.AccountNumber ?? "", model.BankName ?? "", model.AccountHolderName ?? "", model.BankRefNo ?? "");
                //Addedd by Meenakshi --End--

                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start EcsPostingSetService, API URL: " + ServiceURL_4 + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL_4 + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas EcsPostingSetService API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                model.IsSapECSPostError = false;
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:EvMsgFlag":
                                    model.SapECSMsgFlag = reader.ReadString();
                                    break;
                                case "d:EvMessage":
                                    model.SapECSMsg = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas EcsPostingSetService API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas EcsPostingSetService API Error: ", e, typeof(Exception));
                model.IsSapECSPostError = true;
                model.SapECSMsg = e.Message;
            }
            return model;
        }
        public AdaniGasENachRegistrationModel ReadEcsPostingService(string CustomerID)
        {
            AdaniGasENachRegistrationModel model = new AdaniGasENachRegistrationModel();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas ReadEcsPostingService API Call Start, API Name: " + ECSPosting, typeof(afterSalesServices));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(ReadENACHPostingSet, CustomerID);
                //Addedd by Meenakshi --End--

                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start ReadEcsPostingService, API URL: " + ServiceURL_4 + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL_4 + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas ReadEcsPostingService API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:ReferenceNo":
                                    model.ExistingRefNo = reader.ReadString();
                                    break;
                                case "d:Current_Status":
                                    model.ECSCurrentStatus = reader.ReadString();
                                    break;
                                case "d:EvMsgFlag":
                                    model.SapECSMsgFlag = reader.ReadString();
                                    break;
                                case "d:EvMessage":
                                    model.SapECSMsg = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas ReadEcsPostingService API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas ReadEcsPostingService API Error: ", e, typeof(Exception));
                model.IsSapECSPostError = true;
                model.SapECSMsg = e.Message;
            }
            return model;
        }
        public PayOnline GetCustomerDetailsWithPartnerId(string customerID)
        {
            PayOnline obj = new PayOnline();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas QuickPay API Call Start, API Name: " + GetCustomerDetails, typeof(PayOnline));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetCustomerDetails, "U0" + customerID);

                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas QuickPay API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:FName":
                                    obj.FirstName = reader.ReadString();
                                    break;
                                case "d:LName":
                                    obj.LastName = reader.ReadString();
                                    break;
                                case "d:MobileNo":
                                    obj.Mobile = reader.ReadString();
                                    break;
                                case "d:Email":
                                    obj.Email = reader.ReadString();
                                    break;
                                case "d:BP_No":
                                    obj.PartnerNo = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error: ", e, typeof(Exception));
                obj.IsError = true;
                obj.Message = e.Message;
            }
            return obj;
        }

        public List<City> GetCityList()
        {
            List<City> cityList = new List<City>();
            City obj = new City();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas GetCityList API Call Start, API Name: " + QuickBillSetURL, typeof(City));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                //string partialServiceUrl = string.Format(QuickBillSetURL, customerID);
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                HttpResponseMessage response = client.GetAsync(ServiceURL_2 + GetCityListURL).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());

                Diagnostics.Log.Info("WebAPIAdaniGas GetCityList API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));

                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Reg_Str_Area":
                                    obj.CityCode = reader.ReadString();
                                    break;
                                case "d:Reg_Str_Area_Txt":
                                    obj.CityName = reader.ReadString();
                                    cityList.Add(obj);
                                    obj = new City();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetCityList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetCityList API Error: ", e, typeof(Exception));
            }
            return cityList;
        }

        public List<CityWisePrice> GetPriceByCity(string city)
        {
            List<CityWisePrice> cityWisePrice = new List<CityWisePrice>();
            CityWisePrice obj = new CityWisePrice();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas GetPriceByCity API Call Start, API Name: " + QuickBillSetURL, typeof(CityWisePrice));
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetPriceByCityURL, city);

                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetPriceByCity API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:City":
                                    obj.City = reader.ReadString();
                                    break;
                                case "d:Product":
                                    obj.Product = reader.ReadString();
                                    break;
                                case "d:Eff_date":
                                    obj.Eff_date = reader.ReadString();
                                    break;
                                case "d:MMBTU_Rate":
                                    obj.MMBTU_Rate = reader.ReadString();
                                    cityWisePrice.Add(obj);
                                    obj = new CityWisePrice();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetPriceByCity API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetPriceByCity API Error: ", e, typeof(Exception));
            }
            return cityWisePrice;
        }

        public List<CityWisePrice> GetGasPrice(string city)
        {
            List<CityWisePrice> cityWisePrice = new List<CityWisePrice>();
            CityWisePrice obj = new CityWisePrice();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas GetGasPrice API Call Start, API Name: " + GetPriceByCityURL, typeof(CityWisePrice));
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetPriceByCityURL, city);
                if (string.IsNullOrEmpty(city))
                {
                    partialServiceUrl = GetGasPriceURL;
                }
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetGasPrice API Response: " + incomingXml.ToString(), typeof(CityWisePrice));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:City":
                                    obj.City = reader.ReadString();
                                    break;
                                case "d:Product":
                                    obj.Product = reader.ReadString();
                                    break;
                                case "d:Eff_date":
                                    obj.Eff_date = reader.ReadString();
                                    break;
                                case "d:MMBTU_Rate":
                                    obj.MMBTU_Rate = reader.ReadString();
                                    cityWisePrice.Add(obj);
                                    obj = new CityWisePrice();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetGasPrice API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetGasPrice API Error: ", e, typeof(Exception));
            }
            return cityWisePrice;
        }

        public BillingDetailsList GetInvoices()
        {
            var customerID = SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
            var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
            var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;

            BillingDetailsList model = new BillingDetailsList();
            BillingDetailsRecord obj = new BillingDetailsRecord();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas BillingInvoice API Call Start, API Name: " + BillingInvoicesURL, typeof(BillingDetailsList));
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(BillingInvoicesURL, customerID);
                HttpResponseMessage response = client.GetAsync(ServiceURL_utilities + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas BillingInvoices API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:InvoiceID":
                                    obj.InvoiceID = reader.ReadString();
                                    break;
                                case "d:AccountID":
                                    obj.AccountID = reader.ReadString();
                                    break;
                                case "d:ContractAccountID":
                                    obj.ContractAccountID = reader.ReadString();
                                    break;
                                case "d:AmountDue":
                                    obj.AmountDue = reader.ReadString();
                                    break;
                                case "d:Currency":
                                    obj.Currency = reader.ReadString();
                                    break;
                                case "d:DueDate":
                                    obj.DueDate = reader.ReadString();
                                    break;
                                case "d:InvoiceDate":
                                    obj.InvoiceDate = reader.ReadString();
                                    break;
                                case "d:AmountPaid":
                                    obj.AmountPaid = reader.ReadString();
                                    break;
                                case "d:AmountRemaining":
                                    obj.AmountRemaining = reader.ReadString();
                                    break;
                                case "d:InvoiceDescription":
                                    obj.InvoiceDescription = reader.ReadString();
                                    break;
                                case "d:InvoiceStatusID":
                                    obj.InvoiceStatusID = reader.ReadString();
                                    break;
                                case "d:Bill_Posting_Date":
                                    obj.Bill_Posting_Date = reader.ReadString();
                                    break;
                                case "d:Bill_Fiscal_Year":
                                    obj.Bill_Fiscal_Year = reader.ReadString();
                                    break;
                                case "d:Bill_ID":
                                    obj.Bill_ID = reader.ReadString();
                                    break;
                                case "d:Bill_Type":
                                    obj.Bill_Type = reader.ReadString();
                                    break;
                                case "d:Print_Document_No":
                                    obj.Print_Document_No = reader.ReadString();
                                    model.BillingDetails.Add(obj);
                                    obj = new BillingDetailsRecord();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas BillingInvoice API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas BillingInvoice API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public afterSalesServices GetAfterSalesServiceQauntityData()
        {
            afterSalesServices model = new afterSalesServices();
            afterSalesServiceRecords obj = new afterSalesServiceRecords();

            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas GetAfterSalesServiceQauntity API Call Start, API Name: " + GetAfterSalesService, typeof(afterSalesServices));
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;

                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetAfterSalesService);
                HttpResponseMessage response = client.GetAsync(ServiceURL_2 + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetAfterSalesServiceQauntity API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Comp_Cat":
                                    obj.Comp_Cat = reader.ReadString();
                                    break;
                                case "d:SrNo":
                                    obj.SrNo = reader.ReadString();
                                    break;
                                case "d:Comp_Type":
                                    obj.Comp_Type = reader.ReadString();
                                    break;
                                case "d:Comp_Text":
                                    obj.Comp_Text = reader.ReadString();
                                    break;
                                case "d:Task_Group":
                                    obj.Task_Group = reader.ReadString();
                                    break;
                                case "d:Quantity_Min":
                                    obj.Quantity_Min = reader.ReadString();
                                    break;
                                case "d:Quantity_Max":
                                    obj.Quantity_Max = reader.ReadString();
                                    model.afterSalesServicesList.Add(obj);
                                    obj = new afterSalesServiceRecords();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetAfterSalesServiceQauntity API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetAfterSalesServiceQauntity API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public afterSalesServices GetAfterSalesServiceAmountData(string Comp_Cat, string SrNo, string Comp_Type, int Quantity)
        {
            afterSalesServices obj = new afterSalesServices();

            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas GetAfterSalesServiceAmount API Call Start, API Name: " + GetAfterSalesServiceDropDownChange, typeof(afterSalesServices));
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var Partner = SessionHelper.UserSession.AdaniGasUserSessionContext.Partner;

                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetAfterSalesServiceDropDownChange, Comp_Type, Comp_Cat, Partner, Quantity, SrNo);
                HttpResponseMessage response = client.GetAsync(ServiceURL_2 + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetAfterSalesServiceAmount API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Amount":
                                    obj.TempAmount = reader.ReadString();
                                    break;
                                case "d:Tax":
                                    obj.Tax = reader.ReadString();
                                    break;
                                case "d:AmountWithTax":
                                    obj.AmountWithTax = reader.ReadString();
                                    break;
                                case "d:ExtraAmount":
                                    obj.ExtraAmount = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    break;
                                case "d:MaxLength":
                                    obj.MaxLengthgas = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetAfterSalesServiceAmount API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetAfterSalesServiceAmount API Error: ", e, typeof(Exception));
                obj.IsError = true;
                obj.Message = e.Message;
            }
            return obj;
        }

        public afterSalesServices SalesServiceDataPost(string Comp_Cat, string SrNo, string Comp_Type, int Quantity, string Text, string username, string password, string Partner)
        {
            afterSalesServices obj = new afterSalesServices();

            try
            {

                //var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                //var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                //var Partner = SessionHelper.UserSession.AdaniGasUserSessionContext.Partner;

                username = "U0" + username;
                Diagnostics.Log.Info("WebAPIAdaniGas SalesServiceDataPostToSAP API Call Start, API Name: " + PostSalesServiceData, typeof(afterSalesServices));
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedPartner = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Partner, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedComp_Cat = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Comp_Cat, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedComp_Type = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Comp_Type, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedSrNo = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, SrNo, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedText = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Text, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedQuantity = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Quantity.ToString(), EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(PostSalesServiceData, EncryptedPartner, EncryptedComp_Cat, EncryptedComp_Type, EncryptedSrNo, EncryptedText, EncryptedQuantity);
                //Addedd by Meenakshi --End--

                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start SalesServiceDataPost, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas SalesServiceDataPostToSAP API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:MsgFlag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas SalesServiceDataPostToSAP API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas SalesServiceDataPostToSAP API Error: ", e, typeof(Exception));
                obj.IsError = true;
                obj.Message = e.Message;
            }
            return obj;
        }

        public afterSalesServices SalesServiceDataPostGC(string Comp_Cat, string SrNo, string Comp_Type, int Quantity, string Text, string customerID)
        {
            afterSalesServices obj = new afterSalesServices();

            try
            {

                var username = UserName;
                var password = Password;

                var customerDetails = GetCustomerDetailsWithPartnerId(customerID);
                var Partner = customerDetails.PartnerNo;


                Diagnostics.Log.Info("WebAPIAdaniGas SalesServiceDataPostToSAP API Call Start, API Name: " + PostSalesServiceData, typeof(afterSalesServices));
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(PostSalesServiceData, Partner, Comp_Cat, Comp_Type, SrNo, Text, Quantity);

                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas SalesServiceDataPostToSAP API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:MsgFlag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas SalesServiceDataPostToSAP API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas SalesServiceDataPostToSAP API Error: ", e, typeof(Exception));
                obj.IsError = true;
                obj.Message = e.Message;
            }
            return obj;
        }


        public SapPiService.Domain.PaymentHistory GetPaymentInformation()
        {
            var PartnerNo = SessionHelper.UserSession.AdaniGasUserSessionContext.Partner;
            var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
            var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;

            SapPiService.Domain.PaymentHistory model = new SapPiService.Domain.PaymentHistory();
            SapPiService.Domain.PaymentHistoryRecord obj = new SapPiService.Domain.PaymentHistoryRecord();

            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas PaymentHistory API Call Start, API Name: " + GetPaymentInformationForDashboard, typeof(PaymentHistory));
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetPaymentInformationForDashboard, PartnerNo);
                HttpResponseMessage response = client.GetAsync(ServiceURL_utilities + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas PaymentHistory API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:ExecutionDate":
                                    obj.PaymentDate = reader.ReadString();
                                    break;
                                case "d:Amount":
                                    obj.Amount = reader.ReadString();
                                    break;
                                case "d:PaymentMethodDescription":
                                    obj.PaymentMode = reader.ReadString();
                                    break;
                                case "d:Cheque_Number":
                                    obj.ChequeNo = reader.ReadString();
                                    model.PaymentHistoryList.Add(obj);
                                    obj = new SapPiService.Domain.PaymentHistoryRecord();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas PaymentHistory API Error, ErrorMessage: " + ex.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas PaymentHistory API Error: ", ex, typeof(Exception));
            }
            return model;
        }

        public IRestResponse ChnagePassword(ChangePasswordAdaniGas model)
        {
            var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
            var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
            Diagnostics.Log.Info("WebAPIAdaniGas ChnagePassword API Call Start, API Name: " + ChangePasswordSet, typeof(ChangePasswordAdaniGas));
            var client = new RestClient(ServiceURL + ChangePasswordSet);
            var request = new RestRequest(Method.POST);
            var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
            client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
            client.AddDefaultHeader("Content-Type", "application/xml");
            client.AddDefaultHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("application/xml",
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<entry xmlns=\"http://www.w3.org/2005/Atom\" xmlns:d=\"http://schemas.microsoft.com/ado/2007/08/dataservices\" xmlns:m=\"http://schemas.microsoft.com/ado/2007/08/dataservices/metadata\" xml:base=\"http://aigwadev.adani.com:8000/sap/opu/odata/SAP/ZMCF001_SRV/\">\r\n   <content type=\"application/xml\">\r\n      <m:properties>\r\n" +
                "<d:UserName>" + "U0" + model.CustomerID + "</d:UserName>\r\n" +
                "<d:Cust_No>" + model.CustomerID + "</d:Cust_No>\r\n" +
                "<d:Password>" + model.OldPassword + "</d:Password>\r\n" +
                "<d:New_Password>" + model.Password + "</d:New_Password>\r\n" +
                "<d:Repeat_Password>" + model.ConfirmPassword + "</d:Repeat_Password>\r\n" +
                "</m:properties>\r\n" +
                "</content>\r\n</entry>",
                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            Diagnostics.Log.Info("WebAPIAdaniGas ChnagePassword API Response: " + response, typeof(IRestResponse));
            return response;
        }

        public UserLoginInfo PaymentPostToSAP(PayOnline model)
        {
            UserLoginInfo obj = new UserLoginInfo();
            try
            {
                var password = string.Empty;
                var username = string.Empty;
                var AdditionalInfo = DateTime.Now.ToString("dd.MM.yyyy");
                if (SessionHelper.UserSession.AdaniGasUserSessionContext != null && SessionHelper.UserSession.AdaniGasUserSessionContext.IsLoggedIn)
                {
                    password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                    username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                }
                else
                {
                    password = Password;
                    username = UserName;
                }

                Diagnostics.Log.Info("WebAPIAdaniGas PaymentPostToSAP API Call Start, API Name: " + PaymentPosting, typeof(UserLoginInfo));

                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentPostToSAP.ID.ToString()));

                string ClearingAcct = itemInfo.Fields[Templates.PaymentPostToSAP.Field.ClearingAcct].Value;
                string CompanyCode = itemInfo.Fields[Templates.PaymentPostToSAP.Field.CompanyCode].Value;
                string BusArea = itemInfo.Fields[Templates.PaymentPostToSAP.Field.BusArea].Value;
                if (model.PaymentMode.ToLower() == "unified payments")
                {
                    model.PaymentMode = "UPI";
                }

                //var PaymentType = Enum.GetName(typeof(EnumPayment.GatewayType), model.PaymentGateway).ToString();
                var PaymentType = "T";
                Sitecore.Diagnostics.Log.Info("WebAPIAdaniGas PaymentPostToSAP Generating String, ClearingAcct:" + ClearingAcct + ",CompanyCode:" + CompanyCode + ",PaymentMode:" + model.PaymentMode, this);
                string partialServiceUrl = string.Format(PaymentPosting, model.CustomerID, ClearingAcct, CompanyCode, BusArea, model.Amount, model.PaymentMode, model.TransactionId, AdditionalInfo, PaymentType);

                HttpResponseMessage response = client.GetAsync(ServiceURL_4 + partialServiceUrl).Result;
                Diagnostics.Log.Info("WebAPIAdaniGas PaymentPostToSAP API Response: " + response, typeof(HttpResponseMessage));
                if (!response.IsSuccessStatusCode)
                {
                    Diagnostics.Log.Error("WebAPIAdaniGas PaymentPostToSAP API Error, ErrorMessage: " + response.IsSuccessStatusCode, typeof(Exception));
                    obj.MessageFlag = response.StatusCode.ToString();
                    obj.Message = response.ReasonPhrase;
                    return obj;
                }
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());

                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:CustomerNo":
                                    Diagnostics.Log.Info("WebAPIAdaniGas PaymentPostToSAP API CustomerNo: " + reader.ReadString(), typeof(Exception));
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:PaymentLot":
                                    Diagnostics.Log.Info("WebAPIAdaniGas PaymentPostToSAP API PaymentLot: " + reader.ReadString(), typeof(Exception));
                                    obj.Name = reader.ReadString();
                                    break;
                                case "d:Partner_Type":
                                    obj.CustomerType = reader.ReadString();
                                    break;
                                case "d:EvMsgFlag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:EvMessage":
                                    obj.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas PaymentHistory API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas PaymentHistory API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return obj;
        }

        public List<SelectListItem> GetComplaintsQuery(string Complaintscategoryvalue)
        {
            List<SelectListItem> listObj = new List<SelectListItem>();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas GetComplaintsQuery API Call Start, API Name: " + GetComplaintQueryURL, typeof(HttpClient));
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string _apiComplaintQueryURL = string.Format(GetComplaintQueryURL, Complaintscategoryvalue, _customerID);
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + _apiComplaintQueryURL, this);

                HttpResponseMessage responsemessage = client.GetAsync(ServiceURL + _apiComplaintQueryURL).Result;
                Diagnostics.Log.Info("WebAPIAdaniGas GetComplaintsQuery API Response: " + responsemessage, typeof(HttpResponseMessage));
                if (responsemessage.IsSuccessStatusCode)
                {
                    listObj.Add(new SelectListItem { Text = "Select", Value = "" });
                    var jsonString = responsemessage.Content.ReadAsStringAsync().Result;
                    ComplaintQueyResponseModel complaintQueyResponse = JsonConvert.DeserializeObject<ComplaintQueyResponseModel>(jsonString);

                    foreach (var itemList in complaintQueyResponse.d.results.Where(w => !w.MsgFlag.Equals("F")))
                    {
                        string drpText = string.Empty;
                        StringBuilder drpValue = new StringBuilder();

                        drpValue.Append(itemList.Category);
                        drpValue.Append("_" + itemList.Comptype);
                        drpValue.Append("_" + itemList.Taskcode);
                        drpValue.Append("_" + itemList.Partner_Type);
                        drpText = itemList.Text;
                        listObj.Add(new SelectListItem { Text = drpText, Value = drpValue.ToString() });
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetComplaintsQuery API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetComplaintsQuery API Error: ", e, typeof(Exception));
                return null;
            }
            return listObj;
        }

        public ComplaintQueyResponseModel GetComplaintsQueryList()
        {
            D dModel = new D();
            ComplaintQueyResponseModel model = new ComplaintQueyResponseModel();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas GetComplaintsQueryList API Call Start, API Name: " + GetComplaintQueryListURL, typeof(HttpClient));
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;

                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");

                string _apiComplaintURL = string.Format(GetComplaintQueryListURL, _customerID, "C");
                string _apiQueryURL = string.Format(GetComplaintQueryListURL, _customerID, "Q");
                Diagnostics.Log.Info("WebAPIAdaniGas responseofComplaint API Call Start, API URL: " + ServiceURL + _apiComplaintURL, this);
                HttpResponseMessage responseofComplaint = client.GetAsync(ServiceURL + _apiComplaintURL).Result;
                Diagnostics.Log.Info("WebAPIAdaniGas GetComplaintsQueryList API Call Start, API Name: " + _apiComplaintURL, typeof(ComplaintQueyResponseModel));
                Diagnostics.Log.Info("WebAPIAdaniGas GetComplaintsQueryList API Response: " + responseofComplaint, typeof(HttpResponseMessage));
                if (responseofComplaint.IsSuccessStatusCode)
                {
                    var jsonString = responseofComplaint.Content.ReadAsStringAsync().Result;
                    ComplaintQueyResponseModel complaintQueyResponse = JsonConvert.DeserializeObject<ComplaintQueyResponseModel>(jsonString);
                    dModel.results = complaintQueyResponse.d.results.Where(w => !w.MsgFlag.Equals("F"))
                                        .OrderByDescending(o => DateTime.ParseExact(o.Comp_Date, "yyyyMMdd", new System.Globalization.CultureInfo("en-US")))
                                        .ToList();
                }
                Diagnostics.Log.Info("WebAPIAdaniGas responseofquery API Call Start, API URL: " + ServiceURL + _apiQueryURL, this);
                HttpResponseMessage responseofquery = client.GetAsync(ServiceURL + _apiQueryURL).Result;
                Diagnostics.Log.Info("WebAPIAdaniGas GetComplaintsQueryList API Call Start, API Name: " + _apiQueryURL, typeof(ComplaintQueyResponseModel));
                Diagnostics.Log.Info("WebAPIAdaniGas GetComplaintsQueryList API Response: " + responseofquery, typeof(HttpResponseMessage));
                if (responseofquery.IsSuccessStatusCode)
                {
                    var jsonString = responseofquery.Content.ReadAsStringAsync().Result;
                    ComplaintQueyResponseModel complaintQueyResponse = JsonConvert.DeserializeObject<ComplaintQueyResponseModel>(jsonString);
                    dModel.results.AddRange(complaintQueyResponse.d.results.Where(w => !w.MsgFlag.Equals("F"))
                        .OrderByDescending(o => DateTime.ParseExact(o.Comp_Date, "yyyyMMdd", new System.Globalization.CultureInfo("en-US")))
                        .ToList());
                }
                model.d = dModel;

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetComplaintsQueryList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetComplaintsQueryList API Error: ", e, typeof(Exception));
                model.d = dModel;
            }
            return model;
        }


        public HttpResponseMessage ComplaintQueryRegistration(string Partner_No, string Company_Category, string Consumption_Type, string Task_Code, string Text)
        {

            var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
            var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;

            Diagnostics.Log.Info("WebAPIAdaniGas ComplaintQueryRegistration API Call Start, API Name: " + RegisterComplaintQuery, typeof(HttpResponseMessage));
            var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
            client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
            //Addedd by Ketan --start--
            string EncryptedPartner_No = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Partner_No, EncryptionIV)).Replace("%2f", "%252F");
            string EncryptedCompany_Category = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Company_Category, EncryptionIV)).Replace("%2f", "%252F");
            string EncryptedConsumption_Type = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Consumption_Type, EncryptionIV)).Replace("%2f", "%252F");
            string EncryptedTask_Code = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Task_Code, EncryptionIV)).Replace("%2f", "%252F");
            string EncryptedText = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Text, EncryptionIV)).Replace("%2f", "%252F");
            string _RegisterComplaintQuery = string.Format(RegisterComplaintQuery, EncryptedPartner_No, EncryptedCompany_Category, EncryptedConsumption_Type, EncryptedTask_Code, EncryptedText);
            //Addedd by Ketan --End--

            Diagnostics.Log.Info("WebAPIAdaniGas GetEmailList API Call Start, API URL: " + ServiceURL + _RegisterComplaintQuery, this);

            HttpResponseMessage response = client.GetAsync(ServiceURL + _RegisterComplaintQuery).Result;
            Diagnostics.Log.Info("WebAPIAdaniGas ComplaintQueryRegistration API Response: " + response, typeof(HttpResponseMessage));
            return response;

        }
        public IRestResponse RegisterAccount(RegistrationInfoAdaniGas model)
        {
            string patnertype = string.Empty;
            if (model.PartnerType == "9001") // Residemtial
            {
                patnertype = "IND";
            }
            else if (model.PartnerType == "9002") // Residemtial
            {
                patnertype = "COM";
            }
            else  // Residemtial
            {
                patnertype = "DOM";
            }
            string firstname = model.FirstName.Length > 35 ? model.FirstName.Substring(0, 35) : model.FirstName;
            string lastname = model.LastName.Length > 40 ? model.LastName.Substring(0, 40) : model.LastName;

            Diagnostics.Log.Info("WebAPIAdaniGas RegisterAccount API Call Start, API Name: " + _RegisterAccountQuery, typeof(RegistrationInfoAdaniGas));
            var client = new RestClient(ServiceURL_2 + _RegisterAccountQuery);
            var request = new RestRequest(Method.POST);
            var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
            client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
            client.AddDefaultHeader("Content-Type", "application/xml");
            client.AddDefaultHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("undefined"
                , "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                "<entry xmlns=\"http://www.w3.org/2005/Atom\" xmlns:d=\"http://schemas.microsoft.com/ado/2007/08/dataservices\" xmlns:m=\"http://schemas.microsoft.com/ado/2007/08/dataservices/metadata\" xml:base=\"http://aigwadev.adani.com:8000/sap/opu/odata/sap/ZMCF002_SRV/\">\r\n   <id>http://aigwadev.adani.com:8000/sap/opu/odata/sap/ZMCF002_SRV/Create_AccountSet" +
                "('" + model.CustomerID + "')</id>\r\n " +
                "<title type=\"text\">Create_AccountSet('" + model.CustomerID + "')</title>\r\n  " +
                "<updated>2016-11-16T19:07:55Z</updated>\r\n   " +
                "<category term=\"ZMCF002_SRV.Create_Account\" scheme=\"http://schemas.microsoft.com/ado/2007/08/dataservices/scheme\" />\r\n  " +
                "<link href=\"Create_AccountSet('" + model.CustomerID + "')\" rel=\"self\" title=\"Create_Account\" />\r\n  " +
                "<content type=\"application/xml\">\r\n      " +
                "<m:properties>\r\n         " +
                "<d:Cust_No>" + model.CustomerID + "</d:Cust_No>\r\n         " +
                "<d:FirstName>" + WebUtility.HtmlEncode(firstname) + "</d:FirstName>\r\n         " +
                "<d:LastName>" + WebUtility.HtmlEncode(lastname) + "</d:LastName>\r\n         " +
                "<d:Mobile>" + model.MobileNumber + "</d:Mobile>\r\n         " +
                "<d:Email>" + model.Email + "</d:Email>\r\n         " +
                "<d:New_Password>" + model.Password + "</d:New_Password>\r\n         " +
                "<d:Repeat_Password>" + model.ConfirmPassword + "</d:Repeat_Password>\r\n         " +
                "<d:Partner_Type>" + patnertype + "</d:Partner_Type>\r\n         " +
                "<d:Ev_Msg_Flag />\r\n         " +
                "<d:Ev_Message />\r\n      " +
                "</m:properties>\r\n   " +
                "</content>\r\n" +
                "</entry>"
                , ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Diagnostics.Log.Info("WebAPIAdaniGas RegisterAccount API Response: " + response, typeof(IRestResponse));
            return response;
        }

        public RegisterForEBillAlert GetEBillReadEInvoiceSet(string customerID)
        {
            RegisterForEBillAlert model = new RegisterForEBillAlert();

            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas RegisterForEBillReadEInvoiceSet API Call Start, API Name: " + GetEBillReadInvoice, typeof(RegisterForEBillAlert));
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;

                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");

                string partialServiceUrl = string.Format(GetEBillReadInvoice, customerID);
                Diagnostics.Log.Info("WebAPIAdaniGas GetEBillReadEInvoiceSet API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);

                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas RegisterForEBillReadEInvoiceSet API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    model.CustomerID = reader.ReadString();
                                    break;
                                case "d:EInvoice_Type":
                                    model.EInvoice_Type = reader.ReadString();
                                    break;
                                case "d:Email_id":
                                    model.Email_id = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    model.Msg_Flag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    model.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterForEBillReadEInvoiceSet API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterForEBillReadEInvoiceSet API Error: ", e, typeof(Exception));
                model.Msg_Flag = "F";
                model.Message = e.Message;
            }
            return model;
        }

        public RegisterForEBillAlert GetEBillUpdateEInvoiceSet(RegisterForEBillAlert model)
        {
            RegisterForEBillAlert obj = new RegisterForEBillAlert();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas RegisterForEBillAlert API Call Start, API Name: " + GetEBillReadInvoice, typeof(RegisterForEBillAlert));
                var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");

                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, model.CustomerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedEInvoice_Type = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, model.EInvoice_Type, EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(GetEBillUpdateInvoice, EncryptedcustomerID, EncryptedEInvoice_Type);
                //Addedd by Meenakshi --End--
                Diagnostics.Log.Info("WebAPIAdaniGas GetEBillUpdateEInvoiceSet API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas RegisterForEBillAlert API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:EInvoice_Type":
                                    obj.EInvoice_Type = reader.ReadString();
                                    break;
                                case "d:Message_Flag":
                                    obj.Msg_Flag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterForEBillAlert API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterForEBillAlert API Error: ", e, typeof(Exception));
                obj.Msg_Flag = "F";
                obj.Message = e.Message;
            }
            return obj;

        }

        public IRestResponse PostFeedback(CustomerFeedbackAdaniGasModel model)
        {
            Diagnostics.Log.Info("WebAPIAdaniGas PostFeedback API Call Start, API Name: " + _PostFeedbackQuery, typeof(CustomerFeedbackAdaniGasModel));
            var client = new RestClient(ServiceURL_2 + _PostFeedbackQuery);
            var request = new RestRequest(Method.POST);
            var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
            client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
            client.AddDefaultHeader("Content-Type", "application/xml");
            client.AddDefaultHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("undefined"
                , "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                "<entry xmlns=\"http://www.w3.org/2005/Atom\" xmlns:d=\"http://schemas.microsoft.com/ado/2007/08/dataservices\" xmlns:m=\"http://schemas.microsoft.com/ado/2007/08/dataservices/metadata\" xml:base=\"http://aigwadev.adani.com:8000/sap/opu/odata/sap/ZMCF002_SRV/\">\r\n" +
                "<content type=\"application/xml\">\r\n  " +
                " <m:properties>\r\n         " +
                "<d:Cust_No>" + model.CustomerID + "</d:Cust_No>\r\n " +
                "<d:Name>" + model.CustomerName + "</d:Name>\r\n " +
                "<d:Address>" + model.Address + "</d:Address>\r\n " +
                "<d:Response_CQR>" + model.Response_CQR + "</d:Response_CQR>\r\n" +
                "<d:Rep_Performance>" + model.Rep_Performance + "</d:Rep_Performance>\r\n  " +
                "<d:Del_Performance>" + model.Del_Performance + "</d:Del_Performance>\r\n " +
                "<d:Pricing>" + model.Pricing + "</d:Pricing>\r\n " +
                "<d:Handl_Cust_Comp>" + model.Handl_Cust_Comp + "</d:Handl_Cust_Comp>\r\n   " +
                "<d:Overall_Performance>" + model.Overall_Performance + "</d:Overall_Performance>\r\n" +
                "<d:Comments>" + model.Comments + "</d:Comments>\r\n  " +
                "</m:properties>\r\n   " +
                "</content>\r\n" +
                "</entry>"
                , ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Diagnostics.Log.Info("WebAPIAdaniGas PostFeedback API Response: " + response, typeof(IRestResponse));
            return response;
        }


        public IRestResponse PostFeedbackMain(CustomerFeedbackMainAdaniGasModel model)
        {
            Diagnostics.Log.Info("WebAPIAdaniGas PostFeedback API Call Start, API Name: " + _PostFeedbackQuery, typeof(CustomerFeedbackAdaniGasModel));
            var client = new RestClient(ServiceURL_6 + _PostFeedbackQuery);
            var request = new RestRequest(Method.POST);
            var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
            client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
            client.AddDefaultHeader("Content-Type", "application/xml");
            client.AddDefaultHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("undefined"
                , "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                "<entry xmlns=\"http://www.w3.org/2005/Atom\" xmlns:d=\"http://schemas.microsoft.com/ado/2007/08/dataservices\" xmlns:m=\"http://schemas.microsoft.com/ado/2007/08/dataservices/metadata\" xml:base=\"http://aigwadev.adani.com:8000/sap/opu/odata/sap/ZMCF002_SRV/\">\r\n" +
                "<content type=\"application/xml\">\r\n  " +
                " <m:properties>\r\n         " +
                "<d:Cust_No>" + model.CustomerID + "</d:Cust_No>\r\n " +
                 "<d:CompNo>" + model.ComplaintNumber + "</d:CompNo>\r\n " +
                "<d:Response_CQR>" + model.Response_CQR + "</d:Response_CQR>\r\n" +
                "<d:Rep_Performance>" + model.Rep_Performance + "</d:Rep_Performance>\r\n  " +
                "<d:Del_Performance>" + model.Del_Performance + "</d:Del_Performance>\r\n " +
                "<d:Pricing>" + model.Pricing + "</d:Pricing>\r\n " +
                "<d:Handl_Cust_Comp>" + model.Handl_Cust_Comp + "</d:Handl_Cust_Comp>\r\n   " +
                "<d:Overall_Performance>" + model.Overall_Performance + "</d:Overall_Performance>\r\n" +
                //"<d:Comments>" + model.Comments + "</d:Comments>\r\n  " +
                "</m:properties>\r\n   " +
                "</content>\r\n" +
                "</entry>"
                , ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Diagnostics.Log.Info("WebAPIAdaniGas PostFeedback API Response: " + response, typeof(IRestResponse));
            return response;
        }

        public IRestResponse CustomerInfo(string customerId)
        {
            string partialServiceUrl = string.Format(QuickBillSetURL, customerId);
            Diagnostics.Log.Info("WebAPIAdaniGas CustomerInfo API Call Start, API Name: " + QuickBillSetURL, typeof(IRestResponse));
            var client = new RestClient(ServiceURL_2 + partialServiceUrl);
            var request = new RestRequest(Method.GET);
            var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
            client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
            IRestResponse response = client.Execute(request);
            Diagnostics.Log.Info("WebAPIAdaniGas CustomerInfo API Response: " + response, typeof(IRestResponse));
            return response;
        }

        #region Enquiry for New connection
        public List<SelectListItem> GetAreaOfCity(string cityCode)
        {
            List<SelectListItem> listObj = new List<SelectListItem>();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas GetAreaOfCity API Call Start, API Name: " + GetRegStrGroupsSetURL, typeof(RestClient));
                string _apiAreaRetrieveURL = string.Format(GetRegStrGroupsSetURL, cityCode);
                var client = new RestClient(ServiceURL_2 + _apiAreaRetrieveURL);
                var request = new RestRequest(Method.GET);
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
                Diagnostics.Log.Info("WebAPIAdaniGas GetAreaOfCity API Response: " + response, typeof(IRestResponse));
                if (!string.IsNullOrEmpty(jsonString))
                {
                    // convert JSON text contained in string json into an XML node
                    var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonString), new XmlDictionaryReaderQuotas()));
                    IEnumerable<XElement> element = xml.Descendants().Where(p => p.Name.LocalName == "item").ToList();
                    foreach (var _elem in element)
                    {
                        string drpVal = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "Reg_Str_Grp").Value : string.Empty;
                        string drpText = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "Reg_Str_Grp_Txt").Value : string.Empty;
                        listObj.Add(new SelectListItem { Text = drpText, Value = drpVal });
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetAreaOfCity API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetAreaOfCity API Error: ", e, typeof(Exception));
                return null;
            }
            return listObj;
        }

        public IRestResponse SubmitInquiry(NewConnectionEnquiryModel model)
        {
            List<City> cityList = GetCityList();
            var City = cityList.Where(x => x.CityCode == model.City).ToList().FirstOrDefault().CityName;
            string[] args = {
                "Q","02" ,"0033" ,"03",model.Text,model.Plant,model.Partner_Type,model.Name,"",model.Street1, model.Street2, model.Street3, "", "",
                model.PostalCode,City,model.Reg_Str_Grp,"IN",model.Email,model.Mobile
            };
            Diagnostics.Log.Info("WebAPIAdaniGas SubmitInquiry API Call Start, API Name: " + ApplyForInquiryURL, typeof(IRestResponse));
            ApplyForInquiryURL = string.Format(ApplyForInquiryURL, args);
            var client = new RestClient(ServiceURL_2 + ApplyForInquiryURL);
            var request = new RestRequest(Method.GET);
            var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
            client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
            IRestResponse response = client.Execute(request);
            Diagnostics.Log.Info("WebAPIAdaniGas SubmitInquiry API Response: " + response, typeof(IRestResponse));
            return response;
        }

        public List<SelectListItem> CityList(string PartnerTypeCode)
        {
            List<SelectListItem> listObj = new List<SelectListItem>();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas NewConnection CityList API Call Start, API Name: " + RSA_MappingSet, typeof(RestClient));
                string _apiAreaRetrieveURL = string.Format(RSA_MappingSet, PartnerTypeCode);
                var client = new RestClient(ServiceURL_5 + _apiAreaRetrieveURL);
                var request = new RestRequest(Method.GET);
                var authenticationBytes = Encoding.ASCII.GetBytes(eNACHSAPUserName + ":" + eNACHSAPPassword);
                client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
                Diagnostics.Log.Info("WebAPIAdaniGas NewConnection CityList API Response: " + response, typeof(IRestResponse));
                if (!string.IsNullOrEmpty(jsonString))
                {
                    // convert JSON text contained in string json into an XML node
                    var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonString), new XmlDictionaryReaderQuotas()));
                    IEnumerable<XElement> element = xml.Descendants().Where(p => p.Name.LocalName == "item").ToList();
                    foreach (var _elem in element)
                    {
                        string drpVal = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "Werks").Value : string.Empty;
                        string drpText = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "RegioareaDec").Value : string.Empty;
                        listObj.Add(new SelectListItem { Text = drpText, Value = drpVal });
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas NewConnection CityList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas NewConnection CityList API Error: ", e, typeof(Exception));
                return null;
            }
            return listObj.OrderBy(x => x.Text).ToList();
        }

        public List<SelectListItem> AreaOfCity(string cityCode, string PartnerTypeCode)
        {
            List<SelectListItem> listObj = new List<SelectListItem>();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas NewConnection AreaofCityList API Call Start, API Name: " + RSA_MappingSet, typeof(RestClient));
                string _apiAreaRetrieveURL = string.Format(RSA_MappingSet, PartnerTypeCode);
                var client = new RestClient(ServiceURL_5 + _apiAreaRetrieveURL);
                var request = new RestRequest(Method.GET);
                var authenticationBytes = Encoding.ASCII.GetBytes(eNACHSAPUserName + ":" + eNACHSAPPassword);
                client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
                Diagnostics.Log.Info("WebAPIAdaniGas NewConnection AreaofCityList API Response: " + response, typeof(IRestResponse));
                if (!string.IsNullOrEmpty(jsonString))
                {
                    // convert JSON text contained in string json into an XML node
                    var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonString), new XmlDictionaryReaderQuotas()));
                    IEnumerable<XElement> element = xml.Descendants().Where(p => p.Name.LocalName == "item").ToList();
                    foreach (var _elem in element)
                    {
                        if (_elem != null && _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "Werks").Value == cityCode && _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "BPKind").Value == PartnerTypeCode)
                        {
                            string drpVal = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "Regiogroup").Value : string.Empty;
                            string drpText = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "Descript").Value : string.Empty;
                            listObj.Add(new SelectListItem { Text = drpText, Value = drpVal });
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas NewConnection AreaofCityList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas NewConnection AreaofCityList API Error: ", e, typeof(Exception));
                return null;
            }
            return listObj.OrderBy(x => x.Text).ToList();
        }

        public List<SocietyList> SocietyByArea(string AreaCode, string CityCode)
        {
            List<SocietyList> listObj = new List<SocietyList>();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas NewConnection AreaofCityList API Call Start, API Name: " + RSA_MappingSet, typeof(RestClient));
                string _apiAreaRetrieveURL = string.Format(ConnObjSet, AreaCode, CityCode);
                var client = new RestClient(ServiceURL_5 + _apiAreaRetrieveURL);
                var request = new RestRequest(Method.GET);
                var authenticationBytes = Encoding.ASCII.GetBytes(eNACHSAPUserName + ":" + eNACHSAPPassword);
                client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
                Diagnostics.Log.Info("WebAPIAdaniGas NewConnection AreaofCityList API Response: " + response, typeof(IRestResponse));
                //adding Other option
                listObj.Add(new SocietyList { SocietyName = DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/OtherSocietyName", "Other"), SocietyCode = "", AddressLine1 = "", AddressLine2 = "", Street = "", PostalCode = "", Msg_Flag = "", Message = "" });
                if (!string.IsNullOrEmpty(jsonString))
                {
                    // convert JSON text contained in string json into an XML node
                    var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonString), new XmlDictionaryReaderQuotas()));
                    IEnumerable<XElement> element = xml.Descendants().Where(p => p.Name.LocalName == "item").ToList();
                    foreach (var _elem in element)
                    {
                        SocietyList model = new SocietyList();
                        model.SocietyCode = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "RegioGroup").Value : string.Empty;
                        model.SocietyName = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "SocietyName").Value : string.Empty;
                        model.AddressLine1 = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "AddressLine1").Value : string.Empty;
                        model.AddressLine2 = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "AddressLine2").Value : string.Empty;
                        model.Street = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "Street").Value : string.Empty;
                        model.PostalCode = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "PostalCode").Value : string.Empty;
                        model.Msg_Flag = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "Msg_Flag").Value : string.Empty;
                        model.Message = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "Message").Value : string.Empty;
                        listObj.Add(model);
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas NewConnection AreaofCityList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas NewConnection AreaofCityList API Error: ", e, typeof(Exception));
                return null;
            }
            return listObj.OrderBy(x => x.SocietyName).ToList();
        }
        #endregion


        #region ForgotPassword
        public ForgotPasswordAdaniGas ForgotPasswordSendOTP(ForgotPasswordAdaniGas model)
        {
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas ForgotPasswordSendOTP API Call Start, API Name: " + ApplyForInquiryURL, typeof(ForgotPasswordAdaniGas));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Ketan --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, model.CustomerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedOtpPurpose = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, "FP", EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(ForgotPasswordSendOTPCall, EncryptedcustomerID, EncryptedOtpPurpose);
                //Addedd by Ketan --End--
                HttpResponseMessage response = client.GetAsync(ServiceURL_2 + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas ForgotPasswordSendOTP API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Mobile_No":
                                    model.Mobile_No = reader.ReadString();
                                    break;
                                case "d:OTP_Validity_Minutes":
                                    model.OTP_Validity_Minutes = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    model.Msg_Flag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    model.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas ForgotPasswordSendOTP API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas ForgotPasswordSendOTP API Error: ", e, typeof(Exception));
            }
            return model;
        }

        public IRestResponse PostOTPForgotPassword(ForgotPasswordValidateOTP model, string _customerID)
        {
            Diagnostics.Log.Info("WebAPIAdaniGas PostOTPForgotPassword API Call Start, API Name: " + ForgotPasswordPostOTP, typeof(ForgotPasswordAdaniGas));
            var client = new RestClient(ServiceURL_2 + ForgotPasswordPostOTP);
            var request = new RestRequest(Method.POST);
            var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
            client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
            client.AddDefaultHeader("Content-Type", "application/xml");
            client.AddDefaultHeader("X-Requested-With", "XMLHttpRequest");

            request.AddParameter("undefined",
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                "<entry xml:base=\"http://getqa.adani.com:8000/sap/opu/odata/sap/ZMCF002_SRV/\" xmlns=\"http://www.w3.org/2005/Atom\" xmlns:m=\"http://schemas.microsoft.com/ado/2007/08/dataservices/metadata\" xmlns:d=\"http://schemas.microsoft.com/ado/2007/08/dataservices\">\n\t" +
                "<id>http://getqa.adani.com:8000/sap/opu/odata/sap/ZMCF002_SRV/ForgotPasswordSet(Cust_No='" + _customerID + "',Valid_OTP='" + model.Valid_OTP + "',New_Password='" + model.New_Password + "',Repeat_Password='" + model.Repeat_Password + "')</id> \n\t " +
                "<title type=\"text\">ForgotPasswordSet(Cust_No='" + _customerID + "',Valid_OTP='" + model.Valid_OTP + "',New_Password='" + model.New_Password + "',Repeat_Password='" + model.Repeat_Password + "')</title>\n\t " +
                "<updated>2019-06-13T06:31:43Z</updated> \n\t " +
                "<category term=\"ZMCF002_SRV.ForgotPassword\" scheme=\"http://schemas.microsoft.com/ado/2007/08/dataservices/scheme\"/>\n\t " +
                "<link href=\"ForgotPasswordSet(Cust_No='" + _customerID + "',Valid_OTP='" + model.Valid_OTP + "',New_Password='Welcome%40123',Repeat_Password='Welcome%40123')\" rel=\"self\" title=\"ForgotPassword\"/>\n\t " +
                "<content type=\"application/xml\"> \n\t " +
                "<m:properties>\n\t \t   " +
                "<d:Cust_No>" + _customerID + "</d:Cust_No> \n\t \t" +
                "<d:Valid_OTP>" + model.Valid_OTP + "</d:Valid_OTP>\n\t \t   " +
                "<d:New_Password>" + model.New_Password + "</d:New_Password>\n\t \t   " +
                "<d:Repeat_Password>" + model.Repeat_Password + "</d:Repeat_Password>  \n\t \t   " +
                "<d:Ev_Msg_Flag/>\n\t \t   " +
                "<d:Ev_Message/> \n\t " +
                "</m:properties>\n\t " +
                "</content>\n" +
                "</entry>", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            Diagnostics.Log.Info("WebAPIAdaniGas PostOTPForgotPassword API Response: " + response, typeof(IRestResponse));
            return response;
        }
        #endregion


        #region Collection Center
        public List<SelectListItem> GetAreaOfCenterAndCity(string center, string city)
        {
            List<SelectListItem> listObj = new List<SelectListItem>();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas GetAreaOfCenterAndCity API Call Start, API Name: " + GetAreabyCenterAndCityURL, typeof(ForgotPasswordAdaniGas));
                string endPointUrl = string.Format(GetAreabyCenterAndCityURL, center, city);
                var client = new RestClient(ServiceURL_2 + endPointUrl);
                var request = new RestRequest(Method.GET);
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
                IRestResponse response = client.Execute(request);
                Diagnostics.Log.Info("WebAPIAdaniGas GetAreaOfCenterAndCity API Response: " + response, typeof(IRestResponse));
                var jsonString = response.Content;
                if (!string.IsNullOrEmpty(jsonString))
                {
                    //// convert JSON text contained in string json into an XML node
                    var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonString), new XmlDictionaryReaderQuotas()));
                    var TotalCityList = xml.Descendants("item").Elements("NearArea").ToList();
                    if (TotalCityList != null && TotalCityList.Any())
                    {
                        var distinctCityList = TotalCityList.GroupBy(x => x.Value, (key, group) => group.First()).Select(s => s.Value);
                        listObj = distinctCityList.Select(p => new SelectListItem() { Text = p, Value = p }).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetAreaOfCenterAndCity API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetAreaOfCenterAndCity API Error: ", e, typeof(Exception));
                return null;
            }
            return listObj;
        }

        public List<string> GetCompleteCentersList()
        {
            List<string> CityList = new List<string>();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas GetCenterList API Call Start, API Name: " + GetAreaCenterCentersCityURL, typeof(ForgotPasswordAdaniGas));
                string endPointUrl = GetAreaCenterCentersCityURL;
                var client = new RestClient(ServiceURL_2 + endPointUrl);
                var request = new RestRequest(Method.GET);
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
                Diagnostics.Log.Info("WebAPIAdaniGas GetCenterList API Response: " + response, typeof(IRestResponse));
                if (!string.IsNullOrEmpty(jsonString))
                {
                    //// convert JSON text contained in string json into an XML node
                    var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonString), new XmlDictionaryReaderQuotas()));
                    CityList = xml.Descendants("item").Select(i => i.Element("City").Value).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetCenterList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetCenterList API Error: ", e, typeof(Exception));
                return null;
            }
            return CityList;
        }

        public List<Centers> GetCenterList(string center, string city, string area)
        {
            List<Centers> listObj = new List<Centers>();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas GetCenterList API Call Start, API Name: " + GetAreabyCenterAndCityURL, typeof(ForgotPasswordAdaniGas));
                string endPointUrl = string.Format(GetAreabyCenterAndCityURL, center, city);
                var client = new RestClient(ServiceURL_2 + endPointUrl);
                var request = new RestRequest(Method.GET);
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
                Diagnostics.Log.Info("WebAPIAdaniGas GetCenterList API Response: " + response, typeof(IRestResponse));
                if (!string.IsNullOrEmpty(jsonString))
                {
                    //// convert JSON text contained in string json into an XML node
                    var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonString), new XmlDictionaryReaderQuotas()));
                    var CenterList = xml.Descendants("item").Where(i => i.Element("NearArea").Value.ToLower() == area.ToLower()).ToList();
                    if (CenterList != null && CenterList.Any())
                    {
                        listObj = CenterList.Select(p => new Centers()
                        {
                            AddrLine1 = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "AddrLine1").Value,
                            AddrLine2 = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "AddrLine2").Value,
                            AddrLine3 = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "AddrLine3").Value,
                            AddrLine4 = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "AddrLine4").Value,
                            AddrLine5 = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "AddrLine5").Value,
                            AddrLine6 = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "AddrLine6").Value,
                            AddrLine7 = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "AddrLine7").Value,
                            AddrLine8 = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "AddrLine8").Value,
                            Blocked = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "Blocked").Value,
                            City = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "City").Value,
                            FlagCom = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "FlagCom").Value,
                            FlagDom = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "FlagDom").Value,
                            FlagInd = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "FlagInd").Value,
                            IvRadius = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "IvRadius").Value,
                            Landline = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "Landline").Value,
                            Landline2 = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "Landline2").Value,
                            Latitude = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "Latitude").Value,
                            LocNo = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "LocNo").Value,
                            LocType = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "LocType").Value,
                            Longitude = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "Longitude").Value,
                            MobileNo = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "MobileNo").Value,
                            MobileNo2 = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "MobileNo2").Value,
                            NearArea = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "NearArea").Value,
                            PinCode = p.Descendants().FirstOrDefault(x => x.Name.LocalName == "PinCode").Value,
                        }).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetCenterList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetCenterList API Error: ", e, typeof(Exception));
                return null;
            }
            return listObj;
        }
        #endregion

        #region Self billing
        public XDocument GetPreviousReadingSelfBilling(string customerId)
        {
            Diagnostics.Log.Info("WebAPIAdaniGas GetPreviousReadingSelfBilling API Call Start, API Name: " + GetPreviousMeterReadingForSelfBillingURL, typeof(XDocument));
            GetPreviousMeterReadingForSelfBillingURL = string.Format(GetPreviousMeterReadingForSelfBillingURL, customerId);
            var request = new RestRequest(Method.GET);
            //var authenticationBytes = Encoding.ASCII.GetBytes(string.Concat("U0", UserName) + ":" + Password);
            var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
            //var authenticationBytes = Encoding.ASCII.GetBytes("U01000082139" + ":" + "Welcome@12");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
            client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
            HttpResponseMessage response = client.GetAsync(ServiceURL_7 + GetPreviousMeterReadingForSelfBillingURL).Result;
            Diagnostics.Log.Info("WebAPIAdaniGas GetPreviousMeterReadingForSelfBillingURL API Response: " + response, typeof(HttpResponseMessage));
            if (response.IsSuccessStatusCode)
            {
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument responseXml = XDocument.Load(resultContent);
                return responseXml;
            }
            return null;

        }

        #endregion


        #region Meter Reading
        public XDocument GetPreviousReading(string partner)
        {
            //partner = "1000074382";
            Diagnostics.Log.Info("WebAPIAdaniGas PreviousReading API Call Start, API Name: " + GetPreviousMeterReadingURL, typeof(XDocument));
            GetPreviousMeterReadingURL = string.Format(GetPreviousMeterReadingURL, partner);
            var request = new RestRequest(Method.GET);
            //var authenticationBytes = Encoding.ASCII.GetBytes(string.Concat("U0", UserName) + ":" + Password);
            var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
            //var authenticationBytes = Encoding.ASCII.GetBytes("U01000082139" + ":" + "Welcome@12");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
            client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
            HttpResponseMessage response = client.GetAsync(ServiceURL_utilities + GetPreviousMeterReadingURL).Result;
            Diagnostics.Log.Info("WebAPIAdaniGas PreviousReading API Response: " + response, typeof(HttpResponseMessage));
            if (response.IsSuccessStatusCode)
            {
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument responseXml = XDocument.Load(resultContent);
                return responseXml;
            }
            return null;

        }

        public XDocument GetPreviousReading(string partner, string UserName, string Password)
        {

            //partner = "1000074382";
            Diagnostics.Log.Info("WebAPIAdaniGas PreviousReading API Call Start, API Name: " + GetPreviousMeterReadingURL, typeof(XDocument));
            GetPreviousMeterReadingURL = string.Format(GetPreviousMeterReadingURL, partner);
            var request = new RestRequest(Method.GET);
            var authenticationBytes = Encoding.ASCII.GetBytes(string.Concat("U0", UserName) + ":" + Password);
            //var authenticationBytes = Encoding.ASCII.GetBytes("U01000082139" + ":" + "Welcome@12");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
            client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
            HttpResponseMessage response = client.GetAsync(ServiceURL_utilities + GetPreviousMeterReadingURL).Result;
            Diagnostics.Log.Info("WebAPIAdaniGas PreviousReading API Response: " + response, typeof(HttpResponseMessage));
            if (response.IsSuccessStatusCode)
            {
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument responseXml = XDocument.Load(resultContent);
                return responseXml;
            }
            return null;

        }

        public IRestResponse PostCurrentMeterReading(MeterReadingAdaniGas model)
        {
            string _deviceId = SessionHelper.UserSession.AdaniGasUserSessionContext.DeviceId;
            string _userId = SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
            string _password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
            string _regNo = SessionHelper.UserSession.AdaniGasUserSessionContext.RegNo;
            string _readingUnit = SessionHelper.UserSession.AdaniGasUserSessionContext.ReadingUnit;
            string _meterSerialNumber = SessionHelper.UserSession.AdaniGasUserSessionContext.Meter_SerialNumber;
            Diagnostics.Log.Info("WebAPIAdaniGas PostCurrentMeterReading API Call Start, API Name: " + _SubmitMeaterReadinURL, typeof(XDocument));
            _SubmitMeaterReadinURL = string.Format(_SubmitMeaterReadinURL, SessionHelper.UserSession.AdaniGasUserSessionContext.DeviceId);
            var client = new RestClient(ServiceURL_utilities + _SubmitMeaterReadinURL);
            var request = new RestRequest(Method.POST);
            var authenticationBytes = Encoding.ASCII.GetBytes(string.Concat("U0", _userId) + ":" + _password);
            client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
            client.AddDefaultHeader("Content-Type", "application/xml");
            client.AddDefaultHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("undefined"
                , "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                "<entry xmlns=\"http://www.w3.org/2005/Atom\" xmlns:d=\"http://schemas.microsoft.com/ado/2007/08/dataservices\" xmlns:m=\"http://schemas.microsoft.com/ado/2007/08/dataservices/metadata\" xml:base=\"http://aigwadev.adani.com:8000/sap/opu/odata/sap/ERP_UTILITIES_UMC/\">\r\n   " +
                "<content type=\"application/xml\">\r\n      " +
                "<m:properties>\r\n        " +
                "<d:DeviceID>" + _deviceId + "</d:DeviceID>\r\n " +
                "<d:RegisterID>" + _regNo + "</d:RegisterID>\r\n " +
                "<d:ReadingResult>" + model.MeterReading + "</d:ReadingResult>\r\n " +
                "<d:ReadingDateTime>" + model.ReadingDateandTime + "</d:ReadingDateTime>\r\n" +
                "<d:ReadingUnit>" + _readingUnit.ToLower() + "</d:ReadingUnit>\r\n   " +
                "<d:MeterReadingNoteID />\r\n    " +
                "<d:Consumption>10.0000</d:Consumption>\r\n " +
                "<d:MeterReadingReasonID />\r\n" +
                "<d:MeterReadingCategoryID />\r\n " +
                "<d:MeterReadingStatusID />\r\n " +
                "<d:SerialNumber>" + _meterSerialNumber + "</d:SerialNumber>\r\n " +
                "<d:MultipleMeterReadingReasonsFlag>false</d:MultipleMeterReadingReasonsFlag>\r\n " +
                "</m:properties>\r\n" +
                "</content>\r\n</entry>", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Diagnostics.Log.Info("WebAPIAdaniGas PostCurrentMeterReading API Response: " + response, typeof(IRestResponse));
            return response;
        }

        #endregion

        #region Print Invoice
        public IRestResponse PrintBill(string invoice)
        {
            string _userId = string.Concat("U0", SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID); //"U01000082139";
            string _password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password; //"Welcome@1";

            Diagnostics.Log.Info("WebAPIAdaniGas PrintBill API Call Start, API Name: " + PrintBillURL, typeof(XDocument));

            PrintBillURL = string.Format(PrintBillURL, invoice);
            var client = new RestClient(ServiceURL_utilities + PrintBillURL);
            var request = new RestRequest(Method.GET);
            var authenticationBytes = Encoding.ASCII.GetBytes(_userId + ":" + _password);
            client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
            IRestResponse response = client.Execute(request);
            Diagnostics.Log.Info("WebAPIAdaniGas PrintBill API Response: " + response, typeof(IRestResponse));
            return response;
        }
        #endregion

        #region Gas Consumption
        public XDocument GetConsumptionDetail(string contractno, string yrrange)
        {

            //var authenticationBytes = Encoding.ASCII.GetBytes("U01000082139" + ":" + "Welcome@12");
            Diagnostics.Log.Info("WebAPIAdaniGas GasConsumptionDetail API Call Start, API Name: " + PrintBillURL, typeof(XDocument));
            string startdate = yrrange.Split('_').FirstOrDefault() ?? string.Empty;
            string enddate = yrrange.Split('_').LastOrDefault() ?? string.Empty;

            var username = string.Concat("U0", SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID);
            var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;

            GasConsumptionPattern model = new GasConsumptionPattern();
            GasConsumptionDetailURL = string.Format(GasConsumptionDetailURL, contractno, startdate, enddate);

            var authenticationBytes = Encoding.ASCII.GetBytes(username + ":" + password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
            client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
            HttpResponseMessage response = client.GetAsync(ServiceURL_utilities + GasConsumptionDetailURL).Result;
            Diagnostics.Log.Info("WebAPIAdaniGas GasConsumptionDetail API Response: " + response, typeof(XDocument));
            if (response.IsSuccessStatusCode)
            {
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument responseXml = XDocument.Load(resultContent);
                return responseXml;
            }
            return null;


        }

        #endregion

        #region CNG Registration
        public AdaniGasOTPModel SendOtp(AdaniGasOTPModel model)
        {
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas SendOTPset API Call Start, API Name: " + ENachSendOtpSet, typeof(AdaniGasENachRegistrationModel));
                model.IsError = false;
                var authenticationBytes = Encoding.ASCII.GetBytes(eNACHSAPUserName + ":" + eNACHSAPPassword);
                IEnumerable<string> cookies = new List<string>();
                CookieContainer cookieJar = new CookieContainer();
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(eNACHSAPUserName, eNACHSAPPassword);

                var client = new RestClient(ServiceURL_5);
                client.CookieContainer = cookieJar;
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes));
                request.AddHeader("x-csrf-token", "fetch");
                IRestResponse response = client.Execute(request);
                string csrfToken = "";
                if (response.IsSuccessful)
                {
                    csrfToken = response.Headers.Where(x => x.Name == "x-csrf-token").Select(x => x.Value).FirstOrDefault().ToString();
                    if (!string.IsNullOrEmpty(csrfToken))
                    {
                        var cookieContainer = new CookieContainer();
                        var client2 = new RestClient(ServiceURL_5 + ENachSendOtpSet);
                        client.BaseUrl = client2.BaseUrl;
                        request.Method = Method.POST;
                        request.AddOrUpdateParameter("x-csrf-token", csrfToken, ParameterType.HttpHeader);
                        request.AddHeader("content-type", "application/json");
                        request.RequestFormat = DataFormat.Json;
                        var buildUri = client.BuildUri(request);
                        //request.AddHeader("authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes));
                        string EncryptedMobileNo = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.MobileNo, EnachSendOTPEncryptionIV);
                        string EncryptedBPUA = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.BPUA, EnachSendOTPEncryptionIV);
                        request.AddParameter("application/json", "{\r\n\"MobileNo\" : \"" + EncryptedMobileNo + "\",\r\n\"BPUA\" : \"" + EncryptedBPUA + "\"\r\n}", ParameterType.RequestBody);
                        IRestResponse response2 = client.Execute(request);

                        if (response2.IsSuccessful)
                        {
                            XmlDocument doc = JsonConvert.DeserializeXmlNode(response2.Content);
                            XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                            StringReader stringReader = new StringReader(incomingXml.ToString());
                            Diagnostics.Log.Info("WebAPIAdaniGas EnachSendOTP API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));

                            using (XmlReader reader = XmlReader.Create(stringReader))
                            {
                                while (reader.Read())
                                {
                                    if (reader.IsStartElement())
                                    {
                                        switch (reader.Name.ToString())
                                        {
                                            case "Msg_Flag":
                                                model.MessageFlag = reader.ReadString(); //EnachSendOTP_EncryptDecrypt.DecryptString(EnachSendOTPEncryptionKey, reader.ReadString(), EnachSendOTPEncryptionIV);
                                                break;
                                            case "Message":
                                                model.Message = reader.ReadString();
                                                break;
                                            case "BPUA":
                                                model.BPUA = EnachSendOTP_EncryptDecrypt.DecryptString(EnachSendOTPEncryptionKey, reader.ReadString(), EnachSendOTPEncryptionIV);
                                                break;
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(model.MessageFlag))
                            {
                                model.MessageFlag = EnachSendOTP_EncryptDecrypt.DecryptString(EnachSendOTPEncryptionKey, model.MessageFlag, model.BPUA);
                            }
                        }
                        else
                        {
                            model.Message = "OTP Sending Failed";
                            model.IsError = true;
                            return model;
                        }
                    }
                    else
                    {
                        model.Message = "CSRF Token Error";
                        model.IsError = true;
                        return model;
                    }
                }
                else
                {
                    model.Message = "CSRF Token Error";
                    model.IsError = true;
                    return model;
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas AdaniGas CNG send OTP API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas AdaniGas CNG send OTP API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }
        public ValidatedOTPModel ValidateOtp(string MobileNo, string OTP)
        {
            ValidatedOTPModel obj = new ValidatedOTPModel();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas EnachValidateOtp API Call Start, API Name: EnachValidateOtp", typeof(ValidatedOTPModel));
                var authenticationBytes = Encoding.ASCII.GetBytes(eNACHSAPUserName + ":" + eNACHSAPPassword);
                IEnumerable<string> cookies = new List<string>();
                CookieContainer cookieJar = new CookieContainer();
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(eNACHSAPUserName, eNACHSAPPassword);
                string partialServiceUrl = string.Format(ENACHValidateOtpSet, MobileNo, OTP);
                var client = new RestClient(ServiceURL_5 + partialServiceUrl);
                var request = new RestRequest(Method.GET);
                request.Credentials = credentials;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes));

                client.CookieContainer = cookieJar;
                IRestResponse response = client.Execute(request);
                XmlDocument doc = JsonConvert.DeserializeXmlNode(response.Content);
                XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas EnachValidateOtp API Response: " + incomingXml.ToString(), typeof(ValidatedOTPModel));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "Msg_Flag":
                                    obj.Msg_Flag = reader.ReadString();
                                    break;
                                case "Message":
                                    obj.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error: ", e, typeof(Exception));
                obj.IsError = true;
                obj.Message = e.Message;
            }
            return obj;
        }
        #endregion

        public List<PNGNetwork> GetPNGNetworkList(string group, string patnertype)
        {
            List<PNGNetwork> PNGNetworklist = new List<PNGNetwork>();
            PNGNetwork obj = new PNGNetwork();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas GetCityList API Call Start, API Name: " + QuickBillSetURL, typeof(PNGNetwork));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetPNGNetwork, group, patnertype);

                HttpResponseMessage response = client.GetAsync(ServiceURL_2 + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());

                Diagnostics.Log.Info("WebAPIAdaniGas GetCityList API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));

                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {

                                case "d:Conn_Obj_Txt":
                                    obj.name = reader.ReadString(); //"<div id='mainAddress' class='Hazira d-block'>" + reader.ReadString() + "</div>";
                                    PNGNetworklist.Add(obj);
                                    obj = new PNGNetwork();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetCityList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetCityList API Error: ", e, typeof(Exception));
            }
            return PNGNetworklist;
        }

        public AdaniGasNameTransferFetchTheQuote FetchTheQuote(string comptype, string CompCat, string Partner, string Taskcode)
        {
            //https://www.adaniportal.com:8080/sap/opu/odata/sap/ZMCF001_SRV/GetOutstandingSet('Customer Number')

            AdaniGasNameTransferFetchTheQuote obj = new AdaniGasNameTransferFetchTheQuote();
            try
            {
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(FetchQuoteURL, comptype, CompCat, Partner, Taskcode);
                HttpResponseMessage response = client.GetAsync(ServiceURL_2 + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetOutstandingAmount API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Tax":
                                    obj.Tax = reader.ReadString();
                                    break;
                                case "d:Amount":
                                    obj.Amount = reader.ReadString();
                                    break;
                                case "d:AmountWithTax":
                                    obj.AmountWithTax = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas FetchTheQuote API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas FetchTheQuote API Error: ", e, typeof(Exception));
            }
            return obj;
        }


        //Name Transfer request creation
        public afterSalesServices NameTransferRequestDataPost(string Bpno, string Category, string Comptype, string Taskcode, string Text, string Quantity)
        {
            afterSalesServices obj = new afterSalesServices();
            //https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMCF001_SRV/Create_ServReqSet(Bpno='PPco8voKpfztb5t7JgHprw%3D%3D%0A',Category='wbI6giwf5Zam4hFFTyym4Q%3D%3D%0A',Comptype='A%2FdM3ZB11M%2FpyybZfJr9hw%3D%3D%0A',Taskcode='s7QC8WXrLxduj5iuMHYS4Q%3D%3D%0A',Text='3OuoLLfVN86qZudVQPdgqg%3D%3D%0A',Quantity='PqCs5N5Su%2FPYKoadEP%2B70w%3D%3D%0A')
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas SalesServiceDataPostToSAP API Call Start, API Name: " + PostSalesServiceData, typeof(afterSalesServices));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string EncryptedBpno = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Bpno, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedCompCategory = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Category, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedComptype = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Comptype, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedTaskcode = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Taskcode, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedText = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Text, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedQuantity = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, Quantity.ToString(), EncryptionIV)).Replace("%2f", "%252F");

                string partialServiceUrl = string.Format(PostSalesServiceData, EncryptedBpno, EncryptedCompCategory, EncryptedComptype, EncryptedTaskcode, EncryptedText, EncryptedQuantity);

                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start NameTransferRequestDataPost, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas SalesServiceDataPostToSAP API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:MsgFlag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    break;

                                case "d:Complaint":
                                    obj.RequestNumber = reader.ReadString();
                                    break;

                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas SalesServiceDataPostToSAP API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas SalesServiceDataPostToSAP API Error: ", e, typeof(Exception));
                obj.IsError = true;
                obj.Message = e.Message;
            }
            return obj;
        }

        public List<HouseList> HouseNumberBySociety(string societyCode)
        {
            List<HouseList> listObj = new List<HouseList>();
            try
            {
                //https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMCF001_SRV/GetHouseNoSet?$filter=SocietyNo%20eq%20%271000419099%27
                string _apihouseRetrieveURL = string.Format(housegetsuburl, societyCode);
                var client = new RestClient(ServiceURL + _apihouseRetrieveURL);
                var request = new RestRequest(Method.GET);
                var authenticationBytes = Encoding.ASCII.GetBytes(eNACHSAPUserName + ":" + eNACHSAPPassword);
                client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
                Diagnostics.Log.Info("WebAPIAdaniGas NewConnection AreaofCityList API Response: " + response, typeof(IRestResponse));
                ////adding Other option
                //listObj.Add(new SocietyList { SocietyName = DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/OtherSocietyName", "Other"), SocietyCode = "", AddressLine1 = "", AddressLine2 = "", Street = "", PostalCode = "", Msg_Flag = "", Message = "" });
                if (!string.IsNullOrEmpty(jsonString))
                {
                    // convert JSON text contained in string json into an XML node
                    var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonString), new XmlDictionaryReaderQuotas()));
                    IEnumerable<XElement> element = xml.Descendants().Where(p => p.Name.LocalName == "item").ToList();
                    foreach (var _elem in element)
                    {
                        HouseList model = new HouseList();
                        model.HouseNumber = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "HouseNo").Value : string.Empty;
                        model.ConsumerNumber = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "CustomerAcc").Value : string.Empty;
                        model.BPNumber = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "BpNo").Value : string.Empty;
                        model.SocietyCode = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "SocietyNo").Value : string.Empty;
                        listObj.Add(model);
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas NewConnection AreaofCityList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas NewConnection AreaofCityList API Error: ", e, typeof(Exception));
                return null;
            }
            //return null;
            return listObj.OrderBy(x => x.HouseNumber).ToList();
        }

        public AdaniGasNameTransferComplaintSetModel NameTransferRequestApproveApplication(AdaniGasNameTransferComplaintSetModel model)
        {
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas Send Application Status API Call Start, API Name: " + PostNameChangedComplaintCloseSet, typeof(AdaniGasNameTransferComplaintSetModel));
                //model.IsError = false;
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                IEnumerable<string> cookies = new List<string>();
                CookieContainer cookieJar = new CookieContainer();
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(UserName, Password);

                var client = new RestClient(ServiceURL_8);
                client.CookieContainer = cookieJar;
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes));
                request.AddHeader("x-csrf-token", "fetch");
                IRestResponse response = client.Execute(request);
                string csrfToken = "";
                if (response.IsSuccessful)
                {
                    csrfToken = response.Headers.Where(x => x.Name == "x-csrf-token").Select(x => x.Value).FirstOrDefault().ToString();
                    if (!string.IsNullOrEmpty(csrfToken))
                    {
                        var cookieContainer = new CookieContainer();
                        var client2 = new RestClient(ServiceURL_8 + PostNameChangedComplaintCloseSet);
                        client.BaseUrl = client2.BaseUrl;
                        request.Method = Method.POST;//check/verify by ketan
                        request.AddOrUpdateParameter("x-csrf-token", csrfToken, ParameterType.HttpHeader);
                        request.AddHeader("content-type", "application/json");
                        request.RequestFormat = DataFormat.Json;
                        var buildUri = client.BuildUri(request);


                        string EncryptedComplaintNo = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.ComplaintNo, EnachSendOTPEncryptionIV);
                        string EncryptedApprovalStatus = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.ApprovalStatus, EnachSendOTPEncryptionIV);
                        string EncryptedMsgFlag = "";/*EnachSendOTP_EncryptDecrypt.EncryptString(EncryptionKeyServiceURL_8, model.MsgFlag, EncryptionIVServiceURL_8);*/
                        string EncryptedMessage = "";/*EnachSendOTP_EncryptDecrypt.EncryptString(EncryptionKeyServiceURL_8, model.Message, EncryptionIVServiceURL_8);*/
                        string EncryptedBPUA = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.BPUA, EnachSendOTPEncryptionIV);
                        string EncryptedQuantityRejectComment = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.RejectComment, EnachSendOTPEncryptionIV);

                        request.AddParameter("application/json", "{\r\n\"ComplaintNo\" : \"" + EncryptedComplaintNo + "\",\r\n\"ApprovalStatus\" : \"" + EncryptedApprovalStatus + "\",\r\n\"MsgFlag\" : \"" + EncryptedMsgFlag + "\",\r\n\"Message\" : \"" + EncryptedMessage + "\",\r\n\"BPUA\" : \"" + EncryptedBPUA + "\",\r\n\"RejectComment\" : \"" + EncryptedQuantityRejectComment + "\"\r\n}", ParameterType.RequestBody);

                        IRestResponse response2 = client.Execute(request);

                        if (response2.IsSuccessful)
                        {
                            XmlDocument doc = JsonConvert.DeserializeXmlNode(response2.Content);
                            XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                            StringReader stringReader = new StringReader(incomingXml.ToString());
                            Diagnostics.Log.Info("WebAPIAdaniGas NameTransferRequestApproveApplication API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));

                            using (XmlReader reader = XmlReader.Create(stringReader))
                            {
                                while (reader.Read())
                                {
                                    if (reader.IsStartElement())
                                    {
                                        switch (reader.Name.ToString())
                                        {
                                            case "ComplaintNo":
                                                model.ComplaintNo = reader.ReadString();
                                                break;

                                            case "ApprovalStatus":
                                                model.ApprovalStatus = reader.ReadString();
                                                break;

                                            case "MsgFlag":
                                                model.MsgFlag = reader.ReadString();
                                                break;

                                            case "Message":
                                                model.Message = reader.ReadString();
                                                break;

                                            case "BPUA":
                                                model.BPUA = EnachSendOTP_EncryptDecrypt.DecryptString(EnachSendOTPEncryptionKey, reader.ReadString(), EnachSendOTPEncryptionIV);
                                                break;

                                            case "RejectComment":
                                                model.RejectComment = reader.ReadString();
                                                break;
                                        }
                                    }
                                }
                            }
                            //if (!string.IsNullOrEmpty(model.MsgFlag)) //check/verify by ketan
                            //{
                            //    model.MsgFlag = EnachSendOTP_EncryptDecrypt.DecryptString(EnachSendOTPEncryptionKey, model.MsgFlag, model.BPUA);
                            //}
                        }
                        else
                        {
                            model.Message = "Sending Failed Approve status";
                            //model.IsError = true;
                            return model;
                        }
                    }
                    else
                    {
                        model.Message = "CSRF Token Error";
                        //model.IsError = true;
                        return model;
                    }
                }
                else
                {
                    model.Message = "CSRF Token Error";
                    //model.IsError = true;
                    return model;
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas NameTransferRequestApproveApplication API Error, ErrorMessage: " + e.Message, typeof(Exception));
                //model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public AdaniGasNameTransferUpdateName AdaniGasNameTransferUpdateNames(AdaniGasNameTransferUpdateName model)
        {
            //= new AdaniGasNameTransferUpdateName();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas AdaniGasNameTransferUpdateNames API Call Start, API Name: " + NameChangeUpdateNameSet, typeof(afterSalesServices));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");

                string partialServiceUrl = string.Format(NameChangeUpdateNameSet, model.CustomerId, model.Name_First, model.Name_Middle, model.Name_Last);

                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start NameTransferRequestDataPost, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas AdaniGasNameTransferUpdateNames API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:ComplaintNo":
                                    model.CustomerId = reader.ReadString();
                                    break;
                                case "d:Name_First":
                                    model.Name_First = reader.ReadString();
                                    break;

                                case "d:Name_Middle":
                                    model.Name_Middle = reader.ReadString();
                                    break;
                                case "d:Name_Last":
                                    model.Name_Last = reader.ReadString();
                                    break;
                                case "d:Message":
                                    model.Message = reader.ReadString();
                                    break;

                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas AdaniGasNameTransferUpdateName API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas AdaniGasNameTransferUpdateName API Error: ", e, typeof(Exception));
            }


            return model;
        }

        public PayOnline ValidateCustomerById(string customerID)
        {
            PayOnline obj = new PayOnline();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas QuickPay API Call Start, API Name: " + QuickBillSetURL, typeof(PayOnline));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(QuickBillSetURL, customerID);

                HttpResponseMessage response = client.GetAsync(ServiceURL_2 + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas QuickPay API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Name":
                                    obj.CustomerName = reader.ReadString();
                                    break;
                                case "d:Mobile_No":
                                    obj.Mobile = reader.ReadString();
                                    break;
                                case "d:Email_ID":
                                    obj.Email = reader.ReadString();
                                    break;
                                case "d:Bill_No":
                                    obj.Bill_No = reader.ReadString();
                                    break;
                                case "d:Bill_Date":
                                    obj.Bill_Date = reader.ReadString();
                                    break;
                                case "d:Due_Date":
                                    obj.Due_Date = reader.ReadString();
                                    break;
                                case "d:Amount":
                                    obj.Amount = System.Convert.ToDouble(reader.ReadString());
                                    break;
                                case "d:Current_Outstanding_Amount":
                                    obj.Current_Outstanding_Amount = reader.ReadString();
                                    break;
                                case "d:Partner_Type":
                                    obj.Partner_Type = reader.ReadString();
                                    break;
                                case "d:Execution_time":
                                    obj.Execution_time = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error: ", e, typeof(Exception));
                obj.IsError = true;
                obj.Message = e.Message;
            }
            return obj;
        }

        public PayOnline ValidateCustomerByMeterNumber(string meterNumber)
        {
            PayOnline obj = new PayOnline();
            try
            {
                //https://www.adaniportal.com:8080/sap/opu/odata/sap/ZMCF001_SRV/GetCABPDetailsSet('Meter Number')
                Diagnostics.Log.Info("WebAPIAdaniGas ValidateCustomerByMeterNumber API Call Start, API Name: " + GetDetailsByMeterNumSetURL, typeof(PayOnline));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetDetailsByMeterNumSetURL, meterNumber);

                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas ValidateCustomerByMeterNumber API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:ContractAcc":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error: ", e, typeof(Exception));
                obj.IsError = true;
                obj.Message = e.Message;
            }
            return obj;
        }

        public string GetOutstandingAmount(string customerID)
        {
            //https://www.adaniportal.com:8080/sap/opu/odata/sap/ZMCF001_SRV/GetOutstandingSet('Customer Number')
            string outstandingAmount = "0";
            try
            {
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(OutstandingAmountURL, customerID);

                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetOutstandingAmount API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                //case "d:CustNo":
                                //    obj.CustomerID = reader.ReadString();
                                //    break;
                                case "d:TotalAmt":
                                    outstandingAmount = reader.ReadString();
                                    break;
                                    //case "d:Message":
                                    //    obj.Mobile = reader.ReadString();
                                    //    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas QuickPay API Error: ", e, typeof(Exception));
            }
            return outstandingAmount;

        }

        public ManageMobileAdaniGas NameTransferGetMobileNumbersList(string customerID)
        {
            ManageMobileAdaniGas model = new ManageMobileAdaniGas();
            MobileEntry obj = new MobileEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas GetMobileNumbersList API Call Start, API Name: " + GetMobileNumbersListURL, typeof(ManageMobileAdaniGas));
                //var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                //var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetMobileNumbersListURL, customerID);
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetMobileNumbersList API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Mobile_No":
                                    obj.MobileNo = reader.ReadString();
                                    break;
                                case "d:SMS_Enabled":
                                    obj.Ischecked = string.IsNullOrEmpty(reader.ReadString()) ? false : true;
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.MobileNumbersList.Add(obj);
                                    obj = new MobileEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetMobileNumbersList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetMobileNumbersList API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageMobileAdaniGas NameTransferRegisterMobileNumber(string customerID, string mobileNumber)
        {
            ManageMobileAdaniGas model = new ManageMobileAdaniGas();
            //var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
            //var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
            MobileEntry obj = new MobileEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas RegisterMobileNumber API Call Start, API Name: " + RegisterMobileNumberURL, typeof(ManageMobileAdaniGas));

                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV).Replace("%2f", "%252F"));
                string EncryptedmobileNumber = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, mobileNumber, EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(RegisterMobileNumberURL, EncryptedcustomerID, EncryptedmobileNumber);
                //Addedd by Meenakshi --End--

                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());

                Diagnostics.Log.Info("WebAPIAdaniGas RegisterMobileNumber API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Mobile_No":
                                    obj.MobileNo = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.MobileNumbersList.Add(obj);
                                    obj = new MobileEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterMobileNumber API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterMobileNumber API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageMobileAdaniGas NameTransferModifyMobileNumber(string customerID, string oldMobileNumber, string newMobileNumber)
        {
            ManageMobileAdaniGas model = new ManageMobileAdaniGas();
            MobileEntry obj = new MobileEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas ModifyMobileNumber API Call Start, API Name: " + ModifyMobileNumberURL, typeof(ManageMobileAdaniGas));
                //var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                //var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedoldMobileNumber = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, oldMobileNumber, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptednewMobileNumber = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, newMobileNumber, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedUpdate_Flag = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, "U", EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(ModifyMobileNumberURL, EncryptedcustomerID, EncryptedoldMobileNumber, EncryptednewMobileNumber, EncryptedUpdate_Flag);
                //Addedd by Meenakshi --End--
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas ModifyMobileNumber API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Mobile_No":
                                    obj.MobileNo = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.MobileNumbersList.Add(obj);
                                    obj = new MobileEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas ModifyMobileNumber API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas ModifyMobileNumber API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageMobileAdaniGas NameTransferDeleteMobileNumber(string customerID, string mobileNumber)
        {
            ManageMobileAdaniGas model = new ManageMobileAdaniGas();
            MobileEntry obj = new MobileEntry();
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas DeleteMobileNumber API Call Start, API Name: " + DeleteMobileNumberURL, typeof(ManageMobileAdaniGas));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedmobileNumber = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, mobileNumber, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedDelete_Flag = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, "D", EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(DeleteMobileNumberURL, EncryptedcustomerID, EncryptedmobileNumber, EncryptedDelete_Flag);
                //Addedd by Meenakshi --End--
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas DeleteMobileNumber API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Mobile_No":
                                    obj.MobileNo = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.MobileNumbersList.Add(obj);
                                    obj = new MobileEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas DeleteMobileNumber API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas DeleteMobileNumber API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public AdaniGasNameTransferUploadDocument NameTransferApplicationDocumentUpload(AdaniGasNameTransferUploadDocument model)
        {
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas Send Application Status API Call Start, API Name: " + NameChangeDocumentUploadSet, typeof(AdaniGasNameTransferComplaintSetModel));
                //model.IsError = false;
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                IEnumerable<string> cookies = new List<string>();
                CookieContainer cookieJar = new CookieContainer();
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(UserName, Password);

                var client = new RestClient(ServiceURL_9);
                client.CookieContainer = cookieJar;
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes));
                request.AddHeader("x-csrf-token", "fetch");
                IRestResponse response = client.Execute(request);
                string csrfToken = "";
                if (response.IsSuccessful)
                {
                    csrfToken = response.Headers.Where(x => x.Name == "x-csrf-token").Select(x => x.Value).FirstOrDefault().ToString();
                    if (!string.IsNullOrEmpty(csrfToken))
                    {
                        var cookieContainer = new CookieContainer();
                        var client2 = new RestClient(ServiceURL_9 + NameChangeDocumentUploadSet);
                        client.BaseUrl = client2.BaseUrl;
                        request.Method = Method.POST;//check/verify by ketan
                        request.AddOrUpdateParameter("x-csrf-token", csrfToken, ParameterType.HttpHeader);
                        request.AddHeader("content-type", "application/json");
                        request.RequestFormat = DataFormat.Json;
                        var buildUri = client.BuildUri(request);


                        //string EncryptedDocName = EnachSendOTP_EncryptDecrypt.EncryptString(EncryptionKey, model.DocName, EncryptionIVServiceURL_8);
                        //string EncryptedRequestNumber = EnachSendOTP_EncryptDecrypt.EncryptString(EncryptionKey, model.RequestNumber, EncryptionIVServiceURL_8);
                        //string EncryptedMsgFlag = "";/*EnachSendOTP_EncryptDecrypt.EncryptString(EncryptionKeyServiceURL_8, model.MsgFlag, EncryptionIVServiceURL_8);*/
                        //string EncryptedMessage = "";/*EnachSendOTP_EncryptDecrypt.EncryptString(EncryptionKeyServiceURL_8, model.Message, EncryptionIVServiceURL_8);*/
                        //string EncryptedDocument = EnachSendOTP_EncryptDecrypt.EncryptString(EncryptionKey, model.Document, EncryptionIVServiceURL_8);

                        request.AddParameter("application/json", "{\r\n\"DocName\" : \"" + model.DocName + "\",\r\n\"Inquiry\" : \"" + model.RequestNumber + "\",\r\n\"Document\" : \"" + model.Document + "\"\r\n}", ParameterType.RequestBody);

                        IRestResponse response2 = client.Execute(request);

                        if (response2.IsSuccessful)
                        {
                            XmlDocument doc = JsonConvert.DeserializeXmlNode(response2.Content);
                            XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                            StringReader stringReader = new StringReader(incomingXml.ToString());
                            Diagnostics.Log.Info("WebAPIAdaniGas NameTransferRequestApproveApplication API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));

                            using (XmlReader reader = XmlReader.Create(stringReader))
                            {
                                while (reader.Read())
                                {
                                    if (reader.IsStartElement())
                                    {
                                        switch (reader.Name.ToString())
                                        {
                                            case "DocName":
                                                model.DocName = reader.ReadString();
                                                break;

                                            case "Document":
                                                model.Document = reader.ReadString();
                                                break;

                                            case "Inquiry":
                                                model.RequestNumber = reader.ReadString();
                                                break;

                                            case "MsgFlag":
                                                model.MsgFlag = reader.ReadString();
                                                break;

                                            case "Message":
                                                model.Message = reader.ReadString();
                                                break;

                                        }
                                    }
                                }
                            }
                            //if (!string.IsNullOrEmpty(model.MsgFlag))
                            //{
                            //    model.MsgFlag = EnachSendOTP_EncryptDecrypt.DecryptString(EnachSendOTPEncryptionKey, model.MsgFlag, model.RequestNumber);
                            //}
                        }
                        else
                        {
                            model.Message = "Document Uploding Failed";
                            //model.IsError = true;
                            return model;
                        }
                    }
                    else
                    {
                        model.Message = "CSRF Token Error";
                        //model.IsError = true;
                        return model;
                    }
                }
                else
                {
                    model.Message = "CSRF Token Error";
                    //model.IsError = true;
                    return model;
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas NameTransferApplicationDocumentUpload API Error, ErrorMessage: " + e.Message, typeof(Exception));
                // model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageEmailsAdaniGas NameTransferRegisterEmail(string customerID, string emailID)
        {
            ManageEmailsAdaniGas model = new ManageEmailsAdaniGas();
            EmailEntry obj = new EmailEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas RegisterEmail API Call Start, API Name: " + RegisterEmailURL, typeof(ManageEmailsAdaniGas));

                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Ketan --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedemailID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, emailID, EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(RegisterEmailURL, EncryptedcustomerID, EncryptedemailID);
                //Addedd by Ketan --End--
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, typeof(ManageEmailsAdaniGas));
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas RegisterEmail API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Email_ID":
                                    obj.EmailId = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.EmailsList.Add(obj);
                                    obj = new EmailEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas RegisterEmail API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Sitecore.Diagnostics.Log.Error("WebAPIAdaniGas RegisterEmail API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageEmailsAdaniGas NameTransferModifyEmail(string customerID, string oldEmailID, string newEmailID)
        {
            ManageEmailsAdaniGas model = new ManageEmailsAdaniGas();
            EmailEntry obj = new EmailEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas ModifyEmail API Call Start, API Name: " + ModifyEmailURL, this);

                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedoldEmailID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, oldEmailID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptednewEmailID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, newEmailID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedUpdate_Flag = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, "U", EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(ModifyEmailURL, EncryptedcustomerID, EncryptedoldEmailID, EncryptednewEmailID, EncryptedUpdate_Flag);
                //Addedd by Meenakshi --End--
                //    string partialServiceUrl = string.Format(ModifyEmailURL, customerID, oldEmailID, newEmailID);
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas ModifyEmail API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Email_ID":
                                    obj.EmailId = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.EmailsList.Add(obj);
                                    obj = new EmailEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas ModifyEmail API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas ModifyEmail API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageEmailsAdaniGas NameTransferGetEmailList(string customerID)
        {
            ManageEmailsAdaniGas model = new ManageEmailsAdaniGas();
            EmailEntry obj = new EmailEntry();
            try
            {
                //var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                //var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;

                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API Name: " + GetEmailListURL, typeof(ManageEmailsAdaniGas));
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                string partialServiceUrl = string.Format(GetEmailListURL, customerID);
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, typeof(ManageEmailsAdaniGas));
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;
                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetEmailList API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Email_ID":
                                    obj.EmailId = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.EmailsList.Add(obj);
                                    obj = new EmailEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetEmailList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetEmailList API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public ManageEmailsAdaniGas NameTransferDeleteEmail(string customerID, string emailID)
        {
            ManageEmailsAdaniGas model = new ManageEmailsAdaniGas();
            EmailEntry obj = new EmailEntry();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas DeleteEmail API Call Start, API Name: " + DeleteEmailURL, typeof(ManageEmailsAdaniGas));
                //var username = "U0" + SessionHelper.UserSession.AdaniGasUserSessionContext.CustomerID;
                //var password = SessionHelper.UserSession.AdaniGasUserSessionContext.Password;
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                            "User-Agent",
                                            "Application/Website");
                //Addedd by Meenakshi --start--
                string EncryptedcustomerID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, customerID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedemailID = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, emailID, EncryptionIV)).Replace("%2f", "%252F");
                string EncryptedDelete_Flag = HttpUtility.UrlEncode(AES_EncryptDecrypt.EncryptString(EncryptionKey, "D", EncryptionIV)).Replace("%2f", "%252F");
                string partialServiceUrl = string.Format(DeleteEmailURL, EncryptedcustomerID, EncryptedemailID, EncryptedDelete_Flag);
                //Addedd by Meenakshi --End--
                Diagnostics.Log.Info("WebAPIAdaniGas API Call Start, API URL: " + ServiceURL + partialServiceUrl, this);
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas DeleteEmail API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:Cust_No":
                                    obj.CustomerID = reader.ReadString();
                                    break;
                                case "d:Email_ID":
                                    obj.EmailId = reader.ReadString();
                                    break;
                                case "d:Msg_Flag":
                                    obj.MessageFlag = reader.ReadString();
                                    break;
                                case "d:Message":
                                    obj.Message = reader.ReadString();
                                    model.EmailsList.Add(obj);
                                    obj = new EmailEntry();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas DeleteEmail API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas DeleteEmail API Error: ", e, typeof(Exception));
                model.IsError = true;
                model.Message = e.Message;
            }
            return model;
        }

        public List<CheckBoxes> CityLists(string PartnerTypeCode)
        {
            List<CheckBoxes> listObj = new List<CheckBoxes>();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas NewConnection CityList API Call Start, API Name: " + RSA_MappingSet, typeof(RestClient));
                string _apiAreaRetrieveURL = string.Format(RSA_MappingSet, PartnerTypeCode);
                var client = new RestClient(ServiceURL_5 + _apiAreaRetrieveURL);
                var request = new RestRequest(Method.GET);
                var authenticationBytes = Encoding.ASCII.GetBytes(eNACHSAPUserName + ":" + eNACHSAPPassword);
                client.AddDefaultHeader("Authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes) + "");
                IRestResponse response = client.Execute(request);
                var jsonString = response.Content;
                Diagnostics.Log.Info("WebAPIAdaniGas NewConnection CityList API Response: " + response, typeof(IRestResponse));
                if (!string.IsNullOrEmpty(jsonString))
                {
                    // convert JSON text contained in string json into an XML node
                    var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonString), new XmlDictionaryReaderQuotas()));
                    IEnumerable<XElement> element = xml.Descendants().Where(p => p.Name.LocalName == "item").ToList();
                    foreach (var _elem in element)
                    {
                        string drpVal = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "Werks").Value : string.Empty;
                        string drpText = _elem != null ? _elem.Descendants().FirstOrDefault(p => p.Name.LocalName == "RegioareaDec").Value : string.Empty;
                        listObj.Add(new CheckBoxes { Text = drpText, Value = drpVal });
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas NewConnection CityList API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas NewConnection CityList API Error: ", e, typeof(Exception));
                return null;
            }
            return listObj.OrderBy(x => x.Text).ToList();
        }

        public AdaniGasNameTransferComplaintSetModel NameTransferRequestComments(AdaniGasNameTransferComplaintSetModel model)
        {
            try
            {

                Diagnostics.Log.Info("WebAPIAdaniGas Send Application Status API Call Start, API Name: " + PostCommentComplaintTextUpdateSetUpdate, typeof(AdaniGasNameTransferComplaintSetModel));
                //model.IsError = false;
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                IEnumerable<string> cookies = new List<string>();
                CookieContainer cookieJar = new CookieContainer();
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(UserName, Password);

                var client = new RestClient(ServiceURL_8);
                client.CookieContainer = cookieJar;
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Basic " + System.Convert.ToBase64String(authenticationBytes));
                request.AddHeader("x-csrf-token", "fetch");
                IRestResponse response = client.Execute(request);
                string csrfToken = "";
                if (response.IsSuccessful)
                {
                    csrfToken = response.Headers.Where(x => x.Name == "x-csrf-token").Select(x => x.Value).FirstOrDefault().ToString();
                    if (!string.IsNullOrEmpty(csrfToken))
                    {
                        var cookieContainer = new CookieContainer();
                        var client2 = new RestClient(ServiceURL_8 + PostCommentComplaintTextUpdateSetUpdate);
                        client.BaseUrl = client2.BaseUrl;
                        request.Method = Method.POST;
                        request.AddOrUpdateParameter("x-csrf-token", csrfToken, ParameterType.HttpHeader);
                        request.AddHeader("content-type", "application/json");
                        request.RequestFormat = DataFormat.Json;
                        var buildUri = client.BuildUri(request);


                        string EncryptedComplaintNo = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.ComplaintNo, EnachSendOTPEncryptionIV);
                        string EncryptedMsgFlag = "";
                        string EncryptedMessage = "";
                        string EncryptedBPUA = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.BPUA, EnachSendOTPEncryptionIV);
                        string EncryptedQuantityRejectComment = EnachSendOTP_EncryptDecrypt.EncryptString(EnachSendOTPEncryptionKey, model.RejectComment, EnachSendOTPEncryptionIV);

                        request.AddParameter("application/json", "{\r\n\"ComplaintNo\" : \"" + EncryptedComplaintNo + "\",\r\n\"MsgFlag\" : \"" + EncryptedMsgFlag + "\",\r\n\"Message\" : \"" + EncryptedMessage + "\",\r\n\"RejectComment\" : \"" + EncryptedQuantityRejectComment + "\"\r\n}", ParameterType.RequestBody);

                        IRestResponse response2 = client.Execute(request);

                        if (response2.IsSuccessful)
                        {
                            XmlDocument doc = JsonConvert.DeserializeXmlNode(response2.Content);
                            XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                            StringReader stringReader = new StringReader(incomingXml.ToString());
                            Diagnostics.Log.Info("WebAPIAdaniGas NameTransferRequestApproveApplication API Response: " + incomingXml.ToString(), typeof(HttpResponseMessage));

                            using (XmlReader reader = XmlReader.Create(stringReader))
                            {
                                while (reader.Read())
                                {
                                    if (reader.IsStartElement())
                                    {
                                        switch (reader.Name.ToString())
                                        {
                                            case "ComplaintNo":
                                                model.ComplaintNo = reader.ReadString();
                                                break;

                                            case "MsgFlag":
                                                model.MsgFlag = reader.ReadString();
                                                break;

                                            case "Message":
                                                model.Message = reader.ReadString();
                                                break;

                                            case "BPUA":
                                                model.BPUA = EnachSendOTP_EncryptDecrypt.DecryptString(EnachSendOTPEncryptionKey, reader.ReadString(), EnachSendOTPEncryptionIV);
                                                break;

                                            case "RejectComment":
                                                model.RejectComment = reader.ReadString();
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            model.Message = "Sending Failed Approve status";
                            return model;
                        }
                    }
                    else
                    {
                        model.Message = "CSRF Token Error";
                        return model;
                    }
                }
                else
                {
                    model.Message = "CSRF Token Error";
                    return model;
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas NameTransferCommentAdditionalDetailsApplication API Error, ErrorMessage: " + e.Message, typeof(Exception));
                model.Message = e.Message;
            }
            return model;
        }
    }
}


