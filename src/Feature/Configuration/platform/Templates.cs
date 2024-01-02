using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform
{
    public static class Templates
    {
        public static class SEODataTemplate
        {
            public static class PageTemplateFields
            {
                public static readonly ID metatitleID = new ID("{25AB7141-4733-4124-9E83-6575A40CFD0D}");
                public static readonly ID MetaDescription = new ID("{C06D2A54-0763-4F64-BFF3-CE17F2FC7786}");
                public static readonly ID MetaKeywords = new ID("{32EB5D84-731E-4586-9236-FD59FB78BC64}");
                public static readonly ID PageTitle = new ID("{71857051-D7A9-4021-9203-FD92BCAE230C}");
                public static readonly ID OgTitle = new ID("{54E46F9D-3C92-4F33-B5CF-F336C1E5E5AE}");
                public static readonly ID RobotsTags = new ID("{1F0F65BC-B4E8-4FF8-BD6C-C76B93425238}");
                public static readonly ID BrowserTitle = new ID("{1F24877A-68EC-4F77-AF8F-4B2085422ACE}");
                public static readonly ID OgImage = new ID("{525CF566-41C3-46C3-9BEC-445F17C415CD}");
                public static readonly ID OgDescription = new ID("{804541EA-7760-41CD-BADE-0A782D5F1814}");
                public static readonly ID OgKeyword = new ID("{993ABEBF-7059-4A46-A100-CC577BD7A674}");
                public static readonly ID canonicalUrl = new ID("{65D8BB4F-42FD-4052-B07B-D7E4A9C8CB74}");
                public static readonly ID googleSiteVerification = new ID("{5AEC5597-7F11-4D72-920E-7FC48171128B}");
                public static readonly ID msValidate = new ID("{1FB9DCC9-4C4D-4B03-94D1-E32A08F2956A}");
                public static readonly ID telephone = new ID("{3DA198C7-BA30-4113-8512-0C2B1EC89B3A}"); 
                public static readonly ID contactType = new ID("{559E9ABC-BF18-43DB-B058-726AB13535E0}");
                public static readonly ID areaServed = new ID("{F44D864A-1156-48AF-AE26-65F56BF41F15}");
                public static readonly ID streetAddress = new ID("{93B6FCAC-41ED-4C1A-8D06-75D6F87FC149}");
                public static readonly ID addressLocality = new ID("{00981D92-AFD2-4BB0-87F4-4056085CE5CA}");
                public static readonly ID addressRegion = new ID("{50CAFECE-A28E-4859-AF36-FD762E509A38}");
                public static readonly ID postalCode = new ID("{076CF76F-FAC8-4A19-8E9F-5DC94A69D879}");
                public static readonly ID sameASTemplateID = new ID("{0E08FABB-6754-4332-A505-E6CF71F1F16F}");
                public static readonly ID ContactOption = new ID("{7A0F92A3-F637-448E-91F7-D6957E044132}");
                public static readonly ID Logo = new ID("{51C6BD92-03A6-4DB1-BA93-B2183BBE0527}");
                public static class Fields
                {
                    public static readonly ID navLink = new ID("{8CEDC322-2AA6-4EC6-8800-DF54977EC81B}");
                }
            }
        }
        public static class Image
        {
            public static readonly ID Id = new ID("{494F095B-B942-46AF-AFDC-6833DC5504A1}");

            public static class Fields
            {
                public static readonly ID Thumb = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly ID ImageType = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");
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
                public static readonly ID imgType = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");
            }
        }
        public static class commonData
        {
            public static readonly ID TemplateID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID ItemID = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");

            public static class Fields
            {
                public static readonly ID blogLinkId = new ID(" {C0BC1F88-F412-4A77-A43B-1057F4B0A21F}");
                public static readonly ID SiteDomain = new ID("{15E638D2-40BD-41D9-A7C5-5A629F9BDED6}");
            }
        }
        public static class CommmonData
        {
            public static readonly ID id = new ID("{E4ABE0B9-1E2E-446C-B828-FDC52A8CA0A4}");
            public static class Fields
            {
                public static readonly ID WhyAdani = new ID("{6D3053F2-B5F1-45AA-B644-D814D2FC60D7}");
                public static readonly ID OurStories = new ID("{F32ED9A8-41BF-4A0B-9CB8-2138C69E5838}");
                public static readonly ID ClubEvents = new ID("{F23B3F03-840D-4B14-87A6-D25C3FB7A9C3}");
                public static readonly ID OurStory = new ID("{F27F434F-431E-47CA-860F-E9FCB03DDB1F}");
                public static readonly ID HeaderInner = new ID("{3BE223B9-625A-4F07-AA9A-4E827F01460D}");
                public static readonly ID OurValues = new ID("{B7DDCA14-6C40-468F-9D74-547ED3602D4A}");
                public static readonly ID Configuration = new ID("{CAE464A3-42DE-4B3F-8FF8-1D5A2964741D}");
                public static readonly ID ProjectPropertyData = new ID("{6988E9F2-864E-4F81-9439-0211BA0B5306}");
                public static readonly ID Title = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");

            }
        }
        public static class ConfigurationTemplate
        {
            public static readonly ID id = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{09986CB8-AD9C-4B95-87C2-537E74B2044C}");
                public static readonly ID Keys = new ID("{C3B3FED7-3689-42D7-B37A-B21128DD27F5}");
            }
        }

        public static class LinkandKeyword
        {
            public static readonly ID Id = new ID("{D1B7CD65-6321-46D4-8210-0031CA094037}");
            public static class Fields
            {
                public static readonly ID Link = new ID("{115890C6-B02F-4AE2-979D-B1B76DAE97C0}");
                public static readonly ID Keyword = new ID("{CA5231DE-80EC-4990-9051-B23A10E9182D}");
            }
        }
        public static class CommonTextTemp
        {
            public static readonly ID id = new ID("{0A1F3BB4-7D5C-4D33-A44F-DA4B2CB7E125}");
            public static class Fields
            {
                public static readonly ID seeall = new ID("{30801654-721A-48B4-928A-38277073B316}");
                public static readonly ID projectResidential = new ID("{DF6C8091-004B-4243-8F5D-E7D443BCF099}");
                public static readonly ID commercialProjects = new ID("{BF76D3F0-B1AA-4475-8E34-6CCB279A0252}");
                public static readonly ID all = new ID("{E45C3736-0045-4F70-A86C-C7746FD3BC88}");
                public static readonly ID readytomove = new ID("{7083A984-9BFE-4898-B91C-32EB02EF6D3A}");
                public static readonly ID underconstruction = new ID("{1CE2D8C4-4B09-4772-A091-31769F6D1415}");
                public static readonly ID commInOtherCity = new ID("{0392F7A4-9AFE-4805-AD5F-3F3AFA4D3DB7}");
                public static readonly ID commOffer = new ID("{C0F2C171-157F-4073-90BA-6BE12E1686AD}");
                public static readonly ID ressInOtherCity = new ID("{5F414853-05F3-4367-987D-A44FD9EE29BF}");
                public static readonly ID ressOffer = new ID("{57540DCB-9B03-424F-832A-383937FE1465}");
                public static readonly ID filters = new ID("{56973585-9D76-477B-BD4C-253D63DA04C1}");
                public static readonly ID projectfound = new ID("{EC2C0939-6919-4A1C-9E0D-469FB5186AFC}");
                public static readonly ID faq = new ID("{B9AA4B72-1314-44F4-AC9C-79D18F4EC8D7}");
                public static readonly ID planavisit = new ID("{F357542A-E1C8-40C3-9400-31969685EEC8}");
                public static readonly ID searchproject = new ID("{CC874222-9F52-423C-80C4-89C7E391F0D0}");
                public static readonly ID copylink = new ID("{BC5A5C01-8BBC-4D75-BA45-1E42DA371486}");
                public static readonly ID email = new ID("{91B35C98-03A2-47A3-90B9-D39A1C61F459}");
                public static readonly ID twitter = new ID("{6439D441-4CAC-4391-A9E8-2D3357BEEB5C}  ");
                public static readonly ID facebook = new ID("{0D1C7A0F-B9F2-4FE9-B7A4-8A83D2C98ECD}");
                public static readonly ID whatsapp = new ID("{3216616A-C033-4403-BD20-CB0AAB3ECE99}");
                public static readonly ID search = new ID("{F7C5F9B6-6375-4D74-B469-AC3359BD0D42}");
                public static readonly ID submit = new ID("{EA7FBCBE-38C6-4F88-9DB2-F32BF03C3F75}");
                public static readonly ID readless = new ID("{4599D97E-B034-4BDF-B2F5-524C237C59EC}");
                public static readonly ID readmore = new ID("{F769CB9A-813B-48FE-9BDF-079BF52BFDE7}");
                public static readonly ID livinggoodlife = new ID("{118A7E44-C94F-4E29-B1E7-9B24648E504C}");
                public static readonly ID print = new ID("{38589404-F842-4B0A-9B27-5FB0695A9221}");
                public static readonly ID saveaspdf = new ID("{40760869-4043-4344-A0DC-454D58608383}");
                public static readonly ID done = new ID("{4C6A4372-21BD-4DFE-BDD9-B3F1E287C17A}");
                public static readonly ID downloadBrochure = new ID("{1027C6C7-6AB8-4C78-83C5-854BD05C6D81}");
                public static readonly ID share = new ID("{0607BC73-937D-4DAC-94E8-62B3649C9C16}");
                public static readonly ID socialClubs = new ID("{7E573C2A-9EFC-4DCB-AE3C-9C6A2E5B1E0B}");
            }
        }
        //public static readonly  = new ID("");
        public static class ModalText
        {
            public static readonly ID ID = new ID("{E4ABE0B9-1E2E-446C-B828-FDC52A8CA0A4}");
            public static class Fields
            {
                public static readonly ID ShareContact = new ID("{6D94C6AB-D80A-4DCF-A68B-D178A5C6670E}");
                public static readonly ID AgreeText = new ID("{5BF49E9A-1FE3-4858-B72F-A9BE91FA7AC8}");
                public static readonly ID HomeLoadCheck = new ID("{ED20E6C2-FFF7-46A8-A2A3-BAC78EA26B33}");
            }
        }
        public static class ProjectPropertyData
        {
            public static readonly ID ID = new ID("{06F241A2-0DCA-4B80-9896-6B9E6FC86E48}");
            public static class Fields
            {
                public static readonly ID Label = new ID("{70771B6B-55FB-4EA0-8808-16187991EA0D}");
                public static readonly ID Location = new ID("{160C87AC-7712-4019-BDBF-2DBC85CB74BF}");
                public static readonly ID Key = new ID("{4D4E22A6-93C3-467D-B96F-516AA7DE9995}");
            }
        }
        public static class EnquireForm
        {
            public static class Fields
            {
                public static readonly ID EnquireNow = new ID("{1C2FA53A-3A9A-43EC-A3EC-8B02A6E68508}");
                public static readonly ID ShareContact = new ID("{BB5C6396-DB22-4CC0-B208-40735A15C09D}");
                public static readonly ID SendUsQuery = new ID("{C8479040-3D46-4192-B96F-072A81446920}");
                public static readonly ID FirstName = new ID("{45DA7A16-B382-48EC-8C1F-B96E0A17CA12}");
                public static readonly ID LastName = new ID("{5384382A-B338-49F7-AB5F-8DC4A3A6EE16}");
                public static readonly ID Name = new ID("{0C827337-6F75-4B58-B737-879A5C9EB6EE}");
                public static readonly ID Email = new ID("{77714DC0-4E63-4B90-8470-47EF11AAABA7}");
                public static readonly ID ProjectType = new ID("{F0511A3F-D3F8-4363-B656-542E5014F7BF}");
                public static readonly ID PropertyType = new ID("{83032382-FB3A-4FAC-8667-899662A56339}");
                public static readonly ID AgreetoConnect = new ID("{3AFA0D13-34DA-41FB-AACB-8AB4C59EE0E6}");
                public static readonly ID OverrideRegistry = new ID("{9BE40E33-56E1-40A9-B144-C526A898F18F}");
                public static readonly ID SubmitDetails = new ID("{878B91C4-C9A4-4BEA-BD27-C205FCDB79EF}");
                public static readonly ID SubmitButton = new ID("{17818753-F7A9-498D-AD17-6829F7441D1F}");
                public static readonly ID ProjectProperty = new ID("{EBAFFE22-2A31-4DB5-ACEA-1BDCB12BC7CD}");
                public static readonly ID StartDate = new ID("{FFA3C88A-FBDA-4E83-9254-D660EAC2895A}");
                public static readonly ID HomeLoanInterested = new ID("{885D43A4-7D57-4052-9C88-299101854C4D}");
                public static readonly ID TimeSolts = new ID("{A579D746-D190-443C-A0D0-91C91229CDD3}");
                public static readonly ID Date = new ID("{09ED8C6F-2A3D-4DE2-9DFD-AC622E4E3AF8}");
                public static readonly ID From = new ID("{949BC4C1-16E9-4383-9198-EDC3CDFB8DAC}");
                public static readonly ID To = new ID("{CBCE4738-EE8B-4E2F-B455-7D1F6855AF51}");
            }
        }
        public static class DropDown
        {
            public static readonly ID ID = new ID("{D8F7F5A2-2C1D-4348-9B4E-A14D041C6847}");
            public static class Fields
            {
                public static readonly ID ID = new ID("{9603E263-2098-48D2-9143-A2CB1EF62A4F}");
                public static readonly ID Value = new ID("{52078AD7-7711-485D-8122-5B015E133412}");

            }
        }
        public static class BlogContentTemplate
        {
            public static readonly ID ID = new ID("{CEDE9262-F575-4678-9E64-EAD30EE98DD9}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID Description = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");

            }
        }
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
        public static class BlogTemplate
        {
            public static readonly ID ID = new ID("{99DB2995-B8B9-48A7-9857-82BC6E491F16}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{57273724-791A-41C5-91AF-12A2AE12F07C}");
                public static readonly ID Keys = new ID("{F2643E0E-F1B9-4AF1-995E-B2F88C786075}");
                public static readonly ID PageTitle = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");

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
        public static class BlogKeysTemplate
        {
            public static readonly ID ID = new ID("{8CECD9A0-322F-40B5-BE69-F413F862B6DC}");
            public static class Fields
            {
                public static readonly ID IsDefault = new ID("{40984027-82FC-4172-A71D-1B56538896EB}");
                public static readonly ID Link = new ID("{0F9229EC-3937-4FE9-B8C5-A60DC2656CBC}");
                public static readonly ID ImageSrc = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string ImageSrcFieldName = "Image";
                public static readonly ID Title = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID Heading = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly ID SubHeading = new ID("{261596D0-34C2-47CE-92B3-DD0911A40E0E}");
                public static readonly ID DateTime = new ID("{96553D7C-FDDF-4E27-8F65-3D2F62B548FC}");
                public static readonly ID CategoryLink = new ID("{0F9229EC-3937-4FE9-B8C5-A60DC2656CBC}");
                public static readonly ID CategoryTitle = new ID("{9CC1EC95-CA19-4ADA-8317-BA826ED1E1FE}");
                public static readonly ID ReadTime = new ID("{64A8B839-7563-4D8A-B222-B5C7DF229A05}");
                public static readonly ID CategoryHeading = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly ID Slug = new ID("{15F5F951-BF5A-42DB-9AF9-8EF22711273D}");
            }
        }

        public static class QuickLinksTemplate
        {
            public static readonly ID ID = new ID("{9D0D4164-46892-48B7-8B10-55D89389A742}");
            public static class DisclaimerFields
            {
                public static readonly ID Description = new ID("{EEF49601-0303-441E-B372-165D325F2562}");
                public static readonly ID Heading = new ID("{08F96B12-49B1-4B2D-989F-DC58ABDEAF8E}");
                public static readonly ID Key = new ID("{39F3B96E-585A-4DC0-AB0C-5308C9E685D3}");
            }
            public static class TermsConditionFields
            {
                public static readonly ID Description = new ID("{D9CC8472-6F0E-4135-8660-03DC2CDAF46C}");
                public static readonly ID Heading = new ID("{EEB0B055-7DA6-45A9-A769-4158B3677904}");
                public static readonly ID Key = new ID("{F9D3228F-63E1-451F-A1DF-E442F55B99B9}");

            }
            public static class PrivacyPolicyFields
            {
                public static readonly ID Description = new ID("{3616FF53-BADD-4D87-855E-0A7911F46167}");
                public static readonly ID Heading = new ID("{EF0BDCA9-A9A8-4811-B25D-6AD7FEAD6D25}");
                public static readonly ID Key = new ID("{FA27692F-F999-473B-8E62-3D82709DC855}");

            }
            public static class CookiePolicyFields
            {
                public static readonly ID Description = new ID("{2DA2E74F-7B3B-4B34-A626-8FCB6F7DAF89}");
                public static readonly ID Heading = new ID("{ED4D390B-E066-4265-A384-9FDFB9908C30}");
                public static readonly ID Key = new ID("{4A988A67-C9A0-48D4-800B-1189A783B883}");

            }
            public static class FaqFields
            {
                public static readonly ID Faq = new ID("{3D7EF30E-D6A9-42A8-8DC8-9F5189C78570}");
                public static readonly ID Heading = new ID("{277C42D6-2BE5-4680-AAC3-D4843F049F79}");
                public static readonly ID Key = new ID("{9F3D6F69-53B2-4440-BA77-94B0CD4DCBE5}");
            }
        }
        public static class WhyAdaniTemplate
        {
            public static readonly ID TemplateID = new ID("{5257DEFC-AE6E-4D8C-8895-CB504DCD3ED3}");
            public static class WhyAdaniFields
            {
                public static readonly ID WhyUsHighlights = new ID("{202FAFCE-D7CC-42CB-BD2A-EEB55778C8A2}");
                public static readonly ID WhyAdani = new ID("{A7A4B90C-423B-46D2-B3D2-57A9947FB933}");
                public static readonly ID BottmQuote = new ID("{9F147506-19AB-409F-B12A-11A23E80BC30}");
                public static readonly ID Heading = new ID("{EEEB20FF-977E-44AA-9EDA-922C1426413F}");
                public static readonly ID About = new ID("{893A6AFC-DFEA-4BA9-8806-3532E2E3D22C}");
                public static readonly ID ReadMore = new ID("{A2059281-1751-4B39-9F9E-A694F4BB68C9}");
                public static readonly ID BlockHeading = new ID("{764F39F8-ECA9-4464-820B-DDEC358BDA5A}");
            }
            public static class WhyUsHighlightsFields
            {
                public static readonly ID SectionHeading = new ID("{34632574-836D-4C61-8E5B-56DBC459F944}");
                public static readonly ID HeadingAsset = new ID("{639BD893-4ABD-4582-9CC7-518AC0741C95}");
                public static readonly ID Src = new ID("{B0EF4E1D-50D9-454F-A0EE-5693BEC138DB}");
                public static readonly ID ImageTitle = new ID("{4B2BC51D-6767-41F9-904F-312A92CAFB64}");
                public static readonly ID Title = new ID("{625C3936-1DEB-4926-BCFC-D38837D81BBA}");
                public static readonly ID SubHeading = new ID("{9DCA2C6C-E910-4298-BF71-F368B8B0EDC3}");
                public static readonly ID Description = new ID("{FFB06A78-34B8-40B2-AA00-AAD017BDE0EF}");
                public static readonly ID DataAlign = new ID("{D47AFFA5-22AD-4018-A41F-9F491BA5C1D0}");
                public static readonly ID imgType = new ID("{35A37CE1-E5F1-4823-9D87-A84179498A72}");
            }
            public static class WhyAdaniInnerFields
            {
                public static readonly ID AlignCol = new ID("{C8C51E2A-7B39-4D72-8B08-12950C526B76}");
                public static readonly ID SectionHeading = new ID("{321DF323-C7B4-4009-B646-B8DBD6233ECE}");
                public static readonly ID IconPrimary = new ID("{B45816EF-BF33-4C7D-9724-97E94BAC68E3}");
                public static readonly ID IconSecondary = new ID("{5308032E-5EB8-4943-89C3-8DAC42C95AD6}");
                public static readonly ID Heading = new ID("{FA63EF0A-0994-465F-A6DD-D3D563B5316E}");
                public static readonly ID SubHeading = new ID("{ED0D161D-837C-4B38-9719-AECB30900E83}");
                public static readonly ID Description = new ID("{019D60DC-D491-40B8-AD50-AB5146C9C156}");
            }
            public static class BottomQuoteFields
            {
                public static readonly ID SectionHeading = new ID("{15B15F04-E990-49A7-B3B2-3CB672C6EAE9}");
                public static readonly ID IconPrimary = new ID("{2368EC43-B3EA-4001-AD2C-BF5412C6648B}");
                public static readonly ID IconSecondary = new ID("{A517413C-46F4-4693-AF7C-FCFB087A5207}");
            }
            public static class BlockHeadingFields
            {
                public static readonly ID Heading = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");
            }

        }
        public static class QuickLinsFaqTemplate
        {
            public static readonly ID Id = new ID("{209C0EB3-E14C-455B-928D-528D3285B351}");

            public static class CategoryFields
            {
                public static readonly ID Heading = new ID("{D382BB76-D6F4-49FC-AD7E-BA6138982E5D}");
                public static readonly ID Title = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");
                public static readonly ID Id = new ID("{FD898F55-CE07-4A7D-8EA2-A5DF5F723B7B}");
                public static readonly ID Links = new ID("{4579AD66-AC38-4057-BF48-8B1EBBA5E97E}");
            }
        }
        public static class FaqTemplate
        {
            public static readonly ID Id = new ID("{FE549078-8C72-472A-8EA7-51053E0E1D73}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{5718E787-142B-41D9-B5A1-0B18F45B8236}");
                public static readonly ID Content = new ID("{45EFE66E-5AD2-4F1D-BAD5-FDF281688681}");
                public static readonly ID Link = new ID("{8CEDC322-2AA6-4EC6-8800-DF54977EC81B}");
                public static readonly ID PageSpecificBreadCrumb = new ID("{8507CB71-5A3B-41D3-9F81-510C4929DED2}");
            }
        }

        public static class PolicyTemplate
        {
            public static class CookieFields
            {
                public static readonly ID Heading = new ID("{01D6828C-F6E5-4C7A-8758-A6F8A495A9E2}");
                public static readonly ID Content = new ID("{7C258CE0-9C45-40F4-8195-6FBB7990F04E}");
                public static readonly ID AcceptBtn = new ID("{BA7BC355-9681-490F-AFE6-F856CFE7A782}");
                public static readonly ID CancelBtn = new ID("{B1F3EBF2-9777-4880-84AC-F94EC9AB8D2B}");
            }
            public static class DisclaimerFields
            {
                public static readonly ID Heading = new ID("{5C31776E-44C5-4607-901C-45E97938A034}");
                public static readonly ID Content = new ID("{63FBA0D9-AAD0-4E18-BD2B-E0F9ABEB8EDF}");
                public static readonly ID AcceptBtn = new ID("{55E90676-D361-46FE-870F-B74D0CABA4C3}");
                public static readonly ID CancelBtn = new ID("{25A7CDCE-A261-4EE8-9EF0-BB663F82DCCC}");
                public static readonly ID DisclaimerLogo = new ID("{86D6F42B-709E-4F17-B529-04389EEDE7C2}");
                public static readonly ID LogoAlt = new ID("{7C688C5F-D125-4AF8-8D36-FEEDAA5A61A9}");
            }
        }

        public static class OtherArticleTemplate
        {
            public static class OtherArticleFields
            {
                public static readonly ID ArticleType = new ID("{2FA65D3E-CB9E-47F1-A0DF-4944C9D189BB}");
                public static readonly ID ArticleLink = new ID("{AAC3A1B8-7183-4A01-B06E-C9622852C498}");
                public static readonly ID ArticleLinkIcon = new ID("{ABF064B1-E9D0-4A80-851F-62B21CDCB37D}");
                public static readonly ID ArticleLinkTitle = new ID("{1E8236AD-EAC9-468D-AA2A-E9611BE04D46}");
                public static readonly ID ArticleThumb = new ID("{8788AB0C-D0B0-404F-B2FB-F90E3F130058}");
                public static readonly ID ArticleTitle = new ID("{071AF09A-7D9D-45F3-818B-940D8EE055CA}");
                public static readonly ID ArticleDescription = new ID("{EC3E491B-13F4-4D28-B1C0-61829ACA720D}");

            }
        }
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

        public static class GoodnessBannerTemplate
        {
            public static readonly ID TemplateID = new ID("{C6CF8903-F574-49DF-B28C-DE4A4882BC31}");
            public static class Fields
            {
                public static readonly ID srcMobile = new ID("{59520167-01B7-4FEE-B50B-5665079860A0}");
                public static readonly ID src = new ID("{FEF36DDA-E6F8-4CAB-B7D8-51736F92775E}");
                public static readonly ID heading = new ID("{ECAC38C0-E823-46CD-8AE1-63315C1467F8}");
                public static readonly ID description = new ID("{8FA803B4-BD54-4A23-9DD7-D30970F9B7E5}");
                public static readonly ID link = new ID("{6D3FC93A-6919-497F-857F-8E55592A73F6}");
            }
        }
    }
}