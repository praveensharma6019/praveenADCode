using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.AboutUsPage;
using Project.AmbujaCement.Website.Models.ProductList;
using Project.AmbujaCement.Website.Models.ProjectDetails;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.AboutUs
{
    public interface IProjectDetailsService
    {
        BrochureDataModel GetBrochureDataModel(Rendering rendering);
        FeaturesModel GetFeaturesModel(Rendering rendering);
        HeroBannerModel GetHeroBannerModel(Rendering rendering);
    }
}
