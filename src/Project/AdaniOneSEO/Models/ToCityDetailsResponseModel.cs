namespace Project.AdaniOneSEO.Website.Models
{
    public class ToCityDetailsResponseModel
    {
        public MarketingAirline airlineDetails { get; set; }
        public DepartureAirport depatureAirportDetails { get; set; }
        public string departureTime { get; set; }
        public ArrivalAirport arrivalAirportDetails { get; set; }
        public string arrivalTime { get; set; }
        public string duration { get; set; }
        public decimal price { get; set; }
    }

}