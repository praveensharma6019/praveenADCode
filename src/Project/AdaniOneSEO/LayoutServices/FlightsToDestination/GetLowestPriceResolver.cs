using Project.AdaniOneSEO.Website.Services.FlightsToDestination;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.LayoutServices.FlightsToDestination
{
    public class GetLowestPriceResolver : RenderingContentsResolver
    {
        private readonly IGetLowestPrice rootResolver;

        public GetLowestPriceResolver(IGetLowestPrice getLowestPrice)
        {
            this.rootResolver = getLowestPrice;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetLowestPriceData(rendering);
        }
    }
}