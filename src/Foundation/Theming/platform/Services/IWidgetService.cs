using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.Theming.Platform.Services
{
    public interface IWidgetService
    {
        WidgetItem GetWidgetItem(Sitecore.Data.Items.Item widget);
    }
}