using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.Filters.Platform
{
    public static class Templates
    {
        public static class commonData
        {
            public static readonly ID ItemID = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");
        }

        public static class CommonFOlder
        {
            ///sitecore/content/AdaniRealty/Global/CommonText/Common Text
            public static readonly ID CoomonID = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");
            public static class Fields
            {
                public static readonly ID SearchProjectID = new ID("{CC874222-9F52-423C-80C4-89C7E391F0D0}");
                public static readonly ID SearchID = new ID("{F7C5F9B6-6375-4D74-B469-AC3359BD0D42}");
            }
        }

        /// <summary>
        /// City Description component for location page : (Location - {0FBFC615-FA6A-453B-B479-87B53E30D436})
        /// </summary>
        public static class CityDescriptionFilter
        {
            public static readonly ID TemplateID = new ID("{0FBFC615-FA6A-453B-B479-87B53E30D436}");
            public static readonly ID CitiesID = new ID("{C12B9CFC-5A64-4034-978E-5964C036CADD}");
            public static readonly ID ItemID = new ID("{19FADD3A-572B-4BFE-ACB9-9649672754F0}");

            public static class Fields
            {
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID BodyID = new ID("{261596D0-34C2-47CE-92B3-DD0911A40E0E}");
                public static readonly ID SummaryID = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly ID ThumbID = new ID("{BC77A6D4-1B7B-4659-9E19-417FB4717D40}");
                public static readonly ID LinkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string LinkName = "link";
            }
        }
        public static class AboutUsCommonText
        {
            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static class Fields
            {
                public static readonly ID TitleID = new ID("{36DFD09C-9819-48CD-A9BA-DA74CBD686AF}");
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly ID subheadingID = new ID("{89B0A8ED-0EE8-4512-B518-AB2C4C2A0B9E}");
                public static readonly ID readmoreID = new ID("{E7D58D3B-8DC8-49B1-8F18-8795289925A9}");
                public static readonly ID btnTextID = new ID("{7ACE4C96-1542-4EF2-A725-67EC7439245E}");
            }
        }
        public static class LocationSearch
        {
            public static readonly ID TemplateID = new ID("{F86751CB-C115-4FFD-8116-212C852A6777}");
            public static readonly ID ItemID = new ID("{FD1E0414-35BD-49AE-BB7A-B78A28A49A6B}");

            public static class Fields
            {
                public static readonly ID propertStatusID = new ID("{1911E42E-22B2-4B02-8CDC-C305E0328B3F}");
                public static readonly ID propertyTypeID = new ID("{893904E0-D1BD-4930-8BE4-4ED833810E60}");
                public static readonly ID configurationID = new ID("{B27CC19A-F8E6-4D45-A8BA-35664D4305AD}");
                public static readonly ID propertyConfigurationID = new ID("{C8146A6F-B3BB-440D-AD2C-E0DDF529566A}");
                public static readonly ID priceRangeID = new ID("{9564051A-1778-43C4-B61F-0267F4BCB9EF}");
                public static readonly ID allInclusionID = new ID("{D1B7399A-4E62-4450-90D1-6E7E16E23E1E}");
                public static readonly ID rangeStartID = new ID("{41A2BA39-4DCE-4F40-9DB1-ACA8958E9C7E}");
                public static readonly ID rangeEndID = new ID("{C10C7FC2-6D93-4FD7-B72D-D7EC6A91B032}");
                public static readonly ID residentialID = new ID("{34964610-816E-4C94-9783-E25CB980ADD8}");
                public static readonly ID commercialandRetailID = new ID("{0FF54176-D9FC-4941-AED2-B9AFA3AF0CA4}");
                public static readonly ID clearAllID = new ID("{9190A5B7-F49F-4A3C-AB88-DCDE1C092D38}");
                public static readonly ID applyID = new ID("{3C2D61DE-FA9F-4F35-83EA-FEE259FC4266}");
            }
            public static class PropertyFields
            {
                public static readonly ID PropertyFieldsTempateID = new ID("{FCB0BA21-9AAA-45F8-B86C-738FB269E06F}");
                public static readonly ID TextID = new ID("{59D24D0A-F955-4988-B26F-92039B4DF8BD}");
            }
        }
        public static class PropertyTypesCOmponent
        {
            public static readonly ID TemplateID = new ID("{778D2F65-2BAC-4F22-91AB-19BD6E88FC5F}");
            public static readonly ID ItemID = new ID("{6316DD92-A4F9-4302-BA75-727246707415}");
            public static readonly ID CommercialItemID = new ID("{02E532B9-7111-41F0-BB7F-217AD643897A}");
            public static readonly ID ResidentialItemID = new ID("{7BD1615B-E047-4D13-AD31-03DDCF5201EA}");

            public static class Fields
            {
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
            }
        }
        public static class SearchDataTemp
        {
            public static readonly ID TemplateID = new ID("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");
            public static readonly ID LocationDataID = new ID("{E70B0CBD-5D8C-4C8E-934C-18BF24FB9824}");
            public static readonly ID PropertyDataID = new ID("{4F0109B2-9FF5-41C6-A9F2-10E037B73156}");
            public static readonly ID StatusDataID = new ID("{196831D6-EF30-43D3-A7BE-5A85DDDA0CE5}");

            public static class Fields
            {
                public static readonly ID TypeID = new ID("{EC97C30E-D193-4563-9D27-EC530A01F5C9}");
                public static readonly ID PLaceholderID = new ID("{775C21B2-93A2-41D2-AB02-1AD425D0F596}");
                public static readonly ID LabelID = new ID("{071DF731-3F8D-4BE7-A76A-4120773CD04E}");
                public static readonly ID messageID = new ID("{85367EBD-F344-47B1-BCD0-74B9C78C4147}");
            }
            public static class internalOption
            {
                public static readonly ID LocationOptionID = new ID("{BEF14924-4662-4AD5-A312-1E965504F1E3}");
                public static readonly ID PropertyOptionID = new ID("{50B59F86-1F9C-468C-902B-35B429F6ABFB}");
                public static readonly ID PropertyLabelID = new ID("{F1940AF8-A61F-4219-BC68-A273E27220AD}");
                public static readonly ID PropertyKeyID = new ID("{4713B4F7-731E-4FAF-84CE-04932DA67C10}");
                public static readonly string PropertyKeyName = "Link";
                public static readonly ID StatusOptionID = new ID("{1EC22281-C7BA-459B-97C8-C2763AEA4214}");
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID LatitudeID = new ID("{D04153A0-E7B0-43A8-911A-6BB9C6EC8326}");
                public static readonly ID LongitudeID = new ID("{E9E067E6-9B78-4BFC-B6AD-1D9791D8587A}");
                public static readonly ID TextID = new ID("{59D24D0A-F955-4988-B26F-92039B4DF8BD}");
                public static readonly ID LinkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string LinkName = "link";
            }
        }
        public static class LocationFilter
        {
            public static readonly ID TemplateID = new ID("{EEE5CE6D-3121-47DA-9320-F3E885B700F0}");
            public static readonly ID residentialItemID = new ID("{7BD1615B-E047-4D13-AD31-03DDCF5201EA}");
            public static readonly ID residentialTemplateID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID CommercialItemId = new ID("{02E532B9-7111-41F0-BB7F-217AD643897A}");
            public static readonly ID CommercialTemplateID = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
            public static readonly ID ProjectsStatusDataSourceID = new ID("{8477D409-BE0D-442D-A199-F0411A19C4CB}"); ///sitecore/content/AdaniRealty/Global/Project Status
            public static readonly ID ResidentialConfigurationID = new ID("{DC634EC7-31C9-4F19-961D-817FF2289B1A}"); ///sitecore/content/AdaniRealty/Global/Residential Projects Capacity

            public static class Fields
            {
                public static readonly ID FilterHeadingID = new ID("{C0B053EE-1C70-4731-B3C6-DF6639E2DA3D}");
                public static readonly ID ClearallLabelID = new ID("{C4C6255C-4583-4A31-BB5A-8BED73F53A8F}");
                public static readonly ID ApplyFilterLabelID = new ID("{6C439D6F-51CB-4CF3-8CDF-E37C88E184C4}");
                public static readonly ID PropertiesID = new ID("{B0B87592-3283-4A77-B4D5-EC13CD1FCBE4}");
                //Below Fields Are for Price Filter
                public static readonly ID RangeFilterID = new ID("{B715B0BC-A5F5-423A-8DA0-3194E299FCF6}");
                public static readonly ID PriceRangeLabelID = new ID("{B0DCB3D9-74D9-46B2-9571-8037C6731D16}");
                public static readonly ID AllInclusiveLabelID = new ID("{D29032AB-2164-45C1-B7D4-7C530EE98B18}");
                public static readonly ID MinRangeValueID = new ID("{15726D88-2AD2-4418-85D2-7D7C6A801AAF}");
                public static readonly ID MaxRangeValueID = new ID("{57934F54-A78E-4817-A6B5-88250EDAAA31}");
                public static readonly ID CurrencytypeID = new ID("{9BCE588B-4ED6-4A3D-9D5A-E4A4DA388299}");
                public static readonly ID StatusLabelID = new ID("{9012C0DB-A6EA-4E17-9037-6B43E3B01C15}");
                public static readonly ID ConfigurationLabel = new ID("{788FB9EA-2139-4EF7-96A0-23D443F3560E}");
                public static readonly ID additionalsign = new ID("{7235C248-D726-447D-AF98-9674492288D9}");


                public static readonly ID propertyPricefilter = new ID("{6596659C-0AAB-45A1-BC2D-DE84E13A0D6C}");
                ///sitecore/templates/Feature/Realty/Common/_Property/Property Detail/property Price filter

            }
        }
        public static class SEOheadingDescription
        {
            public static class Fields
            {
                public static readonly ID src = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                //public static readonly ID slug = new ID("{15F5F951-BF5A-42DB-9AF9-8EF22711273D}");
                public static readonly ID cityName = new ID("{A959C5FB-8880-4204-AD66-8007045DE1AF}");
                public static readonly ID cityDetail = new ID("{0A7BA4CA-B42C-45CA-9EB8-7E7C26CE37DF}");
                public static readonly ID readmore = new ID("{AA02C38C-21F3-4BE1-A6AE-A30B460D5568}");
                public static readonly ID heading = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID IsSEOPage = new ID("{1A572BA7-D50F-490C-9A69-42AB31286087}");
            }
        }
    }
}
