using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.Services.ContentResolver
{
    public interface IContentResolverService
    {
        WhyUsCardModel GetWhyUsCardModel(Rendering rendering);
        SectionCards<ExtendedContentModel> GetInquireNowCardModel(Rendering rendering);
        SectionCards<VisitOurSchoolDataItemModel> GetVisitOurSchoolCardModel(Rendering rendering);
    }
}
