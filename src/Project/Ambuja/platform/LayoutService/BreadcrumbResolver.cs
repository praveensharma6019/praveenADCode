using Project.AmbujaCement.Website.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class BreadcrumbResolver : RenderingContentsResolver
    {
        private readonly IBreadCrumbService _rootResolver;

        public BreadcrumbResolver(IBreadCrumbService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetBreadcrumb(rendering);

        }
    }
}