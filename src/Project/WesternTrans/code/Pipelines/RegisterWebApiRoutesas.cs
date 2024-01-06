using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.WesternTrans.Website.Pipelines
{
    public class RegisterWebApiRoutesas
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.WesternTrans.Website.Pipelines", "api/WesternTransCompoment/{action}", new
            {
                controller = "WesternTransCompoment"
            });
        }
    }
}