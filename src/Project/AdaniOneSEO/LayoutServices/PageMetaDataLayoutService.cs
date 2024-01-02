using Project.AdaniOneSEO.Website.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.LayoutServices
{
    public class PageMetaDataLayoutService : RenderingContentsResolver
    {
        private readonly IPageMetaData RootResolver;

        public PageMetaDataLayoutService(IPageMetaData Services)
        {
            this.RootResolver = Services;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetPageMetaData(rendering);
        }
    }
}