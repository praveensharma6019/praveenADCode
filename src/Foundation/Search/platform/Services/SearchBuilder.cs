using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.Search.Platform.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace Adani.SuperApp.Realty.Foundation.Search.Platform.Services
{
    public class SearchBuilder : ISearchBuilder
    {
        LogRepository _logRepository = new LogRepository();

        public IQueryable<SearchResultItem> GetBlogCategoryResults()
        {
            IQueryable<SearchResultItem> searchQuery = null;
            _logRepository.Info("Realty Search Started");
            try
            {
                var context = ContentSearchManager.GetIndex(Constants.RealtyWebIndex).CreateSearchContext();
                searchQuery = context.GetQueryable<SearchResultItem>().Where(i => i.TemplateId == new Sitecore.Data.ID("{2FBDA2B6-BA1B-472F-8040-3EB3059024C7}"));
                //searchQuery = context.GetQueryable<SearchResultItem>().Where(item => item["category_title"].Contains("04ec5aab92314c3b88d957753adef368"));
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetSearchResults give error -> " + ex.Message);
            }
            _logRepository.Info("Result Count = " + searchQuery.Count().ToString());
            _logRepository.Info("Realty Search Ended");
            return searchQuery;
        }

        public IQueryable<SearchResultItem> GetBlogCategoryResults(string keyword)
        {
            IQueryable<SearchResultItem> searchQuery = null;
            _logRepository.Info("Realty Search Started");
            try
            {
                var context = ContentSearchManager.GetIndex(Constants.RealtyWebIndex).CreateSearchContext();
                searchQuery = context.GetQueryable<CustomSearchResultItem>().Where(i => i.TemplateId == new Sitecore.Data.ID("{2FBDA2B6-BA1B-472F-8040-3EB3059024C7}")).OrderByDescending(i => i.DateTime);
                searchQuery = searchQuery.Where(i => i.Content.Contains(keyword));
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetSearchResults give error -> " + ex.Message);
            }
            _logRepository.Info("Result Count = " + searchQuery.Count().ToString());
            _logRepository.Info("Realty Search Ended");
            return searchQuery;
        }
    }
}