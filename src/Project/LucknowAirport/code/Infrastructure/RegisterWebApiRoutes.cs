using Sitecore.Pipelines;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.LucknowAirport.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public RegisterWebApiRoutes()
        {
        }

        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.LucknowAirport.Website.Infrastructure", "api/LucknowAirport/{action}", new { controller = "LucknowAirport" });
        }
    }
}