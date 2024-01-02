using Adani.EV.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.EV.Project.Services
{
    public interface ICostCalculatorService
    {
       CostCalulatorSectionsModel GetCostCalulatorSectionsModel(Sitecore.Mvc.Presentation.Rendering rendering);

       ContactusSectionsModel GetcontactusSectionsModel(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
