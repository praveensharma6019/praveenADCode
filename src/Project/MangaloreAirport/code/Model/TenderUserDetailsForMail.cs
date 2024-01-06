using Sitecore.MangaloreAirport.Website;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.MangaloreAirport.Website.Model
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

        public List<UserTenderMapping> TenderUsers
        {
            get;
            set;
        }

        public TenderUserDetailsForMail()
        {
        }
    }
}