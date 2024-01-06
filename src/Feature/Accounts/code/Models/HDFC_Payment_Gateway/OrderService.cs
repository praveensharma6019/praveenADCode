using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Text;
using System.Xml;

namespace Sitecore.Feature.Accounts.Models.HDFC_Payment_Gateway
{
    public class OrderService
    {
        public OrderLink GetOrderLink(PaymentRequestData request, string appId, string secretKey)
        {
            var orderLink = new OrderLink();

            var tranRequest = ConstructPayload(request, appId, secretKey);

            //====================================================================================

            /* This is Payment Gateway Test URL where merchant sends request. This is test enviornment URL, 
			production URL will be different and will be shared by Bank during production movement */

            string TranUrl = "https://securepgtest.fssnet.co.in/ipayb/servlet/PaymentInitHTTPServlet";
            System.Net.HttpWebRequest objRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(TranUrl);
            objRequest.Method = "POST";
            objRequest.ContentLength = tranRequest.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            //create a SSL connection xmlhttp formated object server-to-server
            System.IO.StreamWriter myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(tranRequest);//send data
            myWriter.Close();//closed the myWriter object

            System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();

            //receive the responce from objxmlhttp object 
            using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
            {
                var tranResponse = sr.ReadToEnd();

                string[] ErrorCheck = StringUtils.GetStringInBetween(tranResponse, "!", "!-", false, false);//This line will find Error Keyword in TranResponse	

                if (ErrorCheck[0] != "ERROR")//This block will check for Error in TranResponce
                {
                    /* Merchant MUST map (update) the Payment ID received with the merchant Track Id in his database at this place. */
                    string payid = tranResponse.Substring(0, tranResponse.IndexOf(":http"));
                    string payURL = tranResponse.Substring(tranResponse.IndexOf("http"));
                    /* here redirecting the customer browser from ME site to Payment Gateway Page with the Payment ID */
                    string tranRedirect = payURL + "?PaymentID=" + payid;

                    orderLink.IsSuccess = true;
                    orderLink.PaymentLink = tranRedirect;
                }
                else
                {
                    orderLink.IsSuccess = false;
                    orderLink.Reason = ErrorCheck[1];
                }
            }

            return orderLink;
        }
        private string ConstructPayload(PaymentRequestData request, string transportalId, string password)
        {
            /* 
			to pass Tranportal ID provided by the bank to merchant. Tranportal ID is sensitive 
			information of merchant from the bank, merchant MUST ensure that Tranportal ID is 
			never passed to customer browser by any means. Merchant MUST ensure that Tranportal
			ID is stored in secure environment & securely at merchant end. Tranportal Id is 
			referred as id. Tranportal ID for test and production will be different, please 
			contact bank for test and production Tranportal ID
			*/
            string ReqTranportalId = $"id={transportalId}&";

            /* 
			to pass Tranportal password provided by the bank to merchant. Tranportal password 
			is sensitive information of merchant from the bank, merchant MUST ensure that 
			Tranportal password is never passed to customer browser by any means. Merchant 
			MUST ensure that Tranportal password is stored in secure environment & securely 
			at merchant end. Tranportal password is referred as password. Tranportal password 
			for test and production will be different, please contact bank for test and 
			production Tranportal password 
			*/
            string ReqTranportalPassword = $"password={password}&";

            /* 
			Action Code of the transaction, this refers to type of transaction. 
			Action Code 1 stands of Purchase transaction and 
			action code 4 stands for Authorization (pre-auth). 
			Merchant should confirm from Bank action code enabled for the merchant by the bank
			*/
            string ReqAction = "action=" + "1" + "&";

            /* Transaction language, THIS MUST BE ALWAYS USA. */
            string ReqLangid = "langid=" + "USA" + "&";

            /* 
			Currency code of the transaction. By default INR i.e. 356 is configured. 
			If merchant wishes to do multiple currency code transaction, merchant 
			needs to check with bank team on the available currency code 
			*/
            string ReqCurrency = "currencycode=" + "356" + "&";

            /* Transaction Amount that will be send to payment gateway by merchant for processing
			NOTE - Merchant MUST ensure amount is sent from merchant back-end system like database
			and not from customer browser. In below sample AMT is hard-coded, merchant to pass 
			trasnaction amount here. */
            string ReqAmount = "amt=" + request.OrderAmount + "&";

            /* 
			Response URL where Payment gateway will send response once transaction 
			processing is completed Merchant MUST ensure that below points in Response URL
			1- Response URL must start with http://
			2- the Response URL SHOULD NOT have any additional parameters or query string 
			*/
            string ReqResponseUrl = $"responseURL={request.ReturnUrl}&";

            /* Error URL where Payment gateway will send response in case any issues while processing the transaction 
			Merchant MUST esure that below points in ErrorURL 
			1- error url must start with http://
			2- the error url SHOULD NOT have any additional paramteres or query string
			*/
            string ReqErrorUrl = $"errorURL={request.ErrorUrl}&";

            /* To pass the merchant track id, in below sample merchant track id is hard-coded. Merchant
			MUST pass his transaction ID (track ID) in this parameter. Track Id passed here should be 
			from merchant backend system like database and not from customer browser*/
            string ReqTrackId = "trackid=" + request.OrderId + "&";

            /*
             As per your business model, we recommend five (5) categories of information to be populated in the UDFs. 
             The details to be populated against each UDF's are as follows. 
             UDF 1 to 4 are standardized & remaining one UDF is merchant specific. 
             We shall communicate the same according to the nature of business.
 
            UDF 1 - Service Details
            UDF 2 - Email ID
            UDF 3 - Contact Number.
            UDF 4 - Billing Address
            UDF 5 - Hashing Value             
             */


            /* User De	fined Fileds as per Merchant or bank requirment. Merchant MUST ensure merchant 
			merchant is not passing junk values OR CRLF in any of the UDF. In below sample UDF values 
			are not utilized */
            string ReqUdf1 = $"udf1={request.CustomerReference}&"; // UDF1 values                                                                
            string ReqUdf2 = $"udf2={request.CustomerEmail}&"; // UDF2 values 	                                                               
            string ReqUdf3 = $"udf3={request.CustomerPhone}&"; // UDF3 values                                                             
            string ReqUdf4 = $"udf4={request.CustomerName}&"; // UDF4 values  

            /*
			ME should now do the validations on the amount value set like - 
			a) Transaction Amount should not be blank and should be only numeric
			b) Language should always be USA
			c) Action Code should not be blank
			d) UDF values should not have junk values and CRLF (line terminating parameters)Like--> [ !#$%^&*()+[]\\\';,{}|\":<>?~` ]
			*/

            /*
			==============================HASHING LOGIC CODE START==============================================
			*/

            /*Below are the fields/prametres which will use for hashing using (SHA256) hashing 
			Algorithm and need to pass same hashed valued in UDF5 filed only*/

            string strhashTID = transportalId;  //USE Tranportal ID FIELD Value FOR HASHING 
            string strhashamt = request.OrderAmount.ToString();     //USE Amount FIELD Value FOR HASHING 
            string strhashtrackid = request.OrderId;    //USE Trackid FIELD Value FOR HASHING 
            string strhashcurrency = "356";     //USE Currencycode FIELD Value FOR HASHING 
            string strhashaction = "1";         //USE Action code FIELD Value FOR HASHING 

            /*
			Create a Hashing String to Hash 
			*/
            var Strhashs = strhashTID.Trim() + strhashtrackid.Trim() + strhashamt.Trim() + strhashcurrency.Trim() + strhashaction.Trim();

            /* 
			Use GetSHA256 Function which is defined below for Hashing ,
			It will return Hashed valued of above string
			*/
            string hashString = Crypto.ComputeSha256Hash(Strhashs);

            string ReqUdf5 = $"udf5={hashString}&";    // Passed Calculated Hashed Value in UDF5 Field 

            /*
			=============================HASHING LOGIC CODE END==============================================
			*/

            /* Now merchant sets all the inputs in one string for passing to the Payment Gateway URL */
            return ReqTranportalId + ReqTranportalPassword + ReqAction + ReqLangid + ReqCurrency + ReqAmount + ReqResponseUrl + ReqErrorUrl + ReqTrackId + ReqUdf1 + ReqUdf2 + ReqUdf3 + ReqUdf4 + ReqUdf5;
        }


