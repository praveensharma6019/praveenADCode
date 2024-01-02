using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.LayoutService
{
    public class AboutAdaniRealtyContentsResolver :Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        protected readonly IAboutAdaniRealtyRootResolverService RootResolver;

        public AboutAdaniRealtyContentsResolver(IAboutAdaniRealtyRootResolverService aboutAdaniRealtyService)
        {
            this.RootResolver = aboutAdaniRealtyService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetAboutAdaniRealtyDataList(rendering);
        }
    }
}