using Sitecore.Pipelines;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.AREPRL.Website.Pipelines
{
    public class RegisterWebApiRoutes
    {
        public RegisterWebApiRoutes()
        {
        }

        public void Process(PipelineArgs args)
        {
            RouteCollectionExtensions.MapRoute(RouteTable.Routes, "Sitecore.AREPRL.Website.Pipelines", "api/AREPRL/{action}", new { controller = "AREPRL" });
        }
    }
}