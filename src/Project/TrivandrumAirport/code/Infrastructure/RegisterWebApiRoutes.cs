using Sitecore.Pipelines;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.TrivandrumAirport.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public RegisterWebApiRoutes()
        {
        }

        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.TrivandrumAirport.Website.Infrastructure", "api/TrivandrumAirport/{action}", new { controller = "TrivandrumAirport" });
        }
    }
}