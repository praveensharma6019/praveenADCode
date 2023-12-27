using Adani.SuperApp.Airport.Feature.SiteMap.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.SiteMap.Platform.LayoutServices
{
    public class SiteMapInternationalResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ISiteMap sitemapdata;

        public SiteMapInternationalResolver(ISiteMap sitemapdata)
        {
            this.sitemapdata = sitemapdata;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var datasource = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            return this.sitemapdata.InternationalFlightsSitemapData(datasource);
        }
    }
}