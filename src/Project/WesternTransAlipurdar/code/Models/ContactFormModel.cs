using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class ContactFormModel
    {

        [Required(ErrorMessage = "Please enter Name"), MaxLength(30), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Please enter valid Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Message")]
        [RegularExpression(@"^[\w\s.,@:!?-]+$", ErrorMessage = "Please enter valid message")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Please enter position you want to apply for")]
        public string Inquiry { get; set; }
       
    }
}