using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project.AmbujaCement.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Project.AmbujaCement.Website.Controllers", "formsapi/GetInTouch/submitform", new
            {
                controller = "GetInTouch",
                action = "Index"
            });

            RouteTable.Routes.MapRoute("Project.AmbujaCement.Website.Controllers.VerifyOtp", "formsapi/GetInTouch/verifyotp", new
            {
                controller = "GetInTouch",
                action = "VerifyOtp"
            });

            RouteTable.Routes.MapRoute("Project.AmbujaCement.Website.Infrastructure.DealersRoute", "dealersapi/dealers/{stateName}/{cityName}/{areaName}", new
            {
                controller = "Ambuja",
                action= "GetDealers",
                cityName = UrlParameter.Optional,
                areaName = UrlParameter.Optional
            });
        }
    }
}