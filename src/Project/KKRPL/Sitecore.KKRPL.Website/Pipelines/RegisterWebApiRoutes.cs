using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.KKRPL.Website.Pipelines
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args) => RouteCollectionExtensions.MapRoute(RouteTable.Routes, "Sitecore.KKRPL.Website.Pipelines", "api/KKRPL/{action}", (object)new
        {
            controller = "KKRPL"
        });
    }
}