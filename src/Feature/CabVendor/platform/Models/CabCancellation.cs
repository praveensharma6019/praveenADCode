using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models
{
    public class CancellationData
    {
        public string headerTitle { get; set; }
        public string headerDescription { get; set; }
        public string buttonUrl { get; set; }
        public string buttonText { get; set; }
        public string title { get; set; }
        public List<Reasons> reasons { get; set; }
    }

    public class Reasons
    {
        public string reason { get; set; }
        public string descriptionHint { get; set; }
        public string descriptionLength { get; set; }
    }
}