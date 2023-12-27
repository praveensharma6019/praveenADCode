using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
    public interface ITravelInsurance
    {
        TravelInsurance GetTravelInsuranceData(Item datasource);
    }
}
