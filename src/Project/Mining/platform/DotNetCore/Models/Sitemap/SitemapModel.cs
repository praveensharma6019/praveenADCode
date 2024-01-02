using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.Sitemap
{
    public class SitemapModel
    {
        [SitecoreComponentField]
        public List<SitemapLinks>? SitemapLinks { get; set; }
    }
    public class SitemapLinks
    {
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public List<SitemapInnerLinks>? Items { get; set; }
    }
    public class SitemapInnerLinks
    {
        [SitecoreComponentField]
        public  LinkData? Link { get; set; }
        [SitecoreComponentField]
        public GtmDataModel? GtmData { get; set; }
    }
}
