using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Models
{
    public class CheapestFlightCalenderModel
    {
        public List<CheapestFlightData> FlightCalender { get; set; }
    }
    public class CheapestFlightData
    {
        public decimal Price { get; set; }
        public string Date { get; set; }
    }

}