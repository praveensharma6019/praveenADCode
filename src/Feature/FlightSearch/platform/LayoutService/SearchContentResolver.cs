using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.LayoutService
{
    public class SearchContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IFlightSearch flightSearch;

        public SearchContentResolver(IFlightSearch flightSearch)
        {
            this.flightSearch = flightSearch;
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
            return this.flightSearch.GetBookFlightWidgetData(datasource);
           
        }

    }
}