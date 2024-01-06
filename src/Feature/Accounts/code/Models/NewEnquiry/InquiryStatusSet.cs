using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class InquiryStatusSet
    {
        public string InquiryNo { get; set; }
        public string InquiryDate { get; set; }
        public string ProspectNo { get; set; }
        public string ProspectDate { get; set; }
        public string ActualbpNo { get; set; }
        public string ContractAcc { get; set; }
        public string ActualbpDate { get; set; }
        public string UserID { get; set; }
        public string DocumentUpload { get; set; }
        public string DocumentApproved { get; set; }
        public string AdditionalData { get; set; }
        public string SchemeName { get; set; }
        public string Amount { get; set; }
        public string Payment { get; set; }
        public string InstallationDate { get; set; }
        public string ConnectionDate { get; set; }
        public string Feedback { get; set; }
        public string REMARKDATE { get; set; }
        public string REMARK { get; set; }
        public string ConvDate { get; set; }
        public string OEMId { get; set; }
        public string OEMName { get; set; }
        public string OEMDate { get; set; }
        public string CardGiven { get; set; }
    }
}