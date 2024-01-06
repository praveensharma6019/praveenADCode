using Adani.BAU.Transmission.Feature.Media.Platform.Models;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Adani.BAU.Transmission.Foundation.Theming.Platform.Services;
using Sitecore.Mvc.Presentation;
using Adani.BAU.Transmission.Foundation.Logging.Platform.Repositories;
using System;
using Adani.BAU.Transmission.Foundation.Theming.Platform.Models;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Services
{
    public class ServiceList : IServiceList
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public ServiceList(ILogRepository logRepository, IWidgetService widgetService, IHelper helper)
        {

            this._logRepository = logRepository;
            this._widgetservice = widgetService;
            this._helper = helper;
        }
        public services GetServicesData(Rendering rendering, string queryString)
        {
            services services = new services();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]);
                if (widget != null)
                {
                   // WidgetService widgetService = new WidgetService();
                    services.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    services.widget = new WidgetItem();
                }
                services.widget.widgetItems = GetServiceListData(rendering,queryString);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" Adani.BAU.Transmission.Feature.Media.Platform.ServicesServiceList GetServicesData gives -> " + ex.Message);
            }

            return services;
        }

        private List<object> GetServiceListData(Rendering rendering, string queryString)
        {
            List<object> serviceList = new List<object>();
            try
            {
                
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? RenderingContext.Current.Rendering.Item 
                : null;
                // Null Check for datasource
                if (datasource == null && datasource.Children.Count() == 0)
                {
                    throw new NullReferenceException("Adani.BAU.Transmission.Feature.Media.Platform.Services.GetServiceListData => Rendering Datasource is Empty");
                }

                List<Item> childList = datasource.Children.Where(x => x.TemplateID == Templates.ServicesListCollection.ServiceItemTemplateID).ToList();
                foreach (var child in childList)
                {
                    ServiceItem serviceItem = new ServiceItem();
                    serviceItem.uniqueId = !string.IsNullOrEmpty(child.Fields["UniqueID"].Value.ToString()) ? child.Fields["UniqueID"].Value.ToString() : "";
                    serviceItem.title = !string.IsNullOrEmpty(child.Fields["ServiceTitle"].Value.ToString()) ? child.Fields["ServiceTitle"].Value.ToString() : "";
                    TagName tagName = new TagName();
                    tagName.name = !string.IsNullOrEmpty(child.Fields["name"].Value.ToString()) ? child.Fields["name"].Value.ToString() : null;
                    tagName.textColor = !string.IsNullOrEmpty(child.Fields["name"].Value.ToString()) ? child.Fields["textColor"].Value.ToString() : null;
                    tagName.backgroundColor = !string.IsNullOrEmpty(child.Fields["name"].Value.ToString()) ? child.Fields["backgroundColor"].Value.ToString() : null;
                    serviceItem.TagName = tagName;
                    serviceItem.ctaUrl = _helper.LinkUrl(child.Fields["ServiceUrl"]);
                    serviceItem.imageSrc = _helper.GetImageURL(child.Fields["ServiceImage"]);

                    switch (queryString)
                    {
                        case "app":
                            serviceItem.imageSrc = _helper.GetImageURL(child.Fields["MobileImage"]);
                            serviceItem.ctaUrl = serviceItem.ctaUrl + "?isapp=true";
                            break;
                        case "web":
                            serviceItem.imageSrc = serviceItem.imageSrc;
                            break;
                        default:
                            serviceItem.imageSrc = serviceItem.imageSrc;
                            break;
                    }
                    
                    serviceList.Add(serviceItem);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" Adani.BAU.Transmission.Feature.Media.Platform.ServicesServiceList GetServiceListData gives -> " + ex.Message);
            }
            return serviceList;
        }
    }
}