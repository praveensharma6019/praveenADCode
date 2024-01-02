using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adani.BAU.AdaniUpdates.Project.Routes
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Adani.BAU.AdaniUpdates.Project.Routes", "api/adani-updates/search/{action}", new
            {
                Controller = "AdaniUpdatesSearchApi"
            });
        }
    }
}