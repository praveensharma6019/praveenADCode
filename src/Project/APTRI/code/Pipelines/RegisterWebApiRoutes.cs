namespace Sitecore.APTRI.Website.Pipelines
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Pipelines;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.APTRI.Website.Pipelines", "api/APTRI/{action}", new
            {                               
                controller = "APTRI"
            });


        }
    }
}