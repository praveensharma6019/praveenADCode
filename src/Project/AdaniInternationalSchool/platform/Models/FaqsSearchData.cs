using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class PopularSuggestion
    {
        public string ItemHeading { get; set; }
        public string ItemLink { get; set; }
        public string ItemType { get; set; }
        public string CategoryID { get; set; }
        public GtmDataModel GtmData { get; set; }
    }
  
    public class SearchData
    {
        public string SearchPlaceholder { get; set; }
        public string PopularSearchKeyword { get; set; }
        public string SuggestionKeyword { get; set; }
        public List<PopularSuggestion> PopularSuggestions { get; set; }
    }

}