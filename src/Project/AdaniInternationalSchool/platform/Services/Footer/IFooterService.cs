using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.Services.Footer
{
    public interface IFooterService
    {
        FooterModel Render(Rendering rendering);
    }
}