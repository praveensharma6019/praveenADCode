using Project.AAHL.Website.Models.AboutAHHL;
using Project.AAHL.Website.Models.AboutUs;
using Project.AAHL.Website.Models.Common;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.AboutAHHL
{
    public interface IAboutAHHLServices
    {
        OurAirportModel GetOurAiport(Rendering rendering);
        OurAirportTabModel GetOurAiportTab(Rendering rendering);
    }
}