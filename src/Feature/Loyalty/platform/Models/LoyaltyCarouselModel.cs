using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models
{
    public class LoyaltyCarouselModel
    {
        public List<Category> categories { get; set; }
    }

    public class Category
    {
        public string category { get; set; }
        public List<ProductMapping> productDatas { get; set; }
    }

    public class productData
    {
        public string materialGroup { get; set; }
        public string brand { get; set; }
        public string category { get; set; }
        public string subCategory { get; set; }
        public string skuName { get; set; }
        public string skuCode { get; set; }

        public decimal price { get; set; }
       
        public string pointsLabel { get; set; }
        public List<object> pramotions { get; set; }

        public List<string> productImages { get; set; }

        public string Earn2XString { get; set; }

    }
}