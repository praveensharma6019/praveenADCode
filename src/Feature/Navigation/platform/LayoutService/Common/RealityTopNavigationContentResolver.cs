using Adani.SuperApp.Realty.Feature.Navigation.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.LayoutService.Common
{
    public class RealityTopNavigationContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly INavigationRootResolver navigationRootResolver;

        public RealityTopNavigationContentResolver(INavigationRootResolver navigationRootResolver )
        {
            this.navigationRootResolver = navigationRootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return navigationRootResolver.GetTopnavigationList(rendering);
        }
    }
}