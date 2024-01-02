using Project.AAHL.Website.Services.SitemapXML;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.SitemapXML
{
    public class SitemapXMLContentResolver : RenderingContentsResolver
    {
        private readonly ISitemapXMLService _sitemapXMLService;

        public SitemapXMLContentResolver(ISitemapXMLService sitemapXMLService)
        {
            _sitemapXMLService = sitemapXMLService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _sitemapXMLService.GetSiteMapXML(rendering);
        }
    }
}