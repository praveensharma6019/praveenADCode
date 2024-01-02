using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class BaseCarousel:IBaseCarousel
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public BaseCarousel(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel BaseCarouselData(Rendering rendering,Item datasource)
        {
            WidgetModel OurBusinessListData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);

                if (widget != null)
                {

                    OurBusinessListData.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    OurBusinessListData.widget = new WidgetItem();
                }
                OurBusinessListData.widget.widgetItems = GetBaseCarouselData(datasource);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetBaseCarouselData gives -> " + ex.ToString());
            }
            return OurBusinessListData;
        }

        public List<object> GetBaseCarouselData(Sitecore.Data.Items.Item datasource)
        {
            List<object> BusinessDataList = new List<object>();
            BaseCarouselModel ourBusinessModel;


            if (datasource.Children != null && datasource.Children.Count > 0)
            {
               
                List<BaseCarouselModel> sliderList = new List<BaseCarouselModel>();
                foreach (Item folderData in datasource.Children)
                {
                    ourBusinessModel = new BaseCarouselModel();
                    ourBusinessModel.Title = folderData.Fields[Constant.Title].Value;
                    ourBusinessModel.Detail = folderData.Fields[Constant.Detail].Value;
                    ourBusinessModel.Url = _helper.LinkUrl(folderData.Fields[Constant.OBUrl]);
                    ourBusinessModel.UrlTarget = _helper.LinkUrlTarget(folderData.Fields[Constant.OBUrl]);
                    ourBusinessModel.btnName = folderData.Fields[Constant.btnName].Value;
                    ourBusinessModel.Url2 = folderData.Fields[Constant.OBUrl2] != null ? _helper.GetLinkURL(folderData, Constant.OBUrl2) : string.Empty;
                    ourBusinessModel.Url2Target = folderData.Fields[Constant.OBUrl2] != null ? _helper.LinkUrlTarget(folderData.Fields[Constant.OBUrl2]) : string.Empty;
                    ourBusinessModel.btnName2 = folderData.Fields[Constant.btnName2].Value;
                    ourBusinessModel.SubTitle = folderData.Fields[Constant.SubTitle1].Value;
                    ourBusinessModel.SubDetail = folderData.Fields[Constant.SubDetail].Value;
                    ourBusinessModel.WebImage = folderData.Fields[Constant.WebImage] != null ? _helper.GetImageURL(folderData, Constant.WebImage) : string.Empty;
                    ourBusinessModel.MobileImage = folderData.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(folderData, Constant.MobileImage) : string.Empty;
                    ourBusinessModel.DeepLink = folderData.Fields[Constant.deeplink] != null ? folderData.Fields[Constant.deeplink].Value : string.Empty;
                    ourBusinessModel.DeepLink2 = folderData.Fields[Constant.deeplink2] != null ? folderData.Fields[Constant.deeplink2].Value : string.Empty;
                    ourBusinessModel.isAgePopUp = folderData.Fields[Constant.isAgePopup] != null ? _helper.GetCheckboxOption(folderData.Fields[Constant.isAgePopup]) : false;
                    sliderList.Add(ourBusinessModel);

                }
                BusinessDataList.Add(sliderList);


            }

            return BusinessDataList;
        }
    }
}