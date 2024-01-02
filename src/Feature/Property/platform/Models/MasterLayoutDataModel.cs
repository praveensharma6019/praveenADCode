using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class MasterLayoutDataModel
    {
        public MasterLayoutData masterLayoutData { get; set; }
    }
    public class MasterLayoutData
    {
        public string heading { get; set; }
        public string componentID { get; set; }
        public PointerData pointerData { get; set; }
    }

    public class PointData
    {
        public string left { get; set; }
        public string bottom { get; set; }
        public string title { get; set; }
    }

    public class PointerData
    {
        public string image { get; set; }
        public string imgAlt { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public List<PointData> points { get; set; }
    }



}