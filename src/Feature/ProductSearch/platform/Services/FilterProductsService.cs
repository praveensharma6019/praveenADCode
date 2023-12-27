using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Sitecore.Mvc.Presentation;
using System;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services
{
    public class FilterProductsService : IFilterProductsService
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetService;
        private readonly ISearchItems searchItems;
        private readonly IHelper _helper;

        public FilterProductsService(ILogRepository _logRepository, IWidgetService widgetService, IHelper helper, ISearchItems searchItems)
        {
            this._logRepository = _logRepository;
            this._widgetService = widgetService;
            this._helper = helper;
            this.searchItems = searchItems;
        }
        public FilterProductsWidgets GetProductFilters(Rendering rendering, string queryString, string language, string airport, string storeType, bool isAirportHome, bool restricted)
        {
            FilterProductsWidgets filterProductsWidgets = new FilterProductsWidgets();

            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                filterProductsWidgets.widget = widget != null ? _widgetService.GetWidgetItem(widget) : new WidgetItem();

                if (queryString.Equals("web"))
                {
                    filterProductsWidgets.widget.widgetItems = GetProductListforWeb(rendering, language, airport, storeType, isAirportHome, restricted);
                }
                else
                {
                    filterProductsWidgets.widget.widgetItems = GetProductFilterList(rendering, storeType);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" FilterProductsService GetProductFilters gives -> " + ex.Message);
            }

            
            return filterProductsWidgets;
        }

        private List<Object> GetProductFilterList(Rendering rendering, string storeType)
        {
            List<Object> productFilterList = new List<Object>();

            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                FilterProducts filterProducts;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    filterProducts = new FilterProducts();
                    filterProducts.title = !string.IsNullOrEmpty(item.Fields[FilterConstant.displayName].Value.ToString()) ? item.Fields[FilterConstant.displayName].Value.ToString() : "";
                    filterProducts.apiUrl = !string.IsNullOrEmpty(item.Fields[FilterConstant.apiUrl].Value.ToString()) ? item.Fields[FilterConstant.apiUrl].Value.ToString() : "";

                    filterProducts.exclusive = (item.Fields[FilterConstant.ExclusiveProducts].Value.ToString() == "1") ? true : false;
                    filterProducts.materialGroup = !string.IsNullOrEmpty(item.Fields[FilterConstant.MaterialGroup].Value.ToString()) ? _helper.SanitizeName(item.Fields[FilterConstant.MaterialGroup].Value.ToString()).ToLower() : "";
                    if (!filterProducts.exclusive)
                    {
                        filterProducts.category = !string.IsNullOrEmpty(item.Fields[FilterConstant.Category].Value.ToString()) ? item.Fields[FilterConstant.Category].Value.ToString() : "";
                        filterProducts.subCategory = !string.IsNullOrEmpty(item.Fields[FilterConstant.SubCategory].Value.ToString()) ? item.Fields[FilterConstant.SubCategory].Value.ToString() : "";
                        filterProducts.brand = !string.IsNullOrEmpty(item.Fields[FilterConstant.Brand].Value.ToString()) ? item.Fields[FilterConstant.Brand].Value.ToString() : "";
                        filterProducts.showOnHomepage = (item.Fields[FilterConstant.ShowOnHomepage].Value.ToString() == "1") ? true : false;
                        filterProducts.newArrival = (item.Fields[FilterConstant.NewArrival].Value.ToString() == "1") ? true : false;
                        filterProducts.popular = (item.Fields[FilterConstant.Popular].Value.ToString() == "1") ? true : false;
                        //updated for Feature 39051: Pushing separate SKUs (Departure & Arrival) for various components on homepage for all airports
                        if (storeType.Equals("departure"))
                        {
                            filterProducts.skuCode = !string.IsNullOrEmpty(Convert.ToString(item.Fields[FilterConstant.SKUCode])) ? item.Fields[FilterConstant.SKUCode].Value.ToString() : "";
                        }
                        else if(storeType.Equals("arrival"))
                        {
                            filterProducts.skuCode = !string.IsNullOrEmpty(Convert.ToString(item.Fields[FilterConstant.SKUCodeArrival])) ? item.Fields[FilterConstant.SKUCodeArrival].Value.ToString() : "";
                        }
                    }
                    if (!string.IsNullOrEmpty(item.Fields[FilterConstant.StoreType].Value))
                    {
                        if(item.Fields[FilterConstant.StoreType].Value.Equals(string.Empty) || item.Fields[FilterConstant.StoreType].Value.Equals("All"))
                        {
                            filterProducts.storeType = storeType;
                        }
                        else
                        {
                            filterProducts.storeType = item.Fields[FilterConstant.StoreType].Value.ToLower();
                        }
                    }                  

                    productFilterList.Add(filterProducts);
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(" FilterProductsService GetProductFilterList gives -> " + ex.Message);
            }
            return productFilterList;
        }

        private List<Object> GetProductListforWeb(Rendering rendering, string language, string airport, string storeType, bool isAirportHome, bool restricted)
        {
            List<Object> productTabs = new List<Object>();

            List<Object> productFilters = GetProductFilterList(rendering, storeType);
            List<Result> productList = new List<Result>();

            productList = GetHomeFiltersProducts(productFilters, storeType, airport, language, restricted);

            foreach (FilterProducts productFilter in productFilters)
            {
                ProductTab productTab = new ProductTab();
                productTab.category = productFilter.title;

                productTab.productDatas = productList;
                if (!string.IsNullOrEmpty(productFilter.materialGroup))
                {
                    productTab.productDatas = productTab.productDatas.FindAll(x => x.materialGroup == productFilter.materialGroup.Trim().ToLower());
                }

                if (!string.IsNullOrEmpty(productFilter.category))
                {
                    productTab.productDatas = productTab.productDatas.FindAll(x => x.category == productFilter.category.Trim().ToLower());
                }

                if (!string.IsNullOrEmpty(productFilter.subCategory))
                {
                    productTab.productDatas = productTab.productDatas.FindAll(x => x.subCategory == productFilter.subCategory.Trim().ToLower());
                }

                if (!string.IsNullOrEmpty(productFilter.brand))
                {
                    productTab.productDatas = productTab.productDatas.FindAll(x => x.brand == productFilter.brand.Trim().ToLower());
                }

                if (!string.IsNullOrEmpty(productFilter.skuCode))
                {
                    productTab.productDatas = productTab.productDatas.FindAll(x => productFilter.skuCode.Contains(x.skuCode));
                }

                productTabs.Add(productTab);
            }
            return productTabs;
        }

        private List<Result> GetHomeFiltersProducts(List<Object> productFilters, string storeType, string airport, string language, bool restricted)
        {
            List<Result> productList = new List<Result>();
            FilterRequest filterRequest = new FilterRequest();
            
            List<bool> allExclusive = new List<bool>();
            List<string> skuCodeList = new List<string>();
            List<string> categoryList = new List<string>();
            List<string> subCategoryList = new List<string>();
            List<string> brandList = new List<string>();
            string ExclusiveProductMaterialGroup = string.Empty;
            foreach (FilterProducts productFilter in productFilters)
            {
                if (productFilter.skuCode.Length > 0)
                {
                    skuCodeList.AddRange(productFilter.skuCode.Split(','));
                }
                if (productFilter.category.Length > 0)
                {
                    categoryList.Add(productFilter.category);
                }
                if (productFilter.subCategory.Length > 0)
                {
                    subCategoryList.AddRange(productFilter.subCategory.Split(','));
                }
                if (productFilter.brand.Length > 0)
                {
                    brandList.Add(productFilter.brand);
                }
                allExclusive.Add(productFilter.exclusive);
                ExclusiveProductMaterialGroup = productFilter.exclusive ? productFilter.materialGroup : string.Empty;
            }

            filterRequest.exclusive = allExclusive.TrueForAll(a => a == false) ? false : true ;
            filterRequest.airportCode = airport;
            filterRequest.storeType = storeType;
            filterRequest.language = language;
            filterRequest.restricted = restricted;

            if (filterRequest.exclusive)
            {
                filterRequest.materialGroup = ExclusiveProductMaterialGroup;
                productList = searchItems.GetDutyfreeProductsFromAPI(null, filterRequest, language, airport, storeType);
            }
            else 
            { 
                // for only SKUCode 
                if (skuCodeList.Count > 0)
                {
                    filterRequest.skuCode = skuCodeList.ToArray();
                    productList = searchItems.GetDutyfreeProductsFromAPI(null, filterRequest, language, airport, storeType);
                    filterRequest.skuCode = new List<string>().ToArray();
                }
                // for Category, SubCategory and Brand
                filterRequest.showOnHomepage = true;
                if (categoryList.Count > 0)
                {
                    filterRequest.category = categoryList.ToArray();
                    productList.AddRange(searchItems.GetDutyfreeProductsFromAPI(null, filterRequest, language, airport, storeType));
                    filterRequest.category = new List<string>().ToArray();
                }

                if (subCategoryList.Count > 0)
                {
                    filterRequest.subCategory = subCategoryList.ToArray();
                    productList.AddRange(searchItems.GetDutyfreeProductsFromAPI(null, filterRequest, language, airport, storeType));
                    filterRequest.subCategory = new List<string>().ToArray();
                }

                if (brandList.Count > 0)
                {
                    filterRequest.brand = brandList.ToArray();
                    productList.AddRange(searchItems.GetDutyfreeProductsFromAPI(null, filterRequest, language, airport, storeType));
                }
            }
            return productList;
        }
    }
}