namespace Sitecore.AGELPortal.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            /*RouteTable.Routes.MapRoute("Sitecore.AGELPortal.Website.Infrastructure", "api/AzureAd/{action}", new
            {
                controller = "AzureAd"
            });*/

            RouteTable.Routes.MapRoute("Sitecore.AGELPortal.Website.Infrastructure", "api/AGELPortal/{action}", new
            {
                controller = "AGELPortal"
            });
        }
    }
}