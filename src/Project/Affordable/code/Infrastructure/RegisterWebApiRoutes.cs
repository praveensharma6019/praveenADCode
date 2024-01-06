namespace Sitecore.Affordable.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.Affordable.Website.Infrastructure", "api/Affordable/{action}", new
            {
                controller = "Affordable"
            });
        }
    }
}