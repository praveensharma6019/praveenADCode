using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Services.Common;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICommonComponents, CommonComponents>();
        }
    }
}