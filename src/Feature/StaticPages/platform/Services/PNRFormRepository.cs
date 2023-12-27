using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using Sitecore.Pipelines.LoadLayout;
using Sitecore.Shell.Applications.ContentEditor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Services
{

    public class PNRFormRepository : IPNRRepository
    {
        private readonly IHelper _helper;
        private readonly ILogRepository logRepository;
        private readonly IAPIResponse serviceResponse;


        public PNRFormRepository(ILogRepository _logRepository, IHelper helper,IAPIResponse serviceResponse)
        {
            this.logRepository = _logRepository;
            this._helper = helper;
            this.serviceResponse = serviceResponse;
        }

        private Dictionary<string,string> CreateRequestHeaders()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { Template.ContentType, Template.ApplicationJson },
                { Template.Accept, Template.AcceptType },
            };
            return headers;
        }

        public PNRFormResponse GetPNRResponse(PNRFormInput input)
        {
            string serviceUrl = Sitecore.Configuration.Settings.GetSetting("PNRAPI");
            PNRFormResponse apiResponse = new PNRFormResponse();

            try
            {
                var response = serviceResponse.GetAPIResponse("POST", serviceUrl, CreateRequestHeaders(), null, input);
                apiResponse = response != null ? JsonConvert.DeserializeObject<PNRFormResponse> (response) : null;
            }
            catch (Exception ex)
            {
                this.logRepository.Warn(apiResponse.email);
                this.logRepository.Error(ex.Message);
            }

            return apiResponse;
        }

        public PNRFormLabels GetPNRFormItem(Item currentItem)
        {
            PNRFormLabels formLabels = null;
            if (currentItem != null && currentItem.Fields != null && currentItem.Fields.Count > 0)
            {
                formLabels = new PNRFormLabels();
                formLabels.FormTitle = currentItem.Fields["Title"].ToString();
                formLabels.FirstNameLabel = currentItem.Fields["FirstNameLabel"].ToString();
                formLabels.LastNameLabel = currentItem.Fields["LastNameLabel"].ToString();
                formLabels.PNR = currentItem.Fields["PNRLabel"].ToString();
                formLabels.ContactNumberLabel = currentItem.Fields["MobileLabel"].ToString();
                formLabels.ButtonText = currentItem.Fields["ButtonText"].ToString();
                formLabels.Description = currentItem.Fields["Description"].ToString();
                formLabels.FooterDescription = currentItem.Fields["FooterDescription"].ToString();
                formLabels.EmailLabel = currentItem.Fields["EmailLabel"].ToString();
            }
            return formLabels;
        }

        public bool IsReCaptchValid(string responseData)
        {
            var result = false;
            var captchaResponse = responseData;
            var secretKey = Sitecore.Configuration.Settings.GetSetting("reCaptchaSecretKey");
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);



            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }

    }
}