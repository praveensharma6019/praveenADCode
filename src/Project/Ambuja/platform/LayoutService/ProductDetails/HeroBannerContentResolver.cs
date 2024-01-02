using Project.AmbujaCement.Website.Services.AboutUs;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.ProductList
{
    public class HeroBannerContentResolver : RenderingContentsResolver
    {
        private readonly IProjectDetailsService _productDetailsService;

        public HeroBannerContentResolver(IProjectDetailsService productDetailsService)
        {
            _productDetailsService = productDetailsService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _productDetailsService.GetHeroBannerModel(rendering);
        }
    }
}