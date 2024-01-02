using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public interface IMainCardService
    {
        BaseCards<MainCardItemModel> Render(Rendering rendering);
    }
}