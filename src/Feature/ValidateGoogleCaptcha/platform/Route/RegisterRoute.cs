using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.Feature.ValidateGoogleCaptcha.Route
{
    public class RegisterRoute
    {
        public void Process(PipelineArgs args)
        {
            //RouteTable.Routes.MapRoute("Sitecore.Feature.ValidateGoogleCaptcha.Route", "api/captcha/verify", new
            //{
            //    controller = "ValidateCaptcha",
            //    action = "ValidateReCaptcha"
            //});
            
            //V2 Captcha Route
            RouteTable.Routes.MapRoute("CaptchaServiceApi",
               "api/ValidateCaptcha/ValidateReCaptcha",
               new
               {
                   controller = "ValidateCaptcha",
                   action = "ValidateReCaptcha"
               });

            //V3 Captcha Route
            RouteTable.Routes.MapRoute("CaptchaServiceApiV3",
               "api/ValidateCaptcha/ValidateReCaptchaV3",
               new
               {
                   controller = "ValidateCaptcha",
                   action = "ValidateReCaptchaV3"
               });

        }
    }
}