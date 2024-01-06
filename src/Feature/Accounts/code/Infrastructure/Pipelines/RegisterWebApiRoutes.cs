namespace Sitecore.Feature.Accounts.Infrastructure.Pipelines
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Pipelines;

    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Feature.Accounts.Api", "api/accounts/{action}", new
            {
                controller = "Accounts"
            });
            RouteTable.Routes.MapRoute("Feature.Accounts.Api.AEMLChangeOfName", "api/AEMLChangeOfName/{action}", new
            {
                controller = "AEMLChangeOfName"
            });
            RouteTable.Routes.MapRoute("Feature.Accounts.Api.AEMLComplaint", "api/AEMLComplaint/{action}", new
            {
                controller = "AEMLComplaint"
            });
            RouteTable.Routes.MapRoute("Feature.Accounts.Api.AEMLNewConnection", "api/AEMLNewConnection/{action}", new
            {
                controller = "AEMLNewConnection"
            });
            RouteTable.Routes.MapRoute("Feature.Accounts.Api.AdaniGas", "api/AdaniGas/{action}", new
            {
                controller = "AdaniGas"
            });
            RouteTable.Routes.MapRoute("DefaultCaptchaRoute", "DefaultCaptcha/Generate", new
            {
                controller = "DefaultCaptcha",
                action = "Generate"
            });
            RouteTable.Routes.MapRoute("Feature.AccountsRevamp.Api", "api/accountsrevamp/{action}", new
            {
                controller = "AccountsRevamp"
            });
            RouteTable.Routes.MapRoute("Feature.AEMLRevampChangeOfName.Api", "api/aemlrevampchangeofname/{action}", new
            {
                controller = "AEMLRevampChangeOfName"
            });
            RouteTable.Routes.MapRoute("Feature.AEMLRevampComplaint.Api", "api/aemlrevampcomplaint/{action}", new
            {
                controller = "AEMLRevampComplaint"
            });
            RouteTable.Routes.MapRoute("Feature.AEMLUserDashboard.Api", "api/aemluserdashboard/{action}", new
            {
                controller = "AEMLUserDashboard"
            });
        }
    }
}