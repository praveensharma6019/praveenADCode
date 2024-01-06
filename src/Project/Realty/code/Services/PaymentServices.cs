using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Realty.Website.Model;

namespace Sitecore.Realty.Website.Services
{
    [Service(typeof(IPaymentServices))]
    public class PaymentServices : IPaymentServices
    {
        public void StorePaymentRequest(PropertyCollection model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    model.OrderId = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                    
                    string amount = model.PaymentAmount;

                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database Order Id in AdaniRealty BookNow : - " + model.OrderId, this);
                        using (RealtyBookNowFormDataContext dbcontext = new RealtyBookNowFormDataContext())
                        {
                            RealtyBookNowForm objdata = new RealtyBookNowForm();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            objdata.Remarks = model.Remarks;
                            objdata.FirstName = model.FirstName ?? string.Empty;
                            objdata.LastName = model.LastName ?? string.Empty;
                            objdata.OrderId = model.OrderId;
                            objdata.AadharNumber = model.AadharNumber ?? string.Empty;
                            objdata.PanNumber = model.PanNumber ?? string.Empty;
                            objdata.Country = model.Country ?? string.Empty;
                            objdata.State = model.State ?? string.Empty;
                            objdata.City = model.City ?? string.Empty;
                            objdata.PaymentAmount = amount ?? string.Empty;
                            objdata.FormName = model.FormName ?? string.Empty;
                            objdata.PageInfo = model.PageInfo ?? string.Empty;
                            objdata.PropertyName = model.PropertyName ?? string.Empty;
                            objdata.PropertyCode = model.PropertyCode ?? string.Empty;
                            objdata.PropertyType = model.PropertyType ?? string.Empty;
                            objdata.ParentProject = model.ParentProject ?? string.Empty;
                            objdata.RequestTime = System.Convert.ToDateTime(DateTime.UtcNow);
                            objdata.Checksumkey = string.Empty;
                            objdata.Status = model.PaymentStatus ?? Constants.PaymentResponse.Initiated;
                            objdata.CurrencyType = model.CurrencyType ?? string.Empty;
                            objdata.GatewayType = model.PaymentGateway ?? string.Empty;
                            objdata.msg = model.msg ?? string.Empty;
                            objdata.PaymentType = model.PaymentType ?? string.Empty;
                            objdata.EmailAddress = model.EmailAddress ?? string.Empty;
                            objdata.Mobile = model.Mobile ?? string.Empty;
                            objdata.CreatedDate = System.DateTime.Now;
                            objdata.CreatedBy = model.FirstName + " " + model.LastName;
                            dbcontext.RealtyBookNowForms.InsertOnSubmit(objdata);
                            dbcontext.SubmitChanges();

                        }
                    }
                    catch (System.Exception ex)
                    {
                        // Note : Log the message on any failure to sitecore log
                        Sitecore.Diagnostics.Log.Error("Error at Updating BooknowDetail form " + model.FirstName + model.LastName + ": " + ex.Message, this);
                    }


                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at StorePaymentRequest :" + ex.Message, this);
            }


        }

        public void StorePaymentResponse(PropertyCollection model)
        {
            try
            {
                if (model != null)
                {
                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in Realty BookNow Database :" + model.TransactionId, this);
                        //Update response in DB
                        using (RealtyBookNowFormDataContext dbcontext = new RealtyBookNowFormDataContext())
                        {
                            RealtyBookNowForm ctx = dbcontext.RealtyBookNowForms.Where(x => x.FirstName + " " + x.LastName == model.FullName && x.OrderId == model.OrderId).FirstOrDefault();
                            if (ctx != null)
                            {
                                
                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database Update payment Order Id : - " + model.OrderId, this);
                                // Update response in Database
                                ctx.TransactionId = model.TransactionId ?? string.Empty;
                                ctx.Status = model.PaymentStatus ?? string.Empty;
                                ctx.ResponseCode = model.Responsecode.ToString() ?? string.Empty;
                                ctx.msg = model.msg.ToString() ?? string.Empty;
                                ctx.ResponseTime = System.Convert.ToDateTime(DateTime.UtcNow);
                                ctx.PaymentRef = model.PaymentRef.ToString() ?? string.Empty;
                                ctx.ResponseMessage = model.ResponseMessage ?? string.Empty;
                                ctx.PaymentMode = model.PaymentMode ?? string.Empty;
                                ctx.CurrencyType = model.CurrencyType ?? string.Empty;
                                ctx.Modified_Date = System.DateTime.Now;
                                ctx.ModifiedBy = model.FullName;
                                dbcontext.SubmitChanges();

                                Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in Database update successfull :" + model.TransactionId, this);
                            }
                            else
                            {
                                //kiosk payment
                                try
                                {
                                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentRequest Store request in Database kiosk payment Order Id : - " + model.OrderId, this);
                                    RealtyBookNowForm objdata = new RealtyBookNowForm();
                                    Guid newid = Guid.NewGuid();
                                    objdata.Id = newid;
                                    objdata.FirstName = model.FullName ?? string.Empty;
                                    objdata.OrderId = model.OrderId;
                                    objdata.PaymentAmount = model.PaymentAmount ?? string.Empty;
                                    objdata.RequestTime = model.TransactionDate == null ? DateTime.Now : DateTime.ParseExact(model.TransactionDate, "dd-MM-yyyy HH:mm:ss", null); // System.Convert.ToDateTime(model.TransactionDate);
                                    objdata.Checksumkey = string.Empty;
                                    objdata.CurrencyType = model.CurrencyType ?? string.Empty;
                                    objdata.msg = model.msg ?? string.Empty;
                                    objdata.PaymentType = model.PaymentType ?? string.Empty;
                                    objdata.EmailAddress = model.EmailAddress ?? string.Empty;
                                    objdata.Mobile = model.Mobile ?? string.Empty;
                                    objdata.CreatedDate = System.DateTime.Now;
                                    objdata.CreatedBy = model.FullName;
                                    objdata.TransactionId = model.TransactionId;
                                    objdata.ResponseCode = model.Responsecode;
                                    objdata.Status = model.PaymentStatus;
                                    objdata.PaymentMode = model.PaymentMode;
                                    dbcontext.RealtyBookNowForms.InsertOnSubmit(objdata);
                                    dbcontext.SubmitChanges();

                                    Sitecore.Diagnostics.Log.Info("Method - StorePaymentResponse Store response in Database Add BookNow payment successfull :" + model.TransactionId, this);
                                }
                                catch (System.Exception ex)
                                {
                                    // Note : Log the message on any failure to sitecore log
                                    Sitecore.Diagnostics.Log.Error("Error at Adding BookNow payment Item " + model.FullName + ": " + ex.Message, this);
                                }
                            }

                            
                            
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

        public string BillDeskTransactionRequestAPIRequestPost(PropertyCollection Model)
        {
            Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));
            string paymentURL = string.Empty;
            string MerchantID = string.Empty;
            string SecurityID = string.Empty;
            string callbackURL = string.Empty;
            string ChecksumKey = string.Empty;
            string requestMsg = string.Empty;
            string curreny = string.Empty;
            if (Model.ParentProject.ToUpper() == "AEMPL" )
            {
                paymentURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFieldsAEMPL.BDSK_Request_URL].Value;
                MerchantID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFieldsAEMPL.BDSK_Merchant_ID].Value;
                SecurityID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFieldsAEMPL.BDSK_SECURITY_ID].Value;
                callbackURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFieldsAEMPL.BDSK_Resp_URL_B2B].Value;
                ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFieldsAEMPL.BDSK_ChecksumKey].Value;
                requestMsg = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFieldsAEMPL.BDSK_Req_Msg].Value;
                curreny = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFieldsAEMPL.BDSK_CURRENCY_TYPE].Value;
            }
            else
            {
                paymentURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Request_URL].Value;
                MerchantID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Merchant_ID].Value;
                SecurityID = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_SECURITY_ID].Value;
                callbackURL = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Resp_URL_B2B].Value;
                ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_ChecksumKey].Value;
                requestMsg = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Req_Msg].Value;
                curreny = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_CURRENCY_TYPE].Value;
            }        


            string[] hashVarsSeq = requestMsg.Split('|');
            string hash_string = string.Empty;
            string hash1 = string.Empty;

            string amount = Model.PaymentAmount;
            string paymenttype = Model.PaymentType;
            string ConsumerName = string.Empty;


            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "MerchantID")
                {
                    hash_string = MerchantID;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "UniqueTxnID")
                {
                    hash_string = hash_string + Model.OrderId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txtTxnAmount")
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
                else if (hash_var == "txtAdditionalInfo1")
                {
                    hash_string = hash_string + Model.PropertyCode;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txtAdditionalInfo2")
                {
                    ConsumerName = Model.FirstName + " " + Model.LastName;
                    hash_string = hash_string + ConsumerName;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txtAdditionalInfo3")
                {
                    hash_string = hash_string + Model.Mobile;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txtAdditionalInfo4")
                {
                    hash_string = hash_string + Model.EmailAddress;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txtAdditionalInfo5")
                {
                    hash_string = hash_string + Model.AadharNumber ?? "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txtAdditionalInfo6")
                {
                    hash_string = hash_string + Model.PanNumber ?? "NA";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txtAdditionalInfo7")
                {
                    Model.ParentProject = string.IsNullOrEmpty(Model.ParentProject) ? "NA" : Model.ParentProject;
                    hash_string = hash_string + Model.ParentProject;
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
            outputHTML += "<center><h1>" + DictionaryPhraseRepository.Current.Get("/Realty/BookNow/Redirect Msg", "Please do not refresh this page...") + "</h1></center>";
            outputHTML += "<form method='post' action='" + paymentURL + "' name='f1'>";
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
    }
}