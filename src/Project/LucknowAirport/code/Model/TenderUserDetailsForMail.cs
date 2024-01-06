using Sitecore.LucknowAirport.Website;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.LucknowAirport.Website.Model
{
    public class TenderUserDetailsForMail
    {
        public string TenderId
        {
            get;
            set;
        }

        public string TenderName
        {
            get;
            set;
        }

        public string TenderNIT
        {
            get;
            set;
        }

        public List<LKO_UserTenderMapping> TenderUsers
        {
            get;
            set;
        }

        public TenderUserDetailsForMail()
        {
        }
    }
}