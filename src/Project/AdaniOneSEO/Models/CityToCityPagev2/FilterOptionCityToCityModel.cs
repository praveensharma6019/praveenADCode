using System;
using System.Collections.Generic;
using System.Web.Helpers;

namespace Project.AdaniOneSEO.Website.Models.FlightsToDestination
{
    public class FilterOptionCityToCityModel
    {
        public string TripType { get; set; }
        public string FareType { get; set; }
        public string Cabin { get; set; }
        public string CityFrom { get; set; }
        public string CityFromCode { get; set; }
        public string CityToCode { get; set; }
        public string CityTo { get; set; }
        public int AddDaysToDate { get; set; }
        public string Date { get; set; }
        public string CurrentDate { get; set; }

        public IEnumerable<FlightDetailResponseModel> responseModel { get; set; }
    }


}