﻿using Adani.SuperApp.Realty.Feature.Sitemap.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Adani.SuperApp.Realty.Feature.Sitemap.Platform.LayoutService
{
    public class DynamicSitemapContentResolver : RenderingContentsResolver
    {
        protected readonly ISitemapRootResolverService RootResolver;

        public DynamicSitemapContentResolver(ISitemapRootResolverService sitemapService)
        {
            this.RootResolver = sitemapService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetDynamicSitemapData(rendering);
        }
    }
}