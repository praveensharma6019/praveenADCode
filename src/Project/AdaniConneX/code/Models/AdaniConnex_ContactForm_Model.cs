using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniConneX.Website.Models
{
    public class AdaniConnex_ContactForm_Model
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Message { get; set; }
        public string FormType { get; set; }
        public string FormUrl { get; set; }        
        public DateTime SubmittedDate { get; set; }
        public string reResponse { get; set; }
        public string SendEmail { get; set; }
    }
}