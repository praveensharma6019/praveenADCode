using Project.AdaniOneSEO.Website.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.LayoutServices
{
    public class BannerWidgetLayoutService : RenderingContentsResolver
    {
        private readonly IBannerWidget RootResolver;

        public BannerWidgetLayoutService(IBannerWidget Services)
        {
            this.RootResolver = Services;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetBannerWidget(rendering);
        }
    }
}