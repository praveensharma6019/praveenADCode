using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.Sitemap.Platform
{
    public static class Templates
    {

        #region Home
        public static class Home
        {
            public static readonly ID ItemID = new ID("{6316DD92-A4F9-4302-BA75-727246707415}");
        }
        #endregion

        #region BaseTemplate
        public static class Image
        {
            public static readonly ID TemplateID = new ID("{2C704E65-FE58-4052-BBB4-5D93EFCDC46B}");
            public static readonly string TemplateName = "IImage";
            public static class FieldsID
            {
                public static readonly ID Image = new ID("{0983BE5E-A609-41A5-9FE0-DDF0D65A36C9}");
            }
            public static class FieldsName
            {
                public static readonly string Image = "Image";
            }
        }
        public static class IThumbnail
        {
            public static readonly ID TemplateID = new ID("{93EE5F3A-125F-412D-BB5B-82C1B6A455EF}");
            public static readonly string TemplateName = "IThumbnail";
            public static class FieldsID
            {
                public static readonly ID Thumbnail = new ID("{EB550D65-5550-4133-B0CC-A38E8C51DFAA}");
            }
            public static class FieldsName
            {
                public static readonly string Thumbnail = "Thumbnail";
            }
        }
        public static class ITitle
        {
            public static readonly ID TemplateID = new ID("{6FF34C27-81B2-47A3-A2B9-6C0143CD3ADD}");
            public static readonly ID statusID = new ID("{C980E459-567F-47FD-AD7A-7215C595006D}");
            public static readonly ID Gallery = new ID("{D7F3C8BE-5C1F-4868-B85F-BB8A2E0266A1}");
            public static readonly string TemplateName = "ITitle";
            public static class FieldsID
            {
                public static readonly ID Title = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");
            }
            public static class FieldsName
            {
                public static readonly string Title = "Title";
            }
        }
        public static class ISubTitle
        {
            public static readonly ID TemplateID = new ID("{C422F1E5-737B-44FE-9132-ACCEC5ABFDC5}");
            public static readonly string TemplateName = "ISubTitle";
            public static class FieldsID
            {
                public static readonly ID SubTitle = new ID("{C095C5FC-6D9E-47BC-B37B-1387C4800265}");
            }
            public static class FieldsName
            {
                public static readonly string SubTitle = "SubTitle";
            }
        }
        public static class IDescription
        {
            public static readonly ID TemplateID = new ID("{1D45045B-1CED-429F-BF4A-1EEDBA67A88C}");
            public static readonly string TemplateName = "IDescription";
            public static class FieldsID
            {
                public static readonly ID Description = new ID("{1F902CB6-743F-44F7-BDF4-160E2477EC73}");
            }
            public static class FieldsName
            {
                public static readonly string Description = "Description";
            }
        }
        public static class IBody
        {
            public static readonly ID TemplateID = new ID("{E5CAE71E-064D-4053-90CB-8720159A2DE5}");
            public static readonly string TemplateName = "IBody";
            public static class FieldsID
            {
                public static readonly ID Body = new ID("{5D8C0707-6E04-4051-9B8D-76E8A2D6483A}");
            }
            public static class FieldsName
            {
                public static readonly string Body = "Body";
            }
        }
        public static class ISummary
        {
            public static readonly ID TemplateID = new ID("{9D520C02-7A11-480D-8518-F5EDD7DCD343}");
            public static readonly string TemplateName = "ISummary";
            public static class FieldsID
            {
                public static readonly ID Summary = new ID("{6BB388D8-DEDF-432F-A0C0-C20AD0CC8A0D}");
            }
            public static class FieldsName
            {
                public static readonly string Summary = "Summary";
            }
        }
        public static class ILink
        {
            public static readonly ID TemplateID = new ID("{C5B2D402-6B0C-4A1F-A80D-E70D29E670D9}");
            public static readonly string TemplateName = "ILink";
            public static class FieldsID
            {
                public static readonly ID Link = new ID("{8CEDC322-2AA6-4EC6-8800-DF54977EC81B}");
            }
            public static class FieldsName
            {
                public static readonly string Link = "Link";
            }
        }

        #endregion

        #region Sitemap

        public static class SitemapItem
        {
            public static readonly ID TemplateID = new ID("{48061766-568B-46FD-8FCC-CDDB901AEE54}");
            public static readonly string TemplateName = "Sitemap item";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID SitemapTitle = new ID("{53D054CC-0143-41D7-9BEA-1CD2E9CD2020}");
                    public static readonly ID SitemapPriority = new ID("{A9463271-446D-4142-8630-1A7A195F0737}");
                    public static readonly ID IncludeItemInSitemap = new ID("{57BD8648-95D1-4088-B4FC-4EA45FC4EB67}");
                }
                public static class FieldsName
                {
                    public static readonly string SitemapTitle = "Sitemap Title";
                    public static readonly string SitemapPriority = "Sitemap Priority";
                    public static readonly string IncludeItemInSitemap = "Include Item In Sitemap";
                }

            }

        }
        public static class SitemapFolder
        {
            public static readonly ID TemplateID = new ID("{CEDF5904-4E3B-4757-9341-B92A9D429265}");
            public static readonly string TemplateName = "SitemapFolder";
        }

        public static class SitemapOtherLink
        {
            public static readonly ID TemplateID = new ID("{53E40690-8CD4-4C79-B0D8-B7F391DC6C93}");
            public static readonly string TemplateName = "Sitemap OtherLink";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID SitemapOtherLinkItems = new ID("{ADA580A1-B8F9-4D16-9CF5-F5D418924736}");

                }
                public static class FieldsName
                {
                    public static readonly string SitemapOtherLinkItems = "OtherLinkItems";

                }

            }
        }

        public static class SitemapOtherLinkFolder
        {
            public static readonly ID TemplateID = new ID("{10D70BC1-3D21-46F3-950E-0F1846D434BE}");
            public static readonly ID ItemID = new ID("{0B635622-6983-4E37-B762-18884DC4E14C}");
            public static readonly string TemplateName = "SitemapOtherLinkFolder";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID SitemapTitle = new ID("{F05D5A65-D93A-4C3A-9691-4E3FAC293470}");
                    public static readonly ID SitemapPriority = new ID("{ECA1267F-8AE3-4B1C-A0A1-27EE3FC5A81C}");

                }
                public static class FieldsName
                {
                    public static readonly string SitemapTitle = "Sitemap Title";
                    public static readonly string SitemapPriority = "Sitemap Priority";

                }

            }
        }

        #endregion

        #region Configuration

        public static class Configuration
        {
            public static readonly ID TemplateID = new ID("{0B2A0DEB-0810-431D-919C-6B9B87A68EA2}");
            public static readonly string TemplateName = "Configuration";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID ConfigurationItem = new ID("{B38F8237-A90C-49D4-ADAF-F5291CE290DC}");

                }
                public static class FieldsName
                {
                    public static readonly string ConfigurationItem = "ConfigurationItem";
                }

            }

        }
        public static class ConfigurationFolder
        {
            public static readonly ID TemplateID = new ID("{944F40A6-73E0-4537-B451-0A52FF701E65}");
            public static readonly string TemplateName = "ConfigurationFolder";
        }
        #endregion

        #region
        public static class SEOData
        {
            public static readonly ID IncludedInSItemap = new ID("{9A2BAE6F-EF8B-4267-BB95-8AC6979BCBD5}");
            public static class TemplateIdClass
            {
                public static readonly ID HomePagetemplateID = new ID("{778D2F65-2BAC-4F22-91AB-19BD6E88FC5F}");
                public static readonly ID ResidentialLandingTemplateID = new ID("{A6A604B2-2656-4E5C-952C-05279F72D014}");
                public static readonly ID CommercialLandingTemplateID = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
                //public static readonly ID TownshipLandingTemplateID = new ID("{22C02FBC-20BE-4F58-9E27-562588A523F8}");
                public static readonly ID ClubLandingTemplateID = new ID("{A91C40DF-382D-4D8B-B70B-748F34C9A717}");
                public static readonly ID Residential = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
                public static readonly ID Commercial = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
                public static readonly ID Blog = new ID("{082F055F-6F4E-4C2A-8B94-3F65E2055686}");
                public static readonly ID BLogCategory = new ID("{9C23584B-262A-4523-99BE-65B395431E18}");
                public static readonly ID BlogDetail = new ID("{2FBDA2B6-BA1B-472F-8040-3EB3059024C7}");
                public static readonly ID Township = new ID("{22C02FBC-20BE-4F58-9E27-562588A523F8}");
                public static readonly ID Club = new ID("{463784CA-A045-4C87-9EF5-3CE93772F4BD}");
                public static readonly ID LocationLandingID = new ID("{19FADD3A-572B-4BFE-ACB9-9649672754F0}");

            }

        }
        #endregion
        public static class Commondata
        {
            public static readonly ID ItemID = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");
            public static class Fields
            {
                public static readonly ID morearticlesID = new ID(" {88198DB8-21E4-4DF3-B655-9C0F7DC5EA79}");
                public static readonly ID blogLinkId = new ID(" {C0BC1F88-F412-4A77-A43B-1057F4B0A21F}");
                public static readonly ID SiteDomain = new ID("{15E638D2-40BD-41D9-A7C5-5A629F9BDED6}");

            }
        }

        public static class SEO
        {
            public static readonly ID TemplatId = new ID("{A6A32038-7FDC-4B6A-97B6-8147251BAD9D}");
            public static class Page
            {
                public static readonly ID PageID = new ID("{20B33133-18FA-4C8E-9E58-E112A0C97EF7}");
                public static readonly ID CityName = new ID("{A959C5FB-8880-4204-AD66-8007045DE1AF}");
            }

        }
    }
}