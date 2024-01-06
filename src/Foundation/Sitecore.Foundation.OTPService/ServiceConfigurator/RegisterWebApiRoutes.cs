using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Sitecore.Foundation.OTPService.ServiceConfigurator
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {

            RouteTable.Routes.MapRoute("GETOTPServiceCustomeApi",
              "api/OTPService/GetOTP",
              new
              {
                  controller = "OTPService",
                  action = "GetOTP"
              });

            RouteTable.Routes.MapRoute("VerifyOTPServiceCustomeApi",
             "api/OTPService/VerifyOTP",
             new
             {
                 controller = "OTPService",
                 action = "VerifyOTP"
             });
        }
    }
}