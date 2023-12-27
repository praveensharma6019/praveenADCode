using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;


namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class FilterProductsWidgets
    {
        public WidgetItem widget { get; set; }
      //  public List<FilterProducts> widgetItems { get; set; }
    }
    /// <summary>
    /// Class to get the Material Groups
    /// </summary>
    public class FilterProducts
    {
        public string title { get; set; }
        public string apiUrl { get; set; }

        public string materialGroup { get; set; }

        public string category { get; set; }

        public string subCategory { get; set; }

        public string brand { get; set; }

        public bool popular { get; set; }

        public bool newArrival { get; set; }

        public bool showOnHomepage { get; set; }
        public string skuCode { get; set; }
        public string storeType { get; set; }
        public bool exclusive { get; set; }      
        public FilterProducts()
        {
            exclusive = false;
            category = string.Empty;
            subCategory = string.Empty;
            brand = string.Empty;
            skuCode = string.Empty;
        }
    }

    public class Root
    {
        public bool status { get; set; }
        public Data data { get; set; }
        public object error { get; set; }
        public object warning { get; set; }
    }

    public class Data
    {
        public int count { get; set; }
        public List<Result> result { get; set; }      
    }

    public class OfferPromotion
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
        public string productName { get; set; }
        public string skuDescription { get; set; }
        public string skuSize { get; set; }
        public double price { get; set; }
        public double discountPrice { get; set; }
        public string storeCode { get; set; }
        public string airportStoreCode { get; set; }
        public List<OfferPromotion> promotions { get; set; }      
        public bool availability { get; set; }
        public int availableQuantity { get; set; } = 0;
        public string sellableQuantity { get; set; }
        public string productVideo { get; set; }
        public List<string> productImages { get; set; }
        public bool showOnHomepage { get; set; }       
        public bool isExclusive { get; set; }
        public bool travelExclusive { get; set; }
        public int loyaltyPoints { get; set; }
        public string earn2XString { get; set; }
        public string storeType { get; set; }
        public string exclusiveImage { get; set; }
    }

    public class FilterRequest
    {
        public string language { get; set; }
        public string airportCode { get; set; }
        public bool showOnHomepage { get; set; }
        public string storeType { get; set; }
        public string[] skuCode { get; set; }
        public string[] category { get; set; }
        public string[] subCategory { get; set; }
        public string[] brand { get; set; }
        public bool restricted { get; set; }
        public bool exclusive { get; set; }
        public string materialGroup { get; set; }
        public string pageType { get; set; }
        public FilterRequest()
        {
            language = "en";
            showOnHomepage = false;
            restricted = false;
            materialGroup = string.Empty;
            pageType = string.Empty;
            skuCode = new List<string>().ToArray();
            category = new List<string>().ToArray();
            subCategory = new List<string>().ToArray();
            brand = new List<string>().ToArray();
            exclusive = false;
        }
    }

    public class ProductTab
    {
        //public string tabTitle { get; set; }
        //public List<Result> products { get; set; }

        public string category { get; set; }
        public List<Result> productDatas { get; set; }
        
        public ProductTab()
        {
            category = "";
            productDatas = new List<Result>();
        }
    }
}