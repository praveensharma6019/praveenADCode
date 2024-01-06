using Sitecore.Pipelines;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.MangaloreAirport.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public RegisterWebApiRoutes()
        {
        }

        public void Process(PipelineArgs args)
        {
            RouteCollectionExtensions.MapRoute(RouteTable.Routes, "Sitecore.MangaloreAirport.Website.Infrastructure", "api/MangaloreAirport/{action}", new { controller = "MangaloreAirport" });
        }
    }
}