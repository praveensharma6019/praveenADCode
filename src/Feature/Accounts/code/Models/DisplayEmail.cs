using System.ComponentModel.DataAnnotations;
using System.Web;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Feature.Accounts.Models
{
    public class DisplayEmail
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string AttachmentLink { get; set; }
        public string BodyMessage { get; set; }
        public string EmailDate { get; set; }

        public string FromEmail { get; set; }
    }
}