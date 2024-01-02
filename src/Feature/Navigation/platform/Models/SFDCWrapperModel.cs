using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Models
{
    public class SFDCWrapperModel
    {
        //private static readonly Lazy<SalesForceWrapper> Config = new Lazy<SalesForceWrapper>(() => new SalesForceWrapper()); //private static Singleton singleInstance = null;  

        //public static SalesForceWrapper Instance => Config.Value;
        public readonly Item item = Sitecore.Context.Database.GetItem(Templates.SRDCServiceItemID);
        public readonly string TokenUrl, SecurityToken, ConsumerKey, ConsumerSecret, Username, IsSandboxUser, SfdcSandboxEndPoint, SfdcProductionEndPoint, encText;

        public string Token { get; private set; }
        public string EndPoint { get; private set; }
        private bool ValidToken { get; set; }

        public SFDCWrapperModel()
        {
            try
            {
                //Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - SFDCWrapperModel start", this);
                ConsumerKey = item.Fields[Templates.SFDCService.Fields.ConsumerKeyID].Value;
                ConsumerSecret = item.Fields[Templates.SFDCService.Fields.ConsumerSecretID].Value;
                Username = item.Fields[Templates.SFDCService.Fields.UsernameID].Value;
                TokenUrl = Helper.GetLinkURLbyField(item, item.Fields[Templates.SFDCService.Fields.TokenUrlID]);
                GetToken();
            }
            catch (Exception e)
            {
            }
        }

        public void GetToken()
        {
            try
            {
                var sfdcPd = item.Fields[Templates.SFDCService.Fields.pwdID].Value;

                var data = $"grant_type=password&client_id={ConsumerKey}&client_secret={ConsumerSecret}&username={Username}&password={sfdcPd}";
                var postArray = Encoding.ASCII.GetBytes(data);
                var req = (HttpWebRequest)WebRequest.Create(TokenUrl);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = postArray.Length;
                req.Accept = "application/json";
                req.Method = "POST";
                req.KeepAlive = false;
                using (var reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postArray, 0, postArray.Length);
                    reqStream.Close();
                    var resp = req.GetResponse();
                    var responseStream = resp.GetResponseStream();
                    if (responseStream == null) throw new Exception("Response stream is null");

                    using (var streamReader = new StreamReader(responseStream))
                    {
                        var responseText = streamReader.ReadToEnd();
                        var responseObj = JsonConvert.DeserializeObject<dynamic>(responseText);
                        if (responseObj.error != null) throw new Exception(responseObj.error_description);
                        Token = responseObj.access_token;
                        //EndPoint = responseObj.instance_url;
                        streamReader.Close();
                        streamReader.Dispose();
                    }
                    resp.Dispose();
                    resp.Close();
                    sfdcPd = string.Empty;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public dynamic GenerateLead(LeadObject obj)
        {
            string result;
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(Helper.GetLinkURLbyField(item, item.Fields[Templates.SFDCService.Fields.ServiceURLID]));
                req.ContentType = "application/json";
                req.Headers.Add("Authorization", "Bearer " + Token);
                req.Accept = "application/json";
                req.Method = "POST";
                List<LeadObjectNew> t = new List<LeadObjectNew>();

                LeadObjectNew objnew = new LeadObjectNew();
                //objnew.Firstname = obj.Firstname;
                objnew.LeadSource = obj.LeadSource;
                objnew.LastName = obj.LastName;
                objnew.FormType = obj.FormType;
                objnew.PageInfo = obj.PageInfo;
                objnew.UtmSource = obj.UtmSource;
                objnew.Email = obj.Email;
                objnew.Mobile = obj.Mobile;
                objnew.Budget = obj.Budget;
                objnew.Country = obj.Country;
                objnew.State = obj.State;
                objnew.Remarks = obj.Remarks;
                if (obj.ProductType != null)
                {
                    objnew.ProductType = obj.ProductType != "" && obj.ProductType.Contains(",") ? obj.ProductType.Replace(",", ";") : obj.ProductType;
                }
                objnew.RecordType = obj.RecordType;
                objnew.MasterProjectID = obj.MasterProjectID;
                objnew.ClubProject = obj.ClubProject;
                objnew.PostToSalesforce = obj.PostToSalesforce;
                objnew.Saletype = obj.Saletype;
                objnew.Projectintrested = obj.Projectintrested;
                objnew.Project = obj.Project;
                objnew.Comments = obj.Comments;
                objnew.AssignmentCity = !string.IsNullOrEmpty(obj.AssignmentCity) && obj.AssignmentCity.Contains(",") ? obj.AssignmentCity.Split(',').FirstOrDefault() : obj.AssignmentCity;
                objnew.ScheduleDate = !string.IsNullOrEmpty(obj.ScheduleDate) ? obj.ScheduleDate + "T00:00:00.000" : "";
                objnew.TimeSlot = obj.TimeSlot;
                objnew.Ads = obj.Ads;
                objnew.UtmPlacement = obj.UtmPlacement;
                objnew.IsHomeLoanRequired = obj.IsHomeLoanRequired;
                objnew.prequalification = obj.prequalification;
                objnew.purpose = obj.purpose;
                objnew.agreement = obj.agreement;

                t.Add(objnew);
                using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                {
                    string jsonStr = JsonConvert.SerializeObject(t);
                    streamWriter.Write(jsonStr);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - SFDCWrapperModel query" + jsonStr.ToString(), this);

                }

                var httpResponse = (HttpWebResponse)req.GetResponse();
                using (var streamReader1 = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader1.ReadToEnd();
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

        public class LeadObject
        {
            public string Firstname { get; set; }
            public string LeadSource { get; set; }
            public string LastName { get; set; }
            public string FormType { get; set; }
            public string PageInfo { get; set; }
            public string UtmSource { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string Budget { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string Remarks { get; set; }
            public string RecordType { get; set; }
            public string MasterProjectID { get; set; }
            public string ClubProject { get; set; }
            public string PostToSalesforce { get; set; }
            public string Saletype { get; set; }
            public string Projectintrested { get; set; }
            public string Project { get; set; }
            public string Comments { get; set; }
            public string AssignmentCity { get; set; }
            public string ScheduleDate { get; set; }
            public string TimeSlot { get; set; }
            public string Ads { get; set; }
            public string UtmPlacement { get; set; }
            public string OTP { get; set; }
            public bool agreement { get; set; }
            public bool OTPStatus { get; set; }
            public string ProductType { get; set; }
            public string EnquiryID { get; set; }
            public string Message { get; set; }
            public string purpose { get; set; }
            //public bool userblocked { get; set; }
            public bool IsPreQualificationLeads { get; set; }
            public bool IsHomeLoanRequired { get; set; }
            public string prequalification { get; set; }
        }


        public class LeadObjectNew
        {
            public string Firstname { get; set; }
            public string LeadSource { get; set; }
            public string LastName { get; set; }
            public string FormType { get; set; }
            public string PageInfo { get; set; }
            public string UtmSource { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string Budget { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string Remarks { get; set; }
            public string RecordType { get; set; }
            public string MasterProjectID { get; set; }
            public string ClubProject { get; set; }
            public string PostToSalesforce { get; set; }
            public string Saletype { get; set; }
            public string Projectintrested { get; set; }
            public string Project { get; set; }
            public string Comments { get; set; }
            public string AssignmentCity { get; set; }
            public string ScheduleDate { get; set; }
            public string TimeSlot { get; set; }
            public string ProductType { get; set; }
            public string Ads { get; set; }
            public string UtmPlacement { get; set; }
            public string prequalification { get; set; }
            public bool agreement { get; set; }
            public bool IsHomeLoanRequired { get; set; }
            public string purpose { get; set; }
        }

    }
}
