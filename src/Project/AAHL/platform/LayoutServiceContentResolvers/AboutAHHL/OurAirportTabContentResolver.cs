using Project.AAHL.Website.Services.AboutAHHL;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.AboutAHHL
{
    public class OurAirportTabContentResolver : RenderingContentsResolver
    {
        private readonly IAboutAHHLServices RootResolver;

        public OurAirportTabContentResolver(IAboutAHHLServices aboutAHHLServices)
        {
            this.RootResolver = aboutAHHLServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetOurAiportTab(rendering);
        }
    }
}