using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Realty.Feature.Faq.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.Faq.Platform.LayoutService
{
    public class LocationPageFaqListContentResolver : RenderingContentsResolver
    {
        private readonly IFaqService faqService;
        public LocationPageFaqListContentResolver(IFaqService faqService)
        {
            this.faqService = faqService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return faqService.LocationPageFaqList(rendering);

        }

    }
}