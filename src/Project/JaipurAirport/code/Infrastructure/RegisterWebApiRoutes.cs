using Sitecore.Pipelines;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.JaipurAirport.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public RegisterWebApiRoutes()
        {
        }

        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.JaipurAirport.Website.Infrastructure", "api/JaipurAirport/{action}", new { controller = "JaipurAirport" });
        }
    }
}