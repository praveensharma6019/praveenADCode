using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.MetaData.Platform.Models
{
    public static class DomesticFlightsData
    { 
        public static string GetDomesticFlightsSitemapData()
        {
            StringBuilder returnString = new StringBuilder();
            global::Sitecore.Data.Database db = global::Sitecore.Context.Database;
            var homeItem = db.GetItem(Sitecore.Configuration.Settings.GetSetting("DomesticFlights").ToString());
            var detailList = homeItem.Axes.GetDescendants().Where(x =>x.TemplateID.ToString()== Sitecore.Configuration.Settings.GetSetting("AllDomesticFlights").ToString()).ToList();
            detailList.Add(homeItem);

            var XmlSitemapPriority = Templates.HeadDataCollection.Priority;
            var XmlSitemapChangeFreq = Templates.HeadDataCollection.ChangeFrequency;
            var options = global::Sitecore.Links.LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            foreach (var item in detailList)
            {
                var url = LinkManager.GetItemUrl(item, options);
                var prio = SitecoreItemExtensions.GetDropLinkValue(item.Fields[XmlSitemapPriority]);
                var changefreq = SitecoreItemExtensions.GetDropLinkValue(item.Fields[XmlSitemapChangeFreq]);
                var lastModified = item.Statistics.Updated.ToString("yyyy-MM-dd");
                var mobile = "<mobile:mobile/>";

                returnString.AppendFormat("<url><loc>{0}</loc><lastmod>{1}</lastmod><changefreq>{2}</changefreq><priority>{3}</priority>{4}</url>", url, lastModified, changefreq, prio, mobile);

            }
            return returnString.ToString();
        }
    }
}