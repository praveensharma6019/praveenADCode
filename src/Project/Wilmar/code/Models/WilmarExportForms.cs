using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Wilmar.Website.Models
{
    public class WilmarExportForms
    {
        public string Message { set; get; }
        public string MessageType { set; get; }
        public string Salutation { set; get; }
        public string FirstName { set; get; }
        public string Lastname { set; get; }
        public string Address1 { set; get; }
        public string Address2 { set; get; }
        public string State { set; get; }
        public string City { set; get; }
        public string Pincode { set; get; }
        public string Country { set; get; }
       
        public string Mobile { set; get; }
        public string Email { set; get; }
        public string LandlineNumber { set; get; }
        public string BusinessCategory { set; get; }
        public string Remarks { set; get; }
        public string FormType { set; get; }
        public string PageType { set; get; }
        public DateTime FormSubmit { set; get; }
        public string reResponse { set; get; }
    }
}