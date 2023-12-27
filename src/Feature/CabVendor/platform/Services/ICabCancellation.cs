using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services
{
    public interface ICabCancellation
    {
        CancellationData GetCancellationData(Rendering rendering);
    }
}