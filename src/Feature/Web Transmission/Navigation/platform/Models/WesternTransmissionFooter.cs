using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Models
{
    public class WesternTransmissionFooter
    {
        public List<MainFooterLinks> mainFooterLinks { get; set; }
    }

    public class MainFooterLinks
    {
        public String LinkText { get; set; }
        public string LinkUrl { get; set; }

        public List<SubMenuFooterLink> subMenuFooterLinks { get; set; }
    }

    public class SubMenuFooterLink
    {
        public string ImageSrc { get; set; }

        public string ImageAlt { get; set; }

        public string Title { get; set; }

        public string LinkText { get; set; }
        public string LinkUrl { get; set; }

    }
}