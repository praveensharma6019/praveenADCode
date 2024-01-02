using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class Constraints
    {
        public string stroreType { get; set; }
        public string materialGroup { get; set; }
        public string moduleType { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string errorMessage { get; set; }
        public string unit { get; set; }
        public bool active { get; set; }
    }
}