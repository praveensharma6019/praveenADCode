using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Models.UpcomingEvents;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public interface IUpcomingEventsService
    {
        BaseHeadingModel<EventItemModel> Render(Rendering rendering);
    }
}