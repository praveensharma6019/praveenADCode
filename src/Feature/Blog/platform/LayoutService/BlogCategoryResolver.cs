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

    public class BlogCategoryResolver : RenderingContentsResolver
    {
        private readonly IBlogCategoryService blogCategoryService;
        public BlogCategoryResolver(IBlogCategoryService blogCategoryService)
        {
            this.blogCategoryService = blogCategoryService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string categoryId = Sitecore.Context.Request?.QueryString["catName"];
            int pageNo = !string.IsNullOrWhiteSpace(Sitecore.Context.Request?.QueryString["pageNo"]) ? Convert.ToInt32(Sitecore.Context.Request?.QueryString["pageNo"]) : 0;
            return blogCategoryService.GetBlogCategoryData(rendering, categoryId, pageNo);

        }
    }
}