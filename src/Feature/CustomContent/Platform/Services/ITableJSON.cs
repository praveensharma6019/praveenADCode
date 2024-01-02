using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public interface ITableJSON
    {
        WidgetModel GetTableDataList(Rendering rendering);
    }
}