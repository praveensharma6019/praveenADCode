
namespace Sitecore.AdaniConneX.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.AdaniConneX.Website.Infrastructure", "api/AdaniConnex/{action}", new
            {
                controller = "AdaniConnex"
            });
        }
    }
}