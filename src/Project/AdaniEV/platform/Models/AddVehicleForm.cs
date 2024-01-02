using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class AddVehicleForm
    {
        public string id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string ctaText { get; set; }
        public List<AddVehicleFormWidgetItems> addVehicleFormWidgetItems { get; set; }
    }

    public class AddVehicleFormWidgetItems
    {
         public string id { get; set; }
         public string type { get; set; }
         public string formLabel { get; set; }
    }
}