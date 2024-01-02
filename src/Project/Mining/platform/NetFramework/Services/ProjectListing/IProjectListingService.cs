using Project.Mining.Website.Models;
using Project.Mining.Website.Models.ProjectListing;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.ProjectListing
{
    public interface IProjectListingService
    {
        ProjectListingModel GetProjectListing(Rendering rendering);
        RequestAcallModel GetRequestAcall(Rendering rendering);
    }
}
