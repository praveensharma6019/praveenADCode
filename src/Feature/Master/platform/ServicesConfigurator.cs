using Adani.SuperApp.Airport.Feature.Master.Platform.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.Master.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IMasterDropdownService, MasterDropdownService>();
            serviceCollection.AddScoped<ICountryMasterService, CountryMasterService>();
            serviceCollection.AddScoped<IStateMasterService, StateMasterService>();
            serviceCollection.AddScoped<INationalityService, NationalityService>();
            serviceCollection.AddScoped<IMasterList, MasterList>();
            serviceCollection.AddScoped<ICityMasterService, CityMasterService>();
            serviceCollection.AddScoped<IDomainListService, DomainListService>();
        }
    }
}