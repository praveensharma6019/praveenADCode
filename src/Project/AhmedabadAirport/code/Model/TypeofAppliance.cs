using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.AhmedabadAirport.Website.Model
{
    [Serializable]
    public class TypeofAppliance
    {
        public List<Appliance> ApplianceList
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public TypeofAppliance()
        {
            this.ApplianceList = new List<Appliance>();
        }
    }
}