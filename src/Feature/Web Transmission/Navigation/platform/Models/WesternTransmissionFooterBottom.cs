using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Models
{
    public class WesternTransmissionFooterBottom
    {
        public string Title { get; set; }
        public List<FooterLink> FooterLinks{get;set;}
    }

    public class FooterLink
    {
        public string Title { get; set; }
        public string LinkText { get; set; }

        public string LinkUrl { get; set; }
    }
}