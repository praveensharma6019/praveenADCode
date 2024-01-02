using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class ServiceCarousalRepository : IServiceCarousal
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetService;
        private readonly IHelper _helper;
        public ServiceCarousalRepository(ILogRepository logRepository, IWidgetService widgetService, IHelper helper)
        {
            this._widgetService = widgetService;
            this._logRepository = logRepository;
            this._helper = helper;
        }
        public ServiceCarousalwidgets GetServiceCarousalData(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            ServiceCarousalwidgets serviceCarousalWidgits = new ServiceCarousalwidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                   
                    serviceCarousalWidgits.widget = _widgetService.GetWidgetItem(widget);
                }
                else
                {
                    serviceCarousalWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                serviceCarousalWidgits.widget.widgetItems = GetServiceCarousal(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetServiceCarousalData throws Exception -> " + ex.Message);
            }


            return serviceCarousalWidgits;
        }
        private List<object> GetServiceCarousal(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            _logRepository.Info("GetServiceCarousal Started");
            List<object> _obj = new List<object>();
            try
            {
                ServiceCarousal _servicecarousal = new ServiceCarousal();
                var renDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (renDatasource != null)
                {
                    _logRepository.Info("GetServiceCarousal datasource not null");
                    _servicecarousal.title = !string.IsNullOrEmpty(renDatasource.Fields[Templates.ServiceCarousal.Fields.Title].Value.ToString()) ? renDatasource.Fields[Templates.ServiceCarousal.Fields.Title].Value.ToString() : "";
                    List<HeroCarousel> list = new List<HeroCarousel>();
                    if (((Sitecore.Data.Fields.MultilistField)renDatasource.Fields[Templates.ServiceCarousal.Fields.CarousalList]).Count > 0)
                    {
                        _logRepository.Info("GetServiceCarousal list population");
                        foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)renDatasource.Fields[Templates.ServiceCarousal.Fields.CarousalList]).GetItems())
                        {
                            HeroCarousel serviceItem = new HeroCarousel();
                            serviceItem.title = !string.IsNullOrEmpty(item.Fields[Templates.ServiceCarousalItem.Fields.Title].Value.ToString()) ? item.Fields[Templates.ServiceCarousalItem.Fields.Title].Value.ToString() : "";
                            serviceItem.imageSrc = _helper.GetImageURL(item, Templates.ServiceCarousalItem.Fields.StanderedImage.ToString());
                            serviceItem.description = !string.IsNullOrEmpty(item.Fields[Templates.ServiceCarousalItem.Fields.Description].Value.ToString()) ? item.Fields[Templates.ServiceCarousalItem.Fields.Description].Value.ToString() : "";
                            serviceItem.ctaLink = _helper.GetLinkURL(item, Templates.ServiceCarousalItem.Fields.CTA.ToString());
                            serviceItem.appCtaLink = _helper.GetLinkURL(item, Templates.ServiceCarousalItem.Fields.AppCTA.ToString());
                            serviceItem.mobileImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                            serviceItem.webImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                            list.Add(serviceItem);
                        }
                        _servicecarousal.items = list;
                        _obj.Add(_servicecarousal);
                    }
                    _logRepository.Info("GetServiceCarousal Ended");
                }
                else return null;
            }
            catch(Exception ex)
            {
                _logRepository.Error("GetServiceCarousal throws Exception -> " + ex.Message);
            }
            
            return _obj;
        }
    }
}