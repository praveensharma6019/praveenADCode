//using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform
{
    public class ServiceConfigurator: IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Helper.IHelper, Helper.Helper>();
            
        }

    }
}