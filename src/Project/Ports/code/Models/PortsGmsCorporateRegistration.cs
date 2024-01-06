using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGmsCorporateRegistration
    {
        
        public string Name
        {
            get;
            set;
        }
        [Required(ErrorMessage = "First Name is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string first_name
        {
            get;
            set;
        }
        public string middle_name
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Last Name is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string last_name
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
        [Required(ErrorMessage = "Email is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        [RegularExpression("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$", ErrorMessage = "Please enter a valid email address only allow form adani.com")]
        public string Email
        {
            get;
            set;
        }

        [RegularExpression("^[0-9]{12,12}$", ErrorMessage = "Please Enter Valid Fax Number.")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string Fax_No
        {
            get;
            set;
        }

        [RegularExpression("^[0-9]{8,16}$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Mobile
        {
            get;
            set;
        }



        [Required(ErrorMessage = "Phone Field is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Phone
        {
            get;
            set;
        }
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string DOB
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Company Name is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Company_Name
        {
            get;
            set;
        }
        public string Location
        {
            get;
            set;
        }
        public string Department
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Qustion is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Qustion
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Answer is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Answer
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Address is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Address
        {
            get;
            set;
        }
        [Required(ErrorMessage = "State is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string State
        {
            get;
            set;
        }

        
        
        public HttpPostedFileBase[] Attachment
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Country is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Country
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Pin Code is Required.")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")]
        public string Pin_Code
        {
            get;
            set;
        }


        public string User_Type
        {
            get;
            set;
        }
        public string Status
        {
            get;
            set;
        }
        public string Corporate_User
        {
            get;
            set;
        }
        public string Allow_Login
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

        

    }
}