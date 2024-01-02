using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class ArticleFeaturedFilters
    {
        public string id { get; set; }
        public string type { get; set; }
        public List<widgetItemsList> widgetItems { get; set; }
    }

    public class widgetItemsList
    {
         public string id { get; set; }
         public string ctaText { get; set; }
         public string ctaLink { get; set; }
    }
}