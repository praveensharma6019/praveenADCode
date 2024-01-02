using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project.AAHL.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Project.AAHL.Website.Infrastructure", "api/AAHLContactUs/submitform", new
            {
                controller = "AAHLContactUs",
                action = "Index"
            });
        }
    }
}