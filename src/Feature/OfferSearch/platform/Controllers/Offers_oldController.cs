using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Controllers
{
    public class Offers_oldController : ApiController
    {
        // GET: Offers
        private ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper _helper;
        public Offers_oldController(ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper helper)
        {
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this._helper = helper;
        }

        [HttpPost]
        [Route("api/GetOffers")]
        public IHttpActionResult GetPromotionAndOffers([FromBody] OfferFilters_old filter)
        {
            ResponseDataOld responseData = new ResponseDataOld();
            ResultDataOld resultData = new ResultDataOld();
            if (!string.IsNullOrEmpty(filter.AirportCode))
            {
                IQueryable<SearchResultItem> results = searchBuilder.GetOfferSearchResults(Services.OfferPredicateBuilder_old.GetOfferPredicate(filter));
                try
                {
                    resultData.result = ParseOffer(results, filter);
                    if (filter.appType == "web" && string.IsNullOrEmpty(filter.OfferUniqueID) && string.IsNullOrEmpty(filter.Code) && string.IsNullOrEmpty(filter.LOB))
                    {
                        var tabList = (from OfferMapping_old item in resultData.result select item.tabTitle.ToString()).Distinct();
                        Item metaData = Sitecore.Context.Database.GetItem("/sitecore/content/AirportHome/Datasource/Adani/Offers/OffersMetaData");
                        resultData.metaInformation = GetMetaInformation(tabList, metaData);
                    }
                    if (resultData.result != null)
                    {
                        resultData.count = resultData.result.Count;
                        responseData.status = true;
                        responseData.data = resultData;
                    }
                }
                catch (Exception ex)
                {

                    logRepository.Error("GetPromotionAndOffers in OffersController gives error -> " + ex.Message);
                }
            }


            return Json(responseData);
        }

        [HttpPost]
        [Route("api/GetCartOffers")]
        public IHttpActionResult GetCartPageOffers([FromBody] OfferFilters_old filter)
        {
            ResponseDataOld responseData = new ResponseDataOld();
            ResultDataOld resultData = new ResultDataOld();
            if (!string.IsNullOrEmpty(filter.AirportCode))
            {
                IQueryable<SearchResultItem> results = searchBuilder.GetOfferSearchResults(Services.OfferPredicateBuilder_old.GetCartOfferPredicate(filter));
                try
                {
                    resultData.result = ParseOffer(results, filter);
                    if (resultData.result != null)
                    {
                        resultData.count = resultData.result.Count;
                        responseData.status = true;
                        responseData.data = resultData;
                    }
                }
                catch (Exception ex)
                {

                    logRepository.Error("GetCartPageOffers in OffersController gives error -> " + ex.Message);
                }
            }
            return Json(responseData);
        }

        private List<OfferMapping_old> ParseOffer(IQueryable<SearchResultItem> results, OfferFilters_old filter)
        {
            OfferMapping_old offerMapping = null;
            //bool Addtothelist = false;
            List<OfferMapping_old> offerMappings = new List<OfferMapping_old>();
            try
            {
                //results = results.Where(x => ((DateTime)(x.Fields["effective_to_tdt"])) >= DateTime.Now.Date);
                foreach (SearchResultItem offer in results.ToList().OrderBy(x => Convert.ToInt32(x[offerFilterContent.PromotionOfferRankSolrField])))
                {
                    offerMapping = new OfferMapping_old();
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
                    offerMapping.offerType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTypeSolrField)) ?
                                           offer.Fields[key: offerFilterContent.PromotionOfferTypeSolrField].ToString() :
                                               string.Empty;
                    offerMapping.displayText = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDisplayTextSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferDisplayTextSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.savings = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferSavingsFieldSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferSavingsFieldSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.effectiveFrom = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferEffectiveFromSolrField)) ?
                                                    (offer.Fields[key: offerFilterContent.PromotionOfferEffectiveFromSolrField].ToString()) :
                                               string.Empty;
                    offerMapping.effectiveTo = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferEffectiveToSolrField)) ?
                                                   (offer.Fields[key: offerFilterContent.PromotionOfferEffectiveToSolrField].ToString()) :
                                                        string.Empty;
                    offerMapping.terminalLocationType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTerminalLocationTypeSolrField)) ?
                                           offer.Fields[key: offerFilterContent.PromotionOfferTerminalLocationTypeSolrField].ToString() :
                                               string.Empty;
                    offerMapping.terminalStoreType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTerminalStoreTypeSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTerminalStoreTypeSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.promotionDescription = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferOfferdescriptionSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferOfferdescriptionSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.offerTitle = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferDetailTitleSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferDetailTitleSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.pcmClaimType = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferpcmClaimTypeSolrField)) ?
                                           offer.Fields[key: offerFilterContent.PromotionOfferpcmClaimTypeSolrField].ToString() :
                                               string.Empty;
                    offerMapping.showonHomepage = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferShowonHomepageSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferShowonHomepageSolrField].ToString() :
                                                        string.Empty;
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
                    offerMapping.apptype = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferAppTypeSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferAppTypeSolrField].ToString() :
                                                        string.Empty;
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
                    //Ticket No 23293
                    offerMapping.tabsubtitle = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTabSubTitleSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTabSubTitleSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.offerServicesRedirectionLink = (offer.Fields.ContainsKey(offerFilterContent.OfferServicesRedirectionLinkSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.OfferServicesRedirectionLinkSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.offerFullScreenImage = (offer.Fields.ContainsKey(offerFilterContent.OfferFullScreenImage)) ?
                                getImageSRC(offer.Fields[key: offerFilterContent.OfferFullScreenImage].ToString()) :
                                    string.Empty;
                    offerMapping.notClickable = (offer.Fields.ContainsKey(offerFilterContent.NotClickableSolrField)) ?
                                                    Convert.ToBoolean((offer.Fields[key: offerFilterContent.NotClickableSolrField]).ToString()) :
                                                        false;
                    var TC = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTermsandConditionSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTermsandConditionSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.termCondition = getTCList(TC);
                    offerMapping.isInstoreOffer = (offer.Fields.ContainsKey(offerFilterContent.IsInstoreOfferSolrField)) ?
                                                    Convert.ToBoolean((offer.Fields[key: offerFilterContent.IsInstoreOfferSolrField]).ToString()) :
                                                        false;
                    if (offerMapping.isInstoreOffer)
                    {
                        string similar = (offer.Fields.ContainsKey(offerFilterContent.SimilarOffersSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.SimilarOffersSolrField].ToString() :
                                                        string.Empty;
                        offerMapping.similarOffers = getTCList(similar);
                        offerMapping.tncViewAllLink = (offer.Fields.ContainsKey(offerFilterContent.TNCViewAllLink)) ?
                                                    offer.Fields[key: offerFilterContent.TNCViewAllLink].ToString() :
                                                        string.Empty;
                    }
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
                    if (string.IsNullOrEmpty(filter.OfferUniqueID) && string.IsNullOrEmpty(filter.Code))
                    {
                        if (!string.IsNullOrEmpty(offerMapping.effectiveFrom) &&
                        !string.IsNullOrEmpty(offerMapping.effectiveTo) &&
                        (_helper.getISTDateTime((System.DateTime.Now).ToString()) >= Convert.ToDateTime(offerMapping.effectiveFrom) && Convert.ToDateTime(offerMapping.effectiveTo) >= _helper.getISTDateTime((System.DateTime.Now).ToString())))
                            offerMappings.Add(offerMapping);
                    }
                    else
                    {
                        offerMapping.metaTags = new OfferMapping_old.MetaTags();
                        offerMapping.metaTags.metaTitle = (offer.Fields.ContainsKey(offerFilterContent.metaTitleSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.metaTitleSolrField].ToString() :
                                                      string.Empty;
                        offerMapping.metaTags.metaDescription = (offer.Fields.ContainsKey(offerFilterContent.metaDescriptionSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.metaDescriptionSolrField].ToString() :
                                                      string.Empty;
                        offerMapping.metaTags.keywords = (offer.Fields.ContainsKey(offerFilterContent.keywordsSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.keywordsSolrField].ToString() :
                                                      string.Empty;
                        offerMapping.metaTags.breadcrumbTitle = (offer.Fields.ContainsKey(offerFilterContent.breadcrumbTitle)) ?
                                                  offer.Fields[key: offerFilterContent.breadcrumbTitle].ToString() :
                                                      string.Empty;
                        offerMapping.metaTags.canonical = (offer.Fields.ContainsKey(offerFilterContent.canonicalSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.canonicalSolrField].ToString() :
                                                      string.Empty;

                        if (offerMapping.OfferLOB == "fnb")
                        {
                            offerMapping.storeInfo = new OfferMapping_old.StoreInfo();
                            offerMapping.storeInfo.terminalCode = (offer.Fields.ContainsKey(offerFilterContent.terminalCodeSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.terminalCodeSolrField].ToString() :
                                                      string.Empty;
                            offerMapping.storeInfo.terminalGate = (offer.Fields.ContainsKey(offerFilterContent.terminalGateSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.terminalGateSolrField].ToString() :
                                                      string.Empty;
                            offerMapping.storeInfo.shopId = (offer.Fields.ContainsKey(offerFilterContent.shopIdSolrField)) ?
                                                  offer.Fields[key: offerFilterContent.shopIdSolrField].ToString() :
                                                      string.Empty;
                        }

                        if (!string.IsNullOrEmpty(offerMapping.effectiveFrom) &&
                        !string.IsNullOrEmpty(offerMapping.effectiveTo) &&
                        (_helper.getISTDateTime((System.DateTime.Now).ToString()) < Convert.ToDateTime(offerMapping.effectiveFrom) || Convert.ToDateTime(offerMapping.effectiveTo) < _helper.getISTDateTime((System.DateTime.Now).ToString())))
                        {
                            offerMapping.effectiveTo = "Expired";
                            offerMapping.isExpired = true;
                            offerMappings.Add(offerMapping);
                        }
                        else
                        {
                            offerMappings.Add(offerMapping);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("ParseOffer in OffersController gives error -> " + ex.Message);
            }
            if (string.IsNullOrEmpty(filter.OfferUniqueID) && string.IsNullOrEmpty(filter.Code))
            {
                offerMappings = offerMappings.Where(p => p.showonHomepage.ToLower().Equals("true")).ToList();
            }
            return offerMappings;
        }
        /// <summary>
        /// Code added to resolve Ticket No 19742 to handle "/sitecore/shell" in Media URL
        /// </summary>
        /// <param name="ImageURL"></param>
        /// <returns></returns>
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

        //Ticket No 17128
        [HttpPost]
        [Route("api/GetUnlockedOffers")]
        public IHttpActionResult GetUnlockOffers([FromBody] OfferFilters_old filter)
        {
            ResponseDataOld responseData = new ResponseDataOld();
            ResultDataOld resultData = new ResultDataOld();
            if (!string.IsNullOrEmpty(filter.AirportCode) && Convert.ToBoolean(filter.isUnlockOffer))
            {
                IQueryable<SearchResultItem> results = searchBuilder.GetOfferSearchResults(Services.OfferPredicateBuilder_old.GetOfferPredicate(filter));
                try
                {
                    resultData.result = ParseOffer(results, filter);
                    if (resultData.result != null)
                    {
                        resultData.count = resultData.result.Count;
                        responseData.status = true;
                        responseData.data = resultData;
                    }
                }
                catch (Exception ex)
                {

                    logRepository.Error("GetUnlockOffers in OffersController gives error -> " + ex.Message);
                }
            }


            return Json(responseData);
        }

        private List<MetaInformation> GetMetaInformation(IEnumerable<string> tabList, Item metaDatas)
        {
            List<MetaInformation> metaDataList = new List<MetaInformation>();

            MetaInformation metaInformation = null;
            foreach (string tab in tabList)
            {
                Item metaData = metaDatas.Children.Where(x => x.Name.Equals(tab)).FirstOrDefault();
                if (metaData == null)
                    break;
                metaInformation = new MetaInformation();
                metaInformation.tabTitle = tab;
                metaInformation.data.metaTitle = metaData.Fields[MetaTagsConstant.MetaTitle] != null ? metaData.Fields[MetaTagsConstant.MetaTitle].Value : string.Empty;
                metaInformation.data.metaDescription = metaData.Fields[MetaTagsConstant.MetaDescription] != null ? metaData.Fields[MetaTagsConstant.MetaDescription].Value : string.Empty;
                metaInformation.data.keywords = metaData.Fields[MetaTagsConstant.Keywords] != null ? metaData.Fields[MetaTagsConstant.Keywords].Value : string.Empty;
                metaInformation.data.robots = metaData.Fields[MetaTagsConstant.Robots] != null ? metaData.Fields[MetaTagsConstant.Robots].Value : string.Empty;
                metaInformation.data.oG_Title = metaData.Fields[MetaTagsConstant.OGTitle] != null ? metaData.Fields[MetaTagsConstant.OGTitle].Value : string.Empty;
                metaInformation.data.oG_Image = metaData.Fields[MetaTagsConstant.OGImage] != null ? metaData.Fields[MetaTagsConstant.OGImage].Value : string.Empty;
                metaInformation.data.oG_Description = metaData.Fields[MetaTagsConstant.OGDescription] != null ? metaData.Fields[MetaTagsConstant.OGDescription].Value : string.Empty;
                metaInformation.data.viewport = metaData.Fields[MetaTagsConstant.Viewport] != null ? metaData.Fields[MetaTagsConstant.Viewport].Value : string.Empty;
                metaInformation.data.canonical = metaData.Fields[MetaTagsConstant.Canonical] != null ? _helper.GetLinkURL(metaData, MetaTagsConstant.Canonical) : string.Empty;
                metaInformation.data.richTextBody = metaData.Fields[MetaTagsConstant.RichText] != null ? metaData.Fields[MetaTagsConstant.RichText].Value : string.Empty;
                metaInformation.data.richTextTitle = metaData.Fields[MetaTagsConstant.RichTextTitle] != null ? metaData.Fields[MetaTagsConstant.RichTextTitle].Value : string.Empty;
                metaDataList.Add(metaInformation);
            }

            Item allOffersMeta = metaDatas.Children.Where(x => x.Name.Equals("All Offers")).FirstOrDefault();
            metaInformation = new MetaInformation();
            if (allOffersMeta != null)
            {
                metaInformation.tabTitle = allOffersMeta.Fields[RewardsConstant.Title].Value;
                metaInformation.data.metaTitle = allOffersMeta.Fields[MetaTagsConstant.MetaTitle] != null ? allOffersMeta.Fields[MetaTagsConstant.MetaTitle].Value : string.Empty;
                metaInformation.data.metaDescription = allOffersMeta.Fields[MetaTagsConstant.MetaDescription] != null ? allOffersMeta.Fields[MetaTagsConstant.MetaDescription].Value : string.Empty;
                metaInformation.data.keywords = allOffersMeta.Fields[MetaTagsConstant.Keywords] != null ? allOffersMeta.Fields[MetaTagsConstant.Keywords].Value : string.Empty;
                metaInformation.data.robots = allOffersMeta.Fields[MetaTagsConstant.Robots] != null ? allOffersMeta.Fields[MetaTagsConstant.Robots].Value : string.Empty;
                metaInformation.data.oG_Title = allOffersMeta.Fields[MetaTagsConstant.OGTitle] != null ? allOffersMeta.Fields[MetaTagsConstant.OGTitle].Value : string.Empty;
                metaInformation.data.oG_Image = allOffersMeta.Fields[MetaTagsConstant.OGImage] != null ? allOffersMeta.Fields[MetaTagsConstant.OGImage].Value : string.Empty;
                metaInformation.data.oG_Description = allOffersMeta.Fields[MetaTagsConstant.OGDescription] != null ? allOffersMeta.Fields[MetaTagsConstant.OGDescription].Value : string.Empty;
                metaInformation.data.viewport = allOffersMeta.Fields[MetaTagsConstant.Viewport] != null ? allOffersMeta.Fields[MetaTagsConstant.Viewport].Value : string.Empty;
                metaInformation.data.canonical = allOffersMeta.Fields[MetaTagsConstant.Canonical] != null ? _helper.GetLinkURL(allOffersMeta, MetaTagsConstant.Canonical) : string.Empty;
                metaInformation.data.richTextBody = allOffersMeta.Fields[MetaTagsConstant.RichText] != null ? allOffersMeta.Fields[MetaTagsConstant.RichText].Value : string.Empty;
                metaInformation.data.richTextTitle = allOffersMeta.Fields[MetaTagsConstant.RichTextTitle] != null ? allOffersMeta.Fields[MetaTagsConstant.RichTextTitle].Value : string.Empty;

                metaDataList.Add(metaInformation);
            }

            return metaDataList;
        }
    }
}