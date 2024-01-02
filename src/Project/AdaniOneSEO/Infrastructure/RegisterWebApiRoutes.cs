using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project.AdaniOneSEO.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            //RouteTable.Routes.MapRoute("Project.AdaniOneSEO.Website.Infrastructure.GetFlightDetails", "flights/flight-to-goa", new
            //{
            //    controller = "AdanioneSEO",
            //    action = "GetAllFlights"
            //});

            //RouteTable.Routes.MapRoute("Project.AdaniOneSEO.Website.Infrastructure.GetAllFlightsToCity", "flights/all-flight-to-delhi", new
            //{
            //    controller = "AdanioneSEO",
            //    action = "GetAllFlightsToCity"
            //});

            RouteTable.Routes.MapRoute("Project.AdaniOneSEO.Website.Infrastructure.CreateSitecoreItem", "flights/create-sitecore-item", new
            {
                controller = "AdanioneSEO",
                action = "CreateSitecoreItem"
            });

            RouteTable.Routes.MapRoute("Project.AdaniOneSEO.Website.Infrastructure.GetFlightToCityByDate", "flights/all-flight-to-destination", new
            {
                controller = "AdanioneSEO",
                action = "GetFlightToCityByDate"
            });
        }
    }
}