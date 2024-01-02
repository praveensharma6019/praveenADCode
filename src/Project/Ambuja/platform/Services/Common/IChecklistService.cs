using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.Common;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.Common
{
    public interface IChecklistService
    {
        ChecklistModel GetChecklist(Rendering rendering);
        CommonItemModel GetItemData(Rendering rendering);
    }
}
