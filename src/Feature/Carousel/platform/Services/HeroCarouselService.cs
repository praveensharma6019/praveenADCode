using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class HeroCarouselService : IHeroCarouselService
    {

        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public HeroCarouselService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }



        /// <summary>
        /// Implementation to get the header data
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        public HeroCarouselwidgets GetHeroCarouseldata(Rendering rendering, string queryString, string storeType, string airport, bool restricted)
        {
            HeroCarouselwidgets heroCarouselWidgits = new HeroCarouselwidgets();
            try
            {
                Item widget = null;

                IDictionary<string, string> paramDictionary = rendering.Parameters.ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (string key in paramDictionary.Keys.ToList())
                {
                    if (Sitecore.Data.ID.TryParse(paramDictionary[key], out var itemId))
                    {
                        widget = rendering.RenderingItem.Database.GetItem(itemId);
                    }
                }
                if (widget != null)
                {

                    heroCarouselWidgits.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    heroCarouselWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                heroCarouselWidgits.widget.widgetItems = GetCarouseldata(rendering, queryString, storeType, airport, restricted);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService GetHeroCarouseldata gives -> " + ex.Message);
            }


            return heroCarouselWidgits;
        }



        private List<Object> GetCarouseldata(Rendering rendering, string queryString, string storeType, string airport, bool restricted)
        {
            List<Object> heroCarouselList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                HeroCarousel heroCarousel;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    heroCarousel = new HeroCarousel();
                    heroCarousel.title = !string.IsNullOrEmpty(item.Fields[Constant.Title].Value.ToString()) ? item.Fields[Constant.Title].Value.ToString() : "";
                    heroCarousel.isAirportSelectNeeded = _helper.GetCheckboxOption(item.Fields[Constant.isAirportSelectNeeded]);
                    heroCarousel.subTitle = !string.IsNullOrEmpty(item.Fields[Constant.SubTitle].Value.ToString()) ? item.Fields[Constant.SubTitle].Value.ToString() : "";
                    // heroCarousel.imageSrc = _helper.GetImageURL(item, Constant.StanderedImage);
                    heroCarousel.description = !string.IsNullOrEmpty(item.Fields[Constant.Description].Value.ToString()) ? item.Fields[Constant.Description].Value.ToString() : "";
                    heroCarousel.ctaLink = item.Fields[Constant.CTA] != null ? _helper.GetLinkURL(item, Constant.CTA) : String.Empty;
                    heroCarousel.linkTarget = item.Fields[Constant.CTA] != null ? _helper.LinkUrlTarget(item.Fields[Constant.CTA]) : String.Empty;
                    heroCarousel.autoId = item[Constant.AutoId];
                    heroCarousel.isAgePopup = _helper.GetCheckboxOption(item.Fields[Constant.isAgePopup]);
                    GTMTags tags = new GTMTags
                    {
                        bannerCategory = item[Constant.bannerCategory],
                        faqCategory = item[Constant.faqCategory],
                        subCategory = item[Constant.subCategory],
                        category = item[Constant.category],
                        businessUnit = item[Constant.businessUnit],
                        label = item[Constant.label],
                        source = item[Constant.source],
                        type = item[Constant.type],
                        eventName = item[Constant.eventName]
                    };
                    heroCarousel.tags = tags;
                    if (item.Fields[Constant.AppImage] != null)
                    { heroCarousel.mobileImage = _helper.GetImageURL(item, Constant.AppImage); }
                    if (item.Fields[Constant.DeskTopImage] != null)
                    { heroCarousel.webImage = _helper.GetImageURL(item, Constant.DeskTopImage); }
                    heroCarousel.ctaText = _helper.GetLinkText(item, Constant.CTA);
                    heroCarousel.descriptionApp = !string.IsNullOrEmpty(item.Fields[Constant.descriptionApp].Value.ToString()) ? item.Fields[Constant.descriptionApp].Value.ToString() : string.Empty;

                    switch (queryString)
                    {
                        case "app":
                            if (item.Fields[Constant.MobileImage] != null)
                            { heroCarousel.imageSrc = _helper.GetImageURL(item, Constant.MobileImage); }
                            else { heroCarousel.imageSrc = _helper.GetImageURL(item, Constant.AppImage); }
                            break;
                        case "web":
                            if (item.Fields["Image"] != null)
                            { heroCarousel.imageSrc = _helper.GetImageURL(item, "Image"); }
                            else
                            { heroCarousel.imageSrc = _helper.GetImageURL(item, Constant.StanderedImage); }
                            break;
                        default:
                            if (item.Fields["Image"] != null)
                            { heroCarousel.imageSrc = _helper.GetImageURL(item, "Image"); }
                            else
                            { heroCarousel.imageSrc = _helper.GetImageURL(item, Constant.StanderedImage); }
                            break;
                    }

                    if (item.TemplateID == Constant.TemplateId)
                    {
                        heroCarousel.deepLink = !string.IsNullOrEmpty(item.Fields[Constant.deepLink].Value.ToString()) ? item.Fields[Constant.deepLink].Value.ToString() : "";

                        heroCarousel.materialGroup = !string.IsNullOrEmpty(item.Fields[Constant.MaterialGroup].Value.ToString()) ? _helper.SanitizeName(item.Fields[Constant.MaterialGroup].Value.ToString()) : "";
                        heroCarousel.category = !string.IsNullOrEmpty(item.Fields[Constant.Category].Value.ToString()) ? item.Fields[Constant.Category].Value.ToString() : "";
                        heroCarousel.subCategory = !string.IsNullOrEmpty(item.Fields[Constant.SubCategory].Value.ToString()) ? item.Fields[Constant.SubCategory].Value.ToString() : "";
                        heroCarousel.brand = !string.IsNullOrEmpty(item.Fields[Constant.Brand].Value.ToString()) ? item.Fields[Constant.Brand].Value.ToString() : "";
                        //  heroCarousel.airportCode = !string.IsNullOrEmpty(item.Fields[Constant.AirportCode].Value.ToString()) ? item.Fields[Constant.AirportCode].Value.ToString() : "";
                        heroCarousel.storeType = !string.IsNullOrEmpty(item.Fields[Constant.StoreType].Value.ToString()) ? item.Fields[Constant.StoreType].Value.ToString() : "";                        
                        heroCarousel.restricted = ((Constant.RestrictedCarousal.IndexOf(_helper.SanitizeName(heroCarousel.materialGroup.ToLower())) > -1)  && restricted && !string.IsNullOrEmpty(heroCarousel.materialGroup.Trim())) ? true : false;
                        heroCarousel.thumbnailImage = !string.IsNullOrEmpty(item.Fields[Constant.ThumbnailImage].Value) ? _helper.GetImageURL(item, Constant.ThumbnailImage) : "";
                        heroCarousel.bannerCondition = !string.IsNullOrEmpty(item.Fields[Constant.BannerCondition].Value) ? item.Fields[Constant.BannerCondition].Value.ToString() : "";
                        heroCarousel.skuCode = item.Fields.Contains(Constant.SKUCodeID) ? item.Fields[Constant.SKUCodeID].Value.ToString() : "";
                        heroCarousel.uniqueId = !string.IsNullOrEmpty(item.Fields[Constant.UniqueId].Value.ToString()) ? item.Fields[Constant.UniqueId].Value.ToString() : "";
                        heroCarousel.ctaUrl = !string.IsNullOrEmpty(item.Fields[Constant.ctaurl].Value.ToString()) ? _helper.LinkUrl(item.Fields[Constant.ctaurl]) : "";
                        heroCarousel.promoCode = !string.IsNullOrEmpty(item.Fields[Constant.PromoCode].Value.ToString()) ? item.Fields[Constant.PromoCode].Value.ToString() : "";
                        heroCarousel.disableForAirport = _helper.GetAvaialbilityOnAirport(item, airport, storeType);
                        if (storeType.Equals(heroCarousel.storeType.Trim().ToLower()) || heroCarousel.storeType.Equals(""))
                        {
                            Sitecore.Data.Fields.CheckboxField checkboxFieldForWeb = item.Fields[Constant.OnlyforWeb];
                            Sitecore.Data.Fields.CheckboxField checkboxFieldForAPP = item.Fields[Constant.OnlyforApp];
                            if (checkboxFieldForAPP == null && checkboxFieldForWeb == null)
                            {
                                heroCarousel.checkValidity = item.Fields[Constant.CheckValidity] != null ? _helper.GetCheckboxOption(item.Fields[Constant.CheckValidity]) : false;
                                if (heroCarousel.checkValidity)
                                {
                                    Sitecore.Data.Fields.DateField EffFrom = item.Fields[Constant.effectiveFrom];
                                    heroCarousel.effectiveFrom = EffFrom != null ? EffFrom.DateTime.ToString() : string.Empty;
                                    Sitecore.Data.Fields.DateField EffTo = item.Fields[Constant.effectiveTo];
                                    heroCarousel.effectiveTo = EffTo != null ? EffTo.DateTime.ToString() : string.Empty;
                                    if (_helper.getISTDateTime((System.DateTime.Now).ToString()) >= Convert.ToDateTime(heroCarousel.effectiveFrom) && Convert.ToDateTime(heroCarousel.effectiveTo) >= _helper.getISTDateTime((System.DateTime.Now).ToString()))
                                        heroCarouselList.Add(heroCarousel);
                                }
                                else
                                    heroCarouselList.Add(heroCarousel);
                            }
                            if (checkboxFieldForAPP != null && checkboxFieldForWeb != null)                           
                            {
                                if ((!checkboxFieldForWeb.Checked && !checkboxFieldForAPP.Checked) || (checkboxFieldForWeb.Checked && checkboxFieldForAPP.Checked))
                                {
                                    heroCarousel.checkValidity = item.Fields[Constant.CheckValidity] != null ? _helper.GetCheckboxOption(item.Fields[Constant.CheckValidity]) : false;
                                    if (heroCarousel.checkValidity)
                                    {
                                        Sitecore.Data.Fields.DateField EffFrom = item.Fields[Constant.effectiveFrom];
                                        heroCarousel.effectiveFrom = EffFrom != null ? EffFrom.DateTime.ToString() : string.Empty;
                                        Sitecore.Data.Fields.DateField EffTo = item.Fields[Constant.effectiveTo];
                                        heroCarousel.effectiveTo = EffTo != null ? EffTo.DateTime.ToString() : string.Empty;
                                        if (_helper.getISTDateTime((System.DateTime.Now).ToString()) >= Convert.ToDateTime(heroCarousel.effectiveFrom) && Convert.ToDateTime(heroCarousel.effectiveTo) >= _helper.getISTDateTime((System.DateTime.Now).ToString()))
                                            heroCarouselList.Add(heroCarousel);
                                    }
                                    else
                                        heroCarouselList.Add(heroCarousel);
                                }
                            }
                            if (checkboxFieldForWeb != null && checkboxFieldForWeb.Checked && (queryString.Equals("web") || queryString.Equals(string.Empty)))
                            {
                                heroCarouselList.Add(heroCarousel);
                            }
                            if (checkboxFieldForAPP != null && checkboxFieldForAPP.Checked && queryString.Equals("app"))
                            {
                                heroCarouselList.Add(heroCarousel);
                            }
                        }
                        
                    }
                    else
                    {
                        heroCarouselList.Add(heroCarousel);
                    }                                 
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService GetCarouseldata gives -> " + ex.Message);
            }

            return heroCarouselList;
        }
               
    }
}