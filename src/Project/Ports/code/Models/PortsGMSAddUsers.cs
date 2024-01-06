using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGMSAddUsers
    {
        
       
        [Required(ErrorMessage = "This is Required.")]
        public string Title
        {
            get;
            set;
        }

        [Required(ErrorMessage = "First Name is Required.")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Only alphabet Allowed in First Name.")]
        public string first_name
        {
            get;
            set;
        }

        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Only alphabet Allowed in First Name.")]
        public string middle_name
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Last Name is Required.")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Only alphabet Allowed in Last Name.")]
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
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")]
        public string Email
        {
            get;
            set;
        }


        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Mobile Number is Required.")]
        public string Mobile
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
        [Required(ErrorMessage = "Role is Required.")]
        public string User_Type
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Status is Required.")]
        public Boolean Status
        {
            get;
            set;
        }
        public string BusinessGroup { get; set; }
        public string Team { get; set; }
        public string SiteHead { get; set; }
        public string AddUsersGcaptcha { get; set; }
        public string HO { get; set; }
        public PortsGMSAddUsers()
        {

        }

    }
}