using Newtonsoft.Json.Linq;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Sitecore.Marathon.Website.Services
{
    public static class Captcha
    {
        public static bool IsReCaptchValidV3(string reResponse)
        {
            HttpClient httpClient = new HttpClient();
            string secretKey = DictionaryPhraseRepository.Current.Get("/GoogleCaptcha/SecretKey", "");
            decimal score = Decimal.Parse(DictionaryPhraseRepository.Current.Get("/GoogleCaptcha/Score", ""));
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={reResponse}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= score)
            {
                return false;
            }

            return true;
        }
    }
}