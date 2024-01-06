namespace Sitecore.BelvedereClubGurgaon.Website.Infrastructure
{
    using Sitecore.Pipelines;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.BelvedereClubGurgaon.Website.Infrastructure", "api/belvedereclubgurgaon/{action}", new
            {
                controller = "BelvedereClubGurgaon"
            });
        }
    }
}