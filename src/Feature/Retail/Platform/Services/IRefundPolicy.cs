using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.Services
{
    public interface IRefundPolicy
    {
        WidgetModel GetRefundPolicyData(Sitecore.Mvc.Presentation.Rendering rendering, string outletcode);
    }
}