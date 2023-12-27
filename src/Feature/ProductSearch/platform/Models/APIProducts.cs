using System;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class APIProducts
    {
        //public int id { get; set; }
        //public int category_id { get; set; }
        //public int sub_category_id { get; set; }
        //public int brand_id { get; set; }
        public string material_group_name { get; set; }
        public string category_name { get; set; }
        public string sub_category_name { get; set; }
        public string brand_name { get; set; }
        public string flemingo_platform_id { get; set; }
        public string flemingo_sku_code { get; set; }
        public string sku_name { get; set; }
        public string sku_description { get; set; }
        public decimal size_id { get; set; }      
        public int saleable_quantity { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal volume { get; set; }
        public string volume_unit_id { get; set; }
        public string unit_weight { get; set; }
        public string weight_unit_id { get; set; }
        public int inventory_type_id { get; set; }
        public string supplier_name { get; set; }
        public int threshold_limlit { get; set; }
        public int age_of_product_for_liquor { get; set; }
        public string gender { get; set; }
        public bool status { get; set; }
        //public string image1 { get; set; }
        //public object image2 { get; set; }
        //public object meta_title { get; set; }
        //public object meta_description { get; set; }
        //public object meta_keyword { get; set; }
        //public object meta_tag { get; set; }
        public int price { get; set; }
        public int stock_in_hand { get; set; }
        public int threshold_quantity { get; set; }
        public List<APIPromotion> promotions { get; set; }
        public string store_code { get; set; }
        public string airport_code { get; set; }
        public decimal sku_size { get; set; }
        public string sku_unit { get; set; }
        public string bucket_name { get; set; }
        public bool isRecommended { get; set; }
        public bool isTravelExclusive { get; set; } 
        // Field added to map loyalty points from api
        public decimal loyaltyPoints { get; set; }
        public string loyaltyType { get; set; }
        public bool isExclusive { get; set; }
        public bool isCombo { get; set; }
        public bool sellable { get; set; }
    }
    public class APIProductResponse
    {
        public List<APIProducts> data { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public object warning { get; set; }
        public object error { get; set; }
    }

    public class APIPromotion
    {
        public string promotion_code { get; set; }
        public int buy_quantity { get; set; }
        public int offer_type { get; set; }
        public decimal offer { get; set; }
        public string offer_display_text { get; set; }
        public decimal offer_price { get; set; }
        public decimal discount_price { get; set; }
        public FreebeeOffer FreebeeOffer { get; set; }
    }

    public class FreebeeOffer
    {
        public string offerSKUCode { get; set; }
        public string offerProductName { get; set; }
        public bool isSellable { get; set; }
        public decimal price { get; set; }
    }

    public class APIPramotions
    {
        List<APIPromotion> aPIPramotions { get; set; }
    }

}