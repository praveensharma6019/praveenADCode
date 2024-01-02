using Project.AAHL.Website.Services.AboutUs;
using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.Common
{
    public class BannerContentResolver : RenderingContentsResolver
    {
        private readonly IAboutUsServices RootResolver;

        public BannerContentResolver(IAboutUsServices bannercarouselData)
        {
            this.RootResolver = bannercarouselData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetBanner(rendering);
        }
    }
}