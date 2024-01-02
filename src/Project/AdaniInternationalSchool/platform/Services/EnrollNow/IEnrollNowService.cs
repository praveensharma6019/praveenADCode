using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Models.EnrollNow;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.Services.EnrollNow
{
    public interface IEnrollNowService
    {
        EnrollNowOverviewModel GetOverviewModel(Rendering rendering);
        BaseHeadingModel<AdmissionStage> GetAdmissionDocuments(Rendering rendering);
        AgeCriteriaModel GetAgeCriteriaModel(Rendering rendering);
    }
}
