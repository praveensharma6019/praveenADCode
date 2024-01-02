﻿using Adani.EV.Project.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.LayoutService
{
    public class EVNearBannerResolver : RenderingContentsResolver
    {
        private readonly IHomeService RootResolver;

        public EVNearBannerResolver(IHomeService homeService)
        {
            this.RootResolver = homeService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetEVNearBannerModelModel(rendering);

        }
    }
}