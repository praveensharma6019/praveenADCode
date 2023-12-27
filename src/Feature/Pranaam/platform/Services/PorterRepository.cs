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
    public class PorterRepository: IPorterService
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public PorterRepository(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }
        public Porterwidgets GetPorterCardDetails(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Porterwidgets porterWidgits = new Porterwidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                    
                    porterWidgits.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    porterWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                porterWidgits.widget.widgetItems = GetPorterCard(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetPorterCardDetails throws Exception -> " + ex.Message);
            }


            return porterWidgits;
        }
        private List<object> GetPorterCard(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            _logRepository.Info("GetPorterCard Started");
            List<object> _porterObj = new List<object>();
            try
            {
                PorterService _obj = new PorterService();
                var porterDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (porterDatasource != null)
                {
                    _logRepository.Info("GetPorterCard datasource not null");
                    _obj.title = !string.IsNullOrEmpty(porterDatasource.Fields[Templates.PorterCard.Fields.Title].Value.ToString()) ? porterDatasource.Fields[Templates.PorterCard.Fields.Title].Value.ToString() : "";
                    _obj.desc = !string.IsNullOrEmpty(porterDatasource.Fields[Templates.PorterCard.Fields.Decription].Value.ToString()) ? porterDatasource.Fields[Templates.PorterCard.Fields.Decription].Value.ToString() : "";
                    _obj.src = _helper.GetImageURL(porterDatasource, Templates.PorterCard.Fields.ServiceImage.ToString());
                    _obj.btnTitle = _helper.GetLinkText(porterDatasource, Templates.PorterCard.Fields.KnowmoreCTA.ToString());
                    _obj.btnUrl = _helper.GetLinkURL(porterDatasource, Templates.PorterCard.Fields.KnowmoreCTA.ToString());
                    _obj.alt = _helper.GetImageAlt(porterDatasource, Templates.PorterCard.Fields.ServiceImage.ToString());
                    _obj.price = !string.IsNullOrEmpty(porterDatasource.Fields[Templates.PorterCard.Fields.NewPrice].Value.ToString()) ? porterDatasource.Fields[Templates.PorterCard.Fields.NewPrice].Value.ToString() : "";
                    _obj.mobileImage = _helper.GetImageURL(porterDatasource, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                    _obj.mobileImageAlt = _helper.GetImageURL(porterDatasource, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                    _obj.webImage = _helper.GetImageURL(porterDatasource, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                    _obj.webImageAlt = _helper.GetImageURL(porterDatasource, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                    _obj.thumbnailImage = _helper.GetImageURL(porterDatasource, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                    _obj.thumbnailImageAlt = _helper.GetImageURL(porterDatasource, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                    _porterObj.Add(_obj);
                    _logRepository.Info("GetPorterCard Ended");
                }
                else return null;
            }
            catch(Exception ex)
            {
                _logRepository.Error("GetPorterCard throws Exception -> " + ex.Message);
            }
            
            return _porterObj;
        }
    }
}