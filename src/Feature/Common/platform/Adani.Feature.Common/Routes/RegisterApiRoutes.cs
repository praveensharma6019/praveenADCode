using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adani.Feature.Common.Routes
{
    public class RegisterApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Adani.Feature.Common.Routes.OTP", "api/otp/{action}", new
            {
                Controller = "OtpApi"
            });

            RouteTable.Routes.MapRoute("Adani.Feature.Common.Routes.Email", "api/email/{action}", new
            {
                Controller = "SendEmail"
            });
        }
    }
}