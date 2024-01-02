using Project.AAHL.Website.Models.AboutAHHL;
using Project.AAHL.Website.Models.AboutUs;
using Project.AAHL.Website.Models.AirportConcessions;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.GeneralAviation;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.AirportConcessions
{
    public interface IAirportConcessionsServices
    {
        BeyondAirportsModel GetConcessions(Rendering rendering);
        PartnerWithUsModel GetPartnerWithUs(Rendering rendering);
    }
}