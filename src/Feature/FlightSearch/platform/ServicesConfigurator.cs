using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.IFlightSearch, Services.FlightSearch>();
            serviceCollection.AddTransient<Services.ICityAirportsContent, Services.CityAirportsContent>();
            serviceCollection.AddTransient<Services.IFlightList, Services.FlightList>();
            serviceCollection.AddTransient<Services.ITravelInsurance, Services.TravelInsuranceService>();
            serviceCollection.AddTransient<Services.ICombinedTravelInsurance, Services.CombinedTravelInsuranceService>();
            serviceCollection.AddTransient<Services.ITravelInsuranceLanding, Services.TravelInsuranceLandingService>();
        }
    }
}