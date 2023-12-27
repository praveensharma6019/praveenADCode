using Project.AdaniOneSEO.Website.Services.CityToCityPage;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.LayoutServices.CityToCityPagev2
{
    public class AirportInformationContentResolver : RenderingContentsResolver
    {
        private readonly IAirportInformationService _rootresolver;

        public AirportInformationContentResolver(IAirportInformationService RootResolver)
        {
            _rootresolver = RootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootresolver.GetAirportInformationModel(rendering);
        }
    }
}