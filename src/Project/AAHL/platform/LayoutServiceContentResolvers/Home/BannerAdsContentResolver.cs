using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.Home
{
    public class BannerAdsContentResolver : RenderingContentsResolver
    {
        private readonly IHomeService _homeService;

        public BannerAdsContentResolver(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _homeService.GetBannerAdsModel(rendering);
        }
    }
}