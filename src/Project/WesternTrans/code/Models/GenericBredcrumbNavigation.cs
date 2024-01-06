using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class GenericBredcrumbNavigation
    {
        public List<GenericBredcrumbNavigationItem> Data { get; set; }
    }

    public class GenericBredcrumbNavigationItem
    {
        public string Heading { get; set; }
        public string Link { get; set; }
    }
}