using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.LayoutServices
{
    public class CustomDictionaryListResolver: Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICustomDictionaryList customDictionary;

        public CustomDictionaryListResolver(ICustomDictionaryList _customDictionary)
        {
            this.customDictionary = _customDictionary;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return this.customDictionary.GetDictionary(rendering);
        }
    }
}