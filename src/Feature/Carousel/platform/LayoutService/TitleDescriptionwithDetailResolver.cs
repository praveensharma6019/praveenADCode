using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class TitleDescriptionwithDetailResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ITitleDescriptionWithDetailList DiscoverData;

        public TitleDescriptionwithDetailResolver(ITitleDescriptionWithDetailList discoverData)
        {
            this.DiscoverData = discoverData;
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
           
            return this.DiscoverData.ItemDetails(rendering, datasource);

        }
    }
}