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
    public class TitleDescImageDatasourceService : ITitleDescImageDatasourceService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public TitleDescImageDatasourceService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }




        public WidgetModel GetTitleDescImageDatasource(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            WidgetModel titleDescImageData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                if (widget != null)
                {

                    titleDescImageData.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    titleDescImageData.widget = new WidgetItem();
                }
                titleDescImageData.widget.widgetItems = GetTitleDescImageData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" TitleDescImageDatasourceService GetTitleDescImageDatasource gives -> " + ex.Message);
            }


            return titleDescImageData;
        }

        private List<Object> GetTitleDescImageData(Rendering rendering)
        {
            List<Object> sliderDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasourceItem = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasourceItem == null)
                {
                    _logRepository.Error(" TitleDescImageDatasourceService GetTitleDescImageData data source is empty");
                }

                SliderWithImageAndDetail slider = new SliderWithImageAndDetail();
                slider.isAirportSelectNeeded = _helper.GetCheckboxOption(datasourceItem.Fields[Constant.isAirportSelectNeeded]);
                slider.title = datasourceItem[Constant.Title];
                slider.imageSrc = datasourceItem.Fields[Constant.Image] != null ? _helper.GetImageURL(datasourceItem, Constant.Image) : String.Empty;
                slider.description = datasourceItem[Constant.Description];
                slider.ctaUrl = datasourceItem.Fields[Constant.Link] != null ? _helper.GetLinkURL(datasourceItem, Constant.Link) : String.Empty;
                slider.uniqueId = datasourceItem[Constant.UniqueId];
                slider.mobileImage = datasourceItem.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(datasourceItem, Constant.MobileImage) : String.Empty;
                slider.autoId = datasourceItem[Constant.AutoId];
                slider.linkTarget = datasourceItem.Fields[Constant.Link] != null ? _helper.LinkUrlTarget(datasourceItem.Fields[Constant.Link]) : String.Empty;
                slider.isAgePopup = _helper.GetCheckboxOption(datasourceItem.Fields[Constant.isAgePopup]);
                GTMTags tags = new GTMTags
                {
                    bannerCategory = datasourceItem[Constant.bannerCategory],
                    faqCategory = datasourceItem[Constant.faqCategory],
                    subCategory = datasourceItem[Constant.subCategory],
                    category = datasourceItem[Constant.Category],
                    businessUnit = datasourceItem[Constant.businessUnit],
                    label = datasourceItem[Constant.label],
                    source = datasourceItem[Constant.source],
                    type = datasourceItem[Constant.type],
                    eventName = datasourceItem[Constant.eventName]
                };
                slider.tags = tags;
                sliderDataList.Add(slider);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" TitleDescImageDatasourceService GetTitleDescImageData gives -> " + ex.Message);
            }

            return sliderDataList;
        }
    }
}