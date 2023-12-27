using System;
using System.Linq;
using System.Linq.Expressions;
using Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Services
{
    public static class OfferPredicateBuilder
    {
        internal static Expression<Func<SearchResultItem, bool>> GetOfferPredicate(OfferFilters filter)
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();
            if (!string.IsNullOrEmpty(filter.AirportCode))
            {
                predicate = predicate.And(x => x.Paths.Contains(getLocationID(filter.AirportCode.ToLower())));
            }
           // predicate = predicate.And(x => x.Paths.Contains(ID.Parse(offerFilterContent.OfferListFolderID)));
            predicate = predicate.And(x => x.TemplateId == ID.Parse(offerFilterContent.PromoOfferTemplateID));
            predicate = predicate.And(x => x[offerFilterContent.PromotionOfferActiveSolrField] == "true");
            if (!string.IsNullOrEmpty(filter.SearchText))
            { 
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferSolrField].Contains(filter.SearchText.ToLower()));
            }
            if(!string.IsNullOrEmpty(filter.Code))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionCodeSolrField].Contains(filter.Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.StoreType))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTerminalStoreTypeSolrField].Contains(filter.StoreType.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.appType))
            {
                var filterstring = filter.appType == offerFilterContent.ApptypeFilter ? offerFilterContent.Webtype : offerFilterContent.Apptype;
                predicate = predicate.And(x => x[filterstring] == "true");
            }
            if (!string.IsNullOrEmpty(filter.OfferUniqueID))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferUniqueIDFilterFieldSolrField].Contains(filter.OfferUniqueID.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.tab))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTabTitleSolrField].Contains(filter.tab.ToLower()));
            }
            if(filter.isBankOffer)
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferisBankOfferSolrField]==Convert.ToString(filter.isBankOffer).ToLower());
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferLOBSolrField].Contains(filter.LOB.ToLower()));

            }
            //Ticket No 17128
            if (filter.isUnlockOffer)
            {
                predicate = predicate.And(x => x[offerFilterContent.IsUnlockOffer] == Convert.ToString(filter.isUnlockOffer).ToLower());
            }
            predicate = predicate.And(x => x.Language == "en");

            return predicate;
        }

        #region Adani One Offers
        // Ticket No 17609
        /// <summary>
        /// Predicate Builder for AdaniOne Offers Only
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal static Expression<Func<SearchResultItem, bool>> GetAdaniOneOfferPredicate(OfferFilters filter)
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();

            predicate = predicate.And(x => x.Paths.Contains(ID.Parse(offerFilterContent.NewOfferListFolderID)));
            predicate = predicate.And(x => x.TemplateId == ID.Parse(offerFilterContent.PromoOfferTemplateID));
            predicate = predicate.And(x => x[offerFilterContent.PromotionOfferActiveSolrField] == "true");
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferSolrField].Contains(filter.SearchText.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.Code))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionCodeSolrField].Contains(filter.Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.StoreType))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTerminalStoreTypeSolrField].Contains(filter.StoreType.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.appType))
            {
                var filterstring = filter.appType == offerFilterContent.ApptypeFilter ? offerFilterContent.Webtype : offerFilterContent.Apptype;
                predicate = predicate.And(x => x[filterstring] == "true");
            }
            if (!string.IsNullOrEmpty(filter.OfferUniqueID))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferUniqueIDFilterFieldSolrField].Contains(filter.OfferUniqueID.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.tab))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTabTitleSolrField].Contains(filter.tab.ToLower()));
            }
            if (filter.isBankOffer)
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferisBankOfferSolrField] == Convert.ToString(filter.isBankOffer).ToLower());
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferLOBSolrField].Contains(filter.LOB.ToLower()));

            }
            //Ticket No 17128
            if (filter.isUnlockOffer)
            {
                predicate = predicate.And(x => x[offerFilterContent.IsUnlockOffer] == Convert.ToString(filter.isUnlockOffer).ToLower());
            }
            predicate = predicate.And(x => x.Language == "en");

            return predicate;
        }
        #endregion


        static ID getLocationID(string location)
        {
            ID LocationID = ID.Null;
            if (!string.IsNullOrEmpty(location))
            {
                switch (location.ToLower())
                {
                    case "svpia-ahmedabad-airport":
                        LocationID = Constant.Ahmedabad;
                        break;
                    case "lgbia-guwahati-airport":
                        LocationID = Constant.Guwahati;
                        break;
                    case "jaipur-airport":
                        LocationID = Constant.Jaipur;
                        break;
                    case "ccsia-lucknow-airport":
                        LocationID = Constant.Lucknow;
                        break;
                    case "thiruvananthapuram-airport":
                        LocationID = Constant.Thiruvananthapuram;
                        break;
                    case "mangaluru-airport":
                        LocationID = Constant.Mangaluru;
                        break;
                    case "csmia-mumbai-airport":
                        LocationID = Constant.Mumbai;
                        break;
                    

                }
            }
            return LocationID;
        }
    }
}