
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Farmpik.Website.Data_Annotations;
using System.Linq;
using System.Web;

namespace Sitecore.Farmpik.Website.Models
{
    public class FarmpikContactModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter name"), MaxLength(30)]
        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Name is not valid.")]
        public string Name { set; get; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter Email ID")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { set; get; }

        public string Mobile { set; get; }

        [Required]
        [IsValidCategory(ErrorMessage = "Please select valid category")]
        public string MessageType { set; get; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter address"), MaxLength(500)]
        public string Message { set; get; }
        public string reResponse { set; get; }
        public string FormType { set; get; }
        public string PageInfo { get; set; }
        public DateTime FormSubmitOn { set; get; }
        public string OTP { get; set; }
    }
}
