using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;


namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<Services.ICustomContentJSON, Services.CustomContentJSON>();
            serviceCollection.AddScoped<Services.ICustomDictionary, Services.CustomDictionary>();
            serviceCollection.AddScoped<Services.ICustomDictionaryList, Services.CustomDictionaryList>();
            serviceCollection.AddScoped<Services.ITermsAndConditionJSON, Services.TermsAndConditionJSON>();
            serviceCollection.AddScoped<Services.IResolveBannerJSON, Services.ResolveBannerJSON>();
            serviceCollection.AddScoped<Services.IFaqJSON, Services.FaqJSON>();
            serviceCollection.AddScoped<Services.ITableJSON, Services.TableJSON>();
        }
    }
}