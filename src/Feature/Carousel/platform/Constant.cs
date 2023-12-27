using Sitecore.Data;
using Sitecore.Rules.Conditions.ItemConditions;
using System.Net;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform
{
    public class Constant
    {
        public static readonly string RestrictedCarousal = "liquor|travelexclusive|travel-exclusive|brandboutique|brand-boutique|adanie-one-xclusive|adanioneexclusive|combo-products";
        public static readonly string Title = "Title";
        public static readonly string FAQTitle = "AccordionTitle";
        public static readonly string FAQDescription = "AccordionDescription";        
        public static readonly string StanderedImage = "Standered Image";
        public static readonly string isAirportSelectNeeded = "isAirportSelectNeeded";
        public static readonly string isAgePopup = "isAgePopup";
        public static readonly string isInternational = "IsInternational";
        public static readonly string Description = "Description";
        public static readonly string CTA = "CTA";
        public static readonly string SubTitle = "Sub Title";
        public static readonly string SubTitle1 = "SubTitle";
        public static readonly string OfferEligibility = "OfferEligibility";
        public static readonly string DisplaySequence = "DisplaySequence";
        public static readonly string PromoCode = "PromoCode";
        public static readonly string ButtonText = "ButtonText";
        public static readonly string IsRestricted = "IsRestricted";
        public static readonly string IsUrl = "IsUrl";
        public static readonly ID PopupTemplateId = new ID("{9D20E620-0E21-46C7-B0E2-340E777DB448}");
        public static readonly ID DomainTemplateId = new ID("{8CCD141F-D701-4AF7-BB3F-DB340A70DC96}");
        public static readonly string Value = "Value";
        public static readonly string Label = "Title";
        public static readonly string IsActive = "IsActive";
        public static readonly string MoreButton = "MoreButton";
        public static readonly string ContentType = "Content-Type";
        public static readonly string ApplicationJson = "application/json";
        public static readonly string Accept = "Accept";
        public static readonly string AcceptType = "*/*";
        public static readonly string BannerDisplayRank = "Banner Display rank";
        public static readonly string deeplink = "DeepLink";
        public static readonly string deeplink2 = "DeepLink2";
        public static readonly string CheckValidity = "CheckValidity";
        public static readonly string effectiveTo = "EffectiveTo";
        public static readonly string effectiveFrom = "EffectiveFrom";
        //GTMTags Constants
        public static readonly string bannerCategory = "BannerCategory";
        public static readonly string businessUnit = "BusinessUnit";
        public static readonly string faqCategory = "FaqCategory";
        public static readonly string label = "Label";
        public static readonly string source = "Source";
        public static readonly string subCategory = "SubCategory";
        public static readonly string type = "Type";
        public static readonly string eventName = "Event";
        public static readonly string category = "{68531E40-5B4D-461B-A0C6-C0F3F3377B94}";
        // Field Name for Rewards Section
        public static readonly string RewardVideo = "Video";
        public static readonly string RewardMobileVideo = "MobileVideo";
        public static readonly string AutoId = "AutoId";
        public static readonly string autoid = "autoID";
        public static readonly string bannerlogo = "BannerLogo";

        public static readonly string deepLink = "Deep Link";

        public static readonly string OnlyforWeb = "Only For Web";
        public static readonly string OnlyforApp = "Only for App";

        //notification
        public static readonly string BGColor = "bgcolor";
        public static readonly string TextColor = "txtcolor";
        public static readonly string IconColor = "iconcolor";
        public static readonly string MobileEnable = "mwebenable";
        public static readonly string DesktopEnable = "desktopenable";
        public static readonly string cardBgColor = "cardBgColor";
        public static readonly string gridNumber = "gridNumber";
        public static readonly string listClass = "listClass";

        //Theme Constants
        public static readonly string DeskImage = "DesktopImage";
        public static readonly string Video = "DesktopVideo";
        public static readonly string MobileVideo = "MobileVideo";
        public static readonly string CTAUrl = "CTAUrl";
        public static readonly string WebActive = "WebActive";
        public static readonly string MWebActive = "MWebActive";
        //Hero Slider Constants 
        public static readonly string Link = "Link";
        public static readonly string Image = "Image";
        public static readonly string videoSrc = "VideoLink";
        public static readonly string MaterialGroup = "Material Group";
        public static readonly string Category = "Category";
        public static readonly string SubCategory = "Sub Category";
        public static readonly string Brand = "Brand";
        public static readonly string AirportCode = "Airport Code";
        public static readonly string StoreType = "Store Type";
        public static readonly string ThumbnailImage = "Thumbnail Image";
        public static readonly string BannerCondition = "Banner Condition";

        public static readonly string ctaurl = "ctaUrl";
        public static readonly Sitecore.Data.ID SKUCodeID = new ID("{C0578363-ABFF-4066-A307-4AE2004C0505}");

        public static readonly ID TemplateId = new ID("{0E6437FF-BB3B-4A86-B1CE-D9A3BFA3B684}");
        public static string RenderingParamField = "Widget";
        public static string SellerCard = "SellerCard";
        public static string CardTitle = "CardTitle";
        public static string Price = "Price";
        public static string Amount = "Amount";
        public static string OfferText = "OfferText";
        public static string UniqueId = "UniqueId";
        public static string MobileImage = "MobileImage";
        public static string WebImage = "WebImage";
        public static string SKUCode = "SKUCode";
        public static string btnText = "btnText";
        public static string StoreTypeFiled = "storeType";
        public static string apiURL = "apiURL";
        public static string AppImage = "Mobile Image";
        public static string DeskTopImage = "Desktop Image";
        public static string name = "name";
        public static string backgroundColor = "backgroundColor";
        public static string textColor = "textColor";
        public static readonly ID AirprotServicesMobileDatasource = new ID("{1DFDEB85-A511-4FD0-80ED-90A6431C1CBB}");

        public static readonly ID HelpSupportDatasourceFolder = new ID("{441E39A4-0126-4A22-864B-23A4171C9796}");
        public static readonly ID ContactDetailsTemplateID = new ID("{0A4DB799-BFB9-4BFD-86B8-B6230D78E252}");
        public static readonly ID HelpSupportTemplateID = new ID("{9E882E88-EA2E-42C9-AFD7-0CBC0E5F73AC}");

        //{0A4DB799-BFB9-4BFD-86B8-B6230D78E252}
        public static string descriptionApp = "DescriptionApp";
        public static string ApptypeFilter = "web";

        // Field Name for Offer and discount section
        public static readonly string PromotionType = "{13F94727-5C81-4445-A544-1FF1502C5F75}";
        public static readonly string PromotionCode = "{8DBB88CE-DF19-4DF4-B5F0-74CC5576067D}";
        public static readonly string PromotionDescription = "{18151319-E5C0-4AFD-A5B4-3C1142A0FF24}";
        public static readonly string BuyQuantity = "{E92F6863-73B4-41F3-8807-3DEF796F4ECC}";
        public static readonly string OfferType = "{231744B9-2FA5-4F8A-A96C-4E6EEA0E460C}";
        // public static readonly string OfferValue = "Offer Value";
        public static readonly string DisplayText = "{E3CD71C3-92A9-4D1B-A2AF-2635B08BBD42}";
        public static readonly string Savings = "{A18E2BF4-4003-4AE7-B6AC-6ECFC05FB981}";
        public static readonly string EffectiveFrom = "{C4B04CB2-9DBD-428A-8CAF-CF413D6A389D}";
        public static readonly string EffectiveTo = "{33CFB7D6-3145-4995-870E-5C69B9FC1484}";
        public static readonly string TerminalLocationType = "{1F0C8B3E-FA95-4A1E-A5A0-C7E0D6AA5B2B}";
        public static readonly string TerminalStoreType = "{F09C764B-F771-43D7-B49C-A5B68518F206}";
        public static readonly string Active = "{7AF49F8F-B27E-4EB4-B9AA-5A6CD7C544C9}";
        public static readonly string AppID = "{2D48C07A-8F83-4359-9F11-E831FBCC0881}";
        public static readonly string DesktopImage = "{783243BC-982E-4BB1-8145-BABCCEF1E247}";
        public static readonly string OfferMobileImage = "{8C152BE6-E409-4B42-8AA3-4B130137BE6E}";
        public static readonly string OfferThumbnailImage = "{19877ED9-5996-4E1B-894F-02398DEED82C}";
        public static readonly string OfferImage = "{5F7308D0-0C63-4556-84F5-3B97FF2559EF}";
        public static readonly string DeepLink = "{D5034FA0-81D3-47D7-AA7F-965F39C0B6A5}";
        public static readonly string OfferTitle = "{BF8A6215-9EF6-42D1-BE45-525EA200CA3D}";
        public static readonly string OfferSpecificCTAText = "{37A8C72B-E14F-4E08-83C3-E8D19252EDD6}";
        public static readonly string ShowonHomepage = "{5B093AF9-AF61-4781-9E41-2CA8338DB426}";
        public static readonly string Termandcondition = "{2465ADF3-0DF9-479E-98E8-1D7DBF91C245}";
        public static readonly string TCLink = "{AD97AA03-19AA-4866-9114-4EDB2332351F}";
        public static readonly string CarouselEnable = "{3D96D908-A5A5-4063-BA45-FC3EC97D26C6}";
        public static readonly string DisplayRank = "{8C85644C-FEEC-480F-84B7-C88813EEDBC9}";
        public static readonly string AppType = "{ECE0EA0A-8CF8-433B-96B2-7DB222AD703A}";
        public static readonly string WebType = "{FB818192-9C41-4EDA-8E62-5134063EDEA2}";
        public static readonly string OfferSkuCode = "{3ABE950E-5F71-4EBD-9ED4-CB92AD3DD319}";
        public static readonly string BannerImageDesk = "{6418F92F-E943-4CEA-9A2F-1D92867E24D0}";
        public static readonly string BannerImageMob = "{7AB06A6A-54FB-4335-82C1-F8C6461D2698}";
        public static readonly string CategoryFilter = "{91683985-2B7D-4856-9DE2-E4246AF9D8D5}";
        public static readonly string OfferUinqueID = "{08360427-7B86-4792-9814-77DF113A3472}";
        public static readonly string TCEnabled = "{4A3622E9-79A4-45D0-BCE5-5BF8E90F0098}";
        public static readonly string TabTitle = "{56DF9588-4CA0-457B-B1A0-1B278DC1ADD9}";
        public static readonly string PromotionTypeLabel = "{1BD722FF-FC69-4165-B079-9EA1A9B8E930}";
        public static readonly string isBankOffer = "{2D56B04C-111A-46C1-A9A6-6B119DE9309E}";
        public static readonly string LOB = "{C087C899-EE46-451B-A0AB-85DFA8B2D0F1}";
        public static readonly string displayedon = "{E42A2313-0469-4197-B803-C1098830D940}";
        public static readonly string Offerredirection = "{68E2672F-5B9B-4C96-9B1E-8F32EC8CA134}";
        public static readonly string OfferServicesRedirectionLink = "{FB74F7A6-053C-41EF-873A-5A3D1EEEDA1E}";
        public static readonly string isExclusive = "{7AAD52F1-7BFE-4B30-84BE-62E0C4A724EC}";
        public static readonly string isOfferAndDiscount = "{796553DC-D7F6-425F-894F-950ACC6FD038}";
        public static readonly string isBankOfferText = "{39B26A8B-B8C6-40D6-BCDD-149214249E99}";
        public static readonly string isOnedayOffer = "{22EABA53-5688-4A3D-8352-77C279FF8DAF}";
        //Code Comment to roll back New Offer Journey
        //public static readonly string IsBannerOffer = "{049AF0C3-95ED-4D45-AF3F-B9CB1A640520}";
        //public static readonly string BannerDisplayRank = "{D78FFF1A-37E2-4841-AF57-742CE3AB87E6}";

        // Dictionary Entries
        public static readonly string TermsandconditionfieldID = "{06A7315F-DF0C-4409-BCF1-47478F8AA3D0}";
        public static readonly string CtaText = "{550C98D8-7D50-443D-8533-D16C462F30D7}";


        public static readonly string OperatorType = "{781F6AE0-2533-459D-91FA-29B1F88C5B6D}";
        public static readonly string LocationTitle = "{65292D60-BED0-4207-9D4E-C399DB7F2244}";
        //Code Comment to roll back New Offer Journey
        //public static readonly string GlobalTermsandconditionFieldID = "{89330502-7F8C-4F85-93A3-E1120FE72668}";

        // ID for different States

        public static readonly string Ahmedabad = "{4BE84251-6470-4B7E-A94D-4FADCF0C1B1A}";
        public static readonly string Guwahati = "{A51C1C4A-9FD1-40C9-9165-2675E0BAAFC9}";
        public static readonly string Jaipur = "{A7FD228B-2791-4367-85F2-2D38963FA0E6}";
        public static readonly string Lucknow = "{A49B41EE-E4FA-4040-B535-6E9BF11BCC11}";
        public static readonly string Mangaluru = "{80235B66-A6D6-4C72-ADC8-22CF0299C506}";
        public static readonly string Mumbai = "{F744B88B-E8A8-47F8-8FA1-B17EEFDB815B}";
        public static readonly string Thiruvananthapuram = "{24EC2188-67CB-4168-8296-439845571F8D}";
        public static readonly string adaniOne = "{9034816F-E9CD-4888-800A-049EB877A5A6}";
        public static readonly string moduleType = "Flights";


        // ID for Arrival and departure
        public static readonly string Arrival = "{E330A953-B59B-4631-A01B-06215630DC40}";
        public static readonly string Departure = "{32538FFE-3587-4F05-BD89-FCBB8F0BA3BF}";

        //ID for Offer filter
        public static readonly string OfferTemplateID = "{68EEE3B2-4980-41FE-AB4E-7590B322FAE4}";

        //Long Weekend 
        public static string LWTitle = "CardTitle";
        public static string LWDescription = "CardDescription";
        public static string LWMobileImage = "CardMWebImage";
        public static string LWWebImage = "CardImage";
        public static string LWName = "Name";
        public static string LWRedirectLink = "RedirectLink";

        //OurBusiness
        public static string OBTitle = "Title";
        public static string OBDescription = "Description";
        public static string OBUrl = "Url";
        public static string OBUrl2 = "Url2";
        public static string OBUrlName = "UrlName";
        public static string OBListData = "ListData";
        public static string OBImageTitle = "ImageTitle";
        public static string OBImageDescripton = "ImageDescripton";
        public static string OBWebImage = "WebImage";
        public static string OBCTAUrl = "CTAUrl";
        public static string OBMobileImage = "MobileImage";
        public static string value = "value";
        public static string sign = "sign";
        public static string detail = "details";
        public static string title = "title";
        public static string Detail = "Detail";
        public static string btnName = "btnName";
        public static string btnName2 = "btnName2";
        public static string SubDetail = "SubDetail";

        public static string ServiceID = "ServiceID";
        public static string ServiceUrl = "ServiceUrl";

        //public static string UrlTarget ="UrlTarget";
        //public static string CTAUrlTarget = "CTAUrlTarget";

        //FAQConstants
        public static string AllCategoriesList = "AllCategoriesList";
        public static string Heading = "Heading";
        public static string SeeAllLink = "SeeAllLink";
        public static string Question = "Question";
        public static string FAQQuestionRedirectLink = "FAQQuestionRedirectLink";
        public static string SearchPlaceholderText = "SearchPlaceholderText";
        public static string SearchItemNotFound = "SearchItemNotFound";


        public class LoyaltyRewards
        {
            public static readonly string Title = "{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}";
            public static readonly string Description = "{ED2534C4-B080-48A0-A82D-4CA2EE96A935}";
            public static readonly string StanderedImage = "{DB69B6E3-4E77-43F5-BCD4-3BE6E81C017B}";
            public static readonly string CTA = "{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}";
            public static readonly string DescriptionApp = "{5D7F2597-E8C0-473A-9471-2E85D53D32C7}";
            public static readonly string ImageMobile = "{3693AD53-FC1E-4AC6-BDA6-B358357997D9}";
            public static readonly string Type = "{BC449365-A09B-423C-81E4-17AAA249131E}";
            public static readonly string Location = "{F04E3A3E-9C64-45EF-87E2-66BE01BA55B5}";
            public static readonly string SKUCode = "{D7152E41-6668-42E0-AA49-9E30777D8BD9}";
            public static readonly string CategoryFilter = "{08BDB691-A90C-4351-B118-AD8C2A3B5FF5}";
            public static readonly string Active = "{42CEAC93-042E-49F1-8291-05D87780E584}";
            public static readonly string IsApp = "{83057A50-48F8-41A5-974E-5E2563188190}";
            public static readonly string IsWeb = "{00682F4A-9975-4B39-8A74-C05E2111E34F}";
            public static readonly string Apptype = "app";
            public static readonly string RewardUniqueId = "RewardUniqueId";

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

        public class HowToRedeemDS
        {
            public static readonly ID ComponentTitle = new ID("{53D34366-B0EC-4A28-BDAC-ECA8D1E825C0}");
            public static readonly ID RedeemSteps = new ID("{3C3513E5-C058-47CD-8659-580E339D756D}");
        }

        public class HowToRedeemSection
        {
            // 24723 (How to redeem section)
            public static readonly Sitecore.Data.ID RedeemTitle = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
            public static readonly Sitecore.Data.ID RedeemDescription = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
            public static readonly Sitecore.Data.ID RedeemDesktopImage = new ID("{DB69B6E3-4E77-43F5-BCD4-3BE6E81C017B}");
            public static readonly Sitecore.Data.ID RedeemMobileImage = new ID("{DDCDF1E3-DCAA-4BC1-AD02-903DB11492B6}");
            public static readonly Sitecore.Data.ID RedeemLink = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
            public static readonly Sitecore.Data.ID RedeemDescriptionApp = new ID("{5D7F2597-E8C0-473A-9471-2E85D53D32C7}");
            public static readonly string RedeemCTA = "Link";
        }
        
        public class Calendar
        {
            public static readonly string Title = "Title";
        }
    }
}