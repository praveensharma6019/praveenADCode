using Adani.SuperApp.Airport.Feature.Hotels.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.Services
{
    public class TopHotelsWebService : ITopHotelsWebService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public TopHotelsWebService(ILogRepository logRepository, IHelper helper, IWidgetService widgetservice)
        {
            _logRepository = logRepository;
            _helper = helper;
            _widgetservice = widgetservice;
        }

        public WidgetModel GetTopHotelsData(Rendering rendering)
        {
            WidgetModel widgetList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constants.RenderingParamField]);
                widgetList.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                widgetList.widget.widgetItems = GetDataList(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetTopHotelsData in TopHotelsWebService gives -> " + ex.Message);
            }
            return widgetList;
        }

        private List<object> GetDataList(Rendering rendering)
        {
            List<Object> DataList = new List<Object>();
            try
            {
                var datasourceItem = rendering.Item;
                // Null Check for datasource
                if (datasourceItem != null && datasourceItem.GetChildren() != null && datasourceItem.GetChildren().Count() > 0)
                {
                    TopHotelsModelWeb model = null;
                    foreach (Sitecore.Data.Items.Item item in datasourceItem.GetChildren())
                    {
                        model = new TopHotelsModelWeb();
                        model.title = item.Fields[Constants.Title] != null ? item.Fields[Constants.Title].Value : string.Empty;
                        model.description = item.Fields[Constants.Description] != null ? item.Fields[Constants.Description].Value : string.Empty;
                        model.image = item.Fields[Constants.Image] != null ? _helper.GetImageURL(item, Constants.Image) : string.Empty;
                        model.rating = string.IsNullOrEmpty(item[Constants.Rating].ToString()) ? 0 : Convert.ToInt32(item[Constants.Rating]);
                        model.isInternational = item.Fields[Constants.IsInternational] != null ? _helper.GetCheckboxOption(item.Fields[Constants.IsInternational]) : false;

                        string city = item.Fields[Constants.City] != null ? item.Fields[Constants.City].Value : string.Empty;
                        string sellingCountry = item.Fields[Constants.SellingCountry] != null ? item.Fields[Constants.SellingCountry].Value : string.Empty;
                        string isoCodeA2 = item.Fields[Constants.IsoCodeA2] != null ? item.Fields[Constants.IsoCodeA2].Value : string.Empty;
                        string country = item.Fields[Constants.Country] != null ? item.Fields[Constants.Country].Value : string.Empty;
                        string hotelSlug = item.Fields[Constants.SlugId] != null ? item.Fields[Constants.SlugId].Value : string.Empty;
                        string sellingCurrency = item.Fields[Constants.SellingCurrency] != null ? item.Fields[Constants.SellingCurrency].Value : string.Empty;
                        string searchValue = item.Fields[Constants.Hotel] != null ? item.Fields[Constants.Hotel].Value : string.Empty;
                        string hotelName = item.Fields[Constants.Hotel] != null ? item.Fields[Constants.Hotel].Value : string.Empty;
                        string adults = "2";
                        string rooms = "1";

                        DateTime currentDate = DateTime.Now;
                        string checkInDate = currentDate.ToString("yyyy-MM-dd");
                        string checkOutDate = currentDate.AddDays(1).ToString("yyyy-MM-dd");
                        int time = Convert.ToInt32(currentDate.ToString("HH"));
                        if(time > 17)
                        {
                            checkInDate = currentDate.AddDays(1).ToString("yyyy-MM-dd");
                            checkOutDate = currentDate.AddDays(2).ToString("yyyy-MM-dd");
                        }

                        model.dynamicLink = !string.IsNullOrEmpty(item.Fields[Constants.Link].Value) ? _helper.GetLinkURL(item, Constants.Link) + "?city=" + city + "&checkInDate=" + checkInDate +
                            "&checkOutDate=" + checkOutDate + "&adults=" + adults + "&rooms=" + rooms + "&sellingCurrency=" + sellingCurrency + "&sellingCountry=" + sellingCountry +
                            "&isoCodeA2=" + isoCodeA2 + "&country=" + country + "&hotelSlug=" + hotelSlug + "&searchValue=" + searchValue + "&hotelName=" + hotelName : string.Empty;

                        model.linkTarget = item.Fields[Constants.Link] != null ? _helper.LinkUrlTarget(item.Fields[Constants.Link]) : string.Empty;
                        DataList.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetDataList in TopHotelsWebService gives -> " + ex.Message);
            }
            return DataList;
        }

    }
}