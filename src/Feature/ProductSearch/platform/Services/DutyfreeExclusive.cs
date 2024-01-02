using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services
{
    public class DutyfreeExclusive : IDutyfreeExclusive
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetService;
        private readonly IHelper _helper;
        private readonly ISearchItems searchItems;

        public DutyfreeExclusive(ILogRepository _logRepository, IWidgetService widgetService, IHelper helper, ISearchItems searchItems)
        {
            this._logRepository = _logRepository;
            this._widgetService = widgetService;
            this._helper = helper;
            this.searchItems = searchItems;
        }
        ExclusiveOffersWidgets IDutyfreeExclusive.GetExclusiveOffers(Rendering rendering, string queryString, string language, string airport, string storeType)
        {
            ExclusiveOffersWidgets exclusiveOffersWidget = new ExclusiveOffersWidgets();

            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                exclusiveOffersWidget.widget = widget != null ? _widgetService.GetWidgetItem(widget) : new WidgetItem();

                exclusiveOffersWidget.widget.widgetItems = GetAvailableExclusiveOfferList(rendering, queryString, airport, storeType, language);
                
            }
            catch (Exception ex)
            {
                _logRepository.Error(" DutyfreeExclusive GetExclusiveOffers gives -> " + ex.Message);
            }

            return exclusiveOffersWidget;
        }

        private List<object> GetAvailableExclusiveOfferList(Rendering rendering, string queryString, string airport, string storeType, string language)
        {
            List<Object> offers = new List<object>();

            List<ExclusiveOffer> exclusiveOfferList = GetExclusiveOfferList(rendering, queryString, airport, storeType, language);

            FilterRequest filters = new FilterRequest();
            filters.skuCode = exclusiveOfferList.Select(a => a.skuCode).ToArray();
            filters.airportCode = airport;
            filters.storeType = storeType;

            List<Result> productList = searchItems.GetDutyfreeProductsFromAPI(null, filters, language, airport, storeType);

            List<string> availableProducts = productList.Where(p => p.availability == true).Select(s => s.skuCode).ToList();

            exclusiveOfferList = exclusiveOfferList.Where(p => p.storeType.Equals(storeType) && availableProducts.Contains(p.skuCode)).ToList();

            foreach(ExclusiveOffer offer in exclusiveOfferList)
            {
                offers.Add(offer);
            }

            return offers;
        }


        private List<ExclusiveOffer> GetExclusiveOfferList(Rendering rendering, string queryString, string airport, string storeType, string language)
        {
            List<ExclusiveOffer> exclusiveOfferList = new List<ExclusiveOffer>();

            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Error("DutyfreeExclusive   GetExclusiveOfferList gives error --- Datasource not available");
                    return exclusiveOfferList;
                }
                              
               
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    ExclusiveOffer exclusiveOffer = new ExclusiveOffer();
                    exclusiveOffer.title = !string.IsNullOrEmpty(item.Fields[Constant.BannerTitle].Value.ToString()) ? item.Fields[Constant.BannerTitle].Value.ToString() : "";
                    exclusiveOffer.subTitle = !string.IsNullOrEmpty(item.Fields[Constant.BannerSubTitle].Value.ToString()) ? item.Fields[Constant.BannerSubTitle].Value.ToString() : "";
                    exclusiveOffer.description = !string.IsNullOrEmpty(item.Fields[Constant.BannerDescription].Value.ToString()) ? item.Fields[Constant.BannerDescription].Value.ToString() : "";
                    exclusiveOffer.ctaLink = item.Fields[Constant.CTA] != null ? _helper.GetLinkURL(item, Constant.CTA) : String.Empty;
                    exclusiveOffer.linkTarget = item.Fields[Constant.CTA] != null ? _helper.LinkUrlTarget(item.Fields[Constant.CTA]) : String.Empty;
                    
                    GTMTags tags = new GTMTags
                    {
                        bannerCategory = item[Constant.bannerCategory],
                        faqCategory = item[Constant.faqCategory],
                        subCategory = item[Constant.subCategory],
                        category = item[Constant.category],
                        businessUnit = item[Constant.businessUnit],
                        label = item[Constant.label],
                        source = item[Constant.source],
                        type = item[Constant.GTMtype],
                        eventName = item[Constant.eventName]
                    };
                    exclusiveOffer.tags = tags;
                    if (item.Fields[Constant.BannerAppImage] != null)
                    { exclusiveOffer.mobileImage = _helper.GetImageURL(item, Constant.BannerAppImage); }
                    if (item.Fields[Constant.BannerDeskTopImage] != null)
                    { exclusiveOffer.webImage = _helper.GetImageURL(item, Constant.BannerDeskTopImage); }
                    exclusiveOffer.ctaText = _helper.GetLinkText(item, Constant.CTA);
                    exclusiveOffer.descriptionApp = !string.IsNullOrEmpty(item.Fields[Constant.descriptionApp].Value.ToString()) ? item.Fields[Constant.descriptionApp].Value.ToString() : string.Empty;

                    switch (queryString)
                    {
                        case "app":
                            if (item.Fields[Constant.BannerMobileImage] != null)
                            { exclusiveOffer.imageSrc = _helper.GetImageURL(item, Constant.BannerMobileImage); }
                            else { exclusiveOffer.imageSrc = _helper.GetImageURL(item, Constant.BannerAppImage); }
                            break;
                        case "web":
                            if (item.Fields["Image"] != null)
                            { exclusiveOffer.imageSrc = _helper.GetImageURL(item, "Image"); }
                            else
                            { exclusiveOffer.imageSrc = _helper.GetImageURL(item, Constant.BannerStanderedImage); }
                            break;
                        default:
                            if (item.Fields["Image"] != null)
                            { exclusiveOffer.imageSrc = _helper.GetImageURL(item, "Image"); }
                            else
                            { exclusiveOffer.imageSrc = _helper.GetImageURL(item, Constant.BannerStanderedImage); }
                            break;
                    }

                    if (item.TemplateID == Constant.HeroCarousalTemplateId)
                    {
                        exclusiveOffer.deepLink = !string.IsNullOrEmpty(item.Fields[Constant.BannerDeepLink].Value.ToString()) ? item.Fields[Constant.BannerDeepLink].Value.ToString() : "";

                        exclusiveOffer.materialGroup = !string.IsNullOrEmpty(item.Fields[Constant.BannerMaterialGroup].Value.ToString()) ? item.Fields[Constant.BannerMaterialGroup].Value.ToString() : "";
                        exclusiveOffer.category = !string.IsNullOrEmpty(item.Fields[Constant.BannerCategory].Value.ToString()) ? item.Fields[Constant.BannerCategory].Value.ToString() : "";
                        exclusiveOffer.subCategory = !string.IsNullOrEmpty(item.Fields[Constant.BannerSubCategory].Value.ToString()) ? item.Fields[Constant.BannerSubCategory].Value.ToString() : "";
                        exclusiveOffer.brand = !string.IsNullOrEmpty(item.Fields[Constant.BannerBrand].Value.ToString()) ? item.Fields[Constant.BannerBrand].Value.ToString() : "";
                       // exclusiveOffer.airportCode = !string.IsNullOrEmpty(item.Fields[Constant.AirportCode].Value.ToString()) ? item.Fields[Constant.AirportCode].Value.ToString() : "";
                        exclusiveOffer.storeType = !string.IsNullOrEmpty(item.Fields[Constant.BannerStoreType].Value.ToString()) ? item.Fields[Constant.BannerStoreType].Value.ToString() : "";
                        exclusiveOffer.storeType = (exclusiveOffer.storeType.Trim().ToLower().Equals("arrival") || exclusiveOffer.storeType.Trim().ToLower().Equals("departure")) ? exclusiveOffer.storeType.Trim().ToLower() : storeType;
                        exclusiveOffer.restricted = exclusiveOffer.materialGroup.ToLower().Equals(Constant.RestrictedCarousal.ToLower()) ? true : false;
                        exclusiveOffer.bannerCondition = !string.IsNullOrEmpty(item.Fields[Constant.BannerCondition].Value) ? item.Fields[Constant.BannerCondition].Value.ToString() : "";
                        exclusiveOffer.skuCode = item.Fields.Contains(Constant.SKUCodeID) ? item.Fields[Constant.SKUCodeID].Value.ToString() : "";
                        exclusiveOffer.uniqueId = !string.IsNullOrEmpty(item.Fields[Constant.UniqueId].Value.ToString()) ? item.Fields[Constant.UniqueId].Value.ToString() : "";
                        exclusiveOffer.ctaUrl = !string.IsNullOrEmpty(item.Fields[Constant.BannerCtaurl].Value.ToString()) ? _helper.LinkUrl(item.Fields[Constant.BannerCtaurl]) : "";
                        exclusiveOffer.promoCode = !string.IsNullOrEmpty(item.Fields[Constant.BannerPromoCode].Value.ToString()) ? item.Fields[Constant.BannerPromoCode].Value.ToString() : "";
                        exclusiveOffer.disableForAirport = false;

                        exclusiveOfferList.Add(exclusiveOffer);                        
                    }                    
                }             
              
            }
            catch (Exception ex)
            {
                _logRepository.Error("DutyfreeExclusive   GetExclusiveOfferList gives -> " + ex.Message);
            }

            return exclusiveOfferList;
        }

       
    }
}