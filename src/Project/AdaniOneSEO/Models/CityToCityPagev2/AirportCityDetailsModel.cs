using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Models.CityToCityPagev2
{
    public class AirportCityDetailsModel
    {
        public string CityName { get; set; }
        public string CityImage { get; set; }
        public string AirportName { get; set; }
        public string AirportDescription { get; set; }
        public string AboutAirportHeading { get; set; }
        public string AboutAirportDescription { get; set; }
        public string PlacesToVisitHeading { get; set; }

        public List<PlacesToVisit> PlacesToVisitInCity { get; set; }

        public string BestTimeToVisitHeading { get; set; }
        public string BestTimeToVisitDescription { get; set; }
        public string AirportCityCode { get; set; }
        public string AirportCityAdddress { get; set; }
        public string CityType { get; set; }
    }

    public class PlacesToVisit
    {
        public string PlaceName { get;set; }
        public string PlaceLink { get;set; }
        public string LocationIcon { get;set; }
    }
}