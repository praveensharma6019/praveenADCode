using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.AdmissionPage
{
    public interface IAISLifeAtSchoolPageServices
    {
        SectionCards<LifeAtSchoolArtsDataModel> GetLifeAtSchoolArts(Rendering rendering);
        GalleryContentModel<BaseImageContentModel> GetLifeAtSchoolComplementingTheAcademics(Rendering rendering);
        BaseContentModel<LifeAtSchoolFindOutMoreDataModel> GetLifeAtSchoolFindOutMore(Rendering rendering);
    }
}
