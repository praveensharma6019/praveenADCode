using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.LayoutServices
{
    public class CustomDictionaryResolver: Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICustomDictionary customDictionary;

        public CustomDictionaryResolver(ICustomDictionary _customDictionary)
        {
            this.customDictionary = _customDictionary;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return this.customDictionary.GetDictionary(rendering);
        }
    }
}