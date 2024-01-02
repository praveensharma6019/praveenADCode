using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Project.AdaniFacts.Website.Models
{
    public class FactSearchResultItem : SearchResultItem
    {
        [IndexField("title")]
        public string Title { get; set; }

        [IndexField("summary")]
        public string Summary { get; set; }

        [IndexField("date")]
        public DateTime Datetime { 
            get {
                DateTime.TryParse(Fields["date"]?.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out var date);
                return date; 
            }
        }

        [IndexField("categorylist")]
        public List<string> Category { get; set; }
    }
}