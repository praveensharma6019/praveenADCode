using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Links;
using Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.CustomFields;

namespace Adani.SuperApp.Airport.Feature.MetaData.Platform.Models
{
    public static class XmlSitemap
    {
        public static string GetCacheXml(List<string> languagelist)
        {
            return GetXml(HttpContext.Current.Request.Url.ToString());
        }
        public static string GetHomeCacheXml(List<string> languagelist)
        {
            return GetHomeSitemapXml(HttpContext.Current.Request.Url.ToString());
        }
        private static string GetXml(string requestUrl)
        {
            var homeitem = global::Sitecore.Context.Item.GetHomeItem(requestUrl);
            if (homeitem.Name == Templates.HeadDataCollection.DomesticFlights || homeitem.Name == Templates.HeadDataCollection.InternationalFlights)
            {
                if (homeitem.Name == Templates.HeadDataCollection.DomesticFlights)
                {
                    return DomesticFlightsSitemap.DomesticFlightsSitemapData();
                }
                else
                {
                    return DomesticFlightsSitemap.InternationalFlightsSitemapData();
                }

            }
            else if (homeitem.Name == Templates.HeadDataCollection.DomesticAirlinesXml || homeitem.Name == Templates.HeadDataCollection.InternationalAirlinesXml)
                {
                    if (homeitem.Name == Templates.HeadDataCollection.DomesticAirlinesXml)
                    {
                        return AirlinesSitemap.DomesticAirlinesSitemapData();
                    }
                    else
                    {
                        return AirlinesSitemap.InternationalAirlinesSitemapData();
                    }

                }
                else
            {
                var detailList = homeitem.Name == Templates.HeadDataCollection.AirportHome ? homeitem.Children.ToList() : homeitem.Axes.GetDescendants().ToList();
                detailList.Add(homeitem);
                if (homeitem.Name.ToLower() == Templates.HeadDataCollection.AirportHome.ToLower())
                {
                    var allAirportsItems = Sitecore.Configuration.Settings.GetSetting("AllAirports").ToString();
                    var allAirports = allAirportsItems.Split(',').ToList();
                    detailList = detailList.Where(x => allAirports.Any(y => y.ToString() == x.ID.ToString())).ToList();
                }
                else
                {
                    var airportItems = Sitecore.Configuration.Settings.GetSetting("Airport").ToString();
                    var collection = airportItems.Split(',').ToList();
                    detailList = detailList.Where(x => collection.Any(y => y.ToString() == x.TemplateID.ToString())).ToList();
                }
                var options = global::Sitecore.Links.LinkManager.GetDefaultUrlOptions();
                options.AlwaysIncludeServerUrl = true;
                options.LanguageEmbedding = LanguageEmbedding.Never;
                return CreateSiteMapUrls(detailList, options, homeitem.Name);
            }
        }
        private static string CreateSiteMapUrls(List<Item> detailList, UrlOptions urlOptions, string homeItemName)
        {
            StringBuilder returnString = new StringBuilder();
            var XmlSitemapPriority = Templates.HeadDataCollection.Priority;
            var XmlSitemapChangeFreq = Templates.HeadDataCollection.ChangeFrequency;

            foreach (Item item in detailList)
            {
                var url = LinkManager.GetItemUrl(item, urlOptions);
                if (homeItemName.ToLower() == Templates.HeadDataCollection.AirportHome.ToLower())
                {
                    var lastModifiedDate = item.Statistics.Updated.ToString("yyyy-MM-dd");

                    returnString.AppendFormat("<sitemap><loc>{0}</loc><lastmod>{1}</lastmod></sitemap>", url + "-sitemap.xml", lastModifiedDate);
                }
                else
                {
                    var prio = SitecoreItemExtensions.GetDropLinkValue(item.Fields[XmlSitemapPriority]);
                    var changefreq = SitecoreItemExtensions.GetDropLinkValue(item.Fields[XmlSitemapChangeFreq]);
                    var lastModified = item.Statistics.Updated.ToString("yyyy-MM-dd");
                    var mobile = "<mobile:mobile/>";

                    returnString.AppendFormat("<url><loc>{0}</loc><lastmod>{1}</lastmod><changefreq>{2}</changefreq><priority>{3}</priority>{4}</url>", url, lastModified, changefreq, prio, mobile);
                }
            }
            return returnString.ToString();
        }

