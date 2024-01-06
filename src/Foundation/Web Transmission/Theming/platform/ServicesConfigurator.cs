using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.BAU.Transmission.Foundation.Theming.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<Services.IWidgetService, Services.WidgetService>();
        }
    }
}