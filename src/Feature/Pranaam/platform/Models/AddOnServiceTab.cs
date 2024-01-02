using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class AddOnServiceTab : AddOnService
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "heroContent")]
        public HeroContent heroContent { get; set; }

        [JsonProperty(PropertyName = "tabContent")]
        public List<Dictionary<string, TabData>> tabContent { get; set; }
    }
    public class HeroContent
    {
        [JsonProperty(PropertyName = "src")]
        public string src { get; set; }

        [JsonProperty(PropertyName = "alt")]
        public string alt { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string text { get; set; }
    } 
    public class TabData
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<Service> data { get; set; }
    }
}