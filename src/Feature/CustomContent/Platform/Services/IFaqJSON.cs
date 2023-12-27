using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public interface IFaqJSON
    {
        FaqJSONWidget GetFAQJSONList(Rendering rendering);
    }
}