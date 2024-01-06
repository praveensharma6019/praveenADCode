using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.TrivandrumAirport.Website.Model
{
    [Serializable]
    public class UserDetails
    {
        [Required(ErrorMessage = "Company Field is Required.")]
        public string Company
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public DateTime? CreatedDate
        {
            get;
            set;
        }

        [EmailAddress(ErrorMessage = "Please Enter Valid EmailAddress.")]
        [Required(ErrorMessage = "Email is Required.")]
        public string Email
        {
            get;
            set;
        }

        [RegularExpression("^[0-9]{12,12}$", ErrorMessage = "Please Enter Valid Fax Number.")]
        public string FaxNo
        {
            get;
            set;
        }

        [RegularExpression("^[0-9]{8,16}$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Required(ErrorMessage = "Mobile Number is Required.")]
        
        public string MobileNo
        {
            get;
            set;
        }

        public string ModifiedBy
        {
            get;
            set;
        }

        public DateTime? ModifiedDate
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Name Field is Required.")]
        public string Name
        {
            get;
            set;
        }

        public string reResponse
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string SupportEmailAddress
        {
            get;
            set;
        }

        public Guid TenderID
        {
            get;
            set;
        }

        public string UniqueId
        {
            get;
            set;
        }

        public string UserType
        {
            get;
            set;
        }

        public UserDetails()
        {
        }
    }
}