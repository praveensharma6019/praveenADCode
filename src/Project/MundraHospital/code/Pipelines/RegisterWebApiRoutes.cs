namespace Sitecore.MundraHospital.Website.Pipelines
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Pipelines;

    public class RegisterWebApiRoutes
  {
    public void Process(PipelineArgs args)
    {
     RouteTable.Routes.MapRoute("Sitecore.MundraHospital.Website.Pipelines", "api/mundrahospital/{action}", new
            {
                controller = "MundraHospital"
            });


        }
  }
}