using Sitecore.Data;

namespace Sitecore.AdaniSolar.Website
{
    public class Templates
    {
        public struct MailConfiguration
        {
            public struct SolarWarranty
            {
                public struct Fields
                {
                    public static readonly ID From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                    public static readonly ID Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                    public static readonly ID Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
                }
            }
        }
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

       
        public struct SalesForceCRMSettings
        {

            public static readonly ID Id = new ID("{0067EB49-F500-456F-BB00-F6D9CB4CE025}");
            public struct Fields
            {
                public static readonly ID ConsumerKey = new ID("{D226087A-612E-4D89-BC5B-AD1867C7944F}");
                public static readonly ID ConsumerSecret = new ID("{1CCAB73A-1C4A-4638-A092-C3981C8DC724}");
                public static readonly ID IsSandboxUser = new ID("{677DFE12-A901-4DEE-9096-01C68B21D539}");
                public static readonly ID Insta_SfdcSandboxEndPoint = new ID("{729F040F-43A9-472B-B5FB-790B0BFDA6F1}");
                public static readonly ID Insta_SfdcProductionEndPoint = new ID("{A3A0D596-48FE-4075-B301-8E2CAEE63B0A}");
                public static readonly ID TokenURL = new ID("{24407D4D-685E-4254-B67A-7102E1EF6D06}");
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
            public static readonly ID ID = new ID("{9D076B09-781C-452A-9FFB-6CA4C6AB6CAF}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{D9F6EF3F-9B9B-4F60-A1DE-A6A2BCA5A710}");
                public static readonly ID Summary = new ID("{6303B8BA-063A-466B-8258-C7F73AC62C34}");
                public static readonly ID Body = new ID("{40DAE50C-0866-41BF-A566-088E30DF5C1C}");
                public static readonly ID Image = new ID("{8DDD16D5-6989-45D2-8168-47382FBF5328}");
                public static readonly ID Icon = new ID("{A12A1A93-6EAE-40EB-B14E-664DA3FDB805}");
                public static readonly ID Link = new ID("{6F374F5D-E307-4058-88E3-0810C5BFDAC9}");


                public static readonly ID BackgroundImage = new ID("{7D849622-F574-435F-8D2C-AE1FF352DA70}");
                public static readonly ID Heading = new ID("{8BA2A590-D697-4503-A1DB-A59995976093}");
                public static readonly ID Text = new ID("{8BA2A590-D697-4503-A1DB-A59995976093}");
                public static readonly ID ExploreMore = new ID("{6DBE7C66-318F-46AC-AFC5-45FA85A14B4F}");
            }
        }
    }
}