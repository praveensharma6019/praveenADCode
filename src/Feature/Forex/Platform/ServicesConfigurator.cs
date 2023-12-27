using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;


namespace Adani.SuperApp.Airport.Feature.Forex.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<Services.IImportantInformation, Services.ImportantInformation>();
        }
    }
}