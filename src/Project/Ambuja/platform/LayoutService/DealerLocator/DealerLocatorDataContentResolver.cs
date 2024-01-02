using Project.AmbujaCement.Website.Services.AboutUs;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.DealerLocator
{
    public class DealerLocatorDataContentResolver : RenderingContentsResolver
    {
        private readonly IDealerLocatorListService _productListService;

        public DealerLocatorDataContentResolver(IDealerLocatorListService productListService)
        {
            _productListService = productListService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _productListService.GetDealerLocatorData(rendering);
        }
    }
}