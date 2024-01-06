using Sitecore.Pipelines;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.APMUL.Website.Pipelines
{
    public class RegisterWebApiRoutes
    {
        public RegisterWebApiRoutes()
        {
        }

        public void Process(PipelineArgs args)
        {
            RouteCollectionExtensions.MapRoute(RouteTable.Routes, "Sitecore.APMUL.Website.Pipelines", "api/APMUL/{action}", new { controller = "APMUL" });
        }
    }
}