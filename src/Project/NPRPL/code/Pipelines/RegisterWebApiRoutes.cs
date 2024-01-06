using Sitecore.Pipelines;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.NPRPL.Website.Pipelines
{
    public class RegisterWebApiRoutes
    {
        public RegisterWebApiRoutes()
        {
        }

        public void Process(PipelineArgs args)
        {
            RouteCollectionExtensions.MapRoute(RouteTable.Routes, "Sitecore.NPRPL.Website.Pipelines", "api/NPRPL/{action}", new { controller = "NPRPL" });
        }
    }
}