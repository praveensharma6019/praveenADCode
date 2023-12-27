using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
    public class FlightList : IFlightList
    {
        private readonly ILogRepository _logRepository;

        /// <summary>
        /// instanciate logRepository method
        /// </summary>
        /// <param name="logRepository"></param>
        public FlightList(ILogRepository logRepository)
        {

            this._logRepository = logRepository;
        }

        public SearchResults GetFlightListData(string queryString, string fileds, string airlineType, string contextDB)
        {

            var myResults = new SearchResults
            {
                Results = new List<SearchResult>()
            };
            try
            {
                string searchItem = queryString;
                string strAirlineType = airlineType;
                var searchIndex = ContentSearchManager.GetIndex(Constants.Constants.AirlineSearchIndex); // Get the search index
                //var searchIndex = ContentSearchManager.GetIndex(GetContextIndexValue(contextDB));
                var searchPredicate = FlightList.GetSearchPredicate(searchItem, strAirlineType); // Build the search predicate

                using (var searchContext = searchIndex.CreateSearchContext()) // Get a context of the search index
                {
                    var searchResults = searchContext.GetQueryable<FlightSearchResult>();//.Select(x=>x.AirlineName);
                    if (!string.IsNullOrEmpty(searchItem) || !string.IsNullOrEmpty(strAirlineType))
                    {
                        searchResults = searchContext.GetQueryable<FlightSearchResult>().Where(searchPredicate);//.Select(x => x.AirlineName); // Search the index for items which match the predicate

                    }
                    
                    // This will get all of the results, which is not recommended
                    var fullResults = searchResults.GetResults();//.Where(x=>x.Document.AirlineType.ToLower()=="domestic");//.Select(x => new { x.Document.AirlineName, x.Document.AirlineCode }); ;
                    switch (fileds)
                    {
                        case "all":
                            foreach (var hit in fullResults)
                            {
                                if (!string.IsNullOrEmpty(hit.Document.AirlineCode))
                                {
                                    SearchResult searchResult = new SearchResult();
                                    searchResult.AirlineCode = hit.Document.AirlineCode;
                                    searchResult.AirlineName = hit.Document.AirlineName;
                                    if (!String.IsNullOrEmpty(hit.Document.AirlineLogo) && hit.Document.AirlineLogo.Contains("/sitecore/shell/"))
                                    {
                                        searchResult.AirlineLogo = hit.Document.AirlineLogo.Replace("/sitecore/shell/", "/");
                                    }
                                    else { searchResult.AirlineLogo = hit.Document.AirlineLogo; }
                                    if (!String.IsNullOrEmpty(hit.Document.ThumbnailImage) && hit.Document.ThumbnailImage.Contains("/sitecore/shell/"))
                                    {
                                        searchResult.ThumbnailImage = hit.Document.ThumbnailImage.Replace("/sitecore/shell/", "/");
                                    }
                                    else { searchResult.ThumbnailImage = hit.Document.ThumbnailImage; }
                                    if (!String.IsNullOrEmpty(hit.Document.MobileImage) && hit.Document.MobileImage.Contains("/sitecore/shell/"))
                                    {
                                        searchResult.MobileImage = hit.Document.MobileImage.Replace("/sitecore/shell/", "/");
                                    }
                                    else { searchResult.MobileImage = hit.Document.MobileImage; }
                                    searchResult.AirlineID = hit.Document.AirlineID;
                                    searchResult.AirlineCancellationPolicy = hit.Document.AirlineCancellationPolicy;
                                    searchResult.AirlineType = hit.Document.AirlineType;

                                    myResults.Results.Add(searchResult);
                                }
                            }
                            break;
                        case "namecode":
                            myResults = new SearchResults
                            {
                                NameCodeResults = new List<SearchResultNameCode>()
                            };
                            foreach (var hit in fullResults)
                            {
                                myResults.NameCodeResults.Add(new SearchResultNameCode
                                {
                                    AirlineCode = hit.Document.AirlineCode,
                                    AirlineName = hit.Document.AirlineName

                                });
                            }
                            break;

                        case "cancellationploicy":
                            myResults = new SearchResults
                            {
                                CancellationPloicy = new List<SearchResultCancelPloicy>()
                            };
                            foreach (var hit in fullResults)
                            {
                                myResults.CancellationPloicy.Add(new SearchResultCancelPloicy
                                {
                                    AirlineCode = hit.Document.AirlineCode,
                                    AirlineCancellationPolicy = hit.Document.AirlineCancellationPolicy

                                });
                            }
                            break;

                    }                 

                    


                }

            }
            catch (Exception ex)
            {
                _logRepository.Error("GetFlightListData() gives -> " + ex.Message);
            }

            return myResults;
        }

        public static string GetSingleLineFieldValue(Field field)
        {
            string text = string.Empty;
            if (field != null && field.Value != null)
            {
                text = field.Value;
            }
            return text;

        }
       

        public static Expression<Func<FlightSearchResult, bool>> GetSearchPredicate(string searchTerm, string airlineType)
        {
            var predicate = PredicateBuilder.True<FlightSearchResult>();
            // Search the whole phrase – CONTAINS
            if (!string.IsNullOrEmpty(searchTerm))
            {
                predicate = predicate.Or(x => x.AirlineCode.Contains(searchTerm));               
                // predicate = predicate.Or(x => x.AirlineLogo.Contains(searchTerm));
                //  predicate = predicate.Or(x => x.AirlineID.Contains(searchTerm));
            }
            if (!string.IsNullOrEmpty(airlineType))
            {
                predicate = predicate.Or(x => x.AirlineType.Contains(airlineType));
            }

            return predicate;
        }

        public static string GetContextIndexValue(string contextDB)
        {



            if (contextDB == "master")
            {
                return Constants.Constants.AirlineSearchIndex;
            }
            else if (contextDB == "web")
            {
                return Constants.Constants.AirlineSearchIndexWeb;
            }
            return "";
        }
    }
}