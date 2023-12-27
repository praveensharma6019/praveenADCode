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
    public class TopHotelsAppService : ITopHotelsAppService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public TopHotelsAppService(ILogRepository logRepository, IHelper helper, IWidgetService widgetservice)
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

                _logRepository.Error("GetTopHotelsData in TopHotelsAppService gives -> " + ex.Message);
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
                    TopHotelsModelApp model = null;
                    foreach (Sitecore.Data.Items.Item item in datasourceItem.GetChildren())
                    {
                        model = new TopHotelsModelApp();
                        model.title = item.Fields[Constants.Title] != null ? item.Fields[Constants.Title].Value : string.Empty;
                        model.description = item.Fields[Constants.Description] != null ? item.Fields[Constants.Description].Value : string.Empty;
                        model.mobileImage = item.Fields[Constants.Image] != null ? _helper.GetImageURL(item, Constants.Image) : string.Empty;
                        model.rating = string.IsNullOrEmpty(item[Constants.Rating].ToString()) ? 0 : Convert.ToInt32(item[Constants.Rating]);
                        model.isInternational = item.Fields[Constants.IsInternational] != null ? _helper.GetCheckboxOption(item.Fields[Constants.IsInternational]) : false;
                        model.cityId = item.Fields[Constants.CityId] != null ? item.Fields[Constants.CityId].Value : string.Empty;
                        model.hotelId = item.Fields[Constants.HotelId] != null ? item.Fields[Constants.HotelId].Value : string.Empty;
                        model.isoCodeA2 = item.Fields[Constants.IsoCodeA2] != null ? item.Fields[Constants.IsoCodeA2].Value : string.Empty;
                        model.country = item.Fields[Constants.Country] != null ? item.Fields[Constants.Country].Value : string.Empty;
                        model.slugId = item.Fields[Constants.SlugId] != null ? item.Fields[Constants.SlugId].Value : string.Empty;
                        model.hotel = item.Fields[Constants.Hotel] != null ? item.Fields[Constants.Hotel].Value : string.Empty;
                        model.city = item.Fields[Constants.City] != null ? item.Fields[Constants.City].Value : string.Empty;
                        DataList.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetDataList in TopHotelsAppService gives -> " + ex.Message);
            }
            return DataList;
        }

    }
}