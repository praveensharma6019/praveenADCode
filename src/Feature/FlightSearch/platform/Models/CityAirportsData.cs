using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models
{
    public class CityAirportsData
    {
        public List<CityDetails> ListCity { get; set; } 

    }
    public class CityDetails {
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string About { get; set; }
        public string CityImage { get; set; }
        public string BestTimetoVisitTitle { get; set; }
        public string BestTimetoVisitDetails { get; set; }
        public List<string> Keywords { get; set; }
        public bool IsDomestic { get; set; } = false;
        public AirportDetails ListAirports { get; set; }

    }

    public class AirportDetails
    {
        public string AirportName { get; set; }
        public string AirportCode { get; set; }
        public string City { get; set; }       
        public string Details { get; set; }
        public string AirportImage { get; set; }
        public string HealthandSaftyMeasure { get; set; }
        public string BaggageInformation { get; set; }
        public string AirportAddress { get; set; }       
        public string AirportID { get; set; }

        public List<TerminalDetails> TerminalList { get; set; }
    }

    public class TerminalDetails
    {
        public string TerminalName { get; set; }
        public string TerminalAddress { get; set; }
        public bool PranaamServiceAvailable { get; set; } = false;
    }

    public class Keyword
    {
        public string keywords { get; set; }
    }

    public class AppAirportsData
    {
        public List<AirportDetailsApp> ListAirportApp { get; set; }

    }

    public class AirportDetailsApp
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string AirportName { get; set; }
        public string AirportCode { get; set; }
        public bool IsPranaam { get; set; } = false;
        public bool IsPranaamMasterAirport { get; set; } = false;
        public bool isPopular { get; set; } = false;
        public string Priority { get; set; }
        public bool IsDomestic { get; set; } = false;
        public string AirportID { get; set; }
        //public bool IsMobileView { get; set; } = false;
        public List<string> Keywords { get; set; }
        public string AirportType { get; set; }
        public bool IsMasterAirport { get; set; } = false;


    }
}