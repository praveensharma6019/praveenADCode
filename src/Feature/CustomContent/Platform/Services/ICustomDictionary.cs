using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public interface ICustomDictionary
    {
        object GetDictionary(Rendering rendering);
    }
}
