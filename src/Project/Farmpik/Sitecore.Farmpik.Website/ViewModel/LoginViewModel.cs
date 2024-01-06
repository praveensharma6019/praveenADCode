using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Farmpik.Website.ViewModel 
{
    public class LoginViewModel : RenderingModel
    {
        [Required(ErrorMessage = "Email is required")]
        [Display()]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool InValidCredential { get; set; } = false;

        public string ErrorMessage { get; set; }

        public bool IsSignOut { get; set; } = false;
    }
}