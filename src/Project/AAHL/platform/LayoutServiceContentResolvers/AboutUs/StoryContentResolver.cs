using Project.AAHL.Website.Services.AboutUs;
using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.AboutUs
{
    public class StoryContentResolver : RenderingContentsResolver
    {
        private readonly IAboutUsServices RootResolver;

        public StoryContentResolver(IAboutUsServices aboutUsServices)
        {
            this.RootResolver = aboutUsServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetStory(rendering);
        }
    }
}