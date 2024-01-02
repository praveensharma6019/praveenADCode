using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch.SearchTypes;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Services
{
    public class OfferDataParser
    {
        private ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper _helper;

        public OfferDataParser(ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper helper)
        {
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this._helper = helper;
        }
        internal List<object> ParseOffer(IQueryable<SearchResultItem> results, OfferFilters filter)
        {
            List<object> offerMappingsList = new List<object>();
            OfferMapping offerMapping = null;
            try
            {
                foreach (SearchResultItem offer in results.ToList().OrderBy(x => Convert.ToInt32(x[offerFilterContent.PromotionOfferRankSolrField])))
                {
                    offerMapping = new OfferMapping();
                    offerMapping.title = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTitleSolrField)) ?
                                            offer.Fields[key: offerFilterContent.PromotionOfferTitleSolrField].ToString() :
                                                string.Empty;
                    offerMapping.promotionType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferPromotionTypeSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferPromotionTypeSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.promotionCode = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferCodeSolrField)) ?
                                            offer.Fields[key: offerFilterContent.PromotionOfferCodeSolrField].ToString() :
                                                string.Empty;
                    offerMapping.skuCode = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferSKUCodeSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferSKUCodeSolrField].ToString() :
                                                        string.Empty;
                    //offerMapping.offerType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTypeSolrField)) ?
                    //                       offer.Fields[key: offerFilterContent.PromotionOfferTypeSolrField].ToString() :
                    //                           string.Empty;
                    offerMapping.displayText = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDisplayTextSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferDisplayTextSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.savings = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferSavingsFieldSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferSavingsFieldSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.effectiveFrom = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferEffectiveFromSolrField)) ?
                                                    getISTDateTime(offer.Fields[key: offerFilterContent.PromotionOfferEffectiveFromSolrField].ToString()).ToString() :
                                               string.Empty;
                    offerMapping.effectiveTo = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferEffectiveToSolrField)) ?
                                                   getISTDateTime(offer.Fields[key: offerFilterContent.PromotionOfferEffectiveToSolrField].ToString()).ToString() :
                                                        string.Empty;
                    //Addtothelist = (System.DateTime.Now >= Convert.ToDateTime(offerMapping.effectiveFrom) && Convert.ToDateTime(offerMapping.effectiveTo) >= System.DateTime.Now) ? true : false;
                    offerMapping.terminalLocationType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTerminalLocationTypeSolrField)) ?
                                          getAirportDetails(offer.Fields[key: offerFilterContent.PromotionOfferTerminalLocationTypeSolrField].ToString()) :
                                               new List<OfferAirportModel>();
                    offerMapping.terminalStoreType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTerminalStoreTypeSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTerminalStoreTypeSolrField].ToString() :
                                                        string.Empty;

                    offerMapping.promotionDescription = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferOfferdescriptionSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferOfferdescriptionSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.offerTitle = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDetailTitleSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferDetailTitleSolrField].ToString() :
                                                        string.Empty;
                    //offerMapping.pcmClaimType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferpcmClaimTypeSolrField)) ?
                    //                       offer.Fields[key: offerFilterContent.PromotionOfferpcmClaimTypeSolrField].ToString() :
                    //                           string.Empty;
                    //offerMapping.showonHomepage = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferShowonHomepageSolrField)) ?
                    //                                offer.Fields[key: offerFilterContent.PromotionOfferShowonHomepageSolrField].ToString() :
                    //                                    string.Empty;
                    offerMapping.desktopImageSrc = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDesktopImageSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferDesktopImageSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.mobileImageSrc = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferMobileImageSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferMobileImageSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.thumbnailImageSrc = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferThumbnailImageSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferThumbnailImageSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.extraImage = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferExtraImageSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferExtraImageSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.displayRank = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferRankSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferRankSolrField].ToString() :
                                                        string.Empty;
                    //offerMapping.apptype = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferAppTypeSolrField)) ?
                    //                                offer.Fields[key: offerFilterContent.PromotionOfferAppTypeSolrField].ToString() :
                    //                                    string.Empty;
                    offerMapping.linkText = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferLinkTextSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferLinkTextSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.linkURL = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferLinkUrlSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferLinkUrlSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.tcLinkAlt = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTCLinkTextSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTCLinkTextSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.tcLinkUrl = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTCLinkUrlSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTCLinkUrlSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.bannerImageDeskSrc = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferBannerImageDeskSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferBannerImageDeskSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.bannerImageMobSrc = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferBannerImageMobSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferBannerImageMobSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.categoryFilter = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferCategoryFilterSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferCategoryFilterSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.offerUniqueID = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferUniqueIDFilterSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferUniqueIDFilterSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.sitecoreofferId = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferGroupIDFilterSolrField)) ?
                                                    Sitecore.Data.ID.Parse(offer.Fields[key: offerFilterContent.PromotionOfferGroupIDFilterSolrField]).ToString() :
                                                        string.Empty;
                    offerMapping.autoId = (offer.Fields.ContainsKey(offerFilterContent.AutoIdSolrFiled)) ?
                                                    offer.Fields[key: offerFilterContent.AutoIdSolrFiled].ToString() :
                                                        string.Empty;
                    offerMapping.promotionTypeLabel = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferPromotionTypeLabelFieldSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferPromotionTypeLabelFieldSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.tabTitle = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTabTitleSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTabTitleSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.tcEnable = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTCEnableSolrField)) ?
                                                    Convert.ToBoolean((offer.Fields[key: offerFilterContent.PromotionOfferTCEnableSolrField]).ToString()) :
                                                        false;
                    offerMapping.OfferLOB = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferLOBSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferLOBSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.OfferDiscountPrice = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDiscountPriceSolrField)) ?
                                                    Convert.ToInt32(offer.Fields[key: offerFilterContent.PromotionOfferDiscountPriceSolrField].ToString()) :
                                                        0;
                    offerMapping.OfferDiscountPercent = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDiscountPercentSolrField)) ?
                                                    Convert.ToDouble(offer.Fields[key: offerFilterContent.PromotionOfferDiscountPercentSolrField].ToString()) :
                                                        0;
                    offerMapping.isBankOffer = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferisBankOfferSolrField)) ?
                                                    Convert.ToBoolean((offer.Fields[key: offerFilterContent.PromotionOfferisBankOfferSolrField]).ToString()) :
                                                        false;
                    offerMapping.displayedOn = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferdisplayOnPageOfferSolrField)) ?
                                                   offer.Fields[key: offerFilterContent.PromotionOfferdisplayOnPageOfferSolrField].ToString() :
                                                       string.Empty;
                    offerMapping.offerRedirectionLinkText = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferRedirectionTextOfferSolrField)) ?
                                                   offer.Fields[key: offerFilterContent.PromotionOfferRedirectionTextOfferSolrField].ToString() :
                                                       string.Empty;
                    offerMapping.offerRedirectionLinkURL = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferRedirectionURLOfferSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.PromotionOfferRedirectionURLOfferSolrField].ToString() :
                                                      string.Empty;
                    offerMapping.isTravelExclusive = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferisExlusiveOfferSolrField)) ?
                                                    Convert.ToBoolean((offer.Fields[key: offerFilterContent.PromotionOfferisExlusiveOfferSolrField]).ToString()) :
                                                        false;
                    offerMapping.bankOfferText = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferBankOfferTextSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.PromotionOfferBankOfferTextSolrField].ToString() :
                                                      string.Empty;

                    var TC = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTermsandConditionSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTermsandConditionSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.termCondition = getTCList(TC);
                    var GTC = (offer.Fields.ContainsKey(offerFilterContent.Globaltermsandcondition)) ?
                                                    offer.Fields[key: offerFilterContent.Globaltermsandcondition].ToString() :
                                                        string.Empty;
                    offerMapping.globaltermCondition = getTCList(GTC);
                    offerMapping.seofriendlyUrl = (offer.Fields.ContainsKey(offerFilterContent.SeoFriendlyURL)) ?
                                                  offer.Fields[key: offerFilterContent.SeoFriendlyURL].ToString() :
                                                      string.Empty;


                    //Ticket No 17128
                    if (Convert.ToBoolean(filter.isUnlockOffer))
                    {

                        offerMapping.bookingConfirmedOfferText = (offer.Fields.ContainsKey(offerFilterContent.BookingConfirmedOfferText)) ?
                                                  offer.Fields[key: offerFilterContent.BookingConfirmedOfferText].ToString() :
                                                      string.Empty;
                        offerMapping.bookingConfirmedOfferDescription = (offer.Fields.ContainsKey(offerFilterContent.BookingConfirmedOfferDescription)) ?
                                                  offer.Fields[key: offerFilterContent.BookingConfirmedOfferDescription].ToString() :
                                                      string.Empty;
                        offerMapping.offerLogoDesktop = (offer.Fields.ContainsKey(offerFilterContent.OfferLogoDesktop)) ?
                                                  getImageSRC(offer.Fields[key: offerFilterContent.OfferLogoDesktop].ToString()) :
                                                      string.Empty;
                        offerMapping.offerLogoMobile = (offer.Fields.ContainsKey(offerFilterContent.OfferLogoMobile)) ?
                                                  getImageSRC(offer.Fields[key: offerFilterContent.OfferLogoMobile].ToString()) :
                                                      string.Empty;
                        offerMapping.unlockOfferCTAText = (offer.Fields.ContainsKey(offerFilterContent.UnlockOfferCTAText)) ?
                                                 offer.Fields[key: offerFilterContent.UnlockOfferCTAText].ToString() :
                                                     string.Empty;
                        offerMapping.UnlockOfferCTALink = (offer.Fields.ContainsKey(offerFilterContent.UnlockOfferCTALink)) ?
                                                  offer.Fields[key: offerFilterContent.UnlockOfferCTALink].ToString() :
                                                      string.Empty;
                        offerMapping.unlockOfferCTAVisitWesiteText = (offer.Fields.ContainsKey(offerFilterContent.UnlockOfferCTAVisitWesiteText)) ?
                                                 offer.Fields[key: offerFilterContent.UnlockOfferCTAVisitWesiteText].ToString() :
                                                     string.Empty;
                        offerMapping.unlockOfferCTAVisitWesiteLink = (offer.Fields.ContainsKey(offerFilterContent.UnlockOfferCTAVisitWesiteLink)) ?
                                                  offer.Fields[key: offerFilterContent.UnlockOfferCTAVisitWesiteLink].ToString() :
                                                      string.Empty;
                        var htu = (offer.Fields.ContainsKey(offerFilterContent.HowToUse)) ?
                                                    offer.Fields[key: offerFilterContent.HowToUse].ToString() :
                                                        string.Empty;
                        offerMapping.howToUse = getTCList(htu);

                        // Offer Title filed 19493
                        offerMapping.unlockOfferTitle = (offer.Fields.ContainsKey(offerFilterContent.unlockOfferTitle)) ?
                                                  offer.Fields[key: offerFilterContent.unlockOfferTitle].ToString() :
                                                      string.Empty;
                    }
                    if (!string.IsNullOrEmpty(offerMapping.effectiveFrom) &&
                        !string.IsNullOrEmpty(offerMapping.effectiveTo) &&
                        (System.DateTime.Now >= Convert.ToDateTime(offerMapping.effectiveFrom) && Convert.ToDateTime(offerMapping.effectiveTo) >= System.DateTime.Now))
                        offerMappingsList.Add(offerMapping);
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("ParseOffer in OffersController gives error -> " + ex.Message);
            }

            return offerMappingsList;

        }

        internal async Task<List<object>> ParseOfferAsync(IQueryable<SearchResultItem> results, OfferFilters filter)
        {
            List<object> offerMappingsList = new List<object>();
            OfferMapping offerMapping = null;
            try
            {
                foreach (SearchResultItem offer in results.ToList().OrderBy(x => Convert.ToInt32(x[offerFilterContent.PromotionOfferRankSolrField])))
                {
                    offerMapping = new OfferMapping();
                    offerMapping.title = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTitleSolrField)) ?
                                            offer.Fields[key: offerFilterContent.PromotionOfferTitleSolrField].ToString() :
                                                string.Empty;
                    offerMapping.promotionType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferPromotionTypeSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferPromotionTypeSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.promotionCode = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferCodeSolrField)) ?
                                            offer.Fields[key: offerFilterContent.PromotionOfferCodeSolrField].ToString() :
                                                string.Empty;
                    offerMapping.skuCode = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferSKUCodeSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferSKUCodeSolrField].ToString() :
                                                        string.Empty;
                    //offerMapping.offerType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTypeSolrField)) ?
                    //                       offer.Fields[key: offerFilterContent.PromotionOfferTypeSolrField].ToString() :
                    //                           string.Empty;
                    offerMapping.displayText = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDisplayTextSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferDisplayTextSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.savings = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferSavingsFieldSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferSavingsFieldSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.effectiveFrom = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferEffectiveFromSolrField)) ?
                                                    getISTDateTime(offer.Fields[key: offerFilterContent.PromotionOfferEffectiveFromSolrField].ToString()).ToString() :
                                               string.Empty;
                    offerMapping.effectiveTo = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferEffectiveToSolrField)) ?
                                                   getISTDateTime(offer.Fields[key: offerFilterContent.PromotionOfferEffectiveToSolrField].ToString()).ToString() :
                                                        string.Empty;
                    //Addtothelist = (System.DateTime.Now >= Convert.ToDateTime(offerMapping.effectiveFrom) && Convert.ToDateTime(offerMapping.effectiveTo) >= System.DateTime.Now) ? true : false;
                    offerMapping.terminalLocationType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTerminalLocationTypeSolrField)) ?
                                          getAirportDetails(offer.Fields[key: offerFilterContent.PromotionOfferTerminalLocationTypeSolrField].ToString()) :
                                               new List<OfferAirportModel>();
                    offerMapping.terminalStoreType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTerminalStoreTypeSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTerminalStoreTypeSolrField].ToString() :
                                                        string.Empty;

                    offerMapping.promotionDescription = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferOfferdescriptionSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferOfferdescriptionSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.offerTitle = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDetailTitleSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferDetailTitleSolrField].ToString() :
                                                        string.Empty;
                    //offerMapping.pcmClaimType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferpcmClaimTypeSolrField)) ?
                    //                       offer.Fields[key: offerFilterContent.PromotionOfferpcmClaimTypeSolrField].ToString() :
                    //                           string.Empty;
                    //offerMapping.showonHomepage = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferShowonHomepageSolrField)) ?
                    //                                offer.Fields[key: offerFilterContent.PromotionOfferShowonHomepageSolrField].ToString() :
                    //                                    string.Empty;
                    offerMapping.desktopImageSrc = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDesktopImageSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferDesktopImageSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.mobileImageSrc = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferMobileImageSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferMobileImageSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.thumbnailImageSrc = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferThumbnailImageSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferThumbnailImageSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.extraImage = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferExtraImageSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferExtraImageSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.displayRank = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferRankSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferRankSolrField].ToString() :
                                                        string.Empty;
                    //offerMapping.apptype = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferAppTypeSolrField)) ?
                    //                                offer.Fields[key: offerFilterContent.PromotionOfferAppTypeSolrField].ToString() :
                    //                                    string.Empty;
                    offerMapping.linkText = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferLinkTextSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferLinkTextSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.linkURL = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferLinkUrlSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferLinkUrlSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.tcLinkAlt = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTCLinkTextSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTCLinkTextSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.tcLinkUrl = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTCLinkUrlSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTCLinkUrlSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.bannerImageDeskSrc = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferBannerImageDeskSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferBannerImageDeskSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.bannerImageMobSrc = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferBannerImageMobSolrField)) ?
                                                    getImageSRC(offer.Fields[key: offerFilterContent.PromotionOfferBannerImageMobSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.categoryFilter = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferCategoryFilterSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferCategoryFilterSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.offerUniqueID = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferUniqueIDFilterSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferUniqueIDFilterSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.sitecoreofferId = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferGroupIDFilterSolrField)) ?
                                                    Sitecore.Data.ID.Parse(offer.Fields[key: offerFilterContent.PromotionOfferGroupIDFilterSolrField]).ToString() :
                                                        string.Empty;
                    offerMapping.autoId = (offer.Fields.ContainsKey(offerFilterContent.AutoIdSolrFiled)) ?
                                                    offer.Fields[key: offerFilterContent.AutoIdSolrFiled].ToString() :
                                                        string.Empty;
                    offerMapping.promotionTypeLabel = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferPromotionTypeLabelFieldSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferPromotionTypeLabelFieldSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.tabTitle = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTabTitleSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTabTitleSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.tcEnable = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTCEnableSolrField)) ?
                                                    Convert.ToBoolean((offer.Fields[key: offerFilterContent.PromotionOfferTCEnableSolrField]).ToString()) :
                                                        false;
                    offerMapping.OfferLOB = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferLOBSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferLOBSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.OfferDiscountPrice = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDiscountPriceSolrField)) ?
                                                    Convert.ToInt32(offer.Fields[key: offerFilterContent.PromotionOfferDiscountPriceSolrField].ToString()) :
                                                        0;
                    offerMapping.OfferDiscountPercent = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDiscountPercentSolrField)) ?
                                                    Convert.ToDouble(offer.Fields[key: offerFilterContent.PromotionOfferDiscountPercentSolrField].ToString()) :
                                                        0;
                    offerMapping.isBankOffer = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferisBankOfferSolrField)) ?
                                                    Convert.ToBoolean((offer.Fields[key: offerFilterContent.PromotionOfferisBankOfferSolrField]).ToString()) :
                                                        false;
                    offerMapping.displayedOn = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferdisplayOnPageOfferSolrField)) ?
                                                   offer.Fields[key: offerFilterContent.PromotionOfferdisplayOnPageOfferSolrField].ToString() :
                                                       string.Empty;
                    offerMapping.offerRedirectionLinkText = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferRedirectionTextOfferSolrField)) ?
                                                   offer.Fields[key: offerFilterContent.PromotionOfferRedirectionTextOfferSolrField].ToString() :
                                                       string.Empty;
                    offerMapping.offerRedirectionLinkURL = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferRedirectionURLOfferSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.PromotionOfferRedirectionURLOfferSolrField].ToString() :
                                                      string.Empty;
                    offerMapping.isTravelExclusive = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferisExlusiveOfferSolrField)) ?
                                                    Convert.ToBoolean((offer.Fields[key: offerFilterContent.PromotionOfferisExlusiveOfferSolrField]).ToString()) :
                                                        false;
                    offerMapping.bankOfferText = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferBankOfferTextSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.PromotionOfferBankOfferTextSolrField].ToString() :
                                                      string.Empty;

                    var TC = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTermsandConditionSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTermsandConditionSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.termCondition = getTCList(TC);
                    var GTC = (offer.Fields.ContainsKey(offerFilterContent.Globaltermsandcondition)) ?
                                                    offer.Fields[key: offerFilterContent.Globaltermsandcondition].ToString() :
                                                        string.Empty;
                    offerMapping.globaltermCondition = getTCList(GTC);
                    offerMapping.seofriendlyUrl = (offer.Fields.ContainsKey(offerFilterContent.SeoFriendlyURL)) ?
                                                  offer.Fields[key: offerFilterContent.SeoFriendlyURL].ToString() :
                                                      string.Empty;


                    //Ticket No 17128
                    if (Convert.ToBoolean(filter.isUnlockOffer))
                    {

                        offerMapping.bookingConfirmedOfferText = (offer.Fields.ContainsKey(offerFilterContent.BookingConfirmedOfferText)) ?
                                                  offer.Fields[key: offerFilterContent.BookingConfirmedOfferText].ToString() :
                                                      string.Empty;
                        offerMapping.bookingConfirmedOfferDescription = (offer.Fields.ContainsKey(offerFilterContent.BookingConfirmedOfferDescription)) ?
                                                  offer.Fields[key: offerFilterContent.BookingConfirmedOfferDescription].ToString() :
                                                      string.Empty;
                        offerMapping.offerLogoDesktop = (offer.Fields.ContainsKey(offerFilterContent.OfferLogoDesktop)) ?
                                                  getImageSRC(offer.Fields[key: offerFilterContent.OfferLogoDesktop].ToString()) :
                                                      string.Empty;
                        offerMapping.offerLogoMobile = (offer.Fields.ContainsKey(offerFilterContent.OfferLogoMobile)) ?
                                                  getImageSRC(offer.Fields[key: offerFilterContent.OfferLogoMobile].ToString()) :
                                                      string.Empty;
                        offerMapping.unlockOfferCTAText = (offer.Fields.ContainsKey(offerFilterContent.UnlockOfferCTAText)) ?
                                                 offer.Fields[key: offerFilterContent.UnlockOfferCTAText].ToString() :
                                                     string.Empty;
                        offerMapping.UnlockOfferCTALink = (offer.Fields.ContainsKey(offerFilterContent.UnlockOfferCTALink)) ?
                                                  offer.Fields[key: offerFilterContent.UnlockOfferCTALink].ToString() :
                                                      string.Empty;
                        offerMapping.unlockOfferCTAVisitWesiteText = (offer.Fields.ContainsKey(offerFilterContent.UnlockOfferCTAVisitWesiteText)) ?
                                                 offer.Fields[key: offerFilterContent.UnlockOfferCTAVisitWesiteText].ToString() :
                                                     string.Empty;
                        offerMapping.unlockOfferCTAVisitWesiteLink = (offer.Fields.ContainsKey(offerFilterContent.UnlockOfferCTAVisitWesiteLink)) ?
                                                  offer.Fields[key: offerFilterContent.UnlockOfferCTAVisitWesiteLink].ToString() :
                                                      string.Empty;
                        var htu = (offer.Fields.ContainsKey(offerFilterContent.HowToUse)) ?
                                                    offer.Fields[key: offerFilterContent.HowToUse].ToString() :
                                                        string.Empty;
                        offerMapping.howToUse = getTCList(htu);

                        // Offer Title filed 19493
                        offerMapping.unlockOfferTitle = (offer.Fields.ContainsKey(offerFilterContent.unlockOfferTitle)) ?
                                                  offer.Fields[key: offerFilterContent.unlockOfferTitle].ToString() :
                                                      string.Empty;
                    }
                    if (!string.IsNullOrEmpty(offerMapping.effectiveFrom) &&
                        !string.IsNullOrEmpty(offerMapping.effectiveTo) &&
                        (System.DateTime.Now >= Convert.ToDateTime(offerMapping.effectiveFrom) && Convert.ToDateTime(offerMapping.effectiveTo) >= System.DateTime.Now))
                        offerMappingsList.Add(offerMapping);
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("ParseOffer in OffersController gives error -> " + ex.Message);
            }

            return offerMappingsList;

        }

        private DateTime getISTDateTime(string datetimeString)
        {
            DateTime utcdate = DateTime.Parse(datetimeString, CultureInfo.InvariantCulture);
            return TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }

        private List<OfferAirportModel> getAirportDetails(string airportItem)
        {
            Sitecore.Data.Database contextDB = Sitecore.Context.Database;
            List<OfferAirportModel> TerminalLocationType = new List<OfferAirportModel>();
            OfferAirportModel offerAirportModel;
            try
            {
                if (contextDB != null)
                {
                    foreach (var airportId in airportItem.Split('|'))
                    {
                        offerAirportModel = new OfferAirportModel();
                        var AirportData = contextDB.GetItem(Sitecore.Data.ID.Parse(airportId));
                        offerAirportModel = new OfferAirportModel
                        {
                            title = AirportData.Fields[AirportFieldConstant.Title].Value,
                            description = AirportData.Fields[AirportFieldConstant.Description].Value,
                            descriptionApp = AirportData.Fields[AirportFieldConstant.DescriptionApp].Value

                        };
                        offerAirportModel.airportCTALink = _helper.GetLinkURL(AirportData, AirportFieldConstant.CTALink);
                        offerAirportModel.airportCTAText = _helper.GetLinkText(AirportData, AirportFieldConstant.CTALink);
                        offerAirportModel.airportDesktopImageSrc = _helper.GetImageURL(AirportData, AirportFieldConstant.StanderedImage);
                        offerAirportModel.airportMobileImageSrc = _helper.GetImageURL(AirportData, AirportFieldConstant.Image);
                        TerminalLocationType.Add(offerAirportModel);
                    }
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("getAirportDetails in OffersController gives error -> " + ex.Message);
            }
            return TerminalLocationType;
        }

        private string getImageSRC(string ImageURL)
        {
            return ImageURL.Replace("/sitecore/shell", "");
        }

        private List<string> getTCList(string tC)
        {
            List<string> tconditionList = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(tC))
                {
                    foreach (var tcondition in tC.Split('$'))
                    {
                        tconditionList.Add(tcondition);
                    }
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("getTCList in OffersController gives error -> " + ex.Message);
            }

            return tconditionList;
        }

    }
}