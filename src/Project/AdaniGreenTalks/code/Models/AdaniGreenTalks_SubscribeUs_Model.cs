using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website.Models
{
    public class AdaniGreenTalks_SubscribeUs_Model
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter Email ID")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }


        public string FormType { get; set; }

        public string FormUrl { get; set; }

        public string googleCaptchaToken { get; set; }
    }
}