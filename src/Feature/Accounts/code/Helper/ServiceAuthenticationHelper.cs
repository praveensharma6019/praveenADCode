using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;

namespace Sitecore.Feature.Accounts.Helper
{
    public class ServiceAuthenticationHelper
    {
        public static string AddAuthorizationMethod()
        {
            var authenticationBytes = Encoding.ASCII.GetBytes(ConfigurationHelper.VAPTServiceUserName + ":" + ConfigurationHelper.ServicePassword);
            var authenticationBytes64Value = System.Convert.ToBase64String(authenticationBytes);
            return authenticationBytes64Value;

        }
        public static string FetchTokenValue(string authenticationBytes64Value)
        {
            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationBytes64Value);
                client.DefaultRequestHeaders.Add("Token", "fetch");
                var url = ConfigurationHelper.VAPTServiceUrl;
                var uri = new Uri(url);
                HttpResponseMessage response = client.PostAsync(uri, null).Result;
                var token = response.Headers.GetValues("Token").FirstOrDefault();
                return token;
            }
        }
    }
}