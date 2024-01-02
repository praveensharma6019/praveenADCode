using Project.AAHL.Website.Services.GroundTransportation;
using Project.AAHL.Website.Services.OurLeadership;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.GroundTransportation
{
    public class GroundTransportationResolver : RenderingContentsResolver
    {
        private readonly IGroundTransportationServices RootResolver;

        public GroundTransportationResolver(IGroundTransportationServices groundTransportationServices)
        {
            this.RootResolver = groundTransportationServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetTransportation(rendering);
        }
    }
}