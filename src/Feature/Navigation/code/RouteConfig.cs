//namespace Sitecore.Feature.Navigation

//{
//    using System;
//    using System.Collections.Generic;
//    using System.Web.Mvc;
//    using System.Web.Routing;

//    //public static class RouteConfig
//    //{
//    //    public static void RegisterRoutes(RouteCollection routes)
//    //    {
//    //        //routes.MapRoute("search-togglefacet", "api/feature/search/togglefacet", new { controller = "Search", action = "ToggleFacet", id = UrlParameter.Optional });
//    //        //routes.MapRoute("search-ajaxsearch", "api/feature/search/ajaxsearch", new { controller = "Search", action = "AjaxSearchResults", id = UrlParameter.Optional });

//    //        routes.MapRoute("Navigation", "Navigation/GetCustomerCare", new { controller = "Navigation", action = "GetCustomerCare" });
//    //        //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
//    //        //RouteTable.Routes.MapRoute("Navigation", "Navigation/GetCustomerCare", new { controller = "Navigation", action = "GetCustomerCare" });

//    //    }
//    //}


//    public static class RouteConfig
//    {
//        public static void RegisterRoutes(RouteCollection routes)
//        {
//            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
//            RouteTable.Routes.MapRoute("Navigationt", "api/feature/Navigation/GetCustomerCare", new { controller = "Navigation", action = "GetCustomerCare", id = UrlParameter.Optional });
//        }
//    }

//    //public class RegisterCustomRoute
//    //{
//    //    public virtual void Process(Pipelines.PipelineArgs args)
//    //    {
//    //        RouteTable.Routes.MapMvcAttributeRoutes();
//    //        //RouteTable.Routes.MapRoute("Navigation", "Navigation/GetCustomerCare", new { controller = "Navigation", action = "GetCustomerCare" });
//    //        RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
//    //        RouteTable.Routes.MapRoute("Navigationt", "api/feature/Navigation/GetCustomerCare", new { controller = "Navigation", action = "GetCustomerCare", id = UrlParameter.Optional });

           

//    //    }
//    //}
//}


namespace Sitecore.Feature.Navigation
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           
            routes.MapRoute("Navigationt", "api/feature/Navigation/GetCustomerCare", new { controller = "Navigation", action = "GetCustomerCare", id = UrlParameter.Optional });

        }
    }
}