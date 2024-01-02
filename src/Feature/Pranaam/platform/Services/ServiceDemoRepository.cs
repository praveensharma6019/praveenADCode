using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class ServiceDemoRepository : IServiceDemo
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public ServiceDemoRepository(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }
        public HowItWorkswidgets GetServiceDemoData(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            HowItWorkswidgets howItWorksWidgits = new HowItWorkswidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                  
                    howItWorksWidgits.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    howItWorksWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                howItWorksWidgits.widget.widgetItems = GetServiceDemo(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetServiceDemoData throws Exception -> " + ex.Message);
            }


            return howItWorksWidgits;
        }
        private List<object> GetServiceDemo(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            _logRepository.Info("GetServiceDemo Started");
            List<object> _howItWorksWidgitsObj = new List<object>();
            try
            {
                ServiceDemo _obj = new ServiceDemo();
                var renDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (renDatasource != null)
                {
                    _logRepository.Info("GetServiceDemo datasource not null");
                    _obj.title = !string.IsNullOrEmpty(renDatasource.Fields[Templates.HowItWorks.Fields.Title].Value.ToString()) ? renDatasource.Fields[Templates.HowItWorks.Fields.Title].Value.ToString() : "";
                    _obj.urlMp4 = _helper.GetLinkURL(renDatasource, Templates.HowItWorks.Fields.MP4Video.ToString());
                    _obj.urlOgg = _helper.GetLinkURL(renDatasource, Templates.HowItWorks.Fields.OGGVideo.ToString());
                    _obj.posterImage = _helper.GetImageURL(renDatasource, Templates.HowItWorks.Fields.ThumbnailImage.ToString());
                    _obj.mobileImage = _helper.GetImageURL(renDatasource, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                    _obj.mobileImageAlt = _helper.GetImageURL(renDatasource, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                    _obj.webImage = _helper.GetImageURL(renDatasource, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                    _obj.webImageAlt = _helper.GetImageURL(renDatasource, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                    _obj.thumbnailImage = _helper.GetImageURL(renDatasource, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                    _obj.thumbnailImageAlt = _helper.GetImageURL(renDatasource, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                    _obj.description = !string.IsNullOrEmpty(renDatasource.Fields[Templates.HowItWorks.Fields.Description].Value.ToString()) ? renDatasource.Fields[Templates.HowItWorks.Fields.Description].Value.ToString() : "";
                    _obj.videoDate = !string.IsNullOrEmpty(renDatasource.Fields[Templates.HowItWorks.Fields.VideoDate].Value.ToString()) ? renDatasource.Fields[Templates.HowItWorks.Fields.VideoDate].Value.ToString() : "";
                    _obj.videoName = !string.IsNullOrEmpty(renDatasource.Fields[Templates.HowItWorks.Fields.VideoName].Value.ToString()) ? renDatasource.Fields[Templates.HowItWorks.Fields.VideoName].Value.ToString() : "";
                    _howItWorksWidgitsObj.Add(_obj);
                    _logRepository.Info("GetServiceDemo Ended");
                }
                else return null;
            }
            catch(Exception ex)
            {
                _logRepository.Error("GetServiceDemo throws Exception -> " + ex.Message);
            }
            
            return _howItWorksWidgitsObj;
        }
    }
}