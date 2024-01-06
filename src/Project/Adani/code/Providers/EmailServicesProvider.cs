using Sitecore.Diagnostics;
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
            try
            {
                Log.Info("Inside try block of send email",to);
                using (var client = new HttpClient())
                {
                    var apiUrl = DictionaryPhraseRepository.Current.Get("/ContactUs/ApiUrl", "");
                    Log.Info("api url found for adani:" +apiUrl, apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("To", to),
                    new KeyValuePair<string, string>("From", from),
                    new KeyValuePair<string, string>("Subject", subject),
                    new KeyValuePair<string, string>("Body", HttpUtility.HtmlEncode( body))
                };
                    Log.Info("post data for send email api: "+ postData,postData);
                    var content = new FormUrlEncodedContent(postData);

                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;
                    Log.Info("Response received from api service : "+ response, response);
                    if (response.IsSuccessStatusCode == false)
                    {
                        Log.Info("Status code equals to  false : "+ response.StatusCode, response.StatusCode);
                        throw new Exception(response.Content.ReadAsStringAsync().Result);
                    }

                    else
                    {
                        Log.Info("Response  found : "+ response.StatusCode, response.StatusCode);
                        return response.IsSuccessStatusCode;
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Info("exception in send email api:  "+ex,ex);
                throw new Exception(ex.Message);
            }
          
        }
    }
}