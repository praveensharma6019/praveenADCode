using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Widget.Models;

namespace Adani.SuperApp.Airport.Feature.BrandListing.Models
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
        public string BrandName { get; set; }

        public string BrandCDNImage { get; set; }

        public string BrandDescription { get; set; }

        public string BrandCode { get; set; }

        public string Image { get; set; }

        public bool Visibleonbrandyoulove { get; set; }
    }
    
}