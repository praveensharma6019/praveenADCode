using Project.Mining.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services
{
    public interface IWhyMtcsService
    {
        WhyMtcsModel GetWhyMtcsModel(Rendering rendering);
    }
}
