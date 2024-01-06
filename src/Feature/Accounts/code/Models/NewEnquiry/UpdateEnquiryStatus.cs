using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class UpdateEnquiryStatus
    {
        public string INQTYPE { get; set; }
        public int TASKID { get; set; }
        public string TASKNAME { get; set; }
    }
}