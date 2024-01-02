using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class Filters
    {
        public string slug { get; set; }
        public string language { get; set; }
        public string[] skuCode { get; set; }
        public string materialGroup { get; set; }
        public string[] brand { get; set; }
        public string[] category { get; set; }
        public string[] subCategory { get; set; }
        public string[] offers { get; set; }
        public bool showOnHomepage { get; set; }        
        public string filterType { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; } 
        public string channel { get; set; }
        public string MaterialGroupId { get; set; }

        public string airportCode { get; set; } 
        public string storeCode { get; set; }
        public string terminal { get; set; }
        public string storeType { get; set; }
        public string sort { get; set; }
        public int minPrice { get; set; }
        public int maxPrice { get; set; }

        public string bucketGroup { get; set; }

        public bool travelExclusive { get; set; }
        public bool restricted { get; set; }

        public string agentId { get; set; }

        public string pageType { get; set; }

        public bool exclusive { get; set; }
        public bool isCombo { get; set; }
        public string[] comboFilter { get; set; }

        public bool includeOOS { get; set; }
        public string searchText { get; set; }
        public Filters()
        {
            page = 1;
            pageSize = 10;
            channel = "web";
            language = "en";
            airportCode = "BOM";
            channel = "";
            minPrice = 0;
            maxPrice = 1000000;
            travelExclusive = false;
            restricted = false;
            materialGroup = "";
            brand = new List<string>().ToArray();
            category = new List<string>().ToArray();
            subCategory =  new List<string>().ToArray();
            skuCode = new List<string>().ToArray();
            offers = new List<string>().ToArray();
            agentId = "5000";
            sort = "";
            slug = "";
            pageType = "";
            exclusive = false;
            isCombo = false;
            comboFilter = new List<string>().ToArray();
            includeOOS = false;
            searchText = string.Empty;
            storeType = "departure";
        }
    }

    public class FilterAPI
    {
        public string materialGroup { get; set; }
        public string[] brand { get; set; }
        public string[] category { get; set; }
        public string[] subCategory { get; set; }
        public string[] offers { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public string airportCode { get; set; }
        public string storeType { get; set; }
        public string sort { get; set; }
        public int minPrice { get; set; }
        public int maxPrice { get; set; }
        public bool exclusive { get; set; }
        public bool isCombo { get; set; }
        public bool includeOOS { get; set; }

        public FilterAPI()
        {
            page = 1;
            pageSize = 1;           
            airportCode = "BOM";
            minPrice = 0;
            maxPrice = 1000000;
            materialGroup = "";
            brand = new List<string>().ToArray();
            category = new List<string>().ToArray();
            subCategory = new List<string>().ToArray();
            offers = new List<string>().ToArray();
            sort = "";
            exclusive = false;
            isCombo = false;
            includeOOS = false;
        }
    }

    public class ConstraintFilter
    {
        public string language  { get; set; }
    }

    public class OfferFilter
    {
        public string operatorType { get; set; }
        public string materialGroupName { get; set; }
        public string airportCode { get; set; }

        public OfferFilter()
        {
            operatorType = "departure";
            airportCode = "BOM";
            materialGroupName = "";
        }

    }
}