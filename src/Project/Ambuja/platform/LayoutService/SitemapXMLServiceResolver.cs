using Project.AmbujaCement.Website.Services.SitemapXML;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class SitemapXMLServiceResolver : RenderingContentsResolver
    {
        private readonly ISitemapXMLService _rootResolver;

        public SitemapXMLServiceResolver(ISitemapXMLService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetSiteMapXML(rendering);

        }
    }
}