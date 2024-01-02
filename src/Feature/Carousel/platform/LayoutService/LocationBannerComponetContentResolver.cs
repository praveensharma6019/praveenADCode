﻿using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Realty.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Realty.Feature.Carousel.Platform.LayoutService
{
    public class LocationBannerComponetContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IHeroCarouselService heroCarouselService;

        public LocationBannerComponetContentResolver(IHeroCarouselService heroCarouselService)
        {
            this.heroCarouselService = heroCarouselService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
             return heroCarouselService.GetLocationBanner(rendering);
         
        }

    }
}