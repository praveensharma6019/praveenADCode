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

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public class AirportInformationService : IAirportInformationService
    {
        private readonly ISitecoreService _sitecoreService;

        public AirportInformationService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public AirportInformationModel GetAirportInformationModel(Rendering rendering)
        {
            AirportInformationModel data = new AirportInformationModel();
            try
            {
                //var datasource = Utils.GetRenderingDatasource(rendering);
                //if (datasource == null) return null;
                var datasourcePath = new ID(rendering.DataSource);
                var solrItems = GetBucketedItemsFromSolr(datasourcePath);

                data.Title = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityDetails.AirportInformationFields.Title);
                data.ToCity = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityDetails.AirportInformationFields.ToCity);
                data.ToCityDescription = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityDetails.AirportInformationFields.ToCityDescription);
                data.FromCity = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityDetails.AirportInformationFields.FromCity);
                data.FromCityDescription = Utils.GetValue(solrItems.First(), BaseTemplates.CityToCityDetails.AirportInformationFields.FromCityDescription);               
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