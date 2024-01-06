using System;
using System.Runtime.CompilerServices;

namespace Sitecore.TrivandrumAirport.Website.Model
{
    [Serializable]
    public class TenderLoginModel
    {
        public string TenderId
        {
            get;
            set;
        }

        public string userId
        {
            get;
            set;
        }

        public string UserType
        {
            get;
            set;
        }

        public TenderLoginModel()
        {
        }
    }
}