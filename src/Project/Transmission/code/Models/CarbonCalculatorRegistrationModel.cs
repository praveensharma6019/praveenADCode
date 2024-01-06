using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Transmission.Website.Models
{
    public class CarbonCalculatorRegistrationModel
    {
        [Required(ErrorMessage = "Full Name is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string FullName
        {
            get;
            set;
        }       
       
        public Guid Id
        {
            get;
            set;
        }

        [EmailAddress(ErrorMessage = "Please Enter Valid EmailAddress.")]
        [Required(ErrorMessage = "EmailId is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string EmailId
        {
            get;
            set;
        }
        
        [RegularExpression("^[0-9]{8,16}$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string MobileNumber
        {
            get;
            set;
        }
        
        [Required(ErrorMessage = "Company Name is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Company
        {
            get;
            set;
        }
       
        [Required(ErrorMessage = "Status is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Status
        {
            get;
            set;
        }
        public DateTime? Modified_Date
        {
            get;
            set;
        }

        public DateTime? Created_Date
        {
            get;
            set;
        }
        public string OTP
        {
            get;
            set;
        }
        public string EmailOTP
        {
            get;
            set;
        }

        public string Email_Verified
        {
            get;
            set;
        }
        public string Mobile_Verified
        {
            get;
            set;
        }
        public string InCompleteCarbonCalculator
        {
            get;
            set;
        }
    }
}