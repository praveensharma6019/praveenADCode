using Project.AAHL.Website.Services.OurLeadership;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.OurLeadership
{
    public class OurLeadersContentResolver : RenderingContentsResolver
    {
        private readonly IOurLeadersServices RootResolver;

        public OurLeadersContentResolver(IOurLeadersServices ourLeadersServices)
        {
            this.RootResolver = ourLeadersServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetOurLeaders(rendering);
        }
    }
}