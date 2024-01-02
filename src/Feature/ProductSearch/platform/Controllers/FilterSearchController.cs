using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Resources;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Controllers
{
    public class FilterSearchController : ApiController
    {
        private ILogRepository logRepository;
        private readonly IHelper helper;
        private readonly ISearchBuilder searchBuilder;
        private readonly ISearchItems searchItems;
        public FilterSearchController(ILogRepository _logRepository, IHelper _helper, ISearchBuilder _searchBuilder, ISearchItems _searchItems)
        {
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this.searchItems = _searchItems;
            this.helper = _helper;
        }
        [HttpPost]
        [Route("api/GetProductsforSearchText")]
        public IHttpActionResult GetProductsforSearchText([FromBody] Filters filter)
        {
            SearchResponse searchResponse = new SearchResponse();
            try
            {
                switch (filter.filterType.ToLower().Trim())
                {
                    case "popular":
                        var popularData = GetPopularData();
                        searchResponse.data.popularBrands = popularData.popularBrands;
                        searchResponse.data.popularSearch = popularData.popularSearch;
                        break;

                    default:
                        searchResponse.data = GetSearchProducts(filter, true);
                        break;
                }
            }
            catch (Exception ex)
            {
                logRepository.Info("GetProductsforSearchText method gives error from FilterSearchController  -> " + ex.Message.ToString());
            }

            return Json(searchResponse);
        }


        [HttpPost]
        [Route("api/GetProductsforSearchTextResult")]
        public IHttpActionResult GetProductsforSearchTextResult([FromBody] Filters filter)
        {
            SearchResultData searchResponse = new SearchResultData();

            try
            {
                APIProductSearchData aPIProductSearchData = new APIProductSearchData();
                if (!string.IsNullOrEmpty(filter.airportCode) || !string.IsNullOrEmpty(filter.storeType))
                {

                    string path = HttpContext.Current.Server.MapPath("~/app_data/dutyfree/" + filter.airportCode.ToLower() + "_" + filter.storeType.ToLower() + ".json");
                    StreamReader sr = new StreamReader(path);
                    string jsonString = sr.ReadToEnd();
                    sr.Close();
                    aPIProductSearchData = JsonConvert.DeserializeObject<APIProductSearchData>(jsonString);

                    SearchData searchData = GetSearchProducts(filter, false);
                    if (aPIProductSearchData.data.Count() > 0)
                    {
                        List<ProductSearchData> allProducts = aPIProductSearchData.data;
                        List<SearchProducts> searchProductList = searchData.result;

                        searchResponse.result = from searchProducts in searchProductList
                                                     join productSearchData in allProducts on searchProducts.skuCode equals productSearchData.skuCode
                                                     select new ProductSearchData
                                                     {
                                                         materialGroup = searchProducts.materialGroup,
                                                         materialGroupTitle = searchProducts.materialGroupTitle,
                                                         category = searchProducts.category,
                                                         categoryTitle = searchProducts.categoryTitle,
                                                         subCategory = productSearchData.subCategory,
                                                         subCategoryTitle = productSearchData.subCategoryTitle,
                                                         brand = productSearchData.brand,
                                                         brandTitle = productSearchData.brandTitle,
                                                         skuCode = searchProducts.skuCode,
                                                         skuName = productSearchData.skuName,
                                                         productName = productSearchData.productName,
                                                         productImages = productSearchData.productImages,
                                                         price = productSearchData.price,
                                                         isExclusive = productSearchData.isExclusive,
                                                         availability = productSearchData.availability,
                                                         availableQuantity = productSearchData.availableQuantity,
                                                         isCombo = productSearchData.isCombo,
                                                         promotions = productSearchData.promotions
                                                     };

                        searchResponse.filterResult = GetFilterProductSearchData(searchResponse.result);
                        var airportInfo = searchItems.GetCollectionPoint(filter);
                        searchResponse.airportInfo = airportInfo.airport + " - " + airportInfo.terminal;
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Info("GetProductsforSearchTextResult method gives error from FilterSearchController  -> " + ex.Message.ToString());
            }

            return Json(searchResponse);
        }

        [HttpPost]
        [Route("api/GetProductsforSearchTextAPP")]
        public IHttpActionResult GetProductsforSearchTextAPP([FromBody] Filters filter)
        {
            SearchResponseAPP searchResponse = new SearchResponseAPP();
            try
            {
                switch (filter.filterType.ToLower().Trim())
                {
                    case "popular":
                        var popularData = GetPopularData();
                        searchResponse.data.popularBrands = popularData.popularBrands;
                        searchResponse.data.popularSearch = popularData.popularSearch;
                        searchResponse.status = true;
                        break;

                    default:
                        searchResponse.data = GetSearchProducts(filter, true);
                        searchResponse.status = true; 
                        break;
                }
            }
            catch (Exception ex)
            {
                logRepository.Info("GetProductsforSearchTextAPP method gives error from FilterSearchController  -> " + ex.Message.ToString());
            }

            return Json(searchResponse);
        }

        [HttpPost]
        [Route("api/GetProductsforSearchTextResultAPP")]
        public IHttpActionResult GetProductsforSearchTextResultAPP([FromBody] Filters filter)
        {
            SearchResultDataAPP searchResponse = new SearchResultDataAPP();

            try
            {
                APIProductSearchData aPIProductSearchData = new APIProductSearchData();
                if (!string.IsNullOrEmpty(filter.airportCode) || !string.IsNullOrEmpty(filter.storeType))
                {

                    string path = HttpContext.Current.Server.MapPath("~/app_data/dutyfree/" + filter.airportCode.ToLower() + "_" + filter.storeType.ToLower() + ".json");
                    StreamReader sr = new StreamReader(path);
                    string jsonString = sr.ReadToEnd();
                    sr.Close();
                    aPIProductSearchData = JsonConvert.DeserializeObject<APIProductSearchData>(jsonString);

                    SearchData searchData = GetSearchProducts(filter, false);
                    if (aPIProductSearchData.data.Count() > 0)
                    {
                        List<ProductSearchData> allProducts = aPIProductSearchData.data;
                        List<SearchProducts> searchProductList = searchData.result;

                        searchResponse.data.result = from searchProducts  in searchProductList
                                                     join productSearchData in allProducts on searchProducts.skuCode equals productSearchData.skuCode
                                                     select new ProductSearchData
                                                     {
                                                         materialGroup = searchProducts.materialGroup,
                                                         materialGroupTitle = searchProducts.materialGroupTitle,
                                                         category = searchProducts.category,
                                                         categoryTitle = searchProducts.categoryTitle,
                                                         subCategory = productSearchData.subCategory,
                                                         subCategoryTitle = productSearchData.subCategoryTitle,
                                                         brand = productSearchData.brand,
                                                         brandTitle = productSearchData.brandTitle,
                                                         skuCode = searchProducts.skuCode,
                                                         skuName = productSearchData.skuName,
                                                         productName = productSearchData.productName,
                                                         productImages = productSearchData.productImages,
                                                         price = productSearchData.price,
                                                         isExclusive = productSearchData.isExclusive,
                                                         availability = productSearchData.availability,
                                                         availableQuantity = productSearchData.availableQuantity,
                                                         isCombo = productSearchData.isCombo,
                                                         promotions = productSearchData.promotions
                                                     };

                        searchResponse.data.filterResult = GetFilterProductSearchData(searchResponse.data.result);
                        searchResponse.data.count = searchResponse.data.result.Count();
                        var airportInfo = searchItems.GetCollectionPoint(filter);
                        searchResponse.data.airportInfo = airportInfo.airport + " - " + airportInfo.terminal;
                        searchResponse.status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Info("GetProductsforSearchTextResultAPP method gives error from FilterSearchController  -> " + ex.Message.ToString());
            }

            return Json(searchResponse);
        }

        private SearchData GetSearchProducts(Filters filter, bool setLimit)
        {
            SearchData searchProductsText = new SearchData();
            filter.filterType = "SearchProducts";

            int brandLimit = 15;
            int searchLimit = 50;
            if (setLimit)
            {
                Sitecore.Data.Database contextDB = Sitecore.Context.Database;
                Item searchConfigurationFolder = contextDB.GetItem(SearchConstant.SearchConfigurationFolder.ToString());
                try
                {
                    if (searchConfigurationFolder.HasChildren)
                    {
                        Item brandLimitItem = searchConfigurationFolder.Children.ToList().FirstOrDefault(a => a.Fields["Code"].Value.Trim().ToLower().Equals("brandlimit"));
                        Item searchLimitItem = searchConfigurationFolder.Children.ToList().FirstOrDefault(a => a.Fields["Code"].Value.Trim().ToLower().Equals("searchitemslimit"));

                        if (brandLimitItem != null)
                        {
                            brandLimit = brandLimitItem.Fields.Contains(SearchConstant.value) ? int.Parse(brandLimitItem.Fields["Value"].Value.Trim()) : brandLimit;
                        }

                        if (searchLimitItem != null)
                        {
                            searchLimit = searchLimitItem.Fields.Contains(SearchConstant.value) ? int.Parse(searchLimitItem.Fields["Value"].Value.Trim()) : searchLimit;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logRepository.Error("GetSearchProducts Method in product Search Controller Class gives error -> " + ex.Message);
                }
            }
            try
            {
                searchProductsText = searchItems.GetSearchProducts(filter);
                if (setLimit)
                {
                    searchProductsText.brands = searchProductsText.brands.Take(brandLimit).ToList();
                    searchProductsText.result = searchProductsText.result.Take(searchLimit).ToList();
                }
                List<SearchProducts> searchProductList = searchProductsText.result;
                List<string> allProducts = new List<string>();
                APIProductSearchData aPIProductSearchData = new APIProductSearchData();
                string path = HttpContext.Current.Server.MapPath("~/app_data/dutyfree/" + filter.airportCode.ToLower() + "_" + filter.storeType.ToLower() + ".json");
                StreamReader sr = new StreamReader(path);
                string jsonString = sr.ReadToEnd();
                sr.Close();
                aPIProductSearchData = JsonConvert.DeserializeObject<APIProductSearchData>(jsonString);
                if (aPIProductSearchData.data.Count() > 0)
                {
                    allProducts = aPIProductSearchData.data.Select(a=> a.skuCode).ToList();
                }
                List<SearchProducts> productsOnAirport = new List<SearchProducts>();

                productsOnAirport = searchProductList.Where(a => allProducts.Contains(a.skuCode)).ToList();
                
                searchProductsText.result = productsOnAirport;
                var brandList = productsOnAirport.Select(a=> a.brand);
                searchProductsText.brands = searchProductsText.brands.Where(a=> brandList.Contains(a.brandCode)).Select( ab=> new BrandSearch { brandCode = ab.brandCode, brandName = ab.brandName, materialGroup = ab.materialGroup }).ToList();

            }
            catch (Exception ex)
            {
                logRepository.Info("GetSearchProducts method gives error from FilterSearchController  -> " + ex.Message.ToString());
            }
             
            return searchProductsText;
        }

        private SearchData GetPopularData()
        {
            SearchData searchData = new SearchData();
            Sitecore.Data.Database contextDB = Sitecore.Context.Database;

            Item popularBrandItem = contextDB.GetItem(SearchConstant.PopularBrandItem);
            if (popularBrandItem != null)
            {
                Sitecore.Data.Fields.MultilistField multiselectFieldBrands = popularBrandItem.Fields.Contains(SearchConstant.PopularBrand) ? popularBrandItem.Fields[SearchConstant.PopularBrand] : null;
                if (multiselectFieldBrands != null)
                {
                    searchData.popularBrands = multiselectFieldBrands.GetItems().Select(a =>
                                               new BrandSearch
                                               {
                                                   brandName = a.Fields.Contains(SearchConstant.BrandName) ? a.Fields[SearchConstant.BrandName].Value : string.Empty,
                                                   brandCode = a.Fields.Contains(SearchConstant.BrandCode) ? a.Fields[SearchConstant.BrandCode].Value : string.Empty
                                               }).ToList();
                    searchData.popularBrands = UpdateMaterialGroupforBrands(searchData.popularBrands);
                }
            }
            Item popularSearchFolder = contextDB.GetItem(SearchConstant.PopularSearchFolder);
            if (popularSearchFolder != null)
            {
                var popularSearchItems = popularSearchFolder.GetChildren().Where(a => a.TemplateID == SearchConstant.PopularSearchTemplate);
                if (popularSearchItems.Any())
                {
                    searchData.popularSearch = popularSearchItems.Select(a => new PopularSearch
                    {
                        title = a.Fields.Contains(SearchConstant.PopularSearchTitle) ? a.Fields[SearchConstant.PopularSearchTitle].Value : string.Empty,
                        materialGroup = a.Fields.Contains(SearchConstant.PopularSearchMaterialGroup) ? contextDB.GetItem(a.Fields[SearchConstant.PopularSearchMaterialGroup].Value).Fields[Constant.MaterialGroup_Code].Value : string.Empty,
                        category = a.Fields.Contains(SearchConstant.PopularSearchCategories) ? a.Fields[SearchConstant.PopularSearchCategories].Value.Split(',').ToList() : new List<string>(),
                        subCategory = a.Fields.Contains(SearchConstant.PopularSearchSubcategories) ? a.Fields[SearchConstant.PopularSearchSubcategories].Value.Split(',').ToList() : new List<string>(),
                        brand = a.Fields.Contains(SearchConstant.PopularSearchBrands) ? a.Fields[SearchConstant.PopularSearchBrands].Value.Split(',').ToList() : new List<string>(),
                        skuCode = a.Fields.Contains(SearchConstant.PopularSearchSKUCode) ? a.Fields[SearchConstant.PopularSearchSKUCode].Value : string.Empty,
                        productSEOName = SEOProductName(a.Fields.Contains(SearchConstant.PopularSearchTitle) ? a.Fields[SearchConstant.PopularSearchTitle].Value : string.Empty),
                    }).ToList();
                }
            }

            return searchData;
        }

        private List<filterData> GetFilterProductSearchData(IEnumerable<ProductSearchData> result)
        {
            List<filterData> searchFilterList = new List<filterData>();

            try
            {
                Sitecore.Data.Database contextDB = Sitecore.Context.Database;

                Item filterSearchFolder = contextDB.GetItem(SearchConstant.SearchFiltersFolder);
                if (filterSearchFolder != null)
                {
                    var filterSearchItems = filterSearchFolder.GetChildren();
                    foreach (Item item in filterSearchItems)
                    {
                        string code = item.Fields.Contains(SearchConstant.Code) ? item.Fields[SearchConstant.Code].Value : string.Empty;
                        if (!string.IsNullOrEmpty(code))
                        {
                            filterData searchFilters = new filterData();
                            searchFilters.title = item.Fields.Contains(SearchConstant.Name) ? item.Fields[SearchConstant.Name].Value : string.Empty;

                            if (code.Equals("sort"))
                            {
                                var sortItems = item.GetChildren();
                                if (sortItems.Any())
                                {
                                    searchFilters.filters = sortItems.Select(a => new CodeTitle
                                    {
                                        title = a.Fields.Contains(SearchConstant.Name) ? a.Fields[SearchConstant.Name].Value : string.Empty,
                                        code = a.Fields.Contains(SearchConstant.Code) ? a.Fields[SearchConstant.Code].Value : string.Empty,
                                        imageSrc = a.Fields.Contains(SearchConstant.Image) ? (a.Fields[SearchConstant.Image] != null ? helper.GetImageURL(a, Constant.FilterImage) : string.Empty) : string.Empty,
                                    }).ToList();                                    
                                }
                            }

                            if (code.Equals("categories"))
                            {
                                searchFilters.filters = result.Select(i => new { i.category, i.categoryTitle, i.materialGroup }).Distinct()
                                                        .Select(a => new CodeTitle { title = a.categoryTitle, code = a.category, materialGroup = a.materialGroup }).ToList();

                            }
                            if (code.Equals("allbrands"))
                            {
                                searchFilters.filters = result.Select(i => new { i.brand, i.brandTitle, i.materialGroup }).Distinct()
                                                        .Select(a => new CodeTitle { title = a.brandTitle, code = a.brand, materialGroup = a.materialGroup }).ToList();
                            }
                            if (code.Equals("offers"))
                            {
                                List<Promotion> allOffers = new List<Promotion>();
                                var allOffersList = result.Where(p => p.promotions.Count() > 0).Select(a => a.promotions).ToList();
                                foreach (List<Promotion> offers in allOffersList)
                                {
                                    allOffers.AddRange(offers);
                                }
                                searchFilters.filters = allOffers.Select(i => new { i.displayText }).Distinct()
                                                       .Select(a => new CodeTitle { title = a.displayText, code = a.displayText }).ToList();
                            }
                            searchFilterList.Add(searchFilters);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                logRepository.Info("GetFilterProductSearchData method gives error from FilterSearchController  -> " + ex.Message.ToString());
            }

            return searchFilterList;
        }

        private List<BrandSearch> UpdateMaterialGroupforBrands(List<BrandSearch> brandSearches)
        {
            List<string> brands = brandSearches.Select(a => a.brandCode).ToList();
            List<Result> productList = new List<Result>();
            Filters filter = new Filters();
            filter.storeType = "departure";

            APIProductSearchData aPIProductSearchData = new APIProductSearchData();
            string path = HttpContext.Current.Server.MapPath("~/app_data/dutyfree/" + filter.airportCode.ToLower() + "_" + filter.storeType.ToLower() + ".json");
            StreamReader sr = new StreamReader(path);
            string jsonString = sr.ReadToEnd();
            sr.Close();
            aPIProductSearchData = JsonConvert.DeserializeObject<APIProductSearchData>(jsonString);
            List<ProductSearchData> allProducts = aPIProductSearchData.data;


            foreach (BrandSearch brand in brandSearches)
            {
                var product = allProducts.FirstOrDefault(a => a.brand.Equals(brand.brandCode));
                if (product != null)
                {
                    brand.materialGroup = product.materialGroup;
                }
            }

            return brandSearches;
        }

        private string SEOProductName(string name)
        {
            name = name.Replace("â€™", "").Replace("(", "").Replace(")", "").Replace("+", "-").Replace("&", "").Replace("%", "").Replace("/", "-").Replace(" ", "-");
            name = name.Replace("'", "").Replace("$", "").Replace(",", "").Replace(".", "-").Replace("---", "-");
            return name.ToLower();
        }
    }
}
