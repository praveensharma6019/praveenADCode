using Adani.SuperApp.Realty.Feature.Faq.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Faq.Platform.LayoutService
{
  
        public class QuickLinksFaqResolver : RenderingContentsResolver
        {
            private readonly IQuickLinksFaqService quickLinksFaqService;
            public QuickLinksFaqResolver(IQuickLinksFaqService quickLinksFaqService)
            {
                this.quickLinksFaqService = quickLinksFaqService;
            }
            public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
            {
                return quickLinksFaqService.GetQuickLinksFaqData(rendering);

            }
        }
}