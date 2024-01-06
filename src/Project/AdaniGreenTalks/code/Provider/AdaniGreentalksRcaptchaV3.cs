using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website.Provider
{
    public class AdaniGreentalksRcaptchaV3
    {
        public bool IsReCaptchValidV3(string reResponse)
        {
            Log.Info("Adani GreenTalks Google Rcaptcha V3 validation start here for response" + reResponse, this);
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6Le0pu0gAAAAAOgrah7pbbp55tiieAKO_0lFTNyV&response={reResponse}").Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                Log.Error("Adani GreenTalks Google Rcaptcha V3 validation failed" + res, this);
                return false;
            }
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true" || JSONdata.score <= 0.5)
            {
                Log.Error("Adani GreenTalks Google Rcaptcha V3 validation failed" + JSONdata, this);
         
                return false;
            }
            Log.Error("Adani GreenTalks Google Rcaptcha V3 validation successfull" + JSONdata, this);

            return true;
        }
    }
}