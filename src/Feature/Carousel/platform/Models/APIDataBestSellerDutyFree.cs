using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    

    public class BestSellerRequest
    {
        public string[] skuCode { get; set; }
        public string storeType { get; set; }
    }
    public class Data
    {
        public int count { get; set; }
        public List<Result> result { get; set; }
        public object similar { get; set; }
        public object soldTogether { get; set; }
        public string store { get; set; }
        public string collectionPoint { get; set; }
        public Policy policy { get; set; }
    }

    public class Policy
    {
        public string title { get; set; }
        public string text { get; set; }
        public List<string> lines { get; set; }
    }

    public class Promotion
    {
        public string code { get; set; }
        public int quantity { get; set; }
        public int type { get; set; }
        public string offer { get; set; }
        public string displayText { get; set; }
        public double discountPrice { get; set; }
        public double offerPrice { get; set; }
    }

    public class Result
    {
        public string materialGroup { get; set; }
        public string brand { get; set; }
        public string category { get; set; }
        public string subCategory { get; set; }
        public string subCategoryTitle { get; set; }
        public string categoryTitle { get; set; }
        public string materialGroupTitle { get; set; }
        public string brandTitle { get; set; }
        public string skuCode { get; set; }
        public string skuName { get; set; }
        public string skuDescription { get; set; }
        public string skuSize { get; set; }
        public double price { get; set; }
        public double discountPrice { get; set; }
        public string storeCode { get; set; }
        public string airportStoreCode { get; set; }
        public List<Promotion> promotions { get; set; }
        public List<Specification> specifications { get; set; }
        public List<object> otherDetails { get; set; }
        public string cancellationAndRefundPolicy { get; set; }
        public bool availability { get; set; }
        public string sellableQuantity { get; set; }
        public string productVideo { get; set; }
        public List<string> productImages { get; set; }
        public bool showOnHomepage { get; set; }
        public bool newArrival { get; set; }
        public bool popular { get; set; }
        public string stockInHand { get; set; }
        public string thresholdLimit { get; set; }
        public string thresholdQuantity { get; set; }
        public string skuUnit { get; set; }
        public string buketGroup { get; set; }
        public bool recomended { get; set; }
        public bool travelExclusive { get; set; }
        public int loyaltyPoints { get; set; }
        public string earn2XString { get; set; }
        public string storeType { get; set; }
       
    }

    public class Root
    {
        public bool status { get; set; }
        public Data data { get; set; }
        public object error { get; set; }
        public object warning { get; set; }
    }

    public class Specification
    {
        public string key { get; set; }
        public string value { get; set; }
    }


}