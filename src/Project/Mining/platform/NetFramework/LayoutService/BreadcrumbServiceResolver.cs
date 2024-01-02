using Project.Mining.Website.Home;
using Project.Mining.Website.Services.Banner;
using Project.Mining.Website.Services.Header;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService
{
    public class BreadcrumbServiceResolver : RenderingContentsResolver
    {
        private readonly IBreadcrumbService _rootResolver;

        public BreadcrumbServiceResolver(IBreadcrumbService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return _rootResolver.GetBreadcrum(rendering);
                    
        }
    }
}