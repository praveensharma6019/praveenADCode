using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.Services.Header
{
    public interface IHeaderService
    {
        HeaderModel Render(Rendering rendering);
    }
}