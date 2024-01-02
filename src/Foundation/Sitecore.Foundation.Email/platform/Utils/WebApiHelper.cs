using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Web;
using Sitecore.Foundation.Email.Model;
using static System.Net.WebRequestMethods;

namespace Sitecore.Foundation.Email.Utils
{
    public static class WebApiHelper
    {
        public static ResponseModel SentEmail(ResponseModel sendMailModel)
        {
          
            try
            {
                using (var client = new HttpClient())
                {
                    //var baseurl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SendMailBaseUrl"]);
                    var baseurl = "https://stage.farmpik.com";

                    //var baseurl = Sitecore.Configuration.Settings.GetSetting("SendMailBaseUrl");
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(sendMailModel);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");


                    var postData = new List<KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("To", sendMailModel.To));
                    postData.Add(new KeyValuePair<string, string>("From", sendMailModel.From));
                    postData.Add(new KeyValuePair<string, string>("Cc", sendMailModel.Cc));
                    postData.Add(new KeyValuePair<string, string>("Bcc", sendMailModel.Bcc));
                    postData.Add(new KeyValuePair<string, string>("Subject", sendMailModel.Subject));
                    postData.Add(new KeyValuePair<string, string>("Body", HttpUtility.HtmlEncode(sendMailModel.Body)));


                    var content = new FormUrlEncodedContent(postData);

                    HttpResponseMessage response = client.PostAsync("/EmailApi/EmailService/SendEmail", content).Result;
                    if (response.IsSuccessStatusCode)
                    {

                        string resultContent = response.Content.ReadAsStringAsync().Result;
                        sendMailModel = JsonConvert.DeserializeObject<ResponseModel>(resultContent);

                    }
                    else
                    {
                        sendMailModel.isSucess = false;
                        sendMailModel.isError = true;

                    }
                }
            }
            catch (Exception ex)
            {
                // Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                sendMailModel.isError = true;
                sendMailModel.ErrorMessage = ex.Message;
                sendMailModel.isSucess = false;
            }
            return sendMailModel;
        }

    }
}