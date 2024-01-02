using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.LayoutService
{
    public class CityAirportListContentResolver : RenderingContentsResolver
    {
        private readonly ICityAirportsContent airportsData;

        private readonly ILogRepository logging;

        public CityAirportListContentResolver(ICityAirportsContent _airportsData, ILogRepository _logging)
        {
            this.airportsData = _airportsData;
            this.logging = _logging;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                //Get the datasource for the item
                var datasource = RenderingContext.Current.Rendering.Item;

                // Null Check for datasource
                if (datasource == null)
                {
                    this.logging.Info("CityAirportListResolver- DataSource is NULL");
                    throw new NullReferenceException();
                }

                return this.airportsData.GetCityAirports(datasource);

            }
            catch (Exception ex)
            {

                this.logging.Error("CityAirportListResolver Error"+ex.Message);
            }

            return null;
            
        }

    }
}