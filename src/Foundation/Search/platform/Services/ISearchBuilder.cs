using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Models;
using Sitecore.ContentSearch.SearchTypes;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.Services
{
   public interface ISearchBuilder
    {
        IQueryable<SearchResultItem> GetSearchResults(Expression<Func<SearchResultItem, bool>> expression);
        IQueryable<SearchResultItem> GetProductSearchResults(Expression<Func<SearchResultItem, bool>> expression);
        IQueryable<SearchResultItem> GetOfferSearchResults(Expression<Func<SearchResultItem, bool>> expression);
        Task<IQueryable<SearchResultItem>> GetOfferSearchResultsAsync(Expression<Func<SearchResultItem, bool>> expression);
    }
}
