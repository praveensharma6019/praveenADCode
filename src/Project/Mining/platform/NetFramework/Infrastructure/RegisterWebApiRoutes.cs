using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project.Mining.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Project.Mining.Website.Infrastructure.requestcallbackform", "formsapi/SitecoreForms/requestcallbackform", new
            {
                controller = "SitecoreForms",
                action = "GetRequestCallBackForm"
            });
            RouteTable.Routes.MapRoute("Project.Mining.Website.Infrastructure.ContactUsForm", "formsapi/SitecoreForms/ContactUsForm", new
            {
                controller = "SitecoreForms",
                action = "GetContactForm"
            });
            RouteTable.Routes.MapRoute("Project.Mining.Website.Infrastructure.Subscribeform", "formsapi/SitecoreForms/Subscribeform", new
            {
                controller = "SitecoreForms",
                action = "GetSubscribeForm"
            });
        }
    }
}