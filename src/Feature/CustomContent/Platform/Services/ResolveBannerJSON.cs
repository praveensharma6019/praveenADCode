using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.StringExtensions;
using Sitecore.Web.UI.WebControls.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Item = Sitecore.Data.Items.Item;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public class ResolveBannerJSON : IResolveBannerJSON
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public ResolveBannerJSON(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }
        public WidgetModel GetBanner(Sitecore.Mvc.Presentation.Rendering rendering, string queryString, string storeType)
        {

            WidgetModel fnBSapApi = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                fnBSapApi.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                fnBSapApi.widget.widgetItems = GetBannerDataItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetSapAPIData gives -> " + ex.Message);
            }


            return fnBSapApi;
        }
      

        public List<Object> GetBannerDataItem(Sitecore.Mvc.Presentation.Rendering rendering)
        {

            List<Object> data = new List<Object>();
          RewardBanner rewardBanner = new RewardBanner();
            try
            {
                var datasource = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasource != null)
                {
                    
                    rewardBanner.ButtonText = !string.IsNullOrEmpty(datasource.Fields["ButtonText"].Value.ToString()) ? datasource.Fields["ButtonText"].Value.ToString() : String.Empty;
                    rewardBanner.Title = !string.IsNullOrEmpty(datasource.Fields["Title"].Value.ToString()) ? datasource.Fields["Title"].Value.ToString() : String.Empty;
                    rewardBanner.Description = !string.IsNullOrEmpty(datasource.Fields["Description"].Value.ToString()) ? datasource.Fields["Description"].Value.ToString() : String.Empty;
                    rewardBanner.imageSrc = datasource.Fields[Constant.Image] != null ? _helper.GetImageURL(datasource, Constant.Image) : String.Empty;
                    rewardBanner.mobileImagesrc = datasource.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(datasource, Constant.MobileImage) : String.Empty;
                  

                }
                data.Add(rewardBanner);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" GetThemeDataList gives -> " + ex.Message);
            }
            return data;
        }


    }
}