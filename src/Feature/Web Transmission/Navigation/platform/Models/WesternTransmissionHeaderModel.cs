using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Models
{
    public class WesternTransmissionHeaderModel
    {
        public Sitecore.Data.Fields.MultilistField MenuList { get; set; }
        public string LogoImageSrc { get; set; }

        public string LogoImageAlt { get; set; }

        public List<MenuLink> MenuItem { get; set; }
    }

    public class MenuLink
    {
        

        public string Title { get; set; }

        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public List<subMenuLink> subMenuLinks { get; set; }

    }
    public class subMenuLink
    {
        public string ImageSrc { get; set; }

        public string ImageAlt { get; set; }

        public string Title { get; set; }

        public string LinkText { get; set; }
        public string LinkUrl { get; set; }

    }
}