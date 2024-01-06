using System;
using System.Runtime.CompilerServices;

namespace Sitecore.LucknowAirport.Website.Model
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