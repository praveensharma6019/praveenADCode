using Adani.SuperApp.Realty.Feature.Sitemap.Platform.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Realty.Feature.Sitemap.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISitemapRootResolverService, SitemapRootResolverService>();                      
        }
    }
}