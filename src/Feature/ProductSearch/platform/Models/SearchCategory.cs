using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class SearchCategory
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string CategoryAPICode { get; set; }
    }
}