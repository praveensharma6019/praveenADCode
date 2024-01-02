using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using System.Web.Services.Description;

namespace Adani.SuperApp.Realty.Feature.Blog.Platform
{
    public class ServiceConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {

            serviceCollection.AddTransient<Services.IBlogCategoryService, Services.BlogCategoryService>();
            serviceCollection.AddTransient<Services.IBlogSearchService, Services.BlogSearchService>();
        }
    }
}