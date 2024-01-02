using Project.AdaniOneSEO.Website.Models.CityToCityPagev2;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public interface ICityToCityFilterOptionService
    {
        FilterOptionCityToCityModel GetFilterOptionCityToCityModel(Rendering rendering);
    }
}
