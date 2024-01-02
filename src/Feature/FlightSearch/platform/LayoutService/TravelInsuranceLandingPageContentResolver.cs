using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.LayoutService
{
    public class TravelInsuranceLandingPageContentResolver : RenderingContentsResolver
    {
        private readonly ITravelInsuranceLanding travelInsurance;

        public TravelInsuranceLandingPageContentResolver(ITravelInsuranceLanding travelInsurance)
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
            return this.travelInsurance.GetTravelInsuranceData(datasource);

        }
    }
}