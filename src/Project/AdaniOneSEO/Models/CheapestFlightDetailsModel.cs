using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Models
{
    public class CheapestFlightDetailsModel
    {
        public FareCalender data { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public string warning { get; set; }
        public ErrorDetails error { get; set; }
    }
    public class FareCalender
    {
        public List<CheapestFlightDetails> fareCalendars { get; set; }
    }

    public class CheapestFlightDetails
    {
        public string date { get; set; }
        public List<FlightDetails> prices { get; set; }
    }

    public class FlightDetails
    {
        public string from { get; set; }
        public string to { get; set; }
        public decimal amount { get; set; }
    }
    public class ErrorDetails
    {
        public int statusCode { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string source { get; set; }
    }
}