using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using Sitecore.Ports.Website.Models;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

namespace Sitecore.Ports.Website.Services
{
    public class CreateItemService
    {
        public bool CreateItem(TemplatedModel templateModel)
        {
            try
            {
                Log.Info("In CreateItem nethod templateModel received:" + templateModel, templateModel);
                using (var client = new HttpClient())
                {
                    Log.Info("In CreateItem nethod http client created", string.Empty);
                    var address = System.Web.HttpContext.Current.Request.Url;
                    var hostAddress = address.Scheme + "://" + address.Host;
                    var apiUrl = hostAddress + "/abcd/TemplateItem/CreateItem";
                    Log.Info("CreateItem apiUrl:" + apiUrl, apiUrl);
                    string ItemFieldsCollection = JsonConvert.SerializeObject(templateModel.templateFields);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("templateId", templateModel.templateId),
                    new KeyValuePair<string, string>("parentItem", templateModel.parentItem),
                    new KeyValuePair<string, string>("newItemName", templateModel.newItemName),
                    new KeyValuePair<string, string>("templateFields", ItemFieldsCollection)
                };
                    Log.Info("CreateItem postData:" + postData, postData);
                    var content = new FormUrlEncodedContent(postData);
                    Log.Info("request date before sending request:" + content, content);
                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;
                    Log.Info("response received from createItem Api:" + response, response);
                    Log.Info("response received from createItem Api:" + response.Content, response);
                    Log.Info("response received from createItem Api:" + response.RequestMessage, response);
                    Log.Info("response received from createItem Api:" + response.ReasonPhrase, response);
                    Log.Info("response received from createItem Api:" + response.StatusCode, response);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exception received in createItem Api:" + ex, ex);

                throw ex;
            }
        }
    }
}