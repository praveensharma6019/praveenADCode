using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Realty.Foundation.Logging.Platform.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ILogRepository, LogRepository>();
        }
    }
}