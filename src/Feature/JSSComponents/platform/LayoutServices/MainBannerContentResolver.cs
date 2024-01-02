using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.LayoutService
{
    public class MainBannerContentResolver : RenderingContentsResolver
    {
        private readonly ICommonComponents RootResolver;

        public MainBannerContentResolver(ICommonComponents mainBannerData)
        {
            this.RootResolver = mainBannerData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetMainBanner(rendering);
        }
    }
}