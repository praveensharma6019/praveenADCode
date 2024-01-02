using Project.AAHL.Website.Models.AboutAHHL;
using Project.AAHL.Website.Models.AboutUs;
using Project.AAHL.Website.Models.Cargo;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.GeneralAviation;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Cargo
{
    public interface ICargoServices
    {
        OurWayForwardModel GetOurWayForward(Rendering rendering);
        OperationalEfficiencyModel GetOperationalEfficiency(Rendering rendering);
    }
}