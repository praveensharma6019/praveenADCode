namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Pipelines
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Pipelines;

    public class RegisterWebApiRoutes
    {
        public virtual void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Feature.StaticPages.Api", "api/sitecore/feedbackform/{action}", new
            {
                controller = "FeedbackForm"
            });
            RouteTable.Routes.MapRoute("Feature.Forms.Api", "sitecore/api/PNRForm/{action}", new
            {
                controller = "PNRForm"
            });
            RouteTable.Routes.MapRoute("Feature.CallBackForms.Api", "sitecore/api/CallBackForm/{action}", new
            {
                controller = "CallBackForm"
            });
        }
    }
}