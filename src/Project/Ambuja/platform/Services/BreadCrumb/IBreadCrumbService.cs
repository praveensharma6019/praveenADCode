using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.BreadCrumb;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services
{
    public interface IBreadCrumbService
    {
        BreadCrumbModel GetBreadcrumb(Rendering rendering);
    }
}
