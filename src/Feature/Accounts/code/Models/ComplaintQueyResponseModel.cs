using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class ComplaintQueyResponseModel
    {
        public D d { get; set; }
    }
    public class Metadata
    {
        public string id { get; set; }
        public string uri { get; set; }
        public string type { get; set; }
    }

    public class Result
    {
        public Metadata __metadata { get; set; }

        #region Query/Complaint Dropdown
        public string Category { get; set; }
        public string Comptype { get; set; }
        public string Taskcode { get; set; }
        public string Text { get; set; }
        public string Partner_Type { get; set; }
        public string Cust_No { get; set; }
        public string MsgFlag { get; set; }
        public string Message { get; set; } 
        #endregion

        #region Query/Complaint List
        public string CQR_No { get; set; }
        public string Comp_Date { get; set; }
        public string Status { get; set; }
        public string Complaint_Type { get; set; }
        public string Task_Code { get; set; }
        public string Description { get; set; }
        public string Text_Area { get; set; }
        public string Quantity { get; set; }
        #endregion

    }
    public class D
    {
        public List<Result> results { get; set; }
    }
}