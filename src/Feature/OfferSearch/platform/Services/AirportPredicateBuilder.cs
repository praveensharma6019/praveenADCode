using Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Services
{
    public class AirportPredicateBuilder
    {
        internal static Expression<Func<SearchResultItem, bool>> GetAirportPredicate()
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();
            predicate = predicate.And(x => x.Paths.Contains(ID.Parse(Constant.AirportsCityFolderId)));
            predicate = predicate.And(x => x.TemplateId == ID.Parse(Constant.AirportTemplateId));
            return predicate;
        }

        internal static Expression<Func<SearchResultItem, bool>> GetAirportCityPredicate()
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();
            var templatePredicate = PredicateBuilder.True<SearchResultItem>();
            predicate = predicate.And(x => x.Paths.Contains(ID.Parse(Constant.AirportsCityFolderId)));
            templatePredicate = templatePredicate.Or(x => x.TemplateId == ID.Parse(Constant.AirportsCityTemplateId));
            templatePredicate = templatePredicate.Or(x => x.TemplateId == ID.Parse(Constant.AirportTemplateId));
            templatePredicate = templatePredicate.Or(x => x.TemplateId == ID.Parse(Constant.KeywordsTemplateID));
            predicate = predicate.And(templatePredicate);
            return predicate;
        }

        internal static Expression<Func<SearchResultItem, bool>> GetAirlinesPredicate()
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();
            predicate = predicate.And(x => x.Paths.Contains(ID.Parse(Constant.AirlinesFolderId)));
            predicate = predicate.Or(x => x.TemplateId == ID.Parse(Constant.AirlineTemplateId));
            return predicate;
        }
    }
}