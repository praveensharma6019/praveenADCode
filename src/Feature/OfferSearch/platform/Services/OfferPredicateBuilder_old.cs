using System;
using System.Linq;
using System.Linq.Expressions;
using Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Services
{
    public static class OfferPredicateBuilder_old
    {
        internal static Expression<Func<SearchResultItem, bool>> GetOfferPredicate(OfferFilters_old filter)
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();

            predicate = predicate.And(x => x.Paths.Contains(ID.Parse(offerFilterContent.OfferListFolderID)));
            predicate = predicate.And(x => x.TemplateId == ID.Parse(offerFilterContent.PromoOfferTemplateID));
            predicate = predicate.And(x => x[offerFilterContent.PromotionOfferActiveSolrField] == "true");
            ////Added Location based search Mandatory Ticket No 18385
            predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTerminalLocationTypeSolrField].Contains(filter.AirportCode.ToLower().Trim()));
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferSolrField].Contains(filter.SearchText.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.Code))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionCodeSolrField].Equals(filter.Code.ToLower()));
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
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferUniqueIDFilterFieldSolrField].Equals(filter.OfferUniqueID.ToLower()));
            }
            ////Added Location based search Mandatory Ticket No 18385
            //if (!string.IsNullOrEmpty(filter.AirportCode))
            //{
            //    predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTerminalLocationTypeSolrField].Contains(filter.AirportCode.ToLower()));
            //}
            if (!string.IsNullOrEmpty(filter.tab))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTabTitleSolrField].Contains(filter.tab));
            }
            if (filter.isBankOffer)
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferisBankOfferSolrField] == Convert.ToString(filter.isBankOffer).ToLower());
            }
            if(!string.IsNullOrEmpty(filter.LOB))
            {
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

        internal static Expression<Func<SearchResultItem, bool>> GetCartOfferPredicate(OfferFilters_old filter)
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();

            predicate = predicate.And(x => x.Paths.Contains(ID.Parse(offerFilterContent.OfferListFolderID)));
            predicate = predicate.And(x => x.TemplateId == ID.Parse(offerFilterContent.CartOfferTemplateID));
            predicate = predicate.And(x => x[offerFilterContent.PromotionOfferActiveSolrField] == "true");
            ////Added Location based search Mandatory Ticket No 18385
            predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTerminalLocationTypeSolrField].Contains(filter.AirportCode.ToLower().Trim()));
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferSolrField].Contains(filter.SearchText.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.Code))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionCodeSolrField].Equals(filter.Code.ToLower()));
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
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferUniqueIDFilterFieldSolrField].Equals(filter.OfferUniqueID.ToLower()));
            }
            ////Added Location based search Mandatory Ticket No 18385
            //if (!string.IsNullOrEmpty(filter.AirportCode))
            //{
            //    predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTerminalLocationTypeSolrField].Contains(filter.AirportCode.ToLower()));
            //}
            if (!string.IsNullOrEmpty(filter.tab))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTabTitleSolrField].Contains(filter.tab));
            }
            if (filter.isBankOffer)
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferisBankOfferSolrField] == Convert.ToString(filter.isBankOffer).ToLower());
            }
            if (!string.IsNullOrEmpty(filter.LOB))
            {
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

        internal static Expression<Func<SearchResultItem, bool>> GetOfferSetPredicate(OfferFilters_old filter)
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();

            predicate = predicate.And(x => x.Paths.Contains(ID.Parse(offerFilterContent.OfferListFolderID)));
            predicate = predicate.And(x => x.TemplateId == ID.Parse(offerFilterContent.PromoOfferTemplateID));
            predicate = predicate.And(x => x[offerFilterContent.PromotionOfferActiveSolrField] == "true");
            ////Added Location based search Mandatory Ticket No 18385
            //predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTerminalLocationTypeSolrField].Contains(filter.AirportCode.ToLower().Trim()));
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
            ////Added Location based search Mandatory Ticket No 18385
            //if (!string.IsNullOrEmpty(filter.AirportCode))
            //{
            //    predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTerminalLocationTypeSolrField].Contains(filter.AirportCode.ToLower()));
            //}
            if (!string.IsNullOrEmpty(filter.tab))
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferTabTitleSolrField].Contains(filter.tab));
            }
            if (filter.isBankOffer)
            {
                predicate = predicate.And(x => x[offerFilterContent.PromotionOfferisBankOfferSolrField] == Convert.ToString(filter.isBankOffer).ToLower());
            }
            if (!string.IsNullOrEmpty(filter.LOB))
            {
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
    }
}