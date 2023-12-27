using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform.Models
{
    public class Product
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public int sub_category_id { get; set; }
        public int brand_id { get; set; }
        public string material_group_name { get; set; }
        public string category_name { get; set; }
        public string sub_category_name { get; set; }
        public string brand_name { get; set; }
        //public int flemingo_platform_id { get; set; }
        public string flemingo_sku_code { get; set; }
        public string sku_name { get; set; }
        public string sku_description { get; set; }
       // public int size_id { get; set; }
       // public int type_id { get; set; }
       // public int saleable_quantity { get; set; }
        //public int length { get; set; }
        //public int width { get; set; }
        //public int height { get; set; }
        //public int volume { get; set; }
      //  public string volume_unit_id { get; set; }
      //  public int unit_weight { get; set; }
       // public string weight_unit_id { get; set; }
       // public int inventory_type_id { get; set; }
        public string supplier_name { get; set; }
     //   public int threshold_limlit { get; set; }
        public int age_of_product_for_liquor { get; set; }
        public string gender { get; set; }
        public bool status { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public string image5 { get; set; }
        public string image6 { get; set; }
        public object meta_title { get; set; }
        public List<Promotion> promotions { get; set; }       
        public string store_code { get; set; }
        public string airport_code { get; set; }
        public string bucket_name { get; set; }
        public bool isRecommended { get; set; }
        public decimal travel_exclusive { get; set; }
        public string soldTogether { get; set; }
    }

    public class Promotion
    {
        public string promotion_code { get; set; }
        public decimal buy_quantity { get; set; }
        public decimal offer_type { get; set; }
        public decimal offer { get; set; }
        public string offer_display_text { get; set; }
    }

    public class APIresult
    {
        public string status { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public string warning { get; set; }
       // public string error { get; set; }
        public List<Product> data { get; set; }
    }
}