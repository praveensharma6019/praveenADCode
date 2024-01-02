using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.Services
{
    public class SearchBuilder : ISearchBuilder
    {
        public IQueryable<SearchResultItem> GetSearchResults(Expression<Func<SearchResultItem, bool>> expression)
        {
            IQueryable<SearchResultItem> searchQuery = null;
            LogRepository _logRepository = new LogRepository();
          //  _logRepository.Info("Product Search Started");
            try
            {
                var context = ContentSearchManager.GetIndex(Constant.Indexname).CreateSearchContext();
                searchQuery = context.GetQueryable<SearchResultItem>().Where(expression);
                //var Result = context.GetQueryable<ProductMapping>().Where(expression).ToList();
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetSearchResults give error -> "+ ex.Message);
            }
          //  _logRepository.Info("Result Count = "+ searchQuery.Count().ToString());
            return searchQuery;
        }

        public IQueryable<SearchResultItem> GetProductSearchResults(Expression<Func<SearchResultItem, bool>> expression)
        {
            IQueryable<SearchResultItem> searchQuery = null;
            LogRepository _logRepository = new LogRepository();
            //  _logRepository.Info("Product Search Started");
            try
            {
                var context = ContentSearchManager.GetIndex(Constant.DutyfreeIndexname).CreateSearchContext();
                searchQuery = context.GetQueryable<SearchResultItem>().Where(expression);
             
                //var Result = context.GetQueryable<ProductMapping>().Where(expression).ToList();
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetProductSearchResults give error -> " + ex.Message);
            }
            //  _logRepository.Info("Result Count = "+ searchQuery.Count().ToString());
            return searchQuery;
        }

        public IQueryable<SearchResultItem> GetOfferSearchResults(Expression<Func<SearchResultItem, bool>> expression)
        {
            IQueryable<SearchResultItem> searchQuery = null;
            LogRepository _logRepository = new LogRepository();
            //  _logRepository.Info("Product Search Started");
            try
            {
                var context = ContentSearchManager.GetIndex(Constant.OffersIndexName).CreateSearchContext();
                searchQuery = context.GetQueryable<SearchResultItem>().Where(expression);
                //var Result = context.GetQueryable<ProductMapping>().Where(expression).ToList();
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetOfferSearchResults give error -> " + ex.Message);
            }
            //  _logRepository.Info("Result Count = "+ searchQuery.Count().ToString());
            return searchQuery;
        }

        public async Task<IQueryable<SearchResultItem>> GetOfferSearchResultsAsync(Expression<Func<SearchResultItem, bool>> expression)
        {
            IQueryable<SearchResultItem> searchQuery = null;
            LogRepository _logRepository = new LogRepository();
            //  _logRepository.Info("Product Search Started");
            try
            {
                var context = ContentSearchManager.GetIndex(Constant.OffersIndexName).CreateSearchContext();
                searchQuery = context.GetQueryable<SearchResultItem>().Where(expression);
                //var Result = context.GetQueryable<ProductMapping>().Where(expression).ToList();
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetOfferSearchResults give error -> " + ex.Message);
            }
            //  _logRepository.Info("Result Count = "+ searchQuery.Count().ToString());
            return searchQuery;
        }
    }
}