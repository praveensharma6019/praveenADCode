using Adani.SuperApp.Airport.Feature.SiteMap.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore.ContentSearch.Utilities;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using Sitecore.Data.Fields;

namespace Adani.SuperApp.Airport.Feature.SiteMap.Platform.Services
{
    public class SiteMap : ISiteMap
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        //private readonly IWidgetService _widgetservice;
        public SiteMap(ILogRepository logRepository, IHelper helper)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            //this._widgetservice = widgetService;
        }

        public SiteMapModel DomesticFlightsSitemapData(Item datasource)
        {
            SiteMapModel sitemapmodel = new SiteMapModel();

            List<url> urlList = new List<url>();

            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;

            ISearchIndex index = ContentSearchManager.GetIndex(Constants.citytocity_domestic_webindex);
            using (IProviderSearchContext context = index.CreateSearchContext())
            {
                ID domesticFlightsTemplateID = new Sitecore.Data.ID(new Guid(Constants.CitytoCItyTemplateID));
                ID domesticFlightsHomeItemID = new Sitecore.Data.ID(new Guid(Constants.DomesticFlightsItemId));
                var domesticFlightsResults = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.TemplateId == domesticFlightsTemplateID);
                var Results = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.ItemId == domesticFlightsHomeItemID).Union(domesticFlightsResults);
                urlList = Results.ToList().Select(x =>
                new url((x.Name == "domestic-flights" ? Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/domestic-flights" : Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/domestic-flights/" + x.Name),
                       !string.IsNullOrEmpty(x.Updated.ToString("yyyy-MM-dd")) ? x.Updated.ToString("yyyy-MM-dd") : string.Empty,
                       (x.Fields.ContainsKey("changefrequencynew") && !string.IsNullOrEmpty(x.Fields["changefrequencynew"].ToString()) ? x.Fields["changefrequencynew"].ToString() : string.Empty),
                        (x.Fields.ContainsKey("prioritynew") && !string.IsNullOrEmpty(x.Fields["prioritynew"].ToString()) ? x.Fields["prioritynew"].ToString() : string.Empty),
                        string.Empty
                )
                    ).ToList();

                sitemapmodel.sitemapdata = urlList;

                return sitemapmodel;
            }
        }

        public SiteMapModel InternationalFlightsSitemapData(Item datasource)
        {
            //(x.Fields.ContainsKey("changefrequency") && !string.IsNullOrEmpty(x.Fields["changefrequency"].ToString()) ? x.Fields["changefrequency"].ToString() : string.Empty),
            SiteMapModel sitemapmodel = new SiteMapModel();

            List<url> urlList = new List<url>();

            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;
            ISearchIndex index = ContentSearchManager.GetIndex(Constants.citytocity_international_webindex);
            using (IProviderSearchContext context = index.CreateSearchContext())
            {
                ID itemTemplateID = new Sitecore.Data.ID(new Guid(Constants.CitytoCItyTemplateID));
                ID internationalFlightsHomeItemID = new Sitecore.Data.ID(new Guid(Constants.InternationalFlightsItemId));
                var internationalFlightsResults = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.TemplateId == itemTemplateID);
                var totalResults = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.ItemId == internationalFlightsHomeItemID).Union(internationalFlightsResults);
                urlList = totalResults.ToList().Select(x =>
               new url((x.Name == "international-flights" ? Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/international-flights" : Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/international-flights/" + x.Name),
                      !string.IsNullOrEmpty(x.Updated.ToString("yyyy-MM-dd")) ? x.Updated.ToString("yyyy-MM-dd") : string.Empty,
                      (x.Fields.ContainsKey("changefrequencynew") && !string.IsNullOrEmpty(x.Fields["changefrequencynew"].ToString()) ? x.Fields["changefrequencynew"].ToString() : string.Empty),
                       (x.Fields.ContainsKey("prioritynew") && !string.IsNullOrEmpty(x.Fields["prioritynew"].ToString()) ? x.Fields["prioritynew"].ToString() : string.Empty),
                       string.Empty
               )
                   ).ToList();

                sitemapmodel.sitemapdata = urlList;

                return sitemapmodel;

            }

        }

        public SiteMapModel DomesticAirlinesSitemapData(Item datasource)
        {
            SiteMapModel sitemapmodel = new SiteMapModel();

            List<url> urlList = new List<url>();

            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;
            ISearchIndex index = ContentSearchManager.GetIndex(Constants.domestic_airlines_webindex);
            using (IProviderSearchContext context = index.CreateSearchContext())
            {
                ID itemTemplateID = new Sitecore.Data.ID(new Guid(Constants.DomesticAirlinePageTemplateId));
                ID domesticAirlinesHomeItemID = new Sitecore.Data.ID(new Guid(Constants.DomesticAirlinePageId));
                var domesticAirlinesResults = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.TemplateId == itemTemplateID);
                var totalResults = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.ItemId == domesticAirlinesHomeItemID).Union(domesticAirlinesResults);
                if (totalResults != null && totalResults.Count() > 0)
                {
                    foreach (SearchResultItem sItem in totalResults)
                    {
                        url tempurl = new url(String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
                        tempurl.loc = sItem.Name == "domestic-airlines" ? Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/domestic-airlines" : Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/domestic-airlines/" + sItem.Name;
                        tempurl.lastmod = !string.IsNullOrEmpty(sItem.Updated.ToString("yyyy-MM-dd")) ? sItem.Updated.ToString("yyyy-MM-dd") : string.Empty;

                        if((sItem.Fields.ContainsKey("changefrequency") && !string.IsNullOrEmpty(sItem.Fields["changefrequency"].ToString())))
                        {
                            ID currItem = ID.Parse(sItem.Fields["changefrequency"].ToString());
                            Item mainItem = Sitecore.Context.Database.GetItem(currItem);
                            tempurl.changefreq = mainItem.Fields["Value"].ToString();
                        }

                        if ((sItem.Fields.ContainsKey("priority") && !string.IsNullOrEmpty(sItem.Fields["priority"].ToString())))
                        {
                            ID currItem = ID.Parse(sItem.Fields["priority"].ToString());
                            Item mainItem = Sitecore.Context.Database.GetItem(currItem);
                            tempurl.priority = mainItem.Fields["Value"].ToString();
                        }
                        urlList.Add(tempurl);
                    }
                }

                sitemapmodel.sitemapdata = urlList;

                return sitemapmodel;

            }

        }

        public SiteMapModel InternationalAirlinesSitemapData(Item datasource)
        {
            SiteMapModel sitemapmodel = new SiteMapModel();

            List<url> urlList = new List<url>();

            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;
            ISearchIndex index = ContentSearchManager.GetIndex(Constants.international_airlines_webindex);
            using (IProviderSearchContext context = index.CreateSearchContext())
            {
                ID itemTemplateID = new Sitecore.Data.ID(new Guid(Constants.InternationalAirlinePageTemplateId));
                ID internationalAirlinesHomeItemID = new Sitecore.Data.ID(new Guid(Constants.InternationalAirlinePageId));
                var internationalAirlinesResults = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.TemplateId == itemTemplateID);
                var totalResults = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.ItemId == internationalAirlinesHomeItemID).Union(internationalAirlinesResults);

                if (totalResults != null && totalResults.Count() > 0)
                {
                    foreach (SearchResultItem sItem in totalResults)
                    {
                        url tempurl = new url(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty);
                        tempurl.loc = sItem.Name == "international-airlines" ? Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/international-airlines" : Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/international-airlines/" + sItem.Name;
                        tempurl.lastmod = !string.IsNullOrEmpty(sItem.Updated.ToString("yyyy-MM-dd")) ? sItem.Updated.ToString("yyyy-MM-dd") : string.Empty;

                        if ((sItem.Fields.ContainsKey("changefrequency") && !string.IsNullOrEmpty(sItem.Fields["changefrequency"].ToString())))
                        {
                            ID currItem = ID.Parse(sItem.Fields["changefrequency"].ToString());
                            Item mainItem = Sitecore.Context.Database.GetItem(currItem);
                            tempurl.changefreq = mainItem.Fields["Value"].ToString();
                        }

                        if ((sItem.Fields.ContainsKey("priority") && !string.IsNullOrEmpty(sItem.Fields["priority"].ToString())))
                        {
                            ID currItem = ID.Parse(sItem.Fields["priority"].ToString());
                            Item mainItem = Sitecore.Context.Database.GetItem(currItem);
                            tempurl.priority = mainItem.Fields["Value"].ToString();
                        }
                        urlList.Add(tempurl);
                    }
                }

                sitemapmodel.sitemapdata = urlList;

                return sitemapmodel;

            }

        }
    }
}