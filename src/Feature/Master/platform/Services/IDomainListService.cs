using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Master.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public interface IDomainListService
    {
        WidgetModel GetDomainData(Rendering rendering);
    }
}