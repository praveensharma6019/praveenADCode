using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Controllers
{
    public class ProductSearchController : ApiController
    {
        private readonly IAPIResponse productResponse;
        private readonly ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper helper;
        private readonly ISearchItems searchItems;
        private int apiResultCount;

        public ProductSearchController(IAPIResponse _productResponse, ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper _helper, ISearchItems _searchItems)
        {
            this.productResponse = _productResponse;
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this.helper = _helper;
            this.searchItems = _searchItems;
            this.apiResultCount = 0;
        }

        [HttpPost]
        [Route("api/GetDutyFreeProducts")]
        public async Task<IHttpActionResult> GetDutyFreeProducts([FromBody] Filters filter)
        {
            string requestBody;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Request.InputStream))
            {
                requestBody = await reader.ReadToEndAsync();
            }
            Log.Info("api GetDutyFreeProducts Start " + requestBody, this);
            ResultProductData resultData = new ResultProductData();
            ResponseProductData responseData = new ResponseProductData();
            bool virtualMatetialGroup = false;

            if (string.IsNullOrEmpty(filter.storeType))
            {
                Models.Error error = new Models.Error();
                error.description = "Please provide storeType";
                error.errorCode = "storeType is emply";
                responseData.error = error;
                responseData.status = false;
                return Json(responseData);
            }

            filter.filterType = "Product";
            if (!string.IsNullOrEmpty(filter.slug))
            {
                SetProductFilters(ref filter);
            }
            if (string.IsNullOrEmpty(filter.materialGroup.Trim()))
            {
                UpdateFilter(ref filter);
            }
            Filters exFilter = new Filters();
            APIProductResponse apiResponse = null;
            APIProductResponse APIExclusiveResponse = null;
            List<APIProducts> APIExclusiveProductList = null;
            string singleProductSKU = string.Empty;
            string soldTogetherProductSKU = string.Empty;
            List<string> buyTogetherOnCart = new List<string>();
            bool productIsExclusive = false;
            try
            {
                Sitecore.Data.Database contextDB = GetProductContextDatabase();


                if (!string.IsNullOrEmpty(filter.materialGroup))
                {
                    virtualMatetialGroup = Constant.VirtualMaterialGroup.IndexOf(helper.SanitizeName(filter.materialGroup.ToLower())) > -1 ? true : false;
                    filter.travelExclusive = filter.materialGroup.ToLower().Trim().Equals(Constant.TravelExclusive);
                    contextDB = GetProductContextDatabase();
                    Item materialGroupFolder = contextDB.GetItem(Constant.MaterialGroupFolder.ToString());
                    searchItems.UpdateForBrandBoutique(materialGroupFolder, ref filter);
                }

                if (filter.skuCode != null && filter.skuCode.Length > 0 && filter.skuCode.Length < 2)
                {
                    singleProductSKU = filter.skuCode[0];
                }

                //Set if no Filter provided
                if (string.IsNullOrEmpty(filter.materialGroup.Trim()) && filter.skuCode.Length == 0)
                {
                    filter.materialGroup = "NODATA";
                }
                IQueryable<SearchResultItem> productResults = searchItems.GetSolrProducts(filter);


                Item oldataAPIConfigItem = null;
                string oldataAPIConfigValue = string.Empty;
                try
                {
                    oldataAPIConfigItem = contextDB.GetItem("{E6099C59-6E92-49C6-9836-6F6B3FE68F1F}");
                    oldataAPIConfigValue = oldataAPIConfigItem.Fields["value"].ToString();
                }
                catch (Exception ex)
                { }

                bool callOldDataAPI = (!string.IsNullOrEmpty(oldataAPIConfigValue) && oldataAPIConfigValue.ToLower().Equals("true")) ? true : false;

                if (filter.pageType.ToLower().Trim().Equals("cartdetails"))
                {
                    resultData.result = ParseProductFromSolrResult(productResults.ToList());
                    resultData.count = resultData.result.Count();
                }
                else
                {
                    // Get Solr products and Data API products
                    #region get Solr products and Data API products

                    IQueryable<SearchResultItem> filterdProductResults = null;

                    if (virtualMatetialGroup && string.IsNullOrEmpty(singleProductSKU))
                    {
                        if (filter.materialGroup.ToLower().IndexOf("exclusive") > -1)
                        {
                            productResults = GetExcluiveProducts(filter);
                            Log.Info("DutyFree-Exclusive GetDutyFreeProducts GetAPIExclusiveProducts productResults " + productResults.Count(), this);
                        }
                        if (filter.materialGroup.ToLower().IndexOf("combo") > -1)
                        {
                            productResults = GetComboProducts(filter);
                        }
                        callOldDataAPI = true;
                    }

                    // for Similar and Sold Together Products
                    if (!string.IsNullOrEmpty(singleProductSKU) && !filter.travelExclusive && !filter.pageType.ToLower().Trim().Equals("cart"))
                    {
                        //Get Exclusive Products from Data API for 1 SKUcode
                        APIExclusiveResponse = await searchItems.GetProductsFromAPI(filter, productResults.ToList(), true);

                        if (APIExclusiveResponse != null && APIExclusiveResponse.data != null && APIExclusiveResponse.data.Any(a => a.isExclusive))
                        {
                            Log.Info("DutyFree-Exclusive GetDutyFreeProducts Get Exclusive Products from Data API for 1 SKUcode " + filter.airportCode, this);
                            productIsExclusive = APIExclusiveResponse.data.Any(a => a.isExclusive);
                            productResults = GetExcluiveProducts(filter);
                            APIExclusiveResponse = await searchItems.GetProductsFromAPI(filter, productResults.ToList(), true);
                            APIExclusiveProductList = APIExclusiveResponse.data;
                        }
                        // for Non Exclusive Product
                        if (!productIsExclusive)
                        {
                            var product = productResults.ToList().FirstOrDefault(x => x.Name.Trim().Equals(singleProductSKU.Trim()));
                            filter.bucketGroup = product.Fields.ContainsKey(Constant.bucketGroup) ? product.Fields[Constant.bucketGroup].ToString() : string.Empty;
                            filter.materialGroup = product.Fields[Constant.MaterialGroupCode].ToString();
                            filter.skuCode = Array.Empty<string>();
                            soldTogetherProductSKU = product.Fields.ContainsKey(Constant.soldTogether) ? product.Fields[Constant.soldTogether].ToString() : string.Empty;
                            filterdProductResults = searchItems.GetSolrProducts(filter);
                            List<SearchResultItem> filterProducts = filterdProductResults.ToList();
                            filterProducts = filterProducts.Where(a => a.Fields.ContainsKey(Constant.isrecomended) && a.Fields[Constant.isrecomended].ToString().Equals("true")).ToList();
                            if (filterProducts.FirstOrDefault(a => a.Name.Trim().Equals(singleProductSKU)) == null)
                            {
                                filterProducts.Add(product);
                            }
                            apiResponse = await searchItems.GetProductsFromAPI(filter, filterProducts, callOldDataAPI);

                            if (apiResponse.data != null && apiResponse.data.Count() > 0)
                            {
                                if (!apiResponse.data.Any(a => a.flemingo_sku_code == product.Name))
                                {
                                    List<string> serchedProduct = new List<string>();
                                    serchedProduct.Add(product.Name);
                                    Filters searchedFilter = new Filters();
                                    searchedFilter.airportCode = filter.airportCode;
                                    searchedFilter.storeType = filter.storeType;
                                    searchedFilter.language = filter.language;
                                    searchedFilter.skuCode = serchedProduct.ToArray();
                                    APIProductResponse serchedProductAPIresponse = await searchItems.GetProductsFromAPI(searchedFilter, productResults.ToList(), callOldDataAPI);
                                    if (serchedProductAPIresponse.data != null && serchedProductAPIresponse.data.Count() > 0)
                                    {
                                        apiResponse.data.AddRange(serchedProductAPIresponse.data);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // for you may also like products for Cart
                        if (filter.pageType.ToLower().Trim().Equals("cart"))
                        {
                            buyTogetherOnCart = productResults.ToList().Select(p => p.Fields[Constant.soldTogether].ToString()).ToList();
                            productResults = GetBuyTogetherProductsForCart(buyTogetherOnCart, ref filter);
                        }
                        apiResponse = await searchItems.GetProductsFromAPI(filter, productResults.ToList(), callOldDataAPI);
                    }

                    #endregion

                    //Map Solr products with Data API products
                    #region Map Solr products with Data API products

                    // For Non Exclusive Products mapping
                    if (apiResponse != null && apiResponse.data != null)
                    {
                        List<APIProducts> apiProductList = apiResponse.data;
                        if (apiProductList.Any(p => p.sellable == false && p.flemingo_sku_code.Equals(singleProductSKU)))
                        {
                            // for single Non Selleble Product
                            resultData.result = ParseProductFromSolrResult(productResults.ToList());
                            resultData.count = resultData.result.Count();
                        }
                        else
                        {
                            // for Non Selleble Products
                            apiProductList = apiProductList.Where(p => p.sellable == true).ToList();
                            apiResultCount = apiProductList.Count();
                            resultData.count = apiProductList.Count();
                            if (!string.IsNullOrEmpty(singleProductSKU))
                            {
                                resultData.result = ParseProductsWithAPIresponse(filterdProductResults.ToList(), apiProductList, ref filter);
                                resultData.similar = resultData.result.Where(x => x.skuCode != singleProductSKU && x.recomended == true && x.availability).ToList();
                                int maxSimilarProductsAllowed = GetSimilarProductsConfig();
                                resultData.similar = resultData.similar.Take(maxSimilarProductsAllowed).ToList();
                                resultData.result = resultData.result.Where(x => x.skuCode == singleProductSKU).ToList();
                                resultData.count = resultData.result.Count();
                                if(resultData.count == 0)
                                {
                                    List<SearchResultItem> productWithNoPrice = new List<SearchResultItem>();
                                    productWithNoPrice.Add(productResults.FirstOrDefault(p => p.Name.Equals(singleProductSKU)));
                                    resultData.result = ParseProductFromSolrResult(productWithNoPrice);
                                    resultData.count = resultData.result.Count();
                                }
                                if (!string.IsNullOrEmpty(soldTogetherProductSKU))
                                {
                                    resultData.soldTogether = await GetSoldTogetherProduct(soldTogetherProductSKU, filter, callOldDataAPI);
                                }
                            }
                            else
                            {
                                if (filter.pageType == "cart")
                                {
                                    resultData.count = apiProductList.Count();
                                    resultData.result = ParseProductsWithAPIresponse(productResults.ToList(), apiProductList, ref filter).Where(p => p.availability).OrderBy(p => p.price).ToList();
                                }
                                else
                                {
                                    resultData.count = apiProductList.Count();
                                    if (apiResponse.status && !string.IsNullOrEmpty(apiResponse.message) && filter.skuCode.Count() == 0 && !virtualMatetialGroup && !callOldDataAPI)
                                    {
                                        resultData.count = Convert.ToInt32(apiResponse.message);
                                    }

                                    if (virtualMatetialGroup)
                                    {
                                        SortAPIResult(ref apiProductList, ref filter);
                                        apiProductList = PaginationAPIResult(apiProductList, ref filter);

                                    }
                                    resultData.result = ParseProductsWithAPIresponse(productResults.ToList(), apiProductList, ref filter);
                                }
                            }
                        }
                    }
                    else
                    {
                        //For one Exclusive Products mapping and selection
                        resultData.result = ParseProductsWithAPIresponse(productResults.ToList(), APIExclusiveProductList, ref exFilter);
                        resultData.exclusive = resultData.result.Where(a => a.skuCode != singleProductSKU).ToList();
                        resultData.result = resultData.result.Where(a => a.skuCode == singleProductSKU).ToList();
                        resultData.count = resultData.result.Count();
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("ParseProduct Method GetDutyFreeProducts in ProductSearchController gives error -> " + ex.Message);
            }

            if (resultData.result != null)
            {
                AirportStore airportStore = searchItems.GetCollectionPoint(filter);
                // resultData.policy = GetPolicy(filter.language);
                resultData.policy = airportStore.cancellationPolicy;
                resultData.collectionPoint = airportStore.collectionPoint;
                resultData.store = airportStore.store;
                responseData.status = true;
                responseData.data = resultData;
            }

            return Json(responseData);
        }

        private async Task<List<ProductMapping>> GetSoldTogetherProduct(string soldTogetherProductSKU, Filters filter, bool callOldDataAPI)
        {
            APIProductResponse apisoldTogatherResponse = null;
            List<ProductMapping> soldtogetherProducts = new List<ProductMapping>();
            try
            {
                filter.bucketGroup = string.Empty;
                filter.materialGroup = string.Empty;
                filter.category = Array.Empty<string>();
                filter.subCategory = Array.Empty<string>();
                filter.brand = Array.Empty<string>();
                filter.skuCode = soldTogetherProductSKU.Split(',').Where(p => !string.IsNullOrEmpty(p.Trim())).ToArray();
                if (filter.skuCode.Length > 0)
                {
                    IQueryable<SearchResultItem> soldTogetherSolr = searchItems.GetSolrProducts(filter);
                    apisoldTogatherResponse = await searchItems.GetProductsFromAPI(filter, soldTogetherSolr.ToList(), callOldDataAPI);
                    if (apisoldTogatherResponse != null && apisoldTogatherResponse.data != null && apisoldTogatherResponse.data.Count > 0)
                    {
                        soldtogetherProducts = ParseProductsWithAPIresponse(soldTogetherSolr.ToList(), apisoldTogatherResponse.data, ref filter);
                    }
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("getSoldTogatherProduct Method GetDutyFreeProducts in ProductSearchController gives error -> " + ex.Message);
            }
            return soldtogetherProducts.Where(a => a.availability == true).ToList();
        }

        /// <summary>
        /// Method used to parse the product 
        /// </summary>
        /// <param name="results"></param>
        /// <param name="aPIProductResponse"></param>
        /// <returns>List<ProductMapping></returns>

        internal List<ProductMapping> ParseProductsWithAPIresponse(List<SearchResultItem> results, List<APIProducts> aPIProductResponse, ref Filters filters)
        {
            Log.Info("ParseProductsWithAPIresponse " + filters.airportCode, this);
            List<ProductMapping> productMappingsList = new List<ProductMapping>();
            ProductMapping productMapping = null;
            string productSKUforError = string.Empty;
            string exclusiveBannerPath = "/-/media/Foundation/Adani/Dutyfree/ExclusiveProductsBanners/{SKU}/{SKU}.jpg";
            var dictionaryItems = GetDictionaryDetails(filters.language);
            try
            {
                if (aPIProductResponse != null)
                {
                    foreach (APIProducts apiProduct in aPIProductResponse)
                    {
                        SearchResultItem product = results.FirstOrDefault(x => x.Name == apiProduct.flemingo_sku_code);
                        if (product != null)
                        {
                            productSKUforError = product.Name;
                            if (apiProduct.status)
                            {
                                List<OtherDetails> specifications = new List<OtherDetails>();
                                List<OtherDetails> otherDetails = new List<OtherDetails>();
                                productMapping = new ProductMapping
                                {
                                    brand = (product.Fields.ContainsKey(Constant.BrandCode)) ? product.Fields[key: Constant.BrandCode].ToString() : "",
                                    subCategory = (product.Fields.ContainsKey(Constant.SubCategoryCode)) ? product.Fields[key: Constant.SubCategoryCode].ToString() : "",
                                    category = (product.Fields.ContainsKey(Constant.CategoryCode)) ? product.Fields[key: Constant.CategoryCode].ToString() : "",
                                    materialGroup = (product.Fields.ContainsKey(Constant.MaterialGroupCode)) ? product.Fields[key: Constant.MaterialGroupCode].ToString() : "",
                                    brandTitle = (product.Fields.ContainsKey(Constant.BrandTitle)) ? product.Fields[key: Constant.BrandTitle].ToString() : "",
                                    subCategoryTitle = (product.Fields.ContainsKey(Constant.SubCategoryTitle)) ? product.Fields[key: Constant.SubCategoryTitle].ToString() : "",
                                    categoryTitle = (product.Fields.ContainsKey(Constant.CategoryTitle)) ? product.Fields[key: Constant.CategoryTitle].ToString() : "",
                                    materialGroupTitle = (product.Fields.ContainsKey(Constant.MaterialGroupTitle)) ? product.Fields[key: Constant.MaterialGroupTitle].ToString() : "",
                                    skuCode = product.Name,
                                    skuName = (product.Fields.ContainsKey(Constant.SKUName)) ? product.Fields[key: Constant.SKUName].ToString() : "",
                                    skuDescription = (product.Fields.ContainsKey(Constant.SKUDescription)) ? product.Fields[key: Constant.SKUDescription].ToString() : "",
                                    productName = (product.Fields.ContainsKey(Constant.ProductName)) ? product.Fields[key: Constant.ProductName].ToString() : product.Fields[key: Constant.SKUName].ToString().Trim().Replace(" ", "-").ToLower(),
                                    skuSize = apiProduct.sku_size.ToString(),
                                    skuUnit = apiProduct.sku_unit,
                                    price = apiProduct.price,
                                    buketGroup = (product.Fields.ContainsKey(Constant.bucketGroup)) ? product.Fields[key: Constant.bucketGroup].ToString() : "",
                                    recomended = apiProduct.isRecommended,
                                    travelExclusive = product.Fields.ContainsKey(Constant.istravelExclusive) ? ((product.Fields[key: Constant.istravelExclusive].ToString().Equals("0")) ? false : true) : false,
                                    cancellationAndRefundPolicy = (product.Fields.ContainsKey(Constant.Policy)) ? product.Fields[key: Constant.Policy].ToString() : "",
                                    storeCode = apiProduct.store_code,
                                    isExclusive = apiProduct.isExclusive,
                                    isCombo = apiProduct.isCombo,
                                    // Assigned loyalty points to the json response
                                    loyaltyPoints = Convert.ToInt32(apiProduct.loyaltyPoints),
                                    earn2XString = dictionaryItems.FirstOrDefault(x => x.Key == "Earn").Value + " " + apiProduct.loyaltyType,
                                    specifications = specifications,
                                    storeType = (product.Fields.ContainsKey(Constant.StoreType) && !string.IsNullOrEmpty(product.Fields[Constant.StoreType].ToString())) ?
                                                product.Fields[Constant.StoreType].ToString() :
                                                 string.Empty
                                };

                                // productMapping.productImages = GetProductImages(product);
                                productMapping.productImages = (product.Fields.ContainsKey(Constant.ProductImages)) ? product.Fields[key: Constant.ProductImages].ToString().Replace("/sitecore/shell/", "").Split(',').ToList() : new List<string>();
                                productMapping.promotions = GetAPIOfferList(apiProduct);
                                productMapping.discountPrice = GetDiscountPrice(productMapping.promotions);

                                productMapping.availability = (apiProduct.stock_in_hand - apiProduct.threshold_quantity) > 0 ? true : false;
                                productMapping.availableQuantity = apiProduct.stock_in_hand - apiProduct.threshold_quantity;

                                productMapping.aboutBrand = (product.Fields.ContainsKey(Constant.AboutBrand)) ? product.Fields[key: Constant.AboutBrand].ToString() : string.Empty;
                                productMapping.productHighlights = (product.Fields.ContainsKey(Constant.ProductHighlights)) ? GetContentList(product.Fields[key: Constant.ProductHighlights].ToString()) : new List<string>();
                                productMapping.keyIngredients = (product.Fields.ContainsKey(Constant.KeyIngredients)) ? GetContentList(product.Fields[key: Constant.KeyIngredients].ToString()) : new List<string>();
                                productMapping.benefits = (product.Fields.ContainsKey(Constant.ProductBenefits)) ? GetContentList(product.Fields[key: Constant.ProductBenefits].ToString()) : new List<string>();
                                productMapping.safety = (product.Fields.ContainsKey(Constant.ProductSafety)) ? GetContentList(product.Fields[key: Constant.ProductSafety].ToString()) : new List<string>();

                                productMapping.howToUse = (product.Fields.ContainsKey(Constant.HowToUse)) ? product.Fields[key: Constant.HowToUse].ToString() : string.Empty;
                                productMapping.specifications.Add(new OtherDetails { value = productMapping.brandTitle, key = dictionaryItems.FirstOrDefault(x => x.Key == "Brand").Value });
                                productMapping.specifications.Add(new OtherDetails { value = productMapping.categoryTitle, key = dictionaryItems.FirstOrDefault(x => x.Key == "Category").Value });
                                productMapping.specifications.Add(new OtherDetails { value = productMapping.subCategoryTitle, key = dictionaryItems.FirstOrDefault(x => x.Key == "SubCategory").Value });

                                if (apiProduct.age_of_product_for_liquor > 0)
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = apiProduct.age_of_product_for_liquor.ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "Productageofliquor").Value });
                                }
                                if (product.Fields.ContainsKey(Constant.CountryOfOrigin))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.CountryOfOrigin].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "CountryOfOrigin").Value });
                                }
                                if (product.Fields.ContainsKey(Constant.ProductDescription))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.ProductDescription].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "Productdescription").Value });
                                }

                                if (apiProduct.sku_size > 0 && !string.IsNullOrEmpty(apiProduct.sku_unit) && !apiProduct.sku_unit.ToLower().Trim().Equals("null"))
                                {
                                    switch (apiProduct.sku_unit.Trim().ToLower())
                                    {
                                        case "g":
                                        case "gm":
                                        case "mg":
                                        case "lb":
                                        case "oz":
                                            productMapping.specifications.Add(new OtherDetails { value = apiProduct.sku_size.ToString().Replace(".00", "") + " " + apiProduct.sku_unit.ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "ProductWeight").Value });
                                            break;
                                        case "ml":
                                        case "cl":
                                            productMapping.specifications.Add(new OtherDetails { value = apiProduct.sku_size.ToString().Replace(".00", "") + " " + apiProduct.sku_unit.ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "ProductVolume").Value });
                                            break;
                                        case "capsules":
                                        case "softgel capsules":
                                            productMapping.specifications.Add(new OtherDetails { value = apiProduct.sku_size.ToString().Replace(".00", "") + " " + apiProduct.sku_unit.ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "ProductQuantity").Value });
                                            break;
                                        case "mm":
                                            productMapping.specifications.Add(new OtherDetails { value = apiProduct.sku_size.ToString().Replace(".00", "") + " " + apiProduct.sku_unit.ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "ProductLength").Value });
                                            break;
                                    }

                                }

                                if (!string.IsNullOrEmpty(apiProduct.gender) && !apiProduct.gender.ToString().Equals("0") && !apiProduct.gender.ToString().Equals("1"))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = apiProduct.gender.ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "ProductGender").Value });
                                }
                                if (product.Fields.ContainsKey(Constant.ProductBarcodeNumber))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.ProductBarcodeNumber].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "ProductBarcodeNumber").Value });
                                }
                                if (product.Fields.ContainsKey(Constant.ManufacturerDetails))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.ManufacturerDetails].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "ManufacturerDetails").Value });
                                }

                                if (product.Fields.ContainsKey(Constant.ProductFlavour))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.ProductFlavour].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "ProductFlavour").Value });
                                }

                                if (product.Fields.ContainsKey(Constant.ProductForm))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.ProductForm].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "ProductForm").Value });
                                }

                                if (product.Fields.ContainsKey(Constant.FrameColorFront))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.FrameColorFront].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "FrameColorFront").Value });
                                }

                                if (product.Fields.ContainsKey(Constant.FrameColorTemple))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.FrameColorTemple].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "FrameColorTemple").Value });
                                }

                                if (product.Fields.ContainsKey(Constant.LensColor))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.LensColor].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "LensColor").Value });
                                }

                                if (product.Fields.ContainsKey(Constant.Material))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.Material].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "Material").Value });
                                }

                                if (product.Fields.ContainsKey(Constant.MaterialFittingName))
                                {
                                    productMapping.specifications.Add(new OtherDetails { value = product.Fields[key: Constant.MaterialFittingName].ToString(), key = dictionaryItems.FirstOrDefault(x => x.Key == "MaterialFittingName").Value });
                                }

                                if (apiProduct.isExclusive)
                                {
                                    productMapping.exclusiveImage = exclusiveBannerPath.Replace("{SKU}", apiProduct.flemingo_sku_code);
                                }

                                productMappingsList.Add(productMapping);
                            }
                        }
                        else
                        {
                            logRepository.Info("Product Not uploaded in Sitecore  --- SKU -> " + apiProduct.flemingo_sku_code);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Info("ParseProductsWithAPIresponse Method Product SolrSearch in ProductSearchController Error in SKU -> " + productSKUforError);
                logRepository.Error("ParseProductsWithAPIresponse Method Product SolrSearch in ProductSearchController gives error -> " + ex.Message);
            }
            if (filters.showOnHomepage)
            {
                string storeType = filters.storeType.ToLower();
                productMappingsList = productMappingsList.Where(p => p.storeType.ToLower() == storeType || p.storeType.ToLower().Equals(Constant.StoreTypeAll)).ToList();
            }

            return productMappingsList;

        }

        /// <summary>
        /// Cod to get the context database
        /// </summary>
        /// <returns></returns>
        internal Sitecore.Data.Database GetProductContextDatabase()
        {
            return Sitecore.Context.Database;
        }

        private List<Promotion> GetAPIOfferList(APIProducts apiProduct)
        {
            List<Promotion> offerList = new List<Promotion>();
            try
            {
                foreach (var apiPromotion in apiProduct.promotions)
                {
                    Promotion objPromotions = new Promotion();
                    objPromotions.code = apiPromotion.promotion_code;
                    objPromotions.offer = apiPromotion.offer.ToString();
                    objPromotions.quantity = apiPromotion.buy_quantity;
                    objPromotions.displayText = apiPromotion.offer_display_text;
                    objPromotions.type = apiPromotion.offer_type;
                    objPromotions.offerPrice = apiPromotion.offer_price;
                    objPromotions.discountPrice = apiPromotion.discount_price;
                    if (apiPromotion.FreebeeOffer != null)
                    {
                        objPromotions.offerSKUCode = apiPromotion.FreebeeOffer.offerSKUCode;
                        objPromotions.offerProductName = apiPromotion.FreebeeOffer.offerProductName;
                        objPromotions.offerProductSEOName = helper.SanitizeName(apiPromotion.FreebeeOffer.offerProductName.ToLower().Replace(" ", "-"));
                        objPromotions.sellable = apiPromotion.FreebeeOffer.isSellable;
                        objPromotions.offerProductPrice = apiPromotion.FreebeeOffer.price;
                        if (!string.IsNullOrEmpty(objPromotions.offerSKUCode))
                        {
                            objPromotions.offerProductImage = GetOfferProductImage(objPromotions.offerSKUCode);
                        }
                    }
                    offerList.Add(objPromotions);
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("GetAPIOfferList Method in product Search Controller Class gives error -> " + ex.Message);
            }

            return offerList;
        }


        private string GetImagePublishedPath(SearchResultItem product, string SitecoteImagePath)
        {
            string imagePath = string.Empty;
            try
            {
                imagePath = helper.GetImageUrlfromSitecore(SitecoteImagePath);
            }
            catch (Exception ex)
            {
                logRepository.Info("GetImagePublishedPath Method in product Search Controller Class gives error for SKUCode -> " + product.Name.ToString());
                logRepository.Error("GetImagePublishedPath Method in product Search Controller Class gives error -> " + ex.Message);
            }
            return imagePath;
        }

      

        private int GetSimilarProductsConfig()
        {
            int maxSimilarProducts = 5;
            try
            {
                Sitecore.Data.Database contextDB = GetProductContextDatabase();
                Item SimilarProductsConfigItem = contextDB.GetItem(Constant.SimilarProductsConfig.ToString());
                if (SimilarProductsConfigItem != null)
                {
                    maxSimilarProducts = SimilarProductsConfigItem.Fields[Constant.maxSimilarProductsField].Value.Equals(Constant.maxSimilarProducts)
                        ? Convert.ToInt32(SimilarProductsConfigItem.Fields[Constant.maxSimilarProductsFieldValue].Value) : 5;
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("SimilarProductsConfig Method in product Search Controller Class gives error -> " + ex.Message);
            }
            return maxSimilarProducts;
        }

        private int GetIntrestedProductsConfig()
        {
            int maxProducts = 2;
            try
            {
                Sitecore.Data.Database contextDB = GetProductContextDatabase();
                Item IntrestedProductsConfigItem = contextDB.GetItem(Constant.IntrestedProductsConfig.ToString());
                if (IntrestedProductsConfigItem != null)
                {
                    maxProducts = IntrestedProductsConfigItem.Fields[Constant.IntrestedProductsProductsField].Value.Equals(Constant.IntrestedProducts)
                        ? Convert.ToInt32(IntrestedProductsConfigItem.Fields[Constant.IntrestedProductsFieldValue].Value) : 2;
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("SimilarProductsConfig Method in product Search Controller Class gives error -> " + ex.Message);
            }
            return maxProducts;
        }

        internal void SortAPIResult(ref List<APIProducts> apiProductList, ref Filters filter)
        {
            if (filter.sort != null)
            {
                switch (filter.sort.ToLower())
                {
                    case "discount":
                        apiProductList = (from a in apiProductList orderby a.promotions.Count() descending select a).ToList();
                        break;
                    case "pricehl":
                        apiProductList = (from a in apiProductList orderby a.price descending select a).ToList();
                        break;
                    case "pricelh":
                        apiProductList = (from a in apiProductList orderby a.price select a).ToList();
                        break;
                    case "popularity":
                        // no records if there is no data BUG 12382
                        apiProductList = (from a in apiProductList where a.material_group_name == "Popularity" orderby a.price select a).ToList();
                        break;
                    case "feature":
                        // no records if there is no data BUG 12382
                        apiProductList = (from a in apiProductList where a.material_group_name == "feature" orderby a.price select a).ToList();
                        break;
                    default:
                        apiProductList = (from a in apiProductList orderby a.promotions.Count() descending select a).ToList();
                        break;
                }
            }

            if (filter.minPrice >= 0 && filter.maxPrice >= 0)
            {
                int minP = filter.minPrice;
                int maxP = filter.maxPrice;
                apiProductList = (from a in apiProductList where a.price > minP && a.price < maxP select a).ToList();
            }

            // if include Out of Stock products are included then in-stock products will come first and out-of-stock products will come last 
            if (filter.includeOOS)
            {
                apiProductList = apiProductList.OrderBy(a => !(a.stock_in_hand - a.threshold_quantity > 0) && a.promotions.Count() > 0).ToList();
            }
        }

        private List<ProductMapping> PaginationProductResult(List<ProductMapping> productList, ref Filters filter)
        {
            int lastPageSize = productList.Count % filter.pageSize;
            try
            {
                if (filter.page > 0 && filter.pageSize > 1)
                {
                    if (((productList.Count - lastPageSize) - (filter.page * filter.pageSize)) >= 0)
                    {
                        productList = productList.GetRange(((filter.page * filter.pageSize) - filter.pageSize), filter.pageSize);
                    }
                    else
                    {
                        productList = productList.GetRange(((filter.page * filter.pageSize) - filter.pageSize), lastPageSize);
                    }
                }
            }
            catch (Exception ex)
            {
                productList = productList.GetRange(productList.Count, 0);
            }

            return productList;
        }

        private List<APIProducts> PaginationAPIResult(List<APIProducts> productAPIList, ref Filters filter)
        {
            List<APIProducts> productList = productAPIList.ToList();

            int lastPageSize = productList.Count % filter.pageSize;
            try
            {
                if (filter.page > 0 && filter.pageSize > 1)
                {
                    if (((productList.Count - lastPageSize) - (filter.page * filter.pageSize)) >= 0)
                    {
                        productList = productList.GetRange(((filter.page * filter.pageSize) - filter.pageSize), filter.pageSize);
                    }
                    else
                    {
                        productList = productList.GetRange(((filter.page * filter.pageSize) - filter.pageSize), lastPageSize);
                    }
                }
            }
            catch (Exception ex)
            {
                productList = productList.GetRange(productList.Count, 0);
            }

            return (List<APIProducts>)productList;
        }

        private decimal GetDiscountPrice(List<Promotion> promotions)
        {
            decimal discountPrice = 0;

            if (promotions.Count > 0)
            {
                if (promotions[0].quantity == 1 && promotions[0].type == 1)
                {
                    discountPrice = promotions[0].discountPrice;
                }
            }

            return discountPrice;
        }

        private List<ProductMapping> ParseProductFromSolrResult(List<SearchResultItem> results)
        {
            List<ProductMapping> productMappingsList = new List<ProductMapping>();
            ProductMapping productMapping = null;
            foreach (SearchResultItem product in results)
            {
                List<OtherDetails> specifications = new List<OtherDetails>();
                List<OtherDetails> otherDetails = new List<OtherDetails>();
                productMapping = new ProductMapping
                {
                    brand = (product.Fields.ContainsKey(Constant.BrandCode)) ? product.Fields[key: Constant.BrandCode].ToString() : "",
                    subCategory = (product.Fields.ContainsKey(Constant.SubCategoryCode)) ? product.Fields[key: Constant.SubCategoryCode].ToString() : "",
                    category = (product.Fields.ContainsKey(Constant.CategoryCode)) ? product.Fields[key: Constant.CategoryCode].ToString() : "",
                    materialGroup = (product.Fields.ContainsKey(Constant.MaterialGroupCode)) ? product.Fields[key: Constant.MaterialGroupCode].ToString() : "",
                    brandTitle = (product.Fields.ContainsKey(Constant.BrandTitle)) ? product.Fields[key: Constant.BrandTitle].ToString() : "",
                    subCategoryTitle = (product.Fields.ContainsKey(Constant.SubCategoryTitle)) ? product.Fields[key: Constant.SubCategoryTitle].ToString() : "",
                    categoryTitle = (product.Fields.ContainsKey(Constant.CategoryTitle)) ? product.Fields[key: Constant.CategoryTitle].ToString() : "",
                    materialGroupTitle = (product.Fields.ContainsKey(Constant.MaterialGroupTitle)) ? product.Fields[key: Constant.MaterialGroupTitle].ToString() : "",
                    skuCode = product.Name,
                    skuName = (product.Fields.ContainsKey(Constant.SKUName)) ? product.Fields[key: Constant.SKUName].ToString() : "",
                    skuDescription = (product.Fields.ContainsKey(Constant.SKUDescription)) ? product.Fields[key: Constant.SKUDescription].ToString() : "",
                    productName = (product.Fields.ContainsKey(Constant.ProductName)) ? product.Fields[key: Constant.ProductName].ToString() : product.Fields[key: Constant.SKUName].ToString().Trim().Replace(" ", "-").ToLower(),
                    buketGroup = (product.Fields.ContainsKey(Constant.bucketGroup)) ? product.Fields[key: Constant.bucketGroup].ToString() : "",
                    travelExclusive = product.Fields.ContainsKey(Constant.istravelExclusive) ? ((product.Fields[key: Constant.istravelExclusive].ToString().Equals("0")) ? false : true) : false,
                    cancellationAndRefundPolicy = (product.Fields.ContainsKey(Constant.Policy)) ? product.Fields[key: Constant.Policy].ToString() : "",
                    specifications = specifications,
                    storeType = (product.Fields.ContainsKey(Constant.StoreType) && !string.IsNullOrEmpty(product.Fields[Constant.StoreType].ToString())) ?
                                product.Fields[Constant.StoreType].ToString() :
                                 string.Empty
                };

                productMapping.aboutBrand = (product.Fields.ContainsKey(Constant.AboutBrand)) ? product.Fields[key: Constant.AboutBrand].ToString() : string.Empty;
                productMapping.productHighlights = (product.Fields.ContainsKey(Constant.ProductHighlights)) ? GetContentList(product.Fields[key: Constant.ProductHighlights].ToString()) : new List<string>();
                productMapping.keyIngredients = (product.Fields.ContainsKey(Constant.KeyIngredients)) ? GetContentList(product.Fields[key: Constant.KeyIngredients].ToString()) : new List<string>();
                productMapping.benefits = (product.Fields.ContainsKey(Constant.ProductBenefits)) ? GetContentList(product.Fields[key: Constant.ProductBenefits].ToString()) : new List<string>();
                productMapping.safety = (product.Fields.ContainsKey(Constant.ProductSafety)) ? GetContentList(product.Fields[key: Constant.ProductSafety].ToString()) : new List<string>();
                productMapping.howToUse = (product.Fields.ContainsKey(Constant.HowToUse)) ? product.Fields[key: Constant.HowToUse].ToString() : string.Empty;
                productMapping.productImages = (product.Fields.ContainsKey(Constant.ProductImages)) ? product.Fields[key: Constant.ProductImages].ToString().Replace("/sitecore/shell/", "").Split(',').ToList() : new List<string>();

                productMappingsList.Add(productMapping);
            }

            return productMappingsList;
        }

        private string GetLoyaltyType(string loyaltyType, string languageCode)
        {
            string earnLoyalty = "Earn";
            try
            {
                Sitecore.Globalization.Language language = Sitecore.Globalization.Language.Parse(languageCode);
                Sitecore.Data.Database contextDB = GetProductContextDatabase();
                Item dictionaryItem = null;
                if (!string.IsNullOrEmpty(Sitecore.Configuration.Settings.GetSetting("DictionaryEarn")))
                {
                    dictionaryItem = contextDB.GetItem(Sitecore.Data.ID.Parse(Sitecore.Configuration.Settings.GetSetting("DictionaryEarn")), language);
                }
                if (dictionaryItem != null)
                {
                    earnLoyalty = dictionaryItem.Fields["Phrase"].Value.ToString();
                }
                earnLoyalty = earnLoyalty.Trim() + " " + loyaltyType;
            }
            catch (Exception ex)
            {
                logRepository.Error("GetLoyaltyType Method in product Search Controller Class gives error  -> " + ex.Message);
            }

            return earnLoyalty;
        }

        private void SetProductFilters(ref Filters filter)
        {
            try
            {
                string[] searchString = filter.slug.Split('/');
                List<string> CatCodeList = new List<string>();
                List<string> SubcatCodeList = new List<string>();
                List<string> BrandCodeList = new List<string>();
                string offerFilter = string.Empty;
                offerFilter = searchString.Any(a => a.IndexOf("offer") > -1) ? searchString.Where(a => a.IndexOf("offer") > -1).FirstOrDefault() : string.Empty;
                if (searchString.Length > 0)
                {
                    List<SearchMaterialGroup> searchMaterialGroups = searchItems.GetSolrMaterialgroups();
                    List<SearchCategory> searchCategories = searchItems.GetSolrCategories();
                    List<SearchSubcategory> searchSubcategories = searchItems.GetSolrSubcategories();
                    List<SearchBrand> searchBrands = searchItems.GetSolrBrands();

                    foreach (string slug in searchString)
                    {
                        if (!string.IsNullOrEmpty(slug))
                        {
                            SearchMaterialGroup searchMaterialGroup = searchMaterialGroups.FirstOrDefault(x => x.MaterialGroupName.Equals(slug.Trim()) || x.MaterialGroupCode.Equals(slug.Trim().ToLower()));
                            filter.materialGroup = searchMaterialGroup != null ? searchMaterialGroup.MaterialGroupCode : filter.materialGroup;

                            SearchCategory searchCategory = searchCategories.FirstOrDefault(x => x.CategoryName.Equals(slug.Trim()) || x.CategoryCode.Equals(slug.Trim().ToLower()));
                            if (searchCategory != null && CatCodeList.IndexOf(searchCategory.CategoryCode) == -1)
                            {
                                CatCodeList.Add(searchCategory.CategoryCode);
                            }
                            SearchSubcategory searchSubcategory = searchSubcategories.FirstOrDefault(x => x.SubcategoryName.Equals(helper.SanitizeName(slug.Trim())) || x.SubcategoryCode.Equals(slug.Trim().ToLower()));
                            if (searchSubcategory != null && SubcatCodeList.IndexOf(searchSubcategory.SubcategoryCode) == -1)
                            {
                                SubcatCodeList.Add(searchSubcategory.SubcategoryCode);
                            }
                            SearchBrand searchBrand = searchBrands.FirstOrDefault(x => x.BrandName.Equals(slug.Trim()) || x.BrandCode.Equals(slug.Trim().ToLower()));
                            if (searchBrand != null && BrandCodeList.IndexOf(searchBrand.BrandCode) == -1)
                            {
                                BrandCodeList.Add(searchBrand.BrandCode);
                            }
                        }
                    }
                }

                filter.category = CatCodeList.ToArray();
                filter.subCategory = SubcatCodeList.ToArray();
                filter.brand = BrandCodeList.ToArray();
                if (filter.materialGroup.Equals(string.Empty) && CatCodeList.Count == 0 && SubcatCodeList.Count == 0 && BrandCodeList.Count == 0)
                {
                    filter.materialGroup = "NoRecords";
                }

                if (!string.IsNullOrEmpty(offerFilter))
                {
                    Sitecore.Data.Database contextDB = Sitecore.Context.Database;
                    Item offerFolderItem = contextDB.GetItem(Constant.ExclusiveOffersFolderId.ToString());
                    SelectedFilters selectedFiltersOffer = null;
                    selectedFiltersOffer = searchItems.GetOffersConfiguration(offerFilter.Trim().ToLower(), offerFolderItem);
                    filter.materialGroup = selectedFiltersOffer.materialGroup.ToLower();
                    filter.offers = selectedFiltersOffer.offers;
                    filter.skuCode = selectedFiltersOffer.skuCode;
                    filter.brand = selectedFiltersOffer.brand;
                    filter.category = selectedFiltersOffer.category;
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("SetProductFilters Method in product Search Controller Class gives error  -> " + ex.Message);
            }
        }

        private IQueryable<SearchResultItem> GetBuyTogetherProductsForCart(List<string> buyTogetherOnCart, ref Filters filters)
        {
            IQueryable<SearchResultItem> buyTogetherProducts = null;
            Filters buyTogetherFilters = new Filters();
            buyTogetherFilters.filterType = filters.filterType;
            buyTogetherFilters.storeType = filters.storeType;
            List<string> buyTogetherSkuCodes = new List<string>();
            int maxCodes = GetIntrestedProductsConfig();
            foreach (string buyTogether in buyTogetherOnCart)
            {
                //get distict skuCodes from soldTogetherProductSKU form cart skucode list, except skucode added in cart
                GetTopSKUCodes(buyTogether, ref buyTogetherSkuCodes, ref filters, maxCodes);
            }
            buyTogetherFilters.skuCode = buyTogetherSkuCodes.ToArray();

            buyTogetherProducts = searchItems.GetSolrProducts(buyTogetherFilters);

            return buyTogetherProducts;
        }

        private List<string> GetTopSKUCodes(string buyTogether, ref List<string> buyTogetherSkuCodes, ref Filters filters, int maxCodes)
        {
            foreach (string skuCode in buyTogether.Split(','))
            {
                if (!buyTogetherSkuCodes.Contains(skuCode) && !filters.skuCode.Contains(skuCode) && maxCodes > 0)
                {
                    maxCodes = maxCodes - 1;
                    buyTogetherSkuCodes.Add(skuCode);
                }
            }
            return buyTogetherSkuCodes;
        }

        private Dictionary<string, string> GetDictionaryDetails(string languageCode)
        {
            Dictionary<string, string> dictionatyItems = new Dictionary<string, string>();
            string[] fieldItems = { "Brand", "Category", "SubCategory", "Productageofliquor", "CountryOfOrigin", "Productdescription", "ProductLength", "ProductWidth", "ProductHeight", "ProductWeight", "ProductVolume", "ProductQuantity", "ProductGender", "ProductBarcodeNumber", "ManufacturerDetails", "Earn", "ProductFlavour", "ProductForm", "FrameColorFront", "FrameColorTemple", "LensColor", "Material", "MaterialFittingName" };

            try
            {
                Sitecore.Globalization.Language language = Sitecore.Globalization.Language.Parse(languageCode);
                Sitecore.Data.Database contextDB = GetProductContextDatabase();
                foreach (string item in fieldItems)
                {
                    Item dictionaryItem = contextDB.GetItem(Sitecore.Data.ID.Parse(Sitecore.Configuration.Settings.GetSetting(("Dictionary" + item))), language);
                    if (dictionaryItem != null)
                    {
                        dictionatyItems.Add(item, dictionaryItem.Fields["Phrase"].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetDictionaryDetails Method in product Search Controller Class gives error  ->  -- " + ex.Message);
            }

            return dictionatyItems;
        }

        private List<string> GetContentList(string content)
        {
            List<string> itemContent = new List<string>();

            try
            {
                itemContent = content.Split('\n').ToList();
            }
            catch (Exception ex)
            {
                logRepository.Error("Error in GetContentList method in ProductSearch " + ex.Message);
            }

            return itemContent;
        }



        private IQueryable<SearchResultItem> GetExcluiveProducts(Filters filter)
        {
            IQueryable<SearchResultItem> productSitecoreResult = null;
            Filters exclusiveFilter = new Filters();
            exclusiveFilter.filterType = filter.filterType;
            exclusiveFilter.airportCode = filter.airportCode;
            exclusiveFilter.storeType = filter.storeType;
            exclusiveFilter.includeOOS = true;
            List<APIProduct> AllExclusive = searchItems.GetAPIExclusiveProducts(exclusiveFilter);
            Log.Info("GetExcluiveProducts AllExclusive Count " + AllExclusive.Count, this);
            if (AllExclusive.Count > 0)
            {
                exclusiveFilter.skuCode = AllExclusive.Select(a => a.flemingo_sku_code).ToArray();
                productSitecoreResult = searchItems.GetSolrProducts(exclusiveFilter);
                Log.Info("GetExcluiveProducts productSitecoreResult Count " + productSitecoreResult.Count(), this);
            }

            return productSitecoreResult;
        }

        private IQueryable<SearchResultItem> GetComboProducts(Filters filter)
        {
            IQueryable<SearchResultItem> productSitecoreResult = null;
            Filters comboFilter = new Filters();
            comboFilter.filterType = filter.filterType;
            comboFilter.airportCode = filter.airportCode;
            comboFilter.storeType = filter.storeType;
            comboFilter.includeOOS = true;
            List<APIProduct> AllComboProducts = searchItems.GetAPIComboProducts(comboFilter);
            if (filter.comboFilter.Length > 0)
            {
                AllComboProducts = AllComboProducts.Where(a => filter.comboFilter.Contains(helper.SanitizeName(a.material_group_name))).ToList();
            }
            if (AllComboProducts.Count > 0)
            {
                comboFilter.skuCode = AllComboProducts.Select(a => a.flemingo_sku_code).ToArray();
                productSitecoreResult = searchItems.GetSolrProducts(comboFilter);
            }

            return productSitecoreResult;
        }

        private string GetOfferProductImage(string offerSKUcode)
        {
            string imagepath = string.Empty;
            List<string> imageList = new List<string>();
            try
            {
                List<string> oSKUlist = new List<string>();
                oSKUlist.Add(offerSKUcode);
                Filters Ofilters = new Filters();
                Ofilters.filterType = "Product";
                Ofilters.skuCode = oSKUlist.ToArray();
                IQueryable<SearchResultItem> productResults = searchItems.GetSolrProducts(Ofilters);
                SearchResultItem product = productResults.FirstOrDefault(a => a.Name == offerSKUcode);
                if (product != null)
                {
                    imageList = (product != null && product.Fields.ContainsKey(Constant.ProductImages)) ? product.Fields[key: Constant.ProductImages].ToString().Replace("/sitecore/shell/", "").Split(',').ToList() : new List<string>();
                    imagepath = imageList.Count() > 0 ? imageList.FirstOrDefault().ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetOfferProductImage Method GetDutyFreeProducts in ProductSearchController gives error -> " + ex.Message);
            }

            return imagepath;
        }

        private void UpdateFilter(ref Filters filter)
        {
            Filters searchFilter = new Filters();
            searchFilter.filterType = "Product";
            searchFilter.airportCode = filter.airportCode;
            searchFilter.storeType = "departure";
            SearchResultItem product = null;
            if (filter.skuCode.Length == 0) {
                //For Category
                if (filter.category.Length > 0)
                {
                    searchFilter.category = filter.category;
                    IQueryable<SearchResultItem> productResults = searchItems.GetSolrProducts(searchFilter);
                    if (productResults.Count() > 0)
                    {
                        product = productResults.FirstOrDefault();
                        filter.materialGroup = product.Fields[key: Constant.MaterialGroupCode].ToString();
                    }
                }
                //for SubCategory
                if (filter.subCategory.Length > 0)
                {
                    searchFilter.subCategory = filter.subCategory;
                    IQueryable<SearchResultItem> productResults = searchItems.GetSolrProducts(searchFilter);
                    if (productResults.Count() > 0)
                    {
                        product = productResults.FirstOrDefault();
                        filter.materialGroup = product.Fields[key: Constant.MaterialGroupCode].ToString();
                    }
                }
                // fro Brand
                if (filter.brand.Length > 0)
                {
                    searchFilter.brand = filter.brand;
                    IQueryable<SearchResultItem> productResults = searchItems.GetSolrProducts(searchFilter);
                    if (productResults.Count() > 0)
                    {
                        product = productResults.FirstOrDefault();
                        filter.materialGroup = product.Fields[key: Constant.MaterialGroupCode].ToString();
                    }
                }
            }
        }
    }
}