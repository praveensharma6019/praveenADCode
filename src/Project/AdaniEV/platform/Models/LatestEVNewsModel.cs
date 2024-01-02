using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class LatestEVNewsModel
    {
        public string id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string backgroundColor { get; set; }
        public List<LatestEVNewsWidgetItem> widgetItems { get; set; }
    }

    public class LatestEVNewsWidgetItem
    {
        public string id { get; set; }
        public string imageSrc { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
    }
}