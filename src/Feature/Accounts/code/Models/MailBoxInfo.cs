using Sitecore.Data;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models
{
    public class MailBoxInfo
    {
        public string ItemId { get; set; } 
        public string MailId { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Attachment { get; set; }
        public string OnDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsPVC { get; set; }
        public bool IsNONPVC { get; set; }
        public bool IsTrash { get; set; }
        public string UserId { get; set; }

        public string PageURL { get; set; }
    }
}