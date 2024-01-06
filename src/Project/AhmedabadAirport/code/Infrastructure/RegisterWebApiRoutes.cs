using Sitecore.Pipelines;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.AhmedabadAirport.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public RegisterWebApiRoutes()
        {
        }

        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.AhmedabadAirport.Website.Infrastructure", "api/AhmedabadAirport/{action}", new { controller = "AhmedabadAirport" });
        }
    }
}