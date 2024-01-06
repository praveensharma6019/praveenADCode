using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.AhmedabadAirport.Website.Model
{
    [Serializable]
    public class ApplianceList
    {
        public List<AnswerdQuestionResponse> AnswerdQuestionResponse
        {
            get;
            set;
        }

        public string ApplianceName
        {
            get;
            set;
        }

        public ApplianceList()
        {
            this.AnswerdQuestionResponse = new List<AnswerdQuestionResponse>();
        }
    }
}