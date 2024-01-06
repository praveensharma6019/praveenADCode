using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.ValidateGoogleCaptcha.Models
{
    public class CaptchaRequestModel
    {
        [Required]
        public string reResponse { get; set; }
    }
}