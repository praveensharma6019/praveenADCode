using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.LayoutService
{
    public class FeaturedBlogRootResolver : RenderingContentsResolver
    {
        protected readonly IFeaturedBlogRootResolverService RootResolver;

        public FeaturedBlogRootResolver(IFeaturedBlogRootResolverService featuredBlogService)
        {
            this.RootResolver = featuredBlogService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetFeaturedBlogDataList(rendering);
        }
    }
}