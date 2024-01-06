using Sitecore.AhmedabadAirport.Website;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.AhmedabadAirport.Website.Model
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

        public List<AMD_UserTenderMapping> TenderUsers
        {
            get;
            set;
        }

        public TenderUserDetailsForMail()
        {
        }
    }
}