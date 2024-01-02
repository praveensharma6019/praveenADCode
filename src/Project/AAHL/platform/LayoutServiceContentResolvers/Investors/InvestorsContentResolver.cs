using Project.AAHL.Website.Services.GroundTransportation;
using Project.AAHL.Website.Services.Investors;
using Project.AAHL.Website.Services.OurLeadership;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.Investors
{
    public class InvestorsContentResolver : RenderingContentsResolver
    {
        private readonly IInvestorsServices RootResolver;

        public InvestorsContentResolver(IInvestorsServices investorsServices)
        {
            this.RootResolver = investorsServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetInvestors(rendering);
        }
    }
}