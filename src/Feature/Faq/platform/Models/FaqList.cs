using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Faq.Platform.Models
{
    public class LocationFaqList
    {
        public string FaqLink { get; set; }
        public List<LocationFaq> FaqItems { get; set; }
    }
    //public class LocationFaqItem
    //{ 
    //    public List<LocationFaq> FaqItems { get; set; }
    //}
    public class LocationFaq
    {
        public string Location { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
    public class FaqList
    {
        [JsonProperty(PropertyName = "FaqLink")]
        public string FaqLink { get; set; }
        [JsonProperty(PropertyName = "Data")]
        public List<FaqItem> FaqItems { get; set; }
    }
    public class FaqItem
    {
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Body")]
        public string Body { get; set; }
    }
}