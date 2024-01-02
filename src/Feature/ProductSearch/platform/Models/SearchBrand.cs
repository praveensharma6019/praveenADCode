using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class SearchBrand : SearchResultItem
    {
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public string BrandAPICode { get; set; }
        public string materialGroup { get; set; }
    }
}