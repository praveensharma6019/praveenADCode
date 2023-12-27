using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
    public interface ITravelInsuranceLanding
    {
        TravelInsuranceLanding GetTravelInsuranceData(Item datasource);
    }
}
