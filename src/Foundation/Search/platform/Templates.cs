using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform
{
    public static class Templates
    {
        public static class AirlineCollection
        {
            public static readonly ID AirlineTemplateID = new ID("{6D83803C-31EB-4D22-AF6F-50A226E6D869}");
            public const string LogoField = "Logo";
            public const string MobileImageField = "MobileImage";
            public const string ThumbnailImageField = "ThumbnailImage";
            public const string AirlineType = "AirlineType";
        }

        public static class SitemapCollection
        {
            public static readonly ID SeolandingPageTemplateID = new ID("{4A9D4805-6503-4ED1-B1A3-68C401B9C900}");
            public static readonly ID SeoPageTemplateID = new ID("{F30C68F5-3783-4F1E-BDCB-C6538BB2D9AA}");
            public const string PriorityField = "Priority";

        }

        public static class DFCollection
        {
            public static readonly ID ProductTemplateID = new ID("{93DC548E-C7FF-42E6-87BF-889D03C5CE42}");
            public static readonly ID BrandFolderID = new ID("{04E90FDC-E682-4083-8A64-BE2F92491C25}");
            public static readonly ID MaterialGroupFolderID = new ID("{A3349AE2-51BF-401B-BA75-836884B75910}");
            public static readonly ID MaterialGroupTemplateID = new ID("{F6E33521-E0A2-48D3-8D7C-AC33FC301E5C}");
            public static readonly ID CategoryTemplateID = new ID("{5B0D0497-19EC-44CC-B5AB-9E0245933BEC}");
            public static readonly ID SubCategoryTemplateID = new ID("{2F8490CE-ABB5-4676-8194-5430203E57F1}");
            public static readonly ID BrandTemplateID = new ID("{C06138F7-CD65-437F-ADF8-709D96211C59}");
            public const string Material_Group_Name = "Material Group";
            public const string Material_Group_Code = "Material Group Code";
            public const string Category_Name = "Category";
            public const string SubCategory_Name = "Sub Category Code";
            public const string Brand_Name = "Brand";
            public const string BrandName = "Brand Name";
            public const string BrandCode = "Brand Code";
            public const string ProductName = "SKU Name";
            public const string ProductSEOName = "Product Name";
            public const string Code = "Code";
            public const string Name = "Name";
            public const string Title = "Title";
            public const string TravelExclusive = "Travel Exclusive";
            public const string BucketGroup = "Bucket Group";
            public const string IsRecomended = "IsRecomended";
            public const string ShowonHomepage = "Show on Home Page";
            public const string IsActive = "Active";
            public static readonly ID BrandFieldId = new ID("{6CCF141B-493B-436D-BAB3-665D56B04C14}");
            public static readonly ID MaterialGroupFieldId = new ID("{178808D4-2686-4F6F-9A39-8314036BF30F}");
            public static readonly ID CategoryFieldId = new ID("{33D26D56-91BF-4083-B02D-5DC10D10837C}");
            public static readonly ID SubCategoryFieldId = new ID("{EAC8A653-21D9-4177-A008-D309E1E1A405}");

        }

        public static class OfferCollection
        {
            public static readonly ID PromoOfferTemplateID = new ID("{68EEE3B2-4980-41FE-AB4E-7590B322FAE4}");
            public static readonly ID CartOfferTemplateID = new ID("{3F15B412-3F0D-475F-BAAE-FFC7B6F57D3F}");
            public static readonly ID PromotionTitleFieldID = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
            public static readonly ID PromotionTypeFieldID = new ID("{13F94727-5C81-4445-A544-1FF1502C5F75}");
            public static readonly ID PromotionCodeFieldID = new ID("{8DBB88CE-DF19-4DF4-B5F0-74CC5576067D}");
            public static readonly ID PromotionOfferTypeFieldID = new ID("{231744B9-2FA5-4F8A-A96C-4E6EEA0E460C}");
            public static readonly ID PromotionDisplayTextFieldID = new ID("{E3CD71C3-92A9-4D1B-A2AF-2635B08BBD42}");
            public static readonly ID PromotionTerminalStoreTypeFieldID = new ID("{781F6AE0-2533-459D-91FA-29B1F88C5B6D}");
            public static readonly ID PromotionOfferDescriptionFieldID = new ID("{18151319-E5C0-4AFD-A5B4-3C1142A0FF24}");
            public static readonly ID PromotionOfferTermAndConditionFieldID = new ID("{18151319-E5C0-4AFD-A5B4-3C1142A0FF24}");
            public static readonly ID PromotionDesktopImageFieldID = new ID("{783243BC-982E-4BB1-8145-BABCCEF1E247}");
            public static readonly ID PromotionMobileImageFieldID = new ID("{8C152BE6-E409-4B42-8AA3-4B130137BE6E}");
            public static readonly ID PromotionThumbnailImageFieldID = new ID("{19877ED9-5996-4E1B-894F-02398DEED82C}");
            public static readonly ID PromotionLocationTypeImageFieldID = new ID("{781F6AE0-2533-459D-91FA-29B1F88C5B6D}");
            public static readonly ID PromotionExtraImageFieldID = new ID("{5F7308D0-0C63-4556-84F5-3B97FF2559EF}");
            public static readonly ID PromotionTCLinkFieldID = new ID("{AD97AA03-19AA-4866-9114-4EDB2332351F}");
            public static readonly ID PromotionLinkFieldID = new ID("{D5034FA0-81D3-47D7-AA7F-965F39C0B6A5}");
            public static readonly ID OfferServiceRedirectionURL = new ID("{FB74F7A6-053C-41EF-873A-5A3D1EEEDA1E}");
            public static readonly ID PromotionBannerImageDesktopFieldID = new ID("{6418F92F-E943-4CEA-9A2F-1D92867E24D0}");
            public static readonly ID PromotionBannerImageMobileFieldID = new ID("{7AB06A6A-54FB-4335-82C1-F8C6461D2698}");
            public static readonly ID PromotionOfferUniqueFieldID = new ID("{08360427-7B86-4792-9814-77DF113A3472}");
            public static readonly ID PromotionOfferLocationStoreTypeFieldID = new ID("{1F0C8B3E-FA95-4A1E-A5A0-C7E0D6AA5B2B}");
            public static readonly ID PromotionOfferFullScreenImageFieldID = new ID("{6E76F34D-9D09-49C3-90C4-5D262C9F51CF}");
            //Commencted for roll back new Offer Journey
            public static readonly ID PromotionOfferLocationStoreFieldID = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
            //public static readonly ID PromotionOfferLocationStoreFieldID = new ID("{447348EA-6B2F-4038-A87C-8DC8AD2D1D52}");
            public static readonly ID PromotionOfferOperatorTypeFieldID = new ID("{F09C764B-F771-43D7-B49C-A5B68518F206}");
            public static readonly ID PromotionOfferLabelTypeFieldID = new ID("{1BD722FF-FC69-4165-B079-9EA1A9B8E930}");
            public static readonly ID PromotionOfferTabTitleFieldID = new ID("{56DF9588-4CA0-457B-B1A0-1B278DC1ADD9}");
            public static readonly ID LOBPromotionOfferFieldID = new ID("{C087C899-EE46-451B-A0AB-85DFA8B2D0F1}");
            public static readonly ID OfferRedirectionURLFieldID = new ID("{68E2672F-5B9B-4C96-9B1E-8F32EC8CA134}");
            public static readonly ID OfferDisplayonPageFieldID = new ID("{E42A2313-0469-4197-B803-C1098830D940}");
            public static readonly ID OfferMobileBannerImageFieldID = new ID("{22EABA53-5688-4A3D-8352-77C279FF8DAF}");
            //Ticket No 17128
            public static readonly ID OfferLogoDesktopImageFieldID = new ID("{D5C61B36-642A-4816-83E1-D98543785DC1}");
            public static readonly ID OfferLogoMobileImageFieldID = new ID("{79C7990F-D119-4CDA-9E1F-135FC5A66DFF}");
            public static readonly ID UnlockOfferCTALinkFieldID = new ID("{44DF0C51-A145-4212-A292-AEF7F09247D1}");
            public static readonly ID UnlockOfferCTAVisitWesiteLinkFieldID = new ID("{4B024D4F-CC37-44AC-92BB-7DCD312F22E8}");
            public static readonly ID OfferIsInternational = new ID("{4D2064BD-555D-4682-94F5-1E8CAB35931C}");
            public static readonly ID OfferIsActive = new ID("{7AF49F8F-B27E-4EB4-B9AA-5A6CD7C544C9}");
            public static readonly ID OfferIsApp = new ID("{ECE0EA0A-8CF8-433B-96B2-7DB222AD703A}");
            public static readonly ID OfferIsWeb = new ID("{FB818192-9C41-4EDA-8E62-5134063EDEA2}");
            public static readonly ID OfferIsBankOffer = new ID("{2D56B04C-111A-46C1-A9A6-6B119DE9309E}");
            public static readonly ID OfferIsUnlockedOffer = new ID("{94FA2B9E-7012-44EC-8F63-AD6CA8D81425}");
            public static readonly ID OfferTCViewAllLink = new ID("{6CA58CF5-C220-4AE2-8AB4-20A500E536E8}");
            public static readonly ID CanonicalField = new ID("{3B98DAFD-D768-4070-A3B6-C77225C32505}");
            //Ticket (User Story) 40612
            public static readonly ID OfferDiscountPrice = new ID("{BB37CDFD-A933-4B73-B34E-F9E8FECF38DB}");
            public static readonly ID OfferDiscountPercent = new ID("{40C0DDBB-F9F2-400D-9687-9782EB80FFDD}");
        }

        public static class BlogCollection
        {
            public static readonly ID BlogMainTemplateID = new ID("{416E4F91-A92E-4A79-A6B8-7F94A2079334}");
            public static readonly ID BlogMainTitleFieldID = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
            public static readonly ID BlogCategoryTemplateID = new ID("{87EA4D36-6421-46E5-A0E9-E2C08ECA9AA4}");
            public static readonly ID BlogCategoryTitleFieldID = new ID("{2A568B60-5A63-4C01-B7FA-365550171DA1}");
            public static readonly ID BlogCategoryImageFieldID = new ID("{D323D0B7-F750-4AE4-91F5-13C5E132AD10}");
            public static readonly ID BlogDetailTemplateID = new ID("{7364777D-9B1D-4A7F-95EB-D568FE34DFF8}");
            public static readonly ID BlogDetailTitleFieldID = new ID("{BBC4A55C-77FF-4D40-8A91-F04CE1631672}");
            public static readonly ID BlogDetailImageFieldID = new ID("{8CD074FC-B37D-44FF-8294-DBB5AC41CF4D}");
        }

    }
}