using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Models.LifeAtSchoolActivities;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public interface IMainCardV2Service
    {
        BaseCards<MainCardVideoItemModel> Render(Rendering rendering);
    }
}