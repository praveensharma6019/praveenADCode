using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
    public interface ICityAirportsContent
    {
        CityAirportsData GetCityAirportsData(Item datasource, string isdomestic);

        AirportList GetCityAirports(Item datasource);

        AppAirportsData GetAppAirportsData(Item datasource,string airportType,string isdomestic, string airportcode);
    }
}
