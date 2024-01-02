using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Realty.Feature.Filters.Platform.Services;

namespace Adani.SuperApp.Realty.Feature.Filters.Platform.LayoutService
{
    public class PropertyFilterContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IFilterProductsService filterProductsService;

        public PropertyFilterContentResolver(IFilterProductsService filterProductsService)
        {
            this.filterProductsService = filterProductsService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return filterProductsService.GetLocationFilterData(rendering);
        }
    }
}