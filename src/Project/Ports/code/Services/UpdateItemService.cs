using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using Sitecore.Ports.Website.Models;
using Newtonsoft.Json;

namespace Sitecore.Ports.Website.Services
{
    public class UpdateItemService
    {
        public bool UpdateItem(TemplatedModel model)
        {
            
            using (var client = new HttpClient())
            {
                var address = System.Web.HttpContext.Current.Request.Url;
                var hostAddress = address.Scheme + "://" + address.Host;
                var apiUrl = hostAddress + "/abcd/TemplateItem/UpdateItem";
                string ItemFieldsCollection = JsonConvert.SerializeObject(model.templateFields);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));                
                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("itemPath", model.itemPath),
                    new KeyValuePair<string, string>("templateFields", ItemFieldsCollection)
                };
                var content = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }          
        }
    }
}