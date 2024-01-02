using Project.AmbujaCement.Website.Models.Forms;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.Forms
{
    public interface IAmbujaFormsService
    {
        GetInTouchFormModel GetGetInTouchFormModel(Rendering rendering);
    }
}