        //public OrderInformation GetStatus(PaymentRequestData request, string appId, string secretKey)
        //{
        //    var orderInformation = new OrderInformation
        //    {
        //        ReferenceCode = request.OrderId
        //    };

        //    string tranportalid = $"<id>{appId}</id>"; //Mandatory
        //    string password = $"<password>{secretKey}</password>"; //Mandatory
        //    string trackid = $"<trackid>{request.OrderId}</trackid>"; //Mandatory

        //    string card = ""; // "<card></card>";  //Mandatory for Action code "1" & "4"
        //    string expmonth = ""; // "<expmonth></expmonth>";   //Mandatory for Action code "1" & "4"
        //    string expyear = ""; // "<expyear></expyear>";	   //Mandatory for Action code "1" & "4"
        //    string cvv2 = ""; // "<cvv2></cvv2>";
        //    string member = $"<member>FSS</member>";		  //Mandatory
        //    string currencycode = "<currencycode>356</currencycode>";  // Mandatory
        //    string action = "<action>8</action>";  // Mandatory
        //    string amt = $"<amt>{request.OrderAmount}</amt>";  //Mandatory

        //    string transid = $"<transid>{request.OrderId}</transid>"; //Optional
        //    string udf1 = ""; // "<udf1></udf1>";
        //    string udf2 = ""; // "<udf2></udf2>";
        //    string udf3 = ""; // "<udf3></udf3>";
        //    string udf4 = ""; // "<udf4></udf4>";
        //    string udf5 = "<udf5>TrackID</udf5>";

