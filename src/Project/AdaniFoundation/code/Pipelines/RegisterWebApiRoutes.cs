using Sitecore.Pipelines;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.AdaniFoundation.Website.Pipelines
{
    public class RegisterWebApiRoutes
    {
        public RegisterWebApiRoutes()
        {
        }

        public void Process(PipelineArgs args)
        {
            RouteCollectionExtensions.MapRoute(RouteTable.Routes, "Sitecore.AdaniFoundation.Website.Pipelines", "api/AdaniFoundation/{action}", new { controller = "AdaniFoundation" });
        }
    }
}