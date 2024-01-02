using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Foundation.DataAPI.Platform
{
    public class ServicesConfigurator:IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.IAPIResponse, Services.APIResponse>();
        }
    }
}