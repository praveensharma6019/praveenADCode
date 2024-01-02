using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;

namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.CustomFields
{
    public static class DomesticFlightsSitemap 
    {
        public static string DomesticFlightsSitemapData()
        {
            StringBuilder returnString = new StringBuilder();
            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;
            
            ISearchIndex index = ContentSearchManager.GetIndex(Constants.Constant.citytocity_domestic_webindex);
            using (IProviderSearchContext context = index.CreateSearchContext())
            {
                ID domesticFlightsTemplateID = new Sitecore.Data.ID(new Guid(Constants.Constant.CitytoCItyTemplateID));
                ID domesticFlightsHomeItemID = new Sitecore.Data.ID(new Guid(Constants.Constant.DomesticFlightsItemId));
                var domesticFlightsResults = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.TemplateId == domesticFlightsTemplateID);
                var Results = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.ItemId == domesticFlightsHomeItemID).Union(domesticFlightsResults);

                foreach (var item in Results)
                {
                    var url = (item.Name== "domestic-flights" ? Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/domestic-flights" : Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/domestic-flights/" + item.Name);
                    var prio = (!string.IsNullOrEmpty(item.Fields["priority"].ToString())? string.Join(".", item.Fields["priority"].ToString().ToCharArray()) : string.Empty);
                    var changefreq = (!string.IsNullOrEmpty(item.Fields["changefrequency"].ToString()) ? item.Fields["changefrequency"] : string.Empty);
                    var lastModified = item.Updated.ToString("yyyy-MM-dd");
                    var mobile = "<mobile:mobile/>";

                    returnString.AppendFormat("<url><loc>{0}</loc><lastmod>{1}</lastmod><changefreq>{2}</changefreq><priority>{3}</priority>{4}</url>", url, lastModified, changefreq, prio, mobile);

                }
            }
            return returnString.ToString();
        }
        public static string InternationalFlightsSitemapData()
        {
            StringBuilder returnString = new StringBuilder();
            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;
            ISearchIndex index = ContentSearchManager.GetIndex(Constants.Constant.citytocity_international_webindex);
            using (IProviderSearchContext context = index.CreateSearchContext())
            {
                ID itemTemplateID = new Sitecore.Data.ID(new Guid(Constants.Constant.CitytoCItyTemplateID));
                ID internationalFlightsHomeItemID = new Sitecore.Data.ID(new Guid(Constants.Constant.InternationalFlightsItemId));
                var internationalFlightsResults = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.TemplateId == itemTemplateID);
                var totalResults = context.GetQueryable<SearchResultItem>()
                                     .Where(x => x.ItemId == internationalFlightsHomeItemID).Union(internationalFlightsResults);

                foreach (var item in totalResults)
                {
                    var url = (item.Name == "international-flights" ? Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/international-flights": Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/international-flights/" + item.Name);
                    var prio = (!string.IsNullOrEmpty(item.Fields["priority"].ToString()) ? string.Join(".", item.Fields["priority"].ToString().ToCharArray()) : string.Empty);
                    var changefreq = (!string.IsNullOrEmpty(item.Fields["changefrequency"].ToString()) ? item.Fields["changefrequency"] : string.Empty);
                    var lastModified = item.Updated.ToString("yyyy-MM-dd");
                    var mobile = "<mobile:mobile/>";

                    returnString.AppendFormat("<url><loc>{0}</loc><lastmod>{1}</lastmod><changefreq>{2}</changefreq><priority>{3}</priority>{4}</url>", url, lastModified, changefreq, prio, mobile);

                }
            }
            return returnString.ToString();
        }
    }
}