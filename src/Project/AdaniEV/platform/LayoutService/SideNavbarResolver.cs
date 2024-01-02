using Adani.EV.Project.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.LayoutService
{
    public class SideNavbarResolver : RenderingContentsResolver
    {
        private readonly ISideNavBarService RootResolver;

        public SideNavbarResolver(ISideNavBarService sideNavBarService)
        {
            this.RootResolver = sideNavBarService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetSideNavbarModel(rendering);
        }
    }
}