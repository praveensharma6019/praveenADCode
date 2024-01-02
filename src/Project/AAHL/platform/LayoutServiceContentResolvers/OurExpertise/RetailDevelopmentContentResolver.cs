using Project.AAHL.Website.Services.Common;
using Project.AAHL.Website.Services.OurLeadership;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.OurExpertise
{
    public class RetailDevelopmentContentResolver : RenderingContentsResolver
    {
        private readonly IOurExpertise RootResolver;

        public RetailDevelopmentContentResolver(IOurExpertise ourLeadersServices)
        {
            this.RootResolver = ourLeadersServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetRetailDevelopmentModel(rendering);
        }
    }
}