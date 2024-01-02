using Adani.SuperApp.Realty.Feature.Navigation.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.LayoutService.Common
{
    public class EnquiryFormContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly INavigationRootResolver navigationRootResolver;

        public EnquiryFormContentResolver(INavigationRootResolver navigationRootResolver )
        {
            this.navigationRootResolver = navigationRootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return navigationRootResolver.GetEnquiryItem(rendering);
        }
    }
}