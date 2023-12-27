using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models
{
    public class ProductMapping //: SearchResultItem
    {
        //[IndexField("material_group_s")]
        public string materialGroup { get; set; }        
        // [IndexField("brand_s")]
        public string brand { get; set; }        
        //[IndexField("category_s")]
        public string category { get; set; }        
        //[IndexField("sub_category_t")]
        public string subCategory { get; set; }
        public string subCategoryTitle { get; set; }
        public string categoryTitle { get; set; }
        public string materialGroupTitle { get; set; }
        public string brandTitle { get; set; }
        //[IndexField("sku_name_t")]
        public string skuCode { get; set; }
        public string skuName { get; set; }
        
        //[IndexField("sku_description_t")]
        public string skuDescription { get; set; }
        //[IndexField("itemid")]
       // public string itemID { get; set; } = "";   

        //[IndexField("sku_plateform_t")]
       // public string skuPlateform { get; set; } = "";

      //  public bool active { get; set; }

        public string skuSize { get; set; } = "";

       // public string productUrl { get; set; } = "";

        public decimal price { get; set; } = 0;

        public decimal discountPrice { get; set; } = 0 ;

       // public decimal discountAmount { get; set; } = 0;

        public string storeCode { get; set; } = "";

        public string airportStoreCode { get; set; } = "";

        public List<Promotion> promotions { get; set; }      
       
        public List<OtherDetails> specifications { get; set; }
        public List<OtherDetails> otherDetails { get; set; }

        public string cancellationAndRefundPolicy { get; set; } = "";        
       
        public bool availability { get; set; } = true;
        public string sellableQuantity { get; set; } = "";

        public string productVideo { get; set; } = "";
        public List<string> productImages { get; set; }

        //Configs
        //[IndexField("visible_in_carousel_b")]
        public bool showOnHomepage { get; set; } = false;
        public bool newArrival { get; set; } = false;
        public bool popular { get; set; } = false;
        public string stockInHand { get; set; } = "";
        public string thresholdLimit { get; set; } = "";
        public string thresholdQuantity { get; set; } = "";

        public string skuUnit { get; set; }
        public string buketGroup { get; set; }
        public bool recomended { get; set; }

        public bool travelExclusive { get; set; }
        // Property added for loyalty 
        public int loyaltyPoints { get; set; } = 0;
        public string storeType { get; set; }

        public string Earn2XString { get; set; }
        public int availableQuantity { get; set; }

    }
    public class OtherDetails
    {
        public string key { get; set; }
        public string value { get; set; }
    }
    public class ProductSpecifications
    {
        public string productName { get; set; } = "";
        public string productDescription { get; set; } = "";
        public string productLength { get; set; } = "";
        public string productWidth { get; set; } = "";
        public string productHeight { get; set; } = "";
        public string productVolume { get; set; } = "";
        public string productVolumeUnit { get; set; } = "";
        public string productWeight { get; set; } = "";
        public string productWeightUnit { get; set; } = "";
        public string productGender { get; set; } = "";
        public string productLiquorAge { get; set; } = "";
        public string productLiquorAlchohol { get; set; } = "";
        public string productBarcodeNumber { get; set; } = "";
        public string productBarcodeImage { get; set; } = "";
        public string manufacturerDetails { get; set; } = "";
        public string countryOfOrigin { get; set; } = "";
    }

    public class Promotion
    {
        public string code { get; set; } = "";
        public int quantity { get; set; } = 0;
        public int type { get; set; } = 0;
        public string offer { get; set; } = "";
        public string displayText { get; set; } = "";
        //public string effectiveFrom { get; set; } = "";
       // public string effectiveTo { get; set; } = "";
        public decimal discountPrice { get; set; } = 0;
        public decimal offerPrice { get; set; } = 0;
    }
}