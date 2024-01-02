using Project.AAHL.Website.Models.AboutAHHL;
using Project.AAHL.Website.Models.AboutUs;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.GeneralAviation;
using Project.AAHL.Website.Models.GroundTransportation;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.GroundTransportation
{
    public interface IGroundTransportationServices
    {
        TravelExperience GetTransportation(Rendering rendering);
    }
}