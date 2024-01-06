namespace Sitecore.BelvedereClubAhmedabad.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.BelvedereClubAhmedabad.Website.Infrastructure", "api/belvedereclubahmedabad/{action}", new
            {
                controller = "BelvedereClubAhmedabad"
            });
        }
    }
}