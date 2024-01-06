using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Sitecore.AdaniHousing.Website.Models;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Sitecore.AdaniHousing.Website.Services
{
    public class AdaniHousingWebAPIServices
    {
        private string _customerID;
        private HttpClient client;
        public AdaniHousingWebAPIServices()
        {
            _customerID = Helper.UserSession.UserSessionContext == null ? string.Empty : Helper.UserSession.UserSessionContext.LoanAccountNumber;
            client = new HttpClient();
        }
        public string LMSClientId = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSClientId", "lmsapp");
        public string LMSClient_Secret = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSClient_Secret", "$2a$08$BGVXi7T5Mck1VBOleT9te.RJRTbfWZaznqnaM8rfvcVPwhNbWEF7W");
        public string LMSGrant_Type = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSServiceGrant_Type", "client_credentials");
        public string LMSServiceURL = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSServiceURL", "https://adani.finnonecloud.com:9846/finnone-integration/");
        public string LMSTokenService = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSTokenService", "oauth/token");
        public string LMSbranchId = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSbranchId", "5");
        public string LMSuserCode = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSuserCode", "lmsadmin1");
        public string LMStenantId = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMStenantId", "505");
        public string LMSrequestChannel = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSrequestChannel", "ATM");
        public string LMSprimaryCustomerOnly = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSprimaryCustomerOnly", "0");

        public string LMSgetLoans = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSgetLoans", "rest/getCustomers/restservice/customerService/getLoans");
        public string LMSget_loan_details = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSget_loan_details", "app/restservice/loanservice/get_loan_details");
        public string LMSgenerate_SOA_report = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSGenerate_SOA_Report", "app/restservice/SOAReport/generate_report");
        public string LMSGetinterestCertificate = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSGetinterestCertificate", "app/restservice/interestCertificate/generateInterestCertificate");
        public string LMSgetBalanceConfirmationLetter = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSgetBalanceConfirmationLetter", "app/rest/BalanceConfirmationReport/balanceConfirmation/generateBalanceConfirmationReport");
        public string LMSgetWelcomeLetter = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSgetWelcomeLetter", "app/rest/generateLetter/welcomeLetter/getWelcomeLetter");
        public string LMSgenerateRepaymentSchedule = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSgenerateRepaymentSchedule", "app/rest/repaySchedule/RepayamentReport/generate_repayschedule_report");
        public string LMSget_transactions = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LoanManagement/LMSget_transactions", "app/restservice/loanservice/get_transactions");

        public string FreshDeskUserName = DictionaryPhraseRepository.Current.Get("/WebAPIServices/Freshdesk/UserName", "x86iTqPwWWxKspNF67zp");
        public string FreshDeskPassword = DictionaryPhraseRepository.Current.Get("/WebAPIServices/Freshdesk/Pasword", "x");
        public string FreshDeskServiceURL = DictionaryPhraseRepository.Current.Get("/WebAPIServices/Freshdesk/FreshdeskCreateTicketURL", "https://adanifinserve.freshdesk.com/api/v2/tickets");
        public string STStokenServiceURL = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LeadManagement/STStokenServiceURL", "http://adaniuat.lms.gravithy.com/id/connect/token");
        public string STSLeadCreateServiceURL = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LeadManagement/STSLeadCreateServiceURL", "http://adaniuat.lms.gravithy.com/api/Lead/Create");
        public string STSServiceClientId = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LeadManagement/STSServiceClientId", "Linthub.Mobile");
        public string STSServiceClient_Secret = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LeadManagement/STSServiceClient_Secret", "secret");
        public string STSServiceScope = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LeadManagement/STSServiceScope", "linthub.services linthub.api.odata linthub.api.acralog");
        public string STSServiceGrant_Type = DictionaryPhraseRepository.Current.Get("/WebAPIServices/LeadManagement/STSServiceGrant_Type", "client_credentials");
        public AdaniHousingContactUsModal FreshDeskContactUsCreateTicket(AdaniHousingContactUsModal model)
        {
            try
            {
                string json = "{\"status\": 2, \"priority\": 1, \"email\":\"" + model.EmailID + "\",\"subject\":\"" + model.Subject + "\",\"description\":\"" + model.Message + "\",\"custom_fields\" : { \"cf_customer_name\" : \"" + model.Name + "\",\"cf_customer_mobile_number\":" + model.MobileNo + ",\"cf_customer_email_id\":\"" + model.EmailID + "\" } }";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FreshDeskServiceURL);
                request.ContentType = "application/json";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                // Set the ContentLength property of the WebRequest. 
                request.ContentLength = byteArray.Length;
                string authInfo = FreshDeskUserName + ":" + FreshDeskPassword;
                authInfo = System.Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                //Get the stream that holds request data by calling the GetRequestStream method. 
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream. 
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object. 
                dataStream.Close();
                try
                {
                    Console.WriteLine("Submitting Request");
                    WebResponse response = request.GetResponse();
                    // Get the stream containing content returned by the server.
                    //Send the request to the server by calling GetResponse. 
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access. 
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content. 
                    string Response = reader.ReadToEnd();
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(Response);
                    //XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                    Newtonsoft.Json.Linq.JObject jResponse = Newtonsoft.Json.Linq.JObject.Parse(Response);
                    model.TicketID = jResponse.Value<string>("id");
                    model.IsSubmittedToFreshdesk = !string.IsNullOrEmpty(model.TicketID) ? true : false;

                    //return status code
                    Console.WriteLine("Status Code: {1} {0}", ((HttpWebResponse)response).StatusCode, (int)((HttpWebResponse)response).StatusCode);
                    //return location header
                    Console.WriteLine("Location: {0}", response.Headers["Location"]);
                    //return the response 
                    Console.Out.WriteLine(Response);
                }
                catch (WebException ex)
                {
                    Log.Error("API Error Contact Us form in Freshdesk: Your request is not successful. If you are not able to debug this error properly, mail us at support@freshdesk.com with the follwing X-Request-Id", this);
                    Log.Error("X-Request-Id: " + ex.Response.Headers["X-Request-Id"], this);
                    Log.Error("Error Status Code : " + ((HttpWebResponse)ex.Response).StatusCode + (int)((HttpWebResponse)ex.Response).StatusCode, this);
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        Console.Write("Error Response Contact Us: ");
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
                return model;
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - Store Adani Housing Contact Us form in Freshdesk : :" + model.EmailID + e.Message, this);
                return model;
            }

        }
        public BecomeAPartnerModel FreshDeskBecomePartnerCreateTicket(BecomeAPartnerModel model)
        {
            try
            {
                string json = "{\"status\": 2, \"priority\": 1, \"email\":\"" + model.EmailID + "\",\"subject\":\"" + model.FormName + "\",\"description\":\"" + model.Message + "\",\"custom_fields\" : { \"cf_customer_name\" : \"" + model.Name + "\",\"cf_customer_mobile_number\":" + model.MobileNo + ",\"cf_customer_email_id\":\"" + model.EmailID + "\",\"cf_fsm_service_location\":\"" + model.Pincode + "\" } }";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FreshDeskServiceURL);
                request.ContentType = "application/json";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                // Set the ContentLength property of the WebRequest. 
                request.ContentLength = byteArray.Length;
                string authInfo = FreshDeskUserName + ":" + FreshDeskPassword;
                authInfo = System.Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                //Get the stream that holds request data by calling the GetRequestStream method. 
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream. 
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object. 
                dataStream.Close();
                try
                {
                    Console.WriteLine("Submitting Request");
                    WebResponse response = request.GetResponse();
                    // Get the stream containing content returned by the server.
                    //Send the request to the server by calling GetResponse. 
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access. 
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content. 
                    string Response = reader.ReadToEnd();
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(Response);
                    //XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                    Newtonsoft.Json.Linq.JObject jResponse = Newtonsoft.Json.Linq.JObject.Parse(Response);
                    model.TicketID = jResponse.Value<string>("id");
                    model.IsSubmittedToFreshdesk = !string.IsNullOrEmpty(model.TicketID) ? true : false;

                    //return status code
                    Console.WriteLine("Status Code: {1} {0}", ((HttpWebResponse)response).StatusCode, (int)((HttpWebResponse)response).StatusCode);
                    //return location header
                    Console.WriteLine("Location: {0}", response.Headers["Location"]);
                    //return the response 
                    Console.Out.WriteLine(Response);
                }
                catch (WebException ex)
                {
                    Log.Error("API Error Become A Partner form in Freshdesk: Your request is not successful. If you are not able to debug this error properly, mail us at support@freshdesk.com with the follwing X-Request-Id", this);
                    Log.Error("X-Request-Id: " + ex.Response.Headers["X-Request-Id"], this);
                    Log.Error("Error Status Code : " + ((HttpWebResponse)ex.Response).StatusCode + (int)((HttpWebResponse)ex.Response).StatusCode, this);
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        Console.Write("Error Response Adani Housing Become A Partner form in Freshdesk: ");
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
                return model;
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - Store Adani Housing Become A Partner form in Freshdesk : :" + model.EmailID + e.Message, this);
                return model;
            }

        }
        public string STSserviceTokenGenerate()
        {
            List<string> tokenParam = new List<string>();
            var client = new RestClient(STStokenServiceURL);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", STSServiceClientId, ParameterType.GetOrPost);
            request.AddParameter("client_secret", STSServiceClient_Secret, ParameterType.GetOrPost);
            request.AddParameter("scope", STSServiceScope, ParameterType.GetOrPost);
            request.AddParameter("grant_type", STSServiceGrant_Type, ParameterType.GetOrPost);
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                return "";
            }
        }
        public AdaniHousingApplyforLoanModel ApplyForLoanCreateEnquiry(AdaniHousingApplyforLoanModel model)
        {
            try
            {
                model.IsSubmittedToLMS = false;
                var client = new RestClient(STSLeadCreateServiceURL);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                var ServicerResponse = STSserviceTokenGenerate();
                model.IsSubmittedToLMS = false;
                if (!string.IsNullOrEmpty(ServicerResponse))
                {
                    Newtonsoft.Json.Linq.JObject jServiceResponse = Newtonsoft.Json.Linq.JObject.Parse(ServicerResponse);
                    model.AccessToken = jServiceResponse.Value<string>("access_token");
                    model.TokenType = jServiceResponse.Value<string>("token_type");
                    request.AddHeader("authorization", model.TokenType + " " + model.AccessToken);
                    request.AddHeader("content-type", "application/json");
                    request.AddParameter("application/json", "[{\r\n  \"product\": \"" + model.ProductType + "\",\r\n  \"subProduct\": null,\r\n  \"firstName\": \"" + model.FirstName + "\",\r\n  \"middleName\": null,\r\n  \"lastName\": \"" + model.LastName + "\",\r\n  \"mobile\": \"" + model.MobileNo + "\",\r\n  \"email\": \"" + model.EmailID + "\",\r\n  \"pan\": null,\r\n  \"addressLine1\": null,\r\n  \"addressLine2\": null,\r\n  \"postalCode\": \"" + model.PinCode + "\",\r\n  \"city\": null,\r\n  \"state\": null,\r\n  \"loanAmount\":null,\r\n  \"tenure\": null,\r\n  \"leadSource\": \"Website\"\r\n}]\r\n", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful)
                    {

                        Newtonsoft.Json.Linq.JArray jarr = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
                        //var responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<LeadMgmtResponseParam>(response.Content);
                        //var responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);
                        //if (responseObj.error != null) throw new Exception(responseObj.error_description);
                        foreach (Newtonsoft.Json.Linq.JObject content in jarr.Children<JObject>())
                        {
                            foreach (Newtonsoft.Json.Linq.JProperty prop in content.Properties())
                            {
                                if (prop.Name == "Key")
                                {
                                    model.LMS_RequestKey = (string)prop.Value;
                                }
                                if (prop.Name == "Value")
                                {
                                    model.LMS_Response = (string)prop.Value;
                                }
                            }
                        }
                        model.IsSubmittedToLMS = true;
                    }
                }
                return model;
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - Store Adani Housing ApplyForLoan form in LeadManagementSystem : :" + model.EmailID + e.Message, this);
                return model;
            }
        }

        public RequestCallbackModel FreshDeskReqCallbackTicket(RequestCallbackModel model)
        {
            try
            {
                model.IsSubmittedToFreshdesk = false;
                string json = "{\"status\": 2, \"priority\": 1, \"email\":\"" + model.EmailID + "\",\"subject\":\"RequestCallback\",\"description\":\"" + model.Message + "\",\"custom_fields\" : { \"cf_customer_name\" : \"" + model.Name + "\",\"cf_customer_mobile_number\":" + model.MobileNo + " } }";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FreshDeskServiceURL);
                request.ContentType = "application/json";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                // Set the ContentLength property of the WebRequest. 
                request.ContentLength = byteArray.Length;
                string authInfo = FreshDeskUserName + ":" + FreshDeskPassword;
                authInfo = System.Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                //Get the stream that holds request data by calling the GetRequestStream method. 
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream. 
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object. 
                dataStream.Close();
                try
                {
                    Console.WriteLine("Submitting Request");
                    WebResponse response = request.GetResponse();
                    // Get the stream containing content returned by the server.
                    //Send the request to the server by calling GetResponse. 
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access. 
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content. 
                    string Response = reader.ReadToEnd();
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(Response);
                    //XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                    Newtonsoft.Json.Linq.JObject jResponse = Newtonsoft.Json.Linq.JObject.Parse(Response);
                    model.TicketID = jResponse.Value<string>("id");
                    model.IsSubmittedToFreshdesk = !string.IsNullOrEmpty(model.TicketID) ? true : false;

                    //return status code
                    //Console.WriteLine("Status Code: {1} {0}", ((HttpWebResponse)response).StatusCode, (int)((HttpWebResponse)response).StatusCode);
                    //return location header
                    //Console.WriteLine("Location: {0}", response.Headers["Location"]);
                    //return the response 
                    //Console.Out.WriteLine(Response);
                }
                catch (WebException ex)
                {
                    Console.WriteLine("API Error: Your request is not successful. If you are not able to debug this error properly, mail us at support@freshdesk.com with the follwing X-Request-Id");
                    Console.WriteLine("X-Request-Id: {0}", ex.Response.Headers["X-Request-Id"]);
                    Console.WriteLine("Error Status Code : {1} {0}", ((HttpWebResponse)ex.Response).StatusCode, (int)((HttpWebResponse)ex.Response).StatusCode);
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        Console.Write("Error Response: ");
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
                return model;
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - Store Adani Housing RequestCallback form in Freshdesk : :" + model.EmailID + e.Message, this);
                return model;
            }
        }
        public FreshdeskCreateTicketModel FreshDeskCreateTicket(FreshdeskCreateTicketModel model)
        {
            try
            {
                model.IsSubmittedToFreshdesk = false;
                model.LoanAccountNo = model.LoanAccountNo ?? "";
                model.EmailID = model.EmailID ?? "";
                model.Subject = model.Subject ?? "CreateTicket";
                string json = "{\"status\": 2, \"priority\": 1, \"email\":\"" + model.EmailID + "\",\"subject\":\"" + model.Subject + "\",\"description\":\"" + model.Message + "\",\"custom_fields\" : { \"cf_request_type\": \"Request\",\"cf_customer_name\" : \"" + model.Name + "\",\"cf_customer_mobile_number\":" + model.MobileNo + ",\"cf_loan_account_number\": \"" + model.LoanAccountNo + "\",\"cf_customer_email_id\":\"" + model.EmailID + "\" } }";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FreshDeskServiceURL);
                request.ContentType = "application/json";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                // Set the ContentLength property of the WebRequest. 
                request.ContentLength = byteArray.Length;
                string authInfo = FreshDeskUserName + ":" + FreshDeskPassword;
                authInfo = System.Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                //Get the stream that holds request data by calling the GetRequestStream method. 
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream. 
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object. 
                dataStream.Close();
                try
                {
                    Console.WriteLine("Submitting Request");
                    WebResponse response = request.GetResponse();
                    // Get the stream containing content returned by the server.
                    //Send the request to the server by calling GetResponse. 
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access. 
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content. 
                    string Response = reader.ReadToEnd();
                    //XmlDocument doc = JsonConvert.DeserializeXmlNode(Response);
                    //XDocument incomingXml = XDocument.Parse(doc.InnerXml);
                    Newtonsoft.Json.Linq.JObject jResponse = Newtonsoft.Json.Linq.JObject.Parse(Response);
                    model.TicketID = jResponse.Value<string>("id");
                    model.IsSubmittedToFreshdesk = !string.IsNullOrEmpty(model.TicketID) ? true : false;

                    //return status code
                    //Console.WriteLine("Status Code: {1} {0}", ((HttpWebResponse)response).StatusCode, (int)((HttpWebResponse)response).StatusCode);
                    //return location header
                    //Console.WriteLine("Location: {0}", response.Headers["Location"]);
                    //return the response 
                    //Console.Out.WriteLine(Response);
                }
                catch (WebException ex)
                {
                    Log.Error("API Error: Your request is not successful. If you are not able to debug this error properly, mail us at support@freshdesk.com with the follwing X-Request-Id", this);
                    Log.Error("X-Request-Id:" + ex.Response.Headers["X-Request-Id"], this);
                    Console.WriteLine("Error Status Code : {1} {0}", ((HttpWebResponse)ex.Response).StatusCode, (int)((HttpWebResponse)ex.Response).StatusCode);
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        Log.Error("Error Response: " + reader.ReadToEnd(), this);
                    }
                }
                return model;
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - Store Adani Housing FreshDeskCreateTicket form in Freshdesk : :" + model.EmailID + e.Message, this);
                return model;
            }
        }
        //public AdaniHousingApplyforLoanModel FreshDeskApplyForLoanTicket(AdaniHousingApplyforLoanModel model)
        //{
        //    try
        //    {
        //        model.IsSubmittedToFreshdesk = false;
        //        string json = "{\"status\": 2, \"priority\": 1, \"email\":\"" + model.EmailID + "\",\"subject\":\"ApplyForLoan\",\"description\":\"" + model.ProductType + "\",\"custom_fields\" : { \"cf_customer_name\" : \"" + model.FirstName +" "+ model.LastName + "\",\"cf_customer_mobile_number\":" + model.MobileNo + ",\"cf_loan_amount\":" + model.LoanAmount + ",\"cf_customer_email_id\":\"" + model.EmailID + "\" } }";
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FreshDeskServiceURL);
        //        request.ContentType = "application/json";
        //        request.Method = "POST";
        //        byte[] byteArray = Encoding.UTF8.GetBytes(json);
        //        // Set the ContentLength property of the WebRequest. 
        //        request.ContentLength = byteArray.Length;
        //        string authInfo = FreshDeskUserName + ":" + FreshDeskPassword;
        //        authInfo = System.Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
        //        request.Headers["Authorization"] = "Basic " + authInfo;
        //        //Get the stream that holds request data by calling the GetRequestStream method. 
        //        Stream dataStream = request.GetRequestStream();
        //        // Write the data to the request stream. 
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        // Close the Stream object. 
        //        dataStream.Close();
        //        try
        //        {
        //            Console.WriteLine("Submitting Request");
        //            WebResponse response = request.GetResponse();
        //            // Get the stream containing content returned by the server.
        //            //Send the request to the server by calling GetResponse. 
        //            dataStream = response.GetResponseStream();
        //            // Open the stream using a StreamReader for easy access. 
        //            StreamReader reader = new StreamReader(dataStream);
        //            // Read the content. 
        //            string Response = reader.ReadToEnd();
        //            //XmlDocument doc = JsonConvert.DeserializeXmlNode(Response);
        //            //XDocument incomingXml = XDocument.Parse(doc.InnerXml);
        //            Newtonsoft.Json.Linq.JObject jResponse = Newtonsoft.Json.Linq.JObject.Parse(Response);
        //            model.TicketID = jResponse.Value<string>("id");
        //            model.IsSubmittedToFreshdesk = !string.IsNullOrEmpty(model.TicketID) ? true : false;

        //            //return status code
        //            //Console.WriteLine("Status Code: {1} {0}", ((HttpWebResponse)response).StatusCode, (int)((HttpWebResponse)response).StatusCode);
        //            //return location header
        //            //Console.WriteLine("Location: {0}", response.Headers["Location"]);
        //            //return the response 
        //            //Console.Out.WriteLine(Response);
        //        }
        //        catch (WebException ex)
        //        {
        //            Log.Error("API Error: Your request is not successful. If you are not able to debug this error properly, mail us at support@freshdesk.com with the follwing X-Request-Id", this);
        //            Log.Error("X-Request-Id: "+ ex.Response.Headers["X-Request-Id"], this);
        //            Log.Error("Error Status Code : "+ ((HttpWebResponse)ex.Response).StatusCode + " " + (int)((HttpWebResponse)ex.Response).StatusCode, this);
        //            using (var stream = ex.Response.GetResponseStream())
        //            using (var reader = new StreamReader(stream))
        //            {
        // Log.Error("Error Response: " + reader.ReadToEnd(), this);
        //            }
        //        }
        //        return model;
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error("Error in Method - Store Adani Housing ApplyForLoan form in Freshdesk : :" + model.EmailID + e.Message, this);
        //        return model;
        //    }
        //}

        #region LoanManagment APIs
        public string LMSserviceTokenGenerate()
        {
            try
            {
                List<string> tokenParam = new List<string>();
                var client = new RestClient(LMSServiceURL + LMSTokenService);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("client_id", LMSClientId, ParameterType.GetOrPost);
                request.AddParameter("client_secret", LMSClient_Secret, ParameterType.GetOrPost);
                request.AddParameter("grant_type", LMSGrant_Type, ParameterType.GetOrPost);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    return response.Content;
                }
                else
                {
                    Log.Info("LMS service Token Generate error: " + response.Content.ToString(), this);
                    return "";
                }
            }
            catch (Exception e)
            {
                Log.Error("LMS service Token Generate error: " + e.Message, this);
                return "";
            }
        }
        public LoginInfoModel LMSGetLoans(LoginInfoModel loginInfoModel)
        {
            try
            {
                Log.Info("Start service Method - GetLoans Adani Housing LMS login :" + loginInfoModel.MobileNoOrLoanAccountNumberValue, this);
                string requestURL = LMSServiceURL + LMSgetLoans;
                var client = new RestClient(requestURL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var LMSServiceResponse = LMSserviceTokenGenerate();
                string AccessToken = "";
                if (!string.IsNullOrEmpty(LMSServiceResponse))
                {
                    Newtonsoft.Json.Linq.JObject jServiceResponse = Newtonsoft.Json.Linq.JObject.Parse(LMSServiceResponse);
                    AccessToken = jServiceResponse.Value<string>("access_token");
                    request.AddHeader("access_token", AccessToken);
                    Log.Info("Received Token for GetLoans Adani Housing LMS login :" + AccessToken, this);
                }

                var body = @" {
" + "\n" +
                @"  ""LoanDetailRequestMO"":
" + "\n" +
                @" {
" + "\n" +
                @"  ""mobileNumber"":""" + loginInfoModel.MobileNo + @""",
" + "\n" +
                @"  ""requestHeader"":
" + "\n" +
                @"  {
" + "\n" +
                @"  ""userDetail"":
" + "\n" +
                @"  {
" + "\n" +
                @"  ""branchId"":" + Int64.Parse(LMSbranchId) + @" ,
" + "\n" +
                @"  ""userCode"":""" + LMSuserCode + @"""
" + "\n" +
                @"  },
" + "\n" +
                @"    ""tenantId"":" + Int64.Parse(LMStenantId) + @"
" + "\n" +
                @"  },
" + "\n" +
                @"  ""requestChannel"":""" + LMSrequestChannel + @""",
" + "\n" +
                @"  ""primaryCustomerOnly"":" + (LMSprimaryCustomerOnly == "1" ? "true" : "false") + @"
" + "\n" +
                @" }
" + "\n" +
                @" }
" + "\n";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    Log.Info("Received Data Success for GetLoans Adani Housing LMS login :" + response.Content, this);
                    var responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);
                    //var responseObj2 = JsonConvert.DeserializeObject<JObject>(response.Content);
                    //if (responseObj.error != null) throw new Exception(responseObj.error_description);
                    //var j = Newtonsoft.Json.Linq.JArray.Parse(responseObj);

                    if (responseObj["success"] != null)
                    {
                        Newtonsoft.Json.Linq.JArray jarr = (Newtonsoft.Json.Linq.JArray)responseObj["success"];
                        foreach (Newtonsoft.Json.Linq.JObject content in jarr.Children<JObject>())
                        {
                            var ValidLAN = content.Properties().Where(x => x.Name == "loanDetails" && (JsonConvert.DeserializeObject<dynamic>(x.Value.ToString())[0]["loanAccountNumber"].ToString().Contains("HL") || JsonConvert.DeserializeObject<dynamic>(x.Value.ToString())[0]["loanAccountNumber"].ToString().Contains("LAP"))).FirstOrDefault();
                            if (ValidLAN != null)
                            {
                                getLoansModel getLoansObj = new getLoansModel();
                                foreach (Newtonsoft.Json.Linq.JProperty prop in content.Properties())
                                {
                                    if (prop.Name == "customerName")
                                    {
                                        getLoansObj.customerName = (string)prop.Value;
                                    }
                                    if (prop.Name == "customerNumber")
                                    {
                                        getLoansObj.customerNumber = (string)prop.Value;
                                    }
                                    if (prop.Name == "dateOfBirthOrInception")
                                    {
                                        getLoansObj.dateOfBirthOrInception = (string)prop.Value;
                                    }
                                    if (prop.Name == "panNumber")
                                    {
                                        getLoansObj.panNumber = (string)prop.Value;
                                    }
                                    if (prop.Name == "individualCorporateFlag")
                                    {
                                        getLoansObj.individualCorporateFlag = (string)prop.Value;
                                    }
                                    if (prop.Name == "dateOfBirthOrInception")
                                    {
                                        getLoansObj.dateOfBirthOrInception = (string)prop.Value;
                                    }
                                    if (prop.Name == "loanDetails")
                                    {
                                        var propObj = JsonConvert.DeserializeObject<dynamic>(prop.Value.ToString());
                                        loanDetails loanDetailsObj = new loanDetails();
                                        loanDetailsObj.loanId = propObj[0]["loanId"];
                                        loanDetailsObj.loanAccountNumber = propObj[0]["loanAccountNumber"];
                                        //loginInfoModel.LoanAccountNumber= propObj[0]["loanAccountNumber"];
                                        loanDetailsObj.productName = propObj[0]["productName"];
                                        loanDetailsObj.financedAmount = propObj[0]["financedAmount"];
                                        loanDetailsObj.loanBranchCode = propObj[0]["loanBranchCode"];
                                        loanDetailsObj.tenure = propObj[0]["tenure"];
                                        loanDetailsObj.currencyIsoCode = propObj[0]["currencyIsoCode"];
                                        loanDetailsObj.disbursalDate = propObj[0]["disbursalDate"];
                                        loanDetailsObj.agreementDate = propObj[0]["agreementDate"];
                                        loanDetailsObj.productTypeDescription = propObj[0]["productTypeDescription"];
                                        loanDetailsObj.propertyAddress = propObj[0]["propertyAddress"];
                                        loanDetailsObj.loanDisbursalStatus = propObj[0]["loanDisbursalStatus"];
                                        loanDetailsObj.maturityFlag = propObj[0]["maturityFlag"];
                                        loanDetailsObj.nextDueInstallmentAmount = propObj[0]["nextDueInstallmentAmount"];
                                        loanDetailsObj.nextDueDate = propObj[0]["nextDueDate"];
                                        loanDetailsObj.balanceTenure = propObj[0]["balanceTenure"];
                                        loanDetailsObj.numberOfInstallmentUnpaid = propObj[0]["numberOfInstallmentUnpaid"];
                                        loanDetailsObj.amountOverdue = propObj[0]["amountOverdue"];
                                        loanDetailsObj.mobileNumber = propObj[0]["mobileNumber"];
                                        loanDetailsObj.primaryEmailId = propObj[0]["primaryEmailId"];
                                        loanDetailsObj.vehicleRegistrationNumber = propObj[0]["vehicleRegistrationNumber"];
                                        loanDetailsObj.loanStatus = propObj[0]["loanStatus"];
                                        loanDetailsObj.customerRelationship = propObj[0]["customerRelationship"];
                                        loanDetailsObj.effectiveRateOfInterest = propObj[0]["loanRepaymentDetail"]["effectiveRateOfInterest"];
                                        loanDetailsObj.interestChargeMode = propObj[0]["loanRepaymentDetail"]["interestChargeMode"];
                                        loanDetailsObj.repaymentFrequency = propObj[0]["loanRepaymentDetail"]["repaymentFrequency"];
                                        loanDetailsObj.repaymentDueDay = propObj[0]["loanRepaymentDetail"]["repaymentDueDay"];
                                        loanDetailsObj.installmentAmount = propObj[0]["loanRepaymentDetail"]["installmentAmount"];
                                        loanDetailsObj.contractType = propObj[0]["contractType"];
                                        loanDetailsObj.contractSubType = propObj[0]["contractSubType"];
                                        loanDetailsObj.recoveryType = propObj[0]["recoveryType"];
                                        loanDetailsObj.recoverySubType = propObj[0]["recoverySubType"];
                                        loanDetailsObj.overDraftType = propObj[0]["overDraftType"];
                                        loanDetailsObj.overDraftFlag = propObj[0]["overDraftFlag"];
                                        getLoansObj.loanDetailsList.Add(loanDetailsObj);
                                    }
                                }
                                loginInfoModel.getLoansList.Add(getLoansObj);
                            }
                            else
                            {
                                continue;
                            }                            
                        }
                        if (loginInfoModel.getLoansList.Count > 0)
                        {
                            loginInfoModel.IsAccountValid = true;
                        }
                        else
                        {
                            Log.Info("Received Data Error for GetLoans Adani Housing LMS login :" + response.Content, this);
                            loginInfoModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoUserFound", loginInfoModel.MobileNoOrLoanAccountNumber + " not found");
                            loginInfoModel.IsAccountValid = false;
                        }
                    }
                    else if (responseObj["error"] != null)
                    {
                        Log.Info("Received Data Error for Get_Loan_Details Adani Housing LMS login :" + response.Content, this);
                        loginInfoModel.Message = responseObj["error"][0]["messageArguments"];
                        loginInfoModel.IsAccountValid = false;
                    }
                    else
                    {
                        Log.Info("Received Data Error for GetLoans Adani Housing LMS login :" + response.Content, this);
                        loginInfoModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoUserFound", loginInfoModel.MobileNoOrLoanAccountNumber + " is not valid");
                        loginInfoModel.IsAccountValid = false;
                    }
                }
                else
                {
                    Log.Info("Service Response not successfull for GetLoans Adani Housing LMS login :" + response.Content, this);
                    loginInfoModel.IsAccountValid = false;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - GetLoans Adani Housing LMS login :" + loginInfoModel.MobileNoOrLoanAccountNumberValue + e.Message, this);
            }
            return loginInfoModel;
        }

        public LoginInfoModel LMSGet_Loan_Details(LoginInfoModel loginInfoModel)
        {

            try
            {
                if (loginInfoModel.LoanAccountNumber.ToUpper().Contains("HL") || loginInfoModel.LoanAccountNumber.ToUpper().Contains("LAP"))
                {
                    Log.Info("Start service Method - Get_Loan_Details Adani Housing LMS login :" + loginInfoModel.MobileNoOrLoanAccountNumberValue, this);
                    string requestURL = LMSServiceURL + LMSget_loan_details;
                    var client = new RestClient(requestURL);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    var LMSServiceResponse = LMSserviceTokenGenerate();
                    string AccessToken = "";
                    if (!string.IsNullOrEmpty(LMSServiceResponse))
                    {
                        Newtonsoft.Json.Linq.JObject jServiceResponse = Newtonsoft.Json.Linq.JObject.Parse(LMSServiceResponse);
                        AccessToken = jServiceResponse.Value<string>("access_token");
                        request.AddHeader("access_token", AccessToken);
                        Log.Info("Received Token for Get_Loan_Details Adani Housing LMS login :" + AccessToken, this);
                    }

                    var body = @"{
 " + "\n" +
                    @"  ""loanAccountNumber"": """ + loginInfoModel.LoanAccountNumber + @""",
 " + "\n" +
                    @"  ""primaryCustomerOnly"":" + (LMSprimaryCustomerOnly == "1" ? "true" : "false") + @",
 " + "\n" +
                    @"  ""requestChannel"": """ + LMSrequestChannel + @""",
 " + "\n" +
                    @"  ""requestHeader"": {
 " + "\n" +
                    @"      ""tenantId"":" + Int64.Parse(LMStenantId) + @",
 " + "\n" +
                    @"      ""userDetail"":{
 " + "\n" +
                    @"          ""branchId"":" + Int64.Parse(LMSbranchId) + @",
 " + "\n" +
                    @"          ""userCode"":""" + LMSuserCode + @"""
 " + "\n" +
                    @"      }
 " + "\n" +
                    @"    }
 " + "\n" +
                    @"}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful)
                    {
                        Log.Info("Received Data Success for Get_Loan_Details Adani Housing LMS login :" + response.Content, this);
                        var responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);
                        //var responseObj2 = JsonConvert.DeserializeObject<JObject>(response.Content);
                        //if (responseObj.error != null) throw new Exception(responseObj.error_description);
                        //var j = Newtonsoft.Json.Linq.JArray.Parse(responseObj);

                        if (responseObj["success"] != null)
                        {
                            loginInfoModel.MobileNo = responseObj["success"]["customerDetailMO"]["mobileNumber"];
                            loginInfoModel.LoanAccountNumber = loginInfoModel.MobileNoOrLoanAccountNumberValue;
                            if (loginInfoModel.MobileNo != null)
                            {
                                loginInfoModel.IsAccountValid = true;
                            }
                            else
                            {
                                loginInfoModel.IsAccountValid = false;
                            }
                        }
                        else if (responseObj["error"] != null)
                        {
                            Log.Info("Received Data Error for Get_Loan_Details Adani Housing LMS login :" + response.Content, this);
                            loginInfoModel.Message = responseObj["error"][0]["messageArguments"];
                            loginInfoModel.IsAccountValid = false;
                        }
                        else
                        {
                            Log.Info("Received Data Error for Get_Loan_Details Adani Housing LMS login :" + response.Content, this);
                            loginInfoModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoUserFound", loginInfoModel.MobileNoOrLoanAccountNumber + " not found");
                            loginInfoModel.IsAccountValid = false;
                        }
                    }
                    else
                    {
                        Log.Info("Service Response not successfull for Get_Loan_Details Adani Housing LMS login :" + response.Content, this);
                        loginInfoModel.IsAccountValid = false;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - Get_Loan_Details Adani Housing LMS login :" + loginInfoModel.MobileNoOrLoanAccountNumberValue + e.Message, this);
            }
            return loginInfoModel;
        }
        public LoginInfoModel LMSGet_UserIdentityDetails(LoginInfoModel loginInfoModel)
        {
            try
            {
                Log.Info("Start service Method - LMSGet_UserIdentityDetails Adani Housing LMS login :" + loginInfoModel.LoanAccountNumber, this);
                string requestURL = LMSServiceURL + LMSget_loan_details;
                var client = new RestClient(requestURL);
                client.Timeout = -1;
                var requestToken = new RestRequest(Method.POST);
                requestToken.AddHeader("Content-Type", "application/json");
                var LMSServiceResponse = LMSserviceTokenGenerate();
                string AccessToken = "";
                if (!string.IsNullOrEmpty(LMSServiceResponse))
                {
                    Newtonsoft.Json.Linq.JObject jServiceResponse = Newtonsoft.Json.Linq.JObject.Parse(LMSServiceResponse);
                    AccessToken = jServiceResponse.Value<string>("access_token");
                    //requestToken.AddHeader("access_token", AccessToken);
                    Log.Info("Received Token for LMSGet_UserIdentityDetails Adani Housing LMS login :" + AccessToken, this);
                }
                string body = "";
                if (loginInfoModel.AssociatedLANList.Count > 0)
                {

                    foreach (var LANlist in loginInfoModel.AssociatedLANList)
                    {
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("access_token", AccessToken);
                        if (LANlist.Value != null)
                        {
                            body = @"{
 " + "\n" +
                    @"  ""loanAccountNumber"": """ + LANlist.Value + @""",
 " + "\n" +
                    @"  ""primaryCustomerOnly"":" + (LMSprimaryCustomerOnly == "1" ? "true" : "false") + @",
 " + "\n" +
                    @"  ""requestChannel"": """ + LMSrequestChannel + @""",
 " + "\n" +
                    @"  ""requestHeader"": {
 " + "\n" +
                    @"      ""tenantId"":" + Int64.Parse(LMStenantId) + @",
 " + "\n" +
                    @"      ""userDetail"":{
 " + "\n" +
                    @"          ""branchId"":" + Int64.Parse(LMSbranchId) + @",
 " + "\n" +
                    @"          ""userCode"":""" + LMSuserCode + @"""
 " + "\n" +
                    @"      }
 " + "\n" +
                    @"    }
 " + "\n" +
                    @"}";
                            request.AddParameter("application/json", body, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            if (response.IsSuccessful)
                            {
                                Log.Info("Received Data Success for LMSGet_UserIdentityDetails Adani Housing LMS login :" + response.Content, this);
                                var responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);
                                //var responseObj2 = JsonConvert.DeserializeObject<JObject>(response.Content);
                                //if (responseObj.error != null) throw new Exception(responseObj.error_description);
                                //var j = Newtonsoft.Json.Linq.JArray.Parse(responseObj);

                                if (responseObj["success"] != null)
                                {

                                    if (responseObj["success"]["customerDetailMO"]["identificationDetails"] != null)
                                    {
                                        Newtonsoft.Json.Linq.JArray jarr = (Newtonsoft.Json.Linq.JArray)responseObj["success"]["customerDetailMO"]["identificationDetails"];
                                        foreach (Newtonsoft.Json.Linq.JObject content in jarr.Children<JObject>())
                                        {
                                            identificationDetails detailsObj = new identificationDetails();
                                            foreach (Newtonsoft.Json.Linq.JProperty prop in content.Properties())
                                            {
                                                if (prop.Name == "identificationNumber")
                                                {
                                                    detailsObj.identificationNumber = (string)prop.Value;
                                                }
                                                if (prop.Name == "identificationType")
                                                {
                                                    detailsObj.identificationType = (string)prop.Value;
                                                }
                                            }
                                            foreach (var data in loginInfoModel.getLoansList)
                                            {
                                                foreach (var listItem in data.loanDetailsList)
                                                {
                                                    if (listItem.loanAccountNumber == LANlist.Value)
                                                    {
                                                        listItem.maturityDate = responseObj["success"]["maturityDate"];
                                                        listItem.disbursedAmount = responseObj["success"]["disbursedAmount"];
                                                        listItem.finalSanctionedAmount = responseObj["success"]["finalSanctionedAmount"];
                                                        listItem.status = responseObj["success"]["status"];
                                                        listItem.principalOutstanding = responseObj["success"]["principalOutstanding"];
                                                        listItem.futurePrincipal = responseObj["success"]["futurePrincipal"];
                                                        data.identificationDetailsList.Add(detailsObj);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Log.Info("Received Data Error for LMSGet_UserIdentityDetails Adani Housing LMS login :" + response.Content, this);
                                }
                            }
                            else
                            {
                                Log.Info("Service Response not successfull for LMSGet_UserIdentityDetails Adani Housing LMS login :" + response.Content, this);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Log.Error("Error in Method - LMSGet_UserIdentityDetails Adani Housing LMS login :" + loginInfoModel.MobileNoOrLoanAccountNumberValue + e.Message, this);
            }
            return loginInfoModel;
        }

        public LoginInfoModel UpdateAssocitedLoanList(LoginInfoModel loginInfoModel)
        {
            try
            {
                foreach (var loanAccountList in loginInfoModel.getLoansList.Select(m => m.loanDetailsList))
                {
                    TextValueListItem listItem = new TextValueListItem()
                    {
                        Text = loanAccountList.Select(x => x.productName).FirstOrDefault() + " | " + loanAccountList.Select(x => x.loanAccountNumber).FirstOrDefault() ?? "",
                        Value = loanAccountList.Select(x => x.loanAccountNumber).FirstOrDefault()
                    };
                    loginInfoModel.AssociatedLANList.Add(listItem);
                }
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - AddLoanList Adani Housing LMS login :" + loginInfoModel.MobileNoOrLoanAccountNumberValue + e.Message, this);
            }
            return loginInfoModel;
        }

        public ReportsModel LMSGenerate_SOA_Report(string LANno)
        {
            ReportsModel reportsModel = new ReportsModel();
            try
            {
                Log.Info("Start service Method - LMSGenerate_SOA_Report Adani Housing LMS login :" + LANno, this);
                string requestURL = LMSServiceURL + LMSgenerate_SOA_report;
                var client = new RestClient(requestURL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var LMSServiceResponse = LMSserviceTokenGenerate();
                string AccessToken = "";
                if (!string.IsNullOrEmpty(LMSServiceResponse))
                {
                    Newtonsoft.Json.Linq.JObject jServiceResponse = Newtonsoft.Json.Linq.JObject.Parse(LMSServiceResponse);
                    AccessToken = jServiceResponse.Value<string>("access_token");
                    request.AddHeader("access_token", AccessToken);
                    Log.Info("Received Token for LMSGenerate_SOA_Report Adani Housing LMS login :" + AccessToken, this);
                }

                var body = @"{
 " + "\n" +
                @"  ""loanAccountNumber"": """ + LANno + @""",
 " + "\n" +
                @"  ""requestHeader"": {
 " + "\n" +
                @"      ""tenantId"":" + Int64.Parse(LMStenantId) + @",
 " + "\n" +
                @"      ""userDetail"":{
 " + "\n" +
                @"          ""branchId"":" + Int64.Parse(LMSbranchId) + @",
 " + "\n" +
                @"          ""userCode"":""" + LMSuserCode + @"""
 " + "\n" +
                @"      }
 " + "\n" +
                @"    }
 " + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    Log.Info("Received Data Success for LMSGenerate_SOA_Report Adani Housing LMS login :" + response.Content, this);
                    var responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);

                    if (responseObj["pdfData"] != null)
                    {
                        reportsModel.PDF_report = responseObj["pdfData"];
                        reportsModel.IsRequestValid = true;
                        if (string.IsNullOrEmpty(reportsModel.PDF_report))
                        {
                            reportsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoSOA_report", "Document is not available for LAN: " + LANno);
                            reportsModel.IsRequestValid = false;
                        }
                    }
                    else
                    {
                        Log.Info("Received Data Error for LMSGenerate_SOA_Report Adani Housing LMS login :" + response.Content, this);
                        reportsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoUserFound", LANno + " is not valid");
                        reportsModel.IsRequestValid = false;
                    }
                }
                else
                {
                    Log.Info("Service Response not successfull for LMSGenerate_SOA_Report Adani Housing LMS login :" + response.Content, this);
                    reportsModel.IsRequestValid = false;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - LMSGenerate_SOA_Report Adani Housing LMS login :" + LANno + e.Message, this);
            }
            return reportsModel;
        }
        public ReportsModel LMSGenerateInterestCertificate(string LANno)
        {
            ReportsModel reportsModel = new ReportsModel();
            try
            {
                Log.Info("Start service Method - LMSInterestCertificate Adani Housing LMS login :" + LANno, this);
                string requestURL = LMSServiceURL + LMSGetinterestCertificate;
                var client = new RestClient(requestURL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var LMSServiceResponse = LMSserviceTokenGenerate();
                string AccessToken = "";
                if (!string.IsNullOrEmpty(LMSServiceResponse))
                {
                    Newtonsoft.Json.Linq.JObject jServiceResponse = Newtonsoft.Json.Linq.JObject.Parse(LMSServiceResponse);
                    AccessToken = jServiceResponse.Value<string>("access_token");
                    request.AddHeader("access_token", AccessToken);
                    Log.Info("Received Token for LMSInterestCertificate Adani Housing LMS login :" + AccessToken, this);
                }

                var body = @"{
 " + "\n" +
                @"  ""loanAccountNumber"": """ + LANno + @""",
 " + "\n" +
                @"  ""requestHeader"": {
 " + "\n" +
                @"      ""tenantId"":" + Int64.Parse(LMStenantId) + @",
 " + "\n" +
                @"      ""userDetail"":{
 " + "\n" +
                @"          ""branchId"":" + Int64.Parse(LMSbranchId) + @",
 " + "\n" +
                @"          ""userCode"":""" + LMSuserCode + @"""
 " + "\n" +
                @"      }
 " + "\n" +
                @"    }
 " + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    Log.Info("Received Data Success for LMSInterestCertificate Adani Housing LMS login :" + response.Content, this);
                    var responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);

                    if (responseObj["pdfData"] != null)
                    {
                        reportsModel.PDF_report = responseObj["pdfData"];
                        reportsModel.IsRequestValid = true;
                        if (string.IsNullOrEmpty(reportsModel.PDF_report))
                        {
                            reportsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoSOA_report", "Document is not available for LAN: " + LANno);
                            reportsModel.IsRequestValid = false;
                        }
                    }
                    else
                    {
                        Log.Info("Received Data Error for LMSInterestCertificate Adani Housing LMS login :" + response.Content, this);
                        reportsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoUserFound", LANno + " is not valid");
                        reportsModel.IsRequestValid = false;
                    }
                }
                else
                {
                    Log.Info("Service Response not successfull for LMSInterestCertificate Adani Housing LMS login :" + response.Content, this);
                    reportsModel.IsRequestValid = false;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - LMSInterestCertificate Adani Housing LMS login :" + LANno + e.Message, this);
            }
            return reportsModel;
        }
        public ReportsModel LMSGetBalnaceConfirmLetter(string LANno)
        {
            ReportsModel reportsModel = new ReportsModel();
            try
            {
                Log.Info("Start service Method - LMSGetBalnaceConfirmLetter Adani Housing LMS login :" + LANno, this);
                string requestURL = LMSServiceURL + LMSgetBalanceConfirmationLetter;
                var client = new RestClient(requestURL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var LMSServiceResponse = LMSserviceTokenGenerate();
                string AccessToken = "";
                if (!string.IsNullOrEmpty(LMSServiceResponse))
                {
                    Newtonsoft.Json.Linq.JObject jServiceResponse = Newtonsoft.Json.Linq.JObject.Parse(LMSServiceResponse);
                    AccessToken = jServiceResponse.Value<string>("access_token");
                    request.AddHeader("access_token", AccessToken);
                    Log.Info("Received Token for LMSGetBalnaceConfirmLetter Adani Housing LMS login :" + AccessToken, this);
                }

                var body = @"{
 " + "\n" +
                @"  ""loanAccountNumber"": """ + LANno + @""",
 " + "\n" +
                @"  ""requestChannel"": """ + LMSrequestChannel + @""",
 " + "\n" +
                @"  ""requestHeader"": {
 " + "\n" +
                @"      ""tenantId"":" + Int64.Parse(LMStenantId) + @",
 " + "\n" +
                @"      ""userDetail"":{
 " + "\n" +
                @"          ""branchId"":" + Int64.Parse(LMSbranchId) + @",
 " + "\n" +
                @"          ""userCode"":""" + LMSuserCode + @"""
 " + "\n" +
                @"      }
 " + "\n" +
                @"    }
 " + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    Log.Info("Received Data Success for LMSGetBalnaceConfirmLetter Adani Housing LMS login :" + response.Content, this);
                    var responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);

                    if (responseObj["pdfData"] != null)
                    {
                        reportsModel.PDF_report = responseObj["pdfData"];
                        reportsModel.IsRequestValid = true;
                        if (string.IsNullOrEmpty(reportsModel.PDF_report))
                        {
                            reportsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoSOA_report", "Document is not available for LAN: " + LANno);
                            reportsModel.IsRequestValid = false;
                        }
                    }
                    else
                    {
                        Log.Info("Received Data Error for LMSGetBalnaceConfirmLetter Adani Housing LMS login :" + response.Content, this);
                        reportsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoUserFound", LANno + " is not valid");
                        reportsModel.IsRequestValid = false;
                    }
                }
                else
                {
                    Log.Info("Service Response not successfull for LMSGetBalnaceConfirmLetter Adani Housing LMS login :" + response.Content, this);
                    reportsModel.IsRequestValid = false;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - LMSGetBalnaceConfirmLetter Adani Housing LMS login :" + LANno + e.Message, this);
            }
            return reportsModel;
        }
        public ReportsModel LMSGetWelcomeLetter(string LANno)
        {
            ReportsModel reportsModel = new ReportsModel();
            try
            {
                Log.Info("Start service Method - LMSGetWelcomeLetter Adani Housing LMS login :" + LANno, this);
                string requestURL = LMSServiceURL + LMSgetWelcomeLetter;
                var client = new RestClient(requestURL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var LMSServiceResponse = LMSserviceTokenGenerate();
                string AccessToken = "";
                if (!string.IsNullOrEmpty(LMSServiceResponse))
                {
                    Newtonsoft.Json.Linq.JObject jServiceResponse = Newtonsoft.Json.Linq.JObject.Parse(LMSServiceResponse);
                    AccessToken = jServiceResponse.Value<string>("access_token");
                    request.AddHeader("access_token", AccessToken);
                    Log.Info("Received Token for LMSGetWelcomeLetter Adani Housing LMS login :" + AccessToken, this);
                }

                var body = @"{
 " + "\n" +
                @"  ""loanAccountNumber"": """ + LANno + @""",
 " + "\n" +
                @"  ""requestChannel"": """ + LMSrequestChannel + @""",
 " + "\n" +
                @"  ""requestHeader"": {
 " + "\n" +
                @"      ""tenantId"":" + Int64.Parse(LMStenantId) + @",
 " + "\n" +
                @"      ""userDetail"":{
 " + "\n" +
                @"          ""branchId"":" + Int64.Parse(LMSbranchId) + @",
 " + "\n" +
                @"          ""userCode"":""" + LMSuserCode + @"""
 " + "\n" +
                @"      }
 " + "\n" +
                @"    }
 " + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    Log.Info("Received Data Success for LMSGetWelcomeLetter Adani Housing LMS login :" + response.Content, this);
                    var responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);

                    if (responseObj["pdfData"] != null)
                    {
                        reportsModel.PDF_report = responseObj["pdfData"];
                        reportsModel.IsRequestValid = true;
                        if (string.IsNullOrEmpty(reportsModel.PDF_report))
                        {
                            reportsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoSOA_report", "Document is not available for LAN: " + LANno);
                            reportsModel.IsRequestValid = false;
                        }
                    }
                    else
                    {
                        Log.Info("Received Data Error for LMSGetWelcomeLetter Adani Housing LMS login :" + response.Content, this);
                        reportsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoUserFound", LANno + " is not valid");
                        reportsModel.IsRequestValid = false;
                    }
                }
                else
                {
                    Log.Info("Service Response not successfull for LMSGetWelcomeLetter Adani Housing LMS login :" + response.Content, this);
                    reportsModel.IsRequestValid = false;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - LMSGetWelcomeLetter Adani Housing LMS login :" + LANno + e.Message, this);
            }
            return reportsModel;
        }
        public ReportsModel LMSGenerateRepaymentSchedule(string LANno)
        {
            ReportsModel reportsModel = new ReportsModel();
            try
            {
                Log.Info("Start service Method - LMSGenerateRepaymentSchedule Adani Housing LMS login :" + LANno, this);
                string requestURL = LMSServiceURL + LMSgenerateRepaymentSchedule;
                var client = new RestClient(requestURL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var LMSServiceResponse = LMSserviceTokenGenerate();
                string AccessToken = "";
                if (!string.IsNullOrEmpty(LMSServiceResponse))
                {
                    Newtonsoft.Json.Linq.JObject jServiceResponse = Newtonsoft.Json.Linq.JObject.Parse(LMSServiceResponse);
                    AccessToken = jServiceResponse.Value<string>("access_token");
                    request.AddHeader("access_token", AccessToken);
                    Log.Info("Received Token for LMSGenerateRepaymentSchedule Adani Housing LMS login :" + AccessToken, this);
                }

                var body = @"{
 " + "\n" +
                @"  ""loanAccountNumber"": """ + LANno + @""",
 " + "\n" +
                @"  ""requestChannel"": """ + LMSrequestChannel + @""",
 " + "\n" +
                @"  ""requestHeader"": {
 " + "\n" +
                @"      ""tenantId"":" + Int64.Parse(LMStenantId) + @",
 " + "\n" +
                @"      ""userDetail"":{
 " + "\n" +
                @"          ""branchId"":" + Int64.Parse(LMSbranchId) + @",
 " + "\n" +
                @"          ""userCode"":""" + LMSuserCode + @"""
 " + "\n" +
                @"      }
 " + "\n" +
                @"    }
 " + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    Log.Info("Received Data Success for LMSGenerateRepaymentSchedule Adani Housing LMS login :" + response.Content, this);
                    var responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);

                    if (responseObj["pdfData"] != null)
                    {
                        reportsModel.PDF_report = responseObj["pdfData"];
                        reportsModel.IsRequestValid = true;
                        if (string.IsNullOrEmpty(reportsModel.PDF_report))
                        {
                            reportsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoSOA_report", "Document is not available for LAN: " + LANno);
                            reportsModel.IsRequestValid = false;
                        }
                    }
                    else
                    {
                        Log.Info("Received Data Error for LMSGenerateRepaymentSchedule Adani Housing LMS login :" + response.Content, this);
                        reportsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoUserFound", LANno + " is not valid");
                        reportsModel.IsRequestValid = false;
                    }
                }
                else
                {
                    Log.Info("Service Response not successfull for LMSGenerateRepaymentSchedule Adani Housing LMS login :" + response.Content, this);
                    reportsModel.IsRequestValid = false;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error in Method - LMSGenerateRepaymentSchedule Adani Housing LMS login :" + LANno + e.Message, this);
            }
            return reportsModel;
        }
        public TransactionsModel LMSGet_Transactions(TransactionsModel transactionsModel)
        {
            try
            {
                Log.Info(string.Concat("Start service Method - LMSGet_Transactions Adani Housing LMS login :", transactionsModel.LoanAccountNumber), this);
                string str = string.Concat(this.LMSServiceURL, this.LMSget_transactions);
                RestClient restClient = new RestClient(str)
                {
                    Timeout = -1
                };
                RestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("Content-Type", "application/json");
                string str1 = this.LMSserviceTokenGenerate();
                string str2 = "";
                if (!string.IsNullOrEmpty(str1))
                {
                    str2 = JObject.Parse(str1).Value<string>("access_token");
                    restRequest.AddHeader("access_token", str2);
                    Log.Info(string.Concat("Received Token for LMSGet_Transactions Adani Housing LMS login :", str2), this);
                }
                string str3 = string.Concat(new object[] { "{\r\n\n\"loanAccountNumber\":\"", transactionsModel.LoanAccountNumber, "\",\r\n\n\"noOfTransactionToDisplay\":", 10, ",\r\n \n  \"requestChannel\": \"", this.LMSrequestChannel, "\",\r\n \n  \"requestHeader\": {\r\n \n\"tenantId\":", long.Parse(this.LMStenantId), ",\r\n \n\"userDetail\":{\r\n \n\"branchId\":", long.Parse(this.LMSbranchId), ",\r\n \n \"userCode\":\"", this.LMSuserCode, "\"\r\n \n}\r\n \n}\r\n \n}" });
                restRequest.AddParameter("application/json", str3, ParameterType.RequestBody);
                IRestResponse restResponse = restClient.Execute(restRequest);
                if (!restResponse.IsSuccessful)
                {
                    Log.Info(string.Concat("Service Response not successfull for LMSGet_Transactions Adani Housing LMS login :", restResponse.Content), this);
                    transactionsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/get transaction error", "Error in getting transactions");
                }
                else
                {
                    Log.Info(string.Concat("Received Data Success for LMSGet_Transactions Adani Housing LMS login :", restResponse.Content), this);
                    dynamic obj = JsonConvert.DeserializeObject<object>(restResponse.Content);
                    if (obj["success"] == null)
                    {
                        Log.Info(string.Concat("Received Data Error for LMSGet_Transactions Adani Housing LMS login :", restResponse.Content), this);
                        transactionsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/NoDataFound", "No Record Found");
                    }
                    else if (obj["success"]["lastNPaymentsByCustomer"] != null)
                    {
                        foreach (JObject jObjects in ((JArray)obj["success"]["lastNPaymentsByCustomer"]).Children<JObject>())
                        {
                            LastPaymentsbyCustomer lastPaymentsbyCustomer = new LastPaymentsbyCustomer();
                            foreach (JProperty jProperty in jObjects.Properties())
                            {
                                if (jProperty.Name == "transactionDate")
                                {
                                    lastPaymentsbyCustomer.TransactionDate = (string)jProperty.Value;
                                }
                                if (jProperty.Name == "paymentDate")
                                {
                                    lastPaymentsbyCustomer.PaymentDate = (string)jProperty.Value;
                                }
                                if (jProperty.Name == "amount")
                                {
                                    lastPaymentsbyCustomer.Amount = (int)jProperty.Value;
                                }
                                if (jProperty.Name == "instrumentType")
                                {
                                    lastPaymentsbyCustomer.InstrumentType = (string)jProperty.Value;
                                }
                                if (jProperty.Name == "instrumentNumber")
                                {
                                    lastPaymentsbyCustomer.InstrumentNumber = (string)jProperty.Value;
                                }
                                if (jProperty.Name == "instrumentDate")
                                {
                                    lastPaymentsbyCustomer.InstrumentDate = (string)jProperty.Value;
                                }
                                if (jProperty.Name == "bankName")
                                {
                                    lastPaymentsbyCustomer.BankName = (string)jProperty.Value;
                                }
                                if (jProperty.Name == "referenceNumber")
                                {
                                    lastPaymentsbyCustomer.ReferenceNumber = (string)jProperty.Value;
                                }
                                if (jProperty.Name == "paymentMode")
                                {
                                    lastPaymentsbyCustomer.PaymentMode = (string)jProperty.Value;
                                }
                                if (jProperty.Name == "bounceDate")
                                {
                                    lastPaymentsbyCustomer.BounceDate = (string)jProperty.Value;
                                }
                                if (jProperty.Name == "bounceReason")
                                {
                                    lastPaymentsbyCustomer.BounceReason = (string)jProperty.Value;
                                }
                            }
                            transactionsModel.LastPaymentsbyCustomerList.Add(lastPaymentsbyCustomer);
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("Error in Method - LMSGet_Transactions Adani Housing LMS login :", transactionsModel.LoanAccountNumber, exception.Message), this);
                transactionsModel.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/get transaction error", "Error in getting transactions");
            }
            return transactionsModel;
        }
        public OTPModel SendOTP(string MobileNo, string pageInfo)
        {
            OTPModel otpModel = new OTPModel();
            bool flag;
            try
            {
                otpModel.MobileNo = MobileNo;
                Log.Info(string.Concat("Method - Store Adani Housing StoreOTP in Database :", otpModel.MobileNo), this);
                using (AdaniHousingFormsDataContext adaniHousingFormsDataContext = new AdaniHousingFormsDataContext())
                {
                    AdaniCapitalHousingOTP adaniCapitalHousingOTP = (
                        from x in adaniHousingFormsDataContext.AdaniCapitalHousingOTPs
                        where x.MobileNo == MobileNo && x.PageInfo == pageInfo
                        select x).FirstOrDefault<AdaniCapitalHousingOTP>();
                    if (adaniCapitalHousingOTP != null)
                    {
                        if (Int64.Parse(adaniCapitalHousingOTP.Count) < 3)
                        {
                            flag = false;
                        }
                        else
                        {
                            DateTime value = adaniCapitalHousingOTP.DateTime.Value;
                            flag = value.AddMinutes(10) > DateTime.Now;
                        }
                        if (!flag)
                        {
                            otpModel.Count = adaniCapitalHousingOTP.Count;
                            if (adaniCapitalHousingOTP.DateTime.Value.AddMinutes(10) <= DateTime.Now)
                            {
                                otpModel.Count = "0";
                            }
                        }
                        else
                        {
                            otpModel.Message = DictionaryPhraseRepository.Current.Get("/WebAPIServices/SMS/OTP Limit Msg", "OTP limit exceeds, please try after 10 minutes");
                            return otpModel;
                        }
                    }
                }
                string str = DictionaryPhraseRepository.Current.Get("/WebAPIServices/SMS/OTP url", "http://api.textlocal.in/send/?username=finserve.appsupport@adani.com&hash=bf3e200725490bd0bc413688326ee8fe4410d1380165a1d2b4c8152ed5b882ba&sender=iADANI&numbers=$mobileNo&message=Your+OTP+for+login+is+$otp.+iADANI&unicode=1");
                str = str.Replace("$mobileNo", MobileNo);
                Random random = new Random();
                otpModel.PageInfo = pageInfo ?? "adanihousing";
                OTPModel str1 = otpModel;
                int num = random.Next(0, 1000000);
                str1.OTP = num.ToString("D6");
                str = str.Replace("$otp", otpModel.OTP);
                RestClient restClient = new RestClient(str)
                {
                    Timeout = -1
                };
                IRestResponse restResponse = restClient.Execute(new RestRequest(Method.GET));
                if (!restResponse.IsSuccessful)
                {
                    Log.Error(string.Concat("OTP Api call failed for: ", str), this);
                    Log.Error(string.Concat("OTP Api call failure response: ", restResponse.Content), this);
                }
                else
                {
                    Log.Error(string.Concat("OTP Api call success for: ", str), this);
                }
                dynamic obj = JsonConvert.DeserializeObject<object>(restResponse.Content);
                if (obj["status"] != (dynamic)null & obj["status"] == DictionaryPhraseRepository.Current.Get("/WebAPIServices/SMS/SuccessResponse", "success"))
                {
                    otpModel.Status = (string)obj["status"];
                    otpModel.DateTime = DateTime.Now;
                }
                (new DatabaseServices()).StoreOTP(otpModel);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("SendOTP exception: ", exception.Message), "Adani Housing SendOTP");
                otpModel = null;
            }
            return otpModel;
        }
        public OTPModel ValidateOTP(string MobileNo, string otp, string pageInfo)
        {
            OTPModel model = new OTPModel();
            try
            {
                Log.Info(string.Concat("Method - Store Adani Housing ValidateOTP in Database :", MobileNo), this);
                using (AdaniHousingFormsDataContext adaniHousingFormsDataContext = new AdaniHousingFormsDataContext())
                {
                    AdaniCapitalHousingOTP adaniCapitalHousingOTP = (
                        from x in adaniHousingFormsDataContext.AdaniCapitalHousingOTPs
                        where x.MobileNo == MobileNo && x.PageInfo == pageInfo
                        select x).FirstOrDefault();
                    if (adaniCapitalHousingOTP != null)
                    {
                        if (string.IsNullOrEmpty(adaniCapitalHousingOTP.ValidateOTPCount))
                        {
                            adaniCapitalHousingOTP.ValidateOTPCount = "1";
                            adaniHousingFormsDataContext.SubmitChanges();
                        }
                        else
                        {
                            if (Int64.Parse(adaniCapitalHousingOTP.ValidateOTPCount) <= 3)
                            {
                                int num = int.Parse(adaniCapitalHousingOTP.ValidateOTPCount) + 1;
                                adaniCapitalHousingOTP.ValidateOTPCount = num.ToString();
                                adaniHousingFormsDataContext.SubmitChanges();
                            }
                            else
                            {
                                model.IsOTPvalid = false;
                                model.Message = DictionaryPhraseRepository.Current.Get("/Controller/Messages/OTP validity limit", " You have exceeded the OTP validity attempts");
                                model.IsValidAttemptExceeded = true;
                                return model;
                            }
                        }
                        if (adaniCapitalHousingOTP.OTP != otp)
                        {
                            model.IsOTPvalid = false;
                            return model;
                        }
                        else
                        {
                            model.IsOTPvalid = true;
                            return model;
                        }
                    }
                }
                model.IsOTPvalid = false;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("ValidateOTP exception: ", exception.Message), "Adani Housing ValidateOTP");
                model.IsOTPvalid = false;
            }
            return model;
        }
        #endregion
        public string GetIPAddress()
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
                Log.Error("GetIPAddress exception: " + e.Message, "Adani Housing GetIPAddress");
                return null;
            }
        }
        public bool IsUserLoggedIn()
        {
            if (Helper.UserSession.UserSessionContext == null)
            {
                return false;
            }
            else if (!Helper.UserSession.UserSessionContext.AuthToken.Equals(HttpContext.Current.Request.Cookies["AuthToken"].Value))
            {
                return false;
            }
            return true;
        }
        public bool StoreCurrentSession()
        {
            try
            {
                using (AdaniHousingFormsDataContext adaniHousingFormsDataContext = new AdaniHousingFormsDataContext())
                {
                    if (Helper.UserSession.UserSessionContext == null)
                    {
                        return false;
                    }
                    else if(Helper.UserSession.UserSessionContext.MobileNo != null)
                    {
                        AdaniCapitalHousingOTP oTP = (
                            from x in adaniHousingFormsDataContext.AdaniCapitalHousingOTPs
                            where x.MobileNo == Helper.UserSession.UserSessionContext.MobileNo && x.OTP == Helper.UserSession.UserSessionContext.OTP
                            select x).FirstOrDefault<AdaniCapitalHousingOTP>();
                        if(oTP != null)
                        {
                            oTP.SessionId = Helper.UserSession.UserSessionContext.AuthToken;
                            adaniHousingFormsDataContext.SubmitChanges();
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception e)
            {
                Log.Error("Session store failed for adani housing login: " + e.Message, this);
                return false;
            }
        }
        public bool ValidateCurrentSession()
        {
            try
            {
                using (AdaniHousingFormsDataContext adaniHousingFormsDataContext = new AdaniHousingFormsDataContext())
                {
                    if (Helper.UserSession.UserSessionContext == null)
                    {
                        return false;
                    }
                    else if (Helper.UserSession.UserSessionContext.MobileNo != null)
                    {
                        AdaniCapitalHousingOTP oTP = (
                            from x in adaniHousingFormsDataContext.AdaniCapitalHousingOTPs
                            where x.MobileNo == Helper.UserSession.UserSessionContext.MobileNo && x.SessionId == Helper.UserSession.UserSessionContext.AuthToken
                            select x).FirstOrDefault<AdaniCapitalHousingOTP>();
                        if (oTP != null)
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception e)
            {
                Log.Error("Session store failed for adani housing login: " + e.Message, this);
                return false;
            }
        }
        public string GetLastLoginDateTime()
        {
            try
            {
                using (AdaniHousingFormsDataContext adaniHousingFormsDataContext = new AdaniHousingFormsDataContext())
                {
                    if (Helper.UserSession.UserSessionContext == null)
                    {
                        return string.Empty;
                    }
                    else if (Helper.UserSession.UserSessionContext.MobileNo != null)
                    {
                        AdaniHousingUserLoginHistory m = (
                            from x in adaniHousingFormsDataContext.AdaniHousingUserLoginHistories
                            where x.MobileNo == Helper.UserSession.UserSessionContext.MobileNo 
                            select x).FirstOrDefault<AdaniHousingUserLoginHistory>();
                        if (m != null)
                        {                            
                            return m.LastLoginDateTime.ToString();
                        }
                        return string.Empty;
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
            }
            catch (Exception e)
            {
                Log.Error("failed for adani housing GetLastLoginDateTime: " + e.Message, this);
                return string.Empty;
            }
        }
    }
}