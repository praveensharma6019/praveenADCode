using System;
using System.Runtime.CompilerServices;

namespace Sitecore.MangaloreAirport.Website.Model
{
    [Serializable]
    public class TenderDetails
    {
        public DateTime? Adv_Date
        {
            get;
            set;
        }

        public string Bid_Submision_ClosingDate
        {
            get;
            set;
        }

        public string Business
        {
            get;
            set;
        }

        public DateTime? ClosingDate
        {
            get;
            set;
        }

        public string Cost_of_EMD
        {
            get;
            set;
        }

        public DateTime? CreatedDate
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string DocumentPath
        {
            get;
            set;
        }

        public string Estimated_Cost
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public bool isCorrigendumPresent
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public DateTime? ModifiedDate
        {
            get;
            set;
        }

        public string NITPRNo
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string SupportEmailAddress
        {
            get;
            set;
        }

        public string TenderType
        {
            get;
            set;
        }

        public TenderDetails()
        {
        }
    }
}