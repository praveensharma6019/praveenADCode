using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.Media.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.IServiceList, Services.ServiceList>();
            serviceCollection.AddScoped<Services.ICustomContent, Services.CustomContent>();
            serviceCollection.AddTransient<Services.IServiceListWeb, Services.ServiceListWeb>();
        }
    }
}