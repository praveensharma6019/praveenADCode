using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public class Earn2XLoyaltyProduct : IEarn2XProduct
    {
        private readonly ILogRepository _logRepository;
        private readonly Foundation.Theming.Platform.Services.IWidgetService _widgetservice;
        private readonly ISearchBuilder searchBuilder;
        private readonly IAPIResponse _productResponse;
        private readonly IHelper _helper;
        public Earn2XLoyaltyProduct(ILogRepository logRepository, Foundation.Theming.Platform.Services.IWidgetService widgetService, IHelper helper, IAPIResponse productResponse, ISearchBuilder _searchBuilder)
        {
            this._logRepository = logRepository;
            this._widgetservice = widgetService;
            this.searchBuilder = _searchBuilder;
            this._productResponse = productResponse;
            this._helper = helper;
        }
        public LoyaltyWidgets get2xLoyaltyProductData(Rendering rendering,string type)
        {
            LoyaltyWidgets loyaltyWidgets = new LoyaltyWidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;
                loyaltyWidgets.widget = widget != null? _widgetservice.GetWidgetItem(widget): new Foundation.Theming.Platform.Models.WidgetItem();
                loyaltyWidgets.widget.widgetItems = getLoyaltyProduct(rendering, type);
            }
            catch (Exception ex)
            {

                _logRepository.Error("get2xLoyaltyProductData throws Exception -> " + ex.Message);
            }
            return loyaltyWidgets;
        }

        private List<object> getLoyaltyProduct(Rendering rendering, string type)
        {
            List<object> loyaltyProductObject = new List<object>();
            try
            {
                var loyaltyAPIProductResponseDeserialise= GetLoyaltyProductData(rendering, type);
                //_logRepository.Info("DataAPI FROM SQL SERVER " + loyaltyAPIProductResponseDeserialise.ToString());
                var loyaltyProduct=searchBuilder.GetSearchResults(LoyaltyPredicateBuilder.GetSearchPredicate(getSKUList(loyaltyAPIProductResponseDeserialise)));
                //_logRepository.Info("Data count from Solr API" + loyaltyProduct.ToList().Count);
                loyaltyProductObject=parseLoyaltyProductData(loyaltyAPIProductResponseDeserialise, loyaltyProduct);

            }
            catch (Exception ex)
            {
                _logRepository.Error("getLoyaltyProduct throws Exception -> " + ex.Message);
            }
            
            return loyaltyProductObject;
        }

        private List<object> parseLoyaltyProductData(LoyaltyAPIProductResponse loyaltyAPIProductResponseDeserialise, IQueryable<SearchResultItem> loyaltyProduct)
        {
            List<object> categories = new List<object>();
            Category objcat = null;
            try
            {
                if (loyaltyAPIProductResponseDeserialise != null)
                {
                    var loyaltyCategories = loyaltyAPIProductResponseDeserialise.data.Select(x => x.category_name).ToList().Distinct();
                    //_logRepository.Info("List of categories Count "+ loyaltyCategories.Count());
                    foreach (string Item in loyaltyCategories)
                    {
                        objcat = new Category();
                        objcat.category = Item.ToString();
                        objcat.productDatas = getLoyaltyDataFromCategory(objcat.category, loyaltyProduct, loyaltyAPIProductResponseDeserialise);
                        categories.Add(objcat);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error("parseLoyaltyProductData throws Exception -> " + ex.Message);
            }
            return categories;
        }

        private List<ProductMapping> getLoyaltyDataFromCategory(string category, IQueryable<SearchResultItem> loyaltyProduct, LoyaltyAPIProductResponse loyaltyAPIProductResponseDeserialise)
        {
            List<ProductMapping> productDatas = new List<ProductMapping>();
            List<OtherDetails> specifications = null;
            List<OtherDetails> otherDetails = null;
            ProductMapping objProductData = null;
            string storeType = "Arrival-Departure";
            try
            {
                var categoryCodeLowerCase = _helper.Sanitize(category);
                var skuListByCategory = loyaltyProduct.ToList().Where(p => p.Fields[Templates.LoyaltySKU.CategoryCode].ToString() == categoryCodeLowerCase.ToLower());
                foreach (var i in skuListByCategory)
                {
                    foreach (var Storetype in storeType.Split('-'))
                    {
                        var apiProductdata = loyaltyAPIProductResponseDeserialise.data.ToList().Where(x => x.flemingo_sku_code == i.Fields[Templates.LoyaltySKU.SKUCode].ToString() && x.storeType == Storetype).FirstOrDefault();
                        if (apiProductdata != null)
                        {
                            specifications = new List<OtherDetails>();
                            otherDetails = new List<OtherDetails>();
                            objProductData = new ProductMapping();
                            objProductData.skuCode = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.SKUCode].ToString()) ? i.Fields[Templates.LoyaltySKU.SKUCode].ToString() : string.Empty;
                            objProductData.materialGroup = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.MaterialGroup].ToString()) ? i.Fields[Templates.LoyaltySKU.MaterialGroup].ToString() : string.Empty;
                            objProductData.brand = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.Brand].ToString()) ? i.Fields[Templates.LoyaltySKU.Brand].ToString() : string.Empty;
                            objProductData.category = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.Category].ToString()) ? i.Fields[Templates.LoyaltySKU.Category].ToString() : string.Empty;
                            objProductData.subCategory = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.SubCategory].ToString()) ? i.Fields[Templates.LoyaltySKU.SubCategory].ToString() : string.Empty;
                            objProductData.skuName = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.SKUName].ToString()) ? i.Fields[Templates.LoyaltySKU.SKUName].ToString() : string.Empty;
                            objProductData.subCategoryTitle = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.SubCategoryTitle].ToString()) ? i.Fields[Templates.LoyaltySKU.SubCategoryTitle].ToString() : string.Empty;
                            objProductData.categoryTitle = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.CategoryTitle].ToString()) ? i.Fields[Templates.LoyaltySKU.CategoryTitle].ToString() : string.Empty;
                            objProductData.materialGroupTitle = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.MaterialGroupTitle].ToString()) ? i.Fields[Templates.LoyaltySKU.MaterialGroupTitle].ToString() : string.Empty;
                            objProductData.brandTitle = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.BrandTitle].ToString()) ? i.Fields[Templates.LoyaltySKU.BrandTitle].ToString() : string.Empty;
                            objProductData.skuCode = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.SKUCode].ToString()) ? i.Fields[Templates.LoyaltySKU.SKUCode].ToString() : string.Empty;
                            objProductData.skuDescription = !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.SKUDescription].ToString()) ? i.Fields[Templates.LoyaltySKU.SKUDescription].ToString() : string.Empty;
                            objProductData.skuSize = Convert.ToString(apiProductdata.sku_size);
                            objProductData.price = Convert.ToDecimal(getValue(apiProductdata.price));
                            objProductData.storeCode = Convert.ToString(apiProductdata.store_code);
                            objProductData.airportStoreCode = Convert.ToString(apiProductdata.airport_code);
                            objProductData.promotions = getOfferpromotions(apiProductdata.promotions);
                            objProductData.discountPrice = GetDiscountPrice(objProductData.promotions);
                            objProductData.buketGroup = (i.Fields.ContainsKey(Templates.LoyaltySKU.bucketGroup)) ? i.Fields[key: Templates.LoyaltySKU.bucketGroup].ToString() : "";
                            objProductData.recomended = Convert.ToBoolean(apiProductdata.isRecommended);
                            objProductData.travelExclusive = i.Fields.ContainsKey(Templates.LoyaltySKU.istravelExclusive) ? ((i.Fields[key: Templates.LoyaltySKU.istravelExclusive].ToString().Equals("0")) ? false : true) : false;
                            objProductData.cancellationAndRefundPolicy = (i.Fields.ContainsKey(Templates.LoyaltySKU.Policy)) ? i.Fields[key: Templates.LoyaltySKU.Policy].ToString() : string.Empty;
                            objProductData.loyaltyPoints = Convert.ToInt32(apiProductdata.loyaltyPoints) == 0 ? 0 :
                                                                Convert.ToInt32(apiProductdata.loyaltyPoints);
                            objProductData.skuUnit = !string.IsNullOrEmpty(apiProductdata.sku_unit) ?
                                                        Convert.ToString(apiProductdata.sku_unit) :
                                                        string.Empty;
                            objProductData.specifications = specifications;
                            objProductData.specifications.Add(GetDictionaryKeyValuePair("category", objProductData.categoryTitle));
                            objProductData.specifications.Add(GetDictionaryKeyValuePair("subCategory", objProductData.subCategoryTitle));
                            objProductData.specifications.Add(GetDictionaryKeyValuePair("brand", objProductData.brandTitle));
                            if (apiProductdata.age_of_product_for_liquor > 0)
                            {
                                objProductData.specifications.Add(GetDictionaryKeyValuePair("productageofliquor", apiProductdata.age_of_product_for_liquor.ToString()));
                            }
                            if (i.Fields.ContainsKey(Templates.LoyaltySKU.CountryOfOrigin))
                            {
                                objProductData.specifications.Add(GetDictionaryKeyValuePair("CountryOfOrigin", i.Fields[key: Templates.LoyaltySKU.CountryOfOrigin].ToString()));
                            }
                            if (i.Fields.ContainsKey(Templates.LoyaltySKU.ProductDescription))
                            {
                                objProductData.specifications.Add(GetDictionaryKeyValuePair("productpescription", i.Fields[key: Templates.LoyaltySKU.ProductDescription].ToString()));
                            }
                            if (apiProductdata.length > 0)
                            {
                                objProductData.specifications.Add(GetDictionaryKeyValuePair("productlength", apiProductdata.length.ToString()));
                            }
                            if (apiProductdata.height > 0)
                            {
                                objProductData.specifications.Add(GetDictionaryKeyValuePair("productheight", apiProductdata.height.ToString()));
                            }
                            if (apiProductdata.width > 0)
                            {
                                objProductData.specifications.Add(GetDictionaryKeyValuePair("productwidth", apiProductdata.width.ToString()));
                            }
                            if (apiProductdata.volume > 0)
                            {
                                objProductData.specifications.Add(GetDictionaryKeyValuePair("productvolume", apiProductdata.volume.ToString()));
                            }
                            //if (apiProductdata.unit_weight > 0)
                            //{
                            //    objProductData.specifications.Add(GetDictionaryKeyValuePair("productweight", apiProductdata.unit_weight.ToString()));
                            //}
                            //if (!string.IsNullOrEmpty(apiProductdata.gender.ToString()) && !apiProductdata.gender.ToString().Equals("0"))
                            //{
                            //    objProductData.specifications.Add(GetDictionaryKeyValuePair("productgender", apiProductdata.gender.ToString()));
                            //}
                            if (i.Fields.ContainsKey(Templates.LoyaltySKU.ProductBarcodeNumber))
                            {
                                objProductData.otherDetails.Add(GetDictionaryKeyValuePair("productbarcodeNumber", i.Fields[key: Templates.LoyaltySKU.ProductBarcodeNumber].ToString()));
                            }
                            if (i.Fields.ContainsKey(Templates.LoyaltySKU.ManufacturerDetails))
                            {
                                objProductData.otherDetails.Add(GetDictionaryKeyValuePair("manufacturerdetails", i.Fields[key: Templates.LoyaltySKU.ManufacturerDetails].ToString()));
                            }
                            #region 18963 Need to be removed After Testing
                            //objProductData.storeType = (i.Fields.ContainsKey(Templates.LoyaltySKU.StoreType) && !string.IsNullOrEmpty(i.Fields[Templates.LoyaltySKU.StoreType].ToString())) ?
                            //                    i.Fields[Templates.LoyaltySKU.StoreType].ToString() :
                            //                     string.Empty;
                            #endregion
                            // Changed the Mapping for storeType Field for Loyalty (18963)
                            objProductData.storeType = apiProductdata.storeType;
                            objProductData.productImages = GetProductImages(i);
                            objProductData.Earn2XString = Sitecore.Globalization.Translate.Text(Templates.LoyaltySKU.earn2X);
                            objProductData.availability = (apiProductdata.stock_in_hand - apiProductdata.threshold_quantity) > 0 ? true : false;
                            // Property Added for Ticket No 24408
                            objProductData.availableQuantity = (apiProductdata.stock_in_hand - apiProductdata.threshold_quantity);
                            productDatas.Add(objProductData);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                _logRepository.Error("parseLoyaltyProductData throws Exception -> " + ex.Message);
               
            }
            return productDatas;
        }

        private int getValue(int price)
        {
            
            try
            {
                if(price != 0)
                {
                    return price;
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error("getValue throws Exception -> " + ex.Message);
            }
            return 0;
        }

        private List<string> getSKUList(LoyaltyAPIProductResponse loyaltyAPIProductResponseDeserialise)
        {
            List<string> skuList = new List<string>();
            try
            {
                if(loyaltyAPIProductResponseDeserialise.data.Count>0)
                { 
                    skuList = loyaltyAPIProductResponseDeserialise.data.Select(x => x.flemingo_sku_code).ToList();
                }
                //_logRepository.Info("List of SKUCode for DataAPI in SQL SERVER -> " + skuList.Count);
            }
            catch (Exception ex)
            {

                _logRepository.Error("getLoyaltyProduct throws Exception -> " + ex.Message);
            }
            return skuList;
        }

        private LoyaltyAPIProductResponse GetLoyaltyProductData(Rendering rendering, string type)
        {
            LoyaltyAPIProductResponse LoyaltyAPIProductResponseDeserialise = null;
            try
            {
                var response = _productResponse.GetAPIResponse("GET", GetProductURL(), null, GetParams(type), "");
                LoyaltyAPIProductResponseDeserialise= JsonConvert.DeserializeObject<LoyaltyAPIProductResponse>(response);
                
            }
            catch (Exception ex)
            {

                _logRepository.Error("get2xLoyaltyProductData throws Exception -> " + ex.Message);
            }

            return LoyaltyAPIProductResponseDeserialise;
        }

        

        /// <summary>
        /// Code to get the service URL 
        /// </summary>
        /// <returns></returns>
        private string GetProductURL()
        {

            return Sitecore.Configuration.Settings.GetSetting("LoyaltyProductAPI");
        }

        /// <summary>
        /// Code to create Headers for the API
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> CreateRequestHeaders()
        {
            Random _random = new Random();
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Accept", "application/json");
            headers.Add("traceId", _random.Next(600, 40000).ToString());
            headers.Add("agentId", "5000");
            headers.Add("ChannelId", "Sitecore");
            return headers;
        }
        /// <summary>
        /// Code to add defaut params to the service call
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParams(string type)
        {
            Dictionary<string, string> parameters =
                       new Dictionary<string, string>();
            parameters.Add("loyaltyType", type);
           // parameters.Add("storeType", storetype);
            return parameters;

        }
        /// <summary>
        /// Code to get the list of promotion 
        /// </summary>
        /// <param name="promotions"></param>
        /// <returns></returns>
        private List<Promotion> getOfferpromotions(List<object> promotions)
        {
            List<Promotion> offerList = new List<Promotion>();
            try
            {
                foreach (var apiOffer in promotions)
                {
                    Promotion objPromotions = new Promotion();
                    APIPromotion apiPromotion = JsonConvert.DeserializeObject<APIPromotion>(apiOffer.ToString());
                    objPromotions.code = apiPromotion.promotion_code;
                    objPromotions.offer = apiPromotion.offer.ToString();
                    objPromotions.quantity = apiPromotion.buy_quantity;
                    objPromotions.displayText = apiPromotion.offer_display_text;
                    objPromotions.type = apiPromotion.offer_type;
                    objPromotions.offerPrice = apiPromotion.offer_price;
                    objPromotions.discountPrice = apiPromotion.discount_price;

                    offerList.Add(objPromotions);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetAPIOfferList Method in product Search Controller Class gives error -> " + ex.Message);
            }

            return offerList;
        }

        private List<string> GetProductImages(SearchResultItem product)
        {
            List<string> images = new List<string>();
            if (product.Fields.ContainsKey(Templates.LoyaltySKU.productImage_1) && !string.IsNullOrEmpty(product.Fields[Templates.LoyaltySKU.productImage_1].ToString()))
            {
                images.Add(product.Fields[Templates.LoyaltySKU.productImage_1].ToString().Replace("/sitecore/media library", "~/media"));
            }
            if (product.Fields.ContainsKey(Templates.LoyaltySKU.productImage_2) && !string.IsNullOrEmpty(product.Fields[Templates.LoyaltySKU.productImage_2].ToString()))
            {
                images.Add(product.Fields[Templates.LoyaltySKU.productImage_2].ToString().Replace("/sitecore/media library", "~/media"));
            }
            if (product.Fields.ContainsKey(Templates.LoyaltySKU.productImage_3) && !string.IsNullOrEmpty(product.Fields[Templates.LoyaltySKU.productImage_3].ToString()))
            {
                images.Add(product.Fields[Templates.LoyaltySKU.productImage_3].ToString().Replace("/sitecore/media library", "~/media"));
            }
            if (product.Fields.ContainsKey(Templates.LoyaltySKU.productImage_4) && !string.IsNullOrEmpty(product.Fields[Templates.LoyaltySKU.productImage_4].ToString()))
            {
                images.Add(product.Fields[Templates.LoyaltySKU.productImage_4].ToString().Replace("/sitecore/media library", "~/media"));
            }
            if (product.Fields.ContainsKey(Templates.LoyaltySKU.productImage_5) && !string.IsNullOrEmpty(product.Fields[Templates.LoyaltySKU.productImage_5].ToString()))
            {
                images.Add(product.Fields[Templates.LoyaltySKU.productImage_5].ToString().Replace("/sitecore/media library", "~/media"));
            }
            if (product.Fields.ContainsKey(Templates.LoyaltySKU.productImage_6) && !string.IsNullOrEmpty(product.Fields[Templates.LoyaltySKU.productImage_6].ToString()))
            {
                images.Add(product.Fields[Templates.LoyaltySKU.productImage_6].ToString().Replace("/sitecore/media library", "~/media"));
            }
            return images;
        }
        /// <summary>
        /// Code to get the discount price based on promotions
        /// </summary>
        /// <param name="promotions"></param>
        /// <returns></returns>
        private decimal GetDiscountPrice(List<Promotion> promotions)
        {
           return (promotions.Count > 0 && promotions.FirstOrDefault(x => x.quantity == 1 && x.type == 1)!=null) ?Convert.ToDecimal(promotions.FirstOrDefault(x => x.quantity == 1 && x.type == 1).discountPrice) : 0; 
        }
        /// <summary>
        /// Code to get dictionary Item 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private OtherDetails GetDictionaryKeyValuePair(string key, string value)
        {
            // key = Sitecore.Globalization.Translate.Text(key);
            OtherDetails otherDetails = new OtherDetails { key = Sitecore.Globalization.Translate.Text(key), value = value };
            return otherDetails;
        }

    }
}