using Adani.SuperApp.Realty.Feature.Configuration.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.LayoutService
{
    public class CommonDataContentResolver : RenderingContentsResolver
    {
        private readonly ICommonDataService commonDataService;
        public CommonDataContentResolver(ICommonDataService commonDataService)
        {
            this.commonDataService = commonDataService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return commonDataService.GetCommonData(rendering);

        }
    }
}