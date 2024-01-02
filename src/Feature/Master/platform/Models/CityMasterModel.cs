using Adani.SuperApp.Airport.Feature.Master.Platform.Constant;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Models
{
    public class CityMasterModel : SearchResultItem
    {
        [IndexField(Constants.stateMasterId)]
        public string StateMasterId { get; set; }

        [IndexField(Constants.stateImportId)]
        public string StateImportId { get; set; }

        [IndexField(Constants.sid)]
        public string Id { get; set; }

        [IndexField(Constants.import)]
        public string Import { get; set; }

        [IndexField(Constants.slrname)]
        public string CityName { get; set; }

        [IndexField(Constants.countryMaster)]
        public string CountryMaster { get; set; }

        [IndexField(Constants.countryCode)]
        public string CountryCode { get; set; }

        [IndexField(Constants.stateCode)]
        public string StateCode { get; set; }

        [IndexField(Constants.latitude)]
        public string Latitude { get; set; }

        [IndexField(Constants.longitude)]
        public string Longitude { get; set; }

       
     
    }

    public class SearchResult
    {
        public string StateMasterId { get; set; }
        public string StateImportId { get; set; }
        public string Id { get; set; }
        public string Import { get; set; }
        public string Name { get; set; }
        public string CountryMaster { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
       
    }

    public class SearchResults
    {
        public List<SearchResult> Results { get; set; }
      
    }
}