using Project.Mining.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.OurAccreditation
{
    public interface IOurAccreditationService
    {
        OurAccreditationModel GetOurAccreditation(Rendering rendering);
    }
}
