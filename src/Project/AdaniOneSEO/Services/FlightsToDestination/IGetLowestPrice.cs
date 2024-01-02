using Project.AdaniOneSEO.Website.LayoutServices.FlightsToDestination;
using Project.AdaniOneSEO.Website.Models;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Services.FlightsToDestination
{
    public interface IGetLowestPrice
    {
        CheapestFlightCalenderModel GetLowestPriceData(Rendering rendering);
    }
}