using Project.AmbujaCement.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class MainBannerContentResolver : RenderingContentsResolver
    {
        private readonly IHomeServices RootResolver;

        public MainBannerContentResolver(IHomeServices mainBannerData)
        {
            this.RootResolver = mainBannerData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetMainBanner(rendering);
        }
    }
}