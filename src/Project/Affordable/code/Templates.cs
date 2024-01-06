using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Affordable.Website
{
    public class Templates
    {
        public struct HappyCustomer
        {
            public static readonly ID ID = new ID("{5AA436E5-792B-4640-9979-D83960787831}");

            public struct Fields
            {
                public static readonly ID Description = new ID("{5D5E9B01-CD2F-4E44-9BBE-81D50FBCFF4A}");
                public static readonly ID Resident = new ID("{B4138C6D-CC71-4268-B6E9-6DEAB5A479C7}");
                public static readonly ID Name = new ID("{E0ACDF22-99E5-40F5-8696-43B64E0BFC94}");
                public static readonly ID Image = new ID("{A11CC8A8-57B7-4A9B-9CE0-E2B5B060B016}");
            }
        }
        public struct HasBannerSelector
        {
            public static readonly ID ID = new ID("{1BAD9726-A842-41DF-B087-9F5A7EDBD2EA}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{AD9B3FC2-2E52-45F7-A7B9-95CB6F1F21D4}");
                public static readonly ID Description = new ID("{9587876E-BD33-4BD3-A54A-BD68B367CEF2}");
                public static readonly ID Image = new ID("{4D2D7E85-08D4-4778-ACF9-7B7AD9048FE7}");
            }
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
        public struct LocationDetial
        {
            public static readonly ID ID = new ID("{1AD129EC-8477-4AFC-B678-498EB46B0D10}");
            public static readonly ID TempalateId = new ID("{FC8041CC-1213-4D45-8C14-191D4208CED4}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{EAD93781-22C3-47B9-8990-F98DB611F268}");
                public static readonly ID Address = new ID("{8F32F314-871F-41CB-B5FB-5075CB2FA126}");
                public static readonly ID Location = new ID("{EE0E7D95-E6AD-4536-A7EF-6B7530BC8746}");
                public static readonly ID LocationImage = new ID("{0AC78FF8-4FBA-4CEB-B19B-CE5A167110B5}");

            }
        }
        public struct LinkDetail
        {
            public static readonly ID ID = new ID("{0ADF2427-5B2E-4B17-ADE7-DDB93AF70C84}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{1D53B06E-5ADA-43CA-B590-5E92F3DB5A48}");
                public static readonly ID LinkUrl = new ID("{B4614851-3FCE-4354-A838-3AF49C3EDDAA}");
            }
        }
        public struct ProjectProperty
        {
            public static readonly ID ID = new ID("{78C98776-5133-40F5-86E0-DCD4B4F06228}");
            public struct Fields
            {
                public static readonly ID ProjectStatus = new ID("{506E8B3C-E980-44FC-A8B2-271FA92B264E}");
                public static readonly ID Capacity = new ID("{BB939C2E-A002-45D3-AF01-EAA227CE8D1B}");
                public static readonly ID Price = new ID("{9DE6DFCD-47BA-4BF9-B5DF-D2601750FEF6}");
                public static readonly ID Township = new ID("{15E70F4D-36A4-4E4E-BA07-CE88FE670F8B}");
                public static readonly ID Area = new ID("{75ED3130-1AE7-4241-9590-A63006AFB488}");
                public static readonly ID Location = new ID("{31DB1B8F-3836-4A9C-BDB3-A53B3BCF895E}");
                public static readonly ID Logo = new ID("{74C59F30-0FEA-465F-A0E4-A9EB9E888723}");
                public static readonly ID IsDisplayInSecondary = new ID("{B6727D6F-54D0-4303-8548-C1D51B8455C0}");
                public static readonly ID SimilarProperty = new ID("{0CFCB4B1-297A-424D-BB8A-005BA3F710B5}");
                public static readonly ID SiteStatus = new ID("{FD80A3E9-7BDF-4B66-8FE8-82ECE61DDDF1}");
                public static readonly ID ProjectId = new ID("{6A2179ED-15B8-4C42-8D65-C84F22C4C49E}");
            }


        }

        public struct AffordableLocations
        {
            public static readonly ID ID = new ID("{A12015EF-F068-4893-8C8B-7D449EA158F1}");
            public static readonly ID RootItem = new ID("{A3281CA9-89DF-4A3D-B33D-F56F3D9B2403}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{4834B282-F264-4B96-BFD0-4A66A4D52AF5}");
            }
        }

        public struct AffordableProject
        {
            public static readonly ID ID = new ID("{A12015EF-F068-4893-8C8B-7D449EA158F1}");
            public static readonly ID RootItem = new ID("{6BC956F8-B15B-4527-B354-99B4C9F3209F}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{4834B282-F264-4B96-BFD0-4A66A4D52AF5}");
            }
        }

        public struct AffordablePropertyTypeText
        {

            public static readonly ID TwoBHK = new ID("{B6DD7A54-3FB2-4D56-9A40-0F61C920E621}");
            public static readonly ID ThreeBHK = new ID("{BBA3CDEE-AB98-4FED-B7B6-0862605F3E02}");
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

        public struct SingleText
        {
            public static readonly ID ID = new ID("{B30E86B7-1AA5-47A5-97F3-4D35E54579D8}");

            public struct Fields
            {
                public static readonly ID Text = new ID("{4834B282-F264-4B96-BFD0-4A66A4D52AF5}");
                public static readonly ID Image = new ID("{FB4F7EAF-77E4-4F4E-A071-1B17E8588453}");
            }
        }
        //public struct _HasPageContent
        //{
        //    public static readonly ID ID = new ID("{AF74A00B-8CA7-4C9A-A5C1-156A68590EE2}");

        //    public struct Fields
        //    {
        //        public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
        //        public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
        //        public static readonly ID Body = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
        //        public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
        //    }
        //}

        public struct ResidentTopProperty
        {
            public static readonly ID ID = new ID("{6C85EC44-7F56-4F1C-86A9-A886CBCFA4B2}");

            public struct Fields
            {
                public static readonly ID Logo = new ID("{DF7F6D01-9F38-4EBB-BDD3-320E11C8048F}");
                public static readonly ID Title = new ID("{E200B013-6945-4010-8F04-774C19AE7557}");
                public static readonly ID Description = new ID("{660464DB-5E03-4BE1-B5D6-B372DF232E43}");
                public static readonly ID VideoThumb = new ID("{A6DA6915-6BD2-4472-98C9-83C1E2CA5F74}");
                public static readonly ID VideoLink = new ID("{39EBD241-E3B8-4EFA-8D40-D8723AF2E41E}");
                public static readonly ID LinkText = new ID("{87D8CDDC-F1E7-4838-B76F-729D0C6F09EA}");
                public static readonly ID LinkURL = new ID("{CD52CB6A-63F2-4AB2-B568-172D41E7EA99}");
            }
        }

        public struct AffordablePropertyType
        {
            public static readonly ID GlobalRootFolderID = new ID("{47FA4E52-1AB0-4DFE-9BB5-22965B097F49}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{4834B282-F264-4B96-BFD0-4A66A4D52AF5}");
            }
        }

        public struct AffordableProjectStatus
        {

            public static readonly ID GlobalRootFolderID = new ID("{EC08C6EA-8477-405F-B44C-7FF897DC464D}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{4834B282-F264-4B96-BFD0-4A66A4D52AF5}");
            }
        }

        public struct AffordableSearchPage
        {
            public static readonly ID ID = new ID("{DE78FE51-F1AA-4BCF-B75F-6AD9138EA7FF}");
        }

        public struct SalesForce
        {

            public static readonly ID Id = new ID("{9E003BF5-1E8E-433C-B2C4-367F0BE15B38}");
            public struct Fields
            {
                public static readonly ID SecurityToken = new ID("{4834B282-F264-4B96-BFD0-4A66A4D52AF5}");
                public static readonly ID ConsumerKey = new ID("{D226087A-612E-4D89-BC5B-AD1867C7944F}");
                public static readonly ID ConsumerSecret = new ID("{1CCAB73A-1C4A-4638-A092-C3981C8DC724}");
                public static readonly ID Username = new ID("{EE269E05-867E-46F8-A8D0-0FADCAE252C9}");
                public static readonly ID Password = new ID("{E4C2FF9F-F862-42B1-9213-DB2663CB6577}");
                public static readonly ID IsSandboxUser = new ID("{677DFE12-A901-4DEE-9096-01C68B21D539}");
                public static readonly ID Insta_SfdcSandboxEndPoint = new ID("{729F040F-43A9-472B-B5FB-790B0BFDA6F1}");
                public static readonly ID Insta_SfdcProductionEndPoint = new ID("{A3A0D596-48FE-4075-B301-8E2CAEE63B0A}");
            }
        }

        public struct PaymentConfiguration
        {
            public static readonly ID Id = new ID("{931EE813-19DB-41F5-A5E2-40F23D952BD1}");

            public struct Insta_Mojo
            {
                public static readonly ID Insta_client_id = new ID("{F581F834-1205-433F-B200-466D93F613BD}");
                public static readonly ID Insta_client_secret = new ID("{25A5CFA8-1631-42EA-A186-27B676DD0DFA}");
                public static readonly ID Insta_Endpoint = new ID("{386B1D43-5586-4D15-9876-C132B2C23A35}");
                public static readonly ID Insta_Auth_Endpoint = new ID("{B6D82681-4195-4A50-A3F1-D519847B51C5}");
                public static readonly ID grant_type = new ID("{DB7A2BAB-29C8-4AA7-85AB-FF6F599C0043}");
                public static readonly ID Insta_Redirect_Url = new ID("{A07803E6-8C51-4987-BD8D-D91A13DCCA50}");
            }
        }

        public struct Properties
        {
            public struct Datasource
            {
                public static readonly ID Projects = new ID("{9C204C27-46C4-4B3B-903C-51AC1D076C74}");
            }
        }
    }
}