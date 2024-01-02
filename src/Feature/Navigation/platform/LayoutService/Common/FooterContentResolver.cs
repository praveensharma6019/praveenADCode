
using Adani.SuperApp.Realty.Feature.Navigation.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.LayoutService.Common
{
    public class FooterContentResolver: RenderingContentsResolver
    {
        private readonly IFooterService footerService;
        public FooterContentResolver(IFooterService footerService)
        {
            this.footerService = footerService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string location = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Location");
            location = !string.IsNullOrEmpty(location) ? location.Replace("property-in-", "") : "";
            return footerService.GetFooterData(rendering , location);

        }
    }
}