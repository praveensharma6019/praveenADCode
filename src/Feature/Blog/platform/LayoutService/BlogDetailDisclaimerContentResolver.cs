using Adani.SuperApp.Realty.Feature.Blog.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Blog.Platform.LayoutService
{
    public class BlogDetailDisclaimerContentResolver : RenderingContentsResolver
    {
        private readonly IBlogSearchService blogSearchService;
        public BlogDetailDisclaimerContentResolver(IBlogSearchService blogSearchService)
        {
            this.blogSearchService = blogSearchService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return blogSearchService.GetBlogDisclaimer(rendering);

        }
    }
}