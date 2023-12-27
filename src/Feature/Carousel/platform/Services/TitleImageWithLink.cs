using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class TitleImageWithLink : ITitleImageWithLink
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public TitleImageWithLink(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetData(Rendering rendering)
        {
            WidgetModel widgetList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                widgetList.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                widgetList.widget.widgetItems = GetDataList(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetData in TitleImageWithLink gives -> " + ex.Message);
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
                    TitleImageWithLinkModel model = null;
                    foreach (Sitecore.Data.Items.Item item in datasourceItem.GetChildren())
                    {
                        model = new TitleImageWithLinkModel();
                        model.desktopImage = item.Fields[Constant.DeskImage] != null ? _helper.GetImageURL(item, Constant.DeskImage) : String.Empty;
                        model.mobileImage = item.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(item, Constant.MobileImage) : String.Empty;
                        model.title = item.Fields[Constant.Title] != null ? item.Fields[Constant.Title].Value : String.Empty;
                        model.link = item.Fields[Constant.Link] != null ? _helper.GetLinkURL(item, Constant.Link) : String.Empty;
                        model.isRestricted = item.Fields[Constant.IsRestricted] != null ? _helper.GetCheckboxOption(item.Fields[Constant.IsRestricted]) : false;
                        model.isUrl = item.Fields[Constant.IsUrl] != null ? _helper.GetCheckboxOption(item.Fields[Constant.IsUrl]) : false;
                        model.uniqueId = item.Fields[Constant.UniqueId] != null ? item.Fields[Constant.UniqueId].Value : String.Empty;
                        model.tags.name = item.Fields[Constant.name] != null ? item.Fields[Constant.name].Value : String.Empty;
                        model.tags.textColor = item.Fields[Constant.name] != null ? item.Fields[Constant.textColor].Value : String.Empty;
                        model.tags.backgroundColor = item.Fields[Constant.name] != null ? item.Fields[Constant.backgroundColor].Value : String.Empty;
                        DataList.Add(model);
                    }
                } 
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetDataList in TitleImageWithLink gives -> " + ex.Message);
            }
            return DataList;
        }

    }
}