using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.OurStory;
using Project.AAHL.Website.Models.Partnerships;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Common
{
    public interface IPartnership
    {
        TravelServices GetTravelServices(Rendering rendering);
       
    }
}
