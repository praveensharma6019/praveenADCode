using Project.AmbujaCement.Website.Services.AboutUs;
using Project.AmbujaCement.Website.Services.DealerResult;
using Project.AmbujaCement.Website.Services.FAQ;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.FAQ
{
    public class FAQContentResolver : RenderingContentsResolver
    {
        private readonly IFAQService _faqService;

        public FAQContentResolver(IFAQService FaqService)
        {
            _faqService = FaqService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _faqService.GetFAQModel(rendering);
        }
    }
}