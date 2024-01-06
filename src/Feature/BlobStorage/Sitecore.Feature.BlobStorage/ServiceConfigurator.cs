using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Feature.BlobStorage.Services;

namespace Sitecore.Feature.BlobStorage
{
    public class ServiceConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IBlobStorageService, BlobStorageService>();
        }
    }
}