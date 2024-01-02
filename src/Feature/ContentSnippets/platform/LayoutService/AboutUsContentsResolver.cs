using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.LayoutService
{
    public class AboutUsContentsResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        protected readonly IAboutUsRootResolverService RootResolver;

        public AboutUsContentsResolver(IAboutUsRootResolverService aboutUsService)
        {
            this.RootResolver = aboutUsService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetAboutUsDataList(rendering);
        }
    
    }
}