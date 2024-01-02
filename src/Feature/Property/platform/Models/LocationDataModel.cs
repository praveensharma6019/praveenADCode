using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class LocationDataModel
    {
        public LocationData locationData { get; set; }
    }


    public class FacilitiesDatum
    {
        public string label { get; set; }
        public string icon { get; set; }
    }

    public class LocationData
    {
        public string heading { get; set; }
        public string componentID { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string contactUsLabel { get; set; }
        [JsonProperty(PropertyName = "mapUrl")]
        public string MapIframeUrl { get; set; }
        [JsonProperty(PropertyName = "imgLogo")]
        public string ImageLogo { get; set; }
        [JsonProperty(PropertyName = "projectLocation")]
        public ProjectLocation ProjectLocation { get; set; }
        [JsonProperty(PropertyName = "markerTitle")]
        public string MarkerTitle { get; set; }
        public List<FacilitiesDatum> facilitiesData { get; set; }

    }

    public class ProjectLocation
    {
        public string contactUsLabel { get; set; }

        [JsonProperty(PropertyName = "address1")]
        public string Address1 { get; set; }
        [JsonProperty(PropertyName = "address2")]
        public string Address2 { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "pincode")]
        public string Pincode { get; set; }
        [JsonProperty(PropertyName = "contact")]
        public string Contact { get; set; }
    }


}