namespace Sitecore.Electricity.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.Electricity.Website.Infrastructure", "api/electricity/{action}", new
            {
                controller = "Electricity"
            });
        }
    }
}