using Project.AmbujaCement.Website.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class BannerServiceResolver : RenderingContentsResolver
    {
        private readonly IBannerService _rootResolver;

        public BannerServiceResolver(IBannerService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetBanner(rendering);

        }
    }
}