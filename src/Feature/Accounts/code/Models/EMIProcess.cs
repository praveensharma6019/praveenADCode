using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class EMIProcess
    {
        public string CANumber { get; set; }
        public string ConsumerName { get; set; }
        public string Source { get; set; }
        public bool IsEMIEligible { get; set; }
        public bool ProceedWithEMI { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal EMIInstallmentAmount { get; set; }
        public string Result { get; set; }
    }

}