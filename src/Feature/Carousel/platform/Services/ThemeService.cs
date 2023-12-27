using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class ThemeService : IThemeService
    {
        //Ticket NO  18854
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public ThemeService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetThemeData(Rendering rendering)
        {
            WidgetModel ThemeList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                ThemeList.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                ThemeList.widget.widgetItems = GetThemeDataList(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetThemeData gives -> " + ex.Message);
            }
            return ThemeList;
        }

        private List<object> GetThemeDataList(Rendering rendering)
        {
            List<Object> ThemeDataList = new List<Object>();
            try
            {
                var datasourceItem = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasourceItem != null && datasourceItem.GetChildren() != null && datasourceItem.GetChildren().Count() > 0)
                {
                    ThemeModel themeData = null;
                    foreach (Sitecore.Data.Items.Item item in datasourceItem.GetChildren())
                    {
                        themeData = new ThemeModel();
                        themeData.image = item.Fields[Constant.DeskImage] != null ? _helper.GetImageURL(item, Constant.DeskImage) : String.Empty;
                        themeData.mobileImage = item.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(item, Constant.MobileImage) : String.Empty;
                        themeData.video = item.Fields[Constant.Video] != null ? _helper.LinkUrl(item.Fields[Constant.Video]) : String.Empty;
                        themeData.mobileVideo = item.Fields[Constant.MobileVideo] != null ? _helper.LinkUrl(item.Fields[Constant.MobileVideo]) : String.Empty;
                        themeData.ctaUrl = item.Fields[Constant.CTAUrl] != null ? _helper.GetLinkURL(item,Constant.CTAUrl) : String.Empty;
                        themeData.linkTarget = item.Fields[Constant.CTAUrl] != null ? _helper.LinkUrlTarget(item.Fields[Constant.CTAUrl]) : String.Empty;
                        themeData.webActive = item.Fields[Constant.WebActive] != null ? _helper.GetCheckboxOption(item.Fields[Constant.WebActive]) : false;
                        themeData.mWebActive = item.Fields[Constant.MWebActive] != null ? _helper.GetCheckboxOption(item.Fields[Constant.MWebActive]) : false;
                        themeData.Title= item.Fields[Constant.Calendar.Title]!= null ? item.Fields[Constant.Calendar.Title].Value : String.Empty;
                        ThemeDataList.Add(themeData);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" GetThemeDataList gives -> " + ex.Message);
            }
            return ThemeDataList;
        }

    }
}