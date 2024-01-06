using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.MangaloreAirport.Website.Model
{
    [Serializable]
    public class TenderStatus
    {
        public List<TenderDetails> CloseTender
        {
            get;
            set;
        }

        public List<CorrigendumDetails> CorrigendumTender
        {
            get;
            set;
        }

        public List<TenderDetails> OpenTender
        {
            get;
            set;
        }

        public TenderStatus()
        {
            this.OpenTender = new List<TenderDetails>();
            this.CloseTender = new List<TenderDetails>();
            this.CorrigendumTender = new List<CorrigendumDetails>();
        }
    }
}