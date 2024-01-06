using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.JaipurAirport.Website.Model
{
    public class Contact_Us_CRM_Response
    {
        [JsonProperty("Case Number")]
        public string CaseNumber { get; set; }
        public string Message { get; set; }
    }
}