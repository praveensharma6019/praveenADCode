using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.Adani.Website
{
    public class RouteConfig
    {
        public RouteConfig()
        {
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            RouteCollectionExtensions.IgnoreRoute(routes, "{resource}.axd/{*pathInfo}");
            RouteCollectionExtensions.MapRoute(routes, "Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}