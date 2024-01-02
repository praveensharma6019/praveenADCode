using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;



namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.LayoutService
{
    public class FlightListResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IFlightList flightList;

        public FlightListResolver(IFlightList flightList)
        {
            this.flightList = flightList;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["AirlineID"])? Sitecore.Context.Request.QueryString["AirlineID"].ToLower():"";
            string fileds = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["fields"])? Sitecore.Context.Request.QueryString["fields"].ToLower(): "all";
            string airlineType = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["airlinetype"])? Sitecore.Context.Request.QueryString["airlinetype"].ToLower(): "";
            string contextDB = Sitecore.Context.Database.Name;
            return this.flightList.GetFlightListData(queryString, fileds, airlineType, contextDB);
        }
    }
}