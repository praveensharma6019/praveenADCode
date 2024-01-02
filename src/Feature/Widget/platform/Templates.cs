using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.Carousel.Platform
{
    public static class Templates
    {

       
    public static class Commondata
    {
        public static readonly ID ItemID = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");
        public static class Fields
        {
            public static readonly ID morearticlesID = new ID(" {88198DB8-21E4-4DF3-B655-9C0F7DC5EA79}");
            public static readonly ID blogLinkId = new ID(" {C0BC1F88-F412-4A77-A43B-1057F4B0A21F}");
        }
    }
    /// <summary>
    /// Feed data component : (Hero Banner - ({4A421518-7B4A-4D43-AF9B-94E17E00EC9F})
    /// </summary>
    public static class FeedsData
        {
            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static class Fields
            {
                public static readonly ID srcID = new ID("{B5F61442-FF0F-46A5-90A8-D6D387DE24A0}");
                public static readonly string srcFieldName = "logo";
                public static readonly string LinkFieldName = "link";
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly string headingFieldName = "heading";
            }
        }
        /// <summary>
        /// Township data (about us) - (Hero Banner - ({4A421518-7B4A-4D43-AF9B-94E17E00EC9F})
        /// </summary>
        public static class TownshipData
        {
            public static readonly ID bannerTemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static class Fields
            {
                public static readonly ID ThumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly string ThumbieldName = "thumb";
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly string headingFieldName = "heading";
                public static readonly ID subheadingID = new ID("{89B0A8ED-0EE8-4512-B518-AB2C4C2A0B9E}");
                public static readonly string subheadingFieldName = "subheading";
                public static readonly ID class1ID = new ID("{548B5011-641A-4BD4-B6AE-EC9BCDD853B5}");
                public static readonly ID class2ID = new ID("{1266EF65-0FFD-4602-8A14-2D535F41DEB4}");
                public static readonly ID imgType = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");

            }
        }
        /// <summary>
        /// Allaccolades component - (Allaccolades - {27B219B5-0633-4CC2-AE10-20A0D48F9250})
        /// </summary>
        public static class AllaccoladeDate
        {
            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static class Fields
            {
                public static readonly ID ImageID = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string ImageFieldName = "Image";
                public static readonly ID DateID = new ID("{9F891466-3A4A-40FE-B1BC-EC164207949E}");
                public static readonly string DateFieldName = "Date";
                public static readonly ID winnerID = new ID("{594C1325-6A84-4FA6-9565-B5EEF944DF6A}");
                public static readonly string winnerFieldName = "winner";
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly string TitleFieldName = "Title";
            }
        }
        /// <summary>
        /// TimeLine Component : (Image Template - {494F095B-B942-46AF-AFDC-6833DC5504A1} &
        ///                         (Hero Banner - ({4A421518-7B4A-4D43-AF9B-94E17E00EC9F})
        /// </summary>
        public static class Timeline
        {
            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static class Fields
            {
                public static readonly ID ThumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly string ThumbieldName = "thumb";
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly string headingFieldName = "heading";
                public static readonly ID subheadingID = new ID("{89B0A8ED-0EE8-4512-B518-AB2C4C2A0B9E}");
                public static readonly string subheadingFieldName = "subheading";
                public static readonly ID TitleID = new ID("{36DFD09C-9819-48CD-A9BA-DA74CBD686AF}");
                public static readonly string TitleFieldName = "Title";
                public static readonly ID imgType = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");
                public static readonly ID mobileImage = new ID("{86568F07-2F96-4C45-A847-7DB75C165DC9}");

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
            }
        }
        public static class LocationData
        {
            public static readonly ID Id = new ID("{46D86DD7-9C05-4F7D-8F30-38B8110A29FD}");
            public static class Fields
            {
                public static readonly ID Key = new ID("{275A46E5-EFCE-48F7-A3BC-4BD744A0674E}");
                public static readonly ID Label = new ID("{5287C319-3150-4DB3-98E5-9FACCC771663}");
            }
        }

        public static class EmployeeCardTemplate
        {
            public static readonly ID Id = new ID("{1C238DA7-7BE4-429E-BEAE-19B3DB6A7CAC}");
            public static class Fields
            {
                public static readonly ID Image = new ID("{27FE7B1E-DB46-48C1-857E-C53FCF77F5C8}");
                public static readonly ID Heading = new ID("{827A9102-9F34-43C9-9A67-EE8328F05F98}");
                public static readonly ID Detail = new ID("{FC377362-2449-4773-89F5-8E1781A9810F}");
                public static readonly ID Logo = new ID("{4F1246AD-7141-45E0-BE11-568A14CBE083}");
            }
        }
        public static class ContentTemplate
        {
            public static readonly ID Id = new ID("{C7F2F314-8A8E-4C6A-A9EF-385CDF2422DD}");
            public static class Fields
            {
                public static readonly ID PageData = new ID("{2DF49C4C-AD3C-412B-84F3-428817C8AFD0}");
                public static readonly ID PageHeading = new ID("{F03A98C3-3A24-4585-A07F-5CF16D909D5B}");
                public static readonly ID Upcoming = new ID("{643451FE-9BC2-446D-BB54-6F26E6462453}");
                public static readonly ID Year2021 = new ID("{46F5C1E4-A925-4D1C-8559-6E72B31005AA}");
                public static readonly ID Year2020 = new ID("{33CD74AD-8DBD-4EB4-9C3A-162F126A7A2B}");
                public static readonly ID Events = new ID("{EC44A9A3-F79F-446B-A1FF-C28979BA80C8}");
            }
        }
        public static class CommunicationCornerTemp
        {
            public static readonly ID TemplateId = new ID("{2FBDA2B6-BA1B-472F-8040-3EB3059024C7}");
            public static readonly ID ItemID = new ID("{E20A905E-A436-4179-84A3-5A4221FE012E}");
            public static readonly ID BlogAnchorsID = new ID("{9C23584B-262A-4523-99BE-65B395431E18}");
            public static readonly ID BLogSelectionID = new ID("{13E2C8D3-4A96-4B10-B93B-F03C5B8DCB87}");
            public static class Fields
            {
                public static readonly ID CommunicationCornerlinkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string linkName = "link";
                public static readonly ID blogdataID = new ID("{115890C6-B02F-4AE2-979D-B1B76DAE97C0}");
                public static readonly string bloglinkName = "Link";
                public static readonly ID src = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string srcName = "Image";
                public static readonly ID title = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID redirectUrl = new ID("{2181C7A4-C1D3-451D-BB7F-8B35EAF0D92E}");
                public static readonly ID slug = new ID("{15F5F951-BF5A-42DB-9AF9-8EF22711273D}");
                public static readonly ID CatagorytitleID = new ID("{CA5231DE-80EC-4990-9051-B23A10E9182D}");
                public static readonly ID Body = new ID("{261596D0-34C2-47CE-92B3-DD0911A40E0E}");
                public static readonly ID description = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly ID datetime = new ID("{96553D7C-FDDF-4E27-8F65-3D2F62B548FC}");
                public static readonly ID category = new ID("{09F89371-10E6-48F6-B0D2-0A60442F09BB}");
                public static readonly ID readtime = new ID("{64A8B839-7563-4D8A-B222-B5C7DF229A05}");
                public static readonly ID Quote = new ID("{C5EB1C2E-5470-4B96-8E9B-793FAA823A9E}");
                public static readonly ID QuoteCheckBox = new ID("{3C94169C-907F-4CCA-8ABE-E48FC245FDCF}");
                public static readonly ID BlogTitleFieldID = new ID("{A141D927-B6A9-4BC3-85A7-EE4E8CBCEA8A}"); //Blog Title Field
                public static readonly ID SlugText = new ID("{2928E0C6-2063-4203-8763-DF2B94918037}");
                public static readonly ID blogSchema = new ID("{2D72948E-7F08-4BC1-B204-AD81CF6FB153}");
            }
        }
        
        public static class RestaurantInformationTemp
        {
            public static readonly ID TemplateID = new ID("{47843FFC-527E-48A5-BB98-86DD94EFA078}");
            public static class Fields
            {
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID DiscountID = new ID("{A82EB415-BE69-4B16-8994-02A7160814BD}");
                public static readonly ID ImageID = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string srcName = "Image"; // logo
                public static readonly ID ThumbnailID = new ID("{BC77A6D4-1B7B-4659-9E19-417FB4717D40}");
                public static readonly string ThumbnailName = "Thumbnail"; // src
                public static readonly ID FoodCatagoriesID = new ID("{C976E663-1FD4-4258-B890-A5EEB9BA33AA}");
                public static readonly ID foodPricesID = new ID("{8B5583CF-5DFA-4655-98DC-124725B3AB29}");
                public static readonly ID locationID = new ID("{5ED9FA8B-6CD3-447F-85D6-09D5D9C1845A}");
                public static readonly ID StatusID = new ID("{B03511F9-2D26-4DED-8A39-7EB683B7D578}");
                public static readonly ID OtherRestaurantID = new ID("{D9691A9E-0F97-492D-A1C8-43946A1211AA}");
            }
        }
        public static class RestaurantMenuTemp
        {
            public static readonly ID TemplateID = new ID("{494F095B-B942-46AF-AFDC-6833DC5504A1}");
            public static readonly ID DatasourceTemplateID = new ID("{22011FA7-E290-4103-BE3F-C4DF3D5AFF36}");
            public static class Fields
            {
                public static readonly ID thumbID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly string thumbName = "thumb"; // thumb
                public static readonly ID RestaurantMenuSelectionID = new ID("{56D258CA-AA3A-4BDB-A5AC-50C5196EAB9C}");
            }
        }

        public static class JobContentDetail
        {
            public static readonly ID Id = new ID("{BEE32C05-15B0-44F7-AE2A-220E4875766E}");
            public static class Fields
            {
                public static readonly ID Role = new ID("{9D7F8433-0DCC-40E1-BCB6-732F3131E799}");
                public static readonly ID Department = new ID("{EEED0BE0-74C6-4251-A556-AC7E8CF5B233}");
                public static readonly ID Location = new ID("{96A58431-8725-43CB-AABA-D0F4C9F89D53}");
                public static readonly ID Description = new ID("{8280F998-4278-4829-A7AB-1356A5F30483}");
                public static readonly ID DownloadText = new ID("{11DBF0E5-7289-41CF-BC4D-59B828F516E2}");
                public static readonly ID DownloadUrl = new ID("{7CA3987E-F03D-42B9-9A36-CC271E78952E}");
                public static readonly ID ShareText = new ID("{C56A2057-1A18-4823-9520-1C4EAC964949}");
                public static readonly ID ShareUrl = new ID("{B0E775C0-B43A-4CDC-8F4E-3E0BC16611D1}");
                public static readonly ID ButtonText = new ID("{61009AB8-0EC3-46AF-AF25-4497AEB9E70F}");
                public static readonly ID ButtonUrl = new ID("{6C340194-5C9F-4DA4-A032-4AD3D1BD4C7B}");
                public static readonly ID RealityLogo = new ID("{3FCD3FA3-0B9C-4B8B-9909-4852E9FC068F}");
                public static readonly ID RealityAlt = new ID("{85139F26-D4F9-4B0F-8BE1-AD6BB54564A4}");
            }
        }


        public static class RestaurantTabItem
        {
            public static readonly ID TemplateID = new ID("{7910C884-DACE-4FA0-B1F5-EDEFE8CB71C6}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{5F6DCCF7-CAC3-4EA6-AD01-298820CB3B38}");
                public static readonly ID Link = new ID("{9AC13A78-8D69-41DD-9562-44A6F165AAE5}");
                public static readonly ID LinkTitle = new ID("{4186BFAB-405C-444E-A10A-BA75E9D32750}");
            }
        }

        public static class RestaurantTabDataItem
        {
            public static readonly ID TemplateID = new ID("{3544E827-A727-431D-A699-B4850149CA06}");

            public static class Fields
            {
                public static readonly ID Tabs = new ID("{7FE0150E-9B4C-4025-9764-70601BF8C7D7}");

            }
        }


        public static class RestaurantContentData
        {
            public static readonly ID Id = new ID("{3DB9C28E-4381-4D92-9F59-F41C06B3437E}");
            public static class Fields
            {
                public static readonly ID about = new ID("{F9401FE8-E1C6-464D-9EC8-59C57FD576A6}");
                public static readonly ID menu = new ID("{84AC7B53-1145-4C01-B95F-02D02D672E2B}");
                public static readonly ID otherInfo = new ID("{EE05AB86-ABAA-4220-B79D-59EAF9B5648E}");
                public static readonly ID moreReastaurant = new ID("{894801C6-A0FE-4F7E-8DAC-C8B6DA163A1B}");
                public static readonly ID Restaurantfor = new ID("{2C72C297-D4EF-42AB-8EB5-86D599F0DA7B}");
                public static readonly ID count = new ID("{18D20931-20F0-4FC2-A503-EA05F87B9859}");
                public static readonly ID member = new ID("{7741BD5A-4F59-4609-96B7-03F49A509354}");
                public static readonly ID menuDownload = new ID("{90C898BF-0C1E-43DD-944A-5AEB3871E140}");
                public static readonly ID share = new ID("{01C99E08-5D3E-4650-B00C-1F778A710281}");
                public static readonly ID aboutReastaurant = new ID("{10F6E4A6-CB09-4F76-9842-D97783A72923}");
                public static readonly ID callUs = new ID("{C919B49B-6E43-45C9-B8E8-82BF541056A7}");
                public static readonly ID contactNum = new ID("{72A76ADB-C161-4143-A6B2-25C25E0D5DEA}");
                public static readonly ID openNow = new ID("{988F01C5-4898-4652-953E-D9576365B8C1}");
                public static readonly ID timeSlot1 = new ID("{571D88AC-4A59-4BF9-AA05-2F2002545903}");
                public static readonly ID enquireNow = new ID("{C02343D2-DAA3-4F65-9B9B-85F3A02C3CD6}");
            }
        }

        public class TopBarItem
        {
            public static readonly ID TemplateID = new ID("{AE067030-B69B-43A2-A065-12AD89E4D65A}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{EDC1CA95-EFFC-452A-9B10-713267E6F034}");
                public static readonly ID Location = new ID("{7B31B004-5C23-4C2A-A706-6D31889CF86D}");
                public static readonly ID RoomPrice = new ID("{348E6CE4-D056-4EC1-A8BE-80531D25D611}");
                public static readonly ID Link = new ID("{D4B0372D-775C-46CC-916D-074E5AE70D8F}");
                public static readonly ID LinkTitle = new ID("{1C229ABD-6A3E-411E-9E04-C1C395C23AFE}");
                public static readonly ID About = new ID("{83F0405A-B5A3-47A9-AA34-A359E5033E30}");

            }
        }

        public class FeatureItem
        {
            public static readonly ID TemplateID = new ID("{8FA2F5A1-D9DE-4461-BE18-BBFE38B7CCD1}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{54FAC960-179C-4171-8A1B-1AF30BD6EE8C}");

                public static readonly ID Icon = new ID("{76124162-ADDB-42A1-8E0A-E91C5F243DD6}");
            }
        }

        public class RoomTitleItem
        {
            public static readonly ID TemplateID = new ID("{9476BFB8-A36D-40F4-BE42-81BE33BD025B}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{ECBFF07A-5910-4240-8D9E-162C5CB2B843}");
                public static readonly ID Features = new ID("{8FF7428F-24F4-45EA-B650-BFB57F04E471}");

            }
        }

        public class RoomInfoTabDataItem
        {
            public static readonly ID TemplateID = new ID("{02FC3C38-332F-480E-B4CA-25F735BEB98D}");

            public static class Fields
            {
                public static readonly ID Tabs = new ID("{01868986-DE36-443A-B495-682C42B43BB6}");
            }
        }

        public class RoomInfoTabItem
        {
            public static readonly ID TemplateID = new ID("{C484DBCC-ED3D-448C-95CB-ABCD7B0DB23D}");

            public static class Fields
            {
                public static readonly ID TabTitle = new ID("{79BACF3C-9A88-41F3-8281-6D3EEF298F50}");
                public static readonly ID Target = new ID("{3E3AE9B1-5B8A-470E-A86B-7AA2063E65D1}");
            }
        }

        public class FacilityDataItem
        {
            public static readonly ID TemplateID = new ID("{5318BDBD-08E0-46D6-8D43-37D57B07036A}");

            public static class Fields
            {
                public static readonly ID Facilities = new ID("{B74EBEFF-3B0C-4207-BED0-8EDCE016C2C5}");
            }
        }

        public class FacilityItem
        {
            public static readonly ID TemplateID = new ID("{2F13E3A8-0536-4B03-B5A5-D40BE37FC2B5}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{06A5728F-528A-4040-8672-B8E369A4847E}");

                public static readonly ID Icon = new ID("{9AE3D72F-5586-4A36-85B9-50E7789FCDD3}");
            }
        }

        public class OtherRoomsItem
        {
            public static readonly ID TemplateID = new ID("{E9C4789A-144A-46C8-8680-758FCF675097}");

            public static class Fields
            {
                public static readonly ID Source = new ID("{6F3B41B9-1634-4A77-83E2-9314AB373AED}");

                public static readonly ID Title = new ID("{5C78BD39-664F-4A55-8F5F-0E79C12FF4DB}");

                public static readonly ID NonMemberPrice = new ID("{3CE53345-50CF-4FF8-8DEA-0F91D4A6478C}");

                public static readonly ID MemberPrice = new ID("{B9C427BE-137D-4397-B2F0-3D4898653C20}");

            }
        }

        public static class TimingsItem
        {
            public static readonly ID TemplateID = new ID("{8C93CD22-0524-4584-84B0-9CE291810F20}");

            public static class Fields
            {
                public static readonly ID Day = new ID("{504E91B2-1C18-45A2-8036-6A4960C72C0D}");
                public static readonly ID Time = new ID("{7AA2BBA2-B009-4ECC-9979-D0D7C4B9495D}");
            }
        }

        public static class OpentimingItem
        {
            public static readonly ID TemplateID = new ID("{D35E243F-71F4-41EA-AC7C-4391FA2392EE}");

            public static class Fields
            {
                public static readonly ID Data = new ID("{681A44F2-4696-4ACD-93D3-A2595B1827B7}");

            }
        }

        public static class LifeAtAdaniTemplate
        {
            public static readonly ID TemplateID = new ID("{438FE4B2-3383-46B9-B7AA-D596052918E1}");

            public static class Fields
            {
                public static readonly ID Label = new ID("{60590C83-679D-470A-B489-0F493F6F7F06}");

                public static readonly ID Content = new ID("{512D0A91-0391-4FED-AD56-AF2B607D48AC}");

                public static readonly ID Imgsrc = new ID("{7CA744A4-BFCD-4DC9-A61A-671EB7A3580B}");

                public static readonly ID Imgalt = new ID("{5B8A212A-E3C1-42B6-9A38-1CF0BB987163}");

                public static readonly ID ViewAllJobs = new ID("{96AC80CE-9CB7-42A4-A2E3-84B46D145C59}");

                public static readonly ID ViewAllJobsLink = new ID("{0042CA1A-4E33-4301-A2DA-689AB68BD25D}");

            }
        }

        public static class ProjectActionTemplate
        {
            public static readonly ID TemplateID = new ID("{68026F11-7326-4426-83E0-39E5237C682B}");

            public static class Fields
            {
                public static readonly ID ButtonText = new ID("{09DDE028-FCC7-4B60-AD13-ADB8ED9CBC39}");

                public static readonly ID imgTitle = new ID("{FD6E4FE6-1D3D-45B2-82A9-C25EB861078E}");

                public static readonly ID ModalTitle = new ID("{B88EE9C2-6AF5-481F-8FFD-69661E1CDAD1}");

                public static readonly ID DownloadText = new ID("{0E08BE1B-9B6A-4ADE-B040-7D270151C638}");

                public static readonly ID downloadModalTitle = new ID("{CEB8AEB1-CC0E-4161-B2DF-037125226BD4}");

                public static readonly ID ShareText = new ID("{0559BBC1-CEE1-47D2-9A4B-5CF45428C9BC}");

                public static readonly ID Backlink = new ID("{29ED8A16-4B21-42A8-AEBD-75615B077684}");

                public static readonly ID Src = new ID("{F51E72D2-59E1-4E87-A301-95D85AE7C8F2}");

                public static readonly ID ImgAlt = new ID("{F241AEE9-3CA6-4039-A43E-3EC160503757}");

                public static readonly ID Label = new ID("{B96E7B35-9D85-40A1-9E8A-2B4104890518}");

                public static readonly ID Location = new ID("{A62C5D98-995E-49B7-947C-9D189E0CFF09}");

                public static readonly ID CopyLink = new ID("{4DB9053C-01A9-4A1F-B35E-0386C3E8503A}");

                public static readonly ID Email = new ID("{8ECBAECC-B3BE-4BC4-A2B0-3405BB3624B1}");

                public static readonly ID Twitter = new ID("{BD5BF19C-DC39-43C0-A7E9-0372869D632B}");

                public static readonly ID Facebook = new ID("{F9DBE8ED-0A05-447E-AFFE-E7D7A2569BEC}");

                public static readonly ID Whatsapp = new ID("{24095849-6917-4ED9-AF11-96826A234DD4}");

                public static readonly ID Downloadurl = new ID("{33274950-A144-4B17-9CD4-6EE246D38C00}");

            }
        }


        public static class InfoTabLinkItem
        {
            public static readonly ID TemplateID = new ID("{D6F99689-9CF6-4898-837D-B0471C5854FD}");

            public static class Fields
            {
                public static readonly ID Link = new ID("{A7014993-552E-483B-A36A-37B513E1C618}");
                public static readonly ID LinkTitle = new ID("{8A395FB9-93F7-4DAC-A729-5ABA48B49FC4}");
                public static readonly ID Title = new ID("{1AD5D50C-06B0-4158-8FEB-54CB5931E750}");
            }
        }

        public static class InfoTabsItem
        {
            public static readonly ID TemplateID = new ID("{866AE00C-3E6A-4D0D-AE8C-3C83076AF6CF}");

            public static class Fields
            {
                public static readonly ID Data = new ID("{A1AB0D0E-E799-494F-A8D0-1F0999AC77CA}");

            }
        }

        public static class ShantigramItem
        {
            public static readonly ID TemplateID = new ID("{2F0B771F-A4F0-40EC-846C-B6E852EBC720}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{EA1F52FD-D822-4AC4-BA99-F51F75618FD0}");

                public static readonly ID Imgsrc = new ID("{23E8F033-41DA-46F8-8A77-9E285DDE7028}");

                public static readonly ID Imgalt = new ID("{522C2D5C-E9C0-4B9B-9CF6-FEE3D8ADBEF4}");

                public static readonly ID Heading = new ID("{97E5EBDA-9E20-4DE6-A38C-92C5D4CE023F}");

                public static readonly ID SubHeading = new ID("{ACE51ABA-D34E-44A5-968B-C9E5326EC57E}");

                public static readonly ID ThumbImg = new ID("{A4494476-AE4A-4E8A-B9DF-89995678AD23}");

                public static readonly ID Class1 = new ID("{7069C1F9-0889-4894-851F-FE674A412B4B}");

                public static readonly ID Class2 = new ID("{92B21051-8FF6-42A6-A438-7CD167ADF3D0}");

            }
        }
    }
}
