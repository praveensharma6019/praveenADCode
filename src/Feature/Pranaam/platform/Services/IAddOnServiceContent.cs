using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public interface IAddOnServiceContent
    {
       Models.AddOnServicewidgets GetAddOnService(Rendering rendering);
    }
}