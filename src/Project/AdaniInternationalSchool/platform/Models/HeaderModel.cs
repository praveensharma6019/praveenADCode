using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class HeaderModel
    {
        public HeaderModel() {
            Navigation = new List<NavigationModel>();
            Social = new List<LinkItemModel>();
            Contact= new List<HeaderContactModel>();
        }

        public string LogoSrc { get; set; }
        public string LogoSrcSmall { get; set; }
        public string LogoSrcHamburger { get; set; }
        public string HamburgerBG { get; set; }
        public string LogoAlt { get; set; }
        public string Url { get; set; }

        public GtmDataModel GtmData { get; set; }
        public List<NavigationModel> Navigation { get; set; }
        public List<LinkItemModel> Social { get; set; }
        public List<HeaderContactModel> Contact { get; set; }
    }
}