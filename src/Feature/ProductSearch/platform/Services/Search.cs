using System;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services
{
    public static class Search
    {
        
        internal static Expression<Func<SearchResultItem, bool>> GetSearchPredicate(Filters filter)
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();
            var predicateBrand = PredicateBuilder.True<SearchResultItem>();
            var predicateCategory = PredicateBuilder.True<SearchResultItem>();
            var predicateSubCategory = PredicateBuilder.True<SearchResultItem>();
            var predicateSKUCode = PredicateBuilder.True<SearchResultItem>();
            var predicateGroup = PredicateBuilder.True<SearchResultItem>();
            var predicateCategoryWithSubCategory = PredicateBuilder.True<SearchResultItem>();

            var predicateSearch = PredicateBuilder.True<SearchResultItem>();

            var predicates = PredicateBuilder.True<SearchResultItem>();

            predicate = predicate.And(x => x.Language == filter.language);
            switch (filter.filterType)
            {
                case "Product":
                    {
                        predicate = predicate.And(x => x.Paths.Contains(ID.Parse(Constant.ProductFolderID)));
                        predicate = predicate.And(x => x.TemplateId == ID.Parse(Constant.ProductTemplateId));
                        predicate = predicate.And(x => x[Constant.IsActive] == "true");

                        if (filter.travelExclusive)
                        {
                            //predicate = predicate.And(x => x[Constant.istravelExclusive] == Convert.ToString(filter.travelExclusive));
                            if(filter.storeType.ToLower()=="departure")
                            {
                                predicate = predicate.And(x => x[Constant.istravelExclusive] == Constant.TravelTypeDeparture || x[Constant.istravelExclusive] == Constant.TravelTypeBoth);
                            }
                            else
                            {
                                predicate = predicate.And(x => x[Constant.istravelExclusive] == Constant.TravelTypeArrival || x[Constant.istravelExclusive] == Constant.TravelTypeBoth);
                            }
                        }
                        else if (!string.IsNullOrEmpty(filter.materialGroup))
                        {

                            if (!string.IsNullOrEmpty(filter.bucketGroup))
                            {
                                predicate = predicate.And(x => x[Constant.MaterialGroupCode] == (filter.materialGroup).ToLower() && x[Constant.bucketGroup] == filter.bucketGroup);
                            }
                            else
                            {
                                predicate = predicate.And(x => x[Constant.MaterialGroupCode] == (filter.materialGroup).ToLower());
                            }
                        }
                        if (filter.brand != null && filter.brand.Length > 0)
                        {
                            foreach (string item in filter.brand)
                            {
                                predicateBrand = predicateBrand.Or(x => x[Constant.BrandCode] == (item).ToLower());
                            }
                        }
                        if (filter.category != null && filter.category.Length > 0)
                        {
                            foreach (string item in filter.category)
                            {
                                predicateCategory = predicateCategory.Or(x => x[Constant.CategoryCode] == (item).ToLower());
                            }
                        }
                        if (filter.subCategory != null && filter.subCategory.Length > 0)
                        {
                            foreach (string item in filter.subCategory)
                            {
                                //if (item.ToUpper() == "MEN")
                                //{
                                //    predicateSubCategory = predicateSubCategory.Or(x => x[Constant.SubCategory] == item);
                                //}
                                //else
                                //    predicateSubCategory = predicateSubCategory.Or(x => x[Constant.SubCategory].Contains(item));

                                predicateSubCategory = predicateSubCategory.Or(x => x[Constant.SubCategoryCode] == (item).ToLower());
                            }
                        }
                        if (filter.skuCode != null && filter.skuCode.Length > 0)
                        {
                            foreach (string item in filter.skuCode)
                            {
                                predicateSKUCode = predicateSKUCode.Or(x => x[Constant.SKUCode] == item);
                            }

                        }
                        if (filter.showOnHomepage)
                        {
                            predicate = predicate.And(x => x[Constant.ShowOnHomepage] == Convert.ToString(filter.showOnHomepage));
                        }
                        if (!string.IsNullOrEmpty(filter.bucketGroup))
                        {
                            predicate = predicate.And(x => x[Constant.bucketGroup] == filter.bucketGroup);                           
                        }

                        break;
                    }
                case "MaterialGroup":
                    {
                        predicate = predicate.And(x => x.Paths.Contains(ID.Parse(Constant.MaterialGroupFolder)));
                        predicate = predicate.And(x => x.TemplateId == ID.Parse(Constant.MaterialGroupTemplate));

                        break;
                    }
                case "Category":
                    {
                        predicate = predicate.And(x => x.TemplateId == ID.Parse(Constant.CategoryTemplate));

                        break;
                    }
                case "MaterialGroupFilter":
                    {
                        predicate = predicate.And(x => x.Paths.Contains(ID.Parse(filter.MaterialGroupId)));
                        predicate = predicate.And(x => x.TemplateId == (ID.Parse(Constant.CategoryTemplate)));
                        predicate = predicate.And(x => x[Constant.visibleInFiter] == "true");
                        break;
                    }
                case "AllBrands":
                    {
                        predicate = predicate.And(x => x.TemplateId == (ID.Parse(Constant.BrandTemplate)));
                        break;
                    }
                case "SearchProducts":
                    {
                        predicate = predicate.And(x => x.TemplateId == ID.Parse(Constant.ProductTemplateId));
                        predicate = predicate.And(x => x[Constant.IsActive] == "true");
                        //  predicate = predicate.And(x => x[Constant.ProductTextSearch].MatchWildcard(filter.searchText.ToLower()~));
                        predicateSearch = predicateSearch.Or(s => s[Constant.ProductTextSearch].MatchWildcard("*"+ filter.searchText.ToLower() + "*~0.5^0.5"));
                        predicateSearch = predicateSearch.Or(s => s[Constant.ProductTextSearch].MatchWildcard( filter.searchText.ToLower() + "~0.8^0.5"));
                        predicateSearch = predicateSearch.Or(s => s[Constant.ProductTextSearch].MatchWildcard(filter.searchText.ToLower() + "^1.5"));
                        
                        break;
                    }
            }

            predicateCategoryWithSubCategory = predicateCategoryWithSubCategory.Or(predicateCategory).And(predicateSubCategory);
            predicates = predicates.And(predicate).And(predicateBrand).And(predicateCategoryWithSubCategory).And(predicateSKUCode).And(predicateSearch);
            return predicates;
        }

        internal static Expression<Func<SearchResultItem, bool>> GetMaterialGroupPredicate()
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();

            predicate = predicate.And(x => x.Language == "en");
            predicate = predicate.And(x => x.TemplateId == ID.Parse(Constant.MaterialGroupTemplate));

            return predicate;
        }

        internal static Expression<Func<SearchResultItem, bool>> GetCategoryPredicate()
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();

            predicate = predicate.And(x => x.Language == "en");
            predicate = predicate.And(x => x.TemplateId == ID.Parse(Constant.CategoryTemplate));

            return predicate;
        }

        internal static Expression<Func<SearchResultItem, bool>> GetSubcategoryPredicate()
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();

            predicate = predicate.And(x => x.Language == "en");
            predicate = predicate.And(x => x.TemplateId == ID.Parse(Constant.SubCategoryTemplate));

            return predicate;
        }

        internal static Expression<Func<SearchResultItem, bool>> GetBrandPredicate()
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();

            predicate = predicate.And(x => x.Language == "en");
            predicate = predicate.And(x => x.TemplateId == ID.Parse(Constant.BrandTemplate));

            return predicate;
        }
    }
}