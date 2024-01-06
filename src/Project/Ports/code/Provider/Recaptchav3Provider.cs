using Newtonsoft.Json.Linq;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Ports.Website.Provider
{
    public class Recaptchav3Provider : Controller
    {
        public bool IsReCaptchValid(string reResponse)
        {
            HttpClient httpClient = new HttpClient();

            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6Lfwlk8mAAAAAMvzP3T-slqtuElgqDi41Yqx_m2Q&response={reResponse}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= 0.5)
            {
                return false;
            }

            return true;
        }
    }
}