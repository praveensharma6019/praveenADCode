using Project.AdaniOneSEO.Website.Models.CityToCityPagev2;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public interface ICityToCityPageService
    {
        AirportCityDetailsModel GetAirportCityDetailsModel(Rendering rendering);
    }
}
