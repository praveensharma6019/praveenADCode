using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch.SearchTypes;
using System.Web.Http;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using System.IO;
using Newtonsoft.Json;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Controllers
{
    public class ProductSitemapController : ApiController
    {
        private ILogRepository logRepository;
        private readonly ISearchBuilder searchBuilder;
        private readonly IHelper _helper;
        private readonly ISearchItems searchItems;
        private readonly IAPIResponse productResponse;
        public ProductSitemapController(ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper helper, ISearchItems _searchItems, IAPIResponse _productResponse)
        {
            this.logRepository = _logRepository;
            this.searchBuilder = _searchBuilder;
            this.searchItems = _searchItems;
            this._helper = helper;
            this.productResponse = _productResponse;
        }

        // POST: GetDutyFreeProductSitemap
        [HttpPost]
        [Route("api/GetDutyFreeProductSitemap")]
        public async Task<IHttpActionResult> GetDutyFreeProductSitemap([FromBody] Filters filter)
        {
            ResponseSitemapData responseData = new ResponseSitemapData();
            APIProductResponse apiResponse = null;
            filter.filterType = "Product";
            filter.pageSize = 5000;
            filter.page = 1;
            filter.includeOOS = true;
            List<ProductData> result = new List<ProductData>();

            APIProductSearchData aPIProductSearchData = new APIProductSearchData();

            try
            {
                List<string> enabledMaterialGroups = GetAvaialbleMaterialGroups(filter);
                List<ProductSearchData> productSearchDataList = new List<ProductSearchData>(); 

                foreach (string materialGroup in enabledMaterialGroups)
                {
                    filter.materialGroup = materialGroup;
                    IQueryable<SearchResultItem> productResults = searchItems.GetSolrProducts(filter);
                    List<SearchResultItem> productsSolrList = productResults.ToList();
                    apiResponse = await searchItems.GetProductsFromAPI(filter, productsSolrList, false);
                    if (apiResponse != null && apiResponse.data != null)
                    {
                        productSearchDataList = ParseProductData(apiResponse.data, productsSolrList);
                        result.AddRange(productSearchDataList.Select(a=>
                        new ProductData
                        {
                            materialGroup = a.materialGroup,
                            materialGroupTitle = a.materialGroupTitle,
                            category = a.category,
                            categoryTitle = a.categoryTitle,
                            subCategory = a.subCategory,
                            subCategoryTitle = a.subCategoryTitle,
                            brand = a.brand,
                            brandTitle = a.brandTitle,
                            skuCode = a.skuCode,
                            skuName = a.skuName,
                            productName = a.productName
                        }
                        ));
                        aPIProductSearchData.data.AddRange(productSearchDataList);
                        aPIProductSearchData.count = aPIProductSearchData.data.Count();
                    }
                }

                if (result.Count() > 0)
                {
                    responseData.status = true;
                    responseData.data.count = result.Count();
                    responseData.data.result = result;                    
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetDutyFreeProductSitemap Method in ProductSitemapController gives error  -> " + ex.Message);
            }
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/app_data/dutyfree/" + filter.airportCode.ToLower() + "_" + filter.storeType.ToLower() + ".json");
                string jsonString = JsonConvert.SerializeObject(aPIProductSearchData);
                if (!File.Exists(path))
                {
                    File.Create(path);
                }

                if (File.Exists(path))
                {
                    File.WriteAllText(path, jsonString);
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetDutyFreeProductSitemap Method in ProductSitemapController gives error Unable to create/update File  -> " + ex.Message);
            }

            return Json(responseData);
        }
        private List<ProductSearchData> ParseProductData(List<APIProducts> APIProductList, List<SearchResultItem> productsSolrList)
        {
            List<ProductSearchData> products = new List<ProductSearchData>();
            ProductSearchData productData = null;
            foreach (APIProducts apiProduct in APIProductList)
            {
                SearchResultItem product = productsSolrList.FirstOrDefault(x => x.Name == apiProduct.flemingo_sku_code);
                if (product != null)
                {
                    productData = new ProductSearchData
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
                        productName = (product.Fields.ContainsKey(Constant.ProductName)) ? product.Fields[key: Constant.ProductName].ToString() : product.Fields[key: Constant.SKUName].ToString().Trim().Replace(" ", "-").ToLower(),
                        productImages = (product.Fields.ContainsKey(Constant.ProductImages)) ? product.Fields[key: Constant.ProductImages].ToString().Replace("/sitecore/shell/", "").Split(',').ToList() : new List<string>(),
                        availability = (apiProduct.stock_in_hand - apiProduct.threshold_quantity) > 0 ? true : false,
                        availableQuantity = (apiProduct.stock_in_hand - apiProduct.threshold_quantity),
                        isExclusive = apiProduct.isExclusive,
                        isCombo = apiProduct.isCombo,
                        price = apiProduct.price,
                        promotions = GetAPIOfferList(apiProduct),
                    };
                    products.Add(productData);
                }
            }

            return products;
        }
        private List<string> GetAvaialbleMaterialGroups(Filters filter)
        {
            List<string> materialGroupForAirport = new List<string>();
            try
            {
                Sitecore.Globalization.Language language = Sitecore.Globalization.Language.Parse(filter.language);
                Sitecore.Data.Database contextDB = Sitecore.Context.Database;
                Item materialGroupFolder = contextDB.GetItem(Constant.MaterialGroupFolder.ToString(), language);

                string materialGroupCode = string.Empty;
                if (materialGroupFolder.HasChildren)
                {
                    var materialGroupList = materialGroupFolder.Children.Where(x => x.TemplateID.Guid == Constant.MaterialGroupTemplate);

                    foreach (Item materialGroupItem in materialGroupList)
                    {
                        materialGroupCode = !string.IsNullOrEmpty(materialGroupItem.Fields[Constant.MaterialGroup_Code].Value) ? materialGroupItem.Fields[Constant.MaterialGroup_Code].Value.ToLower().Trim() : string.Empty;

                        //Material Group is not virtual
                        if (!string.IsNullOrEmpty(materialGroupCode) && !(Constant.VirtualMaterialGroup.IndexOf(materialGroupCode) > -1))
                        {
                            if (!_helper.GetAvaialbilityOnAirport(materialGroupItem, filter.airportCode, filter.storeType))
                            {
                                materialGroupForAirport.Add(materialGroupCode);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetAvaialbleMaterialGroups Method in ProductSitemapController gives error  -> " + ex.Message);
            }

            return materialGroupForAirport;
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
                        objPromotions.offerProductSEOName = _helper.SanitizeName(apiPromotion.FreebeeOffer.offerProductName.ToLower().Replace(" ", "-"));
                        objPromotions.sellable = apiPromotion.FreebeeOffer.isSellable;
                        objPromotions.offerProductPrice = apiPromotion.FreebeeOffer.price;
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
    }
}