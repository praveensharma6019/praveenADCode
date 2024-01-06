using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.NPRPL.Website.Models
{

    [Serializable]
    public class AdaniRoadGallery
    {
        public string MonthYearName { get; set; }
        public List<RoadImages> AdaniRoadImages { get; set; }
        public AdaniRoadGallery()
        {
            AdaniRoadImages = new List<RoadImages>();
        }
    }
    [Serializable]
    public class RoadImages
    {
        public string ImageUrl { get; set; }
    }
}