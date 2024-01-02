using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Services.Common
{
    public interface ICommonComponents
    {
        MainBanner GetMainBanner(Rendering rendering);


        HomeMainBanner GetHomeMainBanner(Rendering rendering);
        GalleryContentModel<LifeAtSchoolGalleryItem> GetLifeAtSchool(Rendering rendering);
        VideoBanner GetVideoBanner(Rendering rendering);        
        HomeCurriculum GetCurriculum(Rendering rendering);
        HomeFAQ GetHomeFAQ(Rendering rendering);
    }
}
