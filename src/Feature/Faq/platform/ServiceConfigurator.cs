using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
namespace Adani.SuperApp.Realty.Feature.Faq.Platform
{
    public class ServiceConfigurator:IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.IFaqService, Services.FaqService>();
            serviceCollection.AddTransient<Services.IQuickLinksFaqService, Services.QuickLinksFaqService>();
        }
    }
}