namespace Sitecore.AdaniCare.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.AdaniCare.Website.Infrastructure", "api/AdaniCare/{action}", new
            {
                controller = "AdaniCare"
            });
        }
    }
}