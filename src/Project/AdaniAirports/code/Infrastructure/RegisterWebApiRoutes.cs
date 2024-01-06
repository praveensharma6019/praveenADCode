
namespace Sitecore.AdaniAirports.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.AdaniAirports.Website.Infrastructure", "api/AdaniAirports/{action}", new
            {
                controller = "AdaniAirports"
            });
        }
    }
}