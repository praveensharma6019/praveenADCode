using Sitecore.Pipelines;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.GuwahatiAirport.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public RegisterWebApiRoutes()
        {
        }

        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.GuwahatiAirport.Website.Infrastructure", "api/GuwahatiAirport/{action}", new { controller = "GuwahatiAirport" });
        }
    }
}