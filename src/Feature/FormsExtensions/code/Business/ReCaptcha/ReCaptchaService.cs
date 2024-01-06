using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Feature.FormsExtensions.Models;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Feature.FormsExtensions.Business.ReCaptcha
{
    public class ReCaptchaService : IReCaptchaService
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string SecretKey;

        public ReCaptchaService(string SecretKey)
        {
            this.SecretKey = SecretKey;
        }

        public async Task<bool> Verify(string response, string SecretKey)
        {


            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", SecretKey),
                new KeyValuePair<string, string>("response", response)
            });
            var responseMessage = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", formContent);
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var reCaptchaResponse = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonString));

            return reCaptchaResponse.Success;
        }

        public bool VerifySync(string response, string SecretKey)
        {
            return AsyncHelpers.RunSync(() => Verify(response, SecretKey));
        }

        //V3 captcha method
        public bool IsV3CaptchValid(string response, string SecretKey)
        {
            HttpClient httpClient = new HttpClient();
            var captchaResponse = response;
            string secretKey = DictionaryPhraseRepository.Current.Get("/V3CaptachaKey/SecretKey", "");
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, SecretKey, captchaResponse);
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