using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.WesternTransAlipurdar.Website.Pipelines
{
    public class RegisterWebApiRoutesas
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.WesternTransAlipurdar.Website.Pipelines", "api/WesternTransAlipurdarCompoment/{action}", new
            {
                controller = "WesternTransAlipurdarCompoment"
            });
        }
    }
}