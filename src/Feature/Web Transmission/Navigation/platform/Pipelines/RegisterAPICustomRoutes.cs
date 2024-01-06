using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Pipelines
{
    public class RegisterAPICustomRoutes
    {
        public virtual void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("WebtransCustomApi",
               "WebtransApi/Email/SendEmail",
               new
               {
                   controller = "Email",
                   action = "SendEmail"
               });
        }
    }
}