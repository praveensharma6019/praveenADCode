using Project.AmbujaCement.Website.Services.AboutUs;
using Project.AmbujaCement.Website.Services.DealerResult;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.DealerResult
{
    public class DealerResultContentResolver : RenderingContentsResolver
    {
        private readonly IDealerResultService _dealerresultService;

        public DealerResultContentResolver(IDealerResultService resultService)
        {
            _dealerresultService = resultService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _dealerresultService.GetDealerResultModel(rendering);
        }
    }
}