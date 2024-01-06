namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;
    using System.Web;

    [Serializable]
    public class ComplaintGetCaseDetailsSAP
    {
        public string Flag { get; set; }    //Output flag with numeri value   1:  Successful,   2: Unsuccessful
        public string Message { get; set; } //Details Description of output or exception if any 
        public List<ComplaintCaseDetails> Complaints { get; set; }
    }

    [Serializable]
    public class ComplaintCaseDetails
    {
        public string Complaintnumber { get; set; }
        public string ComplaintType { get; set; }
        public string ComplaintSUbtype { get; set; }
        public string ComplaintStatus { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
    }
}