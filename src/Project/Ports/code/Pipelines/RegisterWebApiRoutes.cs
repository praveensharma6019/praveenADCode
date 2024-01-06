namespace Sitecore.Ports.Website.Pipelines
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Pipelines;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.Ports.Website.Pipelines", "api/ports/{action}", new
            {
                controller = "Ports"
            });

            RouteTable.Routes.MapRoute("Sitecore.Ports.Website.PortsGrievance", "api/PortsGrievance/{action}", new
            {
                controller = "PortsGrievance"
            });


        }
    }
}