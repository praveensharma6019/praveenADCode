using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public interface ICustomDictionaryList
    {
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> GetDictionary(Rendering rendering);
    }
}
