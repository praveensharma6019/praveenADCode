using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Adani.SuperApp.Airport.Foundation.DutyFreeProductImport.Models
{
    public class Product
    {
        [JsonProperty("SR NO")]
        public int SRNO { get; set; }

        [JsonProperty("Material Group")]
        public string MaterialGroup { get; set; }
        public string Category { get; set; }

        [JsonProperty("Sub Category")]
        public string SubCategory { get; set; }
        public string Brand { get; set; }

        [JsonProperty("Flemingo Platform")]
        public string FlemingoPlatform { get; set; }

        [JsonProperty("Flemingo Sku Code")]
        public string FlemingoSkuCode { get; set; }

        [JsonProperty("Sku Name")]
        public string SkuName { get; set; }

        [JsonProperty("Sku Description")]
        public string SkuDescription { get; set; }

        [JsonProperty("Sku Size")]
        public string SkuSize { get; set; }

        [JsonProperty("Sku Type")]
        public string SkuType { get; set; }

        [JsonProperty("Saleable Quantity")]
        public string SaleableQuantity { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Volume { get; set; }

        [JsonProperty("Volume Unit")]
        public string VolumeUnit { get; set; }

        [JsonProperty("Unit Weight")]
        public string UnitWeight { get; set; }

        [JsonProperty("Weight Unit")]
        public string WeightUnit { get; set; }

        [JsonProperty("Inventory Type")]
        public string InventoryType { get; set; }

        [JsonProperty("Supplier Name")]
        public string SupplierName { get; set; }

        [JsonProperty("Threshold Limit")]
        public object ThresholdLimit { get; set; }

        [JsonProperty("Age of Product")]
        public string AgeOfProduct { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Image6 { get; set; }


    }
}