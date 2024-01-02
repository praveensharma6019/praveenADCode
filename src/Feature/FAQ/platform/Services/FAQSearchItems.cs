using Adani.SuperApp.Airport.Feature.FAQ.Models;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Feature.FAQ.Constant;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Fields;

namespace Adani.SuperApp.Airport.Feature.FAQ.Services
{
    public class FAQSearchItems : IFAQSearchItems
    {
        private readonly IAPIResponse faqResponse;
        private readonly ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper helper;
        int cnt = 0;

        public FAQSearchItems(IAPIResponse _faqResponse, ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper _helper)
        {
            this.faqResponse = _faqResponse;
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this.helper = _helper;
        }

        public FAQResponseData GetSolrFAQData(ref Filters filter)
        {
            FAQResponseData responseData = new FAQResponseData();
            ResultData resultData = new ResultData();
            resultData = ProcessFilter(filter);
            if (resultData != null)
            {
                responseData.status = true;
                responseData.data = resultData;
            }
            return responseData;
        }

        private ResultData ProcessFilter(Filters filter)
        {

            ResultData faqResultData = new ResultData();
            InitDictionary(ref faqResultData);
            try
            {
                if (string.IsNullOrEmpty(filter.airportCode))
                {
                    ID[] airportPages = new ID[] {
                        Templates.BombayAirportPageID,
                        Templates.AhemedabadAirportPageID,
                        Templates.LucknowAirportPageID,
                        Templates.JaipurAirportPageID,
                        Templates.GuwahatiAirportPageID,
                        Templates.MangaluruAirportPageID,
                        Templates.ThiruvanthapuramAirportPageID};

                    ID defaultPageID = GetAirportPageID(Constant.Constant.Other);
                    GetFAQList(defaultPageID, filter.serviceType, ref faqResultData);
                    foreach (ID pageID in airportPages)
                    {
                        GetFAQList(pageID, filter.serviceType, ref faqResultData);
                    }
                }
                else if (!string.IsNullOrEmpty(filter.airportCode))
                {
                    ID defaultPageID = GetAirportPageID(Constant.Constant.Other);
                    GetFAQList(defaultPageID, filter.serviceType, ref faqResultData);

                    ID pageID = GetAirportPageID(filter.airportCode);
                    GetFAQList(pageID, filter.serviceType, ref faqResultData);
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("ParseFilter Method in FAQSearchItems Class gives error -> " + ex.Message);
            }
            return faqResultData;
        }

        private void InitDictionary(ref ResultData resultData)
        {
            try
            {
                resultData.flightBookingDictionary = new Dictionary<string, FAQResultData>();
                resultData.dutyFreeDictionary = new Dictionary<string, FAQResultData>();
                resultData.pranaamDictionary = new Dictionary<string, FAQResultData>();
            }
            catch (Exception ex)
            {
                logRepository.Error("InitDictionary Method in FAQSearchItems Class gives error -> " + ex.Message);
            }
        }

        private ID GetAirportPageID(string airportCode)
        {
            ID airportPageID = null;
            try
            {
                switch (airportCode)
                {
                    case "BOM":
                        airportPageID = Templates.BombayAirportPageID;
                        break;
                    case "AMD":
                        airportPageID = Templates.AhemedabadAirportPageID;
                        break;
                    case "GAU":
                        airportPageID = Templates.GuwahatiAirportPageID;
                        break;
                    case "JAI":
                        airportPageID = Templates.JaipurAirportPageID;
                        break;
                    case "LKO":
                        airportPageID = Templates.LucknowAirportPageID;
                        break;
                    case "IXE":
                        airportPageID = Templates.MangaluruAirportPageID;
                        break;
                    case "TRV":
                        airportPageID = Templates.ThiruvanthapuramAirportPageID;
                        break;
                    case "ADLONE":
                    case "OTHER":
                        airportPageID = Templates.HomePageID;
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetAirportPageID Method in FAQSearchItems Class gives error -> " + ex.Message);
            }
            return airportPageID;
        }

        private string GetAirportName(string airportCode)
        {
            string airportName = string.Empty;
            try
            {
                switch (airportCode)
                {
                    case "BOM":
                        airportName = Constant.Constant.MumbaiAirport;
                        break;
                    case "AMD":
                        airportName = Constant.Constant.AhmedabadAirport;
                        break;
                    case "GAU":
                        airportName = Constant.Constant.GuwahatiAirport;
                        break;
                    case "JAI":
                        airportName = Constant.Constant.JaipurAirport;
                        break;
                    case "LKO":
                        airportName = Constant.Constant.LucknowAirport;
                        break;
                    case "IXE":
                        airportName = Constant.Constant.MangluruAirport;
                        break;
                    case "TRV":
                        airportName = Constant.Constant.ThiruvanthapuramAirport;
                        break;
                    case "ADLONE":
                        airportName = Constant.Constant.AdaniOne;
                        break;
                    case "OTHER":
                        airportName = Constant.Constant.OtherAirport;
                        break;
                    default:
                        airportName = Constant.Constant.OtherAirport;
                        break;
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetAirportName Method in FAQSearchItems Class gives error -> " + ex.Message);
            }
            return airportName;
        }

        private void GetFAQList(ID catItemId, string serviceType, ref ResultData resultData)
        {
            try
            {
                Item catItem = Sitecore.Context.Database.GetItem(catItemId);

                if (catItem != null)
                {
                    foreach (Item childPage in catItem.Children)
                    {
                        if (childPage != null && childPage.TemplateID == Templates.FAQLandingPageTemplateID)
                        {
                            foreach (Item faqPage in childPage.Children)
                            {
                                if (!string.IsNullOrEmpty(faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString()))
                                {
                                    if (faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString() == Constant.Constant.FlightBooking || faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString() == Constant.Constant.DutyFree || faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString() == Constant.Constant.Pranaam)
                                    {
                                        string defaultDeviceID = Constant.Constant.DefaultDeviceId;
                                        var layoutField = new LayoutField(faqPage.Fields[Sitecore.FieldIDs.FinalLayoutField]);
                                        var layoutDefinition = LayoutDefinition.Parse(layoutField.Value);
                                        var deviceDefinition = layoutDefinition.GetDevice(defaultDeviceID);
                                        foreach (RenderingDefinition rendering in deviceDefinition.Renderings)
                                        {
                                            if (rendering.ItemID == Templates.FAQAccordionServiceRenderingID.ToString())
                                            {
                                                FAQResultData faqResultData = new FAQResultData();
                                                faqResultData.faqData = new FAQData();
                                                switch (faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString())
                                                {
                                                    case Constant.Constant.DutyFree:
                                                        if (faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString() == serviceType || string.IsNullOrEmpty(serviceType))
                                                        {
                                                            if (catItem.Fields[Constant.Constant.AirportCode] != null)
                                                                faqResultData.airportName = GetAirportName(!string.IsNullOrEmpty(catItem.Fields[Constant.Constant.AirportCode].ToString()) ? catItem.Fields[Constant.Constant.AirportCode].ToString() : string.Empty);
                                                            else
                                                                faqResultData.airportName = GetAirportName(string.Empty);
                                                            faqResultData.faqData = GetFaqData(faqPage, rendering.Datasource);
                                                            resultData.dutyFreeDictionary.Add(cnt + "-" + faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString(), faqResultData);
                                                            cnt++;
                                                        }
                                                        break;
                                                    case Constant.Constant.FlightBooking:
                                                        if (faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString() == serviceType || string.IsNullOrEmpty(serviceType))
                                                        {
                                                            if (catItem.Fields[Constant.Constant.AirportCode] != null)
                                                                faqResultData.airportName = GetAirportName(!string.IsNullOrEmpty(catItem.Fields[Constant.Constant.AirportCode].ToString()) ? catItem.Fields[Constant.Constant.AirportCode].ToString() : string.Empty);
                                                            else
                                                                faqResultData.airportName = GetAirportName(string.Empty);
                                                            faqResultData.faqData = GetFaqData(faqPage, rendering.Datasource);
                                                            resultData.flightBookingDictionary.Add(cnt + "-" + faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString(), faqResultData);
                                                            cnt++;
                                                        }
                                                        break;
                                                    case Constant.Constant.Pranaam:
                                                        if (faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString() == serviceType || string.IsNullOrEmpty(serviceType))
                                                        {
                                                            if (catItem.Fields[Constant.Constant.AirportCode] != null)
                                                                faqResultData.airportName = GetAirportName(!string.IsNullOrEmpty(catItem.Fields[Constant.Constant.AirportCode].ToString()) ? catItem.Fields[Constant.Constant.AirportCode].ToString() : string.Empty);
                                                            else
                                                                faqResultData.airportName = GetAirportName(string.Empty);
                                                            faqResultData.faqData = GetFaqData(faqPage, rendering.Datasource);
                                                            resultData.pranaamDictionary.Add(cnt + "-" + faqPage.Fields[Constant.Constant.FAQPageTitleField].ToString(), faqResultData);
                                                            cnt++;
                                                        }
                                                        break;
                                                    default: break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetFAQList Method in FAQSearchItems Class gives error -> " + ex.Message);
            }
        }

        public FAQData GetFaqData(Item faqPage, string dataSource)
        {
            logRepository.Info("GetFaqData Initiated");
            FAQData _obj = new FAQData();
            try
            {
                if (!string.IsNullOrEmpty(dataSource))
                {
                    Item faqDatasource = Sitecore.Context.Database.GetItem(dataSource);
                    if (faqDatasource != null)
                    {
                        _obj.title = !string.IsNullOrEmpty(faqPage.Fields[Templates.FAQ.Fields.ApiTitle].Value.ToString()) ? faqPage.Fields[Templates.FAQ.Fields.ApiTitle].Value.ToString() : string.Empty;
                        _obj.faqHTML = !string.IsNullOrEmpty(faqPage.Fields[Templates.FAQ.Fields.ApiFAQHTML].Value.ToString()) ? faqPage.Fields[Templates.FAQ.Fields.ApiFAQHTML].Value.ToString() : string.Empty;
                        if (!string.IsNullOrEmpty(faqPage[Templates.FAQ.Fields.ApiCTAKey]))
                        {
                            _obj.ctaURL = helper.GetLinkURL(faqPage, Templates.FAQ.Fields.ApiCTAKey);
                            _obj.ctaText = helper.GetLinkText(faqPage, Templates.FAQ.Fields.ApiCTAKey);
                        }
                        List<FAQCard> faqList = new List<FAQCard>();
                        foreach (Item item in faqDatasource.Children)
                        {
                            FAQCard cardItem = new FAQCard();
                            cardItem.title = !string.IsNullOrEmpty(item.Fields[Templates.FAQApiCard.Fields.Question].Value.ToString()) ? item.Fields[Templates.FAQApiCard.Fields.Question].Value.ToString() : "";
                            cardItem.body = !string.IsNullOrEmpty(item.Fields[Templates.FAQApiCard.Fields.Answer].Value.ToString()) ? item.Fields[Templates.FAQApiCard.Fields.Answer].Value.ToString() : "";
                            faqList.Add(cardItem);
                        }
                        _obj.list = faqList;
                    }
                }
                else return null;
            }
            catch (Exception ex)
            {
                logRepository.Error("GetFaqData throws Exception -> " + ex.Message);
            }

            return _obj;
        }
    }
}