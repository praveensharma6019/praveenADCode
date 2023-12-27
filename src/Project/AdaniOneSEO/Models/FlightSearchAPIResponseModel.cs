using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Models
{
    public class FlightSearchAPIResponseModel
    {
        public ResponseData data { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string  code { get; set; }
        public string  warning { get; set; }
        public ErrorDetails error { get; set; }
    }

    public class ResponseData
    {
        public List<object> splrt { get; set; }
        public List<PricedItineraries> pricedItineraries { get; set; }
        public List<object> specialFares { get; set; }
        public string searchFlightID { get; set; }
        public string currencyCode { get; set; }
    }

    public class PricedItineraries
    {
        public AirItinerary airItinerary { get; set; }
        public AirItineraryPricingInfo airItineraryPricingInfo { get; set; }
        public string sequenceNumber { get; set; }
        public string directionInd { get; set; }
        public string sectorInd { get; set; }
        public SecurityKey securityKey { get; set; }
        public int groupIndex { get; set; }
        public int rowIndex { get; set; }
    }

    public class AirItinerary
    {
        public List<OriginDestinationOption> originDestinationOption { get; set; }
        public string journeyKey { get; set; }
        public FareGroupInformation fareGroupInformation { get; set; }
    }

    public class OriginDestinationOption
    {
        public List<FlightSegment> flightSegment { get; set; }
        public string directionInd { get; set; }
        public string group { get; set; }
        public string rph { get; set; }
        public TechnicalStops technicalStops { get; set; }
        public string fareKey { get; set; }
        public bool fareRefundable { get; set; }
        public bool journeyCancelable { get; set; }
    }

    public class FlightSegment
    {
        public Cabin cabin { get; set; }
        public string status { get; set; }
        public string departureDay { get; set; }
        public string duration { get; set; }
        public string dayChange { get; set; }
        public string seat { get; set; }
        public DepartureAirport departureAirport { get; set; }
        public ArrivalAirport arrivalAirport { get; set; }
        public MarketingAirline marketingAirline { get; set; }
        public OperatingAirline operatingAirline { get; set; }
        public List<object> equipment { get; set; }
        public string remarks { get; set; }
        public string flightNumber { get; set; }
        public string key { get; set; }
        public string departureDate { get; set; }
        public string departureTime { get; set; }
        public string departureTimeZone { get; set; }
        public string departureTimeEpoch { get; set; }
        public string arrivalDate { get; set; }
        public string arrivalTime { get; set; }
        public string arrivalTimeZone { get; set; }
        public string arrivalTimeEpoch { get; set; }
        public string resBookDesigCode { get; set; }
        public string rph { get; set; }
        public string flightID { get; set; }
        public string supplier { get; set; }
        public string fareBasisCode { get; set; }
        public string validatingCarrier { get; set; }
        public string departureTimeZoneRegion { get; set; }
        public string arrivalTimeZoneRegion { get; set; }
        public string cabinBaggage { get; set; }
        public string chekinBaggage { get; set; }
        public string baggageInfoNote { get; set; }
        public bool isTerminalChange { get; set; }
        public bool isAirportChange { get; set; }
        public string transitVisaMessage { get; set; }
    }

    public class Cabin
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class DepartureAirport
    {
        public string locationCode { get; set; }
        public string terminal { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
    }
    public class ArrivalAirport
    {
        public string locationCode { get; set; }
        public string terminal { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
    }
    public class MarketingAirline
    {
        public string code { get; set; }
        public string flightNumber { get; set; }
        public string name { get; set; }
    }
    public class OperatingAirline
    {
        public string code { get; set; }
        public string flightNumber { get; set; }
        public string name { get; set; }
    }

    public class TechnicalStops
    {
        public List<StopDetail> stopDetail { get; set; }
        public int noOfStops { get; set; }
    }

    public class StopDetail
    {
        public string name { get; set; }
        public int duration { get; set; }
        public int layoverType { get; set; }
    }

    public class FareGroupInformation
    {
        public string fareGroup { get; set; }
        public string fareGroupName { get; set; }
        public string productClass { get; set; }
        public string fareGroupDisplayName { get; set; }
        public string comboFareBasisCode { get; set; }
    }

    public class AirItineraryPricingInfo 
    {
        public TotalFare totalFare { get; set; }
        public TotalBaseFare totalBaseFare { get; set; }
        public TotalTax totalTax { get; set; }
        public TotalFee totalFee { get; set; }
        public string totalMarkup { get; set; }
        public Discounts discounts { get; set; }
        public string remark { get; set; }
        public string travelInsuranceCharges { get; set; }
        public string cancellationFeeCharges { get; set; }
        public List<PassengerTypeQuantity> passengerTypeQuantity { get; set; }
    }
    
    public class TotalFare
    {
        public decimal amount { get; set; }
        public decimal strikedOffPrice { get; set; }
    }

    public class TotalBaseFare
    {
        public decimal amount { get; set; }
    }
    public class TotalTax
    {
        public decimal amount { get; set; }
    }
    public class TotalFee
    {
        public decimal amount { get; set; }
    }
    public class Discounts
    {
        public decimal amount { get; set; }
    }

    public class PassengerTypeQuantity
    {
        public BaseFare baseFare { get; set; }
        public Taxes taxes { get; set; }
        public TotalFlightFare totalFare { get; set; }
        public Fees fees { get; set; }
        public string markups { get; set; }
        public string key { get; set; }
        public string code { get; set; }
        public int quantity { get; set; }
    }

    public class BaseFare
    {
        public decimal amount { get; set; }
        public decimal perPaxAmount { get; set; }
    }

    public class Taxes
    {
        public List<Tax> tax { get; set; }
        public decimal amount { get; set; }
        public decimal perPaxAmount { get; set; }
    }

    public class Tax
    {
        public decimal amount { get; set; }
        public string taxCode { get; set; }
        public string taxDesc { get; set; }
        public decimal perPaxAmount { get; set; }
    }

    public class TotalFlightFare
    {
        public decimal amount { get; set; }
        public decimal perPaxAmount { get; set; }
        public decimal strikedOffPrice { get; set; }
    }

    public class Fees 
    {
        public decimal amount { get; set; }
        public decimal perPaxAmount { get; set; }
        public decimal discountedAmount { get; set; }
        public decimal disCountedPerPaxAmount { get; set; }
        public List<string> fee { get; set; }
    }

    public class SecurityKey 
    {
        public string securityGUID { get; set; }
        public string key { get; set; }
    }
}