using Project.AAHL.Website.Services.AboutAHHL;
using Project.AAHL.Website.Services.AboutUs;
using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.AboutAHHL
{
    public class OurAirportContentResolver : RenderingContentsResolver
    {
        private readonly IAboutAHHLServices RootResolver;

        public OurAirportContentResolver(IAboutAHHLServices aboutAHHLServices)
        {
            this.RootResolver = aboutAHHLServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetOurAiport(rendering);
        }
    }
}