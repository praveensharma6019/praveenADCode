using Project.AmbujaCement.Website.Services.AboutUs;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.ProductList
{
    public class ProductListContentResolver : RenderingContentsResolver
    {
        private readonly IProductListService _productListService;

        public ProductListContentResolver(IProductListService productListService)
        {
            _productListService = productListService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _productListService.GetProductListModel(rendering);
        }
    }
}