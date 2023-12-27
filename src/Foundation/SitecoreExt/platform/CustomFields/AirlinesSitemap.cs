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
    public static class AirlinesSitemap
    {
        public static string DomesticAirlinesSitemapData()
        {
            StringBuilder returnString = new StringBuilder();
            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;

            var mainItem = db.GetItem(Constants.Constant.DomesticAirlinesItemId);
            var childrens = mainItem.Axes.GetDescendants();

            foreach (var item in childrens)
            {
                var url = (item.Name == "domestic-airlines" ? Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/domestic-airlines" : Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/domestic-airlines/" + item.Name);
                var prio = (!string.IsNullOrEmpty(item.Fields["priority"].ToString()) ? string.Join(".", item.Fields["priority"].ToString().ToCharArray()) : string.Empty);
                var changefreq = (!string.IsNullOrEmpty(item.Fields["changefrequency"].ToString()) ? item.Fields["changefrequency"].ToString() : string.Empty);
                var lastModified = item.Statistics.Updated.ToString("yyyy-MM-dd");
                var mobile = "<mobile:mobile/>";

                returnString.AppendFormat("<url><loc>{0}</loc><lastmod>{1}</lastmod><changefreq>{2}</changefreq><priority>{3}</priority>{4}</url>", url, lastModified, changefreq, prio, mobile);

            }
            return returnString.ToString();
        }
        public static string InternationalAirlinesSitemapData()
        {
            StringBuilder returnString = new StringBuilder();
            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;

            var mainItem = db.GetItem(Constants.Constant.InternationalAirlinesItemId);
            var childrens = mainItem.Axes.GetDescendants();

            foreach (var item in childrens)
            {
                var url = (item.Name == "international-airlines" ? Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/international-airlines" : Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/international-airlines/" + item.Name);
                var prio = (!string.IsNullOrEmpty(item.Fields["priority"].ToString()) ? string.Join(".", item.Fields["priority"].ToString().ToCharArray()) : string.Empty);
                var changefreq = (!string.IsNullOrEmpty(item.Fields["changefrequency"].ToString()) ? item.Fields["changefrequency"].ToString() : string.Empty);
                var lastModified = item.Statistics.Updated.ToString("yyyy-MM-dd");
                var mobile = "<mobile:mobile/>";

                returnString.AppendFormat("<url><loc>{0}</loc><lastmod>{1}</lastmod><changefreq>{2}</changefreq><priority>{3}</priority>{4}</url>", url, lastModified, changefreq, prio, mobile);

            }
            return returnString.ToString();
        }
    }
}