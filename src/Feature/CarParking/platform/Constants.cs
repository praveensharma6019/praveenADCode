using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CarParking.Platform
{
    public class VendorConstant
    {
        public static readonly Guid CabVendorsFolderID = new Guid("{648A83D8-4409-4954-9C81-56677BAB0B8A}");
        public static readonly Guid VendorTemplateID = new Guid("{5A6D6AF1-290E-4D4F-BD78-32CA9B9DB527}");
        public static readonly Guid InfoTemplateID = new Guid("{407FC9F0-CC99-475B-8F51-541094377FBB}");

        public static readonly Guid CabVendorInfoFolderID = new Guid("{EA21F011-98F6-48B0-8CA8-B0BD7EE72667}");
        public static readonly Guid VendorInfoTemplateID = new Guid("{1DFD515E-2FA2-4A51-8810-E839EE2D0E77}");

        public static readonly Guid AirportTemplateID = new Guid("{486FBEBB-93F6-4E9D-859A-357010D47503}");
        public static readonly Guid TerminalTemplateID = new Guid("{91D1448B-3F86-47D6-B786-4177E2EB4F85}");

        public static readonly ID CityTemplateID = new ID("{D71EDD08-B897-45BB-A1FD-F90666A7F0CE}");
        public static readonly ID KeywordsFolderTemplateID = new ID("{8F4F2A26-49C5-4E2F-9AFB-A4652C015BA2}");
        public static readonly ID KeywordTemplateID = new ID("{64E55D8A-5A56-40EB-9672-4A7F7384D343}");

        public static readonly ID ContactDetailsTemplateID = new ID("{0A4DB799-BFB9-4BFD-86B8-B6230D78E252}");

        public static readonly ID CabImportantInformationTemplateID = new ID("{4E51F498-BEFE-4FFC-ACC2-DBF3A59A2A71}");
        public static readonly ID CabDetailedImportantInformationTemplateID = new ID("{755853D2-81C4-4E24-BA8C-2E87056470EB}");
        public static readonly ID CabCancellationPolicyTemplateID = new ID("{97A04658-DDD4-4F97-B33C-A611EB0C6413}");
        public static readonly ID CabPreBookingTemplateID = new ID("{5F7EAE24-8AF5-4F17-8F3E-22CFA58028FC}");
        public static readonly ID CabPostBookingTemplateID = new ID("{B942ABA7-5B5A-44AB-9CB6-61ED24B6044B}");
        public static readonly ID TitleWithRichText = new ID("{0A4DB799-BFB9-4BFD-86B8-B6230D78E252}");
        public static readonly ID CabCancellationTemplateID = new ID("{DD81037F-3FBE-497B-A3EE-EFA28D6B37C9}");
        public static readonly Sitecore.Data.ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
        public static readonly ID NameValueTemplateID = new ID("{407FC9F0-CC99-475B-8F51-541094377FBB}");
        public static readonly ID NameValueStoretypeTemplateID = new ID("{0FC1E4B0-5D12-4056-B342-DDFC76A0BCD0}");
        public static readonly Sitecore.Data.ID Value = new ID("{C89D7769-7095-49EA-ABB1-47AD56E32802}");
        public static readonly ID TitleWithLinkTemplateID = new ID("{940150CA-75BC-4D94-9DAB-1963AA1491E7}");

        public static readonly Sitecore.Data.ID LinkText = new ID("{186367E6-CC9E-42A5-981F-A3EECBD3F263}");
        public static readonly Sitecore.Data.ID LinkURL = new ID("{FF14F53D-FFC6-4E18-A890-87543CE8278D}");
        public static readonly Sitecore.Data.ID LinkTitle = new ID("{186367E6-CC9E-42A5-981F-A3EECBD3F263}");
        public static readonly Sitecore.Data.ID LinkAutoId = new ID("{C0679279-72D8-439B-82D5-C1ABAC4B1DDC}");
        public static readonly Sitecore.Data.ID LinkDesc = new ID("{B05AFA1B-8C6B-460D-AC64-66617458F331}");
        public static readonly Sitecore.Data.ID LinkImage = new ID("{27377AEE-BADF-4905-8F1B-23A4CA595D27}");
        public static readonly Sitecore.Data.ID LinkUniqueId = new ID("{AB5708DB-2667-4E1B-9B0D-74CDE820BFED}");
        public static readonly Sitecore.Data.ID Name = new ID("{7A4ADE39-7CD4-4713-A53A-8F55A7BDD173}");

        public static readonly string InfoName = "name";
        public static readonly string InfoValue = "value";
        public static readonly string ItemTitle = "Title";
        public static readonly string Code = "Code";
        public static readonly string Description = "Description";
        public static readonly string Image = "Image";
        public static readonly string MobileImage = "MobileImage";
        public static readonly string VendorLogo = "VendorLogo";
        public static readonly string SubTitle = "SubTitle";
        public static readonly string Airport = "Airport";
        public static readonly string AirportName = "AirportName";
        public static readonly string AirportCode = "AirportCode";
        public static readonly string City = "City";
        public static readonly string TerminalName = "TerminalName";
        public static readonly string TerminalCode = "TerminalCode";
        public static readonly string Cabs = "Cabs";
        public static readonly string Vendor = "Vendor";
        public static readonly string Terminal = "Terminal";
        public static readonly string Gate = "Gate";
        public static readonly string CabLocation = "Cab Location";
        public static readonly string RideNowText = "Ride Now Text";
        public static readonly string RideLaterText = "Ride Later Text";
        public static readonly string StepstoBoard = "Steps to Board";
        public static readonly string CancellationCTA = "Cancellation CTA";
        public static readonly string CancellationCTAWeb = "Cancellation CTAWeb";
        public static readonly string ShareMessage = "Share Message";
        public static readonly string PoliceHelpline = "Police Helpline";
        public static readonly string ScheduleCabDetailShareTime = "Cab Detail Share Time";
        public static readonly string TrackingPrecautionMessage = "Precaution Message";
        public static readonly string TripType = "TripType";
        public static readonly string CabSchedule = "CabSchedule";
        public static readonly string CabBookingType = "CabBookingType";
        public static readonly string IsAdaniAirport = "IsAdaniAirport";
        public static readonly string StepsToBoardDetails = "StepsToBoardDetails";

        public static readonly ID CancellationTitle = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
        public static readonly ID CancellationDescription = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
        public static readonly ID CancellationCTALink = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
        public static readonly ID ReasonsTitle = new ID("{A8F96E5C-2B00-4FD2-B7F5-4BC4DC4FDF9B}");
        public static readonly string ReasonsList = "Reasons";
        public static readonly ID Reason = new ID("{7A7F5B3F-F715-4056-98D1-B542125906C8}");
        public static readonly ID HintText = new ID("{32B9CC41-C9A9-46BC-B0F8-81335DA1CA70}");
        public static readonly ID DescriptionLength = new ID("{74F61047-22EC-44A4-8E6C-64F5EE670C99}");
        public static readonly ID DictionaryValue = new ID("{403524F3-7AA3-46BA-A322-DE4C8627CDDB}");
    }
}