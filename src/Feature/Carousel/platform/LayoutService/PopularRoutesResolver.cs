using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Shell.Applications.ContentEditor;
using System;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class PopularRoutesResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IPopularFlightRoutes popularroutesData;

        public PopularRoutesResolver(IPopularFlightRoutes popularroutesData)
        {
            this.popularroutesData = popularroutesData;
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
            return this.popularroutesData.PopularFlightData(rendering,datasource);

        }
    
    }
}