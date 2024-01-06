using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using InstamojoAPI;
using InstaMojoIntegration.Models;
using Sitecore.Marathon.Website.Models;
using Sitecore.Project.Marathon;

namespace Sitecore.Marathon.Website
{
    public class PaymentService
    {
        public ResultPayment Payment(RegistrationModel model)
        {
            Sitecore.Diagnostics.Log.Error("Payment Function Call", "");
            Sitecore.Data.Items.Item itemInfo = null;
            try
            {
                Sitecore.Diagnostics.Log.Error("Get Item From Web Database", "");

                try
                {
                    Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                    itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.Id.ToString()));
                }
                catch (Exception Ex)
                {
                    Sitecore.Diagnostics.Log.Error("Get Item From Web Database Failed", Ex.Message);
                }
                var objDB_Insta_client_id = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_client_id].Value.ToString();
                var objDB_Insta_client_secret = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_client_secret].Value.ToString();
                var objDB_Insta_Endpoint = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Endpoint].Value.ToString();
                var objDB_Insta_Auth_Endpoint = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Auth_Endpoint].Value.ToString();
                var objDB_grant_type = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.grant_type].Value.ToString();
                var objDB_redirect_url = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Redirect_Url].Value.ToString();

                Sitecore.Diagnostics.Log.Error("Get Payment Field From Sitecore Template", "");

                //Logs and DB to be maintained.
                ResultPayment result = new ResultPayment();

                ////below 5 parameters will be stroed in template
                //string Insta_client_id = "test_TPubaCQZDnQ1S7tbL8RGbAZXsRwAVhUFD8D",// "tmLkZZ0zV41nJwhayBGBOI4m4I7bH55qpUBdEXGS",
                //           Insta_client_secret = "test_XEQbtNPK10pKeVL6xC5qGa9WZPltqoIAs7Qrck4NtDUioeElm1PIF17ufcMOKYtX3jzRcKVOlde9pvLyssVEffPNmDy7i99LqW0gBXdHulYBskkBGaPnbOWmJqe",// "IDejdccGqKaFlGav9bntKULvMZ0g7twVFolC9gdrh9peMS0megSFr7iDpWwWIDgFUc3W5SlX99fKnhxsoy6ipdAv9JeQwebmOU6VRvOEQnNMWwZnWglYmDGrfgKRheXs",
                //           Insta_Endpoint = "https://test.instamojo.com/v2/",// InstamojoConstants.INSTAMOJO_API_ENDPOINT,
                //           Insta_Auth_Endpoint = "https://test.instamojo.com/oauth2/token/",
                //           grant_type = "client_credentials"; // InstamojoConstants.INSTAMOJO_AUTH_ENDPOINT;

                Instamojo objClass = InstamojoImplementation.getApi(objDB_Insta_client_id, objDB_Insta_client_secret, objDB_Insta_Endpoint, objDB_Insta_Auth_Endpoint, objDB_grant_type);


                Sitecore.Diagnostics.Log.Error("Insta Mojo Call Success", objClass);


                //New payment request will be created and saved in DB with Pending status, email, name etc will be asked from user.
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj Start" + " ", "");
                PaymentOrder objPaymentRequest = new PaymentOrder();
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj Created" + " " + objPaymentRequest.ToString(), this);

                //Required POST parameters
                objPaymentRequest.name = model.FirstName + " " + model.LastName;
                objPaymentRequest.email = model.Email;
                objPaymentRequest.phone = model.ContactNumber;
                objPaymentRequest.amount = System.Convert.ToDouble(model.FinalAmount);
                objPaymentRequest.transaction_id = model.OrderId; // Unique Id to be provided
                objPaymentRequest.redirect_url = objDB_redirect_url.ToString();

                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj name" + " " + objPaymentRequest.name.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj email" + " " + objPaymentRequest.email.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj phone" + " " + objPaymentRequest.phone.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj amount" + " " + objPaymentRequest.amount.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj transaction_id" + " " + objPaymentRequest.transaction_id.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj redirect_url" + " " + objPaymentRequest.redirect_url.ToString(), this);

                Sitecore.Diagnostics.Log.Info("Paymnet Request Order" + " ", objPaymentRequest.validate().ToString());
                if (objPaymentRequest.validate())
                {
                    result.IsSuccess = false;
                    if (objPaymentRequest.emailInvalid)
                    {
                        result.Message = "Email is not valid";
                    }
                    if (objPaymentRequest.nameInvalid)
                    {
                        result.Message = "Name is not valid";
                    }
                    if (objPaymentRequest.phoneInvalid)
                    {
                        result.Message = "Phone is not valid";
                    }
                    if (objPaymentRequest.amountInvalid)
                    {
                        result.Message = "Amount is not valid";
                    }
                    if (objPaymentRequest.currencyInvalid)
                    {
                        result.Message = "Currency is not valid";
                    }
                    if (objPaymentRequest.transactionIdInvalid)
                    {
                        result.Message = "Transaction Id is not valid";
                    }
                    if (objPaymentRequest.redirectUrlInvalid)
                    {
                        result.Message = "Redirect Url Id is not valid";
                    }
                    if (objPaymentRequest.webhookUrlInvalid)
                    {
                        result.Message = "Webhook URL is not valid";
                    }

                }
                else
                {
                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Start CreatePaymentOrderResponse" + " ", "");
                        CreatePaymentOrderResponse objPaymentResponse = objClass.createNewPaymentRequest(objPaymentRequest);
                        result.IsSuccess = true;
                        result.Message = objPaymentResponse.payment_options.payment_url;
                        Sitecore.Diagnostics.Log.Info("End CreatePaymentOrderResponse" + " ", objPaymentResponse.payment_options.payment_url.ToString());

                    }
                    catch (ArgumentNullException ex)
                    {
                        result.Message = ex.Message;
                    }
                    catch (WebException ex)
                    {
                        result.Message = ex.Message;
                    }
                    catch (IOException ex)
                    {
                        result.Message = ex.Message;
                    }
                    catch (InvalidPaymentOrderException ex)
                    {
                        if (!ex.IsWebhookValid())
                        {
                            result.Message = "Webhook is invalid";
                        }

                        if (!ex.IsCurrencyValid())
                        {
                            result.Message = "Currency is Invalid";
                        }

                        if (!ex.IsTransactionIDValid())
                        {
                            result.Message = "Transaction ID is Inavlid";
                        }
                    }
                    catch (ConnectionException ex)
                    {
                        result.Message = ex.Message;
                        Sitecore.Diagnostics.Log.Error("Payment", ex.Message);
                    }
                    catch (BaseException ex)
                    {
                        result.Message = ex.Message;
                        Sitecore.Diagnostics.Log.Error("Payment", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        result.Message = "Error:" + ex.Message;
                        Sitecore.Diagnostics.Log.Error("Payment" + ex.Message, this);
                    }
                }
                //result will be saved in DB table
                //URL
                //Any error
                Sitecore.Diagnostics.Log.Error("Payment Function Call Success" + result, this);
                return result;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Payment Error" + ex.Message, this);
            }
            Sitecore.Diagnostics.Log.Error("Return null payment", "");
            return null;
        }
        public ResultPayment Donation(Donate model)
        {
            Sitecore.Diagnostics.Log.Error("Payment Function Call", "");
            Sitecore.Data.Items.Item itemInfo = null;
            try
            {
                Sitecore.Diagnostics.Log.Error("Get Item From Web Database", "");

                try
                {
                    Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                    itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.DonationId.ToString()));
                }
                catch (Exception Ex)
                {
                    Sitecore.Diagnostics.Log.Error("Get Item From Web Database Failed", Ex.Message);
                }
                var objDB_Insta_client_id = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_client_id].Value.ToString();
                var objDB_Insta_client_secret = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_client_secret].Value.ToString();
                var objDB_Insta_Endpoint = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Endpoint].Value.ToString();
                var objDB_Insta_Auth_Endpoint = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Auth_Endpoint].Value.ToString();
                var objDB_grant_type = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.grant_type].Value.ToString();
                var objDB_redirect_url = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Redirect_Url].Value.ToString();

                Sitecore.Diagnostics.Log.Error("Get Payment Field From Sitecore Template", "");

                //Logs and DB to be maintained.
                ResultPayment result = new ResultPayment();

                ////below 5 parameters will be stroed in template
                //string Insta_client_id = "test_TPubaCQZDnQ1S7tbL8RGbAZXsRwAVhUFD8D",// "tmLkZZ0zV41nJwhayBGBOI4m4I7bH55qpUBdEXGS",
                //           Insta_client_secret = "test_XEQbtNPK10pKeVL6xC5qGa9WZPltqoIAs7Qrck4NtDUioeElm1PIF17ufcMOKYtX3jzRcKVOlde9pvLyssVEffPNmDy7i99LqW0gBXdHulYBskkBGaPnbOWmJqe",// "IDejdccGqKaFlGav9bntKULvMZ0g7twVFolC9gdrh9peMS0megSFr7iDpWwWIDgFUc3W5SlX99fKnhxsoy6ipdAv9JeQwebmOU6VRvOEQnNMWwZnWglYmDGrfgKRheXs",
                //           Insta_Endpoint = "https://test.instamojo.com/v2/",// InstamojoConstants.INSTAMOJO_API_ENDPOINT,
                //           Insta_Auth_Endpoint = "https://test.instamojo.com/oauth2/token/",
                //           grant_type = "client_credentials"; // InstamojoConstants.INSTAMOJO_AUTH_ENDPOINT;

                Instamojo objClass = InstamojoImplementation.getApi(objDB_Insta_client_id, objDB_Insta_client_secret, objDB_Insta_Endpoint, objDB_Insta_Auth_Endpoint, objDB_grant_type);


                Sitecore.Diagnostics.Log.Error("Insta Mojo Call Success", objClass);


                //New payment request will be created and saved in DB with Pending status, email, name etc will be asked from user.
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj Start" + " ", "");
                PaymentOrder objPaymentRequest = new PaymentOrder();
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj Created" + " " + objPaymentRequest.ToString(), this);

                //Required POST parameters
                objPaymentRequest.name = model.Name;
                objPaymentRequest.email = model.EmailId;
                objPaymentRequest.phone = "1231231231";
                objPaymentRequest.amount = System.Convert.ToDouble(model.Amount);
                objPaymentRequest.transaction_id = model.OrderId; // Unique Id to be provided
                objPaymentRequest.redirect_url = objDB_redirect_url.ToString();

                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj name" + " " + objPaymentRequest.name.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj email" + " " + objPaymentRequest.email.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj phone" + " " + objPaymentRequest.phone.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj amount" + " " + objPaymentRequest.amount.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj transaction_id" + " " + objPaymentRequest.transaction_id.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj redirect_url" + " " + objPaymentRequest.redirect_url.ToString(), this);

                Sitecore.Diagnostics.Log.Info("Paymnet Request Order" + " ", objPaymentRequest.validate().ToString());
                if (objPaymentRequest.validate())
                {
                    result.IsSuccess = false;
                    if (objPaymentRequest.emailInvalid)
                    {
                        result.Message = "Email is not valid";
                    }
                    if (objPaymentRequest.nameInvalid)
                    {
                        result.Message = "Name is not valid";
                    }
                    if (objPaymentRequest.phoneInvalid)
                    {
                        result.Message = "Phone is not valid";
                    }
                    if (objPaymentRequest.amountInvalid)
                    {
                        result.Message = "Amount is not valid";
                    }
                    if (objPaymentRequest.currencyInvalid)
                    {
                        result.Message = "Currency is not valid";
                    }
                    if (objPaymentRequest.transactionIdInvalid)
                    {
                        result.Message = "Transaction Id is not valid";
                    }
                    if (objPaymentRequest.redirectUrlInvalid)
                    {
                        result.Message = "Redirect Url Id is not valid";
                    }
                    if (objPaymentRequest.webhookUrlInvalid)
                    {
                        result.Message = "Webhook URL is not valid";
                    }

                }
                else
                {
                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Start CreatePaymentOrderResponse" + " ", "");
                        CreatePaymentOrderResponse objPaymentResponse = objClass.createNewPaymentRequest(objPaymentRequest);
                        result.IsSuccess = true;
                        result.Message = objPaymentResponse.payment_options.payment_url;
                        Sitecore.Diagnostics.Log.Info("End CreatePaymentOrderResponse" + " ", objPaymentResponse.payment_options.payment_url.ToString());

                    }
                    catch (ArgumentNullException ex)
                    {
                        result.Message = ex.Message;
                    }
                    catch (WebException ex)
                    {
                        result.Message = ex.Message;
                    }
                    catch (IOException ex)
                    {
                        result.Message = ex.Message;
                    }
                    catch (InvalidPaymentOrderException ex)
                    {
                        if (!ex.IsWebhookValid())
                        {
                            result.Message = "Webhook is invalid";
                        }

                        if (!ex.IsCurrencyValid())
                        {
                            result.Message = "Currency is Invalid";
                        }

                        if (!ex.IsTransactionIDValid())
                        {
                            result.Message = "Transaction ID is Inavlid";
                        }
                    }
                    catch (ConnectionException ex)
                    {
                        result.Message = ex.Message;
                        Sitecore.Diagnostics.Log.Error("Payment", ex.Message);
                    }
                    catch (BaseException ex)
                    {
                        result.Message = ex.Message;
                        Sitecore.Diagnostics.Log.Error("Payment", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        result.Message = "Error:" + ex.Message;
                        Sitecore.Diagnostics.Log.Error("Payment" + ex.Message, this);
                    }
                }
                //result will be saved in DB table
                //URL
                //Any error
                Sitecore.Diagnostics.Log.Error("Payment Function Call Success" + result, this);
                return result;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Payment Error" + ex.Message, this);
            }
            Sitecore.Diagnostics.Log.Error("Return null payment", "");
            return null;
        }



        public ResultPayment GroupPayment(AhmedabadMarathonRegistration model, string OrderId, decimal finalAmount)
        {
            Sitecore.Diagnostics.Log.Error("Payment Function Call", "");
            Sitecore.Data.Items.Item itemInfo = null;
            try
            {
                Sitecore.Diagnostics.Log.Error("Get Item From Web Database", "");

                try
                {
                    Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                    itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.Id.ToString()));
                }
                catch (Exception Ex)
                {
                    Sitecore.Diagnostics.Log.Error("Get Item From Web Database Failed", Ex.Message);
                }
                var objDB_Insta_client_id = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_client_id].Value.ToString();
                var objDB_Insta_client_secret = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_client_secret].Value.ToString();
                var objDB_Insta_Endpoint = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Endpoint].Value.ToString();
                var objDB_Insta_Auth_Endpoint = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Auth_Endpoint].Value.ToString();
                var objDB_grant_type = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.grant_type].Value.ToString();
                var objDB_redirect_url = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Redirect_Url].Value.ToString();

                Sitecore.Diagnostics.Log.Error("Get Payment Field From Sitecore Template", "");

                ResultPayment result = new ResultPayment();

 

                Instamojo objClass = InstamojoImplementation.getApi(objDB_Insta_client_id, objDB_Insta_client_secret, objDB_Insta_Endpoint, objDB_Insta_Auth_Endpoint, objDB_grant_type);


                Sitecore.Diagnostics.Log.Error("Insta Mojo Call Success", objClass);


                //New payment request will be created and saved in DB with Pending status, email, name etc will be asked from user.
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj Start" + " ", "");
                PaymentOrder objPaymentRequest = new PaymentOrder();
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj Created" + " " + objPaymentRequest.ToString(), this);

                //Required POST parameters
                objPaymentRequest.name = model.FirstName + " " + model.LastName;
                objPaymentRequest.email = model.Email;
                objPaymentRequest.phone = model.ContactNumber;
                objPaymentRequest.amount = System.Convert.ToDouble(finalAmount);
                objPaymentRequest.transaction_id = OrderId; // Unique Id to be provided
                objPaymentRequest.redirect_url = objDB_redirect_url.ToString();

                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj name" + " " + objPaymentRequest.name.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj email" + " " + objPaymentRequest.email.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj phone" + " " + objPaymentRequest.phone.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj amount" + " " + objPaymentRequest.amount.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj transaction_id" + " " + objPaymentRequest.transaction_id.ToString(), this);
                Sitecore.Diagnostics.Log.Info("objPaymentRequest Obj redirect_url" + " " + objPaymentRequest.redirect_url.ToString(), this);

                Sitecore.Diagnostics.Log.Info("Paymnet Request Order" + " ", objPaymentRequest.validate().ToString());
                if (objPaymentRequest.validate())
                {
                    result.IsSuccess = false;
                    if (objPaymentRequest.emailInvalid)
                    {
                        result.Message = "Email is not valid";
                    }
                    if (objPaymentRequest.nameInvalid)
                    {
                        result.Message = "Name is not valid";
                    }
                    if (objPaymentRequest.phoneInvalid)
                    {
                        result.Message = "Phone is not valid";
                    }
                    if (objPaymentRequest.amountInvalid)
                    {
                        result.Message = "Amount is not valid";
                    }
                    if (objPaymentRequest.currencyInvalid)
                    {
                        result.Message = "Currency is not valid";
                    }
                    if (objPaymentRequest.transactionIdInvalid)
                    {
                        result.Message = "Transaction Id is not valid";
                    }
                    if (objPaymentRequest.redirectUrlInvalid)
                    {
                        result.Message = "Redirect Url Id is not valid";
                    }
                    if (objPaymentRequest.webhookUrlInvalid)
                    {
                        result.Message = "Webhook URL is not valid";
                    }

                }
                else
                {
                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Start CreatePaymentOrderResponse" + " ", "");
                        CreatePaymentOrderResponse objPaymentResponse = objClass.createNewPaymentRequest(objPaymentRequest);
                        result.IsSuccess = true;
                        result.Message = objPaymentResponse.payment_options.payment_url;
                        Sitecore.Diagnostics.Log.Info("End CreatePaymentOrderResponse" + " ", objPaymentResponse.payment_options.payment_url.ToString());

                    }
                    catch (ArgumentNullException ex)
                    {
                        result.Message = ex.Message;
                    }
                    catch (WebException ex)
                    {
                        result.Message = ex.Message;
                    }
                    catch (IOException ex)
                    {
                        result.Message = ex.Message;
                    }
                    catch (InvalidPaymentOrderException ex)
                    {
                        if (!ex.IsWebhookValid())
                        {
                            result.Message = "Webhook is invalid";
                        }

                        if (!ex.IsCurrencyValid())
                        {
                            result.Message = "Currency is Invalid";
                        }

                        if (!ex.IsTransactionIDValid())
                        {
                            result.Message = "Transaction ID is Inavlid";
                        }
                    }
                    catch (ConnectionException ex)
                    {
                        result.Message = ex.Message;
                        Sitecore.Diagnostics.Log.Error("Payment", ex.Message);
                    }
                    catch (BaseException ex)
                    {
                        result.Message = ex.Message;
                        Sitecore.Diagnostics.Log.Error("Payment", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        result.Message = "Error:" + ex.Message;
                        Sitecore.Diagnostics.Log.Error("Payment" + ex.Message, this);
                    }
                }
                Sitecore.Diagnostics.Log.Error("Payment Function Call Success" + result, this);
                return result;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Payment Error" + ex.Message, this);
            }
            Sitecore.Diagnostics.Log.Error("Return null payment", "");
            return null;
        }
    }
}