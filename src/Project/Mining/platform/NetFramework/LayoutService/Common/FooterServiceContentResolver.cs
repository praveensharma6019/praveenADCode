using Project.Mining.Website.Services.Common;
using Project.Mining.Website.Services.ProjectListing;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.LayoutService.Common
{
    public class FooterServiceContentResolver : RenderingContentsResolver
    {
        private readonly IFooterService _rootResolver;

        public FooterServiceContentResolver(IFooterService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetFooterServiceComponent(rendering);

        }
    }
}