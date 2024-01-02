using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
namespace Adani.SuperApp.Realty.Feature.Widget.Platform
{
    public class ServiceConfigurator:IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.IWidgetService, Services.WidgetService>();
            serviceCollection.AddTransient<Services.IJobDetailService, Services.JobDetailService>();
        }
    }
}