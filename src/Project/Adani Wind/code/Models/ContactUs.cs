using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniWind.Website.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage = "Please enter first name.")]
        [RegularExpression("^[a-z A-Z]{3,100}$", ErrorMessage = "Please enter a valid first name.")]
        public string Fullname
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please enter mail id.")]
        [RegularExpression("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,17}$", ErrorMessage = "Please enter a valid email id")]
        public string Email
        {
            get;
            set;
        }
        [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[6789]\d{9}|(\d[ -]?){10}\d$", ErrorMessage = "Please enter a valid contact Number")]
        public string ContactNo
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please enter Purpose"), MaxLength(500)]
        [RegularExpression(@"^[A-Za-z0-9;,.?_\-!@&:""'\/\\ ]{0,500}$", ErrorMessage = "Purpose is not valid, special characters not allowed")]
        public string Purpose
        {
            get;
            set;
        }
        public string reResponse
        {
            get;
            set;
        }
    }
}