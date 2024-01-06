using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Sitecore.AhmedabadAirport.Website.Model
{
    public class EnvelopeUser
    {
        public string email
        {
            get;
            set;
        }

        public List<SelectListItem> EnvelopeList
        {
            get;
            set;
        }

        public string Mobile
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public List<SelectListItem> TenderList
        {
            get;
            set;
        }

        public EnvelopeUser()
        {
        }
    }
}