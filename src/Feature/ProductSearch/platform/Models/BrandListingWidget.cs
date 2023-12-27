using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class BrandListingWidget
    {
        public WidgetItem widget { get; set; }
     //   public List<BrandListing> widgetItems { get; set; }
    }

    /// <summary>
    /// Class to get the Material Groups
    /// </summary>
    public class BrandListing
    {
        public string title { get; set; }

        public string code { get; set; }

        public string cdnPath { get; set; }

        public string description { get; set; }

        public string imageSrc { get; set; }       

        public string materialGroup { get; set; }

        public string brand { get; set; }

        public string airportCode { get; set; }

        public string storeType { get; set; }

        public bool restricted { get; set; }

        public bool disableForAirport { get; set; }
    }
    
}