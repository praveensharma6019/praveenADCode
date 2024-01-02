using System.Web.Http;
using Swashbuckle.Application;
using System.Linq;
using System.Web.Http.Description;


namespace Adani.SuperApp.Airport.Feature.Swagger
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.MultipleApiVersions(
                            (apiDesc, targetApiVersion) => ResolveVersionSupportByRouteConstraint(apiDesc, targetApiVersion),
                            (vc) =>
                            {
                                vc.Version("airport_layout_services", "Airport layout services API (you can switch to custom data API /swagger/docs/airport_custom_services)");
                                vc.Version("airport_custom_services", "Airport custom data API (you can switch to airport layout API /swagger/docs/airport_layout_services)");
                                vc.Version("sc", "Sitecore services (you can switch to Airport services API /swagger/docs/airport_custom_services)");
                            });
                        c.UseFullTypeNameInSchemaIds();
                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DisableValidator();
                    });
        }
        private static bool ResolveVersionSupportByRouteConstraint(ApiDescription apiDesc, string targetApiVersion)
        {
            if (apiDesc.Route.RouteTemplate.StartsWith("sitecore/api/layout/") && targetApiVersion == "airport_layout_services") return true;
            if (apiDesc.Route.RouteTemplate.StartsWith("api/") && targetApiVersion == "airport_custom_services") return true;
            if (apiDesc.Route.RouteTemplate.StartsWith("sitecore") && targetApiVersion == "sc") return true;
            return false;
        }
    }
}
