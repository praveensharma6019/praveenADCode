
namespace Sitecore.MiracleMile.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.MiracleMile.Website.Infrastructure", "api/MiracleMile/{action}", new
            {
                controller = "MiracleMile"
            });
        }
    }
}