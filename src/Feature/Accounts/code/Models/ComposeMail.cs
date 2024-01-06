using System.ComponentModel.DataAnnotations;
using System.Web;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Feature.Accounts.Models
{
    public class ComposeMail
    {
        public string ToEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        
        public HttpPostedFileBase Attachment { get; set; }
        [Required]
        public string BodyMessage { get; set; }
        public string Captcha { get; set; }
    }
}