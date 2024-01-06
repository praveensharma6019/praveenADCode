using Newtonsoft.Json.Linq;
using Sitecore.Feature.ValidateGoogleCaptcha.Models;
using Sitecore.Foundation.Dictionary.Repositories;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sitecore.Feature.ValidateGoogleCaptcha.Services
{
    public class ValidateCaptchaService : IValidateCaptchaService
    {   
        //public async Task<bool> IsReCaptchValidAsync(string reResponse)
        //    {
        //        using (HttpClient httpClient = new HttpClient())
        //        {
        //            string secretKey = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "");
        //            string apiUrl = "https://www.google.com/recaptcha/api/siteverify";
        //            string requestUri = $"{apiUrl}?secret={secretKey}&response={reResponse}";

        //            HttpResponseMessage response = await httpClient.GetAsync(requestUri);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                string jsonResponse = await response.Content.ReadAsStringAsync();
        //                dynamic captchaResult = JObject.Parse(jsonResponse);
        //                bool isSuccess = captchaResult.success;

        //                return isSuccess;
        //            }

        //            return false;
        //        }
        //    }

        public bool VerifyCaptcha(CaptchaData captchaData)
        {
            bool result;
            using (HttpClient httpClient = new HttpClient())
            {
                string secretKey = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "");
                string apiUrl = "https://www.google.com/recaptcha/api/siteverify";
                string requestUri = $"{apiUrl}?secret={secretKey}&response={captchaData.Request}";
                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                using (WebResponse webresponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webresponse.GetResponseStream()))
                    {
                        JObject jResponse = JObject.Parse(stream.ReadToEnd());
                        var isSuccess = jResponse.Value<bool>("success");
                        result = (isSuccess) ? true : false;
                        
                    }
                }

            }
            return result;
        }

        //V3 captcha method
        public bool IsV3CaptchValid(CaptchaData captchaData)
        {
            HttpClient httpClient = new HttpClient();
            var captchaResponse = captchaData.Request;
            string secretKey = DictionaryPhraseRepository.Current.Get("/V3CaptachaKey/SecretKey", "");
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var res = httpClient.GetAsync(requestUri).Result;

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