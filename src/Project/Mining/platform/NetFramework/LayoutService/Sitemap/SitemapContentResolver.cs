using Project.Mining.Website.Services.Banner;
using Project.Mining.Website.Services.Sitemap;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.LayoutService.Sitemap
{
    public class SitemapContentResolver : RenderingContentsResolver
    {
        private readonly ISitemapService _rootResolver;

        public SitemapContentResolver(ISitemapService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetSitemapData(rendering);

        }
    }
}