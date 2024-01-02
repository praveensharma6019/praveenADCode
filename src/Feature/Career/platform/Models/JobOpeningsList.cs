using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Career.Platform.Services
{
    public class JobOpeningsList
    {
        public List<object> data { get; set; }
    }
    public class JobOpeningItem
    {
        public string role { get; set; }
        public string department { get; set; }
        public string location { get; set; }
        public string postingdate { get; set; }
        public string link { get; set; }

    }
    public class JobsAnchorsList
    {
        public List<object> data { get; set; }
    }
    public class JobsAnchorsItem
    {
        public string link { get; set; }
        public string title { get; set; }

    }
    public class EmployeeCareList
    {
        public List<object> data { get; set; }
    }
    public class EmployeeCareItem
    {
        public string name { get; set; }
        public string description { get; set; }
        public string inclusion { get; set; }


    }

    #region AboutCareer

    public class AboutCareerDataItem
    {
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [JsonProperty(PropertyName = "about")]
        public string About { get; set; }

        [JsonProperty(PropertyName = "readMore")]
        public string ReadMore { get; set; }

        [JsonProperty(PropertyName = "terms")]
        public string Terms { get; set; }

        [JsonProperty(PropertyName = "detailLink")]
        public string DetailLink { get; set; }

        [JsonProperty(PropertyName = "DetailLinkText")]
        public string DetailLinkText { get; set; }

        [JsonProperty(PropertyName = "extrCharges")]
        public string ExtrCharges { get; set; }
    }

    public class AboutCareerData
    {
        public AboutCareerDataItem aboutCareer { get; set; }
    }

    #endregion
}