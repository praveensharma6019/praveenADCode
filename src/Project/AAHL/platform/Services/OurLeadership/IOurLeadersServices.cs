using Project.AAHL.Website.Models.OurLeadership;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.OurLeadership
{
    public interface IOurLeadersServices
    {
        OurLeadersModel GetOurLeaders(Rendering rendering);
    }
}