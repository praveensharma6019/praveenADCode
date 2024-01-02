using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class FloorPlanData
    {
        public string tabHeading { get; set; }
        public string src { get; set; }
        public string imgAlt { get; set; }
        public List<Point> points { get; set; }
     
    }
    public class PlanData
    {
        public string componentID { get; set; }
        public string heading { get; set; }
        public List<FloorPlanData> floorPlanData { get; set; }
    }

    public class Point
    {
        public string left { get; set; }
        public string bottom { get; set; }
        public string title { get; set; }
    }

    public class ProjectFloorPlanData
    {
        public PlanData projectFloorPlanData { get; set; }
       
    }
}