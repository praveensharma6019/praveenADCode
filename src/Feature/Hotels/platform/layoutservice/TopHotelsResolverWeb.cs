using Adani.SuperApp.Airport.Feature.Hotels.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.LayoutService
{
    public class TopHotelsResolverWeb : RenderingContentsResolver
    {
        private readonly ITopHotelsWebService _topHotelsWeb;
        public TopHotelsResolverWeb(ITopHotelsWebService topHotelsWeb)
        {
            this._topHotelsWeb = topHotelsWeb;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _topHotelsWeb.GetTopHotelsData(rendering);
        }
    }
}