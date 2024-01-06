
namespace Sitecore.AdaniSolar.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.AdaniSolar.Website.Infrastructure", "api/adanisolar/{action}", new
            {
                controller = "AdaniSolar"
            });
        }
    }
}