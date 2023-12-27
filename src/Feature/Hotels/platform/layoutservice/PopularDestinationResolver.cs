using Adani.SuperApp.Airport.Feature.Hotels.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.LayoutService
{
    public class PopularDestinationResolver : RenderingContentsResolver
    {
        private readonly IPopularDestinationService _popularDestination;
        public PopularDestinationResolver(IPopularDestinationService popularDestination)
        {
            this._popularDestination = popularDestination;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _popularDestination.GetPopularDestinationData(rendering);
        }
    }
}