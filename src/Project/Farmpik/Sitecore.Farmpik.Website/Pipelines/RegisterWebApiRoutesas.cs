using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.Farmpik.Website.Pipelines
{
    public class RegisterWebApiRoutesas
    {
        public void Process(PipelineArgs args)
        {
            //RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.Pipelines.RegisterWebApiRoutes", "api/FarmpikLogin/{action}", new
            //{
            //    controller = "FarmpikLogin"
            //});

            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.RegisterWebRoutes.Login",
             "api/FarmpikLogin/Login",
             new
             {
                 controller = "FarmpikLogin",
                 action = "Login"
             });

            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.RegisterWebRoutes.SignOut",
             "Login/SignOut",
             new
             {
                 controller = "FarmpikLogin",
                 action = "SignOut"
             });


            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.FarmpikDashboard.Index",
             "api/Dashboard/Index",
             new
             {
                 controller = "FarmpikDashboard",
                 action = "Index"
             });


            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.FarmpikTemplate.DownloadTemplate",
            "api/FarmpikTemplate/DownloadTemplate",
              new
              {
                  controller = "FarmpikTemplate",
                  action = "DownloadTemplate"
              });

            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.FarmpikDashboard.ImportTemplate",
             "api/FarmpikDashboard/ImportTemplate",
             new
             {
                 controller = "FarmpikDashboard",
                 action = "ImportTemplate"
             });
            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.ReportTo.ReportToDetail",
          "api/ReportToDetail/csp-endpoint",
          new
          {
              controller = "ReportTo",
              action = "csp-endpoint"
          });


            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.FarmpikDashboard.InsertContact",
               "api/FarmpikContactUs/InsertContact",
               new
               {
                   controller = "Farmpik",
                   action = "InsertContact"
               });

            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.Farmpik.InsertContact",
            "api/Farmpik/InsertContact",
            new
            {
                controller = "Farmpik",
                action = "InsertContact"
            });

            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.FarmpikActivity.GetRecentActivity",
            "api/FarmpikActivity/GetRecentActivity",
            new
            {
                controller = "FarmpikActivity",
                action = "GetRecentActivity"
            });

            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Website.RegisterWebRoutes.GetKnowledgeHubTabs",
             "v1/api/Farmpik/GetKnowledgeHubTabs",
             new
             {
                 controller = "FarmpikActivity",
                 action = "GetKnowledgeHubTabs"
             });
        }
    }
}