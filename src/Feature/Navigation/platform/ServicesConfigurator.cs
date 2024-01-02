using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.INavigationRootResolver, Services.NavigationRootResolver>();
            serviceCollection.AddTransient<Services.IFooterService, Services.FooterService>();
        }
    }
}