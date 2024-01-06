
namespace Sitecore.InspireBusinessPark.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.InspireBusinessPark.Website.Infrastructure", "api/InspireBusinessPark/{action}", new
            {
                controller = "InspireBusinessPark"
            });
        }
    }
}