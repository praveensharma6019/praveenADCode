using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public interface IAllActivitiesService
    {
        BaseCards<AllActivitiesDataModel> Render(Rendering rendering);
    }
}