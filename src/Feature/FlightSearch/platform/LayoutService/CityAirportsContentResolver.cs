using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.LayoutService
{
    public class CityAirportsContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICityAirportsContent airportsData;

        public CityAirportsContentResolver(ICityAirportsContent airportsData)
        {
            this.airportsData = airportsData;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            //Get the datasource for the item
            var datasource = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
            string isdomestic = Sitecore.Context.Request.QueryString["isdomestic"] == "1" ? "in" : "all";

            return this.airportsData.GetCityAirportsData(datasource, isdomestic);

        }

    }
}