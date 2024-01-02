using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.LayoutService
{
    public class HomeMainBannerContentResolver : RenderingContentsResolver
    {
        private readonly ICommonComponents RootResolver;

        public HomeMainBannerContentResolver(ICommonComponents bannercarouselData)
        {
            this.RootResolver = bannercarouselData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetHomeMainBanner(rendering);
        }
    }
}