        public static Item GetHomeItem(this Item item, string currentUrl)
        {
            global::Sitecore.Sites.SiteContext site = global::Sitecore.Context.Site;

            if (site == null)
            {
                return null;
            }

            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;
            if (currentUrl.Contains(Templates.HeadDataCollection.AhmedabadXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("ahmedabad").ToString());
            }
            else if (currentUrl.Contains(Templates.HeadDataCollection.MumbaiXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("mumbai").ToString());
            }
            else if (currentUrl.Contains(Templates.HeadDataCollection.GuwahatiXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("guwahati").ToString());
            }
            else if (currentUrl.Contains(Templates.HeadDataCollection.JaipurXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("jaipur").ToString());
            }
            else if (currentUrl.Contains(Templates.HeadDataCollection.LucknowXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("lucknow").ToString());
            }
            else if (currentUrl.Contains(Templates.HeadDataCollection.MangaluruXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("mangaluru").ToString());
            }
            else if (currentUrl.Contains(Templates.HeadDataCollection.ThiruvananthapuramXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("thiruvananthapuram").ToString());
            }
            else if (currentUrl.Contains(Templates.HeadDataCollection.DomesticFlightsXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("DomesticFlights").ToString());
            }
            else if (currentUrl.Contains(Templates.HeadDataCollection.InternationalFlightsXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("AllInternationalFlights").ToString());
            }
            else if (currentUrl.Contains(Templates.HeadDataCollection.DomesticAirlinesXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("DomesticAirlines").ToString());
            }
            else if (currentUrl.Contains(Templates.HeadDataCollection.InternationalAirlinesXml))
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("InternationalAirlines").ToString());
            }
            else
            {
                return db.GetItem(Sitecore.Configuration.Settings.GetSetting("home").ToString());
            }
        }
        public static string GetHomeSitemapXml(string requestUrl)
        {
            var homeitem = global::Sitecore.Context.Item.GetHomeItem(requestUrl);
            var homeList = homeitem.Children.ToList();
            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;
            if (homeitem.Name.ToLower() == Templates.HeadDataCollection.AirportHome.ToLower())
            {
                Item homeServicesItem = db.GetItem(Sitecore.Configuration.Settings.GetSetting("HomeServices").ToString());
                var servicesGlobalList = homeServicesItem.Children.ToList();
                foreach (var servicePage in servicesGlobalList)
                {
                    homeList.Add(servicePage);
                }
                homeList = homeList.Where(x => x.TemplateID.ToString() != Sitecore.Configuration.Settings.GetSetting("FolderTemplatedId").ToString()).ToList();
                var homeSitemapItems = Sitecore.Configuration.Settings.GetSetting("HomeSitemapUnwantedUrlsIds").ToString();
                var collectionHomeItems = homeSitemapItems.Split(',').ToList();
                homeList = homeList.Where(x => !collectionHomeItems.Any(y => y.ToString() == x.ID.ToString())).ToList();
            }

            var options = global::Sitecore.Links.LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            options.LanguageEmbedding = LanguageEmbedding.Never;
            return CreateHomeSiteMapUrls(homeList, options, homeitem.Name);

        }
        private static string CreateHomeSiteMapUrls(List<Item> detailList, UrlOptions urlOptions, string homeItemName)
        {
            StringBuilder returnString = new StringBuilder();
            foreach (Item item in detailList)
            {
                var url = LinkManager.GetItemUrl(item, urlOptions);
                if (homeItemName.ToLower() == Templates.HeadDataCollection.AirportHome.ToLower())
                {
                    returnString.AppendFormat("<url><loc>{0}</loc></url>", url);
                }
            }
            return returnString.ToString();
        }
    }
}