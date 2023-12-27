using Adani.SuperApp.Airport.Feature.Hotels.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.LayoutService
{
    public class TopHotelsResolverApp : RenderingContentsResolver
    {
        private readonly ITopHotelsAppService _topHotelsApp;
        public TopHotelsResolverApp(ITopHotelsAppService topHotelsApp)
        {
            this._topHotelsApp = topHotelsApp;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _topHotelsApp.GetTopHotelsData(rendering);
        }
    }
}