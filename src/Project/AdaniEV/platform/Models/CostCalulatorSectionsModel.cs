using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class CostCalulatorSectionsModel
    {
        public CalculatorModel calculator { get; set; } = new CalculatorModel();
        public BenefitsModel benefits { get; set; }=new BenefitsModel();
    }
}