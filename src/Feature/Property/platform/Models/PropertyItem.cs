using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class PropertyList
    {
        public string city { get; set; }
        public string headingLabel { get; set; }
        public string subheadingLabel { get; set; }
        [JsonProperty(PropertyName = "SeeAllLink")]
        public string SeeAllLink { get; set; }
        [JsonProperty(PropertyName = "SeeAllKeyword")]
        public string SeeAllKeyword { get; set; }
        public List<PropertyItem> property { get; set; }
    }
    public class PropertyItem
    {
        public string link { get; set; }
        public string linkTarget { get; set; }
        public string logo { get; set; }
        public string logotitle { get; set; }
        public string logoalt { get; set; }
        public string city { get; set; }
        public string src { get; set; }
        public string imgalt { get; set; }
        public string title { get; set; }
        public string imgtitle { get; set; }
        public string location { get; set; }
        public string subType { get; set; }
        public string imgtype { get; set; }
        public string status { get; set; }
        public string category { get; set; }
    }
}