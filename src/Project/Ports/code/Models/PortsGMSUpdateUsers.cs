using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGMSUpdateUsers
    {
        
        public string Name
        {
            get;
            set;
        }
        [Required(ErrorMessage = "This is Required.")]
        public string Title
        {
            get;
            set;
        }
        [Required(ErrorMessage = "First Name is Required.")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string first_name
        {
            get;
            set;
        }
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string middle_name
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Last Name is Required.")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
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
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")]
        public string Email
        {
            get;
            set;
        }


        [RegularExpression("^[0-9]{8,16}$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Required(ErrorMessage = "Mobile Number is Required.")]
        public string Mobile
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Location Number is Required.")]
        public string Location
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Department Number is Required.")]
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

        

        public PortsGms_Registration GMSRegisterdUsers
        {
            get;
            set;
        }

        public PortsGMSUpdateUsers()
        {
            
         this.GMSRegisterdUsers = new PortsGms_Registration();

        }

    }
}