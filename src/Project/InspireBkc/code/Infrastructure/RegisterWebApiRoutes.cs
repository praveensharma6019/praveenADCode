using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;


namespace Sitecore.InspireBkc.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteCollectionExtensions.MapRoute(RouteTable.Routes, "Sitecore.InspireBkc.Website.Infrastructure", "api/InspireBkc/{action}", (object)new
            {
                controller = "InspireBkc"
            });
        }
    }
}
