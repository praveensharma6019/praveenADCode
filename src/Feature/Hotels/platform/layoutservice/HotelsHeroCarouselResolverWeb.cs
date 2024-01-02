using Adani.SuperApp.Airport.Feature.Hotels.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.LayoutService
{
    public class HotelsHeroCarouselResolverWeb : RenderingContentsResolver
    {
        private readonly IHotelsHeroCarouselWeb _heroCarouselWeb;
        public HotelsHeroCarouselResolverWeb(IHotelsHeroCarouselWeb heroCarouselWeb)
        {
            this._heroCarouselWeb = heroCarouselWeb;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _heroCarouselWeb.GetHeroCarouselData(rendering);
        }
    }
}