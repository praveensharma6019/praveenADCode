using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.Foundation.Email.code.Pipelines
{
    public class RegisterEmailAPIRoutes
    {
        public void Process(PipelineArgs args)
        {
            //RouteTable.Routes.MapRoute("Sitecore.Feature.Email.App_Config.Include.Feature", "api/Email/{action}", new
            //{
            //    controller = "Email"
            //});
            RouteTable.Routes.MapRoute("EmailServiceCustomeApi",
               "EmailApi/EmailService/SendEmail",
               new
               {
                   controller = "EmailService",
                   action = "SendEmail"
               });
        }
    }
}