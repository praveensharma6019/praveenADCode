using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class MoreServicesService : IMoreServicesService
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public MoreServicesService(ILogRepository logRepository, IWidgetService widgetService, IHelper helper)
        {

            this._logRepository = logRepository;
            this._widgetservice = widgetService;
            this._helper = helper;
        }
        public WidgetModel GetMoreServicesService(Sitecore.Mvc.Presentation.Rendering rendering, string queryString)
        {
            WidgetModel moreServices = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                if (widget != null)
                {
                    // WidgetService widgetService = new WidgetService();
                    moreServices.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    moreServices.widget = new WidgetItem();
                }
                moreServices.widget.widgetItems = GetSliderData(rendering, queryString);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetMoreServicesService GetMoreServicesService gives -> " + ex.Message);
            }


            return moreServices;
        }

        private List<Object> GetSliderData(Rendering rendering, string queryString)
        {
            List<Object> sliderDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Error(" GetMoreServicesService GetSliderData data source is empty");
                }

                MoreServices slider;
                foreach (Sitecore.Data.Items.Item services in datasource.Children)
                {

                    foreach (Sitecore.Data.Items.Item child in services.Children)
                    {
                        slider = new MoreServices();
                        slider.isAirportSelectNeeded = _helper.GetCheckboxOption(child.Fields[Constant.isAirportSelectNeeded]);
                        slider.title = child[Constant.Title];
                        slider.imageSrc = child.Fields[Constant.Image] != null ? _helper.GetImageURL(child, Constant.Image) : String.Empty;                       
                        slider.btnText = child[Constant.btnText];
                        slider.ctaUrl = child.Fields[Constant.Link] != null ? _helper.GetLinkURL(child, Constant.Link) : String.Empty;
                        slider.uniqueId = child[Constant.UniqueId];
                        slider.mobileImage = child.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(child, Constant.MobileImage) : String.Empty;
                        slider.autoId = child[Constant.AutoId];
                        slider.linkTarget = child.Fields[Constant.Link] != null ? _helper.LinkUrlTarget(child.Fields[Constant.Link]) : String.Empty;
                        slider.isAgePopup = _helper.GetCheckboxOption(child.Fields[Constant.isAgePopup]);
                        GTMTags tags = new GTMTags
                        {
                            bannerCategory = child[Constant.bannerCategory],
                            faqCategory = child[Constant.faqCategory],
                            subCategory = child[Constant.subCategory],
                            category = child[Constant.Category],
                            businessUnit = child[Constant.businessUnit],
                            label = child[Constant.label],
                            source = child[Constant.source],
                            type = child[Constant.type],
                            eventName = child[Constant.eventName]
                        };
                        slider.tags = tags;
                        TagName tagName = new TagName();
                        tagName.name = !string.IsNullOrEmpty(child.Fields["name"].Value.ToString()) ? child.Fields["name"].Value.ToString() : null;
                        tagName.textColor = !string.IsNullOrEmpty(child.Fields["name"].Value.ToString()) ? child.Fields["textColor"].Value.ToString() : null;
                        tagName.backgroundColor = !string.IsNullOrEmpty(child.Fields["name"].Value.ToString()) ? child.Fields["backgroundColor"].Value.ToString() : null;
                        slider.tagName = tagName;

                        switch (queryString)
                        {
                            case "app":
                                slider.imageSrc = _helper.GetImageURL(child.Fields[Constant.MobileImage]);
                                if (!String.IsNullOrEmpty(slider.ctaUrl))
                                {
                                    if (slider.ctaUrl.Contains("?"))
                                    {
                                        slider.ctaUrl += "&isapp=true";
                                    }
                                    else
                                    {
                                        slider.ctaUrl += "?isapp=true";
                                    }
                                }
                                break;
                            case "web":
                                slider.imageSrc = slider.imageSrc;
                                break;
                            default:
                                slider.imageSrc = slider.imageSrc;
                                break;
                        }

                        sliderDataList.Add(slider);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetMoreServicesService GetSliderData gives -> " + ex.Message);
            }

            return sliderDataList;
        }
    }
}