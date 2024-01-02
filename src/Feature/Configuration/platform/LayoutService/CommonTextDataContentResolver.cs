using Adani.SuperApp.Realty.Feature.Configuration.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.LayoutService
{
    public class CommonTextDataContentResolver : RenderingContentsResolver
    {
        private readonly ICommonDataService imagewithTextService;
        public CommonTextDataContentResolver(ICommonDataService imagewithTextService)
        {
            this.imagewithTextService = imagewithTextService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return imagewithTextService.GetCommonText(rendering);

        }
    }
}