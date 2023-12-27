using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Models
{
    public class FlightSearchAPIRequestModel
    {
        public List<PassengerDetails> passengers { get; set; }
        public ProcessingInfo processingInfo { get; set; }
        public TravelPreferences travelPreferences { get; set; }
        public List<OriginDestinationInformation> originDestinationInformation { get; set; }
    }

    public class PassengerDetails
    {
        public string type { get; set; }
        public int count { get; set; }
    }

    public class ProcessingInfo
    {
        public string countryCode { get; set; }
        public bool isSpecialFare { get; set; }
        public string sectorInd { get; set; }
        public string tripType { get; set; }
        public string fareType { get; set; }
    }

    public class TravelPreferences
    {
        public FarePref farePref { get; set; }
        public List<CabinPref> cabinPref { get; set; }
    }

    public class FarePref
    {
        public string fareDisplayCurrency { get; set; }
    }

    public class CabinPref
    {
        public string cabin { get; set; }
    }

    public class OriginDestinationInformation
    {
        public int rph { get; set; }
        public DepartureDateTime departureDateTime { get; set; }
        public OriginLocationDetails originLocation { get; set; }
        public DestinationLocationDetails destinationLocation { get; set; }
    }

    public class DepartureDateTime
    {
        public string windowAfter { get; set; }
        public string windowBefore { get; set; }
    }

    public class LocationDetails
    {
        public string radius { get; set; }
        public string countryCode { get; set; }
    }
    public class OriginLocationDetails : LocationDetails
    {
        public string location { get; set; }
        public string locationCode { get; set; }
    }
    public class DestinationLocationDetails : LocationDetails
    {
        public string location { get; set; }
        public string locationCode { get; set; }
    }
}