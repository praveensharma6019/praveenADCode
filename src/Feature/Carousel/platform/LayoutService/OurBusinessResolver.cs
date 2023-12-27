using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class OurBusinessResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IOurBusiness businessData;

        public OurBusinessResolver(IOurBusiness ourbusinessData)
        {
            this.businessData = ourbusinessData;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            //Get the datasource for the item
            var datasource = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }

            //  string airportcode = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["airportcode"]) ? Sitecore.Context.Request.QueryString["airportcode"].ToLower() : null;
            return this.businessData.OurBusinessData(rendering, datasource);

        }
    }
}