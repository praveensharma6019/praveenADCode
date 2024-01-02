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
    public class BlogSearchResolver : RenderingContentsResolver
    {
        private readonly IBlogSearchService blogSearchService;
        public BlogSearchResolver(IBlogSearchService blogSearchService)
        {
            this.blogSearchService = blogSearchService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string query = Sitecore.Context.Request?.QueryString["query"];
            //int pageNo = !string.IsNullOrWhiteSpace(Sitecore.Context.Request?.QueryString["pageNo"]) ? Convert.ToInt32(Sitecore.Context.Request?.QueryString["pageNo"]) : 0;
            return blogSearchService.GetBlogSearch(query);

        }
    }
}