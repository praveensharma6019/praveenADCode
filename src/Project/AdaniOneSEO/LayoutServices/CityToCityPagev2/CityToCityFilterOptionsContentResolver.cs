using Project.AdaniOneSEO.Website.Services.CityToCityPage;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.LayoutServices.CityToCityPagev2
{
    public class CityToCityFilterOptionsContentResolver : RenderingContentsResolver
    {
        private readonly ICityToCityFilterOptionService _cityToCityPageService;

        public CityToCityFilterOptionsContentResolver(ICityToCityFilterOptionService cityToCityPageService)
        {
            _cityToCityPageService = cityToCityPageService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _cityToCityPageService.GetFilterOptionCityToCityModel(rendering);
        }
    }
}