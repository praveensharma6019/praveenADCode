using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform
{
    public class Constant
    {
        public static readonly ID templateId = new ID("{93DC548E-C7FF-42E6-87BF-889D03C5CE42}");
        

        public static readonly ID productParentItemId = new ID("{11DFEA97-5FDD-4ECE-9623-1189B9AE91C5}");

        public static readonly ID BrandtemplateId = new ID("{C06138F7-CD65-437F-ADF8-709D96211C59}");

        public static readonly ID BrandParentItemId = new ID("{04E90FDC-E682-4083-8A64-BE2F92491C25}");

        public static readonly ID CategoryTemplateId = new ID("{5B0D0497-19EC-44CC-B5AB-9E0245933BEC}");

        public static readonly ID SubCategoryTemplateId = new ID("{2F8490CE-ABB5-4676-8194-5430203E57F1}");

        // ID Stored for Promotions and offers 
        public static readonly ID OfferListFolderID = new ID("{5B4049D7-8A89-49C8-8AEB-61C691930B4C}");

        public static readonly ID PromoOfferTemplateID = new ID("{68EEE3B2-4980-41FE-AB4E-7590B322FAE4}");

        public static readonly ID FolderTemplateId = new ID("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");

        public static readonly ID MediaFolderTemplateId = new ID("{FE5DD826-48C6-436D-B87A-7C4210C7413B}");

        public static readonly string master = "master";
        public static readonly string web = "web";

        #region  Product Fields 

        public static readonly string SKUID = "SKU Id";
        public static readonly string SKUName = "SKU Name";
        public static readonly string SKUDescription = "SKU Description";
        public static readonly string SKUCode = "SKU Code";
        public static readonly string SKUPlateform = "SKU Plateform";
        public static readonly string SKUSize = "SKU Size";
        public static readonly string SKUType = "SKU Type";
        public static readonly string MaterialGroup = "Material Group";
        public static readonly string Category = "Category";
        public static readonly string SubCategory = "Sub Category Code";
        public static readonly string Brand = "Brand";
        public static readonly string SellableQuantity = "Sellable Quantity";
        public static readonly string Length = "Length";
        public static readonly string Width = "Width";
        public static readonly string Height = "Height";
        public static readonly string Volume = "Volume";
        public static readonly string VolumeUnit = "Volume Unit";
        public static readonly string Weight = "Weight";
        public static readonly string WeightUnit = "Weight Unit";
        public static readonly string Gender = "Gender";
        public static readonly string Age = "Age";
        public static readonly string Inventorytype = "Inventory type";
        public static readonly string ThresholdQuantity = "Threshold Quantity";
        public static readonly string SupplierName = "Supplier Name";
        public static readonly string Image1 = "Image 1";
        public static readonly string Image2 = "Image 2";
        public static readonly string Image3 = "Image 3";
        public static readonly string Image4 = "Image 4";
        public static readonly string Image5 = "Image 5";
        public static readonly string Image6 = "Image 6";
        public static readonly string Active = "Active";
        public static readonly string Price = "Price";
        public static readonly string Storecode = "Store Code";
        public static readonly string Storeairportcode = "Store Airport Code";
        public static readonly string Offer = "Offer";
        public static readonly string isRecomended = "IsRecomended";
        public static readonly string travelExclusive = "Travel Exclusive";
        public static readonly string bucketGroup = "Bucket Group";
        public static readonly string SoldTogether = "Sold Together";
        public static readonly string ProductName = "Product Name";

        #endregion

        #region Brand Fields

        public static readonly string BrandCode = "Brand Code";
        public static readonly string BrandName = "Brand Name";
        public static readonly string BrandMaterialGroup = "Material Group";
        public static readonly string BrandRestricted = "Brand Restricted";
        #endregion

        #region Product Offers

        public static readonly ID OffertemplateId = new ID("{ABEA7A9C-8827-4B28-9792-D70E2406A728}");
        public static readonly ID OfferParentItemId = new ID("{E1337B98-22CB-4E08-95C0-A206EA8DAF36}");
        public static readonly string OfferCode = "Offer Code";
        public static readonly string OfferTitle = "Title";
        public static readonly string OfferMaterialGroup = "Material Group";
        public static readonly string OfferCategory = "Category";
        public static readonly string OfferSubCategory = "Sub Category";
        public static readonly string OfferBrand = "Brand";

        #endregion

        #region SubCategoryCode Fields

        public static readonly string SubCategoryCode = "Code";
        public static readonly string SubCategoryName = "Name";
        public static readonly string BrandsFieldName = "Brands";


        #endregion

        #region Bucket Fields
        public static readonly ID BucketFolderTemplateId = new ID("{3D34C235-B83C-4AA5-8E4A-BA1B941D959E}");
        public static readonly ID BucketTemplateId = new ID("{804E0028-1AD8-46A6-844B-8AFAC04D04B4}");
        public static readonly string BucketFieldName = "Title";
        #endregion

        public class PromoOffers
        {
            // Field Name for Promotion , Offer and discount section
            public static readonly string Title = "Title";
            public static readonly string PromotionType = "Promotion Type";
            public static readonly string PromotionCode = "Promotion Code";
            public static readonly string PromotionDescription = "Offer description";
            //public static readonly string OfferSKUCode = "SKUCode";
            public static readonly string OfferType = "Offer Type";
            // public static readonly string OfferValue = "Offer Value";
            public static readonly string DisplayText = "Display Text";
            public static readonly string EffectiveFrom = "Effective From";
            public static readonly string EffectiveTo = "Effective To";
            public static readonly string TerminalLocationType = "Terminal Location Type";
            public static readonly string TerminalStoreType = "Terminal Store Type";
            public static readonly string Active = "Active";
            public static readonly string OfferId = "OfferId";
            public static readonly string DesktopImage = "Desktop Image";
            public static readonly string OfferMobileImage = "Mobile Image";
            public static readonly string OfferThumbnailImage = "Thumbnail Image";
            public static readonly string OfferImage = "Image";
            public static readonly string DeepLink = "Link";
            public static readonly string Currency = "Currency";
            public static readonly string PromotionClaimType = "pcmClaimType";
            public static readonly string ShowonHomepage = "Show on Homepage";
            public static readonly string TermsandCondition = "Terms and Condition";
            public static readonly string ExpiryOption = "Expiry Option";
            public static readonly string ValidationType = "Validation Type";
            public static readonly string DisplayRank = "Display Rank";
            public static readonly string promoDesktopImage = "Desktop Image";
            public static readonly string promoMobileImage = "Mobile Image";
            public static readonly string promoThumbnailImage = "Thumbnail Image";
        }

        
    }
}