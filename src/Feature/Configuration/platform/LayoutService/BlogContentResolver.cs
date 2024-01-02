using Adani.SuperApp.Realty.Feature.Configuration.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.LayoutService
{
    public class BlogContentResolver: RenderingContentsResolver
    {
        private readonly IBlogContentService blogContentService;
        public BlogContentResolver(IBlogContentService blogContentService)
        {
            this.blogContentService = blogContentService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return blogContentService.GetBlockContentData(rendering);

        }
    }
}