using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform
{
    public static class Templates
    {
        public static class NameValueCollection
        {
            public const string NameValueFolder = "{979C4694-EF8D-4D7F-9017-9E3E9903359B}";
            public const string nameValue = "{407FC9F0-CC99-475B-8F51-541094377FBB}";
            public static class Fields
            {
                public static readonly ID NavigationTitle = new ID("{32CFF90D-4FDF-4402-A364-21199E88753D}");
            }
        }

        public static class CityListCollection
        {
            public static readonly ID CityTemplateID = new ID("{D71EDD08-B897-45BB-A1FD-F90666A7F0CE}");
            public static readonly ID KeywordsFolderTemplateID = new ID("{8F4F2A26-49C5-4E2F-9AFB-A4652C015BA2}");
            public static readonly ID KeywordTemplateID = new ID("{64E55D8A-5A56-40EB-9672-4A7F7384D343}");
            public static readonly ID AirportTemplateID = new ID("{486FBEBB-93F6-4E9D-859A-357010D47503}");
            public static readonly ID TerminalTemplateID = new ID("{91D1448B-3F86-47D6-B786-4177E2EB4F85}");

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

        public static class TravelInsuranceConstant
        {
            public static readonly string InsuranceTemplateId = "{57FECE9B-1FD7-4C72-A2AF-4870D2E49F7A}";
            public static readonly string Title = "Title";
            public static readonly string BrandLogo = "BrandLogo";
            public static readonly string TravelLogo = "TravelLogo";
            public static readonly string Heading = "Heading";
            public static readonly string Error = "Error";
            public static readonly string Label = "Label";
            public static readonly string LabelText = "LabelText";
            public static readonly string TNC = "TNC";
            public static readonly string TNCLink = "TNCLink";
            public static readonly string Amount = "Amount";
            public static readonly string Icon = "Icon";
            public static readonly string Description = "Description";
            public static readonly string Id = "Id";
            public static readonly string Tag = "Tag";
            public static readonly string DisclaimerTemplateId = "{3779DB5B-BC4A-401F-8080-4F38F75258D4}";
            public static readonly string InfoTemplateId = "{51B7D025-18B2-4210-8B45-C2D1E9D902E8}";
            public static readonly string BenefitsFolder = "Benefits";
            public static readonly string BreakupsFolder = "Breakups";
            public static readonly string OptionsFolder = "Options";
            public static readonly string TNCApp = "TNCApp";
            public static readonly string IconUrl = "IconUrl";
            public static readonly string BannerImage = "BannerImage";
            public static readonly string MobileBannerImage = "MWebBannerImage";
            public static readonly string FlightBookingWidgetEnable = "FlightBookingWidgetEnable";
            public static readonly string FlightBookingWidgetTitle = "FlightBookingWidgetTitle";
            public static readonly string BenefitsLandingTemplateID = "{22CEDD6C-BAF5-4C69-9555-DA0894D1490A}";
           
           
            public static readonly string BenefitsDetailTemplateID = "{7FF50234-7F34-4FC4-9CE4-58D286D95F9E}";
            public static readonly string BrandTitle = "BrandTitle";
            public static readonly string CommonFolderTemplateID = "{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}";
            public static readonly string BreakupTemplateID = "{F3E7F52C-CEDE-4267-87DE-C96776E5F8E9}";
            public static readonly string BreakUpHeading = "{D844B81F-BB70-4190-B552-7B4F0D3D1654}";
            public static readonly string BreakUpHeadingChildren = "{0C76133C-6BA6-44DD-8F64-F8DC3A9B37CF}";
            public static readonly string DisclaimerLabel = "DisclaimerLabel";
            public static readonly string BookFlightText = "BookFlightText";
            public static readonly string BookFlightLink = "BookFlightLink";
           }

        public static class ZeroCancellationConstant
        {
            public static readonly string DescriptionTemplateId = "{BA14113A-37F1-4583-BBA0-66035D73FBFC}";
            public static readonly string ZeroCancellationTemplateId = "{38490623-7FFE-40F3-ACCD-514BD12E015A}";
            public static readonly string ModelBoxTemplateId = "{06435B40-ACF1-46F4-A552-3168CB4F8F09}";
            public static readonly string WithoutZeroBreakupItemId = "{58170326-2218-47E0-ADB1-49FEB5F9FFED}";
            public static readonly string WithZeroBreakupItemId = "{43B79379-10DE-470F-9CAD-02A1BFC5F7FF}";
            public static readonly string PleaseNoteTemplateId = "{E5439730-FF15-49B5-A152-7F7763400193}";
            public static readonly string CancellationProcessTemplateId = "{20480792-F32E-4003-83F7-39F78B4D789F}";
            public static readonly string AmountLabel = "AmountLabel";
            public static readonly string ZeroCancellationHeading = "Heading";
            public static readonly string TnCLabel = "TnCLabel";
            public static readonly string Disclaimer = "Disclaimer";
            public static readonly string TnCLabelLink = "TnCLabelLink";
            public static readonly string HowToWork = "HowToWork";
            public static readonly string HowToWorkLink = "HowToWorkLink";
            public static readonly string AdditionalBenefit = "AdditionalBenefit";
            public static readonly string Code = "Code";
            public static readonly string Hint = "Hint";
            public static readonly string tnc = "tnc";
            public static readonly string tncLink = "tncLink";
            public static readonly string vendor = "vendor";
            public static readonly string vendorLink = "vendorLink";
            public static readonly string RefundLabel = "RefundLabel";
        }
    }
}