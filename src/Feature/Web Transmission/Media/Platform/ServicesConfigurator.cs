using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.BAU.Transmission.Feature.Media.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.IServiceList, Services.ServiceList>();
            serviceCollection.AddScoped<Services.ICustomContent, Services.CustomContent>();
        }
    }
}