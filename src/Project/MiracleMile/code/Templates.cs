using Sitecore.Data;

namespace Sitecore.MiracleMile.Website
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

        public struct SalesForce
        {

            public static readonly ID Id = new ID("{F24CE2BE-0746-49E2-86F2-D8EE5923DA8E}");
            public struct Fields
            {
                public static readonly ID ConsumerKey = new ID("{D226087A-612E-4D89-BC5B-AD1867C7944F}");
                public static readonly ID ConsumerSecret = new ID("{1CCAB73A-1C4A-4638-A092-C3981C8DC724}");
                public static readonly ID Username = new ID("{EE269E05-867E-46F8-A8D0-0FADCAE252C9}");
                public static readonly ID Password = new ID("{E4C2FF9F-F862-42B1-9213-DB2663CB6577}");
                public static readonly ID IsSandboxUser = new ID("{677DFE12-A901-4DEE-9096-01C68B21D539}");
                public static readonly ID Insta_SfdcSandboxEndPoint = new ID("{729F040F-43A9-472B-B5FB-790B0BFDA6F1}");
                public static readonly ID Insta_SfdcProductionEndPoint = new ID("{A3A0D596-48FE-4075-B301-8E2CAEE63B0A}");
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

        public struct HasBannerSelector
        {
            public static readonly ID ID = new ID("{206B7C63-02E0-4B0C-9403-2E0751096A69}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{60AB17E5-F66D-407B-B796-C6D7C8641CD4}");
                public static readonly ID Description = new ID("{68D496BE-F46E-40A1-B06E-A8ED480D420D}");
                public static readonly ID Image = new ID("{3AD864D0-19A0-4A96-BE93-58D93A53924B}");
            }
        }
        public struct ResidentTopProperty
        {
            public static readonly ID ID = new ID("{438C2AEC-BC00-4A47-AF47-40B3862D2D33}");

            public struct Fields
            {
                public static readonly ID Logo = new ID("{3B16E207-996E-4354-8EEB-F38F41247DA7}");
                public static readonly ID Title = new ID("{F5268537-AD3F-46E1-B1F9-C815280314EE}");
                public static readonly ID Description = new ID("{40410EC8-A341-4F0F-90AB-47203B60DC6D}");
                public static readonly ID VideoThumb = new ID("{6F99FD1A-ED3C-44E0-8644-8B025DA15368}");
                public static readonly ID VideoLink = new ID("{369346A9-D078-4489-8DF1-E11A885E4F74}");
                public static readonly ID LinkText = new ID("{751A155A-5AB3-446C-9246-221800575EDD}");
                public static readonly ID LinkURL = new ID("{FF693EDE-C4F7-4FCE-AD3E-AD943C3FDD42}");
            }
        }
        public struct ResidentialProjects
        {
            public static readonly ID ID = new ID("{B34DAAA4-B562-4063-96B3-42F430BA418E}");
            public struct Fields
            {
                public static readonly ID ProjectStatus = new ID("{C36CF680-7445-486D-9F4D-948F22BF91B1}");
                public static readonly ID Capacity = new ID("{B2E9CDD0-1D1D-4409-892D-7A0FE3B5F875}");
                public static readonly ID Price = new ID("{A42F5BCA-3007-4D0D-A2D8-EEA8B713B9FC}");
                public static readonly ID Township = new ID("{83BD2517-2A27-4D92-B97B-128F9C014907}");
                public static readonly ID Area = new ID("{549C9EC5-F6AE-4AE5-B4DE-195D2FE10777}");
                public static readonly ID Location = new ID("{C8FF2B82-90B4-4828-888C-96E63434D844}");
                public static readonly ID Logo = new ID("{C84D2FD3-A591-4236-BD79-0BBA3ADD7832}");
                public static readonly ID IsDisplayInSecondary = new ID("{8635C694-9B62-40FC-8CF1-F2842BA34518}");
                public static readonly ID SimilarProperty = new ID("{10FA41C2-0F35-4655-8824-6DBD0F228A0F}");
                public static readonly ID SiteStatus = new ID("{8DAEF480-7D35-4CE8-A285-627038D3BD47}");
            }


        }

        public struct LocationDetial
        {
            public static readonly ID ID = new ID("{8D47C002-098B-49E6-84EE-4E0490B4516B}");
            public static readonly ID TempalateId = new ID("{299E3144-CC04-424F-9963-1D6970AA681B}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{9BEC8452-3DE8-49C8-A9B0-6CCCF2C88B01}");
                public static readonly ID Address = new ID("{08B72530-FF74-485D-BE33-2C677B85E002}");
                public static readonly ID Location = new ID("{7BEF369A-22B8-4429-9BFD-5EB3B0EE6E2D}");
                public static readonly ID LocationImage = new ID("{885A6C4A-B919-4A0C-A9C1-C6A5F4A8FF40}");

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
        //public struct SingleText
        //{
        //    public static readonly ID ID = new ID("{3742F700-308E-4A97-B524-4274E2ACCBB1}");

        //    public struct Fields
        //    {
        //        public static readonly ID Text = new ID("{E4059B29-8F8D-4F8B-B265-4F9917EFC4B6}");
        //        public static readonly ID Image = new ID("{89651F3D-7AF2-4471-A3B8-2385F977A988}");
        //    }
        //}

        public struct LinkDetail
        {
            public static readonly ID ID = new ID("{2C22F024-CEBA-4C3B-AB32-631794D7DAC1}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{37284822-EA3A-4B08-95AD-512102C281BD}");
                public static readonly ID LinkUrl = new ID("{276ED1EA-D4A4-4247-B2D2-5C51ECAC66EB}");
            }
        }

        public struct CommercialProperty
        {
            public static readonly ID ID = new ID("{6E504458-4747-4A0E-8A46-483F03E8D0C4}");

            public struct Fields
            {
                public static readonly ID ProjectStatus = new ID("{11772132-EF04-4104-9BFC-6378B6832D65}");
                public static readonly ID Capacity = new ID("{347A1957-2253-4FBC-BD85-D92E868EC2DE}");
                public static readonly ID Price = new ID("{CC08D57F-D530-4FEA-9DDB-329C87451950}");
                public static readonly ID Township = new ID("{8AE049B1-0D45-4F09-9C72-FF7B451CAD8B}");


                public static readonly ID Area = new ID("{4F23D095-4D63-4CFD-9EED-33F7C334D83B}");
                public static readonly ID Location = new ID("{7AB2A9A5-8AC1-45CF-8D57-936EB686B8BA}");
                public static readonly ID Logo = new ID("{56B00537-72B9-4586-8F56-2D535BE65215}");
                public static readonly ID IsDisplayInSecondary = new ID("{62894E89-5B32-4A26-BA63-5DEA64999267}");
                public static readonly ID SimilarProperty = new ID("{C7A25A1E-AA83-4E18-9087-FB29EEBED7FA}");
                public static readonly ID SiteStatus = new ID("{0257D0D4-D463-45C0-B9A7-5813FFED46E8}");
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

        public struct HasRealtyMediaSelector
        {
            public static readonly ID ID = new ID("{6B3FEECC-4E5D-4F9E-B3B4-EC777BCA98EC}");

            public struct Fields
            {
                public static readonly ID MediaSelector = new ID("{A2DE79AC-16A1-44DA-B9C6-0A0A3B6EADA2}");
                public static readonly ID IsBanner = new ID("{95CF4DE4-D47E-4A01-87CC-BFF4DB674289}");
                public static readonly ID Link = new ID("{D051771E-DBC0-453E-9E4C-975324090CB3}");
                public static readonly ID Title = new ID("{92C8A510-2E56-45A3-B7E9-B5FE239C718C}");
                public static readonly ID cssDivClass = new ID("{1E2B30B6-0018-4F90-B7F3-9DE1076A6FF4}");
            }
        }

        public struct RealityProjectLinks
        {
            public static readonly ID Residential = new ID("{A8D66B86-1A80-4A6D-9380-C29284A79D7E}");
            public static readonly ID Commercial = new ID("{6844D2B4-BA62-43DD-BB99-6808F40D2E26}");
        }
        public struct Clubs
        {
            public static readonly ID ID = new ID("{054F8EE4-F03B-455B-8C7F-E940E800A6A8}");
            public struct Fields
            {
                public static readonly ID Logo = new ID("{8D807D99-481E-4743-932E-857133E6A49F}");
                public static readonly ID Location = new ID("{F4700466-A43F-4D78-8F97-88B3E98884D7}");
                public static readonly ID IsDisplayInSecondaryMenu = new ID("{AC1FDDEA-4CD4-4A8E-B756-D270BB94E5FF}");
                public static readonly ID SimilarProperty = new ID("{B18AFC40-BA39-412C-9454-C3052E72B6D4}");
                public static readonly ID ClubTitle = new ID("{39C5B993-7DAF-4780-A53F-F9ABDABC165D}");
                public static readonly ID ClubDescription = new ID("{62065BD4-F184-41B8-9CFB-1A79AD675E51}");
                public static readonly ID Township = new ID("{4EC2A1B1-B5A5-48E4-8112-DCCCA99849E1}");
            }
        }

        public struct RealtyType
        {
            public static readonly ID GlobalRootFolderID = new ID("{72DFBB5D-1A31-4277-9802-2E5BA24D9060}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{E4059B29-8F8D-4F8B-B265-4F9917EFC4B6}");
            }
        }

        public struct RealtyProjectStatus
        {

            public static readonly ID GlobalRootFolderID = new ID("{E537EBF7-16B6-4E2A-A076-21041FB71816}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{E4059B29-8F8D-4F8B-B265-4F9917EFC4B6}");
            }
        }

        public struct RealtyBudget
        {
            public static readonly ID GlobalRootFolderID = new ID("{2A08A705-C2CB-4A6A-A732-A3284E02B514}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{C47EF478-ACA1-40AA-B66E-71ED38D23E8D}");
                public static readonly ID Value = new ID("{FC571102-8E15-498A-9FC4-58E40A8134BC}");
            }
        }



        public struct RealtyArea
        {
            public static readonly ID GlobalRootFolderID = new ID("{DC9FB4E2-0639-4344-9E76-833649D4B7EF}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{C47EF478-ACA1-40AA-B66E-71ED38D23E8D}");
                public static readonly ID Value = new ID("{FC571102-8E15-498A-9FC4-58E40A8134BC}");
            }
        }

        public struct RealtySearchPage
        {
            public static readonly ID ID = new ID("{0FFD2B2C-356A-4E26-B649-4267A7B63108}");
        }


        public struct CommercialMediaSelector
        {
            public static readonly ID ID = new ID("{77A32ACD-32EC-4FD3-9D28-FBF9126FA5EC}");
            public struct Fields
            {
                public static readonly ID Description = new ID("{40FA72C5-63DF-4C60-A8AA-04C05BABBB3A}");
                public static readonly ID MediaSelector = new ID("{6327A0E1-EE78-4ACA-8183-3B003B9CA235}");
                public static readonly ID Link = new ID("{4588B1D8-11B7-4C1D-95BE-3A765B9B627C}");
                public static readonly ID cssClass = new ID("{95047053-737F-45CC-87A7-3C426F774C07}");
            }
        }
        public struct HasRealityMediaImage
        {
            public static readonly ID ID = new ID("{873E09C3-FCC3-4970-B3BE-1E1BDE0FE375}");
            public struct Fields
            {
                public static readonly ID Image = new ID("{71FF6029-CAD2-4128-ABBC-7342C118B79A}");
                public static readonly ID IsOverlay = new ID("{D9B844B8-A365-4E83-8E82-D11FAD7714E7}");
                public static readonly ID OverlayText = new ID("{12BEA075-CCBB-47BF-BF52-622827313AC4}");
                public static readonly ID Link = new ID("{268F6F3B-8104-4235-8493-5C5FC0E3D6A0}");
            }
        }

        public struct TestimonialOfHappyCustonmers
        {
            public static readonly ID ID = new ID("{54414AC9-6641-4D0E-9D24-C42258B28255}");

            public struct Fields
            {
                public static readonly ID Header = new ID("{3817C302-0844-485E-B05D-614EB18EDFB9}");
                public static readonly ID CustomerDetails = new ID("{0023FC41-913B-4ED8-BF4C-5F0D88AC51FF}");
            }
        }

        public struct RealtyPropertyTypeText
        {

            public static readonly ID Commercial = new ID("{DCB89949-DCD5-4925-9876-946850309EDE}");
            public static readonly ID Residential = new ID("{FAB2D3AD-AABE-43EF-9EA5-29D2CB9A4726}");
        }

        public struct RealtyLocations
        {
            public static readonly ID ID = new ID("{3742F700-308E-4A97-B524-4274E2ACCBB1}");
            public static readonly ID RootItem = new ID("{C5E1D984-1FDA-432F-8B67-C5ED0A796E3C}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{E4059B29-8F8D-4F8B-B265-4F9917EFC4B6}");
            }
        }
        public struct ThankYouPage
        {
            public static readonly ID ID = new ID("{8FB5B7CB-7677-46DA-A85E-EB2F45182E2D}");
           
        }

        public struct RealtyProject
        {
            public static readonly ID ID = new ID("{3742F700-308E-4A97-B524-4274E2ACCBB1}");
            public static readonly ID RootItem = new ID("{94A9B5CD-46C7-45B9-9E81-976A112EAA38}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{E4059B29-8F8D-4F8B-B265-4F9917EFC4B6}");
            }
        }

        public struct LeadGeneration
        {
            public static readonly ID LogOut = new ID("{3FE95DC4-687B-41BF-B102-87A0BF58AE6D}");
            public static readonly ID LeadCreation = new ID("{DE12254B-A653-425C-9BCB-B53B5C4AA86A}");
            public static readonly ID ThankYou = new ID("{233251E4-8F6A-4715-AFBC-FFE20B0CC808}");
        }

        public struct PaymentConfiguration
        {
            public static readonly ID ID = new ID("{825E603D-240C-4D6E-8068-F54A6DF6571D}");
            public struct BillDeskFields
            {
                public static readonly ID BDSK_Request_URL = new ID("{FAB9B864-B97A-402B-92EE-E10F77084DF9}");
                public static readonly ID BDSK_Merchant_ID = new ID("{8CB28E8A-D02C-44D4-9B3B-C893F5F986A8}");
                public static readonly ID BDSK_SECURITY_ID = new ID("{AE90154D-E932-4EB1-A1B7-B15AC9489500}");
                public static readonly ID BDSK_CURRENCY_TYPE = new ID("{A37979D1-602F-4B8B-B656-FA4871794B5C}");
                public static readonly ID BDSK_Resp_URL_B2B = new ID("{DEA41781-3821-413F-B2F2-A73BFB520DF9}");
                public static readonly ID BDSK_Resp_URL_S2S = new ID("{1001A3D3-FD64-46D7-9EE9-DC5662FBB0D0}");
                public static readonly ID BDSK_Req_Msg = new ID("{0FAE641C-6E5A-4DC8-8D27-BA2FE84C8206}");
                public static readonly ID BDSK_ChecksumKey = new ID("{30479ECD-ADCA-4171-ABD9-8C3E3B8E2520}");
            }
            public struct BillDeskFieldsAEMPL
            {
                public static readonly ID BDSK_Request_URL = new ID("{9507CAA9-B459-4ECE-B501-21BE617A60C5}");
                public static readonly ID BDSK_Merchant_ID = new ID("{EE616AE6-FC66-4A6A-AD2C-EBBD0BDD38E3}");
                public static readonly ID BDSK_SECURITY_ID = new ID("{AD7C4FCB-1062-4EAC-8EA4-0529D738221A}");
                public static readonly ID BDSK_CURRENCY_TYPE = new ID("{8B75F285-4A73-4BB7-A7DC-E60639DE65A0}");
                public static readonly ID BDSK_Resp_URL_B2B = new ID("{A5C09E03-4E2C-453B-B451-7B74157C610F}");
                public static readonly ID BDSK_Resp_URL_S2S = new ID("{DA084135-FB5C-4159-9DDA-73CAAEB831E0}");
                public static readonly ID BDSK_Req_Msg = new ID("{FEE0F7E6-DD02-4AD1-8429-074F60235AF5}");
                public static readonly ID BDSK_ChecksumKey = new ID("{D16332F1-8C0B-4FE9-8F2F-C0DA35264028}");
            }
        }

        public struct MailConfiguration
        {
            public static readonly ID MailConfigurationItemID = new ID("{ADC0E6D7-218A-487E-9AD1-9731BA7F9756}");
            public struct MailConfigurationFields
            {
                public static readonly ID Customer_SubjectName = new ID("{D01B3698-973C-45E7-B0DC-A6B6F457A6CB}");
                public static readonly ID Customer_MailFrom = new ID("{16D01757-664B-4AA2-A8DF-38B9561D3528}");
                public static readonly ID Customer_SuccessMessage = new ID("{CF2E6F3A-8CC4-4CEC-BD1C-BE42C6277F9F}");
                public static readonly ID Customer_FailureMessage = new ID("{9E27E35A-277C-4A9D-A0DE-7BBE82809E85}");
                public static readonly ID Officials_SubjectName = new ID("{673563DB-6DE9-478E-8BB2-15D4A3590F2B}");
                public static readonly ID Officials_RecipientMail = new ID("{72095B6C-D8E5-4DBD-910F-136BB92AC7D6}");
                public static readonly ID Officials_Message = new ID("{3DB52139-5A36-45F6-8C94-9651FFF6FFE2}");
                public static readonly ID Officials_MailFrom = new ID("{5540C9DA-FABC-48AE-B91B-884C8E8CB829}");
            }
        }

        public struct Pages
        {
            public static readonly ID PaymentSuccess = new ID("{A12576D4-D3EC-4E68-A875-2095A0FCAFC0}");
            public static readonly ID PaymentFailure = new ID("{EC47A408-8B4D-4419-AFE8-EB7BE5E77DD8}");
            public static readonly ID PaymentVDS = new ID("{35809AED-3C9D-44D7-806D-84FC35420874}");
            public static readonly ID PropertyList = new ID("{8CFDD7DA-3B9E-4B81-BC89-684C8F6521BF}");
        }
    }

}

