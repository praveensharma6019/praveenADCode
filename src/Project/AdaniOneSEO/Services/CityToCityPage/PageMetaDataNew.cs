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
using System.Net.Http;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public class PageMetaDataNew : IPageMetaDataNew
    {
        private readonly ISitecoreService _sitecoreService;
        public PageMetaDataNew(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public PageMetaDataModelNew GetPageMetaDataNew(Rendering rendering, string location = null)
        {
            PageMetaDataModelNew data = new PageMetaDataModelNew();
            try
            {
                //var datasource = Utils.GetRenderingDatasource(rendering);
                //if (datasource == null) return null;
                var datasourcePath = new ID(rendering.DataSource);
                string solrQuery = $"_name:\"{location}\"";
                var solrItemsnew = ExecuteSolrQuery(solrQuery);
                var solrItems = GetBucketedItemsFromSolr(datasourcePath);

                data.MetaTitle = Utils.GetValue(solrItems.First(), BaseTemplates.PageMetaData.MetaTitle);
                data.MetaDescription = Utils.GetValue(solrItems.First(), BaseTemplates.PageMetaData.MetaDescription);
                data.Keywords = Utils.GetValue(solrItems.First(), BaseTemplates.PageMetaData.Keywords);
                data.Canonical = Utils.GetLinkURL(solrItems.First(), BaseTemplates.PageMetaData.Canonical.ToString());
                data.Robots = Utils.GetValue(solrItems.First(), BaseTemplates.PageMetaData.Robots);
                data.OG_Title = Utils.GetValue(solrItems.First(), BaseTemplates.PageMetaData.OG_Title);
                data.OG_Image = Utils.GetImageURLByFieldId(solrItems.First(), BaseTemplates.PageMetaData.OG_Image);
                data.OG_Description = Utils.GetValue(solrItems.First(), BaseTemplates.PageMetaData.OG_Description);
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
        private List<Item> ExecuteSolrQuery(string solrQuery)
        {
            // Use HttpClient to query Solr
            using (HttpClient httpClient = new HttpClient())
            {
                // Replace "your_solr_base_url" with your actual Solr base URL
                string solrBaseUrl = "https://dbcm.dev.local:8983/solr/db_master_index";
                string solrSelectEndpoint = "/select";

                // Construct Solr query URL
                string solrQueryUrl = $"{solrBaseUrl}{solrSelectEndpoint}?q={solrQuery}";

                try
                {
                    HttpResponseMessage response = httpClient.GetAsync(solrQueryUrl).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse Solr response and return the list of SolrItems
                        // Adjust this based on your Solr response format
                        string content = response.Content.ReadAsStringAsync().Result;
                        return null;
                    }
                    else
                    {
                        // Handle the error response if needed
                        Console.WriteLine($"Solr query failed. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions if any
                    Console.WriteLine($"Error querying Solr: {ex.Message}");
                }

                // Return an empty list in case of errors
                return null; ;
            }
        }
    }
}