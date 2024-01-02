using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class APIOffersResponse
    {
        public List<APIOffer> data { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public object warning { get; set; }
        public object error { get; set; }
    }

    public class APIExclusiveProducts
    {
        public List<APIProduct> data { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public object warning { get; set; }
        public object error { get; set; }
    }

    public class APIOffer
    {
        public int id { get; set; }
        public int skuId { get; set; }
        public string promotionCode { get; set; }
        //public int buyQuantity { get; set; }
        public string operatorType { get; set; }
        public string promotionType { get; set; }
        public string offerType { get; set; }
      //  public decimal offer { get; set; }
        public string offerDisplayText { get; set; }
        public SkuResponse skuResponse { get; set; }
    }

    public class SkuResponse
    {
        public int id { get; set; }
        public string materialGroupName { get; set; }
        public string categoryName { get; set; }
        public string subCategoryName { get; set; }
        public string brandName { get; set; }
        public string skuCode { get; set; }
    }

    public class APIProduct
    {
        public int id { get; set; }
        public string material_group_name { get; set; }
        public string category_name { get; set; }
        public string sub_category_name { get; set; }
        public string brand_name { get; set; }
        public string flemingo_sku_code { get; set; }
        public string sku_name { get; set; }
    }
}
