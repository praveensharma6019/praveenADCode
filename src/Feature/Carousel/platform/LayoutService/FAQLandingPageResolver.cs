using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class FAQLandingPageResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IFAQLandingPageService _faqLanding;
        public FAQLandingPageResolver(IFAQLandingPageService faqLandinjg)
        {
            this._faqLanding = faqLandinjg;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            Item contextItem = RenderingContext.Current.ContextItem;
            return _faqLanding.GetFAQData(rendering);
        }
    }
}