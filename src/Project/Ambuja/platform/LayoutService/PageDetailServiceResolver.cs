using Project.AmbujaCement.Website.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class PageDetailServiceResolver : RenderingContentsResolver
    {
        private readonly IPageDetailsService _rootResolver;

        public PageDetailServiceResolver(IPageDetailsService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetDetails(rendering);

        }
    }
}