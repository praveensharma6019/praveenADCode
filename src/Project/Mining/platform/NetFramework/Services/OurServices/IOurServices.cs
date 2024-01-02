using Project.Mining.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.OurServices
{
    public interface IOurServices
    {
        OurServicesModel GetOurServicesModel(Rendering rendering);
    }
}
