using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Leaders.Platform.Models
{

    public class LeadersDataItem
    {
        [JsonProperty(PropertyName = "quote")]
        public string quoteText { get; set; }

        [JsonProperty(PropertyName = "src")]
        public string imageSrc { get; set; }

        [JsonProperty(PropertyName = "imgalt")]
        public string imgAlt { get; set; }

        [JsonProperty(PropertyName = "imgtitle")]
        public string imgTitle { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "designation")]
        public string Designation { get; set; }       
    }     
    public class LeadersData
    {
        public string Title { get; set; }
        public List<Object> data { get; set; }
    }

    public class AchievementsDataItem
    {
        [JsonProperty(PropertyName = "icon")]
        public string icon { get; set; }
        [JsonProperty(PropertyName = "iconsrc")]
        public string iconsrc { get; set; }

        [JsonProperty(PropertyName = "count")]
        public string count { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public string Descriptions { get; set; }

        [JsonProperty(PropertyName = "start")]
        public string Start { get; set; }

        [JsonProperty(PropertyName = "delay")]
        public string Delay { get; set; }

        [JsonProperty(PropertyName = "src")]
        public string imageSrc { get; set; }

        [JsonProperty(PropertyName = "imgalt")]
        public string imgAlt { get; set; }

        [JsonProperty(PropertyName = "imgtitle")]
        public string imgTitle { get; set; }

        
       
    }
    public class AchievementsData
    {
        public List<Object> data { get; set; }
    }

}