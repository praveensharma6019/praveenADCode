using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.LayoutServices
{
    public class CabAirportListContentResolver : RenderingContentsResolver
    {
        private readonly ICabAirportsContent airportsData;

        private readonly ILogRepository logging;

        public CabAirportListContentResolver(ICabAirportsContent _airportsData, ILogRepository _logging)
        {
            this.airportsData = _airportsData;
            this.logging = _logging;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                //Get the datasource for the item
                Item datasource = RenderingContext.Current.Rendering.Item;

                // Null Check for datasource
                if (datasource == null)
                {
                    this.logging.Info("CabAirportListResolver- DataSource is NULL");
                    throw new NullReferenceException();
                }

                return this.airportsData.GetCabAirports(datasource);

            }
            catch (Exception ex)
            {

                this.logging.Error("CabAirportListResolver Error" + ex.Message);
            }

            return null;

        }

    }
}