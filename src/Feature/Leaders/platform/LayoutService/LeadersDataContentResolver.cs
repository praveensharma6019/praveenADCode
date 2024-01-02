using Adani.SuperApp.Realty.Feature.Leaders.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Leaders.Platform.LayoutService
{
    public class LeadersDataContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        protected readonly ILeadersDataRootResolverService RootResolver;

        public LeadersDataContentResolver(ILeadersDataRootResolverService leadersDataService)
        {
            this.RootResolver = leadersDataService;
        }        
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetLeadersDataList(rendering);
        }
    }
}