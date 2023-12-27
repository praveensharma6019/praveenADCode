using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Services
{

    public class CallBackRepository : ICallBackRepository
    {
        private readonly IHelper _helper;
        private readonly ILogRepository logRepository;
        private readonly IAPIResponse serviceResponse;


        public CallBackRepository(ILogRepository _logRepository, IHelper helper,IAPIResponse serviceResponse)
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

        public List<ApiResponse> GetEmailResponse(CallBackFormInput input)
        {
            string serviceUrl = Sitecore.Configuration.Settings.GetSetting("EmailAPI");
            List<ApiResponse> apiResponse = null;

            try
            {
                var response = serviceResponse.GetAPIResponse("POST", serviceUrl, CreateRequestHeaders(), null, input);
                apiResponse = response != null ? JsonConvert.DeserializeObject<List<ApiResponse>>(response) : null;
            }
            catch (Exception ex)
            {

                this.logRepository.Error(ex.Message);
            }

            //if(callBackFormResponse != null)
            //{
            //    apiResponse.response.Add(callBackFormResponse);
            //}
            return apiResponse;
        }

        public CallBackFormLabels GetCallBackFormItem(Item currentItem)
        {
            CallBackFormLabels callBackForm = null;
            if (currentItem != null && currentItem.Fields != null && currentItem.Fields.Count > 0)
            {
                callBackForm = new CallBackFormLabels();
                callBackForm.FormTitle = currentItem.Fields["Title"].ToString();
                callBackForm.Description = currentItem.Fields["Description"].ToString();
                callBackForm.BannerImage = _helper.GetImageURL(currentItem, "Image");
                callBackForm.FullNameLabel = currentItem.Fields["FullNameLabel"].ToString();
                callBackForm.ContactNumberLabel = currentItem.Fields["ContactNumberLabel"].ToString();
                callBackForm.EmailAddressLabel = currentItem.Fields["EmailIdLabel"].ToString();
                callBackForm.OrganizationLabel = currentItem.Fields["OrganizationLabel"].ToString();
                callBackForm.ButtonText = currentItem.Fields["ButtonText"].ToString();
                callBackForm.CommercialEmailTo = currentItem.Fields["CommercialEmailTo"].ToString();
                callBackForm.AirlinePartnershipEmailTo = currentItem.Fields["AirlinePartnershipEmailTo"].ToString();
                callBackForm.SuccessMessage = currentItem.Fields["SuccessMessage"].ToString();
                callBackForm.FailureMessage = currentItem.Fields["FailureMessage"].ToString();
            }
            return callBackForm;
        }
    }
}