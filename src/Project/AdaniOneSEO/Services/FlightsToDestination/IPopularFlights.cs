using Project.AdaniOneSEO.Website.LayoutServices.FlightsToDestination;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.Services.FlightsToDestination
{
    public interface IPopularFlights
    {
        PopularFlightsModel GetPopularFlights(Rendering rendering);
    }
}