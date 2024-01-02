using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;


namespace Adani.SuperApp.Airport.Feature.Filters.Platform.Models
{
    public class FilterProductsWidgets
    {
        public WidgetItem widget { get; set; }
      //  public List<FilterProducts> widgetItems { get; set; }
    }
    /// <summary>
    /// Class to get the Material Groups
    /// </summary>
    public class FilterProducts
    {
        public string title { get; set; }
        public string apiUrl { get; set; }

        public string materialGroup { get; set; }

        public string category { get; set; }

        public string subCategory { get; set; }

        public string brand { get; set; }

        public bool popular { get; set; }

        public bool newArrival { get; set; }

        public bool showOnHomepage { get; set; }
        public string skuCode { get; set; }
    }
       
}