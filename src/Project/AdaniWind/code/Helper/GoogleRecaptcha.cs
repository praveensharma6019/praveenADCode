using Newtonsoft.Json.Linq;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Net;
using System.Net.Http;

namespace Sitecore.AdaniWind.Website.Helper
{
    public static class GoogleRecaptcha
    {
        public static bool IsReCaptchValidV3(string reResponse)
        {
            HttpClient httpClient = new HttpClient();
            string secretKey = DictionaryPhraseRepository.Current.Get("/GoogleCaptcha/SecretKey", "");
            string captchaAPI = DictionaryPhraseRepository.Current.Get("/GoogleCaptcha/API", "");
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