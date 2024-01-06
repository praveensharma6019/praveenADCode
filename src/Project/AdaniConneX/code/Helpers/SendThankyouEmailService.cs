using Sitecore.Foundation.Email.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Foundation.Email;
using Sitecore.Foundation.Email.Controllers;
using Sitecore.AdaniConneX.Website.Templates;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using Sitecore.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Sitecore.AdaniConneX.Website.Models;
using Sitecore.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Sitecore.Data.Items;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Net.Http.Headers;

namespace Sitecore.AdaniConneX.Website.Helpers
{
    public class SendThankyouEmailService : Controller
    {

        public async Task<bool> ThankyouNotification(string EmailTo, string EmailBody, Sitecore.Data.Items.Item EmailTemplate)
        {
            var result = false;
            try
            {
                var To = string.Empty;

                Sitecore.Data.Items.Item EmailTemplateContext = Context.Database.GetItem(EmailTemplate.ID);
                if (string.IsNullOrEmpty(EmailTemplateContext.Fields[Templates.EmailTemplate.DatasourceFields.To].Value))
                {
                    To = EmailTo;
                }
                else
                {
                    To = EmailTemplateContext.Fields[Templates.EmailTemplate.DatasourceFields.To].Value;
                }

                using (var client = new HttpClient())
                {
                    var apiUrl = DictionaryPhraseRepository.Current.Get("/EmailService/APIURL", "");
                    Log.Info("api url found for adani:" + apiUrl, apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("To", To),
                    new KeyValuePair<string, string>("From", EmailTemplateContext.Fields[Templates.EmailTemplate.DatasourceFields.From].Value),
                    new KeyValuePair<string, string>("Subject", EmailTemplateContext.Fields[Templates.EmailTemplate.DatasourceFields.Subject].Value),
                    new KeyValuePair<string, string>("Body", HttpUtility.HtmlEncode(EmailBody))
                };
                    Log.Info("post data for send email api: " + postData, postData);
                    var content = new FormUrlEncodedContent(postData);

                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;
                    Log.Info("Response received from api service : " + response, response);
                    if (response.IsSuccessStatusCode != false)
                    {

                        Log.Info("Response  found : " + response.StatusCode, response.StatusCode);
                        return true;
                    }
                    else
                    {
                        Log.Info("Status code equals to  false : " + response.StatusCode, response.StatusCode);
                        throw new Exception(response.Content.ReadAsStringAsync().Result);
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error("something went wrong", ex.Message);
                return result;
            }
        }
    }
}