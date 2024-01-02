using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class ResponseData
    {
        public bool status { get; set; }
        public ResultData data { get; set; }
        public Error error { get; set; }
        public Warning warning { get; set; }
    }

    public class Error
    {
        public string statuscode { get; set; }
        public string description { get; set; }
        public string errorCode { get; set; }
        public string source { get; set; }
    }

    public class Warning
    {
        public string code { get; set; }
        public string description { get; set; }
        public string source { get; set; }
    }

    public class ResultData
    {
        public int count { get; set; }
        public List<Object> result { get; set; }
        public List<Object> similar { get; set; }
        public List<Object> buyTogether { get; set; }
    }

    public class ResponseProductData
    {
        public bool status { get; set; }
        public ResultProductData data { get; set; }
        public Error error { get; set; }
        public Warning warning { get; set; }
    }
    public class ResultProductData
    {
        public int count { get; set; }
        public List<ProductMapping> result { get; set; }
        public List<ProductMapping> exclusive { get; set; }
        public List<ProductMapping> similar { get; set; }
        public List<ProductMapping> soldTogether { get; set; }
        public string store { get; set; }
        public string collectionPoint { get; set; }
        public Policy policy { get; set; }
        public ResultProductData()
        {
            exclusive = new List<ProductMapping>();
            similar = new List<ProductMapping>();
            soldTogether = new List<ProductMapping>();
            policy = new Policy();
        }
    }

    public class Policy
    {
        public string title { get; set; }
        public string text { get; set; }
        public List<string> lines { get; set; }
    }
    public class AirportStore
    {
        public string airport { get; set; }
        public string terminal { get; set; }
        public string collectionPoint { get; set; }
        public string store { get; set; }
        public Policy cancellationPolicy { get; set; }

        public AirportStore()
        {
            collectionPoint = "";
            store = "";
            cancellationPolicy = new Policy();
        }
    }

    public class ResponseSitemapData
    {
        public bool status { get; set; }
        public ProductSitemapData data { get; set; }
        public Error error { get; set; }
        public Warning warning { get; set; }
        public ResponseSitemapData()
        {
            status = false;
            data = new ProductSitemapData();
        }
    }

    public class ProductSitemapData
    {
        public int count { get; set; }
        public List<ProductData> result { get; set; }
        public ProductSitemapData()
        {
            count = 0;
            result = new List<ProductData>();
        }
    }
    public class ProductData
    {
        public string materialGroup { get; set; }
        public string materialGroupTitle { get; set; }
        public string brand { get; set; }
        public string brandTitle { get; set; }
        public string category { get; set; }
        public string categoryTitle { get; set; }
        public string subCategory { get; set; }
        public string subCategoryTitle { get; set; }
        public string skuCode { get; set; }
        public string skuName { get; set; }
        public string productName { get; set; } = "";
    }
    public class ProductSearchData : ProductData
    {
        public List<string> productImages { get; set; }
        public Decimal price { get; set; }
        public bool isExclusive { get; set; }
        public bool availability { get; set; }
        public int availableQuantity { get; set; }
        public bool isCombo { get; set; }
        public List<Promotion> promotions { get; set; }
        public ProductSearchData()
        {
            materialGroup = string.Empty;
            materialGroupTitle = string.Empty;
            category = string.Empty;
            categoryTitle = string.Empty;
            subCategory = string.Empty;
            subCategoryTitle = string.Empty;
            brand = string.Empty;
            brandTitle = string.Empty;
            skuCode = string.Empty;
            skuName = string.Empty;
            productName = string.Empty;
            productImages = new List<string>();
            price = 0;
            isExclusive = false;
            availability = false;
            availableQuantity = 0;
            isCombo = false;
            promotions = new List<Promotion>();
        }

    }
    public class APIProductSearchData
    {
        public int count { get; set; }
        public List<ProductSearchData> data { get; set; }

        public APIProductSearchData()
        {
            count = 0;
            data = new List<ProductSearchData>();
        }
    }
}

