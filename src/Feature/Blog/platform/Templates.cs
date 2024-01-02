using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Blog.Platform
{
    public static class Templates
    {
        public static class BlogAnchorsTemplate
        {
            public static readonly ID ID = new ID("{9C23584B-262A-4523-99BE-65B395431E18}");
            public static class Fields
            {
                public static readonly ID Link = new ID("{115890C6-B02F-4AE2-979D-B1B76DAE97C0}");
                public static readonly ID Keyword = new ID("{CA5231DE-80EC-4990-9051-B23A10E9182D}");
                public static readonly ID BlogAnchorField = new ID("{7B4D3F83-22E5-43F6-8084-71F58863D101}");
                public static readonly ID HashData = new ID("{0B3BCDDA-7277-4455-9CC3-F75B018B98B0}");
                public static readonly ID SeeAllText = new ID("{470F568B-4E02-4565-9557-291FF605CAFD}");
                public static readonly ID OtherArticles = new ID("{E2560F35-6850-48F2-BDD4-4F669B1ECE2A}");
                public static readonly ID OtherArticlesTitle = new ID("{B59EA229-1176-4537-80EB-DA48C6557899}");
                public static readonly ID SlugText = new ID("{2928E0C6-2063-4203-8763-DF2B94918037}");
            }
        }
        public static class commonData
        {
            public static readonly ID TemplateID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID ItemID = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");

            public static class Fields
            {
                public static readonly ID blogLinkId = new ID(" {C0BC1F88-F412-4A77-A43B-1057F4B0A21F}");
            }
        }
        public static class CommunicationDisclaimer
        {
            public static readonly ID TemplateID = new ID("{5EF4202A-AC67-4605-BDC9-E8FA5679F0AE}");

            public static class Fields
            {
                public static readonly ID disclaimerID = new ID(" {1F902CB6-743F-44F7-BDF4-160E2477EC73}");
                public static readonly ID titleID = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");
            }
        }
        
    public static class Communication
        {
            public static readonly ID TemplateID = new ID("{082F055F-6F4E-4C2A-8B94-3F65E2055686}");
            public static readonly ID ItemID = new ID("{E20A905E-A436-4179-84A3-5A4221FE012E}");
            public static class Fields
            {
                public static readonly ID innerTemplateID = new ID("{2FBDA2B6-BA1B-472F-8040-3EB3059024C7}");
                public static readonly ID categoryID = new ID("{09F89371-10E6-48F6-B0D2-0A60442F09BB}");
                public static readonly ID PageTitle = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");

            }
        }
        public static class TitleDescription
        {
            public static readonly ID Id = new ID("{69AD3AEA-D785-4DB3-A69E-86A01336D914}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{F1940AF8-A61F-4219-BC68-A273E27220AD}");
                public static readonly ID Description = new ID("{C9286B52-A596-44A9-9ABC-2BA7C7357E2A}");
                public static readonly ID Link = new ID("{4713B4F7-731E-4FAF-84CE-04932DA67C10}");
            }
        }
        public static class BlogKeysTemplate
        {
            public static readonly ID ID = new ID("{8CECD9A0-322F-40B5-BE69-F413F862B6DC}");
            public static class Fields
            {
                public static readonly ID IsDefault = new ID("{40984027-82FC-4172-A71D-1B56538896EB}");
                public static readonly ID Link = new ID("{0F9229EC-3937-4FE9-B8C5-A60DC2656CBC}");
                public static readonly ID ImageSrc = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly ID Title = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID Heading = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly ID SubHeading = new ID("{261596D0-34C2-47CE-92B3-DD0911A40E0E}");
                public static readonly ID DateTime = new ID("{96553D7C-FDDF-4E27-8F65-3D2F62B548FC}");
                public static readonly ID CategoryLink = new ID("{0F9229EC-3937-4FE9-B8C5-A60DC2656CBC}");
                public static readonly ID CategoryTitle = new ID("{9CC1EC95-CA19-4ADA-8317-BA826ED1E1FE}");
                public static readonly ID ReadTime = new ID("{64A8B839-7563-4D8A-B222-B5C7DF229A05}");
                public static readonly ID CategoryHeading = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly ID LoadingText = new ID("{7493BAC2-CD2B-4149-8ED1-61C8E6FD0D00}");
                public static readonly ID PageCount = new ID("{31570B69-F4E7-4560-A90D-D36DD11BAC26}");
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

    }
}