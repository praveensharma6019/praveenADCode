using System;
using System.Collections.Generic;
using System.Web.Helpers;

namespace Project.AdaniOneSEO.Website.Models.FlightsToDestination
{
    public class FilterOptionsModel
    {
        public virtual string TripType { get; set; }
        public virtual string FareType { get; set; }
        public virtual string Cabin { get; set; }
        public virtual string CityFrom { get; set; }
        public virtual string CityFromCode { get; set; }
        public virtual string CityToCode { get; set; }
        public virtual string CityTo { get; set; }
        public virtual int AddDaysToDate { get; set; }
        public virtual string Date {  get; set; }
        public virtual string CurrentDate { get; set; }

        public IEnumerable<FlightDetailResponseModel> responseModel { get; set; }
    }

    
}