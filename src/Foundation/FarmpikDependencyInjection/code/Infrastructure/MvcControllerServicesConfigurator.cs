namespace Sitecore.Foundation.Farmpik.DependencyInjection.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class MvcControllerServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("*.Farmpik.*");
            serviceCollection.AddMvcWebAPIControllers("*.Farmpik.*");
          
        }
    }
}