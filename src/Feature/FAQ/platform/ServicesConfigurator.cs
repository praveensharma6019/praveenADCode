using Adani.SuperApp.Airport.Feature.FAQ.Controlles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.FAQ
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(FAQFilteredSearchController),
                typeof(FAQFilteredSearchController)));
            serviceCollection.AddTransient<Interfaces.IFAQ, Repositories.FAQRepositories>();
            serviceCollection.AddTransient<Services.IFAQSearchItems, Services.FAQSearchItems>();
        }
    }
}