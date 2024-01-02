using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform
{
    public static class Templates
    {
        public static readonly ID commonItem = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");

        public static readonly ID HomeItemID = new ID("{6316DD92-A4F9-4302-BA75-727246707415}");
        public static readonly ID LocationlandingID = new ID("{19FADD3A-572B-4BFE-ACB9-9649672754F0}");
        public static readonly ID LocalDatasourceFolderTemplateID = new ID("{22011FA7-E290-4103-BE3F-C4DF3D5AFF36}");
        public static readonly ID CommonFolderTemplateID = new ID("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");
        public static readonly ID SRDCServiceItemID = new ID("{18C241F5-EFDD-482C-A063-0831C0FE4E1A}");

        public static class LocationPageTemp
        {
            public static readonly ID TemplateID = new ID("{8ABA5211-2B44-4E9F-860B-FD28DDAE9054}");
            public static readonly ID ItemID = new ID("{57AFF2A6-2113-4E27-8A0B-8F9E4CEA645C}");
            public static class Fields
            {
                public static readonly ID LocationSelectionID = new ID("{851D7AC5-1042-4BC5-8332-FC1F631540F0}");
                public static readonly ID Cities = new ID("{C12B9CFC-5A64-4034-978E-5964C036CADD}");

            }
        }
        public static class BlogCategory
        {
            public static readonly ID TemplateID = new ID("{8ABA5211-2B44-4E9F-860B-FD28DDAE9054}");
            public static readonly ID ItemID = new ID("{DFFAFAC1-F6A6-4707-8BE6-C2A194B50CEF}");
            public static readonly ID BlogAnchorsFolder = new ID("{1C0D5047-2849-44A5-900F-79F2CE21124C}"); ///sitecore/content/AdaniRealty/Home/blogs/Local-Content/Blogs-Anchors
            public static class Fields
            {
                public static readonly ID Keyword = new ID("{CA5231DE-80EC-4990-9051-B23A10E9182D}");
            }
        }


        public static class CommunicationCornerTemp
        {
            public static readonly ID TemplateID = new ID("{2FBDA2B6-BA1B-472F-8040-3EB3059024C7}");
            public static readonly ID BlogAnchorsID = new ID("{9C23584B-262A-4523-99BE-65B395431E18}");
            public static readonly ID BLogSelectionID = new ID("{13E2C8D3-4A96-4B10-B93B-F03C5B8DCB87}");
            public static readonly ID ItemID = new ID("{E20A905E-A436-4179-84A3-5A4221FE012E}"); ///sitecore/content/AdaniRealty/Home/blogs

            public static class Fields
            {
                public static readonly ID CommunicationCornerlinkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string linkName = "link";
                public static readonly ID blogdataID = new ID("{115890C6-B02F-4AE2-979D-B1B76DAE97C0}");
                public static readonly string bloglinkName = "Link";
                public static readonly ID src = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string srcName = "Image";
                public static readonly ID title = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID slug = new ID("{15F5F951-BF5A-42DB-9AF9-8EF22711273D}");
                public static readonly ID CatagorytitleID = new ID("{CA5231DE-80EC-4990-9051-B23A10E9182D}");
                public static readonly ID Body = new ID("{261596D0-34C2-47CE-92B3-DD0911A40E0E}");
                public static readonly ID description = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly ID datetime = new ID("{96553D7C-FDDF-4E27-8F65-3D2F62B548FC}");
                public static readonly ID category = new ID("{09F89371-10E6-48F6-B0D2-0A60442F09BB}");
                public static readonly ID readtime = new ID("{64A8B839-7563-4D8A-B222-B5C7DF229A05}");
                public static readonly ID Quote = new ID("{C5EB1C2E-5470-4B96-8E9B-793FAA823A9E}");
                public static readonly ID BlogTitleFieldID = new ID("{A141D927-B6A9-4BC3-85A7-EE4E8CBCEA8A}"); //Blog Title Field
                public static readonly ID SlugText = new ID("{2928E0C6-2063-4203-8763-DF2B94918037}");
            }
        }
        public static class commonData
        {
            public static readonly ID TemplateID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID ItemID = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");

            public static class Fields
            {
                public static readonly ID blogLinkId = new ID("{C0BC1F88-F412-4A77-A43B-1057F4B0A21F}");
                public static readonly ID residentialID = new ID("{DF6C8091-004B-4243-8F5D-E7D443BCF099}");
                public static readonly ID commercialID = new ID("{BF76D3F0-B1AA-4475-8E34-6CCB279A0252}");
                public static readonly ID ClubId = new ID("{BE5B1594-E8F0-454E-B43B-B68ABA211B96}");
                public static readonly ID OTPlenghtID = new ID("{C0B6939D-8060-42DF-9A1A-78BF6F65785E}");
                public static readonly ID LocationPageBreadcrumb = new ID("{023CAB6C-59B7-4003-BD9E-BD981A9F04E9}");
                public static readonly ID ressOffer = new ID("{57540DCB-9B03-424F-832A-383937FE1465}");
                public static readonly ID commOffer = new ID("{C0F2C171-157F-4073-90BA-6BE12E1686AD}");
                public static readonly ID SiteDomain = new ID("{15E638D2-40BD-41D9-A7C5-5A629F9BDED6}");
            }
        }
        public static class NameValueCollection
        {
            public const string NameValueFolder = "{979C4694-EF8D-4D7F-9017-9E3E9903359B}";
            public const string nameValue = "{407FC9F0-CC99-475B-8F51-541094377FBB}";
            public static class Fields
            {
                public static readonly ID NavigationTitle = new ID("{32CFF90D-4FDF-4402-A364-21199E88753D}");
            }
        }
        public static class headerlogo
        {
            public static readonly ID headerLogID = new ID("{FEE16E6B-0823-44FB-ACD2-F56DB2011AA3}");
            public static readonly string HeaderLogoName = "HeaderLogo";
        }
        public static class SFDCService
        {
            public static readonly ID Id = new ID("{EC2C44D4-90DA-4A5F-915D-7278BDE473C0}");

            public static class Fields
            {
                public static readonly ID ConsumerKeyID = new ID("{7AC3A0B8-09DC-4597-8C5C-07B49DB28A1D}");
                public static readonly ID ConsumerSecretID = new ID("{15747BC8-3FB8-41BD-9D63-C5A4F8EDA19A}");
                public static readonly ID pwdID = new ID("{7F1505C1-CD8D-416A-8005-0BB1A254CF1C}");
                public static readonly ID UsernameID = new ID("{B0418D9C-F7C9-46A2-83CC-906BD3B58B6F}");
                public static readonly ID TokenUrlID = new ID("{61EFE88D-F646-4C41-89DE-0818F018AB3C}");
                public static readonly ID ServiceURLID = new ID("{3664AFCC-B9D1-43EB-96F4-93AB79F4934E}");
                public static readonly ID OTPmessage = new ID("{354BC476-1EA1-45A5-9FBD-9FA4C9220BDE}");
            }
        }
        public static class SFDCStaticValue
        {
            public static readonly ID TemplateId = new ID("{8A13871B-2161-445A-AAE7-A509E193FD0C}");
            public static readonly ID ItemID = new ID("{99EB6503-DD95-4B00-BD76-5FF30C5C9287}");

            public static class Fields
            {
                public static readonly ID PostToSalesforce = new ID("{825DC294-C1F8-4161-ADCA-42161648CAF8}");
                public static readonly ID LeadSource = new ID("{C3212F0D-58C7-41B2-9CFF-75BFF35A4BDF}");
                public static readonly ID UtmSource = new ID("{F76D6A09-72B9-4C04-89F6-E0763195DC04}");
                public static readonly ID Budget = new ID("{36036FE5-439D-490D-A2A1-1FC55A9A5C12}");
                public static readonly ID Remarks = new ID("{D217F05F-5EAE-4D8D-A084-A07D9BCFE07F}");
                public static readonly ID RecordType = new ID("{8D13FC43-2D15-4C5D-AAF8-50EED81DE69A}");
                public static readonly ID commercialRecordType = new ID("{D2DE641C-5E0B-452A-BB2C-256DF71BF926}");
                public static readonly ID OtherRecordType = new ID("{A8ECE29F-3097-4BD9-B36F-A17D092A3466}");
                public static readonly ID clubRecordType = new ID("{1818F7D5-6627-4AD3-8612-DDE2BFD60DF2}");
                public static readonly ID MasterProjectID = new ID("{9E00EB95-11EE-44AC-9D9A-88517E4D5023}");
                public static readonly ID MasterProjectIDs = new ID("{1F2A5D1E-E6D7-4F6B-A029-F68BCF0B4637}");
                public static readonly ID ClubProjectID = new ID("{99B0A84B-7A55-4B0A-A4DD-3DD7FE50DFF5}");
                public static readonly ID ClubProjectIDs = new ID("{EEDF3F69-089A-40F7-B1A7-5E09B20F971B}");
                public static readonly ID Saletype = new ID("{C7406622-BFD1-4E6D-AB1F-8B94AA5490E3}");
                public static readonly ID Comments = new ID("{E0676057-DAFA-47A6-8C82-12E1D86E95B9}");
                public static readonly ID Ads = new ID("{2200949D-07D1-49E0-BE7E-8CB1229AFCF6}");
                public static readonly ID UtmPlacement = new ID("{6B474C54-97E4-4E92-AFBD-2BF2FD94C3A0}");

                public static readonly ID OTPmsg = new ID("{1582F6DA-0480-4426-8269-91E8A51CD7ED}");
                public static readonly ID customerror = new ID("{8E783873-24B1-428B-BE17-828985C267B7}");
                public static readonly ID resubmittheforms = new ID("{21403DE4-F3CD-4E2B-8323-E9A2E9EC77D4}");
                public static readonly ID invalidOTP = new ID("{22B09945-8B23-4976-A1AD-13AA67992C04}");
                public static readonly ID invalidinput = new ID("{0A95CE46-E912-49D2-B5E6-5B31252AB080}");
                public static readonly ID moreTryForOTP = new ID("{FE319443-68AF-4C45-8BF4-3D225F81A742}");
            }
        }

        public static class NavigationRoot
        {
            public static readonly ID Id = new ID("{D7F870BC-89D8-4F17-95EC-59ACFD7DA05C}");

            public static class Fields
            {
                public static readonly ID HeaderLogo = new ID("{FEE16E6B-0823-44FB-ACD2-F56DB2011AA3}");
                public static readonly ID FooterCopyright = new ID("{F6098004-9129-41E4-A3B0-604A7583C26D}");
            }
        }
        public static class AirportNavigation
        {
            public static readonly ID LinkTemplateID = new ID("{4DC30130-56B5-4E49-BB2D-6FC7EA5465DB}");
            public static readonly ID LinkFolderTemplateID = new ID("{7E944CF9-AAB7-427A-AD75-5A798175BC02}");
        }
        /// <summary>
        /// Residential component  : ( _HasPageContent - {D1B7CD65-6321-46D4-8210-0031CA094037} &
        ///                             _Property - {38F56FB0-AB22-4BEB-A142-B4D531ACD492} &
        ///                             Image Template - {494F095B-B942-46AF-AFDC-6833DC5504A1} )
        /// </summary>
        public static class OtherProjects
        {
            public static readonly ID TemplateID = new ID("{01B6A097-8B06-494D-9D0F-939CE584416B}");
            public static readonly ID PropertiesID = new ID("{B0B87592-3283-4A77-B4D5-EC13CD1FCBE4}");
            public static readonly ID ResidentialID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID CommercialID = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
            public static class Fields
            {
                public static readonly ID Location = new ID("{0E915D68-BE8C-4439-AF6C-9EC488D5BD9F}");
                public static readonly ID ProjectType = new ID("{C4EC7EAD-C43D-48A3-8998-97BE18BD3CB4}"); //R & C Prop
                public static readonly ID ProjectTypes = new ID("{8915C2A4-7722-4CA3-9FDD-CE32610EEF80}"); // SEO page
                public static readonly ID Area = new ID("{318E9BB2-30DE-4C55-B3C6-07967D75D3E2}");
                public static readonly ID Subcity = new ID("{E446434D-C8F9-4E16-8492-DFE8E32CD69B}");
                public static readonly ID Flats = new ID("{72039E5D-4EEB-49C9-B13D-EA251744FEE1}");
                public static readonly ID PropertyType = new ID("{A5D160AE-69CC-40B9-97DD-9165968BA6CD}");
                public static readonly ID SEOProjectStatus = new ID("{DBA844B5-317E-45D7-ADA3-496AE903B6B3}");
                public static readonly ID ProjectStatus = new ID("{0350CF91-6ECB-4F5A-BC4C-430D0E32214B}"); // dropdown
                public static readonly ID ConfugurationType = new ID("{0B04038F-0DD0-45EF-A904-D8505DA56AF7}");
                public static readonly ID typeID = new ID("{04CF398B-5FE5-4A5D-93F4-AC750549D77E}");

            }
        }
        public static class ResidentialProjects
        {
            public static readonly ID TemplateID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID ResidentialLandingID = new ID("{A6A604B2-2656-4E5C-952C-05279F72D014}");

            public static class Fields
            {
                public static readonly ID LatitudeID = new ID("{D04153A0-E7B0-43A8-911A-6BB9C6EC8326}");
                public static readonly ID LongitudeID = new ID("{E9E067E6-9B78-4BFC-B6AD-1D9791D8587A}");
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly string TitleFieldName = "Title";
                public static readonly ID SummeryID = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly string SummeryFieldName = "Summery";
                public static readonly ID ImageID = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string ImageFieldName = "Image";
                public static readonly ID mobileimageID = new ID("{86568F07-2F96-4C45-A847-7DB75C165DC9}");
                public static readonly string mobileimageFieldName = "Mobile Image";
                public static readonly ID linkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string linkFieldName = "link";
                public static readonly ID Property_LogoID = new ID("{E8B52311-A1D9-4B04-B5A0-0682EE79EDBF}");
                public static readonly string Property_LogoFieldName = "Property Logo";
                public static readonly ID Property_TypeID = new ID("{75091E77-938C-4379-84FE-174833285D4C}");
                public static readonly string Property_TypeFieldName = "Property Type";
                public static readonly ID LocationID = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly string LocationFieldName = "Location";
                public static readonly ID imgtypeID = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");
                public static readonly string imgtypeFieldName = "imgtype";
                public static readonly ID Site_StatusID = new ID("{C726F4E1-A95A-47E3-B101-76508BF7A465}");
                public static readonly string Site_StatusieldName = "Site Status";
                public static readonly ID TypeID = new ID("{04CF398B-5FE5-4A5D-93F4-AC750549D77E}");
                public static readonly string TypeFieldName = "Type";
                public static readonly ID ProjectStatusID = new ID("{0350CF91-6ECB-4F5A-BC4C-430D0E32214B}");

            }
        }
        /// <summary>
        /// comrcial project component : ( Labels - {18B99DDE-75A9-4DFC-A501-FFAF5904C8AE} & 
        ///                                Hero Banner - {4A421518-7B4A-4D43-AF9B-94E17E00EC9F})
        /// </summary>
        public static class CommercialProjects
        {
            public static readonly ID TemplateID = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
            public static readonly ID CommercialLandingID = new ID("{05BB59FB-7490-4A4B-A9DD-EFBD9839EFC3}");

            public static class Fields
            {
                public static readonly ID LatitudeID = new ID("{D04153A0-E7B0-43A8-911A-6BB9C6EC8326}");
                public static readonly ID LongitudeID = new ID("{E9E067E6-9B78-4BFC-B6AD-1D9791D8587A}");
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly string TitleFieldName = "Title";
                public static readonly ID thumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly string thumbFieldName = "thumb";
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly string headingFieldName = "heading";
                public static readonly ID subheadingID = new ID("{89B0A8ED-0EE8-4512-B518-AB2C4C2A0B9E}");
                public static readonly string subheadingFieldName = "subheading";
                public static readonly ID CTA_TitleID = new ID("{780D29A9-11E5-44F4-8A67-3331E5A88E81}");
                public static readonly string CTA_TitleFieldName = "CTA Title";


            }
        }
        /// <summary>
        /// 
        /// Social Clubs component : ( Labels - {18B99DDE-75A9-4DFC-A501-FFAF5904C8AE} & 
        ///                                Hero Banner - {4A421518-7B4A-4D43-AF9B-94E17E00EC9F})
        /// </summary>
        public static class SocialClubs
        {
            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static readonly ID SocialCLubsID = new ID("{22011FA7-E290-4103-BE3F-C4DF3D5AFF36}");
            public static readonly ID ClubLandingID = new ID("{A91C40DF-382D-4D8B-B70B-748F34C9A717}");

            public static class Fields
            {
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly string TitleFieldName = "Title";
                public static readonly ID thumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly string thumbFieldName = "thumb";
                public static readonly ID mobileimageID = new ID("{86568F07-2F96-4C45-A847-7DB75C165DC9}");
                public static readonly string mobileimageFieldName = "Mobile Image";
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly string headingFieldName = "heading";
                public static readonly ID subheadingID = new ID("{89B0A8ED-0EE8-4512-B518-AB2C4C2A0B9E}");
                public static readonly string subheadingFieldName = "subheading";
                public static readonly ID CTA_TitleID = new ID("{780D29A9-11E5-44F4-8A67-3331E5A88E81}");
                public static readonly string CTA_TitleFieldName = "CTA Title";
                public static readonly ID linkID = new ID("{17B31027-C4FF-4E30-8DCE-6D0DBF71E6A4}");
                public static readonly string linkName = "link";
                public static readonly ID readmoreID = new ID("{E7D58D3B-8DC8-49B1-8F18-8795289925A9}");
                public static readonly ID imgtype = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");


            }
        }
        /// <summary>
        /// Township component  : ( _HasPageContent - {D1B7CD65-6321-46D4-8210-0031CA094037} &
        ///                         Labels - {18B99DDE-75A9-4DFC-A501-FFAF5904C8AE})
        /// </summary>
        public static class Township
        {
            public static readonly ID TemplateID = new ID("{22C02FBC-20BE-4F58-9E27-562588A523F8}");
            public static readonly ID TownshipLandingID = new ID("{4D88E182-FE14-48F7-A1E7-723F45E59CBE}");
            public static class Fields
            {
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly string TitleFieldName = "Title";
                public static readonly ID SummaryID = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly string SummaryFieldName = "Summary";
                public static readonly ID ImageID = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string ImageFieldName = "image";
                public static readonly ID mobileimageID = new ID("{86568F07-2F96-4C45-A847-7DB75C165DC9}");
                public static readonly string mobileimageFieldName = "Mobile Image";
                public static readonly ID BTNtextID = new ID("{7ACE4C96-1542-4EF2-A725-67EC7439245E}");
                public static readonly string BTNTextFieldName = "BTN Text";
                public static readonly ID Link = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string LinkName = "link";
                public static readonly ID readmoreID = new ID("{E7D58D3B-8DC8-49B1-8F18-8795289925A9}");

            }
        }
        /// <summary>
        /// Testimonial component  : ( _HasPageContent - {D1B7CD65-6321-46D4-8210-0031CA094037} &
        ///                             TestimonialList - {FD9E749E-F8CA-4109-9466-6CC099439D48} &
        ///                             Video Component - {97FA0743-CFE7-4B01-B51F-D9939A46C409})
        /// </summary>
        public static class TestimonialList
        {
            public static readonly ID TemplateID = new ID("{FD9E749E-F8CA-4109-9466-6CC099439D48}");
            public static readonly ID TestimonialLandingID = new ID("{22C02FBC-20BE-4F58-9E27-562588A523F8}");

            public static class Fields
            {
                public static readonly ID isVideoID = new ID("{A86DE88B-9365-4600-960D-73C2E2E7CE9E}");
                public static readonly string isVideoFieldName = "isVideo";
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly string TitleFieldName = "Title";
                public static readonly ID SummaryID = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly string SummaryFieldName = "Summary";
                public static readonly ID ImageID = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string ImageFieldName = "image";
                public static readonly ID AuthorID = new ID("{8C97A9F8-CB2C-4D86-BAFF-91D781408EE3}");
                public static readonly string AuthorieldName = "Author";
                public static readonly ID VideoMp4ID = new ID("{795531F5-AE9B-4031-B9F2-8E49CDB47C00}");
                public static readonly string VideoMp4FieldName = "videoMp4";
                public static readonly ID Property_locationID = new ID("{D2DD08A5-98AF-4F18-9E20-24F779B87259}");
                public static readonly string Property_locationFieldName = "Property location";
                public static readonly ID SEOName = new ID("{31B25E24-035C-4D1F-B456-E517CB0E838C}");
                public static readonly ID SEODescription = new ID("{56FFDF2F-4EF3-4AB3-9B40-C82DF45823CF}");
                public static readonly ID UploadDate = new ID("{51357354-2A67-460A-8FE3-E0A86D471A75}");

            }
        }
        /// <summary>
        /// About Adani COmponent : (About Adani template - {3F008B5C-3742-4FA6-9F54-01A240FBD0D8} &
        ///                         _HasPageContent - {D1B7CD65-6321-46D4-8210-0031CA094037})
        /// </summary>
        public static class AboutAdaniHouse
        {
            public static readonly ID TemplateID = new ID("{3F008B5C-3742-4FA6-9F54-01A240FBD0D8}");
            public static readonly ID AboutAdanuID = new ID("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");

            public static class Fields
            {
                public static readonly ID aboutID = new ID("{C592BE47-09CC-40E5-AE70-25308CBAFC5A}");
                public static readonly string aboutFieldName = "about";
                public static readonly ID readMoreID = new ID("{C59BE9FD-7AE9-4B9A-856E-1670F026D17E}");
                public static readonly string readMoreFieldName = "readMore";
                public static readonly ID termsID = new ID("{6A81B7EE-27CD-4B3F-8EC1-7FAB061732A8}");
                public static readonly string termsFieldName = "terms";
                public static readonly ID detailLinkID = new ID("{A27ACE80-AB06-4098-BF3F-E984105E8B35}");
                public static readonly string detailLinkFieldName = "detailLink";
                public static readonly ID extrChargesID = new ID("{3A97F4CD-BAEF-4F90-B56E-CF9D5115EDE8}");
                public static readonly string extrChargesFieldName = "extrCharges";
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly string headingFieldName = "heading";
                public static readonly ID LinkID = new ID("{17B31027-C4FF-4E30-8DCE-6D0DBF71E6A4}");
                public static readonly string LinkFieldName = "link";
                public static readonly ID description = new ID("{779FE384-892C-4023-B94B-B34791D213D2}");
                public static readonly ID headingDisclaimer = new ID("{89B0A8ED-0EE8-4512-B518-AB2C4C2A0B9E}");
                public static readonly ID ReadMore = new ID("{D8FF9A57-EE41-4E2D-8C41-6900A0C0F615}");
                public static readonly ID ReadLess = new ID("{2C727475-80C0-4147-AD76-0C3B89478CCD}");
                public static readonly ID ImageDisclaimer = new ID("{B266AC1C-B70C-40E8-A865-129A2CE1003C}");
                public static readonly ID EmiDisclaimer = new ID("{8B5CCAD2-B379-410F-94A1-B7EFA8469EC8}");

            }
        }
        /// <summary>
        /// Mobile Menu Common Component :  (hero Banner - {4A421518-7B4A-4D43-AF9B-94E17E00EC9F} )
        /// </summary>
        public static class MobileMenuTemp
        {
            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static class Fields
            {
                public static readonly ID class1ID = new ID("{548B5011-641A-4BD4-B6AE-EC9BCDD853B5}");
                public static readonly ID thumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly string headingFieldName = "heading";
                public static readonly ID logoID = new ID("{B5F61442-FF0F-46A5-90A8-D6D387DE24A0}");
                public static readonly string logoFieldName = "logo";
                public static readonly ID linkID = new ID("{17B31027-C4FF-4E30-8DCE-6D0DBF71E6A4}");
                public static readonly string linkFieldName = "link";
            }
        }
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


            }
        }
        /// <summary>
        /// Enquiry Form for Location page
        /// /sitecore/templates/Feature/Realty/Location/Enquire Form
        /// </summary>
        public static class EnquiryFormTemp
        {
            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static readonly ID ItemID = new ID("{ECDBC163-B9BA-4CD4-B272-6B8230F9A7BB}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{CC45F7D1-FEBC-4DA3-B3D3-A68834441719}");
                public static readonly ID planAVsiit = new ID("{00CA89F8-920B-4E34-9870-2E0AE2D952BE}");
                public static readonly ID ContactUsTitle = new ID("{00CA89F8-920B-4E34-9870-2E0AE2D952BE}");
                public static readonly ID PropertyLabel = new ID("{24ABD42E-0678-4DE6-ABDA-EB81998B664C}");
                public static readonly ID ShareContact = new ID("{48D17B96-3087-4849-A328-81F24D548053}");
                public static readonly ID SendUsQuery = new ID("{95A422E8-B9E1-466E-8FFC-4A0E6BD91443}");
                public static readonly ID FirstName = new ID("{1B6CB5C9-D386-4E81-9B35-B6375449ED51}");
                public static readonly ID lastName = new ID("{C55D8797-04E5-4766-9202-8B5D4373ECAD}");
                public static readonly ID name = new ID("{98995044-CC7D-4F91-ABC7-BE7CE3C91185}");
                public static readonly ID Email = new ID("{D18F8D1A-A62D-4180-85F3-C4CE8AC09B74}");
                public static readonly ID ProjectType = new ID("{AAA75079-21E7-4C39-926B-856CEF93B006}");
                public static readonly ID ProjectTypeSelection = new ID("{6434CD27-5AE2-4A28-95D0-5446892A967B}");
                public static readonly ID SelectPropertyType = new ID("{4B586D2B-97F9-4E7B-877F-8A98C84E885D}");
                public static readonly ID PropertyTypeSelection = new ID("{CBFA1F83-C069-4E4B-A3A7-7E24EE14824C}");
                public static readonly ID AgreeToConnect = new ID("{84706836-DA4D-4A8C-AB49-A87FBD9656B2}");
                public static readonly ID overrideRegistry = new ID("{BED545E0-6EA7-4E9B-95AE-F4DF9A084EA3}");
                public static readonly ID submitDetail = new ID("{735E9F3A-1B34-401C-99B7-4F19B7773FDB}");
                public static readonly ID SelectProjectProperty = new ID("{58A385F9-CAC1-421A-8FAA-39855E89783C}");
                public static readonly ID configuration = new ID("{549C900E-543F-4D47-993A-D0F72AFD9EB8}");
                public static readonly ID startDate = new ID("{F880015C-E35B-44C3-9C94-359BE9E299D1}");
                public static readonly ID homeLoanInterested = new ID("{581B0C59-70F9-44C8-859F-27B355523D93}");
                public static readonly ID timeSlotsLabel = new ID("{A697AC2D-BCB8-4F6F-A341-95E64747385C}");
                public static readonly ID timeSlots = new ID("{B2C81525-40B8-4651-84E5-571C87E54045}");
                public static readonly ID date = new ID("{47D7CB91-C453-4CBF-AEF0-0909B740742D}");
                public static readonly ID from = new ID("{6034EB17-37E6-4E56-A8CD-7D97EED39901}");
                public static readonly ID to = new ID("{E1889CCD-8F13-4F3E-BF3C-4F936C9143DF}");
                public static readonly ID heading = new ID("{8670DF43-62EE-481F-A063-1C480904BEC2}");
                public static readonly ID para = new ID("{81297783-11C1-4D61-9B04-63D20322CE51}");
                public static readonly ID cancelLabel = new ID("{C69B2830-D560-44E4-92B8-5F2865C5FFB5}");
                public static readonly ID continueLabel = new ID("{DBD5BB44-9CAB-497A-AE61-9BA0030FA6DF}");
                public static readonly ID description = new ID("{FD63B70F-94B8-48A0-A841-A13AFB90F16F}");
                public static readonly ID PrintLabel = new ID("{0E68DD6B-670F-418B-AB3E-E574C3D65C8A}");
                public static readonly ID saveaspdfLabel = new ID("{EC0956C0-ACB3-4255-91EF-C87028D893E0}");
                public static readonly ID DoneButtonLabel = new ID("{E7E46AB3-65CA-4BCB-9F02-096CB6C92934}");
                public static readonly ID MobileLabel = new ID("{C0BEEDA9-84CE-4A23-99D2-F8AD514BB2C6}");
                public static readonly ID SubmitButtonText = new ID("{17818753-F7A9-498D-AD17-6829F7441D1F}");
                public static readonly ID EnterOTPLabel = new ID("{C2A09BD5-3E01-4A48-8F75-9E1E1F168327}");
                public static readonly ID WehavesentviaSMStoLabel = new ID("{B2FDE286-8B45-4741-BE46-6462FEA493F5}");
                public static readonly ID HavenotreceivedaOTPLabel = new ID("{AEA65F40-C0E1-483F-8803-FD89118C555A}");
                public static readonly ID EditButtonLabel = new ID("{14F084B4-15B3-4AAF-B0C0-141AFEA9518B}");
                public static readonly ID ResendButtonLabel = new ID("{5323AA6A-051E-4E64-AFDB-A4714ADECEC4}");
                public static readonly ID purposelabelId = new ID("{F6B83714-16E6-4FEE-8E1F-01B42AE2122E}");
                public static readonly ID purposeListID = new ID("{D90ACD72-1BFD-4C51-AD5D-6238D2346156}");
                public static readonly ID errorMessageTitle = new ID("{A20B21E3-723C-498F-B43A-C797B01DF1F9}");
                public static readonly ID errorMessageDesription = new ID("{56466476-9936-49C8-A2AC-E376A30DE400}");
                public static readonly ID ITitleFieldID = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");
                public static readonly ID HeaderTextID = new ID("{59D24D0A-F955-4988-B26F-92039B4DF8BD}");
                public static readonly ID BrochureHeadingID = new ID("{7C55F905-7EE6-4F19-AEF8-8C863049CDCD}");
                public static readonly ID messageLabel = new ID("{C31A2166-389C-4A7E-A606-952428E5AB8D}");
                public static readonly ID messageMaxLength = new ID("{2F69E62F-A574-4235-9467-46A0F74F697D}");
                public static readonly ID BrochureFormDescriptionID = new ID("{3E626C43-2540-4433-9FBF-7E79CEDC5783}");
                public static readonly ID BrochureThankyouDescriptionID = new ID("{36EBE911-121C-4073-9B07-0CF998B407D6}");
                public static readonly ID popUpTitle = new ID("{BB6E7FCF-28EF-4ACE-A4B8-2C99AA6264AA}");
                public static readonly ID popupSubTitle = new ID("{3B6EC77D-2B7A-4EEA-9BF7-17B89B6E029D}");
                ///sitecore/templates/Feature/Realty/BasicContent/Section Header/Header/Text -- headerTextID

                public static readonly ID fullname = new ID("{8D8A7EB9-B395-4A05-ABE1-FE6D622E5D90}");
                public static readonly ID fname = new ID("{4E81B7BD-3198-419E-B86F-5D08C138D83A}");
                public static readonly ID lname = new ID("{BD13DA5C-23E9-4495-A607-876F6628CF93}");
                public static readonly ID messageError = new ID("{C9ADBB9E-1082-4102-B1D6-E6B0FE800C94}");
                public static readonly ID email = new ID("{F0C255C7-B795-49A0-9FA5-C8DA67F455F4}");
                public static readonly ID phoneNo = new ID("{E139B4A8-04E2-4ECA-BF7D-7772432FC23D}");
                public static readonly ID projectType = new ID("{622FF69D-ED6D-4857-A285-71F8714E36E2}");
                public static readonly ID projectName = new ID("{6D51B9A2-BE4D-4CDD-9530-F73029AAEF42}");
                public static readonly ID configurationmessage = new ID("{44FC38C4-267F-474D-8341-32AF382FC5AF}");
                public static readonly ID timeslot = new ID("{FD8BD1BC-5BB3-4288-952B-C0E8C3B338B0}");
                public static readonly ID contactAdaniRealty = new ID("{B32ACD27-3000-43F0-AE67-A14A1C88B4B4}");
                public static readonly ID purpose = new ID("{0F7E9D17-8547-41D2-B62A-071F53DF461D}");
                public static readonly ID dateErrormessage = new ID("{019A3021-4AC3-492B-BE79-C55F1502B758}");


            }
        }
        /// <summary>
        /// HamBurger Menu template
        /// </summary>
        public static class HeaderItemTemp
        {
            public static readonly ID TemplateID = new ID("{4540B02D-1C1A-45C5-B188-232AA19C880F}");
            public static readonly ID SectionPromoImagesTemplateID = new ID("{1537D3C3-6D3D-4FA9-AFA6-9AB59A9E479E}");
            public static readonly ID HamburgerMenuLinkTemplateID = new ID("{2D83F0B5-68F6-4D09-A1A5-27A812F9E265}");
            public static readonly ID adaniBusinessesItemID = new ID("{6183CA01-619C-4CBA-8886-2BCDA353098A}");
            public static class Fields
            {
                public static readonly ID headerTextID = new ID("{F2102779-A10A-42AE-9E1E-4456B3CA1116}");
                public static readonly ID headersubTextID = new ID("{BEADCE13-28FE-4475-A17A-AAC45F8BF8B4}");
                public static readonly ID headerLinkID = new ID("{C8046A55-4ABC-40CE-8F00-51BB3F4586DA}");
                public static readonly string headerLinkName = "header Link";
                public static readonly ID headerLeftIconID = new ID("{376D2C27-189F-4202-9643-5CE2B5C0E32C}");
                public static readonly ID headerRightIconID = new ID("{672CD21F-316B-4EFC-BF7F-E25E4EDBD20F}");
                public static readonly ID fatnavChechboxID = new ID("{413EFB82-6890-4B99-97E5-16793E3FF17B}");
                public static readonly ID headerCallbackID = new ID("{363CE4DB-7420-4CF5-991F-63F2F05AED1C}");
                public static readonly ID headerImageID = new ID("{458B967C-3A00-4078-A707-4AE52FA2733B}");
                public static readonly string headerImageName = "header Image";
                public static readonly ID itemLogo = new ID("{8C0988CD-ED69-48F5-A1F1-002A698EA6DD}");
                public static readonly ID itemImage = new ID("{C8D71B07-D90E-4F2F-9147-2A0D9F98C489}");
                public static readonly ID linkHeading = new ID("{6E866C83-F438-4A7E-8616-6D8CECE7A3D8}");

            }
            public static class SectionPromoImmagesFields
            {
                public static readonly ID PromoImageID = new ID("{38AEC11C-5DE4-4D4B-919A-999B6319EF48}");
                public static readonly string promoImageName = "promo Image";
                public static readonly ID PromoLogoID = new ID("{5911EDB6-23BA-4E3A-A8CC-E6DA25F7935A}");
                public static readonly string promoLogoName = "promo Logo";
                public static readonly ID PromoLinkID = new ID("{05DCBC77-7779-4984-81C9-F74B43D3DBE8}");
                public static readonly string PromoLinkName = "promo Link";
                public static readonly ID PromoALTtextID = new ID("{5C4534AA-050C-4194-BA26-22DDC57CD6EE}");

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
        public static class AirportHeaderTemp
        {
            public static readonly ID AirportLinkTemplatedID = new ID("{434AD097-D3D8-4369-9560-26C98DAF23C7}");
            public static readonly ID AirportHeaderTemplatedID = new ID("{D951CDED-CC1F-49DE-A58A-9BE8061178A9}");
            public static class Fields
            {
                public static readonly ID headerText = new ID("{6F5FB194-5C3D-4EA0-84B9-E82215D1B327}");
                public static readonly ID headerLink = new ID("{6F3785C2-050A-4748-B5CC-A3279E8131B8}");
                public static readonly ID headerImage = new ID("{E4E6F98F-D13D-42B1-81D2-874DE1C9CF51}");
                public static readonly ID headerLogoLink = new ID("{F980079D-01C4-47BB-824E-E03030A6119B}");
                public static readonly ID headerBackText = new ID("{570CAD7C-F300-4E35-BC9F-55DD5BAEEC26}");
                public static readonly ID headerBackLink = new ID("{917F23E1-126C-4404-A425-194965F4B101}");
                public static readonly ID airportcode = new ID("{92218134-9CF6-4271-A877-77C7A335DC52}");
            }
        }

        public static class FooterTemplate
        {
            public static readonly ID TemplatedID = new ID("{F72120FB-FCE1-4603-A2BB-A492CBBED142}");
            public static readonly ID SeoTemplateID = new ID("{20B33133-18FA-4C8E-9E58-E112A0C97EF7}");
            public static readonly ID LocationTempID = new ID("{8ABA5211-2B44-4E9F-860B-FD28DDAE9054}");
            public static readonly ID TownshipTempID = new ID("{22C02FBC-20BE-4F58-9E27-562588A523F8}");
            public static readonly ID CommercialTemplateID = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
            public static readonly ID ResidentialTemplateID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID PropertiesList = new ID("{B0B87592-3283-4A77-B4D5-EC13CD1FCBE4}");
            public static readonly ID Seoprop = new ID("{F47F1751-7292-4D8B-B0A1-433913B2B88B}");
            public static readonly ID PropertyName = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
            public static readonly ID PropertyLink = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
            public static readonly ID Location = new ID("{0E915D68-BE8C-4439-AF6C-9EC488D5BD9F}");
            public static readonly ID SEOFooter = new ID("{AE2AC42E-61EF-4A35-AC1E-EBBC160496AA}");


            public static class LinkFields
            {
                public static readonly ID Link = new ID("{8CEDC322-2AA6-4EC6-8800-DF54977EC81B}");
                public static readonly string LinkName = "Link";
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID PropertyLinkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string PropertyTarget = "PropertyLinkID";
            }

            public static class FooterFields
            {
                public static readonly ID MainNavigations = new ID("{D75E4BD0-5AA8-4C02-A2E9-A8644D54F8BA}");
                public static readonly ID CopyRight = new ID("{0E4BD0A8-5392-433A-94E9-1D6262DAD086}");
                public static readonly ID SocialLinks = new ID("{5988C040-71C8-48F6-8631-F1FE1B6E4238}");
            }
            public static class FooterHeading
            {
                public static readonly ID FooterHeadingTemp = new ID("{4AA14A4D-5688-441C-90EF-735069AAC78D}");
                public static readonly ID Heading = new ID("{29D47522-BB3E-485B-8B7B-7CB2902DEBC8}");
                public static readonly ID Links = new ID("{0E25E75E-3845-4257-A5AB-35038150C6C5}");
                public static readonly ID SubHeading = new ID("{04670CC4-0339-41FD-A2FE-562B5EC1673D}");
                public static readonly ID isSeoFooter = new ID("{8232F720-0030-46F6-AD34-9B9C5E69CF0C}");
                public static readonly ID LocationID = new ID("{B380C8A2-8D5C-4A0E-ABD4-D0AC214E3824}");
                public static readonly ID TownshipLocation = new ID("{BC07C5A5-A969-4934-84A2-297E5D279E72}");
            }
        }
        public static class PageTemplate
        {
            public static class PageTemplateFields
            {
                public static readonly ID BreadCrumbTitle = new ID("{175839B7-01DE-49C6-8095-D089BEAE8182}");
            }
        }
        public static class SeoBreadcrum
        {
            public static class fields
            {
                public static readonly ID ParentNodes = new ID("{0F5833EF-FBFC-44E7-A375-AFF513EB8CC0}");
            }
        }
        public static class otherProjectsTemp
        {
            public static class fields
            {
                public static readonly ID projects = new ID("{8872DB71-3496-44AF-9896-A2B5E2F67A83}");
                public static readonly ID Heading = new ID("{FFC5A9B0-1F59-4912-BD43-51FB1CE93AD4}");
            }
        }
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
    }
}
