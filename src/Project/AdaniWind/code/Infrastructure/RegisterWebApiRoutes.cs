using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.AdaniWind.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.AdaniWind.Website.Infrastructure", "api/AdaniWind/{action}", new
            {
                controller = "AdaniWind"
            });
        }
    }
}