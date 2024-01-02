using System;
using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform
{
    public class Constant
    {
        public static readonly string DutyfreeIndexname = "Adani_SuperApp_Dutyfree_web_index";
        public static string RenderingParamField = "Widget";
        public static readonly Guid ProductFolderID = new Guid("{11DFEA97-5FDD-4ECE-9623-1189B9AE91C5}");
        public static readonly Guid ProductTemplateId = new Guid("{93DC548E-C7FF-42E6-87BF-889D03C5CE42}");

        public static readonly ID HeroCarousalTemplateId = new ID("{0E6437FF-BB3B-4A86-B1CE-D9A3BFA3B684}");

        //""
        public static readonly string ProductItemId = "36585a707c564e23baa74c9650fc35ee";

        public static readonly Guid MaterialGroupFolder = new Guid("{A3349AE2-51BF-401B-BA75-836884B75910}");
        public static readonly Guid MaterialGroupTemplate = new Guid("{F6E33521-E0A2-48D3-8D7C-AC33FC301E5C}");
        public static readonly string Language = "en";

        public static readonly Guid CategoryTemplate = new Guid("{5B0D0497-19EC-44CC-B5AB-9E0245933BEC}");
        public static readonly Guid SubCategoryTemplate = new Guid("{2F8490CE-ABB5-4676-8194-5430203E57F1}");
        public static readonly Guid BrandTemplate = new Guid("{C06138F7-CD65-437F-ADF8-709D96211C59}");

        public static readonly Guid BannerTemplate = new Guid("{F48CCC70-AF9E-4300-AA81-6A3CFD2FD07D}");

        public static readonly Guid CancellationPolicyTemplateID = new Guid("{0A4DB799-BFB9-4BFD-86B8-B6230D78E252}");

        public static readonly Guid CollectionPointFolder = new Guid("{CF65B077-C0D9-4DD0-A0D9-2CEEA2F3BFB2}");

        public static readonly Guid SimilarProductsConfig = new Guid("{170FAD57-4BF8-411D-9BED-4495891104C0}");

        public static readonly string maxSimilarProducts = "maxSimilarProducts";
        public static readonly string maxSimilarProductsField = "name";
        public static readonly string maxSimilarProductsFieldValue = "value";

        public static readonly Guid IntrestedProductsConfig = new Guid("{646D151D-6E71-45D3-ABC7-D779C1BF1831}");
        public static readonly string IntrestedProducts = "InterstedProductForeachCartProducts";
        public static readonly string IntrestedProductsProductsField = "name";
        public static readonly string IntrestedProductsFieldValue = "value";        

        public static readonly string PolicyTitle = "Title";
        public static readonly string PolicyText = "RichText";

        public static readonly string filtertypeProduct = "Product";
        public static readonly string filtertypeMaterialGroup = "MaterialGroup";
        public static readonly string filtertypeCategory = "Category";
        public static readonly string CateroryShowInFilter = "Show in filter";
        public static readonly string CateroryBrands = "Brands";

        public static readonly string Brand_Name = "Brand Name";
        public static readonly string Brand_Code = "Brand Code";
        public static readonly string Brand_MaterialGroup = "Material Group";
        public static readonly string BrandImage = "Image";

        //public static readonly string FiltersSearchFolder = "/sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Filters";
        // public static readonly string  FiltersSearchFolder = "{2F4BBBF2-2823-4593-9709-7F6640D5D83B}";
        public static readonly string FiltersSearchName = "Name";
        public static readonly string FiltersSearchCode = "Code";
        public static readonly string FiltersSearchValue = "Value";
        public static readonly string FilterImage = "Image";

        public static readonly string MaterialGroup_Code = "Material Group Code";
        public static readonly string APICode = "APICode";
        public static readonly string MaterialGroup_Title = "Title";
        public static readonly ID MaterialGroup_Exclusive = new ID("{13F2FCD4-BE64-4C9D-8989-391EA58FA769}");
        public static readonly string TravelExclusive = "travelexclusive";
        public static readonly string BrandBoutique = "brandboutique";
        public static readonly string AdanioneExclusive = "adanioneexclusive|adaniexclusive";
        public static readonly string VirtualMaterialGroup = "combo-products|adani-one-exclusive|adanioneexclusive|brandboutique|brand-boutique|travelexclusive|travel-exclusive";
        public static readonly string TravelTypeDeparture = "3";
        public static readonly string TravelTypeArrival = "2";
        public static readonly string TravelTypeBoth = "1";

        public static readonly string StoreType = "store_type";
        public static readonly string StoreTypeAll = "all";

        public static readonly string DesktopImage = "Desktop Image";
        public static readonly string MobileImage = "Mobile Image";

        //Product By Category

        public static readonly string SitecoreWebIndex = "sitecore_web_index";

        //Product text search
        public static readonly string ProductTextSearch = "productstextsearch_t";

        // SOlr Fields Constant
        public static readonly string MaterialGroupCode = "materialgroupcode_s";
        public static readonly string MaterialGroupTitle = "materialgrouptitle_s";
        public static readonly string CategoryCode = "categorycode_s";
        public static readonly string CategoryTitle = "categorytitle_s";
        public static readonly string SubCategoryCode = "subcategorycode_s";
        public static readonly string SubCategoryTitle = "subcategorytitle_s";
        public static readonly string BrandCode = "brandcode_s";
        public static readonly string BrandTitle = "brandtitle_s";
        public static readonly string ProductImages = "productimages_s";


        public static readonly string MaterialGroup = "material_group";
        public static readonly string Brand = "brand_s";
        public static readonly string Category = "category_s";
        public static readonly string SubCategory = "sub_category_code_t";        
        public static readonly string SKUName = "sku_name_t";
        public static readonly string SKUDescription = "sku_description_t";
        public static readonly string itemid = "itemid";
        public static readonly string SKUPlateform = "sku_plateform_t";
        public static readonly string SKUCode = "skucode_s";
        public static readonly string Active = "active_b";
        public static readonly string SkuSize = "sku_size_t";
        public static readonly string customProductUrl = "producturl";
        public static readonly string fullPath = "_fullpath";
        public static readonly string path = "path";
        public static readonly string Popular = "popular_b";
        public static readonly string NewArrival = "new_arrival_b";
        public static readonly string offers = "offer_sm";
        public static readonly string visibleInFiter = "show_in_filter_b";
        public static readonly string storeCode = "store_code";
        public static readonly string airportstoreCode = "store_airport_code_t";
        public static readonly string ProductName = "product_name_t";
        public static readonly string bucketGroup = "bucketgroup_s";
        public static string isrecomended = "isrecomended_s";
        public static string istravelExclusive = "travelexclusive_s";
        public static readonly string IsActive = "isactive_s";
        public static readonly string ShowOnHomepage = "showonhomepage_s";
        // Field Added to get sold together item
        public static string soldTogether = "sold_together";
        public static string ProductDescription = "product_description";
        public static string Policy = "policy";
        public static string CountryOfOrigin = "country_of_origin";

        public static string ProductBarcodeNumber = "product_barcode_number_t";
        public static string ManufacturerDetails = "manufacturer_details_t";

        public static string ProductHighlights = "product_highlights_t";
        public static string KeyIngredients = "product_ingredients_t";
        public static string ProductBenefits = "product_safety_t";
        public static string ProductSafety = "product_safety_t";
        public static string HowToUse = "product_use_t";
        public static string ProductForm = "product_form_t";
        public static string ProductFlavour = "product_flavour_t";
        public static string FrameColorTemple = "frame_colour_temple_t";
        public static string FrameColorFront = "frame_colour_front_t";
        public static string LensColor = "lens_colour_t";
        public static string AboutBrand = "about_brand_t";
        public static string Material = "material_t";
        public static string MaterialFittingName = "material_fitting_name_t";


        // Solr Field for constant for Material Group
        public static readonly string Title = "title";
        public static readonly string SearchMaterialGroupCode = "material_group_code_t";
        public static readonly string SearchAPICode = "apicode_t";
        public static readonly string Path = "path";
        public static readonly string CDNPath = "cdnpath";
        public static readonly string Link = "link";
        public static readonly string Thumbnail = "thumbnailimage";
        public static readonly string Icon = "iconimages";
        public static readonly string MainImage = "mainimage";


        // solr Fields for Categpry Template

        public static readonly string Name = "_name";
        public static readonly string code = "code_t";
        public static readonly string categoryPath = "_fullpath";
        public static readonly string CategoryCDNPath = "cdnpath_t";
        public static readonly string CategoryLink = "link_t";
        public static readonly string CategoryShowOnHomepage = "show_on_home_page_b";
        public static readonly string CategoryIcon = "iconimages_t";
        public static readonly string CategoryMainImage = "mainimage_t";
        public static readonly string CategoryThumbnail = "thumbnailimage_t";

        //Solr Fields for Brand Template
        public static readonly string SearchBrandName = "brand_name_t";
        public static readonly string SearchBrandCode = "brand_code_t";
        public static readonly string SearchBrandMaterialgroup = "material_group";


        // Search Key

        public static readonly string SearchPageId = "{149E1460-D139-434F-BBE0-F47881DFDF5C}";
        public static readonly string PageSize = "Page size";

        // Fields for Promotion
        public static readonly string PramotionsCode = "Pramotions Code";
        public static readonly string BuyQuantity = "Buy Quantity";
        public static readonly string DisplayText = "Display Text";
        public static readonly string PramotionsType = "Pramotions Type";
        public static readonly string EffectiveFrom = "Effective From";
        public static readonly string EffectiveTo = "Effective To";
               
        // Restricted MAterial Group
        public static readonly string restrictedGroup = "liquor";

        // Item Id for constraint
        public static readonly Guid ConstraintFolderId = new Guid("{26F8D217-FB90-44CE-B293-90FF46EA6A4E}");

        public static readonly Guid ExclusiveOffersFolderId = new Guid("{9408B6E2-0E44-40C8-BBD3-6E688575209A}");

        public static readonly Guid ExclusiveOffersTemplateID = new Guid("{9848AE61-C315-420A-A0C2-F6631A94715E}");

        //Field for Exclusive Offers configurations
        public static readonly Sitecore.Data.ID offerTitle = new ID("{4D2A77A1-E403-45BB-85C1-77FBC2A2C9A5}");
        public static readonly Sitecore.Data.ID offerCodes = new ID("{189FDBFD-EA24-4E75-AD3D-4C8CCD7D4C3D}");
        public static readonly Sitecore.Data.ID offerMaterialGroup = new ID("{6A083794-D568-4196-B4EE-FA70E01378A0}");
        public static readonly Sitecore.Data.ID offerCategory = new ID("{A2495335-F8CF-4858-BF3F-DDAD0DA35301}");
        public static readonly Sitecore.Data.ID offerBrand = new ID("{5426122D-ACEE-4475-AE83-8F2ED9E54BC4}");
        public static readonly Sitecore.Data.ID offerSKUCodes = new ID("{9B3B9C0C-6BB1-4850-AB8B-A703DBEE1A4B}");

        //Field for constraints
        public static readonly string storeType = "Store Type";
        public static readonly string matrialGroup = "Matrial Group";
        public static readonly string moduleType = "Module Type";
        public static readonly string type = "Type";
        public static readonly string value = "Value";
        public static readonly string unit = "Unit";
        public static readonly string errorMessage = "Error Message";
        public static readonly string active = "Active";

        //Fields for banner

        public static readonly string BannerTitle = "Title";
        public static readonly string BannerDescription = "Description";
        public static readonly string BannerCTA = "Link";
        public static readonly string CTA = "CTA";
        public static readonly string BannerSubTitle = "Sub Title";
        public static readonly string BannerAppImage = "Mobile Image";
        public static readonly string BannerDeskTopImage = "Desktop Image";
        public static readonly string descriptionApp = "DescriptionApp";
        public static readonly string BannerMobileImage = "MobileImage";
        public static readonly string BannerWebImage = "WebImage";
        public static readonly string BannerStanderedImage = "Standered Image";
        public static readonly string BannerDeepLink = "Deep Link";
        public static readonly string RestrictedCarousal = "liquor";
        public static readonly string BannerCondition = "Banner Condition";
        public static readonly Sitecore.Data.ID SKUCodeID = new ID("{C0578363-ABFF-4066-A307-4AE2004C0505}");
        public static string UniqueId = "UniqueId";
        public static readonly string BannerCtaurl = "ctaUrl";
        public static readonly string BannerPromoCode = "PromoCode";
        public static readonly string BannerMaterialGroup = "Material Group";
        public static readonly string BannerCategory = "Category";
        public static readonly string BannerSubCategory = "Sub Category";
        public static readonly string BannerBrand = "Brand";
        public static readonly string BannerStoreType = "Store Type";

        //GTMTags Constants
        public static readonly string bannerCategory = "BannerCategory";
        public static readonly string businessUnit = "BusinessUnit";
        public static readonly string faqCategory = "FaqCategory";
        public static readonly string label = "Label";
        public static readonly string source = "Source";
        public static readonly string subCategory = "SubCategory";
        public static readonly string GTMtype = "Type";
        public static readonly string eventName = "Event";
        public static readonly string category = "{68531E40-5B4D-461B-A0C6-C0F3F3377B94}";

        public static readonly string FreeOfferText = "{B32933DB-14D2-4470-A7C2-C847CA697C1D}";
    }
    public class BrandListConstant
    {
        public static readonly string BrandName = "Brand Name";
        public static readonly string BrandCDNImage = "Brand CDN Image";

        public static readonly string BrandDescription = "Brand Description";
        public static readonly string BrandCode = "Brand Code";
        public static readonly string Image = "Image";
        public static readonly string Visibleonbrandyoulove = "Visible on brand you love";
        public static readonly string BrandRestricted = "Brand Restricted";

        public static readonly string BrandMaterialGroup = "Material Group";
        public static readonly string BrandDeparture = "Departure";
        public static readonly string BrandArrival = "Arrival";
        public static readonly string AirportCode = "Airport Code";

        public static readonly string StoreType = "Store Type";
        public static readonly string Restricted = "Restricted";
        public static readonly string BrandsList = "Brands";

        public static readonly string DisableForAirports = "Disable for Airports";
    }

    public class FilterConstant
    {
        public static readonly string MaterialGroup = "Material Group";
        public static readonly string Category = "Category";
        public static readonly string SubCategory = "Sub Category";
        public static readonly string Brand = "Brand";
        public static readonly string SKUCode = "SKU Codes";
        public static readonly string SKUCodeArrival = "SKU Codes Arrival";
        public static readonly string StoreType = "Store Type";

        public static readonly string Popular = "Popular";
        public static readonly string ShowOnHomepage = "Show on Home Page";
        public static readonly string NewArrival = "New Arrival";

        public static readonly string apiUrl = "API URL Controller";

        public static readonly string displayName = "Display Name";

        public static readonly string ExclusiveProducts = "ExclusiveProducts";
    }
    /// <summary>
    /// ID for Offer section
    /// </summary>
    public class offerFilterContent
    {
        // ID Stored for Promotions and offers 
        public static readonly ID OfferListFolderID = new ID("{5B4049D7-8A89-49C8-8AEB-61C691930B4C}");
        public static readonly ID PromoOfferTemplateID = new ID("{68EEE3B2-4980-41FE-AB4E-7590B322FAE4}");
        public static readonly string PromotionOfferSolrField = "promotionoffer_sm";
        public static readonly string PromotionCodeSolrField = "offerpromocode_s";
        public static readonly string PromotionOfferTitleSolrField = "title_t";
        public static readonly string PromotionOfferPromotionTypeSolrField = "promotion_type_t";
        public static readonly string PromotionOfferCodeSolrField = "promotion_code_t";
        public static readonly string PromotionOfferSKUCodeSolrField = "skucode_t";
        public static readonly string PromotionOfferTypeSolrField = "offer_type_s";
        public static readonly string PromotionOfferDisplayTextSolrField = "display_text_t";
        public static readonly string PromotionOfferEffectiveFromSolrField = "effective_from_tdt";
        public static readonly string PromotionOfferEffectiveToSolrField = "effective_to_tdt";
        public static readonly string PromotionOfferTerminalLocationTypeSolrField = "locationstoretype_s";
        public static readonly string PromotionOfferOfferdescriptionSolrField = "offer_description_t";
        public static readonly string PromotionOfferTerminalStoreTypeSolrField = "offeroperatortype_s";
        public static readonly string PromotionOfferActiveSolrField = "active_b";
        public static readonly string PromotionOfferDetailTitleSolrField = "offer_title_t";
        public static readonly string PromotionOfferpcmClaimTypeSolrField = "pcmclaimtype_t";

        public static readonly string PromotionOfferShowonHomepageSolrField = "show_on_homepage_b";
        public static readonly string PromotionOfferTermsandConditionSolrField = "terms_and_condition_t";
        public static readonly string PromotionOfferExpiryOptionSolrField = "expiry_option_t";
        public static readonly string PromotionOfferValidationTypeSolrField = "validation_type_t";

        public static readonly string PromotionOfferDesktopImageSolrField = "offerdesktopimageurl_s";
        public static readonly string PromotionOfferMobileImageSolrField = "offermobileimageurl_s";
        public static readonly string PromotionOfferThumbnailImageSolrField = "offerthumbnailimageurl_s";
        public static readonly string PromotionOfferExtraImageSolrField = "offerextraimageurl_s";
        public static readonly string PromotionOfferRankSolrField = "display_rank_t";

        public static readonly string PromotionOfferLinkTextSolrField = "offerlinktext_s";
        public static readonly string PromotionOfferLinkUrlSolrField = "offerlinkurl_s";
        public static readonly string PromotionOfferLocationTypeSolrField = "locationcode_s";
        public static readonly string PromotionOfferAppTypeSolrField = "app_specific_offer_s";


        public static readonly string PromotionOfferTCLinkTextSolrField = "offertclinktext_s";
        public static readonly string PromotionOfferTCLinkUrlSolrField = "offertclinkurl_s";

        public static readonly string PromotionOfferBannerImageDeskSolrField = "offerbannerimagedesksrc_s";
        public static readonly string PromotionOfferBannerImageMobSolrField = "offerbannerimagemobsrc_s";
        public static readonly string PromotionOfferCategoryFilterSolrField = "category_filter_t";
        public static readonly string PromotionOfferUniqueIDFilterSolrField = "offeruniqueid_t";
        public static readonly string PromotionOfferUniqueIDFilterFieldSolrField = "offeruniquesolrid_s";
        public static readonly string PromotionOfferPromotionTypeLabelFieldSolrField = "promotion_type_label_t";
        public static readonly string PromotionOfferSavingsFieldSolrField = "savings_t";

        public static readonly string PromotionOfferGroupIDFilterSolrField = "_group";
        public static readonly string AutoIdSolrFiled = "autoid_t";
        public static readonly string PromotionOfferTabTitleSolrField = "tab_title_t";
        public static readonly string PromotionOfferTCEnableSolrField = "tc_enabled_b";
        //Ticket No 23293
        public static readonly string PromotionOfferTabSubTitleSolrField = "tab_sub_title_t";

        public static readonly string PromotionOfferisBankOfferSolrField = "isbankoffer_b";
        public static readonly string PromotionOfferLOBSolrField = "loboffer_s";

        public static readonly string PromotionOfferRedirectionTextOfferSolrField = "offerredirectiontext_s";
        public static readonly string PromotionOfferRedirectionURLOfferSolrField = "offerredirectionurl_s";
        public static readonly string PromotionOfferisExlusiveOfferSolrField = "isexclusiveoffer_b";
        public static readonly string PromotionOfferdisplayOnPageOfferSolrField = "offerdisplayonpage_s";
        public static readonly string PromotionOfferBankOfferTextSolrField = "bankoffertext_t";

        // Field used for filter perpose
        public static string ApptypeFilter = "web";
        public static string Apptype = "app_b";
        public static string Webtype = "web_b";


        // Ticket No 17128
        public static readonly string BookingConfirmedOfferText = "bookingconfirmedoffertext_t";
        public static readonly string BookingConfirmedOfferDescription = "bookingconfirmedofferdescription_t";
        public static readonly string HowToUse = "howtouse_t";
        public static readonly string OfferLogoDesktop = "offerlogodesktopimage_s";
        public static readonly string OfferLogoMobile = "offerlogomobileimage_s";
        public static readonly string UnlockOfferCTAText = "unlockofferctalinktext_s";
        public static readonly string UnlockOfferCTALink = "unlockofferctalinkurl_s";
        public static readonly string UnlockOfferCTAVisitWesiteText = "unlockoffervisitwebsitectalinktext_s";
        public static readonly string UnlockOfferCTAVisitWesiteLink = "unlockoffervisitwebsitectalinkurl_s";
        public static readonly string IsUnlockOffer = "isunlockedoffer_b";
        //Ticket No 19493 
        public static readonly string unlockOfferTitle = "unlockoffertitle_t";


    }

    public class SearchConstant
    {
        public static readonly Sitecore.Data.ID PopularBrandItem = new ID("{FF996294-04AD-443C-91C7-553639038F77}");
        public static readonly Sitecore.Data.ID PopularBrand = new ID("{09357122-898A-4295-B7CA-4C1A77BC257C}");
        public static readonly Sitecore.Data.ID BrandName = new ID("{4F14A6A6-2930-48A0-B81E-F5D61184B4B6}");
        public static readonly Sitecore.Data.ID BrandCode = new ID("{44672CAC-E7AA-480A-9E91-383034C4D3D8}");

        public static readonly Sitecore.Data.ID PopularSearchFolder = new ID("{6B49F65F-2190-459F-A562-453AC1596EA7}");
        public static readonly Sitecore.Data.ID PopularSearchTemplate = new ID("{09058820-CA3B-43E5-9E63-F2807F78D171}");

        public static readonly Sitecore.Data.ID PopularSearchTitle = new ID("{4C9BDED2-E74B-4F57-880E-0D135B5CBBAB}");
        public static readonly Sitecore.Data.ID PopularSearchMaterialGroup = new ID("{34C52F1C-9C52-4DB7-A007-6DB8F2A904A2}");
        public static readonly Sitecore.Data.ID PopularSearchCategories = new ID("{FA02165E-5FF1-4416-BA27-8C9730263F59}");
        public static readonly Sitecore.Data.ID PopularSearchSubcategories = new ID("{4BA47175-FC79-4771-BC81-3BDB69138CA9}");
        public static readonly Sitecore.Data.ID PopularSearchBrands = new ID("{5085E952-97C4-4DEB-856D-C2D3EC130A42}");
        public static readonly Sitecore.Data.ID PopularSearchSKUCode = new ID("{23042DFE-7BC1-4796-B2A5-8677A6DE5802}");

        public static readonly Sitecore.Data.ID SearchFiltersFolder = new ID("{A5A3BCEC-CFF2-4C53-B46F-2EBEE019416A}");


        public static readonly Sitecore.Data.ID SearchConfigurationFolder = new ID("{40B6A555-2670-48CA-89AB-4453B60573A5}");

        public static readonly Sitecore.Data.ID Name = new ID("{EFC8ACFC-F8CD-477B-A8CF-9BACB78B22CD}");
        public static readonly Sitecore.Data.ID Code = new ID("{40D8C2C1-D3FD-4BE1-89BD-BE9B7C04785E}");
        public static readonly Sitecore.Data.ID value = new ID("{BBB6CBB0-BCB7-4536-864C-1B7A4E09E7F8}");
        public static readonly Sitecore.Data.ID Image = new ID("{5F7308D0-0C63-4556-84F5-3B97FF2559EF}");
    }
}