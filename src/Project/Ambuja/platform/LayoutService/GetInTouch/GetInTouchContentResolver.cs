using Project.AmbujaCement.Website.Services.GetInTouch;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.GetInTouch
{
    public class GetInTouchContentResolver : RenderingContentsResolver
    {
        private readonly IGetInTouchService _getInTouchService;

        public GetInTouchContentResolver(IGetInTouchService getInTouchService)
        {
            _getInTouchService = getInTouchService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _getInTouchService.GetGetInTouchDetailsModel(rendering);
        }
    }
}