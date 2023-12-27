using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam
{
    public class Constants
    {
        /// <summary>
        /// Service Type Id's
        /// </summary>
        public static readonly string DepartureId = "1";
        public static readonly string TransitId = "2";
        public static readonly string RoundTripId = "3";
        public static readonly string ArrivalId = "4";

        /// <summary>
        /// Travel Sector Id's
        /// </summary>
        public static readonly string DomesticToInternationalId = "1";
        public static readonly string InternationalToInternationalId = "2";
        public static readonly string DomesticId = "3";
        public static readonly string InternationalId = "4";
        public static readonly string InternationalToDomesticId = "4";
        public static readonly string DomesticToDomesticId = "4";

        /// <summary>
        /// Service Type Name's
        /// </summary>
        public static readonly string Departure = "Departure";
        public static readonly string Transit = "Transit";
        public static readonly string RoundTrip = "Round Trip";
        public static readonly string Arrival = "Arrival";

        /// <summary>
        /// Travel Sector Name's
        /// </summary>
        public static readonly string DomesticToInternational = "Domestic to International";
        public static readonly string InternationalToInternational = "International to International";
        public static readonly string Domestic = "Domestic";
        public static readonly string International = "International";
        public static readonly string InternationalToDomestic = "International to Domestic";
        public static readonly string DomesticToDomestic = "Domestic to Domestic";

        /// <summary>
        /// City Codes
        /// </summary>
        public static readonly string Jaipur = "JAI";
        public static readonly string Guwahati = "GAU";
        public static readonly string Lucknow = "LKO";
        public static readonly string Mangalore = "IXE";
        public static readonly string Ahmedabad = "AMD";
        public static readonly string Trivandrum = "TRV";

        /// <summary>
        /// Template ID's
        /// </summary>
        public static readonly ID PranaamAirportDS = new ID("{0E6512D2-0E9E-4B2E-9659-58B6A7C7850A}");
        public static readonly ID AirportTemplateID = new ID("{486FBEBB-93F6-4E9D-859A-357010D47503}");
        public static readonly ID PranaamPackageCard = new ID("{85BD7A06-FE9B-4D02-86A8-3D07A04BE328}");
        public static readonly ID PranaamPackagesFolder = new ID("{BCBED7B1-F88D-46D9-9017-562A7ACDA95B}");
        public static readonly ID PranaamPackages = new ID("{A0323420-ABED-4BEC-A1FC-D91BC226611F}");
        public static readonly ID SrpPagePackages = new ID("{A96F7942-08FF-4A11-98C6-018FC459FC8A}");
        public static readonly ID PackageServicesParent = new ID("{B6923FD6-4DDD-48BE-BBC7-2D4F3FE84F9B}");
        public static readonly ID PackageServicesTemplate = new ID("{91D06145-8F8E-4DA8-B8C8-6D011B9D0234}");
        public static readonly ID PackageAddOnsParent = new ID("{EDB54F2C-602A-47EF-AB00-962317BC403A}");
        public static readonly ID PackageAddOnsTemplate = new ID("{8A4342FE-6910-4DF6-9794-6E1606A621B4}");


        /// <summary>
        /// City Datasource
        /// </summary>
        public static readonly ID AMDArrivalDomestic = new ID("{DD845116-502D-43FD-A573-43E886316997}");
        public static readonly ID AMDArrivalInternational = new ID("{40BCEE76-ACB3-40F3-B881-2AD055AC37D3}");
        public static readonly ID AMDDepartureDomestic = new ID("{E3016154-F2A9-4AD3-A886-219F39F8A8A3}");
        public static readonly ID AMDDepartureInternational = new ID("{A452F750-651C-4735-AE39-8C07A53E49DC}");
        public static readonly ID AMDTransitDI = new ID("{1360CCC9-F395-4CDE-8A9F-BAAA9E1FD775}");
        public static readonly ID AMDTransitDD = new ID("{4F412D8B-B38E-4802-BF50-29694D84E858}");
        public static readonly ID AMDTransitII = new ID("{3F6C5425-797C-43C6-8BBB-A92ED8781C29}");
        public static readonly ID AMDTransitID = new ID("{038B6BDF-0F33-4195-90B1-368B10275140}");
        public static readonly ID AMDRoundTripDomestic = new ID("{042FF485-F84F-4504-AF96-17B54D2EBE13}");
        public static readonly ID AMDRoundTripInternational = new ID("{E360A52D-22C9-4705-88CF-07841292D1F6}");

        public static readonly ID LKOArrivalDomestic = new ID("{A4207D21-EFD3-41E3-BD50-164F9EB0B682}");
        public static readonly ID LKOArrivalInternational = new ID("{A5279945-77B3-4AEE-8F97-2227E44EED5E}");
        public static readonly ID LKODepartureDomestic = new ID("{AA3F8167-B8B0-45E8-816C-BB37D1791ACD}");
        public static readonly ID LKODepartureInternational = new ID("{B40CFAB7-0146-4355-A104-C393F04E4051}");
        public static readonly ID LKOTransitDI = new ID("{F8633722-DA17-427D-A8BC-0111AA75E583}");
        public static readonly ID LKOTransitDD = new ID("{F02C8F09-AA75-487B-BADF-05B727F2EC9B}");
        public static readonly ID LKOTransitII = new ID("{B5E58654-0868-4CCD-BC34-CA961CE388BA}");
        public static readonly ID LKOTransitID = new ID("{F0BE5821-61FC-4CA0-B8EF-353E13710F19}");
        public static readonly ID LKORoundTripDomestic = new ID("{D90FBD12-B9AC-44E4-937A-1FB7BDF6B891}");
        public static readonly ID LKORoundTripInternational = new ID("{409CB0E5-2FB3-4F98-8A66-7DE3C7D40C7C}");

        public static readonly ID GAUArrivalDomestic = new ID("{307FA398-0D86-4CB8-8D7E-ABB534413F92}");
        public static readonly ID GAUArrivalInternational = new ID("{E52D6C98-C052-44CF-A46C-EBD378239289}");
        public static readonly ID GAUDepartureDomestic = new ID("{C05E1C4A-F596-43CD-A960-03E32AF58E5A}");
        public static readonly ID GAUDepartureInternational = new ID("{28B8134D-311B-4DAF-B348-45159C5817CC}");
        public static readonly ID GAUTransitDI = new ID("{A8C205D0-9269-4C84-8683-1C62C9E433E7}");
        public static readonly ID GAUTransitDD = new ID("{F7C87282-964D-4CDB-9A5F-34589276392B}");
        public static readonly ID GAUTransitII = new ID("{CE75AD63-477D-4DF5-97F6-64D45239BA2E}");
        public static readonly ID GAUTransitID = new ID("{D184D882-3539-4E02-81FA-E473C2D964E2}");
        public static readonly ID GAURoundTripDomestic = new ID("{C7DEF3E0-0FA7-43AC-AA0C-E229BDC0682E}");
        public static readonly ID GAURoundTripInternational = new ID("{F5C9A7FD-44F5-4F96-9C7C-DE21C92A2B64}");

        public static readonly ID JAIArrivalDomestic = new ID("{F8EEAC3A-2268-483E-8650-BA2BE85A94A2}");
        public static readonly ID JAIArrivalInternational = new ID("{817FE6FD-CBF9-4DA1-B0FA-F1DCA4E6A4A0}");
        public static readonly ID JAIDepartureDomestic = new ID("{B316083C-9721-4F5E-B135-698E4FB46B9D}");
        public static readonly ID JAIDepartureInternational = new ID("{A3CA281D-BE29-4AE9-8FEF-7F8D815E8308}");
        public static readonly ID JAITransitDI = new ID("{A92E4705-EA6E-4E41-821F-60E6EB78B4A5}");
        public static readonly ID JAITransitDD = new ID("{888C111E-05D3-4749-A2E9-DAB74CC1EC39}");
        public static readonly ID JAITransitII = new ID("{9C36E7D5-BAAD-4F96-93A3-A963F29C670C}");
        public static readonly ID JAITransitID = new ID("{69EE41F7-F708-4A33-BAC7-D4DEAD9E83E7}");
        public static readonly ID JAIRoundTripDomestic = new ID("{A169F2BE-9E06-4229-9A2B-A0E63D8CFEE6}");
        public static readonly ID JAIRoundTripInternational = new ID("{3FEC8375-260E-4B64-BB83-BC50F3A44951}");

        public static readonly ID IXEArrivalDomestic = new ID("{EFEE7DF7-7BB2-440E-9B9B-4EA010886D2F}");
        public static readonly ID IXEArrivalInternational = new ID("{8F5209F3-109D-4183-81C9-B20FA249827A}");
        public static readonly ID IXEDepartureDomestic = new ID("{94205B0D-BAAB-468F-B20D-1068DF63FA44}");
        public static readonly ID IXEDepartureInternational = new ID("{52569E68-18B0-4965-9FCE-4D5B2D6AE047}");
        public static readonly ID IXETransitDI = new ID("{8C6C4CC1-FF23-4F00-988D-79D1E11E122F}");
        public static readonly ID IXETransitDD = new ID("{F3641AAE-964E-4C11-936D-A14B89527B2F}");
        public static readonly ID IXETransitII = new ID("{67834781-74CF-43AA-A85B-112C638F300C}");
        public static readonly ID IXETransitID = new ID("{A88DB5D1-3BC1-45F1-8B9B-DB33A39F3A32}");
        public static readonly ID IXERoundTripDomestic = new ID("{3889428F-2948-42F9-B3BE-40F8EB17E9C5}");
        public static readonly ID IXERoundTripInternational = new ID("{7332BC0D-1FF1-4BC7-9787-0AA115C03884}");

        public static readonly ID TRVArrivalDomestic = new ID("{0DF1F87B-C325-4F3A-BD88-CC015E1EC282}");
        public static readonly ID TRVArrivalInternational = new ID("{9489DDDD-41D3-4B01-AB8F-1A4C44106C06}");
        public static readonly ID TRVDepartureDomestic = new ID("{D2D03003-49AA-4EEC-8E15-4029BE01D4F8}");
        public static readonly ID TRVDepartureInternational = new ID("{24CAF8E1-60BC-4CD7-BCE7-8CB39693FDBA}");
        public static readonly ID TRVTransitDI = new ID("{2F77E99A-BBEF-464E-A63B-AEC504BE3DFE}");
        public static readonly ID TRVTransitDD = new ID("{D0489023-003C-408D-B343-697CB65030AB}");
        public static readonly ID TRVTransitII = new ID("{A6E78464-199C-4052-9C6A-0CFE90195BF9}");
        public static readonly ID TRVTransitID = new ID("{806428E0-9E2A-4386-877B-9E2F2A5D8925}");
        public static readonly ID TRVRoundTripDomestic = new ID("{2D07F778-FD45-4033-A832-7EED72C2B2FC}");
        public static readonly ID TRVRoundTripInternational = new ID("{B56F0C77-1AAE-4A84-9338-BC8D6BC9395F}");

        /// <summary>
        /// Service Type Name's
        /// </summary>
        public static readonly ID ArrivalTemplateID = new ID("{4A39CBDE-6C37-4DB7-BBE2-45542791AF87}");
        public static readonly ID DepartureTemplateID = new ID("{B2872D8F-6D0B-41A5-BDD6-C1AF64A7F315}");
        public static readonly ID TransitTemplateID = new ID("{54902155-3CAD-479B-B3B7-947A51B551F4}");
        public static readonly ID RoundtripTemplateID = new ID("{A1CB246D-E97A-495A-97B7-D42D100164EC}");

        public static readonly string AirportCode = "AirportCode";
        public static readonly string AirportName = "AirportName";
        public static readonly string ServiceName = "Name";
        public static readonly string ServiceId = "Id";
        public static readonly string IsActive = "IsActive";
        public static readonly string TravelSectorName = "Name";
        public static readonly string TravelSectorId = "TravelSectorId";
    }
}