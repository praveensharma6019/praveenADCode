using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Adani.Website.Providers
{
    public class EmailServicesProvider : Controller
    {
        public bool sendEmail(string to, string subject, string body, string from)
        {
            using (var client = new HttpClient())
            {
                var apiUrl = DictionaryPhraseRepository.Current.Get("/ContactUs/ApiUrl", "");
                //var apiUrl = "https://stage.adaniupdates.com/api/email/send";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("To", to),
                    new KeyValuePair<string, string>("From", from),
                    new KeyValuePair<string, string>("Subject", subject),
                    new KeyValuePair<string, string>("Body", HttpUtility.HtmlEncode( body))
                };

                var content = new FormUrlEncodedContent(postData);

                HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }
                else 
                {
                    return response.IsSuccessStatusCode;
                }
                
            }
        }
    }
}