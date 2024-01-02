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
    public class AboutCityContentsResolver : RenderingContentsResolver
    {
        protected readonly IAboutCityRootResolverService RootResolver;

        public AboutCityContentsResolver(IAboutCityRootResolverService aboutCityService)
        {
            this.RootResolver = aboutCityService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetAboutCityDataList(rendering);
        }
    
    }
}