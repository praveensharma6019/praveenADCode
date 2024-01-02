using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.About_Page;
using Project.AmbujaCement.Website.Models.Home;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.Home
{
    public interface IHomeServices
    {
        MainBanner GetMainBanner(Rendering rendering);
        //GalleryContentModel<LifeAtSchoolGalleryItem> GetLifeAtSchool(Rendering rendering);
        VideoBanner GetVideoBanner(Rendering rendering);
     //   MainBanner GetMainBanner(Rendering rendering);
        HomeCurriculum GetCurriculum(Rendering rendering);
        HomeFAQ GetHomeFAQ(Rendering rendering);
        GalleryContentModel<ScaleSlider> GetScaleSlider(Rendering rendering);
        CardSliderContentModel<CardSlider> GetCardSlider(Rendering rendering);
        CategoriesListModel GetCategories(Rendering rendering);
        VerticalCarousel GetVerticalCarousel(Rendering rendering);
        SeoData GetSeoData(Sitecore.Mvc.Presentation.Rendering rendering);

        Cookies GetCookies(Rendering rendering);
        CostCalculatorData GetCostCalculatorFormData(Rendering rendering);
        AboutPageModel GetAboutPage(Rendering rendering);
    }
}
