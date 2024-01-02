using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class BaseCarouselResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IBaseCarousel businessData;

        public BaseCarouselResolver(IBaseCarousel ourbusinessData)
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
            return this.businessData.BaseCarouselData(rendering, datasource);

        }
    }
}