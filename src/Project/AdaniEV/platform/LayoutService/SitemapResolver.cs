using Adani.EV.Project.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.LayoutService
{
    public class SitemapResolver : RenderingContentsResolver
    {
        private readonly ISitemapService RootResolver;

        public SitemapResolver(ISitemapService sitemapService)
        {
            this.RootResolver = sitemapService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetSitemapModel(rendering);
        }
    }
}