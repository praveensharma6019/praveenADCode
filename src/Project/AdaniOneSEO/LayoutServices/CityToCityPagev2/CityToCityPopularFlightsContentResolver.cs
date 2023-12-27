using Project.AdaniOneSEO.Website.Services.CityToCityPage;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.LayoutServices.CityToCityPagev2
{
    public class CityToCityPopularFlightsContentResolver : RenderingContentsResolver
    {
        private readonly ICityToCityPopularFlights _rootresolver;

        public CityToCityPopularFlightsContentResolver(ICityToCityPopularFlights rootResolver)
        {
            _rootresolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootresolver.GetPopularFlightsNew(rendering);
        }
    }
}