using Project.AdaniOneSEO.Website.Services.SitemapXml;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.LayoutServices.SitemapXml
{
    public class SitemapXmlContentResolver : RenderingContentsResolver
    {
        private readonly ISitemapXmlService RootResolver;

        public SitemapXmlContentResolver(ISitemapXmlService Services)
        {
            this.RootResolver = Services;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetSiteMapXML(rendering);
        }
    }
}