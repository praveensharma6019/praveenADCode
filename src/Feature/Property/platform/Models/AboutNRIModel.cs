using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class AboutNRIModel
    {
        public AboutNri AboutNri { get; set; }
    }
    public class AboutNri
    {
        public string heading { get; set; }
        public string about { get; set; }
        public string readMore { get; set; }
        public string terms { get; set; }
        public string detailLink { get; set; }
        public string extrCharges { get; set; }
    }

}