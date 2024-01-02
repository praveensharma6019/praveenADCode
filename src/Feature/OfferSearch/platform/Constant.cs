using System;
using System.Web.Http.Results;
using Sitecore.Data;
using Sitecore.Publishing.Pipelines.PublishItem;
using Sitecore.Shell.Framework.Commands;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform
{
    public class Constant
    {
        public static readonly Guid ProductFolderID = new Guid("{11DFEA97-5FDD-4ECE-9623-1189B9AE91C5}");
        public static readonly Guid ProductTemplateId = new Guid("{93DC548E-C7FF-42E6-87BF-889D03C5CE42}");
        public static readonly ID AirportsCityFolderId = new ID("{8A7E30D7-2635-4F5F-9797-69FA0448C63D}");
        public static readonly ID AirlinesFolderId = new ID("{7A3F2197-0DB0-4580-A660-D22A6E63D23C}");
        public static readonly ID AirportsCityTemplateId = new ID("{D71EDD08-B897-45BB-A1FD-F90666A7F0CE}");
        public static readonly ID AirlineTemplateId = new ID("{6D83803C-31EB-4D22-AF6F-50A226E6D869}");
        public static readonly ID AirportTemplateId = new ID("{486FBEBB-93F6-4E9D-859A-357010D47503}");
        public static readonly ID KeywordsFolderTemplateId = new ID("{8F4F2A26-49C5-4E2F-9AFB-A4652C015BA2}");
        public static readonly ID KeywordsTemplateID = new ID("{64E55D8A-5A56-40EB-9672-4A7F7384D343}");
        public static readonly ID OfferListFolderID = new ID("{5B4049D7-8A89-49C8-8AEB-61C691930B4C}");
        public const string AirlineSearchIndex = "Adani_SuperApp_FlightSearch_Master_Index";

        //Airlines Changes
        public static string citytocity_domestic_masterindex = "Adani_SuperApp_DomesticFlights_web_index";
        public static string citytocity_domestic_webindex = "Adani_SuperApp_DomesticFlights_web_index";
        public static string citytocity_international_masterindex = "Adani_SuperApp_InternationalFlights_web_index";
        public static string citytocity_international_webindex = "Adani_SuperApp_InternationalFlights_web_index";
        public static string CitytoCItyTemplateID = "{F30C68F5-3783-4F1E-BDCB-C6538BB2D9AA}";
        public static string LowestFairEndPoint = "LowestFairSeviceAPI";
        public static string TraceID = "TraceID";
        public static string ChannelID = "ChannelID";
        public static string AgentID = "AgentID";
        public static readonly string ContentType = "text/plain";
        public static readonly string RelativeAddress = "relativeAddress";

        //""
        public static readonly string ProductItemId = "36585a707c564e23baa74c9650fc35ee";

        public static readonly Guid MaterialGroupFolder = new Guid("{A3349AE2-51BF-401B-BA75-836884B75910}");
        public static readonly Guid MaterialGroupTemplate = new Guid("{F6E33521-E0A2-48D3-8D7C-AC33FC301E5C}");
        public static readonly string Language = "en";

        public static readonly Guid CategoryTemplate = new Guid("{5B0D0497-19EC-44CC-B5AB-9E0245933BEC}");
        public static readonly Guid SubCategoryTemplate = new Guid("{2F8490CE-ABB5-4676-8194-5430203E57F1}");
        public static readonly Guid BrandTemplate = new Guid("{C06138F7-CD65-437F-ADF8-709D96211C59}");

        public static readonly Guid BannerTemplate = new Guid("{F48CCC70-AF9E-4300-AA81-6A3CFD2FD07D}");

        public static readonly Guid CancellationPolicy = new Guid("{0FA568F7-3F8D-4C26-8A7D-C82B1D8862FB}");            

        public static readonly Guid CollectionPointFolder = new Guid("{CF65B077-C0D9-4DD0-A0D9-2CEEA2F3BFB2}");

        public static readonly Guid SimilarProductsConfig = new Guid("{170FAD57-4BF8-411D-9BED-4495891104C0}");

        public static readonly string maxSimilarProducts = "maxSimilarProducts";
        public static readonly string maxSimilarProductsField = "name";
        public static readonly string maxSimilarProductsFieldValue = "value";

        public static readonly Guid IntrestedProductsConfig = new Guid("{646D151D-6E71-45D3-ABC7-D779C1BF1831}");
        public static readonly string IntrestedProducts = "InterstedProductForeachCartProducts";
        public static readonly string IntrestedProductsProductsField = "name";
        public static readonly string IntrestedProductsFieldValue = "value";

        public static readonly string BrandBoutique = "brandboutique";

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

        public static readonly string TravelExclusive = "travelExclusive";

        public static readonly string TravelTypeDeparture = "3";
        public static readonly string TravelTypeArrival = "2";
        public static readonly string TravelTypeBoth = "1";
        
        public static readonly string StoreType = "store_type";
        public static readonly string StoreTypeAll = "all";

        public static readonly string DesktopImage = "Desktop Image";
        public static readonly string MobileImage = "Mobile Image";

        //Product By Category

        public static readonly string SitecoreWebIndex = "sitecore_web_index";



        // SOlr Fields Constant
        public static readonly string MaterialGroupCode = "materialgroupcode_s";
        public static readonly string MaterialGroupTitle = "materialgrouptitle_s";
        public static readonly string CategoryCode = "categorycode_s";
        public static readonly string CategoryTitle = "categorytitle_s";
        public static readonly string SubCategoryCode = "subcategorycode_s";
        public static readonly string SubCategoryTitle = "subcategorytitle_s";
        public static readonly string BrandCode = "brandcode_s";
        public static readonly string BrandTitle = "brandtitle_s";

        public static readonly string MaterialGroup = "material_group";
        public static readonly string Brand = "brand_s";
        public static readonly string Category = "category_s";
        public static readonly string SubCategory = "sub_category_code_t";
        public static readonly string ShowOnHomepage = "show_on_home_page_b";
        public static readonly string SKUName = "sku_name_t";
        public static readonly string SKUDescription = "sku_description_t";
        public static readonly string itemid = "itemid";
        public static readonly string SKUPlateform = "sku_plateform_t";
        public static readonly string SKUCode = "sku_code_t";
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
        public static readonly string productImage_1 = "image_1_t";
        public static readonly string productImage_2 = "image_2_t";
        public static readonly string productImage_3 = "image_3_t";
        public static readonly string productImage_4 = "image_4_t";
        public static readonly string productImage_5 = "image_5_t";
        public static readonly string productImage_6 = "image_6_t";
       // public static readonly string group = "group";
        public static readonly string bucketGroup = "bucket_group_s";
        public static string isrecomended = "isrecomended";
        public static string istravelExclusive = "travel_exclusive";
        // Field Added to get sold together item
        public static string soldTogether = "sold_together";
        public static string ProductDescription = "product_description";
        public static string Policy = "policy";
        public static string CountryOfOrigin = "country_of_origin";

        public static string ProductBarcodeNumber = "product_barcode_number_t";
        public static string ManufacturerDetails = "manufacturer_details_t";

        // Solr Field for constant for Material Group
        public static readonly string Title = "title";
        public static readonly string SerarchMaterialGroupCode = "material_group_code_t";
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

        // ID for different States
        public static readonly ID Gujrat =new ID("{C11815AD-A409-4D4B-A4BA-FFC90F68B67A}");
        public static readonly ID Ahmedabad =new ID("{7CA153E1-4357-450C-A9C6-49251FC75463}");
        public static readonly ID Guwahati = new ID("{A51C1C4A-9FD1-40C9-9165-2675E0BAAFC9}");
        public static readonly ID Jaipur = new ID("{5ED1E61A-BD55-41A8-9FD2-E9CDFB39E19A}");
        public static readonly ID Lucknow =new ID("{F1EFA9E7-8F0B-4414-86D2-C75EA11978FD}");
        public static readonly ID Mangaluru =new ID("{6E409676-C4A7-4B98-8E3D-264CFB05869C}");
        public static readonly ID Mumbai = new ID("{C4325022-13F9-4830-A2F8-A28307F4C0D4}");
        public static readonly ID Thiruvananthapuram =new ID("{A22B09A6-3909-42CA-BAAF-C7A37E9B95F0}");
        public static readonly string adaniOne = "{9034816F-E9CD-4888-800A-049EB877A5A6}";

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
    }

    public class FilterConstant
    {
        public static readonly string MaterialGroup = "Material Group";
        public static readonly string Category = "Category";
        public static readonly string SubCategory = "Sub Category";
        public static readonly string Brand = "Brand";
        public static readonly string SKUCode = "SKU Codes";

        public static readonly string Popular = "Popular";
        public static readonly string ShowOnHomepage = "Show on Home Page";
        public static readonly string NewArrival = "New Arrival";

        public static readonly string apiUrl = "API URL Controller";

        public static readonly string displayName = "Display Name";
    }
    /// <summary>
    /// ID for Offer section
    /// </summary>
    public class offerFilterContent
    {
        // ID Stored for Promotions and offers 
        public static readonly ID OfferListFolderID = new ID("{5B4049D7-8A89-49C8-8AEB-61C691930B4C}");
        public static readonly ID RewardsFolderID = new ID("{34402E55-8BA4-4A65-8FA4-045A2BB82EC6}");
        //Ticket No 17609
        public static readonly ID NewOfferListFolderID = new ID("{621D32DF-C285-4B79-B2D5-B6764C577183}");
        public static readonly ID PromoOfferTemplateID = new ID("{68EEE3B2-4980-41FE-AB4E-7590B322FAE4}");
        public static readonly ID CartOfferTemplateID = new ID("{3F15B412-3F0D-475F-BAAE-FFC7B6F57D3F}");
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
        //public static readonly string PromotionOfferTerminalLocationTypeSolrField = "airportnamelist_s";
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
        public static readonly string PromotionOfferTabTitleSolrField = "tab_title_s";
        public static readonly string PromotionOfferTCEnableSolrField = "tc_enabled_b";

        public static readonly string PromotionOfferisBankOfferSolrField = "isbankoffer_b";
        public static readonly string NotClickableSolrField = "notclickable_b";
        public static readonly string PromotionOfferLOBSolrField = "loboffer_s";
        public static readonly string PromotionOfferDiscountPriceSolrField = "discountprice_tf";
        public static readonly string PromotionOfferDiscountPercentSolrField = "discountpercent_tl";

        public static readonly string PromotionOfferRedirectionTextOfferSolrField = "offerredirectiontext_s";
        public static readonly string PromotionOfferRedirectionURLOfferSolrField = "offerredirectionurl_s";
        public static readonly string PromotionOfferisExlusiveOfferSolrField = "isexclusiveoffer_b";
        public static readonly string PromotionOfferdisplayOnPageOfferSolrField = "offerdisplayonpage_s";
        public static readonly string PromotionOfferBankOfferTextSolrField = "bankoffertext_t";
        //Ticket 17609
        public static readonly string Globaltermsandcondition = "globaltermscondition_t";
        public static readonly string SeoFriendlyURL = "seofriendlypath_s";


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

        //Ticket No 23293
        public static readonly string PromotionOfferTabSubTitleSolrField = "tab_sub_title_t";

        //Ticket No 29979
        public static readonly string OfferServicesRedirectionLinkSolrField = "offerservicesredirectionlink_s";
        public static readonly string OfferIsInternationalSolrField = "isinternational_b";
        public static readonly string OfferFullScreenImage = "fullscreenimage_s";
        public static readonly string IsInstoreOfferSolrField = "isinstoreoffer_b";
        public static readonly string SimilarOffersSolrField = "similaroffersdata_t";
        public static readonly string TNCViewAllLink = "tcviewallurl_s";
        //Ticket No 37910
        public static readonly string metaTitleSolrField = "metatitle_t";
        public static readonly string metaDescriptionSolrField = "metadescription_t";
        public static readonly string keywordsSolrField = "keywords_t";
        public static readonly string breadcrumbTitle = "breadcrumbtitle_t";
        public static readonly string canonicalSolrField = "canonicallink_s";

        public static readonly string terminalGateSolrField = "terminalgate_t";
        public static readonly string terminalCodeSolrField = "terminalcode_t";
        public static readonly string shopIdSolrField = "shopid_t";
    }

    public class RewardsConstant
    {
        public static readonly string Title = "Title";
        public static readonly string PromotionCode = "Promotion Code";
        public static readonly string DisplayText = "Display Text";
        public static readonly string Description = "Offer description";
        public static readonly string DisplayRank = "Display Rank";
        public static readonly string PromotionType = "Promotion Type";
    }
    public class AirportFieldConstant
    {
        // ID for Airport Section
        public static readonly string Title = "{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}";
        public static readonly string Description = "{ED2534C4-B080-48A0-A82D-4CA2EE96A935}";
        public static readonly string DescriptionApp = "{5D7F2597-E8C0-473A-9471-2E85D53D32C7}";
        public static readonly string CTALink = "{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}";
        public static readonly string StanderedImage = "{DB69B6E3-4E77-43F5-BCD4-3BE6E81C017B}";
        public static readonly string Image = "{5F7308D0-0C63-4556-84F5-3B97FF2559EF}";

    }
    public class AirlinesConstant
    {
        public static readonly string AirlineCode = "airlinecode_t";
        public static readonly string AirlineName = "airlinename_t";
        public static readonly string AirlineLogo = "logo_s";
        public static readonly string AirlineID = "airlineid_t";
    }
    
    public class AirportsConstant
    {
        public static readonly string CountryName = "countryname_t";
        public static readonly string CountryCode = "countrycode_t";
        public static readonly string CityCode = "citycode_t_en";
        public static readonly string CityName = "cityname_t";
        public static readonly string Priority = "priority_tf";
        public static readonly string AirportName = "airportname_t";
        public static readonly string AirportCode = "airportcode_t";
        public static readonly string AirportType = "Airport Type";
        public static readonly string AirportID = "AirportID";
        public static readonly string IsMasterAirport = "IsMasterAirport";
        public static readonly string IsPranaam = "ispranaam_b";
        public static readonly string IsPopular = "ispopular_b";
        public static readonly string Keywords = "keyword_t";
        public static readonly string ParentId = "_parent";
    }

    public static class MetaTagsConstant
    {
        public static readonly string MetaTitle = "MetaTitle";
        public static readonly string MetaDescription = "MetaDescription";
        public static readonly string Keywords = "Keywords";
        public static readonly string Canonical = "Canonical";
        public static readonly string Robots = "Robots";
        public static readonly string OGTitle = "OG-Title";
        public static readonly string OGImage = "OG-Image";
        public static readonly string OGDescription = "OG-Description";
        public static readonly string Viewport = "Viewport";
        public static readonly string RichTextTitle = "Title";
        public static readonly string RichText = "RichText";
    }
}