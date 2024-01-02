using Newtonsoft.Json;
using Sitecore.Shell.Applications.ContentEditor;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model
{
    public class PNRFormInput
    {
        [Required]
        [RegularExpression(@"[a-zA-Z0-9\b]{6}$", ErrorMessage = "Please enter valid PNR")]
        public string pNR { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z. ]{0,30}$", ErrorMessage = "Please enter valid First Name")]
        public string firstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z. ]{0,30}$", ErrorMessage = "Please enter valid Last Name")]
        public string lastName { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please enter valid Mobile number")]
        public string mobile { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd}")]
        public string travelDate
        {
            get;
            set;
        }

        public string reCaptcha { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Please enter valid Email")]
        public string email { get; set; }
    }
}

