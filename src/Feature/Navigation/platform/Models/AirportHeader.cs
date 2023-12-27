using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.Models
{
    public class AirportHeader
    {
        public List<NavigationItems> NavigationLevel1 { get; set; }
    }
    public class CollapsableExpandale
    {
        public string ParentTitle { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string LeftIcon { get; set; }
        public string RightIcon { get; set; }
        public IList<NavigationItems> NavigationLevel1 { get; set; }

    }
}