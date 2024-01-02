using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Adani.SuperApp.Realty.Foundation.Search.Platform.Services
{
    public interface ISearchBuilder
    {
        IQueryable<SearchResultItem> GetBlogCategoryResults();
    }
}