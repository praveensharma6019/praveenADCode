using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.LayoutService
{
    public class VideoBannerContentResolver : RenderingContentsResolver
    {
        private readonly ICommonComponents RootResolver;

        public VideoBannerContentResolver(ICommonComponents videoBannerData)
        {
            this.RootResolver = videoBannerData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetVideoBanner(rendering);
        }
    }
}