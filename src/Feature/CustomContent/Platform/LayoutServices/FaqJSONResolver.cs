using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.LayoutServices
{
    public class FaqJSONResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IFaqJSON faqjson;

        public FaqJSONResolver(IFaqJSON _faqjson)
        {
            this.faqjson = _faqjson;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {                    
            return faqjson.GetFAQJSONList(rendering);
        }
    }
}