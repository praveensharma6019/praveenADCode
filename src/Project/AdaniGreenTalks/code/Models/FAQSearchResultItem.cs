using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website.Models
{
    public class FAQSearchResultItem : SearchResultItem
    {
        [IndexField("Question")]
        public string Question { get; set; }

        [IndexField("Answer")]
        public string Answer { get; set; }

    }
}