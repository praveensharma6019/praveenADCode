using Adani.SuperApp.Airport.Feature.Media.Platform.Models;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using System;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Media.Platform.Services
{
    public class ServiceListWeb : IServiceListWeb
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public ServiceListWeb(ILogRepository logRepository, IWidgetService widgetService, IHelper helper)
        {

            this._logRepository = logRepository;
            this._widgetservice = widgetService;
            this._helper = helper;
        }
        public services GetServicesData(Rendering rendering)
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
                services.widget.widgetItems = GetServiceListData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" Adani.SuperApp.Airport.Feature.Media.Platform.ServicesServiceList GetServicesData gives -> " + ex.Message);
            }

            return services;
        }

        private List<object> GetServiceListData(Rendering rendering)
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
                    throw new NullReferenceException("Adani.SuperApp.Airport.Feature.Media.Platform.Services.GetServiceListData => Rendering Datasource is Empty");
                }

                List<Item> childList = datasource.Children.Where(x => x.TemplateID == Templates.ServicesListCollection.ServiceItemTemplateID).ToList();
                foreach (var child in childList)
                {
                    ServiceItemWeb serviceItem = new ServiceItemWeb();
                    serviceItem.uniqueId = !string.IsNullOrEmpty(child.Fields[Templates.ServicesListCollection.UniqueId].Value.ToString()) ? child.Fields[Templates.ServicesListCollection.UniqueId].Value.ToString() : "";
                    serviceItem.title = !string.IsNullOrEmpty(child.Fields[Templates.ServicesListCollection.ServiceTitle].Value.ToString()) ? child.Fields[Templates.ServicesListCollection.ServiceTitle].Value.ToString() : "";
                    TagName tagName = new TagName();
                    tagName.name = !string.IsNullOrEmpty(child.Fields[Templates.ServicesListCollection.TagName].Value.ToString()) ? child.Fields[Templates.ServicesListCollection.TagName].Value.ToString() : null;
                    tagName.textColor = !string.IsNullOrEmpty(child.Fields[Templates.ServicesListCollection.TagName].Value.ToString()) ? child.Fields[Templates.ServicesListCollection.TextColor].Value.ToString() : null;
                    tagName.backgroundColor = !string.IsNullOrEmpty(child.Fields[Templates.ServicesListCollection.TagName].Value.ToString()) ? child.Fields[Templates.ServicesListCollection.BackgroundColor].Value.ToString() : null;
                    serviceItem.TagName = tagName;
                    serviceItem.ctaUrl = _helper.LinkUrl(child.Fields[Templates.ServicesListCollection.ServiceUrl]);
                    serviceItem.target = _helper.LinkUrlTarget(child.Fields[Templates.ServicesListCollection.ServiceUrl]);
                    serviceItem.imageSrc = _helper.GetImageURL(child.Fields[Templates.ServicesListCollection.Image]);
                    serviceItem.mobileImage = _helper.GetImageURL(child.Fields[Templates.ServicesListCollection.MobileImage]);
                    serviceItem.autoId = !string.IsNullOrEmpty(child.Fields[Templates.ServicesListCollection.AutoId].Value.ToString()) ? child.Fields[Templates.ServicesListCollection.AutoId].Value.ToString() : "";
                    serviceItem.isAgePopup = _helper.GetCheckboxOption(child.Fields[Templates.ServicesListCollection.IsAgePopup]);
                    serviceList.Add(serviceItem);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" Adani.SuperApp.Airport.Feature.Media.Platform.ServicesServiceList GetServiceListData gives -> " + ex.Message);
            }
            return serviceList;
        }
    }
}