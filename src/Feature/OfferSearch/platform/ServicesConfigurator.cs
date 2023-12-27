using Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
           
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(GlobalOffersController),
               typeof(GlobalOffersController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(UnlockOffersController),
               typeof(UnlockOffersController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(Offers_oldController),
               typeof(Offers_oldController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(OffersSRPController),
                typeof(OffersSRPController)));
        }
    }
}