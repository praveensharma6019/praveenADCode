using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.JaipurAirport.Website.Model
{
    [Serializable]
    public class Appliance
    {
        public string Name
        {
            get;
            set;
        }

        public List<Questions> QuestionsList
        {
            get;
            set;
        }

        public Appliance()
        {
            this.QuestionsList = new List<Questions>();
        }
    }
}