namespace Sitecore.ElectricityNew.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.ElectricityNew.Website.Infrastructure", "api/electricitynew/{action}", new
            {
                controller = "ElectricityNew"
            });

            RouteTable.Routes.MapRoute("Sitecore.ElectricityNew.Website.Infrastructure.ITSRForm", "api/ITSRForm/{action}", new
            {
                controller = "ITSRForm"
            });
        }
    }
}