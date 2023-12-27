using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public interface ICustomContentJSON
    {
        ContentJSONList GetCustomContentJSONData(Rendering rendering, string storeType, string infoType, string insurance, string zeroCancellation, string status ,string fareType);
    }
}