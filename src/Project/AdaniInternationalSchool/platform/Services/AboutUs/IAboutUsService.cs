using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.Services.AboutUs
{
    public interface IAboutUsService
    {
        PageDescriptionModel GetAboutUsModel(Rendering rendering);
    }
}
