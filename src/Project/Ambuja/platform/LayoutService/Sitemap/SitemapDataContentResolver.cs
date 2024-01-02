using Project.AmbujaCement.Website.Services.Sitemap;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.Sitemap
{
    public class SitemapDataContentResolver : RenderingContentsResolver
    {
        private readonly ISitemapService _sitemapService;

        public SitemapDataContentResolver(ISitemapService sitemapService)
        {
            _sitemapService = sitemapService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _sitemapService.GetSitemapDataModel(rendering);
        }
    }
}