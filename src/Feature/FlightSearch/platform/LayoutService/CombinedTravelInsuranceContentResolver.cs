using System;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.LayoutService
{
    public class CombinedTravelInsuranceContentResolver : RenderingContentsResolver
    {
        private readonly ICombinedTravelInsurance travelInsurance;

        public CombinedTravelInsuranceContentResolver(ICombinedTravelInsurance travelInsurance)
        {
            this.travelInsurance = travelInsurance;
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
            return this.travelInsurance.GetCombinedTravelInsuranceData(datasource);

        }
    }
}