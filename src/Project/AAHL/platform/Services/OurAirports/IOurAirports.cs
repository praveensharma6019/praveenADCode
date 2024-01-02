using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.OurAirports;
using Project.AAHL.Website.Models.OurStory;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Common
{
    public interface IOurAirports
    {
        Details GetDetails(Rendering rendering);
        TravelReadyAirports GetTravelReadyAirports(Rendering rendering);
        OurAirportMap GetAirportMap(Rendering rendering);
    }
}
