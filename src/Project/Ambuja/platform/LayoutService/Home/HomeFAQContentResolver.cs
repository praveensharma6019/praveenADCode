using Project.AmbujaCement.Website.Services.Home;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService
{
    public class HomeFAQContentResolver : RenderingContentsResolver
    {
        private readonly IHomeServices RootResolver;

        public HomeFAQContentResolver(IHomeServices homeFAQData)
        {
            this.RootResolver = homeFAQData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetHomeFAQ(rendering);
        }
    }
}