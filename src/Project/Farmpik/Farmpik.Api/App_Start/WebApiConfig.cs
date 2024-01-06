/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Sitecore.Farmpik.Api.Website.Filters;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Sitecore.Farmpik.Api.Website
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //var cors = new EnableCorsAttribute(ConfigurationManager.AppSettings["CorsOrigins"], "*", "*");
            config.EnableCors();
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new ValidateModelAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

           // var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
           // json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}