using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using System.Linq;
using Sitecore.Common;
using Sitecore.StringExtensions;
using System.Web.UI.WebControls;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class OffersAndDiscountsService : IOffersAndDiscountsService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public OffersAndDiscountsService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel OffersAndDiscountsData(Sitecore.Mvc.Presentation.Rendering rendering, Item offerItem, string queryString, string cityqueryString)
        {
            WidgetModel OfferListData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);

                if (widget != null)
                {

                    OfferListData.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    OfferListData.widget = new WidgetItem();
                }
                OfferListData.widget.widgetItems = GetOffersData(offerItem, queryString, cityqueryString);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOfferList gives -> " + ex.Message);
            }
            return OfferListData;
        }

        public List<object> GetOffersData(Item datasource, string queryString, string cityqueryString)
        {
            List<object> offersWidgetList = new List<object>();
            OffersAndDiscountsModel offers;
            if(datasource.Children != null && datasource.Children.Count > 0)
            {
                foreach (Item folderData in datasource.Children)
                {
                    offers = new OffersAndDiscountsModel();
                    offers.TabName = folderData.Fields[Constant.Title].Value;
                    List<SliderWithImageAndDetail> sliderList = new List<SliderWithImageAndDetail>();
                    SliderWithImageAndDetail slider;
                    if (folderData.Children != null && folderData.Children.Count > 0)
                    {
                        foreach (Item offerData in folderData.Children)
                        {
                            slider = new SliderWithImageAndDetail();
                            slider.isAirportSelectNeeded = _helper.GetCheckboxOption(offerData.Fields[Constant.isAirportSelectNeeded]);
                            slider.title = offerData[Constant.Title];
                            slider.imageSrc = offerData.Fields[Constant.Image] != null ? _helper.GetImageURL(offerData, Constant.Image) : String.Empty;
                            slider.btnText = offerData[Constant.btnText];
                            slider.description = offerData[Constant.Description];
                            slider.ctaUrl = offerData.Fields[Constant.Link] != null ? _helper.GetLinkURL(offerData, Constant.Link) : String.Empty;
                            slider.uniqueId = offerData[Constant.UniqueId];
                            slider.mobileImage = offerData.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(offerData, Constant.MobileImage) : String.Empty;
                            slider.subTitle = offerData[Constant.SubTitle1];
                            slider.videoSrc = offerData.Fields[Constant.videoSrc] != null ? _helper.GetImageURL(offerData, Constant.videoSrc) : String.Empty;
                            slider.linkTarget = offerData.Fields[Constant.Link] != null ? _helper.LinkUrlTarget(offerData.Fields[Constant.Link]) : String.Empty;
                            slider.autoId = offerData[Constant.AutoId];
                            slider.isAgePopup = _helper.GetCheckboxOption(offerData.Fields[Constant.isAgePopup]);
                            slider.displaySequence = string.IsNullOrEmpty(offerData[Constant.DisplaySequence].ToString()) ? 0 : Convert.ToInt32(offerData[Constant.DisplaySequence]);
                            GTMTags tags = new GTMTags
                            {
                                bannerCategory = offerData[Constant.bannerCategory],
                                faqCategory = offerData[Constant.faqCategory],
                                subCategory = offerData[Constant.subCategory],
                                category = offerData[Constant.Category],
                                businessUnit = offerData[Constant.businessUnit],
                                label = offerData[Constant.label],
                                source = offerData[Constant.source],
                                type = offerData[Constant.type],
                                eventName = offerData[Constant.eventName]
                            };
                            slider.tags = tags;
                            TagName tagName = new TagName();
                            tagName.name = !string.IsNullOrEmpty(offerData.Fields["name"].Value.ToString()) ? offerData.Fields["name"].Value.ToString() : null;
                            tagName.textColor = !string.IsNullOrEmpty(offerData.Fields["name"].Value.ToString()) ? offerData.Fields["textColor"].Value.ToString() : null;
                            tagName.backgroundColor = !string.IsNullOrEmpty(offerData.Fields["name"].Value.ToString()) ? offerData.Fields["backgroundColor"].Value.ToString() : null;
                            slider.tagName = tagName;

                            switch (queryString)
                            {
                                case "app":
                                    slider.imageSrc = _helper.GetImageURL(offerData.Fields[Constant.MobileImage]);
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

                            IEnumerable<Item> contactDetailsItems = offerData.Children.Where(x => x.TemplateID == Constant.ContactDetailsTemplateID).ToList();
                            ContactDetailsItems contactItems = new ContactDetailsItems();
                            if (contactDetailsItems != null && contactDetailsItems.Count() > 0)
                            {
                                foreach (var contact in contactDetailsItems)
                                {
                                    if (contact != null)
                                    {
                                        if (contact.Name == "Phone")
                                        {
                                            ContactDetails phone = new ContactDetails();
                                            phone.name = contact.Fields["Name"].Value;
                                            phone.title = contact.Fields["Title"].Value;
                                            phone.richText = contact.Fields["RichText"].Value;
                                            contactItems.phone = phone;
                                        }
                                        if (contact.Name == "Email")
                                        {
                                            ContactDetails email = new ContactDetails();
                                            email.name = contact.Fields["Name"].Value;
                                            email.title = contact.Fields["Title"].Value;
                                            email.richText = contact.Fields["RichText"].Value;
                                            contactItems.email = email;
                                        }
                                    }
                                }
                            }
                            slider.contactDetail = contactItems;
                            sliderList.Add(slider);
                            offers.OfferList = sliderList;
                        }
                    }
                    offersWidgetList.Add(offers);
                }
            }
            return offersWidgetList;
        }
    }
}
