using Project.AAHL.Website.Models.CompetetiveAdvantage;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.CompetetiveAdvantage
{
    public interface ICompetetiveAdvantageService
    {
        SectionWithImagesModel GetSectionWithImages(Rendering rendering);
    }
}
