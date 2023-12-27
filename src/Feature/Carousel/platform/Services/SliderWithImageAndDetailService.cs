using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class SliderWithImageAndDetailService : ISliderWithImageAndDetailService
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetService;
        private readonly IHelper _helper;
        public SliderWithImageAndDetailService(ILogRepository logRepository, IWidgetService widgetService, IHelper helper)
        {
            this._widgetService = widgetService;
            this._logRepository = logRepository;
            this._helper = helper;
        }
        public WidgetModel GetSliderWithImageAndDetail(Sitecore.Mvc.Presentation.Rendering rendering, string queryString, string cityqueryString)
        {
            WidgetModel sliderWithImageAndDetail = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                if (widget != null)
                {
                    // WidgetService widgetService = new WidgetService();
                    sliderWithImageAndDetail.widget = _widgetService.GetWidgetItem(widget);
                }
                else
                {
                    sliderWithImageAndDetail.widget = new WidgetItem();
                }
                sliderWithImageAndDetail.widget.widgetItems = GetSliderData(rendering, queryString, cityqueryString);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" SliderWithImageAndDetailService GetSliderWithImageAndDetail gives -> " + ex.Message);
            }


            return sliderWithImageAndDetail;
        }

        private List<Object> GetSliderData(Rendering rendering, string queryString, string cityqueryString)
        {
            List<Object> sliderDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Error(" SliderWithImageAndDetailService GetSliderData data source is empty");
                }

                SliderWithImageAndDetail slider;
                if (datasource.ID == Constant.HelpSupportDatasourceFolder)
                {
                    if (!string.IsNullOrEmpty(cityqueryString))
                    {
                        slider = new SliderWithImageAndDetail();
                        var cityDetailsItems = datasource.Children.Where(x => x.Name.ToLower() == cityqueryString).FirstOrDefault();
                        if (cityDetailsItems != null && cityDetailsItems.Children.Count > 0)
                        {
                            var helpsupportItems = cityDetailsItems.Children.Where(x => x.TemplateID == Constant.HelpSupportTemplateID).ToList();
                            if (helpsupportItems != null)
                            {
                                foreach (Sitecore.Data.Items.Item item in helpsupportItems)
                                {
                                    slider = new SliderWithImageAndDetail();
                                    slider.isAirportSelectNeeded = _helper.GetCheckboxOption(item.Fields[Constant.isAirportSelectNeeded]);
                                    slider.title = item[Constant.Title];
                                    slider.imageSrc = item.Fields[Constant.Image] != null ? _helper.GetImageURL(item, Constant.Image) : String.Empty;                                   
                                    slider.btnText = item[Constant.btnText];
                                    slider.description = item[Constant.Description];
                                    slider.ctaUrl = item.Fields[Constant.Link] != null ? _helper.GetLinkURL(item, Constant.Link) : String.Empty;
                                    slider.uniqueId = item[Constant.UniqueId];
                                    slider.mobileImage = item.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(item, Constant.MobileImage) : String.Empty;
                                    slider.subTitle = item[Constant.SubTitle1];// item.Fields[Constant.Link] != null ? _helper.GetLinkText(item, Constant.Link) : String.Empty;
                                    slider.videoSrc = item.Fields[Constant.videoSrc] != null ? _helper.GetImageURL(item, Constant.videoSrc) : String.Empty;
                                    TagName tagName = new TagName();
                                    tagName.name = !string.IsNullOrEmpty(item.Fields["name"].Value.ToString()) ? item.Fields["name"].Value.ToString() : null;
                                    tagName.textColor = !string.IsNullOrEmpty(item.Fields["name"].Value.ToString()) ? item.Fields["textColor"].Value.ToString() : null;
                                    tagName.backgroundColor = !string.IsNullOrEmpty(item.Fields["name"].Value.ToString()) ? item.Fields["backgroundColor"].Value.ToString() : null;
                                    slider.tagName = tagName;

                                    switch (queryString)
                                    {
                                        case "app":
                                            slider.imageSrc = _helper.GetImageURL(item.Fields[Constant.MobileImage]);
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
                                    IEnumerable<Item> contactDetailsItems = item.Children.Where(x => x.TemplateID == Constant.ContactDetailsTemplateID).ToList();
                                    if (contactDetailsItems != null && contactDetailsItems.Count() > 0)
                                    {
                                        ContactDetailsItems contactItems = new ContactDetailsItems();
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
                                        slider.contactDetail = contactItems;
                                        sliderDataList.Add(slider);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var helpsupportItems = datasource.Children.Where(x => x.TemplateID == Constant.HelpSupportTemplateID).ToList();
                        if (helpsupportItems != null)
                        {
                            foreach (Sitecore.Data.Items.Item item in helpsupportItems)
                            {
                                slider = new SliderWithImageAndDetail();
                                slider.isAirportSelectNeeded = _helper.GetCheckboxOption(item.Fields[Constant.isAirportSelectNeeded]);
                                slider.title = item[Constant.Title];
                                slider.imageSrc = item.Fields[Constant.Image] != null ? _helper.GetImageURL(item, Constant.Image) : String.Empty;                               
                                slider.btnText = item[Constant.btnText];
                                slider.description = item[Constant.Description];
                                slider.ctaUrl = item.Fields[Constant.Link] != null ? _helper.GetLinkURL(item, Constant.Link) : String.Empty;
                                slider.uniqueId = item[Constant.UniqueId];
                                slider.mobileImage = item.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(item, Constant.MobileImage) : String.Empty;
                                slider.subTitle = item[Constant.SubTitle1];// item.Fields[Constant.Link] != null ? _helper.GetLinkText(item, Constant.Link) : String.Empty;
                                slider.videoSrc = item.Fields[Constant.videoSrc] != null ? _helper.GetImageURL(item, Constant.videoSrc) : String.Empty;
                                TagName tagName = new TagName();
                                tagName.name = !string.IsNullOrEmpty(item.Fields["name"].Value.ToString()) ? item.Fields["name"].Value.ToString() : null;
                                tagName.textColor = !string.IsNullOrEmpty(item.Fields["name"].Value.ToString()) ? item.Fields["textColor"].Value.ToString() : null;
                                tagName.backgroundColor = !string.IsNullOrEmpty(item.Fields["name"].Value.ToString()) ? item.Fields["backgroundColor"].Value.ToString() : null;
                                slider.tagName = tagName;

                                switch (queryString)
                                {
                                    case "app":
                                        slider.imageSrc = _helper.GetImageURL(item.Fields[Constant.MobileImage]);
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

                                IEnumerable<Item> contactDetailsItems = item.Children.Where(x => x.TemplateID == Constant.ContactDetailsTemplateID).ToList();
                                //List<ContactDetails> contactDetails = new List<ContactDetails>();
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
                                sliderDataList.Add(slider);
                            }
                        }
                        
                    }
                }
                else
                {
                    foreach (Sitecore.Data.Items.Item item in datasource.Children)
                    {
                        slider = new SliderWithImageAndDetail();
                        slider.isAirportSelectNeeded = _helper.GetCheckboxOption(item.Fields[Constant.isAirportSelectNeeded]);
                        slider.title = item[Constant.Title];
                        slider.imageSrc = item.Fields[Constant.Image] != null ? _helper.GetImageURL(item, Constant.Image) : String.Empty;                       
                        slider.btnText = item[Constant.btnText];
                        slider.description = item[Constant.Description];
                        slider.ctaUrl = item.Fields[Constant.Link] != null ? _helper.GetLinkURL(item, Constant.Link) : String.Empty;
                        slider.uniqueId = item[Constant.UniqueId];
                        slider.mobileImage = item.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(item, Constant.MobileImage) : String.Empty;
                        slider.subTitle = item[Constant.SubTitle1];// item.Fields[Constant.Link] != null ? _helper.GetLinkText(item, Constant.Link) : String.Empty;
                        slider.videoSrc = item.Fields[Constant.videoSrc] != null ? _helper.GetImageURL(item, Constant.videoSrc) : String.Empty;
                        slider.linkTarget = item.Fields[Constant.Link] != null ? _helper.LinkUrlTarget(item.Fields[Constant.Link]) : String.Empty;
                        slider.autoId = item[Constant.AutoId];
                        slider.isAgePopup = _helper.GetCheckboxOption(item.Fields[Constant.isAgePopup]);
                        slider.promoCode = item[Constant.PromoCode];
                        GTMTags tags = new GTMTags
                        {
                            bannerCategory = item[Constant.bannerCategory],
                            faqCategory = item[Constant.faqCategory],
                            subCategory = item[Constant.subCategory],
                            category = item[Constant.Category],
                            businessUnit = item[Constant.businessUnit],
                            label = item[Constant.label],
                            source = item[Constant.source],
                            type = item[Constant.type],
                            eventName = item[Constant.eventName]
                        };
                        slider.tags = tags;
                        TagName tagName = new TagName();
                        tagName.name = !string.IsNullOrEmpty(item.Fields["name"].Value.ToString()) ? item.Fields["name"].Value.ToString() : null;
                        tagName.textColor = !string.IsNullOrEmpty(item.Fields["name"].Value.ToString()) ? item.Fields["textColor"].Value.ToString() : null;
                        tagName.backgroundColor = !string.IsNullOrEmpty(item.Fields["name"].Value.ToString()) ? item.Fields["backgroundColor"].Value.ToString() : null;
                        slider.tagName = tagName;

                        switch (queryString)
                        {
                            case "app":
                                slider.imageSrc = _helper.GetImageURL(item.Fields[Constant.MobileImage]);
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

                        IEnumerable<Item> contactDetailsItems = item.Children.Where(x => x.TemplateID == Constant.ContactDetailsTemplateID).ToList();
                        //List<ContactDetails> contactDetails = new List<ContactDetails>();
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
                        slider.checkValidity = item.Fields[Constant.CheckValidity] != null ? _helper.GetCheckboxOption(item.Fields[Constant.CheckValidity]) : false;
                        if (slider.checkValidity)
                        {
                            Sitecore.Data.Fields.DateField EffFrom = item.Fields[Constant.effectiveFrom];
                            slider.effectiveFrom = EffFrom != null ? EffFrom.DateTime.ToString() : string.Empty;
                            Sitecore.Data.Fields.DateField EffTo = item.Fields[Constant.effectiveTo];
                            slider.effectiveTo = EffTo != null ? EffTo.DateTime.ToString() : string.Empty;
                            if (_helper.getISTDateTime((System.DateTime.Now).ToString()) >= Convert.ToDateTime(slider.effectiveFrom) && Convert.ToDateTime(slider.effectiveTo) >= _helper.getISTDateTime((System.DateTime.Now).ToString()))
                                sliderDataList.Add(slider);
                        }
                        else
                            sliderDataList.Add(slider);
                    }
                }
            }
                
            catch (Exception ex)
            {

                _logRepository.Error(" SliderWithImageAndDetailService GetSliderData gives -> " + ex.Message);
            }

            return sliderDataList;
        }

    }
}