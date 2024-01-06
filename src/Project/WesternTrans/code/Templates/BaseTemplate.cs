using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Templates
{
    public class BaseTemplate
    {
        public class Fields
        {       
            public static readonly ID CTALink = new ID("{EA04F485-FC1C-4B34-9254-663B9C36154D}");
            public static readonly ID CTAText = new ID("{427C783B-B3AB-4B6B-AA94-634B295D0279}");
            public static readonly ID isExternalLink = new ID("{89CEF00A-67B7-443D-B364-68A8CD58F92C}");
            public static readonly ID HTMLText = new ID("{DBBB0B25-CF8D-43FE-886E-C0015C0EBFDF}");
            public static readonly ID Image = new ID("{E69ACBFB-DA11-4F8E-8EEE-75CF1258880D}");
            public static readonly ID ImageAltText = new ID("{ABC0ED02-469D-4E63-835A-88010FAC755F}");
            public static readonly ID Heading = new ID("{1B127634-AD1B-4DE0-AABE-C1B4EDDF859C}");
            public static readonly ID Link = new ID("{5CF01601-8ECE-4B2C-9101-C7C14E2DB44F}");
            public static readonly ID DatasourceTreeList = new ID("{02D4FC9B-8E59-42E4-B9F8-EF26E18DF5D2}");
            public static readonly ID ControllerActionDropdown = new ID("{92E5E9D1-A1D5-4D8C-BABE-CE69D470609C}");
            public static readonly ID ControllerAction = new ID("{DED9E431-3604-4921-A4B3-A6EC7636A5B6}");
            public static readonly ID Designation = new ID("{627977E7-4AE7-4071-AF71-9F9C981568DA}");
            public static readonly ID Name = new ID("{399ED991-78C4-4331-8321-07E18C58384A}");
            public static readonly ID RootItem = new ID("{CBC3406B-ED8A-4847-909D-3735716BA49D}");
            public static readonly ID Logo = new ID("{E748D808-64C1-4DEC-9718-F35CF9689E4B}");
            public static readonly ID Description = new ID("{46280F1B-FB23-40E1-9C4E-3EC861D537C0}");
            public static readonly ID SubHeading = new ID("{A030DA33-47FE-444B-B7FE-9B766192E1E3}");

            public class ContactUs
            {
                public static readonly ID ContactCityDropdown = new ID("{9F311092-8C96-4F46-AA3C-2674A67661E9}");
            }
            public class MediaLibrary
            {
                public static readonly ID Title = new ID("{3F4B20E9-36E6-4D45-A423-C86567373F82}");
            }

        }

        public class Pages
        {
            public static readonly ID Home = new ID("{16191AFC-BCE4-40A7-A146-89793183A5B7}");
            public static readonly ID AboutUs = new ID("{D734B153-0D07-44AE-9824-5B56DEB20981}");
            public static readonly ID Business = new ID("{C62A78C9-E37C-4E52-971A-5CFFDA4F010D}");            
            public static readonly ID OneVisionOneTeam = new ID("{AD126C29-99E8-460B-8C13-C75E9EE9458D}");
            public static readonly ID LegalDisclaimer = new ID("{9C5C7132-DDBA-41CD-8B0C-4EEDB7EDD6F5}");
            public static readonly ID TermsOfUse = new ID("{6B43F5D0-AF85-4388-8E10-478FC9E09E5D}");
            public static readonly ID PrivacyPolicy = new ID("{0BDE44BF-6051-44BC-8B59-9F8F281200C1}");
            public static readonly ID Investor = new ID("{3557B200-A91D-43BB-B09F-D2545DEE89BB}");
            public static readonly ID InvestorDownload = new ID("{DD184C00-73E8-47D4-81E6-708E1A6591E2}");
            public static readonly ID CorporateGovernance = new ID("{EF41AD11-2A88-4616-BFFD-34DC6CA86057}");
            public static readonly ID ContactUs = new ID("{98569AD2-F18D-4A78-8FB9-C8651CE2A0C3}");
            public class AboutUsPages
            {
                public static readonly ID ChairmanMessage = new ID("{715F7FDB-86A4-4B9D-8C26-D4CF17F713FC}");
            }           
        }
        
        public class ContactForm
        {
            public class FormFields
            {
                public static readonly ID SectionHeadingField = new ID("{DFAC13E6-ABBC-4AD6-8FE2-5D1C3B078AF7}");
                public static readonly ID NameField = new ID("{A976AA64-8CF4-489F-B205-E754A3547D51}");
                public static readonly ID EmailField = new ID("{DDEC8A5D-68C5-405E-94E4-F6A27FC45BA8}");
                public static readonly ID MessageField = new ID("{53EDDCC4-70E0-4955-A725-E12E88C9C84D}");
                public static readonly ID InquiryDropDownField = new ID("{8C9E5A0C-BB18-49C5-8400-2072EDA160B2}");
                public static readonly ID SubmitField = new ID("{09E16AEA-6E1A-4809-ACC4-7356828C321E}");
                public static readonly ID TermsField = new ID("{832836B8-65C1-4DEE-93E6-C8EF836224A2}");
            }

            public class FormFieldDataTypes
            {
                public static readonly ID Input = new ID("{0908030B-4564-42EA-A6FA-C7A5A2D921A8}");
                public static readonly ID Dropdown = new ID("{9121D435-48B8-4649-9D13-03D680474FAD}");
                public static readonly ID Button= new ID("{94A46D66-B1B8-405D-AAE4-7B5A9CD61C5E}");
                public static readonly ID MultiLineInput = new ID("{D8386D04-C1E3-4CD3-9227-9E9F86EF3C88}");
                public static readonly ID InquiryDropDownField = new ID("{8C9E5A0C-BB18-49C5-8400-2072EDA160B2}");
                public static readonly ID SubmitField = new ID("{09E16AEA-6E1A-4809-ACC4-7356828C321E}");
            }
            public class FormDataTypeFields
            {
                public static readonly ID PlaceholderText = new ID("{A6972F08-CBB5-4830-A8C9-8A0AE34650D1}");
                public static readonly ID MinAllowedLength = new ID("{9F7DA002-7B86-436B-8DDA-7EFE5837B88E}");
                public static readonly ID MaxAllowedLength = new ID("{71061C01-A495-4BF3-A52C-D378346BFAE2}");
                public static readonly ID DefaultValue = new ID("{B775BCA9-2605-4D60-B367-A0C354BE2504}");
                public static readonly ID Required = new ID("{8A43AF61-0812-4B37-A343-96CDE3F12BB1}");
                public static readonly ID LabelTitle = new ID("{71FFD7B2-8B09-4F7B-8A66-1E4CEF653E8D}");
                public static readonly ID DropDownDefaultSelection = new ID("{21ED172D-97E2-4AC4-8EAF-0A8860B891F4}");
                public static readonly ID DropDownOptionslabel = new ID("{3A07C171-9BCA-464D-8670-C5703C6D3F11}");
                public static readonly ID MultiLineRows = new ID("{E43E4E5F-6B45-4E3B-8D85-85AB87D2CC12}");
                public static readonly ID Text = new ID("{9666782B-21BB-40CE-B38F-8F6C53FA5070}");
            }          

        }
    }
}