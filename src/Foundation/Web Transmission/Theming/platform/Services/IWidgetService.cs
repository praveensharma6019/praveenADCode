using Adani.BAU.Transmission.Foundation.Theming.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Foundation.Theming.Platform.Services
{
    public interface IWidgetService
    {
        WidgetItem GetWidgetItem(Sitecore.Data.Items.Item widget);
    }
}