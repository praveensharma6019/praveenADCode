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
using Sitecore.StringExtensions;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class ServiceListService : IServiceListService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public ServiceListService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetServiceListData(Rendering rendering)
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

                _logRepository.Error("GetServiceListData in ServiceListService gives -> " + ex.Message);
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
                    ServiceListModel model = null;
                    foreach (Sitecore.Data.Items.Item item in datasourceItem.GetChildren())
                    {
                        model = new ServiceListModel();
                        model.tabTitle = item.Fields[Constant.AutoId] == null || string.IsNullOrEmpty(item.Fields[Constant.AutoId].Value) ? item.Name : item.Fields[Constant.AutoId].Value;
                        if(item.Children != null)
                        {
                            var servicesList = item.GetChildren();
                            model.servicesList.AddRange(servicesList.Select(x => new Models.Services
                            {
                                title = x.Fields[Constant.Title] != null ? x.Fields[Constant.Title].Value : string.Empty,
                                serviceId = x.Fields[Constant.ServiceID] != null ? x.Fields[Constant.ServiceID].Value : string.Empty,
                                serviceUrl = x.Fields[Constant.ServiceUrl] != null ? _helper.LinkUrl(x.Fields[Constant.ServiceUrl]) : string.Empty
                            }));
                        }
                        DataList.Add(model);
                    }
                }
            }

            catch (Exception ex)
            {
                _logRepository.Error("GetDataList in ServiceListService gives -> " + ex.Message);
            }
            return DataList;
        }

    }
}