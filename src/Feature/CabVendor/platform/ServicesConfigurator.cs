using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(VendorSearchController),
              typeof(VendorSearchController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(VendorSearchV2Controller),
              typeof(VendorSearchV2Controller)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(VendorFilterController),
              typeof(VendorFilterController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(VendorFilterV2Controller),
              typeof(VendorFilterV2Controller)));
            serviceCollection.AddScoped<Services.ICabCancellation, Services.CabCancellation>();
            serviceCollection.AddScoped<Services.ICabAirportsContent, Services.CabAirportsContent>();
            serviceCollection.AddScoped<Services.IFAQ, Services.FAQ>();
            serviceCollection.AddScoped<Services.ICabServiceTitleDescription, Services.CabServiceTitleDescription>();
        }
    }
}