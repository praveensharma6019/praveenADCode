using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class WhyInvestModel
    {
        public WhyInvest whyInvest { get; set; }
    }
    public class Benefit
    {
        public string icon { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
    }


    public class WhyInvest
    {
        public string heading { get; set; }
        public string about { get; set; }
        public string readMore { get; set; }
        public List<Benefit> benefits { get; set; }
    }
}