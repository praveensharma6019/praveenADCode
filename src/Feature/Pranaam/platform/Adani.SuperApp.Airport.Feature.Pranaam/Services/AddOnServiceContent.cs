using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class AddOnServiceContent : IAddOnServiceContent
    {
        private readonly ILogRepository _logRepository;
        public AddOnServiceContent(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public Models.AddOnService GetAddOnService(Sitecore.Data.Items.Item item)
        {
            AddOnService addOnService = new AddOnService();
            try
            {
                addOnService.Items = new List<Service>();
                if (item == null) return new AddOnService();
                addOnService.Title = item.Fields[Templates.AddOnServiceList.Fields.AddOnServiceTitle]?.Name;
                addOnService.Items = GetSelectedServices(item.Fields[Templates.AddOnServiceList.Fields.AddOnServiceList]);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
          
            return addOnService;
        }

        private List<Service> GetSelectedServices(Field field)
        {
            List<Service> services = new List<Service>();
            try
            {
                MultilistField multilistField = field;
                Sitecore.Data.Items.Item[] items = multilistField.GetItems();
                if (items != null && items.Length > 0)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        Service service = new Service();
                        Sitecore.Data.Items.Item serviceItem = items[i];
                        service.Src = Helper.GetImageURL(serviceItem, serviceItem.Fields[Templates.AddOnService.Fields.ServiceImage]?.Name);
                        service.Alt = Helper.GetImageAlt(serviceItem, serviceItem.Fields[Templates.AddOnService.Fields.ServiceImage]?.Name);
                        service.Title = serviceItem.Fields[Templates.AddOnService.Fields.Title]?.Value;
                        service.Description = serviceItem.Fields[Templates.AddOnService.Fields.Description]?.Value;
                        service.InitialPrice = serviceItem.Fields[Templates.AddOnService.Fields.OldPrice]?.Value;
                        service.FinalPrice = serviceItem.Fields[Templates.AddOnService.Fields.NewPrice]?.Value;
                        service.BtnText = Helper.GetLinkURL(serviceItem, serviceItem.Fields[Templates.AddOnService.Fields.KnowmoreCTA]?.Name);
                        service.BtnUrl = Helper.GetLinkURL(serviceItem, serviceItem.Fields[Templates.AddOnService.Fields.KnowmoreCTA]?.Name);
                        service.btnVariant = service.btnVariant;
                        service.DesktopImage = Helper.GetImageURL(serviceItem, serviceItem.Fields[Templates.DeviceSpecificImage.Fields.DesktopImage]?.Name);
                        service.DesktopImageAlt = Helper.GetImageAlt(serviceItem, serviceItem.Fields[Templates.DeviceSpecificImage.Fields.DesktopImage]?.Name);
                        service.ThumbnailImage = Helper.GetImageURL(serviceItem, serviceItem.Fields[Templates.DeviceSpecificImage.Fields.ThumbnailImage]?.Name);
                        service.ThumbnailImageAlt = Helper.GetImageAlt(serviceItem, serviceItem.Fields[Templates.DeviceSpecificImage.Fields.ThumbnailImage]?.Name);
                        service.MobileImage = Helper.GetImageURL(serviceItem, serviceItem.Fields[Templates.DeviceSpecificImage.Fields.MobileImage]?.Name);
                        service.MobileImageAlt = Helper.GetImageAlt(serviceItem, serviceItem.Fields[Templates.DeviceSpecificImage.Fields.MobileImage]?.Name);
                        services.Add(service);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return services;
        }
    }
}
