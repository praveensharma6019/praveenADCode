using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace InstamojoAPI
{
    public class InstamojoImplementation : Instamojo
    {
        private volatile static Instamojo uniqueInstance; // for singleton design pattern
        static readonly object _locker = new object(); // for multithreading purpose

        /** The client id. */
        private String clientId;

        /** The client secret. */
        private String clientSecret;

        /** The api endpoint. */
        private String apiEndpoint;

        /** The auth endpoint. */
        private String authEndpoint;

        /** The access token. */
        private static AccessTokenResponse accessToken;

        private static long tokenCreationTime;

        private InstamojoImplementation() { }

        private InstamojoImplementation(String clientId, String clientSecret, String apiEndpoint, String authEndpoint) 
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.apiEndpoint = apiEndpoint;
            this.authEndpoint = authEndpoint;
        }

        /**
     * Gets api.
     *
     * @param clientId     the client id
     * @param clientSecret the client secret
     * @return the api
     * @throws IOException the io exception
     */
        public static Instamojo getApi(String clientId, String clientSecret, String apiEndpoint, String authEndpoint, string grant_type) 
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new BaseException("Please enter ClientId");
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new BaseException("Please enter clientSecret");
            }
            if (string.IsNullOrEmpty(apiEndpoint))
            {
                throw new BaseException("Please enter apiEndpoint");
            }
            if (string.IsNullOrEmpty(authEndpoint))
            {
                throw new BaseException("Please enter authEndpoint");

            }
            return getInstamojo(clientId, clientSecret, apiEndpoint, authEndpoint, grant_type);
        }

        //public static Instamojo getApi(String clientId, String clientSecret) 
        //{
        //    return getApi(clientId, clientSecret, InstamojoConstants.INSTAMOJO_API_ENDPOINT, InstamojoConstants.INSTAMOJO_AUTH_ENDPOINT);
        //}

        private static Instamojo getInstamojo(String clientId, String clientSecret, String apiEndpoint, String authEndpoint, String grant_type)
        {
            try
            {
                Sitecore.Diagnostics.Log.Error("Get Item From  getInstamojo called", "");

                if (uniqueInstance == null)
                {
                    Sitecore.Diagnostics.Log.Error("Get Item From  getInstamojo uniqueInstance null", "");
                    lock (_locker)
                    {
                        if (uniqueInstance == null)
                        {
                            uniqueInstance = new InstamojoImplementation(clientId, clientSecret, apiEndpoint, authEndpoint);
                            Sitecore.Diagnostics.Log.Error("Get Item From  getInstamojo uniqueInstance null 1", "");
                            accessToken = getAccessToken(clientId, clientSecret, authEndpoint, grant_type);
                        }
                    }
                }
                else
                {

                    Sitecore.Diagnostics.Log.Error("Get Item From  getInstamojo uniqueInstance not null", "");
                    //uniqueInstance = new InstamojoImplementation(clientId, clientSecret, apiEndpoint, authEndpoint);
                    //accessToken = getAccessToken(clientId, clientSecret, authEndpoint, grant_type);
                    if (isTokenExpired())
                    {
                        lock (_locker)
                        {
                            if (isTokenExpired())
                            {
                                accessToken = getAccessToken(clientId, clientSecret, authEndpoint, grant_type);
                            }
                        }
                    }
                }
                return uniqueInstance;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Get Item From  getInstamojo exception " + ex.ToString(), "");
                return uniqueInstance;
            }
           
        }

        private static bool isTokenExpired()
        {
            long durationInSeconds = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - tokenCreationTime;
            if (durationInSeconds >=  accessToken.expires_in)
            {
                return true;
            }

            return false;
        }

        private static AccessTokenResponse getAccessToken(string clientId, string clientSecret, string authEndpoint, string grant_type)
		{
            AccessTokenResponse objPaymentRequestDetailsResponse;
            try
            {
                Sitecore.Diagnostics.Log.Error("Get Item From  getInstamojo getAccessToken  1", "");
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["client_id"] = clientId;
                    values["client_secret"] = clientSecret;
                    values["grant_type"] = grant_type;

                    Sitecore.Diagnostics.Log.Error("Get Item From  getInstamojo getAccessToken  1" + clientId + clientSecret + " " + grant_type + " " + authEndpoint, "");

                    var response = client.UploadValues(authEndpoint, values); //"https://test.instamojo.com/oauth2/token/", values);
                    var responseString = Encoding.Default.GetString(response);
                    //var responseString = @"{""access_token"": ""7wvWW6J-6c0LyiiTfoKyavF8MTapHjfAjGzjPUFTUQg.ltGfbeMSjJPaUn5ZXM1nDkcYlxg5nJEmdI_Rs6P4xQI"", ""expires_in"": 35999, ""scope"": ""convenience-fee:read payment-requests:write payments:refund users:read users:write payments:fulfil payment-requests:read payouts:read gateway-orders:read gateway-orders:write payments:read"", ""token_type"": ""bearer""}";

                    Sitecore.Diagnostics.Log.Error("Get Item From  getInstamojo getAccessToken  2" + responseString, "");
                    objPaymentRequestDetailsResponse = JsonDeserialize<AccessTokenResponse>(responseString);

                    Sitecore.Diagnostics.Log.Error("Get Item From  getInstamojo getAccessToken  3" + objPaymentRequestDetailsResponse, "");

                    if (string.IsNullOrEmpty(objPaymentRequestDetailsResponse.access_token))
                    {
                        throw new BaseException("Could not get the access token due to " + objPaymentRequestDetailsResponse.error);
                    }
                }
            }
            catch (IOException ex)
            {
                Sitecore.Diagnostics.Log.Error("Get Item From getAccessToken  " + ex.ToString(), "");
                throw new IOException(ex.Message);
            }
            catch (WebException ex)
            {
                Sitecore.Diagnostics.Log.Error("Get Item From getAccessToken  WebException " + ex.ToString(), "");
                objPaymentRequestDetailsResponse = JsonDeserialize<AccessTokenResponse>(ex.Message);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Get Item From getAccessToken Exception " + ex.ToString(), "");
                throw new Exception(ex.Message);
            }

            tokenCreationTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            return objPaymentRequestDetailsResponse;
        }

        /**
        * @param string Path
        * @return string adds the Path to endpoint with.
        */
        private string build_api_call_url(string Path)
        {
            if (Path.LastIndexOf("/?") == -1 && Path.LastIndexOf("?") == -1 && Path.LastIndexOf("/") == -1)
            {
                apiEndpoint = apiEndpoint + Path + "/";
                return apiEndpoint;
            }
            return apiEndpoint + Path;
        }

        /**
         * @param method: string httpWebRequest Method
         * @param path: string path for building API URL
         * @param PostData: object 
         * @return JSON string
        */
        private string api_call(string method, string path, object PostData = null)
        {
            string URL = build_api_call_url(path);

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.ContentType = "application/json";

            // Build Headers
            myHttpWebRequest.Headers["Authorization"] = accessToken.token_type + " " + accessToken.access_token;

            myHttpWebRequest.Method = method;

            if (method == "POST")
            {
				JavaScriptSerializer ser = new JavaScriptSerializer();
				ser.RegisterConverters(new JavaScriptConverter[] { new JavaScriptConverters.NullPropertiesConverter() });
				string strJSONData = ser.Serialize(PostData);
                using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(strJSONData);
                    streamWriter.Flush();
                }
            }
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(myHttpWebResponse.GetResponseStream()))
            {
                var streamRead = streamReader.ReadToEnd().Trim();
                return streamRead;
            }
        }

        private static T JsonDeserialize<T>(string jsonString)
        {
			return new JavaScriptSerializer().Deserialize<T>(jsonString);
        }

        private static string showErrorMessage(string errorMessage)
        {
            return errorMessage;
        }

        /********************************   Request a Payment Order  *******************************************/

        #region Creating a payment Order
        /**
        * @param objPaymentRequest: PaymentOrder object .
        * @return objPaymentCreateResponse: CreatePaymentOrderResponse object.
        */
        public CreatePaymentOrderResponse createNewPaymentRequest(PaymentOrder objPaymentRequest)
        {
            if (objPaymentRequest == null)
            {
				throw new ArgumentNullException(typeof(PaymentOrder).Name, "PaymentOrder Object Can not be Null ");
            }

            bool isInValid= objPaymentRequest.validate();
            if (isInValid)
            {
                throw new InvalidPaymentOrderException();
            }

            try
            {
                string stream = api_call("POST", "gateway/orders/", objPaymentRequest);
                CreatePaymentOrderResponse objPaymentCreateResponse = JsonDeserialize<CreatePaymentOrderResponse>(stream);

                return objPaymentCreateResponse;
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex.InnerException);
            }
            catch (BaseException ex)
            {
                throw new BaseException(ex.Message, ex.InnerException);
            }
            catch (UriFormatException ex)
            {
                throw new UriFormatException(ex.Message, ex.InnerException);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse err = ex.Response as HttpWebResponse;
                    if (err != null)
                    {
                        string htmlResponse = new StreamReader(err.GetResponseStream()).ReadToEnd();
						throw new InvalidPaymentOrderException(htmlResponse);
                    }
                }
                throw new WebException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        #endregion

        # region  Get All your Payment Orders List
        /**
        * @param strPaymentRequestId: string id as provided by paymentRequestCreate, paymentRequestsList, webhook or redirect.
        * @return objPaymentRequestStatus: PaymentCreateResponse object.
        */
        public PaymentOrderListResponse getPaymentOrderList(PaymentOrderListRequest objPaymentOrderListRequest)
        {
            if (objPaymentOrderListRequest == null)
            {
				throw new ArgumentNullException(typeof(PaymentOrderListRequest).Name, "PaymentOrderListRequest Object Can not be Null");
            }
            string queryString = "", stream = "";

            if (string.IsNullOrEmpty(objPaymentOrderListRequest.id))
            {
                queryString = "id=" + objPaymentOrderListRequest.id;
            }
            if (string.IsNullOrEmpty(objPaymentOrderListRequest.transaction_id))
            {
                string transQuery = "transaction_id=" + objPaymentOrderListRequest.transaction_id;
                queryString += string.IsNullOrEmpty(queryString) ? transQuery : ("&" + transQuery);
            }
            if (objPaymentOrderListRequest.page != 0)
            {
                string pageQuery = "page=" + objPaymentOrderListRequest.page;
                queryString += string.IsNullOrEmpty(queryString) ? pageQuery : ("&" + pageQuery);
            }
            if (objPaymentOrderListRequest.limit != 0)
            {
                string limitQuery = "limit=" + objPaymentOrderListRequest.limit;
                queryString += string.IsNullOrEmpty(queryString) ? limitQuery : ("&" + limitQuery);
            }
            try
            {
                if (queryString != "")
                {
                    stream = api_call("GET", "gateway/orders/?" + queryString, objPaymentOrderListRequest);
                }
                else
                {
                    stream = api_call("GET", "gateway/orders/", null);
                }

                PaymentOrderListResponse objPaymentRequestStatusResponse = JsonDeserialize<PaymentOrderListResponse>(stream);
                return objPaymentRequestStatusResponse;
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex.InnerException);
            }
            catch (BaseException ex)
            {
                throw new BaseException(ex.Message, ex.InnerException);
            }
            catch (UriFormatException ex)
            {
                throw new UriFormatException(ex.Message, ex.InnerException);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse err = ex.Response as HttpWebResponse;
                    if (err != null)
                    {
                        string htmlResponse = new StreamReader(err.GetResponseStream()).ReadToEnd();
                        throw new WebException(err.StatusDescription + " " + htmlResponse);
                    }
                }
                throw new WebException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        #endregion

        # region  Get details of this payment order by Order Id
        public PaymentOrderDetailsResponse getPaymentOrderDetails(string strOrderId)
        {
            if (string.IsNullOrEmpty(strOrderId))
            {
				throw new ArgumentNullException(typeof(string).Name, "Order Id Can not be Null or Empty");
            }
            try
            {
                string stream = api_call("GET", "gateway/orders/id:" + strOrderId + "/", null);
                PaymentOrderDetailsResponse objPaymentRequestDetailsResponse = JsonDeserialize<PaymentOrderDetailsResponse>(stream);
                return objPaymentRequestDetailsResponse;
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex.InnerException);
            }
            catch (BaseException ex)
            {
                throw new BaseException(ex.Message, ex.InnerException);
            }
            catch (UriFormatException ex)
            {
                throw new UriFormatException(ex.Message, ex.InnerException);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse err = ex.Response as HttpWebResponse;
                    if (err != null)
                    {
                        string htmlResponse = new StreamReader(err.GetResponseStream()).ReadToEnd();
                        throw new WebException(err.StatusDescription + " " + htmlResponse);
                    }
                }
                throw new WebException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        #endregion

        # region  Get details of this payment order by Transaction Id
        public PaymentOrderDetailsResponse getPaymentOrderDetailsByTransactionId(string strTransactionId)
        {
            if (string.IsNullOrEmpty(strTransactionId))
            {
				throw new ArgumentNullException(typeof(string).Name, "Transaction Id Can not be Null or Empty");
            }
            try
            {
                string stream = api_call("GET", "gateway/orders/transaction_id:" + strTransactionId + "/", null);
                PaymentOrderDetailsResponse objPaymentRequestDetailsResponse = JsonDeserialize<PaymentOrderDetailsResponse>(stream);
                return objPaymentRequestDetailsResponse;
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex.InnerException);
            }
            catch (BaseException ex)
            {
                throw new BaseException(ex.Message, ex.InnerException);
            }
            catch (UriFormatException ex)
            {
                throw new UriFormatException(ex.Message, ex.InnerException);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse err = ex.Response as HttpWebResponse;
                    if (err != null)
                    {
                        string htmlResponse = new StreamReader(err.GetResponseStream()).ReadToEnd();
                        throw new WebException(err.StatusDescription + " " + htmlResponse);
                    }
                }
                throw new WebException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


        #region Creating a Refund
        /**
        * @param objPaymentRequest: PaymentOrder object .
        * @return objPaymentCreateResponse: CreatePaymentOrderResponse object.
        */
        public CreateRefundResponce createNewRefundRequest(Refund objCreateRefund)
        {            
            if (objCreateRefund == null)
            {
                throw new ArgumentException("Refund object can not be null.");
            }
            if (objCreateRefund.payment_id == null)
            {
				throw new ArgumentNullException(typeof(Refund).Name, "PaymentId cannot be null ");
            }
            bool isInValid = objCreateRefund.validate();
            if (isInValid)
            {
                throw new InvalidPaymentOrderException();
            }
            try
            {
                string stream = api_call("POST", "payments/" + objCreateRefund.payment_id + "/refund/", objCreateRefund);
                CreateRefundResponce objRefundResponse = JsonDeserialize<CreateRefundResponce>(stream);
                return objRefundResponse;
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex.InnerException);
            }
            catch (BaseException ex)
            {
                throw new BaseException(ex.Message, ex.InnerException);
            }
            catch (UriFormatException ex)
            {
                throw new UriFormatException(ex.Message, ex.InnerException);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse err = ex.Response as HttpWebResponse;
                    if (err != null)
                    {
                        string htmlResponse = new StreamReader(err.GetResponseStream()).ReadToEnd();
                        throw new WebException(err.StatusDescription + " " + htmlResponse);
                    }
                }
                throw new WebException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        #endregion

    }
}
