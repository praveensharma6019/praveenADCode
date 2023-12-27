using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform
{
    public static class Templates
	{
        public static class ServicesListCollection
        {
            public static readonly ID ServiceItemTemplateID = new ID("{0B2191D6-78AE-4742-AD47-ADC20306DE48}");
            public static string RenderingParamField = "Widget";
        }

        public static class LoyaltyBannerFields
        {
            public static string Title = "Title";
            public static string Image = "Image";
            public static string Link = "Link";
            public static string RewardList = "Rewards List";
            public static string AppImage = "MobileAppImage";

        }

        public static class RewardsFields
        {
            public static string Title = "Title";
            public static string Descriptions = "Description";
            public static string Image = "Standered Image";
            public static string Link = "CTA";
            public static string Active = "Active";
            public static readonly string RewardsList = "RewardsList";
        }

        public static class LoyaltyRewardsJourneyBanner
        {
            public static string Title = "Title";
            public static string Image = "Image";
            public static string SubTitle = "Sub Title";
            public static string RewardList = "Rewards List";
        }

        public static class LoyaltySKU
        {
            public static readonly string SKUCode = "sku_code_t";
            public static readonly string Name = "_name";
            public static readonly string CategoryCode = "categorycode";
            public static readonly string Brand = "brandtitle_s";
            public static readonly string MaterialGroup = "material_group";
            public static readonly string Category = "category_s";
            public static readonly string SubCategory = "sub_category_code_t";
            public static readonly string SKUName = "sku_name_t";
            public static readonly string SubCategoryTitle = "subcategorytitle";
            public static readonly string MaterialGroupTitle = "materialgrouptitle";
            public static readonly string BrandTitle = "brandtitle";
            public static readonly string CategoryTitle = "categorytitle";
            public static readonly string SKUDescription = "sku_description_t";
            public static readonly string SkuSize = "sku_size_t";
            public static readonly string bucketGroup = "bucket_group_s";
            public static string istravelExclusive = "travel_exclusive";
            public static string Policy = "policy";
            public static string CountryOfOrigin = "country_of_origin";
            public static string ProductDescription = "product_description";
            public static string ProductBarcodeNumber = "product_barcode_number_t";
            public static string ManufacturerDetails = "manufacturer_details_t";
            public static readonly string StoreType = "store_type";
            public static readonly string StoreTypeAll = "all";
            public static readonly string productImage_1 = "image_1_t";
            public static readonly string productImage_2 = "image_2_t";
            public static readonly string productImage_3 = "image_3_t";
            public static readonly string productImage_4 = "image_4_t";
            public static readonly string productImage_5 = "image_5_t";
            public static readonly string productImage_6 = "image_6_t";
            public static readonly string earn2X = "earn_two_x"; 
            public static readonly System.Guid ProductFolderID = new System.Guid("{11DFEA97-5FDD-4ECE-9623-1189B9AE91C5}");
            public static readonly System.Guid ProductTemplateId = new System.Guid("{93DC548E-C7FF-42E6-87BF-889D03C5CE42}");
        }

        public static class DictionaryKey
        {
            public static readonly string newUserloyaltyMessage = "newuserloyaltymessage";
        }

        public static class Media
        {
            public static readonly string MediaField = "Media";
            public static readonly string RichText = "RichText"; 
        }
        
    }
}