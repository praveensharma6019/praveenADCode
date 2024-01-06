using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.AdaniSolar.Website.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Sitecore.AdaniSolar.Website.Model
{
    public class SalesForceWrapper
    {
        public readonly string TokenUrl, SecurityToken, ConsumerKey, ConsumerSecret, ResourceEndpointUrl, Username, Password, IsSandboxUser, SfdcSandboxEndPoint, SfdcProductionEndPoint;

        public string Token { get; private set; }
        public string EndPoint { get; private set; }
        private bool ValidToken { get; set; }

        public SalesForceWrapper()
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - get Token call", this);

                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - get configuration", this);

                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.SalesForceCRMSettings.Id.ToString()));
                ConsumerKey = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.ConsumerKey].Value;// "be41119b-9aff-4103-a89c-0b6f9920029a";
                ConsumerSecret = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.ConsumerSecret].Value;// "2hV[enYR-u?P67UVa[5c5GFQ3m/U0uM.";

                IsSandboxUser = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.IsSandboxUser].Value.ToLower();
                TokenUrl = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.TokenURL].Value; //"https://login.microsoftonline.com/04c72f56-1848-46a2-8167-8e5d36510cbc/oauth2/token";

                string resourceSandbox = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcSandboxEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";
                string resourceProd = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcProductionEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";

                ResourceEndpointUrl = IsSandboxUser.Equals("true", StringComparison.CurrentCultureIgnoreCase)               //string resource = itemInfo.Fields[Templates.SalesForceCRMSettings.Fields.Insta_SfdcProductionEndPoint].Value; //"https://adanisolardev.crm8.dynamics.com/";
                   ? resourceSandbox           //"https://test.salesforce.com/services/oauth2/token"
                   : resourceProd;       //https://login.salesforce.com/services/oauth2/token";


                GetToken();
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Enquire now - Salesforce lead generation - get configuration" + e.Message, this);
            }
        }


        public void GetToken()
        {
            var startTime = DateTime.Now;

            try
            {
                var data = $"grant_type=client_credentials&client_id={ConsumerKey}&resource={ResourceEndpointUrl}&client_secret={ConsumerSecret}";
                var postArray = Encoding.ASCII.GetBytes(data);
                var req = (HttpWebRequest)WebRequest.Create(TokenUrl);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = postArray.Length;
                req.Accept = "application/json";
                req.Method = "POST";
                var reqStream = req.GetRequestStream();
                reqStream.Write(postArray, 0, postArray.Length);
                reqStream.Close();

                var resp = req.GetResponse();
                var responseStream = resp.GetResponseStream();
                if (responseStream == null) throw new Exception("Response stream is null");
                var streamReader = new StreamReader(responseStream);
                var responseText = streamReader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseText);
                if (responseObj.error != null) throw new Exception(responseObj.error_description);
                var Token = responseObj.access_token;
                var EndPoint = responseObj.instance_url;
                ValidToken = true;
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - get token - " + Token + "," + EndPoint, this);
            }
            catch (Exception ex)
            {
                //throw ex;
                Sitecore.Diagnostics.Log.Error("Enquire now - Salesforce lead generation - get token - get response Failed" + ex.Message, this);
                //WriteLine(exception);
            }

            var timeSpan = DateTime.Now - startTime;
        }

        public dynamic GenerateLead(CRMObject obj)
        {
            string result;
            try
            {
                var req1 = (HttpWebRequest)WebRequest.Create(ResourceEndpointUrl + "api/data/v9.0/leads");        //"https://adanisolardev.crm8.dynamics.com/api/data/v9.0/leads"
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead: " + req1, this);

                req1.ContentType = "application/json";
                req1.Headers.Add("Authorization", "Bearer " + Token);
                req1.Accept = "application/json";
                req1.Method = "POST";
                Sitecore.Diagnostics.Log.Info("Adani Solar CRM lead generation - generate lead start after getting token:" + obj.firstname + "," + obj.emailaddress1 + "," + obj.mobilephone + "," + obj.description + "," + obj.ispl_category + "," + obj.ispl_websitecountry + "," + obj.ispl_websiteregion + "," + obj.subject, this);
                using (var streamWriter = new StreamWriter(req1.GetRequestStream()))
                {
                    string jsonStr = JsonConvert.SerializeObject(obj);
                    streamWriter.Write(jsonStr);
                }
                var httpResponse1 = (HttpWebResponse)req1.GetResponse();
                using (var streamReader1 = new StreamReader(httpResponse1.GetResponseStream()))
                {
                    result = streamReader1.ReadToEnd();
                    Sitecore.Diagnostics.Log.Info("Adani Solar CRM lead generation - generate lead success result:" + result, this);
                }
                return true;
            }
            catch (Exception exception)
            {
                if (!ValidToken || !exception.Message.Contains("401")) return null;
                ValidToken = false;
                GetToken();
                GenerateLead(obj);
            }

            return false;
        }

        public dynamic GetRegion_Countries()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(ResourceEndpointUrl + "api/data/v9.0/ispl_countries?$select=ispl_name");
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + Token);
                request.Accept = "application/json";
                request.Method = "GET";
                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseString);
                if (responseObj.error != null) throw new Exception(responseObj.error_description);

                return responseObj;
            }
            catch (Exception exception)
            {
                if (!ValidToken || !exception.Message.Contains("401")) return null;
                ValidToken = false;
                GetToken();
                GetRegion_Territories();
            }
            return null;
        }

        public dynamic GetRegion_Territories()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(ResourceEndpointUrl + "/api/data/v9.0/territories?$select=name");
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + Token);
                request.Accept = "application/json";
                request.Method = "GET";
                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseString);
                if (responseObj.error != null) throw new Exception(responseObj.error_description);

                return responseObj;
            }
            catch (Exception exception)
            {
                if (!ValidToken || !exception.Message.Contains("401")) return null;
                ValidToken = false;
                GetToken();
                GetRegion_Territories();
            }
            return null;
        }
    }

}
