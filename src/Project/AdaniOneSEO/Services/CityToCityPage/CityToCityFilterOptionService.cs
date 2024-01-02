using Glass.Mapper.Sc;
using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.CityToCityPagev2;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using System.Linq;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public class CityToCityFilterOptionService : ICityToCityFilterOptionService
    {
        private readonly ISitecoreService _sitecoreService;

        public CityToCityFilterOptionService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public FilterOptionCityToCityModel GetFilterOptionCityToCityModel(Rendering rendering)
        {
            FilterOptionCityToCityModel data = new FilterOptionCityToCityModel();
            try
            {
                var datasourcePath = new ID(rendering.DataSource);
                var solrItems = GetBucketedItemsFromSolr(datasourcePath);

                data.TripType = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityFilterOption.TripType);
                data.FareType = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityFilterOption.FareType);
                data.Cabin = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityFilterOption.Cabin);
                data.CityFrom = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityFilterOption.CityFrom);
                data.CityFromCode = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityFilterOption.CityFromCode);               
                data.CityToCode = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityFilterOption.CityToCode);               
                data.CityTo = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityFilterOption.CityTo);               
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return data;
        }
        private List<Item> GetBucketedItemsFromSolr(ID itemId)
        {
            using (var context = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
            {
                var query = context.GetQueryable<SearchResultItem>()
                    .Where(item => item.ItemId == itemId)
                    .ToList();

                return query.Select(result => result.GetItem()).ToList();
            }
        }
    }
}