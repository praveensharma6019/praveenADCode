using Adani.SuperApp.Realty.Feature.Configuration.Platform.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Services
{
    public interface IQuickLinksService
    {
        QuickLinksModel GetQuickLinksData(Rendering rendering);
    }
}