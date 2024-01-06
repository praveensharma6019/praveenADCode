using Adani.Foundation.Messaging.Models;
using Newtonsoft.Json;
using RestSharp;
using Sitecore.Configuration;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using static System.Net.WebRequestMethods;

namespace Adani.Foundation.Messaging.Services.SMS
{
    public class SMSService : ISMSService
    {
        public bool Send(string recipient, string messageBody, IEnumerable<KeyValuePair<string, string>> data)
        {
            var formattedMessage = ReplaceFieldValues(messageBody, data);

            try
            {
                var apiPath = Settings.GetSetting("SMSSender:ApiPath");
                var apiurl = string.Format(apiPath, recipient, HttpUtility.UrlEncode(formattedMessage));

                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(apiurl).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public bool SendSMSFarmpik(string mobile, FarmpikSMSModel data)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Preparing Farmpik SMS Service for mobile:" + mobile, "");
                var smsapiurl = "https://www.uat.adanione.com/api/cleartripwrapper/api/trip/sendsms";

                var smsapivalue = "degf@2457@!$#(&&$%^$2342";
                string rawData = string.Format("{0} ~ {1}", smsapivalue, mobile);
                //calling function to compute hash value
                var authorValue = HashString(rawData, "~6nqej2");

                Sitecore.Diagnostics.Log.Info("Preparing Farmpik SMS Service HttpClient for:" + smsapiurl, "");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Author", authorValue);
                var serlisedObj = JsonConvert.SerializeObject(data);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, smsapiurl);
                request.Content = new StringContent(serlisedObj,
                                    Encoding.UTF8,
                                    "application/json");
                var response = client.SendAsync(request).Result;
                Sitecore.Diagnostics.Log.Info("SMS Service Response:" + response, "");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Farmpik SMS API Exception:" + ex.InnerException.StackTrace, ex);
                return false;
            }
        }

        private static string ReplaceFieldValues(string content, IEnumerable<KeyValuePair<string, string>> formData)
        {
            string text = content;
            if (string.IsNullOrEmpty(text)) return string.Empty;

            foreach (var item in formData)
            {
                text = text.Replace($"[{item.Key}]", item.Value);
            }

            return text;
        }

        //added by neeraj yadav to calculate hash value of a string
        private string HashString(string text, string salt = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                string hash = BitConverter
                    .ToString(hashBytes).Replace("-", string.Empty);
                return hash;
            }
        }
    }
}
