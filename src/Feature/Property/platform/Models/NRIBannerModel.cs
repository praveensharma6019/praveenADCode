using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class NRIBannerModel
    {
        public NriBanner nriBanner { get; set; }
    }

    public class NriBanner
    {
        public string src { get; set; }
        public string alt { get; set; }
        public string title { get; set; }
        public string class1 { get; set; }
        public string class2 { get; set; }
    }
}