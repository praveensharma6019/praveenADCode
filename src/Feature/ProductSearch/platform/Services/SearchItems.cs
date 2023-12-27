using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services
{
    public class SearchItems : ISearchItems
    {
        private readonly IAPIResponse productResponse;
        private readonly ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper helper;
        private readonly IAPIResponse dataAPI;

        public SearchItems(IAPIResponse _productResponse, ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper _helper)
        {
            this.productResponse = _productResponse;
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this.helper = _helper;
        }

        public IQueryable<SearchResultItem> GetSolrProducts(Filters filter)
        {
            if (!string.IsNullOrEmpty(filter.materialGroup))
            {
                filter.materialGroup = helper.SanitizeName(filter.materialGroup).ToLower();
            }
            if (filter.brand != null && filter.brand.Any())
            {
                List<string> barnds = new List<string>();
                foreach (string str in filter.brand)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        barnds.Add(helper.SanitizeName(str).ToLower());
                    }
                }
                filter.brand = barnds.ToArray();
            }

            if (filter.category != null && filter.category.Any())
            {
                List<string> category = new List<string>();
                foreach (string str in filter.category)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        category.Add(helper.SanitizeName(str).ToLower());
                    }
                }
                filter.category = category.ToArray();
            }
            if (filter.subCategory != null && filter.subCategory.Any())
            {
                List<string> subCategory = new List<string>();
                foreach (string str in filter.subCategory)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        subCategory.Add(helper.SanitizeName(str).ToLower());
                    }
                }
                filter.subCategory = subCategory.ToArray();
            }
            if (filter.skuCode != null && filter.skuCode.Any())
            {
                List<string> skuCode = new List<string>();
                foreach (string str in filter.skuCode)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        skuCode.Add(str);
                    }
                }
                filter.skuCode = skuCode.ToArray();
            }

            return searchBuilder.GetProductSearchResults(Services.Search.GetSearchPredicate(filter));
        }

        public List<SearchMaterialGroup> GetSolrMaterialgroups()
        {
            List<SearchMaterialGroup> searchResultList = new List<SearchMaterialGroup>();
            SearchMaterialGroup searchMaterialGroupItem = null;
            try
            {
                IQueryable<SearchResultItem> searchResults = searchBuilder.GetProductSearchResults(Services.Search.GetMaterialGroupPredicate());
                if (searchResults.ToList().Count > 0)
                {
                    foreach (SearchResultItem item in searchResults.ToList())
                    {
                        searchMaterialGroupItem = new SearchMaterialGroup();
                        searchMaterialGroupItem.MaterialGroupName = item.Fields.ContainsKey(Constant.Title) ? item.Fields[key: Constant.Title].ToString() : string.Empty;
                        searchMaterialGroupItem.MaterialGroupCode = item.Fields.ContainsKey(Constant.SearchMaterialGroupCode) ? item.Fields[key: Constant.SearchMaterialGroupCode].ToString().ToLower() : string.Empty;
                        searchMaterialGroupItem.MaterialGroupAPICode = item.Fields.ContainsKey(Constant.SearchAPICode) ? item.Fields[key: Constant.SearchAPICode].ToString() : string.Empty;

                        searchResultList.Add(searchMaterialGroupItem);
                    }

                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetSolrMaterialgroups Method in SearchItems service gives error  -> " + ex.Message);
            }

            return searchResultList;
        }

        public List<SearchCategory> GetSolrCategories()
        {
            List<SearchCategory> searchResultList = new List<SearchCategory>();
            SearchCategory searchCategoryItem = null;
            try
            {
                IQueryable<SearchResultItem> searchResults = searchBuilder.GetProductSearchResults(Services.Search.GetCategoryPredicate());
                if (searchResults.ToList().Count > 0)
                {
                    foreach (SearchResultItem item in searchResults.ToList())
                    {
                        searchCategoryItem = new SearchCategory();
                        searchCategoryItem.CategoryName = item.Fields.ContainsKey(Constant.Name) ? item.Fields[key: Constant.Name].ToString() : string.Empty;
                        searchCategoryItem.CategoryCode = item.Fields.ContainsKey(Constant.code) ? item.Fields[key: Constant.code].ToString().ToLower() : string.Empty;
                        searchCategoryItem.CategoryAPICode = item.Fields.ContainsKey(Constant.SearchAPICode) ? item.Fields[key: Constant.SearchAPICode].ToString().ToLower() : string.Empty;
                        if(!string.IsNullOrEmpty(searchCategoryItem.CategoryAPICode))
                        {
                            searchResultList.Add(searchCategoryItem);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetSolrCategories Method in SearchItems service gives error  -> " + ex.Message);
            }
            return searchResultList;
        }

        public List<SearchSubcategory> GetSolrSubcategories()
        {
            List<SearchSubcategory> searchResultList = new List<SearchSubcategory>();
            SearchSubcategory searchSubategoryItem = null;
            try
            {
                IQueryable<SearchResultItem> searchResults = searchBuilder.GetProductSearchResults(Services.Search.GetSubcategoryPredicate());
                if (searchResults.ToList().Count > 0)
                {
                    foreach (SearchResultItem item in searchResults.ToList())
                    {
                        searchSubategoryItem = new SearchSubcategory();
                        searchSubategoryItem.SubcategoryName = item.Fields.ContainsKey(Constant.Name) ? helper.SanitizeName(item.Fields[key: Constant.Name].ToString()) : string.Empty;
                        searchSubategoryItem.SubcategoryCode = item.Fields.ContainsKey(Constant.code) ? item.Fields[key: Constant.code].ToString().ToLower() : string.Empty;
                        searchSubategoryItem.SubcategoryAPICode = item.Fields.ContainsKey(Constant.SearchAPICode) ? item.Fields[key: Constant.SearchAPICode].ToString().ToLower() : string.Empty;
                        if(!string.IsNullOrEmpty(searchSubategoryItem.SubcategoryAPICode))
                        {
                            searchResultList.Add(searchSubategoryItem);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetSolrSubcategories Method in SearchItems service gives error  -> " + ex.Message);
            }

            return searchResultList;
        }

        public List<SearchBrand> GetSolrBrands()
        {
            List<SearchBrand> searchResultList = new List<SearchBrand>();
            SearchBrand searchBrandItem = null;
            try
            {
                IQueryable<SearchResultItem> searchResults = searchBuilder.GetProductSearchResults(Services.Search.GetBrandPredicate());
                if (searchResults.ToList().Count > 0)
                {
                    foreach (SearchResultItem item in searchResults.ToList())
                    {
                        searchBrandItem = new SearchBrand();
                        searchBrandItem.BrandName = item.Fields.ContainsKey(Constant.SearchBrandName) ? item.Fields[key: Constant.SearchBrandName].ToString() : string.Empty;
                        searchBrandItem.BrandCode = item.Fields.ContainsKey(Constant.SearchBrandCode) ? item.Fields[key: Constant.SearchBrandCode].ToString().ToLower() : string.Empty;
                        searchBrandItem.BrandName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(searchBrandItem.BrandName.ToLower());
                        searchBrandItem.BrandAPICode = item.Fields.ContainsKey(Constant.SearchAPICode) ? item.Fields[key: Constant.SearchAPICode].ToString().ToLower() : string.Empty;
                        searchBrandItem.materialGroup = item.Fields.ContainsKey(Constant.SearchBrandMaterialgroup) ? helper.SanitizeName(item.Fields[key: Constant.SearchBrandMaterialgroup].ToString().ToLower()) : string.Empty;


                        searchResultList.Add(searchBrandItem);
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetSolrBrands Method in SearchItems service gives error  -> " + ex.Message);
            }

            return searchResultList;
        }

        public void UpdateForBrandBoutique(Item materialGroupFolder, ref Filters filter)
        {
            try
            {
                bool allBrandBoutique = false;

                if (filter.materialGroup.ToLower().Trim().Equals(Constant.BrandBoutique))
                {
                    allBrandBoutique = true;
                    filter.materialGroup = "";
                }
                if (filter.category.Length > 0)
                {
                    filter.category = filter.category.Where(c => !c.Trim().ToLower().Equals(Constant.BrandBoutique)).ToArray();
                }
                if (filter.brand.Length > 0)
                {
                    allBrandBoutique = false;
                }

                if (allBrandBoutique)
                {
                    if (materialGroupFolder.HasChildren)
                    {
                        Item BrandBoutiqueMG = materialGroupFolder.Children.ToList().FirstOrDefault(m => helper.SanitizeName(m.Name.ToLower()).Equals(Constant.BrandBoutique));

                        if (BrandBoutiqueMG.HasChildren)
                        {
                            Item BrandBoutiqueCat = BrandBoutiqueMG.Children.ToList().FirstOrDefault(m => helper.SanitizeName(m.Name.ToLower()).Equals(Constant.BrandBoutique));
                            if (!String.IsNullOrEmpty(BrandBoutiqueCat.Fields[Constant.CateroryBrands].ToString()))
                            {
                                List<string> barnds = new List<string>();
                                Sitecore.Data.Fields.MultilistField multiselectField = BrandBoutiqueCat.Fields[Constant.CateroryBrands];
                                foreach (Sitecore.Data.Items.Item brand in multiselectField.GetItems())
                                {
                                    if (!string.IsNullOrEmpty(brand.Fields[Constant.Brand_Code].ToString()))
                                    {
                                        barnds.Add(brand.Fields[Constant.Brand_Code].ToString().Trim().ToLower());
                                    }
                                }
                                filter.brand = barnds.ToArray();
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                logRepository.Error("UpdateForBrandBoutique Method in SearchItems Class gives error -> " + ex.Message);
            }


        }

        public SelectedFilters GetOffersConfiguration(string offerFilter, Item offerConfigFolderItem)
        {
            SelectedFilters selectedFiltersOffer = new SelectedFilters();
            Item offerConfigItem = null;
            try
            {
                if (offerConfigFolderItem.HasChildren)
                {
                    offerConfigItem = offerConfigFolderItem.GetChildren().ToList().FirstOrDefault(o => o.TemplateID.Guid.Equals(Constant.ExclusiveOffersTemplateID)
                                      && o.Fields.Contains(Constant.offerTitle) && o.Fields[Constant.offerTitle].Value.Trim().ToLower().Equals(offerFilter));
                }

                if (offerConfigItem != null)
                {
                    selectedFiltersOffer.materialGroup = offerConfigItem.Fields.Contains(Constant.offerMaterialGroup) ? offerConfigItem.Fields[Constant.offerMaterialGroup].Value.ToString() : string.Empty;
                    selectedFiltersOffer.offers = offerConfigItem.Fields.Contains(Constant.offerCodes) ? offerConfigItem.Fields[Constant.offerCodes].Value.ToString().Split(',').Where(c => !string.IsNullOrEmpty(c)).Select(c => c.Trim()).ToArray() : new List<string>().ToArray();
                    selectedFiltersOffer.category = offerConfigItem.Fields.Contains(Constant.offerCategory) ? offerConfigItem.Fields[Constant.offerCategory].Value.ToString().Split(',').Where(c => !string.IsNullOrEmpty(c)).Select(c => c.Trim()).ToArray() : new List<string>().ToArray();
                    selectedFiltersOffer.brand = offerConfigItem.Fields.Contains(Constant.offerBrand) ? offerConfigItem.Fields[Constant.offerBrand].Value.ToString().Split(',').Where(c => !string.IsNullOrEmpty(c)).Select(c => c.Trim()).ToArray() : new List<string>().ToArray();
                    selectedFiltersOffer.skuCode = offerConfigItem.Fields.Contains(Constant.offerSKUCodes) ? offerConfigItem.Fields[Constant.offerSKUCodes].Value.ToString().Split(',').Where(c => !string.IsNullOrEmpty(c)).Select(c => c.Trim()).ToArray() : new List<string>().ToArray();
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetOffersConfiguration Method in SearchItems Class gives error -> " + ex.Message);
            }

            return selectedFiltersOffer;
        }

        public List<Result> GetDutyfreeProductsFromAPI(FilterProducts filterProducts, FilterRequest filterRequest, string language, string airport, string storeType)
        {
            List<Result> productList = new List<Result>();
            if (filterRequest == null)
            {
                filterRequest = new FilterRequest();
                filterRequest.airportCode = airport.ToUpper();
                filterRequest.language = language;
                filterRequest.showOnHomepage = (filterProducts.showOnHomepage == true) ? true : false;
                filterRequest.storeType = !string.IsNullOrEmpty(filterProducts.storeType) ? filterProducts.storeType.ToLower() : storeType;
                filterRequest.skuCode = !string.IsNullOrEmpty(filterProducts.skuCode) ? filterProducts.skuCode.Split(',') : new List<string>().ToArray();
            }

            using (var client = new HttpClient())
            {
                var model = new Root();
                client.BaseAddress = new Uri(helper.GetUrlDomain());
                var myContent = JsonConvert.SerializeObject(filterRequest);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = Task.Run(() => client.PostAsync("api/GetDutyFreeProducts", byteContent)).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (Res != null && Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var ProductResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    model = JsonConvert.DeserializeObject<Root>(ProductResponse);
                    if (model.data.result != null)
                    {
                        foreach (Result product in model.data.result)
                        {
                            productList.Add(product);
                        }
                    }
                }
            }

            return productList;
        }

        public List<APIProduct> GetAPIExclusiveProducts(Filters filter)
        {
            List<APIProduct> exclusiveProducts = new List<APIProduct>();
            APIExclusiveProducts APIResponse = null;
            try
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                Guid guID = Guid.NewGuid();
                headers.Add("traceId", guID.ToString());
                headers.Add("channelId", "Sitecore");
                Dictionary<string, string> apiParms = new Dictionary<string, string>();
                apiParms.Add("airportCode", (string.IsNullOrEmpty(filter.airportCode.Trim()) ? "BOM" : filter.airportCode.Trim()));
                apiParms.Add("storeType", (string.IsNullOrEmpty(filter.storeType.Trim()) ? "departure" : filter.storeType.Trim()));
                apiParms.Add("includeOOS", filter.includeOOS.ToString());
                var response = productResponse.GetAPIResponse("GET", Sitecore.Configuration.Settings.GetSetting("dutyfreeExclusiveProductsAPI"), headers, apiParms, "");
                APIResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<APIExclusiveProducts>(response);
                if (APIResponse.data != null && APIResponse.data.Any())
                {
                    exclusiveProducts = APIResponse.data.Select(x=> new APIProduct { id=x.id,material_group_name=x.material_group_name,category_name=x.category_name,sub_category_name=x.sub_category_name,brand_name=x.brand_name,flemingo_sku_code=x.flemingo_sku_code,sku_name=x.sku_name}).ToList();
                    Log.Info("DutyFree-Exclusive GetAPIExclusiveProducts APIResponse.data Count " + APIResponse.data.Count(), this);
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetAPIExclusiveProducts Method in ProductFilterSearchController gives error for other filters -> " + ex.Message);
            }
            return exclusiveProducts;
        }


        public List<APIProduct> GetAPIComboProducts(Filters filter)
        {
            List<APIProduct> comboProducts = new List<APIProduct>();
            APIExclusiveProducts APIResponse = null;
            try
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                Guid guID = Guid.NewGuid();
                headers.Add("traceId", guID.ToString());
                headers.Add("channelId", "Sitecore");
                Dictionary<string, string> apiParms = new Dictionary<string, string>();
                apiParms.Add("airportCode", (string.IsNullOrEmpty(filter.airportCode.Trim()) ? "BOM" : filter.airportCode.Trim()));
                apiParms.Add("storeType", (string.IsNullOrEmpty(filter.storeType.Trim()) ? "departure" : filter.storeType.Trim()));
                apiParms.Add("includeOOS", filter.includeOOS.ToString());
                var response = productResponse.GetAPIResponse("GET", Sitecore.Configuration.Settings.GetSetting("dutyfreeComboProductsAPI"), headers, apiParms, "");
                APIResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<APIExclusiveProducts>(response);
                if (APIResponse.data != null && APIResponse.data.Any())
                {
                    comboProducts = APIResponse.data;
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetAPIComboProducts Method in ProductFilterSearchController gives error for other filters -> " + ex.Message);
            }
            return comboProducts;
        }

        public SearchData GetSearchProducts(Filters filter)
        {
            //Search text
            SearchData searchData = new SearchData();
            var regex = new Regex("[!@#$%\\^&*\\(\\)-+=\\/\\{\\}\\[\\]\\|:;\"'<>,.\\?\\~`;]");
            string queryTerm = regex.Replace(filter.searchText.Trim(), string.Empty); ;
            if (queryTerm.Length > 2)
            {
                List<SearchResultItem> results = null;
                var index = ContentSearchManager.GetIndex(Constant.DutyfreeIndexname);
                using (var context = index.CreateSearchContext())
                {


                    // Perform fuzzy search
                    var queryable = context.GetQueryable<SearchResultItem>();
                    results = queryable
                        .Where(item => item.Language == "en"
                                       && item.TemplateId == ID.Parse(Constant.ProductTemplateId)
                                       && item[Constant.IsActive] == "true")
                        .Where(item => item[Constant.ProductTextSearch].Equals(queryTerm)
                                       || item[Constant.ProductTextSearch].StartsWith(queryTerm)
                                       || item[Constant.ProductTextSearch].EndsWith(queryTerm)
                                       || item[Constant.ProductTextSearch].Contains(queryTerm))
                        .Take(10000)
                        .ToList();
                }
                
                // IQueryable<SearchResultItem> results = searchBuilder.GetProductSearchResults(Services.Search.GetSearchPredicate(filter));
                List<SearchProducts> searchProducts = new List<SearchProducts>();
                searchProducts = results.Select(a => new SearchProducts
                {
                    skuCode = a.Name,
                    brand = (a.Fields.ContainsKey(Constant.BrandCode)) ? a.Fields[key: Constant.BrandCode].ToString() : "",
                    brandTitle = (a.Fields.ContainsKey(Constant.BrandTitle)) ? a.Fields[key: Constant.BrandTitle].ToString() : "",
                    materialGroup = (a.Fields.ContainsKey(Constant.MaterialGroupCode)) ? a.Fields[key: Constant.MaterialGroupCode].ToString() : "",
                    materialGroupTitle = (a.Fields.ContainsKey(Constant.MaterialGroupTitle)) ? a.Fields[key: Constant.MaterialGroupTitle].ToString() : "",
                    category = (a.Fields.ContainsKey(Constant.CategoryCode)) ? a.Fields[key: Constant.CategoryCode].ToString() : "",
                    categoryTitle = (a.Fields.ContainsKey(Constant.CategoryTitle)) ? a.Fields[key: Constant.CategoryTitle].ToString() : "",
                    subCategory = (a.Fields.ContainsKey(Constant.SubCategoryCode)) ? a.Fields[key: Constant.SubCategoryCode].ToString() : "",
                    subCategoryTitle = (a.Fields.ContainsKey(Constant.SubCategoryTitle)) ? a.Fields[key: Constant.SubCategoryTitle].ToString() : "",
                    productName = (a.Fields.ContainsKey(Constant.SKUName)) ? a.Fields[key: Constant.SKUName].ToString() : "",
                    productSEOName = (a.Fields.ContainsKey(Constant.ProductName)) ? a.Fields[key: Constant.ProductName].ToString() : ""
                }).ToList();

                if (filter.restricted)
                {
                    searchProducts = searchProducts.Where(p => !p.materialGroup.ToLower().Equals("liquor")).ToList();
                }

                //Sort Products with relevence
                List<SearchProducts> filteredProducts = new List<SearchProducts>();
                List<SearchProducts> notFilteredProducts = new List<SearchProducts>();
                //Brand
                filteredProducts = searchProducts.Where(p => p.brandTitle.ToLower().StartsWith(queryTerm.ToLower())).ToList();
                notFilteredProducts.AddRange(searchProducts.Where(p => !p.brandTitle.ToLower().StartsWith(queryTerm.ToLower())).ToList());
                //Category
                filteredProducts.AddRange(notFilteredProducts.Where(p => p.categoryTitle.ToLower().StartsWith(queryTerm.ToLower())).ToList());
                notFilteredProducts = notFilteredProducts.Where(p => !p.categoryTitle.ToLower().StartsWith(queryTerm.ToLower())).ToList();
                //Sub Category
                filteredProducts.AddRange(notFilteredProducts.Where(p => p.subCategoryTitle.ToLower().Contains(queryTerm.ToLower())).ToList());
                notFilteredProducts = notFilteredProducts.Where(p => !p.subCategoryTitle.ToLower().Contains(queryTerm.ToLower())).ToList();
                //Product Name
                filteredProducts.AddRange(notFilteredProducts);

                searchData.result = filteredProducts;
                // GetSolrBrands
                List<SearchBrand> searchBrands = GetSolrBrands();
                searchData.brands = searchBrands.Where(a => a.BrandName.ToLower().Equals(queryTerm.ToLower()) || a.BrandName.ToLower().StartsWith(queryTerm.ToLower()) || a.BrandName.ToLower().Contains(queryTerm.ToLower()))
                                    .Select(a => new BrandSearch { brandCode = a.BrandCode, brandName = a.BrandName, materialGroup = a.materialGroup }).ToList();
                if (searchData.brands.Count() == 0)
                {
                    searchData.brands = results.ToList().Select(a => new BrandSearch
                    {
                        brandCode = (a.Fields.ContainsKey(Constant.BrandCode)) ? a.Fields[key: Constant.BrandCode].ToString() : "",
                        brandName = (a.Fields.ContainsKey(Constant.BrandTitle)) ? a.Fields[key: Constant.BrandTitle].ToString() : "",
                        materialGroup = (a.Fields.ContainsKey(Constant.MaterialGroupCode)) ? a.Fields[key: Constant.MaterialGroupCode].ToString() : "",
                    }).Select(i => new { i.brandCode, i.brandName, i.materialGroup }).Distinct().Select(a => new BrandSearch { brandCode = a.brandCode, brandName = a.brandName, materialGroup = a.materialGroup }).ToList();
                }
                if (filter.restricted)
                {
                    searchData.brands = searchData.brands.Where(p => !p.materialGroup.ToLower().Equals("liquor")).ToList();
                }
            }

            return searchData;
        }

        /// <summary>
        /// Code to get the API Response
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="productsSolr"></param>
        /// <returns></returns>
        public async Task<APIProductResponse> GetProductsFromAPI(Filters filter, List<SearchResultItem> productsSolr, bool callOldDataAPI)
        {
            APIProductResponse aPIProductResponseDeserialise = null;
            if (filter.skuCode.Length == 0 && !string.IsNullOrEmpty(filter.materialGroup) && !filter.materialGroup.Trim().ToLower().Equals(Constant.TravelExclusive) && string.IsNullOrEmpty(filter.bucketGroup) && !callOldDataAPI)
            {
                //Call new Data API with Filters
                var response = await productResponse.GetAPIResponseNew("POST", GetProductURL("filters"), CreateRequestHeaders(ref filter), GetParams(ref filter), GetFilterAPIbody(filter));
                aPIProductResponseDeserialise = JsonConvert.DeserializeObject<APIProductResponse>(response);
            }
            else
            {
                try
                {
                    List<string> SKUList = new List<string>();
                    string skuCode = string.Empty;
                    foreach (var product in productsSolr)
                    {
                        if (filter.restricted)
                        {
                            skuCode = (product.Fields.ContainsKey(Constant.MaterialGroupCode) && product.Fields[key: Constant.MaterialGroupCode].ToString().Equals(Constant.restrictedGroup)) ?
                                          string.Empty : product.Name.Trim();
                        }
                        else
                        {
                            skuCode = product.Name.Trim();
                        }
                        if (!string.IsNullOrEmpty(skuCode))
                        {
                            SKUList.Add(skuCode);
                        }
                    }
                    if (!(SKUList.Count > 0))
                    {
                        logRepository.Info("GetProductsFromAPI Method in product Search Controller Class -- SKUList is 0");
                    }
                    var response = await productResponse.GetAPIResponseNew("POST", GetProductURL("sku"), CreateRequestHeaders(ref filter), GetParams(ref filter), SKUList);



                    aPIProductResponseDeserialise = JsonConvert.DeserializeObject<APIProductResponse>(response);
                    if (aPIProductResponseDeserialise.data != null)
                    {
                        aPIProductResponseDeserialise.data = aPIProductResponseDeserialise.data.OrderByDescending(p => (p.stock_in_hand - p.threshold_quantity)).ToList();
                        if (filter.offers.Length > 0)
                        {
                            string[] filterOffers = filter.offers;
                            aPIProductResponseDeserialise.data = aPIProductResponseDeserialise.data.Where(p => p.promotions.Where(o => filterOffers.Contains(o.offer_display_text)).Count() > 0).ToList();
                        }
                        //  apiResultCount = aPIProductResponseDeserialise.data.Count > 0 ? aPIProductResponseDeserialise.data.Count : 0;
                    }
                    else
                    {
                        logRepository.Info("GetProductsFromAPI Method in product Search Controller Class -- aPIProductResponse data is null");
                    }
                }
                catch (Exception ex)
                {
                    logRepository.Error("GetProductsFromAPI Method in product Search Controller Class gives error -> " + ex.Message);
                }
            }

            return aPIProductResponseDeserialise;
        }


        public AirportStore GetCollectionPoint(Filters filter)
        {
            AirportStore airportStore = new AirportStore();
            try
            {
                string airportCode = filter.airportCode.ToLower().Trim();
                string storeType = filter.storeType.ToLower().Trim();
                string storeKey = !string.IsNullOrEmpty(Sitecore.Configuration.Settings.GetSetting("DutyfreeStore")) ? Sitecore.Configuration.Settings.GetSetting("DutyfreeStore") : "store";
                Sitecore.Data.Database contextDB = Sitecore.Context.Database;
                Sitecore.Globalization.Language language = Sitecore.Globalization.Language.Parse(filter.language);
                Item collectionPointFolder = contextDB.GetItem(Constant.CollectionPointFolder.ToString(), language);
                if (collectionPointFolder.HasChildren)
                {
                    Item airport = collectionPointFolder.Children.ToList().FirstOrDefault(a => a.Fields["name"].Value.Trim().ToLower().Equals(airportCode));
                    airportStore.airport = airport.Name;
                    if (airport != null)
                    {
                        if (airport.HasChildren)
                        {
                            Item airportCollectionPoint = airport.Children.ToList().FirstOrDefault(a => a.Fields["name"].Value.Trim().ToLower().Equals(storeType));
                            if (airportCollectionPoint != null)
                            {
                                airportStore.collectionPoint = airportCollectionPoint.Fields["value"].ToString();
                            }
                            Item storeItem = airport.Children.ToList().FirstOrDefault(a => a.Fields["name"].Value.Trim().ToLower().Equals(storeKey));
                            if (storeItem != null)
                            {
                                airportStore.store = storeItem.Fields["value"].ToString();
                            }
                            Item policyItem = airport.Children.ToList().FirstOrDefault(a => a.TemplateID.Guid == Constant.CancellationPolicyTemplateID);
                            if (policyItem != null)
                            {
                                airportStore.cancellationPolicy.title = !string.IsNullOrEmpty(policyItem.Fields[Constant.PolicyTitle].Value.ToString()) ? policyItem.Fields[Constant.PolicyTitle].Value.ToString() : "";
                                airportStore.cancellationPolicy.text = !string.IsNullOrEmpty(policyItem.Fields[Constant.PolicyText].Value.ToString()) ? policyItem.Fields[Constant.PolicyText].Value.ToString() : "";

                                List<string> lines = new List<string>();
                                if (policyItem.HasChildren)
                                {
                                    foreach (Item lineItem in policyItem.Children)
                                    {
                                        if (!string.IsNullOrEmpty(lineItem.Fields["value"].ToString()))
                                        {
                                            lines.Add(lineItem.Fields["value"].ToString());
                                        }
                                    }
                                }
                                airportStore.cancellationPolicy.lines = lines;
                            }
                            airportStore.terminal = airportStore.collectionPoint;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetCollectionPoint Method in product Search Controller Class gives error -> " + ex.Message);
            }
            return airportStore;
        }




        /// <summary>
        /// Code to get the service URL 
        /// </summary>
        /// <returns></returns>
        private string GetProductURL(string apiURL)
        {
            if (!apiURL.Equals("sku"))
            {
                return Sitecore.Configuration.Settings.GetSetting("ProductAPIwithFilter");
            }
            return Sitecore.Configuration.Settings.GetSetting("ProductAPI");
        }

        /// <summary>
        /// Code to create Headers for the API
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CreateRequestHeaders(ref Filters filter)
        {
            Guid guID = Guid.NewGuid();
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");
            headers.Add("traceId", guID.ToString());
            headers.Add("agentId", filter.agentId.ToString());
            headers.Add("ChannelId", "Sitecore");
            return headers;
        }

        /// <summary>
        /// Code to pass the params to the API
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetParams(ref Filters filter)
        {
            Dictionary<string, string> parameters =
                       new Dictionary<string, string>();
            parameters.Add("storeType", filter.storeType);
            parameters.Add("airportCode", filter.airportCode);
            return parameters;

        }

        private FilterAPI GetFilterAPIbody(Filters filter)
        {
            FilterAPI filterAPI = new FilterAPI();

            List<SearchMaterialGroup> searchMaterialGroups = GetSolrMaterialgroups();
            SearchMaterialGroup materialGroupSearched = searchMaterialGroups.FirstOrDefault(a => a.MaterialGroupCode.ToLower().Equals(helper.SanitizeName(filter.materialGroup).ToLower()));
            filterAPI.materialGroup = materialGroupSearched != null ? materialGroupSearched.MaterialGroupAPICode : string.Empty;

            if (filter.category.Length > 0)
            {
                List<SearchCategory> searchCategories = GetSolrCategories();
                searchCategories = searchCategories.Where(a => filter.category.Contains(a.CategoryCode.ToLower())).ToList();
                filterAPI.category = searchCategories.Select(a => a.CategoryAPICode).ToArray();
            }
            if (filter.subCategory.Length > 0)
            {
                List<SearchSubcategory> searchSubcategories = GetSolrSubcategories();
                searchSubcategories = searchSubcategories.Where(a => filter.subCategory.Contains(a.SubcategoryCode.ToLower())).ToList();
                filterAPI.subCategory = searchSubcategories.Select(a => a.SubcategoryAPICode).ToArray();
            }
            if (filter.brand.Length > 0)
            {
                List<SearchBrand> searchBrands = GetSolrBrands();
                searchBrands = searchBrands.Where(a => filter.brand.Contains(a.BrandCode.ToLower())).ToList();
                filterAPI.brand = searchBrands.Select(a => a.BrandAPICode).ToArray();
            }
            filterAPI.offers = filter.offers;
            filterAPI.page = filter.page;
            filterAPI.pageSize = filter.pageSize;
            filterAPI.airportCode = filter.airportCode;
            filterAPI.storeType = filter.storeType;
            filterAPI.sort = filter.sort;
            filterAPI.minPrice = filter.minPrice;
            filterAPI.maxPrice = filter.maxPrice;
            filterAPI.exclusive = filter.exclusive;
            filterAPI.isCombo = filter.isCombo;
            filterAPI.includeOOS = filter.includeOOS;

            return filterAPI;
        }


    }
}