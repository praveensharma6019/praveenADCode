using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.IHeaderBuilder, Services.HeaderBuilder>();
            serviceCollection.AddTransient<Services.INavigationRootResolver, Services.NavigationRootResolver>();
            serviceCollection.AddScoped<Services.IDutyFreeHeader, Services.DutyFreeHeader>();
            serviceCollection.AddTransient<Services.IHeaderDataList, Services.HeaderDataList>();
            serviceCollection.AddTransient<Services.IFooterDataList, Services.FooterDataList>();
        }
    }
}