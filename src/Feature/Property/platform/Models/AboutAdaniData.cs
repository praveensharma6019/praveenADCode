using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class AdaniData
    {
        public string componentID { get; set; }
        public AboutData aboutData { get; set; }
        public string readMore { get; set; }
        public string readLess { get; set; }
        public string description { get; set; }

    }

    public class AboutData
    {
        public string heading { get; set; }
        public string desc { get; set; }
        public string readMore { get; set; }
        public string terms { get; set; }
    }

    public class AboutAdaniData
    {
        public AdaniData aboutAdaniData { get; set; }
    }
}