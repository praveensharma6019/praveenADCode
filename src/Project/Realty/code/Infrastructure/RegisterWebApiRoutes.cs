
namespace Sitecore.Realty.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.Realty.Website.Infrastructure", "api/realty/{action}", new
            {
                controller = "Realty"
            });
        }
    }
}