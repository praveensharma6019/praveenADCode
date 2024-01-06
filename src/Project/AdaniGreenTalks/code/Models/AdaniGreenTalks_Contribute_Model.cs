using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website.Models
{
    public class AdaniGreenTalks_Contribute_Model
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string City { get; set; }
        public string Goal { get; set; }
        
        public string FellowName { get; set; }

        public DateTime SubmittedDate { get; set; }

        public string FormType { get; set; }

        public string FormUrl { get; set; }

        public string googleCaptchaToken { get; set; }
    }
}