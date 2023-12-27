using Project.AdaniOneSEO.Website.Services.FlightsToDestination;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.LayoutServices.FlightsToDestination
{
    public class PopularFlightLayoutService : RenderingContentsResolver
    {
        private readonly IPopularFlights _rootResolver;

        public PopularFlightLayoutService(IPopularFlights rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetPopularFlights(rendering);

        }
    }
}