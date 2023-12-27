using Adani.SuperApp.Airport.Feature.Master.Platform.Constant;
using Adani.SuperApp.Airport.Feature.Master.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using static Sitecore.ContentSearch.Linq.Extensions.ReflectionExtensions;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public class CityMasterService : ICityMasterService
    {
        private readonly ILogRepository _logRepository;

        /// <summary>
        /// instanciate logRepository method
        /// </summary>
        /// <param name="logRepository"></param>
        public CityMasterService(ILogRepository logRepository)
        {

            this._logRepository = logRepository;
        }
        public SearchResults GetCityMasterData(string stateCode, string countrycode, string contextdb)
        {
            var myResults = new SearchResults
            {
                Results = new List<SearchResult>()
            };
            if (!string.IsNullOrEmpty(stateCode) || !string.IsNullOrEmpty(countrycode))
            {
                string currentindex = contextdb == "master" ? Constant.Constants.master_index : Constant.Constants.web_index;
                var searchIndex = ContentSearchManager.GetIndex(currentindex);
                var searchPredicate = CityMasterService.GetSearchPredicate(stateCode, countrycode);
                try
                {
                    using (var context = searchIndex.CreateSearchContext())
                    {
                        var results = context.GetQueryable<CityMasterModel>().Where(searchPredicate).ToList().GroupBy(x => new { x.CityName , x.StateCode , x.CountryCode, x.StateImportId, x.StateMasterId }).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Name).ToList(); 
                        if (results != null && results.Count > 0)
                        {
                            foreach (var hit in results)
                            {
                                if (!string.IsNullOrEmpty(hit.Id))
                                {
                                    SearchResult searchResult = new SearchResult();
                                    searchResult.Id = hit.Id;
                                    searchResult.Name = hit.CityName;
                                    searchResult.StateImportId = hit.StateImportId;
                                    searchResult.StateMasterId = hit.StateMasterId;
                                    searchResult.Import = hit.Import;
                                    searchResult.CountryCode = hit.CountryCode;
                                    searchResult.CountryMaster = hit.CountryMaster;
                                    searchResult.StateCode = hit.StateCode;
                                    searchResult.Latitude = hit.Latitude;
                                    searchResult.Longitude = hit.Longitude;
                                    myResults.Results.Add(searchResult);

                                }
                            }
                        }


                    }
                }
                catch (Exception ex)
                {
                    _logRepository.Error("CityMasterService.GetCityMasterData() gives -> " + ex.Message);
                }
            }
          
            return myResults;


        }

        public static Expression<Func<CityMasterModel, bool>> GetSearchPredicate(string stateCode, string countrycode)
        {
            var predicate = PredicateBuilder.True<CityMasterModel>();
            if (!string.IsNullOrEmpty(stateCode) && string.IsNullOrEmpty(countrycode))
            {
                predicate = predicate.Or(x => x.StateCode == stateCode).And<CityMasterModel>(i => i.TemplateId == new Sitecore.Data.ID(Templates.CityStateMaster.citymaster_template));
            }
            else if (!string.IsNullOrEmpty(countrycode) && string.IsNullOrEmpty(stateCode))
            {
                predicate = predicate.Or(x => x.CountryCode == countrycode).And<CityMasterModel>(i => i.TemplateId == new Sitecore.Data.ID(Templates.CityStateMaster.citymaster_template));
            }
            else if (!string.IsNullOrEmpty(countrycode) && !string.IsNullOrEmpty(stateCode))
            {
                predicate = predicate.Or(x => x.CountryCode == countrycode && x.StateCode == stateCode).And<CityMasterModel>(i => i.TemplateId == new Sitecore.Data.ID(Templates.CityStateMaster.citymaster_template));
            }


            return predicate;
        }
    }
}