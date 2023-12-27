using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models
{
    public class AirportModel
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string AirportName { get; set; }
        public string AirportCode { get; set; }
        public bool IsPranaam { get; set; } = false;
        public bool isPopular { get; set; } = false;
        public string Priority { get; set; }
        public List<string> Keywords { get; set; }
    }
}