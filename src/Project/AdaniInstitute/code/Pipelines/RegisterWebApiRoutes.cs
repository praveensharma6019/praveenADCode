namespace Sitecore.AdaniInstitute.Website.Pipelines
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Pipelines;

    public class RegisterWebApiRoutes
  {
    public void Process(PipelineArgs args)
    {
     RouteTable.Routes.MapRoute("Sitecore.AdaniInstitute.Website.Pipelines", "api/AdaniInstitute/{action}", new
            {
                controller = "AdaniInstitute"
     });


        }
  }
}