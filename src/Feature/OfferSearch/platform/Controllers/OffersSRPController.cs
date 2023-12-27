using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Controllers
{
    public class OffersSRPController : ApiController
    {
        // GET: Offers
        private ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper _helper;
        public OffersSRPController(ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper helper)
        {
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this._helper = helper;
        }

        [HttpPost]
        [Route("api/GetOffersSRP")]
        public IHttpActionResult GetPromotionAndOffers([FromBody] OfferFilters_old filter)
        {
            Sitecore.Data.Database contextDB = GetRewardContextDatabase();

            Item rewardsFolderID = contextDB.GetItem(offerFilterContent.RewardsFolderID.ToString());

            IEnumerable<Item> rewardListItems = null;
            rewardListItems = rewardsFolderID.Children.Where(x => x.TemplateID == offerFilterContent.PromoOfferTemplateID).ToList();

            ResponseData responseData = new ResponseData();
            ResultData resultData = new ResultData();
            if (!string.IsNullOrEmpty(filter.AirportCode))
            {
                IQueryable<SearchResultItem> results = searchBuilder.GetSearchResults(Services.OfferPredicateBuilderSRP.GetOfferPredicate(filter));
                IQueryable<SearchResultItem> airportCityItems = searchBuilder.GetSearchResults(Services.AirportPredicateBuilder.GetAirportCityPredicate());
                var searchIndex = ContentSearchManager.GetIndex(Constant.AirlineSearchIndex);
                var airlineSearchPredicate = Services.AirportPredicateBuilder.GetAirlinesPredicate();

                using (var searchContext = searchIndex.CreateSearchContext())
                {
                    var airlineItems = searchContext.GetQueryable<SearchResultItem>().Where(airlineSearchPredicate);

                    try
                    {
                        if (rewardListItems != null && results != null)
                        {
                            resultData.result = ParseReward(rewardListItems, filter);
                            resultData.result.AddRange(ParseOffer(results, filter));
                        }

                        if (filter.airlinesEnabled && airlineItems != null)
                        {
                            resultData.airlines = ParseAirlines(airlineItems);
                        }

                        if (filter.airportsEnabled && airportCityItems != null)
                        {
                            var cityItems = airportCityItems.Where(x => x.TemplateId == Constant.AirportsCityTemplateId).ToList();
                            var airportItems = airportCityItems.Where(x => x.TemplateId == Constant.AirportTemplateId).ToList();
                            var keywordItems = airportCityItems.Where(x => x.TemplateId == Constant.KeywordsTemplateID).ToList();
                            resultData.airports = ParseAirports(cityItems, airportItems, keywordItems);
                        }
                        if (resultData.result != null)
                        {
                            responseData.status = true;
                            responseData.data = resultData;
                        }
                    }
                    catch (Exception ex)
                    {
                        logRepository.Error("GetOffersSRP in OffersController gives error -> " + ex.Message);
                    }
                }

            }
            return Json(responseData);
        }

        private List<object> ParseReward(IEnumerable<Item> rewards, OfferFilters_old filter)
        {
            List<object> rewardMappingList = new List<object>();
            OfferMapping_old rewardMapping = null;
            try
            {
                if (rewards != null)
                {
                    foreach (Item reward in rewards.ToList().OrderBy(x => Convert.ToInt32(x[RewardsConstant.DisplayRank])))
                    {
                        rewardMapping = new OfferMapping_old();
                        rewardMapping.title = !string.IsNullOrEmpty(reward.Fields[RewardsConstant.Title].Value) ? reward.Fields[Constant.Title].Value : string.Empty;
                        rewardMapping.promotionCode = !string.IsNullOrEmpty(reward.Fields[RewardsConstant.PromotionCode].Value) ? reward.Fields[RewardsConstant.PromotionCode].Value : string.Empty;
                        rewardMapping.promotionDescription = !string.IsNullOrEmpty(reward.Fields[RewardsConstant.Description].Value) ? reward.Fields[RewardsConstant.Description].Value : string.Empty;
                        rewardMapping.displayRank = !string.IsNullOrEmpty(reward.Fields[RewardsConstant.DisplayRank].Value) ? reward.Fields[RewardsConstant.DisplayRank].Value : string.Empty;
                        rewardMapping.promotionType = !string.IsNullOrEmpty(reward.Fields[RewardsConstant.PromotionType].Value) ? reward.Fields[RewardsConstant.PromotionType].Value : string.Empty;

                        if(filter.isInternational)
                        {
                            rewardMapping.title = !string.IsNullOrEmpty(reward.Fields[RewardsConstant.DisplayText].Value) ? reward.Fields[RewardsConstant.DisplayText].Value : string.Empty;
                        }
                        rewardMappingList.Add(rewardMapping);
                    }
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("ParseReward in OffersController gives error -> " + ex.Message);
            }

            return rewardMappingList;
        }

        private Database GetRewardContextDatabase()
        {
            return Sitecore.Context.Database;
        }

        private List<object> ParseOffer(IQueryable<SearchResultItem> results, OfferFilters_old filter)
        {
            List<object> offerMappingsList = new List<object>();
            OfferMapping_old offerMapping = null;
            //bool Addtothelist = false;

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
                                                    getISTDateTime(offer.Fields[key: offerFilterContent.PromotionOfferEffectiveFromSolrField].ToString()).ToString() :
                                               string.Empty;
                    offerMapping.effectiveTo = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferEffectiveToSolrField)) ?
                                                   getISTDateTime(offer.Fields[key: offerFilterContent.PromotionOfferEffectiveToSolrField].ToString()).ToString() :
                                                        string.Empty;
                    //Addtothelist = (System.DateTime.Now >= Convert.ToDateTime(offerMapping.effectiveFrom) && Convert.ToDateTime(offerMapping.effectiveTo) >= System.DateTime.Now) ? true : false;
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
                    offerMapping.isInternational = (offer.Fields.ContainsKey(offerFilterContent.OfferIsInternationalSolrField)) ?
                                                    Convert.ToBoolean((offer.Fields[key: offerFilterContent.OfferIsInternationalSolrField]).ToString()) :
                                                        false;
                    var TC = (offer.Fields.ContainsKey(offerFilterContent.PromotionOfferTermsandConditionSolrField)) ?
                                                    offer.Fields[key: offerFilterContent.PromotionOfferTermsandConditionSolrField].ToString() :
                                                        string.Empty;
                    offerMapping.termCondition = getTCList(TC);
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

        private DateTime getISTDateTime(string datetimeString)
        {
            // Ticket No 20584 (ParseExact changed to Parse)
            DateTime utcdate = DateTime.Parse(datetimeString, CultureInfo.InvariantCulture);
            return TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }

        private List<Object> ParseAirports(List<SearchResultItem> airportCityItems, List<SearchResultItem> airportItems, List<SearchResultItem> keywordItems)
        {
            List<Object> airportsList = new List<Object>();
            AirportModel airportmapping = null;
            List<string> keywordsList = new List<string>();
            try
            {
                if (airportCityItems != null)
                {
                    foreach (SearchResultItem city in airportCityItems)
                    {
                        airportmapping = new AirportModel();
                        airportmapping.CountryName = (city.Fields.ContainsKey(AirportsConstant.CountryName)) ?
                                                    city.Fields[key: AirportsConstant.CountryName].ToString() :
                                                        string.Empty;
                        airportmapping.CountryCode = (city.Fields.ContainsKey(AirportsConstant.CountryCode)) ?
                                                    city.Fields[key: AirportsConstant.CountryCode].ToString() :
                                                        string.Empty;
                        airportmapping.CityCode = (city.Fields.ContainsKey(AirportsConstant.CityCode)) ?
                                                    city.Fields[key: AirportsConstant.CityCode].ToString() :
                                                        string.Empty;
                        airportmapping.CityName = (city.Fields.ContainsKey(AirportsConstant.CityName)) ?
                                                    city.Fields[key: AirportsConstant.CityName].ToString() :
                                                        string.Empty;
                        airportmapping.Priority = (city.Fields.ContainsKey(AirportsConstant.Priority)) ?
                                                    city.Fields[key: AirportsConstant.Priority].ToString() :
                                                        string.Empty;
                        airportmapping.isPopular = (city.Fields.ContainsKey(AirportsConstant.IsPopular)) ?
                                                    Convert.ToBoolean((city.Fields[key: AirportsConstant.IsPopular]).ToString()) :
                                                        false;

                        var airportItem = airportItems.FirstOrDefault(x => x.Parent == city.ItemId);

                        if (airportItem != null)
                        {
                            airportmapping.AirportName = (airportItem.Fields.ContainsKey(AirportsConstant.AirportName)) ?
                                                    airportItem.Fields[key: AirportsConstant.AirportName].ToString() :
                                                        string.Empty;
                            airportmapping.AirportCode = (airportItem.Fields.ContainsKey(AirportsConstant.AirportCode)) ?
                                                    airportItem.Fields[key: AirportsConstant.AirportCode].ToString() :
                                                        string.Empty;
                            airportmapping.IsPranaam = (airportItem.Fields.ContainsKey(AirportsConstant.IsPranaam)) ?
                                                    Convert.ToBoolean((airportItem.Fields[key: AirportsConstant.IsPranaam]).ToString()) :
                                                        false;
                        }

                        var keywordItem = keywordItems.Where(x => x.Path.Contains(city.Path)).ToList();

                        if (keywordItem != null)
                        {
                            airportmapping.Keywords = new List<string>();
                            var key = from SearchResultItem keywords in keywordItem select keywords.Fields[AirportsConstant.Keywords].ToString();
                            airportmapping.Keywords.AddRange(key);
                        }
                    airportsList.Add(airportmapping);
                    }

                }
            }
            catch (Exception ex)
            {

                logRepository.Error("ParseAirports in OffersController gives error -> " + ex.Message);
            }

            return airportsList;
        }

        private List<Object> ParseAirlines(IQueryable<SearchResultItem> airlines)
        {
            List<object> airlineList = new List<object>();
            AirlineModel airlineMapping = null;
            try
            {
                if (airlines != null)
                {
                    foreach (SearchResultItem airline in airlines)
                    {
                        airlineMapping = new AirlineModel();
                        airlineMapping.AirlineCode = (airline.Fields.ContainsKey(AirlinesConstant.AirlineCode)) ?
                                                    airline.Fields[key: AirlinesConstant.AirlineCode].ToString() :
                                                        string.Empty;
                        airlineMapping.AirlineName = (airline.Fields.ContainsKey(AirlinesConstant.AirlineName)) ?
                                                    airline.Fields[key: AirlinesConstant.AirlineName].ToString() :
                                                        string.Empty;
                        airlineMapping.AirlineID = (airline.Fields.ContainsKey(AirlinesConstant.AirlineID)) ?
                                                    airline.Fields[key: AirlinesConstant.AirlineID].ToString() :
                                                        string.Empty;
                        airlineMapping.AirlineLogo = (airline.Fields.ContainsKey(AirlinesConstant.AirlineLogo)) ?
                                                    getImageSRC(airline.Fields[key: AirlinesConstant.AirlineLogo].ToString()) :
                                                        string.Empty;
                        airlineList.Add(airlineMapping);
                    }
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("ParseAirlines in OffersController gives error -> " + ex.Message);
            }

            return airlineList;
        }
    }
}