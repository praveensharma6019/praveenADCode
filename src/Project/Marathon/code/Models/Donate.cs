using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Sitecore.Marathon.Website.Models
{
    [Table("AhmedabadMarathonDonations")]
    public class Donate
    {
        public string AffiliateCode
        {
            get;
            set;
        }

        public string Amount
        {
            get;
            set;
        }

        public string CauseTitle
        {
            get;
            set;
        }

        public string EmailId
        {
            get;
            set;
        }

        public string ErrorMessge
        {
            get;
            set;
        }

        public string MobileNumber
        {
            get;
            set;
        }

        [Key]
        public string Name
        {
            get;
            set;
        }

        public string OrderId
        {
            get;
            set;
        }

        public string PaymentStatus
        {
            get;
            set;
        }

        public string TotalAmount
        {
            get;
            set;
        }

        public Guid Userid
        {
            get;
            set;
        }
        public string TaxExemptionCause
        {
            get;
            set;
        }
        public string reResponse
        {
            get;
            set;
        }

        public Donate()
        {
        }
    }
}