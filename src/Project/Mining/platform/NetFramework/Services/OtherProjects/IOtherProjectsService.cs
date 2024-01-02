using Project.Mining.Website.Models;
using Project.Mining.Website.Models.ProjectDetails;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services
{
    public interface IOtherProjectsService
    {
        OtherProjectsModel GetOtherProject(Rendering rendering);
    }
}
