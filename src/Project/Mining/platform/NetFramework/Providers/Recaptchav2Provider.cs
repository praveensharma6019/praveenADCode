﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace Project.Mining.Website.Providers
{
    public class Recaptchav2Provider : Controller
    {
        public bool IsReCaptchValid(string reResponse)
        {
            using (var client = new HttpClient())
            {
                string SecretKey =  "";
                var apiUrlCaptcha = "https://stage.adaniuat.com/api/ValidateCaptcha/ValidateReCaptcha";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("reResponse", reResponse),
                    new KeyValuePair<string, string>("SecretKey", SecretKey)
                };
                var content = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = client.PostAsync(apiUrlCaptcha, content).Result;
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return response.IsSuccessStatusCode;
                }
            }
        }
        public bool IsReCaptchValidV3(string reResponse)
        {
            using (var client = new HttpClient())
            {
                string SecretKey = "6Lf6VgUgAAAAAFq77YhGWeUDGGlXskkJvvPqBQjJ";
                var apiUrlCaptcha = "https://stage.adaniuat.com/api/ValidateCaptcha/ValidateReCaptchaV3";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("reResponse", reResponse),
                    new KeyValuePair<string, string>("SecretKey", SecretKey)
                };
                var content = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = client.PostAsync(apiUrlCaptcha, content).Result;
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return response.IsSuccessStatusCode;
                }
            }
        }
    }
}