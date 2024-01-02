using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Sitemap.Platform.Models
{
    #region Model Type Item

    public class SitemapDataItem
    {

        [JsonProperty(PropertyName = "heading")]
        public string SitemapHeading { get; set; }

        [JsonProperty(PropertyName = "md")]
        public string SitemapMD { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<SitemapInnerDataItem> SitemapItems { get; set; }

    }
    public class SitemapInnerDataItem
    {

        [JsonProperty(PropertyName = "title")]
        public string SitemapInnerTitle { get; set; }
        [JsonProperty(PropertyName = "link")]
        public string link { get; set; }
        [JsonProperty(PropertyName = "target")]
        public string target { get; set; }
        [JsonProperty(PropertyName = "keys")]
        public List<SitemapInnerDataItemKeyPage> SitemapInnerkeysPage { get; set; }
    }

    public class SitemapInnerDataItemKeyPage
    {
        [JsonProperty(PropertyName = "page")]
        public string SitemapInnerDataItemkeyPage { get; set; }
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "target")]
        public string target { get; set; }
    }

    public class ConfigurationDataItem
    {

        [JsonProperty(PropertyName = "title")]
        public string ConfigurationInnerTitle { get; set; }

        [JsonProperty(PropertyName = "keys")]
        public List<ConfigurationInnerDataItem> ConfigurationInnerkeysPage { get; set; }
    }

    public class ConfigurationInnerDataItem
    {

        [JsonProperty(PropertyName = "link")]
        public string ConfigurationInnerLink { get; set; }

        [JsonProperty(PropertyName = "keyword")]
        public string ConfigurationInnerkeyword { get; set; }
    }

    #endregion

    #region Model Data
    public class SitemapData
    {
        public string PageTitle { get; set; }
        public List<Object> links { get; set; }
    }
    public class ConfigurationData
    {
        public List<Object> items { get; set; }
    }
    #endregion
    public class DynamicSitemap
    {
        public string url { get; set; }
        public string priority { get; set; }
        public string lastmod { get; set; }
        public double descPriority { get; set; }

    }
}