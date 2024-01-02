using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class PackageServices
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "serviceDetails")]
        public List<PackageServicesList> serviceDetails { get; set; }
    }
    public class PackageServicesList
    {
        [JsonProperty(PropertyName = "addOnServiceId")]
        public string addOnServiceId { get; set; }

        [JsonProperty(PropertyName = "addOnServiceName")]
        public string addOnServiceName { get; set; }

        [JsonProperty(PropertyName = "addOnServiceDescription")]
        public string addOnServiceDescription { get; set; }

    }
}