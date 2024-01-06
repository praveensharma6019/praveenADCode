using Sitecore.Affordable.Website.Models;
using Sitecore.Affordable.Website.Models.InstaMojo;
using System;
using System.IO;
using System.Net;

namespace Sitecore.Affordable.Website
{
    public class PaymentService
    {
        public PaymentService() { }
        public ResultPayment Payment(BookNow model)
        {
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.Id.ToString()));
                var objDB_Insta_client_id = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_client_id].Value.ToString();
                var objDB_Insta_client_secret = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_client_secret].Value.ToString();
                var objDB_Insta_Endpoint = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Endpoint].Value.ToString();
                var objDB_Insta_Auth_Endpoint = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Auth_Endpoint].Value.ToString();
                var objDB_grant_type = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.grant_type].Value.ToString();
                var objDB_redirect_url = itemInfo.Fields[Templates.PaymentConfiguration.Insta_Mojo.Insta_Redirect_Url].Value.ToString();
                //Logs and DB to be maintained.
                ResultPayment result = new ResultPayment();
                Instamojo objClass = InstamojoImplementation.getApi(objDB_Insta_client_id, objDB_Insta_client_secret, objDB_Insta_Endpoint, objDB_Insta_Auth_Endpoint, objDB_grant_type);
                //New payment request will be created and saved in DB with Pending status, email, name etc will be asked from user.
                PaymentOrder objPaymentRequest = new PaymentOrder();

                //Required POST parameters
                objPaymentRequest.name = model.Name;
                objPaymentRequest.email = model.Email;
                objPaymentRequest.phone = model.MoblieNo;
                objPaymentRequest.amount = model.Amount;
                objPaymentRequest.transaction_id = model.transaction_id; // (Guid.NewGuid()).ToString(); // Unique Id to be provided
                objPaymentRequest.redirect_url = objDB_redirect_url.ToString();


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
                        CreatePaymentOrderResponse objPaymentResponse = objClass.createNewPaymentRequest(objPaymentRequest);
                        result.IsSuccess = true;
                        result.Message = objPaymentResponse.payment_options.payment_url;
                        //result.Message = objDB_redirect_url;
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