using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.Models.FlightsToDestination
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class AirlineDetails
    {
        public string code { get; set; }
        public string flightNumber { get; set; }
        public string name { get; set; }
    }

    public class ArrivalAirportDetails
    {
        public string locationCode { get; set; }
        public string terminal { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
    }

    public class DepatureAirportDetails
    {

        public string locationCode { get; set; }
        public string terminal { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
    }

    public class FlightDetailResponseModel
    {
        
        public AirlineDetails airlineDetails { get; set; }
        public DepatureAirportDetails depatureAirportDetails { get; set; }
        public string departureTime { get; set; }
        public ArrivalAirportDetails arrivalAirportDetails { get; set; }
        public string arrivalTime { get; set; }
        public string duration { get; set; }
        public string minutes { get; set; }
        public string price { get; set; }
        public string date { get; set; }
        public string error { get; set; }
    }


}