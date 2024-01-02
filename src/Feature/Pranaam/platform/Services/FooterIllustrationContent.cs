using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class FooterIllustrationContent : IFooterIllustration
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public FooterIllustrationContent(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public FooterIllustartionWidgets GetFooterIllustartionWidget(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            FooterIllustartionWidgets _footerIllustration = new FooterIllustartionWidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {

                    _footerIllustration.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    _footerIllustration.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                _footerIllustration.widget.widgetItems = GetDataFooterIllustration(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetPranaamSteps throws exception -> " + ex.Message);
            }


            return _footerIllustration;
        }
        private List<Object> GetDataFooterIllustration(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            FooterIllustration _obj = new FooterIllustration();
            List<Object> _listObj = new List<object>();
            try
            {
                var ds = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if(ds != null)
                {
                    _obj.Title = !string.IsNullOrEmpty(ds.Fields[Templates.FooterIllustration.Fields.Title].Value.ToString()) ? ds.Fields[Templates.FooterIllustration.Fields.Title].Value.ToString() : "";
                    if (((Sitecore.Data.Fields.MultilistField)ds.Fields[Templates.FooterIllustration.Fields.DescriptionData]).Count > 0)
                    {
                        _obj.DescriptionData = ((Sitecore.Data.Fields.MultilistField)ds.Fields[Templates.FooterIllustration.Fields.DescriptionData]).GetItems().Select(x => x.Fields[Templates.Paragraph.Fields.ParagraphContent].Value).ToList();
                    }
                    _obj.mobileImage = _helper.GetImageURL(ds, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                    _obj.mobileImageAlt = _helper.GetImageURL(ds, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                    _obj.webImage = _helper.GetImageURL(ds, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                    _obj.webImageAlt = _helper.GetImageURL(ds, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                    _obj.thumbnailImage = _helper.GetImageURL(ds, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                    _obj.thumbnailImageAlt = _helper.GetImageURL(ds, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                    _obj.CTAText = _helper.GetLinkText(ds, Templates.FooterIllustration.Fields.CTA.ToString());
                    _obj.CTAUrl = _helper.GetLinkURL(ds, Templates.FooterIllustration.Fields.CTA.ToString());
                    if (((Sitecore.Data.Fields.MultilistField)ds.Fields[Templates.FooterIllustration.Fields.AppDesc]).Count > 0)
                    {
                        _obj.AppDesc = ((Sitecore.Data.Fields.MultilistField)ds.Fields[Templates.FooterIllustration.Fields.AppDesc]).GetItems().Select(x => x.Fields[Templates.Paragraph.Fields.ParagraphContent].Value).ToList();
                    }
                    _listObj.Add(_obj);
                }
                else return null;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return _listObj;
        }
    }
}