        //    string data = tranportalid + password + card + expmonth + expyear + cvv2 + currencycode + action + amt + trackid + member + udf1 + udf2 + udf3 + udf4 + udf5 + transid;

        //    string url = "https://securepgtest.fssnet.co.in/ipayb/servlet/TranPortalXMLServlet";

        //    System.IO.StreamWriter myWriter = null;
        //    // it will open a http connection with provided url
        //    System.Net.HttpWebRequest objRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);//send data using objxmlhttp object
        //    objRequest.Method = "POST";
        //    objRequest.ContentLength = data.Length;
        //    objRequest.ContentType = "application/x-www-form-urlencoded";//to set content type
        //    myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
        //    myWriter.Write(data);
        //    myWriter.Close();

        //    System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();
        //    using System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream());

        //    var parsedResult = StringUtils.ParseXMLStream(sr);

        //    //This line will find Error Keyword in TranResponse
        //    string[] ErrorCheck = StringUtils.GetStringInBetween(parsedResult.result, "!", "!-", false, false);

        //    //This block will check for Error in TranResponce
        //    //<result>!ERROR!-GW00201-Transaction not found.</result><error_code_tag>GW00201</error_code_tag><error_service_tag>null</error_service_tag>

        //    if (ErrorCheck[0] == "ERROR")
        //    {
        //        if (parsedResult.error_code_tag == "GW00201")
        //        {
        //            orderInformation.OrderStatus = OrderStatus.NOTAVAILABLE;
        //        }
        //        else
        //        {
        //            orderInformation.OrderStatus = OrderStatus.ERROR;
        //        }
        //        orderInformation.ErrorCode = parsedResult.error_code_tag;
        //        orderInformation.Message = ErrorCheck[1];
        //    }
        //    else
        //    {
        //        orderInformation.OrderStatus = MapOrderStatus(parsedResult.result);
        //        orderInformation.PostDate = parsedResult.postdate;
        //        orderInformation.TransactionCode = parsedResult.tranid;
        //        orderInformation.PaymentCode = parsedResult.payid;
        //        orderInformation.Amount = parsedResult.amt;
        //        orderInformation.Message = $"PAYMENT STATUS:{orderInformation.OrderStatus.ToString()}";
        //    }

        //    //var logStr = System.Text.Json.JsonSerializer.Serialize(parsedResult);
        //    //Console.WriteLine(logStr);            

        //    return orderInformation;
        //}

        //public static bool IsPropertyExist(dynamic settings, string name)
        //{
        //    if (settings is ExpandoObject)
        //        return ((IDictionary<string, object>)settings).ContainsKey(name);

        //    return settings.GetType().GetProperty(name) != null;
        //}

        //private OrderStatus MapOrderStatus(string orderStatus)
        //{
        //    if (orderStatus.Contains("ERROR")) return OrderStatus.ERROR;
        //    if (orderStatus == "SUCCESS") return OrderStatus.SUCCESS;
        //    return OrderStatus.NOTAVAILABLE;
        //}

    }
}