using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.LayoutService
{
    public class AirportListContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICityAirportsContent airportsData;

        public AirportListContentResolver(ICityAirportsContent airportsData)
        {
            this.airportsData = airportsData;
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
            string airportType = Sitecore.Context.Request.QueryString["airporttype"];
            string isdomestic = Sitecore.Context.Request.QueryString["isdomestic"]=="1"?"in":"all";
            string airportcode = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["airportcode"]) ? Sitecore.Context.Request.QueryString["airportcode"].ToLower() : null;
            return this.airportsData.GetAppAirportsData(datasource, airportType, isdomestic, airportcode);

        }

    }
}