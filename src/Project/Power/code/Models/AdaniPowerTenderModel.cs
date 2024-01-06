using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Power.Website.Models
{
    [Serializable]
    public class AdaniPowerTenderModel
    {
        [Required(ErrorMessage = "Name Field is Required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company Field is Required.")]
        public string Company { get; set; }

        //[RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Please Enter Valid Number")]
        //public string PhoneNo { get; set; }

        [RegularExpression(@"^((\+)?(\d{2}[-])?(\d{10}){1})?(\d{11}){0,1}?$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Required(ErrorMessage = "Mobile Number is Required.")]
        public string MobileNo { get; set; }
        public Guid Id
        {
            get;
            set;
        }
        //[Required(ErrorMessage = "Fax Number is Required.")]
        [RegularExpression(@"^[0-9]{12,12}$", ErrorMessage = "Please Enter Valid Fax Number.")]
        public string FaxNo { get; set; }

        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress(ErrorMessage = "Please Enter Valid EmailAddress.")]
        public string Email { get; set; }
        public string userid { get; set; }
        public string UserType { get; set; }
        public Guid TenderID { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }

}