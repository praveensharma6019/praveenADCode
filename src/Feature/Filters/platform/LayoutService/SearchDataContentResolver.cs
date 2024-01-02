using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Realty.Feature.Filters.Platform.Services;

namespace Adani.SuperApp.Realty.Feature.Filters.Platform.LayoutService
{
    public class SearchDataContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IFilterProductsService filterProductsService;

        public SearchDataContentResolver(IFilterProductsService filterProductsService)
        {
            this.filterProductsService = filterProductsService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return filterProductsService.GetSearchDataList(rendering);
        }
    }
}