using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniConneX.Website.Models
{
    public class AdaniConnex_JoinUsForm_Model
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "401"), MaxLength(30), MinLength(2)] 
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "405"), MaxLength(10), MinLength(10)]
        public string Contact { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "403")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required]
        [FileName(ErrorMessage = "406")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "406")]
        //[FileHeaderSignature("JVBER", ErrorMessage = "406")]
        public HttpPostedFileBase CVFile { get; set; }

        public string CVName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string FormType { get; set; }
        public string FormUrl { get; set; }
        public string reResponse { get; set; }
        public string SendEmail { get; set; }
    }
}