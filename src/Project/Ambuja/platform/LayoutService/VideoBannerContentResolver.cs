using Project.AmbujaCement.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class VideoBannerContentResolver : RenderingContentsResolver
    {
        private readonly IHomeServices RootResolver;

        public VideoBannerContentResolver(IHomeServices videoBannerData)
        {
            this.RootResolver = videoBannerData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetVideoBanner(rendering);
        }
    }
}