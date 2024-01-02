using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Repositories
{

    public class FeedbackRepository : IFeedbackRepository
    {
      
        private readonly IAPIResponse serviceResponse;
        private readonly ILogRepository logRepository;


        public FeedbackRepository(IAPIResponse _serviceResponse, ILogRepository _logRepository)
        {
            this.serviceResponse = _serviceResponse;
            this.logRepository = _logRepository;
        }


        /// <summary>
        /// Code to create Headers for the API
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CreateRequestHeaders(FeedbackForm feedback)
        {
            Random _random = new Random();
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { Constants.ContentType, Constants.ApplicationJson },
                { Constants.Accept, Constants.PlainText },
                { Constants.TraceId, _random.Next(600, 40000).ToString() },
                { Constants.AgentId, feedback.MobileNumber },
                { Constants.ChannelId, Constants.Sitecore}
            };
            return headers;
        }


        /// <summary>
        /// To get the status of service
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        FeedbackFormResponse IFeedbackRepository.GetSubmitFormDetails(FeedbackForm feedback)
        {

            //stubbed since FED values are not capturing
            
            //FlightDetail flightDetail = new FlightDetail()
            //{
            //    FlightDate = "03-01-2022",
            //    FlightNumber = "12345"
            //};
            //feedback.FlightDetail = flightDetail;
            FeedbackFormResponse serviceResponse = GetIncidentforSubmitForm(feedback);

            
            return serviceResponse;
        }

        /// <summary>
        /// To get the incident details
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        public FeedbackFormResponse GetIncidentforSubmitForm(FeedbackForm feedback)
        {
            string serviceUrl = Sitecore.Configuration.Settings.GetSetting(Constants.FeedbackAPI);

            FeedbackFormResponse feedbackFormResponse = new FeedbackFormResponse();

            try
            {
                var response = serviceResponse.GetAPIResponse(Constants.Post, serviceUrl, CreateRequestHeaders(feedback), null, feedback);

                feedbackFormResponse = response != null ? JsonConvert.DeserializeObject<FeedbackFormResponse>(response) : null;
            }
            catch (Exception ex)
            {

                this.logRepository.Error(ex.Message);
            }

            return feedbackFormResponse;
        }

               

        public SubmitViewModel GetSubmitFormItem(Item currentItem, SubmitViewModel submitViewModel)
        {
            if (currentItem != null && currentItem.Fields != null && currentItem.Fields.Count > 0)
            {
                SubmitFormItem submitFormItem = new SubmitFormItem()
                {
                    SelectAirportLabel = currentItem.Fields[Template.SubmitForm.Fields.SelectAirportId].ToString(),
                    IssueFacedLabel = currentItem.Fields[Template.SubmitForm.Fields.IssueFacedId].ToString(),
                    FullNameLabel = currentItem.Fields[Template.SubmitForm.Fields.FullNameId].ToString(),
                    MobileNoLabel = currentItem.Fields[Template.SubmitForm.Fields.MobileNoId].ToString(),
                    WritePlacholderLabel = currentItem.Fields[Template.SubmitForm.Fields.WritePlacholderId].ToString(),
                    HelpTextLabel = currentItem.Fields[Template.SubmitForm.Fields.HelpTextId].ToString(),
                    EmailIDLabel = currentItem.Fields[Template.SubmitForm.Fields.EmailIDId].ToString(),
                    AgreeLabel = currentItem.Fields[Template.SubmitForm.Fields.AgreeId].ToString(),
                    SubmitButtonLabel = currentItem.Fields[Template.SubmitForm.Fields.SubmitButtonId].ToString(),
                    ModalLabel = currentItem.Fields[Template.SubmitForm.Fields.ModalLabelId].ToString(),
                    CloseLabel=currentItem.Fields[Template.SubmitForm.Fields.CloseLabelId].ToString()

                };
                submitFormItem.IssueList = new List<string>();
                
                var IssueListItem = new MultilistField(currentItem.Fields[new ID(Template.SubmitForm.Fields.IssueListId)]).GetItems();//GetMultiListValueItems(currentItem, new ID(Template.SubmitForm.Fields.IssueListId));

                foreach (var option in IssueListItem)
                {
                    submitFormItem.IssueList.Add(option.Fields[Template.SubmitForm.Fields.IssueValueId].Value);
                }

                submitFormItem.AirportList = new List<Airports>();
               

                MultilistField airportList = currentItem.Fields[Template.SubmitForm.Fields.AirportListValueId];

                if (airportList!=null && airportList.GetItems()!=null)
                {
                    List<Airports> airportListItem = new List<Airports>();

                    foreach (var airportItem in airportList.GetItems())
                    {
                        Airports airports = new Airports()
                        {
                            AirportCode = airportItem.Fields[Template.SubmitForm.Fields.AirportCodeValueId]?.Value,
                            AirportName = airportItem.Fields[Template.SubmitForm.Fields.AirportNameValueId]?.Value
                        };

                        airportListItem.Add(airports);
                    }

                    submitFormItem.AirportList = airportListItem;
                }
                
                submitViewModel.SubmitFormItems = submitFormItem;
                submitViewModel.FeedbackFormData = new FeedbackForm();
            }

            return submitViewModel;

        }

       
    }
}