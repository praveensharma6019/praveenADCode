using Adani.SuperApp.Realty.Feature.Leaders.Platform.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
namespace Adani.SuperApp.Realty.Feature.Leaders.Platform
{
    public class ServiceConfigurator:IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {           
            serviceCollection.AddTransient<ILeadersDataRootResolverService, LeadersDataRootResolverService>();
            serviceCollection.AddTransient<IAchievementsRootResolverService, AchievementsRootResolverService>();
        }
    }
}