using Sitecore.Data;

namespace Sitecore.Adani.Website
{
    public class Templates
    {
        public struct NavigationBar
        {
            public static readonly ID ID = new ID("{ED211CFC-8DB9-4688-8834-AF4EFD5C4BEB}");

            public struct Fields
            {
                public static readonly ID Logo = new ID("{B9C2150E-0D0C-4EA1-BA62-0ED5D6FB81FE}");
                public static readonly ID Title = new ID("{C2078AB7-2B96-4ADD-B54F-5E6AC9207C5A}");
                public static readonly ID Description = new ID("{DE37FC6B-222E-46D2-A252-67E32D6A364B}");
                public static readonly ID VideoThumb = new ID("{43CF7251-2789-4A35-BE35-DB8B33D78795}");
                public static readonly ID VideoLink = new ID("{49A09990-AAB8-4BD1-B07E-67B0F23D9B9C}");
                public static readonly ID LinkText = new ID("{FE5A056F-82E7-432A-8798-996DEA92623C}");
                public static readonly ID LinkURL = new ID("{555EEE4B-86A7-4BC3-82DC-A5BBBEACAEEE}");
            }
        }

        public struct LinkMenuItem
        {
            public static readonly ID ID = new ID("{18BAF6B0-E0D6-4CCE-9184-A4849343E7E4}");

            public struct Fields
            {
                public static readonly ID Icon = new ID("{2C24649E-4460-4114-B026-886CFBE1A96D}");
                public static readonly ID DividerBefore = new ID("{4231CD60-47C1-42AD-B838-0A6F8F1C4CFB}");
                public static readonly ID Image = new ID("{ECC76733-D0D9-48D6-B55E-293FE5B13EA6}");
                public static readonly ID ShortDesc = new ID("{92A73427-1B36-4BCF-8F6F-9F72C350C65D}");
                public static readonly ID BGCSS = new ID("{4CB29455-78C6-4998-8A83-73C19AE52474}");
                public static readonly ID LinkToSection = new ID("{2DE66FB6-840D-4708-9D00-CEAF062955FA}");
                public static readonly ID ActiveImage = new ID("{C0947EAD-9FAC-4F79-940F-5E5B33B9930A}");
                public static readonly ID TagLine = new ID("{5D8603E0-F2C0-4958-A0E0-B4378564FD1B}");
                public static readonly ID VideoThumb = new ID("{6E5E40E5-EE0D-449F-85DF-9D077CDBF44C}");
                public static readonly ID Title = new ID("{1B483E91-D8C4-4D19-BA03-462074B55936}");
            }
        }

        public struct Navigable
        {
            public static readonly ID ID = new ID("{A1CBA309-D22B-46D5-80F8-2972C185363F}");

            public struct Fields
            {
                public static readonly ID ShowInNavigation = new ID("{5585A30D-B115-4753-93CE-422C3455DEB2}");
                public static readonly ID NavigationTitle = new ID("{1B483E91-D8C4-4D19-BA03-462074B55936}");
                public static readonly ID ShowChildren = new ID("{68016087-AA00-45D6-922A-678475C50D4A}");
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public static readonly ID ShowInNewTab = new ID("{10FF37C5-B368-4FAA-89C6-48432D9EBAA0}");
                public static readonly ID Link = new ID("{719E9180-72AB-4592-BE8B-B24A3F28554C}");
                public static readonly ID IsExternalLink = new ID("{40AC25E7-2AFB-4D57-A831-7855C433E1C8}");
            }
        }

        public struct LinkMenu
        {
            public static readonly ID ID = new ID("{AC7394D5-2AA7-4FDB-A7D8-B1B87F9FA661}");
        }

        public struct HasMediaImage
        {
            public static readonly ID ID = new ID("{2DB11E19-02FC-4458-995D-0A3F9C1D3F45}");

            public struct Fields
            {
                public static readonly ID Image = new ID("{9F51DEAD-AD6E-41C2-9759-7BE17EB474A4}");

            }
        }

        public struct _HasMedia
        {
            public static readonly ID ID = new ID("{A44E450E-BA3F-4FAF-9C53-C63241CC34EB}");
            public static readonly ID FolderIdForComInnerFeatures = new ID("{0437FEE2-44C9-46A6-ABE9-28858D9FEE8C}");
            public struct Fields
            {
                public static readonly ID HighResolutionImage = new ID("{9F51DEAD-AD6E-41C2-9759-7BE17EB474A4}");
                public static readonly ID Title = new ID("{63DDA48B-B0CB-45A7-9A1B-B26DDB41009B}");
                public static readonly ID Description = new ID("{302C9F8D-F703-4F76-A4AB-73D222648232}");
                public static readonly ID Thumbnail = new ID("{4FF62B0A-D73B-4436-BEA2-023154F2FFC4}");
                public static readonly ID MediaLink = new ID("{EF85F9C6-11B7-40C2-8173-336D79D70E13}");
            }
        }

        public struct _HasPageContent
        {
            public static readonly ID ID = new ID("{AF74A00B-8CA7-4C9A-A5C1-156A68590EE2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public static readonly ID Body = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
                public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
            }
        }

        public struct SingleText
        {
            public static readonly ID ID = new ID("{3742F700-308E-4A97-B524-4274E2ACCBB1}");

            public struct Fields
            {
                public static readonly ID Text = new ID("{E4059B29-8F8D-4F8B-B265-4F9917EFC4B6}");
                public static readonly ID Image = new ID("{89651F3D-7AF2-4471-A3B8-2385F977A988}");
            }
        }

        public struct LinkDetail
        {
            public static readonly ID ID = new ID("{2C22F024-CEBA-4C3B-AB32-631794D7DAC1}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{37284822-EA3A-4B08-95AD-512102C281BD}");
                public static readonly ID LinkUrl = new ID("{276ED1EA-D4A4-4247-B2D2-5C51ECAC66EB}");
            }
        }

        public struct HasPageContent
        {
            public static readonly ID ID = new ID("{AF74A00B-8CA7-4C9A-A5C1-156A68590EE2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
                public const string Title_FieldName = "Title";
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public const string Summary_FieldName = "Summary";
                public static readonly ID Body = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
                public const string Body_FieldName = "Body";
                public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
            }
        }

        public struct Businesses
        {
            public static readonly ID ID = new ID("{8AE7425F-2808-4FF7-BC7F-BAB984AB2A4C}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{A4A406D1-5556-482B-929E-64FFAA9BE54D}");
                public static readonly ID Summary = new ID("{F3E4EC25-1740-4044-8E0A-83F11D7B95C9}");
                public static readonly ID Body = new ID("{F3E4EC25-1740-4044-8E0A-83F11D7B95C9}");
                public static readonly ID Image = new ID("{A4A29F58-C8C2-4E28-848D-A5BB1640AEDE}");
                public static readonly ID Icon = new ID("{1ED3FAAE-D04B-4A45-808F-70DF5BFA883B}");
                public static readonly ID Link = new ID("{57930462-D11B-4368-B2E6-A8DA1CB40222}");


                public static readonly ID BackgroundImage = new ID("{918FBE69-5F7C-41DE-81A4-79AA6A2021DD}");
                public static readonly ID Heading = new ID("{BD416B24-64AC-452A-B074-55A706B03C66}");
                public static readonly ID Text = new ID("{AAEA1746-E62D-458A-ABAE-7624C2BD1730}");
                public static readonly ID ExploreMore = new ID("{9ACA63BA-8AE8-4616-A8A1-179CF28A507B}");
            }
        }

        
    }
}