using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Models
{
    public class JobDetails
    {
        [JsonProperty(PropertyName = "jobdeatils")]
        public ObjectItems JobList { get; set; }
    }

    public class ObjectItems
    {
        [JsonProperty(PropertyName = "data")]
        public List<object> JobDetails { get; set; }
    }
}