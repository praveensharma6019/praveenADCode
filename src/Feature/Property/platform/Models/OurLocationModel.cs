using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class OurLocationModel
    {
        public OurLocations ourLocations { get; set; }
    }

    public class OurLocationData
    {
        public string cityName { get; set; }
        public string about { get; set; }
        public string readMore { get; set; }
        public List<string> features { get; set; }
    }

    public class OurLocations
    {
        public string heading { get; set; }
        public List<OurLocationData> data { get; set; }
    }

}