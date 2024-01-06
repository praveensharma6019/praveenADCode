using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.AhmedabadAirport.Website.Model
{
    [Serializable]
    public class SubSurveyResult
    {
        public List<ApplianceList> ApplianceList
        {
            get;
            set;
        }

        public string Appliancetype
        {
            get;
            set;
        }

        public SubSurveyResult()
        {
            this.ApplianceList = new List<ApplianceList>();
        }
    }
}