using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.TrivandrumAirport.Website.Model
{
    [Serializable]
    public class CorrigendumDetails
    {
        public DateTime Date
        {
            get;
            set;
        }

        public List<TenderDetails> NITPRNo
        {
            get;
            set;
        }

        public List<TenderDetails> TenderDocument
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public CorrigendumDetails()
        {
        }
    }
}