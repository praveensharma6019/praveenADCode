using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class AddOnServiceContent : IAddOnServiceContent
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetService;
        private readonly IHelper _helper;
        public AddOnServiceContent(ILogRepository logRepository, IWidgetService widgetService, IHelper helper)
        {
            this._widgetService = widgetService;
            this._logRepository = logRepository;
            this._helper = helper;
        }
        public AddOnServicewidgets GetAddOnService(Rendering rendering)
        {
            AddOnServicewidgets addOnWidgits = new AddOnServicewidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                   // WidgetService widgetService = new WidgetService();
                    addOnWidgits.widget = _widgetService.GetWidgetItem(widget);
                }
                else
                {
                    addOnWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                addOnWidgits.widget.widgetItems = GetAddOnServiceData(rendering?.RenderingItem?.Database?.GetItem(rendering?.DataSource));
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetAddOnService throws Exception -> " + ex.Message);
            }


            return addOnWidgits;
        }

        private List<object> GetAddOnServiceData(Sitecore.Data.Items.Item item)
        {
            _logRepository.Info("GetAddOnServiceData Started");
           AddOnService addOnService = new AddOnService();
            List<object> listObject = new List<object>();
            addOnService.Items = new List<Service>();
            if (item == null) return null;
            addOnService.Title = item?.Fields[Templates.AddOnServiceList.Fields.AddOnServiceTitle]?.Value;
            addOnService.Items = GetSelectedServices(item.Fields[Templates.AddOnServiceList.Fields.AddOnServiceList]);
            listObject.Add(addOnService);
            _logRepository.Info("GetAddOnServiceData Ended");
            return listObject;
        }

        private List<Service> GetSelectedServices(Field field)
        {
            _logRepository.Info("GetSelectedServices Started");
            MultilistField multilistField = field;
            Sitecore.Data.Items.Item[] items = multilistField.GetItems();
            List<Service> services = new List<Service>();
            if (items != null && items.Length > 0)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    Service service = new Service();
                    Sitecore.Data.Items.Item serviceItem = items[i];
                    service.Id = serviceItem.Fields[Templates.AddOnService.Fields.Id]?.Value;
                    service.Src = _helper.GetImageURL(serviceItem, serviceItem.Fields[Templates.AddOnService.Fields.ServiceImage]?.Name);
                    service.Alt = _helper.GetImageAlt(serviceItem, serviceItem.Fields[Templates.AddOnService.Fields.ServiceImage]?.Name);
                    service.Title = serviceItem.Fields[Templates.AddOnService.Fields.Title]?.Value;
                    service.Description = serviceItem.Fields[Templates.AddOnService.Fields.Description]?.Value;
                    service.InitialPrice = serviceItem.Fields[Templates.AddOnService.Fields.OldPrice]?.Value;
                    service.FinalPrice = serviceItem.Fields[Templates.AddOnService.Fields.NewPrice]?.Value;
                    service.BtnText = _helper.GetLinkText(serviceItem, serviceItem.Fields[Templates.AddOnService.Fields.KnowmoreCTA]?.Name);
                    service.BtnUrl = _helper.GetLinkURL(serviceItem, serviceItem.Fields[Templates.AddOnService.Fields.KnowmoreCTA]?.Name);
                    service.btnVariant = _helper.GetDropLinkValue(serviceItem.Fields["ButtonVariant"]);
                    services.Add(service);
                }
            }
            _logRepository.Info("GetSelectedServices Ended");
            return services;
        }
    }
}
