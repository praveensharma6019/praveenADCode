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
    public class FAQSearchService : IFAQSearchService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public FAQSearchService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetFAQSearchData(Rendering rendering, Item contextItem)
        {
            WidgetModel widgetList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                widgetList.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                widgetList.widget.widgetItems = GetFAQSearchDataList(contextItem);
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetFAQSearchData in FAQSearchService gives -> " + ex.Message);
            }
            return widgetList;
        }

        private List<object> GetFAQSearchDataList(Item contextItem)
        {
            List<Object> DataList = new List<Object>();
            try
            {
                if (contextItem != null)
                {
                    FAQSearchModel model = new FAQSearchModel();
                    model.title = contextItem.Fields[Constant.Title] != null ? contextItem.Fields[Constant.Title].Value : string.Empty;
                    model.description = contextItem.Fields[Constant.Description] != null ? contextItem.Fields[Constant.Description].Value : string.Empty;
                    model.searchText = contextItem.Fields[Constant.SearchPlaceholderText] != null ? contextItem.Fields[Constant.SearchPlaceholderText].Value : string.Empty;
                    model.itemNotFoundText = contextItem.Fields[Constant.SearchItemNotFound] != null ? contextItem.Fields[Constant.SearchItemNotFound].Value : string.Empty;
                    DataList.Add(model);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetFAQSearchDataList in FAQSearchService gives -> " + ex.Message);
            }
            return DataList;
        }

    }
}