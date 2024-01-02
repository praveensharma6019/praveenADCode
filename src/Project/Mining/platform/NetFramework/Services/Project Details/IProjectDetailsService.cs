using Project.Mining.Website.Models;
using Project.Mining.Website.Models.ProjectDetails;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.Header
{
    public interface IProjectDetailsService
    {
        ProjectDetailModel GetProjectDetail(Rendering rendering);
    }
}
