using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.Carousel.Platform
{
    public static class Templates
    {
        public static readonly ID commonItem = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");

        /// <summary>
        /// carosuel Component : (Video Component - {97FA0743-CFE7-4B01-B51F-D9939A46C409} & 
        ///                       Hero Banner - {4A421518-7B4A-4D43-AF9B-94E17E00EC9F} &
        ///                       Image Template - {494F095B-B942-46AF-AFDC-6833DC5504A1})
        /// </summary>
        public static class HeroBanner
        {
            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static class Fields
            {
                public static readonly ID isVideoID = new ID("{A86DE88B-9365-4600-960D-73C2E2E7CE9E}");
                public static readonly string isVideoFieldName = "isVideo";
                public static readonly ID LogoID = new ID("{B5F61442-FF0F-46A5-90A8-D6D387DE24A0}");
                public static readonly string LogoFieldName = "logo";
                public static readonly ID LinkID = new ID("{17B31027-C4FF-4E30-8DCE-6D0DBF71E6A4}");
                public static readonly string LinkFieldName = "link";
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly string headingFieldName = "heading";
                public static readonly ID subheadingID = new ID("{89B0A8ED-0EE8-4512-B518-AB2C4C2A0B9E}");
                public static readonly string subheadingFieldName = "subheading";
                public static readonly ID reranoID = new ID("{2DFD1678-DDD5-4101-977A-31ACD1B9CECA}");
                public static readonly string reranoFieldName = "rerano";
                public static readonly ID imgtypeID = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");
                public static readonly string imgtypeFieldName = "imgtype";
                public static readonly ID thumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly string thumbFieldName = "thumb";
                public static readonly ID VideoMp4ID = new ID("{795531F5-AE9B-4031-B9F2-8E49CDB47C00}");
                public static readonly string VideoMp4FieldName = "videoMp4";
                public static readonly ID videoposterID = new ID("{AF50B2BD-4B11-467D-AC89-6ACB266C3C38}");
                public static readonly string videoposterFieldName = "videoposter";
                public static readonly ID videoOggID = new ID("{1FF9D71A-82BF-4599-BEA6-6BEBD7C95069}");
                public static readonly string videoOggFieldName = "videoOgg";
                public static readonly ID videoposterMobileID = new ID("{6BAC1E87-C807-4F20-84AD-6676FC699819}");
                public static readonly string videoposterMobileFieldName = "videoposterMobile";
                public static readonly ID videoMp4MobileID = new ID("{055C8277-18FA-46DC-81BF-A2D3EB097D7E}");
                public static readonly string videoMp4MobileFieldName = "videoMp4Mobile";
                public static readonly ID videoOggMobileID = new ID("{F346F662-F1D6-4BCD-9BF6-A801FFF7EA1E}");
                public static readonly string videoOggMobileFieldName = "videoOggMobile";
                public static readonly ID SEOName = new ID("{C9514E8A-DFE9-4D51-9290-96C2EAD34847}");
                public static readonly ID SEODescription = new ID("{9F1C4AA5-3867-4861-A14B-C3288D556F3B}");
                public static readonly ID UploadDate = new ID("{D01BC0E7-4449-4B54-8BCB-C7F8AADF5EB5}");
                public static readonly ID SrcMobileId = new ID("{1FF9D71A-82BF-4599-BEA6-6BEBD7C95069}");
                public static readonly string MobileImageFieldName = "Mobile Image";
                public static readonly ID propertyTypeId = new ID("{780D29A9-11E5-44F4-8A67-3331E5A88E81}");
                public static readonly string propertyTypeFieldName = "CTA Title";
                public static readonly ID propertyNameId = new ID("{7ACE4C96-1542-4EF2-A725-67EC7439245E}");
                public static readonly string propertyNameFieldName = "BTN Text";
            }
        }
        public static class Communicationcornertemplate
        {
            public static readonly ID TemplateID = new ID("{2FBDA2B6-BA1B-472F-8040-3EB3059024C7}");
            public static class Fields
            {
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID ImageID = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string ImageFieldName = "Image";
            }
        }
        public static class mediaCoverageTemp
        {
            public static readonly ID TemplateID = new ID("{10BE6E63-937F-4F2C-BB8E-07128A9D5228}");
            public static class Fields
            {
                public static readonly ID posterSrc = new ID("{4800751B-78C9-4553-B87C-E7673827A478}");
                public static readonly ID title = new ID("{081C0FBD-A1DF-4C15-BBFF-D5411E18F806}");
                public static readonly ID date = new ID("{4AA40CAD-19BD-4F22-A9D1-957ABA555646}");
                public static readonly ID link = new ID("{6D1445AB-8D87-4DF2-9DD3-1E11F3F0E190}");
                public static readonly ID linkTitle = new ID("{7BFE7151-44B8-47DC-B169-17A0A2E09C69}");
                public static readonly ID modalTitle = new ID("{B6E7B6AF-7F57-40BA-95BA-361EEDF762CE}");
                public static readonly ID galleryData = new ID("{BB4D61A3-AD3F-49AD-B855-3529076D2AD1}");
                public static readonly ID pdfSrc = new ID("{45C35686-23DA-4DC8-A044-E95B56AAF515}");

                public static class imageFields
                {
                    public static readonly ID TemplateID = new ID("{494F095B-B942-46AF-AFDC-6833DC5504A1}");
                    public static readonly ID src = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                    public static readonly ID imgType = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");
                }
            }
        }

        public static class Highlights
        {
            public static readonly ID cards = new ID("{564AB313-122B-4D63-BBC3-00FDB9DD758F}");
            public static readonly ID disclaimer = new ID("{6E3BDEB1-FE53-4AFE-B77C-9E33659BFEAA}");
            public static readonly ID heading = new ID("{A24FF3AF-4C9B-4ED9-8DB0-54602CAA9028}");

            public static class card
            {
                public static readonly ID frontImage = new ID("{09FC050C-43FA-4D29-AAA3-863A02518873}");
                public static readonly ID backImage = new ID("{0B710765-7880-4417-AF7F-B888E4DC37CE}");
                public static readonly ID title = new ID("{52C3BAB8-E578-4501-A627-6FFF238F77CE}");
                public static readonly ID cardLink = new ID("{29E7BCEA-49BE-417F-94EB-157A163C675A}");
            }
        }
        public static class tempAmenities
        {
            public static readonly ID mainHeading = new ID("{F4F007DA-D490-4BBB-A3D1-5A1AABB37B30}");
            public static readonly ID id = new ID("{62CCE485-8FF9-4C6B-B25D-44EBD53D08D3}");
            public static readonly ID dataLists = new ID("{70C849B5-AFBC-4C53-AB9C-510F43465636}");
            public static class dataList
            {
                public static readonly ID sectionHeading = new ID("{B1C1362D-D4EB-46D9-A4F2-8F482035E02B}");
                public static readonly ID subHeading = new ID("{AA998E86-6A89-4577-AC4A-F038315EF37C}");
                public static readonly ID description = new ID("{E403F867-555B-401F-910F-07EF401020E3}");
                public static readonly ID imageSource = new ID("{DA1CC2F7-435D-446E-AE6D-219805DE198C}");
                public static readonly ID imageSourceMobile = new ID("{EBA2FB9C-8544-4007-AC50-5F269EF4DA34}");
                public static readonly ID imageSourceTablet = new ID("{49D03313-D916-4CA3-BE1C-B6685A19CEF3}");
            }
        }
        public static class ClubSelection
        {
            public static readonly ID TemplateID = new ID("{CCBABC95-54F4-4F95-BE03-6B9510D6D1C4}");
            public static class Fields
            {
                public static readonly ID ItemID = new ID("{E197515F-BA7C-4994-8CD8-C368218B5153}");
                public static readonly ID LinkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string LinkfieldName = "link";
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID SummaryID = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly ID BodyID = new ID("{261596D0-34C2-47CE-92B3-DD0911A40E0E}");
                public static readonly ID ImageID = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string ImageFieldName = "Image";
                public static readonly ID LocationID = new ID("{B380C8A2-8D5C-4A0E-ABD4-D0AC214E3824}");
                public static readonly ID MediaLibraryID = new ID("{3BAB7A7E-77A4-4B8C-BE72-81CF5CBAB4E8}");
                public static readonly ID readlessID = new ID("{4599D97E-B034-4BDF-B2F5-524C237C59EC}");
                public static readonly ID readmoreID = new ID("{F769CB9A-813B-48FE-9BDF-079BF52BFDE7}");
                public static readonly ID thumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly string thumbFieldName = "thumb";
                public static readonly ID imageTypeID = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");

            }
            public static class BaseFields
            {
                public static readonly ID TitleField = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");
            }
        }
        /// <summary>
        /// Banner Component : (Image Template - {494F095B-B942-46AF-AFDC-6833DC5504A1} & Style class : {AF5CEB5B-6D37-4B57-BF58-2E933966E725}) templates field
        /// </summary>
        public static class BannerTemplate
        {
            public static class Fields
            {
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly string headingFieldName = "heading";
                public static readonly ID thumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly ID TitleID = new ID("{36DFD09C-9819-48CD-A9BA-DA74CBD686AF}");
                public static readonly ID SubheaadingID = new ID("{89B0A8ED-0EE8-4512-B518-AB2C4C2A0B9E}");
                public static readonly ID imgtypeID = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");
                public static readonly string thumbFieldName = "thumb";
                public static readonly string MobileImage = "{86568F07-2F96-4C45-A847-7DB75C165DC9}";
                public static readonly ID logoID = new ID("{B5F61442-FF0F-46A5-90A8-D6D387DE24A0}");
                public static readonly string logoFieldName = "logo";
                public static readonly ID BTNtextID = new ID("{7ACE4C96-1542-4EF2-A725-67EC7439245E}");
                public static readonly string BTNTextFieldName = "BTN Text";
                public static readonly ID class1ID = new ID("{AF50B2BD-4B11-467D-AC89-6ACB266C3C38}");
                public static readonly string class1FieldName = "class1";
                public static readonly ID class2ID = new ID("{1FF9D71A-82BF-4599-BEA6-6BEBD7C95069}");
                public static readonly ID readmoreID = new ID("{E7D58D3B-8DC8-49B1-8F18-8795289925A9}");
                public static readonly ID CTATitleID = new ID("{780D29A9-11E5-44F4-8A67-3331E5A88E81}");
                public static readonly string class2FieldName = "class2";
                public static readonly ID LinkID = new ID("{17B31027-C4FF-4E30-8DCE-6D0DBF71E6A4}");
                public static readonly string LinkFieldName = "link";
            }
        }
        public static class BrandICons
        {
            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static readonly ID CommercialTemplateID = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
            public static readonly ID CommercialItemID = new ID("{02E532B9-7111-41F0-BB7F-217AD643897A}");
            public static readonly ID ResidentialTemplateID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID ResidentialItemID = new ID("{7BD1615B-E047-4D13-AD31-03DDCF5201EA}");

            public static class Fields
            {
                public static readonly ID LatitudeID = new ID("{D04153A0-E7B0-43A8-911A-6BB9C6EC8326}");
                public static readonly ID LongitudeID = new ID("{E9E067E6-9B78-4BFC-B6AD-1D9791D8587A}");
                public static readonly ID thumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly string thumbFieldName = "thumb";
                public static readonly ID mobileimageID = new ID("{86568F07-2F96-4C45-A847-7DB75C165DC9}");
                public static readonly string mobileimageFieldName = "Mobile Image";
                public static readonly ID LocationID = new ID("{B380C8A2-8D5C-4A0E-ABD4-D0AC214E3824}");
                public static readonly string LocationFieldName = "Location";
                public static readonly ID LinkID = new ID("{17B31027-C4FF-4E30-8DCE-6D0DBF71E6A4}");
                public static readonly string LinkFieldName = "link";
            }
        }
        /// <summary>
        /// PropertiesDetail Component 
        /// </summary>
        public static class PropertiesLIst
        {
            public static readonly ID TemplateID = new ID("{01B6A097-8B06-494D-9D0F-939CE584416B}");
            public static readonly ID PropertiesID = new ID("{B0B87592-3283-4A77-B4D5-EC13CD1FCBE4}");
            public static readonly ID CommercialID = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
            public static readonly ID CommercialLandingID = new ID("{05BB59FB-7490-4A4B-A9DD-EFBD9839EFC3}");
            public static readonly ID ClubID = new ID("{463784CA-A045-4C87-9EF5-3CE93772F4BD}");
            public static readonly ID ClubLandingID = new ID("{A91C40DF-382D-4D8B-B70B-748F34C9A717}");
            public static readonly ID ResidentialID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID ResidentialItemID = new ID("{A6A604B2-2656-4E5C-952C-05279F72D014}");
            public static readonly ID ItemID = new ID("{A57CFE78-4712-413D-B0C9-3499073AEE6F}");
            public static readonly ID MediaSelectionID = new ID("{3BAB7A7E-77A4-4B8C-BE72-81CF5CBAB4E8}");

            public static class Fields
            {
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID LocationID = new ID("{B380C8A2-8D5C-4A0E-ABD4-D0AC214E3824}");
                public static readonly ID SiteStatus = new ID("{C726F4E1-A95A-47E3-B101-76508BF7A465}");
                public static readonly ID ProjectStatus = new ID("{0350CF91-6ECB-4F5A-BC4C-430D0E32214B}"); // dropdown
                public static readonly ID ReraID = new ID("{C9CA18D8-A0CD-4AEA-A7D3-9EB86D315D8B}");
                public static readonly ID PropertyLinkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly ID projectSpec = new ID("{68009697-D024-4247-8FA2-95E3A225236B}");
                public static readonly ID areaLabel = new ID("{5915A6B2-FE92-4FC1-9FB9-A2DA8530FFA3}");
                public static readonly ID priceLabel = new ID("{C6163940-69AB-4D50-A828-8194A127600E}");
                public static readonly ID areaDetail = new ID("{3CF2B01F-A43C-477A-805B-565A0FB57AB4}");
                public static readonly ID priceDetail = new ID("{1503CC29-BC69-43B3-8D7E-067B76301F41}");
                public static readonly ID onwards = new ID("{D3487E58-9C53-4BBC-827A-BA99FA1CE3C6}");
                public static readonly ID thumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly ID imgtypeID = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");
                public static readonly ID PropertyPriceID = new ID("{33D8D8A2-5674-4AE6-BBA9-B9D9F8C9A4F5}");
                public static readonly ID PropertyPriceFilterID = new ID("{6596659C-0AAB-45A1-BC2D-DE84E13A0D6C}");
                public static readonly ID PropertyTypeID = new ID("{75091E77-938C-4379-84FE-174833285D4C}");
                public static readonly ID typeID = new ID("{04CF398B-5FE5-4A5D-93F4-AC750549D77E}");
                public static readonly ID PropertyLogoID = new ID("{E8B52311-A1D9-4B04-B5A0-0682EE79EDBF}");
                public static readonly string PropertyLogoFieldName = "Property Logo";
                public static readonly ID StaticTextLabelID = new ID("{5F4C1D62-59E7-48B1-9476-85CA8D5284B9}");
                public static readonly ID condition = new ID("{BCF10793-231D-45DB-A0C5-D4E1EEAC29BF}");
                public static readonly ID Locationpagebreadcrumb = new ID("{023CAB6C-59B7-4003-BD9E-BD981A9F04E9}");
                public static readonly ID Projectsfoundtext = new ID("{F103DBA2-0A9D-403E-80BA-09E54D46E620}");
                public static readonly ID Projectfoundtext = new ID("{E3CCF8B1-8B31-4666-8933-E958E2B8DD0F}");
                public static readonly ID PropertyRankingonSite = new ID("{048ED6AD-A91D-463F-AF1B-0476D9E670E6}"); // Property Ranking on Site
                public static readonly ID townshipID = new ID("{D04792D9-42E1-4D98-98C6-CA3AC179AE4A}"); // Property Ranking on Site
                public static readonly ID projectsFound = new ID("{B4F43317-BD69-44E8-A82D-D72C9E708126}");

                public static readonly ID CityName = new ID("{A959C5FB-8880-4204-AD66-8007045DE1AF}");
                public static readonly ID Flats = new ID("{72039E5D-4EEB-49C9-B13D-EA251744FEE1}");
                public static readonly ID PropertyType = new ID("{A5D160AE-69CC-40B9-97DD-9165968BA6CD}");
                public static readonly ID SEOProjectStatus = new ID("{DBA844B5-317E-45D7-ADA3-496AE903B6B3}");
                public static readonly ID ConfugurationType = new ID("{0B04038F-0DD0-45EF-A904-D8505DA56AF7}");
                public static readonly ID Location = new ID("{0E915D68-BE8C-4439-AF6C-9EC488D5BD9F}");
                public static readonly ID Area = new ID("{318E9BB2-30DE-4C55-B3C6-07967D75D3E2}");
                public static readonly ID Subcity = new ID("{E446434D-C8F9-4E16-8492-DFE8E32CD69B}");
                public static readonly ID ProjectType = new ID("{C4EC7EAD-C43D-48A3-8998-97BE18BD3CB4}"); //R & C Prop
                public static readonly ID SEOProjectType = new ID("{8915C2A4-7722-4CA3-9FDD-CE32610EEF80}"); // SEO page
                public static readonly ID isProjectCompleted = new ID("{E99A2C77-88B6-4D2D-886F-A08DF8D24810}");
            }
        }
        public static class SeoPropertyList
        {
            public static readonly ID TemplateID = new ID("{2201EA62-B7E9-48BB-97F9-13D50D4C18E7}");
            public static readonly ID Projects = new ID("{6DAEF7B4-56E6-4BCD-B738-320F51A924A6}");
        }
        public static class OfficeListTemp
        {
            public static readonly ID BaseTemplateID = new ID("{6FF34C27-81B2-47A3-A2B9-6C0143CD3ADD}");
            public static readonly ID ItemtemplateID = new ID("{93C13122-FE8C-4182-9A27-69E757FDDA18}");
            public static class BaseFields
            {
                public static readonly ID TitleField = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");
            }
            public static class Fields
            {
                public static readonly ID imageField = new ID("{7B588E4B-F089-4259-87EB-4DB9D2DB1C36}");
                public static readonly string imagesFieldName = "images";
                public static readonly ID TitleField = new ID("{2837F5AB-8D44-484C-ABCD-B166B46BBDD5}");
                public static readonly ID AddressField = new ID("{4BA98FB3-BB6E-4D23-BC6B-BC9D0A9A3ADE}");
            }
        }
        public static class ContactUsPageDataTemp
        {
            public static readonly ID BaseTemplateID = new ID("{D491691E-DBB7-4050-96CA-D65DF5EC74D2}");

            public static class Fields
            {
                public static readonly ID SummaryField = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
            }
        }

        /// <summary>
        /// internal image properties
        /// </summary>
        public static class ImageFeilds
        {
            public static readonly ID TemplateID = new ID("{F1828A2C-7E5D-4BBD-98CA-320474871548}");
            public static class Fields
            {
                public static readonly ID AltID = new ID("{65885C44-8FCD-4A7F-94F1-EE63703FE193}");
                public static readonly string AltFieldName = "Alt";
                public static readonly ID TitleID = new ID("{3F4B20E9-36E6-4D45-A423-C86567373F82}");
                public static readonly string TitleFieldName = "Title";
            }
        }

        public static class SEOpage
        {
            public static readonly ID BaseTemplateID = new ID("{20B33133-18FA-4C8E-9E58-E112A0C97EF7}");
        }
    }
}
