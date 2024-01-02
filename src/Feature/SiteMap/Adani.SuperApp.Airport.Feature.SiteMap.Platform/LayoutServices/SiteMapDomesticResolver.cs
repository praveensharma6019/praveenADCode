using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.SiteMap.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.SiteMap.Platform.LayoutServices
{
    public class SiteMapDomesticResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ISiteMap sitemapdata;

        public SiteMapDomesticResolver(ISiteMap sitemapdata)
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
            return this.sitemapdata.DomesticFlightsSitemapData(datasource);
        }
       
    }
}