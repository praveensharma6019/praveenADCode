using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models
{
    public class LowestFairAPIResponse
    {
        public Data data { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public object warning { get; set; }
        public object error { get; set; }
    }

    public class Data
    {
        public List<FareCalendar> fareCalendars { get; set; }
    }

    public class FareCalendar
    {
        public string date { get; set; }
        public List<Price> prices { get; set; }
    }

    public class Price
    {
        public string from { get; set; }
        public string to { get; set; }
        public float amount { get; set; }
    }

}