using Project.Mining.Website.Home;
using Project.Mining.Website.Services.Banner;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService
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
          return _rootResolver.GetBannerModel(rendering);
                    
        }
    }
}