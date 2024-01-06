using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Salesforce.Common;
using Salesforce.Force;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Script.Serialization;


namespace Sitecore.MiracleMile.Website.Models
{
    public class SalesForceWrapper
    {
        //private static readonly Lazy<SalesForceWrapper> Config = new Lazy<SalesForceWrapper>(() => new SalesForceWrapper()); //private static Singleton singleInstance = null;  

        //public static SalesForceWrapper Instance => Config.Value;

        public readonly string TokenUrl, SecurityToken, ConsumerKey, ConsumerSecret, Username, Password, IsSandboxUser, SfdcSandboxEndPoint, SfdcProductionEndPoint;

        public string Token { get; private set; }
        public string EndPoint { get; private set; }
        private bool ValidToken { get; set; }

        public SalesForceWrapper()
        {
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - get configuration", this);
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.SalesForce.Id.ToString()));

                ConsumerKey = itemInfo.Fields[Templates.SalesForce.Fields.ConsumerKey].Value;
                ConsumerSecret = itemInfo.Fields[Templates.SalesForce.Fields.ConsumerSecret].Value;
                Password = itemInfo.Fields[Templates.SalesForce.Fields.Password].Value;
                Username = itemInfo.Fields[Templates.SalesForce.Fields.Username].Value;
                SfdcSandboxEndPoint = itemInfo.Fields[Templates.SalesForce.Fields.Insta_SfdcSandboxEndPoint].Value;
                SfdcProductionEndPoint = itemInfo.Fields[Templates.SalesForce.Fields.Insta_SfdcSandboxEndPoint].Value;
                IsSandboxUser = itemInfo.Fields[Templates.SalesForce.Fields.IsSandboxUser].Value.ToLower();
                TokenUrl = IsSandboxUser.Equals("true", StringComparison.CurrentCultureIgnoreCase)
                    ? SfdcSandboxEndPoint           //"https://test.salesforce.com/services/oauth2/token"
                    : SfdcProductionEndPoint;       //https://login.salesforce.com/services/oauth2/token";

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - get Configuration done: " + ConsumerKey + "," + ConsumerSecret + "," + Username + "," + Password + "," + TokenUrl, this);
                //ConsumerKey = "3MVG9N6eDmZRVJOmww7dH9YRZ7BGAgv1Po8FRMlkI7Q5o7zXTwdxfUEyBu5MWtBjetNxiPaLWLWONo_hp.SJ6";
                //ConsumerSecret = "2A9D0E4964D332E07E55D1A66E0EE0F61993264B243EB094128B6C9167FBD949";
                //Username = "integrationuser@adani.com";
                //Password = "priyanka@88aWoTr4Lny55bQBvGiYOf5F9W";
                //IsSandboxUser = "true";
                //SfdcSandboxEndPoint = "https://test.salesforce.com/services/oauth2/token";
                //SfdcProductionEndPoint = "https://test.salesforce.com/services/oauth2/token";
                //TokenUrl = IsSandboxUser.Equals("true", StringComparison.CurrentCultureIgnoreCase)
                //    ? SfdcSandboxEndPoint
                //    : SfdcProductionEndPoint;

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - get Token call", this);
                GetToken();
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Enquire now - Salesforce lead generation - get configuration" + e.Message, this);
            }
        }

        public async Task<ForceClient> GetClient()
        {
            var auth = new AuthenticationClient();
            await auth.UsernamePasswordAsync(ConsumerKey, ConsumerSecret, Username, Password, TokenUrl);
            return new ForceClient(auth.InstanceUrl, auth.AccessToken, auth.ApiVersion);
        }

        public void GetToken()
        {
            var startTime = DateTime.Now;

            try
            {
                var data = $"grant_type=password&client_id={ConsumerKey}&client_secret={ConsumerSecret}&username={Username}&password={Password}";
                var postArray = Encoding.ASCII.GetBytes(data);
                var req = (HttpWebRequest)WebRequest.Create(TokenUrl);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = postArray.Length;
                req.Accept = "application/json";
                req.Method = "POST";
                var reqStream = req.GetRequestStream();
                reqStream.Write(postArray, 0, postArray.Length);
                reqStream.Close();

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - get token - get response", this);
                var resp = req.GetResponse();

                var responseStream = resp.GetResponseStream();
                if (responseStream == null) throw new Exception("Response stream is null");
                var streamReader = new StreamReader(responseStream);
                var responseText = streamReader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseText);
                if (responseObj.error != null) throw new Exception(responseObj.error_description);
                Token = responseObj.access_token;
                EndPoint = responseObj.instance_url;
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - get token - "+Token+","+EndPoint, this);
            }
            catch (Exception ex)
            {
                //throw ex;
                Sitecore.Diagnostics.Log.Error("Enquire now - Salesforce lead generation - get token - get response Failed"+ex.Message, this);
                //WriteLine(exception);
            }

            var timeSpan = DateTime.Now - startTime;
        }

        public dynamic GenerateLead(LeadObject obj)
        {
            string result;
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(EndPoint + "/services/apexrest/EnquiryFromWeb");
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead: "+ req, this);
                req.ContentType = "application/json";
                req.Headers.Add("Authorization", "Bearer " + Token);
                req.Accept = "application/json";
                req.Method = "POST";
                List<LeadObject> t = new List<LeadObject>();
                t.Add(obj);
                using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                {
                    string jsonStr = JsonConvert.SerializeObject(t);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead: "+jsonStr, this);
                    streamWriter.Write(jsonStr);
                }

                var httpResponse = (HttpWebResponse)req.GetResponse();
                using (var streamReader1 = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader1.ReadToEnd();
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead:" + result, this);
                }
                return result;
            }
            catch (Exception exception)
            {
                if (!ValidToken || !exception.Message.Contains("401")) return null;
                ValidToken = false;
                GetToken();
                GenerateLead(obj);
            }

            return null;
        }

        public dynamic PostObject(string objectName, object sobj)
        {
            var jObject = JObject.FromObject(sobj);
            jObject.Remove("Id");
            jObject.Remove("Name");
            var body = JsonConvert.SerializeObject(jObject);
            var url = EndPoint + "/services/data/v44.0/sobjects/" + objectName;
            return ExecuteRequest(url, "POST", body);
        }

        public dynamic PatchObject(string objectName, string id, object sobj)
        {
            var body = JsonConvert.SerializeObject(sobj);
            var url = EndPoint + "/services/data/v44.0/sobjects/" + objectName + "/" + id;
            return ExecuteRequest(url, "PATCH", body);
        }

        private dynamic ExecuteRequest(string url, string method, string body = null)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + Token);
                request.Accept = "application/json";
                request.ContentType = "application/json";

                if (method != "GET")
                {
                    request.Method = method;
                    var data = Encoding.UTF8.GetBytes(body);

                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                var resp = request.GetResponse();
                var responseStream = resp.GetResponseStream();
                if (responseStream == null) throw new Exception("Response stream is null");
                var streamReader = new StreamReader(responseStream);
                var responseText = streamReader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(responseText)) return true;
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseText);
                if (responseObj.error != null) throw new Exception(responseObj.error_description);
                ValidToken = true;

                return responseObj;
            }
            catch (Exception exception)
            {
                //WriteLine(exception);

                if (!ValidToken || !exception.Message.Contains("401")) return null;

                ValidToken = false;
                GetToken();
                ExecuteRequest(url, method, body);
            }

            return null;
        }
    }

    public class LeadObject
    {
        public string Firstname { get; set; }
        public string LeadSource { get; set; }
        public string FormType { get; set; }
        public string PageInfo { get; set; }
        public string UtmSource { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Budget { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Remarks { get; set; }
        public string Project { get; set; }
        public string PostToSalesforce { get; set; }
        public string Saletype { get; set; }
        public string Projectintrested { get; set; }
        public string MasterProjectID { get; set; }
        public string AssignmentCity { get; set; }
        public string RecordType { get; set; }
        public string UtmPlacement { get; set; }
        public string Ads { get; set; }
    }
}
