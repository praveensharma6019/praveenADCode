using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FAQ.Models
{
    [JsonObject]
    public class Filters
    {
        [JsonProperty(PropertyName = "airportcode")]
        public string airportCode { get; set; }

        [JsonProperty(PropertyName = "servicetype")]
        public string serviceType { get; set; }
    }
}