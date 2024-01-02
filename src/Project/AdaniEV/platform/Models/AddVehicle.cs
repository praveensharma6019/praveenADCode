using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class AddVehicle
    {
        public string id { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string type { get; set; }
        public string imageSrc { get; set; }
        public List<AddVehicleWidgetItems> addVehicleWidgetItems { get; set; }
    }

    public class AddVehicleWidgetItems
    {
         public string id { get; set; }
         public string imageSrc { get; set; }
         public string title { get; set; }
    }
}