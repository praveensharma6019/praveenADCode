using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.BKRPL.Website.Pipelines
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args) => RouteCollectionExtensions.MapRoute(RouteTable.Routes, "Sitecore.BKRPL.Website.Pipelines", "api/BKRPL/{action}", (object)new
        {
            controller = "BKRPL"
        });
    }
}
