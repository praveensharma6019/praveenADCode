using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGmsRegistration
    {

        [Required(ErrorMessage = "First Name is Required.")]
        [StringLength(50, ErrorMessage = "Maximum 150 characters allowed")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Only alphabet Allowed in First Name.")]
        public string first_name
        {
            get;
            set;
        }

        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Only alphabet Allowed in Middle Name.")]
        public string middle_name
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Last Name is Required.")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Only alphabet Allowed in Last Name.")]
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

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [EmailAddress(ErrorMessage = "Please Enter Valid EmailAddress.")]
        [Required(ErrorMessage = "Email is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Email
        {
            get;
            set;
        }

        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string Fax_No
        {
            get;
            set;
        }

        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [StringLength(10, ErrorMessage = "Maximum 10 characters allowed")]
        public string Mobile
        {
            get;
            set;
        }


        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Please Enter Valid Mobile Number.")]
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
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Only alphabet Allowed in First Name.")]
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
        [StringLength(50, ErrorMessage = "Maximum 150 characters allowed")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Only alphabet Allowed in State.")]
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
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Only alphabet Allowed in First Name.")]
        public string Country
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Pin Code is Required.")]
        [StringLength(6, ErrorMessage = "Maximum 6 characters allowed")]
        [RegularExpression(@"^[0-9]{6,6}$", ErrorMessage = "Please Enter Valid PinCode.")]
        public string Pin_Code
        {
            get;
            set;
        }

        public string InCompleteGrievance { set; get; }

       
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
        public string RegistrationGcaptcha
        {
            get;
            set;
        }
      
    }
}