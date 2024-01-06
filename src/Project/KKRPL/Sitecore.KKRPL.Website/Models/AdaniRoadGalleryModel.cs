using System;
using System.Collections.Generic;

namespace Sitecore.KKRPL.Website.Models
{
    [Serializable]
    public class AdaniRoadGallery
    {
        public string MonthYearName { get; set; }

        public List<RoadImages> AdaniRoadImages { get; set; }

        public AdaniRoadGallery() => this.AdaniRoadImages = new List<RoadImages>();
    }
}
