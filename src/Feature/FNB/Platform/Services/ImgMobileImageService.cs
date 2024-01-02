using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public class ImgMobileImageService : IImageMobileImageInterface
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public ImgMobileImageService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel Illustration(Rendering rendering,string isApp)
        {
            WidgetModel widgetList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constants.RenderingParamField]);
                widgetList.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                widgetList.widget.widgetItems = IllustrationData(rendering, isApp);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetData in Illustration gives -> " + ex.Message);
            }
            return widgetList;
        }

      

        //public ImgMobileImgIllustration Illustration(Rendering rendering, Item datasource)
        //{
        //    throw new NotImplementedException();
        //}

        private List<object> IllustrationData(Rendering rendering, string isApp)
        {
            List<Object> IllustrationDataList = new List<Object>();
            try
            {
                var datasourceItem = rendering.Item;
                // Null Check for datasource
                if (datasourceItem != null && datasourceItem.GetChildren() != null && datasourceItem.GetChildren().Count() > 0)
                {
                    
                    foreach (Sitecore.Data.Items.Item item in datasourceItem.GetChildren())
                    {
                        ImgMobileImgIllustrationModel model = new ImgMobileImgIllustrationModel();
                        model.ImageSrc = item.Fields[Illustrations.ImageSrc] != null ? _helper.GetImageURL(item, Illustrations.ImageSrc) : String.Empty;
                        model.MobileImage = item.Fields[Illustrations.MobileImage] != null ? _helper.GetImageURL(item, Illustrations.MobileImage) : String.Empty;

                        if (isApp == "true")
                        {
                            if ((Sitecore.Data.Fields.ImageField)item.Fields[Illustrations.MobileImage] != null)
                            { model.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)item.Fields[Illustrations.MobileImage]); }
                            else { model.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)item.Fields[Illustrations.ImageSrc]); }
                        }
                        IllustrationDataList.Add(model);
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error("GetDataList in IllustrationData gives -> " + ex.Message);
            }
            return IllustrationDataList;
        }

    }
}
