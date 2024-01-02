using Adani.SuperApp.Realty.Feature.Career.Platform.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Realty.Feature.Career.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
           serviceCollection.AddTransient<ICareerServices, CareerServices>();
           serviceCollection.AddTransient<IAboutCareerServices, AboutCareerServices>();
        }
    }
}