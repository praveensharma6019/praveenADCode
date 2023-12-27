using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.Models
{
    public class NavigationItems
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public string LeftIcon { get; set; }
        public string RightIcon { get; set; }
        public IList<NavigationItems2> NavigationLevel2 { get; set; }
    }
    public class NavigationItems2
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string LeftIcon { get; set; }
        public string RightIcon { get; set; }
        public IList<NavigationItems2> NavigationLevel3 { get; set; }
    }
}