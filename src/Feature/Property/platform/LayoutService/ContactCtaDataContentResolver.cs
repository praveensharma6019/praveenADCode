using Adani.SuperApp.Realty.Feature.Property.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.LayoutService
{
    public class ContactCtaDataContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IProperyDataBasicService properyDataBasicService;
        public ContactCtaDataContentResolver(IProperyDataBasicService properyDataBasicService)
        {
            this.properyDataBasicService = properyDataBasicService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return properyDataBasicService.GetContactCtaDataModel(rendering);

        }
    }
}