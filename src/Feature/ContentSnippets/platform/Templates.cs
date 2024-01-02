using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform
{
    public static class Templates
    {
        public static readonly ID commonItem = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");

        public static readonly ID commondataitem = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");  ///sitecore/content/AdaniRealty/Global/CommonText/Common Text
        public static readonly ID readlessID = new ID("{4599D97E-B034-4BDF-B2F5-524C237C59EC}");
        public static readonly ID readmoreID = new ID("{F769CB9A-813B-48FE-9BDF-079BF52BFDE7}");
        public static class commonData
        {
            public static readonly ID TemplateID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID ItemID = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");

            public static class Fields
            {
                public static readonly ID blogLinkId = new ID(" {C0BC1F88-F412-4A77-A43B-1057F4B0A21F}");
            }
        }
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
        public static class AOGBriefTemp
        {
            public static readonly ID pageHeading = new ID("{251AC3D8-F7B3-4DBA-A1D7-2CD22170F764}");
            public static readonly ID pageHeadingInGradiant = new ID("{2E465F5B-8A58-470C-A6C9-85196AEE2026}");
            public static readonly ID sup = new ID("{FB68A611-E425-4B89-BB3F-60E553D56549}");
            public static readonly ID subHeading = new ID("{5B5F1A84-D1F9-4E8C-A14A-DEB9210FFDB9}");
            public static readonly ID description = new ID("{263A68A2-4F05-4262-9839-6ED141F3FD4E}");
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
            public static readonly ID SubTitle = new ID("{C095C5FC-6D9E-47BC-B37B-1387C4800265}");
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

        #region AboutAdani

        public static class AboutAdani
        {

            public static readonly ID TemplateID = new ID("{9968A4E8-89DF-45A0-B3B2-EA3FCB1406DB}");
            public static readonly string TemplateName = "LeadersData";
            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID Name = new ID("{BC40052C-4198-4352-8BAA-1088A754E7CD}");
                    public static readonly ID Quote = new ID("{6324B285-4EF5-45C9-8F9F-194B22D1D2B2}");
                    public static readonly ID Designation = new ID("{69EBF323-9B3B-4D90-8E01-C1417C034519}");
                }
                public static class FieldsName
                {
                    public static readonly string Name = "Name";
                    public static readonly string Quote = "Quote";
                    public static readonly string Designation = "Designation";
                }
            }
        }

        public static class AboutAdaniFolder
        {
            public static readonly ID TemplateID = new ID("{0E303CD6-6694-4D0C-9B44-41E4BC2F1F68}");
            public static readonly string TemplateName = "LeadersDataFolder";

        }

        #endregion

        #region AboutAdaniRealty

        public static class AboutAdaniRealty
        {

            public static readonly ID TemplateID = new ID("{F2E25D90-18FB-46CF-8F7A-A798B9B2EBA3}");
            public static readonly string TemplateName = "AboutAdaniRealty";

        }

        public static class AboutAdaniRealtyFolder
        {
            public static readonly ID TemplateID = new ID("{D5CC40EB-92E0-472C-8B9A-A6CCEFA48A5F}");
            public static readonly string TemplateName = "AboutAdaniRealtyFolder";

        }

        #endregion

        #region AboutUs
        public static class AboutUs
        {
            public static readonly ID TemplateID = new ID("{D596F78B-6F97-4E92-B4F8-AE492F307D61}");
            public static readonly string TemplateName = "AboutUs";
        }
        public static class AboutUsFolder
        {
            public static readonly ID TemplateID = new ID("{95AD5645-6973-480D-8E83-E9C435E69690}");
            public static readonly string TemplateName = "AboutUsFolder";
        }

        #endregion

        #region Location
        public static class Location
        {
            public static readonly ID TemplateID = new ID("{4344EE18-0924-454F-8170-B1CC8B9BFEB0}");
            public static readonly string TemplateName = "Location";
        }
        public static class LocationFolder
        {
            public static readonly ID TemplateID = new ID("{3FDB1888-CBED-4915-B8E3-EBF814466227}");
            public static readonly string TemplateName = "LocationFolder";
        }

        #endregion

        #region OurLocation
        public static class OurLocation
        {

            public static readonly ID TemplateID = new ID("{05C8301E-9DB9-4B44-918D-60B54161E8F1}");
            public static readonly string TemplateName = "OurLocation";
            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID ProjectList = new ID("{DE24359B-A90A-476B-A3B8-91D58F3C2022}");

                }
                public static class FieldsName
                {
                    public static readonly string ProjectList = "ProjectList";

                }
            }
        }

        public static class OurLocationFolder
        {
            public static readonly ID TemplateID = new ID("{CC8C6E23-A82E-45C4-98E5-BC2B17B43990}");
            public static readonly string TemplateName = "OurLocationFolder";
        }
        #endregion

        #region HasPageContent
        public static class HasPageContent
        {

            public static readonly ID TemplateID = new ID("{D1B7CD65-6321-46D4-8210-0031CA094037}");
            public static readonly string TemplateName = "_HasPageContent";
            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID HasPageContentTitle = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                    public static readonly ID HasPageContentSummary = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                    public static readonly ID HasPageContentBody = new ID("{261596D0-34C2-47CE-92B3-DD0911A40E0E}");
                    public static readonly ID HasPageContentImage = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                    public static readonly ID HasPageContentThumbnail = new ID("{BC77A6D4-1B7B-4659-9E19-417FB4717D40}");
                    public static readonly ID HasPageContentLink = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                    public static readonly ID SlugText = new ID("{2928E0C6-2063-4203-8763-DF2B94918037}");
                    public static readonly ID Slug = new ID("{15F5F951-BF5A-42DB-9AF9-8EF22711273D}");
                }
                public static class FieldsName
                {
                    public static readonly string HasPageContentTitle = "Title";
                    public static readonly string HasPageContentSummary = "Summary";
                    public static readonly string HasPageContentBody = "Body";
                    public static readonly string HasPageContentImage = "Image";
                    public static readonly string HasPageContentThumbnail = "Thumbnail";
                    public static readonly string HasPageContentLink = "link";
                }
            }
        }


        #endregion

        #region Commercial

        public static class Commercial
        {

            public static readonly ID TemplateID = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
            public static readonly string TemplateName = "Commercial";
            public static readonly ID CommercialItemID = new ID("{02E532B9-7111-41F0-BB7F-217AD643897A}");
        }
        public static class Club
        {

            public static readonly ID TemplateID = new ID("{463784CA-A045-4C87-9EF5-3CE93772F4BD}");
            public static readonly string TemplateName = "Club";
            public static readonly ID ClubItemID = new ID("{B7C13B72-2D02-43DC-9548-81C934CBA5BF}");

        }

        #endregion

        #region Residential
        public static class Residential
        {

            public static readonly ID TemplateID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly string TemplateName = "Residential";
            public static readonly ID ResidentialItemID = new ID("{7BD1615B-E047-4D13-AD31-03DDCF5201EA}");
        }


        #endregion

        #region Property
        public static class Property
        {

            public static readonly ID TemplateID = new ID("{38F56FB0-AB22-4BEB-A142-B4D531ACD492}");
            public static readonly string TemplateName = "_Property";
            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID Location = new ID("{B380C8A2-8D5C-4A0E-ABD4-D0AC214E3824}");
                    public static readonly ID ProjectStatus = new ID("{0350CF91-6ECB-4F5A-BC4C-430D0E32214B}");
                    public static readonly ID SiteStatus = new ID("{C726F4E1-A95A-47E3-B101-76508BF7A465}");
                    public static readonly ID ProjectArea = new ID("{68009697-D024-4247-8FA2-95E3A225236B}");
                    public static readonly ID PropertyLogo = new ID("{E8B52311-A1D9-4B04-B5A0-0682EE79EDBF}");
                    public static readonly ID SimilarProjects = new ID("{03ACB328-43E8-45F4-8639-796126D43E23}");
                    public static readonly ID ProjectLayout = new ID("{ABB0D7C1-C5CD-4184-ABEE-DEA53137963D}");
                    public static readonly ID MapLink = new ID("{76ED815D-19E6-4593-B57E-04CEA5D03CBE}");
                    public static readonly ID UnitSize = new ID("{3CF2B01F-A43C-477A-805B-565A0FB57AB4}");
                    public static readonly ID Possession = new ID("{1503CC29-BC69-43B3-8D7E-067B76301F41}");
                    public static readonly ID Brochure = new ID("{673B0698-703B-48EF-8CA9-B250C81CFD71}");
                    public static readonly ID PropertyType = new ID("{75091E77-938C-4379-84FE-174833285D4C}");
                    public static readonly ID Status = new ID("{8212329C-CF1E-45A6-8919-BF4E955A5DFE}");
                    public static readonly ID Type = new ID("{04CF398B-5FE5-4A5D-93F4-AC750549D77E}");
                    public static readonly ID PropertyTypeTitle = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                    public static readonly ID PropertyPrice = new ID("{33D8D8A2-5674-4AE6-BBA9-B9D9F8C9A4F5}");
                    public static readonly ID allinclusivelabel = new ID("{EB80261D-90DE-43EF-AB01-09514ED6F33D}");
                    public static readonly ID pricestartinglabel = new ID("{01FD9FB1-C5FD-4009-974C-B03CC7515B11}");
                    public static readonly ID PropertyPriceFilterID = new ID("{6596659C-0AAB-45A1-BC2D-DE84E13A0D6C}");
                    public static readonly ID condition = new ID("{BCF10793-231D-45DB-A0C5-D4E1EEAC29BF}");
                }
                public static class FieldsName
                {
                    public static readonly string Location = "Location";
                    public static readonly string ProjectStatus = "Project Status";
                    public static readonly string SiteStatus = "Site Status";
                    public static readonly string ProjectArea = "Project Area";
                    public static readonly string PropertyLogo = "Property Logo";
                    public static readonly string SimilarProjects = "Similar Projects";
                    public static readonly string ProjectLayout = "Project Layout";
                    public static readonly string MapLink = "Map Link";
                    public static readonly string UnitSize = "Unit Size";
                    public static readonly string Possession = "Possession";
                    public static readonly string Brochure = "Brochure";
                    public static readonly string PropertyType = "Property Type";
                    public static readonly string Status = "status";
                    public static readonly string Type = "Type";

                }
            }
        }


        #endregion

        #region GetInTouch

        public static class GetInTouch
        {

            public static readonly ID TemplateID = new ID("{EAC9EC75-1B6B-4A76-BE07-73561B6E2997}");
            public static readonly string TemplateName = "GetInTouch";
        }
        public static class GetInTouchFolder
        {

            public static readonly ID TemplateID = new ID("{24B674E2-105F-4EB9-BF6B-F6C5D793A652}");
            public static readonly string TemplateName = "GetInTouchFolder";
        }
        #endregion

        #region GoodLife

        public static class GoodLife
        {

            public static readonly ID TemplateID = new ID("{EAB6B0AF-84A0-4FDD-9AE3-0C931B326C10}");
            public static readonly string TemplateName = "GoodLifeItem";

        }
        public static class GoodLifeFolder
        {

            public static readonly ID TemplateID = new ID("{C89AB34D-1F50-497A-879D-2AA29A358E44}");
            public static readonly string TemplateName = "GoodLifeFolder";
        }
        #endregion

        #region AboutGoodLife

        public static class AboutGoodLife
        {

            public static readonly ID TemplateID = new ID("{329F7C75-D6F1-497A-A01A-92DDD0C52F77}");
            public static readonly string TemplateName = "About Good Life";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID About = new ID("{EC00F896-B8DD-4B6B-A0B1-BD01B88C851D}");
                    public static readonly ID ReadMore = new ID("{B4B57729-B069-4D32-926A-7A516DABD391}");
                    public static readonly ID Terms = new ID("{2FD6C3A5-86E7-458C-95BE-73193B5AB7E2}");
                    public static readonly ID DetailLink = new ID("{7B607154-FF2E-4AFA-B23E-336506781593}");
                    public static readonly ID ExtraCharges = new ID("{2CC3FDD8-0C93-4617-8460-E46F0A152736}");
                }
                public static class FieldsName
                {
                    public static readonly string About = "About";
                    public static readonly string ReadMore = "Read More";
                    public static readonly string Terms = "Terms";
                    public static readonly string DetailLink = "Detail Link";
                    public static readonly string ExtraCharges = "Extra Charges";

                }

            }
            public static class AboutGoodLifeFolder
            {

                public static readonly ID TemplateID = new ID("{26A7B8C1-B33E-4E67-8BA9-B96C33B17548}");
                public static readonly string TemplateName = "AboutGoodLifeFolder";
            }

        }
        #endregion

        #region FeaturedBlog

        public static class FeaturedBlog
        {

            public static readonly ID TemplateID = new ID("{3D7C4B69-EA89-4B5E-AF6F-1CB4CD5A7B53}");
            public static readonly string TemplateName = "FeaturedBlog";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID Title = new ID("{0B63C6C5-C1B7-458F-9A0D-ACFD35F8CD88}");
                    public static readonly ID Summary = new ID("{19F46055-FC89-45A8-A53B-532FF62EF569}");
                    public static readonly ID Body = new ID("{9CD041C6-D888-44BE-AA8A-209375535C0E}");
                    public static readonly ID Image = new ID("{CCB1C7D7-5B71-4650-BE78-24589806F6E2}");
                    public static readonly ID Link = new ID("{32956AC2-DECA-4FF7-B651-78B89368D861}");
                    public static readonly ID Category = new ID("{09F89371-10E6-48F6-B0D2-0A60442F09BB}");
                    public static readonly ID BlogAnchors = new ID("{9C23584B-262A-4523-99BE-65B395431E18}");
                    public static readonly ID DateTime = new ID("{96553D7C-FDDF-4E27-8F65-3D2F62B548FC}");

                }
                public static class FieldsName
                {

                    public static readonly string Title = "FeaturedTitle";
                    public static readonly string Summary = "FeaturedSummary";
                    public static readonly string Body = "FeaturedBody";
                    public static readonly string Image = "FeaturedImage";
                    public static readonly string Link = "FeaturedLink";
                    public static readonly string Category = "category";

                }

            }
            public static class FeaturedBlogFolder
            {
                public static readonly ID TemplateID = new ID("{C8333721-BA89-405D-992E-9B9554C09028}");
                public static readonly string TemplateName = "FeaturedBlogFolder";
            }
        }

        public static class FeaturesItem
        {

            public static readonly ID TemplateID = new ID("{63D345E4-A790-4821-B210-06D6DCC471EF}");
            public static readonly string TemplateName = "Features";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID FeaturesItem = new ID("{B68FED40-E147-4811-A162-5AED7CC24DBD}");
                }
                public static class FieldsName
                {
                    public static readonly string FeaturesItem = "FeaturesItem";

                }

            }
        }

        public static class FeaturedBlogCategoryListDataItem
        {

            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static readonly string TemplateName = "Hero Banner";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID CategoryTitle = new ID("{CA5231DE-80EC-4990-9051-B23A10E9182D}");
                    public static readonly ID CategoryLink = new ID("{17B31027-C4FF-4E30-8DCE-6D0DBF71E6A4}");
                }
                public static class FieldsName
                {
                    public static readonly string CategoryTitle = "Title";
                    public static readonly string CategoryLink = "Link";

                }

            }

        }

        #endregion

        #region AboutCity
        public static class AboutCity
        {

            public static readonly ID TemplateID = new ID("{5EF4202A-AC67-4605-BDC9-E8FA5679F0AE}");
            public static readonly string TemplateName = "AboutCity";

        }
        #endregion

        #region StaticText
        public static class StaticText
        {
            public static readonly ID TemplateID = new ID("{C7AB7C1B-B2FD-4B47-BD0B-9CDE6117918B}");
            public static readonly string TemplateName = "StaticText";

        }
        #endregion

        #region  RoomDetails-FeaturesList
        public static class Features
        {
            public static readonly ID TemplateID = new ID("{A8A04A17-4CCF-4ACA-B37B-26394BEF6CF1}");
            public static readonly string TemplateName = " Features";
        }
        public static class FeaturesFolder
        {
            public static readonly ID TemplateID = new ID("{5121F5A9-28E0-40A2-9224-4E29E56B679D}");
            public static readonly string TemplateName = " FeaturesFolder";
        }
        #endregion

        #region  RoomDetails-Facilities

        public static class RoomDetailsFacilities
        {
            public static readonly ID TemplateID = new ID("{F309EB6A-B3CA-40BD-A402-BF7D8E4F9545}");
            public static readonly string TemplateName = "Facilities";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID IsMostPopularFacilities = new ID("{57A3C7D4-35A6-4D7F-899C-C1DCA80DCEE2}");
                }
                public static class FieldsName
                {
                    public static readonly string IsMostPopularFacilities = "IsMostPopularFacilities";

                }

            }

        }
        public static class RoomDetailsFacilitiesFolder
        {
            public static readonly ID TemplateID = new ID("{155BB581-6401-488C-8532-0EE27EF882BC}");
            public static readonly string TemplateName = "FacilitiesFolder";

        }
        #endregion

        #region  RoomDetails-FacilitiesCategories

        public static class RoomDetailsFacilitiesCategories
        {
            public static readonly ID TemplateID = new ID("{8E26D194-D410-46F6-8671-B5675229FBB5}");
            public static readonly string TemplateName = "FacilitiesCategories";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID Facilities = new ID("{5B1B547F-06D7-43F4-B76A-59EC31AD2D60}");
                }
                public static class FieldsName
                {
                    public static readonly string Facilities = "Facilities";

                }

            }

        }
        public static class RoomDetailsFacilitiesCategoriesFolder
        {
            public static readonly ID TemplateID = new ID("{035D0799-56B4-4741-894E-91202B1D61E8}");
            public static readonly string TemplateName = "FacilitiesCategories Folder";

        }
        #endregion

        #region RoomTitle
        public static class RoomTitle
        {
            public static readonly ID TemplateID = new ID("{4088A9BD-D4A3-467B-9675-49A375D2A49D}");
            public static readonly string TemplateName = "RoomTitle";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID Features = new ID("{9545E11B-4D29-4164-A26E-3B20386A9422}");
                }
                public static class FieldsName
                {
                    public static readonly string Features = "Features";

                }

            }

        }
        #endregion

        #region RoomInfoTabs
        public static class RoomInfoTabs
        {
            public static readonly ID TemplateID = new ID("{DE26FA26-D9DA-4F90-AD94-16BD7439CF5F}");
            public static readonly string TemplateName = "RoomInfoTabs";

        }

        public static class RoomInfoTabsFolder
        {
            public static readonly ID TemplateID = new ID("{2532DA2B-5C0F-40B4-8582-9EEB7D8FA085}");
            public static readonly string TemplateName = "RoomInfoTabs Folder";
        }
        #endregion

        #region otherRooms
        public static class OtherRooms
        {
            public static readonly ID TemplateID = new ID("{E9C4789A-144A-46C8-8680-758FCF675097}");
            public static readonly string TemplateName = "OtherRooms";

        }
        public static class OtherRoomsFolder
        {
            public static readonly ID TemplateID = new ID("{E6503583-EB1A-4FFF-8C63-77C6BD2663D7}");
            public static readonly string TemplateName = "OtherRoomsFolder";
        }
        #endregion

        #region ClubHeroBanner
        public static class ClubHeroBanner
        {
            public static readonly ID TemplateID = new ID("{F4889F6A-0345-45BC-994E-A347512F211F}");
            public static readonly string TemplateName = "ClubHeroBanner";

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
                public static readonly ID videoposterMobileID = new ID("{6BAC1E87-C807-4F20-84AD-6676FC699819}");
                public static readonly string videoposterMobileFieldName = "videoposterMobile";
                public static readonly ID videoMp4MobileID = new ID("{055C8277-18FA-46DC-81BF-A2D3EB097D7E}");
                public static readonly string videoMp4MobileFieldName = "videoMp4Mobile";
                public static readonly ID videoOggMobileID = new ID("{F346F662-F1D6-4BCD-9BF6-A801FFF7EA1E}");
                public static readonly string videoOggMobileFieldName = "videoOggMobile";
                public static readonly ID SrcMobileId = new ID("{1FF9D71A-82BF-4599-BEA6-6BEBD7C95069}");
                public static readonly string MobileImageFieldName = "Mobile Image";
                public static readonly ID SEOName = new ID("{C9514E8A-DFE9-4D51-9290-96C2EAD34847}");
                public static readonly ID SEODescription = new ID("{9F1C4AA5-3867-4861-A14B-C3288D556F3B}");
                public static readonly ID UploadDate = new ID("{D01BC0E7-4449-4B54-8BCB-C7F8AADF5EB5}");


            }
        }
        public static class ClubHeroBannerDataFolder
        {
            public static readonly ID TemplateID = new ID("{CFEEE506-E158-4F61-A371-74043A422262}");
            public static readonly string TemplateName = "ClubHeroBannerFolder";
        }
        #endregion

        #region AboutAdaniSocialClub
        public static class AboutAdaniSocialClub
        {
            public static readonly ID TemplateID = new ID("{9E204E96-EC09-4F51-987E-736DF896EC5F}");
            public static readonly string TemplateName = "AboutAdaniSocialClub";

        }
        public static class AboutAdaniSocialClubFolder
        {
            public static readonly ID TemplateID = new ID("{F3AB43B1-89FE-489E-99EE-7F5BA22F8C16}");
            public static readonly string TemplateName = "AboutAdaniSocialClubFolder";
        }
        #endregion

        #region ClubHighLights
        public static class ClubHighLights
        {
            public static readonly ID TemplateID = new ID("{5D989CD7-ADAB-4881-8633-985924623312}");
            public static readonly string TemplateName = "ClubHighlights";

        }
        public static class ClubHighLightsFolder
        {
            public static readonly ID TemplateID = new ID("{DB6B760C-2361-4C86-B745-3E0A2029716A}");
            public static readonly string TemplateName = "ClubHighlightsFolder";
        }
        #endregion

        #region AboutClub
        public static class AboutClub
        {
            public static readonly ID TemplateID = new ID("{CD9E92EF-163F-4D5A-B6AA-2665C17F7FC1}");
            public static readonly string TemplateName = "AboutClub";

        }
        public static class AboutClubFolder
        {
            public static readonly ID TemplateID = new ID("{B05B6E43-8DF3-4620-A6CC-73BF9F13F1E6}");
            public static readonly string TemplateName = "AboutClubFolder";
        }
        # endregion

        #region ConfirmBanner
        public static class ConfirmBanner
        {
            public static readonly ID TemplateID = new ID("{15E61415-55AD-4F2A-A9F7-754AC40EE8CC}");
            public static readonly string TemplateName = "ConfirmBanner";

        }

        public static class ConfirmBannerFolder
        {
            public static readonly ID TemplateID = new ID("{B553CFF9-B6BD-406E-8086-AF9ED15D5199}");
            public static readonly string TemplateName = "ConfirmBannerFolder";
        }
        #endregion

        #region Explore
        public static class Explore
        {
            public static readonly ID TemplateID = new ID("{738696B7-2203-49BE-BEF8-D8B42AD0BA6D}");
            public static readonly string TemplateName = "Explore";

        }

        public static class ExploreFolder
        {
            public static readonly ID TemplateID = new ID("{62653E9B-0359-4E36-8CBC-81012DA22772}");
            public static readonly string TemplateName = "ExploreFolder";
        }
        #endregion

        #region OrderDetails
        public static class OrderDetails
        {
            public static readonly ID TemplateID = new ID("{0A634E84-A720-4806-8531-B2C2C4CC0EDF}");
            public static readonly string TemplateName = "OrderDetails";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID OrderDetailsItems = new ID("{40AC0137-8622-4173-AE91-14E493B8E23A}");
                }
                public static class FieldsName
                {
                    public static readonly string OrderDetailsItems = "OrderDetails";

                }

            }

        }
        public static class OrderDetailsItem
        {
            public static readonly ID TemplateID = new ID("{9E63C80F-2895-45F7-A7C3-99C1795DDB0D}");
            public static readonly string TemplateName = "OrderDetailsItem";

        }
        public static class OrderDetailsFolder
        {
            public static readonly ID TemplateID = new ID("{6F97458B-19E8-41CD-853A-F6C4BFA6D460}");
            public static readonly string TemplateName = "OrderDetailsFolder";
        }
        #endregion

        #region SaveDetails
        public static class SaveDetails
        {
            public static readonly ID TemplateID = new ID("{B36E00A9-DA26-44BB-B73A-6C702D8032B4}");
            public static readonly string TemplateName = "SaveDetails";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID SaveDetailsItems = new ID("{FA7D6ECE-710A-43F8-B92F-4371B725C07A}");
                }
                public static class FieldsName
                {
                    public static readonly string SaveDetailsItems = "SaveDetails";

                }

            }

        }
        public static class SaveDetailsItems
        {
            public static readonly ID TemplateID = new ID("{A4FD6AD8-8EE6-489E-B7C1-27B55A25D3A6}");
            public static readonly string TemplateName = "SaveDetailsItems";

        }
        public static class SaveDetailsFolder
        {
            public static readonly ID TemplateID = new ID("{1D8C2AC2-246D-4C31-BDB6-F2ED6A57FC39}");
            public static readonly string TemplateName = "SaveDetailsFolder";
        }
        #endregion

        #region Configuration
        public static class Configuration
        {
            public static readonly ID TemplateID = new ID("{8C296A77-75FC-4B98-B55B-AEA0F1A8FEE4}");
            public static readonly string TemplateName = "Configuration";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID KeysItems = new ID("{E2C0EC29-1DE1-42CF-B7D5-A5C55E411FD8}");
                }
                public static class FieldsName
                {
                    public static readonly string KeysItems = "Keys";

                }

            }

        }
        public static class ConfigurationKeysItem
        {
            public static readonly ID TemplateID = new ID("{772E93CD-D469-4509-BE78-4B516AE3C8E3}");
            public static readonly string TemplateName = "ConfigurationKeyItem";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID Link = new ID("{E2C0EC29-1DE1-42CF-B7D5-A5C55E411FD8}");
                    public static readonly ID keyword = new ID("{E2C0EC29-1DE1-42CF-B7D5-A5C55E411FD8}");
                }
                public static class FieldsName
                {
                    public static readonly string Link = "Link";
                    public static readonly string keyword = "keyword";
                }

            }

        }
        public static class ConfigurationFolder
        {
            public static readonly ID TemplateID = new ID("{EB7CE83D-109E-4F3E-A218-5AC678A55E43}");
            public static readonly string TemplateName = "ConfigurationFolder";
        }
        #endregion

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