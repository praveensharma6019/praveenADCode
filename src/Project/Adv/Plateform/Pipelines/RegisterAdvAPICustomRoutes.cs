using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adani.BAU.Adv.Project.pipelines
{
    public class RegisterAdvAPICustomRoutes
    {
        public virtual void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("OperatorListing",
               "api/SocialSubscribe/JoinSocialSubscribe",
               new
               {
                   controller = "SocialSubscribe",
                   action = "JoinSocialSubscribe"
               });
        }
    }
}