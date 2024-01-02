using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Foundation.Search.Platform.Models
{
    public class CustomSearchResultItem:SearchResultItem
    {
        [IndexField("title_t")]
        public string Title { get; set; }
        [IndexField("summary")]
        public string Summary { get; set; }
        [IndexField("body")]
        public string Body { get; set; }
        [IndexField("templateid")]
        public string TemplateID { get; set; }
        [IndexField("slug")]
        public string Slug { get; set; }
        [IndexField("DateTime")]
        public DateTime DateTime { get; set; }
    }
}