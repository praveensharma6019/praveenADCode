using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;


namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models
{
    public class LoyaltyPredicateBuilder
    {
        internal static Expression<Func<SearchResultItem, bool>> GetSearchPredicate(List<string> skuCode)
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();
            var predicateSKUCode = PredicateBuilder.True<SearchResultItem>();
            predicate = predicate.And(x => x.Language == "en");
            predicate = predicate.And(x => x.Paths.Contains(ID.Parse(Templates.LoyaltySKU.ProductFolderID)));
            predicate = predicate.And(x => x.TemplateId == ID.Parse(Templates.LoyaltySKU.ProductTemplateId));
            if (skuCode != null && skuCode.Count > 0)
            {
                foreach (string item in skuCode)
                {
                    predicateSKUCode = predicateSKUCode.Or(x => x[Templates.LoyaltySKU.Name] == item);
                }

            }
            return predicate= predicate.And(predicateSKUCode);
        }

        internal static Expression<Func<SearchResultItem, bool>> GetSearchPredicate(string category)
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();
            var predicateSKUCode = PredicateBuilder.True<SearchResultItem>();
            predicate = predicate.And(x => x.Language == "en");
            predicate = predicate.And(x => x.Paths.Contains(ID.Parse(Templates.LoyaltySKU.ProductFolderID)));
            predicate = predicate.And(x => x.TemplateId == ID.Parse(Templates.LoyaltySKU.ProductTemplateId));
            predicate = predicate.And(x => x[Templates.LoyaltySKU.CategoryCode] == category.ToLower());
            return predicate = predicate.And(predicateSKUCode);
        }
    }
}