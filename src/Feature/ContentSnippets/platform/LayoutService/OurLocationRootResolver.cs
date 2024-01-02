﻿using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.LayoutService
{
    public class OurLocationRootResolver : RenderingContentsResolver
    {
        protected readonly IOurLocationRootResolverService RootResolver;

        public OurLocationRootResolver(IOurLocationRootResolverService locationService)
        {
            this.RootResolver = locationService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetOurLocationDataList(rendering);
        }
    }
}