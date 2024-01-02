using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.LayoutService
{
    public class LocationContentsResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        protected readonly ILocationRootResolverService RootResolver;

        public LocationContentsResolver(ILocationRootResolverService locationService)
        {
            this.RootResolver = locationService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetLocationDataList(rendering);
        }
    
    }
}