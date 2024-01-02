using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services
{
    public interface ISearchItems
    {
        System.Linq.IQueryable<SearchResultItem> GetSolrProducts(Filters filter);
        List<SearchMaterialGroup> GetSolrMaterialgroups();

        List<SearchCategory> GetSolrCategories();

        List<SearchSubcategory> GetSolrSubcategories();

        List<SearchBrand> GetSolrBrands();

        void UpdateForBrandBoutique(Item materialGroupFolder, ref Filters filter);

        SelectedFilters GetOffersConfiguration(string offerFilter, Item offerConfigFolderItem);

        List<Result> GetDutyfreeProductsFromAPI(FilterProducts filterProducts, FilterRequest filterRequest, string language, string airport, string storeType);

        List<APIProduct> GetAPIExclusiveProducts(Filters filter);

        SearchData GetSearchProducts(Filters filters);

        Task<APIProductResponse> GetProductsFromAPI(Filters filter, List<SearchResultItem> productsSolr, bool callOldDataAPI);

        List<APIProduct> GetAPIComboProducts(Filters filter);

        AirportStore GetCollectionPoint(Filters filter);
    }
}