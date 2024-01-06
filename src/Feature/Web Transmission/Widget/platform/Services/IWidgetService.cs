using Adani.BAU.Transmission.Feature.Widget.Platform.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.BAU.Transmission.Feature.Widget.Platform.Services
{
    public interface IWidgetService
    {
        WidgetItem GetWidgetItem(Item widget);
    }
}
