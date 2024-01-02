using Project.AmbujaCement.Website.Models.GetInTouchPage;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.GetInTouch
{
    public interface IGetInTouchService
    {
        GetInTouchDetailsModel GetGetInTouchDetailsModel(Rendering rendering);
        TwoColumnCardModel GetTwoColumnCardModel(Rendering rendering);
    }
}
