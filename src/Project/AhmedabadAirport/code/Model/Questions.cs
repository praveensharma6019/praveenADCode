using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.AhmedabadAirport.Website.Model
{
    [Serializable]
    public class Questions
    {
        public List<Options> Option
        {
            get;
            set;
        }

        public string Question
        {
            get;
            set;
        }

        public Questions()
        {
            this.Option = new List<Options>();
        }
    }
}