using Adani.SuperApp.Airport.Feature.Hotels.Platform.Controllers;
using Adani.SuperApp.Airport.Feature.Hotels.Platform.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(HotelPoliciesController),
              typeof(HotelPoliciesController)));
            serviceCollection.AddScoped<ITopHotelsWebService, TopHotelsWebService>();
            serviceCollection.AddScoped<ITopHotelsAppService, TopHotelsAppService>();
            serviceCollection.AddScoped<IHotelsHeroCarouselWeb, HotelsHeroCarouselWebService>();
            serviceCollection.AddScoped<IPopularDestinationService, PopularDestinationService>();
        }
    }
}