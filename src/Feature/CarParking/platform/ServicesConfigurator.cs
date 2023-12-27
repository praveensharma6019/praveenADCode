using Adani.SuperApp.Airport.Feature.CarParking.Platform.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.CarParking.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
          
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(CarParkingFilterController),
              typeof(CarParkingFilterController)));
          
        }
    }
}