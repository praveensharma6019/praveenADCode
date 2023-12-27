using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Constants;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models
{
    public class FlightSearchResult : SearchResultItem
    {
        [IndexField(Constants.Constants.AirlineCode)]
        public virtual string AirlineCode { get; set; }

        [IndexField(Constants.Constants.AirlineName)]
        public virtual string AirlineName { get; set; }

        [IndexField(Constants.Constants.AirlineLogo)]
        public virtual string AirlineLogo { get; set; }
        [IndexField(Constants.Constants.ThumbnailImage)]
        public virtual string ThumbnailImage { get; set; }
        [IndexField(Constants.Constants.MobileImage)]
        public virtual string MobileImage { get; set; }

        [IndexField(Constants.Constants.AirlineID)]
        public virtual string AirlineID { get; set; }

        [IndexField(Constants.Constants.AirlineType)]
        public virtual string AirlineType { get; set; }

        public virtual string AirlineCancellationPolicy { get; set; }

        
    }

    public class SearchResult
    {
        public string AirlineCode { get; set; }
        public string AirlineName { get; set; }
        public string AirlineLogo { get; set; }
        public string ThumbnailImage { get; set; }
        public string MobileImage { get; set; }
        public string AirlineID { get; set; }
        public string AirlineCancellationPolicy { get; set; }        
        public string AirlineType { get; set; }
    }
    public class SearchResultNameCode
    {
        public string AirlineCode { get; set; }
        public string AirlineName { get; set; }
        

    }
    public class SearchResultCancelPloicy
    {
        public string AirlineCode { get; set; }
        public string AirlineCancellationPolicy { get; set; }
        


    }

    /// <summary>
    /// Custom search result model for binding to front end
    /// </summary>
    public class SearchResults
    {
        public List<SearchResult> Results { get; set; }
        public List<SearchResultNameCode> NameCodeResults { get; set; }
        public List<SearchResultCancelPloicy> CancellationPloicy { get; set; }
    }
}