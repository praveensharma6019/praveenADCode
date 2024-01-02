using Project.Mining.Website.Models.Home;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.Home
{
    public interface ISubscribeUsService
    {
        SubscribeUsModel GetSubscribeUs(Rendering rendering);
    }
}
