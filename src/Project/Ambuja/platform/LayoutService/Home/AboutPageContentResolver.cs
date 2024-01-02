using Project.AmbujaCement.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class AboutPageContentResolver : RenderingContentsResolver
    {
        private readonly IHomeServices RootResolver;

        public AboutPageContentResolver(IHomeServices aboutPageData)
        {
            this.RootResolver = aboutPageData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetAboutPage(rendering);
        }
    }
}